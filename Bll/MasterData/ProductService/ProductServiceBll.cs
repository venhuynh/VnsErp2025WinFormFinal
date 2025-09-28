using System;
using System.Collections.Generic;
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

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo ProductServiceBll
        /// </summary>
        public ProductServiceBll()
        {
            _dataAccess = new ProductServiceDataAccess();
            _categoryDataAccess = new ProductServiceCategoryDataAccess();
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