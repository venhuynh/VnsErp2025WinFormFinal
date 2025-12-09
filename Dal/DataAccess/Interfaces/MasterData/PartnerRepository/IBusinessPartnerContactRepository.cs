using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.PartnerRepository;

/// <summary>
/// Data Access Layer cho BusinessPartnerContact
/// </summary>
public interface IBusinessPartnerContactRepository
{
    /// <summary>
    /// Lấy tất cả BusinessPartnerContact với thông tin BusinessPartnerSite
    /// </summary>
    /// <returns>Danh sách BusinessPartnerContact</returns>
    List<BusinessPartnerContact> GetAll();

    /// <summary>
    /// Lấy tất cả BusinessPartnerContact với thông tin BusinessPartnerSite (Async)
    /// </summary>
    /// <returns>Danh sách BusinessPartnerContact</returns>
    Task<List<BusinessPartnerContact>> GetAllAsync();

    /// <summary>
    /// Lấy BusinessPartnerContact theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerContact</param>
    /// <returns>BusinessPartnerContact hoặc null</returns>
    BusinessPartnerContact GetById(Guid id);

    /// <summary>
    /// Lưu hoặc cập nhật BusinessPartnerContact
    /// </summary>
    /// <param name="entity">BusinessPartnerContact entity</param>
    /// <returns>ID của entity đã lưu</returns>
    Guid SaveOrUpdate(BusinessPartnerContact entity);

    /// <summary>
    /// Xóa BusinessPartnerContact theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerContact</param>
    /// <returns>True nếu xóa thành công</returns>
    bool Delete(Guid id);

    /// <summary>
    /// Kiểm tra Phone có tồn tại không
    /// </summary>
    /// <param name="phone">Phone cần kiểm tra</param>
    /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
    /// <returns>True nếu đã tồn tại</returns>
    bool IsPhoneExists(string phone, Guid? excludeId = null);

    /// <summary>
    /// Kiểm tra Email có tồn tại không
    /// </summary>
    /// <param name="email">Email cần kiểm tra</param>
    /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
    /// <returns>True nếu đã tồn tại</returns>
    bool IsEmailExists(string email, Guid? excludeId = null);

    /// <summary>
    /// Cập nhật chỉ avatar thumbnail của BusinessPartnerContact (chỉ xử lý hình ảnh thumbnail)
    /// </summary>
    /// <param name="contactId">ID của liên hệ</param>
    /// <param name="avatarThumbnailBytes">Dữ liệu hình ảnh thumbnail</param>
    void UpdateAvatarOnly(Guid contactId, byte[] avatarThumbnailBytes);

    /// <summary>
    /// Xóa chỉ avatar của BusinessPartnerContact (chỉ xử lý hình ảnh)
    /// </summary>
    /// <param name="contactId">ID của liên hệ</param>
    void DeleteAvatarOnly(Guid contactId);
}