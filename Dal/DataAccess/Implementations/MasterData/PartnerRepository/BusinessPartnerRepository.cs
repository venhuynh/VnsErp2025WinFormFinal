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
/// Data Access cho thực thể BusinessPartner (LINQ to SQL trên VnsErp2025DataContext).
/// Cung cấp CRUD đầy đủ, phương thức đặc thù và phiên bản đồng bộ/bất đồng bộ.
/// </summary>
public class BusinessPartnerRepository : IBusinessPartnerRepository
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
    /// Khởi tạo một instance mới của class BusinessPartnerRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public BusinessPartnerRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("BusinessPartnerRepository được khởi tạo với connection string");
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
        
        // Load BusinessPartner_BusinessPartnerCategories (EntitySet) để có thể truy cập categories
        loadOptions.LoadWith<BusinessPartner>(bp => bp.BusinessPartner_BusinessPartnerCategories);
        
        // Load BusinessPartnerCategory từ BusinessPartner_BusinessPartnerCategory để có thể truy cập category details
        loadOptions.LoadWith<BusinessPartner_BusinessPartnerCategory>(m => m.BusinessPartnerCategory);
        
        // KHÔNG load BusinessPartnerCategory1 (parent) trong LoadOptions vì sẽ tạo cycle
        // Parent category sẽ được materialize thủ công trong MaterializeNavigationProperties nếu cần
        
        // Load ApplicationUser navigation properties cho CreatedBy và ModifiedBy
        loadOptions.LoadWith<BusinessPartner>(bp => bp.ApplicationUser);
        loadOptions.LoadWith<BusinessPartner>(bp => bp.ApplicationUser2);
        
        // Load BusinessPartnerSites nếu cần
        loadOptions.LoadWith<BusinessPartner>(bp => bp.BusinessPartnerSites);
        
        context.LoadOptions = loadOptions;

        return context;
    }

    /// <summary>
    /// Tạo DataContext mới cho Lookup (không load navigation properties để tối ưu hiệu năng)
    /// </summary>
    /// <returns>DataContext mới không có LoadOptions</returns>
    private VnsErp2025DataContext CreateLookupContext()
    {
        // Tạo context mới nhưng KHÔNG set LoadOptions để tránh load navigation properties không cần thiết
        return new VnsErp2025DataContext(_connectionString);
    }

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Thêm đối tác mới với validation cơ bản (mã/code duy nhất, tên bắt buộc).
    /// </summary>
    /// <param name="code">Mã đối tác</param>
    /// <param name="name">Tên đối tác</param>
    /// <param name="partnerType">Loại đối tác</param>
    /// <param name="isActive">Trạng thái</param>
    /// <param name="createdBy">ID người tạo (optional)</param>
    /// <returns>BusinessPartnerDetailDto đã tạo</returns>
    public BusinessPartnerDetailDto AddNewPartner(string code, string name, int partnerType, bool isActive = true, Guid? createdBy = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException(@"Mã đối tác không được rỗng", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Tên đối tác không được rỗng", nameof(name));

            if (IsPartnerCodeExists(code))
                throw new DataAccessException($"Mã đối tác '{code}' đã tồn tại");

            using var context = CreateNewContext();
            var entity = new BusinessPartner
            {
                Id = Guid.NewGuid(),
                PartnerCode = code,
                PartnerName = name,
                PartnerType = partnerType,
                IsActive = isActive,
                CreatedDate = DateTime.Now,
                UpdatedDate = null,
                CreatedBy = createdBy,
                ModifiedBy = null,
                IsDeleted = false,
                DeletedBy = null,
                DeletedDate = null
            };

            context.BusinessPartners.InsertOnSubmit(entity);
            context.SubmitChanges();
            
            _logger.Info($"Đã thêm mới đối tác: {entity.PartnerCode} - {entity.PartnerName}");
            return entity.ToDetailDto();
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 2627)
        {
            _logger.Error($"Trùng mã đối tác '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Trùng mã đối tác '{code}'", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi thêm đối tác mới '{code}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi thêm đối tác mới '{code}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Thêm đối tác mới (Async).
    /// </summary>
    /// <param name="code">Mã đối tác</param>
    /// <param name="name">Tên đối tác</param>
    /// <param name="partnerType">Loại đối tác</param>
    /// <param name="isActive">Trạng thái</param>
    /// <param name="createdBy">ID người tạo (optional)</param>
    public async Task<BusinessPartnerDetailDto> AddNewPartnerAsync(string code, string name, int partnerType, bool isActive = true, Guid? createdBy = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException(@"Mã đối tác không được rỗng", nameof(code));
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException(@"Tên đối tác không được rỗng", nameof(name));

            if (await IsPartnerCodeExistsAsync(code))
                throw new DataAccessException($"Mã đối tác '{code}' đã tồn tại");

            using var context = CreateNewContext();
            var entity = new BusinessPartner
            {
                Id = Guid.NewGuid(),
                PartnerCode = code,
                PartnerName = name,
                PartnerType = partnerType,
                IsActive = isActive,
                CreatedDate = DateTime.Now,
                UpdatedDate = null,
                CreatedBy = createdBy,
                ModifiedBy = null,
                IsDeleted = false,
                DeletedBy = null,
                DeletedDate = null
            };

            context.BusinessPartners.InsertOnSubmit(entity);
            await Task.Run(() => context.SubmitChanges());
            
            _logger.Info($"Đã thêm mới đối tác (async): {entity.PartnerCode} - {entity.PartnerName}");
            return entity.ToDetailDto();
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 2627)
        {
            _logger.Error($"Trùng mã đối tác '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Trùng mã đối tác '{code}'", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi thêm đối tác '{code}': {sqlEx.Message}", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi thêm đối tác mới '{code}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi thêm đối tác mới '{code}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Thêm mới hoặc cập nhật đầy đủ thông tin đối tác.
    /// Nếu Id tồn tại -> cập nhật tất cả trường theo DTO truyền vào.
    /// Nếu không tồn tại -> thêm mới.
    /// </summary>
    /// <param name="dto">BusinessPartnerDetailDto</param>
    /// <param name="userId">ID người dùng thực hiện (optional, dùng cho audit fields)</param>
    public void SaveOrUpdate(BusinessPartnerDetailDto dto, Guid? userId = null)
    {
        if (dto == null) throw new ArgumentNullException(nameof(dto));
        try
        {
            using var context = CreateNewContext();
            var existing = context.BusinessPartners.FirstOrDefault(x => x.Id == dto.Id && !x.IsDeleted);
            if (existing == null || dto.Id == Guid.Empty)
            {
                // Thêm mới - Chuyển đổi DTO sang Entity
                var entity = dto.ToEntity();
                if (entity.Id == Guid.Empty) entity.Id = Guid.NewGuid();
                entity.CreatedDate = DateTime.Now;
                entity.CreatedBy = userId;
                entity.ModifiedBy = null;
                entity.IsDeleted = false;
                entity.DeletedBy = null;
                entity.DeletedDate = null;
                context.BusinessPartners.InsertOnSubmit(entity);
                
                // Tạo BusinessPartnerSite là trụ sở chính
                var mainSite = new Dal.DataContext.BusinessPartnerSite
                {
                    Id = Guid.NewGuid(),
                    PartnerId = entity.Id,
                    SiteCode = $"{entity.PartnerCode}-MAIN",
                    SiteName = $"Trụ sở chính - {entity.PartnerName}",
                    Address = entity.Address,
                    City = entity.City,
                    Province = entity.City,
                    Country = entity.Country,
                    Phone = entity.Phone,
                    Email = entity.Email,
                    IsDefault = true,
                    IsActive = entity.IsActive,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null
                };
                
                context.BusinessPartnerSites.InsertOnSubmit(mainSite);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới đối tác và trụ sở chính (SaveOrUpdate): {entity.PartnerCode} - {entity.PartnerName}");
            }
            else
            {
                // Cập nhật - Sử dụng converter để cập nhật entity từ DTO
                dto.ToEntity(existing);
                existing.UpdatedDate = DateTime.Now;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật đối tác (SaveOrUpdate): {existing.PartnerCode} - {existing.PartnerName}");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu/cập nhật đối tác: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu/cập nhật đối tác: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy đối tác theo Id (chỉ lấy các đối tác chưa bị xóa).
    /// </summary>
    public BusinessPartnerDetailDto GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID: {id} - {partner.PartnerName}");
                return partner.ToDetailDto();
            }
            
            return null;
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 1205)
        {
            // Deadlock retry
            System.Threading.Thread.Sleep(100);
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID (retry): {id} - {partner.PartnerName}");
                return partner.ToDetailDto();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy đối tác theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy đối tác theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy đối tác theo Id (Async) - chỉ lấy các đối tác chưa bị xóa.
    /// </summary>
    public async Task<BusinessPartnerDetailDto> GetByIdAsync(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted));
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID (async): {id} - {partner.PartnerName}");
                return partner.ToDetailDto();
            }
            
            return null;
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 1205)
        {
            // Deadlock retry
            await Task.Delay(100);
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted));
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID (async retry): {id} - {partner.PartnerName}");
                return partner.ToDetailDto();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy đối tác theo Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy đối tác theo Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy đối tác theo mã (chỉ lấy các đối tác chưa bị xóa).
    /// </summary>
    public BusinessPartnerDetailDto GetByCode(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.PartnerCode == code && !x.IsDeleted);
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo mã: {code} - {partner.PartnerName}");
                return partner.ToDetailDto();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy đối tác theo mã '{code}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy đối tác theo mã '{code}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy đối tác theo mã (Async) - chỉ lấy các đối tác chưa bị xóa.
    /// </summary>
    public async Task<BusinessPartnerDetailDto> GetByCodeAsync(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.PartnerCode == code && !x.IsDeleted));
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo mã (async): {code} - {partner.PartnerName}");
                return partner.ToDetailDto();
            }
            
            return null;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy đối tác theo mã '{code}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy đối tác theo mã '{code}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Tìm kiếm đối tác theo tên (contains, case-insensitive) - chỉ lấy các đối tác chưa bị xóa.
    /// </summary>
    public List<BusinessPartnerListDto> SearchByName(string keyword)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Lấy tất cả categories để tính FullPath
            var allCategories = context.BusinessPartnerCategories.ToList();
            var categoryDict = allCategories.ToDictionary(c => c.Id);
            
            List<BusinessPartner> results;
            
            if (string.IsNullOrWhiteSpace(keyword))
            {
                results = context.BusinessPartners.Where(x => !x.IsDeleted).ToList();
                _logger.Debug($"Đã tìm kiếm đối tác (không có keyword): {results.Count} kết quả");
            }
            else
            {
                var lower = keyword.ToLower();
                results = context.BusinessPartners
                    .Where(x => !x.IsDeleted && x.PartnerName.ToLower().Contains(lower))
                    .ToList();
                
                _logger.Debug($"Đã tìm kiếm đối tác theo keyword '{keyword}': {results.Count} kết quả");
            }
            
            // Chuyển đổi sang DTO - Lấy thông tin từ navigation properties trước khi dispose
            var dtos = results.Select(p => p.ToListDto(categoryDict)).ToList();
            
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tìm kiếm theo tên '{keyword}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tìm kiếm theo tên '{keyword}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động (chỉ lấy các đối tác chưa bị xóa).
    /// </summary>
    public List<BusinessPartnerListDto> GetActivePartners()
    {
        try
        {
            using var context = CreateNewContext();
            
            // Lấy tất cả categories để tính FullPath
            var allCategories = context.BusinessPartnerCategories.ToList();
            var categoryDict = allCategories.ToDictionary(c => c.Id);
            
            var partners = context.BusinessPartners.Where(x => x.IsActive == true && !x.IsDeleted).ToList();
            
            // Chuyển đổi sang DTO - Lấy thông tin từ navigation properties trước khi dispose
            var dtos = partners.Select(p => p.ToListDto(categoryDict)).ToList();
            
            _logger.Debug($"Đã lấy {dtos.Count} đối tác đang hoạt động");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách đối tác active: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách đối tác active: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động (Async) - chỉ lấy các đối tác chưa bị xóa.
    /// </summary>
    public async Task<List<BusinessPartnerListDto>> GetActivePartnersAsync()
    {
        try
        {
            _logger.Debug("[GetActivePartnersAsync] Bắt đầu lấy danh sách đối tác active");
            
            // Sử dụng Task.Run để chạy trên thread pool
            return await Task.Run(() =>
            {
                _logger.Debug("[GetActivePartnersAsync] Tạo DataContext mới");
                using var context = CreateNewContext();
                
                // Lấy tất cả categories để tính FullPath
                var allCategories = context.BusinessPartnerCategories.ToList();
                var categoryDict = allCategories.ToDictionary(c => c.Id);
                
                _logger.Debug("[GetActivePartnersAsync] Đang query database");
                var partners = context.BusinessPartners.Where(x => x.IsActive == true && !x.IsDeleted).ToList();
                _logger.Debug($"[GetActivePartnersAsync] Đã query được {partners.Count} đối tác");
                
                // Chuyển đổi sang DTO - Lấy thông tin từ navigation properties trước khi dispose
                var dtos = partners.Select(p => p.ToListDto(categoryDict)).ToList();
                
                _logger.Debug($"[GetActivePartnersAsync] Đã lấy {dtos.Count} đối tác đang hoạt động (async)");
                return dtos;
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"[GetActivePartnersAsync] LỖI TỔNG QUÁT khi lấy danh sách đối tác active (async): {ex.Message}", ex);
            _logger.Error($"[GetActivePartnersAsync] StackTrace: {ex.StackTrace}");
            throw new DataAccessException($"Lỗi khi lấy danh sách đối tác active (async): {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động cho Lookup (chỉ load các trường cần thiết).
    /// Không load navigation properties để tối ưu hiệu năng.
    /// LINQ to SQL sẽ tự động chỉ select các trường được sử dụng (Id, PartnerCode, PartnerType, PartnerName, LogoThumbnailData).
    /// </summary>
    public List<BusinessPartnerLookupDto> GetActivePartnersForLookup()
    {
        try
        {
            _logger.Debug("[GetActivePartnersForLookup] Bắt đầu lấy danh sách đối tác cho lookup");
            
            using var context = CreateLookupContext();
            
            // Query BusinessPartner entities nhưng không load navigation properties
            // LINQ to SQL sẽ chỉ select các trường được truy cập sau khi ToList()
            // Tuy nhiên, để đảm bảo chỉ load các trường cần thiết, ta query trực tiếp
            var partners = context.BusinessPartners
                .Where(x => x.IsActive == true && !x.IsDeleted)
                .ToList();
            
            // Chuyển đổi sang DTO
            var dtos = partners.Select(p => p.ToLookupDto()).ToList();
            
            _logger.Debug($"[GetActivePartnersForLookup] Đã lấy {dtos.Count} đối tác cho lookup");
            return dtos;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách đối tác cho lookup: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách đối tác cho lookup: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động cho Lookup (Async) - chỉ load các trường cần thiết.
    /// </summary>
    public async Task<List<BusinessPartnerLookupDto>> GetActivePartnersForLookupAsync()
    {
        try
        {
            _logger.Debug("[GetActivePartnersForLookupAsync] Bắt đầu lấy danh sách đối tác cho lookup (async)");
            
            return await Task.Run(() =>
            {
                using var context = CreateLookupContext();
                
                // Query BusinessPartner entities nhưng không load navigation properties
                var partners = context.BusinessPartners
                    .Where(x => x.IsActive == true && !x.IsDeleted)
                    .ToList();
                
                // Chuyển đổi sang DTO
                var dtos = partners.Select(p => p.ToLookupDto()).ToList();
                
                _logger.Debug($"[GetActivePartnersForLookupAsync] Đã lấy {dtos.Count} đối tác cho lookup (async)");
                return dtos;
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách đối tác cho lookup (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách đối tác cho lookup (async): {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật thông tin liên hệ (điện thoại, email) cho đối tác.
    /// </summary>
    /// <param name="id">ID đối tác</param>
    /// <param name="phone">Số điện thoại</param>
    /// <param name="email">Email</param>
    /// <param name="modifiedBy">ID người cập nhật (optional)</param>
    public void UpdateContactInfo(Guid id, string phone, string email, Guid? modifiedBy = null)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                throw new DataAccessException($"Không tìm thấy đối tác với Id: {id}");

            entity.Phone = phone;
            entity.Email = email;
            entity.UpdatedDate = DateTime.Now;
            entity.ModifiedBy = modifiedBy;
            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật thông tin liên hệ cho đối tác: {id} - {entity.PartnerName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật thông tin liên hệ đối tác {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật thông tin liên hệ đối tác {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kích hoạt/Vô hiệu hóa đối tác.
    /// </summary>
    /// <param name="id">ID đối tác</param>
    /// <param name="isActive">Trạng thái hoạt động</param>
    /// <param name="modifiedBy">ID người cập nhật (optional)</param>
    public void SetActive(Guid id, bool isActive, Guid? modifiedBy = null)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                throw new DataAccessException($"Không tìm thấy đối tác với Id: {id}");

            entity.IsActive = isActive;
            entity.UpdatedDate = DateTime.Now;
            entity.ModifiedBy = modifiedBy;
            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật trạng thái đối tác: {id} - {entity.PartnerName} (IsActive={isActive})");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật trạng thái đối tác {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật trạng thái đối tác {id}: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa đối tác theo Id (Soft Delete - đánh dấu IsDeleted = true).
    /// </summary>
    /// <param name="id">ID đối tác</param>
    /// <param name="deletedBy">ID người xóa (optional)</param>
    public void DeletePartner(Guid id, Guid? deletedBy = null)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return;
                
            entity.IsDeleted = true;
            entity.DeletedBy = deletedBy;
            entity.DeletedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa đối tác (soft delete): {id} - {entity.PartnerName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa đối tác {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa đối tác {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa đối tác theo Id (Async) - Soft Delete.
    /// </summary>
    /// <param name="id">ID đối tác</param>
    /// <param name="deletedBy">ID người xóa (optional)</param>
    public async Task DeletePartnerAsync(Guid id, Guid? deletedBy = null)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            if (entity == null)
                return;
                
            entity.IsDeleted = true;
            entity.DeletedBy = deletedBy;
            entity.DeletedDate = DateTime.Now;
            entity.UpdatedDate = DateTime.Now;
            await Task.Run(() => context.SubmitChanges());
            
            _logger.Info($"Đã xóa đối tác (async, soft delete): {id} - {entity.PartnerName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa đối tác {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa đối tác {id}: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra tồn tại theo Id (chỉ kiểm tra các đối tác chưa bị xóa).
    /// </summary>
    public bool Exists(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var result = context.BusinessPartners.Any(x => x.Id == id && !x.IsDeleted);
            
            _logger.Debug($"Exists check cho partner ID {id}: {result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra tồn tại đối tác Id {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra tồn tại đối tác Id {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra tồn tại mã đối tác (chỉ kiểm tra các đối tác chưa bị xóa).
    /// </summary>
    public bool IsPartnerCodeExists(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            
            using var context = CreateNewContext();
            var result = context.BusinessPartners.Any(x => x.PartnerCode == code && !x.IsDeleted);
            
            _logger.Debug($"IsPartnerCodeExists: Code='{code}', Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra mã đối tác '{code}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra mã đối tác '{code}': {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra tồn tại mã đối tác (Async) - chỉ kiểm tra các đối tác chưa bị xóa.
    /// </summary>
    public async Task<bool> IsPartnerCodeExistsAsync(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return false;
            
            using var context = CreateNewContext();
            var result = await Task.Run(() => context.BusinessPartners.Any(x => x.PartnerCode == code && !x.IsDeleted));
            
            _logger.Debug($"IsPartnerCodeExistsAsync: Code='{code}', Result={result}");
            return result;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi kiểm tra mã '{code}': {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi kiểm tra mã '{code}': {sqlEx.Message}", sqlEx) 
            { 
                SqlErrorNumber = sqlEx.Number, 
                ThoiGianLoi = DateTime.Now 
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra mã đối tác '{code}': {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra mã đối tác '{code}': {ex.Message}", ex);
        }
    }

    #endregion
}
