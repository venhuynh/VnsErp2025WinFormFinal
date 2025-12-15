using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.VersionAndUserManagementDal;

/// <summary>
/// Interface cho Repository quản lý phiên bản ứng dụng
/// </summary>
public interface IApplicationVersionRepository
{
    /// <summary>
    /// Lấy phiên bản đang hoạt động
    /// </summary>
    /// <returns>ApplicationVersion hoặc null nếu không có</returns>
    ApplicationVersion GetActiveVersion();

    /// <summary>
    /// Lấy phiên bản đang hoạt động (async)
    /// </summary>
    /// <returns>ApplicationVersion hoặc null nếu không có</returns>
    Task<ApplicationVersion> GetActiveVersionAsync();

    /// <summary>
    /// Lấy tất cả phiên bản
    /// </summary>
    /// <returns>Danh sách phiên bản</returns>
    List<ApplicationVersion> GetAllVersions();

    /// <summary>
    /// Lấy tất cả phiên bản (async)
    /// </summary>
    /// <returns>Danh sách phiên bản</returns>
    Task<List<ApplicationVersion>> GetAllVersionsAsync();

    /// <summary>
    /// Lấy phiên bản theo ID
    /// </summary>
    /// <param name="id">ID phiên bản</param>
    /// <returns>ApplicationVersion hoặc null</returns>
    ApplicationVersion GetById(Guid id);

    /// <summary>
    /// Lấy phiên bản theo ID (async)
    /// </summary>
    /// <param name="id">ID phiên bản</param>
    /// <returns>ApplicationVersion hoặc null</returns>
    Task<ApplicationVersion> GetByIdAsync(Guid id);

    /// <summary>
    /// Tạo phiên bản mới
    /// </summary>
    /// <param name="version">ApplicationVersion entity</param>
    /// <returns>ApplicationVersion đã tạo</returns>
    ApplicationVersion Create(ApplicationVersion version);

    /// <summary>
    /// Tạo phiên bản mới (async)
    /// </summary>
    /// <param name="version">ApplicationVersion entity</param>
    /// <returns>ApplicationVersion đã tạo</returns>
    Task<ApplicationVersion> CreateAsync(ApplicationVersion version);

    /// <summary>
    /// Cập nhật phiên bản
    /// </summary>
    /// <param name="version">ApplicationVersion entity</param>
    /// <returns>ApplicationVersion đã cập nhật</returns>
    ApplicationVersion Update(ApplicationVersion version);

    /// <summary>
    /// Cập nhật phiên bản (async)
    /// </summary>
    /// <param name="version">ApplicationVersion entity</param>
    /// <returns>ApplicationVersion đã cập nhật</returns>
    Task<ApplicationVersion> UpdateAsync(ApplicationVersion version);

    /// <summary>
    /// Đặt một phiên bản làm Active và vô hiệu hóa các phiên bản khác
    /// </summary>
    /// <param name="versionId">ID phiên bản cần đặt làm Active</param>
    void SetActiveVersion(Guid versionId);

    /// <summary>
    /// Đặt một phiên bản làm Active và vô hiệu hóa các phiên bản khác (async)
    /// </summary>
    /// <param name="versionId">ID phiên bản cần đặt làm Active</param>
    Task SetActiveVersionAsync(Guid versionId);
}
