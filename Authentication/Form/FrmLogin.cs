using Bll.Authentication;
using Bll.Common;
using Common.Utils;
using Common.Validation;
using BllValidation = Common.Validation;
using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Windows.Forms;


namespace Authentication.Form
{
    /// <summary>
    /// Form đăng nhập hệ thống với validation tự động
    /// Sử dụng DXValidationProvider để kiểm tra dữ liệu đầu vào
    /// </summary>
    public partial class FrmLogin : DevExpress.XtraEditors.XtraForm
    {
        #region Constants
        /// <summary>
        /// Độ dài tối thiểu của mật khẩu
        /// </summary>
        private const int MIN_PASSWORD_LENGTH = 3;
        #endregion

        private readonly LoginBll _loginBll = new LoginBll();

        #region Constructor
        /// <summary>
        /// Khởi tạo form đăng nhập
        /// </summary>
        public FrmLogin()
        {
            InitializeComponent();
            InitializeValidation();
            SetupEventHandlers();
            LoadSavedCredentials();

            // Không gọi TaoTaiKhoanAdminPublic() trong constructor vì có thể database chưa được cấu hình
            // Có thể gọi từ bên ngoài nếu cần thiết
        }
        #endregion

        #region Private Methods - Validation Setup
        /// <summary>
        /// Khởi tạo hệ thống validation cho form
        /// </summary>
        private void InitializeValidation()
        {
            // Thiết lập chế độ validation thủ công - chỉ validate khi user click OK
            dxValidationProvider1.ValidationMode = ValidationMode.Manual;

            // Thiết lập validation rules cho các controls
            SetupUserNameValidation();
            SetupPasswordValidation();
        }

        /// <summary>
        /// Thiết lập validation cho trường tên đăng nhập
        /// Kiểm tra: không rỗng, độ dài từ 3-50 ký tự
        /// </summary>
        private void SetupUserNameValidation()
        {
            // Sử dụng CustomValidationRule với logic từ BLL ValidationHelper
            var userNameValidationRule = new CustomUserNameValidationRule();
            dxValidationProvider1.SetValidationRule(UserNameTextEdit, userNameValidationRule);
        }

        /// <summary>
        /// Thiết lập validation cho trường mật khẩu
        /// </summary>
        private void SetupPasswordValidation()
        {
            // Sử dụng CustomValidationRule với logic từ BLL ValidationHelper
            var passwordValidationRule = new CustomPasswordValidationRule();
            dxValidationProvider1.SetValidationRule(PasswordTextEdit, passwordValidationRule);
        }

        /// <summary>
        /// Thiết lập các event handlers cho form
        /// </summary>
        private void SetupEventHandlers()
        {
            // Validation events - DXValidationProvider tự động clear error khi validation pass
            UserNameTextEdit.Validating += UserNameTextEdit_Validating;
            PasswordTextEdit.Validating += PasswordTextEdit_Validating;
            
            // Keyboard navigation events
            UserNameTextEdit.KeyDown += TextEdit_KeyDown;
            PasswordTextEdit.KeyDown += TextEdit_KeyDown;
            
            // Remember Me checkbox event
            RememberMeCheckBox.CheckedChanged += RememberMeCheckBox_CheckedChanged;
        }
        #endregion

        #region Private Methods - Remember Me
        /// <summary>
        /// Lưu thông tin đăng nhập vào User Settings
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu (đã mã hóa)</param>
        private void SaveCredentials(string username, string password)
        {
            try
            {
                // Lưu vào User Settings (an toàn hơn Registry)
                Properties.Settings.Default.RememberMe = true;
                Properties.Settings.Default.SavedUsername = username;
                Properties.Settings.Default.SavedPassword = password; // Đã được mã hóa từ LoginBll
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi lưu thông tin đăng nhập");
            }
        }

        /// <summary>
        /// Xóa thông tin đăng nhập đã lưu
        /// </summary>
        private void ClearSavedCredentials()
        {
            try
            {
                Properties.Settings.Default.RememberMe = false;
                Properties.Settings.Default.SavedUsername = string.Empty;
                Properties.Settings.Default.SavedPassword = string.Empty;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi xóa thông tin đăng nhập");
            }
        }

        /// <summary>
        /// Tải thông tin đăng nhập đã lưu và hiển thị lên form
        /// </summary>
        private void LoadSavedCredentials()
        {
            try
            {
                if (Properties.Settings.Default.RememberMe && 
                    !string.IsNullOrWhiteSpace(Properties.Settings.Default.SavedUsername))
                {
                    // Hiển thị thông tin đã lưu
                    UserNameTextEdit.Text = Properties.Settings.Default.SavedUsername;
                    RememberMeCheckBox.Checked = true;
                    
                    // Chỉ hiển thị mật khẩu nếu có (có thể là mật khẩu đã mã hóa)
                    if (!string.IsNullOrWhiteSpace(Properties.Settings.Default.SavedPassword))
                    {
                        PasswordTextEdit.Text = Properties.Settings.Default.SavedPassword;
                        // Focus vào password field và select all để user có thể nhập mật khẩu mới
                        PasswordTextEdit.Focus();
                        PasswordTextEdit.SelectAll();
                    }
                    else
                    {
                        // Không có mật khẩu đã lưu, focus vào password field
                        PasswordTextEdit.Focus();
                    }
                }
                else
                {
                    // Không có thông tin đã lưu, focus vào username
                    UserNameTextEdit.Focus();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi tải thông tin đăng nhập");
                // Nếu có lỗi, focus vào username field
                UserNameTextEdit.Focus();
            }
        }
        #endregion

        #region Event Handlers - Validation
        /// <summary>
        /// Xử lý sự kiện validating cho trường tên đăng nhập
        /// </summary>
        /// <param name="sender">Control gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void UserNameTextEdit_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // DXValidationProvider tự động clear error khi validation rule pass
            // Không cần xử lý thêm logic ở đây
        }

        /// <summary>
        /// Xử lý sự kiện validating cho trường mật khẩu
        /// </summary>
        /// <param name="sender">Control gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void PasswordTextEdit_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // DXValidationProvider tự động clear error khi validation rule pass
            // Không cần xử lý thêm logic ở đây
        }
        #endregion

        #region Event Handlers - Navigation
        /// <summary>
        /// Xử lý sự kiện phím tắt cho navigation giữa các controls
        /// </summary>
        /// <param name="sender">Control gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện phím</param>
        private void TextEdit_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                switch (sender)
                {
                    case var _ when sender == UserNameTextEdit:
                        // Chuyển focus đến trường mật khẩu
                        PasswordTextEdit.Focus();
                        break;
                    case var _ when sender == PasswordTextEdit:
                        // Thực hiện đăng nhập
                        OkButton.PerformClick();
                        break;
                }
                e.Handled = true;
            }
        }

        /// <summary>
        /// Xử lý sự kiện thay đổi trạng thái Remember Me checkbox
        /// </summary>
        /// <param name="sender">Control gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void RememberMeCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                // Nếu user bỏ check Remember Me, xóa thông tin đã lưu ngay lập tức
                if (!RememberMeCheckBox.Checked)
                {
                    ClearSavedCredentials();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi xử lý Remember Me");
            }
        }
        #endregion

        #region Event Handlers - Button Actions
        /// <summary>
        /// Xử lý sự kiện click nút Đăng nhập
        /// </summary>
        /// <param name="sender">Control gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void OkButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Bước 1: Validate tất cả controls trước khi xử lý
                if (!ValidateFormInput())
                {
                    return;
                }

                // Bước 2: Lấy thông tin đăng nhập
                var loginCredentials = GetLoginCredentials();

                // Bước 3: Thực hiện xác thực thông qua LoginBll
                var loginResult = _loginBll.XacThucDangNhap(loginCredentials.Username, loginCredentials.Password);

                if (loginResult.ThanhCong)
                {
                    // Lưu thông tin user vào ApplicationSystemUtils
                    if (loginResult.User != null)
                    {
                        ApplicationSystemUtils.SetCurrentUser(loginResult.User);
                    }
                    
                    // Xử lý Remember Me
                    HandleRememberMe(loginCredentials.Username, loginCredentials.Password);
                    
                    ShowSuccessMessage();
                    CloseFormWithSuccess();
                }
                else
                {
                    // Hiển thị thông báo lỗi cụ thể từ LoginBll
                    ShowAuthenticationFailedMessage(loginResult.ThongBaoLoi);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hệ thống");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Hủy
        /// </summary>
        /// <param name="sender">Control gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            CloseFormWithCancel();
        }

        /// <summary>
        /// Xử lý chức năng Remember Me
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        private void HandleRememberMe(string username, string password)
        {
            try
            {
                if (RememberMeCheckBox.Checked)
                {
                    // Lưu thông tin đăng nhập
                    SaveCredentials(username, password);
                }
                else
                {
                    // Xóa thông tin đăng nhập đã lưu
                    ClearSavedCredentials();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi xử lý Remember Me");
            }
        }
        #endregion

        #region Private Methods - Validation & Authentication
        /// <summary>
        /// Validate tất cả controls trên form
        /// </summary>
        /// <returns>True nếu validation thành công, False nếu có lỗi</returns>
        private bool ValidateFormInput()
        {
            if (!dxValidationProvider1.Validate())
            {
                ShowValidationErrorMessage();
                return false;
            }

            // Kiểm tra bổ sung nếu cần
            return PerformAdditionalValidation();
        }

        /// <summary>
        /// Thực hiện validation bổ sung ngoài DXValidationProvider
        /// </summary>
        /// <returns>True nếu validation thành công</returns>
        private bool PerformAdditionalValidation()
        {
            var credentials = GetLoginCredentials();

            // Kiểm tra độ dài mật khẩu (đã có trong DXValidationProvider nhưng kiểm tra thêm)
            if (credentials.Password.Length < MIN_PASSWORD_LENGTH)
            {
                ShowValidationErrorMessage($"Mật khẩu phải có ít nhất {MIN_PASSWORD_LENGTH} ký tự.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Lấy thông tin đăng nhập từ form
        /// </summary>
        /// <returns>Object chứa username và password</returns>
        private (string Username, string Password) GetLoginCredentials()
        {
            return (UserNameTextEdit.Text.Trim(), PasswordTextEdit.Text);
        }

        #endregion

        #region Private Methods - UI Messages
        /// <summary>
        /// Hiển thị thông báo lỗi validation
        /// </summary>
        /// <param name="customMessage">Thông báo tùy chỉnh (optional)</param>
        private void ShowValidationErrorMessage(string customMessage = null)
        {
            var message = customMessage ?? "Vui lòng kiểm tra lại thông tin đã nhập.";
            MsgBox.ShowWarning(message, "Lỗi xác thực");
        }

        /// <summary>
        /// Hiển thị thông báo đăng nhập thành công
        /// </summary>
        private void ShowSuccessMessage()
        {
            MsgBox.ShowSuccess("Đăng nhập thành công.", "Thành công");
        }

        /// <summary>
        /// Hiển thị thông báo đăng nhập thất bại với message cụ thể
        /// </summary>
        /// <param name="message">Thông báo lỗi</param>
        private void ShowAuthenticationFailedMessage(string message = null)
        {
            var errorMessage = message ?? "Tên đăng nhập hoặc mật khẩu không đúng.";
            MsgBox.ShowError(errorMessage, "Đăng nhập thất bại");
        }

        #endregion

        #region Public Methods - User Management
        /// <summary>
        /// Tạo tài khoản admin mặc định để test hệ thống
        /// Có thể gọi từ bên ngoài form
        /// </summary>
        public void TaoTaiKhoanAdminPublic()
        {
            TaoTaiKhoanAdmin();
        }

        /// <summary>
        /// Tạo tài khoản user tùy chỉnh
        /// Có thể gọi từ bên ngoài form
        /// </summary>
        /// <param name="userName">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="active">Trạng thái active</param>
        public void TaoTaiKhoanUserPublic(string userName, string password, bool active = true)
        {
            TaoTaiKhoanUser(userName, password, active);
        }
        #endregion

        #region Private Methods - User Management
        /// <summary>
        /// Tạo tài khoản admin mặc định để test hệ thống
        /// </summary>
        private void TaoTaiKhoanAdmin()
        {
            try
            {
                // Kiểm tra xem admin đã tồn tại chưa
                var existingUser = _loginBll.XacThucDangNhap("admin", "admin");
                if (existingUser.MaLoi != LoginErrorCode.UserKhongTonTai)
                {
                    MsgBox.ShowSuccess("Tài khoản admin đã tồn tại trong hệ thống.", "Thông báo");
                    return;
                }

                // Tạo tài khoản admin mới
                var adminUser = _loginBll.TaoUserMoi("admin", "admin", true);
                
                if (adminUser != null)
                {
                    MsgBox.ShowSuccess($"Đã tạo tài khoản admin thành công!\nUsername: admin\nPassword: admin\nID: {adminUser.Id}", "Tạo tài khoản thành công");
                }
                else
                {
                    MsgBox.ShowError("Không thể tạo tài khoản admin.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi khi tạo tài khoản admin");
            }
        }

        /// <summary>
        /// Tạo tài khoản user tùy chỉnh
        /// </summary>
        /// <param name="userName">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <param name="active">Trạng thái active</param>
        private void TaoTaiKhoanUser(string userName, string password, bool active = true)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
                {
                    MsgBox.ShowWarning("Vui lòng nhập đầy đủ tên đăng nhập và mật khẩu.", "Thông báo");
                    return;
                }

                // Tạo user mới
                var newUser = _loginBll.TaoUserMoi(userName, password, active);
                
                if (newUser != null)
                {
                    MsgBox.ShowSuccess($"Đã tạo tài khoản thành công!\nUsername: {userName}\nPassword: {password}\nID: {newUser.Id}", "Tạo tài khoản thành công");
                }
                else
                {
                    MsgBox.ShowError("Không thể tạo tài khoản.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi khi tạo tài khoản");
            }
        }
        #endregion

        #region Private Methods - Form Management
        /// <summary>
        /// Đóng form với kết quả thành công
        /// </summary>
        private void CloseFormWithSuccess()
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        /// <summary>
        /// Đóng form với kết quả hủy
        /// </summary>
        private void CloseFormWithCancel()
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
        #endregion
    }

    #region Custom Validation Rules - Form Specific
    /// <summary>
    /// Custom validation rule cho tên đăng nhập - Form specific
    /// Kiểm tra: không rỗng, độ dài từ 3-50 ký tự
    /// Sử dụng ValidationHelper từ BLL để thực hiện các check
    /// </summary>
    public class CustomUserNameValidationRule : ValidationRule
    {
        /// <summary>
        /// Thực hiện validation cho tên đăng nhập
        /// Kiểm tra: không rỗng, độ dài từ 3-50 ký tự
        /// </summary>
        /// <param name="control">Control cần validate</param>
        /// <param name="value">Giá trị cần kiểm tra</param>
        /// <returns>True nếu validation thành công</returns>
        public override bool Validate(Control control, object value)
        {
            string username = BllValidation.ValidationHelper.NormalizeString(value?.ToString());

            // Rule 1: Kiểm tra không được để trống - sử dụng BLL ValidationHelper
            if (BllValidation.ValidationHelper.IsNullOrWhiteSpace(username))
            {
                ErrorText = "Vui lòng nhập tên đăng nhập";
                ErrorType = ErrorType.Critical;
                return false;
            }

            // Rule 2: Kiểm tra độ dài - sử dụng BLL ValidationHelper
            if (!BllValidation.ValidationHelper.IsValidLength(username, 3, 50))
            {
                ErrorText = "Tên đăng nhập phải có từ 3 đến 50 ký tự";
                ErrorType = ErrorType.Warning;
                return false;
            }

            // Validation thành công
            return true;
        }
    }

    /// <summary>
    /// Custom validation rule cho mật khẩu - Form specific
    /// Sử dụng ValidationHelper từ BLL để thực hiện các check
    /// </summary>
    public class CustomPasswordValidationRule : ValidationRule
    {
        /// <summary>
        /// Thực hiện validation cho mật khẩu
        /// </summary>
        /// <param name="control">Control cần validate</param>
        /// <param name="value">Giá trị cần kiểm tra</param>
        /// <returns>True nếu validation thành công</returns>
        public override bool Validate(Control control, object value)
        {
            string password = value?.ToString() ?? string.Empty;

            // Rule 1: Kiểm tra không được để trống - sử dụng BLL ValidationHelper
            if (BllValidation.ValidationHelper.IsNullOrWhiteSpace(password))
            {
                ErrorText = "Vui lòng nhập mật khẩu";
                ErrorType = ErrorType.Critical;
                return false;
            }

            // Rule 2: Kiểm tra độ dài tối thiểu - sử dụng BLL ValidationHelper
            if (!BllValidation.ValidationHelper.IsMinLength(password, 3))
            {
                ErrorText = "Mật khẩu phải có ít nhất 3 ký tự";
                ErrorType = ErrorType.Warning;
                return false;
            }

            // Rule 3: Kiểm tra độ phức tạp (tùy chọn) - sử dụng BLL ValidationHelper
            // Có thể bật lên true nếu cần mật khẩu phức tạp
            bool requireComplexity = false;
            if (requireComplexity && !BllValidation.ValidationHelper.IsPasswordComplex(password))
            {
                ErrorText = "Mật khẩu phải chứa chữ hoa, chữ thường, số và ký tự đặc biệt";
                ErrorType = ErrorType.Warning;
                return false;
            }

            // Validation thành công
            return true;
        }
    }
    #endregion
}