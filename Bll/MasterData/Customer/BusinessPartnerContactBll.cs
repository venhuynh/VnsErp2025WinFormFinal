using Dal.DataAccess.MasterData;
using Dal.DataContext;
using Dal.Logging;
using System;
using System.Collections.Generic;
using Dal.DataAccess.MasterData.Partner;

namespace Bll.MasterData.Customer
{
    /// <summary>
    /// Business Logic Layer cho BusinessPartnerContact
    /// </summary>
    public class BusinessPartnerContactBll
    {
        private readonly BusinessPartnerCategoryRepository _dataAccess;
        private readonly ILogger _logger;

        public BusinessPartnerContactBll()
        {
            _logger = new ConsoleLogger();
            _dataAccess = new BusinessPartnerContactDataAccess(_logger);
        }

        public BusinessPartnerContactBll(string connectionString, ILogger logger = null)
        {
            _logger = logger ?? new ConsoleLogger();
            _dataAccess = new BusinessPartnerContactDataAccess(connectionString, _logger);
        }

        /// <summary>
        /// Lấy tất cả BusinessPartnerContact
        /// </summary>
        /// <returns>Danh sách BusinessPartnerContact</returns>
        public List<BusinessPartnerContact> GetAll()
        {
            try
            {
                _logger?.LogInfo("Bắt đầu lấy danh sách BusinessPartnerContact");
                var result = _dataAccess.GetAll();
                _logger?.LogInfo($"Hoàn thành lấy danh sách BusinessPartnerContact: {result.Count} records");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lấy danh sách BusinessPartnerContact: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu lấy BusinessPartnerContact với ID: {id}");
                var result = _dataAccess.GetById(id);
                _logger?.LogInfo(result != null ? "Tìm thấy BusinessPartnerContact" : "Không tìm thấy BusinessPartnerContact");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lấy BusinessPartnerContact theo ID: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu thêm mới BusinessPartnerContact: {entity.FullName}");
                _dataAccess.Add(entity);
                _logger?.LogInfo($"Hoàn thành thêm mới BusinessPartnerContact với ID: {entity.Id}");
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi thêm mới BusinessPartnerContact: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu lưu BusinessPartnerContact: {entity.FullName}");
                if (entity.Id == Guid.Empty)
                {
                    _dataAccess.Add(entity);
                }
                else
                {
                    _dataAccess.Update(entity);
                }
                _logger?.LogInfo($"Hoàn thành lưu BusinessPartnerContact với ID: {entity.Id}");
                return entity.Id;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lưu BusinessPartnerContact: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu xóa BusinessPartnerContact với ID: {id}");
                // Sử dụng DeleteById thay vì GetById + Delete để tránh DataContext conflict
                var result = _dataAccess.DeleteById(id);
                if (result)
                {
                    _logger?.LogInfo("Xóa BusinessPartnerContact thành công");
                    return true;
                }
                _logger?.LogInfo("Không tìm thấy BusinessPartnerContact để xóa");
                return false;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi xóa BusinessPartnerContact: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Kiểm tra Phone: {phone}");
                var result = _dataAccess.IsPhoneExists(phone, excludeId);
                _logger?.LogInfo(result ? "Phone đã tồn tại" : "Phone chưa tồn tại");
                return result;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi kiểm tra Phone: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu cập nhật avatar cho BusinessPartnerContact với ID: {contactId}");
                _dataAccess.UpdateAvatarOnly(contactId, avatarBytes);
                _logger?.LogInfo($"Hoàn thành cập nhật avatar cho BusinessPartnerContact với ID: {contactId}");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi cập nhật avatar cho BusinessPartnerContact với ID {contactId}: {ex.Message}", ex);
                throw;
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
                _logger?.LogInfo($"Bắt đầu xóa avatar cho BusinessPartnerContact với ID: {contactId}");
                _dataAccess.DeleteAvatarOnly(contactId);
                _logger?.LogInfo($"Hoàn thành xóa avatar cho BusinessPartnerContact với ID: {contactId}");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi xóa avatar cho BusinessPartnerContact với ID {contactId}: {ex.Message}", ex);
                throw;
            }
        }

        public void UpdateEntityWithoutAvatar(BusinessPartnerContact entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));

            try
            {
                // Retrieve the existing entity from the database
                var existingEntity = _dataAccess.GetById(entity.Id);
                if (existingEntity == null) throw new InvalidOperationException($"Entity with ID {entity.Id} does not exist.");

                // Update fields except for the Avatar
                existingEntity.SiteId = entity.SiteId;
                existingEntity.FullName = entity.FullName;
                existingEntity.Position = entity.Position;
                existingEntity.Phone = entity.Phone;
                existingEntity.Email = entity.Email;
                existingEntity.IsPrimary = entity.IsPrimary;
                existingEntity.IsActive = entity.IsActive;

                // Save changes to the database
                _dataAccess.SaveOrUpdate(existingEntity);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating entity without avatar: {ex.Message}", ex);
                throw;
            }
        }
    }
}