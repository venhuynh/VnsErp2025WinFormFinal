using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.IO;
using System.Threading.Tasks;
using Bll.Common.ImageStorage;

namespace Bll.Inventory.InventoryManagement
{
    /// <summary>
    /// Business Logic Layer cho StockInOutImage
    /// Sử dụng ImageStorageService để lưu trữ hình ảnh trên NAS/Local thay vì database
    /// </summary>
    public class StockInOutImageBll
{
    #region Fields

    private IStockInOutImageRepository _dataAccess;
    private IImageStorageService _imageStorage;
    private readonly object _imageStorageLock = new object();
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public StockInOutImageBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IStockInOutImageRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    try
                    {
                        // Sử dụng global connection string từ ApplicationStartupManager
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataAccess = new StockInOutImageRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInOutImageRepository: {ex.Message}", ex);
                        throw;
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

    #region Business Methods

    /// <summary>
    /// Lưu hình ảnh từ file vào storage (NAS/Local) và metadata vào database
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <param name="imageFilePath">Đường dẫn file ảnh</param>
    /// <returns>StockInOutImage đã lưu</returns>
    public async Task<Dal.DataContext.StockInOutImage> SaveImageFromFileAsync(Guid stockInOutMasterId, string imageFilePath)
    {
        try
        {
            if (!File.Exists(imageFilePath))
            {
                throw new FileNotFoundException($"File ảnh không tồn tại: {imageFilePath}");
            }

            // 1. Đọc file ảnh
            var imageData = await Task.Run(() => File.ReadAllBytes(imageFilePath));

            // 2. Tạo tên file mới để tránh trùng lặp
            var fileExtension = Path.GetExtension(imageFilePath);
            var fileName = $"StockInOut_{stockInOutMasterId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}.{fileExtension.TrimStart('.')}";

            // 3. Lưu vào storage (NAS/Local) thông qua ImageStorageService
            var storageResult = await GetImageStorage().SaveImageAsync(
                imageData: imageData,
                fileName: fileName,
                category: ImageCategory.StockInOut,
                entityId: stockInOutMasterId,
                generateThumbnail: false // StockInOut images thường không cần thumbnail
            );

            if (!storageResult.Success)
            {
                throw new InvalidOperationException(
                    $"Không thể lưu hình ảnh vào storage: {storageResult.ErrorMessage}");
            }

            // 4. Lấy thông tin user hiện tại
            var currentUser = Bll.Common.ApplicationSystemUtils.GetCurrentUser();
            var createBy = currentUser?.Id ?? Guid.Empty;

            // 5. Tạo StockInOutImage entity (KHÔNG lưu ImageData vào database)
            var stockInOutImage = new Dal.DataContext.StockInOutImage
            {
                Id = Guid.NewGuid(),
                StockInOutMasterId = stockInOutMasterId,
                ImageData = null, // KHÔNG lưu ImageData vào database nữa
                FileName = storageResult.FileName,
                RelativePath = storageResult.RelativePath,
                FullPath = storageResult.FullPath,
                // Lấy storage type từ config thay vì hardcode "NAS"
                StorageType = GetStorageTypeFromConfig(),
                FileSize = storageResult.FileSize,
                FileExtension = fileExtension.TrimStart('.').ToLower(),
                MimeType = GetMimeType(fileExtension),
                Checksum = storageResult.Checksum,
                FileExists = true,
                CreateDate = DateTime.Now,
                CreateBy = createBy,
                ModifiedDate = null,
                ModifiedBy = Guid.Empty
            };

            // 6. Lưu metadata vào database
            GetDataAccess().SaveOrUpdate(stockInOutImage);

            _logger.Info($"Đã lưu hình ảnh phiếu nhập/xuất, StockInOutMasterId={stockInOutMasterId}, ImageId={stockInOutImage.Id}, RelativePath={storageResult.RelativePath}");

            return stockInOutImage;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu hình ảnh từ file '{imageFilePath}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lưu hình ảnh từ file (synchronous version - backward compatibility)
    /// </summary>
    public Dal.DataContext.StockInOutImage SaveImageFromFile(Guid stockInOutMasterId, string imageFilePath)
    {
        return SaveImageFromFileAsync(stockInOutMasterId, imageFilePath).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Lưu hình ảnh từ byte array vào storage (NAS/Local) và metadata vào database
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <param name="imageData">Dữ liệu hình ảnh (byte array)</param>
    /// <param name="fileExtension">Phần mở rộng file (ví dụ: "jpg", "png")</param>
    /// <returns>StockInOutImage đã lưu</returns>
    public async Task<Dal.DataContext.StockInOutImage> SaveImageFromBytesAsync(Guid stockInOutMasterId, byte[] imageData, string fileExtension = "jpg")
    {
        try
        {
            if (imageData == null || imageData.Length == 0)
            {
                throw new ArgumentException("Dữ liệu hình ảnh không được để trống", nameof(imageData));
            }

            // 1. Tạo tên file mới để tránh trùng lặp
            var fileName = $"StockInOut_{stockInOutMasterId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}.{fileExtension.TrimStart('.')}";

            // 2. Lưu vào storage (NAS/Local) thông qua ImageStorageService
            var storageResult = await GetImageStorage().SaveImageAsync(
                imageData: imageData,
                fileName: fileName,
                category: ImageCategory.StockInOut,
                entityId: stockInOutMasterId,
                generateThumbnail: false // StockInOut images thường không cần thumbnail
            );

            if (!storageResult.Success)
            {
                throw new InvalidOperationException(
                    $"Không thể lưu hình ảnh vào storage: {storageResult.ErrorMessage}");
            }

            // 3. Lấy thông tin user hiện tại
            var currentUser = Bll.Common.ApplicationSystemUtils.GetCurrentUser();
            var createBy = currentUser?.Id ?? Guid.Empty;

            // 4. Tạo StockInOutImage entity (KHÔNG lưu ImageData vào database)
            var stockInOutImage = new Dal.DataContext.StockInOutImage
            {
                Id = Guid.NewGuid(),
                StockInOutMasterId = stockInOutMasterId,
                ImageData = null, // KHÔNG lưu ImageData vào database nữa
                FileName = storageResult.FileName,
                RelativePath = storageResult.RelativePath,
                FullPath = storageResult.FullPath,
                // Lấy storage type từ config thay vì hardcode "NAS"
                StorageType = GetStorageTypeFromConfig(),
                FileSize = storageResult.FileSize,
                FileExtension = fileExtension.TrimStart('.').ToLower(),
                MimeType = GetMimeType($".{fileExtension.TrimStart('.')}"),
                Checksum = storageResult.Checksum,
                FileExists = true,
                CreateDate = DateTime.Now,
                CreateBy = createBy,
                ModifiedDate = null,
                ModifiedBy = Guid.Empty
            };

            // 5. Lưu metadata vào database
            GetDataAccess().SaveOrUpdate(stockInOutImage);

            _logger.Info($"Đã lưu hình ảnh phiếu nhập/xuất từ byte array, StockInOutMasterId={stockInOutMasterId}, ImageId={stockInOutImage.Id}, RelativePath={storageResult.RelativePath}");

            return stockInOutImage;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu hình ảnh từ byte array: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy dữ liệu hình ảnh từ storage (NAS/Local) bằng RelativePath
    /// Tối ưu hơn GetImageDataAsync vì không cần query database lại
    /// </summary>
    /// <param name="relativePath">Đường dẫn tương đối của hình ảnh trong storage</param>
    /// <returns>Dữ liệu hình ảnh (byte array) hoặc null</returns>
    public async Task<byte[]> GetImageDataByRelativePathAsync(string relativePath)
    {
        try
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                _logger.Warning("RelativePath is null or empty");
                return null;
            }

            // Lấy trực tiếp từ storage (NAS/Local) sử dụng RelativePath
            // Logic tương tự SaveImageFromFileAsync: sử dụng ImageStorageService để đọc file
            var imageData = await GetImageStorage().GetImageAsync(relativePath);
            
            if (imageData == null)
            {
                _logger.Warning($"Không thể đọc file từ storage, RelativePath={relativePath}");
            }
            else
            {
                _logger.Debug($"Đã load hình ảnh từ storage, RelativePath={relativePath}, Size={imageData.Length} bytes");
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
    /// Lấy dữ liệu hình ảnh từ storage (NAS/Local) bằng ImageId
    /// Query database để lấy RelativePath, sau đó load từ storage
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    /// <returns>Dữ liệu hình ảnh (byte array) hoặc null</returns>
    public async Task<byte[]> GetImageDataAsync(Guid imageId)
    {
        try
        {
            // 1. Query database để lấy metadata (RelativePath)
            // Logic tương tự SaveImageFromFileAsync: metadata được lưu trong database
            var stockInOutImage = GetDataAccess().GetById(imageId);
            if (stockInOutImage == null)
            {
                _logger.Warning($"Không tìm thấy hình ảnh với ID '{imageId}'");
                return null;
            }

            // 2. Lấy RelativePath từ metadata (đã được lưu khi SaveImageFromFileAsync)
            var relativePath = stockInOutImage.RelativePath;
            
            if (string.IsNullOrEmpty(relativePath))
            {
                _logger.Warning($"Hình ảnh '{imageId}' không có RelativePath");
                return null;
            }

            // 3. Load từ storage sử dụng RelativePath
            // Logic tương tự SaveImageFromFileAsync: sử dụng ImageStorageService để đọc file
            var imageData = await GetImageDataByRelativePathAsync(relativePath);
            
            return imageData;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy dữ liệu hình ảnh '{imageId}': {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Xóa hình ảnh (database + file từ storage)
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    public async Task DeleteImageAsync(Guid imageId)
    {
        try
        {
            // 1. Lấy thông tin hình ảnh trước khi xóa
            var stockInOutImage = GetDataAccess().GetById(imageId);
            if (stockInOutImage == null)
            {
                _logger.Warning($"Không tìm thấy hình ảnh với ID '{imageId}'");
                return;
            }

            var relativePath = stockInOutImage.RelativePath;

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
            GetDataAccess().Delete(imageId);

            _logger.Info($"Đã xóa hình ảnh, ImageId={imageId}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa hình ảnh '{imageId}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Xóa hình ảnh (synchronous version - backward compatibility)
    /// </summary>
    public void DeleteImage(Guid imageId)
    {
        DeleteImageAsync(imageId).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Lấy hình ảnh theo ID
    /// </summary>
    /// <param name="imageId">ID hình ảnh</param>
    /// <returns>StockInOutImage hoặc null</returns>
    public Dal.DataContext.StockInOutImage GetById(Guid imageId)
    {
        try
        {
            return GetDataAccess().GetById(imageId);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy hình ảnh '{imageId}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách hình ảnh theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách hình ảnh</returns>
    public System.Collections.Generic.List<Dal.DataContext.StockInOutImage> GetByStockInOutMasterId(Guid stockInOutMasterId)
    {
        try
        {
            return GetDataAccess().GetByStockInOutMasterId(stockInOutMasterId);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách hình ảnh cho StockInOutMasterId '{stockInOutMasterId}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Query hình ảnh theo khoảng thời gian và từ khóa
    /// </summary>
    /// <param name="fromDate">Từ ngày</param>
    /// <param name="toDate">Đến ngày</param>
    /// <param name="keyword">Từ khóa tìm kiếm (tên file, đường dẫn)</param>
    /// <returns>Danh sách hình ảnh</returns>
    public System.Collections.Generic.List<Dal.DataContext.StockInOutImage> QueryImages(DateTime fromDate, DateTime toDate, string keyword = null)
    {
        try
        {
            _logger.Debug("QueryImages: Bắt đầu query hình ảnh, FromDate={0}, ToDate={1}, Keyword={2}", 
                fromDate, toDate, keyword ?? "null");

            var result = GetDataAccess().QueryImages(fromDate, toDate, keyword);

            _logger.Info("QueryImages: Query thành công, ResultCount={0}", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi query hình ảnh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Private Helper Methods

    /// <summary>
    /// Lấy MIME type từ file extension
    /// </summary>
    private string GetMimeType(string fileExtension)
    {
        if (string.IsNullOrEmpty(fileExtension))
            return "image/jpeg";

        var ext = fileExtension.TrimStart('.').ToLower();
        return ext switch
        {
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",
            "bmp" => "image/bmp",
            "webp" => "image/webp",
            _ => "image/jpeg"
        };
    }

    /// <summary>
    /// Lấy storage type từ config
    /// </summary>
    private string GetStorageTypeFromConfig()
    {
        try
        {
            var config = ImageStorageConfiguration.LoadFromConfig();
            return config?.StorageType ?? "NAS";
        }
        catch
        {
            return "NAS"; // Fallback to NAS if config cannot be loaded
        }
    }

    #endregion
}
}