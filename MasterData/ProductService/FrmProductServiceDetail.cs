using Bll.Common.ImageService;
using Bll.MasterData.ProductServiceBll;
using Common.Common;
using Common.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraSplashScreen;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MasterData.ProductService
{
    /// <summary>
    /// Form chi tiết sản phẩm/dịch vụ - thêm mới và chỉnh sửa.
    /// Cung cấp chức năng CRUD đầy đủ với validation nghiệp vụ và giao diện thân thiện.
    /// </summary>
    public partial class FrmProductServiceDetail : XtraForm
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho sản phẩm/dịch vụ
        /// </summary>
        private readonly ProductServiceBll _productServiceBll = new ProductServiceBll();

        /// <summary>
        /// Business Logic Layer cho danh mục sản phẩm/dịch vụ
        /// </summary>
        private readonly ProductServiceCategoryBll _productServiceCategoryBll = new ProductServiceCategoryBll();

        /// <summary>
        /// Business Logic Layer cho hình ảnh sản phẩm
        /// </summary>
        private readonly ProductImageBll _productImageBll = new ProductImageBll();

        /// <summary>
        /// Service validation hình ảnh
        /// </summary>
        private readonly ImageValidationService _imageValidationService = new ImageValidationService();

        /// <summary>
        /// Service nén hình ảnh
        /// </summary>
        private readonly ImageCompressionService _imageCompressionService = new ImageCompressionService();

        /// <summary>
        /// ID của sản phẩm/dịch vụ đang chỉnh sửa (Guid.Empty nếu thêm mới)
        /// </summary>
        private readonly Guid _productServiceId;

        /// <summary>
        /// Trạng thái đang trong chế độ chỉnh sửa (true) hay thêm mới (false)
        /// </summary>
        private bool IsEditMode => _productServiceId != Guid.Empty;

        /// <summary>
        /// Trạng thái hình ảnh đã thay đổi
        /// </summary>
        private bool _hasImageChanged;

        /// <summary>
        /// Cache danh sách sản phẩm/dịch vụ đang hoạt động
        /// </summary>
        private List<ProductServiceDto> _activeProductServicesCache;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

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

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Form load event.
        /// </summary>
        private async void FrmProductServiceDetail_Load(object sender, EventArgs e)
        {
            try
            {
                await LoadCategoriesAsync();
                
                if (IsEditMode)
                {
                    await LoadProductServiceDataAsync();
                }
                
                // Thiết lập focus cho control đầu tiên
                CodeTextEdit.Focus();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
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

                // Cho phép chỉnh sửa tất cả các trường kể cả khi edit
                // Validation sẽ đảm bảo mã không trùng lặp

                // Đăng ký event handlers
                SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
                CloseBarButtonItem.ItemClick += CancelBarButtonItem_ItemClick;
                ClearThumbnailBarButtonItem.ItemClick += ClearThumbnailBarButtonItem_ItemClick;

                // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
                RequiredFieldHelper.MarkRequiredFields(this, typeof(ProductServiceDto));

                // Thiết lập SuperToolTip cho các controls
                SetupSuperToolTips();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khởi tạo form");
            }
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

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

                // GetFilteredAsync() already returns List<ProductServiceDto>
                _activeProductServicesCache = activeProductServices ?? new List<ProductServiceDto>();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi tải danh sách sản phẩm/dịch vụ đang hoạt động");
            }
        }

        /// <summary>
        /// Load danh sách danh mục vào CategoryIdSearchLookupEdit (async).
        /// </summary>
        private async Task LoadCategoriesAsync()
        {
            try
            {
                // GetCategoriesWithCountsAsync() already returns List<ProductServiceCategoryDto>
                var (categories, counts) = await _productServiceCategoryBll.GetCategoriesWithCountsAsync();
                
                // Calculate hierarchy properties manually on DTOs
                var categoryDict = categories.ToDictionary(c => c.Id);
                var dtos = categories.Select(dto =>
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
                
                // Thiết lập BindingSource
                productServiceCategoryDtoBindingSource.DataSource = dtos;
                
                // Đăng ký event để tự động tạo mã sản phẩm khi thay đổi danh mục (chỉ khi thêm mới)
                CategoryIdSearchLookupEdit.EditValueChanged += CategoryIdSearchLookupEdit_EditValueChanged;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải danh sách danh mục");
            }
        }

        /// <summary>
        /// Load dữ liệu sản phẩm/dịch vụ để chỉnh sửa.
        /// </summary>
        private async Task LoadProductServiceDataAsync()
        {
            try
            {
                // GetById() already returns ProductServiceDto
                var productService = _productServiceBll.GetById(_productServiceId);
                if (productService == null)
                {
                    ShowError("Không tìm thấy sản phẩm/dịch vụ");
                    DialogResult = DialogResult.Cancel;
                    return;
                }

                BindDataToControls(productService);
                
                // Load hình ảnh từ bảng ProductImage nếu có
                LoadProductImagesFromDatabase();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi tải dữ liệu sản phẩm/dịch vụ");
            }
        }

        /// <summary>
        /// Load hình ảnh từ bảng ProductImage
        /// </summary>
        private void LoadProductImagesFromDatabase()
        {
            try
            {
                // Tìm ảnh chính của sản phẩm
                var primaryImage = _productImageBll.GetPrimaryByProductId(_productServiceId);
                if (primaryImage != null && primaryImage.ImageData != null && primaryImage.ImageData.Length > 0)
                {
                    // Load ảnh chính vào ThumbnailImagePictureEdit
                    var compressedImage = LoadThumbnailImage(primaryImage.ImageData.ToArray());
                    if (compressedImage != null)
                    {
                        ThumbnailImagePictureEdit.Image = compressedImage;
                        _hasImageChanged = false; // Reset flag vì đây là load từ database
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi load hình ảnh từ database");
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU FORM ==========

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
            
            // Chọn danh mục trong SearchLookUpEdit
            if (dto.CategoryId.HasValue)
            {
                CategoryIdSearchLookupEdit.EditValue = dto.CategoryId.Value;
            }
            else
            {
                CategoryIdSearchLookupEdit.EditValue = null;
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
            
            // Lấy giá trị từ SearchLookUpEdit
            if (CategoryIdSearchLookupEdit.EditValue != null && CategoryIdSearchLookupEdit.EditValue != DBNull.Value)
            {
                if (CategoryIdSearchLookupEdit.EditValue is Guid guidValue)
                {
                    categoryId = guidValue;
                }
                else if (Guid.TryParse(CategoryIdSearchLookupEdit.EditValue.ToString(), out var parsedGuid))
                {
                    categoryId = parsedGuid;
                }
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

        #endregion

        #region ========== XỬ LÝ HÌNH ẢNH ==========

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

        #endregion

        #region ========== VALIDATION ==========

        /// <summary>
        /// Validate dữ liệu đầu vào.
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không</returns>
        private bool ValidateInput()
        {
            dxErrorProvider1.ClearErrors();

            // Validate mã sản phẩm/dịch vụ (bắt buộc và không trùng lặp)
            if (!ValidateCode())
            {
                return false;
            }

            // Validate tên sản phẩm/dịch vụ (bắt buộc và không trùng lặp)
            if (!ValidateName())
            {
                return false;
            }

            // Validate mô tả (tùy chọn, nhưng có giới hạn độ dài)
            if (!ValidateDescription())
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Validate mã sản phẩm/dịch vụ (bắt buộc và không trùng lặp)
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        private bool ValidateCode()
        {
            // Kiểm tra không được để trống
            if (string.IsNullOrWhiteSpace(CodeTextEdit?.Text))
            {
                dxErrorProvider1.SetError(CodeTextEdit, "Mã sản phẩm/dịch vụ không được để trống", ErrorType.Critical);
                CodeTextEdit?.Focus();
                return false;
            }

            var code = CodeTextEdit.Text.Trim();

            // Kiểm tra độ dài Code
            if (code.Length > 50)
            {
                dxErrorProvider1.SetError(CodeTextEdit, "Mã sản phẩm/dịch vụ không được vượt quá 50 ký tự", ErrorType.Critical);
                CodeTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp Code
            if (IsEditMode)
            {
                // Nếu đang chỉnh sửa, chỉ kiểm tra trùng khi mã đã thay đổi
                var existingProduct = _productServiceBll.GetById(_productServiceId);
                if (existingProduct != null && existingProduct.Code != code)
                {
                    // Mã đã thay đổi, kiểm tra trùng lặp (loại trừ ID hiện tại)
                    if (_productServiceBll.IsCodeExists(code, _productServiceId))
                    {
                        dxErrorProvider1.SetError(CodeTextEdit, "Mã sản phẩm/dịch vụ đã tồn tại trong hệ thống", ErrorType.Critical);
                        CodeTextEdit?.Focus();
                        return false;
                    }
                }
            }
            else
            {
                // Nếu thêm mới, luôn kiểm tra trùng
                if (_productServiceBll.IsCodeExists(code, null))
                {
                    dxErrorProvider1.SetError(CodeTextEdit, "Mã sản phẩm/dịch vụ đã tồn tại trong hệ thống", ErrorType.Critical);
                    CodeTextEdit?.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validate tên sản phẩm/dịch vụ (bắt buộc và không trùng lặp)
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        private bool ValidateName()
        {
            // Kiểm tra không được để trống
            if (string.IsNullOrWhiteSpace(NameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(NameTextEdit, "Tên sản phẩm/dịch vụ không được để trống", ErrorType.Critical);
                NameTextEdit?.Focus();
                return false;
            }

            var name = NameTextEdit.Text.Trim();

            // Kiểm tra độ dài Name
            if (name.Length > 200)
            {
                dxErrorProvider1.SetError(NameTextEdit, "Tên sản phẩm/dịch vụ không được vượt quá 200 ký tự", ErrorType.Critical);
                NameTextEdit?.Focus();
                return false;
            }

            // Kiểm tra trùng lặp Name
            if (IsEditMode)
            {
                // Nếu đang chỉnh sửa, chỉ kiểm tra trùng khi tên đã thay đổi
                var existingProduct = _productServiceBll.GetById(_productServiceId);
                if (existingProduct != null && existingProduct.Name != name)
                {
                    // Tên đã thay đổi, kiểm tra trùng lặp (loại trừ ID hiện tại)
                    if (_productServiceBll.IsNameExists(name, _productServiceId))
                    {
                        dxErrorProvider1.SetError(NameTextEdit, "Tên sản phẩm/dịch vụ đã tồn tại trong hệ thống", ErrorType.Critical);
                        NameTextEdit?.Focus();
                        return false;
                    }
                }
            }
            else
            {
                // Nếu thêm mới, luôn kiểm tra trùng
                if (_productServiceBll.IsNameExists(name, null))
                {
                    dxErrorProvider1.SetError(NameTextEdit, "Tên sản phẩm/dịch vụ đã tồn tại trong hệ thống", ErrorType.Critical);
                    NameTextEdit?.Focus();
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Validate mô tả (tùy chọn, nhưng có giới hạn độ dài)
        /// </summary>
        /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
        private bool ValidateDescription()
        {
            // Kiểm tra độ dài Description (nếu có)
            if (!string.IsNullOrWhiteSpace(DescriptionTextEdit?.Text) && DescriptionTextEdit.Text.Trim().Length > 1000)
            {
                dxErrorProvider1.SetError(DescriptionTextEdit, "Mô tả không được vượt quá 1000 ký tự", ErrorType.Critical);
                DescriptionTextEdit?.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region ========== CHỨC NĂNG LƯU DỮ LIỆU ==========

        /// <summary>
        /// Lưu dữ liệu sản phẩm/dịch vụ (async với waiting form).
        /// </summary>
        private async void SaveProductService()
        {
            try
            {
                // Lưu dữ liệu với waiting form
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await SaveProductServiceAsync();
                });

                // Thông báo thành công và đóng form
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
        /// Lưu dữ liệu sản phẩm/dịch vụ (async implementation).
        /// </summary>
        private Task SaveProductServiceAsync()
        {
            var dto = GetDataFromControls();

            // SaveOrUpdate expects ProductServiceDto directly
            _productServiceBll.SaveOrUpdate(dto);

            // Nếu có thay đổi ảnh và đã lưu thành công, cập nhật lại ảnh hiển thị
            if (_hasImageChanged && ThumbnailImagePictureEdit.Image != null)
            {
                // GetById() already returns ProductServiceDto
                var savedProduct = _productServiceBll.GetById(dto.Id);
                if (savedProduct != null && savedProduct.ThumbnailImage != null)
                {
                    var compressedImage = LoadThumbnailImage(savedProduct.ThumbnailImage.ToArray());
                    if (compressedImage != null)
                    {
                        ThumbnailImagePictureEdit.Image = compressedImage;
                    }
                    // Bước 2: Thêm hình ảnh vào bảng ProductImage nếu có thay đổi
                    if (_hasImageChanged)
                    {
                        SaveProductImages(savedProduct.Id);
                    }
                }
                
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Thêm hình ảnh vào bảng ProductImage (sau khi đã cập nhật ProductService)
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        private void SaveProductImages(Guid productId)
        {
            try
            {
                if (ThumbnailImagePictureEdit.Image == null)
                {
                    // Nếu không có ảnh, không làm gì với ProductImage
                    // (Ảnh thumbnail trong ProductService đã được xử lý ở SaveProductService)
                    return;
                }

                // Lấy tên sản phẩm để tạo caption
                var productName = NameTextEdit?.Text?.Trim() ?? "Sản phẩm";

                // Lưu ảnh thumbnail vào file tạm thời để sử dụng SaveImageFromFile
                var tempFilePath = SaveImageToTempFile(ThumbnailImagePictureEdit.Image);
                if (!string.IsNullOrEmpty(tempFilePath))
                {
                    try
                    {
                        // Thêm ảnh mới vào ProductImage (không xóa ảnh cũ)
                        var savedImage = _productImageBll.SaveImageFromFile(
                            productId, 
                            tempFilePath, 
                            isPrimary: true, 
                            caption: $"{productName} - Ảnh chính",
                            altText: $"Ảnh chính của {productName}"
                        );

                        if (savedImage != null)
                        {
                            ShowInfo($"Đã cập nhật hình ảnh sản phẩm thành công!\n\n✅ Bước 1: Cập nhật thumbnail trong ProductService\n✅ Bước 2: Thêm ảnh chính vào ProductImage\n\nẢnh: {savedImage.FileName ?? "N/A"}");
                        }
                    }
                    finally
                    {
                        // Xóa file tạm thời
                        if (File.Exists(tempFilePath))
                        {
                            File.Delete(tempFilePath);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi lưu hình ảnh sản phẩm");
            }
        }

        /// <summary>
        /// Lưu ảnh vào file tạm thời để sử dụng SaveImageFromFile
        /// </summary>
        /// <param name="image">Ảnh cần lưu</param>
        /// <returns>Đường dẫn file tạm thời</returns>
        private string SaveImageToTempFile(Image image)
        {
            try
            {
                if (image == null) return null;

                // Tạo file tạm thời
                var tempFileName = $"temp_product_image_{Guid.NewGuid():N}.jpg";
                var tempFilePath = Path.Combine(Path.GetTempPath(), tempFileName);

                // Lưu ảnh dưới dạng JPEG
                using (var ms = new MemoryStream())
                {
                    image.Save(ms, ImageFormat.Jpeg);
                    File.WriteAllBytes(tempFilePath, ms.ToArray());
                }

                return tempFilePath;
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi lưu ảnh vào file tạm thời");
                return null;
            }
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Event handler khi user thay đổi danh mục -> tự động tạo mã sản phẩm.
        /// </summary>
        private void CategoryIdSearchLookupEdit_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                // Chỉ tự động tạo mã khi đang thêm mới (không phải edit mode)
                if (IsEditMode)
                    return;

                // Lấy danh mục được chọn
                Guid? selectedCategoryId = null;
                if (CategoryIdSearchLookupEdit.EditValue != null && CategoryIdSearchLookupEdit.EditValue != DBNull.Value)
                {
                    if (CategoryIdSearchLookupEdit.EditValue is Guid guidValue)
                    {
                        selectedCategoryId = guidValue;
                    }
                    else if (Guid.TryParse(CategoryIdSearchLookupEdit.EditValue.ToString(), out var parsedGuid))
                    {
                        selectedCategoryId = parsedGuid;
                    }
                }

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

        #region ========== SỰ KIỆN TOOLBAR ==========

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

        #endregion

        #region ========== SỰ KIỆN HÌNH ẢNH ==========

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
                    if (!File.Exists(selectedFilePath))
                    {
                        ShowError("File đã chọn không tồn tại trên hệ thống");
                        return;
                    }

                    // Kiểm tra file với ImageValidationService
                    var fileInfo = new FileInfo(selectedFilePath);
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
                            
                            ShowInfo($"Đã chọn và resize ảnh thành công: {fileInfo.Name}\nKích thước gốc: {(fileInfo.Length / 1024.0):F1} KB\nĐã resize về thumbnail 512x512\n\nLưu ý: Khi lưu sản phẩm, ảnh này sẽ được:\n1️⃣ Cập nhật thumbnail trong ProductService\n2️⃣ Thêm vào ProductImage làm ảnh chính");
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
                
                ShowInfo("Đã xóa ảnh thumbnail thành công\n\nLưu ý: Khi lưu sản phẩm:\n1️⃣ Thumbnail trong ProductService sẽ được xóa\n2️⃣ Không ảnh hưởng đến ảnh cũ trong ProductImage");
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lỗi khi xóa ảnh thumbnail");
            }
        }

        #endregion

        #region ========== PHÍM TẮT ==========

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

        #region ========== TIỆN ÍCH HIỂN THỊ ==========

        /// <summary>
        /// Thực hiện operation với waiting form (hiển thị splash screen trong khi xử lý)
        /// </summary>
        /// <param name="operation">Operation cần thực hiện (async)</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị waiting form
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng waiting form
                SplashScreenManager.CloseForm();
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (CodeTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        CodeTextEdit,
                        title: "<b><color=DarkBlue>🔖 Mã sản phẩm/dịch vụ</color></b>",
                        content: "Nhập mã sản phẩm/dịch vụ duy nhất. Trường này là bắt buộc. Mã sẽ được tự động tạo khi chọn danh mục (chế độ thêm mới)."
                    );
                }

                if (NameTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        NameTextEdit,
                        title: "<b><color=DarkBlue>📦 Tên sản phẩm/dịch vụ</color></b>",
                        content: "Nhập tên đầy đủ của sản phẩm hoặc dịch vụ. Trường này là bắt buộc."
                    );
                }

                if (CategoryIdSearchLookupEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        CategoryIdSearchLookupEdit,
                        title: "<b><color=DarkBlue>📂 Danh mục</color></b>",
                        content: "Chọn danh mục sản phẩm/dịch vụ (tùy chọn). Mã sản phẩm sẽ được tự động tạo khi chọn danh mục (chế độ thêm mới)."
                    );
                }

                if (DescriptionTextEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        DescriptionTextEdit,
                        title: "<b><color=DarkBlue>📝 Mô tả</color></b>",
                        content: "Nhập mô tả chi tiết về sản phẩm/dịch vụ (tối đa 1000 ký tự)."
                    );
                }

                if (ThumbnailImagePictureEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        ThumbnailImagePictureEdit,
                        title: "<b><color=DarkBlue>🖼️ Ảnh đại diện</color></b>",
                        content: "Chọn ảnh đại diện cho sản phẩm/dịch vụ. Ảnh sẽ được tự động resize và nén."
                    );
                }

                if (IsServiceToggleSwitch != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsServiceToggleSwitch,
                        title: "<b><color=DarkBlue>🏷️ Loại</color></b>",
                        content: "Bật nếu đây là dịch vụ, tắt nếu là sản phẩm."
                    );
                }

                if (IsActiveToggleSwitch != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        IsActiveToggleSwitch,
                        title: "<b><color=DarkBlue>✅ Trạng thái</color></b>",
                        content: "Bật nếu sản phẩm/dịch vụ đang hoạt động."
                    );
                }

                if (SaveBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: "Lưu thông tin sản phẩm/dịch vụ vào hệ thống."
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

                if (ClearThumbnailBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ClearThumbnailBarButtonItem,
                        title: "<b><color=Orange>🗑️ Xóa ảnh</color></b>",
                        content: "Xóa ảnh đại diện khỏi sản phẩm/dịch vụ."
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
        /// Hiển thị thông báo thông tin
        /// </summary>
        /// <param name="message">Nội dung thông báo</param>
        private void ShowInfo(string message)
        {
            MsgBox.ShowSuccess(message);
        }

        /// <summary>
        /// Hiển thị thông báo lỗi
        /// </summary>
        /// <param name="message">Nội dung thông báo lỗi</param>
        private void ShowError(string message)
        {
            MsgBox.ShowError(message);
        }

        /// <summary>
        /// Hiển thị thông báo lỗi với thông tin ngữ cảnh
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