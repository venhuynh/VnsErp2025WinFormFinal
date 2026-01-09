using System;
using System.Collections.Generic;
using DTO.Inventory.StockTakking;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho StocktakingMaster
/// Quản lý các thao tác CRUD với bảng StocktakingMaster (Phiếu kiểm kho)
/// </summary>
public interface IStocktakingMasterRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả StocktakingMaster
    /// </summary>
    /// <returns>Danh sách tất cả StocktakingMasterDto</returns>
    List<StocktakingMasterDto> GetAll();

    /// <summary>
    /// Lấy StocktakingMaster theo ID
    /// </summary>
    /// <param name="id">ID của StocktakingMaster</param>
    /// <returns>StocktakingMasterDto hoặc null</returns>
    StocktakingMasterDto GetById(Guid id);

    /// <summary>
    /// Lấy danh sách StocktakingMaster theo WarehouseId
    /// </summary>
    /// <param name="warehouseId">ID kho</param>
    /// <returns>Danh sách StocktakingMasterDto</returns>
    List<StocktakingMasterDto> GetByWarehouseId(Guid warehouseId);

    /// <summary>
    /// Lấy danh sách StocktakingMaster theo trạng thái
    /// </summary>
    /// <param name="stocktakingStatus">Trạng thái kiểm kho</param>
    /// <returns>Danh sách StocktakingMasterDto</returns>
    List<StocktakingMasterDto> GetByStatus(int stocktakingStatus);

    /// <summary>
    /// Lấy danh sách StocktakingMaster theo loại
    /// </summary>
    /// <param name="stocktakingType">Loại kiểm kho</param>
    /// <returns>Danh sách StocktakingMasterDto</returns>
    List<StocktakingMasterDto> GetByType(int stocktakingType);

    /// <summary>
    /// Tìm StocktakingMaster theo số phiếu
    /// </summary>
    /// <param name="voucherNumber">Số phiếu kiểm kho</param>
    /// <returns>StocktakingMasterDto nếu tìm thấy, null nếu không tìm thấy</returns>
    StocktakingMasterDto FindByVoucherNumber(string voucherNumber);

    /// <summary>
    /// Lấy số thứ tự tiếp theo cho số phiếu kiểm kho
    /// Dựa trên năm để tìm số thứ tự cao nhất và trả về số tiếp theo
    /// </summary>
    /// <param name="stocktakingDate">Ngày của phiếu kiểm kho</param>
    /// <returns>Số thứ tự tiếp theo (bắt đầu từ 1 nếu chưa có phiếu nào trong năm đó)</returns>
    int GetNextSequenceNumber(DateTime stocktakingDate);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật StocktakingMaster
    /// </summary>
    /// <param name="dto">StocktakingMasterDto cần lưu</param>
    /// <returns>StocktakingMasterDto đã được lưu</returns>
    StocktakingMasterDto SaveOrUpdate(StocktakingMasterDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa StocktakingMaster theo ID (soft delete)
    /// </summary>
    /// <param name="id">ID của StocktakingMaster cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    bool Delete(Guid id);

    #endregion
}
