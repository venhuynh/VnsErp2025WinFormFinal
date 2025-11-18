using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces;

/// <summary>
/// Data Access cho thực thể ApplicationUser (LINQ to SQL trên DataContext).
/// Cung cấp các truy vấn/biến đổi phổ biến: lấy theo UserName, active/inactive, tìm kiếm, đổi mật khẩu, kích hoạt/vô hiệu.
/// </summary>
public interface IApplicationUserRepository
{
    #region CRUD - Create

    /// <summary>
    /// Thêm user mới với validation.
    /// </summary>
    /// <param name="userName">Tên đăng nhập</param>
    /// <param name="password">Mật khẩu</param>
    /// <param name="active">Trạng thái active (mặc định: true)</param>
    /// <returns>ApplicationUser đã được tạo</returns>
    ApplicationUser AddNewUser(string userName, string password, bool active = true);

    /// <summary>
    /// Thêm user mới với validation (Async).
    /// </summary>
    /// <param name="userName">Tên đăng nhập</param>
    /// <param name="password">Mật khẩu</param>
    /// <param name="active">Trạng thái active (mặc định: true)</param>
    /// <returns>ApplicationUser đã được tạo</returns>
    Task<ApplicationUser> AddNewUserAsync(string userName, string password, bool active = true);

    #endregion

    #region CRUD - Read

    /// <summary>
    /// Lấy user theo UserName.
    /// </summary>
    /// <param name="userName">Tên đăng nhập</param>
    /// <returns>ApplicationUser hoặc null</returns>
    ApplicationUser GetByUserName(string userName);

    /// <summary>
    /// Lấy user theo UserName (Async).
    /// </summary>
    /// <param name="userName">Tên đăng nhập</param>
    /// <returns>ApplicationUser hoặc null</returns>
    Task<ApplicationUser> GetByUserNameAsync(string userName);

    /// <summary>
    /// Lấy danh sách user đang active.
    /// </summary>
    /// <returns>Danh sách ApplicationUser đang active</returns>
    List<ApplicationUser> GetActiveUsers();

    /// <summary>
    /// Lấy danh sách user không active.
    /// </summary>
    /// <returns>Danh sách ApplicationUser không active</returns>
    List<ApplicationUser> GetInactiveUsers();

    /// <summary>
    /// Kiểm tra UserName có tồn tại hay không.
    /// </summary>
    /// <param name="userName">Tên đăng nhập cần kiểm tra</param>
    /// <returns>True nếu UserName đã tồn tại</returns>
    bool IsUserNameExists(string userName);

    /// <summary>
    /// Kiểm tra UserName có tồn tại hay không (Async).
    /// </summary>
    /// <param name="userName">Tên đăng nhập cần kiểm tra</param>
    /// <returns>True nếu UserName đã tồn tại</returns>
    Task<bool> IsUserNameExistsAsync(string userName);

    #endregion

    #region CRUD - Update

    /// <summary>
    /// Kích hoạt user.
    /// </summary>
    /// <param name="id">ID của user cần kích hoạt</param>
    void ActivateUser(Guid id);

    /// <summary>
    /// Vô hiệu hóa user.
    /// </summary>
    /// <param name="id">ID của user cần vô hiệu hóa</param>
    void DeactivateUser(Guid id);

    /// <summary>
    /// Đổi mật khẩu user.
    /// </summary>
    /// <param name="id">ID của user</param>
    /// <param name="newPassword">Mật khẩu mới</param>
    void ChangePassword(Guid id, string newPassword);

    #endregion

    #region Transactional Operations

    /// <summary>
    /// Chuyển dữ liệu giữa hai user trong một transaction.
    /// </summary>
    /// <param name="fromUserId">ID của user nguồn</param>
    /// <param name="toUserId">ID của user đích</param>
    /// <param name="newUserName">Tên đăng nhập mới cho user đích</param>
    void TransferUserData(Guid fromUserId, Guid toUserId, string newUserName);

    #endregion
}
