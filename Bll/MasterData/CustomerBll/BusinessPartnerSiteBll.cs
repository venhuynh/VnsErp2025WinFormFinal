using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.PartnerRepository;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using DTO.MasterData.CustomerPartner;
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

        #region ========== CREATE OPERATIONS ==========

        /// <summary>
        /// Tạo mới BusinessPartnerSite từ DTO
        /// </summary>
        /// <param name="dto">BusinessPartnerSiteDto</param>
        /// <returns>True nếu tạo thành công</returns>
        public bool CreateSite(BusinessPartnerSiteDto dto)
        {
            try
            {
                _logger.Debug($"[CreateSite] Bắt đầu tạo mới BusinessPartnerSite - SiteCode: {dto?.SiteCode}, SiteName: {dto?.SiteName}");
                _logger.Debug($"[CreateSite] DTO.Id TRƯỚC KHI tạo entity mới: {dto?.Id} (IsEmpty: {dto?.Id == Guid.Empty})");

                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                // Kiểm tra SiteCode đã tồn tại chưa
                if (IsSiteCodeExists(dto.SiteCode))
                {
                    _logger.Warning($"[CreateSite] SiteCode đã tồn tại: {dto.SiteCode}");
                    return false;
                }

                // Normalize Email để đảm bảo match với constraint CK_BusinessPartnerSite_EmailFormat
                dto.Email = NormalizeEmail(dto.Email);

                // Đảm bảo Id = Guid.Empty để DAL biết đây là tạo mới
                dto.Id = Guid.Empty;
                dto.CreatedDate = DateTime.Now;
                dto.UpdatedDate = null;
                
                _logger.Debug($"[CreateSite] Gọi SaveOrUpdate với dto.Id = {dto.Id} (IsEmpty: {dto.Id == Guid.Empty})");

                // Lưu vào database
                var result = GetDataAccess().SaveOrUpdate(dto);
                _logger.Debug($"[CreateSite] SaveOrUpdate trả về Id: {result}");
                return result != Guid.Empty;
            }
            catch (Exception ex)
            {
                _logger.Error($"[CreateSite] LỖI khi tạo mới BusinessPartnerSite: {ex.Message}", ex);
                _logger.Error($"[CreateSite] DTO.Id tại thời điểm lỗi: {dto?.Id}");
                throw new Exception($"Lỗi khi tạo mới BusinessPartnerSite: {ex.Message}", ex);
            }
        }

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
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                // Normalize Email để đảm bảo match với constraint CK_BusinessPartnerSite_EmailFormat
                dto.Email = NormalizeEmail(dto.Email);

                // Set UpdatedDate nếu đang update
                if (dto.Id != Guid.Empty)
                {
                    dto.UpdatedDate = DateTime.Now;
                }
                else
                {
                    dto.CreatedDate = DateTime.Now;
                    dto.UpdatedDate = null;
                }

                return GetDataAccess().SaveOrUpdate(dto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu BusinessPartnerSite: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả BusinessPartnerSite
        /// </summary>
        /// <returns>Danh sách BusinessPartnerSiteDto</returns>
        public List<BusinessPartnerSiteDto> GetAll()
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
        /// <returns>Danh sách BusinessPartnerSiteDto</returns>
        public List<BusinessPartnerSiteDto> GetAllSites()
        {
            return GetAll();
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
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy BusinessPartnerSite theo ID: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== UPDATE OPERATIONS ==========

        /// <summary>
        /// Cập nhật BusinessPartnerSite từ DTO
        /// </summary>
        /// <param name="dto">BusinessPartnerSiteDto</param>
        /// <returns>True nếu cập nhật thành công</returns>
        public bool UpdateSite(BusinessPartnerSiteDto dto)
        {
            try
            {
                if (dto == null)
                {
                    throw new ArgumentNullException(nameof(dto));
                }

                // Kiểm tra SiteCode đã tồn tại chưa (loại trừ chính nó)
                if (IsSiteCodeExists(dto.SiteCode, dto.Id))
                {
                    return false;
                }

                // Lấy entity hiện tại
                var existingDto = GetById(dto.Id);
                if (existingDto == null)
                {
                    return false;
                }

                // Normalize Email để đảm bảo match với constraint CK_BusinessPartnerSite_EmailFormat
                dto.Email = NormalizeEmail(dto.Email);

                // Set UpdatedDate
                dto.UpdatedDate = DateTime.Now;
                dto.CreatedDate = existingDto.CreatedDate; // Giữ nguyên CreatedDate

                // Lưu vào database
                var result = SaveOrUpdate(dto);
                return result != Guid.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật BusinessPartnerSite: {ex.Message}", ex);
            }
        }

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
                return GetDataAccess().IsSiteCodeExists(siteCode, excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra SiteCode: {ex.Message}", ex);
            }
        }

        #endregion
    }
}
