using Dal.DataContext;
using System;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

public interface IStockInOutImageRepository
{
    /// <summary>
    /// Lưu hoặc cập nhật hình ảnh nhập/xuất kho
    /// </summary>
    /// <param name="stockInOutImage">Entity hình ảnh cần lưu</param>
    void SaveOrUpdate(StockInOutImage stockInOutImage);
}