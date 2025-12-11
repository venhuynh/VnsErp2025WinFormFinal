using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể ProductServiceCategory (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IProductServiceCategoryRepository
{
    #region Read Operations

    /// <summary>
    /// Lấy ProductServiceCategory theo Id
    /// </summary>
    /// <param name="id">Id của category</param>
    /// <returns>ProductServiceCategory hoặc null nếu không tìm thấy</returns>
    ProductServiceCategory GetById(Guid id);

    /// <summary>
    /// Lấy ProductServiceCategory theo Id (Async)
    /// </summary>
    /// <param name="id">Id của category</param>
    /// <returns>ProductServiceCategory hoặc null nếu không tìm thấy</returns>
    Task<ProductServiceCategory> GetByIdAsync(Guid id);

    /// <summary>
    /// Lấy tất cả ProductServiceCategory
    /// </summary>
    /// <returns>Danh sách tất cả ProductServiceCategory</returns>
    List<ProductServiceCategory> GetAll();

    /// <summary>
    /// Lấy tất cả ProductServiceCategory (Async)
    /// </summary>
    /// <returns>Danh sách tất cả ProductServiceCategory</returns>
    Task<List<ProductServiceCategory>> GetAllAsync();

    /// <summary>
    /// Lấy số lượng sản phẩm theo từng category
    /// </summary>
    /// <returns>Dictionary với Key là CategoryId, Value là số lượng sản phẩm</returns>
    Dictionary<Guid, int> GetProductCountByCategory();

    /// <summary>
    /// Lấy số lượng sản phẩm theo từng category (Async)
    /// </summary>
    /// <returns>Dictionary với Key là CategoryId, Value là số lượng sản phẩm</returns>
    Task<Dictionary<Guid, int>> GetProductCountByCategoryAsync();

    /// <summary>
    /// Lấy tất cả ProductServiceCategory đang hoạt động (IsActive = true)
    /// </summary>
    /// <returns>Danh sách ProductServiceCategory active</returns>
    List<ProductServiceCategory> GetActiveCategories();

    /// <summary>
    /// Lấy tất cả ProductServiceCategory đang hoạt động (Async)
    /// </summary>
    /// <returns>Danh sách ProductServiceCategory active</returns>
    Task<List<ProductServiceCategory>> GetActiveCategoriesAsync();

    /// <summary>
    /// Lấy danh mục con của một danh mục cha
    /// </summary>
    /// <param name="parentId">Id của danh mục cha (null cho danh mục cấp 1)</param>
    /// <returns>Danh sách danh mục con</returns>
    List<ProductServiceCategory> GetCategoriesByParent(Guid? parentId);

    /// <summary>
    /// Lấy danh mục con của một danh mục cha (Async)
    /// </summary>
    /// <param name="parentId">Id của danh mục cha (null cho danh mục cấp 1)</param>
    /// <returns>Danh sách danh mục con</returns>
    Task<List<ProductServiceCategory>> GetCategoriesByParentAsync(Guid? parentId);

    #endregion

    #region Create/Update Operations

    /// <summary>
    /// Lưu hoặc cập nhật ProductServiceCategory
    /// </summary>
    /// <param name="category">ProductServiceCategory cần lưu hoặc cập nhật</param>
    void SaveOrUpdate(ProductServiceCategory category);

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa ProductServiceCategory theo Id
    /// </summary>
    /// <param name="id">Id của category cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy hoặc có ràng buộc</returns>
    bool DeleteCategory(Guid id);

    #endregion

    #region Validation Operations

    /// <summary>
    /// Kiểm tra tên category đã tồn tại chưa (loại trừ Id khi cập nhật)
    /// </summary>
    /// <param name="categoryName">Tên category cần kiểm tra</param>
    /// <param name="excludeId">Id cần loại trừ khỏi kiểm tra (dùng khi update)</param>
    /// <returns>True nếu tên đã tồn tại</returns>
    bool IsCategoryNameExists(string categoryName, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra tên category đã tồn tại chưa (Async)
    /// </summary>
    /// <param name="categoryName">Tên category cần kiểm tra</param>
    /// <param name="excludeId">Id cần loại trừ khỏi kiểm tra (dùng khi update)</param>
    /// <returns>True nếu tên đã tồn tại</returns>
    Task<bool> IsCategoryNameExistsAsync(string categoryName, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra mã category đã tồn tại chưa (loại trừ Id khi cập nhật)
    /// </summary>
    /// <param name="categoryCode">Mã category cần kiểm tra</param>
    /// <param name="excludeId">Id cần loại trừ khỏi kiểm tra (dùng khi update)</param>
    /// <returns>True nếu mã đã tồn tại</returns>
    bool IsCategoryCodeExists(string categoryCode, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra mã category đã tồn tại chưa (Async)
    /// </summary>
    /// <param name="categoryCode">Mã category cần kiểm tra</param>
    /// <param name="excludeId">Id cần loại trừ khỏi kiểm tra (dùng khi update)</param>
    /// <returns>True nếu mã đã tồn tại</returns>
    Task<bool> IsCategoryCodeExistsAsync(string categoryCode, Guid? excludeId = null);

    #endregion
}