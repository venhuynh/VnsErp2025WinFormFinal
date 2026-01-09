using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.DtoConverter.MasterData.Customer;
using Dal.Exceptions;
using DTO.MasterData.CustomerPartner;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.PartnerRepository;

/// <summary>
/// Data Access cho thực thể BusinessPartnerCategory (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public class BusinessPartnerCategoryRepository : IBusinessPartnerCategoryRepository
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
    /// Khởi tạo một instance mới của class BusinessPartnerCategoryRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public BusinessPartnerCategoryRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("BusinessPartnerCategoryRepository được khởi tạo với connection string");
    }

    #endregion

    #region ========== HELPER METHODS ==========

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
    /// Lấy tất cả sub-categories của một category (đệ quy).
    /// </summary>
    private List<Guid> GetSubCategories(Guid categoryId, Dictionary<Guid, BusinessPartnerCategory> categoryDict)
    {
        var result = new List<Guid>();
        var directChildren = categoryDict.Values.Where(c => c.ParentId == categoryId).ToList();
        
        foreach (var child in directChildren)
        {
            result.Add(child.Id);
            // Đệ quy lấy các cháu
            var grandChildren = GetSubCategories(child.Id, categoryDict);
            result.AddRange(grandChildren);
        }
        
        return result;
    }

    /// <summary>
    /// Chuyển tất cả partners từ category này sang category "Chưa phân loại".
    /// </summary>
    private void MovePartnersToUncategorizedCategory(Guid categoryId, VnsErp2025DataContext context)
    {
        // Tìm hoặc tạo category "Chưa phân loại"
        var uncategorizedCategory = context.BusinessPartnerCategories
            .FirstOrDefault(x => x.CategoryName.Trim().ToLower() == "chưa phân loại");

        if (uncategorizedCategory == null)
        {
            // Tạo category "Chưa phân loại" nếu chưa có
            uncategorizedCategory = new BusinessPartnerCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Chưa phân loại",
                Description = "Danh mục mặc định cho các đối tác chưa được phân loại",
                ParentId = null
            };
            context.BusinessPartnerCategories.InsertOnSubmit(uncategorizedCategory);
            context.SubmitChanges(); // Submit để có ID
        }

        // Chuyển tất cả partners từ category cũ sang "Chưa phân loại"
        // Lưu ý: Không thể cập nhật CategoryId vì nó là phần của composite primary key
        // Cần tạo record mới và xóa record cũ
        var oldMappings = context.BusinessPartner_BusinessPartnerCategories
            .Where(m => m.CategoryId == categoryId).ToList();

        foreach (var oldMapping in oldMappings)
        {
            // Kiểm tra xem mapping mới đã tồn tại chưa (tránh duplicate)
            var existingMapping = context.BusinessPartner_BusinessPartnerCategories
                .FirstOrDefault(m => m.PartnerId == oldMapping.PartnerId && 
                                    m.CategoryId == uncategorizedCategory.Id);
            
            if (existingMapping == null)
            {
                // Tạo mapping mới
                var newMapping = new BusinessPartner_BusinessPartnerCategory
                {
                    PartnerId = oldMapping.PartnerId,
                    CategoryId = uncategorizedCategory.Id,
                    CreatedDate = DateTime.Now,
                    CreatedBy = oldMapping.CreatedBy
                };
                context.BusinessPartner_BusinessPartnerCategories.InsertOnSubmit(newMapping);
            }
            
            // Xóa mapping cũ
            context.BusinessPartner_BusinessPartnerCategories.DeleteOnSubmit(oldMapping);
        }
        
        context.SubmitChanges();
        _logger.Info($"Đã chuyển {oldMappings.Count} partners từ category {categoryId} sang 'Chưa phân loại'");
    }

    /// <summary>
    /// Chuyển tất cả partners từ category này sang category "Chưa phân loại" (Async).
    /// </summary>
    private async Task MovePartnersToUncategorizedCategoryAsync(Guid categoryId, VnsErp2025DataContext context)
    {
        // Tìm hoặc tạo category "Chưa phân loại"
        var uncategorizedCategory = context.BusinessPartnerCategories
            .FirstOrDefault(x => x.CategoryName.Trim().ToLower() == "chưa phân loại");

        if (uncategorizedCategory == null)
        {
            // Tạo category "Chưa phân loại" nếu chưa có
            uncategorizedCategory = new BusinessPartnerCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = "Chưa phân loại",
                Description = "Danh mục mặc định cho các đối tác chưa được phân loại",
                ParentId = null
            };
            context.BusinessPartnerCategories.InsertOnSubmit(uncategorizedCategory);
            await Task.Run(() => context.SubmitChanges()); // Submit để có ID
        }

        // Chuyển tất cả partners từ category cũ sang "Chưa phân loại"
        // Lưu ý: Không thể cập nhật CategoryId vì nó là phần của composite primary key
        // Cần tạo record mới và xóa record cũ
        var oldMappings = context.BusinessPartner_BusinessPartnerCategories
            .Where(m => m.CategoryId == categoryId).ToList();

        foreach (var oldMapping in oldMappings)
        {
            // Kiểm tra xem mapping mới đã tồn tại chưa (tránh duplicate)
            var existingMapping = context.BusinessPartner_BusinessPartnerCategories
                .FirstOrDefault(m => m.PartnerId == oldMapping.PartnerId && 
                                    m.CategoryId == uncategorizedCategory.Id);
            
            if (existingMapping == null)
            {
                // Tạo mapping mới
                var newMapping = new BusinessPartner_BusinessPartnerCategory
                {
                    PartnerId = oldMapping.PartnerId,
                    CategoryId = uncategorizedCategory.Id,
                    CreatedDate = DateTime.Now,
                    CreatedBy = oldMapping.CreatedBy
                };
                context.BusinessPartner_BusinessPartnerCategories.InsertOnSubmit(newMapping);
            }
            
            // Xóa mapping cũ
            context.BusinessPartner_BusinessPartnerCategories.DeleteOnSubmit(oldMapping);
        }
        
        await Task.Run(() => context.SubmitChanges());
        _logger.Info($"Đã chuyển {oldMappings.Count} partners từ category {categoryId} sang 'Chưa phân loại' (async)");
    }

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Thêm danh mục đối tác mới với validation cơ bản.
    /// </summary>
    /// <param name="categoryName">Tên danh mục</param>
    /// <param name="description">Mô tả</param>
    /// <returns>BusinessPartnerCategoryDto đã tạo</returns>
    public BusinessPartnerCategoryDto AddNewCategory(string categoryName, string description = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Tên danh mục không được rỗng", nameof(categoryName));

            if (IsCategoryNameExists(categoryName))
                throw new DataAccessException($"Tên danh mục '{categoryName}' đã tồn tại");

            using var context = CreateNewContext();
            var entity = new BusinessPartnerCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = categoryName.Trim(),
                Description = description?.Trim(),
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.BusinessPartnerCategories.InsertOnSubmit(entity);
            context.SubmitChanges();
            
            _logger.Info($"Đã thêm mới danh mục: {entity.CategoryName}");
            return entity.ToDto();
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 2627)
        {
            _logger.Error($"Trùng tên danh mục '{categoryName}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Trùng tên danh mục '{categoryName}'", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi thêm danh mục '{categoryName}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi thêm danh mục '{categoryName}': {sqlEx.Message}", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi thêm danh mục mới '{categoryName}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi thêm danh mục mới '{categoryName}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Thêm danh mục đối tác mới (Async).
    /// </summary>
    public async Task<BusinessPartnerCategoryDto> AddNewCategoryAsync(string categoryName, string description = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Tên danh mục không được rỗng", nameof(categoryName));

            if (await IsCategoryNameExistsAsync(categoryName))
                throw new DataAccessException($"Tên danh mục '{categoryName}' đã tồn tại");

            using var context = CreateNewContext();
            var entity = new BusinessPartnerCategory
            {
                Id = Guid.NewGuid(),
                CategoryName = categoryName.Trim(),
                Description = description?.Trim(),
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            context.BusinessPartnerCategories.InsertOnSubmit(entity);
            await Task.Run(() => context.SubmitChanges());
            
            _logger.Info($"Đã thêm mới danh mục (async): {entity.CategoryName}");
            return entity.ToDto();
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 2627)
        {
            _logger.Error($"Trùng tên danh mục '{categoryName}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Trùng tên danh mục '{categoryName}'", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi thêm danh mục '{categoryName}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi thêm danh mục '{categoryName}': {sqlEx.Message}", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi thêm danh mục mới '{categoryName}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi thêm danh mục mới '{categoryName}': {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy danh mục theo Id.
    /// </summary>
    public BusinessPartnerCategoryDto GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var category = context.BusinessPartnerCategories.FirstOrDefault(x => x.Id == id);
            
            if (category != null)
            {
                _logger.Debug($"Đã lấy danh mục theo ID: {id} - {category.CategoryName}");
                return category.ToDto();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh mục theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh mục theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả danh mục.
    /// </summary>
    public List<BusinessPartnerCategoryDto> GetAll()
    {
        try
        {
            using var context = CreateNewContext();
            var categories = context.BusinessPartnerCategories.ToList();
            
            // Chuyển đổi sang DTO
            var dtos = categories.Select(c => c.ToDto()).ToList();
            
            _logger.Debug($"Đã lấy {dtos.Count} danh mục");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả danh mục (Async).
    /// </summary>
    public async Task<List<BusinessPartnerCategoryDto>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var categories = await Task.Run(() => context.BusinessPartnerCategories.ToList());
            
            // Chuyển đổi sang DTO
            var dtos = categories.Select(c => c.ToDto()).ToList();
            
            _logger.Debug($"Đã lấy {dtos.Count} danh mục (async)");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tất cả danh mục: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Tìm kiếm danh mục theo tên (contains, case-insensitive).
    /// </summary>
    public List<BusinessPartnerCategoryDto> SearchByName(string keyword)
    {
        try
        {
            using var context = CreateNewContext();
            if (string.IsNullOrWhiteSpace(keyword))
            {
                var all = context.BusinessPartnerCategories.ToList();
                _logger.Debug($"Đã tìm kiếm danh mục (không có keyword): {all.Count} kết quả");
                return all.Select(c => c.ToDto()).ToList();
            }
            
            var lower = keyword.ToLower();
            var results = context.BusinessPartnerCategories
                .Where(x => x.CategoryName.ToLower().Contains(lower))
                .ToList();
            
            // Chuyển đổi sang DTO
            var dtos = results.Select(c => c.ToDto()).ToList();
            
            _logger.Debug($"Đã tìm kiếm danh mục theo keyword '{keyword}': {dtos.Count} kết quả");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tìm kiếm theo tên '{keyword}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tìm kiếm theo tên '{keyword}': {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật thông tin danh mục.
    /// </summary>
    public void UpdateCategory(Guid id, string categoryName, string description = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Tên danh mục không được rỗng", nameof(categoryName));

            using var context = CreateNewContext();
            var entity = context.BusinessPartnerCategories.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                throw new DataAccessException($"Không tìm thấy danh mục với Id: {id}");

            // Kiểm tra tên trùng lặp (trừ chính nó)
            if (IsCategoryNameExists(categoryName, id))
                throw new DataAccessException($"Tên danh mục '{categoryName}' đã tồn tại");

            entity.CategoryName = categoryName.Trim();
            entity.Description = description?.Trim();
            entity.ModifiedDate = DateTime.Now;
            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật danh mục: {id} - {entity.CategoryName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật danh mục {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật danh mục {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Cập nhật thông tin danh mục (Async).
    /// </summary>
    public async Task UpdateCategoryAsync(Guid id, string categoryName, string description = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Tên danh mục không được rỗng", nameof(categoryName));

            using var context = CreateNewContext();
            var entity = context.BusinessPartnerCategories.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                throw new DataAccessException($"Không tìm thấy danh mục với Id: {id}");

            // Kiểm tra tên trùng lặp (trừ chính nó)
            if (await IsCategoryNameExistsAsync(categoryName, id))
                throw new DataAccessException($"Tên danh mục '{categoryName}' đã tồn tại");

            entity.CategoryName = categoryName.Trim();
            entity.Description = description?.Trim();
            entity.ModifiedDate = DateTime.Now;
            await Task.Run(() => context.SubmitChanges());
            
            _logger.Info($"Đã cập nhật danh mục (async): {id} - {entity.CategoryName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật danh mục {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật danh mục {id}: {ex.Message}", ex);
        }
    }

    #endregion

    #region Delete

    /// <summary>
    /// Xóa danh mục theo Id. Nếu có partners, chuyển sang category "Chưa phân loại".
    /// </summary>
    public void DeleteCategory(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.BusinessPartnerCategories.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return;

            // Nếu có partners, chuyển sang category "Chưa phân loại"
            if (HasPartners(id))
            {
                MovePartnersToUncategorizedCategory(id, context);
            }

            context.BusinessPartnerCategories.DeleteOnSubmit(entity);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa danh mục: {id} - {entity.CategoryName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa danh mục {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa danh mục {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa danh mục theo Id (Async). Nếu có partners, chuyển sang category "Chưa phân loại".
    /// </summary>
    public async Task DeleteCategoryAsync(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.BusinessPartnerCategories.FirstOrDefault(x => x.Id == id);
            if (entity == null)
                return;

            // Nếu có partners, chuyển sang category "Chưa phân loại"
            if (await HasPartnersAsync(id))
            {
                await MovePartnersToUncategorizedCategoryAsync(id, context);
            }

            context.BusinessPartnerCategories.DeleteOnSubmit(entity);
            await Task.Run(() => context.SubmitChanges());
            
            _logger.Info($"Đã xóa danh mục (async): {id} - {entity.CategoryName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa danh mục {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa danh mục {id}: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra tồn tại theo Id.
    /// </summary>
    public bool Exists(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var result = context.BusinessPartnerCategories.Any(x => x.Id == id);
            
            _logger.Debug($"Exists check cho category ID {id}: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tồn tại danh mục Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra tồn tại danh mục Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra tồn tại tên danh mục.
    /// </summary>
    public bool IsCategoryNameExists(string categoryName, Guid excludeId = default(Guid))
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return false;
            using var context = CreateNewContext();
            var result = context.BusinessPartnerCategories.Any(x => 
                x.CategoryName.Trim().ToLower() == categoryName.Trim().ToLower() && x.Id != excludeId);
            
            _logger.Debug($"IsCategoryNameExists: Name='{categoryName}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra tồn tại tên danh mục (Async).
    /// </summary>
    public async Task<bool> IsCategoryNameExistsAsync(string categoryName, Guid excludeId = default(Guid))
    {
        try
        {
            if (string.IsNullOrWhiteSpace(categoryName)) return false;
            using var context = CreateNewContext();
            var result = await Task.Run(() => context.BusinessPartnerCategories.Any(x => 
                x.CategoryName.Trim().ToLower() == categoryName.Trim().ToLower() && x.Id != excludeId));
            
            _logger.Debug($"IsCategoryNameExistsAsync: Name='{categoryName}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra tên danh mục '{categoryName}': {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== BUSINESS LOGIC METHODS ==========

    /// <summary>
    /// Kiểm tra xem danh mục có đối tác nào không.
    /// </summary>
    public bool HasPartners(Guid categoryId)
    {
        try
        {
            using var context = CreateNewContext();
            var result = context.BusinessPartner_BusinessPartnerCategories.Any(m => m.CategoryId == categoryId);
            
            _logger.Debug($"HasPartners check cho category {categoryId}: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra danh mục có đối tác {categoryId}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra danh mục có đối tác {categoryId}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra xem danh mục có đối tác nào không (Async).
    /// </summary>
    public async Task<bool> HasPartnersAsync(Guid categoryId)
    {
        try
        {
            using var context = CreateNewContext();
            var result = await Task.Run(() => context.BusinessPartner_BusinessPartnerCategories.Any(m => m.CategoryId == categoryId));
            
            _logger.Debug($"HasPartnersAsync check cho category {categoryId}: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra danh mục có đối tác {categoryId}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra danh mục có đối tác {categoryId}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy số lượng đối tác của một danh mục cụ thể.
    /// </summary>
    public int GetPartnerCount(Guid categoryId)
    {
        try
        {
            using var context = CreateNewContext();
            var count = context.BusinessPartner_BusinessPartnerCategories.Count(m => m.CategoryId == categoryId);
            
            _logger.Debug($"GetPartnerCount cho category {categoryId}: {count}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số lượng đối tác của danh mục {categoryId}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm số lượng đối tác của danh mục {categoryId}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy số lượng đối tác của một danh mục cụ thể (Async).
    /// </summary>
    public async Task<int> GetPartnerCountAsync(Guid categoryId)
    {
        try
        {
            using var context = CreateNewContext();
            var count = await Task.Run(() => context.BusinessPartner_BusinessPartnerCategories.Count(m => m.CategoryId == categoryId));
            
            _logger.Debug($"GetPartnerCountAsync cho category {categoryId}: {count}");
            return count;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số lượng đối tác của danh mục {categoryId}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm số lượng đối tác của danh mục {categoryId}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đếm số lượng đối tác theo từng danh mục (bao gồm cả đối tác của sub-categories).
    /// </summary>
    public Dictionary<Guid, int> GetPartnerCountByCategory()
    {
        try
        {
            using var context = CreateNewContext();
            
            // Lấy tất cả categories để xây dựng cây phân cấp
            var allCategories = context.BusinessPartnerCategories.ToList();
            var categoryDict = allCategories.ToDictionary(c => c.Id);
            
            // Lấy tất cả mappings trực tiếp
            var directMappings = context.BusinessPartner_BusinessPartnerCategories.ToList();

            var result = new Dictionary<Guid, int>();
            
            // Với mỗi category, đếm tất cả đối tác (bao gồm cả sub-categories)
            foreach (var category in allCategories)
            {
                var allPartnerIds = new HashSet<Guid>();
                
                // Lấy đối tác trực tiếp
                var directPartners = directMappings.Where(m => m.CategoryId == category.Id).Select(m => m.PartnerId);
                foreach (var partnerId in directPartners)
                {
                    allPartnerIds.Add(partnerId);
                }
                
                // Lấy đối tác từ tất cả sub-categories
                var subCategories = GetSubCategories(category.Id, categoryDict);
                foreach (var subCategoryId in subCategories)
                {
                    var subPartners = directMappings.Where(m => m.CategoryId == subCategoryId).Select(m => m.PartnerId);
                    foreach (var partnerId in subPartners)
                    {
                        allPartnerIds.Add(partnerId);
                    }
                }
                
                result[category.Id] = allPartnerIds.Count;
            }

            _logger.Debug($"GetPartnerCountByCategory: Found {result.Count} categories with partners");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số lượng đối tác theo danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm số lượng đối tác theo danh mục: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tên các đối tác theo từng danh mục (bao gồm cả đối tác của sub-categories).
    /// </summary>
    public Dictionary<Guid, string> GetPartnerNamesByCategory()
    {
        try
        {
            using var context = CreateNewContext();
            
            // Lấy tất cả categories để xây dựng cây phân cấp
            var allCategories = context.BusinessPartnerCategories.ToList();
            var categoryDict = allCategories.ToDictionary(c => c.Id);
            
            // Lấy tất cả mappings trực tiếp
            var directMappings = context.BusinessPartner_BusinessPartnerCategories
                .Join(context.BusinessPartners, m => m.PartnerId, p => p.Id, (m, p) => new { m.CategoryId, p.PartnerName })
                .ToList();

            var result = new Dictionary<Guid, string>();
            
            // Với mỗi category, lấy tất cả đối tác (bao gồm cả sub-categories)
            foreach (var category in allCategories)
            {
                var allPartnerNames = new HashSet<string>();
                
                // Lấy đối tác trực tiếp
                var directPartners = directMappings.Where(m => m.CategoryId == category.Id).Select(m => m.PartnerName);
                foreach (var partnerName in directPartners)
                {
                    allPartnerNames.Add(partnerName);
                }
                
                // Lấy đối tác từ tất cả sub-categories
                var subCategories = GetSubCategories(category.Id, categoryDict);
                foreach (var subCategoryId in subCategories)
                {
                    var subPartners = directMappings.Where(m => m.CategoryId == subCategoryId).Select(m => m.PartnerName);
                    foreach (var partnerName in subPartners)
                    {
                        allPartnerNames.Add(partnerName);
                    }
                }
                
                result[category.Id] = string.Join(", ", allPartnerNames.OrderBy(x => x));
            }

            _logger.Debug($"GetPartnerNamesByCategory: Found {result.Count} categories");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy tên đối tác theo danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy tên đối tác theo danh mục: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy mã các đối tác theo từng danh mục (bao gồm cả đối tác của sub-categories).
    /// </summary>
    public Dictionary<Guid, string> GetPartnerCodesByCategory()
    {
        try
        {
            using var context = CreateNewContext();
            
            // Lấy tất cả categories để xây dựng cây phân cấp
            var allCategories = context.BusinessPartnerCategories.ToList();
            var categoryDict = allCategories.ToDictionary(c => c.Id);
            
            // Lấy tất cả mappings trực tiếp
            var directMappings = context.BusinessPartner_BusinessPartnerCategories
                .Join(context.BusinessPartners, m => m.PartnerId, p => p.Id, (m, p) => new { m.CategoryId, p.PartnerCode })
                .ToList();

            var result = new Dictionary<Guid, string>();
            
            // Với mỗi category, lấy tất cả đối tác (bao gồm cả sub-categories)
            foreach (var category in allCategories)
            {
                var allPartnerCodes = new HashSet<string>();
                
                // Lấy đối tác trực tiếp
                var directPartners = directMappings.Where(m => m.CategoryId == category.Id).Select(m => m.PartnerCode);
                foreach (var partnerCode in directPartners)
                {
                    allPartnerCodes.Add(partnerCode);
                }
                
                // Lấy đối tác từ tất cả sub-categories
                var subCategories = GetSubCategories(category.Id, categoryDict);
                foreach (var subCategoryId in subCategories)
                {
                    var subPartners = directMappings.Where(m => m.CategoryId == subCategoryId).Select(m => m.PartnerCode);
                    foreach (var partnerCode in subPartners)
                    {
                        allPartnerCodes.Add(partnerCode);
                    }
                }
                
                result[category.Id] = string.Join(", ", allPartnerCodes.OrderBy(x => x));
            }

            _logger.Debug($"GetPartnerCodesByCategory: Found {result.Count} categories");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy mã đối tác theo danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy mã đối tác theo danh mục: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Đếm số lượng đối tác theo từng danh mục (Async).
    /// </summary>
    public async Task<Dictionary<Guid, int>> GetPartnerCountByCategoryAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var result = await Task.Run(() => GetPartnerCountByCategory());
            
            _logger.Debug($"GetPartnerCountByCategoryAsync: Found {result.Count} categories");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đếm số lượng đối tác theo danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đếm số lượng đối tác theo danh mục: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== CREATE OPERATIONS (SaveOrUpdate) ==========

    /// <summary>
    /// Lưu/cập nhật đầy đủ thông tin danh mục.
    /// </summary>
    public void SaveOrUpdate(BusinessPartnerCategoryDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "DTO không được null");

            using var context = CreateNewContext();
            var existing = context.BusinessPartnerCategories.FirstOrDefault(x => x.Id == dto.Id);
            
            if (existing == null)
            {
                // Thêm mới
                if (IsCategoryNameExists(dto.CategoryName))
                    throw new DataAccessException($"Tên danh mục '{dto.CategoryName}' đã tồn tại");
                
                // Chuyển đổi DTO sang Entity
                var entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
                
                context.BusinessPartnerCategories.InsertOnSubmit(entity);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới danh mục (SaveOrUpdate): {entity.CategoryName}");
            }
            else
            {
                // Cập nhật
                if (IsCategoryNameExists(dto.CategoryName, dto.Id))
                    throw new DataAccessException($"Tên danh mục '{dto.CategoryName}' đã tồn tại");

                // Sử dụng converter để cập nhật entity từ DTO
                dto.ToEntity(existing);
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật danh mục (SaveOrUpdate): {existing.CategoryName}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu/cập nhật danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu/cập nhật danh mục: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lưu/cập nhật đầy đủ thông tin danh mục (Async).
    /// </summary>
    public async Task SaveOrUpdateAsync(BusinessPartnerCategoryDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "DTO không được null");

            using var context = CreateNewContext();
            var existing = context.BusinessPartnerCategories.FirstOrDefault(x => x.Id == dto.Id);
            
            if (existing == null)
            {
                // Thêm mới
                if (await IsCategoryNameExistsAsync(dto.CategoryName))
                    throw new DataAccessException($"Tên danh mục '{dto.CategoryName}' đã tồn tại");
                
                // Chuyển đổi DTO sang Entity
                var entity = dto.ToEntity();
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
                
                context.BusinessPartnerCategories.InsertOnSubmit(entity);
                await Task.Run(() => context.SubmitChanges());
                
                _logger.Info($"Đã thêm mới danh mục (SaveOrUpdateAsync): {entity.CategoryName}");
            }
            else
            {
                // Cập nhật
                if (await IsCategoryNameExistsAsync(dto.CategoryName, dto.Id))
                    throw new DataAccessException($"Tên danh mục '{dto.CategoryName}' đã tồn tại");
                
                // Sử dụng converter để cập nhật entity từ DTO
                dto.ToEntity(existing);
                await Task.Run(() => context.SubmitChanges());
                
                _logger.Info($"Đã cập nhật danh mục (SaveOrUpdateAsync): {existing.CategoryName}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu/cập nhật danh mục: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu/cập nhật danh mục: {ex.Message}", ex);
        }
    }

    #endregion
}
