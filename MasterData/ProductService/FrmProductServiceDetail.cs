using Bll.Utils;
using Bll.Common.ImageService;
using DevExpress.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using MasterData.ProductService.Converters;
using MasterData.ProductService.Dto;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.MasterData.ProductServiceBll;

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
        private readonly ImageService _imageService = new ImageService();
        private readonly ImageValidationService _imageValidationService = new ImageValidationService();
        private readonly ImageCompressionService _imageCompressionService = new ImageCompressionService();
        private readonly Guid _productServiceId;
        private bool IsEditMode => _productServiceId != Guid.Empty;
        private bool _hasImageChanged = false;
        private List<Dal.DataContext.ProductService> _activeProductServicesCache;

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
        /// Load danh sách sản phẩm/dịch vụ đang hoạt động và lưu vào cache cục bộ.
        /// </summary>
        private async Task LoadActiveProductServices()
        {
            try
            {
                var activeProductServices = await _productServiceBll.GetFilteredAsync(
                    isActive: true,
                    orderBy: "Name",
                    orderDirection: "ASC");

                _activeProductServicesCache = activeProductServices ?? new List<Dal.DataContext.ProductService>();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải danh sách sản phẩm/dịch vụ đang hoạt động");
            }
        }

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
            // Không còn sử dụng ThumbnailPathButtonEdit
            
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
                    // Sử dụng ImageService để load và compress ảnh
                    var compressedImage = LoadThumbnailImage(dto.ThumbnailImage);
                    if (compressedImage != null)
                    {
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
                    // Sử dụng ImageCompressionService để xử lý ảnh
                    thumbnailImage = ProcessThumbnailImage(ThumbnailImagePictureEdit.Image);
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
        /// Load và compress thumbnail image từ byte array
        /// </summary>
        /// <param name="imageData">Dữ liệu hình ảnh</param>
        /// <returns>Hình ảnh đã được resize và compress cho thumbnail</returns>
        private Image LoadThumbnailImage(byte[] imageData)
        {
            try
            {
                if (imageData == null || imageData.Length == 0)
                    return null;

                // Resize và compress cho thumbnail (max 512x512, max 100KB)
                var compressedData = _imageCompressionService.CompressImage(imageData, 100000, 512);
                return Image.FromStream(new MemoryStream(compressedData));
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi load thumbnail image");
                return null;
            }
        }

        /// <summary>
        /// Load và compress image từ file path cho thumbnail
        /// </summary>
        /// <param name="filePath">Đường dẫn file</param>
        /// <returns>Hình ảnh đã được resize và compress cho thumbnail</returns>
        private Image LoadAndCompressImage(string filePath)
        {
            try
            {
                if (!File.Exists(filePath))
                    return null;

                var imageData = File.ReadAllBytes(filePath);
                
                // Resize và compress cho thumbnail (max 512x512, max 100KB)
                var compressedData = _imageCompressionService.CompressImage(imageData, 100000, 512);
                return Image.FromStream(new MemoryStream(compressedData));
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi load và compress image");
                return null;
            }
        }

        /// <summary>
        /// Xử lý thumbnail image để lưu vào database
        /// </summary>
        /// <param name="image">Hình ảnh cần xử lý</param>
        /// <returns>Dữ liệu byte đã được resize và compress cho thumbnail</returns>
        private byte[] ProcessThumbnailImage(Image image)
        {
            try
            {
                if (image == null)
                    return null;

                // Convert Image to byte array
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    var imageData = ms.ToArray();

                    // Resize và compress cho thumbnail (max 512x512, max 100KB)
                    return _imageCompressionService.CompressImage(imageData, 100000, 512);
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xử lý thumbnail image");
                return null;
            }
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
            // Không còn validate ThumbnailPath vì đã chuyển sang ThumbnailImage

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

                // Lưu sản phẩm/dịch vụ với ảnh thumbnail
                _productServiceBll.SaveOrUpdate(entity);

                // Nếu có thay đổi ảnh và đã lưu thành công, cập nhật lại ảnh hiển thị
                if (_hasImageChanged && ThumbnailImagePictureEdit.Image != null)
                {
                    // Cập nhật lại ảnh hiển thị với ảnh đã lưu từ database
                    var savedProduct = _productServiceBll.GetById(entity.Id);
                    if (savedProduct != null && savedProduct.ThumbnailImage != null)
                    {
                        var compressedImage = LoadThumbnailImage(savedProduct.ThumbnailImage.ToArray());
                        if (compressedImage != null)
                        {
                            ThumbnailImagePictureEdit.Image = compressedImage;
                        }
                    }
                }

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

                    // Kiểm tra file với ImageValidationService
                    var fileInfo = new System.IO.FileInfo(selectedFilePath);
                    var imageData = File.ReadAllBytes(selectedFilePath);
                    
                    // Validate hình ảnh cho thumbnail (chấp nhận mọi kích thước)
                    var validationResult = _imageValidationService.ValidateImageForThumbnail(imageData, fileInfo.Name);
                    if (!validationResult.IsValid)
                    {
                        var errorMessage = string.Join("\n", validationResult.Errors);
                        ShowError($"File ảnh không hợp lệ:\n{errorMessage}");
                        return;
                    }

                    // Hiển thị warnings nếu có
                    if (validationResult.Warnings.Any())
                    {
                        var warningMessage = string.Join("\n", validationResult.Warnings);
                        ShowInfo($"Cảnh báo về file ảnh:\n{warningMessage}");
                    }

                    // Load và hiển thị ảnh
                    try
                    {
                        // Xóa ảnh cũ trước khi load ảnh mới
                        if (ThumbnailImagePictureEdit.Image != null)
                        {
                            ThumbnailImagePictureEdit.Image.Dispose();
                            ThumbnailImagePictureEdit.Image = null;
                        }

                        // Sử dụng ImageCompressionService để nén ảnh
                        var compressedImage = LoadAndCompressImage(selectedFilePath);
                        if (compressedImage != null)
                        {
                            ThumbnailImagePictureEdit.Image = compressedImage;
                            
                            // Đánh dấu đã thay đổi hình ảnh
                            _hasImageChanged = true;
                            
                            ShowInfo($"Đã chọn và resize ảnh thành công: {fileInfo.Name}\nKích thước gốc: {(fileInfo.Length / 1024.0):F1} KB\nĐã resize về thumbnail 512x512");
                        }
                    }
                    catch (Exception ex)
                    {
                        ShowError(ex, "Không thể load ảnh từ file đã chọn. File có thể bị hỏng hoặc không phải định dạng ảnh hợp lệ");
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
                // Không còn cần xóa ThumbnailPathButtonEdit vì đã chuyển sang ThumbnailImage
                
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