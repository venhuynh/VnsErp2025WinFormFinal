using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.Exceptions;
using DTO.MasterData.CustomerPartner;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.PartnerRepository;

/// <summary>
/// Data Access Layer cho BusinessPartnerSite
/// </summary>
public class BusinessPartnerSiteRepository : IBusinessPartnerSiteRepository
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
    /// Khởi tạo một instance mới của class BusinessPartnerSiteRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public BusinessPartnerSiteRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("BusinessPartnerSiteRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<BusinessPartnerSite>(s => s.BusinessPartner);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật BusinessPartnerSite
    /// </summary>
    /// <param name="dto">BusinessPartnerSiteDto</param>
    /// <returns>ID của entity đã lưu</returns>
    public Guid SaveOrUpdate(BusinessPartnerSiteDto dto)
    {
        try
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            _logger.Debug($"[SaveOrUpdate] Bắt đầu lưu BusinessPartnerSite - Id: {dto.Id}, SiteCode: {dto.SiteCode}, SiteName: {dto.SiteName}");
            _logger.Debug($"[SaveOrUpdate] DTO.Id == Guid.Empty: {dto.Id == Guid.Empty}, DTO.Id: '{dto.Id}'");

            using var context = CreateNewContext();
            
            if (dto.Id == Guid.Empty)
            {
                // Thêm mới
                _logger.Debug("[SaveOrUpdate] Chế độ: CREATE (dto.Id == Guid.Empty)");
                var entity = dto.ToEntity();
                entity.Id = Guid.NewGuid();
                entity.CreatedDate = DateTime.Now;
                _logger.Debug($"[SaveOrUpdate] Đã tạo Id mới: {entity.Id}, SiteCode: {entity.SiteCode}");
                context.BusinessPartnerSites.InsertOnSubmit(entity);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới BusinessPartnerSite: {entity.SiteCode} - {entity.SiteName} (Id: {entity.Id})");
                return entity.Id;
            }
            else
            {
                // Cập nhật
                _logger.Debug($"[SaveOrUpdate] Chế độ: UPDATE (dto.Id != Guid.Empty), Id: {dto.Id}");
                var existingEntity = context.BusinessPartnerSites.FirstOrDefault(s => s.Id == dto.Id);
                if (existingEntity != null)
                {
                    _logger.Debug($"[SaveOrUpdate] Tìm thấy existing entity: {existingEntity.SiteCode} - {existingEntity.SiteName}");
                    // Cập nhật entity từ DTO
                    dto.ToEntity(existingEntity);
                    existingEntity.UpdatedDate = DateTime.Now;
                    
                    context.SubmitChanges();
                    _logger.Info($"Đã cập nhật BusinessPartnerSite: {existingEntity.SiteCode} - {existingEntity.SiteName} (Id: {existingEntity.Id})");
                    return existingEntity.Id;
                }
                else
                {
                    _logger.Error($"[SaveOrUpdate] LỖI: Không tìm thấy BusinessPartnerSite với Id: {dto.Id}, SiteCode: {dto.SiteCode}, SiteName: {dto.SiteName}");
                    _logger.Error($"[SaveOrUpdate] Đang cố UPDATE nhưng entity không tồn tại trong database. Có thể đây là lỗi logic - dto nên có Id = Guid.Empty để CREATE.");
                    throw new DataAccessException($"Không tìm thấy BusinessPartnerSite để cập nhật (Id: {dto.Id}, SiteCode: {dto.SiteCode})");
                }
            }
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lưu BusinessPartnerSite: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lưu BusinessPartnerSite: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu BusinessPartnerSite: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lưu BusinessPartnerSite: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả BusinessPartnerSite với thông tin đầy đủ bao gồm PartnerName
    /// </summary>
    /// <returns>Danh sách BusinessPartnerSiteDto</returns>
    public List<BusinessPartnerSiteDto> GetAll()
    {
        try
        {
            using var context = CreateNewContext();

            // Load tất cả BusinessPartnerSite với BusinessPartner đã được include
            var sites = context.BusinessPartnerSites
                .OrderBy(s => s.SiteName)
                .ToList();

            // Chuyển đổi sang DTO, lấy thông tin partner từ eager-loaded navigation properties
            var dtos = sites.Select(site =>
            {
                string partnerName = null;
                string partnerCode = null;
                int? partnerType = null;
                string partnerTaxCode = null;
                string partnerPhone = null;
                string partnerEmail = null;
                string partnerWebsite = null;

                try
                {
                    var businessPartner = site.BusinessPartner;
                    if (businessPartner != null)
                    {
                        partnerName = businessPartner.PartnerName;
                        partnerCode = businessPartner.PartnerCode;
                        partnerType = businessPartner.PartnerType;
                        partnerTaxCode = businessPartner.TaxCode;
                        partnerPhone = businessPartner.Phone;
                        partnerEmail = businessPartner.Email;
                        partnerWebsite = businessPartner.Website;
                    }
                }
                catch
                {
                    // Navigation property chưa được load hoặc đã bị dispose
                }

                return site.ToSiteDto(partnerName, partnerCode, partnerType, partnerTaxCode, partnerPhone, partnerEmail, partnerWebsite);
            }).ToList();

            _logger.Debug($"Đã lấy {dtos.Count} BusinessPartnerSite");
            return dtos;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy danh sách BusinessPartnerSite: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy danh sách BusinessPartnerSite: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách BusinessPartnerSite: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách BusinessPartnerSite: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả BusinessPartnerSite với thông tin đầy đủ bao gồm PartnerName (Async)
    /// </summary>
    /// <returns>Danh sách BusinessPartnerSiteDto</returns>
    public async Task<List<BusinessPartnerSiteDto>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();

            var sites = await Task.Run(() => context.BusinessPartnerSites
                .OrderBy(s => s.SiteName)
                .ToList());
            
            // Chuyển đổi sang DTO, lấy thông tin partner từ eager-loaded navigation properties
            var dtos = sites.Select(site =>
            {
                string partnerName = null;
                string partnerCode = null;
                int? partnerType = null;
                string partnerTaxCode = null;
                string partnerPhone = null;
                string partnerEmail = null;
                string partnerWebsite = null;

                try
                {
                    var businessPartner = site.BusinessPartner;
                    if (businessPartner != null)
                    {
                        partnerName = businessPartner.PartnerName;
                        partnerCode = businessPartner.PartnerCode;
                        partnerType = businessPartner.PartnerType;
                        partnerTaxCode = businessPartner.TaxCode;
                        partnerPhone = businessPartner.Phone;
                        partnerEmail = businessPartner.Email;
                        partnerWebsite = businessPartner.Website;
                    }
                }
                catch
                {
                    // Navigation property chưa được load hoặc đã bị dispose
                }

                return site.ToSiteDto(partnerName, partnerCode, partnerType, partnerTaxCode, partnerPhone, partnerEmail, partnerWebsite);
            }).ToList();
            
            _logger.Debug($"Đã lấy {dtos.Count} BusinessPartnerSite (async)");
            return dtos;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy danh sách BusinessPartnerSite: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy danh sách BusinessPartnerSite: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách BusinessPartnerSite: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách BusinessPartnerSite: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy BusinessPartnerSite theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerSite</param>
    /// <returns>BusinessPartnerSiteDto hoặc null</returns>
    public BusinessPartnerSiteDto GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var site = context.BusinessPartnerSites.FirstOrDefault(s => s.Id == id);
            
            if (site != null)
            {
                _logger.Debug($"Đã lấy BusinessPartnerSite theo ID: {id} - {site.SiteName}");
                
                // Lấy thông tin partner từ eager-loaded navigation property
                string partnerName = null;
                string partnerCode = null;
                int? partnerType = null;
                string partnerTaxCode = null;
                string partnerPhone = null;
                string partnerEmail = null;
                string partnerWebsite = null;

                try
                {
                    var businessPartner = site.BusinessPartner;
                    if (businessPartner != null)
                    {
                        partnerName = businessPartner.PartnerName;
                        partnerCode = businessPartner.PartnerCode;
                        partnerType = businessPartner.PartnerType;
                        partnerTaxCode = businessPartner.TaxCode;
                        partnerPhone = businessPartner.Phone;
                        partnerEmail = businessPartner.Email;
                        partnerWebsite = businessPartner.Website;
                    }
                }
                catch
                {
                    // Navigation property chưa được load hoặc đã bị dispose
                }

                return site.ToSiteDto(partnerName, partnerCode, partnerType, partnerTaxCode, partnerPhone, partnerEmail, partnerWebsite);
            }
            
            return null;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy BusinessPartnerSite theo ID: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy BusinessPartnerSite theo ID: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy BusinessPartnerSite theo ID: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy BusinessPartnerSite theo ID: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== UPDATE OPERATIONS ==========
    // Update operations are handled by SaveOrUpdate method
    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa BusinessPartnerSite theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerSite</param>
    /// <returns>True nếu xóa thành công</returns>
    public bool Delete(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var entity = context.BusinessPartnerSites.FirstOrDefault(s => s.Id == id);
            if (entity != null)
            {
                context.BusinessPartnerSites.DeleteOnSubmit(entity);
                context.SubmitChanges();
                
                _logger.Info($"Đã xóa BusinessPartnerSite: {id} - {entity.SiteName}");
                return true;
            }
            
            _logger.Warning($"Không tìm thấy BusinessPartnerSite để xóa: {id}");
            return false;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi xóa BusinessPartnerSite: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi xóa BusinessPartnerSite: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa BusinessPartnerSite: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa BusinessPartnerSite: {ex.Message}", ex);
        }
    }

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra SiteCode có tồn tại không
    /// </summary>
    /// <param name="siteCode">SiteCode cần kiểm tra</param>
    /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
    /// <returns>True nếu đã tồn tại</returns>
    public bool IsSiteCodeExists(string siteCode, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(siteCode))
                return false;

            using var context = CreateNewContext();
            var query = context.BusinessPartnerSites.Where(s => s.SiteCode == siteCode);
            
            if (excludeId.HasValue)
            {
                query = query.Where(s => s.Id != excludeId.Value);
            }
            
            var result = query.Any();
            _logger.Debug($"IsSiteCodeExists: SiteCode='{siteCode}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi kiểm tra SiteCode: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi kiểm tra SiteCode: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra SiteCode: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra SiteCode: {ex.Message}", ex);
        }
    }

    #endregion
}
