using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể ProductVariant (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IProductVariantRepository
{
    #region Read Operations

    /// <summary>
    /// Lấy biến thể theo ID
    /// </summary>
    ProductVariant GetById(Guid id);

    /// <summary>
    /// Lấy biến thể theo ID (Async)
    /// </summary>
    Task<ProductVariant> GetByIdAsync(Guid id);

    /// <summary>
    /// Lấy tất cả biến thể
    /// </summary>
    List<ProductVariant> GetAll();

    /// <summary>
    /// Lấy tất cả biến thể (Async)
    /// </summary>
    Task<List<ProductVariant>> GetAllAsync();

    /// <summary>
    /// Lấy tất cả biến thể với thông tin đầy đủ (Async)
    /// </summary>
    Task<List<ProductVariant>> GetAllWithDetailsAsync();

    /// <summary>
    /// Lấy danh sách biến thể theo ProductId
    /// </summary>
    List<ProductVariant> GetByProductId(Guid productId);

    /// <summary>
    /// Lấy danh sách biến thể theo ProductId (Async)
    /// </summary>
    Task<List<ProductVariant>> GetByProductIdAsync(Guid productId);

    /// <summary>
    /// Lấy biến thể theo mã
    /// </summary>
    ProductVariant GetByVariantCode(string variantCode);

    /// <summary>
    /// Lấy biến thể theo mã (Async)
    /// </summary>
    Task<ProductVariant> GetByVariantCodeAsync(string variantCode);

    /// <summary>
    /// Lấy biến thể theo trạng thái hoạt động
    /// </summary>
    List<ProductVariant> GetByStatus(bool isActive);

    /// <summary>
    /// Lấy biến thể theo trạng thái hoạt động (Async)
    /// </summary>
    Task<List<ProductVariant>> GetByStatusAsync(bool isActive);

    #endregion

    #region Create/Update Operations

    /// <summary>
    /// Lưu hoặc cập nhật biến thể
    /// </summary>
    void SaveOrUpdate(ProductVariant variant);

    /// <summary>
    /// Lưu biến thể và giá trị thuộc tính
    /// </summary>
    Task<Guid> SaveAsync(ProductVariant variant, List<(Guid AttributeId, string Value)> attributeValues);

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa biến thể
    /// </summary>
    Task DeleteAsync(Guid id);

    #endregion

    #region Validation & Business Rules

    /// <summary>
    /// Kiểm tra mã biến thể có trùng không
    /// </summary>
    bool IsVariantCodeExists(string variantCode, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra mã biến thể có trùng không (Async)
    /// </summary>
    Task<bool> IsVariantCodeExistsAsync(string variantCode, Guid? excludeId = null);

    #endregion

    #region Statistics Methods

    /// <summary>
    /// Đếm tổng số biến thể
    /// </summary>
    int GetTotalCount();

    /// <summary>
    /// Đếm số biến thể theo sản phẩm
    /// </summary>
    int GetCountByProduct(Guid productId);

    /// <summary>
    /// Đếm số biến thể theo trạng thái
    /// </summary>
    int GetCountByStatus(bool isActive);

    /// <summary>
    /// Đếm số biến thể theo đơn vị tính
    /// </summary>
    int GetCountByUnitOfMeasure(Guid unitOfMeasureId);

    /// <summary>
    /// Lấy biến thể theo khoảng thời gian tạo
    /// </summary>
    List<ProductVariant> GetByCreatedDateRange(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Lấy biến thể theo khoảng thời gian tạo (Async)
    /// </summary>
    Task<List<ProductVariant>> GetByCreatedDateRangeAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Lấy biến thể theo khoảng thời gian cập nhật
    /// </summary>
    List<ProductVariant> GetByModifiedDateRange(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Lấy biến thể theo khoảng thời gian cập nhật (Async)
    /// </summary>
    Task<List<ProductVariant>> GetByModifiedDateRangeAsync(DateTime fromDate, DateTime toDate);

    /// <summary>
    /// Lấy biến thể được tạo gần đây nhất
    /// </summary>
    List<ProductVariant> GetRecentlyCreated(int count = 10);

    /// <summary>
    /// Lấy biến thể được cập nhật gần đây nhất
    /// </summary>
    List<ProductVariant> GetRecentlyModified(int count = 10);

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy danh sách giá trị thuộc tính của biến thể
    /// </summary>
    List<(Guid AttributeId, string AttributeName, string Value)> GetAttributeValues(Guid variantId);

    /// <summary>
    /// Lấy danh sách giá trị thuộc tính của biến thể (Async)
    /// </summary>
    Task<List<(Guid AttributeId, string AttributeName, string Value)>> GetAttributeValuesAsync(Guid variantId);

    /// <summary>
    /// Lấy DataContext để sử dụng với LinqServerModeSource
    /// </summary>
    Task<VnsErp2025DataContext> GetDataContextAsync();

    /// <summary>
    /// Lấy queryable cho LinqInstantFeedbackSource (chỉ trả về Entity)
    /// </summary>
    IQueryable<ProductVariant> GetQueryableForInstantFeedback();

    /// <summary>
    /// Lấy queryable của ProductServices để join trong BLL
    /// </summary>
    IQueryable<ProductService> GetProductServicesQueryable();

    /// <summary>
    /// Lấy queryable của UnitOfMeasures để join trong BLL
    /// </summary>
    IQueryable<UnitOfMeasure> GetUnitOfMeasuresQueryable();

    /// <summary>
    /// Cập nhật VariantFullName cho tất cả biến thể hiện có
    /// </summary>
    Task UpdateAllVariantFullNamesAsync();

    #endregion
}
