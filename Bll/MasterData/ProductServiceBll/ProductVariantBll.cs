using Dal.DataAccess.MasterData.ProductServiceDal;
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

        private readonly ProductVariantDataAccess _dataAccess;
        private readonly ProductServiceDataAccess _productServiceDataAccess;
        private readonly UnitOfMeasureDataAccess _unitOfMeasureDataAccess;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo ProductVariantBll
        /// </summary>
        public ProductVariantBll()
        {
            _dataAccess = new ProductVariantDataAccess();
            _productServiceDataAccess = new ProductServiceDataAccess();
            _unitOfMeasureDataAccess = new UnitOfMeasureDataAccess();
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
                return _dataAccess.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy biến thể: {ex.Message}", ex);
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
                return _dataAccess.GetByProductId(productId);
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
        public bool IsVariantCodeExists(string variantCode, Guid? excludeId = null)
        {
            try
            {
                return _dataAccess.IsVariantCodeExists(variantCode, excludeId);
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
                ValidateVariantData(variant, attributeValues);

                // Kiểm tra trùng mã biến thể
                if (IsVariantCodeExists(variant.VariantCode, variant.Id == Guid.Empty ? null : variant.Id))
                {
                    throw new Exception($"Mã biến thể '{variant.VariantCode}' đã tồn tại. Vui lòng chọn mã khác.");
                }

                // Lưu biến thể
                var savedVariantId = await _dataAccess.SaveAsync(variant, attributeValues);

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
                await _dataAccess.DeleteAsync(id);
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
                return _dataAccess.GetAttributeValues(variantId);
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
                return _dataAccess.GetQueryableForInstantFeedback();
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
                return _dataAccess.GetTotalCount();
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
                return await _dataAccess.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể sản phẩm với thông tin đầy đủ
        /// Bao gồm thông tin sản phẩm gốc, đơn vị tính
        /// Tuân thủ quy tắc: Dal -> Bll (chỉ trả về Entity)
        /// </summary>
        /// <returns>Danh sách ProductVariant entity</returns>
        public async Task<List<ProductVariant>> GetAllWithDetailsAsync()
        {
            try
            {
                // Lấy dữ liệu từ DAL (tuân thủ Dal -> Bll)
                var variants = await _dataAccess.GetAllAsync();
                
                // Load thông tin liên quan cho mỗi variant
                foreach (var variant in variants)
                {
                    // Load thông tin sản phẩm gốc
                    var product = await _productServiceDataAccess.GetByIdAsync(variant.ProductId);
                    if (product != null)
                    {
                        // Có thể set thông tin vào variant nếu cần, hoặc để navigation property tự load
                    }
                    
                    // Load thông tin đơn vị tính
                    var unit = _unitOfMeasureDataAccess.GetById(variant.UnitId);
                    if (unit != null)
                    {
                        // Có thể set thông tin vào variant nếu cần, hoặc để navigation property tự load
                    }
                }
                
                return variants;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        #endregion

        #region Private Methods


        /// <summary>
        /// Validate dữ liệu biến thể
        /// </summary>
        private void ValidateVariantData(ProductVariant variant, List<(Guid AttributeId, string Value)> attributeValues)
        {
            if (variant == null)
                throw new ArgumentNullException(nameof(variant), "Dữ liệu biến thể không được để trống");

            if (variant.ProductId == Guid.Empty)
                throw new ArgumentException("Vui lòng chọn sản phẩm/dịch vụ");

            if (string.IsNullOrWhiteSpace(variant.VariantCode))
                throw new ArgumentException("Mã biến thể không được để trống");

            if (variant.UnitId == Guid.Empty)
                throw new ArgumentException("Vui lòng chọn đơn vị tính");

            if (attributeValues != null)
            {
                foreach (var (attributeId, value) in attributeValues)
                {
                    if (attributeId == Guid.Empty)
                        throw new ArgumentException("Vui lòng chọn đầy đủ thuộc tính");

                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Vui lòng nhập đầy đủ giá trị thuộc tính");
                }
            }
        }

        #endregion
    }
}
