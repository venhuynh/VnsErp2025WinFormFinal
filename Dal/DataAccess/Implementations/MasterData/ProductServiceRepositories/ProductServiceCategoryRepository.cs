using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể ProductServiceCategory (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public class ProductServiceCategoryRepository : IProductServiceCategoryRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class ProductServiceCategoryRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public ProductServiceCategoryRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("ProductServiceCategoryRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Tìm hoặc tạo danh mục mặc định "Phân loại chưa đặt tên".
    /// </summary>
    /// <param name="context">DataContext</param>
    /// <returns>Danh mục mặc định</returns>
    private ProductServiceCategory GetOrCreateDefaultCategory(VnsErp2025DataContext context)
    {
        const string defaultCategoryName = "Phân loại chưa đặt tên";
        
        // Tìm danh mục mặc định
        var defaultCategory = context.ProductServiceCategories.FirstOrDefault(x => x.CategoryName == defaultCategoryName);
        
        if (defaultCategory == null)
        {
            // Tạo danh mục mặc định mới
            defaultCategory = new ProductServiceCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = defaultCategoryName,
                Description = "Danh mục mặc định cho các sản phẩm/dịch vụ chưa được phân loại",
                ParentId = null
            };
            
            context.ProductServiceCategories.InsertOnSubmit(defaultCategory);
            context.SubmitChanges();
            
            _logger.Info($"Đã tạo danh mục mặc định: {defaultCategoryName}");
        }
        
        return defaultCategory;
    }

    #endregion

    #region Read

    /// <summary>
    /// Lấy danh mục theo Id.
    /// </summary>
    public ProductServiceCategory GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var category = context.ProductServiceCategories.FirstOrDefault(x => x.Id == id);
            
            if (category != null)
            {
                _logger.Debug($"Đã lấy danh mục theo ID: {id} - {category.CategoryName}");
            }
            else
            {
                _logger.Debug($"Không tìm thấy danh mục với ID: {id}");
            }
            
            return category;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh mục theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh mục theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh mục theo Id (Async).
    /// </summary>
    public async Task<ProductServiceCategory> GetByIdAsync(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var category = await Task.Run(() => context.ProductServiceCategories.FirstOrDefault(x => x.Id == id));
            
            if (category != null)
            {
                _logger.Debug($"Đã lấy danh mục theo ID (async): {id} - {category.CategoryName}");
            }
            else
            {
                _logger.Debug($"Không tìm thấy danh mục với ID (async): {id}");
            }
            
            return category;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh mục theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh mục theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả danh mục.
    /// </summary>
    public List<ProductServiceCategory> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            var categories = context.ProductServiceCategories.ToList();
            
            _logger.Debug($"Đã lấy {categories.Count} danh mục");
            return categories;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả danh mục (Async).
    /// </summary>
    public async Task<List<ProductServiceCategory>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var categories = await Task.Run(() => context.ProductServiceCategories.ToList());
            
            _logger.Debug($"Đã lấy {categories.Count} danh mục (async)");
            return categories;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
        }
    }

    #endregion

    #region Create/Update

    /// <summary>
    /// Lưu hoặc cập nhật danh mục sản phẩm/dịch vụ.
    /// </summary>
    /// <param name="category">Danh mục cần lưu hoặc cập nhật</param>
    public void SaveOrUpdate(ProductServiceCategory category)
    {
        try
        {
            if (category == null) 
                throw new ArgumentNullException(nameof(category));
            
            using var context = CreateNewContext();
            var existing = category.Id != Guid.Empty 
                ? context.ProductServiceCategories.FirstOrDefault(x => x.Id == category.Id) 
                : null;
            
            if (existing == null)
            {
                // Thêm mới
                if (category.Id == Guid.Empty)
                    category.Id = Guid.NewGuid();
                    
                context.ProductServiceCategories.InsertOnSubmit(category);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới danh mục: {category.CategoryName}");
            }
            else
            {
                // Cập nhật
                existing.CategoryName = category.CategoryName;
                existing.Description = category.Description;
                existing.ParentId = category.ParentId;
                existing.CategoryCode = category.CategoryCode;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật danh mục: {existing.CategoryName}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu/cập nhật danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu/cập nhật danh mục: {ex.Message}", ex);
        }
    }

    #endregion

    #region Delete

    /// <summary>
    /// Xóa danh mục theo ID với logic di chuyển sản phẩm/dịch vụ sang danh mục mặc định.
    /// </summary>
    /// <param name="id">ID danh mục cần xóa</param>
    /// <returns>True nếu xóa thành công</returns>
    public bool DeleteCategory(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var category = context.ProductServiceCategories.FirstOrDefault(x => x.Id == id);
            if (category == null)
            {
                _logger.Warning($"Không tìm thấy danh mục để xóa: {id}");
                return false;
            }

            // Kiểm tra có danh mục con không
            var hasChildren = context.ProductServiceCategories.Any(x => x.ParentId == id);
            if (hasChildren)
            {
                _logger.Warning($"Không thể xóa danh mục {id} - {category.CategoryName} vì có danh mục con");
                throw new InvalidOperationException("Không thể xóa danh mục có danh mục con");
            }

            // Kiểm tra có sản phẩm/dịch vụ không
            var hasProducts = context.ProductServices.Any(x => x.CategoryId == id);
            if (hasProducts)
            {
                // Tìm hoặc tạo danh mục mặc định
                var defaultCategory = GetOrCreateDefaultCategory(context);
                
                // Di chuyển sản phẩm/dịch vụ sang danh mục mặc định
                var productsToMove = context.ProductServices.Where(x => x.CategoryId == id).ToList();
                foreach (var product in productsToMove)
                {
                    product.CategoryId = defaultCategory.Id;
                }
                
                _logger.Info($"Đã di chuyển {productsToMove.Count} sản phẩm/dịch vụ từ danh mục {id} sang danh mục mặc định");
            }

            context.ProductServiceCategories.DeleteOnSubmit(category);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa danh mục: {id} - {category.CategoryName}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa danh mục {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa danh mục {id}: {ex.Message}", ex);
        }
    }

    #endregion

    #region Validation & Business Rules

    /// <summary>
    /// Kiểm tra tên danh mục có tồn tại không.
    /// </summary>
    /// <param name="categoryName">Tên danh mục</param>
    /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
    /// <returns>True nếu tồn tại</returns>
    public bool IsCategoryNameExists(string categoryName, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return false;

            using var context = CreateNewContext();
            var query = context.ProductServiceCategories.Where(x => x.CategoryName == categoryName.Trim());
            
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            var result = query.Any();
            _logger.Debug($"IsCategoryNameExists: Name='{categoryName}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra tên danh mục có tồn tại không (Async).
    /// </summary>
    /// <param name="categoryName">Tên danh mục</param>
    /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
    /// <returns>Task chứa True nếu tồn tại</returns>
    public async Task<bool> IsCategoryNameExistsAsync(string categoryName, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                return false;

            using var context = CreateNewContext();
            var query = context.ProductServiceCategories.Where(x => x.CategoryName == categoryName.Trim());
            
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            var result = await Task.Run(() => query.Any());
            _logger.Debug($"IsCategoryNameExistsAsync: Name='{categoryName}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra mã danh mục có tồn tại không.
    /// </summary>
    /// <param name="categoryCode">Mã danh mục</param>
    /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
    /// <returns>True nếu tồn tại</returns>
    public bool IsCategoryCodeExists(string categoryCode, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryCode))
                return false;

            using var context = CreateNewContext();
            var query = context.ProductServiceCategories.Where(x => x.CategoryCode == categoryCode.Trim());
            
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            var result = query.Any();
            _logger.Debug($"IsCategoryCodeExists: Code='{categoryCode}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra mã danh mục '{categoryCode}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra mã danh mục '{categoryCode}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra mã danh mục có tồn tại không (Async).
    /// </summary>
    /// <param name="categoryCode">Mã danh mục</param>
    /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
    /// <returns>Task chứa True nếu tồn tại</returns>
    public async Task<bool> IsCategoryCodeExistsAsync(string categoryCode, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryCode))
                return false;

            using var context = CreateNewContext();
            var query = context.ProductServiceCategories.Where(x => x.CategoryCode == categoryCode.Trim());
            
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            var result = await Task.Run(() => query.Any());
            _logger.Debug($"IsCategoryCodeExistsAsync: Code='{categoryCode}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra mã danh mục '{categoryCode}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra mã danh mục '{categoryCode}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đếm số lượng sản phẩm/dịch vụ theo từng danh mục.
    /// </summary>
    /// <returns>Dictionary với Key là CategoryId, Value là số lượng sản phẩm/dịch vụ</returns>
    public Dictionary<Guid, int> GetProductCountByCategory()
    {
        try
        {
            using var context = CreateNewContext();
            var result = context.ProductServices
                .Where(x => x.CategoryId.HasValue)
                .GroupBy(x => x.CategoryId.Value)
                .ToDictionary(g => g.Key, g => g.Count());
            
            _logger.Debug($"GetProductCountByCategory: Found {result.Count} categories with products");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm sản phẩm/dịch vụ theo danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm sản phẩm/dịch vụ theo danh mục: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đếm số lượng sản phẩm/dịch vụ theo từng danh mục (Async).
    /// </summary>
    /// <returns>Task chứa Dictionary với Key là CategoryId, Value là số lượng sản phẩm/dịch vụ</returns>
    public async Task<Dictionary<Guid, int>> GetProductCountByCategoryAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var result = await Task.Run(() => context.ProductServices
                .Where(x => x.CategoryId.HasValue)
                .GroupBy(x => x.CategoryId.Value)
                .ToDictionary(g => g.Key, g => g.Count()));
            
            _logger.Debug($"GetProductCountByCategoryAsync: Found {result.Count} categories with products");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm sản phẩm/dịch vụ theo danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm sản phẩm/dịch vụ theo danh mục: {ex.Message}", ex);
        }
    }

    #endregion
}
