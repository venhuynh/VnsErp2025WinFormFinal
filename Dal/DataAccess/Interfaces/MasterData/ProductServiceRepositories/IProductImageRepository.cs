using System;
using System.Collections.Generic;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access Layer cho ProductImage
/// </summary>
public interface IProductImageRepository
{
    #region Read Operations

    /// <summary>
    /// Lấy danh sách hình ảnh theo ProductId
    /// </summary>
    /// <param name="productId">Id của ProductService</param>
    /// <returns>Danh sách ProductImage</returns>
    List<ProductImage> GetByProductId(Guid productId);

    /// <summary>
    /// Lấy ProductImage theo Id
    /// </summary>
    /// <param name="imageId">Id của ProductImage</param>
    /// <returns>ProductImage hoặc null nếu không tìm thấy</returns>
    ProductImage GetById(Guid imageId);

    /// <summary>
    /// Lấy dữ liệu hình ảnh (byte array) theo imageId
    /// </summary>
    /// <param name="imageId">Id của ProductImage</param>
    /// <returns>Byte array của hình ảnh hoặc null</returns>
    byte[] GetImageData(Guid imageId);

    /// <summary>
    /// Lấy hình ảnh chính (primary) của ProductService
    /// </summary>
    /// <param name="productId">Id của ProductService</param>
    /// <returns>ProductImage chính hoặc null</returns>
    ProductImage GetPrimaryByProductId(Guid productId);

    /// <summary>
    /// Tìm kiếm ProductImage theo danh sách ProductIds
    /// </summary>
    /// <param name="productIds">Danh sách ProductService Ids</param>
    /// <returns>Danh sách ProductImage</returns>
    List<ProductImage> SearchByProductIds(List<Guid> productIds);

    #endregion

    #region Create/Update Operations

    /// <summary>
    /// Lưu hoặc cập nhật ProductImage
    /// </summary>
    /// <param name="productImage">ProductImage cần lưu hoặc cập nhật</param>
    void SaveOrUpdate(ProductImage productImage);

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa ProductImage (soft delete - đánh dấu IsDeleted = true)
    /// </summary>
    /// <param name="imageId">Id của ProductImage cần xóa</param>
    void Delete(Guid imageId);

    /// <summary>
    /// Xóa vĩnh viễn ProductImage khỏi database
    /// </summary>
    /// <param name="imageId">Id của ProductImage cần xóa vĩnh viễn</param>
    void DeletePermanent(Guid imageId);

    #endregion

    #region Utility Operations

    /// <summary>
    /// Kiểm tra ProductService có hình ảnh chính hay chưa
    /// </summary>
    /// <param name="productId">Id của ProductService</param>
    /// <returns>True nếu có hình ảnh chính</returns>
    bool HasPrimaryImage(Guid productId);

    /// <summary>
    /// Đặt hình ảnh làm hình chính cho ProductService
    /// </summary>
    /// <param name="imageId">Id của ProductImage cần đặt làm chính</param>
    void SetAsPrimary(Guid imageId);

    #endregion
}