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

    /// <summary>
    /// Lấy số thứ tự tiếp theo cho phiếu nhập kho
    /// </summary>
    /// <param name="stockInDate">Ngày nhập kho</param>
    /// <param name="loaiNhapKho">Loại nhập kho (StockInOutType)</param>
    /// <returns>Số thứ tự tiếp theo (1-999)</returns>
    int GetNextSequenceNumber(DateTime stockInDate, int loaiNhapKho);
}