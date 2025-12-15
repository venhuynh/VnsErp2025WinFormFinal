using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.VersionAndUserManagementDal;

/// <summary>
/// Interface cho Repository quản lý MAC address được phép
/// </summary>
public interface IAllowedMacAddressRepository
{
    /// <summary>
    /// Kiểm tra MAC address có được phép không
    /// </summary>
    /// <param name="macAddress">Địa chỉ MAC cần kiểm tra</param>
    /// <returns>True nếu được phép, False nếu không</returns>
    bool IsMacAddressAllowed(string macAddress);

    /// <summary>
    /// Kiểm tra MAC address có được phép không (async)
    /// </summary>
    /// <param name="macAddress">Địa chỉ MAC cần kiểm tra</param>
    /// <returns>True nếu được phép, False nếu không</returns>
    Task<bool> IsMacAddressAllowedAsync(string macAddress);

    /// <summary>
    /// Lấy tất cả MAC address được phép
    /// </summary>
    /// <returns>Danh sách MAC address</returns>
    List<AllowedMacAddress> GetAll();

    /// <summary>
    /// Lấy tất cả MAC address được phép (async)
    /// </summary>
    /// <returns>Danh sách MAC address</returns>
    Task<List<AllowedMacAddress>> GetAllAsync();

    /// <summary>
    /// Lấy MAC address theo ID
    /// </summary>
    /// <param name="id">ID MAC address</param>
    /// <returns>AllowedMacAddress hoặc null</returns>
    AllowedMacAddress GetById(Guid id);

    /// <summary>
    /// Lấy MAC address theo ID (async)
    /// </summary>
    /// <param name="id">ID MAC address</param>
    /// <returns>AllowedMacAddress hoặc null</returns>
    Task<AllowedMacAddress> GetByIdAsync(Guid id);

    /// <summary>
    /// Tạo MAC address mới
    /// </summary>
    /// <param name="macAddress">AllowedMacAddress entity</param>
    /// <returns>AllowedMacAddress đã tạo</returns>
    AllowedMacAddress Create(AllowedMacAddress macAddress);

    /// <summary>
    /// Tạo MAC address mới (async)
    /// </summary>
    /// <param name="macAddress">AllowedMacAddress entity</param>
    /// <returns>AllowedMacAddress đã tạo</returns>
    Task<AllowedMacAddress> CreateAsync(AllowedMacAddress macAddress);

    /// <summary>
    /// Cập nhật MAC address
    /// </summary>
    /// <param name="macAddress">AllowedMacAddress entity</param>
    /// <returns>AllowedMacAddress đã cập nhật</returns>
    AllowedMacAddress Update(AllowedMacAddress macAddress);

    /// <summary>
    /// Cập nhật MAC address (async)
    /// </summary>
    /// <param name="macAddress">AllowedMacAddress entity</param>
    /// <returns>AllowedMacAddress đã cập nhật</returns>
    Task<AllowedMacAddress> UpdateAsync(AllowedMacAddress macAddress);

    /// <summary>
    /// Xóa MAC address
    /// </summary>
    /// <param name="id">ID MAC address cần xóa</param>
    void Delete(Guid id);

    /// <summary>
    /// Xóa MAC address (async)
    /// </summary>
    /// <param name="id">ID MAC address cần xóa</param>
    Task DeleteAsync(Guid id);
}
