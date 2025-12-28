using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bll.MasterData.ProductServiceBll
{
    /// <summary>
    /// Business logic layer cho ProductServiceCategory. 
    /// Cung cấp các method để lấy danh mục sản phẩm/dịch vụ và đếm số lượng sản phẩm/dịch vụ theo từng danh mục.
    /// </summary>
    public class ProductServiceCategoryBll
    {
        #region Fields

        private IProductServiceCategoryRepository _dataAccess;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public ProductServiceCategoryBll()
        {
            
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IProductServiceCategoryRepository GetDataAccess()
        {
            if (_dataAccess == null)
            {
                lock (_lockObject)
                {
                    if (_dataAccess == null)
                    {
                        try
                        {
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _dataAccess = new ProductServiceCategoryRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                        }
                    }
                }
            }

            return _dataAccess;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <returns>Danh sách ProductServiceCategoryDto</returns>
        public List<ProductServiceCategoryDto> GetAll()
        {
            return GetDataAccess().GetAll();
        }

        /// <summary>
        /// Lấy tất cả danh mục sản phẩm/dịch vụ (Async).
        /// </summary>
        /// <returns>Task chứa danh sách ProductServiceCategoryDto</returns>
        public Task<List<ProductServiceCategoryDto>> GetAllAsync()
        {
            return GetDataAccess().GetAllAsync();
        }

        /// <summary>
        /// Lấy danh mục sản phẩm/dịch vụ theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục</param>
        /// <returns>ProductServiceCategoryDto hoặc null</returns>
        public ProductServiceCategoryDto GetById(Guid id)
        {
            return GetDataAccess().GetById(id);
        }

        /// <summary>
        /// Lấy danh mục sản phẩm/dịch vụ theo ID (Async).
        /// </summary>
        /// <param name="id">ID của danh mục</param>
        /// <returns>Task chứa ProductServiceCategoryDto hoặc null</returns>
        public Task<ProductServiceCategoryDto> GetByIdAsync(Guid id)
        {
            return GetDataAccess().GetByIdAsync(id);
        }

        /// <summary>
        /// Đếm số lượng sản phẩm/dịch vụ theo từng danh mục.
        /// </summary>
        /// <returns>Dictionary với Key là CategoryId, Value là số lượng sản phẩm/dịch vụ</returns>
        public Dictionary<Guid, int> GetProductCountByCategory()
        {
            return GetDataAccess().GetProductCountByCategory();
        }

        /// <summary>
        /// Đếm số lượng sản phẩm/dịch vụ theo từng danh mục (Async).
        /// </summary>
        /// <returns>Task chứa Dictionary với Key là CategoryId, Value là số lượng sản phẩm/dịch vụ</returns>
        public Task<Dictionary<Guid, int>> GetProductCountByCategoryAsync()
        {
            return GetDataAccess().GetProductCountByCategoryAsync();
        }

        /// <summary>
        /// Lấy danh sách danh mục với số lượng sản phẩm/dịch vụ (để sử dụng với converter).
        /// </summary>
        /// <returns>Tuple chứa danh sách categories và dictionary đếm số lượng</returns>
        public (List<ProductServiceCategoryDto> Categories, Dictionary<Guid, int> Counts) GetCategoriesWithCounts()
        {
            var categories = GetAll();
            var counts = GetProductCountByCategory();
            return (categories, counts);
        }

        /// <summary>
        /// Lấy danh sách danh mục với số lượng sản phẩm/dịch vụ (Async).
        /// </summary>
        /// <returns>Task chứa Tuple với danh sách categories và dictionary đếm số lượng</returns>
        public async Task<(List<ProductServiceCategoryDto> Categories, Dictionary<Guid, int> Counts)> GetCategoriesWithCountsAsync()
        {
            var categories = await GetAllAsync();
            var counts = await GetProductCountByCategoryAsync();
            return (categories, counts);
        }

        /// <summary>
        /// Lấy danh mục con của danh mục cha.
        /// </summary>
        /// <param name="parentId">ID danh mục cha (null cho danh mục cấp 1)</param>
        /// <returns>Danh sách danh mục con</returns>
        public List<ProductServiceCategoryDto> GetCategoriesByParent(Guid? parentId)
        {
            return GetDataAccess().GetCategoriesByParent(parentId);
        }

        /// <summary>
        /// Lấy danh mục con của danh mục cha (Async).
        /// </summary>
        /// <param name="parentId">ID danh mục cha (null cho danh mục cấp 1)</param>
        /// <returns>Task chứa danh sách danh mục con</returns>
        public Task<List<ProductServiceCategoryDto>> GetCategoriesByParentAsync(Guid? parentId)
        {
            return GetDataAccess().GetCategoriesByParentAsync(parentId);
        }

        /// <summary>
        /// Lấy tất cả danh mục đang hoạt động (IsActive = true).
        /// </summary>
        /// <returns>Danh sách danh mục active</returns>
        public List<ProductServiceCategoryDto> GetActiveCategories()
        {
            return GetDataAccess().GetActiveCategories();
        }

        /// <summary>
        /// Lấy tất cả danh mục đang hoạt động (Async).
        /// </summary>
        /// <returns>Task chứa danh sách danh mục active</returns>
        public Task<List<ProductServiceCategoryDto>> GetActiveCategoriesAsync()
        {
            return GetDataAccess().GetActiveCategoriesAsync();
        }

        /// <summary>
        /// Lấy danh mục cấp 1 (root categories).
        /// </summary>
        /// <returns>Danh sách danh mục root</returns>
        public List<ProductServiceCategoryDto> GetRootCategories()
        {
            return GetCategoriesByParent(null);
        }

        /// <summary>
        /// Lấy danh mục cấp 1 (Async).
        /// </summary>
        /// <returns>Task chứa danh sách danh mục root</returns>
        public Task<List<ProductServiceCategoryDto>> GetRootCategoriesAsync()
        {
            return GetCategoriesByParentAsync(null);
        }

        /// <summary>
        /// Lấy danh mục cấp 1 đang hoạt động.
        /// </summary>
        /// <returns>Danh sách danh mục root active</returns>
        public List<ProductServiceCategoryDto> GetActiveRootCategories()
        {
            var active = GetActiveCategories();
            return active.Where(x => x.ParentId == null)
                .OrderBy(x => x.SortOrder ?? int.MaxValue)
                .ThenBy(x => x.CategoryName)
                .ToList();
        }

        /// <summary>
        /// Lấy danh mục cấp 1 đang hoạt động (Async).
        /// </summary>
        /// <returns>Task chứa danh sách danh mục root active</returns>
        public async Task<List<ProductServiceCategoryDto>> GetActiveRootCategoriesAsync()
        {
            var active = await GetActiveCategoriesAsync();
            return await Task.Run(() => active.Where(x => x.ParentId == null)
                .OrderBy(x => x.SortOrder ?? int.MaxValue)
                .ThenBy(x => x.CategoryName)
                .ToList());
        }

        /// <summary>
        /// Lấy cây phân cấp danh mục đầy đủ (root + children).
        /// </summary>
        /// <returns>Dictionary với Key là danh mục root, Value là danh sách con</returns>
        public Dictionary<ProductServiceCategoryDto, List<ProductServiceCategoryDto>> GetCategoryHierarchy()
        {
            var allCategories = GetAll();
            var result = new Dictionary<ProductServiceCategoryDto, List<ProductServiceCategoryDto>>();
            var roots = allCategories.Where(x => x.ParentId == null)
                .OrderBy(x => x.SortOrder ?? int.MaxValue)
                .ToList();
            
            foreach (var root in roots)
            {
                var children = allCategories.Where(x => x.ParentId == root.Id)
                    .OrderBy(x => x.SortOrder ?? int.MaxValue)
                    .ThenBy(x => x.CategoryName)
                    .ToList();
                result[root] = children;
            }

            return result;
        }

        /// <summary>
        /// Lấy cây phân cấp danh mục đầy đủ (Async).
        /// </summary>
        /// <returns>Task chứa Dictionary với Key là danh mục root, Value là danh sách con</returns>
        public async Task<Dictionary<ProductServiceCategoryDto, List<ProductServiceCategoryDto>>> GetCategoryHierarchyAsync()
        {
            var allCategories = await GetAllAsync();
            return await Task.Run(() =>
            {
                var result = new Dictionary<ProductServiceCategoryDto, List<ProductServiceCategoryDto>>();
                var roots = allCategories.Where(x => x.ParentId == null)
                    .OrderBy(x => x.SortOrder ?? int.MaxValue)
                    .ToList();
                
                foreach (var root in roots)
                {
                    var children = allCategories.Where(x => x.ParentId == root.Id)
                        .OrderBy(x => x.SortOrder ?? int.MaxValue)
                        .ThenBy(x => x.CategoryName)
                        .ToList();
                    result[root] = children;
                }

                return result;
            });
        }

        #endregion

        #region ========== CREATE OPERATIONS ==========

        /// <summary>
        /// Thêm mới danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="dto">ProductServiceCategoryDto</param>
        public void Insert(ProductServiceCategoryDto dto)
        {
            GetDataAccess().SaveOrUpdate(dto);
        }

        #endregion

        #region ========== UPDATE OPERATIONS ==========

        /// <summary>
        /// Cập nhật danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="dto">ProductServiceCategoryDto</param>
        public void Update(ProductServiceCategoryDto dto)
        {
            GetDataAccess().SaveOrUpdate(dto);
        }

        /// <summary>
        /// Lưu hoặc cập nhật danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="dto">ProductServiceCategoryDto</param>
        public void SaveOrUpdate(ProductServiceCategoryDto dto)
        {
            GetDataAccess().SaveOrUpdate(dto);
        }

        /// <summary>
        /// Cập nhật trạng thái IsActive của danh mục.
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <param name="isActive">Trạng thái mới</param>
        public void UpdateCategoryStatus(Guid categoryId, bool isActive)
        {
            var category = GetById(categoryId);
            if (category != null)
            {
                category.IsActive = isActive;
                Update(category);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái IsActive của danh mục (Async).
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <param name="isActive">Trạng thái mới</param>
        public async Task UpdateCategoryStatusAsync(Guid categoryId, bool isActive)
        {
            var category = await GetByIdAsync(categoryId);
            if (category != null)
            {
                category.IsActive = isActive;
                Update(category);
            }
        }

        /// <summary>
        /// Cập nhật thứ tự sắp xếp (SortOrder) của danh mục.
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <param name="sortOrder">Thứ tự mới</param>
        public void UpdateCategorySortOrder(Guid categoryId, int sortOrder)
        {
            var category = GetById(categoryId);
            if (category != null)
            {
                category.SortOrder = sortOrder;
                Update(category);
            }
        }

        /// <summary>
        /// Cập nhật thứ tự sắp xếp (Async).
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <param name="sortOrder">Thứ tự mới</param>
        public async Task UpdateCategorySortOrderAsync(Guid categoryId, int sortOrder)
        {
            var category = await GetByIdAsync(categoryId);
            if (category != null)
            {
                category.SortOrder = sortOrder;
                Update(category);
            }
        }

        #endregion

        #region ========== VALIDATION & EXISTS CHECKS ==========

        /// <summary>
        /// Kiểm tra tên danh mục có tồn tại không.
        /// </summary>
        /// <param name="categoryName">Tên danh mục cần kiểm tra</param>
        /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsCategoryNameExists(string categoryName, Guid? excludeId = null)
        {
            return GetDataAccess().IsCategoryNameExists(categoryName, excludeId);
        }

        /// <summary>
        /// Kiểm tra tên danh mục có tồn tại không (Async).
        /// </summary>
        /// <param name="categoryName">Tên danh mục cần kiểm tra</param>
        /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
        /// <returns>Task chứa True nếu tồn tại, False nếu không</returns>
        public Task<bool> IsCategoryNameExistsAsync(string categoryName, Guid? excludeId = null)
        {
            return GetDataAccess().IsCategoryNameExistsAsync(categoryName, excludeId);
        }

        /// <summary>
        /// Kiểm tra mã danh mục có tồn tại không.
        /// </summary>
        /// <param name="categoryCode">Mã danh mục cần kiểm tra</param>
        /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsCategoryCodeExists(string categoryCode, Guid? excludeId = null)
        {
            return GetDataAccess().IsCategoryCodeExists(categoryCode, excludeId);
        }

        /// <summary>
        /// Kiểm tra mã danh mục có tồn tại không (Async).
        /// </summary>
        /// <param name="categoryCode">Mã danh mục cần kiểm tra</param>
        /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
        /// <returns>Task chứa True nếu tồn tại, False nếu không</returns>
        public Task<bool> IsCategoryCodeExistsAsync(string categoryCode, Guid? excludeId = null)
        {
            return GetDataAccess().IsCategoryCodeExistsAsync(categoryCode, excludeId);
        }

        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa danh mục sản phẩm/dịch vụ theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục cần xóa</param>
        public void Delete(Guid id)
        {
            GetDataAccess().DeleteCategory(id);
        }

        /// <summary>
        /// Xóa nhiều danh mục với logic di chuyển sản phẩm/dịch vụ sang "Phân loại chưa đặt tên".
        /// Xóa theo thứ tự từ level cao xuống level thấp để tránh lỗi foreign key constraint.
        /// </summary>
        /// <param name="categoryIds">Danh sách ID danh mục cần xóa</param>
        public async Task DeleteCategoriesWithProductMigration(List<Guid> categoryIds)
        {
            if (categoryIds == null || categoryIds.Count == 0) return;

            var allCategories = await GetDataAccess().GetAllAsync();
            var categoryDict = allCategories.ToDictionary(c => c.Id);

            var categoriesToDelete = categoryIds.Select(id => 
            {
                var category = categoryDict.TryGetValue(id, out var value) ? value : null;
                if (category == null) return null;
                
                var level = CalculateCategoryLevel(category, categoryDict);
                return new { Category = category, Level = level };
            }).Where(x => x != null).OrderByDescending(x => x.Level).ToList();

            foreach (var item in categoriesToDelete)
            {
                try
                {
                    Delete(item.Category.Id);
                }
                catch (Exception ex)
                {
                    throw new Exception($"Lỗi xóa category {item.Category.CategoryName}: {ex.Message}", ex);
                }
            }
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Tính level của category trong cây phân cấp.
        /// </summary>
        /// <param name="category">Danh mục cần tính</param>
        /// <param name="categoryDict">Dictionary các danh mục</param>
        /// <returns>Level trong cây phân cấp (0 = root)</returns>
        private int CalculateCategoryLevel(ProductServiceCategoryDto category, 
            Dictionary<Guid, ProductServiceCategoryDto> categoryDict)
        {
            int level = 0;
            var current = category;
            while (current.ParentId.HasValue && categoryDict.ContainsKey(current.ParentId.Value))
            {
                level++;
                current = categoryDict[current.ParentId.Value];
                if (level > 10) break; // Tránh infinite loop
            }
            return level;
        }

        #endregion
    }
}
