using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Common;
using Common.Utils;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

// ReSharper disable UnusedType.Global
// ReSharper disable InconsistentNaming

namespace Authentication.Form
{
    /// <summary>
    /// Form cấu hình NAS cho Image Storage
    /// </summary>
    public partial class FrmNASConfig : DevExpress.XtraEditors.XtraForm
    {
        #region Fields

        private readonly ILogger _logger;
        private readonly SettingBll _settingBll;

        #endregion

        #region Constructor

        public FrmNASConfig()
        {
            InitializeComponent();
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
            _settingBll = new SettingBll();
            KhoiTaoForm();
        }

        #endregion

        #region Methods

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void KhoiTaoForm()
        {
            try
            {
                // Không load dữ liệu từ database trong constructor để tránh lỗi khi database chưa được cấu hình
                // Dữ liệu sẽ được load trong Load event
                
                // Đăng ký Load event để load dữ liệu khi form hiển thị
                this.Load += FrmNASConfig_Load;
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khởi tạo form FrmNASConfig: {ex.Message}", ex);
                MsgBox.ShowException(ex, "Lỗi khởi tạo form");
            }
        }

        /// <summary>
        /// Xử lý sự kiện Load form - Load dữ liệu từ database khi form hiển thị
        /// </summary>
        private void FrmNASConfig_Load(object sender, EventArgs e)
        {
            try
            {
                // Load dữ liệu từ Database (bảng Setting)
                TaiDuLieuTuAppConfig();

                // Hiển thị thông tin hiện tại
                HienThiThongTinHienTai();
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi load dữ liệu form FrmNASConfig: {ex.Message}", ex);
                // Không hiển thị message box để tránh block form, chỉ log lỗi
                // Form vẫn có thể được sử dụng với giá trị mặc định
            }
        }

        /// <summary>
        /// Tải dữ liệu từ Database (bảng Setting)
        /// </summary>
        private void TaiDuLieuTuAppConfig()
        {
            try
            {
                _logger.Debug("Bắt đầu tải dữ liệu từ Database");

                // Load các giá trị từ Database
                var storageType = _settingBll.GetValue("NAS", "StorageType", "NAS");
                var serverName = _settingBll.GetDecryptedValue("NAS", "ServerName", "");
                var shareName = _settingBll.GetDecryptedValue("NAS", "ShareName", "ERP_Images");
                var basePath = _settingBll.GetDecryptedValue("NAS", "BasePath", "");
                var username = _settingBll.GetDecryptedValue("NAS", "Username", "");
                var password = _settingBll.GetDecryptedValue("NAS", "Password", "");
                var protocol = _settingBll.GetValue("NAS", "Protocol", "SMB");
                var timeout = _settingBll.GetValue("NAS", "ConnectionTimeout", "30");
                var retry = _settingBll.GetValue("NAS", "RetryAttempts", "3");
                var enableCache = _settingBll.GetValue("NAS", "EnableCache", "false");
                var cacheSize = _settingBll.GetValue("NAS", "CacheSize", "500");

                _logger.Debug($"Đã đọc từ Database - StorageType={storageType}, ServerName={serverName}, ShareName={shareName}, Username={username}, Password={(string.IsNullOrEmpty(password) ? "(empty)" : "***")}");

                // Gán giá trị vào controls
                StorageTypeComboBoxEdit.EditValue = storageType;
                ServerNameTextEdit.EditValue = serverName;
                ShareNameTextEdit.EditValue = shareName;
                BasePathTextEdit.EditValue = basePath;
                UsernameTextEdit.EditValue = username;
                PasswordTextEdit.EditValue = password;
                ProtocolComboBoxEdit.EditValue = protocol;
                
                if (int.TryParse(timeout, out int timeoutValue))
                    ConnectionTimeoutSpinEdit.EditValue = timeoutValue;
                else
                    ConnectionTimeoutSpinEdit.EditValue = 30;

                if (int.TryParse(retry, out int retryValue))
                    RetryAttemptsSpinEdit.EditValue = retryValue;
                else
                    RetryAttemptsSpinEdit.EditValue = 3;

                EnableCacheCheckEdit.Checked = bool.TryParse(enableCache, out bool cacheValue) && cacheValue;

                if (int.TryParse(cacheSize, out int cacheSizeValue))
                    CacheSizeSpinEdit.EditValue = cacheSizeValue;
                else
                    CacheSizeSpinEdit.EditValue = 500;

                _logger.Info($"Đã tải dữ liệu từ Database thành công - StorageType={storageType}, ServerName={serverName}, ShareName={shareName}, Username={username}");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi tải dữ liệu từ Database: {ex.Message}", ex);
                MsgBox.ShowException(ex, "Lỗi tải dữ liệu từ Database");
            }
        }

        /// <summary>
        /// Hiển thị thông tin hiện tại (đã được load trong TaiDuLieuTuAppConfig)
        /// </summary>
        private void HienThiThongTinHienTai()
        {
            // Dữ liệu đã được gán trực tiếp vào controls trong TaiDuLieuTuAppConfig
            // Method này có thể được mở rộng nếu cần xử lý thêm
        }

        /// <summary>
        /// Xử lý sự kiện click nút OK
        /// </summary>
        private void OKSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu
                if (!KiemTraDuLieuHopLe())
                {
                    return;
                }

                // Lưu cấu hình
                LuuCauHinh();

                _logger.Info("Đã lưu cấu hình NAS thành công");
                // Thông báo thành công
                MsgBox.ShowSuccess("Cấu hình NAS đã được lưu thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi lưu cấu hình NAS: {ex.Message}", ex);
                MsgBox.ShowException(ex, "Lỗi lưu cấu hình");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Cancel
        /// </summary>
        private void CancelSimpleButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        /// <summary>
        /// Xử lý sự kiện click nút Test Connection
        /// </summary>
        private async void TestConnectionSimpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu trước khi test
                if (!KiemTraDuLieuHopLe())
                {
                    return;
                }

                TestConnectionSimpleButton.Enabled = false;
                TestConnectionSimpleButton.Text = @"Đang kiểm tra...";

                // Test kết nối NAS
                var result = await TestConnectionAsync();

                TestConnectionSimpleButton.Enabled = true;
                TestConnectionSimpleButton.Text = @"Kiểm tra kết nối";

                if (result)
                {
                    _logger.Info("Kiểm tra kết nối NAS thành công");
                    MsgBox.ShowSuccess("Kết nối NAS thành công!", "Thành công");
                }
                else
                {
                    _logger.Warning("Không thể kết nối đến NAS");
                    MsgBox.ShowError("Không thể kết nối đến NAS.\nVui lòng kiểm tra lại thông tin cấu hình.", "Lỗi kết nối");
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi kiểm tra kết nối NAS: {ex.Message}", ex);
                TestConnectionSimpleButton.Enabled = true;
                TestConnectionSimpleButton.Text = @"Kiểm tra kết nối";
                MsgBox.ShowException(ex, @"Lỗi kiểm tra kết nối");
            }
        }

        /// <summary>
        /// Kiểm tra kết nối NAS
        /// </summary>
        private async Task<bool> TestConnectionAsync()
        {
            try
            {
                // Lấy thông tin từ form
                var serverName = ServerNameTextEdit.Text?.Trim();
                var shareName = ShareNameTextEdit.Text?.Trim();
                var basePath = BasePathTextEdit.Text?.Trim();
                var username = UsernameTextEdit.Text?.Trim();
                var password = PasswordTextEdit.Text;

                // Tạo đường dẫn đầy đủ
                string fullPath;
                if (!string.IsNullOrEmpty(basePath))
                {
                    fullPath = basePath;
                }
                else if (!string.IsNullOrEmpty(serverName) && !string.IsNullOrEmpty(shareName))
                {
                    fullPath = $"{serverName}\\{shareName}";
                }
                else
                {
                    return false;
                }

                // Test kết nối bằng cách kiểm tra thư mục có tồn tại không
                return await Task.Run(() =>
                {
                    try
                    {
                        _logger.Debug($"Bắt đầu kiểm tra kết nối NAS, FullPath={fullPath}");

                        // Nếu có username và password, thử map network drive tạm thời
                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        {
                            // Sử dụng WNetAddConnection2 để map network drive
                            // Note: Cần thêm using System.Runtime.InteropServices;
                            // Hoặc sử dụng cách đơn giản hơn: kiểm tra thư mục có accessible không
                            _logger.Debug($"Sử dụng credentials: Username={username}");
                        }

                        // Kiểm tra thư mục có tồn tại và có quyền truy cập không
                        if (!Directory.Exists(fullPath))
                        {
                            _logger.Warning($"Thư mục không tồn tại: {fullPath}");
                            return false;
                        }

                        // Thử tạo một file test tạm thời
                        var testFile = Path.Combine(fullPath, $"test_{Guid.NewGuid():N}.tmp");
                        try
                        {
                            File.WriteAllText(testFile, "test");
                            File.Delete(testFile);
                            _logger.Info($"Kiểm tra kết nối NAS thành công, FullPath={fullPath}");
                            return true;
                        }
                        catch (Exception ex)
                        {
                            _logger.Warning($"Không thể ghi file test vào NAS: {ex.Message}");
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khi kiểm tra kết nối NAS: {ex.Message}", ex);
                        return false;
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi ngoại lệ khi kiểm tra kết nối NAS: {ex.Message}", ex);
                return false;
            }
        }

        /// <summary>
        /// Kiểm tra dữ liệu đầu vào có hợp lệ không
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        private bool KiemTraDuLieuHopLe()
        {
            // Xóa lỗi cũ
            dxErrorProvider1.ClearErrors();

            bool isValid = true;

            // StorageType không được để trống
            if (string.IsNullOrEmpty(StorageTypeComboBoxEdit.Text))
            {
                dxErrorProvider1.SetError(StorageTypeComboBoxEdit, "Loại storage không được để trống");
                isValid = false;
            }

            // Nếu StorageType là NAS, kiểm tra các field NAS
            if (StorageTypeComboBoxEdit.Text == "NAS")
            {
                // ServerName không được để trống
                if (string.IsNullOrEmpty(ServerNameTextEdit.Text?.Trim()))
                {
                    dxErrorProvider1.SetError(ServerNameTextEdit, "Tên server NAS không được để trống");
                    if (isValid)
                    {
                        ServerNameTextEdit.Focus();
                        isValid = false;
                    }
                }

                // ShareName không được để trống
                if (string.IsNullOrEmpty(ShareNameTextEdit.Text?.Trim()))
                {
                    dxErrorProvider1.SetError(ShareNameTextEdit, "Tên share không được để trống");
                    if (isValid)
                    {
                        ShareNameTextEdit.Focus();
                        isValid = false;
                    }
                }

                // Username không được để trống
                if (string.IsNullOrEmpty(UsernameTextEdit.Text?.Trim()))
                {
                    dxErrorProvider1.SetError(UsernameTextEdit, "Tên đăng nhập không được để trống");
                    if (isValid)
                    {
                        UsernameTextEdit.Focus();
                        isValid = false;
                    }
                }

                // Password không được để trống
                if (string.IsNullOrEmpty(PasswordTextEdit.Text))
                {
                    dxErrorProvider1.SetError(PasswordTextEdit, "Mật khẩu không được để trống");
                    if (isValid)
                    {
                        PasswordTextEdit.Focus();
                        isValid = false;
                    }
                }

                // ConnectionTimeout phải > 0
                if (ConnectionTimeoutSpinEdit.Value <= 0)
                {
                    dxErrorProvider1.SetError(ConnectionTimeoutSpinEdit, "Timeout kết nối phải lớn hơn 0");
                    if (isValid)
                    {
                        ConnectionTimeoutSpinEdit.Focus();
                        isValid = false;
                    }
                }

                // RetryAttempts phải >= 0
                if (RetryAttemptsSpinEdit.Value < 0)
                {
                    dxErrorProvider1.SetError(RetryAttemptsSpinEdit, "Số lần retry phải >= 0");
                    if (isValid)
                    {
                        RetryAttemptsSpinEdit.Focus();
                        isValid = false;
                    }
                }
            }

            return isValid;
        }

        /// <summary>
        /// Lưu cấu hình vào Database (bảng Setting)
        /// </summary>
        private void LuuCauHinh()
        {
            try
            {
                // Lấy username hiện tại (có thể từ ApplicationUser hoặc Environment)
                var updatedBy = Environment.UserName ?? "System";

                var nasSettings = new Dictionary<string, string>
                {
                    { "StorageType", StorageTypeComboBoxEdit.Text?.Trim() ?? "NAS" },
                    { "ServerName", ServerNameTextEdit.Text?.Trim() ?? "" },
                    { "ShareName", ShareNameTextEdit.Text?.Trim() ?? "ERP_Images" },
                    { "BasePath", BasePathTextEdit.Text?.Trim() ?? "" },
                    { "Username", UsernameTextEdit.Text?.Trim() ?? "" },
                    { "Password", PasswordTextEdit.Text ?? "" },
                    { "Protocol", ProtocolComboBoxEdit.Text?.Trim() ?? "SMB" },
                    { "ConnectionTimeout", ConnectionTimeoutSpinEdit.Value.ToString() },
                    { "RetryAttempts", RetryAttemptsSpinEdit.Value.ToString() },
                    { "EnableCache", EnableCacheCheckEdit.Checked.ToString() },
                    { "CacheSize", CacheSizeSpinEdit.Value.ToString() }
                };

                // Lưu vào database
                _settingBll.SaveNASSettings(nasSettings, updatedBy);

                _logger.Info("Đã cập nhật cấu hình NAS vào Database");
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khi lưu cấu hình NAS vào Database: {ex.Message}", ex);
                throw new Exception($"Không thể lưu cấu hình: {ex.Message}", ex);
            }
        }

        #endregion
    }
}

