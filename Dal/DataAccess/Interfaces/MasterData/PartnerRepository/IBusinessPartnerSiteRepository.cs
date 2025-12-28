using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MasterData.CustomerPartner;

namespace Dal.DataAccess.Interfaces.MasterData.PartnerRepository;

/// <summary>
/// Interface cho Data Access Layer của BusinessPartnerSite
/// </summary>
public interface IBusinessPartnerSiteRepository
{
    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Lưu hoặc cập nhật BusinessPartnerSite
    /// </summary>
    /// <param name="dto">BusinessPartnerSiteDto</param>
    /// <returns>ID của entity đã lưu</returns>
    Guid SaveOrUpdate(BusinessPartnerSiteDto dto);

    #endregion

    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả BusinessPartnerSite
    /// </summary>
    /// <returns>Danh sách BusinessPartnerSiteDto</returns>
    List<BusinessPartnerSiteDto> GetAll();

    /// <summary>
    /// Lấy tất cả BusinessPartnerSite (Async)
    /// </summary>
    /// <returns>Danh sách BusinessPartnerSiteDto</returns>
    Task<List<BusinessPartnerSiteDto>> GetAllAsync();

    /// <summary>
    /// Lấy BusinessPartnerSite theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerSite</param>
    /// <returns>BusinessPartnerSiteDto hoặc null</returns>
    BusinessPartnerSiteDto GetById(Guid id);

    #endregion

    #region ========== UPDATE OPERATIONS ==========
    // Update operations are handled by SaveOrUpdate method
    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa BusinessPartnerSite theo ID
    /// </summary>
    /// <param name="id">ID của BusinessPartnerSite</param>
    /// <returns>True nếu xóa thành công</returns>
    bool Delete(Guid id);

    #endregion

    #region ========== VALIDATION & EXISTS CHECKS ==========

    /// <summary>
    /// Kiểm tra SiteCode có tồn tại không
    /// </summary>
    /// <param name="siteCode">SiteCode cần kiểm tra</param>
    /// <param name="excludeId">ID cần loại trừ (cho trường hợp update)</param>
    /// <returns>True nếu đã tồn tại</returns>
    bool IsSiteCodeExists(string siteCode, Guid? excludeId = null);

    #endregion
}
