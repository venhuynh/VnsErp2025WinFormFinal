using System;
using System.Windows.Forms;
using DevExpress.XtraEditors.DXErrorProvider;
using BllValidation = Bll.Validation;
using Bll.Utils;

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

        #region Constructor
        /// <summary>
        /// Khởi tạo form đăng nhập
        /// </summary>
        public FrmLogin()
        {
            InitializeComponent();
            InitializeValidation();
            SetupEventHandlers();
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

                // Bước 3: Thực hiện xác thực
                if (AuthenticateUser(loginCredentials.Username, loginCredentials.Password))
                {
                    ShowSuccessMessage();
                    CloseFormWithSuccess();
                }
                else
                {
                    ShowAuthenticationFailedMessage();
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

        /// <summary>
        /// Thực hiện xác thực người dùng
        /// </summary>
        /// <param name="username">Tên đăng nhập</param>
        /// <param name="password">Mật khẩu</param>
        /// <returns>True nếu xác thực thành công</returns>
        private bool AuthenticateUser(string username, string password)
        {
            // TODO: Thay thế logic xác thực thực tế
            // Hiện tại sử dụng hardcoded credentials để demo
            return username == "admin" && password == "password";
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
            MsgBox.ShowInfo("Đăng nhập thành công.", "Thành công");
        }

        /// <summary>
        /// Hiển thị thông báo đăng nhập thất bại
        /// </summary>
        private void ShowAuthenticationFailedMessage()
        {
            MsgBox.ShowError("Tên đăng nhập hoặc mật khẩu không đúng.", "Đăng nhập thất bại");
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
    /// Sử dụng ValidationHelper từ BLL để thực hiện các check
    /// </summary>
    public class CustomUserNameValidationRule : ValidationRule
    {
        /// <summary>
        /// Thực hiện validation cho tên đăng nhập
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

            // Rule 3: Kiểm tra định dạng email - sử dụng BLL ValidationHelper
            if (!BllValidation.ValidationHelper.IsValidEmail(username))
            {
                ErrorText = "Tên đăng nhập phải có định dạng email hợp lệ";
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