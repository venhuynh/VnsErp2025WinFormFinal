using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.PartnerRepository;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.MasterData.CustomerBll
{
    /// <summary>
    /// Business Logic Layer cho BusinessPartnerSite
    /// </summary>
    public class BusinessPartnerSiteBll
    {
        #region Fields

        private IBusinessPartnerSiteRepository _dataAccess;
        private readonly object _lockObject = new object();
        private readonly ILogger _logger;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public BusinessPartnerSiteBll()
        {
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IBusinessPartnerSiteRepository GetDataAccess()
        {
            if (_dataAccess == null)
            {
                lock (_lockObject)
                {
                    if (_dataAccess == null)
                    {
                        try
                        {
                            // Sử dụng global connection string từ ApplicationStartupManager
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _dataAccess = new BusinessPartnerSiteRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                        }
                    }
                }
            }

            return _dataAccess;
        }

        /// <summary>
        /// Normalize Email để đảm bảo match với constraint CK_BusinessPartnerSite_EmailFormat
        /// Constraint: Email IS NULL OR Email LIKE '%@%.%'
        /// </summary>
        /// <param name="email">Email cần normalize</param>
        /// <returns>Email hợp lệ hoặc null</returns>
        private string NormalizeEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return null;
            }

            var trimmedEmail = email.Trim();
            
            // Kiểm tra format email đơn giản: phải có @ và ít nhất một dấu chấm sau @
            if (!trimmedEmail.Contains("@") || !trimmedEmail.Contains(".") || 
                trimmedEmail.IndexOf("@") >= trimmedEmail.LastIndexOf("."))
            {
                // Email không hợp lệ, trả về null để tránh vi phạm constraint
                return null;
            }

            return trimmedEmail;
        }

        #endregion

        /// <summary>
        /// Lấy tất cả BusinessPartnerSite
        /// </summary>
        /// <returns>Danh sách BusinessPartnerSite</returns>
        public List<BusinessPartnerSite> GetAll()
        {
            try
            {
                return GetDataAccess().GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách BusinessPartnerSite: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả BusinessPartnerSite (alias cho GetAll)
        /// </summary>
        /// <returns>Danh sách BusinessPartnerSite</returns>
        public List<BusinessPartnerSite> GetAllSites()
        {
            return GetAll();
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
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy BusinessPartnerSite theo ID: {ex.Message}", ex);
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
                // KHÔNG set Id ở đây - để DAL tự xử lý
                // Nếu entity.Id == Guid.Empty, DAL sẽ tự tạo Id mới
                // Nếu entity.Id != Guid.Empty, DAL sẽ cập nhật entity hiện có
                return GetDataAccess().SaveOrUpdate(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu BusinessPartnerSite: {ex.Message}", ex);
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
                return GetDataAccess().Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa BusinessPartnerSite: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa BusinessPartnerSite theo ID (alias cho Delete)
        /// </summary>
        /// <param name="id">ID của BusinessPartnerSite</param>
        /// <returns>True nếu xóa thành công</returns>
        public bool DeleteSite(Guid id)
        {
            return Delete(id);
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
                return GetDataAccess().IsSiteCodeExists(siteCode, excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra SiteCode: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo mới BusinessPartnerSite từ Entity
        /// </summary>
        /// <param name="entity">BusinessPartnerSite entity</param>
        /// <returns>True nếu tạo thành công</returns>
        public bool CreateSite(BusinessPartnerSite entity)
        {
            try
            {
                _logger.Debug($"[CreateSite] Bắt đầu tạo mới BusinessPartnerSite - SiteCode: {entity?.SiteCode}, SiteName: {entity?.SiteName}");
                _logger.Debug($"[CreateSite] Entity.Id TRƯỚC KHI tạo entity mới: {entity?.Id} (IsEmpty: {entity?.Id == Guid.Empty})");

                // Kiểm tra SiteCode đã tồn tại chưa
                if (IsSiteCodeExists(entity.SiteCode))
                {
                    _logger.Warning($"[CreateSite] SiteCode đã tồn tại: {entity.SiteCode}");
                    return false;
                }

                // Normalize Email để đảm bảo match với constraint CK_BusinessPartnerSite_EmailFormat
                // Constraint: Email IS NULL OR Email LIKE '%@%.%'
                var normalizedEmail = NormalizeEmail(entity.Email);

                // Tạo entity mới hoàn toàn (detached) để tránh vấn đề với DataContext tracking
                // Copy tất cả properties từ entity được pass vào, nhưng đảm bảo Id = Guid.Empty
                var newEntity = new BusinessPartnerSite
                {
                    Id = Guid.Empty, // Đảm bảo Id là Empty để DAL biết đây là tạo mới
                    PartnerId = entity.PartnerId,
                    SiteCode = entity.SiteCode,
                    SiteName = entity.SiteName,
                    Address = entity.Address,
                    City = entity.City,
                    Province = entity.Province,
                    Country = entity.Country,
                    PostalCode = entity.PostalCode,
                    District = entity.District,
                    Phone = entity.Phone,
                    Email = normalizedEmail, // Email đã được normalize
                    IsDefault = entity.IsDefault,
                    IsActive = entity.IsActive,
                    SiteType = entity.SiteType,
                    Notes = entity.Notes,
                    GoogleMapUrl = entity.GoogleMapUrl,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null
                };

                _logger.Debug($"[CreateSite] Entity mới được tạo với Id: {newEntity.Id} (IsEmpty: {newEntity.Id == Guid.Empty})");
                
                // Kiểm tra lại Id trước khi gọi SaveOrUpdate để đảm bảo không bị thay đổi
                if (newEntity.Id != Guid.Empty)
                {
                    _logger.Warning($"[CreateSite] CẢNH BÁO: Entity.Id đã bị thay đổi từ Guid.Empty thành {newEntity.Id}. Đặt lại về Guid.Empty.");
                    newEntity.Id = Guid.Empty;
                }
                
                _logger.Debug($"[CreateSite] Gọi SaveOrUpdate với newEntity.Id = {newEntity.Id} (IsEmpty: {newEntity.Id == Guid.Empty})");

                // Lưu vào database - truyền trực tiếp vào DAL để tránh BLL layer can thiệp
                var result = GetDataAccess().SaveOrUpdate(newEntity);
                _logger.Debug($"[CreateSite] SaveOrUpdate trả về Id: {result}");
                return result != Guid.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error($"[CreateSite] LỖI khi tạo mới BusinessPartnerSite: {ex.Message}", ex);
                _logger.Error($"[CreateSite] Entity.Id tại thời điểm lỗi: {entity?.Id}");
                throw new Exception($"Lỗi khi tạo mới BusinessPartnerSite: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật BusinessPartnerSite từ Entity
        /// </summary>
        /// <param name="entity">BusinessPartnerSite entity</param>
        /// <returns>True nếu cập nhật thành công</returns>
        public bool UpdateSite(BusinessPartnerSite entity)
        {
            try
            {
                // Kiểm tra SiteCode đã tồn tại chưa (loại trừ chính nó)
                if (IsSiteCodeExists(entity.SiteCode, entity.Id))
                {
                    return false;
                }

                // Lấy entity hiện tại
                var existingEntity = GetById(entity.Id);
                if (existingEntity == null)
                {
                    return false;
                }

                // Normalize Email để đảm bảo match với constraint CK_BusinessPartnerSite_EmailFormat
                var normalizedEmail = NormalizeEmail(entity.Email);

                // Cập nhật thông tin cơ bản
                existingEntity.PartnerId = entity.PartnerId;
                existingEntity.SiteCode = entity.SiteCode;
                existingEntity.SiteName = entity.SiteName;
                existingEntity.Address = entity.Address;
                existingEntity.City = entity.City;
                existingEntity.Province = entity.Province;
                existingEntity.Country = entity.Country;
                existingEntity.Phone = entity.Phone;
                existingEntity.Email = normalizedEmail; // Email đã được normalize
                existingEntity.IsDefault = entity.IsDefault;
                existingEntity.IsActive = entity.IsActive;
                existingEntity.UpdatedDate = DateTime.Now;
                
                // Cập nhật các fields mở rộng
                existingEntity.PostalCode = entity.PostalCode;
                existingEntity.District = entity.District;
                existingEntity.SiteType = entity.SiteType;
                existingEntity.Notes = entity.Notes;
                existingEntity.GoogleMapUrl = entity.GoogleMapUrl;

                // Lưu vào database
                var result = SaveOrUpdate(existingEntity);
                return result != Guid.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật BusinessPartnerSite: {ex.Message}", ex);
            }
        }
    }
}
