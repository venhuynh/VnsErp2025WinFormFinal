using Dal.DataAccess.MasterData.Partner;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataAccess.Implementations.MasterData.PartnerRepository;

namespace Bll.MasterData.Customer
{
    /// <summary>
    /// Business logic layer cho BusinessPartnerCategory. 
    /// Cung cấp các method để lấy danh mục đối tác và đếm số lượng đối tác theo từng danh mục.
    /// </summary>
    public class BusinessPartnerCategoryBll
    {
        private readonly BusinessPartnerRepository _businessPartnerCategoryDataAccess = new();

        /// <summary>
        /// Lấy tất cả danh mục đối tác.
        /// </summary>
        /// <returns>Danh sách BusinessPartnerCategory</returns>
        public List<BusinessPartnerCategory> GetAll()
        {
            return _businessPartnerCategoryDataAccess.GetAll();
        }

        /// <summary>
        /// Lấy tất cả danh mục đối tác (Async).
        /// </summary>
        /// <returns>Task chứa danh sách BusinessPartnerCategory</returns>
        public Task<List<BusinessPartnerCategory>> GetAllAsync()
        {
            return _businessPartnerCategoryDataAccess.GetAllAsync();
        }

        /// <summary>
        /// Lấy danh mục đối tác theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục</param>
        /// <returns>BusinessPartnerCategory hoặc null</returns>
        public BusinessPartnerCategory GetById(Guid id)
        {
            return _businessPartnerCategoryDataAccess.GetById(id);
        }

        /// <summary>
        /// Đếm số lượng đối tác theo từng danh mục.
        /// </summary>
        /// <returns>Dictionary với Key là CategoryId, Value là số lượng đối tác</returns>
        public Dictionary<Guid, int> GetPartnerCountByCategory()
        {
            return _businessPartnerCategoryDataAccess.GetPartnerCountByCategory();
        }

        /// <summary>
        /// Đếm số lượng đối tác theo từng danh mục (Async).
        /// </summary>
        /// <returns>Task chứa Dictionary với Key là CategoryId, Value là số lượng đối tác</returns>
        public Task<Dictionary<Guid, int>> GetPartnerCountByCategoryAsync()
        {
            return _businessPartnerCategoryDataAccess.GetPartnerCountByCategoryAsync();
        }

        /// <summary>
        /// Lấy danh sách danh mục với số lượng đối tác (để sử dụng với converter).
        /// </summary>
        /// <returns>Tuple chứa danh sách categories và dictionary đếm số lượng</returns>
        public (List<BusinessPartnerCategory> Categories, Dictionary<Guid, int> Counts) GetCategoriesWithCounts()
        {
            var categories = GetAll();
            var counts = GetPartnerCountByCategory();
            return (categories, counts);
        }

        /// <summary>
        /// Lấy danh sách danh mục với số lượng đối tác (Async).
        /// </summary>
        /// <returns>Task chứa Tuple với danh sách categories và dictionary đếm số lượng</returns>
        public async Task<(List<BusinessPartnerCategory> Categories, Dictionary<Guid, int> Counts)> GetCategoriesWithCountsAsync()
        {
            var categories = await GetAllAsync();
            var counts = await GetPartnerCountByCategoryAsync();
            return (categories, counts);
        }


        /// <summary>
        /// Kiểm tra xem danh mục có đối tác nào không.
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>True nếu có đối tác, False nếu không</returns>
        public bool HasPartners(Guid categoryId)
        {
            return _businessPartnerCategoryDataAccess.HasPartners(categoryId);
        }

        /// <summary>
        /// Lấy số lượng đối tác của một danh mục cụ thể.
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Số lượng đối tác</returns>
        public int GetPartnerCount(Guid categoryId)
        {
            return _businessPartnerCategoryDataAccess.GetPartnerCount(categoryId);
        }

        /// <summary>
        /// Thêm mới danh mục đối tác.
        /// </summary>
        /// <param name="category">Danh mục cần thêm</param>
        public void Insert(BusinessPartnerCategory category)
        {
            _businessPartnerCategoryDataAccess.SaveOrUpdate(category);
        }

        /// <summary>
        /// Cập nhật danh mục đối tác.
        /// </summary>
        /// <param name="category">Danh mục cần cập nhật</param>
        public void Update(BusinessPartnerCategory category)
        {
            _businessPartnerCategoryDataAccess.SaveOrUpdate(category);
        }

        /// <summary>
        /// Kiểm tra tên danh mục có tồn tại không.
        /// </summary>
        /// <param name="categoryName">Tên danh mục cần kiểm tra</param>
        /// <param name="excludeId">ID danh mục cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsCategoryNameExists(string categoryName, Guid excludeId)
        {
            return _businessPartnerCategoryDataAccess.IsCategoryNameExists(categoryName, excludeId);
        }

        /// <summary>
        /// Xóa danh mục đối tác theo ID.
        /// </summary>
        /// <param name="id">ID của danh mục cần xóa</param>
        public void Delete(Guid id)
        {
            _businessPartnerCategoryDataAccess.DeleteCategory(id);
        }
    }
}
