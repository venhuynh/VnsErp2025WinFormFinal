using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Dal.DtoConverter;
using DTO.Inventory.InventoryManagement;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

/// <summary>
/// Repository implementation cho ProductVariantIdentifier
/// Quản lý các thao tác CRUD với bảng ProductVariantIdentifier (Định danh biến thể sản phẩm)
/// </summary>
public class ProductVariantIdentifierRepository : IProductVariantIdentifierRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class ProductVariantIdentifierRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public ProductVariantIdentifierRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("ProductVariantIdentifierRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        // Load ProductVariant và các navigation properties liên quan
        loadOptions.LoadWith<ProductVariantIdentifier>(pvi => pvi.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(pv => pv.ProductService);
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Lấy dictionary thông tin ProductVariant cho danh sách ProductVariantIdentifier
    /// Bao gồm: ProductVariantFullName và CustomerCategory
    /// </summary>
    private Dictionary<Guid, (string ProductVariantFullName, string CustomerCategory)> GetProductVariantDict(
        VnsErp2025DataContext context, 
        IEnumerable<ProductVariantIdentifier> identifiers)
    {
        var productVariantIds = identifiers
            .Where(pvi => pvi.ProductVariantId != Guid.Empty)
            .Select(pvi => pvi.ProductVariantId)
            .Distinct()
            .ToList();

        if (!productVariantIds.Any())
            return new Dictionary<Guid, (string, string)>();

        var productVariants = context.ProductVariants
            .Where(pv => productVariantIds.Contains(pv.Id))
            .Select(pv => new
            {
                pv.Id,
                pv.VariantFullName,
                // CustomerCategory có thể lấy từ BusinessPartner nếu có liên kết
                // Hoặc có thể là field riêng trong ProductVariant
                // Tạm thời để null, có thể mở rộng sau
                CustomerCategory = (string)null
            })
            .ToList();

        return productVariants.ToDictionary(
            pv => pv.Id,
            pv => (pv.VariantFullName, pv.CustomerCategory)
        );
    }

    /// <summary>
    /// Lấy thông tin ProductVariant cho một ProductVariantIdentifier
    /// </summary>
    private (string ProductVariantFullName, string CustomerCategory) GetProductVariantInfo(
        VnsErp2025DataContext context, 
        Guid productVariantId)
    {
        if (productVariantId == Guid.Empty)
            return (null, null);

        var productVariant = context.ProductVariants.FirstOrDefault(pv => pv.Id == productVariantId);
        if (productVariant == null)
            return (null, null);

        // CustomerCategory có thể lấy từ BusinessPartner nếu có liên kết
        // Hoặc có thể là field riêng trong ProductVariant
        // Tạm thời để null, có thể mở rộng sau
        return (productVariant.VariantFullName, null);
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả ProductVariantIdentifier
    /// </summary>
    /// <returns>Danh sách tất cả ProductVariantIdentifierDto</returns>
    public List<ProductVariantIdentifierDto> GetAll()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetAll: Lấy tất cả định danh sản phẩm");

            var entities = context.ProductVariantIdentifiers
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetAll: Lấy được {0} định danh sản phẩm", entities.Count);
            return entities.ToDtoList(productVariantDict);
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
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy định danh sản phẩm, Id={0}", id);

            var entity = context.ProductVariantIdentifiers.FirstOrDefault(pvi => pvi.Id == id);

            if (entity == null)
            {
                _logger.Warning("GetById: Không tìm thấy định danh sản phẩm, Id={0}", id);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (productVariantFullName, customerCategory) = GetProductVariantInfo(context, entity.ProductVariantId);

            _logger.Info("GetById: Lấy định danh sản phẩm thành công, Id={0}", id);
            return entity.ToDto(productVariantFullName, customerCategory);
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
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByProductVariantId: Lấy danh sách định danh sản phẩm, ProductVariantId={0}", productVariantId);

            var entities = context.ProductVariantIdentifiers
                .Where(pvi => pvi.ProductVariantId == productVariantId && pvi.IsActive)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByProductVariantId: Lấy được {0} định danh sản phẩm", entities.Count);
            return entities.ToDtoList(productVariantDict);
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
        using var context = CreateNewContext();
        try
        {
            if (string.IsNullOrWhiteSpace(identifierValue))
            {
                _logger.Warning("FindByIdentifier: IdentifierValue is null or empty");
                return null;
            }

            _logger.Debug("FindByIdentifier: Tìm định danh sản phẩm, IdentifierValue={0}", identifierValue);

            var trimmedValue = identifierValue.Trim().ToLower();

            // Tìm ProductVariantIdentifier theo tất cả các loại định danh
            var entity = context.ProductVariantIdentifiers.FirstOrDefault(pvi =>
                pvi.IsActive &&
                (
                    (pvi.SerialNumber != null && pvi.SerialNumber.Trim().ToLower() == trimmedValue) ||
                    (pvi.Barcode != null && pvi.Barcode.Trim().ToLower() == trimmedValue) ||
                    (pvi.QRCode != null && pvi.QRCode.Trim().ToLower() == trimmedValue) ||
                    (pvi.SKU != null && pvi.SKU.Trim().ToLower() == trimmedValue) ||
                    (pvi.RFID != null && pvi.RFID.Trim().ToLower() == trimmedValue) ||
                    (pvi.MACAddress != null && pvi.MACAddress.Trim().ToLower() == trimmedValue) ||
                    (pvi.IMEI != null && pvi.IMEI.Trim().ToLower() == trimmedValue) ||
                    (pvi.AssetTag != null && pvi.AssetTag.Trim().ToLower() == trimmedValue) ||
                    (pvi.LicenseKey != null && pvi.LicenseKey.Trim().ToLower() == trimmedValue) ||
                    (pvi.UPC != null && pvi.UPC.Trim().ToLower() == trimmedValue) ||
                    (pvi.EAN != null && pvi.EAN.Trim().ToLower() == trimmedValue) ||
                    (pvi.ISBN != null && pvi.ISBN.Trim().ToLower() == trimmedValue)
                ));

            if (entity == null)
            {
                _logger.Warning("FindByIdentifier: Không tìm thấy định danh sản phẩm với giá trị, IdentifierValue={0}", identifierValue);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (productVariantFullName, customerCategory) = GetProductVariantInfo(context, entity.ProductVariantId);

            _logger.Info("FindByIdentifier: Tìm thấy định danh sản phẩm, Id={0}, IdentifierValue={1}", entity.Id, identifierValue);
            return entity.ToDto(productVariantFullName, customerCategory);
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
        using var context = CreateNewContext();
        try
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
            {
                _logger.Warning("FindBySerialNumber: SerialNumber is null or empty");
                return null;
            }

            _logger.Debug("FindBySerialNumber: Tìm định danh sản phẩm, SerialNumber={0}", serialNumber);

            var entity = context.ProductVariantIdentifiers.FirstOrDefault(pvi =>
                pvi.IsActive &&
                pvi.SerialNumber != null &&
                pvi.SerialNumber.Trim().ToLower() == serialNumber.Trim().ToLower());

            if (entity == null)
            {
                _logger.Warning("FindBySerialNumber: Không tìm thấy định danh sản phẩm với SerialNumber, SerialNumber={0}", serialNumber);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (productVariantFullName, customerCategory) = GetProductVariantInfo(context, entity.ProductVariantId);

            _logger.Info("FindBySerialNumber: Tìm thấy định danh sản phẩm, Id={0}, SerialNumber={1}", entity.Id, serialNumber);
            return entity.ToDto(productVariantFullName, customerCategory);
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
        using var context = CreateNewContext();
        try
        {
            if (string.IsNullOrWhiteSpace(barcode))
            {
                _logger.Warning("FindByBarcode: PartNumber is null or empty");
                return null;
            }

            _logger.Debug("FindByBarcode: Tìm định danh sản phẩm, PartNumber={0}", barcode);

            var entity = context.ProductVariantIdentifiers.FirstOrDefault(pvi =>
                pvi.IsActive &&
                pvi.Barcode != null &&
                pvi.Barcode.Trim().ToLower() == barcode.Trim().ToLower());

            if (entity == null)
            {
                _logger.Warning("FindByBarcode: Không tìm thấy định danh sản phẩm với PartNumber, PartNumber={0}", barcode);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (productVariantFullName, customerCategory) = GetProductVariantInfo(context, entity.ProductVariantId);

            _logger.Info("FindByBarcode: Tìm thấy định danh sản phẩm, Id={0}, PartNumber={1}", entity.Id, barcode);
            return entity.ToDto(productVariantFullName, customerCategory);
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
        using var context = CreateNewContext();
        try
        {
            if (string.IsNullOrWhiteSpace(sku))
            {
                _logger.Warning("FindBySKU: SKU is null or empty");
                return null;
            }

            _logger.Debug("FindBySKU: Tìm định danh sản phẩm, SKU={0}", sku);

            var entity = context.ProductVariantIdentifiers.FirstOrDefault(pvi =>
                pvi.IsActive &&
                pvi.SKU != null &&
                pvi.SKU.Trim().ToLower() == sku.Trim().ToLower());

            if (entity == null)
            {
                _logger.Warning("FindBySKU: Không tìm thấy định danh sản phẩm với SKU, SKU={0}", sku);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (productVariantFullName, customerCategory) = GetProductVariantInfo(context, entity.ProductVariantId);

            _logger.Info("FindBySKU: Tìm thấy định danh sản phẩm, Id={0}, SKU={1}", entity.Id, sku);
            return entity.ToDto(productVariantFullName, customerCategory);
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
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStatus: Lấy danh sách định danh sản phẩm, Status={0}", status);

            var statusValue = (int)status;
            var entities = context.ProductVariantIdentifiers
                .Where(pvi => pvi.IsActive && pvi.Status == statusValue)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);

            _logger.Info("GetByStatus: Lấy được {0} định danh sản phẩm với Status={1}", entities.Count, status);
            return entities.ToDtoList(productVariantDict);
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
        var context = new VnsErp2025DataContext(_connectionString);
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu định danh sản phẩm, Id={0}, ProductVariantId={1}",
                dto.Id, dto.ProductVariantId);

            var existingEntity = dto.Id != Guid.Empty ? 
                context.ProductVariantIdentifiers.FirstOrDefault(pvi => pvi.Id == dto.Id) : null;

            ProductVariantIdentifier entity;
            if (existingEntity == null)
            {
                // Thêm mới
                entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                    dto.Id = entity.Id;
                }
                if (entity.CreatedDate == default(DateTime))
                {
                    entity.CreatedDate = DateTime.Now;
                    dto.CreatedDate = entity.CreatedDate;
                }
                context.ProductVariantIdentifiers.InsertOnSubmit(entity);
                _logger.Info("SaveOrUpdate: Thêm mới định danh sản phẩm, Id={0}", entity.Id);
            }
            else
            {
                // Cập nhật
                dto.ToEntity(existingEntity);
                existingEntity.UpdatedDate = DateTime.Now;
                entity = existingEntity;
                _logger.Info("SaveOrUpdate: Cập nhật định danh sản phẩm, Id={0}", entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu định danh sản phẩm thành công, Id={0}", entity.Id);
            
            // Load lại entity và fetch related data để convert sang DTO
            var savedEntity = context.ProductVariantIdentifiers.FirstOrDefault(pvi => pvi.Id == entity.Id);
            if (savedEntity == null)
                return null;

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var (productVariantFullName, customerCategory) = GetProductVariantInfo(context, savedEntity.ProductVariantId);

            return savedEntity.ToDto(productVariantFullName, customerCategory);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu định danh sản phẩm: {ex.Message}", ex);
            throw;
        }
        finally
        {
            context.Dispose();
        }
    }


    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa ProductVariantIdentifier theo ID
    /// Thực hiện soft delete bằng cách set IsActive = false
    /// </summary>
    /// <param name="id">ID của ProductVariantIdentifier cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    public bool Delete(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Xóa định danh sản phẩm, Id={0}", id);

            var entity = context.ProductVariantIdentifiers.FirstOrDefault(pvi => pvi.Id == id);

            if (entity == null)
            {
                _logger.Warning("Delete: Không tìm thấy định danh sản phẩm để xóa, Id={0}", id);
                return false;
            }

            // Soft delete: Set IsActive = false thay vì xóa thực sự
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.Now;

            context.SubmitChanges();
            _logger.Info("Delete: Xóa định danh sản phẩm thành công (soft delete), Id={0}", id);
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa định danh sản phẩm: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
