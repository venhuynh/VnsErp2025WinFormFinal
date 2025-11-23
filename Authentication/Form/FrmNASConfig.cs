using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Helpers;
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

        #endregion

        #region Constructor

        public FrmNASConfig()
        {
            InitializeComponent();
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
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
                // Load dữ liệu từ App.config
                TaiDuLieuTuAppConfig();

                // Hiển thị thông tin hiện tại
                HienThiThongTinHienTai();
            }
            catch (Exception ex)
            {
                _logger.Error($"Lỗi khởi tạo form FrmNASConfig: {ex.Message}", ex);
                MsgBox.ShowException(ex, "Lỗi khởi tạo form");
            }
        }

        /// <summary>
        /// Tải dữ liệu từ App.config
        /// </summary>
        private void TaiDuLieuTuAppConfig()
        {
            try
            {
                // Load các giá trị từ App.config
                StorageTypeComboBoxEdit.EditValue = AppConfigHelper.GetAppSetting("ImageStorage.StorageType", "NAS");
                ServerNameTextEdit.EditValue = AppConfigHelper.GetAppSetting("ImageStorage.NAS.ServerName");
                ShareNameTextEdit.EditValue = AppConfigHelper.GetAppSetting("ImageStorage.NAS.ShareName", "ERP_Images");
                BasePathTextEdit.EditValue = AppConfigHelper.GetAppSetting("ImageStorage.NAS.BasePath");
                UsernameTextEdit.EditValue = AppConfigHelper.GetAppSetting("ImageStorage.NAS.Username");
                PasswordTextEdit.EditValue = AppConfigHelper.GetAppSetting("ImageStorage.NAS.Password");
                ProtocolComboBoxEdit.EditValue = AppConfigHelper.GetAppSetting("ImageStorage.NAS.Protocol", "SMB");
                
                var timeout = AppConfigHelper.GetAppSetting("ImageStorage.NAS.ConnectionTimeout", "30");
                if (int.TryParse(timeout, out int timeoutValue))
                    ConnectionTimeoutSpinEdit.EditValue = timeoutValue;
                else
                    ConnectionTimeoutSpinEdit.EditValue = 30;

                var retry = AppConfigHelper.GetAppSetting("ImageStorage.NAS.RetryAttempts", "3");
                if (int.TryParse(retry, out int retryValue))
                    RetryAttemptsSpinEdit.EditValue = retryValue;
                else
                    RetryAttemptsSpinEdit.EditValue = 3;

                var enableCache = AppConfigHelper.GetAppSetting("ImageStorage.NAS.EnableNASCache", "false");
                EnableCacheCheckEdit.Checked = bool.TryParse(enableCache, out bool cacheValue) && cacheValue;

                var cacheSize = AppConfigHelper.GetAppSetting("ImageStorage.NAS.NASCacheSize", "500");
                if (int.TryParse(cacheSize, out int cacheSizeValue))
                    CacheSizeSpinEdit.EditValue = cacheSizeValue;
                else
                    CacheSizeSpinEdit.EditValue = 500;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi tải dữ liệu từ App.config");
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

                // Thông báo thành công
                MsgBox.ShowSuccess("Cấu hình NAS đã được lưu thành công!");
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
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
                    MsgBox.ShowSuccess("Kết nối NAS thành công!", "Thành công");
                }
                else
                {
                    MsgBox.ShowError("Không thể kết nối đến NAS.\nVui lòng kiểm tra lại thông tin cấu hình.", "Lỗi kết nối");
                }
            }
            catch (Exception ex)
            {
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
                        // Nếu có username và password, thử map network drive tạm thời
                        if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                        {
                            // Sử dụng WNetAddConnection2 để map network drive
                            // Note: Cần thêm using System.Runtime.InteropServices;
                            // Hoặc sử dụng cách đơn giản hơn: kiểm tra thư mục có accessible không
                        }

                        // Kiểm tra thư mục có tồn tại và có quyền truy cập không
                        if (!Directory.Exists(fullPath)) return false;
                        // Thử tạo một file test tạm thời
                        var testFile = Path.Combine(fullPath, $"test_{Guid.NewGuid():N}.tmp");
                        try
                        {
                            File.WriteAllText(testFile, "test");
                            File.Delete(testFile);
                            return true;
                        }
                        catch
                        {
                            return false;
                        }

                        return false;
                    }
                    catch
                    {
                        return false;
                    }
                });
            }
            catch
            {
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
        /// Lưu cấu hình vào App.config
        /// </summary>
        private void LuuCauHinh()
        {
            try
            {
                var settings = new Dictionary<string, string>
                {
                    { "ImageStorage.StorageType", StorageTypeComboBoxEdit.Text?.Trim() ?? "NAS" },
                    { "ImageStorage.NAS.ServerName", ServerNameTextEdit.Text?.Trim() ?? "" },
                    { "ImageStorage.NAS.ShareName", ShareNameTextEdit.Text?.Trim() ?? "ERP_Images" },
                    { "ImageStorage.NAS.BasePath", BasePathTextEdit.Text?.Trim() ?? "" },
                    { "ImageStorage.NAS.Username", UsernameTextEdit.Text?.Trim() ?? "" },
                    { "ImageStorage.NAS.Password", PasswordTextEdit.Text ?? "" },
                    { "ImageStorage.NAS.Protocol", ProtocolComboBoxEdit.Text?.Trim() ?? "SMB" },
                    { "ImageStorage.NAS.ConnectionTimeout", ConnectionTimeoutSpinEdit.Value.ToString() },
                    { "ImageStorage.NAS.RetryAttempts", RetryAttemptsSpinEdit.Value.ToString() },
                    { "ImageStorage.NAS.EnableNASCache", EnableCacheCheckEdit.Checked.ToString() },
                    { "ImageStorage.NAS.NASCacheSize", CacheSizeSpinEdit.Value.ToString() }
                };

                AppConfigHelper.UpdateAppSettings(settings);

            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể lưu cấu hình: {ex.Message}", ex);
            }
        }

        #endregion
    }
}

