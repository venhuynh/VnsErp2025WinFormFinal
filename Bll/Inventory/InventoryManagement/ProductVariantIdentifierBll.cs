using Bll.Common.ImageService;
using Bll.Common.ImageStorage;
using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace Bll.Inventory.InventoryManagement
{
    /// <summary>
    /// Business Logic Layer cho ProductVariantIdentifier
    /// Quản lý các thao tác business logic với ProductVariantIdentifier (Định danh biến thể sản phẩm)
    /// </summary>
    public class ProductVariantIdentifierBll
    {
        #region Fields

        private IProductVariantIdentifierRepository _productVariantIdentifierRepository;
        private readonly ILogger _logger;
        private readonly object Lock = new object();
        private IImageStorageService _imageStorage;
        private readonly object _imageStorageLock = new object();
        private readonly ImageCompressionService _imageCompression;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ProductVariantIdentifierBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
            _imageCompression = new ImageCompressionService();
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo ProductVariantIdentifierRepository (lazy initialization)
        /// </summary>
        private IProductVariantIdentifierRepository GetProductVariantIdentifierRepository()
        {
            if (_productVariantIdentifierRepository == null)
            {
                lock (Lock)
                {
                    if (_productVariantIdentifierRepository == null)
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

                            _productVariantIdentifierRepository = new ProductVariantIdentifierRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            _logger.Error($"Lỗi khởi tạo ProductVariantIdentifierRepository: {ex.Message}", ex);
                            throw;
                        }
                    }
                }
            }

            return _productVariantIdentifierRepository;
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
        /// Lấy tất cả ProductVariantIdentifier
        /// </summary>
        /// <returns>Danh sách tất cả ProductVariantIdentifierDto</returns>
        public List<ProductVariantIdentifierDto> GetAll()
        {
            try
            {
                _logger.Debug("GetAll: Lấy tất cả định danh sản phẩm");

                var dtos = GetProductVariantIdentifierRepository().GetAll();

                _logger.Info("GetAll: Lấy được {0} định danh sản phẩm", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetAll: Lỗi lấy danh sách định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy ProductVariantIdentifier theo ID
        /// </summary>
        /// <param name="id">ID của ProductVariantIdentifier</param>
        /// <returns>ProductVariantIdentifierDto hoặc null</returns>
        public ProductVariantIdentifierDto GetById(Guid id)
        {
            try
            {
                _logger.Debug("GetById: Lấy định danh sản phẩm, Id={0}", id);

                var dto = GetProductVariantIdentifierRepository().GetById(id);

                if (dto == null)
                {
                    _logger.Warning("GetById: Không tìm thấy định danh sản phẩm, Id={0}", id);
                }
                else
                {
                    _logger.Info("GetById: Lấy định danh sản phẩm thành công, Id={0}", id);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetById: Lỗi lấy định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách ProductVariantIdentifier theo ProductVariantId
        /// </summary>
        /// <param name="productVariantId">ID biến thể sản phẩm</param>
        /// <returns>Danh sách ProductVariantIdentifierDto</returns>
        public List<ProductVariantIdentifierDto> GetByProductVariantId(Guid productVariantId)
        {
            try
            {
                _logger.Debug("GetByProductVariantId: Lấy danh sách định danh sản phẩm, ProductVariantId={0}", productVariantId);

                var dtos = GetProductVariantIdentifierRepository().GetByProductVariantId(productVariantId);

                _logger.Info("GetByProductVariantId: Lấy được {0} định danh sản phẩm", dtos.Count);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByProductVariantId: Lỗi lấy danh sách định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Tìm ProductVariantIdentifier theo giá trị định danh (SerialNumber, PartNumber, QRCode, SKU, RFID, MACAddress, IMEI, AssetTag, LicenseKey, UPC, EAN, ISBN)
        /// </summary>
        /// <param name="identifierValue">Giá trị định danh cần tìm</param>
        /// <returns>ProductVariantIdentifierDto nếu tìm thấy, null nếu không tìm thấy</returns>
        public ProductVariantIdentifierDto FindByIdentifier(string identifierValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(identifierValue))
                {
                    _logger.Warning("FindByIdentifier: IdentifierValue is null or empty");
                    return null;
                }

                _logger.Debug("FindByIdentifier: Tìm định danh sản phẩm, IdentifierValue={0}", identifierValue);

                // Sử dụng Repository để tìm kiếm (BLL -> Repository)
                var dto = GetProductVariantIdentifierRepository().FindByIdentifier(identifierValue);

                if (dto == null)
                {
                    _logger.Warning("FindByIdentifier: Không tìm thấy định danh sản phẩm với giá trị, IdentifierValue={0}", identifierValue);
                }
                else
                {
                    _logger.Info("FindByIdentifier: Tìm thấy định danh sản phẩm, Id={0}, IdentifierValue={1}", dto.Id, identifierValue);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"FindByIdentifier: Lỗi tìm định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Tìm ProductVariantIdentifier theo SerialNumber
        /// </summary>
        /// <param name="serialNumber">Số serial cần tìm</param>
        /// <returns>ProductVariantIdentifierDto nếu tìm thấy, null nếu không tìm thấy</returns>
        public ProductVariantIdentifierDto FindBySerialNumber(string serialNumber)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(serialNumber))
                {
                    _logger.Warning("FindBySerialNumber: SerialNumber is null or empty");
                    return null;
                }

                _logger.Debug("FindBySerialNumber: Tìm định danh sản phẩm, SerialNumber={0}", serialNumber);

                var dto = GetProductVariantIdentifierRepository().FindBySerialNumber(serialNumber);

                if (dto == null)
                {
                    _logger.Warning("FindBySerialNumber: Không tìm thấy định danh sản phẩm với SerialNumber, SerialNumber={0}", serialNumber);
                }
                else
                {
                    _logger.Info("FindBySerialNumber: Tìm thấy định danh sản phẩm, Id={0}, SerialNumber={1}", dto.Id, serialNumber);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"FindBySerialNumber: Lỗi tìm định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Tìm ProductVariantIdentifier theo PartNumber
        /// </summary>
        /// <param name="barcode">Mã vạch cần tìm</param>
        /// <returns>ProductVariantIdentifierDto nếu tìm thấy, null nếu không tìm thấy</returns>
        public ProductVariantIdentifierDto FindByBarcode(string barcode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(barcode))
                {
                    _logger.Warning("FindByBarcode: PartNumber is null or empty");
                    return null;
                }

                _logger.Debug("FindByBarcode: Tìm định danh sản phẩm, PartNumber={0}", barcode);

                var dto = GetProductVariantIdentifierRepository().FindByBarcode(barcode);

                if (dto == null)
                {
                    _logger.Warning("FindByBarcode: Không tìm thấy định danh sản phẩm với PartNumber, PartNumber={0}", barcode);
                }
                else
                {
                    _logger.Info("FindByBarcode: Tìm thấy định danh sản phẩm, Id={0}, PartNumber={1}", dto.Id, barcode);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"FindByBarcode: Lỗi tìm định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Tìm ProductVariantIdentifier theo SKU
        /// </summary>
        /// <param name="sku">Mã SKU cần tìm</param>
        /// <returns>ProductVariantIdentifierDto nếu tìm thấy, null nếu không tìm thấy</returns>
        public ProductVariantIdentifierDto FindBySKU(string sku)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sku))
                {
                    _logger.Warning("FindBySKU: SKU is null or empty");
                    return null;
                }

                _logger.Debug("FindBySKU: Tìm định danh sản phẩm, SKU={0}", sku);

                var dto = GetProductVariantIdentifierRepository().FindBySKU(sku);

                if (dto == null)
                {
                    _logger.Warning("FindBySKU: Không tìm thấy định danh sản phẩm với SKU, SKU={0}", sku);
                }
                else
                {
                    _logger.Info("FindBySKU: Tìm thấy định danh sản phẩm, Id={0}, SKU={1}", dto.Id, sku);
                }

                return dto;
            }
            catch (Exception ex)
            {
                _logger.Error($"FindBySKU: Lỗi tìm định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy danh sách ProductVariantIdentifier theo tình trạng (Status)
        /// </summary>
        /// <param name="status">Tình trạng cần tìm</param>
        /// <returns>Danh sách ProductVariantIdentifierDto</returns>
        public List<ProductVariantIdentifierDto> GetByStatus(ProductVariantIdentifierStatusEnum status)
        {
            try
            {
                _logger.Debug("GetByStatus: Lấy danh sách định danh sản phẩm, Status={0}", status);

                var dtos = GetProductVariantIdentifierRepository().GetByStatus(status);

                _logger.Info("GetByStatus: Lấy được {0} định danh sản phẩm với Status={1}", dtos.Count, status);
                return dtos;
            }
            catch (Exception ex)
            {
                _logger.Error($"GetByStatus: Lỗi lấy danh sách định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        #region ========== CREATE/UPDATE OPERATIONS ==========

        /// <summary>
        /// Lưu hoặc cập nhật ProductVariantIdentifier
        /// </summary>
        /// <param name="dto">ProductVariantIdentifierDto cần lưu</param>
        /// <returns>ProductVariantIdentifierDto đã được lưu</returns>
        public ProductVariantIdentifierDto SaveOrUpdate(ProductVariantIdentifierDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                _logger.Debug("SaveOrUpdate: Bắt đầu lưu định danh sản phẩm, Id={0}, ProductVariantId={1}",
                    dto.Id, dto.ProductVariantId);

                // Business logic validation
                ValidateBeforeSave(dto);

                var result = GetProductVariantIdentifierRepository().SaveOrUpdate(dto);

                _logger.Info("SaveOrUpdate: Lưu định danh sản phẩm thành công, Id={0}", result?.Id ?? dto.Id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveOrUpdate: Lỗi lưu định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        
        /// <summary>
        /// Validate dữ liệu trước khi lưu
        /// </summary>
        /// <param name="dto">ProductVariantIdentifierDto cần validate</param>
        private void ValidateBeforeSave(ProductVariantIdentifierDto dto)
        {
            // Kiểm tra ProductVariantId không được rỗng
            if (dto.ProductVariantId == Guid.Empty)
            {
                throw new ArgumentException("ProductVariantId không được để trống", nameof(dto));
            }

            // Kiểm tra ít nhất một định danh phải có giá trị
            if (!dto.HasAnyIdentifier())
            {
                throw new InvalidOperationException("Phải có ít nhất một định danh được nhập (SerialNumber, PartNumber, QRCode, SKU, v.v.)");
            }

            // Kiểm tra tính duy nhất của các định danh (nếu cần)
            // Có thể thêm logic kiểm tra duplicate ở đây
        }

        #endregion

        #region ========== QR CODE IMAGE OPERATIONS ==========

        /// <summary>
        /// Cập nhật hình ảnh QR code cho ProductVariantIdentifier từ byte array
        /// Lưu ảnh gốc lên NAS và lưu dữ liệu ảnh vào database
        /// </summary>
        /// <param name="identifierId">ID của ProductVariantIdentifier</param>
        /// <param name="imageBytes">Byte array của hình ảnh QR code (null để xóa)</param>
        /// <returns>ProductVariantIdentifierDto đã được cập nhật</returns>
        public async Task<ProductVariantIdentifierDto> UpdateQRCodeImageAsync(Guid identifierId, byte[] imageBytes)
        {
            try
            {
                var identifier = GetById(identifierId);
                if (identifier == null)
                {
                    throw new Exception($"Không tìm thấy ProductVariantIdentifier với ID {identifierId}");
                }

                // Kiểm tra xem hình ảnh có bị khóa không
                if (identifier.QRCodeImageLocked)
                {
                    throw new InvalidOperationException(
                        $"Hình ảnh QR code đã bị khóa. Không thể cập nhật hình ảnh cho ProductVariantIdentifier {identifierId}.");
                }

                // Sử dụng UpdateQRCodeImageOnlyAsync để xử lý
                await UpdateQRCodeImageOnlyAsync(identifierId, imageBytes);

                // Lấy lại identifier đã cập nhật
                return GetById(identifierId);
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi cập nhật QR code image cho ProductVariantIdentifier {identifierId}: {ex.Message}", ex);
                throw new Exception($"Lỗi khi cập nhật QR code image: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Chỉ cập nhật/xóa hình ảnh QR code của ProductVariantIdentifier theo ID, không ảnh hưởng đến các trường khác
        /// Lưu ảnh gốc lên NAS và lưu dữ liệu ảnh vào database
        /// </summary>
        /// <param name="identifierId">ID của ProductVariantIdentifier cần cập nhật</param>
        /// <param name="imageBytes">Byte array của hình ảnh QR code (null để xóa)</param>
        /// <returns>Task</returns>
        public async Task UpdateQRCodeImageOnlyAsync(Guid identifierId, byte[] imageBytes)
        {
            try
            {
                // Kiểm tra identifier có tồn tại không
                var identifier = GetById(identifierId);
                if (identifier == null)
                {
                    throw new Exception($"Không tìm thấy ProductVariantIdentifier với ID {identifierId}");
                }

                // Kiểm tra xem hình ảnh có bị khóa không
                if (identifier.QRCodeImageLocked)
                {
                    throw new InvalidOperationException(
                        $"Hình ảnh QR code đã bị khóa. Không thể cập nhật hình ảnh cho ProductVariantIdentifier {identifierId}.");
                }

                // Xử lý xóa hình ảnh
                if (imageBytes == null || imageBytes.Length == 0)
                {
                    // Xóa thông tin trong database
                    identifier.QRCodeImage = null;
                    identifier.QRCodeImagePath = null;
                    identifier.QRCodeImageFullPath = null;
                    identifier.QRCodeImageFileName = null;
                    identifier.QRCodeImageStorageType = null;

                    // Lưu vào database
                    SaveOrUpdate(identifier);

                    _logger.Info($"Đã xóa QR code image cho ProductVariantIdentifier {identifierId}");
                    return;
                }

                // Xử lý upload hình ảnh mới
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

                var fileName = $"PVI_{identifierId}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";

                // 2. Lưu ảnh gốc lên NAS
                var storageResult = await GetImageStorage().SaveImageAsync(
                    imageData: imageBytes,
                    fileName: fileName,
                    category: ImageCategory.ProductVariant, // Sử dụng ProductVariant category vì liên quan đến ProductVariant
                    entityId: identifier.ProductVariantId,
                    generateThumbnail: false // Không tạo thumbnail trên NAS
                );

                if (!storageResult.Success)
                {
                    throw new InvalidOperationException(
                        $"Không thể lưu hình ảnh vào storage: {storageResult.ErrorMessage}");
                }

                // 3. Cập nhật thông tin hình ảnh vào DTO
                identifier.QRCodeImage = imageBytes; // Lưu dữ liệu ảnh vào database
                identifier.QRCodeImagePath = storageResult.RelativePath;
                identifier.QRCodeImageFullPath = storageResult.FullPath;
                identifier.QRCodeImageFileName = storageResult.FileName;
                identifier.QRCodeImageStorageType = "NAS";

                // 4. Lưu vào database
                SaveOrUpdate(identifier);

                _logger.Info($"Đã cập nhật QR code image cho ProductVariantIdentifier {identifierId}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi cập nhật QR code image cho ProductVariantIdentifier {identifierId}: {ex.Message}", ex);
                throw new Exception($"Lỗi khi cập nhật QR code image: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa ProductVariantIdentifier theo ID (soft delete)
        /// </summary>
        /// <param name="id">ID của ProductVariantIdentifier cần xóa</param>
        /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
        public bool Delete(Guid id)
        {
            try
            {
                _logger.Debug("Delete: Xóa định danh sản phẩm, Id={0}", id);

                var result = GetProductVariantIdentifierRepository().Delete(id);

                if (result)
                {
                    _logger.Info("Delete: Xóa định danh sản phẩm thành công (soft delete), Id={0}", id);
                }
                else
                {
                    _logger.Warning("Delete: Không tìm thấy định danh sản phẩm để xóa, Id={0}", id);
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.Error($"Delete: Lỗi xóa định danh sản phẩm: {ex.Message}", ex);
                throw;
            }
        }

        #endregion

        
    }
}
