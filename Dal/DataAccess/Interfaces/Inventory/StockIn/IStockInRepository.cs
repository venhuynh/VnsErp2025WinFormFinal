using DTO.Inventory;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dal.DataAccess.Interfaces.Inventory.StockIn;

/// <summary>
/// Interface cho Repository xử lý dữ liệu StockIn (Phiếu nhập kho)
/// </summary>
public interface IStockInRepository
{
    #region Retrieve
    /// <summary>
    /// Lấy thông tin master của phiếu nhập/xuất kho dựa trên ID duy nhất 
    /// và chuyển đổi sang đối tượng Data Transfer Object (DTO).
    /// </summary>
    /// <param name="stockInOutMasterId">ID duy nhất của phiếu nhập/xuất kho.</param>
    /// <returns>
    /// Một đối tượng <see cref="StockInOutMasterForUIDto"/> chứa thông tin master của phiếu nhập/xuất kho nếu tìm thấy; 
    /// ngược lại, trả về <c>null</c>.
    /// </returns>
    StockInOutMasterForUIDto GetStockInOutMasterForUIDtoById(Guid stockInOutMasterId);
    
    /// <summary>
    /// Lấy danh sách chi tiết của phiếu nhập/xuất kho dựa trên ID master
    /// và chuyển đổi sang danh sách Data Transfer Objects (DTOs).
    /// </summary>
    /// <param name="stockInOutMasterId">ID duy nhất của phiếu nhập/xuất kho.</param>
    /// <returns>
    /// Danh sách <see cref="StockInOutDetailForUIDto"/> chứa thông tin chi tiết của phiếu nhập/xuất kho.
    /// </returns>
    List<StockInOutDetailForUIDto> GetStockInOutDetailsByMasterId(Guid stockInOutMasterId);
    
    /// <summary>
    /// Lấy số thứ tự tiếp theo cho số phiếu nhập/xuất kho
    /// Dựa trên ngày và loại nhập/xuất kho để tìm số thứ tự cao nhất và trả về số tiếp theo
    /// </summary>
    /// <param name="stockInOutDate">Ngày của phiếu nhập/xuất kho</param>
    /// <param name="loaiNhapKhoInt">Loại nhập/xuất kho (số nguyên)</param>
    /// <returns>Số thứ tự tiếp theo (bắt đầu từ 1 nếu chưa có phiếu nào trong tháng/năm đó)</returns>
    int GetNextSequenceNumber(DateTime stockInOutDate, int loaiNhapKhoInt);
    
    #endregion
    
    #region Save/Update
    
    /// <summary>
    /// Lưu phiếu nhập/xuất kho mới (Insert)
    /// </summary>
    /// <param name="masterDto">Thông tin master phiếu nhập/xuất kho</param>
    /// <param name="detailDtos">Danh sách chi tiết phiếu nhập/xuất kho</param>
    /// <returns>ID của master đã được lưu</returns>
    Task<Guid> SaveAsync(StockInOutMasterForUIDto masterDto, List<StockInOutDetailForUIDto> detailDtos);
    
    /// <summary>
    /// Cập nhật phiếu nhập/xuất kho (Update)
    /// </summary>
    /// <param name="masterDto">Thông tin master phiếu nhập/xuất kho</param>
    /// <param name="detailDtos">Danh sách chi tiết phiếu nhập/xuất kho</param>
    /// <returns>ID của master đã được cập nhật</returns>
    Task<Guid> UpdateAsync(StockInOutMasterForUIDto masterDto, List<StockInOutDetailForUIDto> detailDtos);
    
    #endregion
}