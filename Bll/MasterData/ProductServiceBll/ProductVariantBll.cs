using Common.Appconfig;
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
    /// Business Logic Layer cho ProductVariant.
    /// Cung cấp các phương thức xử lý nghiệp vụ cho biến thể sản phẩm.
    /// </summary>
    public class ProductVariantBll
    {
        #region Fields

        private IProductVariantRepository _dataAccess;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo ProductVariantBll
        /// </summary>
        public ProductVariantBll()
        {
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IProductVariantRepository GetDataAccess()
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

                            _dataAccess = new ProductVariantRepository(globalConnectionString);
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

        #region Public Methods

        /// <summary>
        /// Lấy biến thể theo ID
        /// </summary>
        /// <param name="id">ID biến thể</param>
        /// <returns>ProductVariant entity</returns>
        public ProductVariant GetById(Guid id)
        {
            try
            {
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo ID (Async)
        /// </summary>
        /// <param name="id">ID biến thể</param>
        /// <returns>ProductVariant entity</returns>
        public async Task<ProductVariant> GetByIdAsync(Guid id)
        {
            try
            {
                return await GetDataAccess().GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy biến thể theo ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách biến thể theo ProductId
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>Danh sách biến thể</returns>
        public List<ProductVariant> GetByProductId(Guid productId)
        {
            try
            {
                return GetDataAccess().GetByProductId(productId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã biến thể có trùng không
        /// </summary>
        /// <param name="variantCode">Mã biến thể</param>
        /// <param name="excludeId">ID biến thể cần loại trừ (khi edit)</param>
        /// <returns>True nếu trùng</returns>
        private bool IsVariantCodeExists(string variantCode, Guid? excludeId = null)
        {
            try
            {
                return GetDataAccess().IsVariantCodeExists(variantCode, excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi kiểm tra mã biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu biến thể (tạo mới hoặc cập nhật)
        /// </summary>
        /// <param name="variant">Entity biến thể</param>
        /// <param name="attributeValues">Danh sách giá trị thuộc tính (AttributeId, Value)</param>
        /// <returns>ID biến thể đã lưu</returns>
        public async Task<Guid> SaveAsync(ProductVariant variant, List<(Guid AttributeId, string Value)> attributeValues)
        {
            try
            {
                // Validate dữ liệu
                ValidateVariantData(attributeValues);

                // Kiểm tra trùng mã biến thể
                if (IsVariantCodeExists(variant.VariantCode, variant.Id == Guid.Empty ? null : variant.Id))
                {
                    throw new Exception($"Mã biến thể '{variant.VariantCode}' đã tồn tại. Vui lòng chọn mã khác.");
                }

                // Lưu biến thể
                var savedVariantId = await GetDataAccess().SaveAsync(variant, attributeValues);

                return savedVariantId;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lưu biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa biến thể
        /// </summary>
        /// <param name="id">ID biến thể</param>
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                await GetDataAccess().DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi xóa biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách giá trị thuộc tính của biến thể
        /// </summary>
        /// <param name="variantId">ID biến thể</param>
        /// <returns>Danh sách giá trị thuộc tính (AttributeId, AttributeName, Value)</returns>
        public List<(Guid AttributeId, string AttributeName, string Value)> GetAttributeValues(Guid variantId)
        {
            try
            {
                return GetDataAccess().GetAttributeValues(variantId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy giá trị thuộc tính: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy queryable cho LinqInstantFeedbackSource (trả về Entity)
        /// </summary>
        /// <returns>IQueryable của ProductVariant entity</returns>
        public IQueryable<ProductVariant> GetQueryableForInstantFeedback()
        {
            try
            {
                // Lấy queryable entity từ DAL
                return GetDataAccess().GetQueryableForInstantFeedback();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy queryable cho LinqInstantFeedbackSource: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tổng số bản ghi
        /// </summary>
        /// <returns>Tổng số bản ghi</returns>
        public int GetTotalCount()
        {
            try
            {
                return GetDataAccess().GetTotalCount();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy tổng số bản ghi: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể sản phẩm
        /// </summary>
        /// <returns>Danh sách biến thể</returns>
        public async Task<List<ProductVariant>> GetAllAsync()
        {
            try
            {
                return await GetDataAccess().GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể sản phẩm với thông tin đầy đủ
        /// Bao gồm thông tin sản phẩm gốc, đơn vị tính, thuộc tính và hình ảnh
        /// Tuân thủ quy tắc: Dal -> Bll (chỉ trả về Entity)
        /// </summary>
        /// <returns>Danh sách ProductVariant entity với thông tin đầy đủ</returns>
        public async Task<List<ProductVariant>> GetAllWithDetailsAsync()
        {
            try
            {
                // Lấy dữ liệu từ DAL với thông tin liên quan đã được preload
                var variants = await GetDataAccess().GetAllWithDetailsAsync();
                
                // DAL đã preload tất cả navigation properties thông qua DataLoadOptions
                // Bao gồm: ProductService, UnitOfMeasure, ProductVariantAttributes, ProductVariantImages
                // Và cả thông tin sản phẩm gốc: ProductServiceCategory, ProductServiceAttributes, ProductServiceImages
                
                return variants;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy DataContext để sử dụng với LinqServerModeSource
        /// Tuân thủ quy tắc: Dal -> Bll -> GUI
        /// </summary>
        /// <returns>DataContext</returns>
        public async Task<VnsErp2025DataContext> GetDataContextAsync()
        {
            try
            {
                // Lấy DataContext từ DAL (tuân thủ Dal -> Bll)
                return await GetDataAccess().GetDataContextAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy DataContext: {ex.Message}", ex);
            }
        }

        #endregion

        #region Private Methods


        /// <summary>
        /// Validate dữ liệu biến thể
        /// </summary>
        private void ValidateVariantData(List<(Guid AttributeId, string Value)> attributeValues)
        {
            if (attributeValues == null) return;
            
            foreach (var (attributeId, value) in attributeValues)
            {
                if (attributeId == Guid.Empty)
                    throw new ArgumentException("Vui lòng chọn đầy đủ thuộc tính");

                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Vui lòng nhập đầy đủ giá trị thuộc tính");
            }
        }

        /// <summary>
        /// Cập nhật VariantFullName cho tất cả biến thể hiện có
        /// </summary>
        public async Task UpdateAllVariantFullNamesAsync()
        {
            try
            {
                await GetDataAccess().UpdateAllVariantFullNamesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi cập nhật VariantFullName: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
