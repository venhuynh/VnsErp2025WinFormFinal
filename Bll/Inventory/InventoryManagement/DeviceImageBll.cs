using Common;
using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
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

namespace Bll.Inventory.InventoryManagement
{
    /// <summary>
    /// Business Logic Layer cho DeviceImage
    /// Sử dụng ImageStorageService để lưu trữ hình ảnh trên NAS/Local thay vì database
    /// </summary>
    public class DeviceImageBll
    {
        #region Fields

        private IDeviceImageRepository _dataAccess;
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
        public DeviceImageBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
            _imageCompression = new ImageCompressionService();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IDeviceImageRepository GetDataAccess()
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

                            _dataAccess = new DeviceImageRepository(globalConnectionString);
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

        #region Public Methods

        /// <summary>
        /// Lấy tất cả hình ảnh
        /// </summary>
        /// <returns>Danh sách tất cả hình ảnh</returns>
        public List<DeviceImage> GetAll()
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
        /// Lấy danh sách hình ảnh của thiết bị
        /// </summary>
        /// <param name="deviceId">ID thiết bị</param>
        /// <returns>Danh sách hình ảnh</returns>
        public List<DeviceImage> GetByDeviceId(Guid deviceId)
        {
            try
            {
                return GetDataAccess().GetByDeviceId(deviceId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy danh sách hình ảnh cho thiết bị '{deviceId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy hình ảnh theo ID
        /// </summary>
        /// <param name="imageId">ID hình ảnh</param>
        /// <returns>Hình ảnh hoặc null</returns>
        public DeviceImage GetById(Guid imageId)
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
                var deviceImage = GetDataAccess().GetById(imageId);
                if (deviceImage == null)
                {
                    _logger.Warning($"Không tìm thấy hình ảnh với ID '{imageId}'");
                    return null;
                }

                // Sử dụng RelativePath
                var relativePath = deviceImage.RelativePath;
                
                if (string.IsNullOrEmpty(relativePath))
                {
                    _logger.Warning($"Hình ảnh '{imageId}' không có RelativePath");
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
                var deviceImage = GetDataAccess().GetById(imageId);
                if (deviceImage == null)
                {
                    return null;
                }

                // ThumbnailPath không còn tồn tại, generate thumbnail từ original image
                var relativePath = deviceImage.RelativePath;
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
        /// Lấy hình ảnh chính của thiết bị
        /// </summary>
        /// <param name="deviceId">ID thiết bị</param>
        /// <returns>Hình ảnh chính hoặc null</returns>
        public DeviceImage GetPrimaryByDeviceId(Guid deviceId)
        {
            try
            {
                return GetDataAccess().GetPrimaryByDeviceId(deviceId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy hình ảnh chính cho thiết bị '{deviceId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hình ảnh từ file và thông tin metadata
        /// Sử dụng ImageStorageService để lưu trên NAS/Local, chỉ lưu metadata vào database
        /// </summary>
        /// <param name="deviceId">ID thiết bị</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh</param>
        /// <param name="isPrimary">Có phải hình ảnh chính không</param>
        /// <param name="caption">Chú thích</param>
        /// <param name="altText">Alt text</param>
        /// <param name="displayOrder">Thứ tự hiển thị</param>
        /// <returns>DeviceImage đã lưu</returns>
        public async Task<DeviceImage> SaveImageFromFileAsync(Guid deviceId, string imageFilePath, bool isPrimary = false, string caption = null, string altText = null, int? displayOrder = null)
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
                    deviceId, 
                    DateTime.Now, 
                    Guid.NewGuid().ToString("N").Substring(0, 8), 
                    fileExtension);

                // 3. Lưu vào storage (NAS/Local) thông qua ImageStorageService
                // Sử dụng ImageCategory.StockInOut vì Device thuộc quản lý kho
                var storageResult = await GetImageStorage().SaveImageAsync(
                    imageData: imageData,
                    fileName: fileName,
                    category: ImageCategory.StockInOut,
                    entityId: deviceId,
                    generateThumbnail: true
                );

                if (!storageResult.Success)
                {
                    throw new BusinessLogicException(
                        $"Không thể lưu hình ảnh vào storage: {storageResult.ErrorMessage}");
                }

                // 4. Tạo thumbnail từ ảnh gốc để lưu vào ImageData (tương tự ProductImageBll)
                // Thumbnail được lưu trong database để tăng tốc độ truy vấn và cải thiện UX
                // Tương tự ProductImageBll.SaveImageFromFileAsync và EmployeeBll.UpdateAvatarOnly
                byte[] thumbnailData = null;
                try
                {
                    // Tạo thumbnail với kích thước tối đa 300px và target size ~50KB
                    // Tương tự ProductImageBll và ProductServiceBll.UpdateThumbnailImageAsync
                    thumbnailData = _imageCompression.CompressImage(
                        imageData: imageData,
                        targetSize: 50000, // Target size: ~50KB
                        maxDimension: 300, // Kích thước tối đa 300px cho thumbnail trong list view
                        format: ImageFormat.Jpeg
                    );
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Không thể tạo thumbnail cho hình ảnh thiết bị {deviceId}: {ex.Message}");
                    // Tiếp tục lưu ảnh dù không tạo được thumbnail
                }

                // 5. Lấy thông tin user hiện tại (DeviceImage có thêm logic này so với ProductImage)
                var currentUser = Bll.Common.ApplicationSystemUtils.GetCurrentUser();
                var createBy = currentUser?.Id ?? Guid.Empty;

                // 6. Nếu đặt làm primary, bỏ IsPrimary của các hình ảnh khác (DeviceImage có logic này)
                if (isPrimary)
                {
                    var existingImages = GetDataAccess().GetByDeviceId(deviceId);
                    foreach (var existingImage in existingImages)
                    {
                        if (existingImage.IsPrimary == true)
                        {
                            existingImage.IsPrimary = false;
                            existingImage.ModifiedDate = DateTime.Now;
                            GetDataAccess().SaveOrUpdate(existingImage);
                        }
                    }
                }

                // 7. Tạo DeviceImage entity với thumbnail trong ImageData
                // Tương tự ProductImageBll: lưu thumbnail vào ImageData để hiển thị ngay trong list view
                // Convert byte[] sang Binary như ProductImageBll và EmployeeBll.UpdateAvatarOnly
                var deviceImage = new DeviceImage
                {
                    Id = Guid.NewGuid(),
                    DeviceId = deviceId,
                    FileName = storageResult.FileName,
                    RelativePath = storageResult.RelativePath,
                    FullPath = storageResult.FullPath,
                    StorageType = "NAS", // Hoặc từ config
                    FileExtension = fileExtension.TrimStart('.').ToLower(),
                    Checksum = storageResult.Checksum,
                    FileSize = storageResult.FileSize,
                    MimeType = GetMimeTypeFromExtension(fileExtension),
                    // Lưu thumbnail vào ImageData để hiển thị ngay trong list view (tương tự ProductImageBll)
                    // Convert byte[] sang Binary như ProductImageBll và EmployeeBll.UpdateAvatarOnly
                    ImageData = thumbnailData != null ? new System.Data.Linq.Binary(thumbnailData) : null,
                    FileExists = true,
                    CreateDate = DateTime.Now,
                    CreateBy = createBy,
                    ModifiedBy = Guid.Empty, // Set from context if available
                    IsPrimary = isPrimary,
                    Caption = caption,
                    AltText = altText,
                    DisplayOrder = displayOrder,
                    IsActive = true,
                    MigrationStatus = "Migrated"
                };

                // 8. Lưu metadata vào database (tương tự ProductImageBll)
                GetDataAccess().SaveOrUpdate(deviceImage);

                _logger.Info($"Đã lưu hình ảnh thiết bị, DeviceId={deviceId}, ImageId={deviceImage.Id}, RelativePath={storageResult.RelativePath}");

                return deviceImage;
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
        public DeviceImage SaveImageFromFile(Guid deviceId, string imageFilePath, bool isPrimary = false, string caption = null, string altText = null, int? displayOrder = null)
        {
            return SaveImageFromFileAsync(deviceId, imageFilePath, isPrimary, caption, altText, displayOrder).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Cập nhật hình ảnh chính cho thiết bị
        /// Sử dụng ImageStorageService để lưu trên NAS/Local
        /// </summary>
        /// <param name="deviceId">ID thiết bị</param>
        /// <param name="imageFilePath">Đường dẫn file ảnh mới</param>
        /// <returns>DeviceImage đã cập nhật</returns>
        public async Task<DeviceImage> UpdatePrimaryImageAsync(Guid deviceId, string imageFilePath)
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
                    deviceId, 
                    DateTime.Now, 
                    Guid.NewGuid().ToString("N").Substring(0, 8));

                // 3. Lưu vào storage (NAS/Local)
                var storageResult = await GetImageStorage().SaveImageAsync(
                    imageData: imageData,
                    fileName: fileName,
                    category: ImageCategory.StockInOut,
                    entityId: deviceId,
                    generateThumbnail: true
                );

                if (!storageResult.Success)
                {
                    throw new BusinessLogicException(
                        $"Không thể lưu hình ảnh chính vào storage: {storageResult.ErrorMessage}");
                }

                // 4. Lấy thông tin user hiện tại
                var currentUser = Bll.Common.ApplicationSystemUtils.GetCurrentUser();
                var modifiedBy = currentUser?.Id ?? Guid.Empty;

                // 5. Kiểm tra xem đã có hình ảnh chính chưa
                var existingPrimary = GetDataAccess().GetPrimaryByDeviceId(deviceId);
                
                DeviceImage deviceImage;
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
                    deviceImage = existingPrimary;
                    deviceImage.FileName = storageResult.FileName;
                    deviceImage.RelativePath = storageResult.RelativePath;
                    deviceImage.FullPath = storageResult.FullPath;
                    deviceImage.StorageType = "NAS";
                    deviceImage.FileExtension = "jpg";
                    deviceImage.Checksum = storageResult.Checksum;
                    deviceImage.FileSize = storageResult.FileSize;
                    deviceImage.MimeType = "image/jpeg";
                    deviceImage.ImageData = null; // KHÔNG lưu ImageData
                    deviceImage.ModifiedDate = DateTime.Now;
                    deviceImage.ModifiedBy = modifiedBy;
                    deviceImage.FileExists = true;
                }
                else
                {
                    // Tạo mới hình ảnh chính
                    deviceImage = new DeviceImage
                    {
                        Id = Guid.NewGuid(),
                        DeviceId = deviceId,
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
                        CreateBy = modifiedBy,
                        ModifiedBy = modifiedBy,
                        IsPrimary = true,
                        IsActive = true,
                        MigrationStatus = "Migrated"
                    };
                }

                // 6. Lưu metadata vào database
                GetDataAccess().SaveOrUpdate(deviceImage);

                _logger.Info($"Đã cập nhật hình ảnh chính, DeviceId={deviceId}, ImageId={deviceImage.Id}");

                return deviceImage;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi cập nhật hình ảnh chính cho thiết bị '{deviceId}': {ex.Message}", ex);
                throw new BusinessLogicException($"Lỗi khi cập nhật hình ảnh chính cho thiết bị '{deviceId}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật hình ảnh chính (synchronous version - backward compatibility)
        /// </summary>
        public DeviceImage UpdatePrimaryImage(Guid deviceId, string imageFilePath)
        {
            return UpdatePrimaryImageAsync(deviceId, imageFilePath).GetAwaiter().GetResult();
        }

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
        /// Xóa hình ảnh hoàn chỉnh (database + file từ storage)
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

                var deviceId = imageInfo.DeviceId;
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

                // 3. Xóa trong database
                GetDataAccess().DeletePermanent(imageId);

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
        /// Xóa tất cả hình ảnh của thiết bị
        /// </summary>
        /// <param name="deviceId">ID thiết bị</param>
        public void DeleteAllImages(Guid deviceId)
        {
            try
            {
                var images = GetDataAccess().GetByDeviceId(deviceId);
                foreach (var image in images)
                {
                    GetDataAccess().Delete(image.Id);
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa tất cả hình ảnh của thiết bị '{deviceId}': {ex.Message}", ex);
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
        /// Tìm kiếm hình ảnh theo danh sách DeviceId
        /// </summary>
        /// <param name="deviceIds">Danh sách ID thiết bị</param>
        /// <returns>Danh sách hình ảnh phù hợp</returns>
        public List<DeviceImage> SearchByDeviceIds(List<Guid> deviceIds)
        {
            try
            {
                if (deviceIds == null || !deviceIds.Any())
                {
                    return new List<DeviceImage>();
                }

                return GetDataAccess().SearchByDeviceIds(deviceIds);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm hình ảnh theo thiết bị: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm hình ảnh theo danh sách DeviceId (Async)
        /// </summary>
        /// <param name="deviceIds">Danh sách ID thiết bị</param>
        /// <returns>Danh sách hình ảnh phù hợp</returns>
        public async Task<List<DeviceImage>> SearchByDeviceIdsAsync(List<Guid> deviceIds)
        {
            try
            {
                if (deviceIds == null || !deviceIds.Any())
                {
                    return new List<DeviceImage>();
                }

                return await Task.Run(() => GetDataAccess().SearchByDeviceIds(deviceIds));
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi tìm kiếm hình ảnh theo thiết bị: {ex.Message}", ex);
            }
        }

        #endregion

        #region Private Methods

        /// <summary>
        /// Nén hình ảnh mà không thay đổi kích thước (dimensions)
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
        /// Nén hình ảnh mà không thay đổi kích thước (dimensions)
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

        #endregion
    }
}

