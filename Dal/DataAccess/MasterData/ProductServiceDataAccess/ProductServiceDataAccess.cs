using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;

namespace Dal.DataAccess.MasterData.ProductServiceDataAccess
{
    /// <summary>
    /// Data Access cho thực thể ProductService (LINQ to SQL trên VnsErp2025DataContext).
    /// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
    /// </summary>
    public class ProductServiceDataAccess : BaseDataAccess<ProductService>
    {
        #region Constructors

        /// <summary>
        /// Khởi tạo mặc định.
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ProductServiceDataAccess(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Khởi tạo với connection string.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ProductServiceDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        #endregion

        #region Create

        #endregion

        #region Read

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo Id.
        /// </summary>
        public ProductService GetById(Guid id)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo Id {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Override GetById từ BaseDataAccess để sử dụng Guid thay vì object.
        /// </summary>
        public override ProductService GetById(object id)
        {
            if (id is Guid guidId)
                return GetById(guidId);
            return null;
        }

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo Id (Async).
        /// </summary>
        public async Task<ProductService> GetByIdAsync(Guid id)
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServices.FirstOrDefault(x => x.Id == id));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo Id {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả sản phẩm/dịch vụ.
        /// </summary>
        public override List<ProductService> GetAll()
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices
                    .OrderBy(ps => ps.Code)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả sản phẩm/dịch vụ (Async).
        /// </summary>
        public override async Task<List<ProductService>> GetAllAsync()
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.ProductServices
                    .OrderBy(ps => ps.Code)
                    .ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo mã.
        /// </summary>
        public ProductService GetByCode(string code)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return null;

                using var context = CreateContext();
                return context.ProductServices.FirstOrDefault(x => x.Code == code.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo mã '{code}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo tên.
        /// </summary>
        public ProductService GetByName(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return null;

                using var context = CreateContext();
                return context.ProductServices.FirstOrDefault(x => x.Name == name.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo tên '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo danh mục.
        /// </summary>
        public List<ProductService> GetByCategory(Guid categoryId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices
                    .Where(ps => ps.CategoryId == categoryId)
                    .OrderBy(ps => ps.Code)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo danh mục {categoryId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo loại (sản phẩm hoặc dịch vụ).
        /// </summary>
        public List<ProductService> GetByType(bool isService)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices
                    .Where(ps => ps.IsService == isService)
                    .OrderBy(ps => ps.Code)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo loại {(isService ? "dịch vụ" : "sản phẩm")}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy sản phẩm/dịch vụ theo trạng thái hoạt động.
        /// </summary>
        public List<ProductService> GetByStatus(bool isActive)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices
                    .Where(ps => ps.IsActive == isActive)
                    .OrderBy(ps => ps.Code)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo trạng thái {(isActive ? "hoạt động" : "không hoạt động")}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm sản phẩm/dịch vụ theo từ khóa.
        /// </summary>
        public List<ProductService> Search(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return GetAll();

                var searchTerm = keyword.Trim().ToLower();

                using var context = CreateContext();
                return context.ProductServices
                    .Where(ps => ps.Code.ToLower().Contains(searchTerm) ||
                                ps.Name.ToLower().Contains(searchTerm) ||
                                (ps.Description != null && ps.Description.ToLower().Contains(searchTerm)))
                    .OrderBy(ps => ps.Code)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi tìm kiếm sản phẩm/dịch vụ với từ khóa '{keyword}': {ex.Message}", ex);
            }
        }

        #endregion

        #region Update

        #endregion

        #region Delete

        /// <summary>
        /// Xóa sản phẩm/dịch vụ theo ID.
        /// </summary>
        /// <param name="id">ID sản phẩm/dịch vụ cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        public bool DeleteProductService(Guid id)
        {
            try
            {
                using var context = CreateContext();
                var productService = context.ProductServices.FirstOrDefault(x => x.Id == id);
                if (productService == null)
                    return false;

                // Kiểm tra có biến thể sản phẩm không
                var hasVariants = context.ProductVariants.Any(x => x.ProductId == id);
                if (hasVariants)
                    throw new InvalidOperationException("Không thể xóa sản phẩm/dịch vụ có biến thể");

                // Kiểm tra có ảnh sản phẩm không
                var hasImages = context.ProductImages.Any(x => x.ProductId == id);
                if (hasImages)
                {
                    // Xóa tất cả ảnh sản phẩm trước
                    var imagesToDelete = context.ProductImages.Where(x => x.ProductId == id).ToList();
                    context.ProductImages.DeleteAllOnSubmit(imagesToDelete);
                }

                context.ProductServices.DeleteOnSubmit(productService);
                context.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa sản phẩm/dịch vụ {id}: {ex.Message}", ex);
            }
        }

        #endregion

        #region Validation & Business Rules

        /// <summary>
        /// Kiểm tra mã sản phẩm/dịch vụ có tồn tại không.
        /// </summary>
        /// <param name="code">Mã sản phẩm/dịch vụ</param>
        /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại</returns>
        public bool IsCodeExists(string code, Guid? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return false;

                using var context = CreateContext();
                var query = context.ProductServices.Where(x => x.Code == code.Trim());
                
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);

                return query.Any();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra mã sản phẩm/dịch vụ '{code}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã sản phẩm/dịch vụ có tồn tại không (Async).
        /// </summary>
        /// <param name="code">Mã sản phẩm/dịch vụ</param>
        /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
        /// <returns>Task chứa True nếu tồn tại</returns>
        public async Task<bool> IsCodeExistsAsync(string code, Guid? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(code))
                    return false;

                using var context = CreateContext();
                var query = context.ProductServices.Where(x => x.Code == code.Trim());
                
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);

                return await Task.Run(() => query.Any());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra mã sản phẩm/dịch vụ '{code}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên sản phẩm/dịch vụ có tồn tại không.
        /// </summary>
        /// <param name="name">Tên sản phẩm/dịch vụ</param>
        /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
        /// <returns>True nếu tồn tại</returns>
        public bool IsNameExists(string name, Guid? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return false;

                using var context = CreateContext();
                var query = context.ProductServices.Where(x => x.Name == name.Trim());
                
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);

                return query.Any();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra tên sản phẩm/dịch vụ '{name}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên sản phẩm/dịch vụ có tồn tại không (Async).
        /// </summary>
        /// <param name="name">Tên sản phẩm/dịch vụ</param>
        /// <param name="excludeId">ID sản phẩm/dịch vụ cần loại trừ (khi cập nhật)</param>
        /// <returns>Task chứa True nếu tồn tại</returns>
        public async Task<bool> IsNameExistsAsync(string name, Guid? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                    return false;

                using var context = CreateContext();
                var query = context.ProductServices.Where(x => x.Name == name.Trim());
                
                if (excludeId.HasValue)
                    query = query.Where(x => x.Id != excludeId.Value);

                return await Task.Run(() => query.Any());
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra tên sản phẩm/dịch vụ '{name}': {ex.Message}", ex);
            }
        }

        #endregion

        #region Statistics Methods

        /// <summary>
        /// Đếm tổng số sản phẩm/dịch vụ.
        /// </summary>
        /// <returns>Số lượng sản phẩm/dịch vụ</returns>
        public int GetTotalCount()
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices.Count();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm tổng số sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số sản phẩm.
        /// </summary>
        /// <returns>Số lượng sản phẩm</returns>
        public int GetProductCount()
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices.Count(ps => !ps.IsService);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm số sản phẩm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số dịch vụ.
        /// </summary>
        /// <returns>Số lượng dịch vụ</returns>
        public int GetServiceCount()
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices.Count(ps => ps.IsService);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm số dịch vụ: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đếm số sản phẩm/dịch vụ theo danh mục.
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <returns>Số lượng sản phẩm/dịch vụ</returns>
        public int GetCountByCategory(Guid categoryId)
        {
            try
            {
                using var context = CreateContext();
                return context.ProductServices.Count(ps => ps.CategoryId == categoryId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm số sản phẩm/dịch vụ theo danh mục {categoryId}: {ex.Message}", ex);
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
                using var context = CreateContext();
                return context.ProductVariants.Count(pv => pv.ProductId == productServiceId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm số biến thể cho sản phẩm/dịch vụ {productServiceId}: {ex.Message}", ex);
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
                using var context = CreateContext();
                // Đếm cả ảnh của sản phẩm và ảnh của các biến thể
                var productImages = context.ProductImages.Count(pi => pi.ProductId == productServiceId);
                var variantImages = context.ProductImages.Count(pi => 
                    pi.VariantId.HasValue && 
                    context.ProductVariants.Any(pv => pv.Id == pi.VariantId && pv.ProductId == productServiceId));
                
                return productImages + variantImages;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm số hình ảnh cho sản phẩm/dịch vụ {productServiceId}: {ex.Message}", ex);
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
                if (productServiceIds == null || !productServiceIds.Any())
                    return new Dictionary<Guid, (int, int)>();

                using var context = CreateContext();
                var result = new Dictionary<Guid, (int, int)>();

                // Đếm biến thể cho tất cả sản phẩm trong một query
                var variantCounts = context.ProductVariants
                    .Where(pv => productServiceIds.Contains(pv.ProductId))
                    .GroupBy(pv => pv.ProductId)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Đếm hình ảnh cho tất cả sản phẩm trong một query
                var imageCounts = new Dictionary<Guid, int>();
                
                // Đếm ảnh trực tiếp của sản phẩm
                var productImageCounts = context.ProductImages
                    .Where(pi => productServiceIds.Contains((Guid)pi.ProductId))
                    .GroupBy(pi => pi.ProductId)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Đếm ảnh của biến thể
                var variantImageCounts = context.ProductImages
                    .Where(pi => pi.VariantId.HasValue && 
                                context.ProductVariants.Any(pv => pv.Id == pi.VariantId && productServiceIds.Contains(pv.ProductId)))
                    .GroupBy(pi => context.ProductVariants.First(pv => pv.Id == pi.VariantId).ProductId)
                    .ToDictionary(g => g.Key, g => g.Count());

                // Kết hợp kết quả
                foreach (var id in productServiceIds)
                {
                    var variantCount = variantCounts.ContainsKey(id) ? variantCounts[id] : 0;
                    var productImageCount = productImageCounts.ContainsKey(id) ? productImageCounts[id] : 0;
                    var variantImageCount = variantImageCounts.ContainsKey(id) ? variantImageCounts[id] : 0;
                    
                    result[id] = (variantCount, productImageCount + variantImageCount);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đếm số lượng cho nhiều sản phẩm/dịch vụ: {ex.Message}", ex);
            }
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lưu hoặc cập nhật sản phẩm/dịch vụ.
        /// </summary>
        /// <param name="productService">Sản phẩm/dịch vụ cần lưu hoặc cập nhật</param>
        public void SaveOrUpdate(ProductService productService)
        {
            try
            {
                if (productService == null) 
                    throw new ArgumentNullException(nameof(productService));
                
                using var context = CreateContext();
                var existing = productService.Id != Guid.Empty ? context.ProductServices.FirstOrDefault(x => x.Id == productService.Id) : null;
                
                if (existing == null)
                {
                    // Thêm mới
                    if (productService.Id == Guid.Empty)
                        productService.Id = Guid.NewGuid();
                    context.ProductServices.InsertOnSubmit(productService);
                }
                else
                {
                    // Cập nhật
                    existing.Code = productService.Code;
                    existing.Name = productService.Name;
                    existing.CategoryId = productService.CategoryId;
                    existing.IsService = productService.IsService;
                    existing.Description = productService.Description;
                    existing.IsActive = productService.IsActive;
                    existing.ThumbnailImage = productService.ThumbnailImage;
                }
                
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lưu hoặc cập nhật sản phẩm/dịch vụ '{productService?.Name}': {ex.Message}", ex);
            }
        }

        #endregion

        /// <summary>
        /// Xóa sản phẩm/dịch vụ theo ID.
        /// </summary>
        /// <param name="id">ID sản phẩm/dịch vụ cần xóa</param>
        public void DeleteById(Guid id)
        {
            DeleteProductService(id);
        }

        /// <summary>
        /// Lấy số tiếp theo cho mã sản phẩm trong danh mục.
        /// </summary>
        /// <param name="categoryId">ID danh mục</param>
        /// <param name="prefix">Prefix chữ cái đầu</param>
        /// <returns>Số tiếp theo (1-9999)</returns>
        public int GetNextProductNumber(Guid categoryId, string prefix)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(prefix))
                    return 1;

                using var context = CreateContext();
                
                // Tìm mã sản phẩm có prefix giống và cùng categoryId
                var existingCodes = context.ProductServices
                    .Where(ps => ps.CategoryId == categoryId && ps.Code.StartsWith(prefix))
                    .Select(ps => ps.Code)
                    .ToList();

                if (!existingCodes.Any())
                    return 1;

                // Tìm số lớn nhất trong các mã hiện có
                var maxNumber = 0;
                foreach (var code in existingCodes)
                {
                    // Lấy phần số cuối (4 chữ số)
                    if (code.Length >= prefix.Length + 4)
                    {
                        var numberPart = code.Substring(prefix.Length, 4);
                        if (int.TryParse(numberPart, out var number))
                        {
                            maxNumber = Math.Max(maxNumber, number);
                        }
                    }
                }

                // Trả về số tiếp theo, nhưng không vượt quá 9999
                return Math.Min(maxNumber + 1, 9999);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy số tiếp theo cho mã sản phẩm: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy mã danh mục từ ID danh mục.
        /// </summary>
        /// <param name="categoryId">ID của danh mục</param>
        /// <returns>Mã danh mục</returns>
        public string GetCategoryCode(Guid categoryId)
        {
            try
            {
                using var context = CreateContext();
                var category = context.ProductServiceCategories
                    .Where(c => c.Id == categoryId)
                    .Select(c => c.CategoryCode)
                    .FirstOrDefault();
                return category ?? string.Empty;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy mã danh mục: {ex.Message}", ex);
            }
        }

    }
}