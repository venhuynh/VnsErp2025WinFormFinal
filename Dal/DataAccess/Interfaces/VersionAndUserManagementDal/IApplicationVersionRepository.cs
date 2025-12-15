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
    /// <returns>VnsErpApplicationVersion hoặc null nếu không có</returns>
    VnsErpApplicationVersion GetActiveVersion();

    /// <summary>
    /// Lấy phiên bản đang hoạt động (async)
    /// </summary>
    /// <returns>VnsErpApplicationVersion hoặc null nếu không có</returns>
    Task<VnsErpApplicationVersion> GetActiveVersionAsync();

    /// <summary>
    /// Lấy tất cả phiên bản
    /// </summary>
    /// <returns>Danh sách phiên bản</returns>
    List<VnsErpApplicationVersion> GetAllVersions();

    /// <summary>
    /// Lấy tất cả phiên bản (async)
    /// </summary>
    /// <returns>Danh sách phiên bản</returns>
    Task<List<VnsErpApplicationVersion>> GetAllVersionsAsync();

    /// <summary>
    /// Lấy phiên bản theo ID
    /// </summary>
    /// <param name="id">ID phiên bản</param>
    /// <returns>VnsErpApplicationVersion hoặc null</returns>
    VnsErpApplicationVersion GetById(Guid id);

    /// <summary>
    /// Lấy phiên bản theo ID (async)
    /// </summary>
    /// <param name="id">ID phiên bản</param>
    /// <returns>VnsErpApplicationVersion hoặc null</returns>
    Task<VnsErpApplicationVersion> GetByIdAsync(Guid id);

    /// <summary>
    /// Tạo phiên bản mới
    /// </summary>
    /// <param name="version">VnsErpApplicationVersion entity</param>
    /// <returns>VnsErpApplicationVersion đã tạo</returns>
    VnsErpApplicationVersion Create(VnsErpApplicationVersion version);

    /// <summary>
    /// Tạo phiên bản mới (async)
    /// </summary>
    /// <param name="version">VnsErpApplicationVersion entity</param>
    /// <returns>VnsErpApplicationVersion đã tạo</returns>
    Task<VnsErpApplicationVersion> CreateAsync(VnsErpApplicationVersion version);

    /// <summary>
    /// Cập nhật phiên bản
    /// </summary>
    /// <param name="version">VnsErpApplicationVersion entity</param>
    /// <returns>VnsErpApplicationVersion đã cập nhật</returns>
    VnsErpApplicationVersion Update(VnsErpApplicationVersion version);

    /// <summary>
    /// Cập nhật phiên bản (async)
    /// </summary>
    /// <param name="version">VnsErpApplicationVersion entity</param>
    /// <returns>VnsErpApplicationVersion đã cập nhật</returns>
    Task<VnsErpApplicationVersion> UpdateAsync(VnsErpApplicationVersion version);

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
