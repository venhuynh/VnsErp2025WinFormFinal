using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DtoConverter;
using DTO.DeviceAssetManagement;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using Dal.Connection;


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

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả Device
    /// </summary>
    /// <returns>Danh sách tất cả DeviceDto</returns>
    public List<DeviceDto> GetAll()
    {
        try
        {
            _logger.Debug("GetAll: Lấy tất cả thiết bị");

            var dtos = GetDeviceRepository().GetAll();

            _logger.Info("GetAll: Lấy được {0} thiết bị", dtos.Count);
            return dtos;
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
        try
        {
            _logger.Debug("GetById: Lấy thiết bị, Id={0}", id);

            var dto = GetDeviceRepository().GetById(id);

            if (dto == null)
            {
                _logger.Warning("GetById: Không tìm thấy thiết bị, Id={0}", id);
            }
            else
            {
                _logger.Info("GetById: Lấy thiết bị thành công, Id={0}", id);
            }

            return dto;
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
        try
        {
            _logger.Debug("GetByStockInOutMasterId: Lấy danh sách thiết bị, StockInOutMasterId={0}", stockInOutMasterId);

            var dtos = GetDeviceRepository().GetByStockInOutMasterId(stockInOutMasterId);

            _logger.Info("GetByStockInOutMasterId: Lấy được {0} thiết bị", dtos.Count);
            return dtos;
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
        try
        {
            _logger.Debug("GetByStockInOutDetailId: Lấy danh sách thiết bị, StockInOutDetailId={0}", stockInOutDetailId);

            var dtos = GetDeviceRepository().GetByStockInOutDetailId(stockInOutDetailId);

            _logger.Info("GetByStockInOutDetailId: Lấy được {0} thiết bị", dtos.Count);
            return dtos;
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
        try
        {
            if (string.IsNullOrWhiteSpace(barCode))
            {
                _logger.Warning("FindByBarCode: BarCode is null or empty");
                return null;
            }

            _logger.Debug("FindByBarCode: Tìm thiết bị theo mã vạch, BarCode={0}", barCode);

            // Sử dụng Repository để tìm kiếm (BLL -> Repository)
            var dto = GetDeviceRepository().FindByBarCode(barCode);

            if (dto == null)
            {
                _logger.Warning("FindByBarCode: Không tìm thấy thiết bị với mã vạch, BarCode={0}", barCode);
            }
            else
            {
                _logger.Info("FindByBarCode: Tìm thấy thiết bị, DeviceId={0}, BarCode={1}", dto.Id, barCode);
            }

            return dto;
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
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu thiết bị, Id={0}, StockInOutDetailId={1}",
                dto.Id, dto.StockInOutDetailId);

            var result = GetDeviceRepository().SaveOrUpdate(dto);

            _logger.Info("SaveOrUpdate: Lưu thiết bị thành công, Id={0}", result?.Id ?? dto.Id);
            return result;
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