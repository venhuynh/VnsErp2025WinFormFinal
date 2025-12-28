using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MasterData.ProductService;

namespace Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;

/// <summary>
/// Interface cho Data Access Layer của ProductService
/// </summary>
public interface IProductServiceRepository
{
    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật sản phẩm/dịch vụ.
    /// </summary>
    /// <param name="dto">ProductServiceDto</param>
    void SaveOrUpdate(ProductServiceDto dto);

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo Id.
    /// </summary>
    /// <param name="id">Id của sản phẩm/dịch vụ</param>
    /// <returns>ProductServiceDto hoặc null nếu không tìm thấy</returns>
    ProductServiceDto GetById(Guid id);

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo Id (Async).
    /// </summary>
    /// <param name="id">Id của sản phẩm/dịch vụ</param>
    /// <returns>ProductServiceDto hoặc null nếu không tìm thấy</returns>
    Task<ProductServiceDto> GetByIdAsync(Guid id);

    /// <summary>
    /// Lấy tất cả sản phẩm/dịch vụ.
    /// </summary>
    /// <returns>Danh sách tất cả ProductServiceDto</returns>
    List<ProductServiceDto> GetAll();

    /// <summary>
    /// Lấy tất cả sản phẩm/dịch vụ (Async).
    /// </summary>
    /// <returns>Danh sách tất cả ProductServiceDto</returns>
    Task<List<ProductServiceDto>> GetAllAsync();

    /// <summary>
    /// Tìm kiếm sản phẩm/dịch vụ theo từ khóa.
    /// </summary>
    /// <param name="keyword">Từ khóa tìm kiếm</param>
    /// <returns>Danh sách ProductServiceDto</returns>
    List<ProductServiceDto> Search(string keyword);

    /// <summary>
    /// Tìm kiếm sản phẩm/dịch vụ trong toàn bộ database (Async)
    /// </summary>
    /// <param name="searchText">Text tìm kiếm</param>
    /// <returns>Danh sách kết quả tìm kiếm</returns>
    Task<List<ProductServiceDto>> SearchAsync(string searchText);

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
    Task<List<ProductServiceDto>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        string searchText = null,
        Guid? categoryId = null,
        bool? isService = null,
        bool? isActive = null);

    /// <summary>
    /// Lấy dữ liệu thumbnail image cho lazy loading
    /// </summary>
    /// <param name="productId">ID sản phẩm</param>
    /// <returns>Dữ liệu ảnh thumbnail</returns>
    byte[] GetThumbnailImageData(Guid productId);

    /// <summary>
    /// Lấy danh sách sản phẩm với search và filter (optimized)
    /// </summary>
    Task<List<ProductServiceDto>> GetFilteredAsync(
        string searchText = null,
        Guid? categoryId = null,
        bool? isService = null,
        bool? isActive = null,
        string orderBy = "Name",
        string orderDirection = "ASC");

    /// <summary>
    /// Lấy danh sách mã code unique
    /// </summary>
    /// <returns>Danh sách mã code unique</returns>
    Task<List<object>> GetUniqueCodesAsync();

    /// <summary>
    /// Lấy danh sách tên unique
    /// </summary>
    /// <returns>Danh sách tên unique</returns>
    Task<List<object>> GetUniqueNamesAsync();

    /// <summary>
    /// Lấy danh sách tên danh mục unique
    /// </summary>
    /// <returns>Danh sách tên danh mục unique</returns>
    Task<List<object>> GetUniqueCategoryNamesAsync();

    /// <summary>
    /// Lấy danh sách loại hiển thị unique
    /// </summary>
    /// <returns>Danh sách loại hiển thị unique</returns>
    Task<List<object>> GetUniqueTypeDisplaysAsync();

    /// <summary>
    /// Lấy danh sách trạng thái hiển thị unique
    /// </summary>
    /// <returns>Danh sách trạng thái hiển thị unique</returns>
    Task<List<object>> GetUniqueStatusDisplaysAsync();

    #endregion

    #region ========== UPDATE OPERATIONS ==========
    // Update operations are handled by SaveOrUpdate method
    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa sản phẩm/dịch vụ theo ID.
    /// </summary>
    /// <param name="id">ID sản phẩm/dịch vụ cần xóa</param>
    /// <returns>True nếu xóa thành công</returns>
    bool DeleteProductService(Guid id);

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra mã sản phẩm/dịch vụ có tồn tại không.
    /// </summary>
    /// <param name="code">Mã sản phẩm/dịch vụ</param>
    /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
    /// <returns>True nếu tồn tại</returns>
    bool IsCodeExists(string code, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra mã sản phẩm/dịch vụ có tồn tại không (Async).
    /// </summary>
    /// <param name="code">Mã sản phẩm/dịch vụ</param>
    /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
    /// <returns>True nếu tồn tại</returns>
    Task<bool> IsCodeExistsAsync(string code, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra tên sản phẩm/dịch vụ có tồn tại không.
    /// </summary>
    /// <param name="name">Tên sản phẩm/dịch vụ</param>
    /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
    /// <returns>True nếu tồn tại</returns>
    bool IsNameExists(string name, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra tên sản phẩm/dịch vụ có tồn tại không (Async).
    /// </summary>
    /// <param name="name">Tên sản phẩm/dịch vụ</param>
    /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
    /// <returns>True nếu tồn tại</returns>
    Task<bool> IsNameExistsAsync(string name, Guid? excludeId = null);

    #endregion

    #region ========== BUSINESS LOGIC METHODS ==========

    /// <summary>
    /// Đếm số lượng biến thể của sản phẩm/dịch vụ
    /// </summary>
    /// <param name="productServiceId">ID sản phẩm/dịch vụ</param>
    /// <returns>Số lượng biến thể</returns>
    int GetVariantCount(Guid productServiceId);

    /// <summary>
    /// Đếm số lượng hình ảnh của sản phẩm/dịch vụ
    /// </summary>
    /// <param name="productServiceId">ID sản phẩm/dịch vụ</param>
    /// <returns>Số lượng hình ảnh</returns>
    int GetImageCount(Guid productServiceId);

    /// <summary>
    /// Đếm số lượng biến thể và hình ảnh cho nhiều sản phẩm/dịch vụ
    /// </summary>
    /// <param name="productServiceIds">Danh sách ID sản phẩm/dịch vụ</param>
    /// <returns>Dictionary với key là ProductServiceId và value là (VariantCount, ImageCount)</returns>
    Dictionary<Guid, (int VariantCount, int ImageCount)> GetCountsForProducts(List<Guid> productServiceIds);

    /// <summary>
    /// Lấy counts cho nhiều sản phẩm cùng lúc (optimized async version)
    /// </summary>
    /// <param name="productIds">Danh sách ID sản phẩm</param>
    /// <returns>Dictionary chứa counts</returns>
    Task<Dictionary<Guid, (int VariantCount, int ImageCount)>> GetCountsForProductsAsync(List<Guid> productIds);

    #endregion

    #region ========== HELPER METHODS ==========

    /// <summary>
    /// Lấy số tiếp theo cho mã sản phẩm trong danh mục.
    /// </summary>
    /// <param name="categoryId">ID danh mục</param>
    /// <param name="prefix">Prefix chữ cái đầu</param>
    /// <returns>Số tiếp theo (1-9999)</returns>
    int GetNextProductNumber(Guid categoryId, string prefix);

    /// <summary>
    /// Lấy mã danh mục từ ID danh mục.
    /// </summary>
    /// <param name="categoryId">ID của danh mục</param>
    /// <returns>Mã danh mục</returns>
    string GetCategoryCode(Guid categoryId);

    #endregion
}
