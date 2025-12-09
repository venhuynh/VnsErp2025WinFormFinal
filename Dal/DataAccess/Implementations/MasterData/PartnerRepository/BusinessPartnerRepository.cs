using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using Dal.Exceptions;
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
        /// Materialize navigation properties để tránh lỗi "Cannot access a disposed object"
        /// Force load EntitySet và navigation properties trước khi dispose DataContext
        /// </summary>
        /// <param name="partner">BusinessPartner entity</param>
        private void MaterializeNavigationProperties(BusinessPartner partner)
        {
            if (partner == null) return;

            _logger.Debug($"[Materialize] Bắt đầu materialize navigation properties cho partner {partner.Id} ({partner.PartnerCode})");

            try
            {
                // Force load ApplicationUser (CreatedBy) - materialize toàn bộ object
                try
                {
                    _logger.Debug($"[Materialize] Đang materialize ApplicationUser cho partner {partner.Id}");
                    var createdByUser = partner.ApplicationUser;
                    if (createdByUser != null)
                    {
                        // Materialize tất cả properties cần thiết
                        var userName = createdByUser.UserName;
                        var userId = createdByUser.Id;
                        _logger.Debug($"[Materialize] Đã materialize ApplicationUser: {userName} (Id: {userId})");
                    }
                    else
                    {
                        _logger.Debug($"[Materialize] ApplicationUser là null cho partner {partner.Id}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"[Materialize] LỖI khi materialize ApplicationUser cho partner {partner.Id}: {ex.Message}", ex);
                    _logger.Error($"[Materialize] StackTrace: {ex.StackTrace}");
                }

                // Force load ApplicationUser2 (ModifiedBy) - materialize toàn bộ object
                try
                {
                    _logger.Debug($"[Materialize] Đang materialize ApplicationUser2 cho partner {partner.Id}");
                    var modifiedByUser = partner.ApplicationUser2;
                    if (modifiedByUser != null)
                    {
                        // Materialize tất cả properties cần thiết
                        var userName = modifiedByUser.UserName;
                        var userId = modifiedByUser.Id;
                        _logger.Debug($"[Materialize] Đã materialize ApplicationUser2: {userName} (Id: {userId})");
                    }
                    else
                    {
                        _logger.Debug($"[Materialize] ApplicationUser2 là null cho partner {partner.Id}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"[Materialize] LỖI khi materialize ApplicationUser2 cho partner {partner.Id}: {ex.Message}", ex);
                    _logger.Error($"[Materialize] StackTrace: {ex.StackTrace}");
                }

                // Force load BusinessPartner_BusinessPartnerCategories (EntitySet)
                try
                {
                    _logger.Debug($"[Materialize] Đang materialize BusinessPartner_BusinessPartnerCategories cho partner {partner.Id}");
                    var categories = partner.BusinessPartner_BusinessPartnerCategories;
                    if (categories != null)
                    {
                        // Force load toàn bộ collection
                        var categoriesList = categories.ToList();
                        _logger.Debug($"[Materialize] Đã load {categoriesList.Count} categories cho partner {partner.Id}");
                        
                        // Force load BusinessPartnerCategory cho mỗi mapping
                        int index = 0;
                        foreach (var mapping in categoriesList)
                        {
                            try
                            {
                                _logger.Debug($"[Materialize] Đang materialize category mapping {index} cho partner {partner.Id}");
                                var category = mapping.BusinessPartnerCategory;
                                if (category != null)
                                {
                                    // Materialize tất cả category properties cần thiết
                                    var categoryName = category.CategoryName;
                                    var categoryCode = category.CategoryCode;
                                    var parentId = category.ParentId;
                                    var categoryId = category.Id;
                                    _logger.Debug($"[Materialize] Đã materialize category: {categoryName} (Id: {categoryId}, ParentId: {parentId})");
                                }
                                else
                                {
                                    _logger.Warning($"[Materialize] BusinessPartnerCategory là null cho mapping {index}");
                                }
                                index++;
                            }
                            catch (Exception ex)
                            {
                                _logger.Error($"[Materialize] LỖI khi materialize BusinessPartnerCategory cho mapping {index}: {ex.Message}", ex);
                                _logger.Error($"[Materialize] StackTrace: {ex.StackTrace}");
                            }
                        }
                    }
                    else
                    {
                        _logger.Debug($"[Materialize] BusinessPartner_BusinessPartnerCategories là null cho partner {partner.Id}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"[Materialize] LỖI khi materialize BusinessPartner_BusinessPartnerCategories cho partner {partner.Id}: {ex.Message}", ex);
                    _logger.Error($"[Materialize] StackTrace: {ex.StackTrace}");
                }

                // Force load BusinessPartnerSites nếu cần
                try
                {
                    _logger.Debug($"[Materialize] Đang materialize BusinessPartnerSites cho partner {partner.Id}");
                    var sites = partner.BusinessPartnerSites;
                    if (sites != null)
                    {
                        var sitesList = sites.ToList();
                        _logger.Debug($"[Materialize] Đã load {sitesList.Count} sites cho partner {partner.Id}");
                        foreach (var site in sitesList)
                        {
                            var siteName = site.SiteName; // Materialize
                        }
                    }
                    else
                    {
                        _logger.Debug($"[Materialize] BusinessPartnerSites là null cho partner {partner.Id}");
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"[Materialize] LỖI khi materialize BusinessPartnerSites cho partner {partner.Id}: {ex.Message}", ex);
                    _logger.Error($"[Materialize] StackTrace: {ex.StackTrace}");
                }

                _logger.Debug($"[Materialize] Hoàn thành materialize navigation properties cho partner {partner.Id}");
            }
            catch (Exception ex)
            {
                _logger.Error($"[Materialize] LỖI TỔNG QUÁT khi materialize navigation properties cho partner {partner.Id}: {ex.Message}", ex);
                _logger.Error($"[Materialize] StackTrace: {ex.StackTrace}");
                // Không throw exception, chỉ log warning vì có thể một số properties chưa được load
            }
        }

    #endregion

    #region Create

    /// <summary>
    /// Thêm đối tác mới với validation cơ bản (mã/code duy nhất, tên bắt buộc).
    /// </summary>
    /// <param name="code">Mã đối tác</param>
    /// <param name="name">Tên đối tác</param>
    /// <param name="partnerType">Loại đối tác</param>
    /// <param name="isActive">Trạng thái</param>
    /// <param name="createdBy">ID người tạo (optional)</param>
    /// <returns>Đối tác đã tạo</returns>
    public BusinessPartner AddNewPartner(string code, string name, int partnerType, bool isActive = true, Guid? createdBy = null)
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
            return entity;
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
    public async Task<BusinessPartner> AddNewPartnerAsync(string code, string name, int partnerType, bool isActive = true, Guid? createdBy = null)
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
            return entity;
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

    #endregion

    #region Read

    /// <summary>
    /// Lấy đối tác theo Id (chỉ lấy các đối tác chưa bị xóa).
    /// </summary>
    public BusinessPartner GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted);
            
            if (partner != null)
            {
                // Materialize navigation properties trước khi dispose context
                MaterializeNavigationProperties(partner);
                
                _logger.Debug($"Đã lấy đối tác theo ID: {id} - {partner.PartnerName}");
            }
            
            return partner;
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 1205)
        {
            // Deadlock retry
            System.Threading.Thread.Sleep(100);
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.Id == id);
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID (retry): {id} - {partner.PartnerName}");
            }
            
            return partner;
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
    public async Task<BusinessPartner> GetByIdAsync(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.Id == id && !x.IsDeleted));
            
            if (partner != null)
            {
                // Materialize navigation properties trước khi dispose context
                await Task.Run(() => MaterializeNavigationProperties(partner));
                
                _logger.Debug($"Đã lấy đối tác theo ID (async): {id} - {partner.PartnerName}");
            }
            
            return partner;
        }
        catch (SqlException sqlEx) when (sqlEx.Number == 1205)
        {
            // Deadlock retry
            await Task.Delay(100);
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.Id == id));
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo ID (async retry): {id} - {partner.PartnerName}");
            }
            
            return partner;
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
    public BusinessPartner GetByCode(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            
            using var context = CreateNewContext();
            var partner = context.BusinessPartners.FirstOrDefault(x => x.PartnerCode == code && !x.IsDeleted);
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo mã: {code} - {partner.PartnerName}");
            }
            
            return partner;
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
    public async Task<BusinessPartner> GetByCodeAsync(string code)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(code)) return null;
            
            using var context = CreateNewContext();
            var partner = await Task.Run(() => context.BusinessPartners.FirstOrDefault(x => x.PartnerCode == code && !x.IsDeleted));
            
            if (partner != null)
            {
                _logger.Debug($"Đã lấy đối tác theo mã (async): {code} - {partner.PartnerName}");
            }
            
            return partner;
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
    public List<BusinessPartner> SearchByName(string keyword)
    {
        try
        {
            using var context = CreateNewContext();
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
            
            // Materialize navigation properties cho tất cả partners trước khi dispose context
            foreach (var partner in results)
            {
                MaterializeNavigationProperties(partner);
            }
            
            return results;
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
    public List<BusinessPartner> GetActivePartners()
    {
        try
        {
            using var context = CreateNewContext();
            var partners = context.BusinessPartners.Where(x => x.IsActive == true && !x.IsDeleted).ToList();
            
            // Materialize navigation properties cho tất cả partners trước khi dispose context
            foreach (var partner in partners)
            {
                MaterializeNavigationProperties(partner);
            }
            
            _logger.Debug($"Đã lấy {partners.Count} đối tác đang hoạt động");
            return partners;
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
    public async Task<List<BusinessPartner>> GetActivePartnersAsync()
    {
        try
        {
            _logger.Debug("[GetActivePartnersAsync] Bắt đầu lấy danh sách đối tác active");
            
            // Sử dụng Task.Run để chạy trên thread pool, nhưng đảm bảo materialize đầy đủ
            return await Task.Run(() =>
            {
                _logger.Debug("[GetActivePartnersAsync] Tạo DataContext mới");
                using var context = CreateNewContext();
                
                _logger.Debug("[GetActivePartnersAsync] Đang query database");
                var partners = context.BusinessPartners.Where(x => x.IsActive == true && !x.IsDeleted).ToList();
                _logger.Debug($"[GetActivePartnersAsync] Đã query được {partners.Count} đối tác");
                
                // Materialize navigation properties cho tất cả partners TRƯỚC KHI dispose context
                int index = 0;
                foreach (var partner in partners)
                {
                    try
                    {
                        _logger.Debug($"[GetActivePartnersAsync] Đang materialize partner {index + 1}/{partners.Count}: {partner.PartnerCode}");
                        MaterializeNavigationProperties(partner);
                        _logger.Debug($"[GetActivePartnersAsync] Đã materialize xong partner {index + 1}: {partner.PartnerCode}");
                        index++;
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"[GetActivePartnersAsync] LỖI khi materialize partner {index + 1} ({partner.PartnerCode}): {ex.Message}", ex);
                        _logger.Error($"[GetActivePartnersAsync] StackTrace: {ex.StackTrace}");
                    }
                }
                
                _logger.Debug($"[GetActivePartnersAsync] Hoàn thành materialize, chuẩn bị dispose context");
                // Context sẽ được dispose ở đây khi ra khỏi using block
                _logger.Debug($"[GetActivePartnersAsync] Đã lấy {partners.Count} đối tác đang hoạt động (async)");
                return partners;
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
    /// Tạo DataContext mới cho Lookup (không load navigation properties để tối ưu hiệu năng)
    /// </summary>
    /// <returns>DataContext mới không có LoadOptions</returns>
    private VnsErp2025DataContext CreateLookupContext()
    {
        // Tạo context mới nhưng KHÔNG set LoadOptions để tránh load navigation properties không cần thiết
        return new VnsErp2025DataContext(_connectionString);
    }

    /// <summary>
    /// Lấy danh sách đối tác đang hoạt động cho Lookup (chỉ load các trường cần thiết).
    /// Không load navigation properties để tối ưu hiệu năng.
    /// LINQ to SQL sẽ tự động chỉ select các trường được sử dụng (Id, PartnerCode, PartnerType, PartnerName, LogoThumbnailData).
    /// </summary>
    public List<BusinessPartner> GetActivePartnersForLookup()
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
            
            // Materialize chỉ các trường cần thiết để đảm bảo chúng được load từ DB
            // Các trường khác sẽ là null/default nhưng không ảnh hưởng vì ta chỉ dùng các trường này
            foreach (var partner in partners)
            {
                // Force materialize các trường cần thiết
                var _ = partner.Id;
                var __ = partner.PartnerCode;
                var ___ = partner.PartnerType;
                var ____ = partner.PartnerName;
                var _____ = partner.LogoThumbnailData;
            }
            
            _logger.Debug($"[GetActivePartnersForLookup] Đã lấy {partners.Count} đối tác cho lookup");
            return partners;
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
    public async Task<List<BusinessPartner>> GetActivePartnersForLookupAsync()
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
                
                // Materialize chỉ các trường cần thiết
                foreach (var partner in partners)
                {
                    var _ = partner.Id;
                    var __ = partner.PartnerCode;
                    var ___ = partner.PartnerType;
                    var ____ = partner.PartnerName;
                    var _____ = partner.LogoThumbnailData;
                }
                
                _logger.Debug($"[GetActivePartnersForLookupAsync] Đã lấy {partners.Count} đối tác cho lookup (async)");
                return partners;
            });
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách đối tác cho lookup (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách đối tác cho lookup (async): {ex.Message}", ex);
        }
    }

    #endregion

    #region Update

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

    #region Delete

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

    #region Exists Checks

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

    #region Save/Update Full Entity

    /// <summary>
    /// Thêm mới hoặc cập nhật đầy đủ thông tin đối tác.
    /// Nếu Id tồn tại -> cập nhật tất cả trường theo entity truyền vào.
    /// Nếu không tồn tại -> thêm mới.
    /// </summary>
    /// <param name="source">Entity đối tác</param>
    /// <param name="userId">ID người dùng thực hiện (optional, dùng cho audit fields)</param>
    public void SaveOrUpdate(BusinessPartner source, Guid? userId = null)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));
        try
        {
            using var context = CreateNewContext();
            var existing = context.BusinessPartners.FirstOrDefault(x => x.Id == source.Id && !x.IsDeleted);
            if (existing == null || source.Id == Guid.Empty)
            {
                // ensure new Id
                if (source.Id == Guid.Empty) source.Id = Guid.NewGuid();
                source.CreatedDate = DateTime.Now;
                source.CreatedBy = userId;
                source.ModifiedBy = null;
                source.IsDeleted = false;
                source.DeletedBy = null;
                source.DeletedDate = null;
                context.BusinessPartners.InsertOnSubmit(source);
                
                // Tạo BusinessPartnerSite là trụ sở chính
                var mainSite = new BusinessPartnerSite
                {
                    Id = Guid.NewGuid(),
                    PartnerId = source.Id,
                    SiteCode = $"{source.PartnerCode}-MAIN",
                    SiteName = $"Trụ sở chính - {source.PartnerName}",
                    Address = source.Address,
                    City = source.City,
                    Province = source.City, // Sử dụng City làm Province cho trụ sở chính
                    Country = source.Country,
                    Phone = source.Phone,
                    Email = source.Email,
                    IsDefault = true, // Đánh dấu là trụ sở chính
                    IsActive = source.IsActive,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null
                };
                
                context.BusinessPartnerSites.InsertOnSubmit(mainSite);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới đối tác và trụ sở chính (SaveOrUpdate): {source.PartnerCode} - {source.PartnerName}");
            }
            else
            {
                // copy fields
                existing.PartnerCode = source.PartnerCode;
                existing.PartnerName = source.PartnerName;
                existing.PartnerType = source.PartnerType;
                existing.TaxCode = source.TaxCode;
                existing.Phone = source.Phone;
                existing.Email = source.Email;
                existing.Website = source.Website;
                existing.Address = source.Address;
                existing.City = source.City;
                existing.Country = source.Country;
                existing.IsActive = source.IsActive;
                
                // Copy Logo fields (metadata only, Logo binary field not in DataContext)
                existing.LogoFileName = source.LogoFileName;
                existing.LogoRelativePath = source.LogoRelativePath;
                existing.LogoFullPath = source.LogoFullPath;
                existing.LogoStorageType = source.LogoStorageType;
                existing.LogoFileSize = source.LogoFileSize;
                existing.LogoChecksum = source.LogoChecksum;
                
                // Copy LogoThumbnailData (binary data stored in database)
                existing.LogoThumbnailData = source.LogoThumbnailData;
                
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
}
