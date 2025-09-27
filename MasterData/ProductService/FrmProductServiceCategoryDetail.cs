using Bll.MasterData.ProductService;
using Bll.Utils;
using MasterData.ProductService.Converters;
using System;
using System.Linq;
using System.Windows.Forms;
using MasterData.ProductService.Dto;
using DevExpress.XtraTreeList;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form chi tiết danh mục sản phẩm/dịch vụ - thêm mới và chỉnh sửa.
    /// </summary>
    public partial class FrmProductServiceCategoryDetail : DevExpress.XtraEditors.XtraForm
    {
        #region Fields

        private readonly ProductServiceCategoryBll _productServiceCategoryBll = new ProductServiceCategoryBll();
        private readonly Guid _categoryId;
        private bool _isEditMode => _categoryId != Guid.Empty;

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
                this.Text = _isEditMode ? "Điều chỉnh danh mục sản phẩm/dịch vụ" : "Thêm mới danh mục sản phẩm/dịch vụ";

                // Load dữ liệu nếu đang chỉnh sửa
                if (_isEditMode)
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
        /// Load danh sách danh mục cha vào TreeList.
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
                
                // Thiết lập TreeList
                ParentCategoryTreeList.DataSource = dtos;
                ParentCategoryTreeList.KeyFieldName = "Id";
                ParentCategoryTreeList.ParentFieldName = "ParentId";
                ParentCategoryTreeList.RootValue = null;
                
                // Thiết lập cột hiển thị
                ParentCategoryTreeList.Columns.Clear();
                var nameColumn = ParentCategoryTreeList.Columns.Add();
                nameColumn.FieldName = "CategoryName";
                nameColumn.Caption = "Tên danh mục";
                nameColumn.VisibleIndex = 0;
                nameColumn.Width = 200;
                
                var descColumn = ParentCategoryTreeList.Columns.Add();
                descColumn.FieldName = "Description";
                descColumn.Caption = "Mô tả";
                descColumn.VisibleIndex = 1;
                descColumn.Width = 150;
                
                var countColumn = ParentCategoryTreeList.Columns.Add();
                countColumn.FieldName = "ProductCount";
                countColumn.Caption = "Số SP/DV";
                countColumn.VisibleIndex = 2;
                countColumn.Width = 80;
                
                // Mở rộng tất cả các nút
                ParentCategoryTreeList.ExpandAll();
                
                // Thiết lập chọn dòng
                ParentCategoryTreeList.OptionsSelection.EnableAppearanceFocusedCell = false;
                ParentCategoryTreeList.OptionsSelection.MultiSelect = false;
                ParentCategoryTreeList.OptionsSelection.UseIndicatorForSelection = true;
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
                    this.DialogResult = DialogResult.Cancel;
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
            
            // Chọn danh mục cha trong TreeList
            if (dto.ParentId.HasValue)
            {
                var selectedNode = ParentCategoryTreeList.FindNode(treeNode => 
                {
                    var nodeId = treeNode.GetValue("Id");
                    return nodeId != null && (Guid)nodeId == dto.ParentId.Value;
                });
                if (selectedNode != null)
                {
                    ParentCategoryTreeList.FocusedNode = selectedNode;
                    ParentCategoryTreeList.Selection.Clear();
                    ParentCategoryTreeList.Selection.Add(selectedNode);
                }
            }
            else
            {
                ParentCategoryTreeList.FocusedNode = null;
                ParentCategoryTreeList.Selection.Clear();
            }
        }

        /// <summary>
        /// Lấy dữ liệu từ các control và tạo DTO.
        /// </summary>
        /// <returns>DTO chứa dữ liệu từ form</returns>
        private ProductServiceCategoryDto GetDataFromControls()
        {
            Guid? parentId = null;
            if (ParentCategoryTreeList.FocusedNode != null)
            {
                parentId = (Guid)ParentCategoryTreeList.FocusedNode.GetValue("Id");
            }
            
            return new ProductServiceCategoryDto
            {
                Id = _categoryId,
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
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên danh mục không được để trống", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài CategoryName
            if (CategoryNameTextEdit.Text.Trim().Length > 200)
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên danh mục không được vượt quá 200 ký tự", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp tên danh mục (không tính bản ghi đang chỉnh sửa)
            var categoryName = CategoryNameTextEdit.Text.Trim();
            if (_productServiceCategoryBll.IsCategoryNameExists(categoryName, _categoryId))
            {
                dxErrorProvider1.SetError(CategoryNameTextEdit, "Tên danh mục đã tồn tại trong hệ thống", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                CategoryNameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài Description
            if (!string.IsNullOrWhiteSpace(DescriptionMemoEdit?.Text) && DescriptionMemoEdit.Text.Trim().Length > 255)
            {
                dxErrorProvider1.SetError(DescriptionMemoEdit, "Mô tả không được vượt quá 255 ký tự", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                DescriptionMemoEdit?.Focus();
                return false;
            }

            // Kiểm tra ParentId không được trỏ đến chính nó (khi edit)
            if (_isEditMode && ParentCategoryTreeList.FocusedNode != null)
            {
                var selectedParentId = (Guid)ParentCategoryTreeList.FocusedNode.GetValue("Id");
                if (selectedParentId == _categoryId)
                {
                    dxErrorProvider1.SetError(ParentCategoryTreeList, "Danh mục không thể là danh mục cha của chính nó", DevExpress.XtraEditors.DXErrorProvider.ErrorType.Critical);
                    ParentCategoryTreeList?.Focus();
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

                if (_isEditMode)
                {
                    // Cập nhật danh mục hiện có
                    _productServiceCategoryBll.Update(entity);
                    ShowInfo("Cập nhật danh mục sản phẩm/dịch vụ thành công!");
                }
                else
                {
                    // Thêm danh mục mới
                    _productServiceCategoryBll.Insert(entity);
                    ShowInfo("Thêm mới danh mục sản phẩm/dịch vụ thành công!");
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
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

        #endregion

        #region Event Handlers

        /// <summary>
        /// Người dùng bấm nút Lưu.
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            if (ValidateInput())
            {
                SaveCategory();
            }
        }

        /// <summary>
        /// Người dùng bấm nút Hủy.
        /// </summary>
        private void CancelBarButtonItem_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
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
            else if (keyData == Keys.Escape)
            {
                CancelBarButtonItem_ItemClick(null, null);
                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #endregion
    }
}