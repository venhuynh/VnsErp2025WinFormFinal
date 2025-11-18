using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể ProductService (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public interface IProductServiceRepository
{
    #region Read Operations

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo Id.
    /// </summary>
    ProductService GetById(Guid id);

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo Id (Async).
    /// </summary>
    Task<ProductService> GetByIdAsync(Guid id);

    /// <summary>
    /// Lấy tất cả sản phẩm/dịch vụ.
    /// </summary>
    List<ProductService> GetAll();

    /// <summary>
    /// Lấy tất cả sản phẩm/dịch vụ (Async).
    /// </summary>
    Task<List<ProductService>> GetAllAsync();

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo mã.
    /// </summary>
    ProductService GetByCode(string code);

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo tên.
    /// </summary>
    ProductService GetByName(string name);

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo danh mục.
    /// </summary>
    List<ProductService> GetByCategory(Guid categoryId);

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo loại (sản phẩm hoặc dịch vụ).
    /// </summary>
    List<ProductService> GetByType(bool isService);

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo trạng thái hoạt động.
    /// </summary>
    List<ProductService> GetByStatus(bool isActive);

    /// <summary>
    /// Tìm kiếm sản phẩm/dịch vụ theo từ khóa.
    /// </summary>
    List<ProductService> Search(string keyword);

    #endregion

    #region Create/Update Operations

    /// <summary>
    /// Lưu hoặc cập nhật sản phẩm/dịch vụ.
    /// </summary>
    void SaveOrUpdate(ProductService productService);

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa sản phẩm/dịch vụ theo ID.
    /// </summary>
    /// <returns>True nếu xóa thành công</returns>
    bool DeleteProductService(Guid id);

    #endregion

    #region Validation & Business Rules

    /// <summary>
    /// Kiểm tra mã sản phẩm/dịch vụ có tồn tại không.
    /// </summary>
    bool IsCodeExists(string code, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra mã sản phẩm/dịch vụ có tồn tại không (Async).
    /// </summary>
    Task<bool> IsCodeExistsAsync(string code, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra tên sản phẩm/dịch vụ có tồn tại không.
    /// </summary>
    bool IsNameExists(string name, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra tên sản phẩm/dịch vụ có tồn tại không (Async).
    /// </summary>
    Task<bool> IsNameExistsAsync(string name, Guid? excludeId = null);

    #endregion

    #region Statistics Methods

    /// <summary>
    /// Đếm tổng số sản phẩm/dịch vụ.
    /// </summary>
    int GetTotalCount();

    /// <summary>
    /// Đếm số sản phẩm.
    /// </summary>
    int GetProductCount();

    /// <summary>
    /// Đếm số dịch vụ.
    /// </summary>
    int GetServiceCount();

    /// <summary>
    /// Đếm số sản phẩm/dịch vụ theo danh mục.
    /// </summary>
    int GetCountByCategory(Guid categoryId);

    /// <summary>
    /// Đếm số lượng biến thể của sản phẩm/dịch vụ
    /// </summary>
    int GetVariantCount(Guid productServiceId);

    /// <summary>
    /// Đếm số lượng hình ảnh của sản phẩm/dịch vụ
    /// </summary>
    int GetImageCount(Guid productServiceId);

    /// <summary>
    /// Đếm số lượng biến thể và hình ảnh cho nhiều sản phẩm/dịch vụ
    /// </summary>
    Dictionary<Guid, (int VariantCount, int ImageCount)> GetCountsForProducts(List<Guid> productServiceIds);

    #endregion

    #region Pagination & Optimization Methods

    /// <summary>
    /// Lấy số lượng tổng cộng với filter
    /// </summary>
    Task<int> GetCountAsync(
        string searchText = null,
        Guid? categoryId = null,
        bool? isService = null,
        bool? isActive = null);

    /// <summary>
    /// Lấy dữ liệu phân trang
    /// </summary>
    Task<List<ProductService>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        string searchText = null,
        Guid? categoryId = null,
        bool? isService = null,
        bool? isActive = null);

    /// <summary>
    /// Lấy dữ liệu thumbnail image cho lazy loading
    /// </summary>
    byte[] GetThumbnailImageData(Guid productId);

    /// <summary>
    /// Lấy danh sách sản phẩm với search và filter (optimized)
    /// </summary>
    Task<List<ProductService>> GetFilteredAsync(
        string searchText = null,
        Guid? categoryId = null,
        bool? isService = null,
        bool? isActive = null,
        string orderBy = "Name",
        string orderDirection = "ASC");

    /// <summary>
    /// Lấy counts cho nhiều sản phẩm cùng lúc (optimized async version)
    /// </summary>
    Task<Dictionary<Guid, (int VariantCount, int ImageCount)>> GetCountsForProductsAsync(List<Guid> productIds);

    #endregion

    #region Search and Filter Methods

    /// <summary>
    /// Tìm kiếm sản phẩm/dịch vụ trong toàn bộ database
    /// </summary>
    Task<List<ProductService>> SearchAsync(string searchText);

    /// <summary>
    /// Lấy danh sách mã code unique
    /// </summary>
    Task<List<object>> GetUniqueCodesAsync();

    /// <summary>
    /// Lấy danh sách tên unique
    /// </summary>
    Task<List<object>> GetUniqueNamesAsync();

    /// <summary>
    /// Lấy danh sách tên danh mục unique
    /// </summary>
    Task<List<object>> GetUniqueCategoryNamesAsync();

    /// <summary>
    /// Lấy danh sách loại hiển thị unique
    /// </summary>
    Task<List<object>> GetUniqueTypeDisplaysAsync();

    /// <summary>
    /// Lấy danh sách trạng thái hiển thị unique
    /// </summary>
    Task<List<object>> GetUniqueStatusDisplaysAsync();

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy số tiếp theo cho mã sản phẩm trong danh mục.
    /// </summary>
    int GetNextProductNumber(Guid categoryId, string prefix);

    /// <summary>
    /// Lấy mã danh mục từ ID danh mục.
    /// </summary>
    string GetCategoryCode(Guid categoryId);

    #endregion
}
