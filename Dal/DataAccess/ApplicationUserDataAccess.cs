using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;
using Dal.Helpers;

namespace Dal.DataAccess
{
    /// <summary>
    /// Data Access cho thực thể ApplicationUser (LINQ to SQL trên DataContext).
    /// Cung cấp các truy vấn/biến đổi phổ biến: lấy theo UserName, active/inactive, tìm kiếm, đổi mật khẩu, kích hoạt/vô hiệu.
    /// </summary>
    public class ApplicationUserDataAccess : BaseDataAccess<ApplicationUser>
    {
        #region Constructors

        /// <summary>
        /// Khởi tạo mặc định.
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ApplicationUserDataAccess(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Khởi tạo với connection string.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ApplicationUserDataAccess(string connectionString, ILogger logger = null) : base(connectionString,
            logger)
        {
        }

        #endregion

        #region CRUD - Create

        /// <summary>
        /// Thêm user mới với validation.
        /// </summary>
        public ApplicationUser AddNewUser(string userName, string password, bool active = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    throw new ArgumentException("UserName không được rỗng", nameof(userName));

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Mật khẩu không được rỗng", nameof(password));

                if (IsUserNameExists(userName))
                    throw new DataAccessException($"UserName '{userName}' đã tồn tại");

                using var context = CreateContext();
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    HashPassword = password,
                    Active = active
                };

                context.ApplicationUsers.InsertOnSubmit(user);
                context.SubmitChanges();

                return user;
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Duplicate key
            {
                throw new DataAccessException($"UserName '{userName}' đã tồn tại trong hệ thống", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi thêm user '{userName}': {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi thêm user mới '{userName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm user mới với validation (Async).
        /// </summary>
        public async Task<ApplicationUser> AddNewUserAsync(string userName, string password, bool active = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    throw new ArgumentException("UserName không được rỗng", nameof(userName));

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException("Mật khẩu không được rỗng", nameof(password));

                if (await IsUserNameExistsAsync(userName))
                    throw new DataAccessException($"UserName '{userName}' đã tồn tại");

                using var context = CreateContext();
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    HashPassword = password,
                    Active = active
                };

                context.ApplicationUsers.InsertOnSubmit(user);
                await Task.Run(() => context.SubmitChanges());

                return user;
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Duplicate key
            {
                throw new DataAccessException($"UserName '{userName}' đã tồn tại trong hệ thống", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi thêm user '{userName}': {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi thêm user mới '{userName}': {ex.Message}", ex);
            }
        }

        #endregion

        #region CRUD - Read

        /// <summary>
        /// Lấy user theo UserName.
        /// </summary>
        /// <param name="userName">Tên đăng nhập</param>
        /// <returns>ApplicationUser hoặc null</returns>
        public ApplicationUser GetByUserName(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return null;

                using var context = CreateContext();
                return context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 1205) // Deadlock
            {
                System.Threading.Thread.Sleep(100);
                using var context = CreateContext();
                return context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy user '{userName}': {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy ApplicationUser theo UserName '{userName}': {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// Lấy user theo UserName (Async).
        /// </summary>
        /// <param name="userName">Tên đăng nhập</param>
        /// <returns>ApplicationUser hoặc null</returns>
        public async Task<ApplicationUser> GetByUserNameAsync(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return null;

                using var context = CreateContext();
                return await Task.Run(() => context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName));
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 1205) // Deadlock
            {
                await Task.Delay(100);
                using var context = CreateContext();
                return await Task.Run(() => context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName));
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy user '{userName}': {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy ApplicationUser theo UserName '{userName}': {ex.Message}",
                    ex);
            }
        }

        /// <summary>
        /// Lấy danh sách user đang active.
        /// </summary>
        public List<ApplicationUser> GetActiveUsers()
        {
            try
            {
                using var context = CreateContext();
                return context.ApplicationUsers.Where(u => u.Active == true).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách user active: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách user không active.
        /// </summary>
        public List<ApplicationUser> GetInactiveUsers()
        {
            try
            {
                using var context = CreateContext();
                return context.ApplicationUsers.Where(u => u.Active == false).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách user không active: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra UserName có tồn tại hay không.
        /// </summary>
        public bool IsUserNameExists(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return false;

                using var context = CreateContext();
                return context.ApplicationUsers.Any(u => u.UserName == userName);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra UserName '{userName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra UserName có tồn tại hay không (Async).
        /// </summary>
        public async Task<bool> IsUserNameExistsAsync(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return false;

                using var context = CreateContext();
                return await Task.Run(() => context.ApplicationUsers.Any(u => u.UserName == userName));
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi kiểm tra UserName '{userName}': {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra UserName '{userName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm user theo từ khóa (trên trường UserName).
        /// </summary>
        public List<ApplicationUser> SearchUsers(string keyword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(keyword))
                    return GetAll();

                using var context = CreateContext();
                var lower = keyword.ToLower();
                return context.ApplicationUsers.Where(u => u.UserName.ToLower().Contains(lower)).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi tìm kiếm user với từ khóa '{keyword}': {ex.Message}", ex);
            }
        }

        #endregion

        #region CRUD - Update

        /// <summary>
        /// Kích hoạt user.
        /// </summary>
        public void ActivateUser(Guid id)
        {
            try
            {
                using var context = CreateContext();
                var user = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

                if (user == null)
                    throw new DataAccessException($"Không tìm thấy user với ID: {id}");

                user.Active = true;
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kích hoạt user {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Vô hiệu hóa user.
        /// </summary>
        public void DeactivateUser(Guid id)
        {
            try
            {
                using var context = CreateContext();
                var user = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

                if (user == null)
                    throw new DataAccessException($"Không tìm thấy user với ID: {id}");

                user.Active = false;
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi vô hiệu hóa user {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đổi mật khẩu user.
        /// </summary>
        public void ChangePassword(Guid id, string newPassword)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(newPassword))
                    throw new ArgumentException("Mật khẩu mới không được rỗng", nameof(newPassword));

                using var context = CreateContext();
                var user = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

                if (user == null)
                    throw new DataAccessException($"Không tìm thấy user với ID: {id}");

                user.HashPassword = newPassword;
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đổi mật khẩu user {id}: {ex.Message}", ex);
            }
        }

        #endregion

        #region Transactional Operations

        /// <summary>
        /// Chuyển dữ liệu giữa hai user trong một transaction.
        /// </summary>
        public void TransferUserData(Guid fromUserId, Guid toUserId, string newUserName)
        {
            using var context = CreateContext();
            using var transaction = context.Connection.BeginTransaction();
            try
            {
                var fromUser = context.ApplicationUsers.FirstOrDefault(u => u.Id == fromUserId);
                var toUser = context.ApplicationUsers.FirstOrDefault(u => u.Id == toUserId);

                if (fromUser == null)
                    throw new DataAccessException($"Không tìm thấy user nguồn với ID: {fromUserId}");
                if (toUser == null)
                    throw new DataAccessException($"Không tìm thấy user đích với ID: {toUserId}");

                toUser.UserName = newUserName;
                toUser.HashPassword = fromUser.HashPassword;
                toUser.Active = fromUser.Active;

                fromUser.Active = false;

                context.SubmitChanges();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
                throw;
            }
        }

        #endregion
    }
}