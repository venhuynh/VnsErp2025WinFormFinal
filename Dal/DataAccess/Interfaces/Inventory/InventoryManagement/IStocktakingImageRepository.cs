using System;
using System.Collections.Generic;
using DTO.Inventory.StockTakking;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho StocktakingImage
/// Quản lý các thao tác CRUD với bảng StocktakingImage (Hình ảnh kiểm kho)
/// </summary>
public interface IStocktakingImageRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả StocktakingImage
    /// </summary>
    /// <returns>Danh sách tất cả StocktakingImageDto</returns>
    List<StocktakingImageDto> GetAll();

    /// <summary>
    /// Lấy StocktakingImage theo ID
    /// </summary>
    /// <param name="id">ID của StocktakingImage</param>
    /// <returns>StocktakingImageDto hoặc null</returns>
    StocktakingImageDto GetById(Guid id);

    /// <summary>
    /// Lấy danh sách StocktakingImage theo StocktakingMasterId
    /// </summary>
    /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
    /// <returns>Danh sách StocktakingImageDto</returns>
    List<StocktakingImageDto> GetByStocktakingMasterId(Guid? stocktakingMasterId);

    /// <summary>
    /// Lấy danh sách StocktakingImage theo StocktakingDetailId
    /// </summary>
    /// <param name="stocktakingDetailId">ID chi tiết kiểm kho</param>
    /// <returns>Danh sách StocktakingImageDto</returns>
    List<StocktakingImageDto> GetByStocktakingDetailId(Guid? stocktakingDetailId);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật StocktakingImage
    /// </summary>
    /// <param name="dto">StocktakingImageDto cần lưu</param>
    /// <returns>StocktakingImageDto đã được lưu</returns>
    StocktakingImageDto SaveOrUpdate(StocktakingImageDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa StocktakingImage theo ID (hard delete)
    /// </summary>
    /// <param name="id">ID của StocktakingImage cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    bool Delete(Guid id);

    /// <summary>
    /// Xóa tất cả StocktakingImage theo StocktakingMasterId (hard delete)
    /// </summary>
    /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
    /// <returns>Số lượng bản ghi đã xóa</returns>
    int DeleteByStocktakingMasterId(Guid stocktakingMasterId);

    /// <summary>
    /// Xóa tất cả StocktakingImage theo StocktakingDetailId (hard delete)
    /// </summary>
    /// <param name="stocktakingDetailId">ID chi tiết kiểm kho</param>
    /// <returns>Số lượng bản ghi đã xóa</returns>
    int DeleteByStocktakingDetailId(Guid stocktakingDetailId);

    #endregion
}
