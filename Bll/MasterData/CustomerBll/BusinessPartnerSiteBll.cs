using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.PartnerRepository;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using System;
using System.Collections.Generic;

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

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public BusinessPartnerSiteBll()
        {
            
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
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
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
                // Kiểm tra SiteCode đã tồn tại chưa
                if (IsSiteCodeExists(entity.SiteCode))
                {
                    return false;
                }

                // Set thông tin cần thiết cho entity mới
                entity.Id = Guid.Empty; // Để DAL biết đây là tạo mới
                entity.CreatedDate = DateTime.Now;
                entity.UpdatedDate = null;

                // Lưu vào database
                var result = SaveOrUpdate(entity);
                return result != Guid.Empty;
            }
            catch (Exception ex)
            {
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
                return result != Guid.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật BusinessPartnerSite: {ex.Message}", ex);
            }
        }
    }
}
