using Bll.MasterData.ProductService;
using Bll.Utils;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;
using System;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
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
        private readonly ProductImageBll _productImageBll = new ProductImageBll();
        private readonly Guid _productServiceId;
        private bool IsEditMode => _productServiceId != Guid.Empty;
        private bool _hasImageChanged = false;

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

                // Trong EditMode, không cho phép thay đổi mã và phân loại
                if (IsEditMode)
                {
                    CodeTextEdit.Enabled = false;
                    CategoryIdTreeListLookUpEdit.Enabled = false;
                    
                    // Thêm tooltip để giải thích
                    CodeTextEdit.Properties.NullText = "Mã không thể thay đổi khi chỉnh sửa";
                    CategoryIdTreeListLookUpEdit.Properties.NullText = "Phân loại không thể thay đổi khi chỉnh sửa";
                }

                // Đăng ký event handlers
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CancelBarButtonItem_ItemClick;
                ClearThumbnailBarButtonItem.ItemClick += ClearThumbnailBarButtonItem_ItemClick;
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
                try
                {
                    using (var originalImage = Image.FromStream(new MemoryStream(dto.ThumbnailImage)))
                    {
                        // Nén ảnh khi load để tránh lỗi Out of memory
                        var compressedImage = _productImageBll.CompressImage(originalImage, 80, 2048);
                        ThumbnailImagePictureEdit.Image = compressedImage;
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi khi load ảnh thumbnail từ database");
                }
            }
            
            
        }

        /// <summary>
        /// Lấy dữ liệu từ các control và tạo DTO.
        /// </summary>
        /// <returns>DTO chứa dữ liệu từ form</returns>
        private ProductServiceDto GetDataFromControls()
        {
            Guid? categoryId = null;
            
            // Lấy giá trị từ TreeListLookUpEdit
            if (CategoryIdTreeListLookUpEdit.EditValue != null && CategoryIdTreeListLookUpEdit.EditValue != DBNull.Value)
            {
                categoryId = (Guid)CategoryIdTreeListLookUpEdit.EditValue;
            }

            // Lấy ảnh thumbnail nếu có
            byte[] thumbnailImage = null;
            if (ThumbnailImagePictureEdit.Image != null)
            {
                try
                {
                    // Tạo một bản sao của ảnh để tránh lỗi GDI+
                    using (var imageCopy = new Bitmap(ThumbnailImagePictureEdit.Image))
                    {
                        using (var ms = new MemoryStream())
                        {
                            // Lưu ảnh với định dạng JPEG và chất lượng cao
                            var jpegCodec = GetJpegCodec();
                            if (jpegCodec != null)
                            {
                                var encoderParams = new EncoderParameters(1);
                                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, 90L);
                                imageCopy.Save(ms, jpegCodec, encoderParams);
                            }
                            else
                            {
                                // Fallback nếu không có JPEG codec
                                imageCopy.Save(ms, ImageFormat.Jpeg);
                            }
                            thumbnailImage = ms.ToArray();
                        }
                    }
                }
                catch (Exception ex)
                {
                    ShowError(ex, "Lỗi khi xử lý ảnh thumbnail");
                    thumbnailImage = null;
                }
            }

            return new ProductServiceDto
            {
                Id = _productServiceId, // Sử dụng _productServiceId (Guid.Empty cho thêm mới, ID thực cho edit)
                Code = CodeTextEdit?.Text?.Trim(),
                Name = NameTextEdit?.Text?.Trim(),
                Description = DescriptionTextEdit?.Text?.Trim(),
                CategoryId = categoryId,
                IsService = IsServiceToggleSwitch.IsOn,
                IsActive = IsActiveToggleSwitch.IsOn,
                ThumbnailPath = ThumbnailPathButtonEdit?.Text?.Trim(),
                ThumbnailImage = thumbnailImage
            };
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
        /// Lấy JPEG codec để lưu ảnh với chất lượng cao
        /// </summary>
        /// <returns>JPEG codec hoặc null</returns>
        private ImageCodecInfo GetJpegCodec()
        {
            try
            {
                var codecs = ImageCodecInfo.GetImageEncoders();
                foreach (var codec in codecs)
                {
                    if (codec.FormatID == ImageFormat.Jpeg.Guid)
                    {
                        return codec;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Validate dữ liệu đầu vào.
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không</returns>
        private bool ValidateInput()
        {
            dxErrorProvider1.ClearErrors();

            // Code bắt buộc
            if (string.IsNullOrWhiteSpace(CodeTextEdit?.Text))
            {
                dxErrorProvider1.SetError(CodeTextEdit, "Mã sản phẩm/dịch vụ không được để trống", ErrorType.Critical);
                CodeTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài Code
            var code = CodeTextEdit.Text.Trim();
            if (code.Length > 50)
            {
                dxErrorProvider1.SetError(CodeTextEdit, "Mã sản phẩm/dịch vụ không được vượt quá 50 ký tự", ErrorType.Critical);
                CodeTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp Code (chỉ kiểm tra khi thêm mới)
            if (!IsEditMode && _productServiceBll.IsCodeExists(code, _productServiceId))
            {
                dxErrorProvider1.SetError(CodeTextEdit, "Mã sản phẩm/dịch vụ đã tồn tại trong hệ thống", ErrorType.Critical);
                CodeTextEdit?.Focus();
                return false;
            }

            // Name bắt buộc
            if (string.IsNullOrWhiteSpace(NameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(NameTextEdit, "Tên sản phẩm/dịch vụ không được để trống", ErrorType.Critical);
                NameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài Name
            if (NameTextEdit.Text.Trim().Length > 200)
            {
                dxErrorProvider1.SetError(NameTextEdit, "Tên sản phẩm/dịch vụ không được vượt quá 200 ký tự", ErrorType.Critical);
                NameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp Name (chỉ kiểm tra khi thêm mới)
            var name = NameTextEdit.Text.Trim();
            if (!IsEditMode && _productServiceBll.IsNameExists(name, _productServiceId))
            {
                dxErrorProvider1.SetError(NameTextEdit, "Tên sản phẩm/dịch vụ đã tồn tại trong hệ thống", ErrorType.Critical);
                NameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài Description
            if (!string.IsNullOrWhiteSpace(DescriptionTextEdit?.Text) && DescriptionTextEdit.Text.Trim().Length > 1000)
            {
                dxErrorProvider1.SetError(DescriptionTextEdit, "Mô tả không được vượt quá 1000 ký tự", ErrorType.Critical);
                DescriptionTextEdit?.Focus();
                return false;
            }

            // Kiểm tra độ dài ThumbnailPath
            if (!string.IsNullOrWhiteSpace(ThumbnailPathButtonEdit?.Text) && ThumbnailPathButtonEdit.Text.Trim().Length > 500)
            {
                dxErrorProvider1.SetError(ThumbnailPathButtonEdit, "Đường dẫn ảnh không được vượt quá 500 ký tự", ErrorType.Critical);
                ThumbnailPathButtonEdit?.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lưu dữ liệu sản phẩm/dịch vụ.
        /// </summary>
        private void SaveProductService()
        {
            try
            {
                var dto = GetDataFromControls();
                var entity = dto.ToEntity();

                // Sử dụng SaveOrUpdateWithImage để xử lý hình ảnh
                var imagePath = ThumbnailPathButtonEdit.Text?.Trim();
                _productServiceBll.SaveOrUpdateWithImage(entity, imagePath, _hasImageChanged);

                var message = IsEditMode ? "Cập nhật sản phẩm/dịch vụ thành công!" : "Thêm mới sản phẩm/dịch vụ thành công!";
                ShowInfo(message);

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi lưu dữ liệu sản phẩm/dịch vụ");
            }
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

        #region Event Handlers

        /// <summary>
        /// Người dùng bấm nút Lưu.
        /// </summary>
        private void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (ValidateInput())
            {
                SaveProductService();
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
        /// Người dùng bấm nút Browse để chọn ảnh thumbnail.
        /// </summary>
        private void ThumbnailPathButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
        {
            try
            {
                // Sử dụng xtraOpenFileDialog1 đã được cấu hình trong Designer
                if (xtraOpenFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    var selectedFilePath = xtraOpenFileDialog1.FileName;
                    
                    // Kiểm tra file có tồn tại không
                    if (!System.IO.File.Exists(selectedFilePath))
                    {
                        ShowError("File đã chọn không tồn tại trên hệ thống");
                        return;
                    }

                    // Kiểm tra kích thước file (giới hạn 5MB)
                    var fileInfo = new System.IO.FileInfo(selectedFilePath);
                    //if (fileInfo.Length > 5 * 1024 * 1024) // 5MB
                    //{
                    //    ShowError("File ảnh quá lớn. Vui lòng chọn file nhỏ hơn 5MB");
                    //    return;
                    //}

                    // Cập nhật đường dẫn
                    ThumbnailPathButtonEdit.Text = selectedFilePath;

                    // Load và hiển thị ảnh
                    try
                    {
                        // Xóa ảnh cũ trước khi load ảnh mới
                        if (ThumbnailImagePictureEdit.Image != null)
                        {
                            ThumbnailImagePictureEdit.Image.Dispose();
                            ThumbnailImagePictureEdit.Image = null;
                        }

                        // Nén ảnh trước khi hiển thị để tránh lỗi Out of memory
                        var compressedImage = _productImageBll.CompressImage(selectedFilePath, 80, 2048);
                        ThumbnailImagePictureEdit.Image = compressedImage;
                        
                        // Đánh dấu đã thay đổi hình ảnh
                        _hasImageChanged = true;
                        
                        ShowInfo($"Đã chọn và nén ảnh thành công: {fileInfo.Name} ({(fileInfo.Length / 1024.0):F1} KB)");
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex, "Không thể load ảnh từ file đã chọn. File có thể bị hỏng hoặc không phải định dạng ảnh hợp lệ");
                        ThumbnailPathButtonEdit.Text = string.Empty;
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi chọn file ảnh");
            }
        }

        /// <summary>
        /// Người dùng bấm nút Clear để xóa ảnh thumbnail.
        /// </summary>
        private void ClearThumbnailBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Xóa đường dẫn
                ThumbnailPathButtonEdit.Text = string.Empty;
                
                // Dispose và xóa ảnh
                if (ThumbnailImagePictureEdit.Image != null)
                {
                    ThumbnailImagePictureEdit.Image.Dispose();
                    ThumbnailImagePictureEdit.Image = null;
                }
                
                // Đánh dấu đã thay đổi hình ảnh (xóa ảnh)
                _hasImageChanged = true;
                
                ShowInfo("Đã xóa ảnh thumbnail thành công");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xóa ảnh thumbnail");
            }
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