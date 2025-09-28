using Bll.MasterData.ProductService;
using Bll.Utils;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form chi tiết sản phẩm/dịch vụ - thêm mới và chỉnh sửa.
    /// </summary>
    public partial class FrmProductServiceDetail : XtraForm
    {
        #region Fields

        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();
        private readonly ProductServiceCategoryBll _productServiceCategoryBll = new ProductServiceCategoryBll();
        private readonly Guid _productServiceId;
        private bool IsEditMode => _productServiceId != Guid.Empty;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo form cho thêm mới sản phẩm/dịch vụ.
        /// </summary>
        public FrmProductServiceDetail() : this(Guid.Empty)
        {
        }

        /// <summary>
        /// Khởi tạo form cho chỉnh sửa sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="productServiceId">ID của sản phẩm/dịch vụ cần chỉnh sửa</param>
        public FrmProductServiceDetail(Guid productServiceId)
        {
            InitializeComponent();
            _productServiceId = productServiceId;
            InitializeForm();
        }

        #endregion

        #region Form Events

        /// <summary>
        /// Form load event.
        /// </summary>
        private void FrmProductServiceDetail_Load(object sender, EventArgs e)
        {
            try
            {
                LoadCategories();
                
                if (IsEditMode)
                {
                    LoadProductServiceData();
                }
                
                // Thiết lập focus cho control đầu tiên
                CodeTextEdit.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Khởi tạo form và thiết lập các thuộc tính cơ bản.
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Thiết lập tiêu đề form
                Text = IsEditMode ? "Điều chỉnh sản phẩm/dịch vụ" : "Thêm mới sản phẩm/dịch vụ";

                // Thiết lập giá trị mặc định
                IsActiveToggleSwitch.IsOn = true; // Mặc định là hoạt động
                IsServiceToggleSwitch.IsOn = false; // Mặc định là sản phẩm
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        /// <summary>
        /// Load danh sách danh mục vào CategoryIdTreeListLookUpEdit.
        /// </summary>
        private void LoadCategories()
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
                CategoryIdTreeListLookUpEdit.Properties.DataSource = dtos;
                CategoryIdTreeListLookUpEdit.Properties.ValueMember = "Id";
                CategoryIdTreeListLookUpEdit.Properties.DisplayMember = "CategoryName";
                
                // Thiết lập TreeList bên trong TreeListLookUpEdit
                CategoryIdTreeListLookUpEdit.Properties.TreeList.KeyFieldName = "Id";
                CategoryIdTreeListLookUpEdit.Properties.TreeList.ParentFieldName = "ParentId";
                CategoryIdTreeListLookUpEdit.Properties.TreeList.RootValue = null;
                
                // Thiết lập cột hiển thị
                CategoryIdTreeListLookUpEdit.Properties.TreeList.Columns.Clear();
                var nameColumn = CategoryIdTreeListLookUpEdit.Properties.TreeList.Columns.Add();
                nameColumn.FieldName = "CategoryName";
                nameColumn.Caption = "Tên danh mục";
                nameColumn.VisibleIndex = 0;
                nameColumn.Width = 200;
                
                var descColumn = CategoryIdTreeListLookUpEdit.Properties.TreeList.Columns.Add();
                descColumn.FieldName = "Description";
                descColumn.Caption = "Mô tả";
                descColumn.VisibleIndex = 1;
                descColumn.Width = 150;
                
                var countColumn = CategoryIdTreeListLookUpEdit.Properties.TreeList.Columns.Add();
                countColumn.FieldName = "ProductCount";
                countColumn.Caption = "Số SP/DV";
                countColumn.VisibleIndex = 2;
                countColumn.Width = 80;
                
                // Thiết lập TreeList để hiển thị đúng cấu trúc cây
                CategoryIdTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowIndentAsRowStyle = true;
                CategoryIdTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowHorzLines = true;
                CategoryIdTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowVertLines = true;
                CategoryIdTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowRoot = true;
                CategoryIdTreeListLookUpEdit.Properties.TreeList.OptionsView.ShowButtons = true;
                
                // Mở rộng tất cả các nút
                CategoryIdTreeListLookUpEdit.Properties.TreeList.ExpandAll();
                
                // Thiết lập các tùy chọn
                CategoryIdTreeListLookUpEdit.Properties.AllowNullInput = DefaultBoolean.True;
                CategoryIdTreeListLookUpEdit.Properties.NullText = @"Chọn danh mục (tùy chọn)";
                
                // Các tính năng bổ sung theo tài liệu DevExpress
                CategoryIdTreeListLookUpEdit.Properties.AutoComplete = true; // Tự động hoàn thành khi gõ
                CategoryIdTreeListLookUpEdit.Properties.AutoExpandAllNodes = true; // Tự động mở rộng tất cả nodes
                CategoryIdTreeListLookUpEdit.Properties.PopupFilterMode = PopupFilterMode.Contains; // Lọc khi gõ
                CategoryIdTreeListLookUpEdit.Properties.TextEditStyle = TextEditStyles.Standard; // Cho phép chỉnh sửa text
                
                // Đăng ký event để tự động tạo mã sản phẩm khi thay đổi danh mục
                CategoryIdTreeListLookUpEdit.EditValueChanged += CategoryIdTreeListLookUpEdit_EditValueChanged;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải danh sách danh mục");
            }
        }

        /// <summary>
        /// Load dữ liệu sản phẩm/dịch vụ để chỉnh sửa.
        /// </summary>
        private void LoadProductServiceData()
        {
            try
            {
                var productService = _productServiceBll.GetById(_productServiceId);
                if (productService == null)
                {
                    ShowError("Không tìm thấy sản phẩm/dịch vụ");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                var dto = productService.ToDto(categoryId => _productServiceBll.GetCategoryName(categoryId));
                BindDataToControls(dto);
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu sản phẩm/dịch vụ");
            }
        }

        /// <summary>
        /// Bind dữ liệu DTO vào các control.
        /// </summary>
        /// <param name="dto">DTO chứa dữ liệu</param>
        private void BindDataToControls(ProductServiceDto dto)
        {
            CodeTextEdit.Text = dto.Code;
            NameTextEdit.Text = dto.Name;
            DescriptionTextEdit.Text = dto.Description;
            IsServiceToggleSwitch.IsOn = dto.IsService;
            IsActiveToggleSwitch.IsOn = dto.IsActive;
            ThumbnailPathButtonEdit.Text = dto.ThumbnailPath;
            
            // Chọn danh mục trong TreeListLookUpEdit
            if (dto.CategoryId.HasValue)
            {
                CategoryIdTreeListLookUpEdit.EditValue = dto.CategoryId.Value;
            }
            else
            {
                CategoryIdTreeListLookUpEdit.EditValue = null;
            }

            // Load ảnh thumbnail nếu có
            if (dto.ThumbnailImage != null && dto.ThumbnailImage.Length > 0)
            {
                ThumbnailImagePictureEdit.Image = Image.FromStream(new MemoryStream(dto.ThumbnailImage));
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
        /// Event handler khi user thay đổi danh mục -> tự động tạo mã sản phẩm.
        /// </summary>
        private void CategoryIdTreeListLookUpEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Chỉ tự động tạo mã khi đang thêm mới (không phải edit mode)
                if (IsEditMode)
                    return;

                // Lấy danh mục được chọn
                var selectedCategoryId = CategoryIdTreeListLookUpEdit.EditValue as Guid?;
                if (selectedCategoryId == null || selectedCategoryId == Guid.Empty)
                {
                    // Nếu không chọn danh mục, để trống mã
                    CodeTextEdit.Text = string.Empty;
                    return;
                }

                // Tự động tạo mã sản phẩm mới
                var newCode = _productServiceBll.GenerateProductCode(selectedCategoryId.Value);
                if (!string.IsNullOrEmpty(newCode))
                {
                    CodeTextEdit.Text = newCode;
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tự động tạo mã sản phẩm");
            }
        }

        #endregion
    }
}