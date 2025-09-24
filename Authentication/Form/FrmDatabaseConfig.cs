using Bll.Utils;
using Dal.Connection;
using DevExpress.XtraEditors.DXErrorProvider;
using System;
using System.Windows.Forms;
using ValidationHelper = Bll.Validation.ValidationHelper;
using System.Net;
using System.Net.Sockets;

namespace Authentication.Form
{
    public partial class FrmDatabaseConfig : DevExpress.XtraEditors.XtraForm
    {
        #region thuocTinhDonGian

        public DatabaseConfig _databaseConfig;
        private ConnectionManager _connectionManager;

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
        /// Tải dữ liệu từ Settings
        /// </summary>
        private void TaiDuLieuTuSettings()
        {
            try
            {
                var settings = Properties.Settings.Default;
                
                // Cập nhật DatabaseConfig từ Settings
                _databaseConfig.ServerName = settings.DatabaseServer ?? "localhost";
                _databaseConfig.DatabaseName = settings.DatabaseName ?? "VnsErp2025";
                _databaseConfig.UserId = settings.DatabaseUserId ?? string.Empty;
                // Giải mã mật khẩu trước khi dùng
                _databaseConfig.Password = Dal.Connection.ConnectionStringHelper.DecodeConnectionString(settings.DatabasePassword ?? string.Empty);
                // Luôn dùng SQL Authentication
                _databaseConfig.UseIntegratedSecurity = false;
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi tải dữ liệu từ Settings");
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
                    MsgBox.ShowInfo("Kết nối cơ sở dữ liệu thành công!\nCấu hình đã được lưu.", "Thành công");
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
        /// Lưu cấu hình vào file config
        /// </summary>
        private void LuuCauHinh()
        {
            try
            {
                // Cập nhật App.config với thông tin mới
                CapNhatAppConfig();
                
                // Có thể thêm logic lưu vào file cấu hình khác nếu cần
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi lưu cấu hình");
                throw;
            }
        }

        /// <summary>
        /// Cập nhật App.config với thông tin mới
        /// </summary>
        private void CapNhatAppConfig()
        {
            try
            {
                // Lưu vào Settings của project
                var settings = Properties.Settings.Default;
                
                // Cập nhật các giá trị
                settings.DatabaseServer = _databaseConfig.ServerName;
                settings.DatabaseName = _databaseConfig.DatabaseName;
                settings.DatabaseUserId = _databaseConfig.UserId;
                // Mã hóa mật khẩu trước khi lưu
                settings.DatabasePassword = Dal.Connection.ConnectionStringHelper.EncodeConnectionString(_databaseConfig.Password);
                settings.UseIntegratedSecurity = _databaseConfig.UseIntegratedSecurity;
                
                // Lưu settings
                settings.Save();
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể lưu cấu hình: {ex.Message}", ex);
            }
        }

        #endregion

        
    }

}