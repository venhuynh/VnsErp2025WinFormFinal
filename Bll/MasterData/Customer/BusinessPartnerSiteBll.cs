using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataAccess.MasterData.CustomerDal;
using Dal.DataContext;
using Dal.Logging;

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
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _dataAccess?.Dispose();
        }
    }
}
