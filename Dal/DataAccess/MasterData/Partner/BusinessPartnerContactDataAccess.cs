using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;

namespace Dal.DataAccess.MasterData.Partner
{
    /// <summary>
    /// Data Access Layer cho BusinessPartnerContact
    /// </summary>
    public class BusinessPartnerContactDataAccess : BaseDataAccess<BusinessPartnerContact>
    {
        public BusinessPartnerContactDataAccess(ILogger logger = null) : base(logger)
        {
        }

        public BusinessPartnerContactDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        /// <summary>
        /// Lấy tất cả BusinessPartnerContact với thông tin BusinessPartnerSite
        /// </summary>
        /// <returns>Danh sách BusinessPartnerContact</returns>
        public override List<BusinessPartnerContact> GetAll()
        {
            try
            {
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include BusinessPartnerSite
                var loadOptions = new System.Data.Linq.DataLoadOptions();
                loadOptions.LoadWith<BusinessPartnerContact>(c => c.BusinessPartnerSite);
                context.LoadOptions = loadOptions;

                // Load tất cả BusinessPartnerContact với BusinessPartnerSite đã được include
                var contacts = context.BusinessPartnerContacts.ToList();

                return contacts;
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy danh sách BusinessPartnerContact: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả BusinessPartnerContact với thông tin BusinessPartnerSite (Async)
        /// </summary>
        /// <returns>Danh sách BusinessPartnerContact</returns>
        public override async Task<List<BusinessPartnerContact>> GetAllAsync()
        {
            try
            {
                using var context = CreateContext();

                // Cấu hình DataLoadOptions để preload navigation properties
                var loadOptions = new System.Data.Linq.DataLoadOptions();
                loadOptions.LoadWith<BusinessPartnerContact>(c => c.BusinessPartnerSite);
                context.LoadOptions = loadOptions;

                return await Task.Run(() => context.BusinessPartnerContacts
                    .OrderBy(c => c.FullName)
                    .ToList());
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy danh sách BusinessPartnerContact: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy BusinessPartnerContact theo ID
        /// </summary>
        /// <param name="id">ID của BusinessPartnerContact</param>
        /// <returns>BusinessPartnerContact hoặc null</returns>
        public BusinessPartnerContact GetById(Guid id)
        {
            try
            {
                using var context = CreateContext();
                return context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == id);
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy BusinessPartnerContact theo ID: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy BusinessPartnerContact theo ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật BusinessPartnerContact
        /// </summary>
        /// <param name="entity">BusinessPartnerContact entity</param>
        /// <returns>ID của entity đã lưu</returns>
        public Guid SaveOrUpdate(BusinessPartnerContact entity)
        {
            try
            {
                using var context = CreateContext();

                if (entity.Id == Guid.Empty)
                {
                    // Thêm mới
                    entity.Id = Guid.NewGuid();
                    context.BusinessPartnerContacts.InsertOnSubmit(entity);
                }
                else
                {
                    // Cập nhật
                    var existingEntity = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == entity.Id);
                    if (existingEntity != null)
                    {
                        existingEntity.SiteId = entity.SiteId;
                        existingEntity.FullName = entity.FullName;
                        existingEntity.Position = entity.Position;
                        existingEntity.Phone = entity.Phone;
                        existingEntity.Email = entity.Email;
                        existingEntity.IsPrimary = entity.IsPrimary;
                        existingEntity.Avatar = entity.Avatar;
                        existingEntity.IsActive = entity.IsActive;
                    }
                    else
                    {
                        throw new DataAccessException("Không tìm thấy BusinessPartnerContact để cập nhật");
                    }
                }

                context.SubmitChanges();
                return entity.Id;
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lưu BusinessPartnerContact: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lưu BusinessPartnerContact: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa BusinessPartnerContact theo ID
        /// </summary>
        /// <param name="id">ID của BusinessPartnerContact</param>
        /// <returns>True nếu xóa thành công</returns>
        public bool Delete(Guid id)
        {
            try
            {
                using var context = CreateContext();
                var entity = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == id);
                if (entity != null)
                {
                    context.BusinessPartnerContacts.DeleteOnSubmit(entity);
                    context.SubmitChanges();
                    return true;
                }
                return false;
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi xóa BusinessPartnerContact: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa BusinessPartnerContact: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra Phone có tồn tại không
        /// </summary>
        /// <param name="phone">Phone cần kiểm tra</param>
        /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
        /// <returns>True nếu đã tồn tại</returns>
        public bool IsPhoneExists(string phone, Guid? excludeId = null)
        {
            try
            {
                using var context = CreateContext();
                var query = context.BusinessPartnerContacts.Where(c => c.Phone == phone);

                if (excludeId.HasValue)
                {
                    query = query.Where(c => c.Id != excludeId.Value);
                }

                return query.Any();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi kiểm tra Phone: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra Phone: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra Email có tồn tại không
        /// </summary>
        /// <param name="email">Email cần kiểm tra</param>
        /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
        /// <returns>True nếu đã tồn tại</returns>
        public bool IsEmailExists(string email, Guid? excludeId = null)
        {
            try
            {
                using var context = CreateContext();
                var query = context.BusinessPartnerContacts.Where(c => c.Email == email);

                if (excludeId.HasValue)
                {
                    query = query.Where(c => c.Id != excludeId.Value);
                }

                return query.Any();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi kiểm tra Email: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra Email: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật chỉ avatar của BusinessPartnerContact (chỉ xử lý hình ảnh)
        /// </summary>
        /// <param name="contactId">ID của liên hệ</param>
        /// <param name="avatarBytes">Dữ liệu hình ảnh</param>
        public void UpdateAvatarOnly(Guid contactId, byte[] avatarBytes)
        {
            try
            {
                using var context = CreateContext();
                
                // Tìm entity hiện tại
                var existingEntity = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == contactId);
                if (existingEntity == null)
                {
                    throw new DataAccessException($"Không tìm thấy BusinessPartnerContact với ID: {contactId}");
                }

                // Cập nhật chỉ avatar
                existingEntity.Avatar = avatarBytes != null ? new System.Data.Linq.Binary(avatarBytes) : null;
                
                // Submit changes
                context.SubmitChanges();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi cập nhật avatar: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi cập nhật avatar: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa chỉ avatar của BusinessPartnerContact (chỉ xử lý hình ảnh)
        /// </summary>
        /// <param name="contactId">ID của liên hệ</param>
        public void DeleteAvatarOnly(Guid contactId)
        {
            try
            {
                using var context = CreateContext();
                
                // Tìm entity hiện tại
                var existingEntity = context.BusinessPartnerContacts.FirstOrDefault(c => c.Id == contactId);
                if (existingEntity == null)
                {
                    throw new DataAccessException($"Không tìm thấy BusinessPartnerContact với ID: {contactId}");
                }

                // Xóa chỉ avatar
                existingEntity.Avatar = null;
                
                // Submit changes
                context.SubmitChanges();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi xóa avatar: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa avatar: {ex.Message}", ex);
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
