using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DataAccess.MasterData.Partner
{
    /// <summary>
    /// Data Access Layer cho BusinessPartnerSite
    /// </summary>
    public class BusinessPartnerSiteDataAccess : BaseDataAccess<BusinessPartnerSite>
    {
        public BusinessPartnerSiteDataAccess()
        {
        }

        public BusinessPartnerSiteDataAccess(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Lấy tất cả BusinessPartnerSite
        /// </summary>
        /// <returns>Danh sách BusinessPartnerSite</returns>
        public override List<BusinessPartnerSite> GetAll()
        {
            try
            {
                using var context = CreateContext();
                return context.BusinessPartnerSites.ToList();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy danh sách BusinessPartnerSite: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                return context.BusinessPartnerSites.FirstOrDefault(s => s.Id == id);
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy BusinessPartnerSite theo ID: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                
                if (entity.Id == Guid.Empty)
                {
                    // Thêm mới
                    entity.Id = Guid.NewGuid();
                    entity.CreatedDate = DateTime.Now;
                    context.BusinessPartnerSites.InsertOnSubmit(entity);
                }
                else
                {
                    // Cập nhật
                    var existingEntity = context.BusinessPartnerSites.FirstOrDefault(s => s.Id == entity.Id);
                    if (existingEntity != null)
                    {
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
                    }
                    else
                    {
                        throw new DataAccessException("Không tìm thấy BusinessPartnerSite để cập nhật");
                    }
                }
                
                context.SubmitChanges();
                return entity.Id;
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lưu BusinessPartnerSite: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                var entity = context.BusinessPartnerSites.FirstOrDefault(s => s.Id == id);
                if (entity != null)
                {
                    context.BusinessPartnerSites.DeleteOnSubmit(entity);
                    context.SubmitChanges();
                    return true;
                }
                return false;
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi xóa BusinessPartnerSite: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                var query = context.BusinessPartnerSites.Where(s => s.SiteCode == siteCode);
                
                if (excludeId.HasValue)
                {
                    query = query.Where(s => s.Id != excludeId.Value);
                }
                
                return query.Any();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi kiểm tra SiteCode: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra SiteCode: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            // Implement IDisposable if needed
        }
    }
}
