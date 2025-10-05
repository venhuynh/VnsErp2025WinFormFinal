using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.MasterData.ProductServiceDal;

namespace Bll.MasterData.ProductServiceBll
{
    /// <summary>
    /// Business Logic Layer cho ProductService.
    /// Cung cấp các phương thức xử lý nghiệp vụ cho sản phẩm/dịch vụ.
    /// </summary>
    public class ProductServiceBll
    {
        #region Fields

        private readonly ProductServiceDataAccess _dataAccess;
        private readonly ProductServiceCategoryDataAccess _categoryDataAccess;
        private readonly ProductImageBll _productImageBll;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo ProductServiceBll
        /// </summary>
        public ProductServiceBll()
        {
            _dataAccess = new ProductServiceDataAccess();
            _categoryDataAccess = new ProductServiceCategoryDataAccess();
            _productImageBll = new ProductImageBll();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Lấy tất cả sản phẩm/dịch vụ (Sync)
        /// </summary>
        /// <returns>Danh sách ProductService entities</returns>
        public List<Dal.DataContext.ProductService> GetAll()
        {
            try
            {
                return _dataAccess.GetAll();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy danh sách sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả sản phẩm/dịch vụ (Async)
        /// </summary>
        /// <returns>Danh sách ProductService entities</returns>
        public async Task<List<Dal.DataContext.ProductService>> GetAllAsync()
        {
            try
            {
                return await Task.Run(() => _dataAccess.GetAll());
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy danh sách sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo ID
        /// </summary>
        /// <param name="id">ID sản phẩm/dịch vụ</param>
        /// <returns>ProductService entity</returns>
        public Dal.DataContext.ProductService GetById(Guid id)
        {
            try
            {
                return _dataAccess.GetById(id);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy sản phẩm/dịch vụ theo ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu sản phẩm/dịch vụ (thêm mới hoặc cập nhật)
        /// </summary>
        /// <param name="productService">ProductService entity cần lưu</param>
        public void Save(Dal.DataContext.ProductService productService)
        {
            try
            {
                if (productService == null)
                    throw new ArgumentNullException(nameof(productService));

                _dataAccess.SaveOrUpdate(productService);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lưu sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productService">ProductService entity cần lưu hoặc cập nhật</param>
        public void SaveOrUpdate(Dal.DataContext.ProductService productService)
        {
            try
            {
                if (productService == null)
                    throw new ArgumentNullException(nameof(productService));

                _dataAccess.SaveOrUpdate(productService);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lưu hoặc cập nhật sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        // Phương thức SaveOrUpdateWithImage đã được loại bỏ vì không còn cần thiết
        // Thay vào đó sử dụng SaveOrUpdate thông thường với ThumbnailImage

        /// <summary>
        /// Xóa sản phẩm/dịch vụ theo ID
        /// </summary>
        /// <param name="id">ID sản phẩm/dịch vụ cần xóa</param>
        public void Delete(Guid id)
        {
            try
            {
                var result = _dataAccess.DeleteProductService(id);
                if (!result)
                {
                    throw new BusinessLogicException($"Không thể xóa sản phẩm/dịch vụ với ID {id}");
                }
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi xóa sản phẩm/dịch vụ với ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tên danh mục theo ID
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>Tên danh mục hoặc null nếu không tìm thấy</returns>
        public string GetCategoryName(Guid? categoryId)
        {
            try
            {
                if (categoryId == null || categoryId == Guid.Empty)
                    return null;

                var category = _categoryDataAccess.GetById(categoryId.Value);
                return category?.CategoryName;
            }
            catch (Exception)
            {
                // Log lỗi nhưng không throw để không ảnh hưởng đến việc hiển thị danh sách
                // Có thể sử dụng logger ở đây nếu cần
                return null;
            }
        }

        /// <summary>
        /// Đếm số lượng biến thể của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productServiceId">ID sản phẩm/dịch vụ</param>
        /// <returns>Số lượng biến thể</returns>
        public int GetVariantCount(Guid productServiceId)
        {
            try
            {
                return _dataAccess.GetVariantCount(productServiceId);
            }
            catch (Exception)
            {
                // Trả về 0 nếu có lỗi để không ảnh hưởng đến việc hiển thị
                return 0;
            }
        }

        /// <summary>
        /// Đếm số lượng hình ảnh của sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productServiceId">ID sản phẩm/dịch vụ</param>
        /// <returns>Số lượng hình ảnh</returns>
        public int GetImageCount(Guid productServiceId)
        {
            try
            {
                return _dataAccess.GetImageCount(productServiceId);
            }
            catch (Exception)
            {
                // Trả về 0 nếu có lỗi để không ảnh hưởng đến việc hiển thị
                return 0;
            }
        }

        /// <summary>
        /// Đếm số lượng biến thể và hình ảnh cho nhiều sản phẩm/dịch vụ
        /// </summary>
        /// <param name="productServiceIds">Danh sách ID sản phẩm/dịch vụ</param>
        /// <returns>Dictionary với key là ProductServiceId và value là (VariantCount, ImageCount)</returns>
        public Dictionary<Guid, (int VariantCount, int ImageCount)> GetCountsForProducts(List<Guid> productServiceIds)
        {
            try
            {
                return _dataAccess.GetCountsForProducts(productServiceIds);
            }
            catch (Exception)
            {
                // Trả về dictionary rỗng nếu có lỗi
                return new Dictionary<Guid, (int, int)>();
            }
        }

        /// <summary>
        /// Tự động tạo mã sản phẩm dựa trên danh mục.
        /// Format: [CategoryCode] + [4 số từ 0001-9999]
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Mã sản phẩm được tạo</returns>
        public string GenerateProductCode(Guid categoryId)
        {
            try
            {
                // Lấy mã danh mục
                var categoryCode = GetCategoryCode(categoryId);
                if (string.IsNullOrWhiteSpace(categoryCode))
                {
                    return string.Empty;
                }

                // Tìm số tiếp theo trong danh mục này
                var nextNumber = _dataAccess.GetNextProductNumber(categoryId, categoryCode);

                // Tạo mã hoàn chỉnh
                return $"{categoryCode}{nextNumber:D4}";
            }
            catch (Exception)
            {
                // Trả về chuỗi rỗng nếu có lỗi
                return string.Empty;
            }
        }

        /// <summary>
        /// Lấy mã danh mục từ ID danh mục.
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Mã danh mục</returns>
        private string GetCategoryCode(Guid categoryId)
        {
            try
            {
                return _dataAccess.GetCategoryCode(categoryId);
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Kiểm tra mã sản phẩm/dịch vụ có tồn tại không.
        /// </summary>
        /// <param name="code">Mã sản phẩm/dịch vụ cần kiểm tra</param>
        /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsCodeExists(string code, Guid? excludeId = null)
        {
            return _dataAccess.IsCodeExists(code, excludeId);
        }

        /// <summary>
        /// Kiểm tra tên sản phẩm/dịch vụ có tồn tại không.
        /// </summary>
        /// <param name="name">Tên sản phẩm/dịch vụ cần kiểm tra</param>
        /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsNameExists(string name, Guid? excludeId = null)
        {
            return _dataAccess.IsNameExists(name, excludeId);
        }

        #endregion

        #region Optimization Methods

        /// <summary>
        /// Lấy số lượng tổng cộng với filter
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="categoryId">Filter theo category</param>
        /// <param name="isService">Filter theo loại</param>
        /// <param name="isActive">Filter theo trạng thái</param>
        /// <returns>Số lượng tổng cộng</returns>
        public async Task<int> GetCountAsync(
            string searchText = null,
            Guid? categoryId = null,
            bool? isService = null,
            bool? isActive = null)
        {
            try
            {
                return await _dataAccess.GetCountAsync(searchText, categoryId, isService, isActive);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi đếm số lượng sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy dữ liệu phân trang
        /// </summary>
        /// <param name="pageIndex">Trang hiện tại (0-based)</param>
        /// <param name="pageSize">Số dòng mỗi trang</param>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="categoryId">Filter theo category</param>
        /// <param name="isService">Filter theo loại</param>
        /// <param name="isActive">Filter theo trạng thái</param>
        /// <returns>Danh sách entities</returns>
        public async Task<List<Dal.DataContext.ProductService>> GetPagedAsync(
            int pageIndex,
            int pageSize,
            string searchText = null,
            Guid? categoryId = null,
            bool? isService = null,
            bool? isActive = null)
        {
            try
            {
                return await _dataAccess.GetPagedAsync(pageIndex, pageSize, searchText, categoryId, isService, isActive);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy dữ liệu phân trang: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy dữ liệu thumbnail image cho lazy loading
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>Dữ liệu ảnh thumbnail</returns>
        public byte[] GetThumbnailImageData(Guid productId)
        {
            try
            {
                return _dataAccess.GetThumbnailImageData(productId);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy dữ liệu ảnh thumbnail: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách sản phẩm với search và filter (optimized)
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <param name="categoryId">Filter theo category</param>
        /// <param name="isService">Filter theo loại</param>
        /// <param name="isActive">Filter theo trạng thái</param>
        /// <param name="orderBy">Sắp xếp theo</param>
        /// <param name="orderDirection">Hướng sắp xếp</param>
        /// <returns>Danh sách entities</returns>
        public async Task<List<Dal.DataContext.ProductService>> GetFilteredAsync(
            string searchText = null,
            Guid? categoryId = null,
            bool? isService = null,
            bool? isActive = null,
            string orderBy = "Name",
            string orderDirection = "ASC")
        {
            try
            {
                return await _dataAccess.GetFilteredAsync(searchText, categoryId, isService, isActive, orderBy, orderDirection);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi lấy dữ liệu với filter: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy counts cho nhiều sản phẩm cùng lúc (optimized async version)
        /// </summary>
        /// <param name="productIds">Danh sách ID sản phẩm</param>
        /// <returns>Dictionary chứa counts</returns>
        public async Task<Dictionary<Guid, (int VariantCount, int ImageCount)>> GetCountsForProductsAsync(List<Guid> productIds)
        {
            try
            {
                if (productIds == null || !productIds.Any())
                    return new Dictionary<Guid, (int, int)>();

                return await _dataAccess.GetCountsForProductsAsync(productIds);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi khi đếm số lượng cho nhiều sản phẩm: {ex.Message}", ex);
            }
        }

        #endregion

        #region Search and Filter Methods

        /// <summary>
        /// Tìm kiếm sản phẩm/dịch vụ trong toàn bộ database (Sync)
        /// </summary>
        /// <param name="searchText">Text tìm kiếm</param>
        /// <returns>Danh sách kết quả tìm kiếm</returns>
        public List<Dal.DataContext.ProductService> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                    return new List<Dal.DataContext.ProductService>();

                return _dataAccess.Search(searchText);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi tìm kiếm sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm/dịch vụ trong toàn bộ database (Async)
        /// </summary>
        /// <param name="searchText">Text tìm kiếm</param>
        /// <returns>Danh sách kết quả tìm kiếm</returns>
        public async Task<List<Dal.DataContext.ProductService>> SearchAsync(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                    return new List<Dal.DataContext.ProductService>();

                return await _dataAccess.SearchAsync(searchText);
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi tìm kiếm sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách mã code unique
        /// </summary>
        /// <returns>Danh sách mã code unique</returns>
        public async Task<List<object>> GetUniqueCodesAsync()
        {
            try
            {
                return await _dataAccess.GetUniqueCodesAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách mã code unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách tên unique
        /// </summary>
        /// <returns>Danh sách tên unique</returns>
        public async Task<List<object>> GetUniqueNamesAsync()
        {
            try
            {
                return await _dataAccess.GetUniqueNamesAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách tên unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách tên danh mục unique
        /// </summary>
        /// <returns>Danh sách tên danh mục unique</returns>
        public async Task<List<object>> GetUniqueCategoryNamesAsync()
        {
            try
            {
                return await _dataAccess.GetUniqueCategoryNamesAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách tên danh mục unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách loại hiển thị unique
        /// </summary>
        /// <returns>Danh sách loại hiển thị unique</returns>
        public async Task<List<object>> GetUniqueTypeDisplaysAsync()
        {
            try
            {
                return await _dataAccess.GetUniqueTypeDisplaysAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách loại hiển thị unique: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách trạng thái hiển thị unique
        /// </summary>
        /// <returns>Danh sách trạng thái hiển thị unique</returns>
        public async Task<List<object>> GetUniqueStatusDisplaysAsync()
        {
            try
            {
                return await _dataAccess.GetUniqueStatusDisplaysAsync();
            }
            catch (Exception ex)
            {
                throw new BusinessLogicException($"Lỗi lấy danh sách trạng thái hiển thị unique: {ex.Message}", ex);
            }
        }

        #endregion
    }

    /// <summary>
    /// Exception cho Business Logic Layer
    /// </summary>
    public class BusinessLogicException : Exception
    {
        public BusinessLogicException(string message) : base(message) { }
        public BusinessLogicException(string message, Exception innerException) : base(message, innerException) { }
    }
}