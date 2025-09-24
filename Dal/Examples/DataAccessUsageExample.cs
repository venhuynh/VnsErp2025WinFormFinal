using System;
using System.Threading.Tasks;
using Dal.DataAccess;
using Dal.Exceptions;

namespace Dal.Examples
{
    /// <summary>
    /// Ví dụ sử dụng DataAccess với async/await và error handling
    /// </summary>
    public class DataAccessUsageExample
    {
        #region thuocTinhDonGian

        private readonly ApplicationUserDataAccess _userDataAccess;

        #endregion

        #region phuongThuc

        public DataAccessUsageExample()
        {
            _userDataAccess = new ApplicationUserDataAccess();
        }

        /// <summary>
        /// Ví dụ sử dụng synchronous methods
        /// </summary>
        public void SynchronousExample()
        {
            try
            {
                // Lấy user theo UserName
                var user = _userDataAccess.LayTheoUserName("admin");
                if (user != null)
                {
                    Console.WriteLine($"Tìm thấy user: {user.UserName}, Active: {user.Active}");
                }

                // Lấy danh sách user active
                var activeUsers = _userDataAccess.LayUserActive();
                Console.WriteLine($"Số lượng user active: {activeUsers.Count}");

                // Thêm user mới
                var newUser = _userDataAccess.ThemUserMoi("testuser", "password123", true);
                Console.WriteLine($"Đã tạo user mới: {newUser.UserName}");

                // Kích hoạt user
                _userDataAccess.KichHoatUser(newUser.Id);
                Console.WriteLine($"Đã kích hoạt user: {newUser.UserName}");
            }
            catch (DataAccessException ex)
            {
                Console.WriteLine($"Lỗi DataAccess: {ex.Message}");
                if (ex.SqlErrorNumber.HasValue)
                {
                    Console.WriteLine($"SQL Error Number: {ex.SqlErrorNumber}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Ví dụ sử dụng asynchronous methods
        /// </summary>
        public async Task AsynchronousExample()
        {
            try
            {
                // Lấy user theo UserName (Async)
                var user = await _userDataAccess.LayTheoUserNameAsync("admin");
                if (user != null)
                {
                    Console.WriteLine($"Tìm thấy user: {user.UserName}, Active: {user.Active}");
                }

                // Kiểm tra UserName tồn tại (Async)
                var exists = await _userDataAccess.UserNameTonTaiAsync("admin");
                Console.WriteLine($"UserName 'admin' tồn tại: {exists}");

                // Thêm user mới (Async)
                var newUser = await _userDataAccess.ThemUserMoiAsync("asyncuser", "password456", true);
                Console.WriteLine($"Đã tạo user mới: {newUser.UserName}");

                // Lấy tất cả user (Async)
                var allUsers = await _userDataAccess.LayTatCaAsync();
                Console.WriteLine($"Tổng số user: {allUsers.Count}");
            }
            catch (DataAccessException ex)
            {
                Console.WriteLine($"Lỗi DataAccess: {ex.Message}");
                if (ex.SqlErrorNumber.HasValue)
                {
                    Console.WriteLine($"SQL Error Number: {ex.SqlErrorNumber}");
                    Console.WriteLine($"Thời gian lỗi: {ex.ThoiGianLoi}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Ví dụ xử lý transaction phức tạp
        /// </summary>
        public void TransactionExample()
        {
            try
            {
                // Tạo 2 users để test
                var user1 = _userDataAccess.ThemUserMoi("user1", "pass1", true);
                var user2 = _userDataAccess.ThemUserMoi("user2", "pass2", false);

                Console.WriteLine($"Tạo user1: {user1.UserName}, user2: {user2.UserName}");

                // Transfer data từ user1 sang user2
                _userDataAccess.TransferUserData(user1.Id, user2.Id, "transferred_user");

                Console.WriteLine("Transfer user data thành công!");

                // Verify kết quả
                var updatedUser1 = _userDataAccess.LayTheoId(user1.Id);
                var updatedUser2 = _userDataAccess.LayTheoId(user2.Id);

                Console.WriteLine($"User1 sau transfer - Active: {updatedUser1.Active}");
                Console.WriteLine($"User2 sau transfer - UserName: {updatedUser2.UserName}, Active: {updatedUser2.Active}");
            }
            catch (DataAccessException ex)
            {
                Console.WriteLine($"Lỗi trong transaction: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không xác định: {ex.Message}");
            }
        }

        /// <summary>
        /// Ví dụ xử lý lỗi cụ thể
        /// </summary>
        public void ErrorHandlingExample()
        {
            try
            {
                // Thử thêm user với UserName trống
                _userDataAccess.ThemUserMoi("", "password");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Lỗi validation: {ex.Message}");
            }

            try
            {
                // Thử thêm user trùng UserName
                _userDataAccess.ThemUserMoi("admin", "password1");
                _userDataAccess.ThemUserMoi("admin", "password2"); // Sẽ bị lỗi duplicate
            }
            catch (DataAccessException ex) when (ex.SqlErrorNumber == 2627)
            {
                Console.WriteLine($"Lỗi duplicate key: {ex.Message}");
            }
            catch (DataAccessException ex)
            {
                Console.WriteLine($"Lỗi DataAccess khác: {ex.Message}");
            }

            try
            {
                // Thử lấy user không tồn tại
                var user = _userDataAccess.LayTheoUserName("nonexistent_user");
                if (user == null)
                {
                    Console.WriteLine("User không tồn tại - đây là trường hợp bình thường");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi không mong đợi: {ex.Message}");
            }
        }

        /// <summary>
        /// Ví dụ sử dụng pattern matching (C# 7.0+)
        /// </summary>
        public void PatternMatchingExample()
        {
            try
            {
                var user = _userDataAccess.LayTheoUserName("admin");
                
                var result = user switch
                {
                    { Active: true, UserName: var name } when name.StartsWith("admin") => "Admin user đang active",
                    { Active: false } => "User không active",
                    null => "User không tồn tại",
                    _ => "User thường"
                };

                Console.WriteLine($"Kết quả pattern matching: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
            }
        }

        #endregion
    }
}
