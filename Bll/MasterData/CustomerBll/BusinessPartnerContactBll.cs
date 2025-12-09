using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.PartnerRepository;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Bll.MasterData.CustomerBll
{
    /// <summary>
    /// Business Logic Layer cho BusinessPartnerContact
    /// </summary>
    public class BusinessPartnerContactBll
    {
        #region Fields

        private IBusinessPartnerContactRepository _dataAccess;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public BusinessPartnerContactBll()
        {
            
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IBusinessPartnerContactRepository GetDataAccess()
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

                            _dataAccess = new BusinessPartnerContactRepository(globalConnectionString);
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
        /// Lấy tất cả BusinessPartnerContact
        /// </summary>
        /// <returns>Danh sách BusinessPartnerContact</returns>
        public List<BusinessPartnerContact> GetAll()
        {
            try
            {
                return GetDataAccess().GetAll();
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
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
                return GetDataAccess().GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy BusinessPartnerContact theo ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm mới BusinessPartnerContact
        /// </summary>
        /// <param name="entity">BusinessPartnerContact entity</param>
        /// <returns>ID của entity đã thêm</returns>
        public Guid Add(BusinessPartnerContact entity)
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
                throw new Exception($"Lỗi khi thêm mới BusinessPartnerContact: {ex.Message}", ex);
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
                if (entity.Id == Guid.Empty)
                {
                    entity.Id = Guid.NewGuid();
                }
                return GetDataAccess().SaveOrUpdate(entity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu BusinessPartnerContact: {ex.Message}", ex);
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
                return GetDataAccess().Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa BusinessPartnerContact: {ex.Message}", ex);
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
                return GetDataAccess().IsPhoneExists(phone, excludeId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi kiểm tra Phone: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật chỉ avatar thumbnail của BusinessPartnerContact (chỉ xử lý hình ảnh thumbnail)
        /// </summary>
        /// <param name="contactId">ID của liên hệ</param>
        /// <param name="avatarThumbnailBytes">Dữ liệu hình ảnh thumbnail</param>
        public void UpdateAvatarOnly(Guid contactId, byte[] avatarThumbnailBytes)
        {
            try
            {
                GetDataAccess().UpdateAvatarOnly(contactId, avatarThumbnailBytes);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật avatar thumbnail cho BusinessPartnerContact với ID {contactId}: {ex.Message}", ex);
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
                GetDataAccess().DeleteAvatarOnly(contactId);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa avatar cho BusinessPartnerContact với ID {contactId}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật entity mà không thay đổi avatar (giữ nguyên các trường Avatar metadata và thumbnail)
        /// </summary>
        /// <param name="entity">BusinessPartnerContact entity</param>
        public void UpdateEntityWithoutAvatar(BusinessPartnerContact entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                // Retrieve the existing entity from the database
                var existingEntity = GetDataAccess().GetById(entity.Id);
                if (existingEntity == null) throw new InvalidOperationException($"Entity with ID {entity.Id} does not exist.");

                // Update fields except for the Avatar fields (metadata and thumbnail)
                existingEntity.SiteId = entity.SiteId;
                existingEntity.FullName = entity.FullName;
                existingEntity.Position = entity.Position;
                existingEntity.Phone = entity.Phone;
                existingEntity.Email = entity.Email;
                existingEntity.IsPrimary = entity.IsPrimary;
                existingEntity.IsActive = entity.IsActive;
                
                // Cập nhật các fields mới
                existingEntity.Mobile = entity.Mobile;
                existingEntity.Fax = entity.Fax;
                existingEntity.Department = entity.Department;
                existingEntity.BirthDate = entity.BirthDate;
                existingEntity.Gender = entity.Gender;
                existingEntity.LinkedIn = entity.LinkedIn;
                existingEntity.Skype = entity.Skype;
                existingEntity.WeChat = entity.WeChat;
                existingEntity.Notes = entity.Notes;
                
                // Không cập nhật các trường Avatar (giữ nguyên giá trị hiện có):
                // - AvatarFileName, AvatarRelativePath, AvatarFullPath
                // - AvatarStorageType, AvatarFileSize, AvatarChecksum
                // - AvatarThumbnailData

                // Save changes to the database
                GetDataAccess().SaveOrUpdate(existingEntity);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật entity không có avatar: {ex.Message}", ex);
            }
        }
    }
}
