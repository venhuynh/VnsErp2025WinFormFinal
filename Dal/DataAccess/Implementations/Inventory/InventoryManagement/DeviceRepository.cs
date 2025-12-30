using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Dal.DtoConverter;
using DTO.DeviceAssetManagement;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

public class DeviceRepository : IDeviceRepository
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
    /// Khởi tạo một instance mới của class StockInOutDetailRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public DeviceRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("DeviceRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<Device>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        loadOptions.LoadWith<ProductVariant>(v => v.UnitOfMeasure);
        // Load Warranties cho Device
        loadOptions.LoadWith<Device>(d => d.Warranties);
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Lấy dictionary thông tin ProductVariant cho danh sách Device
    /// </summary>
    private Dictionary<Guid, (string ProductVariantName, string ProductVariantCode, string UnitName)> GetProductVariantDict(VnsErp2025DataContext context, IEnumerable<Device> devices)
    {
        var productVariantIds = devices
            .Where(d => d.ProductVariantId != Guid.Empty)
            .Select(d => d.ProductVariantId)
            .Distinct()
            .ToList();

        if (!productVariantIds.Any())
            return new Dictionary<Guid, (string, string, string)>();

        var productVariants = context.ProductVariants
            .Where(pv => productVariantIds.Contains(pv.Id))
            .Select(pv => new
            {
                pv.Id,
                pv.VariantFullName,
                pv.VariantCode,
                UnitName = pv.UnitOfMeasure != null ? pv.UnitOfMeasure.Name : null
            })
            .ToList();

        return productVariants.ToDictionary(
            pv => pv.Id,
            pv => (pv.VariantFullName, pv.VariantCode, pv.UnitName)
        );
    }

    /// <summary>
    /// Lấy dictionary thông tin Warranty cho danh sách Device
    /// </summary>
    private Dictionary<Guid, (DateTime? WarrantyFrom, DateTime? WarrantyUntil, int? WarrantyType)> GetWarrantyDict(VnsErp2025DataContext context, IEnumerable<Device> devices)
    {
        var deviceIds = devices.Select(d => d.Id).Distinct().ToList();

        if (!deviceIds.Any())
            return new Dictionary<Guid, (DateTime?, DateTime?, int?)>();

        var warranties = context.Warranties
            .Where(w => deviceIds.Contains(w.DeviceId.Value) && w.IsActive)
            .GroupBy(w => w.DeviceId)
            .Select(g => new
            {
                DeviceId = g.Key,
                LatestWarranty = g.OrderByDescending(w => w.CreatedDate).FirstOrDefault()
            })
            .ToList();

        return warranties
            .Where(w => w.LatestWarranty != null)
            .ToDictionary(
                w => w.DeviceId.Value,
                w => (w.LatestWarranty.WarrantyFrom, w.LatestWarranty.WarrantyUntil, (int?)w.LatestWarranty.WarrantyType)
            );
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả Device
    /// </summary>
    /// <returns>Danh sách tất cả DeviceDto</returns>
    public List<DeviceDto> GetAll()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetAll: Lấy tất cả thiết bị");

            var entities = context.Devices.ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);
            var warrantyDict = GetWarrantyDict(context, entities);

            _logger.Info("GetAll: Lấy được {0} thiết bị", entities.Count);
            return entities.ToDtoList(productVariantDict, warrantyDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetAll: Lỗi lấy danh sách thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy Device theo ID
    /// </summary>
    /// <param name="id">ID của Device</param>
    /// <returns>DeviceDto hoặc null</returns>
    public DeviceDto GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy thiết bị, Id={0}", id);

            var entity = context.Devices.FirstOrDefault(d => d.Id == id);

            if (entity == null)
            {
                _logger.Warning("GetById: Không tìm thấy thiết bị, Id={0}", id);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            string productVariantName = null;
            string productVariantCode = null;
            string unitName = null;
            if (entity.ProductVariantId != Guid.Empty)
            {
                var productVariant = context.ProductVariants.FirstOrDefault(pv => pv.Id == entity.ProductVariantId);
                if (productVariant != null)
                {
                    productVariantName = productVariant.VariantFullName;
                    productVariantCode = productVariant.VariantCode;
                    if (productVariant.UnitOfMeasure != null)
                    {
                        unitName = productVariant.UnitOfMeasure.Name;
                    }
                }
            }

            DateTime? warrantyFrom = null;
            DateTime? warrantyUntil = null;
            int? warrantyType = null;
            var latestWarranty = context.Warranties
                .Where(w => w.DeviceId == entity.Id && w.IsActive)
                .OrderByDescending(w => w.CreatedDate)
                .FirstOrDefault();
            if (latestWarranty != null)
            {
                warrantyFrom = latestWarranty.WarrantyFrom;
                warrantyUntil = latestWarranty.WarrantyUntil;
                warrantyType = latestWarranty.WarrantyType;
            }

            _logger.Info("GetById: Lấy thiết bị thành công, Id={0}", id);
            return entity.ToDto(productVariantName, productVariantCode, unitName, warrantyFrom, warrantyUntil, warrantyType);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách Device theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách DeviceDto</returns>
    public List<DeviceDto> GetByStockInOutMasterId(Guid stockInOutMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStockInOutMasterId: Lấy danh sách thiết bị, StockInOutMasterId={0}", stockInOutMasterId);

            var entities = (from d in context.Devices
                          join detail in context.StockInOutDetails on d.StockInOutDetailId equals detail.Id
                          where detail.StockInOutMasterId == stockInOutMasterId
                          select d).ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);
            var warrantyDict = GetWarrantyDict(context, entities);

            _logger.Info("GetByStockInOutMasterId: Lấy được {0} thiết bị", entities.Count);
            return entities.ToDtoList(productVariantDict, warrantyDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStockInOutMasterId: Lỗi lấy danh sách thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách Device theo StockInOutDetailId
    /// </summary>
    /// <param name="stockInOutDetailId">ID chi tiết phiếu nhập/xuất kho</param>
    /// <returns>Danh sách DeviceDto</returns>
    public List<DeviceDto> GetByStockInOutDetailId(Guid stockInOutDetailId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStockInOutDetailId: Lấy danh sách thiết bị, StockInOutDetailId={0}", stockInOutDetailId);

            var entities = context.Devices
                .Where(d => d.StockInOutDetailId.HasValue && d.StockInOutDetailId.Value == stockInOutDetailId)
                .ToList();

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            var productVariantDict = GetProductVariantDict(context, entities);
            var warrantyDict = GetWarrantyDict(context, entities);

            _logger.Info("GetByStockInOutDetailId: Lấy được {0} thiết bị", entities.Count);
            return entities.ToDtoList(productVariantDict, warrantyDict);
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStockInOutDetailId: Lỗi lấy danh sách thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tìm Device theo mã BarCode (SerialNumber, IMEI, MACAddress, AssetTag, hoặc LicenseKey)
    /// </summary>
    /// <param name="barCode">Mã BarCode cần tìm</param>
    /// <returns>DeviceDto nếu tìm thấy, null nếu không tìm thấy</returns>
    public DeviceDto FindByBarCode(string barCode)
    {
        using var context = CreateNewContext();
        try
        {
            if (string.IsNullOrWhiteSpace(barCode))
            {
                _logger.Warning("FindByBarCode: BarCode is null or empty");
                return null;
            }

            _logger.Debug("FindByBarCode: Tìm thiết bị theo mã vạch, BarCode={0}", barCode);

            var trimmedBarCode = barCode.Trim().ToLower();

            // Tìm Device theo SerialNumber, IMEI, MACAddress, AssetTag, hoặc LicenseKey
            // Sử dụng ToLower() để so sánh không phân biệt hoa thường (LINQ to SQL hỗ trợ)
            var entity = context.Devices.FirstOrDefault(d =>
                (d.SerialNumber != null && d.SerialNumber.Trim().ToLower() == trimmedBarCode) ||
                (d.IMEI != null && d.IMEI.Trim().ToLower() == trimmedBarCode) ||
                (d.MACAddress != null && d.MACAddress.Trim().ToLower() == trimmedBarCode) ||
                (d.AssetTag != null && d.AssetTag.Trim().ToLower() == trimmedBarCode) ||
                (d.LicenseKey != null && d.LicenseKey.Trim().ToLower() == trimmedBarCode)
            );

            if (entity == null)
            {
                _logger.Warning("FindByBarCode: Không tìm thấy thiết bị với mã vạch, BarCode={0}", barCode);
                return null;
            }

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            string productVariantName = null;
            string productVariantCode = null;
            string unitName = null;
            if (entity.ProductVariantId != Guid.Empty)
            {
                var productVariant = context.ProductVariants.FirstOrDefault(pv => pv.Id == entity.ProductVariantId);
                if (productVariant != null)
                {
                    productVariantName = productVariant.VariantFullName;
                    productVariantCode = productVariant.VariantCode;
                    if (productVariant.UnitOfMeasure != null)
                    {
                        unitName = productVariant.UnitOfMeasure.Name;
                    }
                }
            }

            DateTime? warrantyFrom = null;
            DateTime? warrantyUntil = null;
            int? warrantyType = null;
            var latestWarranty = context.Warranties
                .Where(w => w.DeviceId == entity.Id && w.IsActive)
                .OrderByDescending(w => w.CreatedDate)
                .FirstOrDefault();
            if (latestWarranty != null)
            {
                warrantyFrom = latestWarranty.WarrantyFrom;
                warrantyUntil = latestWarranty.WarrantyUntil;
                warrantyType = latestWarranty.WarrantyType;
            }

            _logger.Info("FindByBarCode: Tìm thấy thiết bị, DeviceId={0}, BarCode={1}", entity.Id, barCode);
            return entity.ToDto(productVariantName, productVariantCode, unitName, warrantyFrom, warrantyUntil, warrantyType);
        }
        catch (Exception ex)
        {
            _logger.Error($"FindByBarCode: Lỗi tìm thiết bị theo mã vạch: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật Device
    /// </summary>
    /// <param name="dto">DeviceDto cần lưu</param>
    /// <returns>DeviceDto đã được lưu</returns>
    public DeviceDto SaveOrUpdate(DeviceDto dto)
    {
        using var context = CreateNewContext();
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu thiết bị, Id={0}, StockInOutDetailId={1}",
                dto.Id, dto.StockInOutDetailId);

            var existingEntity = dto.Id != Guid.Empty ? 
                context.Devices.FirstOrDefault(d => d.Id == dto.Id) : null;

            Device entity;
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
                context.Devices.InsertOnSubmit(entity);
                _logger.Info("SaveOrUpdate: Thêm mới thiết bị, Id={0}", entity.Id);
            }
            else
            {
                // Cập nhật
                dto.ToEntity(existingEntity);
                existingEntity.UpdatedDate = DateTime.Now;
                entity = existingEntity;
                _logger.Info("SaveOrUpdate: Cập nhật thiết bị, Id={0}", entity.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu thiết bị thành công, Id={0}", entity.Id);
            
            // Load lại entity và fetch related data để convert sang DTO
            var savedEntity = context.Devices.FirstOrDefault(d => d.Id == entity.Id);
            if (savedEntity == null)
                return null;

            // Fetch related data trước khi convert để tránh DataContext disposed errors
            string productVariantName = null;
            string productVariantCode = null;
            string unitName = null;
            if (savedEntity.ProductVariantId != Guid.Empty)
            {
                var productVariant = context.ProductVariants.FirstOrDefault(pv => pv.Id == savedEntity.ProductVariantId);
                if (productVariant != null)
                {
                    productVariantName = productVariant.VariantFullName;
                    productVariantCode = productVariant.VariantCode;
                    if (productVariant.UnitOfMeasure != null)
                    {
                        unitName = productVariant.UnitOfMeasure.Name;
                    }
                }
            }

            DateTime? warrantyFrom = null;
            DateTime? warrantyUntil = null;
            int? warrantyType = null;
            var latestWarranty = context.Warranties
                .Where(w => w.DeviceId == savedEntity.Id && w.IsActive)
                .OrderByDescending(w => w.CreatedDate)
                .FirstOrDefault();
            if (latestWarranty != null)
            {
                warrantyFrom = latestWarranty.WarrantyFrom;
                warrantyUntil = latestWarranty.WarrantyUntil;
                warrantyType = latestWarranty.WarrantyType;
            }

            return savedEntity.ToDto(productVariantName, productVariantCode, unitName, warrantyFrom, warrantyUntil, warrantyType);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}