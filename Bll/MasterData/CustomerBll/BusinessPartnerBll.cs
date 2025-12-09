using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.PartnerRepository;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bll.Common.ImageStorage;
using Bll.Common.ImageService;
using Bll.Common;
using Common;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.MasterData.CustomerBll
{
    /// <summary>
    /// Business logic layer cho BusinessPartner. Trả về entity để UI tự chuyển đổi sang DTO.
    /// Tránh phụ thuộc ngược sang project MasterData (DTO) để không tạo vòng tham chiếu.
    /// </summary>
    public class BusinessPartnerBll
    {
        #region Fields

        private IBusinessPartnerRepository _dataAccess;
        private readonly object _lockObject = new object();
        private readonly IImageStorageService _imageStorage;
        private readonly ImageCompressionService _imageCompression;
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public BusinessPartnerBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
            _imageStorage = ImageStorageFactory.CreateFromConfig(_logger);
            _imageCompression = new ImageCompressionService();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IBusinessPartnerRepository GetDataAccess()
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

                            _dataAccess = new BusinessPartnerRepository(globalConnectionString);
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

        #endregion


        /// <summary>
        /// Lấy toàn bộ đối tác (entities) - Async.
        /// </summary>
        public async Task<List<BusinessPartner>> GetAllAsync()
        {
            try
            {
                _logger.Debug("[BLL] GetAllAsync: Bắt đầu gọi repository");
                var result = await GetDataAccess().GetActivePartnersAsync();
                _logger.Debug($"[BLL] GetAllAsync: Đã nhận được {result?.Count ?? 0} entities từ repository");
                
                // Log thông tin về navigation properties của entities đầu tiên (nếu có)
                if (result != null && result.Count > 0)
                {
                    var firstPartner = result[0];
                    try
                    {
                        var hasApplicationUser = firstPartner.ApplicationUser != null;
                        var hasApplicationUser2 = firstPartner.ApplicationUser2 != null;
                        var categoriesCount = firstPartner.BusinessPartner_BusinessPartnerCategories?.Count ?? 0;
                        _logger.Debug($"[BLL] GetAllAsync: Entity đầu tiên - ApplicationUser: {hasApplicationUser}, ApplicationUser2: {hasApplicationUser2}, Categories: {categoriesCount}");
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"[BLL] GetAllAsync: LỖI khi kiểm tra navigation properties của entity đầu tiên: {ex.Message}", ex);
                        _logger.Error($"[BLL] GetAllAsync: StackTrace: {ex.StackTrace}");
                    }
                }
                
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"[BLL] GetAllAsync: LỖI TỔNG QUÁT: {ex.Message}", ex);
                _logger.Error($"[BLL] GetAllAsync: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Lấy toàn bộ đối tác (entities) - Sync.
        /// </summary>
        public List<BusinessPartner> GetAll()
        {
            // TODO: IBusinessPartnerRepository không có GetAll(), sử dụng GetActivePartners() tạm thời
            return GetDataAccess().GetActivePartners();
        }

        /// <summary>
        /// Lấy dictionary chứa tất cả BusinessPartnerCategory để tính FullPath
        /// </summary>
        /// <returns>Dictionary với Key là CategoryId, Value là BusinessPartnerCategory</returns>
        public async Task<Dictionary<Guid, BusinessPartnerCategory>> GetCategoryDictAsync()
        {
            try
            {
                _logger.Debug("[BLL] GetCategoryDictAsync: Bắt đầu lấy tất cả categories");
                var categoryBll = new BusinessPartnerCategoryBll();
                var categories = await categoryBll.GetAllAsync();
                var categoryDict = categories.ToDictionary(c => c.Id);
                _logger.Debug($"[BLL] GetCategoryDictAsync: Đã lấy được {categoryDict.Count} categories");
                return categoryDict;
            }
            catch (Exception ex)
            {
                _logger.Error($"[BLL] GetCategoryDictAsync: LỖI: {ex.Message}", ex);
                _logger.Error($"[BLL] GetCategoryDictAsync: StackTrace: {ex.StackTrace}");
                throw;
            }
        }

        /// <summary>
        /// Tồn tại mã?
        /// </summary>
        public bool IsCodeExists(string code)
        {
            return GetDataAccess().IsPartnerCodeExists(code);
        }

        /// <summary>
        /// Xóa đối tác theo Id (Soft Delete).
        /// </summary>
        /// <param name="id">ID đối tác</param>
        /// <param name="deletedBy">ID người xóa (optional)</param>
        public void Delete(Guid id, Guid? deletedBy = null)
        {
            GetDataAccess().DeletePartner(id, deletedBy);
        }

        /// <summary>
        /// Lấy đối tác theo Id (entity).
        /// </summary>
        public BusinessPartner GetById(Guid id)
        {
            return GetDataAccess().GetById(id);
        }

        /// <summary>
        /// Lấy đối tác theo Id (Async, entity).
        /// </summary>
        public Task<BusinessPartner> GetByIdAsync(Guid id)
        {
            return GetDataAccess().GetByIdAsync(id);
        }

        /// <summary>
        /// Lưu/cập nhật đầy đủ thông tin đối tác.
        /// </summary>
        /// <param name="entity">Entity đối tác</param>
        /// <param name="userId">ID người dùng thực hiện (optional, dùng cho audit fields)</param>
        public void SaveOrUpdate(BusinessPartner entity, Guid? userId = null)
        {
            GetDataAccess().SaveOrUpdate(entity, userId);
        }

        /// <summary>
        /// Upload logo từ file và lưu lên NAS, đồng thời tạo thumbnail lưu vào database.
        /// </summary>
        /// <param name="partnerId">ID đối tác</param>
        /// <param name="logoFilePath">Đường dẫn file logo</param>
        /// <param name="userId">ID người dùng thực hiện (optional, sẽ lấy từ GetCurrentUser nếu null)</param>
        /// <returns>BusinessPartner entity đã được cập nhật</returns>
        public async Task<BusinessPartner> UploadLogoAsync(Guid partnerId, string logoFilePath, Guid? userId = null)
        {
            try
            {
                if (!File.Exists(logoFilePath))
                {
                    throw new FileNotFoundException($"File logo không tồn tại: {logoFilePath}");
                }

                // 1. Đọc file logo
                var logoData = await Task.Run(() => File.ReadAllBytes(logoFilePath));

                // 2. Tạo tên file mới để tránh trùng lặp (format tương tự StockInOutImageBll)
                var fileExtension = Path.GetExtension(logoFilePath);
                var fileName = $"BP_{partnerId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}.{fileExtension.TrimStart('.')}";

                // 3. Lưu logo lên NAS (sử dụng ImageCategory.Company vì tương tự logo công ty)
                var storageResult = await _imageStorage.SaveImageAsync(
                    imageData: logoData,
                    fileName: fileName,
                    category: ImageCategory.Company, // Sử dụng Company category cho BusinessPartner logo
                    entityId: partnerId,
                    generateThumbnail: false // Không tạo thumbnail trên NAS, sẽ tạo và lưu vào database
                );

                if (!storageResult.Success)
                {
                    throw new InvalidOperationException(
                        $"Không thể lưu logo vào storage: {storageResult.ErrorMessage}");
                }

                // 4. Tạo thumbnail từ logo gốc và resize/compress để lưu vào database
                byte[] thumbnailData = null;
                try
                {
                    thumbnailData = _imageCompression.CompressImage(
                        imageData: logoData,
                        targetSize: 100000, // Target size: ~100KB
                        maxDimension: ApplicationConstants.THUMBNAIL_MAX_DIMENSION, // 300px
                        format: ImageFormat.Jpeg
                    );
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Không thể tạo thumbnail cho logo đối tác {partnerId}: {ex.Message}");
                    // Tiếp tục lưu logo dù không tạo được thumbnail
                }

                // 5. Lấy đối tác từ database
                var partner = GetDataAccess().GetById(partnerId);
                if (partner == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy đối tác với Id: {partnerId}");
                }

                // 6. Lấy thông tin user hiện tại nếu userId chưa có
                if (userId == null || userId == Guid.Empty)
                {
                    var currentUser = ApplicationSystemUtils.GetCurrentUser();
                    userId = currentUser?.Id ?? Guid.Empty;
                }

                // 7. Cập nhật thông tin logo và thumbnail
                partner.LogoFileName = storageResult.FileName;
                partner.LogoRelativePath = storageResult.RelativePath;
                partner.LogoFullPath = storageResult.FullPath;
                partner.LogoStorageType = "NAS"; // Hoặc từ config
                partner.LogoFileSize = storageResult.FileSize;
                partner.LogoChecksum = storageResult.Checksum;
                
                // Lưu thumbnail vào database (LogoThumbnailData)
                if (thumbnailData != null && thumbnailData.Length > 0)
                {
                    partner.LogoThumbnailData = new System.Data.Linq.Binary(thumbnailData);
                }

                partner.UpdatedDate = DateTime.Now;
                // Không set ModifiedBy trực tiếp - SaveOrUpdate sẽ tự xử lý để tránh ForeignKeyReferenceAlreadyHasValueException

                // 8. Lưu vào database
                GetDataAccess().SaveOrUpdate(partner, userId);

                _logger.Info($"Đã upload logo và tạo thumbnail cho đối tác, PartnerId={partnerId}, PartnerName={partner.PartnerName}, RelativePath={storageResult.RelativePath}");
                
                return partner;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi upload logo từ file '{logoFilePath}' cho đối tác {partnerId}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Upload logo từ byte array và lưu lên NAS, đồng thời tạo thumbnail lưu vào database.
        /// </summary>
        /// <param name="partnerId">ID đối tác</param>
        /// <param name="logoData">Dữ liệu logo (byte array)</param>
        /// <param name="fileName">Tên file logo (optional, sẽ tự tạo nếu null)</param>
        /// <param name="userId">ID người dùng thực hiện (optional, sẽ lấy từ GetCurrentUser nếu null)</param>
        /// <returns>BusinessPartner entity đã được cập nhật</returns>
        public async Task<BusinessPartner> UploadLogoFromBytesAsync(Guid partnerId, byte[] logoData, string fileName = null, Guid? userId = null)
        {
            try
            {
                if (logoData == null || logoData.Length == 0)
                {
                    throw new ArgumentException("Dữ liệu logo không được rỗng", nameof(logoData));
                }

                // Tạo tên file nếu chưa có (format tương tự StockInOutImageBll)
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    var fileExtension = ".jpg"; // Mặc định JPEG
                    // Thử detect extension từ image data
                    try
                    {
                        using (var ms = new MemoryStream(logoData))
                        using (var img = Image.FromStream(ms))
                        {
                            if (img.RawFormat.Equals(ImageFormat.Png))
                                fileExtension = ".png";
                            else if (img.RawFormat.Equals(ImageFormat.Gif))
                                fileExtension = ".gif";
                        }
                    }
                    catch
                    {
                        // Nếu không detect được, dùng .jpg mặc định
                    }
                    
                    fileName = $"BP_{partnerId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}.{fileExtension.TrimStart('.')}";
                }

                // 1. Lưu logo lên NAS
                var storageResult = await _imageStorage.SaveImageAsync(
                    imageData: logoData,
                    fileName: fileName,
                    category: ImageCategory.Company, // Sử dụng Company category cho BusinessPartner logo
                    entityId: partnerId,
                    generateThumbnail: false // Không tạo thumbnail trên NAS, sẽ tạo và lưu vào database
                );

                if (!storageResult.Success)
                {
                    throw new InvalidOperationException(
                        $"Không thể lưu logo vào storage: {storageResult.ErrorMessage}");
                }

                // 2. Tạo thumbnail từ logo gốc và resize/compress để lưu vào database
                byte[] thumbnailData = null;
                try
                {
                    thumbnailData = _imageCompression.CompressImage(
                        imageData: logoData,
                        targetSize: 100000, // Target size: ~100KB
                        maxDimension: ApplicationConstants.THUMBNAIL_MAX_DIMENSION, // 300px
                        format: ImageFormat.Jpeg
                    );
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Không thể tạo thumbnail cho logo đối tác {partnerId}: {ex.Message}");
                    // Tiếp tục lưu logo dù không tạo được thumbnail
                }

                // 3. Lấy đối tác từ database
                var partner = GetDataAccess().GetById(partnerId);
                if (partner == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy đối tác với Id: {partnerId}");
                }

                // 4. Lấy thông tin user hiện tại nếu userId chưa có
                if (userId == null || userId == Guid.Empty)
                {
                    var currentUser = ApplicationSystemUtils.GetCurrentUser();
                    userId = currentUser?.Id ?? Guid.Empty;
                }

                // 5. Cập nhật thông tin logo và thumbnail
                partner.LogoFileName = storageResult.FileName;
                partner.LogoRelativePath = storageResult.RelativePath;
                partner.LogoFullPath = storageResult.FullPath;
                partner.LogoStorageType = "NAS"; // Hoặc từ config
                partner.LogoFileSize = storageResult.FileSize;
                partner.LogoChecksum = storageResult.Checksum;
                
                // Lưu thumbnail vào database (LogoThumbnailData)
                if (thumbnailData != null && thumbnailData.Length > 0)
                {
                    partner.LogoThumbnailData = new System.Data.Linq.Binary(thumbnailData);
                }

                partner.UpdatedDate = DateTime.Now;
                // Không set ModifiedBy trực tiếp - SaveOrUpdate sẽ tự xử lý để tránh ForeignKeyReferenceAlreadyHasValueException

                // 6. Lưu vào database
                GetDataAccess().SaveOrUpdate(partner, userId);

                _logger.Info($"Đã upload logo và tạo thumbnail cho đối tác (từ bytes), PartnerId={partnerId}, PartnerName={partner.PartnerName}, RelativePath={storageResult.RelativePath}");
                
                return partner;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi upload logo từ bytes cho đối tác {partnerId}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Upload logo đối tác từ byte array với maxDimension tùy chỉnh cho thumbnail.
        /// Lưu file gốc trên NAS và thumbnail trong database với kích thước phù hợp với thiết kế cột.
        /// </summary>
        /// <param name="partnerId">ID đối tác</param>
        /// <param name="logoData">Dữ liệu logo (byte array)</param>
        /// <param name="thumbnailMaxDimension">Kích thước tối đa cho thumbnail (pixels). Mặc định 120px để phù hợp với cột logo trong GridView</param>
        /// <param name="fileName">Tên file logo (optional, sẽ tự tạo nếu null)</param>
        /// <param name="userId">ID người dùng thực hiện (optional, sẽ lấy từ GetCurrentUser nếu null)</param>
        /// <returns>BusinessPartner entity đã được cập nhật</returns>
        public async Task<BusinessPartner> UploadLogoFromBytesAsync(Guid partnerId, byte[] logoData, int thumbnailMaxDimension, string fileName = null, Guid? userId = null)
        {
            try
            {
                if (logoData == null || logoData.Length == 0)
                {
                    throw new ArgumentException("Dữ liệu logo không được rỗng", nameof(logoData));
                }

                if (thumbnailMaxDimension <= 0)
                {
                    thumbnailMaxDimension = ApplicationConstants.THUMBNAIL_MAX_DIMENSION; // Fallback to default
                }

                // Tạo tên file nếu chưa có (format tương tự StockInOutImageBll)
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    var fileExtension = ".jpg"; // Mặc định JPEG
                    // Thử detect extension từ image data
                    try
                    {
                        using (var ms = new MemoryStream(logoData))
                        using (var img = Image.FromStream(ms))
                        {
                            if (img.RawFormat.Equals(ImageFormat.Png))
                                fileExtension = ".png";
                            else if (img.RawFormat.Equals(ImageFormat.Gif))
                                fileExtension = ".gif";
                        }
                    }
                    catch
                    {
                        // Nếu không detect được, dùng .jpg mặc định
                    }
                    
                    fileName = $"BP_{partnerId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}.{fileExtension.TrimStart('.')}";
                }

                // 1. Lưu logo lên NAS
                var storageResult = await _imageStorage.SaveImageAsync(
                    imageData: logoData,
                    fileName: fileName,
                    category: ImageCategory.Company, // Sử dụng Company category cho BusinessPartner logo
                    entityId: partnerId,
                    generateThumbnail: false // Không tạo thumbnail trên NAS, sẽ tạo và lưu vào database
                );

                if (!storageResult.Success)
                {
                    throw new InvalidOperationException(
                        $"Không thể lưu logo vào storage: {storageResult.ErrorMessage}");
                }

                // 2. Tạo thumbnail từ logo gốc với kích thước tùy chỉnh để phù hợp với thiết kế cột
                byte[] thumbnailData = null;
                try
                {
                    thumbnailData = _imageCompression.CompressImage(
                        imageData: logoData,
                        targetSize: 50000, // Target size: ~50KB (nhỏ hơn vì thumbnail nhỏ hơn)
                        maxDimension: thumbnailMaxDimension, // Sử dụng kích thước tùy chỉnh (120px cho cột logo)
                        format: ImageFormat.Jpeg
                    );
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Không thể tạo thumbnail cho logo đối tác {partnerId}: {ex.Message}");
                    // Tiếp tục lưu logo dù không tạo được thumbnail
                }

                // 3. Lấy đối tác từ database
                var partner = GetDataAccess().GetById(partnerId);
                if (partner == null)
                {
                    throw new InvalidOperationException($"Không tìm thấy đối tác với Id: {partnerId}");
                }

                // 4. Lấy thông tin user hiện tại nếu userId chưa có
                if (userId == null || userId == Guid.Empty)
                {
                    var currentUser = ApplicationSystemUtils.GetCurrentUser();
                    userId = currentUser?.Id ?? Guid.Empty;
                }

                // 5. Cập nhật thông tin logo và thumbnail
                partner.LogoFileName = storageResult.FileName;
                partner.LogoRelativePath = storageResult.RelativePath;
                partner.LogoFullPath = storageResult.FullPath;
                partner.LogoStorageType = "NAS"; // Hoặc từ config
                partner.LogoFileSize = storageResult.FileSize;
                partner.LogoChecksum = storageResult.Checksum;
                
                // Lưu thumbnail vào database (LogoThumbnailData) với kích thước phù hợp với thiết kế cột
                if (thumbnailData != null && thumbnailData.Length > 0)
                {
                    partner.LogoThumbnailData = new System.Data.Linq.Binary(thumbnailData);
                }

                partner.UpdatedDate = DateTime.Now;
                // Không set ModifiedBy trực tiếp - SaveOrUpdate sẽ tự xử lý để tránh ForeignKeyReferenceAlreadyHasValueException

                // 6. Lưu vào database
                GetDataAccess().SaveOrUpdate(partner, userId);

                _logger.Info($"Đã upload logo và tạo thumbnail ({thumbnailMaxDimension}px) cho đối tác, PartnerId={partnerId}, PartnerName={partner.PartnerName}, RelativePath={storageResult.RelativePath}");
                
                return partner;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi upload logo từ bytes cho đối tác {partnerId}: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy dữ liệu logo từ storage (NAS/Local) bằng RelativePath
        /// Tối ưu hơn GetLogoDataAsync vì không cần query database lại
        /// </summary>
        /// <param name="relativePath">Đường dẫn tương đối của logo trong storage</param>
        /// <returns>Dữ liệu logo (byte array) hoặc null</returns>
        public async Task<byte[]> GetLogoDataByRelativePathAsync(string relativePath)
        {
            try
            {
                if (string.IsNullOrEmpty(relativePath))
                {
                    _logger.Warning("RelativePath is null or empty");
                    return null;
                }

                // Lấy trực tiếp từ storage (NAS/Local) sử dụng RelativePath
                var logoData = await _imageStorage.GetImageAsync(relativePath);
                
                if (logoData == null)
                {
                    _logger.Warning($"Không thể đọc file từ storage, RelativePath={relativePath}");
                }
                else
                {
                    _logger.Debug($"Đã load logo từ storage, RelativePath={relativePath}, Size={logoData.Length} bytes");
                }

                return logoData;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy dữ liệu logo từ RelativePath '{relativePath}': {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Lấy dữ liệu logo từ storage (NAS/Local) bằng PartnerId
        /// Query database để lấy RelativePath, sau đó load từ storage
        /// </summary>
        /// <param name="partnerId">ID đối tác</param>
        /// <returns>Dữ liệu logo (byte array) hoặc null</returns>
        public async Task<byte[]> GetLogoDataAsync(Guid partnerId)
        {
            try
            {
                // 1. Query database để lấy metadata (RelativePath)
                var partner = GetDataAccess().GetById(partnerId);
                if (partner == null)
                {
                    _logger.Warning($"Không tìm thấy đối tác với ID '{partnerId}'");
                    return null;
                }

                // 2. Lấy RelativePath từ metadata
                var relativePath = partner.LogoRelativePath;
                
                if (string.IsNullOrEmpty(relativePath))
                {
                    _logger.Warning($"Đối tác '{partnerId}' không có LogoRelativePath");
                    return null;
                }

                // 3. Load từ storage sử dụng RelativePath
                var logoData = await GetLogoDataByRelativePathAsync(relativePath);
                
                return logoData;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lấy dữ liệu logo cho đối tác '{partnerId}': {ex.Message}", ex);
                return null;
            }
        }

        /// <summary>
        /// Xóa logo (file từ storage + metadata trong database)
        /// </summary>
        /// <param name="partnerId">ID đối tác</param>
        /// <param name="userId">ID người dùng thực hiện (optional, sẽ lấy từ GetCurrentUser nếu null)</param>
        public async Task DeleteLogoAsync(Guid partnerId, Guid? userId = null)
        {
            try
            {
                // 1. Lấy thông tin đối tác trước khi xóa
                var partner = GetDataAccess().GetById(partnerId);
                if (partner == null)
                {
                    _logger.Warning($"Không tìm thấy đối tác với ID '{partnerId}'");
                    return;
                }

                var relativePath = partner.LogoRelativePath;

                // 2. Xóa file từ storage (NAS/Local) nếu có RelativePath
                if (!string.IsNullOrEmpty(relativePath))
                {
                    try
                    {
                        var deleted = await _imageStorage.DeleteImageAsync(relativePath);
                        if (deleted)
                        {
                            _logger.Info($"Đã xóa file logo từ storage, RelativePath={relativePath}");
                        }
                        else
                        {
                            _logger.Warning($"Không thể xóa file logo từ storage, RelativePath={relativePath}");
                        }
                    }
                    catch (Exception storageEx)
                    {
                        _logger.Warning($"Lỗi khi xóa file logo từ storage '{relativePath}': {storageEx.Message}");
                        // Không throw exception vì có thể file đã bị xóa hoặc không có quyền
                    }
                }

                // 3. Lấy thông tin user hiện tại nếu userId chưa có
                if (userId == null || userId == Guid.Empty)
                {
                    var currentUser = ApplicationSystemUtils.GetCurrentUser();
                    userId = currentUser?.Id ?? Guid.Empty;
                }

                // 4. Xóa metadata trong database (set các field về null)
                partner.LogoFileName = null;
                partner.LogoRelativePath = null;
                partner.LogoFullPath = null;
                partner.LogoStorageType = null;
                partner.LogoFileSize = null;
                partner.LogoChecksum = null;
                partner.LogoThumbnailData = null;
                partner.UpdatedDate = DateTime.Now;
                // Không set ModifiedBy trực tiếp - SaveOrUpdate sẽ tự xử lý để tránh ForeignKeyReferenceAlreadyHasValueException

                GetDataAccess().SaveOrUpdate(partner, userId);

                _logger.Info($"Đã xóa logo cho đối tác, PartnerId={partnerId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi xóa logo cho đối tác '{partnerId}': {ex.Message}", ex);
                throw;
            }
        }
    }
}
