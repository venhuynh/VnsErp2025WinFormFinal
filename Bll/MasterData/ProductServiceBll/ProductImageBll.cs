using Common;
using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DtoConverter;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bll.Common.ImageStorage;
using Bll.Common.ImageService;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using ProductImage = Dal.DataContext.ProductImage;

namespace Bll.MasterData.ProductServiceBll
{
    /// <summary>
    /// Business Logic Layer cho ProductImage
    /// Sử dụng ImageStorageService để lưu trữ hình ảnh trên NAS/Local thay vì database
    /// </summary>
    public class ProductImageBll
    {

        #region Fields

        private IProductImageRepository _dataAccess;
        private IImageStorageService _imageStorage;
        private readonly object _imageStorageLock = new object();
        private readonly ImageCompressionService _imageCompression;
        private readonly ILogger _logger;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ProductImageBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
            _imageCompression = new ImageCompressionService();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IProductImageRepository GetDataAccess()
        {
            if (_dataAccess == null)
            {
                lock (_lockObject)
                {
                    if (_dataAccess == null)
                    {
                        try
                        {
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _dataAccess = new ProductImageRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                        }
                    }
                }
            }

            return _dataAccess;
        }

        /// <summary>
        /// Lấy hoặc khởi tạo ImageStorageService (lazy initialization)
        /// Chỉ khởi tạo khi thực sự cần dùng (khi upload/delete image)
        /// </summary>
        private IImageStorageService GetImageStorage()
        {
            if (_imageStorage == null)
            {
                lock (_imageStorageLock)
                {
                    if (_imageStorage == null)
                    {
                        try
                        {
                            _imageStorage = ImageStorageFactory.CreateFromConfig(_logger);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khi khởi tạo ImageStorageService: {ex.Message}", ex);
                            throw new InvalidOperationException(
                                "Không thể khởi tạo ImageStorageService. Vui lòng kiểm tra cấu hình NAS trong bảng Setting. " +
                                "Nếu không sử dụng NAS, hãy đặt NAS.StorageType = 'Local' trong bảng Setting.", ex);
                        }
                    }
                }
            }

            return _imageStorage;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả hình ảnh
        /// </summary>
        /// <returns>Danh sách tất cả hình ảnh</returns>
        public List<ProductImageDto> GetAll()
        {
            try
            {
                return GetDataAccess().GetAll();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy tất cả hình ảnh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách hình ảnh của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <returns>Danh sách hình ảnh</returns>
        public List<ProductImageDto> GetByProductId(Guid productId)
        {
            try
            {
                return GetDataAccess().GetByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy danh sách hình ảnh cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy hình ảnh theo ID
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        /// <returns>Hình ảnh hoặc null</returns>
        public ProductImageDto GetById(Guid imageId)
        {
            try
            {
                return GetDataAccess().GetById(imageId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra file hình ảnh có tồn tại trên storage (NAS/Local) không
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>True nếu file tồn tại</returns>
        public async Task<bool> CheckImageFileExistsAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(relativePath))
                {
                    return false;
                }

                return await GetImageStorage().ImageExistsAsync(relativePath);
            }
            catch (Exception ex)
            {
                _logger.Warning($"Lỗi khi kiểm tra file tồn tại '{relativePath}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lấy dữ liệu hình ảnh từ storage (NAS/Local) theo RelativePath
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>Dữ liệu hình ảnh (byte array) hoặc null</returns>
        public async Task<byte[]> GetImageDataByRelativePathAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(relativePath))
                {
                    _logger.Warning("RelativePath không được để trống");
                    return null;
                }

                // Lấy từ storage
                var imageData = await GetImageStorage().GetImageAsync(relativePath);
                
                if (imageData == null)
                {
                    _logger.Warning($"Không thể đọc file từ storage, RelativePath={relativePath}");
                }

                return imageData;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy dữ liệu hình ảnh từ RelativePath '{relativePath}': {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Lấy dữ liệu hình ảnh từ storage (NAS/Local)
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        /// <returns>Dữ liệu hình ảnh (byte array) hoặc null</returns>
        public async Task<byte[]> GetImageDataAsync(Guid imageId)
        {
            try
            {
                var productImage = GetDataAccess().GetById(imageId);
                if (productImage == null)
                {
                    _logger.Warning($"Không tìm thấy hình ảnh với ID '{imageId}'");
                    return null;
                }

                // Sử dụng RelativePath
                var relativePath = productImage.RelativePath;
                
                if (string.IsNullOrEmpty(relativePath))
                {
                    _logger.Warning($"Hình ảnh '{imageId}' không có RelativePath hoặc ImagePath");
                    return null;
                }

                // Lấy từ storage
                return await GetImageDataByRelativePathAsync(relativePath);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy dữ liệu hình ảnh '{imageId}': {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Lấy thumbnail từ storage
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        /// <returns>Dữ liệu thumbnail (byte array) hoặc null</returns>
        public async Task<byte[]> GetThumbnailDataAsync(Guid imageId)
        {
            try
            {
                var productImage = GetDataAccess().GetById(imageId);
                if (productImage == null)
                {
                    return null;
                }

                // ThumbnailPath không còn tồn tại, generate thumbnail từ original image
                var relativePath = productImage.RelativePath;
                if (!string.IsNullOrEmpty(relativePath))
                {
                    return await GetImageStorage().GetThumbnailAsync(relativePath);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy thumbnail '{imageId}': {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Lấy hình ảnh chính của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <returns>Hình ảnh chính hoặc null</returns>
        public ProductImageDto GetPrimaryByProductId(Guid productId)
        {
            try
            {
                return GetDataAccess().GetPrimaryByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        #region ========== CREATE OPERATIONS ==========

        /// <summary>
        /// Lưu hình ảnh từ file và thông tin metadata
        /// Sử dụng ImageStorageService để lưu trên NAS/Local, chỉ lưu metadata vào database
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh</param>
        /// <param name="isPrimary">Có phải hình ảnh chính không</param>
        /// <param name="caption">Chú thích</param>
        /// <param name="altText">Alt text</param>
        /// <returns>ProductImageDto đã lưu</returns>
        public async Task<ProductImageDto> SaveImageFromFileAsync(Guid productId, string imageFilePath, bool isPrimary = false, string caption = null, string altText = null)
        {
            try
            {
                if (!File.Exists(imageFilePath))
                    throw new BusinessLogicException($"File ảnh không tồn tại: {imageFilePath}");

                // 1. Đọc file ảnh
                var imageData = await Task.Run(() => File.ReadAllBytes(imageFilePath));

                // 2. Tạo tên file mới để tránh trùng lặp
                var fileExtension = Path.GetExtension(imageFilePath);
                var fileName = string.Format(ApplicationConstants.PRODUCT_IMAGE_FILENAME_FORMAT, 
                    productId, 
                    DateTime.Now, 
                    Guid.NewGuid().ToString("N").Substring(0, 8), 
                    fileExtension);

                // 3. Lưu vào storage (NAS/Local) thông qua ImageStorageService
                var storageResult = await GetImageStorage().SaveImageAsync(
                    imageData: imageData,
                    fileName: fileName,
                    category: ImageCategory.Product,
                    entityId: productId,
                    generateThumbnail: true
                );

                if (!storageResult.Success)
                {
                    throw new BusinessLogicException(
                        $"Không thể lưu hình ảnh vào storage: {storageResult.ErrorMessage}");
                }

                // 4. Tạo thumbnail từ ảnh gốc để lưu vào ImageData (tương tự EmployeeBll.UpdateAvatarOnly)
                // Thumbnail được lưu trong database để tăng tốc độ truy vấn và cải thiện UX
                byte[] thumbnailData = null;
                try
                {
                    // Tạo thumbnail với kích thước tối đa 300px và target size ~50KB
                    // Tương tự ProductServiceBll.UpdateThumbnailImageAsync
                    thumbnailData = _imageCompression.CompressImage(
                        imageData: imageData,
                        targetSize: 50000, // Target size: ~50KB
                        maxDimension: 300, // Kích thước tối đa 300px cho thumbnail trong list view
                        format: ImageFormat.Jpeg
                    );
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Không thể tạo thumbnail cho hình ảnh sản phẩm {productId}: {ex.Message}");
                    // Tiếp tục lưu ảnh dù không tạo được thumbnail
                }

                // 5. Tạo ProductImageDto với thumbnail trong ImageData
                var productImageDto = new ProductImageDto
                {
                    Id = Guid.NewGuid(),
                    ProductId = productId,
                    FileName = storageResult.FileName,
                    RelativePath = storageResult.RelativePath,
                    FullPath = storageResult.FullPath,
                    StorageType = "NAS", // Hoặc từ config
                    FileExtension = fileExtension.TrimStart('.').ToLower(),
                    Checksum = storageResult.Checksum,
                    FileSize = storageResult.FileSize,
                    MimeType = GetMimeTypeFromExtension(fileExtension),
                    // Lưu thumbnail vào ImageData để hiển thị ngay trong list view (tương tự EmployeeBll)
                    ImageData = thumbnailData,
                    FileExists = true,
                    CreateDate = DateTime.Now,
                    CreateBy = Guid.Empty, // Set from context if available
                    ModifiedBy = Guid.Empty, // Set from context if available
                    MigrationStatus = "Migrated"
                };

                // 7. Lưu metadata vào database
                GetDataAccess().SaveOrUpdate(productImageDto);

                _logger.Info($"Đã lưu hình ảnh sản phẩm, ProductId={productId}, ImageId={productImageDto.Id}, RelativePath={storageResult.RelativePath}");

                return productImageDto;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lưu hình ảnh từ file '{imageFilePath}': {ex.Message}", ex);
                throw new BusinessLogicException($"Lỗi khi lưu hình ảnh từ file '{imageFilePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hình ảnh từ file (synchronous version - backward compatibility)
        /// </summary>
        public ProductImageDto SaveImageFromFile(Guid productId, string imageFilePath, bool isPrimary = false, string caption = null, string altText = null)
        {
            return SaveImageFromFileAsync(productId, imageFilePath, isPrimary, caption, altText).GetAwaiter().GetResult();
        }

        #region ========== UPDATE OPERATIONS ==========

        /// <summary>
        /// Cập nhật hình ảnh chính cho sản phẩm/dịch vụ
        /// Sử dụng ImageStorageService để lưu trên NAS/Local
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh mới</param>
        /// <returns>ProductImageDto đã cập nhật</returns>
        public async Task<ProductImageDto> UpdatePrimaryImageAsync(Guid productId, string imageFilePath)
        {
            try
            {
                if (!File.Exists(imageFilePath))
                    throw new BusinessLogicException($"File ảnh không tồn tại: {imageFilePath}");

                // 1. Nén ảnh trước khi lưu để đảm bảo kích thước hợp lý
                byte[] imageData;
                using (var compressedImage = CompressImage(imageFilePath, ApplicationConstants.DEFAULT_COMPRESSION_QUALITY, ApplicationConstants.DEFAULT_MAX_DIMENSION))
                {
                    // Convert Image to byte array
                    using (var ms = new MemoryStream())
                    {
                        compressedImage.Save(ms, ImageFormat.Jpeg);
                        imageData = ms.ToArray();
                    }
                }

                // 2. Tạo tên file mới với extension .jpg
                var fileName = string.Format(ApplicationConstants.PRIMARY_IMAGE_FILENAME_FORMAT, 
                    productId, 
                    DateTime.Now, 
                    Guid.NewGuid().ToString("N").Substring(0, 8));

                // 3. Lưu vào storage (NAS/Local)
                var storageResult = await GetImageStorage().SaveImageAsync(
                    imageData: imageData,
                    fileName: fileName,
                    category: ImageCategory.Product,
                    entityId: productId,
                    generateThumbnail: true
                );

                if (!storageResult.Success)
                {
                    throw new BusinessLogicException(
                        $"Không thể lưu hình ảnh chính vào storage: {storageResult.ErrorMessage}");
                }

                // 4. Đọc thông tin ảnh (không cần vì không có ImageWidth/ImageHeight properties)
                // var imageInfo = GetImageInfo(imageFilePath);

                // 5. Kiểm tra xem đã có hình ảnh chính chưa
                var existingPrimary = GetDataAccess().GetPrimaryByProductId(productId);
                
                ProductImageDto productImageDto;
                if (existingPrimary != null)
                {
                    // Xóa file cũ từ storage nếu có
                    if (!string.IsNullOrEmpty(existingPrimary.RelativePath))
                    {
                        try
                        {
                            await GetImageStorage().DeleteImageAsync(existingPrimary.RelativePath);
                        }
                        catch (Exception deleteEx)
                        {
                            _logger.Warning($"Không thể xóa file cũ từ storage: {deleteEx.Message}");
                        }
                    }

                    // Cập nhật hình ảnh chính hiện có
                    productImageDto = existingPrimary;
                    productImageDto.FileName = storageResult.FileName;
                    productImageDto.RelativePath = storageResult.RelativePath;
                    productImageDto.FullPath = storageResult.FullPath;
                    productImageDto.StorageType = "NAS";
                    productImageDto.FileExtension = "jpg";
                    productImageDto.Checksum = storageResult.Checksum;
                    productImageDto.FileSize = storageResult.FileSize;
                    productImageDto.MimeType = "image/jpeg";
                    productImageDto.ImageData = null; // KHÔNG lưu ImageData
                    productImageDto.ModifiedDate = DateTime.Now;
                    productImageDto.FileExists = true;
                }
                else
                {
                    // Tạo mới hình ảnh chính
                    productImageDto = new ProductImageDto
                    {
                        Id = Guid.NewGuid(),
                        ProductId = productId,
                        FileName = storageResult.FileName,
                        RelativePath = storageResult.RelativePath,
                        FullPath = storageResult.FullPath,
                        StorageType = "NAS",
                        FileExtension = "jpg",
                        Checksum = storageResult.Checksum,
                        FileSize = storageResult.FileSize,
                        MimeType = "image/jpeg",
                        ImageData = null, // KHÔNG lưu ImageData
                        FileExists = true,
                        CreateDate = DateTime.Now,
                        CreateBy = Guid.Empty, // Set from context if available
                        ModifiedBy = Guid.Empty, // Set from context if available
                        MigrationStatus = "Migrated"
                    };
                }

                // 6. Lưu metadata vào database
                GetDataAccess().SaveOrUpdate(productImageDto);

                _logger.Info($"Đã cập nhật hình ảnh chính, ProductId={productId}, ImageId={productImageDto.Id}");

                return productImageDto;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi cập nhật hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
                throw new BusinessLogicException($"Lỗi khi cập nhật hình ảnh chính cho sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật hình ảnh chính (synchronous version - backward compatibility)
        /// </summary>
        public ProductImageDto UpdatePrimaryImage(Guid productId, string imageFilePath)
        {
            return UpdatePrimaryImageAsync(productId, imageFilePath).GetAwaiter().GetResult();
        }

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa hình ảnh
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void DeleteImage(Guid imageId)
        {
            try
            {
                GetDataAccess().Delete(imageId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa hình ảnh hoàn chỉnh (database + file từ storage + cập nhật ProductService)
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public async Task DeleteImageCompleteAsync(Guid imageId)
        {
            try
            {
                // 1. Lấy thông tin hình ảnh trước khi xóa
                var imageInfo = GetDataAccess().GetById(imageId);
                if (imageInfo == null)
                {
                    throw new BusinessLogicException($"Không tìm thấy hình ảnh với ID '{imageId}'");
                }

                var productId = imageInfo.ProductId;
                var relativePath = imageInfo.RelativePath;

                // 2. Xóa file từ storage (NAS/Local) nếu có RelativePath
                if (!string.IsNullOrEmpty(relativePath))
                {
                    try
                    {
                        var deleted = await GetImageStorage().DeleteImageAsync(relativePath);
                        if (deleted)
                        {
                            _logger.Info($"Đã xóa file từ storage, RelativePath={relativePath}");
                        }
                        else
                        {
                            _logger.Warning($"Không thể xóa file từ storage, RelativePath={relativePath}");
                        }
                    }
                    catch (Exception storageEx)
                    {
                        _logger.Warning($"Lỗi khi xóa file từ storage '{relativePath}': {storageEx.Message}");
                        // Không throw exception vì có thể file đã bị xóa hoặc không có quyền
                    }
                }

                // 3. ThumbnailPath không còn tồn tại, không cần xóa thumbnail riêng

                // 4. Xóa trong database
                GetDataAccess().Delete(imageId);

                // 5. Cập nhật ProductService nếu cần (ProductImage không còn IsPrimary property)
                if (productId.HasValue)
                {
                    // Kiểm tra xem còn hình ảnh nào khác không, nếu không thì cập nhật ProductService
                    var remainingImages = GetDataAccess().GetByProductId(productId.Value);
                    if (!remainingImages.Any())
                    {
                        UpdateProductServiceAfterPrimaryImageDelete(productId.Value);
                    }
                }

                _logger.Info($"Đã xóa hoàn chỉnh hình ảnh, ImageId={imageId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi xóa hoàn chỉnh hình ảnh '{imageId}': {ex.Message}", ex);
                throw new BusinessLogicException($"Lỗi khi xóa hoàn chỉnh hình ảnh '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa hình ảnh hoàn chỉnh (synchronous version - backward compatibility)
        /// </summary>
        public void DeleteImageComplete(Guid imageId)
        {
            DeleteImageCompleteAsync(imageId).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Cập nhật ProductService sau khi xóa ảnh chính
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        private void UpdateProductServiceAfterPrimaryImageDelete(Guid productId)
        {
            try
            {
                // TODO: Implement logic để cập nhật ProductService
                // Có thể cần:
                // 1. Xóa đường dẫn ảnh chính trong ProductService
                // 2. Đặt ảnh chính mới nếu có ảnh khác
                // 3. Cập nhật trạng thái sản phẩm
                
                System.Diagnostics.Debug.WriteLine($"Cần cập nhật ProductService cho sản phẩm '{productId}' sau khi xóa ảnh chính");
                
                // Tạm thời chỉ log - cần implement logic cụ thể
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi khi cập nhật ProductService: {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa tất cả hình ảnh của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productId">ID sản phẩm/dịch vụ</param>
        public void DeleteAllImages(Guid productId)
        {
            try
            {
                var images = GetDataAccess().GetByProductId(productId);
                foreach (var image in images)
                {
                    GetDataAccess().Delete(image.Id);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa tất cả hình ảnh của sản phẩm '{productId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa hình ảnh (alias cho DeleteImage)
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void Delete(Guid imageId)
        {
            DeleteImage(imageId);
        }

        #endregion

        #region ========== BUSINESS LOGIC METHODS ==========

        /// <summary>
        /// Đặt hình ảnh làm hình ảnh chính
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        public void SetAsPrimary(Guid imageId)
        {
            try
            {
                GetDataAccess().SetAsPrimary(imageId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi đặt hình ảnh làm chính '{imageId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén hình ảnh từ file (sử dụng trong UpdatePrimaryImageAsync)
        /// </summary>
        /// <param name="imageFilePath">Đường dẫn file ảnh gốc</param>
        /// <param name="quality">Chất lượng nén (0-100). Mặc định sử dụng ApplicationConstants.DEFAULT_COMPRESSION_QUALITY</param>
        /// <param name="maxDimension">Kích thước tối đa cho mỗi chiều (pixel). Mặc định sử dụng ApplicationConstants.DEFAULT_MAX_DIMENSION</param>
        /// <returns>Đối tượng Image đã được nén và resize nếu cần</returns>
        private Image CompressImage(string imageFilePath, long quality = -1, int maxDimension = -1)
        {
            try
            {
                if (!File.Exists(imageFilePath))
                    throw new BusinessLogicException($"File ảnh không tồn tại: {imageFilePath}");

                // Sử dụng giá trị mặc định nếu không được chỉ định
                var actualQuality = quality == -1 ? ApplicationConstants.DEFAULT_COMPRESSION_QUALITY : quality;
                var actualMaxDimension = maxDimension == -1 ? ApplicationConstants.DEFAULT_MAX_DIMENSION : maxDimension;

                using var originalImage = Image.FromFile(imageFilePath);
                
                return CompressImage(originalImage, actualQuality, actualMaxDimension);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén hình ảnh từ file '{imageFilePath}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén hình ảnh từ Image object (sử dụng trong CompressImage từ file)
        /// </summary>
        /// <param name="originalImage">Đối tượng Image gốc</param>
        /// <param name="quality">Chất lượng nén (0-100). Mặc định sử dụng ApplicationConstants.DEFAULT_COMPRESSION_QUALITY</param>
        /// <param name="maxDimension">Kích thước tối đa cho mỗi chiều (pixel). Mặc định sử dụng ApplicationConstants.DEFAULT_MAX_DIMENSION</param>
        /// <returns>Đối tượng Image đã được nén và resize nếu cần</returns>
        private Image CompressImage(Image originalImage, long quality = -1, int maxDimension = -1)
        {
            try
            {
                if (originalImage == null)
                    throw new ArgumentNullException(nameof(originalImage));

                // Sử dụng giá trị mặc định nếu không được chỉ định
                var actualQuality = quality == -1 ? ApplicationConstants.DEFAULT_COMPRESSION_QUALITY : quality;
                var actualMaxDimension = maxDimension == -1 ? ApplicationConstants.DEFAULT_MAX_DIMENSION : maxDimension;

                // Tính toán kích thước mới nếu ảnh quá lớn
                var newSize = CalculateNewSize(originalImage.Width, originalImage.Height, actualMaxDimension);
                
                // Tạo ảnh mới với kích thước đã tính toán
                using (var resizedImage = new Bitmap(newSize.Width, newSize.Height))
                {
                    using (var graphics = Graphics.FromImage(resizedImage))
                    {
                        // Thiết lập chất lượng vẽ
                        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        graphics.SmoothingMode = SmoothingMode.HighQuality;
                        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                        graphics.CompositingQuality = CompositingQuality.HighQuality;

                        // Vẽ ảnh gốc lên ảnh mới
                        graphics.DrawImage(originalImage, 0, 0, newSize.Width, newSize.Height);
                    }

                    // Nén ảnh với chất lượng JPEG và trả về ảnh mới
                    return CompressToJpeg(resizedImage, actualQuality);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén hình ảnh: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Nén ảnh thành JPEG với chất lượng cụ thể
        /// </summary>
        /// <param name="image">Ảnh cần nén</param>
        /// <param name="quality">Chất lượng (0-100)</param>
        /// <returns>Ảnh đã nén</returns>
        private Image CompressToJpeg(Image image, long quality)
        {
            try
            {
                // Lấy JPEG codec
                var jpegCodec = GetEncoder(ImageFormat.Jpeg);
                if (jpegCodec == null)
                {
                    // Nếu không có JPEG codec, trả về ảnh gốc
                    return new Bitmap(image);
                }

                // Thiết lập tham số nén
                var encoderParams = new EncoderParameters(1);
                encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);

                // Tạo bản sao của ảnh để tránh lỗi GDI+
                using (var imageCopy = new Bitmap(image))
                {
                    // Nén ảnh
                    using var ms = new MemoryStream();
                    
                    imageCopy.Save(ms, jpegCodec, encoderParams);
                    ms.Position = 0;
                    
                    // Tạo ảnh mới từ MemoryStream
                    return new Bitmap(ms);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi nén JPEG: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tính toán kích thước mới để giữ tỷ lệ và không vượt quá kích thước tối đa
        /// </summary>
        /// <param name="originalWidth">Chiều rộng gốc</param>
        /// <param name="originalHeight">Chiều cao gốc</param>
        /// <param name="maxDimension">Kích thước tối đa cho mỗi chiều</param>
        /// <returns>Kích thước mới</returns>
        private Size CalculateNewSize(int originalWidth, int originalHeight, int maxDimension)
        {
            // Nếu ảnh đã nhỏ hơn kích thước tối đa, giữ nguyên
            if (originalWidth <= maxDimension && originalHeight <= maxDimension)
            {
                return new Size(originalWidth, originalHeight);
            }

            // Tính tỷ lệ để giảm kích thước
            double ratio = Math.Min((double)maxDimension / originalWidth, (double)maxDimension / originalHeight);
            
            return new Size(
                (int)(originalWidth * ratio),
                (int)(originalHeight * ratio)
            );
        }

        /// <summary>
        /// Lấy ImageCodecInfo cho một định dạng ảnh cụ thể
        /// </summary>
        /// <param name="format">Định dạng ảnh</param>
        /// <returns>ImageCodecInfo hoặc null</returns>
        private ImageCodecInfo GetEncoder(ImageFormat format)
        {
            try
            {
                var codecs = ImageCodecInfo.GetImageEncoders();
                foreach (var codec in codecs)
                {
                    if (codec.FormatID == format.Guid)
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
        /// Lấy MimeType từ file extension
        /// </summary>
        /// <param name="extension">File extension (ví dụ: .jpg, .png)</param>
        /// <returns>MimeType string</returns>
        private string GetMimeTypeFromExtension(string extension)
        {
            var ext = extension.TrimStart('.').ToLower();
            return ext switch
            {
                "jpg" or "jpeg" => "image/jpeg",
                "png" => "image/png",
                "gif" => "image/gif",
                "bmp" => "image/bmp",
                "webp" => "image/webp",
                _ => "image/jpeg" // Default
            };
        }

        /// <summary>
        /// Tìm kiếm hình ảnh theo danh sách ProductId
        /// </summary>
        /// <param name="productIds">Danh sách ID sản phẩm/dịch vụ</param>
        /// <returns>Danh sách hình ảnh phù hợp</returns>
        public List<ProductImageDto> SearchByProductIds(List<Guid> productIds)
        {
            try
            {
                if (productIds == null || !productIds.Any())
                {
                    return new List<ProductImageDto>();
                }

                return GetDataAccess().SearchByProductIds(productIds);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm hình ảnh theo danh sách ProductId (Async)
        /// </summary>
        /// <param name="productIds">Danh sách ID sản phẩm/dịch vụ</param>
        /// <returns>Danh sách hình ảnh phù hợp</returns>
        public async Task<List<ProductImageDto>> SearchByProductIdsAsync(List<Guid> productIds)
        {
            try
            {
                if (productIds == null || !productIds.Any())
                {
                    return new List<ProductImageDto>();
                }

                return await Task.Run(() => GetDataAccess().SearchByProductIds(productIds));
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm hình ảnh theo sản phẩm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm hình ảnh theo từ khóa (tìm kiếm trong ProductService và Category trước)
        /// </summary>
        /// <param name="searchKeyword">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách hình ảnh phù hợp</returns>
        public async Task<List<ProductImageDto>> SearchAsync(string searchKeyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchKeyword))
                {
                    return new List<ProductImageDto>();
                }

                // Tìm kiếm sản phẩm/dịch vụ trước
                var productServiceBll = new ProductServiceBll();
                var searchResults = await productServiceBll.SearchAsync(searchKeyword.Trim());
                
                if (!searchResults.Any())
                {
                    return new List<ProductImageDto>();
                }

                // Lấy danh sách ProductId từ kết quả tìm kiếm
                var productIds = searchResults.Select(x => x.Id).ToList();
                
                // Tìm kiếm hình ảnh theo danh sách ProductId
                return await SearchByProductIdsAsync(productIds);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm hình ảnh: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== HELPER METHODS (Image Storage & Processing) ==========

        /// <summary>
        /// Kiểm tra file hình ảnh có tồn tại trên storage (NAS/Local) không
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>True nếu file tồn tại</returns>
        public async Task<bool> CheckImageFileExistsAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(relativePath))
                {
                    return false;
                }

                return await GetImageStorage().ImageExistsAsync(relativePath);
            }
            catch (Exception ex)
            {
                _logger.Warning($"Lỗi khi kiểm tra file tồn tại '{relativePath}': {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Lấy dữ liệu hình ảnh từ storage (NAS/Local) theo RelativePath
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối</param>
        /// <returns>Dữ liệu hình ảnh (byte array) hoặc null</returns>
        public async Task<byte[]> GetImageDataByRelativePathAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(relativePath))
                {
                    _logger.Warning("RelativePath không được để trống");
                    return null;
                }

                // Lấy từ storage
                var imageData = await GetImageStorage().GetImageAsync(relativePath);
                
                if (imageData == null)
                {
                    _logger.Warning($"Không thể đọc file từ storage, RelativePath={relativePath}");
                }

                return imageData;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy dữ liệu hình ảnh từ RelativePath '{relativePath}': {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Lấy dữ liệu hình ảnh từ storage (NAS/Local)
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        /// <returns>Dữ liệu hình ảnh (byte array) hoặc null</returns>
        public async Task<byte[]> GetImageDataAsync(Guid imageId)
        {
            try
            {
                var productImage = GetDataAccess().GetById(imageId);
                if (productImage == null)
                {
                    _logger.Warning($"Không tìm thấy hình ảnh với ID '{imageId}'");
                    return null;
                }

                // Sử dụng RelativePath
                var relativePath = productImage.RelativePath;
                
                if (string.IsNullOrEmpty(relativePath))
                {
                    _logger.Warning($"Hình ảnh '{imageId}' không có RelativePath hoặc ImagePath");
                    return null;
                }

                // Lấy từ storage
                return await GetImageDataByRelativePathAsync(relativePath);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy dữ liệu hình ảnh '{imageId}': {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Lấy thumbnail từ storage
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        /// <returns>Dữ liệu thumbnail (byte array) hoặc null</returns>
        public async Task<byte[]> GetThumbnailDataAsync(Guid imageId)
        {
            try
            {
                var productImage = GetDataAccess().GetById(imageId);
                if (productImage == null)
                {
                    return null;
                }

                // ThumbnailPath không còn tồn tại, generate thumbnail từ original image
                var relativePath = productImage.RelativePath;
                if (!string.IsNullOrEmpty(relativePath))
                {
                    return await GetImageStorage().GetThumbnailAsync(relativePath);
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy thumbnail '{imageId}': {ex.Message}", ex);
                return null;
            }
        }

        #endregion

        #region ========== PRIVATE HELPER METHODS ==========
