using Dal.Connection;
using Dal.DataAccess.Implementations.MasterData.PartnerRepository;
using Dal.DataAccess.Interfaces.MasterData.PartnerRepository;
using DTO.MasterData.CustomerPartner;
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

        #region ========== HELPER METHODS ==========

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

        #region ========== CREATE OPERATIONS ==========

        /// <summary>
        /// Thêm mới BusinessPartnerContact
        /// </summary>
        /// <param name="dto">BusinessPartnerContactDto</param>
        /// <returns>ID của entity đã thêm</returns>
        public Guid Add(BusinessPartnerContactDto dto)
        {
            try
            {
                if (dto.Id == Guid.Empty)
                {
                    dto.Id = Guid.NewGuid();
                }
                return GetDataAccess().SaveOrUpdate(dto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi thêm mới BusinessPartnerContact: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu hoặc cập nhật BusinessPartnerContact
        /// </summary>
        /// <param name="dto">BusinessPartnerContactDto</param>
        /// <returns>ID của entity đã lưu</returns>
        public Guid SaveOrUpdate(BusinessPartnerContactDto dto)
        {
            try
            {
                if (dto.Id == Guid.Empty)
                {
                    dto.Id = Guid.NewGuid();
                }
                return GetDataAccess().SaveOrUpdate(dto);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu BusinessPartnerContact: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả BusinessPartnerContact
        /// </summary>
        /// <returns>Danh sách BusinessPartnerContactDto</returns>
        public List<BusinessPartnerContactDto> GetAll()
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
        /// <returns>BusinessPartnerContactDto hoặc null</returns>
        public BusinessPartnerContactDto GetById(Guid id)
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

        #endregion

        #region ========== UPDATE OPERATIONS ==========

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

        #endregion

        #region ========== DELETE OPERATIONS ==========

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

        #endregion

        #region ========== VALIDATION & EXISTS CHECKS ==========

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

        #endregion
    }
}
