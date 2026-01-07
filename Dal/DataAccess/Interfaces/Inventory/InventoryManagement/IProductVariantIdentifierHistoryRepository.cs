using System;
using System.Collections.Generic;
using DTO.Inventory.InventoryManagement;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho ProductVariantIdentifierHistory
/// Quản lý các thao tác CRUD với bảng ProductVariantIdentifierHistory (Lịch sử thay đổi định danh biến thể sản phẩm)
/// </summary>
public interface IProductVariantIdentifierHistoryRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả ProductVariantIdentifierHistory
    /// </summary>
    /// <returns>Danh sách tất cả ProductVariantIdentifierHistoryDto</returns>
    List<ProductVariantIdentifierHistoryDto> GetAll();

    /// <summary>
    /// Lấy ProductVariantIdentifierHistory theo ID
    /// </summary>
    /// <param name="id">ID của ProductVariantIdentifierHistory</param>
    /// <returns>ProductVariantIdentifierHistoryDto hoặc null</returns>
    ProductVariantIdentifierHistoryDto GetById(Guid id);

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifierHistory theo ProductVariantIdentifierId
    /// </summary>
    /// <param name="productVariantIdentifierId">ID định danh sản phẩm</param>
    /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
    List<ProductVariantIdentifierHistoryDto> GetByProductVariantIdentifierId(Guid productVariantIdentifierId);

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifierHistory theo ProductVariantId
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
    List<ProductVariantIdentifierHistoryDto> GetByProductVariantId(Guid productVariantId);

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifierHistory theo loại thay đổi (ChangeType)
    /// </summary>
    /// <param name="changeType">Loại thay đổi</param>
    /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
    List<ProductVariantIdentifierHistoryDto> GetByChangeType(int changeType);

    /// <summary>
    /// Lấy danh sách ProductVariantIdentifierHistory theo loại thay đổi (ChangeTypeEnum)
    /// </summary>
    /// <param name="changeTypeEnum">Loại thay đổi dưới dạng enum</param>
    /// <returns>Danh sách ProductVariantIdentifierHistoryDto</returns>
    List<ProductVariantIdentifierHistoryDto> GetByChangeTypeEnum(ProductVariantIdentifierHistoryChangeTypeEnum changeTypeEnum);

    #endregion

    #region ========== CREATE/UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật ProductVariantIdentifierHistory
    /// </summary>
    /// <param name="dto">ProductVariantIdentifierHistoryDto cần lưu</param>
    /// <returns>ProductVariantIdentifierHistoryDto đã được lưu</returns>
    ProductVariantIdentifierHistoryDto SaveOrUpdate(ProductVariantIdentifierHistoryDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa ProductVariantIdentifierHistory theo ID
    /// </summary>
    /// <param name="id">ID của ProductVariantIdentifierHistory cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy</returns>
    bool Delete(Guid id);

    #endregion
}
