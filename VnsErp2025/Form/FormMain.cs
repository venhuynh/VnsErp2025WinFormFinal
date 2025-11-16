using Authentication.Form;
using Bll.Common;
using Bll.Utils;
using Dal.Connection;
using Dal.DataContext;
using DevExpress.XtraBars;
using MasterData.Company;
using MasterData.Customer;
using MasterData.ProductService;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
// ReSharper disable InconsistentNaming

namespace VnsErp2025.Form
{
    /// <summary>
    /// Form chính của ứng dụng VNS ERP 2025 - giao diện chính sau khi đăng nhập thành công.
    /// Quản lý các chức năng hệ thống, hiển thị thông tin user và database, điều hướng đến các module con.
    /// </summary>
    public partial class FormMain : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        #region Constants

        /// <summary>
        /// Khoảng thời gian refresh thông tin database (30 giây)
        /// </summary>
        private const int DATABASE_REFRESH_INTERVAL = 30000;

        #endregion

        #region Private Fields

        /// <summary>
        /// Thông tin user hiện tại đã đăng nhập vào hệ thống
        /// </summary>
        private ApplicationUser _currentUser;

        /// <summary>
        /// Timer tự động refresh thông tin database định kỳ
        /// </summary>
        private Timer _dbRefreshTimer;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo form chính của ứng dụng
        /// </summary>
        public FormMain()
        {
            InitializeComponent();
            InitializeForm();
        }

        #endregion

        #region Initialization Methods

        /// <summary>
        /// Khởi tạo toàn bộ form và các thành phần giao diện
        /// Thực hiện theo thứ tự: User Info → Form Properties → UI Components → Timer → Welcome Message
        /// </summary>
        private void InitializeForm()
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                
                LoadCurrentUserInfo();
                SetupFormProperties();
                SetupRibbon();
                SetupStatusBar();
                SetupDatabaseRefreshTimer();
                //ShowWelcomeMessage();

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi khởi tạo form chính");
            }
        }

        /// <summary>
        /// Tải thông tin user hiện tại từ session hoặc tạo user demo nếu chưa có
        /// </summary>
        private void LoadCurrentUserInfo()
        {
            try
            {
                _currentUser = ApplicationSystemUtils.GetCurrentUser();
                
                // Tạo user demo nếu chưa có user nào đăng nhập (chỉ dùng cho môi trường phát triển)
                if (_currentUser == null)
                {
                    _currentUser = CreateDemoUser();
                    ApplicationSystemUtils.SetCurrentUser(_currentUser);
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi tải thông tin user");
            }
        }

        /// <summary>
        /// Tạo user demo cho môi trường phát triển
        /// </summary>
        /// <returns>User demo với thông tin cơ bản</returns>
        private ApplicationUser CreateDemoUser()
        {
            return new ApplicationUser
            {
                Id = Guid.NewGuid(),
                UserName = "admin",
                Active = true,
            };
        }

        /// <summary>
        /// Thiết lập các thuộc tính cơ bản của form (tiêu đề, kích thước, vị trí)
        /// </summary>
        private void SetupFormProperties()
        {
            this.Text = $@"VNS ERP 2025 - Chào mừng {_currentUser?.UserName ?? "User"}";
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;
            
            // TODO: Thiết lập icon ứng dụng khi có
            // this.Icon = Properties.Resources.AppIcon;
        }

        /// <summary>
        /// Thiết lập giao diện ribbon với các trang và nhóm chức năng
        /// </summary>
        private void SetupRibbon()
        {
            ribbonPage1.Text = @"Trang chủ";
            PartnerRibbonPageGroup.Text = @"Hệ thống";
        }

        /// <summary>
        /// Thiết lập status bar hiển thị thông tin user, thời gian và database
        /// </summary>
        private void SetupStatusBar()
        {
            SetupUserInfoInStatusBar();
            SetupDatabaseInfoInStatusBar();
        }

        /// <summary>
        /// Thêm thông tin user và thời gian hiện tại vào status bar
        /// </summary>
        private void SetupUserInfoInStatusBar()
        {
            var userInfo = new BarStaticItem();
            userInfo.Caption = $@"User: {_currentUser?.UserName ?? "Unknown"} | Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
            ribbonStatusBar.ItemLinks.Add(userInfo);
        }

        /// <summary>
        /// Thêm thông tin database vào status bar và đăng ký event handler
        /// </summary>
        private void SetupDatabaseInfoInStatusBar()
        {
            var dbInfo = GetDatabaseInfo();
            DBInfoBarStaticItem.Caption = dbInfo;
            DBInfoBarStaticItem.ItemClick += OnDatabaseInfoItemClick;
        }

        /// <summary>
        /// Thiết lập timer tự động refresh thông tin database
        /// </summary>
        private void SetupDatabaseRefreshTimer()
        {
            try
            {
                _dbRefreshTimer = new System.Windows.Forms.Timer();
                _dbRefreshTimer.Interval = DATABASE_REFRESH_INTERVAL;
                _dbRefreshTimer.Tick += OnDatabaseRefreshTimerTick;
                _dbRefreshTimer.Start();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, @"Lỗi thiết lập timer refresh database");
            }
        }

        /// <summary>
        /// Hiển thị thông báo chào mừng user khi khởi động form
        /// </summary>
        private void ShowWelcomeMessage()
        {
            if (_currentUser != null)
            {
                MsgBox.ShowSuccess($@"Chào mừng {_currentUser.UserName} đến với VNS ERP 2025!", "Chào mừng");
            }
        }

        #endregion

        #region Database Information Methods

        /// <summary>
        /// Lấy thông tin database để hiển thị trên status bar
        /// </summary>
        /// <returns>Chuỗi thông tin database (Server | Database) hoặc thông báo lỗi</returns>
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

                    return ParseConnectionString(connectionString);
                }
            }
            catch (Exception ex)
            {
                return $"DB: Lỗi kết nối - {ex.Message}";
            }
        }

        /// <summary>
        /// Parse connection string để lấy thông tin server và database
        /// </summary>
        /// <param name="connectionString">Connection string cần parse</param>
        /// <returns>Thông tin server và database</returns>
        private string ParseConnectionString(string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            var serverName = builder.DataSource ?? "Unknown";
            var databaseName = builder.InitialCatalog ?? "Unknown";
            
            return $"DB: {serverName} | {databaseName}";
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết về database trong dialog
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
                        MsgBox.ShowWarning(@"Không có thông tin kết nối database.", "Thông tin Database");
                        return;
                    }

                    var details = BuildDatabaseDetailsMessage(connectionManager, connectionString);
                    MsgBox.ShowSuccess(details, @"Thông tin Database");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, @"Lỗi lấy thông tin database");
            }
        }

        /// <summary>
        /// Xây dựng thông điệp chi tiết về database
        /// </summary>
        /// <param name="connectionManager">Connection manager</param>
        /// <param name="connectionString">Connection string</param>
        /// <returns>Thông điệp chi tiết</returns>
        private string BuildDatabaseDetailsMessage(ConnectionManager connectionManager, string connectionString)
        {
            var builder = new SqlConnectionStringBuilder(connectionString);
            
            return $"Thông tin Database:\n\n" +
                   $"Server: {builder.DataSource ?? "Unknown"}\n" +
                   $"Database: {builder.InitialCatalog ?? "Unknown"}\n" +
                   $"User: {builder.UserID ?? "Windows Authentication"}\n" +
                   $"Connection Timeout: {builder.ConnectTimeout} giây\n" +
                   $"Command Timeout: {connectionManager.CommandTimeout} giây\n" +
                   $"Trạng thái: {connectionManager.State}\n" +
                   $"Kết nối hoạt động: {(connectionManager.IsHealthy() ? "Có" : "Không")}\n\n" +
                   $"Connection String:\n{connectionString}";
        }

        /// <summary>
        /// Refresh thông tin database trên status bar (public để có thể gọi từ bên ngoài)
        /// </summary>
        private void RefreshDatabaseInfo()
        {
            try
            {
                var dbInfo = GetDatabaseInfo();
                DBInfoBarStaticItem.Caption = dbInfo;
            }
            catch (Exception ex)
            {
                DBInfoBarStaticItem.Caption = $@"DB: Lỗi - {ex.Message}";
            }
        }

        #endregion

        #region User Management Methods

        /// <summary>
        /// Hiển thị thông tin chi tiết của user hiện tại
        /// </summary>
        private void ShowUserInfo()
        {
            if (_currentUser != null)
            {
                var userInfo = BuildUserInfoMessage();
                MsgBox.ShowSuccess(userInfo, "Thông tin tài khoản");
            }
            else
            {
                MsgBox.ShowWarning("Không tìm thấy thông tin user.", "Lỗi");
            }
        }

        /// <summary>
        /// Xây dựng thông điệp thông tin user
        /// </summary>
        /// <returns>Thông điệp thông tin user</returns>
        private string BuildUserInfoMessage()
        {
            return $"Thông tin tài khoản:\n\n" +
                   $"Tên đăng nhập: {_currentUser.UserName}\n" +
                   $"ID: {_currentUser.Id}\n" +
                   $"Trạng thái: {(_currentUser.Active ? "Hoạt động" : "Không hoạt động")}";
        }

        /// <summary>
        /// Thực hiện đăng xuất user và hiển thị lại form đăng nhập
        /// </summary>
        private void PerformLogout()
        {
            _currentUser = null;
            ApplicationSystemUtils.LogoutCurrentUser();
            this.Hide();
            
            ShowLoginForm();
        }

        /// <summary>
        /// Hiển thị form đăng nhập và xử lý kết quả
        /// </summary>
        private void ShowLoginForm()
        {
            using (var loginForm = new FrmLogin())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Đăng nhập lại thành công - refresh thông tin và hiển thị lại form chính
                    LoadCurrentUserInfo();
                    SetupFormProperties();
                    SetupStatusBar();
                    this.Show();
                }
                else
                {
                    // Thoát ứng dụng nếu user hủy đăng nhập
                    Application.Exit();
                }
            }
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Xử lý sự kiện click vào database info item trên status bar
        /// </summary>
        /// <param name="sender">Đối tượng gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void OnDatabaseInfoItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ShowDatabaseDetails();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị thông tin database");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút cấu hình SQL Server
        /// </summary>
        /// <param name="sender">Đối tượng gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void ConfigSqlServerInfoBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ShowDatabaseConfigForm();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi cấu hình SQL Server");
            }
        }

        /// <summary>
        /// Hiển thị form cấu hình database và xử lý kết quả
        /// </summary>
        private void ShowDatabaseConfigForm()
        {
            using (OverlayManager.ShowScope(this))
            {
                using (var configForm = new FrmDatabaseConfig())
                {
                    var result = configForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        RefreshDatabaseInfo();
                        MsgBox.ShowSuccess("Cấu hình kết nối đã được cập nhật.", "Thông báo");
                    }
                }
            }
        }

        /// <summary>
        /// Xử lý sự kiện timer refresh database
        /// </summary>
        /// <param name="sender">Đối tượng gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void OnDatabaseRefreshTimerTick(object sender, EventArgs e)
        {
            try
            {
                RefreshDatabaseInfo();
            }
            catch (Exception ex)
            {
                // Chỉ log lỗi để tránh spam thông báo cho user
                System.Diagnostics.Debug.WriteLine($"Lỗi refresh database info: {ex.Message}");
            }
        }

        #region MasterData

        #region Khách hàng - Đối tác

        /// <summary>
        /// Xử lý sự kiện click nút Partner - hiển thị form quản lý đối tác
        /// </summary>
        /// <param name="sender">Đối tượng gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void PartnerButton_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                
                ApplicationSystemUtils.ShowOrActivateForm<FrmBusinessPartnerList>(this);
                
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }
        private void PhanLoaiKhachHangBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmBusinessPartnerCategory>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }


        private void SiteKhachHangBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmBusinessPartnerSite>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }


        private void LienHeKhachHangDoiTacBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmBusinessPartnerContact>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }
        #endregion

        #region Công ty
        private void CongTyBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmCompany>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }


        private void ChiNhanhBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmCompanyBranch>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }

        private void PhongBanBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmDepartment>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }

        private void ChucVuBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmPosition>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý chức vụ");
            }
        }


        private void NhanVienBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmEmployee>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý chức vụ");
            }
        }
        #endregion

        #region Sản phẩm dịch vụ

        /// <summary>
        /// Xử lý sự kiện click nút Product Service - hiển thị form quản lý sản phẩm/dịch vụ
        /// </summary>
        /// <param name="sender">Đối tượng gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void ProductServiceBtn_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmProductServiceList>(this);


                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }


        private void HinhAnhSPDVBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmProductImage>(this);


                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }
        
        private void PhanLoaiSPDVBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmProductServiceCategory>(this);


                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }


        private void DonViTinhBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmUnitOfMeasure>(this);


                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }

        private void BienTheSPDVBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmProductVariant>(this);


                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý đối tác");
            }
        }
        #endregion

        #endregion

        #endregion


    }
}