using Bll.Utils;
using Dal.Connection;
using Dal.DataContext;
using DevExpress.XtraBars;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace VnsErp2025.Form
{
    /// <summary>
    /// Form chính của ứng dụng VNS ERP 2025
    /// Hiển thị sau khi đăng nhập thành công
    /// </summary>
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region thuocTinhDonGian
        /// <summary>
        /// Thông tin user hiện tại đã đăng nhập
        /// </summary>
        private ApplicationUser _currentUser;

        /// <summary>
        /// Timer để refresh thông tin database định kỳ
        /// </summary>
        private System.Windows.Forms.Timer _dbRefreshTimer;
        #endregion

        #region Constructor
        /// <summary>
        /// Khởi tạo form chính
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            InitializeForm();
        }
        #endregion

        #region phuongThuc
        /// <summary>
        /// Khởi tạo form và các thành phần
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                // Lấy thông tin user hiện tại
                LoadCurrentUserInfo();
                
                // Thiết lập giao diện
                SetupFormProperties();
                
                // Thiết lập ribbon
                SetupRibbon();
                
                // Thiết lập status bar
                SetupStatusBar();
                
                // Thiết lập timer refresh database
                SetupDatabaseRefreshTimer();
                
                // Hiển thị thông báo chào mừng
                ShowWelcomeMessage();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi khởi tạo form chính");
            }
        }

        /// <summary>
        /// Lấy thông tin user hiện tại từ session
        /// </summary>
        private void LoadCurrentUserInfo()
        {
            try
            {
                // Lấy thông tin user từ ApplicationSystemUtils
                _currentUser = ApplicationSystemUtils.GetCurrentUser();
                
                // Nếu chưa có user, tạo user mẫu (tạm thời cho demo)
                if (_currentUser == null)
                {
                    _currentUser = new ApplicationUser
                    {
                        Id = Guid.NewGuid(),
                        UserName = "admin",
                        Active = true,
                    };
                    
                    // Lưu vào ApplicationSystemUtils
                    ApplicationSystemUtils.SetCurrentUser(_currentUser);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi tải thông tin user");
            }
        }

        /// <summary>
        /// Thiết lập các thuộc tính của form
        /// </summary>
        private void SetupFormProperties()
        {
            // Thiết lập thuộc tính form
            this.Text = $"VNS ERP 2025 - Chào mừng {_currentUser?.UserName ?? "User"}";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // Thiết lập icon (nếu có)
            // this.Icon = Properties.Resources.AppIcon;
        }

        /// <summary>
        /// Thiết lập ribbon control
        /// </summary>
        private void SetupRibbon()
        {
            // Thiết lập tên các trang ribbon
            ribbonPage1.Text = "Trang chủ";
            ribbonPageGroup1.Text = "Hệ thống";
             
        }
         

        /// <summary>
        /// Thiết lập status bar
        /// </summary>
        private void SetupStatusBar()
        {
            // Thêm thông tin user vào status bar
            var userInfo = new BarStaticItem();
            userInfo.Caption = $"User: {_currentUser?.UserName ?? "Unknown"} | Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            ribbonStatusBar.ItemLinks.Add(userInfo);

            // Thêm thông tin database vào status bar
            var dbInfo = GetDatabaseInfo();
            DBInfoBarStaticItem.Caption = dbInfo;

            // Thêm event handler cho database info item
            DBInfoBarStaticItem.ItemClick += BarStaticItem1_ItemClick;
        }

        /// <summary>
        /// Lấy thông tin database để hiển thị
        /// </summary>
        /// <returns>Thông tin database dạng string</returns>
        private string GetDatabaseInfo()
        {
            try
            {
                using (var connectionManager = new ConnectionManager())
                {
                    var connectionString = connectionManager.ConnectionString;
                    
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        return "DB: Không có thông tin kết nối";
                    }

                    // Parse connection string để lấy thông tin server và database
                    var builder = new SqlConnectionStringBuilder(connectionString);
                    
                    var serverName = builder.DataSource ?? "Unknown";
                    var databaseName = builder.InitialCatalog ?? "Unknown";
                    var connectionState = connectionManager.State.ToString();
                    
                    return $"DB: {serverName} | {databaseName}";
                }
            }
            catch (Exception ex)
            {
                return $"DB: Lỗi kết nối - {ex.Message}";
            }
        }

        /// <summary>
        /// Thiết lập timer để refresh thông tin database định kỳ
        /// </summary>
        private void SetupDatabaseRefreshTimer()
        {
            try
            {
                _dbRefreshTimer = new System.Windows.Forms.Timer();
                _dbRefreshTimer.Interval = 30000; // 30 giây
                _dbRefreshTimer.Tick += DbRefreshTimer_Tick;
                _dbRefreshTimer.Start();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi thiết lập timer refresh database");
            }
        }

        /// <summary>
        /// Event handler cho timer refresh database
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Event args</param>
        private void DbRefreshTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                RefreshDatabaseInfo();
            }
            catch (Exception ex)
            {
                // Không hiển thị lỗi cho timer để tránh spam
                System.Diagnostics.Debug.WriteLine($"Lỗi refresh database info: {ex.Message}");
            }
        }

        /// <summary>
        /// Hiển thị thông báo chào mừng
        /// </summary>
        private void ShowWelcomeMessage()
        {
            if (_currentUser != null)
            {
                MsgBox.ShowInfo($"Chào mừng {_currentUser.UserName} đến với VNS ERP 2025!", "Chào mừng");
            }
        }
        #endregion

        #region Event Handlers
        /// <summary>
        /// Xử lý sự kiện click nút đăng xuất
        /// </summary>
        private void BtnDangXuat_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var result = MsgBox.GetConfirmFromYesNoCancelDialog("Bạn có chắc chắn muốn đăng xuất?", "Xác nhận đăng xuất");
                if (result == DialogResult.Yes)
                {
                    // Xóa thông tin user hiện tại
                    _currentUser = null;
                    ApplicationSystemUtils.LogoutCurrentUser();
                    
                    // Đóng form chính
                    this.Hide();
                    
                    // Hiển thị lại form đăng nhập
                    using (var loginForm = new Authentication.Form.FrmLogin())
                    {
                        if (loginForm.ShowDialog() == DialogResult.OK)
                        {
                            // Đăng nhập lại thành công
                            LoadCurrentUserInfo();
                            SetupFormProperties();
                            SetupStatusBar();
                            this.Show();
                        }
                        else
                        {
                            // Thoát ứng dụng
                            Application.Exit();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi đăng xuất");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút thoát
        /// </summary>
        private void BtnThoat_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var result = MsgBox.GetConfirmFromYesNoCancelDialog("Bạn có chắc chắn muốn thoát ứng dụng?", "Xác nhận thoát");
                if (result == DialogResult.Yes)
                {
                    Application.Exit();
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi thoát ứng dụng");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút thông tin user
        /// </summary>
        private void BtnThongTinUser_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (_currentUser != null)
                {
                    var userInfo = $"Thông tin tài khoản:\n\n" +
                                   $"Tên đăng nhập: {_currentUser.UserName}\n" +
                                   $"ID: {_currentUser.Id}\n" +
                                   $"Trạng thái: {(_currentUser.Active ? "Hoạt động" : "Không hoạt động")}";
                    
                    MsgBox.ShowInfo(userInfo, "Thông tin tài khoản");
                }
                else
                {
                    MsgBox.ShowWarning("Không tìm thấy thông tin user.", "Lỗi");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị thông tin user");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click vào database info item
        /// </summary>
        /// <param name="sender">Control gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void BarStaticItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                // Hiển thị thông tin chi tiết về database
                ShowDatabaseDetails();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị thông tin database");
            }
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết về database
        /// </summary>
        private void ShowDatabaseDetails()
        {
            try
            {
                using (var connectionManager = new ConnectionManager())
                {
                    var connectionString = connectionManager.ConnectionString;
                    
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        MsgBox.ShowWarning("Không có thông tin kết nối database.", "Thông tin Database");
                        return;
                    }

                    var builder = new SqlConnectionStringBuilder(connectionString);
                    
                    var details = $"Thông tin Database:\n\n" +
                                 $"Server: {builder.DataSource ?? "Unknown"}\n" +
                                 $"Database: {builder.InitialCatalog ?? "Unknown"}\n" +
                                 $"User: {builder.UserID ?? "Windows Authentication"}\n" +
                                 $"Connection Timeout: {builder.ConnectTimeout} giây\n" +
                                 $"Command Timeout: {connectionManager.CommandTimeout} giây\n" +
                                 $"Trạng thái: {connectionManager.State}\n" +
                                 $"Kết nối hoạt động: {(connectionManager.KiemTraHoatDong() ? "Có" : "Không")}\n\n" +
                                 $"Connection String:\n{connectionString}";
                    
                    MsgBox.ShowInfo(details, "Thông tin Database");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi lấy thông tin database");
            }
        }

        /// <summary>
        /// Refresh thông tin database trên status bar
        /// </summary>
        public void RefreshDatabaseInfo()
        {
            try
            {
                var dbInfo = GetDatabaseInfo();
                DBInfoBarStaticItem.Caption = dbInfo;
            }
            catch (Exception ex)
            {
                DBInfoBarStaticItem.Caption = $"DB: Lỗi - {ex.Message}";
            }
        }
        #endregion

        
    }
}