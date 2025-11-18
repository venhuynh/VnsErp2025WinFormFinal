using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
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

        /// <summary>
        /// Lấy tất cả danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <returns>Danh sách ProductServiceCategory</returns>
        private List<ProductServiceCategory> GetAll()
        {
            return GetDataAccess().GetAll();
        }

        /// <summary>
        /// Lấy tất cả danh mục sản phẩm/dịch vụ (Async).
        /// </summary>
        /// <returns>Task chứa danh sách ProductServiceCategory</returns>
        public Task<List<ProductServiceCategory>> GetAllAsync()
        {
            return GetDataAccess().GetAllAsync();
        }

        /// <summary>
        /// Lấy danh mục sản phẩm/dịch vụ theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục</param>
        /// <returns>ProductServiceCategory hoặc null</returns>
        public ProductServiceCategory GetById(Guid id)
        {
            return GetDataAccess().GetById(id);
        }

        /// <summary>
        /// Lấy danh mục sản phẩm/dịch vụ theo ID (Async).
        /// </summary>
        /// <param name="id">ID của danh mục</param>
        /// <returns>Task chứa ProductServiceCategory hoặc null</returns>
        public Task<ProductServiceCategory> GetByIdAsync(Guid id)
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
        public (List<ProductServiceCategory> Categories, Dictionary<Guid, int> Counts) GetCategoriesWithCounts()
        {
            var categories = GetAll();
            var counts = GetProductCountByCategory();
            return (categories, counts);
        }

        /// <summary>
        /// Lấy danh sách danh mục với số lượng sản phẩm/dịch vụ (Async).
        /// </summary>
        /// <returns>Task chứa Tuple với danh sách categories và dictionary đếm số lượng</returns>
        public async Task<(List<ProductServiceCategory> Categories, Dictionary<Guid, int> Counts)> GetCategoriesWithCountsAsync()
        {
            var categories = await GetAllAsync();
            var counts = await GetProductCountByCategoryAsync();
            return (categories, counts);
        }


        /// <summary>
        /// Thêm mới danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="category">Danh mục cần thêm</param>
        public void Insert(ProductServiceCategory category)
        {
            GetDataAccess().SaveOrUpdate(category);
        }

        /// <summary>
        /// Cập nhật danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="category">Danh mục cần cập nhật</param>
        public void Update(ProductServiceCategory category)
        {
            GetDataAccess().SaveOrUpdate(category);
        }

        /// <summary>
        /// Lưu hoặc cập nhật danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="category">Danh mục cần lưu hoặc cập nhật</param>
        public void SaveOrUpdate(ProductServiceCategory category)
        {
            GetDataAccess().SaveOrUpdate(category);
        }


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

            // Lấy tất cả categories để xác định thứ tự xóa
            var allCategories = await GetDataAccess().GetAllAsync();
            var categoryDict = allCategories.ToDictionary(c => c.Id);

            // Tạo danh sách categories cần xóa với thông tin level
            var categoriesToDelete = categoryIds.Select(id => 
            {
                var category = categoryDict.TryGetValue(id, out var value) ? value : null;
                if (category == null) return null;
                
                // Tính level để xác định thứ tự xóa (level cao hơn = xóa trước)
                var level = CalculateCategoryLevel(category, categoryDict);
                return new { Category = category, Level = level };
            }).Where(x => x != null).OrderByDescending(x => x.Level).ToList();

            // Xóa theo thứ tự từ level cao xuống level thấp
            foreach (var item in categoriesToDelete)
            {
                try
                {
                    // Xóa danh mục (logic migration đã được xử lý trong DataAccess)
                    Delete(item.Category.Id);
                }
                catch (Exception ex)
                {
                    // Log lỗi nhưng tiếp tục xóa các item khác
                    throw new Exception($"Lỗi xóa category {item.Category.CategoryName}: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Tính level của category trong cây phân cấp.
        /// </summary>
        private int CalculateCategoryLevel(ProductServiceCategory category, 
            Dictionary<Guid, ProductServiceCategory> categoryDict)
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

    }
}
