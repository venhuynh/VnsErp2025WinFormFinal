using System;
using System.Collections.Generic;
using DTO.VersionAndUserManagementDto;

namespace Dal.DataAccess.Interfaces;

/// <summary>
/// Data Access cho thực thể ApplicationUser (LINQ to SQL trên DataContext).
/// Cung cấp các truy vấn/biến đổi phổ biến: lấy theo UserName, active/inactive, tìm kiếm, đổi mật khẩu, kích hoạt/vô hiệu.
/// </summary>
public interface IApplicationUserRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả người dùng
    /// </summary>
    /// <returns>Danh sách ApplicationUserDto</returns>
    List<ApplicationUserDto> GetAll();

    /// <summary>
    /// Lấy user theo ID
    /// </summary>
    /// <param name="id">ID người dùng</param>
    /// <returns>ApplicationUserDto hoặc null</returns>
    ApplicationUserDto GetById(Guid id);

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Tạo user mới
    /// </summary>
    /// <param name="dto">ApplicationUserDto</param>
    /// <returns>ApplicationUserDto đã được tạo</returns>
    ApplicationUserDto Create(ApplicationUserDto dto);

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật user
    /// </summary>
    /// <param name="dto">ApplicationUserDto</param>
    /// <returns>ApplicationUserDto đã được cập nhật</returns>
    ApplicationUserDto Update(ApplicationUserDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa user
    /// </summary>
    /// <param name="id">ID user cần xóa</param>
    void Delete(Guid id);

    #endregion
}
