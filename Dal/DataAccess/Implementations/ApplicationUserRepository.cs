using Dal.DataAccess.Interfaces;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations
{
    /// <summary>
    /// Data Access cho thực thể ApplicationUser (LINQ to SQL trên DataContext).
    /// Cung cấp các truy vấn/biến đổi phổ biến: lấy theo UserName, active/inactive, tìm kiếm, đổi mật khẩu, kích hoạt/vô hiệu.
    /// </summary>
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        #region Private Fields

        /// <summary>
        /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// Instance logger để theo dõi các thao tác và lỗi
        /// </summary>
        private readonly CustomLogger _logger;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo một instance mới của class ApplicationUserRepository.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
        /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
        public ApplicationUserRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
            _logger.Info("ApplicationUserRepository được khởi tạo với connection string");
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Tạo DataContext mới cho mỗi operation để tránh cache issue
        /// </summary>
        /// <returns>DataContext mới</returns>
        private VnsErp2025DataContext CreateNewContext()
        {
            var context = new VnsErp2025DataContext(_connectionString);

            // Configure eager loading cho navigation properties
            var loadOptions = new DataLoadOptions();
            context.LoadOptions = loadOptions;

            return context;
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
                    throw new ArgumentException(@"UserName không được rỗng", nameof(userName));

                if (string.IsNullOrWhiteSpace(password))
                    throw new ArgumentException(@"Mật khẩu không được rỗng", nameof(password));

                if (IsUserNameExists(userName))
                    throw new DataAccessException($"UserName '{userName}' đã tồn tại");

                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
                return context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 1205) // Deadlock
            {
                System.Threading.Thread.Sleep(100);
                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
                return await Task.Run(() => context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName));
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 1205) // Deadlock
            {
                await Task.Delay(100);
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
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


        #endregion

        #region CRUD - Update

        /// <summary>
        /// Kích hoạt user.
        /// </summary>
        public void ActivateUser(Guid id)
        {
            try
            {
                using var context = CreateNewContext();
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
                using var context = CreateNewContext();
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

                using var context = CreateNewContext();
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
            using var context = CreateNewContext();
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