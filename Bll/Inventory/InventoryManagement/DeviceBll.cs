using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Bll.Inventory.InventoryManagement
{
    public class DeviceBll
{
    #region Fields

    private IDeviceRepository _deviceRepository;
    private readonly ILogger _logger;
    private readonly object Lock = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public DeviceBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo DeviceRepository (lazy initialization)
    /// </summary>
    private IDeviceRepository GetDeviceRepository()
    {
        if (_deviceRepository == null)
        {
            lock (Lock)
            {
                if (_deviceRepository == null)
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

                        _deviceRepository = new DeviceRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo DeviceRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _deviceRepository;
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
        try
        {
            _logger.Debug("GetByStockInOutMasterId: Lấy danh sách thiết bị, StockInOutMasterId={0}", stockInOutMasterId);

            var devices = GetDeviceRepository().GetByStockInOutMasterId(stockInOutMasterId);

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
        try
        {
            _logger.Debug("GetById: Lấy thiết bị, Id={0}", id);

            var device = GetDeviceRepository().GetById(id);

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
        try
        {
            _logger.Debug("GetByStockInOutDetailId: Lấy danh sách thiết bị, StockInOutDetailId={0}", stockInOutDetailId);

            var devices = GetDeviceRepository().GetByStockInOutDetailId(stockInOutDetailId);

            _logger.Info("GetByStockInOutDetailId: Lấy được {0} thiết bị", devices.Count);
            return devices;
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
    /// <returns>Device entity nếu tìm thấy, null nếu không tìm thấy</returns>
    public Device FindByBarCode(string barCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(barCode))
            {
                _logger.Warning("FindByBarCode: BarCode is null or empty");
                return null;
            }

            _logger.Debug("FindByBarCode: Tìm thiết bị theo mã vạch, BarCode={0}", barCode);

            // Sử dụng DataContext trực tiếp để tìm kiếm
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            if (string.IsNullOrEmpty(globalConnectionString))
            {
                throw new InvalidOperationException(
                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
            }

            using var context = new VnsErp2025DataContext(globalConnectionString);
            var trimmedBarCode = barCode.Trim();

            // Tìm Device theo SerialNumber, IMEI, MACAddress, AssetTag, hoặc LicenseKey
            var device = context.Devices.FirstOrDefault(d =>
                (d.SerialNumber != null && d.SerialNumber.Trim().Equals(trimmedBarCode, StringComparison.OrdinalIgnoreCase)) ||
                (d.IMEI != null && d.IMEI.Trim().Equals(trimmedBarCode, StringComparison.OrdinalIgnoreCase)) ||
                (d.MACAddress != null && d.MACAddress.Trim().Equals(trimmedBarCode, StringComparison.OrdinalIgnoreCase)) ||
                (d.AssetTag != null && d.AssetTag.Trim().Equals(trimmedBarCode, StringComparison.OrdinalIgnoreCase)) ||
                (d.LicenseKey != null && d.LicenseKey.Trim().Equals(trimmedBarCode, StringComparison.OrdinalIgnoreCase))
            );

            if (device == null)
            {
                _logger.Warning("FindByBarCode: Không tìm thấy thiết bị với mã vạch, BarCode={0}", barCode);
            }
            else
            {
                _logger.Info("FindByBarCode: Tìm thấy thiết bị, DeviceId={0}, BarCode={1}", device.Id, barCode);
            }

            return device;
        }
        catch (Exception ex)
        {
            _logger.Error($"FindByBarCode: Lỗi tìm thiết bị theo mã vạch: {ex.Message}", ex);
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
        try
        {
            if (device == null)
                throw new ArgumentNullException(nameof(device));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu thiết bị, Id={0}, StockInOutDetailId={1}",
                device.Id, device.StockInOutDetailId);

            GetDeviceRepository().SaveOrUpdate(device);

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
}