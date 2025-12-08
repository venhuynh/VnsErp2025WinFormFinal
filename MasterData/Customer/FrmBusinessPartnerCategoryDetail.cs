using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DTO.MasterData.CustomerPartner;
using System;
using System.Linq;
using System.Windows.Forms;
using Bll.MasterData.CustomerBll;
using Common.Utils;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;

namespace MasterData.Customer
{
    /// <summary>
    /// Form chi tiết danh mục đối tác - thêm mới và chỉnh sửa.
    /// Cung cấp giao diện nhập liệu cho thông tin danh mục đối tác với validation và xử lý lỗi.
    /// </summary>
    public partial class FrmBusinessPartnerCategoryDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho danh mục đối tác
        /// </summary>
        private readonly BusinessPartnerCategoryBll _businessPartnerCategoryBll = new();

        /// <summary>
        /// ID của danh mục đang chỉnh sửa (Guid.Empty nếu thêm mới)
        /// </summary>
        private readonly Guid _categoryId;

        /// <summary>
        /// Trạng thái chỉnh sửa (true nếu đang chỉnh sửa, false nếu thêm mới)
        /// </summary>
        private bool IsEditMode => _categoryId != Guid.Empty;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho thêm mới danh mục.
        /// </summary>
        public FrmBusinessPartnerCategoryDetail() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Khởi tạo form cho chỉnh sửa danh mục.
        /// </summary>
        /// <param name="categoryId">ID của danh mục cần chỉnh sửa</param>
        public FrmBusinessPartnerCategoryDetail(Guid categoryId)
        {
            InitializeComponent();
            _categoryId = categoryId;
            InitializeForm();
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo form và load dữ liệu nếu cần.
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Thiết lập tiêu đề form
                Text = IsEditMode ? "Điều chỉnh danh mục đối tác" : "Thêm mới danh mục đối tác";

                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                RequiredFieldHelper.MarkRequiredFields(this, typeof(BusinessPartnerCategoryDto));

                // Load danh sách danh mục cha
                LoadParentCategories();

                // Thiết lập SuperToolTip cho các controls
                SetupSuperToolTips();

                // Load dữ liệu nếu đang chỉnh sửa
                if (IsEditMode)
                {
                    LoadCategoryData();
                }

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
        /// Load danh sách danh mục cha vào TreeListLookUpEdit.
        /// </summary>
        private void LoadParentCategories()
        {
            try
            {
                var (categories, counts) = _businessPartnerCategoryBll.GetCategoriesWithCounts();
                
                // Chuyển đổi sang DTO với hierarchy
                var dtos = categories.Select(c =>
                {
                    var count = counts.TryGetValue(c.Id, out var count1) ? count1 : 0;
                    return c.ToDtoWithCount(count);
                }).ToList();

                // Tính toán FullPath và Level cho hierarchical display
                var entityDict = categories.ToDictionary(e => e.Id);
                var dtoList = dtos.Select(dto =>
                {
                    var entity = categories.FirstOrDefault(e => e.Id == dto.Id);
                    if (entity != null)
                    {
                        // Tính Level
                        int level = 0;
                        var current = entity;
                        while (current.ParentId.HasValue && entityDict.ContainsKey(current.ParentId.Value))
                        {
                            level++;
                            current = entityDict[current.ParentId.Value];
                            if (level > 10) break; // Tránh infinite loop
                        }
                        dto.Level = level;

                        // Tính FullPath
                        var pathParts = new System.Collections.Generic.List<string> { entity.CategoryName };
                        current = entity;
                        while (current.ParentId.HasValue && entityDict.ContainsKey(current.ParentId.Value))
                        {
                            current = entityDict[current.ParentId.Value];
                            pathParts.Insert(0, current.CategoryName);
                            if (pathParts.Count > 10) break; // Tránh infinite loop
                        }
                        dto.FullPath = string.Join(" > ", pathParts);

                        // Lấy tên parent category
                        if (entity.ParentId.HasValue && entityDict.TryGetValue(entity.ParentId.Value, out var value))
                        {
                            dto.ParentCategoryName = value.CategoryName;
                        }
                    }
                    return dto;
                }).ToList();

                // Loại bỏ category hiện tại khỏi danh sách parent (tránh circular reference)
                if (IsEditMode)
                {
                    dtoList = dtoList.Where(d => d.Id != _categoryId).ToList();
                }

                // Bind vào BindingSource
                businessPartnerCategoryDtoBindingSource.DataSource = dtoList;

                // Thiết lập SearchLookUpEdit
                ParentCategorySearchLookUpEdit.Properties.DataSource = businessPartnerCategoryDtoBindingSource;
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
                
                // Đăng ký event để đảm bảo giá trị được set đúng
                ParentCategorySearchLookUpEdit.EditValueChanged += ParentCategorySearchLookUpEdit_EditValueChanged;
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
                var category = _businessPartnerCategoryBll.GetById(_categoryId);
                if (category == null)
                {
                    ShowError("Không tìm thấy danh mục đối tác");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                var dto = category.ToDto();
                BindDataToControls(dto);
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
        private void BindDataToControls(BusinessPartnerCategoryDto dto)
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
        private BusinessPartnerCategoryDto GetDataFromControls()
        {
            var dto = new BusinessPartnerCategoryDto
            {
                Id = _categoryId,
                CategoryCode = CategoryCodeTextEdit?.Text?.Trim(),
                CategoryName = CategoryNameTextEdit?.Text?.Trim(),
                Description = DescriptionMemoEdit?.Text?.Trim(),
                IsActive = IsActiveToogleSwitch.IsOn
            };

            // Lấy ParentId từ SearchLookUpEdit
            // Lưu ý: EditValue sẽ trả về giá trị của ValueMember (Id), không phải DisplayMember
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
                        var selectedRow = parentCategoryGridView.GetFocusedRow() as BusinessPartnerCategoryDto;
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
        /// Lưu dữ liệu danh mục.
        /// </summary>
        private void SaveCategory()
        {
            try
            {
                var dto = GetDataFromControls();
                var entity = dto.ToEntity();

                if (IsEditMode)
                {
                    // Cập nhật danh mục hiện có
                    _businessPartnerCategoryBll.Update(entity);
                    ShowInfo("Cập nhật danh mục đối tác thành công!");
                }
                else
                {
                    // Thêm danh mục mới
                    _businessPartnerCategoryBll.Insert(entity);
                    ShowInfo("Thêm mới danh mục đối tác thành công!");
                }

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
                    if (parentCategoryGridView.GetFocusedRow() is BusinessPartnerCategoryDto selectedRow)
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
        /// Xử lý sự kiện click button Lưu
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateInput())
            {
                SaveCategory();
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Hủy
        /// </summary>
        private void CancelBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Xử lý phím tắt cho form
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

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Validate dữ liệu đầu vào.
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không</returns>
        private bool ValidateInput()
        {
            dxErrorProvider1.ClearErrors();

            // CategoryName bắt buộc
            if (string.IsNullOrWhiteSpace(CategoryNameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại không được để trống",
                    ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài CategoryName
            if (CategoryNameTextEdit.Text.Trim().Length > 100)
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại không được vượt quá 100 ký tự",
                    ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp tên phân loại (không tính bản ghi đang chỉnh sửa)
            var categoryName = CategoryNameTextEdit.Text.Trim();
            if (_businessPartnerCategoryBll.IsCategoryNameExists(categoryName, _categoryId))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên phân loại đã tồn tại trong hệ thống",
                    ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài CategoryCode
            if (!string.IsNullOrWhiteSpace(CategoryCodeTextEdit?.Text) && CategoryCodeTextEdit.Text.Trim().Length > 50)
            {
                dxErrorProvider1.SetError(CategoryCodeTextEdit, "Mã danh mục không được vượt quá 50 ký tự",
                    ErrorType.Critical);
                CategoryCodeTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài Description
            if (!string.IsNullOrWhiteSpace(DescriptionMemoEdit?.Text) && DescriptionMemoEdit.Text.Trim().Length > 255)
            {
                dxErrorProvider1.SetError(DescriptionMemoEdit, "Mô tả không được vượt quá 255 ký tự",
                    ErrorType.Critical);
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
                    var allCategories = _businessPartnerCategoryBll.GetAll();
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
                        content: "Nhập mã danh mục đối tác (tối đa 50 ký tự). Trường này là tùy chọn."
                    );
                }

                if (CategoryNameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        CategoryNameTextEdit,
                        title: "<b><color=DarkBlue>📋 Tên phân loại</color></b>",
                        content: "Nhập tên phân loại đối tác. Trường này là bắt buộc."
                    );
                }

                if (ParentCategorySearchLookUpEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        ParentCategorySearchLookUpEdit,
                        title: "<b><color=DarkBlue>🌳 Danh mục cha</color></b>",
                        content: "Chọn danh mục cha cho danh mục này (tùy chọn). Sử dụng để tạo cấu trúc phân cấp. Đường dẫn sẽ hiển thị dưới dạng HTML."
                    );
                }

                if (DescriptionMemoEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        DescriptionMemoEdit,
                        title: "<b><color=DarkBlue>📝 Mô tả</color></b>",
                        content: "Nhập mô tả chi tiết về phân loại đối tác (tối đa 255 ký tự)."
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
                        content: "Lưu thông tin danh mục đối tác vào hệ thống."
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
        /// Hiển thị thông tin.
        /// </summary>
        /// <param name="message">Thông báo</param>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị lỗi.
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        /// <summary>
        /// Hiển thị lỗi với thông tin ngữ cảnh.
        /// </summary>
        /// <param name="ex">Exception</param>
        /// <param name="context">Ngữ cảnh lỗi</param>
        private void ShowError(Exception ex, string context = null)
        {
            MsgBox.ShowException(string.IsNullOrWhiteSpace(context)
                ? ex
                : new Exception(context + ": " + ex.Message, ex));
        }

        #endregion
    }
}
