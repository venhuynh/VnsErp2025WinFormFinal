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
    public class WarrantyBll
{
    #region Fields

    private IWarrantyRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public WarrantyBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IWarrantyRepository GetDataAccess()
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

                        _dataAccess = new WarrantyRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo WarrantyRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _dataAccess;
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy danh sách bảo hành theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách Warranty entities</returns>
    public List<Warranty> GetByStockInOutMasterId(Guid stockInOutMasterId)
    {
        try
        {
            _logger.Debug("GetByStockInOutMasterId: Lấy danh sách bảo hành, StockInOutMasterId={0}", stockInOutMasterId);
            
            var warranties = GetDataAccess().GetByStockInOutMasterId(stockInOutMasterId);
            
            _logger.Info("GetByStockInOutMasterId: Lấy được {0} bảo hành", warranties.Count);
            return warranties;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStockInOutMasterId: Lỗi lấy danh sách bảo hành: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Query danh sách bảo hành với filter theo từ khóa và khoảng thời gian
    /// </summary>
    /// <param name="fromDate">Từ ngày (nullable)</param>
    /// <param name="toDate">Đến ngày (nullable)</param>
    /// <param name="keyword">Từ khóa tìm kiếm (tìm trong Device properties: SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey, ProductVariantName, CustomerName, Notes)</param>
    /// <returns>Danh sách Warranty entities</returns>
    public List<Warranty> Query(DateTime? fromDate, DateTime? toDate, string keyword)
    {
        try
        {
            _logger.Debug("Query: Query danh sách bảo hành, FromDate={0}, ToDate={1}, Keyword={2}", 
                fromDate, toDate, keyword);
            
            var warranties = GetDataAccess().Query(fromDate, toDate, keyword);
            
            _logger.Info("Query: Lấy được {0} bảo hành", warranties.Count);
            return warranties;
        }
        catch (Exception ex)
        {
            _logger.Error($"Query: Lỗi query danh sách bảo hành: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tìm Warranty theo DeviceId
    /// </summary>
    /// <param name="deviceId">ID của Device cần tìm</param>
    /// <returns>Warranty entity nếu tìm thấy, null nếu không tìm thấy</returns>
    public Warranty FindByDeviceId(Guid deviceId)
    {
        try
        {
            if (deviceId == Guid.Empty)
            {
                _logger.Warning("FindByDeviceId: DeviceId is empty");
                return null;
            }

            _logger.Debug("FindByDeviceId: Tìm bảo hành theo DeviceId, DeviceId={0}", deviceId);

            var warranty = GetDataAccess().FindByDeviceId(deviceId);

            if (warranty == null)
            {
                _logger.Warning("FindByDeviceId: Không tìm thấy bảo hành với DeviceId, DeviceId={0}", deviceId);
            }
            else
            {
                _logger.Info("FindByDeviceId: Tìm thấy bảo hành, WarrantyId={0}, DeviceId={1}", warranty.Id, deviceId);
            }

            return warranty;
        }
        catch (Exception ex)
        {
            _logger.Error($"FindByDeviceId: Lỗi tìm bảo hành theo DeviceId: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tìm Warranty theo thông tin Device (SerialNumber, IMEI, MACAddress, AssetTag, LicenseKey)
    /// </summary>
    /// <param name="deviceInfo">Thông tin Device cần tìm (SerialNumber, IMEI, MACAddress, AssetTag, hoặc LicenseKey)</param>
    /// <returns>Warranty entity nếu tìm thấy, null nếu không tìm thấy</returns>
    public Warranty FindByDeviceInfo(string deviceInfo)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(deviceInfo))
            {
                _logger.Warning("FindByDeviceInfo: DeviceInfo is null or empty");
                return null;
            }

            _logger.Debug("FindByDeviceInfo: Tìm bảo hành theo DeviceInfo, DeviceInfo={0}", deviceInfo);

            var warranty = GetDataAccess().FindByDeviceInfo(deviceInfo);

            if (warranty == null)
            {
                _logger.Warning("FindByDeviceInfo: Không tìm thấy bảo hành với DeviceInfo, DeviceInfo={0}", deviceInfo);
            }
            else
            {
                _logger.Info("FindByDeviceInfo: Tìm thấy bảo hành, WarrantyId={0}, DeviceId={1}, DeviceInfo={2}", 
                    warranty.Id, warranty.DeviceId, deviceInfo);
            }

            return warranty;
        }
        catch (Exception ex)
        {
            _logger.Error($"FindByDeviceInfo: Lỗi tìm bảo hành theo DeviceInfo: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu hoặc cập nhật bảo hành
    /// </summary>
    /// <param name="warranty">Entity bảo hành cần lưu</param>
    public void SaveOrUpdate(Warranty warranty)
    {
        try
        {
            if (warranty == null)
                throw new ArgumentNullException(nameof(warranty));

            _logger.Debug("SaveOrUpdate: Bắt đầu lưu bảo hành, Id={0}, DeviceId={1}", 
                warranty.Id, warranty.DeviceId);

            GetDataAccess().SaveOrUpdate(warranty);

            _logger.Info("SaveOrUpdate: Lưu bảo hành thành công, Id={0}", warranty.Id);
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu bảo hành: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa bảo hành
    /// </summary>
    /// <param name="warrantyId">ID bảo hành cần xóa</param>
    public void Delete(Guid warrantyId)
    {
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa bảo hành, Id={0}", warrantyId);

            GetDataAccess().Delete(warrantyId);

            _logger.Info("Delete: Xóa bảo hành thành công, Id={0}", warrantyId);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa bảo hành: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
}