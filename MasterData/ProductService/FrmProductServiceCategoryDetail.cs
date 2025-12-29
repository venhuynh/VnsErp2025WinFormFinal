using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Bll.MasterData.ProductServiceBll;
using Common.Utils;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.MasterData.ProductService;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form chi tiết danh mục sản phẩm/dịch vụ.
    /// Cung cấp chức năng thêm mới và chỉnh sửa danh mục với validation nghiệp vụ và giao diện thân thiện.
    /// </summary>
    public partial class FrmProductServiceCategoryDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho danh mục sản phẩm/dịch vụ
        /// </summary>
        private readonly ProductServiceCategoryBll _productServiceCategoryBll = new ProductServiceCategoryBll();

        /// <summary>
        /// ID danh mục (Guid.Empty cho thêm mới, ID thực cho chỉnh sửa)
        /// </summary>
        private readonly Guid _categoryId;

        /// <summary>
        /// Trạng thái chỉnh sửa (true) hay thêm mới (false)
        /// </summary>
        private bool IsEditMode => _categoryId != Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho thêm mới danh mục
        /// </summary>
        public FrmProductServiceCategoryDetail() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Khởi tạo form cho chỉnh sửa danh mục
        /// </summary>
        /// <param name="categoryId">ID của danh mục cần chỉnh sửa</param>
        public FrmProductServiceCategoryDetail(Guid categoryId)
        {
            InitializeComponent();
            _categoryId = categoryId;
            InitializeForm();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo form và load dữ liệu nếu cần
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Thiết lập tiêu đề form
                Text = IsEditMode ? "Điều chỉnh danh mục sản phẩm/dịch vụ" : "Thêm mới danh mục sản phẩm/dịch vụ";

                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                RequiredFieldHelper.MarkRequiredFields(this, typeof(ProductServiceCategoryDto));

                // Load danh sách danh mục cha
                LoadParentCategories();

                // Thiết lập SuperToolTip cho các controls
                SetupSuperToolTips();

                // Load dữ liệu nếu đang chỉnh sửa
                if (IsEditMode)
                {
                    LoadCategoryData();
                }

                // Đăng ký event để tự động tạo mã danh mục khi thay đổi tên danh mục
                CategoryNameTextEdit.TextChanged += CategoryNameTextEdit_TextChanged;

                // Đăng ký event để đảm bảo giá trị được set đúng
                ParentCategorySearchLookUpEdit.EditValueChanged += ParentCategorySearchLookUpEdit_EditValueChanged;

                // Thiết lập focus cho control đầu tiên
                CategoryCodeTextEdit.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load danh sách danh mục cha vào SearchLookUpEdit.
        /// </summary>
        private void LoadParentCategories()
        {
            try
            {
                // GetCategoriesWithCounts() already returns List<ProductServiceCategoryDto>
                var (categories, counts) = _productServiceCategoryBll.GetCategoriesWithCounts();
                
                // Calculate hierarchy properties manually on DTOs
                var categoryDict = categories.ToDictionary(c => c.Id);
                var dtoList = categories.Select(dto =>
                {
                    // Set ProductCount from counts dictionary
                    dto.ProductCount = counts.TryGetValue(dto.Id, out var count) ? count : 0;

                    // Calculate Level
                    int level = 0;
                    var current = dto;
                    while (current.ParentId.HasValue && categoryDict.ContainsKey(current.ParentId.Value))
                    {
                        level++;
                        current = categoryDict[current.ParentId.Value];
                        if (level > 10) break; // Tránh infinite loop
                    }
                    dto.Level = level;

                    // Calculate FullPath
                    var pathParts = new System.Collections.Generic.List<string> { dto.CategoryName };
                    current = dto;
                    while (current.ParentId.HasValue && categoryDict.ContainsKey(current.ParentId.Value))
                    {
                        current = categoryDict[current.ParentId.Value];
                        pathParts.Insert(0, current.CategoryName);
                        if (pathParts.Count > 10) break; // Tránh infinite loop
                    }
                    dto.FullPath = string.Join(" > ", pathParts);

                    // Set ParentCategoryName
                    if (dto.ParentId.HasValue && categoryDict.TryGetValue(dto.ParentId.Value, out var parent))
                    {
                        dto.ParentCategoryName = parent.CategoryName;
                    }

                    // Calculate HasChildren
                    dto.HasChildren = categories.Any(c => c.ParentId == dto.Id);

                    return dto;
                }).ToList();

                // Loại bỏ category hiện tại khỏi danh sách parent (tránh circular reference)
                if (IsEditMode)
                {
                    dtoList = dtoList.Where(d => d.Id != _categoryId).ToList();
                }

                // Bind vào BindingSource
                productServiceCategoryDtoBindingSource.DataSource = dtoList;

                // Thiết lập SearchLookUpEdit
                ParentCategorySearchLookUpEdit.Properties.DataSource = productServiceCategoryDtoBindingSource;
                ParentCategorySearchLookUpEdit.Properties.ValueMember = "Id";
                ParentCategorySearchLookUpEdit.Properties.DisplayMember = "FullPathHtml";
                ParentCategorySearchLookUpEdit.Properties.PopupView = parentCategoryGridView;

                // Thiết lập GridView
                parentCategoryGridView.OptionsView.ShowGroupPanel = false;
                parentCategoryGridView.OptionsView.ShowIndicator = false;
                parentCategoryGridView.OptionsSelection.EnableAppearanceFocusedCell = false;
                parentCategoryGridView.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;

                // Sắp xếp theo Level và CategoryName để hiển thị hierarchical
                parentCategoryGridView.SortInfo.ClearAndAddRange(new[] {
                    new DevExpress.XtraGrid.Columns.GridColumnSortInfo(colParentFullPathHtml, DevExpress.Data.ColumnSortOrder.Ascending)
                }, 0);

                // Thiết lập các tùy chọn SearchLookUpEdit
                ParentCategorySearchLookUpEdit.Properties.AllowNullInput = DefaultBoolean.True;
                ParentCategorySearchLookUpEdit.Properties.NullText = @"Chọn danh mục cha (tùy chọn)";
                ParentCategorySearchLookUpEdit.Properties.TextEditStyle = TextEditStyles.Standard;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải danh sách danh mục cha");
            }
        }

        /// <summary>
        /// Load dữ liệu danh mục để chỉnh sửa.
        /// </summary>
        private void LoadCategoryData()
        {
            try
            {
                // GetById() already returns ProductServiceCategoryDto
                var category = _productServiceCategoryBll.GetById(_categoryId);
                if (category == null)
                {
                    ShowError("Không tìm thấy danh mục sản phẩm/dịch vụ");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                BindDataToControls(category);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu danh mục");
            }
        }

        /// <summary>
        /// Bind dữ liệu DTO vào các control.
        /// </summary>
        /// <param name="dto">DTO chứa dữ liệu</param>
        private void BindDataToControls(ProductServiceCategoryDto dto)
        {
            CategoryCodeTextEdit.Text = dto.CategoryCode;
            CategoryNameTextEdit.Text = dto.CategoryName;
            DescriptionMemoEdit.Text = dto.Description;
            IsActiveToogleSwitch.IsOn = dto.IsActive;
            
            // Bind ParentId
            if (dto.ParentId.HasValue)
            {
                ParentCategorySearchLookUpEdit.EditValue = dto.ParentId.Value;
            }
            else
            {
                ParentCategorySearchLookUpEdit.EditValue = null;
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ các control và tạo DTO.
        /// </summary>
        /// <returns>DTO chứa dữ liệu từ form</returns>
        private ProductServiceCategoryDto GetDataFromControls()
        {
            var dto = new ProductServiceCategoryDto
            {
                Id = _categoryId,
                CategoryCode = CategoryCodeTextEdit?.Text?.Trim(),
                CategoryName = CategoryNameTextEdit?.Text?.Trim(),
                Description = DescriptionMemoEdit?.Text?.Trim(),
                IsActive = IsActiveToogleSwitch.IsOn
            };

            // Lấy ParentId từ SearchLookUpEdit
            var editValue = ParentCategorySearchLookUpEdit.EditValue;
            if (editValue != null && editValue != DBNull.Value)
            {
                // Nếu EditValue là Guid, sử dụng trực tiếp
                if (editValue is Guid guidValue)
                {
                    dto.ParentId = guidValue;
                }
                // Nếu EditValue là string, parse thành Guid
                else if (editValue is string stringValue && Guid.TryParse(stringValue, out var parsedGuid))
                {
                    dto.ParentId = parsedGuid;
                }
                // Nếu EditValue là object khác, thử convert
                else
                {
                    try
                    {
                        dto.ParentId = (Guid)Convert.ChangeType(editValue, typeof(Guid));
                    }
                    catch
                    {
                        // Nếu không convert được, thử lấy từ selected row trong GridView
                        var selectedRow = parentCategoryGridView.GetFocusedRow() as ProductServiceCategoryDto;
                        if (selectedRow != null)
                        {
                            dto.ParentId = selectedRow.Id;
                        }
                        else
                        {
                            dto.ParentId = null;
                        }
                    }
                }
            }
            else
            {
                dto.ParentId = null;
            }

            return dto;
        }

        /// <summary>
        /// Validate dữ liệu đầu vào.
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không</returns>
        private bool ValidateInput()
        {
            dxErrorProvider1.ClearErrors();

            // CategoryCode bắt buộc
            if (string.IsNullOrWhiteSpace(CategoryCodeTextEdit?.Text))
            {
                dxErrorProvider1.SetError(CategoryCodeTextEdit, "Mã danh mục không được để trống", ErrorType.Critical);
                CategoryCodeTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài CategoryCode
            var categoryCode = CategoryCodeTextEdit.Text.Trim();
            if (categoryCode.Length < 2)
            {
                dxErrorProvider1.SetError(CategoryCodeTextEdit, "Mã danh mục phải có ít nhất 2 ký tự", ErrorType.Critical);
                CategoryCodeTextEdit?.Focus();
                return false;
            }

            if (categoryCode.Length > 50)
            {
                dxErrorProvider1.SetError(CategoryCodeTextEdit, "Mã danh mục không được vượt quá 50 ký tự", ErrorType.Critical);
                CategoryCodeTextEdit?.Focus();
                return false;
            }

            // Kiểm tra format CategoryCode (chỉ chữ cái và số)
            if (!Regex.IsMatch(categoryCode, @"^[A-Z0-9]+$"))
            {
                dxErrorProvider1.SetError(CategoryCodeTextEdit, "Mã danh mục chỉ được chứa chữ cái và số (không có khoảng trắng, ký tự đặc biệt)", ErrorType.Critical);
                CategoryCodeTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp CategoryCode (không tính bản ghi đang chỉnh sửa)
            if (_productServiceCategoryBll.IsCategoryCodeExists(categoryCode, _categoryId))
            {
                dxErrorProvider1.SetError(CategoryCodeTextEdit, "Mã danh mục đã tồn tại trong hệ thống", ErrorType.Critical);
                CategoryCodeTextEdit?.Focus();
                return false;
            }

            // CategoryName bắt buộc
            if (string.IsNullOrWhiteSpace(CategoryNameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên danh mục không được để trống", ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài CategoryName
            if (CategoryNameTextEdit.Text.Trim().Length > 200)
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên danh mục không được vượt quá 200 ký tự", ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp tên danh mục (không tính bản ghi đang chỉnh sửa)
            var categoryName = CategoryNameTextEdit.Text.Trim();
            if (_productServiceCategoryBll.IsCategoryNameExists(categoryName, _categoryId))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên danh mục đã tồn tại trong hệ thống", ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài Description
            if (!string.IsNullOrWhiteSpace(DescriptionMemoEdit?.Text) && DescriptionMemoEdit.Text.Trim().Length > 255)
            {
                dxErrorProvider1.SetError(DescriptionMemoEdit, "Mô tả không được vượt quá 255 ký tự", ErrorType.Critical);
                DescriptionMemoEdit?.Focus();
                return false;
            }

            // Kiểm tra circular reference (không cho phép chọn chính nó hoặc con của nó làm parent)
            if (IsEditMode && ParentCategorySearchLookUpEdit.EditValue != null)
            {
                if (Guid.TryParse(ParentCategorySearchLookUpEdit.EditValue.ToString(), out var selectedParentId))
                {
                    if (selectedParentId == _categoryId)
                    {
                        dxErrorProvider1.SetError(ParentCategorySearchLookUpEdit, 
                            "Không thể chọn chính danh mục này làm danh mục cha",
                            ErrorType.Critical);
                        ParentCategorySearchLookUpEdit?.Focus();
                        return false;
                    }

                    // Kiểm tra xem selectedParentId có phải là con của _categoryId không
                    var allCategories = _productServiceCategoryBll.GetAll();
                    var categoryDict = allCategories.ToDictionary(c => c.Id);
                    var current = allCategories.FirstOrDefault(c => c.Id == selectedParentId);
                    while (current != null && current.ParentId.HasValue)
                    {
                        if (current.ParentId.Value == _categoryId)
                        {
                            dxErrorProvider1.SetError(ParentCategorySearchLookUpEdit,
                                "Không thể chọn danh mục con của danh mục này làm danh mục cha",
                                ErrorType.Critical);
                            ParentCategorySearchLookUpEdit?.Focus();
                            return false;
                        }
                        current = categoryDict.ContainsKey(current.ParentId.Value) 
                            ? categoryDict[current.ParentId.Value] 
                            : null;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Lưu dữ liệu danh mục.
        /// </summary>
        private void SaveCategory()
        {
            try
            {
                var dto = GetDataFromControls();

                // SaveOrUpdate expects ProductServiceCategoryDto directly
                _productServiceCategoryBll.SaveOrUpdate(dto);

                var message = IsEditMode ? "Cập nhật danh mục sản phẩm/dịch vụ thành công!" : "Thêm mới danh mục sản phẩm/dịch vụ thành công!";
                ShowInfo(message);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lưu dữ liệu danh mục");
            }
        }

        #endregion

        #region ========== SỰ KIỆN CONTROLS ==========

        /// <summary>
        /// Event handler khi user thay đổi tên danh mục -> tự động tạo mã danh mục.
        /// </summary>
        private void CategoryNameTextEdit_TextChanged(object sender, EventArgs e)
        {
            try
            {
                // Chỉ tự động tạo mã khi đang thêm mới (không phải edit mode)
                // hoặc khi CategoryCode đang trống
                if (IsEditMode && !string.IsNullOrWhiteSpace(CategoryCodeTextEdit?.Text))
                    return;

                // Lấy tên danh mục
                var categoryName = CategoryNameTextEdit?.Text?.Trim();
                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    if (CategoryCodeTextEdit != null) 
                        CategoryCodeTextEdit.Text = string.Empty;
                    return;
                }

                // Tự động tạo mã danh mục
                var newCode = GenerateCategoryCode(categoryName);
                if (!string.IsNullOrEmpty(newCode))
                {
                    if (CategoryCodeTextEdit != null) 
                        CategoryCodeTextEdit.Text = newCode;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tự động tạo mã danh mục");
            }
        }

        /// <summary>
        /// Xử lý sự kiện EditValueChanged của ParentCategorySearchLookUpEdit
        /// </summary>
        private void ParentCategorySearchLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Đảm bảo EditValue được set đúng
                var editValue = ParentCategorySearchLookUpEdit.EditValue;
                
                // Nếu EditValue là null hoặc DBNull, clear selection
                if (editValue == null || editValue == DBNull.Value)
                {
                    // Không cần làm gì, giá trị đã là null
                    return;
                }

                // Kiểm tra xem giá trị có hợp lệ không
                Guid? parentId = null;
                if (editValue is Guid guidValue)
                {
                    parentId = guidValue;
                }
                else if (editValue is string stringValue && Guid.TryParse(stringValue, out var parsedGuid))
                {
                    parentId = parsedGuid;
                }
                else
                {
                    // Nếu không parse được, thử lấy từ selected row
                    if (parentCategoryGridView.GetFocusedRow() is ProductServiceCategoryDto selectedRow)
                    {
                        parentId = selectedRow.Id;
                        // Set lại EditValue để đảm bảo consistency
                        ParentCategorySearchLookUpEdit.EditValue = parentId.Value;
                    }
                }

                // Debug: Log giá trị để kiểm tra
                System.Diagnostics.Debug.WriteLine($"ParentCategorySearchLookUpEdit_EditValueChanged: EditValue = {editValue}, ParentId = {parentId}");
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để không làm gián đoạn user
                System.Diagnostics.Debug.WriteLine($"Lỗi trong ParentCategorySearchLookUpEdit_EditValueChanged: {ex.Message}");
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Người dùng bấm nút Lưu
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateInput())
            {
                SaveCategory();
            }
        }

        /// <summary>
        /// Người dùng bấm nút Hủy
        /// </summary>
        private void CancelBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Xử lý phím tắt
        /// </summary>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == (Keys.Control | Keys.S))
            {
                SaveBarButtonItem_ItemClick(null, null);
                return true;
            }

            if (keyData == Keys.Escape)
            {
                CancelBarButtonItem_ItemClick(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (CategoryCodeTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        CategoryCodeTextEdit,
                        title: "<b><color=DarkBlue>🔖 Mã danh mục</color></b>",
                        content: "Nhập mã danh mục duy nhất (chỉ chữ cái và số, không có khoảng trắng). Trường này là bắt buộc. Mã sẽ được tự động tạo khi nhập tên danh mục."
                    );
                }

                if (CategoryNameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        CategoryNameTextEdit,
                        title: "<b><color=DarkBlue>📋 Tên danh mục</color></b>",
                        content: "Nhập tên danh mục sản phẩm/dịch vụ. Trường này là bắt buộc."
                    );
                }

                if (DescriptionMemoEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        DescriptionMemoEdit,
                        title: "<b><color=DarkBlue>📝 Mô tả</color></b>",
                        content: "Nhập mô tả chi tiết về danh mục (tối đa 255 ký tự)."
                    );
                }

                if (ParentCategorySearchLookUpEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        ParentCategorySearchLookUpEdit,
                        title: "<b><color=DarkBlue>🌳 Danh mục cha</color></b>",
                        content: "Chọn danh mục cha (tùy chọn). Để trống nếu đây là danh mục gốc. Đường dẫn sẽ hiển thị dưới dạng HTML."
                    );
                }

                if (IsActiveToogleSwitch != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsActiveToogleSwitch,
                        title: "<b><color=DarkBlue>✅ Trạng thái hoạt động</color></b>",
                        content: "Bật/tắt trạng thái hoạt động của danh mục. Danh mục không hoạt động sẽ không hiển thị trong một số danh sách."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu thông tin danh mục sản phẩm/dịch vụ vào hệ thống."
                    );
                }

                if (CloseBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CloseBarButtonItem,
                        title: "<b><color=Red>❌ Đóng</color></b>",
                        content: "Đóng form mà không lưu thay đổi."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        /// <summary>
        /// Tạo mã danh mục từ tên danh mục
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <returns>Mã danh mục</returns>
        private string GenerateCategoryCode(string categoryName)
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return "CAT";
            
            // Lấy chữ cái đầu của mỗi từ trong tên danh mục
            var words = categoryName.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var code = string.Empty;
            
            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    var firstChar = word.Trim().FirstOrDefault();
                    if (char.IsLetter(firstChar))
                    {
                        code += char.ToUpper(firstChar);
                    }
                }
            }
            
            // Đảm bảo mã có ít nhất 2 ký tự
            return code.Length >= 2 ? code : "CAT";
        }

        /// <summary>
        /// Hiển thị thông tin
        /// </summary>
        /// <param name="message">Thông báo</param>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="context">Ngữ cảnh lỗi</param>
        private void ShowError(Exception ex, string context = null)
        {
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}
