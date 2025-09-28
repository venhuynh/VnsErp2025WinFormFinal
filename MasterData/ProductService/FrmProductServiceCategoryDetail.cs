using System;
using System.Linq;
using System.Windows.Forms;
using Bll.MasterData.ProductService;
using Bll.Utils;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form chi tiết danh mục sản phẩm/dịch vụ - thêm mới và chỉnh sửa.
    /// </summary>
    public partial class FrmProductServiceCategoryDetail : XtraForm
    {
        #region Fields

        private readonly ProductServiceCategoryBll _productServiceCategoryBll = new ProductServiceCategoryBll();
        private readonly Guid _categoryId;
        private bool IsEditMode => _categoryId != Guid.Empty;
        private bool _hasUserSelectedParent; // Track xem user có thực sự chọn parent category hay không

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo form cho thêm mới danh mục.
        /// </summary>
        public FrmProductServiceCategoryDetail() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Khởi tạo form cho chỉnh sửa danh mục.
        /// </summary>
        /// <param name="categoryId">ID của danh mục cần chỉnh sửa</param>
        public FrmProductServiceCategoryDetail(Guid categoryId)
        {
            InitializeComponent();
            _categoryId = categoryId;
            InitializeForm();
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Khởi tạo form và load dữ liệu nếu cần.
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Thiết lập tiêu đề form
                Text = IsEditMode ? "Điều chỉnh danh mục sản phẩm/dịch vụ" : "Thêm mới danh mục sản phẩm/dịch vụ";

                // Load dữ liệu nếu đang chỉnh sửa
                if (IsEditMode)
                {
                    LoadCategoryData();
                }

                // Load danh sách danh mục cha
                LoadParentCategories();

                // Thiết lập focus cho control đầu tiên
                CategoryNameTextEdit.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        /// <summary>
        /// Load danh sách danh mục cha vào TreeListLookUpEdit.
        /// </summary>
        private void LoadParentCategories()
        {
            try
            {
                var (categories, counts) = _productServiceCategoryBll.GetCategoriesWithCounts();
                var dtos = categories.Select(c => 
                {
                    var count = counts.ContainsKey(c.Id) ? counts[c.Id] : 0;
                    return c.ToDtoWithCount(count);
                }).ToList();
                
                // Thiết lập TreeListLookUpEdit
                ParentCategoryTreeListTreeListLookUpEdit.Properties.DataSource = dtos;
                ParentCategoryTreeListTreeListLookUpEdit.Properties.ValueMember = "Id";
                ParentCategoryTreeListTreeListLookUpEdit.Properties.DisplayMember = "CategoryName";
                
                // Thiết lập TreeList bên trong TreeListLookUpEdit
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.KeyFieldName = "Id";
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.ParentFieldName = "ParentId";
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.RootValue = null;
                
                // Thiết lập cột hiển thị
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.Columns.Clear();
                var nameColumn = ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.Columns.Add();
                nameColumn.FieldName = "CategoryName";
                nameColumn.Caption = "Tên danh mục";
                nameColumn.VisibleIndex = 0;
                nameColumn.Width = 200;
                
                var descColumn = ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.Columns.Add();
                descColumn.FieldName = "Description";
                descColumn.Caption = "Mô tả";
                descColumn.VisibleIndex = 1;
                descColumn.Width = 150;
                
                var countColumn = ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.Columns.Add();
                countColumn.FieldName = "ProductCount";
                countColumn.Caption = "Số SP/DV";
                countColumn.VisibleIndex = 2;
                countColumn.Width = 80;
                
                // Thiết lập TreeList để hiển thị đúng cấu trúc cây
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowIndentAsRowStyle = true;
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowHorzLines = true;
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowVertLines = true;
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowRoot = true;
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowButtons = true;
                
                // Mở rộng tất cả các nút
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TreeList.ExpandAll();
                
                // Thiết lập các tùy chọn
                ParentCategoryTreeListTreeListLookUpEdit.Properties.AllowNullInput = DefaultBoolean.True;
                ParentCategoryTreeListTreeListLookUpEdit.Properties.NullText = @"Chọn danh mục cha (tùy chọn)";
                
                // Các tính năng bổ sung theo tài liệu DevExpress
                ParentCategoryTreeListTreeListLookUpEdit.Properties.AutoComplete = true; // Tự động hoàn thành khi gõ
                ParentCategoryTreeListTreeListLookUpEdit.Properties.AutoExpandAllNodes = true; // Tự động mở rộng tất cả nodes
                ParentCategoryTreeListTreeListLookUpEdit.Properties.PopupFilterMode = PopupFilterMode.Contains; // Lọc khi gõ
                ParentCategoryTreeListTreeListLookUpEdit.Properties.TextEditStyle = TextEditStyles.Standard; // Cho phép chỉnh sửa text
                
                // Đăng ký event để track khi user chọn parent category
                ParentCategoryTreeListTreeListLookUpEdit.EditValueChanged += ParentCategoryTreeListTreeListLookUpEdit_EditValueChanged;
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
                var category = _productServiceCategoryBll.GetById(_categoryId);
                if (category == null)
                {
                    ShowError("Không tìm thấy danh mục sản phẩm/dịch vụ");
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
        private void BindDataToControls(ProductServiceCategoryDto dto)
        {
            CategoryNameTextEdit.Text = dto.CategoryName;
            DescriptionMemoEdit.Text = dto.Description;
            
            // Chọn danh mục cha trong TreeListLookUpEdit
            if (dto.ParentId.HasValue)
            {
                ParentCategoryTreeListTreeListLookUpEdit.EditValue = dto.ParentId.Value;
                _hasUserSelectedParent = true;
            }
            else
            {
                ParentCategoryTreeListTreeListLookUpEdit.EditValue = null;
                _hasUserSelectedParent = false;
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ các control và tạo DTO.
        /// </summary>
        /// <returns>DTO chứa dữ liệu từ form</returns>
        private ProductServiceCategoryDto GetDataFromControls()
        {
            Guid? parentId = null;
            
            // Lấy giá trị từ TreeListLookUpEdit
            if (ParentCategoryTreeListTreeListLookUpEdit.EditValue != null && ParentCategoryTreeListTreeListLookUpEdit.EditValue != DBNull.Value)
            {
                parentId = (Guid)ParentCategoryTreeListTreeListLookUpEdit.EditValue;
            }
            
            return new ProductServiceCategoryDto
            {
                Id = _categoryId, // Sử dụng _categoryId (Guid.Empty cho thêm mới, ID thực cho edit)
                CategoryName = CategoryNameTextEdit?.Text?.Trim(),
                Description = DescriptionMemoEdit?.Text?.Trim(),
                ParentId = parentId
            };
        }
        

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

            // Kiểm tra ParentId không được trỏ đến chính nó (khi edit)
            if (IsEditMode && ParentCategoryTreeListTreeListLookUpEdit.EditValue != null && ParentCategoryTreeListTreeListLookUpEdit.EditValue != DBNull.Value)
            {
                var selectedParentId = (Guid)ParentCategoryTreeListTreeListLookUpEdit.EditValue;
                if (selectedParentId == _categoryId)
                {
                    dxErrorProvider1.SetError(ParentCategoryTreeListTreeListLookUpEdit, "Danh mục không thể là danh mục cha của chính nó", ErrorType.Critical);
                    ParentCategoryTreeListTreeListLookUpEdit?.Focus();
                    return false;
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
                var entity = dto.ToEntity();

                // Sử dụng SaveOrUpdate thay vì Insert/Update riêng biệt
                _productServiceCategoryBll.SaveOrUpdate(entity);

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

        /// <summary>
        /// Hiển thị thông tin.
        /// </summary>
        /// <param name="message">Thông báo</param>
        private void ShowInfo(string message)
        {
            MsgBox.ShowInfo(message);
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
            if (string.IsNullOrWhiteSpace(context))
                MsgBox.ShowException(ex);
            else
                MsgBox.ShowException(new Exception(context + ": " + ex.Message, ex));
        }

        /// <summary>
        /// Event handler khi user thay đổi giá trị trong TreeListLookUpEdit.
        /// </summary>
        private void ParentCategoryTreeListTreeListLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            if (ParentCategoryTreeListTreeListLookUpEdit.EditValue != null && ParentCategoryTreeListTreeListLookUpEdit.EditValue != DBNull.Value)
            {
                _hasUserSelectedParent = true;
            }
            else
            {
                _hasUserSelectedParent = false;
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Người dùng bấm nút Lưu.
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateInput())
            {
                SaveCategory();
            }
        }

        /// <summary>
        /// Người dùng bấm nút Hủy.
        /// </summary>
        private void CancelBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Xử lý phím tắt.
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
    }
}