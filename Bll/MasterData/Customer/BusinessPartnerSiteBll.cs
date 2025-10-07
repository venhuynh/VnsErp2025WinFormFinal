using Dal.DataAccess.MasterData.Partner;
using Dal.DataContext;
using Dal.Logging;
using System;
using System.Collections.Generic;

namespace Bll.MasterData.Customer
{
    /// <summary>
    /// Business Logic Layer cho BusinessPartnerSite
    /// </summary>
    public class BusinessPartnerSiteBll
    {
        private readonly BusinessPartnerSiteDataAccess _dataAccess;
        private readonly ILogger _logger;

        public BusinessPartnerSiteBll()
        {
            _logger = new ConsoleLogger();
            _dataAccess = new BusinessPartnerSiteDataAccess();
        }

        public BusinessPartnerSiteBll(string connectionString, ILogger logger = null)
        {
            _logger = logger ?? new ConsoleLogger();
            _dataAccess = new BusinessPartnerSiteDataAccess(connectionString);
        }

        /// <summary>
        /// Lấy tất cả BusinessPartnerSite
        /// </summary>
        /// <returns>Danh sách BusinessPartnerSite</returns>
        public List<BusinessPartnerSite> GetAll()
        {
            try
            {
                _logger?.LogInfo("Bắt đầu lấy danh sách BusinessPartnerSite");
                var result = _dataAccess.GetAll();
                _logger?.LogInfo($"Hoàn thành lấy danh sách BusinessPartnerSite: {result.Count} records");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lấy danh sách BusinessPartnerSite: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu lấy BusinessPartnerSite với ID: {id}");
                var result = _dataAccess.GetById(id);
                _logger?.LogInfo(result != null ? "Tìm thấy BusinessPartnerSite" : "Không tìm thấy BusinessPartnerSite");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lấy BusinessPartnerSite theo ID: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu lưu BusinessPartnerSite: {entity.SiteName}");
                var result = _dataAccess.SaveOrUpdate(entity);
                _logger?.LogInfo($"Hoàn thành lưu BusinessPartnerSite với ID: {result}");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lưu BusinessPartnerSite: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu xóa BusinessPartnerSite với ID: {id}");
                var result = _dataAccess.Delete(id);
                _logger?.LogInfo(result ? "Xóa BusinessPartnerSite thành công" : "Không thể xóa BusinessPartnerSite");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi xóa BusinessPartnerSite: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Kiểm tra SiteCode: {siteCode}");
                var result = _dataAccess.IsSiteCodeExists(siteCode, excludeId);
                _logger?.LogInfo(result ? "SiteCode đã tồn tại" : "SiteCode chưa tồn tại");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi kiểm tra SiteCode: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu tạo mới BusinessPartnerSite: {entity.SiteName}");
                
                // Kiểm tra SiteCode đã tồn tại chưa
                if (IsSiteCodeExists(entity.SiteCode))
                {
                    _logger?.LogWarning($"SiteCode đã tồn tại: {entity.SiteCode}");
                    return false;
                }

                // Set thông tin cần thiết cho entity mới
                entity.Id = Guid.Empty; // Để DAL biết đây là tạo mới
                entity.CreatedDate = DateTime.Now;
                entity.UpdatedDate = null;

                // Lưu vào database
                var result = SaveOrUpdate(entity);
                _logger?.LogInfo($"Tạo mới BusinessPartnerSite thành công với ID: {result}");
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi tạo mới BusinessPartnerSite: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu cập nhật BusinessPartnerSite: {entity.SiteName}");
                
                // Kiểm tra SiteCode đã tồn tại chưa (loại trừ chính nó)
                if (IsSiteCodeExists(entity.SiteCode, entity.Id))
                {
                    _logger?.LogWarning($"SiteCode đã tồn tại: {entity.SiteCode}");
                    return false;
                }

                // Lấy entity hiện tại
                var existingEntity = GetById(entity.Id);
                if (existingEntity == null)
                {
                    _logger?.LogWarning($"Không tìm thấy BusinessPartnerSite với ID: {entity.Id}");
                    return false;
                }

                // Cập nhật thông tin
                existingEntity.PartnerId = entity.PartnerId;
                existingEntity.SiteCode = entity.SiteCode;
                existingEntity.SiteName = entity.SiteName;
                existingEntity.Address = entity.Address;
                existingEntity.City = entity.City;
                existingEntity.Province = entity.Province;
                existingEntity.Country = entity.Country;
                existingEntity.ContactPerson = entity.ContactPerson;
                existingEntity.Phone = entity.Phone;
                existingEntity.Email = entity.Email;
                existingEntity.IsDefault = entity.IsDefault;
                existingEntity.IsActive = entity.IsActive;
                existingEntity.UpdatedDate = DateTime.Now;

                // Lưu vào database
                var result = SaveOrUpdate(existingEntity);
                _logger?.LogInfo($"Cập nhật BusinessPartnerSite thành công với ID: {result}");
                return true;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi cập nhật BusinessPartnerSite: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _dataAccess?.Dispose();
        }
    }
}
