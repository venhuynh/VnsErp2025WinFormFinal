using Dal.DataAccess.MasterData.ProductService;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bll.MasterData.ProductService
{
    /// <summary>
    /// Business logic layer cho ProductServiceCategory. 
    /// Cung cấp các method để lấy danh mục sản phẩm/dịch vụ và đếm số lượng sản phẩm/dịch vụ theo từng danh mục.
    /// </summary>
    public class ProductServiceCategoryBll
    {
        private readonly ProductServiceCategoryDataAccess _productServiceCategoryDataAccess = new();

        /// <summary>
        /// Lấy tất cả danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <returns>Danh sách ProductServiceCategory</returns>
        public List<ProductServiceCategory> GetAll()
        {
            return _productServiceCategoryDataAccess.GetAll();
        }

        /// <summary>
        /// Lấy tất cả danh mục sản phẩm/dịch vụ (Async).
        /// </summary>
        /// <returns>Task chứa danh sách ProductServiceCategory</returns>
        public Task<List<ProductServiceCategory>> GetAllAsync()
        {
            return _productServiceCategoryDataAccess.GetAllAsync();
        }

        /// <summary>
        /// Lấy danh mục sản phẩm/dịch vụ theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục</param>
        /// <returns>ProductServiceCategory hoặc null</returns>
        public ProductServiceCategory GetById(Guid id)
        {
            return _productServiceCategoryDataAccess.GetById(id);
        }

        /// <summary>
        /// Lấy danh mục sản phẩm/dịch vụ theo ID (Async).
        /// </summary>
        /// <param name="id">ID của danh mục</param>
        /// <returns>Task chứa ProductServiceCategory hoặc null</returns>
        public Task<ProductServiceCategory> GetByIdAsync(Guid id)
        {
            return _productServiceCategoryDataAccess.GetByIdAsync(id);
        }

        /// <summary>
        /// Lấy danh mục sản phẩm/dịch vụ theo tên.
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <returns>ProductServiceCategory hoặc null</returns>
        public ProductServiceCategory GetByName(string categoryName)
        {
            return _productServiceCategoryDataAccess.GetByName(categoryName);
        }

        /// <summary>
        /// Lấy danh mục sản phẩm/dịch vụ theo tên (Async).
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <returns>Task chứa ProductServiceCategory hoặc null</returns>
        public Task<ProductServiceCategory> GetByNameAsync(string categoryName)
        {
            return _productServiceCategoryDataAccess.GetByNameAsync(categoryName);
        }

        /// <summary>
        /// Lấy danh sách danh mục con của một danh mục cha.
        /// </summary>
        /// <param name="parentId">ID danh mục cha</param>
        /// <returns>Danh sách danh mục con</returns>
        public List<ProductServiceCategory> GetChildren(Guid parentId)
        {
            return _productServiceCategoryDataAccess.GetChildren(parentId);
        }

        /// <summary>
        /// Lấy danh sách danh mục con của một danh mục cha (Async).
        /// </summary>
        /// <param name="parentId">ID danh mục cha</param>
        /// <returns>Task chứa danh sách danh mục con</returns>
        public Task<List<ProductServiceCategory>> GetChildrenAsync(Guid parentId)
        {
            return _productServiceCategoryDataAccess.GetChildrenAsync(parentId);
        }

        /// <summary>
        /// Lấy danh sách danh mục gốc (không có ParentId).
        /// </summary>
        /// <returns>Danh sách danh mục gốc</returns>
        public List<ProductServiceCategory> GetRootCategories()
        {
            return _productServiceCategoryDataAccess.GetRootCategories();
        }

        /// <summary>
        /// Lấy danh sách danh mục gốc (không có ParentId) (Async).
        /// </summary>
        /// <returns>Task chứa danh sách danh mục gốc</returns>
        public Task<List<ProductServiceCategory>> GetRootCategoriesAsync()
        {
            return _productServiceCategoryDataAccess.GetRootCategoriesAsync();
        }

        /// <summary>
        /// Đếm số lượng sản phẩm/dịch vụ theo từng danh mục.
        /// </summary>
        /// <returns>Dictionary với Key là CategoryId, Value là số lượng sản phẩm/dịch vụ</returns>
        public Dictionary<Guid, int> GetProductCountByCategory()
        {
            return _productServiceCategoryDataAccess.GetProductCountByCategory();
        }

        /// <summary>
        /// Đếm số lượng sản phẩm/dịch vụ theo từng danh mục (Async).
        /// </summary>
        /// <returns>Task chứa Dictionary với Key là CategoryId, Value là số lượng sản phẩm/dịch vụ</returns>
        public Task<Dictionary<Guid, int>> GetProductCountByCategoryAsync()
        {
            return _productServiceCategoryDataAccess.GetProductCountByCategoryAsync();
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
        /// Kiểm tra xem danh mục có sản phẩm/dịch vụ nào không.
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>True nếu có sản phẩm/dịch vụ, False nếu không</returns>
        public bool HasProducts(Guid categoryId)
        {
            return _productServiceCategoryDataAccess.HasProducts(categoryId);
        }

        /// <summary>
        /// Kiểm tra xem danh mục có sản phẩm/dịch vụ nào không (Async).
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Task chứa True nếu có sản phẩm/dịch vụ, False nếu không</returns>
        public Task<bool> HasProductsAsync(Guid categoryId)
        {
            return _productServiceCategoryDataAccess.HasProductsAsync(categoryId);
        }

        /// <summary>
        /// Lấy số lượng sản phẩm/dịch vụ của một danh mục cụ thể.
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Số lượng sản phẩm/dịch vụ</returns>
        public int GetProductCount(Guid categoryId)
        {
            return _productServiceCategoryDataAccess.GetProductCount(categoryId);
        }

        /// <summary>
        /// Lấy số lượng sản phẩm/dịch vụ của một danh mục cụ thể (Async).
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Task chứa số lượng sản phẩm/dịch vụ</returns>
        public Task<int> GetProductCountAsync(Guid categoryId)
        {
            return _productServiceCategoryDataAccess.GetProductCountAsync(categoryId);
        }

        /// <summary>
        /// Thêm mới danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="category">Danh mục cần thêm</param>
        public void Insert(ProductServiceCategory category)
        {
            _productServiceCategoryDataAccess.Add(category);
        }

        /// <summary>
        /// Thêm mới danh mục sản phẩm/dịch vụ (Async).
        /// </summary>
        /// <param name="category">Danh mục cần thêm</param>
        /// <returns>Task</returns>
        public Task InsertAsync(ProductServiceCategory category)
        {
            return _productServiceCategoryDataAccess.AddAsync(category);
        }

        /// <summary>
        /// Cập nhật danh mục sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="category">Danh mục cần cập nhật</param>
        public void Update(ProductServiceCategory category)
        {
            _productServiceCategoryDataAccess.Update(category);
        }

        /// <summary>
        /// Cập nhật danh mục sản phẩm/dịch vụ (Async).
        /// </summary>
        /// <param name="category">Danh mục cần cập nhật</param>
        /// <returns>Task</returns>
        public Task UpdateAsync(ProductServiceCategory category)
        {
            return Task.Run(() => _productServiceCategoryDataAccess.Update(category));
        }

        /// <summary>
        /// Cập nhật tên và mô tả danh mục.
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <param name="categoryName">Tên mới</param>
        /// <param name="description">Mô tả mới</param>
        /// <returns>True nếu cập nhật thành công</returns>
        public bool UpdateCategory(Guid id, string categoryName, string description = null)
        {
            return _productServiceCategoryDataAccess.UpdateCategory(id, categoryName, description);
        }

        /// <summary>
        /// Cập nhật tên và mô tả danh mục (Async).
        /// </summary>
        /// <param name="id">ID danh mục</param>
        /// <param name="categoryName">Tên mới</param>
        /// <param name="description">Mô tả mới</param>
        /// <returns>Task chứa True nếu cập nhật thành công</returns>
        public Task<bool> UpdateCategoryAsync(Guid id, string categoryName, string description = null)
        {
            return _productServiceCategoryDataAccess.UpdateCategoryAsync(id, categoryName, description);
        }

        /// <summary>
        /// Kiểm tra tên danh mục có tồn tại không.
        /// </summary>
        /// <param name="categoryName">Tên danh mục cần kiểm tra</param>
        /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsCategoryNameExists(string categoryName, Guid? excludeId = null)
        {
            return _productServiceCategoryDataAccess.IsCategoryNameExists(categoryName, excludeId);
        }

        /// <summary>
        /// Kiểm tra tên danh mục có tồn tại không (Async).
        /// </summary>
        /// <param name="categoryName">Tên danh mục cần kiểm tra</param>
        /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
        /// <returns>Task chứa True nếu tồn tại, False nếu không</returns>
        public Task<bool> IsCategoryNameExistsAsync(string categoryName, Guid? excludeId = null)
        {
            return _productServiceCategoryDataAccess.IsCategoryNameExistsAsync(categoryName, excludeId);
        }

        /// <summary>
        /// Xóa danh mục sản phẩm/dịch vụ theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục cần xóa</param>
        public void Delete(Guid id)
        {
            _productServiceCategoryDataAccess.DeleteCategory(id);
        }

        /// <summary>
        /// Xóa danh mục sản phẩm/dịch vụ theo ID (Async).
        /// </summary>
        /// <param name="id">ID của danh mục cần xóa</param>
        /// <returns>Task</returns>
        public Task DeleteAsync(Guid id)
        {
            return _productServiceCategoryDataAccess.DeleteCategoryAsync(id);
        }

        /// <summary>
        /// Thêm danh mục mới với validation.
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <param name="description">Mô tả</param>
        /// <param name="parentId">ID danh mục cha (tùy chọn)</param>
        /// <returns>Danh mục đã tạo</returns>
        public ProductServiceCategory AddNewCategory(string categoryName, string description = null, Guid? parentId = null)
        {
            return _productServiceCategoryDataAccess.AddNewCategory(categoryName, description, parentId);
        }

        /// <summary>
        /// Thêm danh mục mới với validation (Async).
        /// </summary>
        /// <param name="categoryName">Tên danh mục</param>
        /// <param name="description">Mô tả</param>
        /// <param name="parentId">ID danh mục cha (tùy chọn)</param>
        /// <returns>Task chứa danh mục đã tạo</returns>
        public Task<ProductServiceCategory> AddNewCategoryAsync(string categoryName, string description = null, Guid? parentId = null)
        {
            return _productServiceCategoryDataAccess.AddNewCategoryAsync(categoryName, description, parentId);
        }

    }
}
