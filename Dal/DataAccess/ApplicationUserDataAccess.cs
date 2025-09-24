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
    /// Data Access cho ApplicationUser entity (Simple Approach)
    /// </summary>
    public class ApplicationUserDataAccess : BaseDataAccess<ApplicationUser>
    {
        #region thuocTinhDonGian

        #endregion

        #region phuongThuc

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        /// <param name="logger">Logger instance</param>
        public ApplicationUserDataAccess(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Constructor với connection string
        /// </summary>
        /// <param name="connectionString">Connection string</param>
        /// <param name="logger">Logger instance</param>
        public ApplicationUserDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        /// <summary>
        /// Lấy user theo UserName
        /// </summary>
        /// <param name="userName">Tên user</param>
        /// <returns>ApplicationUser hoặc null</returns>
        public ApplicationUser LayTheoUserName(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return null;

                using var context = new VnsErp2025DataContext(_connStr);
                return context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 1205) // Deadlock
            {
                // Retry logic cho deadlock
                System.Threading.Thread.Sleep(100);
                using var context = new VnsErp2025DataContext(_connStr);
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
                throw new DataAccessException($"Lỗi khi lấy ApplicationUser theo UserName '{userName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy user theo UserName (Async)
        /// </summary>
        /// <param name="userName">Tên user</param>
        /// <returns>ApplicationUser hoặc null</returns>
        public async Task<ApplicationUser> LayTheoUserNameAsync(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return null;

                using var context = new VnsErp2025DataContext(_connStr);
                return await Task.Run(() => context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName));
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 1205) // Deadlock
            {
                // Retry logic cho deadlock
                await Task.Delay(100);
                using var context = new VnsErp2025DataContext(_connStr);
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
                throw new DataAccessException($"Lỗi khi lấy ApplicationUser theo UserName '{userName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách user đang active
        /// </summary>
        /// <returns>Danh sách user active</returns>
        public List<ApplicationUser> LayUserActive()
        {
            try
            {
                using var context = new VnsErp2025DataContext(_connStr);
                return context.ApplicationUsers.Where(u => u.Active == true).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách user active: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách user không active
        /// </summary>
        /// <returns>Danh sách user không active</returns>
        public List<ApplicationUser> LayUserKhongActive()
        {
            try
            {
                using var context = new VnsErp2025DataContext(_connStr);
                return context.ApplicationUsers.Where(u => u.Active == false).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy danh sách user không active: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kiểm tra UserName có tồn tại hay không
        /// </summary>
        /// <param name="userName">Tên user</param>
        /// <returns>True nếu tồn tại</returns>
        public bool UserNameTonTai(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return false;

                using var context = new VnsErp2025DataContext(_connStr);
                return context.ApplicationUsers.Any(u => u.UserName == userName);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi kiểm tra UserName '{userName}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tìm kiếm user theo từ khóa
        /// </summary>
        /// <param name="tuKhoa">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách user tìm được</returns>
        public List<ApplicationUser> TimKiemUser(string tuKhoa)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tuKhoa))
                    return LayTatCa();

                using var context = new VnsErp2025DataContext(_connStr);
                var lowerTuKhoa = tuKhoa.ToLower();
                return context.ApplicationUsers.Where(u => 
                    u.UserName.ToLower().Contains(lowerTuKhoa)
                ).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi tìm kiếm user với từ khóa '{tuKhoa}': {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Kích hoạt user
        /// </summary>
        /// <param name="id">ID của user</param>
        public void KichHoatUser(Guid id)
        {
            try
            {
                using var context = new VnsErp2025DataContext(_connStr);
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
        /// Vô hiệu hóa user
        /// </summary>
        /// <param name="id">ID của user</param>
        public void VoHieuHoaUser(Guid id)
        {
            try
            {
                using var context = new VnsErp2025DataContext(_connStr);
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
        /// Đổi mật khẩu user
        /// </summary>
        /// <param name="id">ID của user</param>
        /// <param name="matKhauMoi">Mật khẩu mới</param>
        public void DoiMatKhau(Guid id, string matKhauMoi)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(matKhauMoi))
                    throw new ArgumentException("Mật khẩu mới không được rỗng", nameof(matKhauMoi));

                using var context = new VnsErp2025DataContext(_connStr);
                var user = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
                
                if (user == null)
                    throw new DataAccessException($"Không tìm thấy user với ID: {id}");

                user.HashPassword = matKhauMoi;
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đổi mật khẩu user {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Thêm user mới với validation
        /// </summary>
        /// <param name="userName">Tên user</param>
        /// <param name="matKhau">Mật khẩu</param>
        /// <param name="active">Trạng thái active</param>
        /// <returns>User đã tạo</returns>
        public ApplicationUser ThemUserMoi(string userName, string matKhau, bool active = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    throw new ArgumentException("UserName không được rỗng", nameof(userName));

                if (string.IsNullOrWhiteSpace(matKhau))
                    throw new ArgumentException("Mật khẩu không được rỗng", nameof(matKhau));

                if (UserNameTonTai(userName))
                    throw new DataAccessException($"UserName '{userName}' đã tồn tại");

                using var context = new VnsErp2025DataContext(_connStr);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    HashPassword = matKhau,
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
        /// Thêm user mới với validation (Async)
        /// </summary>
        /// <param name="userName">Tên user</param>
        /// <param name="matKhau">Mật khẩu</param>
        /// <param name="active">Trạng thái active</param>
        /// <returns>User đã tạo</returns>
        public async Task<ApplicationUser> ThemUserMoiAsync(string userName, string matKhau, bool active = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    throw new ArgumentException("UserName không được rỗng", nameof(userName));

                if (string.IsNullOrWhiteSpace(matKhau))
                    throw new ArgumentException("Mật khẩu không được rỗng", nameof(matKhau));

                if (await UserNameTonTaiAsync(userName))
                    throw new DataAccessException($"UserName '{userName}' đã tồn tại");

                using var context = new VnsErp2025DataContext(_connStr);
                var user = new ApplicationUser
                {
                    Id = Guid.NewGuid(),
                    UserName = userName,
                    HashPassword = matKhau,
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

        /// <summary>
        /// Kiểm tra UserName có tồn tại hay không (Async)
        /// </summary>
        /// <param name="userName">Tên user</param>
        /// <returns>True nếu tồn tại</returns>
        public async Task<bool> UserNameTonTaiAsync(string userName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName))
                    return false;

                using var context = new VnsErp2025DataContext(_connStr);
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
        /// Transfer user data với transaction (Ví dụ operation phức tạp)
        /// </summary>
        /// <param name="fromUserId">ID user nguồn</param>
        /// <param name="toUserId">ID user đích</param>
        /// <param name="newUserName">Tên user mới</param>
        public void TransferUserData(Guid fromUserId, Guid toUserId, string newUserName)
        {
            using var context = new VnsErp2025DataContext(_connStr);
            using var transaction = context.Connection.BeginTransaction();
            try
            {
                // Lấy users
                var fromUser = context.ApplicationUsers.FirstOrDefault(u => u.Id == fromUserId);
                var toUser = context.ApplicationUsers.FirstOrDefault(u => u.Id == toUserId);

                if (fromUser == null)
                    throw new DataAccessException($"Không tìm thấy user nguồn với ID: {fromUserId}");
                if (toUser == null)
                    throw new DataAccessException($"Không tìm thấy user đích với ID: {toUserId}");

                // Transfer data
                toUser.UserName = newUserName;
                toUser.HashPassword = fromUser.HashPassword;
                toUser.Active = fromUser.Active;

                // Deactivate source user
                fromUser.Active = false;

                // Save changes
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