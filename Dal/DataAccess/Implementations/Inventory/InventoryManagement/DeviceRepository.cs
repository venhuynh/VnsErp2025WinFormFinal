using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
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
        loadOptions.LoadWith<StockInOutDetail>(d => d.StockInOutMaster);
        loadOptions.LoadWith<StockInOutMaster>(m => m.CompanyBranch);
        loadOptions.LoadWith<StockInOutMaster>(m => m.BusinessPartnerSite);
        loadOptions.LoadWith<BusinessPartnerSite>(s => s.BusinessPartner);
        loadOptions.LoadWith<StockInOutDetail>(d => d.ProductVariant);
        loadOptions.LoadWith<ProductVariant>(v => v.ProductService);
        loadOptions.LoadWith<ProductVariant>(v => v.UnitOfMeasure);
        loadOptions.LoadWith<Device>(d => d.ProductVariant);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy danh sách Device theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách Device entities</returns>
    public List<Device> GetByStockInOutMasterId(Guid stockInOutMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStockInOutMasterId: Lấy danh sách thiết bị, StockInOutMasterId={0}", stockInOutMasterId);

            var devices = (from d in context.Devices
                          join detail in context.StockInOutDetails on d.StockInOutDetailId equals detail.Id
                          where detail.StockInOutMasterId == stockInOutMasterId
                          select d).ToList();

            _logger.Info("GetByStockInOutMasterId: Lấy được {0} thiết bị", devices.Count);
            return devices;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStockInOutMasterId: Lỗi lấy danh sách thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy Device theo ID
    /// </summary>
    /// <param name="id">ID của Device</param>
    /// <returns>Device entity hoặc null</returns>
    public Device GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy thiết bị, Id={0}", id);

            var device = context.Devices.FirstOrDefault(d => d.Id == id);

            if (device == null)
            {
                _logger.Warning("GetById: Không tìm thấy thiết bị, Id={0}", id);
            }
            else
            {
                _logger.Info("GetById: Lấy thiết bị thành công, Id={0}", id);
            }

            return device;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách Device theo StockInOutDetailId
    /// </summary>
    /// <param name="stockInOutDetailId">ID chi tiết phiếu nhập/xuất kho</param>
    /// <returns>Danh sách Device entities</returns>
    public List<Device> GetByStockInOutDetailId(Guid stockInOutDetailId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStockInOutDetailId: Lấy danh sách thiết bị, StockInOutDetailId={0}", stockInOutDetailId);

            var devices = context.Devices
                .Where(d => d.StockInOutDetailId.HasValue && d.StockInOutDetailId.Value == stockInOutDetailId)
                .ToList();

            _logger.Info("GetByStockInOutDetailId: Lấy được {0} thiết bị", devices.Count);
            return devices;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStockInOutDetailId: Lỗi lấy danh sách thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu hoặc cập nhật Device
    /// </summary>
    /// <param name="device">Device entity cần lưu</param>
    public void SaveOrUpdate(Device device)
    {
        using var context = CreateNewContext();
        try
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu thiết bị, Id={0}, StockInOutDetailId={1}",
                device.Id, device.StockInOutDetailId);

            var existingDevice = context.Devices.FirstOrDefault(d => d.Id == device.Id);

            if (existingDevice == null)
            {
                // Thêm mới
                if (device.Id == Guid.Empty)
                {
                    device.Id = Guid.NewGuid();
                }
                if (device.CreatedDate == default(DateTime))
                {
                    device.CreatedDate = DateTime.Now;
                }
                context.Devices.InsertOnSubmit(device);
                _logger.Info("SaveOrUpdate: Thêm mới thiết bị, Id={0}", device.Id);
            }
            else
            {
                // Cập nhật
                existingDevice.ProductVariantId = device.ProductVariantId;
                existingDevice.StockInOutDetailId = device.StockInOutDetailId;
                existingDevice.WarrantyId = device.WarrantyId;
                existingDevice.SerialNumber = device.SerialNumber;
                existingDevice.MACAddress = device.MACAddress;
                existingDevice.IMEI = device.IMEI;
                existingDevice.AssetTag = device.AssetTag;
                existingDevice.LicenseKey = device.LicenseKey;
                existingDevice.HostName = device.HostName;
                existingDevice.IPAddress = device.IPAddress;
                existingDevice.Status = device.Status;
                existingDevice.DeviceType = device.DeviceType;
                existingDevice.Notes = device.Notes;
                existingDevice.IsActive = device.IsActive;
                existingDevice.UpdatedDate = DateTime.Now;
                existingDevice.UpdatedBy = device.UpdatedBy;
                _logger.Info("SaveOrUpdate: Cập nhật thiết bị, Id={0}", device.Id);
            }

            context.SubmitChanges();
            _logger.Info("SaveOrUpdate: Lưu thiết bị thành công, Id={0}", device.Id);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu thiết bị: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}