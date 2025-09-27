using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;

namespace Dal.DataAccess.MasterData.ProductService
{
    /// <summary>
    /// Data Access cho thực thể ProductServiceCategory (LINQ to SQL trên VnsErp2025DataContext).
    /// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
    /// </summary>
    public class ProductServiceCategoryDataAccess : BaseDataAccess<ProductServiceCategory>
    {
        #region Constructors

        /// <summary>
        /// Khởi tạo mặc định.
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ProductServiceCategoryDataAccess(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Khởi tạo với connection string.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ProductServiceCategoryDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        #endregion

        #region Create

        /// <summary>
        /// Thêm danh mục sản phẩm/dịch vụ mới với validation cơ bản.
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <param name="description">Mô tả</param>
        /// <param name="parentId">ID danh mục cha (tùy chọn)</param>
        /// <returns>Danh mục đã tạo</returns>
        public ProductServiceCategory AddNewCategory(string categoryName, string description = null, Guid? parentId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("Tên danh mục không được rỗng", nameof(categoryName));

                if (IsCategoryNameExists(categoryName))
                    throw new InvalidOperationException($"Danh mục '{categoryName}' đã tồn tại");

                var category = new ProductServiceCategory
                {
                    Id = Guid.NewGuid(),
                    CategoryName = categoryName.Trim(),
                    Description = description?.Trim(),
                    ParentId = parentId
                };

                Add(category);
                return category;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi thêm danh mục mới '{categoryName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm danh mục sản phẩm/dịch vụ mới (Async).
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <param name="description">Mô tả</param>
        /// <param name="parentId">ID danh mục cha (tùy chọn)</param>
        /// <returns>Task chứa danh mục đã tạo</returns>
        public async Task<ProductServiceCategory> AddNewCategoryAsync(string categoryName, string description = null, Guid? parentId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    throw new ArgumentException("Tên danh mục không được rỗng", nameof(categoryName));

                if (await IsCategoryNameExistsAsync(categoryName))
                    throw new InvalidOperationException($"Danh mục '{categoryName}' đã tồn tại");

                var category = new ProductServiceCategory
                {
                    Id = Guid.NewGuid(),
                    CategoryName = categoryName.Trim(),
                    Description = description?.Trim(),
                    ParentId = parentId
                };

                await AddAsync(category);
                return category;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi thêm danh mục mới '{categoryName}': {ex.Message}", ex);
            }
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
                using var context = CreateContext();
                return context.ProductServiceCategories.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh mục theo Id {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Override GetById từ BaseDataAccess để sử dụng Guid thay vì object.
        /// </summary>
        public override ProductServiceCategory GetById(object id)
        {
            if (id is Guid guidId)
                return GetById(guidId);
            return null;
        }

        /// <summary>
        /// Lấy danh mục theo Id (Async).
        /// </summary>
        public async Task<ProductServiceCategory> GetByIdAsync(Guid id)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServiceCategories.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh mục theo Id {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả danh mục.
        /// </summary>
        public override List<ProductServiceCategory> GetAll()
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServiceCategories.ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả danh mục (Async).
        /// </summary>
        public override async Task<List<ProductServiceCategory>> GetAllAsync()
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServiceCategories.ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh mục theo tên.
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <returns>Danh mục hoặc null</returns>
        public ProductServiceCategory GetByName(string categoryName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    return null;

                using var context = CreateContext();
                return context.ProductServiceCategories.FirstOrDefault(x => x.CategoryName == categoryName.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh mục theo tên '{categoryName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh mục theo tên (Async).
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <returns>Task chứa danh mục hoặc null</returns>
        public async Task<ProductServiceCategory> GetByNameAsync(string categoryName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoryName))
                    return null;

                using var context = CreateContext();
                return await Task.Run(() => context.ProductServiceCategories.FirstOrDefault(x => x.CategoryName == categoryName.Trim()));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh mục theo tên '{categoryName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách danh mục con của một danh mục cha.
        /// </summary>
        /// <param name="parentId">ID danh mục cha</param>
        /// <returns>Danh sách danh mục con</returns>
        public List<ProductServiceCategory> GetChildren(Guid parentId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServiceCategories.Where(x => x.ParentId == parentId).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh mục con của {parentId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách danh mục con của một danh mục cha (Async).
        /// </summary>
        /// <param name="parentId">ID danh mục cha</param>
        /// <returns>Task chứa danh sách danh mục con</returns>
        public async Task<List<ProductServiceCategory>> GetChildrenAsync(Guid parentId)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServiceCategories.Where(x => x.ParentId == parentId).ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh mục con của {parentId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách danh mục gốc (không có ParentId).
        /// </summary>
        /// <returns>Danh sách danh mục gốc</returns>
        public List<ProductServiceCategory> GetRootCategories()
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServiceCategories.Where(x => x.ParentId == null).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh mục gốc: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách danh mục gốc (không có ParentId) (Async).
        /// </summary>
        /// <returns>Task chứa danh sách danh mục gốc</returns>
        public async Task<List<ProductServiceCategory>> GetRootCategoriesAsync()
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServiceCategories.Where(x => x.ParentId == null).ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh mục gốc: {ex.Message}", ex);
            }
        }

        #endregion

        #region Update

        /// <summary>
        /// Cập nhật tên danh mục.
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <param name="newCategoryName">Tên mới</param>
        /// <param name="newDescription">Mô tả mới (tùy chọn)</param>
        /// <returns>True nếu cập nhật thành công</returns>
        public bool UpdateCategory(Guid id, string newCategoryName, string newDescription = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newCategoryName))
                    throw new ArgumentException("Tên danh mục không được rỗng", nameof(newCategoryName));

                if (IsCategoryNameExists(newCategoryName, id))
                    throw new InvalidOperationException($"Danh mục '{newCategoryName}' đã tồn tại");

                var category = GetById(id);
                if (category == null)
                    return false;

                category.CategoryName = newCategoryName.Trim();
                if (newDescription != null)
                    category.Description = newDescription.Trim();

                Update(category);
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi cập nhật danh mục {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật tên danh mục (Async).
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <param name="newCategoryName">Tên mới</param>
        /// <param name="newDescription">Mô tả mới (tùy chọn)</param>
        /// <returns>Task chứa True nếu cập nhật thành công</returns>
        public async Task<bool> UpdateCategoryAsync(Guid id, string newCategoryName, string newDescription = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newCategoryName))
                    throw new ArgumentException("Tên danh mục không được rỗng", nameof(newCategoryName));

                if (await IsCategoryNameExistsAsync(newCategoryName, id))
                    throw new InvalidOperationException($"Danh mục '{newCategoryName}' đã tồn tại");

                var category = await GetByIdAsync(id);
                if (category == null)
                    return false;

                category.CategoryName = newCategoryName.Trim();
                if (newDescription != null)
                    category.Description = newDescription.Trim();

                await Task.Run(() => Update(category));
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi cập nhật danh mục {id}: {ex.Message}", ex);
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
                using var context = CreateContext();
                var category = context.ProductServiceCategories.FirstOrDefault(x => x.Id == id);
                if (category == null)
                    return false;

                // Kiểm tra có danh mục con không
                var hasChildren = context.ProductServiceCategories.Any(x => x.ParentId == id);
                if (hasChildren)
                    throw new InvalidOperationException("Không thể xóa danh mục có danh mục con");

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
                }

                context.ProductServiceCategories.DeleteOnSubmit(category);
                context.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa danh mục {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa danh mục theo ID với logic di chuyển sản phẩm/dịch vụ sang danh mục mặc định (Async).
        /// </summary>
        /// <param name="id">ID danh mục cần xóa</param>
        /// <returns>Task chứa True nếu xóa thành công</returns>
        public async Task<bool> DeleteCategoryAsync(Guid id)
        {
            try
            {
                using var context = CreateContext();
                var category = await Task.Run(() => context.ProductServiceCategories.FirstOrDefault(x => x.Id == id));
                if (category == null)
                    return false;

                // Kiểm tra có danh mục con không
                var hasChildren = await Task.Run(() => context.ProductServiceCategories.Any(x => x.ParentId == id));
                if (hasChildren)
                    throw new InvalidOperationException("Không thể xóa danh mục có danh mục con");

                // Kiểm tra có sản phẩm/dịch vụ không
                var hasProducts = await Task.Run(() => context.ProductServices.Any(x => x.CategoryId == id));
                if (hasProducts)
                {
                    // Tìm hoặc tạo danh mục mặc định
                    var defaultCategory = await GetOrCreateDefaultCategoryAsync(context);
                    
                    // Di chuyển sản phẩm/dịch vụ sang danh mục mặc định
                    var productsToMove = await Task.Run(() => context.ProductServices.Where(x => x.CategoryId == id).ToList());
                    foreach (var product in productsToMove)
                    {
                        product.CategoryId = defaultCategory.Id;
                    }
                }

                context.ProductServiceCategories.DeleteOnSubmit(category);
                await Task.Run(() => context.SubmitChanges());
                return true;
            }
            catch (Exception ex)
            {
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

                using var context = CreateContext();
                var query = context.ProductServiceCategories.Where(x => x.CategoryName == categoryName.Trim());
                
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);

                return query.Any();
            }
            catch (Exception ex)
            {
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

                using var context = CreateContext();
                var query = context.ProductServiceCategories.Where(x => x.CategoryName == categoryName.Trim());
                
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);

                return await Task.Run(() => query.Any());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra danh mục có sản phẩm/dịch vụ không.
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>True nếu có sản phẩm/dịch vụ</returns>
        public bool HasProducts(Guid categoryId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices.Any(x => x.CategoryId == categoryId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra sản phẩm/dịch vụ của danh mục {categoryId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra danh mục có sản phẩm/dịch vụ không (Async).
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>Task chứa True nếu có sản phẩm/dịch vụ</returns>
        public async Task<bool> HasProductsAsync(Guid categoryId)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServices.Any(x => x.CategoryId == categoryId));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra sản phẩm/dịch vụ của danh mục {categoryId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy số lượng sản phẩm/dịch vụ của một danh mục.
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>Số lượng sản phẩm/dịch vụ</returns>
        public int GetProductCount(Guid categoryId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices.Count(x => x.CategoryId == categoryId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm sản phẩm/dịch vụ của danh mục {categoryId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy số lượng sản phẩm/dịch vụ của một danh mục (Async).
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>Task chứa số lượng sản phẩm/dịch vụ</returns>
        public async Task<int> GetProductCountAsync(Guid categoryId)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServices.Count(x => x.CategoryId == categoryId));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm sản phẩm/dịch vụ của danh mục {categoryId}: {ex.Message}", ex);
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
                using var context = CreateContext();
                return context.ProductServices
                    .Where(x => x.CategoryId.HasValue)
                    .GroupBy(x => x.CategoryId.Value)
                    .ToDictionary(g => g.Key, g => g.Count());
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServices
                    .Where(x => x.CategoryId.HasValue)
                    .GroupBy(x => x.CategoryId.Value)
                    .ToDictionary(g => g.Key, g => g.Count()));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm sản phẩm/dịch vụ theo danh mục: {ex.Message}", ex);
            }
        }

        #endregion

        #region Helper Methods

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
            }
            
            return defaultCategory;
        }

        /// <summary>
        /// Tìm hoặc tạo danh mục mặc định "Phân loại chưa đặt tên" (Async).
        /// </summary>
        /// <param name="context">DataContext</param>
        /// <returns>Task chứa danh mục mặc định</returns>
        private async Task<ProductServiceCategory> GetOrCreateDefaultCategoryAsync(VnsErp2025DataContext context)
        {
            const string defaultCategoryName = "Phân loại chưa đặt tên";
            
            // Tìm danh mục mặc định
            var defaultCategory = await Task.Run(() => context.ProductServiceCategories.FirstOrDefault(x => x.CategoryName == defaultCategoryName));
            
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
                await Task.Run(() => context.SubmitChanges());
            }
            
            return defaultCategory;
        }

        #endregion
    }
}
