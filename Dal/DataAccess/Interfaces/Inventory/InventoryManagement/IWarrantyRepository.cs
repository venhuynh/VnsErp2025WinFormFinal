using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

public interface IWarrantyRepository
{
    /// <summary>
    /// Lấy danh sách bảo hành theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách Warranty entities</returns>
    List<Warranty> GetByStockInOutMasterId(Guid stockInOutMasterId);

    /// <summary>
    /// Lưu hoặc cập nhật bảo hành
    /// </summary>
    /// <param name="warranty">Entity bảo hành cần lưu</param>
    void SaveOrUpdate(Warranty warranty);

    /// <summary>
    /// Xóa bảo hành
    /// </summary>
    /// <param name="warrantyId">ID bảo hành cần xóa</param>
    void Delete(Guid warrantyId);
}