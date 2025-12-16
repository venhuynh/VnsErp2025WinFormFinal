using Dal.Connection;
using System;
using System.Windows.Forms;
using Common.Utils;
using Microsoft.Win32;
using Common.Appconfig;

namespace Authentication.Form
{
    public partial class FrmDatabaseConfig : DevExpress.XtraEditors.XtraForm
    {
        #region thuocTinhDonGian

        private DatabaseConfig _databaseConfig;
        private ConnectionManager _connectionManager;
        private const string REGISTRY_KEY = @"HKEY_CURRENT_USER\Software\Software\VietNhatSolutions\VnsErp2025";

        #endregion

        #region phuongThuc

        public FrmDatabaseConfig()
        {
            InitializeComponent();
            KhoiTaoForm();
        }

        /// <summary>
        /// Khởi tạo form
        /// </summary>
        private void KhoiTaoForm()
        {
            try
            {
                // Khởi tạo DatabaseConfig từ cấu hình hiện tại
                _databaseConfig = DatabaseConfig.Instance;
                
                // Load dữ liệu từ Settings
                TaiDuLieuTuSettings();
                
                // Hiển thị thông tin hiện tại vào các textbox
                HienThiThongTinHienTai();
                
                // Khởi tạo ConnectionManager
                _connectionManager = new ConnectionManager();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi khởi tạo form");
            }
        }

        /// <summary>
        /// Tải dữ liệu từ Registry
        /// </summary>
        private void TaiDuLieuTuSettings()
        {
            try
            {
                var dbConfig = GetMsSqlServerInfo();
                
                if (dbConfig != null)
                {
                    // Cập nhật DatabaseConfig từ Registry
                    _databaseConfig.ServerName = dbConfig.Dns ?? "localhost";
                    _databaseConfig.DatabaseName = dbConfig.Database ?? "VnsErp2025";
                    _databaseConfig.UserId = dbConfig.Username ?? string.Empty;
                    _databaseConfig.Password = dbConfig.Password ?? string.Empty;
                }
                else
                {
                    // Sử dụng giá trị mặc định nếu không tìm thấy trong Registry
                    _databaseConfig.ServerName = "localhost";
                    _databaseConfig.DatabaseName = "VnsErp2025";
                    _databaseConfig.UserId = string.Empty;
                    _databaseConfig.Password = string.Empty;
                }
                
                // Luôn dùng SQL Authentication
                _databaseConfig.UseIntegratedSecurity = false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi tải dữ liệu từ Registry");
            }
        }

        /// <summary>
        /// Hiển thị thông tin cấu hình hiện tại vào các textbox
        /// </summary>
        private void HienThiThongTinHienTai()
        {
            try
            {
                // Gán dữ liệu vào BindingSource
                databaseConfigBindingSource.DataSource = _databaseConfig;
                
                // Cập nhật các textbox với dữ liệu hiện tại
                ServerNameTextEdit.EditValue = _databaseConfig.ServerName;
                DatabaseNameTextEdit.EditValue = _databaseConfig.DatabaseName;
                UserIdTextEdit.EditValue = _databaseConfig.UserId;
                PasswordTextEdit.EditValue = _databaseConfig.Password;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị thông tin");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút OK
        /// </summary>
        private void OKSmpleButton_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate dữ liệu đầu vào bằng dxValidationProvider
                if (!KiemTraDuLieuHopLeBangValidationProvider())
                {
                    return;
                }

                // Cập nhật thông tin từ form vào DatabaseConfig
                CapNhatThongTinTuForm();

                // Kiểm tra kết nối
                if (KiemTraKetNoi())
                {
                    // Lưu cấu hình
                    LuuCauHinh();
                    
                    // Thông báo thành công và đóng form
                    MsgBox.ShowSuccess("Kết nối cơ sở dữ liệu thành công!\nCấu hình đã được lưu.", "Thành công");
                    DialogResult = DialogResult.OK;
                    Close();
                }
                else
                {
                    MsgBox.ShowError("Không thể kết nối đến cơ sở dữ liệu.\nVui lòng kiểm tra lại thông tin kết nối.", "Lỗi kết nối");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi xử lý");
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
        /// Kiểm tra dữ liệu đầu vào có hợp lệ không bằng dxValidationProvider
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        private bool KiemTraDuLieuHopLeBangValidationProvider()
        {
            //Xóa lỗi cũ
            dxErrorProvider1.ClearErrors();

            //ServerNameTextEdit không được để trống
            if (string.IsNullOrEmpty(ServerNameTextEdit.Text))
            {
                dxErrorProvider1.SetError(ServerNameTextEdit, "Tên máy chủ không được để trống");

                //set focus vào control lỗi
                ServerNameTextEdit.Focus();
                
                return false;
            }

            //DatabaseNameTextEdit không được để trống
            if (string.IsNullOrEmpty(DatabaseNameTextEdit.Text))
            {
                dxErrorProvider1.SetError(DatabaseNameTextEdit, "Tên cơ sở dữ liệu không được để trống");
                DatabaseNameTextEdit.Focus();
                return false;
            }

            //UserIdTextEdit không được để trống
            if (string.IsNullOrEmpty(UserIdTextEdit.Text))
            {
                dxErrorProvider1.SetError(UserIdTextEdit, "Tên đăng nhập không được để trống");
                UserIdTextEdit.Focus();
                return false;
            }

            // PasswordTextEdit không được để trống
            if (string.IsNullOrEmpty(PasswordTextEdit.Text))
            {
                dxErrorProvider1.SetError(PasswordTextEdit, "Mật khẩu không được để trống");
                PasswordTextEdit.Focus();
                return false;
            }

            return true;
        }


        /// <summary>
        /// Cập nhật thông tin từ form vào DatabaseConfig
        /// </summary>
        private void CapNhatThongTinTuForm()
        {
            _databaseConfig.ServerName = ServerNameTextEdit.Text.Trim();
            _databaseConfig.DatabaseName = DatabaseNameTextEdit.Text.Trim();
            _databaseConfig.UserId = UserIdTextEdit.Text.Trim();
            _databaseConfig.Password = PasswordTextEdit.Text;
            
            // Luôn dùng SQL Authentication
            _databaseConfig.UseIntegratedSecurity = false;
        }

        /// <summary>
        /// Kiểm tra kết nối cơ sở dữ liệu
        /// </summary>
        /// <returns>True nếu kết nối thành công</returns>
        private bool KiemTraKetNoi()
        {
            try
            {
                // Lấy trực tiếp thông số từ các input hiện tại
                var server = ServerNameTextEdit.Text?.Trim();
                var database = DatabaseNameTextEdit.Text?.Trim();
                var userId = UserIdTextEdit.Text?.Trim();
                var password = PasswordTextEdit.Text; // không Trim để tránh cắt khoảng trắng hợp lệ

                // Luôn dùng SQL Authentication theo yêu cầu
                var connectionString = Dal.Connection.ConnectionStringHelper.BuildDetailedConnectionString(
                    server,
                    database,
                    integratedSecurity: false,
                    userId: userId,
                    password: password,
                    timeout: 15,
                    commandTimeout: 30,
                    pooling: true,
                    minPoolSize: 1,
                    maxPoolSize: 100
                );

                // Thiết lập connection string mới cho ConnectionManager và test kết nối
                _connectionManager.SetConnectionString(connectionString);
                return _connectionManager.TestConnection();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi kiểm tra kết nối");
                return false;
            }
        }

        /// <summary>
        /// Lưu cấu hình vào Registry
        /// </summary>
        private void LuuCauHinh()
        {
            try
            {
                // Cập nhật Registry với thông tin mới
                CapNhatAppConfig();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi lưu cấu hình");
                throw;
            }
        }

        /// <summary>
        /// Cập nhật Registry với thông tin mới
        /// </summary>
        private void CapNhatAppConfig()
        {
            try
            {
                // Sử dụng method từ ConnectionStringHelper để lưu cấu hình
                Dal.Connection.ConnectionStringHelper.SetDbConfig(
                    _databaseConfig.ServerName,
                    _databaseConfig.DatabaseName,
                    _databaseConfig.UserId,
                    _databaseConfig.Password
                );
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể lưu cấu hình vào Registry: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Đọc thông tin kết nối của MS Sql Server từ Registry
        /// Tất cả thông tin được giải mã khi đọc từ Registry
        /// </summary>
        /// <returns>DbConfigInfo chứa thông tin kết nối, hoặc null nếu không tìm thấy</returns>
        private DbConfigInfo GetMsSqlServerInfo()
        {
            try
            {
                // Đọc và giải mã tất cả các trường từ Registry
                string encryptedDns = (string)Registry.GetValue(REGISTRY_KEY, "dns", "") ?? string.Empty;
                string encryptedDatabase = (string)Registry.GetValue(REGISTRY_KEY, "database", "") ?? string.Empty;
                string encryptedUsername = (string)Registry.GetValue(REGISTRY_KEY, "username", "") ?? string.Empty;
                string encryptedPassword = (string)Registry.GetValue(REGISTRY_KEY, "password", "") ?? string.Empty;

                var model = new DbConfigInfo
                {
                    Dns = string.Empty,
                    Database = string.Empty,
                    Username = string.Empty,
                    Password = string.Empty
                };

                // Giải mã dns
                if (!string.IsNullOrEmpty(encryptedDns))
                {
                    try
                    {
                        model.Dns = VntaCrypto.Decrypt(encryptedDns);
                    }
                    catch
                    {
                        // Nếu không thể giải mã, có thể là dữ liệu cũ chưa mã hóa, thử dùng trực tiếp
                        model.Dns = encryptedDns;
                    }
                }

                // Giải mã database
                if (!string.IsNullOrEmpty(encryptedDatabase))
                {
                    try
                    {
                        model.Database = VntaCrypto.Decrypt(encryptedDatabase);
                    }
                    catch
                    {
                        // Nếu không thể giải mã, có thể là dữ liệu cũ chưa mã hóa, thử dùng trực tiếp
                        model.Database = encryptedDatabase;
                    }
                }

                // Giải mã username
                if (!string.IsNullOrEmpty(encryptedUsername))
                {
                    try
                    {
                        model.Username = VntaCrypto.Decrypt(encryptedUsername);
                    }
                    catch
                    {
                        // Nếu không thể giải mã, có thể là dữ liệu cũ chưa mã hóa, thử dùng trực tiếp
                        model.Username = encryptedUsername;
                    }
                }

                // Giải mã password
                if (!string.IsNullOrEmpty(encryptedPassword))
                {
                    try
                    {
                        model.Password = VntaCrypto.Decrypt(encryptedPassword);
                    }
                    catch
                    {
                        // Nếu không thể giải mã, có thể là dữ liệu cũ chưa mã hóa, thử dùng trực tiếp
                        model.Password = encryptedPassword;
                    }
                }

                if (string.IsNullOrEmpty(model.Dns))
                    return null;

                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// Cấu trúc thông tin database config từ Registry
        /// </summary>
        private class DbConfigInfo
        {
            public string Dns { get; set; }
            public string Database { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        #endregion

        
    }

}