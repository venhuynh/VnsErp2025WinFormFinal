using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MasterData.ProductService;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Interface cho Data Access Layer của ProductServiceCategory
/// </summary>
public interface IProductServiceCategoryRepository
{
    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật ProductServiceCategory
    /// </summary>
    /// <param name="dto">ProductServiceCategoryDto</param>
    void SaveOrUpdate(ProductServiceCategoryDto dto);

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy ProductServiceCategory theo Id
    /// </summary>
    /// <param name="id">Id của category</param>
    /// <returns>ProductServiceCategoryDto hoặc null nếu không tìm thấy</returns>
    ProductServiceCategoryDto GetById(Guid id);

    /// <summary>
    /// Lấy ProductServiceCategory theo Id (Async)
    /// </summary>
    /// <param name="id">Id của category</param>
    /// <returns>ProductServiceCategoryDto hoặc null nếu không tìm thấy</returns>
    Task<ProductServiceCategoryDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Lấy tất cả ProductServiceCategory
    /// </summary>
    /// <returns>Danh sách tất cả ProductServiceCategoryDto</returns>
    List<ProductServiceCategoryDto> GetAll();

    /// <summary>
    /// Lấy tất cả ProductServiceCategory (Async)
    /// </summary>
    /// <returns>Danh sách tất cả ProductServiceCategoryDto</returns>
    Task<List<ProductServiceCategoryDto>> GetAllAsync();

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
    /// <returns>Danh sách ProductServiceCategoryDto active</returns>
    List<ProductServiceCategoryDto> GetActiveCategories();

    /// <summary>
    /// Lấy tất cả ProductServiceCategory đang hoạt động (Async)
    /// </summary>
    /// <returns>Danh sách ProductServiceCategoryDto active</returns>
    Task<List<ProductServiceCategoryDto>> GetActiveCategoriesAsync();

    /// <summary>
    /// Lấy danh mục con của một danh mục cha
    /// </summary>
    /// <param name="parentId">Id của danh mục cha (null cho danh mục cấp 1)</param>
    /// <returns>Danh sách danh mục con</returns>
    List<ProductServiceCategoryDto> GetCategoriesByParent(Guid? parentId);

    /// <summary>
    /// Lấy danh mục con của một danh mục cha (Async)
    /// </summary>
    /// <param name="parentId">Id của danh mục cha (null cho danh mục cấp 1)</param>
    /// <returns>Danh sách danh mục con</returns>
    Task<List<ProductServiceCategoryDto>> GetCategoriesByParentAsync(Guid? parentId);

    #endregion

    #region ========== UPDATE OPERATIONS ==========
    // Update operations are handled by SaveOrUpdate method
    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa ProductServiceCategory theo Id
    /// </summary>
    /// <param name="id">Id của category cần xóa</param>
    /// <returns>True nếu xóa thành công, False nếu không tìm thấy hoặc có ràng buộc</returns>
    bool DeleteCategory(Guid id);

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

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
