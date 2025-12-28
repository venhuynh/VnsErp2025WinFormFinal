using System;
using System.Collections.Generic;
using DTO.DeviceAssetManagement;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho Device
/// Quản lý các thao tác CRUD với bảng Device (Thiết bị)
/// </summary>
public interface IDeviceRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả Device
    /// </summary>
    /// <returns>Danh sách tất cả DeviceDto</returns>
    List<DeviceDto> GetAll();

    /// <summary>
    /// Lấy Device theo ID
    /// </summary>
    /// <param name="id">ID của Device</param>
    /// <returns>DeviceDto hoặc null</returns>
    DeviceDto GetById(Guid id);

    /// <summary>
    /// Lấy danh sách Device theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách DeviceDto</returns>
    List<DeviceDto> GetByStockInOutMasterId(Guid stockInOutMasterId);

    /// <summary>
    /// Lấy danh sách Device theo StockInOutDetailId
    /// </summary>
    /// <param name="stockInOutDetailId">ID chi tiết phiếu nhập/xuất kho</param>
    /// <returns>Danh sách DeviceDto</returns>
    List<DeviceDto> GetByStockInOutDetailId(Guid stockInOutDetailId);

    /// <summary>
    /// Tìm Device theo mã BarCode (SerialNumber, IMEI, MACAddress, AssetTag, hoặc LicenseKey)
    /// </summary>
    /// <param name="barCode">Mã BarCode cần tìm</param>
    /// <returns>DeviceDto nếu tìm thấy, null nếu không tìm thấy</returns>
    DeviceDto FindByBarCode(string barCode);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật Device
    /// </summary>
    /// <param name="dto">DeviceDto cần lưu</param>
    /// <returns>DeviceDto đã được lưu</returns>
    DeviceDto SaveOrUpdate(DeviceDto dto);

    #endregion
}
