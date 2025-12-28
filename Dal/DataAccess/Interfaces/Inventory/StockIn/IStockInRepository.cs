using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataAccess.Implementations.Inventory.StockIn;

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
    /// Tạo mới Master entity (Create)
    /// </summary>
    /// <param name="master">Entity phiếu nhập kho (master) - Id phải là Guid.Empty</param>
    /// <returns>ID phiếu nhập kho đã tạo</returns>
    Task<Guid> CreateMasterAsync(StockInOutMaster master);

    /// <summary>
    /// Cập nhật Master entity (Update)
    /// Tạo context mới không có LoadOptions để tránh lỗi foreign key reference
    /// </summary>
    /// <param name="master">Entity phiếu nhập kho (master) - Id phải khác Guid.Empty</param>
    /// <returns>ID phiếu nhập kho đã cập nhật</returns>
    Task<Guid> UpdateMasterAsync(StockInOutMaster master);

    /// <summary>
    /// Lưu danh sách Detail entities cho một Master đã tồn tại
    /// </summary>
    /// <param name="masterId">ID phiếu nhập kho (master) đã tồn tại</param>
    /// <param name="details">Danh sách entity chi tiết phiếu nhập kho</param>
    /// <param name="deleteExisting">Có xóa các detail cũ không (mặc định: true)</param>
    Task SaveDetailsAsync(Guid masterId, List<StockInOutDetail> details, bool deleteExisting = true);

    /// <summary>
    /// Lấy số thứ tự tiếp theo cho phiếu nhập kho
    /// </summary>
    /// <param name="stockInDate">Ngày nhập kho</param>
    /// <param name="loaiNhapKho">Loại nhập kho (StockInOutType)</param>
    /// <returns>Số thứ tự tiếp theo (1-999)</returns>
    int GetNextSequenceNumber(DateTime stockInDate, int loaiNhapKho);

    /// <summary>
    /// Lấy danh sách chi tiết phiếu nhập/xuất kho theo MasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách StockInOutDetail entities</returns>
    List<StockInOutDetail> GetDetailsByMasterId(Guid stockInOutMasterId);

    /// <summary>
    /// Lấy thông tin master phiếu nhập/xuất kho theo ID với đầy đủ navigation properties
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>StockInOutMaster entity với navigation properties đã load</returns>
    StockInOutMaster GetMasterById(Guid stockInOutMasterId);

    /// <summary>
    /// Query lịch sử nhập xuất kho với filter
    /// </summary>
    /// <param name="query">Query criteria</param>
    /// <returns>Danh sách StockInOutMaster entities</returns>
    List<StockInOutMaster> QueryHistory(StockInHistoryQueryCriteria query);

    /// <summary>
    /// Đếm số lượng bản ghi theo query (không phân trang)
    /// </summary>
    /// <param name="query">Query criteria</param>
    /// <returns>Tổng số bản ghi</returns>
    int CountHistory(StockInHistoryQueryCriteria query);

    List<StockInOutMaster> GetPhieuNhapLapRap(int xuatLinhKienLapRapEnumValue);
}