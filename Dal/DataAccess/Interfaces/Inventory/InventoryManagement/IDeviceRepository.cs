using System;
using System.Collections.Generic;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

public interface IDeviceRepository
{
    /// <summary>
    /// Lấy danh sách Device theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách Device entities</returns>
    List<Device> GetByStockInOutMasterId(Guid stockInOutMasterId);

    /// <summary>
    /// Lấy Device theo ID
    /// </summary>
    /// <param name="id">ID của Device</param>
    /// <returns>Device entity hoặc null</returns>
    Device GetById(Guid id);

    /// <summary>
    /// Lấy danh sách Device theo StockInOutDetailId
    /// </summary>
    /// <param name="stockInOutDetailId">ID chi tiết phiếu nhập/xuất kho</param>
    /// <returns>Danh sách Device entities</returns>
    List<Device> GetByStockInOutDetailId(Guid stockInOutDetailId);

    /// <summary>
    /// Lấy tất cả Device
    /// </summary>
    /// <returns>Danh sách tất cả Device entities</returns>
    List<Device> GetAll();

    /// <summary>
    /// Lưu hoặc cập nhật Device
    /// </summary>
    /// <param name="device">Device entity cần lưu</param>
    void SaveOrUpdate(Device device);
}