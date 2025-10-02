using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.DataAccess.MasterData.ProductServiceDal
{
    /// <summary>
    /// Data Access cho thực thể ProductVariant (LINQ to SQL trên VnsErp2025DataContext).
    /// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
    /// </summary>
    public class ProductVariantDataAccess : BaseDataAccess<ProductVariant>
    {
        #region Constructors

        /// <summary>
        /// Khởi tạo mặc định.
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ProductVariantDataAccess(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Khởi tạo với connection string.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ProductVariantDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        #endregion

        #region Create

        #endregion

        #region Read

        /// <summary>
        /// Lấy biến thể theo ID
        /// </summary>
        /// <param name="id">ID biến thể</param>
        /// <returns>ProductVariant entity</returns>
        public ProductVariant GetById(Guid id)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Override GetById từ BaseDataAccess để sử dụng Guid thay vì object.
        /// </summary>
        public override ProductVariant GetById(object id)
        {
            if (id is Guid guidId)
                return GetById(guidId);
            return null;
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
                using var context = CreateContext();
                return await Task.Run(() => context.ProductVariants.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể
        /// </summary>
        public override List<ProductVariant> GetAll()
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants
                    .OrderBy(pv => pv.VariantCode)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy tất cả biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể (Async)
        /// </summary>
        public override async Task<List<ProductVariant>> GetAllAsync()
        {
            try
            {
                using var context = CreateContext();
                
                // Cấu hình DataLoadOptions để preload navigation properties
                var loadOptions = new System.Data.Linq.DataLoadOptions();
                loadOptions.LoadWith<ProductVariant>(pv => pv.ProductService);
                loadOptions.LoadWith<ProductVariant>(pv => pv.UnitOfMeasure);
                context.LoadOptions = loadOptions;
                
                return await Task.Run(() => context.ProductVariants
                    .OrderBy(pv => pv.VariantCode)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy tất cả biến thể: {ex.Message}", ex);
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
                using var context = CreateContext();
                return context.ProductVariants
                    .Where(x => x.ProductId == productId)
                    .OrderBy(x => x.VariantCode)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách biến thể theo ProductId (Async)
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>Danh sách biến thể</returns>
        public async Task<List<ProductVariant>> GetByProductIdAsync(Guid productId)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductVariants
                    .Where(x => x.ProductId == productId)
                    .OrderBy(x => x.VariantCode)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo mã
        /// </summary>
        /// <param name="variantCode">Mã biến thể</param>
        /// <returns>ProductVariant entity</returns>
        public ProductVariant GetByVariantCode(string variantCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(variantCode))
                    return null;

                using var context = CreateContext();
                return context.ProductVariants.FirstOrDefault(x => x.VariantCode == variantCode.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể theo mã: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo mã (Async)
        /// </summary>
        /// <param name="variantCode">Mã biến thể</param>
        /// <returns>ProductVariant entity</returns>
        public async Task<ProductVariant> GetByVariantCodeAsync(string variantCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(variantCode))
                    return null;

                using var context = CreateContext();
                return await Task.Run(() => context.ProductVariants.FirstOrDefault(x => x.VariantCode == variantCode.Trim()));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể theo mã: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo trạng thái hoạt động
        /// </summary>
        /// <param name="isActive">Trạng thái hoạt động</param>
        /// <returns>Danh sách biến thể</returns>
        public List<ProductVariant> GetByStatus(bool isActive)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants
                    .Where(x => x.IsActive == isActive)
                    .OrderBy(x => x.VariantCode)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể theo trạng thái: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo trạng thái hoạt động (Async)
        /// </summary>
        /// <param name="isActive">Trạng thái hoạt động</param>
        /// <returns>Danh sách biến thể</returns>
        public async Task<List<ProductVariant>> GetByStatusAsync(bool isActive)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductVariants
                    .Where(x => x.IsActive == isActive)
                    .OrderBy(x => x.VariantCode)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể theo trạng thái: {ex.Message}", ex);
            }
        }

        #endregion

        #region Update

        #endregion

        #region Delete

        /// <summary>
        /// Xóa biến thể
        /// </summary>
        /// <param name="id">ID biến thể</param>
        public async Task DeleteAsync(Guid id)
        {
            try
            {
                using var context = CreateContext();
                
                // Xóa VariantAttributes trước
                var variantAttributes = context.VariantAttributes.Where(x => x.VariantId == id).ToList();
                context.VariantAttributes.DeleteAllOnSubmit(variantAttributes);

                // Xóa biến thể
                var variant = context.ProductVariants.FirstOrDefault(x => x.Id == id);
                if (variant != null)
                {
                    context.ProductVariants.DeleteOnSubmit(variant);
                }

                await Task.Run(() => context.SubmitChanges());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi xóa biến thể: {ex.Message}", ex);
            }
        }

        #endregion

        #region Validation & Business Rules

        /// <summary>
        /// Kiểm tra mã biến thể có trùng không
        /// </summary>
        /// <param name="variantCode">Mã biến thể</param>
        /// <param name="excludeId">ID biến thể cần loại trừ</param>
        /// <returns>True nếu trùng</returns>
        public bool IsVariantCodeExists(string variantCode, Guid? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(variantCode))
                    return false;

                using var context = CreateContext();
                var query = context.ProductVariants.Where(x => x.VariantCode == variantCode.Trim());
                
                if (excludeId.HasValue)
                {
                    query = query.Where(x => x.Id != excludeId.Value);
                }

                return query.Any();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi kiểm tra mã biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã biến thể có trùng không (Async)
        /// </summary>
        /// <param name="variantCode">Mã biến thể</param>
        /// <param name="excludeId">ID biến thể cần loại trừ</param>
        /// <returns>True nếu trùng</returns>
        public async Task<bool> IsVariantCodeExistsAsync(string variantCode, Guid? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(variantCode))
                    return false;

                using var context = CreateContext();
                var query = context.ProductVariants.Where(x => x.VariantCode == variantCode.Trim());
                
                if (excludeId.HasValue)
                {
                    query = query.Where(x => x.Id != excludeId.Value);
                }

                return await Task.Run(() => query.Any());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi kiểm tra mã biến thể: {ex.Message}", ex);
            }
        }

        #endregion

        #region Statistics Methods

        /// <summary>
        /// Đếm tổng số biến thể
        /// </summary>
        /// <returns>Số lượng biến thể</returns>
        public int GetTotalCount()
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants.Count();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi đếm tổng số biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số biến thể theo sản phẩm
        /// </summary>
        /// <param name="productId">ID sản phẩm</param>
        /// <returns>Số lượng biến thể</returns>
        public int GetCountByProduct(Guid productId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants.Count(x => x.ProductId == productId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi đếm số biến thể theo sản phẩm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số biến thể theo trạng thái
        /// </summary>
        /// <param name="isActive">Trạng thái hoạt động</param>
        /// <returns>Số lượng biến thể</returns>
        public int GetCountByStatus(bool isActive)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants.Count(x => x.IsActive == isActive);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi đếm số biến thể theo trạng thái: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo khoảng thời gian tạo
        /// </summary>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <returns>Danh sách biến thể</returns>
        public List<ProductVariant> GetByCreatedDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants
                    .Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể theo khoảng thời gian tạo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo khoảng thời gian tạo (Async)
        /// </summary>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <returns>Danh sách biến thể</returns>
        public async Task<List<ProductVariant>> GetByCreatedDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductVariants
                    .Where(x => x.CreatedDate >= fromDate && x.CreatedDate <= toDate)
                    .OrderByDescending(x => x.CreatedDate)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể theo khoảng thời gian tạo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo khoảng thời gian cập nhật
        /// </summary>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <returns>Danh sách biến thể</returns>
        public List<ProductVariant> GetByModifiedDateRange(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants
                    .Where(x => x.ModifiedDate >= fromDate && x.ModifiedDate <= toDate)
                    .OrderByDescending(x => x.ModifiedDate)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể theo khoảng thời gian cập nhật: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo khoảng thời gian cập nhật (Async)
        /// </summary>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <returns>Danh sách biến thể</returns>
        public async Task<List<ProductVariant>> GetByModifiedDateRangeAsync(DateTime fromDate, DateTime toDate)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductVariants
                    .Where(x => x.ModifiedDate >= fromDate && x.ModifiedDate <= toDate)
                    .OrderByDescending(x => x.ModifiedDate)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể theo khoảng thời gian cập nhật: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể được tạo gần đây nhất
        /// </summary>
        /// <param name="count">Số lượng biến thể cần lấy</param>
        /// <returns>Danh sách biến thể</returns>
        public List<ProductVariant> GetRecentlyCreated(int count = 10)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants
                    .OrderByDescending(x => x.CreatedDate)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể được tạo gần đây: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể được cập nhật gần đây nhất
        /// </summary>
        /// <param name="count">Số lượng biến thể cần lấy</param>
        /// <returns>Danh sách biến thể</returns>
        public List<ProductVariant> GetRecentlyModified(int count = 10)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductVariants
                    .OrderByDescending(x => x.ModifiedDate)
                    .Take(count)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể được cập nhật gần đây: {ex.Message}", ex);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lưu hoặc cập nhật biến thể
        /// </summary>
        /// <param name="variant">Biến thể cần lưu hoặc cập nhật</param>
        public void SaveOrUpdate(ProductVariant variant)
        {
            try
            {
                if (variant == null) 
                    throw new ArgumentNullException(nameof(variant));
                
                using var context = CreateContext();
                var existing = variant.Id != Guid.Empty ? context.ProductVariants.FirstOrDefault(x => x.Id == variant.Id) : null;
                
                if (existing == null)
                {
                    // Thêm mới
                    if (variant.Id == Guid.Empty)
                        variant.Id = Guid.NewGuid();
                    
                    // Thiết lập CreatedDate và ModifiedDate cho biến thể mới
                    variant.CreatedDate = DateTime.Now;
                    variant.ModifiedDate = DateTime.Now;
                    
                    context.ProductVariants.InsertOnSubmit(variant);
                }
                else
                {
                    // Cập nhật
                    existing.ProductId = variant.ProductId;
                    existing.VariantCode = variant.VariantCode;
                    existing.UnitId = variant.UnitId;
                    existing.IsActive = variant.IsActive;
                    existing.ThumbnailImage = variant.ThumbnailImage;
                    
                    // Cập nhật ModifiedDate
                    existing.ModifiedDate = DateTime.Now;
                }
                
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lưu hoặc cập nhật biến thể '{variant?.VariantCode}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu biến thể và giá trị thuộc tính
        /// </summary>
        /// <param name="variant">Biến thể</param>
        /// <param name="attributeValues">Danh sách giá trị thuộc tính (AttributeId, Value)</param>
        /// <returns>ID biến thể đã lưu</returns>
        public async Task<Guid> SaveAsync(ProductVariant variant, List<(Guid AttributeId, string Value)> attributeValues)
        {
            try
            {
                using var context = CreateContext();
                var currentTime = DateTime.Now;
                
                // Lưu hoặc cập nhật biến thể
                var existingVariant = context.ProductVariants.FirstOrDefault(x => x.Id == variant.Id);
                if (existingVariant != null)
                {
                    // Cập nhật
                    existingVariant.ProductId = variant.ProductId;
                    existingVariant.VariantCode = variant.VariantCode;
                    existingVariant.UnitId = variant.UnitId;
                    existingVariant.IsActive = variant.IsActive;
                    existingVariant.ThumbnailImage = variant.ThumbnailImage;
                    
                    // Cập nhật ModifiedDate
                    existingVariant.ModifiedDate = currentTime;
                }
                else
                {
                    // Tạo mới
                    variant.Id = Guid.NewGuid();
                    
                    // Thiết lập CreatedDate và ModifiedDate cho biến thể mới
                    variant.CreatedDate = currentTime;
                    variant.ModifiedDate = currentTime;
                    
                    context.ProductVariants.InsertOnSubmit(variant);
                }

                // Xóa các VariantAttribute cũ nếu đang edit
                if (existingVariant != null)
                {
                    var oldVariantAttributes = context.VariantAttributes.Where(x => x.VariantId == variant.Id).ToList();
                    context.VariantAttributes.DeleteAllOnSubmit(oldVariantAttributes);
                }

                // Lưu giá trị thuộc tính mới
                if (attributeValues != null && attributeValues.Any())
                {
                    foreach (var (attributeId, value) in attributeValues)
                    {
                        // Tạo AttributeValue nếu chưa có
                        var existingAttrValue = context.AttributeValues.FirstOrDefault(x => 
                            x.AttributeId == attributeId && x.Value == value);
                        
                        if (existingAttrValue == null)
                        {
                            existingAttrValue = new AttributeValue
                            {
                                Id = Guid.NewGuid(),
                                AttributeId = attributeId,
                                Value = value
                            };
                            context.AttributeValues.InsertOnSubmit(existingAttrValue);
                        }

                        // Tạo VariantAttribute
                        var variantAttribute = new VariantAttribute
                        {
                            VariantId = variant.Id,
                            AttributeId = attributeId,
                            AttributeValueId = existingAttrValue.Id
                        };
                        context.VariantAttributes.InsertOnSubmit(variantAttribute);
                    }
                }

                await Task.Run(() => context.SubmitChanges());
                return variant.Id;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lưu biến thể: {ex.Message}", ex);
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
                using var context = CreateContext();
                
                var query = from va in context.VariantAttributes
                           join av in context.AttributeValues on va.AttributeValueId equals av.Id
                           join a in context.Attributes on va.AttributeId equals a.Id
                           where va.VariantId == variantId
                           select new { a.Id, a.Name, av.Value };

                var results = query.ToList();
                return results.Select(x => (x.Id, x.Name, x.Value)).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy giá trị thuộc tính: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách giá trị thuộc tính của biến thể (Async)
        /// </summary>
        /// <param name="variantId">ID biến thể</param>
        /// <returns>Danh sách giá trị thuộc tính (AttributeId, AttributeName, Value)</returns>
        public async Task<List<(Guid AttributeId, string AttributeName, string Value)>> GetAttributeValuesAsync(Guid variantId)
        {
            try
            {
                using var context = CreateContext();
                
                var query = from va in context.VariantAttributes
                           join av in context.AttributeValues on va.AttributeValueId equals av.Id
                           join a in context.Attributes on va.AttributeId equals a.Id
                           where va.VariantId == variantId
                           select new { a.Id, a.Name, av.Value };

                var results = await Task.Run(() => query.ToList());
                return results.Select(x => (x.Id, x.Name, x.Value)).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy giá trị thuộc tính: {ex.Message}", ex);
            }
        }

        #endregion

        #region LinqInstantFeedbackSource Support

        /// <summary>
        /// Lấy queryable cho LinqInstantFeedbackSource (chỉ trả về Entity)
        /// </summary>
        /// <returns>IQueryable của ProductVariant entity</returns>
        public IQueryable<ProductVariant> GetQueryableForInstantFeedback()
        {
            try
            {
                // Không sử dụng 'using' để tránh dispose context sớm
                // LinqInstantFeedbackSource sẽ tự quản lý lifecycle của context
                var context = CreateContext();
                
                // Trả về queryable của ProductVariant entity
                return context.ProductVariants;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy queryable cho LinqInstantFeedbackSource: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Lấy queryable của ProductServices để join trong BLL
        /// </summary>
        /// <returns>IQueryable của ProductService entity</returns>
        public IQueryable<ProductService> GetProductServicesQueryable()
        {
            try
            {
                // Không sử dụng 'using' để tránh dispose context sớm
                var context = CreateContext();
                return context.ProductServices;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy queryable ProductServices: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy queryable của UnitOfMeasures để join trong BLL
        /// </summary>
        /// <returns>IQueryable của UnitOfMeasure entity</returns>
        public IQueryable<UnitOfMeasure> GetUnitOfMeasuresQueryable()
        {
            try
            {
                // Không sử dụng 'using' để tránh dispose context sớm
                var context = CreateContext();
                return context.UnitOfMeasures;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy queryable UnitOfMeasures: {ex.Message}", ex);
            }
        }


        #endregion

    }
}
