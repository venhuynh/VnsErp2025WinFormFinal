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

namespace Dal.DataAccess.Implementations.MasterData.ProductServiceRepositories;

/// <summary>
/// Data Access cho thực thể ProductService (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public class ProductServiceRepository : IProductServiceRepository
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
    /// Khởi tạo một instance mới của class ProductServiceRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public ProductServiceRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("ProductServiceRepository được khởi tạo với connection string");
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

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;

        return context;
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

            using var context = CreateNewContext();
            
            // Tìm mã sản phẩm có prefix giống và cùng categoryId
            var existingCodes = context.ProductServices
                .Where(ps => ps.CategoryId == categoryId && ps.Code.StartsWith(prefix))
                .Select(ps => ps.Code)
                .ToList();

            if (!existingCodes.Any())
            {
                _logger.Debug($"GetNextProductNumber: Không tìm thấy mã nào với prefix '{prefix}' trong category {categoryId}, trả về 1");
                return 1;
            }

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
            var nextNumber = Math.Min(maxNumber + 1, 9999);
            _logger.Debug($"GetNextProductNumber: CategoryId={categoryId}, Prefix='{prefix}', MaxNumber={maxNumber}, NextNumber={nextNumber}");
            return nextNumber;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy số tiếp theo cho mã sản phẩm: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var category = context.ProductServiceCategories
                .Where(c => c.Id == categoryId)
                .Select(c => c.CategoryCode)
                .FirstOrDefault();
            
            var result = category ?? string.Empty;
            _logger.Debug($"GetCategoryCode: CategoryId={categoryId}, Code='{result}'");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy mã danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy mã danh mục: {ex.Message}", ex);
        }
    }

    #endregion

    #region Read

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo Id.
    /// </summary>
    public ProductService GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var productService = context.ProductServices.FirstOrDefault(x => x.Id == id);
            
            if (productService != null)
            {
                _logger.Debug($"Đã lấy sản phẩm/dịch vụ theo ID: {id} - {productService.Name}");
            }
            else
            {
                _logger.Debug($"Không tìm thấy sản phẩm/dịch vụ với ID: {id}");
            }
            
            return productService;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy sản phẩm/dịch vụ theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy sản phẩm/dịch vụ theo Id (Async).
    /// </summary>
    public async Task<ProductService> GetByIdAsync(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var productService = await Task.Run(() => context.ProductServices.FirstOrDefault(x => x.Id == id));
            
            if (productService != null)
            {
                _logger.Debug($"Đã lấy sản phẩm/dịch vụ theo ID (async): {id} - {productService.Name}");
            }
            else
            {
                _logger.Debug($"Không tìm thấy sản phẩm/dịch vụ với ID (async): {id}");
            }
            
            return productService;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy sản phẩm/dịch vụ theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy sản phẩm/dịch vụ theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả sản phẩm/dịch vụ.
    /// </summary>
    public List<ProductService> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            var products = context.ProductServices
                .OrderBy(ps => ps.Code)
                .ToList();
            
            _logger.Debug($"Đã lấy {products.Count} sản phẩm/dịch vụ");
            return products;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả sản phẩm/dịch vụ: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả sản phẩm/dịch vụ: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả sản phẩm/dịch vụ (Async).
    /// </summary>
    public async Task<List<ProductService>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var products = await Task.Run(() => context.ProductServices
                .OrderBy(ps => ps.Code)
                .ToList());
            
            _logger.Debug($"Đã lấy {products.Count} sản phẩm/dịch vụ (async)");
            return products;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả sản phẩm/dịch vụ: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var productService = context.ProductServices.FirstOrDefault(x => x.Code == code.Trim());
            
            if (productService != null)
            {
                _logger.Debug($"Đã lấy sản phẩm/dịch vụ theo mã: '{code}' - {productService.Name}");
            }
            else
            {
                _logger.Debug($"Không tìm thấy sản phẩm/dịch vụ với mã: '{code}'");
            }
            
            return productService;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy sản phẩm/dịch vụ theo mã '{code}': {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var productService = context.ProductServices.FirstOrDefault(x => x.Name == name.Trim());
            
            if (productService != null)
            {
                _logger.Debug($"Đã lấy sản phẩm/dịch vụ theo tên: '{name}'");
            }
            else
            {
                _logger.Debug($"Không tìm thấy sản phẩm/dịch vụ với tên: '{name}'");
            }
            
            return productService;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy sản phẩm/dịch vụ theo tên '{name}': {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var products = context.ProductServices
                .Where(ps => ps.CategoryId == categoryId)
                .OrderBy(ps => ps.Code)
                .ToList();
            
            _logger.Debug($"Đã lấy {products.Count} sản phẩm/dịch vụ theo danh mục: {categoryId}");
            return products;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy sản phẩm/dịch vụ theo danh mục {categoryId}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var products = context.ProductServices
                .Where(ps => ps.IsService == isService)
                .OrderBy(ps => ps.Code)
                .ToList();
            
            _logger.Debug($"Đã lấy {products.Count} {(isService ? "dịch vụ" : "sản phẩm")}");
            return products;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy sản phẩm/dịch vụ theo loại {(isService ? "dịch vụ" : "sản phẩm")}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var products = context.ProductServices
                .Where(ps => ps.IsActive == isActive)
                .OrderBy(ps => ps.Code)
                .ToList();
            
            _logger.Debug($"Đã lấy {products.Count} sản phẩm/dịch vụ {(isActive ? "hoạt động" : "không hoạt động")}");
            return products;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy sản phẩm/dịch vụ theo trạng thái {(isActive ? "hoạt động" : "không hoạt động")}: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var results = context.ProductServices
                .Where(ps => ps.Code.ToLower().Contains(searchTerm) ||
                            ps.Name.ToLower().Contains(searchTerm) ||
                            (ps.Description != null && ps.Description.ToLower().Contains(searchTerm)) ||
                            context.ProductServiceCategories.Any(c => c.Id == ps.CategoryId && c.CategoryName.ToLower().Contains(searchTerm)))
                .OrderBy(ps => ps.Name)
                .ToList();
            
            _logger.Debug($"Tìm kiếm với từ khóa '{keyword}': tìm thấy {results.Count} kết quả");
            return results;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tìm kiếm sản phẩm/dịch vụ với từ khóa '{keyword}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tìm kiếm sản phẩm/dịch vụ với từ khóa '{keyword}': {ex.Message}", ex);
        }
    }

    #endregion

    #region Create/Update

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
            
            using var context = CreateNewContext();
            var existing = productService.Id != Guid.Empty 
                ? context.ProductServices.FirstOrDefault(x => x.Id == productService.Id) 
                : null;
            
            if (existing == null)
            {
                // Thêm mới
                if (productService.Id == Guid.Empty)
                    productService.Id = Guid.NewGuid();
                    
                context.ProductServices.InsertOnSubmit(productService);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới sản phẩm/dịch vụ: {productService.Name} (ID: {productService.Id})");
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
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật sản phẩm/dịch vụ: {existing.Name} (ID: {existing.Id})");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu/cập nhật sản phẩm/dịch vụ '{productService?.Name}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu/cập nhật sản phẩm/dịch vụ '{productService?.Name}': {ex.Message}", ex);
        }
    }

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
            using var context = CreateNewContext();
            var productService = context.ProductServices.FirstOrDefault(x => x.Id == id);
            if (productService == null)
            {
                _logger.Warning($"Không tìm thấy sản phẩm/dịch vụ để xóa: {id}");
                return false;
            }

            // Kiểm tra có biến thể sản phẩm không
            var hasVariants = context.ProductVariants.Any(x => x.ProductId == id);
            if (hasVariants)
            {
                _logger.Warning($"Không thể xóa sản phẩm/dịch vụ {id} - {productService.Name} vì có biến thể");
                throw new InvalidOperationException("Không thể xóa sản phẩm/dịch vụ có biến thể");
            }

            // Kiểm tra có ảnh sản phẩm không
            var hasImages = context.ProductImages.Any(x => x.ProductId == id);
            if (hasImages)
            {
                // Xóa tất cả ảnh sản phẩm trước
                var imagesToDelete = context.ProductImages.Where(x => x.ProductId == id).ToList();
                context.ProductImages.DeleteAllOnSubmit(imagesToDelete);
                _logger.Info($"Đã xóa {imagesToDelete.Count} ảnh của sản phẩm/dịch vụ {id}");
            }

            context.ProductServices.DeleteOnSubmit(productService);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa sản phẩm/dịch vụ: {id} - {productService.Name}");
            return true;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa sản phẩm/dịch vụ {id}: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var query = context.ProductServices.Where(x => x.Code == code.Trim());
            
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            var result = query.Any();
            _logger.Debug($"IsCodeExists: Code='{code}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra mã sản phẩm/dịch vụ '{code}': {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var query = context.ProductServices.Where(x => x.Code == code.Trim());
            
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            var result = await Task.Run(() => query.Any());
            _logger.Debug($"IsCodeExistsAsync: Code='{code}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra mã sản phẩm/dịch vụ '{code}': {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var query = context.ProductServices.Where(x => x.Name == name.Trim());
            
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            var result = query.Any();
            _logger.Debug($"IsNameExists: Name='{name}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tên sản phẩm/dịch vụ '{name}': {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var query = context.ProductServices.Where(x => x.Name == name.Trim());
            
            if (excludeId.HasValue)
                query = query.Where(x => x.Id != excludeId.Value);

            var result = await Task.Run(() => query.Any());
            _logger.Debug($"IsNameExistsAsync: Name='{name}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tên sản phẩm/dịch vụ '{name}': {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var count = context.ProductServices.Count();
            _logger.Debug($"GetTotalCount: {count}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm tổng số sản phẩm/dịch vụ: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var count = context.ProductServices.Count(ps => !ps.IsService);
            _logger.Debug($"GetProductCount: {count}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số sản phẩm: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var count = context.ProductServices.Count(ps => ps.IsService);
            _logger.Debug($"GetServiceCount: {count}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số dịch vụ: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var count = context.ProductServices.Count(ps => ps.CategoryId == categoryId);
            _logger.Debug($"GetCountByCategory: CategoryId={categoryId}, Count={count}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số sản phẩm/dịch vụ theo danh mục {categoryId}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            var count = context.ProductVariants.Count(pv => pv.ProductId == productServiceId);
            _logger.Debug($"GetVariantCount: ProductServiceId={productServiceId}, Count={count}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số biến thể cho sản phẩm/dịch vụ {productServiceId}: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            // Đếm ảnh của sản phẩm (ProductImage không còn VariantId property)
            var productImages = context.ProductImages.Count(pi => pi.ProductId == productServiceId);
            
            _logger.Debug($"GetImageCount: ProductServiceId={productServiceId}, ProductImages={productImages}");
            return productImages;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số hình ảnh cho sản phẩm/dịch vụ {productServiceId}: {ex.Message}", ex);
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

            using var context = CreateNewContext();
            var result = new Dictionary<Guid, (int, int)>();

            // Đếm biến thể cho tất cả sản phẩm trong một query
            var variantCounts = context.ProductVariants
                .Where(pv => productServiceIds.Contains(pv.ProductId))
                .GroupBy(pv => pv.ProductId)
                .ToDictionary(g => g.Key, g => g.Count());

            // Đếm ảnh trực tiếp của sản phẩm
            var productImageCounts = context.ProductImages
                .Where(pi => productServiceIds.Contains((Guid)pi.ProductId))
                .GroupBy(pi => pi.ProductId)
                .ToDictionary(g => g.Key, g => g.Count());

            // Kết hợp kết quả (ProductImage không còn VariantId property)
            foreach (var id in productServiceIds)
            {
                var variantCount = variantCounts.ContainsKey(id) ? variantCounts[id] : 0;
                var productImageCount = productImageCounts.ContainsKey(id) ? productImageCounts[id] : 0;
                
                result[id] = (variantCount, productImageCount);
            }

            _logger.Debug($"GetCountsForProducts: Processed {result.Count} products");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số lượng cho nhiều sản phẩm/dịch vụ: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm số lượng cho nhiều sản phẩm/dịch vụ: {ex.Message}", ex);
        }
    }

    #endregion

    #region Pagination & Optimization Methods

    /// <summary>
    /// Lấy số lượng tổng cộng với filter
    /// </summary>
    public async Task<int> GetCountAsync(
        string searchText = null,
        Guid? categoryId = null,
        bool? isService = null,
        bool? isActive = null)
    {
        try
        {
            using var context = CreateNewContext();
            
            var query = context.ProductServices.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(x => 
                    x.Code.Contains(searchText) || 
                    x.Name.Contains(searchText) || 
                    x.Description.Contains(searchText));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (isService.HasValue)
            {
                query = query.Where(x => x.IsService == isService.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            var count = await Task.FromResult(query.Count());
            _logger.Debug($"GetCountAsync: Count={count}, SearchText='{searchText}', CategoryId={categoryId}, IsService={isService}, IsActive={isActive}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số lượng sản phẩm/dịch vụ: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm số lượng sản phẩm/dịch vụ: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy dữ liệu phân trang
    /// </summary>
    public async Task<List<ProductService>> GetPagedAsync(
        int pageIndex,
        int pageSize,
        string searchText = null,
        Guid? categoryId = null,
        bool? isService = null,
        bool? isActive = null)
    {
        try
        {
            using var context = CreateNewContext();
            
            var query = context.ProductServices.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(x => 
                    x.Code.Contains(searchText) || 
                    x.Name.Contains(searchText) || 
                    x.Description.Contains(searchText));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (isService.HasValue)
            {
                query = query.Where(x => x.IsService == isService.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            // Apply pagination
            var skip = pageIndex * pageSize;
            var result = query
                .OrderBy(x => x.Name)
                .Skip(skip)
                .Take(pageSize)
                .ToList();

            _logger.Debug($"GetPagedAsync: PageIndex={pageIndex}, PageSize={pageSize}, ResultCount={result.Count}");
            return await Task.FromResult(result);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy dữ liệu phân trang: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy dữ liệu phân trang: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy dữ liệu thumbnail image cho lazy loading
    /// </summary>
    public byte[] GetThumbnailImageData(Guid productId)
    {
        try
        {
            using var context = CreateNewContext();
            var imageData = context.ProductServices
                .Where(x => x.Id == productId)
                .Select(x => x.ThumbnailImage.ToArray())
                .FirstOrDefault();
            
            if (imageData != null)
            {
                _logger.Debug($"GetThumbnailImageData: ProductId={productId}, ImageSize={imageData.Length} bytes");
            }
            else
            {
                _logger.Debug($"GetThumbnailImageData: ProductId={productId}, No image found");
            }
            
            return imageData;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy dữ liệu ảnh thumbnail: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy dữ liệu ảnh thumbnail: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách sản phẩm với search và filter (optimized)
    /// </summary>
    public async Task<List<ProductService>> GetFilteredAsync(
        string searchText = null,
        Guid? categoryId = null,
        bool? isService = null,
        bool? isActive = null,
        string orderBy = "Name",
        string orderDirection = "ASC")
    {
        try
        {
            using var context = CreateNewContext();
            
            var query = context.ProductServices.AsQueryable();

            // Apply filters
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(x => 
                    x.Code.Contains(searchText) || 
                    x.Name.Contains(searchText) || 
                    x.Description.Contains(searchText));
            }

            if (categoryId.HasValue)
            {
                query = query.Where(x => x.CategoryId == categoryId.Value);
            }

            if (isService.HasValue)
            {
                query = query.Where(x => x.IsService == isService.Value);
            }

            if (isActive.HasValue)
            {
                query = query.Where(x => x.IsActive == isActive.Value);
            }

            // Apply ordering
            switch (orderBy.ToLower())
            {
                case "code":
                    query = orderDirection.ToUpper() == "DESC" 
                        ? query.OrderByDescending(x => x.Code)
                        : query.OrderBy(x => x.Code);
                    break;
                case "name":
                    query = orderDirection.ToUpper() == "DESC" 
                        ? query.OrderByDescending(x => x.Name)
                        : query.OrderBy(x => x.Name);
                    break;
                case "category":
                    query = orderDirection.ToUpper() == "DESC" 
                        ? query.OrderByDescending(x => x.CategoryId)
                        : query.OrderBy(x => x.CategoryId);
                    break;
                default:
                    query = query.OrderBy(x => x.Name);
                    break;
            }

            var result = await Task.FromResult(query.ToList());
            _logger.Debug($"GetFilteredAsync: ResultCount={result.Count}, OrderBy={orderBy}, OrderDirection={orderDirection}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy dữ liệu với filter: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy dữ liệu với filter: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy counts cho nhiều sản phẩm cùng lúc (optimized async version)
    /// </summary>
    public async Task<Dictionary<Guid, (int VariantCount, int ImageCount)>> GetCountsForProductsAsync(List<Guid> productIds)
    {
        try
        {
            if (productIds == null || !productIds.Any())
                return new Dictionary<Guid, (int, int)>();

            using var context = CreateNewContext();
            
            // Get variant counts
            var variantCounts = await Task.FromResult(
                context.ProductVariants
                    .Where(x => productIds.Contains(x.ProductId))
                    .GroupBy(x => x.ProductId)
                    .ToDictionary(g => g.Key, g => g.Count())
            );

            // Get image counts
            var imageCounts = await Task.FromResult(
                context.ProductImages
                    .Where(x => productIds.Contains(x.ProductId.Value))
                    .GroupBy(x => x.ProductId)
                    .ToDictionary(g => g.Key, g => g.Count())
            );

            // Combine results
            var result = new Dictionary<Guid, (int, int)>();
            foreach (var productId in productIds)
            {
                var variantCount = variantCounts.ContainsKey(productId) ? variantCounts[productId] : 0;
                var imageCount = imageCounts.ContainsKey(productId) ? imageCounts[productId] : 0;
                result[productId] = (variantCount, imageCount);
            }

            _logger.Debug($"GetCountsForProductsAsync: Processed {result.Count} products");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số lượng cho nhiều sản phẩm: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm số lượng cho nhiều sản phẩm: {ex.Message}", ex);
        }
    }

    #endregion

    #region Search and Filter Methods

    /// <summary>
    /// Tìm kiếm sản phẩm/dịch vụ trong toàn bộ database
    /// </summary>
    /// <param name="searchText">Text tìm kiếm</param>
    /// <returns>Danh sách kết quả tìm kiếm</returns>
    public async Task<List<ProductService>> SearchAsync(string searchText)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return new List<ProductService>();

            using var context = CreateNewContext();
            
            var searchTerm = searchText.Trim().ToLower();
            
            var query = context.ProductServices
                .Where(x => x.Code.ToLower().Contains(searchTerm) ||
                           x.Name.ToLower().Contains(searchTerm) ||
                           (x.Description != null && x.Description.ToLower().Contains(searchTerm)) ||
                           context.ProductServiceCategories.Any(c => c.Id == x.CategoryId && c.CategoryName.ToLower().Contains(searchTerm)))
                .OrderBy(x => x.Name);

            var result = await Task.FromResult(query.ToList());
            _logger.Debug($"SearchAsync: SearchText='{searchText}', ResultCount={result.Count}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi tìm kiếm sản phẩm/dịch vụ: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi tìm kiếm sản phẩm/dịch vụ: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            
            var codes = context.ProductServices
                .Where(x => !string.IsNullOrEmpty(x.Code))
                .Select(x => x.Code)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var result = await Task.FromResult(codes.Cast<object>().ToList());
            _logger.Debug($"GetUniqueCodesAsync: Count={result.Count}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách mã code unique: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi lấy danh sách mã code unique: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            
            var names = context.ProductServices
                .Where(x => !string.IsNullOrEmpty(x.Name))
                .Select(x => x.Name)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var result = await Task.FromResult(names.Cast<object>().ToList());
            _logger.Debug($"GetUniqueNamesAsync: Count={result.Count}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách tên unique: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi lấy danh sách tên unique: {ex.Message}", ex);
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
            using var context = CreateNewContext();
            
            var categoryNames = context.ProductServiceCategories
                .Where(x => !string.IsNullOrEmpty(x.CategoryName))
                .Select(x => x.CategoryName)
                .Distinct()
                .OrderBy(x => x)
                .ToList();

            var result = await Task.FromResult(categoryNames.Cast<object>().ToList());
            _logger.Debug($"GetUniqueCategoryNamesAsync: Count={result.Count}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách tên danh mục unique: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi lấy danh sách tên danh mục unique: {ex.Message}", ex);
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
            var typeDisplays = new List<object>
            {
                "Sản phẩm",
                "Dịch vụ"
            };

            var result = await Task.FromResult(typeDisplays);
            _logger.Debug($"GetUniqueTypeDisplaysAsync: Count={result.Count}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách loại hiển thị unique: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi lấy danh sách loại hiển thị unique: {ex.Message}", ex);
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
            var statusDisplays = new List<object>
            {
                "Hoạt động",
                "Không hoạt động"
            };

            var result = await Task.FromResult(statusDisplays);
            _logger.Debug($"GetUniqueStatusDisplaysAsync: Count={result.Count}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách trạng thái hiển thị unique: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi lấy danh sách trạng thái hiển thị unique: {ex.Message}", ex);
        }
    }

    #endregion
}
