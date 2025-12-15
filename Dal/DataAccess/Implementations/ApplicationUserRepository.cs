using Dal.DataAccess.Interfaces;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;

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
            var logger = LoggerFactory.CreateLogger(LogCategory.DAL);
            logger.Info("ApplicationUserRepository được khởi tạo với connection string");
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
            // Load Employee và các navigation properties liên quan
            loadOptions.LoadWith<ApplicationUser>(u => u.Employee);
            loadOptions.LoadWith<Employee>(e => e.Department);
            loadOptions.LoadWith<Employee>(e => e.Position);
            context.LoadOptions = loadOptions;

            return context;
        }

        #endregion

        #region CRUD - Create

        /// <summary>
        /// Tạo user mới
        /// </summary>
        public ApplicationUser Create(ApplicationUser user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                if (user.Id == Guid.Empty)
                    user.Id = Guid.NewGuid();

                // Validate UserName
                if (string.IsNullOrWhiteSpace(user.UserName))
                    throw new ArgumentException("UserName không được để trống", nameof(user));

                if (IsUserNameExists(user.UserName))
                    throw new DataAccessException($"UserName '{user.UserName}' đã tồn tại");

                using var context = CreateNewContext();
                context.ApplicationUsers.InsertOnSubmit(user);
                context.SubmitChanges();
                
                return user;
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Duplicate key
            {
                throw new DataAccessException($"UserName '{user.UserName}' đã tồn tại trong hệ thống", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi tạo user '{user.UserName}': {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi tạo user mới: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo user mới (async)
        /// </summary>
        public async Task<ApplicationUser> CreateAsync(ApplicationUser user)
        {
            return await Task.Run(() => Create(user));
        }

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
                    Active = active,
                    EmployeeId = null // Có thể set EmployeeId sau
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
                    Active = active,
                    EmployeeId = null // Có thể set EmployeeId sau
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
        /// Lấy tất cả người dùng
        /// </summary>
        public List<ApplicationUser> GetAll()
        {
            try
            {
                using var context = CreateNewContext();
                var users = context.ApplicationUsers
                    .OrderBy(u => u.UserName)
                    .ToList();
                
                return users;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả người dùng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả người dùng (async)
        /// </summary>
        public async Task<List<ApplicationUser>> GetAllAsync()
        {
            try
            {
                using var context = CreateNewContext();
                var users = await Task.Run(() => 
                    context.ApplicationUsers
                        .OrderBy(u => u.UserName)
                        .ToList());
                
                return users;
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả người dùng (async): {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy user theo ID
        /// </summary>
        public ApplicationUser GetById(Guid id)
        {
            try
            {
                using var context = CreateNewContext();
                return context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy user theo ID: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy user theo ID (async)
        /// </summary>
        public async Task<ApplicationUser> GetByIdAsync(Guid id)
        {
            try
            {
                using var context = CreateNewContext();
                return await Task.Run(() => context.ApplicationUsers.FirstOrDefault(u => u.Id == id));
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy user theo ID (async): {ex.Message}", ex);
            }
        }

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
        /// Cập nhật user
        /// </summary>
        public ApplicationUser Update(ApplicationUser user)
        {
            try
            {
                if (user == null)
                    throw new ArgumentNullException(nameof(user));

                using var context = CreateNewContext();
                var existing = context.ApplicationUsers.FirstOrDefault(u => u.Id == user.Id);

                if (existing == null)
                    throw new DataAccessException($"Không tìm thấy user với ID: {user.Id}");

                // Validate UserName nếu thay đổi
                if (existing.UserName != user.UserName && IsUserNameExists(user.UserName))
                    throw new DataAccessException($"UserName '{user.UserName}' đã tồn tại");

                // Cập nhật các thuộc tính
                existing.UserName = user.UserName;
                existing.HashPassword = user.HashPassword;
                existing.Active = user.Active;
                existing.EmployeeId = user.EmployeeId;

                context.SubmitChanges();
                
                return existing;
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Duplicate key
            {
                throw new DataAccessException($"UserName '{user.UserName}' đã tồn tại trong hệ thống", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi cập nhật user: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi cập nhật user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật user (async)
        /// </summary>
        public async Task<ApplicationUser> UpdateAsync(ApplicationUser user)
        {
            return await Task.Run(() => Update(user));
        }

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

        #region CRUD - Delete

        /// <summary>
        /// Xóa user
        /// </summary>
        public void Delete(Guid id)
        {
            try
            {
                using var context = CreateNewContext();
                var user = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

                if (user == null)
                    throw new DataAccessException($"Không tìm thấy user với ID: {id}");

                context.ApplicationUsers.DeleteOnSubmit(user);
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa user: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa user (async)
        /// </summary>
        public async Task DeleteAsync(Guid id)
        {
            await Task.Run(() => Delete(id));
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