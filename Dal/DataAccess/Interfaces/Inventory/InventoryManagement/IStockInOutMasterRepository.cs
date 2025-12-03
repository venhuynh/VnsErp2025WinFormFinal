using System;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

public interface IStockInOutMasterRepository
{
    /// <summary>
    /// Lấy VocherNumber từ StockInOutMaster theo ID
    /// </summary>
    /// <param name="id">ID của StockInOutMaster</param>
    /// <returns>VocherNumber hoặc null nếu không tìm thấy</returns>
    string GetVocherNumber(Guid id);
}