using System;
using System.Collections.Generic;
using DTO.MasterData.ProductService;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Interface cho Data Access Layer của ProductImage
/// </summary>
public interface IProductImageRepository
{
    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật ProductImage
    /// </summary>
    /// <param name="dto">ProductImageDto</param>
    void SaveOrUpdate(ProductImageDto dto);

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả hình ảnh
    /// </summary>
    /// <returns>Danh sách tất cả ProductImageDto</returns>
    List<ProductImageDto> GetAll();

    /// <summary>
    /// Lấy danh sách hình ảnh theo ProductId
    /// </summary>
    /// <param name="productId">Id của ProductService</param>
    /// <returns>Danh sách ProductImageDto</returns>
    List<ProductImageDto> GetByProductId(Guid productId);

    /// <summary>
    /// Lấy ProductImage theo Id
    /// </summary>
    /// <param name="imageId">Id của ProductImage</param>
    /// <returns>ProductImageDto hoặc null nếu không tìm thấy</returns>
    ProductImageDto GetById(Guid imageId);

    /// <summary>
    /// Lấy hình ảnh chính (primary) của ProductService
    /// </summary>
    /// <param name="productId">Id của ProductService</param>
    /// <returns>ProductImageDto chính hoặc null</returns>
    ProductImageDto GetPrimaryByProductId(Guid productId);

    /// <summary>
    /// Tìm kiếm ProductImage theo danh sách ProductIds
    /// </summary>
    /// <param name="productIds">Danh sách ProductService Ids</param>
    /// <returns>Danh sách ProductImageDto</returns>
    List<ProductImageDto> SearchByProductIds(List<Guid> productIds);

    #endregion

    #region ========== UPDATE OPERATIONS ==========
    // Update operations are handled by SaveOrUpdate method
    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa ProductImage
    /// </summary>
    /// <param name="imageId">Id của ProductImage cần xóa</param>
    void Delete(Guid imageId);

    #endregion

    #region ========== BUSINESS LOGIC METHODS ==========

    /// <summary>
    /// Đặt hình ảnh làm hình chính cho ProductService
    /// </summary>
    /// <param name="imageId">Id của ProductImage cần đặt làm chính</param>
    void SetAsPrimary(Guid imageId);

    #endregion
}
