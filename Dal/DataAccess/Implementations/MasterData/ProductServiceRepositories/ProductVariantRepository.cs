using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories
{
    /// <summary>
    /// Data Access cho thực thể ProductVariant (LINQ to SQL trên VnsErp2025DataContext).
    /// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
    /// </summary>
    public class ProductVariantRepository : IProductVariantRepository
    {
        #region Private Fields

        /// <summary>
        /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Instance logger để theo dõi các thao tác và lỗi
        /// </summary>
        private readonly CustomLogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo một instance mới của class AttributeRepository.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
        /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
        public ProductVariantRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
            _logger.Info("AttributeRepository được khởi tạo với connection string");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Tạo DataContext mới cho mỗi operation để tránh cache issue
        /// </summary>
        /// <returns>DataContext mới</returns>
        private VnsErp2025DataContext CreateNewContext()
        {
            var context = new VnsErp2025DataContext(_connectionString);

            // Cấu hình DataLoadOptions để preload navigation properties
            // Tránh vòng lặp bằng cách không load ProductVariants từ ProductService
            var loadOptions = new DataLoadOptions();
            loadOptions.LoadWith<ProductVariant>(pv => pv.ProductService);
            loadOptions.LoadWith<ProductVariant>(pv => pv.UnitOfMeasure);
            loadOptions.LoadWith<ProductVariant>(pv => pv.VariantAttributes);
            loadOptions.LoadWith<ProductVariant>(pv => pv.ProductImages);

            // Preload thông tin sản phẩm gốc (không load ProductVariants để tránh vòng lặp)
            loadOptions.LoadWith<ProductService>(ps => ps.ProductServiceCategory);
            loadOptions.LoadWith<ProductService>(ps => ps.ProductImages);

            context.LoadOptions = loadOptions;

            return context;
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
                using var context = CreateNewContext();
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
        protected ProductVariant GetById(object id)
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
                using var context = CreateNewContext();
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
        public List<ProductVariant> GetAll()
        {
            try
            {
                using var context = CreateNewContext();
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
        public async Task<List<ProductVariant>> GetAllAsync()
        {
            try
            {
                using var context = CreateNewContext();

                //// Cấu hình DataLoadOptions để preload navigation properties
                //var loadOptions = new DataLoadOptions();
                //loadOptions.LoadWith<ProductVariant>(pv => pv.ProductService);
                //loadOptions.LoadWith<ProductVariant>(pv => pv.UnitOfMeasure);
                //context.LoadOptions = loadOptions;

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
        /// Lấy tất cả biến thể với thông tin đầy đủ (Async)
        /// Bao gồm thông tin sản phẩm gốc, đơn vị tính và các thông tin liên quan
        /// </summary>
        /// <returns>Danh sách biến thể với thông tin đầy đủ</returns>
        public async Task<List<ProductVariant>> GetAllWithDetailsAsync()
        {
            try
            {
                using var context = CreateNewContext();

                

                return await Task.Run(() => context.ProductVariants
                    .OrderBy(pv => pv.ProductService.Name)
                    .ThenBy(pv => pv.VariantCode)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy tất cả biến thể với thông tin đầy đủ: {ex.Message}", ex);
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();

                // Xóa ProductImages liên quan trước (để tránh foreign key constraint)
                var productImages = context.ProductImages.Where(x => x.VariantId == id).ToList();
                context.ProductImages.DeleteAllOnSubmit(productImages);

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

                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
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
                    existing.VariantFullName = variant.VariantFullName; // Cập nhật VariantFullName

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
                using var context = CreateNewContext();
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

                    // Xóa các VariantAttribute cũ trước khi thêm mới
                    var oldVariantAttributes = context.VariantAttributes.Where(x => x.VariantId == variant.Id).ToList();
                    context.VariantAttributes.DeleteAllOnSubmit(oldVariantAttributes);
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

                // Lưu giá trị thuộc tính mới và tính toán VariantFullName
                var variantFullNameParts = new List<string>();

                if (attributeValues != null && attributeValues.Any())
                {
                    foreach (var (attributeId, value) in attributeValues)
                    {
                        // Tạo AttributeValue mới cho mỗi biến thể (không chia sẻ giữa các biến thể)
                        var attributeValue = new AttributeValue
                        {
                            Id = Guid.NewGuid(),
                            AttributeId = attributeId,
                            Value = value
                        };
                        context.AttributeValues.InsertOnSubmit(attributeValue);

                        // Tạo VariantAttribute
                        var variantAttribute = new VariantAttribute
                        {
                            VariantId = variant.Id,
                            AttributeId = attributeId,
                            AttributeValueId = attributeValue.Id
                        };
                        context.VariantAttributes.InsertOnSubmit(variantAttribute);

                        // Lấy tên thuộc tính để tạo VariantFullName
                        var attribute = context.Attributes.FirstOrDefault(a => a.Id == attributeId);
                        if (attribute != null)
                        {
                            variantFullNameParts.Add($"{attribute.Name} : {value}");
                        }
                    }
                }

                // Cập nhật VariantFullName cho biến thể
                if (existingVariant != null)
                {
                    // Cập nhật cho biến thể hiện có
                    existingVariant.VariantFullName = variantFullNameParts.Any()
                        ? string.Join(", ", variantFullNameParts)
                        : variant.VariantCode;
                }
                else
                {
                    // Cập nhật cho biến thể mới
                    variant.VariantFullName = variantFullNameParts.Any()
                        ? string.Join(", ", variantFullNameParts)
                        : variant.VariantCode;
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
                using var context = CreateNewContext();

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
                using var context = CreateNewContext();

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

        /// <summary>
        /// Lấy DataContext để sử dụng với LinqServerModeSource
        /// </summary>
        /// <returns>VnsErp2025DataContext</returns>
        public async Task<VnsErp2025DataContext> GetDataContextAsync()
        {
            try
            {
                return await Task.Run(() => CreateNewContext());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy DataContext: {ex.Message}", ex);
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
                var context = CreateNewContext();

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
                var context = CreateNewContext();
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
                var context = CreateNewContext();
                return context.UnitOfMeasures;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy queryable UnitOfMeasures: {ex.Message}", ex);
            }
        }


        /// <summary>
        /// Cập nhật VariantFullName cho tất cả biến thể hiện có
        /// </summary>
        public async Task UpdateAllVariantFullNamesAsync()
        {
            try
            {
                using var context = CreateNewContext();

                // Lấy tất cả biến thể
                var variants = context.ProductVariants.ToList();

                foreach (var variant in variants)
                {
                    // Lấy danh sách thuộc tính của biến thể
                    var variantAttributes = context.VariantAttributes
                        .Where(va => va.VariantId == variant.Id)
                        .ToList();

                    if (variantAttributes.Any())
                    {
                        var fullNameParts = new List<string>();

                        foreach (var va in variantAttributes)
                        {
                            // Lấy thông tin thuộc tính và giá trị
                            var attribute = context.Attributes.FirstOrDefault(a => a.Id == va.AttributeId);
                            var attributeValue = context.AttributeValues.FirstOrDefault(av => av.Id == va.AttributeValueId);

                            if (attribute != null && attributeValue != null)
                            {
                                fullNameParts.Add($"{attribute.Name} : {attributeValue.Value}");
                            }
                        }

                        // Cập nhật VariantFullName - mỗi thuộc tính trên 1 dòng
                        variant.VariantFullName = string.Join("\n", fullNameParts);
                    }
                    else
                    {
                        // Nếu không có thuộc tính, sử dụng mã biến thể
                        variant.VariantFullName = variant.VariantCode;
                    }
                }

                // Lưu thay đổi
                await Task.Run(() => context.SubmitChanges());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi cập nhật VariantFullName: {ex.Message}", ex);
            }
        }

        #endregion

    }
}