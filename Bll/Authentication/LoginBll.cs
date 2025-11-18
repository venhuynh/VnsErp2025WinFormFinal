using Common.Appconfig;
using Common.Utils;
using Dal.DataAccess.Implementations;
using Dal.DataAccess.Interfaces;
using Dal.DataContext;
using System;
using System.Threading.Tasks;

namespace Bll.Authentication
{
    /// <summary>
    /// Business Logic Layer cho Authentication
    /// Xử lý logic đăng nhập, xác thực user
    /// </summary>
    public class LoginBll
    {
        #region Fields

        private IApplicationUserRepository _userDataAccess;
        private readonly object _lockObject = new object();

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public LoginBll()
        {
        }

        #endregion

        #region Helper Methods

        /// <summary>
        /// Lấy hoặc khởi tạo Repository (lazy initialization)
        /// </summary>
        private IApplicationUserRepository GetDataAccess()
        {
            if (_userDataAccess == null)
            {
                lock (_lockObject)
                {
                    if (_userDataAccess == null)
                    {
                        try
                        {
                            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                            if (string.IsNullOrEmpty(globalConnectionString))
                            {
                                throw new InvalidOperationException(
                                    "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                            }

                            _userDataAccess = new ApplicationUserRepository(globalConnectionString);
                        }
                        catch (Exception ex)
                        {
                            throw new InvalidOperationException(
                                "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                        }
                    }
                }
            }

            return _userDataAccess;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Xác thực đăng nhập user
        /// </summary>
        /// <param name="userName">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>LoginResult chứa thông tin kết quả đăng nhập</returns>
        public LoginResult XacThucDangNhap(string userName, string password)
        {
            try
            {
                // Validation đầu vào
                if (string.IsNullOrWhiteSpace(userName))
                {
                    return new LoginResult
                    {
                        ThanhCong = false,
                        ThongBaoLoi = "Vui lòng nhập tên đăng nhập",
                        MaLoi = LoginErrorCode.UserNameRong
                    };
                }

                if (string.IsNullOrWhiteSpace(password))
                {
                    return new LoginResult
                    {
                        ThanhCong = false,
                        ThongBaoLoi = "Vui lòng nhập mật khẩu",
                        MaLoi = LoginErrorCode.MatKhauRong
                    };
                }

                // Tìm user theo userName
                var user = GetDataAccess().GetByUserName(userName.Trim());
                if (user == null)
                {
                    return new LoginResult
                    {
                        ThanhCong = false,
                        ThongBaoLoi = "Tên đăng nhập hoặc mật khẩu không đúng",
                        MaLoi = LoginErrorCode.UserKhongTonTai
                    };
                }

                // Kiểm tra user có active không
                if (!user.Active)
                {
                    return new LoginResult
                    {
                        ThanhCong = false,
                        ThongBaoLoi = "Tài khoản đã bị vô hiệu hóa. Vui lòng liên hệ quản trị viên",
                        MaLoi = LoginErrorCode.UserBiVoHieuHoa
                    };
                }

                // Verify password
                bool passwordValid = SecurityHelper.VerifyPassword(password, user.HashPassword);
                if (!passwordValid)
                {
                    return new LoginResult
                    {
                        ThanhCong = false,
                        ThongBaoLoi = "Tên đăng nhập hoặc mật khẩu không đúng",
                        MaLoi = LoginErrorCode.MatKhauSai
                    };
                }

                // Đăng nhập thành công
                return new LoginResult
                {
                    ThanhCong = true,
                    ThongBaoLoi = "Đăng nhập thành công",
                    MaLoi = LoginErrorCode.ThanhCong,
                    User = new UserInfo
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Active = user.Active
                    }
                };
            }
            catch (Exception)
            {
                return new LoginResult
                {
                    ThanhCong = false,
                    ThongBaoLoi = "Lỗi hệ thống. Vui lòng thử lại sau",
                    MaLoi = LoginErrorCode.LoiHeThong
                };
            }
        }

        /// <summary>
        /// Xác thực đăng nhập user (Async version)
        /// </summary>
        /// <param name="userName">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>LoginResult chứa thông tin kết quả đăng nhập</returns>
        public async Task<LoginResult> XacThucDangNhapAsync(string userName, string password)
        {
            // Có thể implement async version nếu cần
            return await Task.Run(() => XacThucDangNhap(userName, password));
        }

        /// <summary>
        /// Tạo user mới với password đã hash
        /// </summary>
        /// <param name="userName">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu gốc</param>
        /// <param name="active">Trạng thái active</param>
        /// <returns>User đã tạo</returns>
        public ApplicationUser TaoUserMoi(string userName, string password, bool active = true)
        {
            try
            {
                // Hash password trước khi lưu
                string hashedPassword = SecurityHelper.HashPassword(password);
                
                // Tạo user mới
                return GetDataAccess().AddNewUser(userName, hashedPassword, active);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
    }

    #region Supporting Classes

    /// <summary>
    /// Kết quả xác thực đăng nhập
    /// </summary>
    public class LoginResult
    {
        public bool ThanhCong { get; set; }
        public string ThongBaoLoi { get; set; }
        public LoginErrorCode MaLoi { get; set; }
        public UserInfo User { get; set; }
    }

    /// <summary>
    /// Thông tin user sau khi đăng nhập thành công
    /// </summary>
    public class UserInfo
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public bool Active { get; set; }
    }

    /// <summary>
    /// Mã lỗi đăng nhập
    /// </summary>
    public enum LoginErrorCode
    {
        ThanhCong = 0,
        UserNameRong = 1,
        MatKhauRong = 2,
        UserKhongTonTai = 3,
        UserBiVoHieuHoa = 4,
        MatKhauSai = 5,
        LoiHeThong = 99
    }

    #endregion
}
