using System;
using System.Collections.Generic;
using DTO.VersionAndUserManagementDto;

namespace Dal.DataAccess.Interfaces.VersionAndUserManagementDal;

/// <summary>
/// Interface cho Repository quản lý phiên bản ứng dụng
/// </summary>
public interface IApplicationVersionRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy phiên bản đang hoạt động
    /// </summary>
    /// <returns>ApplicationVersionDto hoặc null nếu không có</returns>
    ApplicationVersionDto GetActiveVersion();

    /// <summary>
    /// Lấy tất cả phiên bản
    /// </summary>
    /// <returns>Danh sách ApplicationVersionDto</returns>
    List<ApplicationVersionDto> GetAllVersions();

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Tạo phiên bản mới
    /// </summary>
    /// <param name="dto">ApplicationVersionDto</param>
    /// <returns>ApplicationVersionDto đã tạo</returns>
    ApplicationVersionDto Create(ApplicationVersionDto dto);

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật phiên bản
    /// </summary>
    /// <param name="dto">ApplicationVersionDto</param>
    /// <returns>ApplicationVersionDto đã cập nhật</returns>
    ApplicationVersionDto Update(ApplicationVersionDto dto);

    /// <summary>
    /// Đặt một phiên bản làm Active và vô hiệu hóa các phiên bản khác
    /// </summary>
    /// <param name="versionId">ID phiên bản cần đặt làm Active</param>
    void SetActiveVersion(Guid versionId);

    #endregion
}
