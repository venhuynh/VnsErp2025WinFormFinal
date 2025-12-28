using System;
using System.Collections.Generic;
using DTO.VersionAndUserManagementDto;

namespace Dal.DataAccess.Interfaces.VersionAndUserManagementDal;

/// <summary>
/// Interface cho Repository quản lý MAC address được phép
/// </summary>
public interface IAllowedMacAddressRepository
{
    #region ========== READ OPERATIONS ==========

    /// <summary>
    /// Kiểm tra MAC address có được phép không
    /// </summary>
    /// <param name="macAddress">Địa chỉ MAC cần kiểm tra</param>
    /// <returns>True nếu được phép, False nếu không</returns>
    bool IsMacAddressAllowed(string macAddress);

    /// <summary>
    /// Lấy tất cả MAC address được phép
    /// </summary>
    /// <returns>Danh sách AllowedMacAddressDto</returns>
    List<AllowedMacAddressDto> GetAll();

    #endregion

    #region ========== CREATE OPERATIONS ==========

    /// <summary>
    /// Tạo MAC address mới
    /// </summary>
    /// <param name="dto">AllowedMacAddressDto</param>
    /// <returns>AllowedMacAddressDto đã tạo</returns>
    AllowedMacAddressDto Create(AllowedMacAddressDto dto);

    #endregion

    #region ========== UPDATE OPERATIONS ==========

    /// <summary>
    /// Cập nhật MAC address
    /// </summary>
    /// <param name="dto">AllowedMacAddressDto</param>
    /// <returns>AllowedMacAddressDto đã cập nhật</returns>
    AllowedMacAddressDto Update(AllowedMacAddressDto dto);

    #endregion

    #region ========== DELETE OPERATIONS ==========

    /// <summary>
    /// Xóa MAC address
    /// </summary>
    /// <param name="id">ID MAC address cần xóa</param>
    void Delete(Guid id);

    #endregion
}
