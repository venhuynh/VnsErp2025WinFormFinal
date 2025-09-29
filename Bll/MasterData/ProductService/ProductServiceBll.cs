using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.MasterData.ProductServiceDataAccess;
using Dal.DataContext;
using Dal.Exceptions;

namespace Bll.MasterData.ProductService
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