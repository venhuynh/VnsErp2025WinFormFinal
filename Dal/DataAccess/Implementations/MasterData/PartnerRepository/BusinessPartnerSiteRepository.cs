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
        loadOptions.LoadWith<BusinessPartnerSite>(s => s.BusinessPartner);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Lấy tất cả BusinessPartnerSite với thông tin đầy đủ bao gồm PartnerName
    /// </summary>
    /// <returns>Danh sách BusinessPartnerSite</returns>
    public List<BusinessPartnerSite> GetAll()
    {
        try
        {
            using var context = CreateNewContext();

            // Load tất cả BusinessPartnerSite với BusinessPartner đã được include
            var sites = context.BusinessPartnerSites
                .OrderBy(s => s.SiteName)
                .ToList();

            _logger.Debug($"Đã lấy {sites.Count} BusinessPartnerSite");
            return sites;
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
    /// <returns>Danh sách BusinessPartnerSite</returns>
    public async Task<List<BusinessPartnerSite>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();

            var sites = await Task.Run(() => context.BusinessPartnerSites
                .OrderBy(s => s.SiteName)
                .ToList());
            
            _logger.Debug($"Đã lấy {sites.Count} BusinessPartnerSite (async)");
            return sites;
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
    /// <returns>BusinessPartnerSite hoặc null</returns>
    public BusinessPartnerSite GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var site = context.BusinessPartnerSites.FirstOrDefault(s => s.Id == id);
            
            if (site != null)
            {
                _logger.Debug($"Đã lấy BusinessPartnerSite theo ID: {id} - {site.SiteName}");
            }
            
            return site;
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

    /// <summary>
    /// Lưu hoặc cập nhật BusinessPartnerSite
    /// </summary>
    /// <param name="entity">BusinessPartnerSite entity</param>
    /// <returns>ID của entity đã lưu</returns>
    public Guid SaveOrUpdate(BusinessPartnerSite entity)
    {
        try
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            using var context = CreateNewContext();
            
            if (entity.Id == Guid.Empty)
            {
                // Thêm mới
                entity.Id = Guid.NewGuid();
                entity.CreatedDate = DateTime.Now;
                context.BusinessPartnerSites.InsertOnSubmit(entity);
                context.SubmitChanges();
                
                _logger.Info($"Đã thêm mới BusinessPartnerSite: {entity.SiteCode} - {entity.SiteName}");
            }
            else
            {
                // Cập nhật
                var existingEntity = context.BusinessPartnerSites.FirstOrDefault(s => s.Id == entity.Id);
                if (existingEntity != null)
                {
                    // Cập nhật các field cơ bản
                    existingEntity.PartnerId = entity.PartnerId;
                    existingEntity.SiteCode = entity.SiteCode;
                    existingEntity.SiteName = entity.SiteName;
                    existingEntity.Address = entity.Address;
                    existingEntity.City = entity.City;
                    existingEntity.Province = entity.Province;
                    existingEntity.Country = entity.Country;
                    existingEntity.Phone = entity.Phone;
                    existingEntity.Email = entity.Email;
                    existingEntity.IsDefault = entity.IsDefault;
                    existingEntity.IsActive = entity.IsActive;
                    existingEntity.UpdatedDate = DateTime.Now;
                    
                    // Cập nhật các fields mở rộng
                    existingEntity.PostalCode = entity.PostalCode;
                    existingEntity.District = entity.District;
                    existingEntity.SiteType = entity.SiteType;
                    existingEntity.Notes = entity.Notes;
                    existingEntity.GoogleMapUrl = entity.GoogleMapUrl;
                    
                    context.SubmitChanges();
                    _logger.Info($"Đã cập nhật BusinessPartnerSite: {existingEntity.SiteCode} - {existingEntity.SiteName}");
                }
                else
                {
                    throw new DataAccessException("Không tìm thấy BusinessPartnerSite để cập nhật");
                }
            }
            
            return entity.Id;
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
