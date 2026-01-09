using System;
using System.Collections.Generic;
using DTO.Inventory.StockTakking;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho StocktakingAdjustment
/// Quản lý các thao tác CRUD với bảng StocktakingAdjustment (Điều chỉnh kho sau kiểm kho)
/// </summary>
public interface IStocktakingAdjustmentRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả StocktakingAdjustment
    /// </summary>
    /// <returns>Danh sách tất cả StocktakingAdjustmentDto</returns>
    List<StocktakingAdjustmentDto> GetAll();

    /// <summary>
    /// Lấy StocktakingAdjustment theo ID
    /// </summary>
    /// <param name="id">ID của StocktakingAdjustment</param>
    /// <returns>StocktakingAdjustmentDto hoặc null</returns>
    StocktakingAdjustmentDto GetById(Guid id);

    /// <summary>
    /// Lấy danh sách StocktakingAdjustment theo StocktakingMasterId
    /// </summary>
    /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
    /// <returns>Danh sách StocktakingAdjustmentDto</returns>
    List<StocktakingAdjustmentDto> GetByStocktakingMasterId(Guid stocktakingMasterId);

    /// <summary>
    /// Lấy danh sách StocktakingAdjustment theo StocktakingDetailId
    /// </summary>
    /// <param name="stocktakingDetailId">ID chi tiết kiểm kho</param>
    /// <returns>Danh sách StocktakingAdjustmentDto</returns>
    List<StocktakingAdjustmentDto> GetByStocktakingDetailId(Guid stocktakingDetailId);

    /// <summary>
    /// Lấy danh sách StocktakingAdjustment chưa được áp dụng
    /// </summary>
    /// <param name="stocktakingMasterId">ID phiếu kiểm kho (tùy chọn)</param>
    /// <returns>Danh sách StocktakingAdjustmentDto</returns>
    List<StocktakingAdjustmentDto> GetUnapplied(Guid? stocktakingMasterId = null);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật StocktakingAdjustment
    /// </summary>
    /// <param name="dto">StocktakingAdjustmentDto cần lưu</param>
    /// <returns>StocktakingAdjustmentDto đã được lưu</returns>
    StocktakingAdjustmentDto SaveOrUpdate(StocktakingAdjustmentDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa StocktakingAdjustment theo ID (soft delete)
    /// </summary>
    /// <param name="id">ID của StocktakingAdjustment cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    bool Delete(Guid id);

    #endregion
}
