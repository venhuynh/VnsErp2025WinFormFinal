using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.ProductServiceRepositories;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.Exceptions;
using DTO.MasterData.ProductService;
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
        /// Tạo VariantNameForReport từ thông tin sản phẩm và các thuộc tính
        /// Format: Tên sản phẩm (dòng đầu) + Các thuộc tính (mỗi thuộc tính trên 1 dòng)
        /// </summary>
        /// <param name="context">DataContext</param>
        /// <param name="variant">ProductVariant entity</param>
        /// <returns>VariantNameForReport (không có HTML tags)</returns>
        private string BuildVariantNameForReport(VnsErp2025DataContext context, ProductVariant variant)
        {
            var reportNameParts = new List<string>();

            // Thông tin sản phẩm từ ProductService (dòng đầu)
            var productService = variant.ProductService ?? 
                                (variant.ProductId != Guid.Empty 
                                    ? context.ProductServices.FirstOrDefault(p => p.Id == variant.ProductId) 
                                    : null);
            
            var productName = productService?.Name ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productName))
            {
                reportNameParts.Add(productName);
            }

            // Các thông tin biến thể (thuộc tính) - mỗi thuộc tính trên 1 dòng
            var variantAttributes = context.VariantAttributes
                .Where(va => va.VariantId == variant.Id)
                .ToList();

            if (variantAttributes.Any())
            {
                foreach (var va in variantAttributes)
                {
                    var attribute = context.Attributes.FirstOrDefault(a => a.Id == va.AttributeId);
                    var attributeValue = context.AttributeValues.FirstOrDefault(av => av.Id == va.AttributeValueId);

                    if (attribute != null && attributeValue != null)
                    {
                        reportNameParts.Add($"{attribute.Name}: {attributeValue.Value}");
                    }
                }
            }

            return reportNameParts.Any() 
                ? string.Join(Environment.NewLine, reportNameParts) 
                : string.Empty;
        }

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
            // ProductVariant không còn ProductImages navigation property

            // Preload thông tin sản phẩm gốc (không load ProductVariants để tránh vòng lặp)
            loadOptions.LoadWith<ProductService>(ps => ps.ProductServiceCategory);
            // ProductService có ProductImages nhưng không load để tránh vòng lặp

            context.LoadOptions = loadOptions;

            return context;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy biến thể theo ID
        /// </summary>
        /// <param name="id">ID biến thể</param>
        /// <returns>ProductVariantDto</returns>
        public ProductVariantDto GetById(Guid id)
        {
            try
            {
                using var context = CreateNewContext();
                var entity = context.ProductVariants.FirstOrDefault(x => x.Id == id);
                if (entity == null) return null;

                // Lấy thông tin liên quan
                var product = entity.ProductService;
                var unit = entity.UnitOfMeasure;

                return entity.ToDto(
                    productCode: product?.Code,
                    productName: product?.Name,
                    unitCode: unit?.Code,
                    unitName: unit?.Name,
                    productThumbnailImage: product?.ThumbnailImage?.ToArray()
                );
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy biến thể theo ID (Async)
        /// </summary>
        /// <param name="id">ID biến thể</param>
        /// <returns>ProductVariantDto</returns>
        public async Task<ProductVariantDto> GetByIdAsync(Guid id)
        {
            try
            {
                using var context = CreateNewContext();
                var entity = await Task.Run(() => context.ProductVariants.FirstOrDefault(x => x.Id == id));
                if (entity == null) return null;

                // Lấy thông tin liên quan
                var product = entity.ProductService;
                var unit = entity.UnitOfMeasure;

                return entity.ToDto(
                    productCode: product?.Code,
                    productName: product?.Name,
                    unitCode: unit?.Code,
                    unitName: unit?.Name,
                    productThumbnailImage: product?.ThumbnailImage?.ToArray()
                );
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy biến thể: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả biến thể (Async)
        /// </summary>
        public async Task<List<ProductVariantDto>> GetAllAsync()
        {
            try
            {
                using var context = CreateNewContext();

                var entities = await Task.Run(() => context.ProductVariants
                    .OrderBy(pv => pv.VariantCode)
                    .ToList());

                // Tạo dictionaries cho hiệu quả
                var productDict = context.ProductServices.ToDictionary(p => p.Id);
                var unitDict = context.UnitOfMeasures.ToDictionary(u => u.Id);

                return entities.ToDtos(productDict, unitDict);
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
        /// <returns>Danh sách ProductVariantDto với thông tin đầy đủ</returns>
        public async Task<List<ProductVariantDto>> GetAllWithDetailsAsync()
        {
            try
            {
                using var context = CreateNewContext();

                var entities = await Task.Run(() => context.ProductVariants
                    .OrderBy(pv => pv.ProductService.Name)
                    .ThenBy(pv => pv.VariantCode)
                    .ToList());

                // Tạo dictionaries cho hiệu quả
                var productDict = context.ProductServices.ToDictionary(p => p.Id);
                var unitDict = context.UnitOfMeasures.ToDictionary(u => u.Id);

                return entities.ToDtos(productDict, unitDict);
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
        /// <returns>Danh sách ProductVariantDto</returns>
        public List<ProductVariantDto> GetByProductId(Guid productId)
        {
            try
            {
                using var context = CreateNewContext();
                var entities = context.ProductVariants
                    .Where(x => x.ProductId == productId)
                    .OrderBy(x => x.VariantCode)
                    .ToList();

                // Tạo dictionaries cho hiệu quả
                var productDict = context.ProductServices.ToDictionary(p => p.Id);
                var unitDict = context.UnitOfMeasures.ToDictionary(u => u.Id);

                return entities.ToDtos(productDict, unitDict);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy danh sách biến thể: {ex.Message}", ex);
            }
        }



        /// <summary>
        /// Lấy thông tin thuộc tính của biến thể để hiển thị trong cấu hình thiết bị
        /// Format: ProductService.Name + Environment.NewLine + AttributeName: AttributeValue (mỗi dòng một thuộc tính)
        /// </summary>
        /// <param name="variantId">ID của biến thể sản phẩm</param>
        /// <returns>String chứa thông tin sản phẩm và các thuộc tính</returns>
        public string GetForNewAttribute(Guid variantId)
        {
            try
            {
                using var context = CreateNewContext();

                var variant = context.ProductVariants.FirstOrDefault(x => x.Id == variantId);
                if (variant == null)
                {
                    throw new DataAccessException($"Không tìm thấy thông tin biến thể sản phẩm có Id = {variantId}");
                }

                var result = new StringBuilder();

                // Thêm tên sản phẩm
                if (variant.ProductService != null && !string.IsNullOrWhiteSpace(variant.ProductService.Name))
                {
                    result.Append(variant.ProductService.Name);
                    result.Append(Environment.NewLine);
                }

                // Lấy danh sách VariantAttributes với thông tin Attribute và AttributeValue
                var variantAttributes = (from va in context.VariantAttributes
                                       join a in context.Attributes on va.AttributeId equals a.Id
                                       join av in context.AttributeValues on va.AttributeValueId equals av.Id
                                       where va.VariantId == variantId
                                       select new { AttributeName = a.Name, AttributeValue = av.Value })
                                       .OrderBy(x => x.AttributeName)
                                       .ToList();

                // Thêm các thuộc tính: AttributeName: AttributeValue
                if (variantAttributes.Any())
                {
                    foreach (var item in variantAttributes)
                    {
                        if (!string.IsNullOrWhiteSpace(item.AttributeName) && !string.IsNullOrWhiteSpace(item.AttributeValue))
                        {
                            result.Append(item.AttributeName);
                            result.Append(": ");
                            result.Append(item.AttributeValue);
                            result.Append(Environment.NewLine);
                        }
                    }
                }

                return result.ToString().TrimEnd();
            }
            catch (DataAccessException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi lấy giá trị thuộc tính cho biến thể: {ex.Message}", ex);
            }
        }    
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

                // ProductImage không còn VariantId property, không cần xóa ProductImages

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

        #region ========== VALIDATION & EXISTS CHECKS ==========

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

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lưu hoặc cập nhật biến thể
        /// </summary>
        /// <param name="variant">Biến thể DTO cần lưu hoặc cập nhật</param>
        public void SaveOrUpdate(ProductVariantDto variant)
        {
            try
            {
                if (variant == null)
                    throw new ArgumentNullException(nameof(variant));

                using var context = CreateNewContext();
                var existing = variant.Id != Guid.Empty ? context.ProductVariants.FirstOrDefault(x => x.Id == variant.Id) : null;

                // Convert DTO to Entity
                var entity = variant.ToEntity(existing);

                if (existing == null)
                {
                    // Thêm mới
                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();

                    // Thiết lập CreatedDate và ModifiedDate cho biến thể mới
                    entity.CreatedDate = DateTime.Now;
                    entity.ModifiedDate = DateTime.Now;

                    // Tạo VariantNameForReport (chỉ tên sản phẩm và các thuộc tính, không có HTML tags)
                    entity.VariantNameForReport = BuildVariantNameForReport(context, entity);

                    context.ProductVariants.InsertOnSubmit(entity);
                }
                else
                {
                    // Cập nhật ModifiedDate
                    existing.ModifiedDate = DateTime.Now;
                    // Cập nhật VariantNameForReport (chỉ tên sản phẩm và các thuộc tính, không có HTML tags)
                    existing.VariantNameForReport = BuildVariantNameForReport(context, existing);
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
        /// <param name="variant">Biến thể DTO</param>
        /// <param name="attributeValues">Danh sách giá trị thuộc tính (AttributeId, Value)</param>
        /// <returns>ID biến thể đã lưu</returns>
        public async Task<Guid> SaveAsync(ProductVariantDto variant, List<(Guid AttributeId, string Value)> attributeValues)
        {
            try
            {
                using var context = CreateNewContext();
                var currentTime = DateTime.Now;

                // Lưu hoặc cập nhật biến thể
                var existingVariant = context.ProductVariants.FirstOrDefault(x => x.Id == variant.Id);
                
                // Convert DTO to Entity
                var entity = variant.ToEntity(existingVariant);
                
                if (existingVariant == null)
                {
                    // Tạo mới
                    if (entity.Id == Guid.Empty)
                        entity.Id = Guid.NewGuid();

                    // Thiết lập CreatedDate và ModifiedDate cho biến thể mới
                    entity.CreatedDate = currentTime;
                    entity.ModifiedDate = currentTime;

                    context.ProductVariants.InsertOnSubmit(entity);
                }
                else
                {
                    // Cập nhật ModifiedDate
                    existingVariant.ModifiedDate = currentTime;

                    // Xóa các VariantAttribute cũ trước khi thêm mới (chỉ khi có attributeValues)
                    if (attributeValues != null)
                    {
                        var oldVariantAttributes = context.VariantAttributes.Where(x => x.VariantId == entity.Id).ToList();
                        context.VariantAttributes.DeleteAllOnSubmit(oldVariantAttributes);
                    }
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
                            VariantId = entity.Id,
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
                string variantFullNameValue = variantFullNameParts.Any()
                    ? string.Join(", ", variantFullNameParts)
                    : entity.VariantCode;

                var targetVariant = existingVariant ?? entity;
                targetVariant.VariantFullName = variantFullNameValue;
                // Lưu VariantNameForReport (chỉ tên sản phẩm và các thuộc tính, không có HTML tags)
                targetVariant.VariantNameForReport = BuildVariantNameForReport(context, targetVariant);

                await Task.Run(() => context.SubmitChanges());
                return entity.Id;
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
        /// Cập nhật VariantFullName cho tất cả biến thể hiện có
        /// Format: Tên sản phẩm - Đơn vị tính - Mã biến thể - Các thông tin biến thể (mỗi thuộc tính trên 1 dòng)
        /// </summary>
        public async Task UpdateAllVariantFullNamesAsync()
        {
            try
            {
                using var context = CreateNewContext();

                // Lấy tất cả biến thể với navigation properties
                var variants = context.ProductVariants.ToList();

                foreach (var variant in variants)
                {
                    // Format HTML theo pattern VariantFullNameHtml trong ProductVariantListDto
                    var html = string.Empty;

                    // Thông tin sản phẩm từ ProductService (dòng đầu)
                    var productName = variant.ProductService?.Name ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(productName))
                    {
                        html += $"<b><color='blue'>{productName}</color></b>";
                    }


                    //Cập nhật luôn hình ảnh ThumbnailImage
                    variant.ThumbnailImage = variant.ProductService?.ThumbnailImage;

                    // Tổng hợp các thông tin biến thể
                    var variantParts = new List<string>();


                    // Đơn vị tính (nếu có)
                    var unitName = variant.UnitOfMeasure?.Name ?? string.Empty;
                    if (!string.IsNullOrWhiteSpace(unitName))
                    {
                        variantParts.Add($"<br/><color='blue'>Đơn vị tính:</color> <b>{unitName}</b>");
                    }

                    // Các thông tin biến thể (thuộc tính) - mỗi thuộc tính trên 1 dòng
                    var variantAttributes = context.VariantAttributes
                        .Where(va => va.VariantId == variant.Id)
                        .ToList();

                    var attributeParts = new List<string>();
                    if (variantAttributes.Any())
                    {
                        foreach (var va in variantAttributes)
                        {
                            // Lấy thông tin thuộc tính và giá trị
                            var attribute = context.Attributes.FirstOrDefault(a => a.Id == va.AttributeId);
                            var attributeValue = context.AttributeValues.FirstOrDefault(av => av.Id == va.AttributeValueId);

                            if (attribute != null && attributeValue != null)
                            {
                                // Mỗi thuộc tính trên 1 dòng
                                attributeParts.Add($"<color='blue'>{attribute.Name}:</color> <b>{attributeValue.Value}</b>");
                            }
                        }
                    }

                    // Thêm các thuộc tính vào variantParts (mỗi thuộc tính là một phần riêng)
                    if (attributeParts.Any())
                    {
                        variantParts.AddRange(attributeParts);
                    }

                    if (variantParts.Any())
                    {
                        html += string.Join("<br/>", variantParts);
                    }


                    // Lưu HTML vào VariantFullName
                    variant.VariantFullName = html;

                    // Lưu VariantNameForReport (chỉ tên sản phẩm và các thuộc tính, không có HTML tags)
                    var reportNameParts = new List<string>();
                    
                    // Thông tin sản phẩm từ ProductService (dòng đầu)
                    if (!string.IsNullOrWhiteSpace(productName))
                    {
                        reportNameParts.Add(productName);
                    }

                    // Các thông tin biến thể (thuộc tính) - mỗi thuộc tính trên 1 dòng
                    if (variantAttributes.Any())
                    {
                        foreach (var va in variantAttributes)
                        {
                            var attribute = context.Attributes.FirstOrDefault(a => a.Id == va.AttributeId);
                            var attributeValue = context.AttributeValues.FirstOrDefault(av => av.Id == va.AttributeValueId);

                            if (attribute != null && attributeValue != null)
                            {
                                reportNameParts.Add($"{attribute.Name}: {attributeValue.Value}");
                            }
                        }
                    }

                    variant.VariantNameForReport = reportNameParts.Any() 
                        ? string.Join(Environment.NewLine, reportNameParts) 
                        : string.Empty;
                }

                // Lưu thay đổi
                await Task.Run(() => context.SubmitChanges());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi cập nhật VariantFullName: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật VariantFullName cho một biến thể cụ thể
        /// Format: Tên sản phẩm - Đơn vị tính - Mã biến thể - Các thông tin biến thể (mỗi thuộc tính trên 1 dòng)
        /// </summary>
        /// <param name="variantId">ID biến thể cần cập nhật</param>
        public async Task UpdateVariantFullNameAsync(Guid variantId)
        {
            try
            {
                using var context = CreateNewContext();

                // Lấy biến thể với navigation properties
                var variant = context.ProductVariants
                    .FirstOrDefault(v => v.Id == variantId);

                if (variant == null)
                {
                    return; // Biến thể không tồn tại, bỏ qua
                }

                // Force load navigation properties nếu chưa có
                if (variant.ProductService == null && variant.ProductId != Guid.Empty)
                {
                    var productService = context.ProductServices.FirstOrDefault(p => p.Id == variant.ProductId);
                    if (productService != null)
                    {
                        variant.ProductService = productService;
                    }
                }

                if (variant.UnitOfMeasure == null && variant.UnitId != Guid.Empty)
                {
                    var unitOfMeasure = context.UnitOfMeasures.FirstOrDefault(u => u.Id == variant.UnitId);
                    if (unitOfMeasure != null)
                    {
                        variant.UnitOfMeasure = unitOfMeasure;
                    }
                }

                // Format HTML theo pattern VariantFullNameHtml trong ProductVariantListDto
                var html = string.Empty;

                // Thông tin sản phẩm từ ProductService (dòng đầu)
                var productName = variant.ProductService?.Name ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(productName))
                {
                    html += $"<b><color='blue'>{productName}</color></b>";
                }

                //Cập nhật luôn hình ảnh ThumbnailImage
                variant.ThumbnailImage = variant.ProductService?.ThumbnailImage;

                // Tổng hợp các thông tin biến thể
                var variantParts = new List<string>();

                // Đơn vị tính (nếu có)
                var unitName = variant.UnitOfMeasure?.Name ?? string.Empty;
                if (!string.IsNullOrWhiteSpace(unitName))
                {
                    variantParts.Add($"<br/><color='blue'>Đơn vị tính:</color> <b>{unitName}</b>");
                }

                // Các thông tin biến thể (thuộc tính) - mỗi thuộc tính trên 1 dòng
                var variantAttributes = context.VariantAttributes
                    .Where(va => va.VariantId == variant.Id)
                    .ToList();

                var attributeParts = new List<string>();
                if (variantAttributes.Any())
                {
                    foreach (var va in variantAttributes)
                    {
                        // Lấy thông tin thuộc tính và giá trị
                        var attribute = context.Attributes.FirstOrDefault(a => a.Id == va.AttributeId);
                        var attributeValue = context.AttributeValues.FirstOrDefault(av => av.Id == va.AttributeValueId);

                        if (attribute != null && attributeValue != null)
                        {
                            // Mỗi thuộc tính trên 1 dòng
                            attributeParts.Add($"<color='blue'>{attribute.Name}:</color> <b>{attributeValue.Value}</b>");
                        }
                    }
                }

                // Thêm các thuộc tính vào variantParts (mỗi thuộc tính là một phần riêng)
                if (attributeParts.Any())
                {
                    variantParts.AddRange(attributeParts);
                }

                if (variantParts.Any())
                {
                    html += string.Join("<br/>", variantParts);
                }

                // Lưu HTML vào VariantFullName
                variant.VariantFullName = html;

                // Lưu VariantNameForReport (chỉ tên sản phẩm và các thuộc tính, không có HTML tags)
                var reportNameParts = new List<string>();
                
                // Thông tin sản phẩm từ ProductService (dòng đầu)
                if (!string.IsNullOrWhiteSpace(productName))
                {
                    reportNameParts.Add(productName);
                }

                // Các thông tin biến thể (thuộc tính) - mỗi thuộc tính trên 1 dòng
                if (variantAttributes.Any())
                {
                    foreach (var va in variantAttributes)
                    {
                        var attribute = context.Attributes.FirstOrDefault(a => a.Id == va.AttributeId);
                        var attributeValue = context.AttributeValues.FirstOrDefault(av => av.Id == va.AttributeValueId);

                        if (attribute != null && attributeValue != null)
                        {
                            reportNameParts.Add($"{attribute.Name}: {attributeValue.Value}");
                        }
                    }
                }

                variant.VariantNameForReport = reportNameParts.Any() 
                    ? string.Join(Environment.NewLine, reportNameParts) 
                    : string.Empty;

                // Lưu thay đổi
                await Task.Run(() => context.SubmitChanges());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi cập nhật VariantFullName cho biến thể {variantId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Chỉ cập nhật/xóa thumbnail image của biến thể, không ảnh hưởng đến các trường khác
        /// </summary>
        public async Task UpdateThumbnailOnlyAsync(
            Guid variantId,
            Binary thumbnailImage,
            string thumbnailFileName,
            string thumbnailRelativePath,
            string thumbnailFullPath,
            string thumbnailStorageType,
            long? thumbnailFileSize,
            string thumbnailChecksum)
        {
            try
            {
                using var context = CreateNewContext();
                var variant = context.ProductVariants.FirstOrDefault(x => x.Id == variantId);
                
                if (variant == null)
                {
                    throw new DataAccessException($"Không tìm thấy biến thể với ID {variantId}");
                }

                // Chỉ cập nhật các field thumbnail, không ảnh hưởng đến các trường khác
                variant.ThumbnailImage = thumbnailImage;
                variant.ThumbnailFileName = thumbnailFileName;
                variant.ThumbnailRelativePath = thumbnailRelativePath;
                variant.ThumbnailFullPath = thumbnailFullPath;
                variant.ThumbnailStorageType = thumbnailStorageType;
                variant.ThumbnailFileSize = thumbnailFileSize;
                variant.ThumbnailChecksum = thumbnailChecksum;
                
                // Cập nhật ModifiedDate
                variant.ModifiedDate = DateTime.Now;

                // Lưu thay đổi
                await Task.Run(() => context.SubmitChanges());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi cập nhật thumbnail cho biến thể {variantId}: {ex.Message}", ex);
            }
        }

        #endregion

    }
}