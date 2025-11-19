using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.DataAccess.Interfaces.Inventory.StockIn;

/// <summary>
/// Interface cho Repository xử lý dữ liệu StockIn (Phiếu nhập kho)
/// </summary>
public interface IStockInRepository
{
    /// <summary>
    /// Lưu phiếu nhập kho (master và detail) với transaction
    /// </summary>
    /// <param name="master">Entity phiếu nhập kho (master)</param>
    /// <param name="details">Danh sách entity chi tiết phiếu nhập kho</param>
    /// <returns>ID phiếu nhập kho đã lưu</returns>
    Task<Guid> SaveAsync(StockInOutMaster master, List<StockInOutDetail> details);
}