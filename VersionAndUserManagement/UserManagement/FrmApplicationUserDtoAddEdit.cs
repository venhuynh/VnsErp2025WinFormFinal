using Bll.Common;
using Common.Common;
using Common.Utils;
using DevExpress.XtraBars;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraSplashScreen;
using DTO.VersionAndUserManagementDto;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VersionAndUserManagement.UserManagement
{
    /// <summary>
    /// Form thêm mới/chỉnh sửa người dùng ứng dụng
    /// </summary>
    public partial class FrmApplicationUserDtoAddEdit : DevExpress.XtraEditors.XtraForm
    {
        #region ========== EVENTS ==========

        /// <summary>
        /// Event được trigger khi lưu thành công, trả về DTO đã được cập nhật
        /// </summary>
        public event Action<ApplicationUserDto> UserSaved;

        #endregion

        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// Business Logic Layer cho người dùng ứng dụng
        /// </summary>
        private readonly ApplicationUserBll _applicationUserBll;

        /// <summary>
        /// ID người dùng được chọn
        /// </summary>
        private readonly Guid _userId;

        /// <summary>
        /// Dữ liệu người dùng hiện tại
        /// </summary>
        private ApplicationUserDto _currentUser;

        /// <summary>
        /// Trạng thái chỉnh sửa
        /// </summary>
        private readonly bool _isEditMode;

        /// <summary>
        /// Trạng thái hiển thị mật khẩu (chỉ hiển thị khi tạo mới hoặc đổi mật khẩu)
        /// </summary>
        private bool _showPasswordFields;

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo form cho chế độ thêm mới/sửa người dùng.
        /// </summary>
        /// <param name="userId">ID người dùng (Guid.Empty cho thêm mới)</param>
        public FrmApplicationUserDtoAddEdit(Guid userId)
        {
            InitializeComponent();
            _applicationUserBll = new ApplicationUserBll();
            _userId = userId;
            _isEditMode = userId != Guid.Empty;
            _showPasswordFields = !_isEditMode; // Hiển thị mật khẩu khi tạo mới

            InitializeForm();

            SaveBarButtonItem.ItemClick += SaveBarButtonItem_ItemClick;
            CloseBarButtonItem.ItemClick += CloseBarButtonItem_ItemClick;
            ChangePasswordHyperlinkLabelControl.Click += ChangePasswordHyperlinkLabelControl_Click;
        }

        #endregion

        #region ========== KHỞI TẠO FORM ==========

        /// <summary>
        /// Khởi tạo form cho chế độ thêm mới/sửa.
        /// </summary>
        private void InitializeForm()
        {
            // Cấu hình form
            Text = _isEditMode ? "Chỉnh sửa người dùng" : "Thêm mới người dùng";

            // Load dữ liệu người dùng nếu là edit mode
            if (_isEditMode)
            {
                LoadUserData();
            }
            else
            {
                // Set default values for new user
                ActiveCheckEdit.EditValue = true;
            }

            // Setup password fields visibility
            SetupPasswordFieldsVisibility();

            // Setup validation
            SetupValidation();

            // Thiết lập SuperToolTip cho các controls
            SetupSuperToolTips();
        }

        /// <summary>
        /// Thiết lập hiển thị các trường mật khẩu
        /// </summary>
        private void SetupPasswordFieldsVisibility()
        {
            PasswordTextEdit.Visible = _showPasswordFields;
            RetypePasswordTextEdit.Visible = _showPasswordFields;
            ChangePasswordHyperlinkLabelControl.Visible = _isEditMode && !_showPasswordFields;
            
            // Ẩn/hiện layout items
            layoutControlItem2.Visibility = _showPasswordFields 
                ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always 
                : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem3.Visibility = _showPasswordFields 
                ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always 
                : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
            layoutControlItem4.Visibility = (_isEditMode && !_showPasswordFields) 
                ? DevExpress.XtraLayout.Utils.LayoutVisibility.Always 
                : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Load dữ liệu người dùng để chỉnh sửa
        /// </summary>
        private void LoadUserData()
        {
            try
            {
                var users = _applicationUserBll.GetAll();
                var user = users.FirstOrDefault(u => u.Id == _userId);
                
                if (user != null)
                {
                    _currentUser = user;
                    BindDataToControls();
                }
                else
                {
                    MsgBox.ShowError("Không tìm thấy thông tin người dùng.");
                    Close();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi tải dữ liệu người dùng: {ex.Message}");
            }
        }

        /// <summary>
        /// Bind dữ liệu người dùng vào các control
        /// </summary>
        private void BindDataToControls()
        {
            if (_currentUser == null) return;

            // Bind data to controls
            UserNameTextEdit.EditValue = _currentUser.UserName;
            ActiveCheckEdit.EditValue = _currentUser.Active;
            
            // Bind Employee nếu có
            if (_currentUser.EmployeeId.HasValue)
            {
                EmployeeSearchLookUpEdit.EditValue = _currentUser.EmployeeId.Value;
            }

            // Không hiển thị mật khẩu khi edit (chỉ hiển thị khi đổi mật khẩu)
            PasswordTextEdit.EditValue = string.Empty;
            RetypePasswordTextEdit.EditValue = string.Empty;
        }

        #endregion

        #region ========== SỰ KIỆN FORM ==========

        /// <summary>
        /// Xử lý sự kiện click button Lưu
        /// </summary>
        private async void SaveBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (!ValidateForm())
                    return;

                // Lưu dữ liệu với waiting form
                await ExecuteWithWaitingFormAsync(async () =>
                {
                    await SaveUserAsync();
                });

                // Thông báo thành công và đóng form
                MsgBox.ShowSuccess(_isEditMode
                    ? "Cập nhật người dùng thành công!"
                    : "Thêm mới người dùng thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                ShowError(ex, "Lưu dữ liệu người dùng");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click button Đóng
        /// </summary>
        private void CloseBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Xử lý sự kiện click link "Đổi mật khẩu"
        /// </summary>
        private void ChangePasswordHyperlinkLabelControl_Click(object sender, EventArgs e)
        {
            _showPasswordFields = true;
            SetupPasswordFieldsVisibility();
            PasswordTextEdit.Focus();
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thu thập dữ liệu từ các control để tạo DTO lưu xuống DB.
        /// </summary>
        private ApplicationUserDto GetDataFromControls()
        {
            var dto = new ApplicationUserDto
            {
                Id = _currentUser?.Id ?? Guid.Empty,
                UserName = UserNameTextEdit.Text?.Trim(),
                Active = (bool)(ActiveCheckEdit.EditValue ?? true),
                EmployeeId = EmployeeSearchLookUpEdit.EditValue as Guid?
            };

            // Xử lý mật khẩu
            if (_showPasswordFields)
            {
                var password = PasswordTextEdit.Text?.Trim();
                if (!string.IsNullOrEmpty(password))
                {
                    // TODO: Hash password trước khi lưu
                    // Hiện tại lưu plain text, cần implement password hashing
                    dto.HashPassword = password;
                }
                else if (_currentUser != null)
                {
                    // Giữ nguyên mật khẩu cũ nếu không nhập mới
                    dto.HashPassword = _currentUser.HashPassword;
                }
            }
            else if (_currentUser != null)
            {
                // Giữ nguyên mật khẩu cũ
                dto.HashPassword = _currentUser.HashPassword;
            }

            return dto;
        }

        /// <summary>
        /// Lưu dữ liệu người dùng và trigger event UserSaved
        /// </summary>
        private async Task SaveUserAsync()
        {
            // Bước 1: Thu thập dữ liệu từ form và build DTO
            var userDto = GetDataFromControls();

            // Bước 2: Lưu DTO qua BLL
            ApplicationUserDto savedDto;
            if (_isEditMode)
            {
                savedDto = await Task.Run(() => _applicationUserBll.Update(userDto));
            }
            else
            {
                savedDto = await Task.Run(() => _applicationUserBll.Create(userDto));
            }

            // Bước 3: Trigger event để form cha có thể update datasource
            if (savedDto != null)
            {
                UserSaved?.Invoke(savedDto);
            }
        }

        /// <summary>
        /// Kiểm tra hợp lệ dữ liệu bắt buộc sử dụng dxErrorProvider1
        /// </summary>
        private bool ValidateForm()
        {
            dxErrorProvider1.ClearErrors();

            // UserName bắt buộc
            if (string.IsNullOrWhiteSpace(UserNameTextEdit?.Text))
            {
                dxErrorProvider1.SetError(UserNameTextEdit, "Tên đăng nhập không được để trống",
                    ErrorType.Critical);
                UserNameTextEdit?.Focus();
                return false;
            }

            // Validate độ dài UserName
            if (UserNameTextEdit.Text.Trim().Length > 50)
            {
                dxErrorProvider1.SetError(UserNameTextEdit, "Tên đăng nhập không được vượt quá 50 ký tự",
                    ErrorType.Critical);
                UserNameTextEdit?.Focus();
                return false;
            }

            // Validate trùng lặp UserName - chỉ kiểm tra khi tạo mới hoặc đổi tên
            var userName = UserNameTextEdit.Text.Trim();
            var existing = _applicationUserBll.GetAll()
                .FirstOrDefault(u => u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase) 
                    && (!_isEditMode || u.Id != _userId));

            if (existing != null)
            {
                dxErrorProvider1.SetError(UserNameTextEdit, $"Tên đăng nhập '{userName}' đã tồn tại trong hệ thống",
                    ErrorType.Critical);
                UserNameTextEdit?.Focus();
                return false;
            }

            // Validate mật khẩu nếu hiển thị
            if (_showPasswordFields)
            {
                var password = PasswordTextEdit.Text?.Trim();
                var retypePassword = RetypePasswordTextEdit.Text?.Trim();

                // Mật khẩu bắt buộc khi tạo mới
                if (!_isEditMode && string.IsNullOrWhiteSpace(password))
                {
                    dxErrorProvider1.SetError(PasswordTextEdit, "Mật khẩu không được để trống",
                        ErrorType.Critical);
                    PasswordTextEdit?.Focus();
                    return false;
                }

                // Kiểm tra độ dài mật khẩu
                if (!string.IsNullOrWhiteSpace(password) && password.Length > 500)
                {
                    dxErrorProvider1.SetError(PasswordTextEdit, "Mật khẩu không được vượt quá 500 ký tự",
                        ErrorType.Critical);
                    PasswordTextEdit?.Focus();
                    return false;
                }

                // Kiểm tra mật khẩu nhập lại khớp
                if (!string.IsNullOrWhiteSpace(password) && password != retypePassword)
                {
                    dxErrorProvider1.SetError(RetypePasswordTextEdit, "Mật khẩu nhập lại không khớp",
                        ErrorType.Critical);
                    RetypePasswordTextEdit?.Focus();
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region ========== TIỆN ÍCH ==========

        /// <summary>
        /// Setup validation cho form
        /// </summary>
        private void SetupValidation()
        {
            // Đánh dấu các trường bắt buộc theo DataAnnotations của DTO
            RequiredFieldHelper.MarkRequiredFields(this, typeof(ApplicationUserDto));
        }

        /// <summary>
        /// Thực thi async operation với waiting form (hiển thị splash screen)
        /// </summary>
        /// <param name="operation">Operation async cần thực thi</param>
        private async Task ExecuteWithWaitingFormAsync(Func<Task> operation)
        {
            try
            {
                // Hiển thị waiting form
                SplashScreenManager.ShowForm(typeof(WaitForm1));

                // Thực hiện operation
                await operation();
            }
            finally
            {
                // Đóng waiting form
                SplashScreenManager.CloseForm();
            }
        }

        #endregion

        #region ========== TIỆN ÍCH HỖ TRỢ ==========

        /// <summary>
        /// Hiển thị lỗi qua XtraMessageBox với thông báo tiếng Việt
        /// </summary>
        /// <param name="ex">Exception cần hiển thị</param>
        /// <param name="action">Tên hành động đang thực hiện khi xảy ra lỗi</param>
        private void ShowError(Exception ex, string action)
        {
            MsgBox.ShowException(ex, $"Lỗi {action}");
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho các controls trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                if (UserNameTextEdit != null)
                {
                    var userNameTip = _isEditMode
                        ? "Tên đăng nhập của người dùng. Có thể thay đổi nhưng phải đảm bảo không trùng với tên đăng nhập khác."
                        : "Nhập tên đăng nhập cho người dùng mới (tối đa 50 ký tự). Trường này là bắt buộc và phải duy nhất trong hệ thống.";
                    
                    SuperToolTipHelper.SetTextEditSuperTip(
                        UserNameTextEdit,
                        title: "<b><color=DarkBlue>👤 Tên đăng nhập</color></b>",
                        content: userNameTip
                    );
                }

                if (PasswordTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        PasswordTextEdit,
                        title: "<b><color=DarkBlue>🔒 Mật khẩu</color></b>",
                        content: "Nhập mật khẩu cho người dùng (tối đa 500 ký tự). Mật khẩu sẽ được hash trước khi lưu vào hệ thống."
                    );
                }

                if (RetypePasswordTextEdit != null)
                {
                    SuperToolTipHelper.SetTextEditSuperTip(
                        RetypePasswordTextEdit,
                        title: "<b><color=DarkBlue>🔒 Nhập lại mật khẩu</color></b>",
                        content: "Nhập lại mật khẩu để xác nhận. Mật khẩu nhập lại phải khớp với mật khẩu đã nhập."
                    );
                }

                if (EmployeeSearchLookUpEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        EmployeeSearchLookUpEdit,
                        title: "<b><color=DarkBlue>👔 Nhân viên</color></b>",
                        content: "Chọn nhân viên liên kết với tài khoản người dùng này (tùy chọn). Mỗi nhân viên chỉ có thể liên kết với một tài khoản người dùng."
                    );
                }

                if (ActiveCheckEdit != null)
                {
                    SuperToolTipHelper.SetBaseEditSuperTip(
                        ActiveCheckEdit,
                        title: "<b><color=DarkBlue>✅ Đang hoạt động</color></b>",
                        content: "Đánh dấu tài khoản người dùng này có đang được kích hoạt hay không. Tài khoản không hoạt động sẽ không thể đăng nhập."
                    );
                }
                 

                if (SaveBarButtonItem != null)
                {
                    var saveTip = _isEditMode
                        ? "Lưu các thay đổi thông tin người dùng vào hệ thống."
                        : "Lưu thông tin người dùng mới vào hệ thống.";
                    
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SaveBarButtonItem,
                        title: "<b><color=Blue>💾 Lưu</color></b>",
                        content: saveTip
                    );
                }

                if (CloseBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CloseBarButtonItem,
                        title: "<b><color=Red>❌ Đóng</color></b>",
                        content: "Đóng form mà không lưu thay đổi."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
            }
        }

        #endregion
    }
}
