using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

public interface IStockInOutImageRepository
{
    /// <summary>
    /// Lưu hoặc cập nhật hình ảnh nhập/xuất kho
    /// </summary>
    /// <param name="stockInOutImage">Entity hình ảnh cần lưu</param>
    void SaveOrUpdate(StockInOutImage stockInOutImage);

    /// <summary>
    /// Lấy hình ảnh theo ID
    /// </summary>
    /// <param name="id">ID hình ảnh</param>
    /// <returns>StockInOutImage hoặc null</returns>
    StockInOutImage GetById(Guid id);

    /// <summary>
    /// Xóa hình ảnh theo ID
    /// </summary>
    /// <param name="id">ID hình ảnh cần xóa</param>
    void Delete(Guid id);

    /// <summary>
    /// Lấy danh sách hình ảnh theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách hình ảnh</returns>
    List<StockInOutImage> GetByStockInOutMasterId(Guid stockInOutMasterId);
}