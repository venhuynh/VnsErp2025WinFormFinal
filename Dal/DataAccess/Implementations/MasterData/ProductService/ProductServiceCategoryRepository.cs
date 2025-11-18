using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;

namespace Dal.DataAccess.MasterData.ProductServiceDal
{
    /// <summary>
    /// Data Access cho thực thể ProductServiceCategory (LINQ to SQL trên VnsErp2025DataContext).
    /// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
    /// </summary>
    public class ProductServiceCategoryRepository : BaseDataAccess<ProductServiceCategory>
    {
        #region Constructors

        /// <summary>
        /// Khởi tạo mặc định.
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ProductServiceCategoryRepository(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Khởi tạo với connection string.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ProductServiceCategoryRepository(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        #endregion

        #region Create


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
        protected override ProductServiceCategory GetById(object id)
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


        #endregion

        #region Update


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

                using var context = CreateContext();
                var query = context.ProductServiceCategories.Where(x => x.CategoryCode == categoryCode.Trim());
                
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);

                return query.Any();
            }
            catch (Exception ex)
            {
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

                using var context = CreateContext();
                var query = context.ProductServiceCategories.Where(x => x.CategoryCode == categoryCode.Trim());
                
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);

                return await Task.Run(() => query.Any());
            }
            catch (Exception ex)
            {
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


        #endregion

        /// <summary>
        /// Lưu hoặc cập nhật danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="category">Danh mục cần lưu hoặc cập nhật</param>
        public void SaveOrUpdate(ProductServiceCategory category)
        {
            if (category == null) throw new ArgumentNullException(nameof(category));
            
            using var context = CreateContext();
            var existing = category.Id != Guid.Empty ? context.ProductServiceCategories.FirstOrDefault(x => x.Id == category.Id) : null;
            
            if (existing == null)
            {
                // Thêm mới
                if (category.Id == Guid.Empty)
                    category.Id = Guid.NewGuid();
                context.ProductServiceCategories.InsertOnSubmit(category);
            }
            else
            {
                // Cập nhật
                existing.CategoryName = category.CategoryName;
                existing.Description = category.Description;
                existing.ParentId = category.ParentId;
            }
            
            context.SubmitChanges();
        }
    }
}
