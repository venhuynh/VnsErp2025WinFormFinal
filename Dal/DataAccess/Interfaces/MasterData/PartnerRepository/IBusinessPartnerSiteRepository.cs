using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.PartnerRepository;

/// <summary>
/// Data Access Layer cho BusinessPartnerSite
/// </summary>
public interface IBusinessPartnerSiteRepository
{
    /// <summary>
    /// Lấy tất cả BusinessPartnerSite với thông tin đầy đủ bao gồm PartnerName
    /// </summary>
    /// <returns>Danh sách BusinessPartnerSite</returns>
    List<BusinessPartnerSite> GetAll();

    /// <summary>
    /// Lấy tất cả BusinessPartnerSite với thông tin đầy đủ bao gồm PartnerName (Async)
    /// </summary>
    /// <returns>Danh sách BusinessPartnerSite</returns>
    Task<List<BusinessPartnerSite>> GetAllAsync();

    /// <summary>
    /// Lấy BusinessPartnerSite theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerSite</param>
    /// <returns>BusinessPartnerSite hoặc null</returns>
    BusinessPartnerSite GetById(Guid id);

    /// <summary>
    /// Lưu hoặc cập nhật BusinessPartnerSite
    /// </summary>
    /// <param name="entity">BusinessPartnerSite entity</param>
    /// <returns>ID của entity đã lưu</returns>
    Guid SaveOrUpdate(BusinessPartnerSite entity);

    /// <summary>
    /// Xóa BusinessPartnerSite theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerSite</param>
    /// <returns>True nếu xóa thành công</returns>
    bool Delete(Guid id);

    /// <summary>
    /// Kiểm tra SiteCode có tồn tại không
    /// </summary>
    /// <param name="siteCode">SiteCode cần kiểm tra</param>
    /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
    /// <returns>True nếu đã tồn tại</returns>
    bool IsSiteCodeExists(string siteCode, Guid? excludeId = null);
}