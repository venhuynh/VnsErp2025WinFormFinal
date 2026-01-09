using System;
using System.Collections.Generic;
using DTO.Inventory.StockTakking;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho StocktakingDetail
/// Quản lý các thao tác CRUD với bảng StocktakingDetail (Chi tiết kiểm kho)
/// </summary>
public interface IStocktakingDetailRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả StocktakingDetail
    /// </summary>
    /// <returns>Danh sách tất cả StocktakingDetailDto</returns>
    List<StocktakingDetailDto> GetAll();

    /// <summary>
    /// Lấy StocktakingDetail theo ID
    /// </summary>
    /// <param name="id">ID của StocktakingDetail</param>
    /// <returns>StocktakingDetailDto hoặc null</returns>
    StocktakingDetailDto GetById(Guid id);

    /// <summary>
    /// Lấy danh sách StocktakingDetail theo StocktakingMasterId
    /// </summary>
    /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
    /// <returns>Danh sách StocktakingDetailDto</returns>
    List<StocktakingDetailDto> GetByStocktakingMasterId(Guid stocktakingMasterId);

    /// <summary>
    /// Lấy danh sách StocktakingDetail theo ProductVariantId
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách StocktakingDetailDto</returns>
    List<StocktakingDetailDto> GetByProductVariantId(Guid productVariantId);

    /// <summary>
    /// Lấy danh sách StocktakingDetail chưa được kiểm đếm
    /// </summary>
    /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
    /// <returns>Danh sách StocktakingDetailDto</returns>
    List<StocktakingDetailDto> GetUncountedByStocktakingMasterId(Guid stocktakingMasterId);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật StocktakingDetail
    /// </summary>
    /// <param name="dto">StocktakingDetailDto cần lưu</param>
    /// <returns>StocktakingDetailDto đã được lưu</returns>
    StocktakingDetailDto SaveOrUpdate(StocktakingDetailDto dto);

    /// <summary>
    /// Lưu hoặc cập nhật danh sách StocktakingDetail
    /// </summary>
    /// <param name="dtos">Danh sách StocktakingDetailDto cần lưu</param>
    /// <returns>Danh sách StocktakingDetailDto đã được lưu</returns>
    List<StocktakingDetailDto> SaveOrUpdateList(List<StocktakingDetailDto> dtos);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa StocktakingDetail theo ID (soft delete)
    /// </summary>
    /// <param name="id">ID của StocktakingDetail cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    bool Delete(Guid id);

    /// <summary>
    /// Xóa tất cả StocktakingDetail theo StocktakingMasterId (soft delete)
    /// </summary>
    /// <param name="stocktakingMasterId">ID phiếu kiểm kho</param>
    /// <returns>Số lượng bản ghi đã xóa</returns>
    int DeleteByStocktakingMasterId(Guid stocktakingMasterId);

    #endregion
}
