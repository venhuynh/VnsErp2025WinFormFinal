using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

public interface IStockInOutMasterRepository
{
    /// <summary>
    /// Lấy VoucherNumber từ StockInOutMaster theo ID
    /// </summary>
    /// <param name="id">ID của StockInOutMaster</param>
    /// <returns>VoucherNumber hoặc null nếu không tìm thấy</returns>
    string GetVocherNumber(Guid id);

    /// <summary>
    /// Lấy danh sách StockInOutMaster theo danh sách ID
    /// </summary>
    /// <param name="masterIds">Danh sách ID của StockInOutMaster</param>
    /// <returns>Danh sách StockInOutMaster entities với navigation properties đã load</returns>
    List<StockInOutMaster> GetMastersByIds(List<Guid> masterIds);

    /// <summary>
    /// Lấy StockInOutMaster theo ID với tất cả navigation properties bao gồm cả details
    /// </summary>
    /// <param name="id">ID của StockInOutMaster</param>
    /// <returns>StockInOutMaster entity với tất cả navigation properties đã load hoặc null nếu không tìm thấy</returns>
    StockInOutMaster GetMasterByIdWithDetails(Guid id);
}