using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataContext;
using DTO.MasterData.ProductService;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể ProductVariant (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IProductVariantRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy biến thể theo ID
    /// </summary>
    ProductVariantDto GetById(Guid id);

    /// <summary>
    /// Lấy biến thể theo ID (Async)
    /// </summary>
    Task<ProductVariantDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Lấy tất cả biến thể (Async)
    /// </summary>
    Task<List<ProductVariantSimpleDto>> GetAllAsync();

    /// <summary>
    /// Lấy tất cả biến thể với thông tin đầy đủ (Async)
    /// </summary>
    Task<List<ProductVariantDto>> GetAllWithDetailsAsync();

    /// <summary>
    /// Lấy danh sách biến thể theo ProductId
    /// </summary>
    List<ProductVariantDto> GetByProductId(Guid productId);

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu biến thể và giá trị thuộc tính
    /// </summary>
    Task<Guid> SaveAsync(ProductVariantDto variant, List<(Guid AttributeId, string Value)> attributeValues);

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật biến thể
    /// </summary>
    void SaveOrUpdate(ProductVariantDto variant);

    /// <summary>
    /// Cập nhật VariantFullName cho tất cả biến thể hiện có
    /// </summary>
    Task UpdateAllVariantFullNamesAsync();

    /// <summary>
    /// Cập nhật VariantFullName cho một biến thể cụ thể
    /// </summary>
    /// <param name="variantId">ID biến thể cần cập nhật</param>
    Task UpdateVariantFullNameAsync(Guid variantId);

    /// <summary>
    /// Chỉ cập nhật/xóa thumbnail image của biến thể, không ảnh hưởng đến các trường khác
    /// </summary>
    /// <param name="variantId">ID biến thể cần cập nhật</param>
    /// <param name="thumbnailImage">Thumbnail image dạng Binary (null để xóa)</param>
    /// <param name="thumbnailFileName">Tên file thumbnail (null để xóa)</param>
    /// <param name="thumbnailRelativePath">Đường dẫn tương đối thumbnail (null để xóa)</param>
    /// <param name="thumbnailFullPath">Đường dẫn đầy đủ thumbnail (null để xóa)</param>
    /// <param name="thumbnailStorageType">Loại storage (null để xóa)</param>
    /// <param name="thumbnailFileSize">Kích thước file (null để xóa)</param>
    /// <param name="thumbnailChecksum">Checksum file (null để xóa)</param>
    Task UpdateThumbnailOnlyAsync(
        Guid variantId,
        Binary thumbnailImage,
        string thumbnailFileName,
        string thumbnailRelativePath,
        string thumbnailFullPath,
        string thumbnailStorageType,
        long? thumbnailFileSize,
        string thumbnailChecksum);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa biến thể
    /// </summary>
    Task DeleteAsync(Guid id);

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra mã biến thể có trùng không
    /// </summary>
    bool IsVariantCodeExists(string variantCode, Guid? excludeId = null);

    #endregion

    #region ========== HELPER METHODS ==========

    /// <summary>
    /// Lấy danh sách giá trị thuộc tính của biến thể
    /// </summary>
    List<(Guid AttributeId, string AttributeName, string Value)> GetAttributeValues(Guid variantId);

    /// <summary>
    /// Lấy DataContext để sử dụng với LinqServerModeSource
    /// </summary>
    Task<VnsErp2025DataContext> GetDataContextAsync();

    /// <summary>
    /// Lấy queryable cho LinqInstantFeedbackSource (chỉ trả về Entity)
    /// </summary>
    IQueryable<ProductVariant> GetQueryableForInstantFeedback();

    /// <summary>
    /// Đếm tổng số biến thể
    /// </summary>
    int GetTotalCount();

    /// <summary>
    /// Lấy thông tin cho thuộc tính mới
    /// </summary>
    string GetForNewAttribute(Guid variantId);

    #endregion
}
