using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bll.Common.ImageStorage;
using Bll.Common.ImageService;
using Bll.Common;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.MasterData.ProductServiceBll
{
    /// <summary>
    /// Business Logic Layer cho ProductVariant.
    /// Cung cấp các phương thức xử lý nghiệp vụ cho biến thể sản phẩm.
    /// </summary>
    public class ProductVariantBll
    {
        #region Fields

        private IProductVariantRepository _dataAccess;
        private readonly object _lockObject = new object();
        private IImageStorageService _imageStorage;
        private readonly object _imageStorageLock = new object();
        private readonly ImageCompressionService _imageCompression;
        private readonly ILogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo ProductVariantBll
        /// </summary>
        public ProductVariantBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
            _imageCompression = new ImageCompressionService();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IProductVariantRepository GetDataAccess()
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

                            _dataAccess = new ProductVariantRepository(globalConnectionString);
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
        /// Lấy biến thể theo ID
        /// </summary>
        /// <param name="id">ID biến thể</param>
        /// <returns>ProductVariantDto</returns>
        public ProductVariantDto GetById(Guid id)
        {
            try
            {
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo ID (Async)
        /// </summary>
        /// <param name="id">ID biến thể</param>
        /// <returns>ProductVariantDto</returns>
        public async Task<ProductVariantDto> GetByIdAsync(Guid id)
        {
            try
            {
                return await GetDataAccess().GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy biến thể theo ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách biến thể theo ProductId
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>Danh sách ProductVariantDto</returns>
        public List<ProductVariantDto> GetByProductId(Guid productId)
        {
            try
            {
                return GetDataAccess().GetByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể sản phẩm
        /// </summary>
        /// <returns>Danh sách ProductVariantDto</returns>
        public async Task<List<ProductVariantSimpleDto>> GetAllAsync()
        {
            try
            {
                return await GetDataAccess().GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể sản phẩm với thông tin đầy đủ
        /// Bao gồm thông tin sản phẩm gốc, đơn vị tính, thuộc tính và hình ảnh
        /// </summary>
        /// <returns>Danh sách ProductVariantDto với thông tin đầy đủ</returns>
        public async Task<List<ProductVariantDto>> GetAllWithDetailsAsync()
        {
            try
            {
                return await GetDataAccess().GetAllWithDetailsAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể sản phẩm đang hoạt động với thông tin đầy đủ
        /// Tương tự như GetAllWithDetailsAsync nhưng chỉ lấy các record có IsActive = true
        /// </summary>
        /// <returns>Danh sách ProductVariantDto đang hoạt động với thông tin đầy đủ</returns>
        public async Task<List<ProductVariantDto>> GetAllInUseWithDetailsAsync()
        {
            try
            {
                var variants = await GetDataAccess().GetAllWithDetailsAsync();
                
                // Filter chỉ lấy các record đang hoạt động (IsActive = true)
                return variants
                    .Where(v => v.IsActive)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể đang hoạt động: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách giá trị thuộc tính của biến thể
        /// </summary>
        /// <param name="variantId">ID biến thể</param>
        /// <returns>Danh sách giá trị thuộc tính (AttributeId, AttributeName, Value)</returns>
        public List<(Guid AttributeId, string AttributeName, string Value)> GetAttributeValues(Guid variantId)
        {
            try
            {
                return GetDataAccess().GetAttributeValues(variantId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy giá trị thuộc tính: {ex.Message}", ex);
            }
        }

        public string GetForNewAttribute(Guid variantId)
        {
            return GetDataAccess().GetForNewAttribute(variantId);
        }
        #endregion

        #region ========== WRITE OPERATIONS ==========

        /// <summary>
        /// Lưu biến thể (tạo mới hoặc cập nhật)
        /// </summary>
        /// <param name="variant">ProductVariantDto</param>
        /// <param name="attributeValues">Danh sách giá trị thuộc tính (AttributeId, Value)</param>
        /// <returns>ID biến thể đã lưu</returns>
        public async Task<Guid> SaveAsync(ProductVariantDto variant, List<(Guid AttributeId, string Value)> attributeValues)
        {
            try
            {
                // Validate dữ liệu
                ValidateVariantData(attributeValues);

                // Kiểm tra trùng mã biến thể
                if (IsVariantCodeExists(variant.VariantCode, variant.Id == Guid.Empty ? null : variant.Id))
                {
                    throw new Exception($"Mã biến thể '{variant.VariantCode}' đã tồn tại. Vui lòng chọn mã khác.");
                }

                // Lưu biến thể
                var savedVariantId = await GetDataAccess().SaveAsync(variant, attributeValues);

                // Cập nhật VariantFullName cho biến thể vừa lưu
                await UpdateVariantFullNameAsync(savedVariantId);

                return savedVariantId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa biến thể
        /// </summary>
        /// <param name="id">ID biến thể</param>
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await GetDataAccess().DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa biến thể: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== UPDATE OPERATIONS ==========

        /// <summary>
        /// Cập nhật VariantFullName cho tất cả biến thể hiện có
        /// </summary>
        public async Task UpdateAllVariantFullNamesAsync()
        {
            try
            {
                await GetDataAccess().UpdateAllVariantFullNamesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật VariantFullName: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật VariantFullName cho một biến thể cụ thể
        /// Gọi method từ repository (tuân thủ nguyên tắc BLL không sử dụng DataContext trực tiếp)
        /// </summary>
        /// <param name="variantId">ID biến thể cần cập nhật</param>
        private async Task UpdateVariantFullNameAsync(Guid variantId)
        {
            try
            {
                await GetDataAccess().UpdateVariantFullNameAsync(variantId);
            }
            catch (Exception ex)
            {
                // Log lỗi nhưng không throw để không ảnh hưởng đến quá trình lưu chính
                // Có thể log vào logger nếu cần
                System.Diagnostics.Debug.WriteLine($"Lỗi cập nhật VariantFullName cho biến thể {variantId}: {ex.Message}");
            }
        }

        /// <summary>
        /// Cập nhật thumbnail image cho biến thể sản phẩm từ byte array
        /// Lưu ảnh gốc lên NAS và thumbnail đã resize vào database
        /// </summary>
        /// <param name="variantId">ID biến thể sản phẩm</param>
        /// <param name="imageBytes">Byte array của hình ảnh gốc (null để xóa)</param>
        /// <param name="thumbnailMaxDimension">Kích thước tối đa của thumbnail (mặc định 120px)</param>
        /// <returns>ProductVariantDto đã được cập nhật</returns>
        public async Task<ProductVariantDto> UpdateThumbnailImageAsync(Guid variantId, byte[] imageBytes, int thumbnailMaxDimension = 120)
        {
            try
            {
                var variant = GetById(variantId);
                if (variant == null)
                {
                    throw new Exception($"Không tìm thấy biến thể sản phẩm với ID {variantId}");
                }

                // Sử dụng UpdateThumbnailImageOnlyAsync để xử lý
                await UpdateThumbnailImageOnlyAsync(variantId, imageBytes, thumbnailMaxDimension);

                // Lấy lại variant đã cập nhật
                return GetById(variantId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi cập nhật thumbnail image cho biến thể {variantId}: {ex.Message}", ex);
                throw new Exception($"Lỗi khi cập nhật thumbnail image: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Chỉ cập nhật/xóa thumbnail image của biến thể theo ID, không ảnh hưởng đến các trường khác
        /// Tự động resize hình ảnh trước khi lưu thumbnail
        /// </summary>
        /// <param name="variantId">ID biến thể cần cập nhật</param>
        /// <param name="imageBytes">Byte array của hình ảnh gốc (null để xóa thumbnail)</param>
        /// <param name="thumbnailMaxDimension">Kích thước tối đa của thumbnail (mặc định 120px)</param>
        /// <returns>Task</returns>
        public async Task UpdateThumbnailImageOnlyAsync(Guid variantId, byte[] imageBytes, int thumbnailMaxDimension = 120)
        {
            try
            {
                // Kiểm tra variant có tồn tại không
                var variant = GetById(variantId);
                if (variant == null)
                {
                    throw new Exception($"Không tìm thấy biến thể sản phẩm với ID {variantId}");
                }

                // Xử lý xóa thumbnail
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    // Xóa thông tin trong database (không cần xóa file trên NAS vì không có thông tin trong DTO)
                    await GetDataAccess().UpdateThumbnailOnlyAsync(
                        variantId: variantId,
                        thumbnailImage: null,
                        thumbnailFileName: null,
                        thumbnailRelativePath: null,
                        thumbnailFullPath: null,
                        thumbnailStorageType: null,
                        thumbnailFileSize: null,
                        thumbnailChecksum: null);

                    _logger.Info($"Đã xóa thumbnail cho biến thể {variantId}");
                    return;
                }

                // Xử lý upload thumbnail mới
                if (thumbnailMaxDimension <= 0)
                {
                    thumbnailMaxDimension = 120; // Default 120px cho cột thumbnail
                }

                // 1. Tạo tên file
                var fileExtension = ".jpg"; // Mặc định JPEG
                try
                {
                    using (var ms = new MemoryStream(imageBytes))
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

                var fileName = $"PV_{variantId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";

                // 2. Lưu ảnh gốc lên NAS
                var storageResult = await GetImageStorage().SaveImageAsync(
                    imageData: imageBytes,
                    fileName: fileName,
                    category: ImageCategory.ProductVariant,
                    entityId: variantId,
                    generateThumbnail: false // Không tạo thumbnail trên NAS, sẽ tạo và lưu vào database
                );

                if (!storageResult.Success)
                {
                    throw new InvalidOperationException(
                        $"Không thể lưu hình ảnh vào storage: {storageResult.ErrorMessage}");
                }

                // 3. Tạo thumbnail từ ảnh gốc với kích thước tùy chỉnh
                byte[] thumbnailData = null;
                try
                {
                    thumbnailData = _imageCompression.CompressImage(
                        imageData: imageBytes,
                        targetSize: 50000, // Target size: ~50KB
                        maxDimension: thumbnailMaxDimension, // Sử dụng kích thước tùy chỉnh (120px cho cột thumbnail)
                        format: ImageFormat.Jpeg
                    );
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Không thể tạo thumbnail cho biến thể {variantId}: {ex.Message}");
                    // Tiếp tục lưu ảnh dù không tạo được thumbnail
                }

                // 4. Lưu thumbnail đã resize vào database
                Binary thumbnailBinary = null;
                if (thumbnailData != null && thumbnailData.Length > 0)
                {
                    thumbnailBinary = new Binary(thumbnailData);
                }
                else
                {
                    // Nếu không tạo được thumbnail, lưu ảnh gốc đã resize nhỏ
                    thumbnailBinary = new Binary(imageBytes);
                }

                // 6. Cập nhật chỉ các field thumbnail trong database
                await GetDataAccess().UpdateThumbnailOnlyAsync(
                    variantId: variantId,
                    thumbnailImage: thumbnailBinary,
                    thumbnailFileName: storageResult.FileName,
                    thumbnailRelativePath: storageResult.RelativePath,
                    thumbnailFullPath: storageResult.FullPath,
                    thumbnailStorageType: "NAS",
                    thumbnailFileSize: storageResult.FileSize,
                    thumbnailChecksum: storageResult.Checksum);

                _logger.Info($"Đã cập nhật thumbnail cho biến thể {variantId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi cập nhật thumbnail image cho biến thể {variantId}: {ex.Message}", ex);
                throw new Exception($"Lỗi khi cập nhật thumbnail image: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== QUERY OPERATIONS ==========

        /// <summary>
        /// Lấy queryable cho LinqInstantFeedbackSource (trả về Entity)
        /// </summary>
        /// <returns>IQueryable của ProductVariant entity</returns>
        public IQueryable<ProductVariant> GetQueryableForInstantFeedback()
        {
            try
            {
                // Lấy queryable entity từ DAL
                return GetDataAccess().GetQueryableForInstantFeedback();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy queryable cho LinqInstantFeedbackSource: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tổng số bản ghi
        /// </summary>
        /// <returns>Tổng số bản ghi</returns>
        public int GetTotalCount()
        {
            try
            {
                return GetDataAccess().GetTotalCount();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tổng số bản ghi: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy DataContext để sử dụng với LinqServerModeSource
        /// Tuân thủ quy tắc: Dal -> Bll -> GUI
        /// </summary>
        /// <returns>DataContext</returns>
        public async Task<VnsErp2025DataContext> GetDataContextAsync()
        {
            try
            {
                // Lấy DataContext từ DAL (tuân thủ Dal -> Bll)
                return await GetDataAccess().GetDataContextAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy DataContext: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== VALIDATION METHODS ==========

        /// <summary>
        /// Kiểm tra mã biến thể có trùng không
        /// </summary>
        /// <param name="variantCode">Mã biến thể</param>
        /// <param name="excludeId">ID biến thể cần loại trừ (khi edit)</param>
        /// <returns>True nếu trùng</returns>
        private bool IsVariantCodeExists(string variantCode, Guid? excludeId = null)
        {
            try
            {
                return GetDataAccess().IsVariantCodeExists(variantCode, excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi kiểm tra mã biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Validate dữ liệu biến thể
        /// </summary>
        private void ValidateVariantData(List<(Guid AttributeId, string Value)> attributeValues)
        {
            if (attributeValues == null) return;
            
            foreach (var (attributeId, value) in attributeValues)
            {
                if (attributeId == Guid.Empty)
                    throw new ArgumentException("Vui lòng chọn đầy đủ thuộc tính");

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Vui lòng nhập đầy đủ giá trị thuộc tính");
            }
        }

        #endregion

       
    }
}
