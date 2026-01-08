using Authentication.Form;
using Bll.Common;
using Common.Common;
using Common.Utils;
using DevExpress.XtraBars;
using DeviceAssetManagement.Management.DeviceMangement;
using DTO.VersionAndUserManagementDto;
using Inventory.Management;
using Inventory.ProductVariantIdentifier;
using Inventory.Query;
using Inventory.StockIn.NhapBaoHanh;
using Inventory.StockIn.NhapHangThuongMai;
using Inventory.StockIn.NhapLapRap;
using Inventory.StockIn.NhapLuuChuyenKho;
using Inventory.StockIn.NhapNoiBo;
using Inventory.StockIn.NhapThietBiMuon;
using Inventory.StockOut.XuatBaoHanh;
using Inventory.StockOut.XuatChoThueMuon;
using Inventory.StockOut.XuatHangThuongMai;
using Inventory.StockOut.XuatLapRap;
using Inventory.StockOut.XuatLuuChuyenKho;
using Inventory.StockOut.XuatNoiBo;
using MasterData.Company;
using MasterData.Customer;
using MasterData.ProductService;
using System;
using System.Data.SqlClient;
using System.Windows.Forms;
using Dal.Connection;
using VersionAndUserManagement.AllowedMacAddress;
using VersionAndUserManagement.ApplicationVersion;
using VersionAndUserManagement.UserManagement;

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
        private ApplicationUserDto _currentUser;

        /// <summary>
        /// Timer tự động refresh thông tin database định kỳ
        /// </summary>
        private Timer _dbRefreshTimer;

        /// <summary>
        /// Update checker để kiểm tra cập nhật tự động
        /// </summary>
        private UpdateChecker _updateChecker;

        /// <summary>
        /// Business Logic Layer cho phiên bản ứng dụng
        /// </summary>
        private ApplicationVersionBll _applicationVersionBll;

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
                SetupSuperToolTips();
                SetupDatabaseRefreshTimer();
                SetupUpdateChecker();
                LoadVersionInfo();
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
                // GetCurrentUser() trả về ApplicationUser entity, cần convert sang DTO
                var userEntity = ApplicationSystemUtils.GetCurrentUser();

                if (userEntity != null)
                {
                    // Convert entity sang DTO
                    _currentUser = new ApplicationUserDto
                    {
                        Id = userEntity.Id,
                        UserName = userEntity.UserName,
                        HashPassword = userEntity.HashPassword ?? string.Empty,
                        Active = userEntity.Active,
                        EmployeeId = userEntity.EmployeeId
                    };
                }
                else
                {
                    // Tạo user demo nếu chưa có user nào đăng nhập (chỉ dùng cho môi trường phát triển)
                    _currentUser = CreateDemoUser();

                    // Set user entity vào ApplicationSystemUtils (convert DTO sang entity)
                    var demoUserEntity = new ApplicationUserDto()
                    {
                        Id = _currentUser.Id,
                        UserName = _currentUser.UserName,
                        HashPassword = _currentUser.HashPassword ?? string.Empty,
                        Active = _currentUser.Active,
                        EmployeeId = _currentUser.EmployeeId
                    };
                    ApplicationSystemUtils.SetCurrentUser(demoUserEntity);
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
        private ApplicationUserDto CreateDemoUser()
        {
            return new ApplicationUserDto
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

            // Thiết lập icon ứng dụng
            ApplicationIconHelper.SetFormIcon(this);
        }

        /// <summary>
        /// Thiết lập giao diện ribbon với các trang và nhóm chức năng
        /// </summary>
        private void SetupRibbon()
        {
            // Đăng ký event handlers cho các nút Version and User Management
            if (AllowedMacAddressBarButtonItem != null)
                AllowedMacAddressBarButtonItem.ItemClick += AllowedMacAddressBarButtonItem_ItemClick;

            if (ApplicationUserBarButtonItem != null)
                ApplicationUserBarButtonItem.ItemClick += ApplicationUserBarButtonItem_ItemClick;

            if (ApplicationVersionBarButtonItem != null)
                ApplicationVersionBarButtonItem.ItemClick += ApplicationVersionBarButtonItem_ItemClick;

            if (DatabaseConfigBarButtonItem != null)
                DatabaseConfigBarButtonItem.ItemClick += DatabaseConfigBarButtonItem_ItemClick;

            if (NasConfigBarButtonItem != null)
                NasConfigBarButtonItem.ItemClick += NasConfigBarButtonItem_ItemClick;

            if (AttributeBarButtonItem != null)
                AttributeBarButtonItem.ItemClick += AttributeBarButtonItem_ItemClick;

            if (DeviceDtoMangementBarButtonItem != null)
                DeviceDtoMangementBarButtonItem.ItemClick += DeviceDtoMangementBarButtonItem_ItemClick;

            if (DinhDanhSpHhBarButtonItem != null)
                DinhDanhSpHhBarButtonItem.ItemClick += DinhDanhSpHhBarButtonItem_ItemClick;
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
        /// Tải thông tin phiên bản và ngày phát hành hiện tại vào status bar
        /// </summary>
        private void LoadVersionInfo()
        {
            try
            {
                if (_applicationVersionBll == null)
                {
                    _applicationVersionBll = new ApplicationVersionBll();
                }

                var activeVersion = _applicationVersionBll.GetActiveVersion();

                if (activeVersion != null && ReleaserVersionAndDateBarStaticItem != null)
                {
                    var version = activeVersion.Version ?? "N/A";
                    var releaseDate = activeVersion.ReleaseDate.ToString("dd/MM/yyyy");
                    var statusIcon = activeVersion.IsActive ? "<color=#4CAF50>●</color>" : "<color=#757575>○</color>";

                    // Format HTML với màu sắc và bold để làm nổi bật
                    ReleaserVersionAndDateBarStaticItem.Caption =
                        $@"<color=#757575>Phiên bản:</color> <b><color=#2196F3>{version}</color></b> {statusIcon} | " +
                        $@"<color=#757575>Ngày phát hành:</color> <b><color=#4CAF50>{releaseDate}</color></b>";
                }
                else if (ReleaserVersionAndDateBarStaticItem != null)
                {
                    // Nếu không có phiên bản active, hiển thị phiên bản từ Assembly
                    var currentVersion = _applicationVersionBll?.GetCurrentApplicationVersion() ?? "N/A";
                    ReleaserVersionAndDateBarStaticItem.Caption =
                        $@"<color=#757575>Phiên bản:</color> <b><color=#2196F3>{currentVersion}</color></b> | " +
                        $@"<color=#757575>Chưa có thông tin phát hành</color>";
                }
            }
            catch (Exception ex)
            {
                // Không block ứng dụng nếu có lỗi khi load phiên bản
                System.Diagnostics.Debug.WriteLine($"Lỗi load thông tin phiên bản: {ex.Message}");
                if (ReleaserVersionAndDateBarStaticItem != null)
                {
                    ReleaserVersionAndDateBarStaticItem.Caption =
                        "@<color='blue'>Phiên bản:</color> <color=#F44336>Không xác định</color>";
                }
            }
        }

        /// <summary>
        /// Thêm thông tin user và thời gian hiện tại vào status bar
        /// </summary>
        private void SetupUserInfoInStatusBar()
        {
            var userInfo = new BarStaticItem();
            userInfo.Caption =
                $@"User: {_currentUser?.UserName ?? "Unknown"} | Thời gian: {DateTime.Now:dd/MM/yyyy HH:mm:ss}";
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
                _dbRefreshTimer = new System.Windows.Forms.Timer
                {
                    Interval = DATABASE_REFRESH_INTERVAL
                };
                _dbRefreshTimer.Tick += OnDatabaseRefreshTimerTick;
                _dbRefreshTimer.Start();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, @"Lỗi thiết lập timer refresh database");
            }
        }

        /// <summary>
        /// Thiết lập update checker để kiểm tra cập nhật tự động
        /// </summary>
        private void SetupUpdateChecker()
        {
            try
            {
                _updateChecker = new UpdateChecker();
                _updateChecker.UpdateAvailable += OnUpdateAvailable;
                _updateChecker.StartPeriodicCheck();

                // Kiểm tra khi khởi động (background)
                _ = _updateChecker.CheckOnStartupAsync();
            }
            catch (Exception ex)
            {
                // Không block ứng dụng nếu có lỗi với update checker
                System.Diagnostics.Debug.WriteLine($"Lỗi thiết lập update checker: {ex.Message}");
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi có bản cập nhật mới
        /// </summary>
        private void OnUpdateAvailable(object sender, UpdateAvailableEventArgs e)
        {
            try
            {
                // Hiển thị thông báo trên UI thread
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => OnUpdateAvailable(sender, e)));
                    return;
                }

                var versionInfo = e.VersionInfo;
                var message = $"Có bản cập nhật mới: {versionInfo.Version}\n\n" +
                              $"Phiên bản hiện tại: {e.CurrentVersion}\n" +
                              $"Phiên bản mới: {versionInfo.Version}\n\n";

                if (versionInfo.ReleaseNotes != null && !string.IsNullOrEmpty(versionInfo.ReleaseNotes.Vietnamese))
                {
                    message += $"Thông tin cập nhật:\n{versionInfo.ReleaseNotes.Vietnamese}\n\n";
                }

                if (versionInfo.Changes != null && versionInfo.Changes.Length > 0)
                {
                    message += "Các thay đổi:\n";
                    foreach (var change in versionInfo.Changes)
                    {
                        message += $"• {change}\n";
                    }

                    message += "\n";
                }

                message += "Bạn có muốn cập nhật ngay bây giờ không?";

                var result = MsgBox.ShowYesNoCancel(message, "Cập nhật ứng dụng");

                if (result == DialogResult.Yes)
                {
                    // TODO: Mở form cập nhật
                    // ShowUpdateForm(versionInfo);
                    MsgBox.ShowWarning("Chức năng cập nhật đang được phát triển. Vui lòng tải bản cập nhật thủ công.",
                        "Thông báo");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi xử lý thông báo cập nhật: {ex.Message}");
            }
        }

        /// <summary>
        /// Thiết lập SuperToolTip cho tất cả các BarButtonItem trong form
        /// </summary>
        private void SetupSuperToolTips()
        {
            try
            {
                // Hệ thống
                if (ConfigSqlServerInfoBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ConfigSqlServerInfoBarButtonItem,
                        title: "<b><color=DarkBlue>⚙️ Cấu hình SQL Server</color></b>",
                        content:
                        "Cấu hình kết nối đến SQL Server database.<br/><br/><b>Chức năng:</b><br/>• Thiết lập thông tin server, database<br/>• Cấu hình authentication (Windows/SQL)<br/>• Kiểm tra kết nối database<br/>• Lưu cấu hình vào file config<br/><br/><color=Gray>Lưu ý:</color> Cần khởi động lại ứng dụng sau khi thay đổi cấu hình."
                    );
                }

                // Master Data - Khách hàng - Đối tác
                if (KhachHangDoiTacBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        KhachHangDoiTacBarButtonItem,
                        title: "<b><color=Blue>🤝 Khách hàng - Đối tác</color></b>",
                        content:
                        "Quản lý danh sách khách hàng và đối tác trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách khách hàng/đối tác<br/>• Thêm, sửa, xóa thông tin<br/>• Tìm kiếm và lọc dữ liệu<br/>• Quản lý thông tin liên hệ<br/><br/><color=Gray>Lưu ý:</color> Dữ liệu này được sử dụng trong các module bán hàng, mua hàng và kho."
                    );
                }

                if (PhanLoaiKhachHangBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        PhanLoaiKhachHangBarButtonItem,
                        title: "<b><color=Blue>📂 Phân loại khách hàng</color></b>",
                        content:
                        "Quản lý các phân loại khách hàng/đối tác (ví dụ: Khách hàng VIP, Đối tác chiến lược, v.v.).<br/><br/><b>Chức năng:</b><br/>• Tạo và quản lý các phân loại<br/>• Gán phân loại cho khách hàng/đối tác<br/>• Hỗ trợ báo cáo và phân tích<br/><br/><color=Gray>Lưu ý:</color> Phân loại giúp tổ chức và quản lý khách hàng hiệu quả hơn."
                    );
                }

                if (SiteKhachHangBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SiteKhachHangBarButtonItem,
                        title: "<b><color=Blue>📍 Site khách hàng</color></b>",
                        content:
                        "Quản lý các địa điểm/chi nhánh của khách hàng/đối tác.<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa địa điểm<br/>• Quản lý thông tin địa chỉ chi tiết<br/>• Gán địa điểm cho khách hàng/đối tác<br/><br/><color=Gray>Lưu ý:</color> Một khách hàng có thể có nhiều địa điểm giao hàng."
                    );
                }

                if (LienHeKhachHangDoiTacBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        LienHeKhachHangDoiTacBarButtonItem,
                        title: "<b><color=Blue>📞 Liên hệ khách hàng - Đối tác</color></b>",
                        content:
                        "Quản lý thông tin liên hệ của khách hàng/đối tác.<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa người liên hệ<br/>• Quản lý thông tin: tên, chức vụ, email, điện thoại<br/>• Gán người liên hệ cho khách hàng/đối tác<br/><br/><color=Gray>Lưu ý:</color> Thông tin liên hệ giúp giao tiếp hiệu quả với khách hàng/đối tác."
                    );
                }

                // Master Data - Công ty
                if (CongTyBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        CongTyBarButtonItem,
                        title: "<b><color=Green>🏢 Công ty</color></b>",
                        content:
                        "Quản lý thông tin công ty trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách công ty<br/>• Thêm, sửa, xóa thông tin công ty<br/>• Quản lý thông tin: tên, mã số thuế, địa chỉ<br/><br/><color=Gray>Lưu ý:</color> Thông tin công ty được sử dụng trong các báo cáo và tài liệu."
                    );
                }

                if (ChiNhanhBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ChiNhanhBarButtonItem,
                        title: "<b><color=Green>🏛️ Chi nhánh</color></b>",
                        content:
                        "Quản lý các chi nhánh của công ty.<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa chi nhánh<br/>• Quản lý thông tin: tên, địa chỉ, mã chi nhánh<br/>• Gán chi nhánh cho công ty<br/><br/><color=Gray>Lưu ý:</color> Chi nhánh được sử dụng để phân bổ hàng hóa và quản lý kho."
                    );
                }

                if (PhongBanBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        PhongBanBarButtonItem,
                        title: "<b><color=Green>🏢 Phòng ban</color></b>",
                        content:
                        "Quản lý các phòng ban trong công ty.<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa phòng ban<br/>• Quản lý cấu trúc phòng ban (có thể có phòng ban con)<br/>• Gán phòng ban cho chi nhánh<br/><br/><color=Gray>Lưu ý:</color> Phòng ban giúp tổ chức nhân sự và phân quyền trong hệ thống."
                    );
                }

                if (ChucVuBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ChucVuBarButtonItem,
                        title: "<b><color=Green>👔 Chức vụ</color></b>",
                        content:
                        "Quản lý các chức vụ trong công ty.<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa chức vụ<br/>• Quản lý thông tin: tên chức vụ, mô tả<br/>• Gán chức vụ cho nhân viên<br/><br/><color=Gray>Lưu ý:</color> Chức vụ được sử dụng để quản lý nhân sự và phân quyền."
                    );
                }

                if (NhanVienBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NhanVienBarButtonItem,
                        title: "<b><color=Green>👥 Nhân viên</color></b>",
                        content:
                        "Quản lý thông tin nhân viên trong công ty.<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa nhân viên<br/>• Quản lý thông tin: tên, mã nhân viên, phòng ban, chức vụ<br/>• Gán nhân viên cho phòng ban và chức vụ<br/><br/><color=Gray>Lưu ý:</color> Thông tin nhân viên được sử dụng trong các module quản lý và báo cáo."
                    );
                }

                // Master Data - Sản phẩm Dịch vụ
                if (SanPhamDichVuBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        SanPhamDichVuBarButtonItem,
                        title: "<b><color=Purple>📦 Sản phẩm - Dịch vụ</color></b>",
                        content:
                        "Quản lý danh sách sản phẩm và dịch vụ trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách sản phẩm/dịch vụ<br/>• Thêm, sửa, xóa thông tin<br/>• Quản lý giá, đơn vị tính, phân loại<br/>• Quản lý hình ảnh và mô tả<br/><br/><color=Gray>Lưu ý:</color> Dữ liệu này được sử dụng trong các module bán hàng, mua hàng và kho."
                    );
                }

                if (HinhAnhSPDVBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        HinhAnhSPDVBarButtonItem,
                        title: "<b><color=Purple>🖼️ Hình ảnh sản phẩm - Dịch vụ</color></b>",
                        content:
                        "Quản lý hình ảnh cho sản phẩm và dịch vụ.<br/><br/><b>Chức năng:</b><br/>• Upload, xóa hình ảnh<br/>• Quản lý nhiều hình ảnh cho một sản phẩm<br/>• Đặt hình ảnh chính<br/><br/><color=Gray>Lưu ý:</color> Hình ảnh giúp hiển thị sản phẩm/dịch vụ một cách trực quan."
                    );
                }

                if (PhanLoaiSPDVBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        PhanLoaiSPDVBarButtonItem,
                        title: "<b><color=Purple>📂 Phân loại sản phẩm - Dịch vụ</color></b>",
                        content:
                        "Quản lý các phân loại sản phẩm/dịch vụ (ví dụ: Điện tử, Quần áo, Dịch vụ tư vấn, v.v.).<br/><br/><b>Chức năng:</b><br/>• Tạo và quản lý các phân loại<br/>• Gán phân loại cho sản phẩm/dịch vụ<br/>• Hỗ trợ báo cáo và phân tích<br/><br/><color=Gray>Lưu ý:</color> Phân loại giúp tổ chức và tìm kiếm sản phẩm/dịch vụ hiệu quả hơn."
                    );
                }

                if (DonViTinhBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DonViTinhBarButtonItem,
                        title: "<b><color=Purple>📏 Đơn vị tính</color></b>",
                        content:
                        "Quản lý các đơn vị tính (ví dụ: Cái, Hộp, Thùng, Kg, v.v.).<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa đơn vị tính<br/>• Quản lý quy đổi giữa các đơn vị<br/>• Gán đơn vị tính cho sản phẩm<br/><br/><color=Gray>Lưu ý:</color> Đơn vị tính được sử dụng trong các phiếu nhập/xuất kho và báo cáo."
                    );
                }

                if (BienTheSPDVBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        BienTheSPDVBarButtonItem,
                        title: "<b><color=Purple>🎨 Biến thể sản phẩm - Dịch vụ</color></b>",
                        content:
                        "Quản lý các biến thể của sản phẩm/dịch vụ (ví dụ: Màu sắc, Kích thước, v.v.).<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa biến thể<br/>• Quản lý thuộc tính biến thể (màu, size, v.v.)<br/>• Gán biến thể cho sản phẩm<br/><br/><color=Gray>Lưu ý:</color> Biến thể giúp quản lý các phiên bản khác nhau của cùng một sản phẩm."
                    );
                }

                if (AttributeBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        AttributeBarButtonItem,
                        title: "<b><color=Purple>🏷️ Thuộc tính</color></b>",
                        content:
                        "Quản lý các thuộc tính của sản phẩm/dịch vụ (ví dụ: Màu sắc, Kích thước, Chất liệu, v.v.).<br/><br/><b>Chức năng:</b><br/>• Thêm, sửa, xóa thuộc tính<br/>• Quản lý kiểu dữ liệu của thuộc tính<br/>• Gán thuộc tính cho sản phẩm/dịch vụ<br/><br/><color=Gray>Lưu ý:</color> Thuộc tính giúp mô tả chi tiết các đặc điểm của sản phẩm/dịch vụ."
                    );
                }

                // Nhập kho
                if (NhapBaoHanhBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NhapBaoHanhBarButtonItem,
                        title: "<b><color=Orange>📥 Nhập bảo hành</color></b>",
                        content:
                        "Tạo phiếu nhập kho cho hàng hóa thiết bị bảo hành.<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu nhập bảo hành<br/>• Quản lý chi tiết hàng hóa nhập<br/>• Theo dõi số lượng và giá trị<br/><br/><color=Gray>Lưu ý:</color> Phiếu nhập bảo hành được sử dụng khi nhận hàng hóa từ khách hàng để bảo hành."
                    );
                }

                if (NhapHangThuongMaiBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NhapHangThuongMaiBarButtonItem,
                        title: "<b><color=Orange>📥 Nhập hàng thương mại</color></b>",
                        content:
                        "Tạo phiếu nhập kho cho hàng hóa thương mại (mua từ nhà cung cấp).<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu nhập hàng thương mại<br/>• Quản lý chi tiết hàng hóa nhập<br/>• Theo dõi nhà cung cấp, số lượng, giá trị<br/><br/><color=Gray>Lưu ý:</color> Phiếu nhập hàng thương mại được sử dụng khi nhận hàng từ nhà cung cấp."
                    );
                }

                if (NhapLuuChuyenKhoBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NhapLuuChuyenKhoBarButtonItem,
                        title: "<b><color=Orange>📥 Nhập lưu chuyển kho</color></b>",
                        content:
                        "Tạo phiếu nhập kho từ việc chuyển kho (từ kho khác đến).<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu nhập lưu chuyển kho<br/>• Quản lý chi tiết hàng hóa nhập<br/>• Theo dõi kho nguồn, kho đích<br/><br/><color=Gray>Lưu ý:</color> Phiếu nhập lưu chuyển kho được sử dụng khi chuyển hàng giữa các kho."
                    );
                }

                if (NhapNoiBoBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NhapNoiBoBarButtonItem,
                        title: "<b><color=Orange>📥 Nhập nội bộ</color></b>",
                        content:
                        "Tạo phiếu nhập kho cho hàng hóa thiết bị nội bộ.<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu nhập nội bộ<br/>• Quản lý chi tiết hàng hóa thiết bị nhập<br/>• Theo dõi số lượng và giá trị<br/><br/><color=Gray>Lưu ý:</color> Phiếu nhập nội bộ được sử dụng khi nhận hàng hóa thiết bị từ các đơn vị nội bộ."
                    );
                }

                if (NhapThietBiMuonBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NhapThietBiMuonBarButtonItem,
                        title: "<b><color=Orange>📥 Nhập thiết bị mượn - thuê</color></b>",
                        content:
                        "Tạo phiếu nhập kho cho thiết bị được mượn hoặc thuê về.<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu nhập thiết bị mượn/thuê<br/>• Quản lý chi tiết thiết bị nhập<br/>• Theo dõi khách hàng, số lượng<br/><br/><color=Gray>Lưu ý:</color> Phiếu nhập thiết bị mượn/thuê được sử dụng khi nhận lại thiết bị từ khách hàng sau khi cho mượn/thuê."
                    );
                }

                // Xuất kho
                if (XuatBaoHanhBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        XuatBaoHanhBarButtonItem,
                        title: "<b><color=Red>📤 Xuất bảo hành</color></b>",
                        content:
                        "Tạo phiếu xuất kho cho hàng hóa thiết bị bảo hành (trả lại cho khách hàng).<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu xuất bảo hành<br/>• Quản lý chi tiết hàng hóa xuất<br/>• Theo dõi số lượng và giá trị<br/><br/><color=Gray>Lưu ý:</color> Phiếu xuất bảo hành được sử dụng khi trả lại hàng hóa đã bảo hành cho khách hàng."
                    );
                }

                if (XuatHangThuongMaiBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        XuatHangThuongMaiBarButtonItem,
                        title: "<b><color=Red>📤 Xuất hàng thương mại</color></b>",
                        content:
                        "Tạo phiếu xuất kho cho hàng hóa thương mại (bán cho khách hàng).<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu xuất hàng thương mại<br/>• Quản lý chi tiết hàng hóa xuất<br/>• Theo dõi khách hàng, số lượng, giá trị<br/><br/><color=Gray>Lưu ý:</color> Phiếu xuất hàng thương mại được sử dụng khi bán hàng cho khách hàng."
                    );
                }

                if (XuatLuuChuyenKhoBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        XuatLuuChuyenKhoBarButtonItem,
                        title: "<b><color=Red>📤 Xuất lưu chuyển kho</color></b>",
                        content:
                        "Tạo phiếu xuất kho để chuyển kho (từ kho này sang kho khác).<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu xuất lưu chuyển kho<br/>• Quản lý chi tiết hàng hóa xuất<br/>• Theo dõi kho nguồn, kho đích<br/><br/><color=Gray>Lưu ý:</color> Phiếu xuất lưu chuyển kho được sử dụng khi chuyển hàng giữa các kho."
                    );
                }

                if (XuatNoiBoBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        XuatNoiBoBarButtonItem,
                        title: "<b><color=Red>📤 Xuất nội bộ</color></b>",
                        content:
                        "Tạo phiếu xuất kho cho hàng hóa thiết bị nội bộ.<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu xuất nội bộ<br/>• Quản lý chi tiết hàng hóa thiết bị xuất<br/>• Theo dõi số lượng và giá trị<br/><br/><color=Gray>Lưu ý:</color> Phiếu xuất nội bộ được sử dụng khi xuất hàng hóa thiết bị cho các đơn vị nội bộ."
                    );
                }

                if (XuatChoThueMuonBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        XuatChoThueMuonBarButtonItem,
                        title: "<b><color=Red>📤 Xuất thiết bị mượn - thuê</color></b>",
                        content:
                        "Tạo phiếu xuất kho cho thiết bị cho mượn hoặc cho thuê.<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu xuất thiết bị mượn/thuê<br/>• Quản lý chi tiết thiết bị xuất<br/>• Theo dõi khách hàng, số lượng<br/><br/><color=Gray>Lưu ý:</color> Phiếu xuất thiết bị mượn/thuê được sử dụng khi cho khách hàng mượn hoặc thuê thiết bị."
                    );
                }

                if (XuatLapRapBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        XuatLapRapBarButtonItem,
                        title: "<b><color=Red>📤 Xuất linh kiện lắp ráp</color></b>",
                        content:
                        "Tạo phiếu xuất kho cho linh kiện để lắp ráp thành một bộ máy hoàn chỉnh.<br/><br/><b>Chức năng:</b><br/>• Tạo phiếu xuất linh kiện lắp ráp<br/>• Quản lý chi tiết linh kiện xuất<br/>• Đọc mã vạch để tự động thêm linh kiện<br/>• Theo dõi số lượng linh kiện xuất<br/><br/><color=Gray>Lưu ý:</color> Phiếu xuất linh kiện lắp ráp được sử dụng khi xuất các linh kiện để lắp ráp thành một bộ máy hoàn chỉnh."
                    );
                }

                // Quản lý kho - Tồn kho theo tháng
                if (InventoryBalanceBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        InventoryBalanceBarButtonItem,
                        title: "<b><color=Teal>📊 Tồn kho theo tháng</color></b>",
                        content:
                        "Quản lý và theo dõi tồn kho theo từng kỳ (tháng/năm).<br/><br/><b>Chức năng:</b><br/>• Xem tồn kho theo kỳ (tháng/năm)<br/>• Tính tổng kết nhập/xuất cho kỳ<br/>• Kết chuyển tồn kho sang kỳ tiếp theo<br/>• Khóa/mở khóa tồn kho<br/>• Xuất báo cáo tồn kho<br/><br/><color=Gray>Lưu ý:</color> Module này giúp quản lý tồn kho theo từng kỳ một cách có hệ thống và chính xác."
                    );
                }

                // Truy vấn
                if (StockInOutMasterHistoryBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        StockInOutMasterHistoryBarButtonItem,
                        title: "<b><color=DarkBlue>📋 Phiếu xuất kho</color></b>",
                        content:
                        "Xem lịch sử các phiếu nhập/xuất kho trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách phiếu nhập/xuất<br/>• Tìm kiếm và lọc theo nhiều tiêu chí<br/>• Xem chi tiết từng phiếu<br/>• In và xuất báo cáo<br/><br/><color=Gray>Lưu ý:</color> Module này giúp tra cứu và theo dõi lịch sử giao dịch kho."
                    );
                }

                if (StockInOutProductHistoryBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        StockInOutProductHistoryBarButtonItem,
                        title: "<b><color=DarkBlue>📦 Sản phẩm - Dịch vụ</color></b>",
                        content:
                        "Xem lịch sử nhập/xuất kho theo từng sản phẩm/dịch vụ.<br/><br/><b>Chức năng:</b><br/>• Xem lịch sử nhập/xuất của sản phẩm<br/>• Theo dõi số lượng tồn kho<br/>• Xem chi tiết các phiếu liên quan<br/><br/><color=Gray>Lưu ý:</color> Module này giúp tra cứu lịch sử giao dịch của từng sản phẩm/dịch vụ."
                    );
                }


                if (StockInOutImagesBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        StockInOutImagesBarButtonItem,
                        title: "<b><color=DarkBlue>🖼️ Hình ảnh</color></b>",
                        content:
                        "Xem và quản lý hình ảnh liên quan đến các phiếu nhập/xuất kho.<br/><br/><b>Chức năng:</b><br/>• Xem hình ảnh của phiếu nhập/xuất<br/>• Upload và quản lý hình ảnh<br/>• Xem hình ảnh sản phẩm/thiết bị<br/><br/><color=Gray>Lưu ý:</color> Module này giúp lưu trữ và tra cứu hình ảnh liên quan đến kho."
                    );
                }

                if (StockInOutDocumentBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        StockInOutDocumentBarButtonItem,
                        title: "<b><color=DarkBlue>📎 Chứng từ</color></b>",
                        content:
                        "Xem và quản lý chứng từ liên quan đến các phiếu nhập/xuất kho.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách chứng từ của phiếu nhập/xuất<br/>• Upload và quản lý chứng từ (PDF, Word, Excel, v.v.)<br/>• Tải xuống và mở chứng từ<br/>• Xóa chứng từ<br/><br/><color=Gray>Lưu ý:</color> Module này giúp lưu trữ và tra cứu các file chứng từ liên quan đến kho."
                    );
                }

                if (DeviceDtoMangementBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DeviceDtoMangementBarButtonItem,
                        title: "<b><color=DarkBlue>📱 Danh sách Thiết bị - Tài sản</color></b>",
                        content:
                        "Quản lý danh sách thiết bị và tài sản trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách thiết bị/tài sản<br/>• Thêm, sửa, xóa thông tin thiết bị/tài sản<br/>• Quản lý thông tin chi tiết: mã thiết bị, tên, mô tả<br/>• Quản lý bảo hành và thông tin liên quan<br/>• Tìm kiếm và lọc dữ liệu<br/><br/><color=Gray>Lưu ý:</color> Module này giúp quản lý toàn bộ thiết bị và tài sản trong kho một cách hiệu quả."
                    );
                }

                // Version and User Management
                if (AllowedMacAddressBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        AllowedMacAddressBarButtonItem,
                        title: "<b><color=DarkGreen>🔒 MAC Address được phép</color></b>",
                        content:
                        "Quản lý danh sách địa chỉ MAC được phép truy cập ứng dụng.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách MAC address được phép<br/>• Thêm, sửa, xóa MAC address<br/>• Kích hoạt/vô hiệu hóa MAC address<br/>• Quản lý tên máy tính và mô tả<br/><br/><color=Gray>Lưu ý:</color> Chỉ các máy tính có MAC address trong danh sách mới được phép sử dụng ứng dụng."
                    );
                }

                if (ApplicationUserBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ApplicationUserBarButtonItem,
                        title: "<b><color=DarkGreen>👤 Người dùng ứng dụng</color></b>",
                        content:
                        "Quản lý tài khoản người dùng trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách người dùng<br/>• Thêm, sửa, xóa tài khoản người dùng<br/>• Quản lý mật khẩu và quyền truy cập<br/>• Gán nhân viên cho tài khoản<br/>• Kích hoạt/vô hiệu hóa tài khoản<br/><br/><color=Gray>Lưu ý:</color> Tài khoản người dùng được sử dụng để đăng nhập và phân quyền trong hệ thống."
                    );
                }

                if (ApplicationVersionBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        ApplicationVersionBarButtonItem,
                        title: "<b><color=DarkGreen>📦 Phiên bản ứng dụng</color></b>",
                        content:
                        "Quản lý các phiên bản của ứng dụng VNS ERP 2025.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách các phiên bản<br/>• Thêm, sửa, xóa phiên bản<br/>• Đặt phiên bản đang hoạt động<br/>• Cập nhật phiên bản từ Assembly<br/>• Quản lý ghi chú phát hành<br/><br/><color=Gray>Lưu ý:</color> Chỉ có một phiên bản có thể được đặt là Active tại một thời điểm."
                    );
                }

                if (DatabaseConfigBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DatabaseConfigBarButtonItem,
                        title: "<b><color=DarkBlue>🗄️ Cấu hình Database</color></b>",
                        content:
                        "Cấu hình kết nối đến SQL Server database.<br/><br/><b>Chức năng:</b><br/>• Thiết lập thông tin server, database<br/>• Cấu hình authentication (Windows/SQL)<br/>• Kiểm tra kết nối database<br/>• Lưu cấu hình vào file config<br/><br/><color=Gray>Lưu ý:</color> Cần khởi động lại ứng dụng sau khi thay đổi cấu hình."
                    );
                }

                if (NasConfigBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        NasConfigBarButtonItem,
                        title: "<b><color=DarkBlue>💾 Cấu hình NAS</color></b>",
                        content:
                        "Cấu hình kết nối đến Network Attached Storage (NAS) để lưu trữ file.<br/><br/><b>Chức năng:</b><br/>• Thiết lập thông tin NAS server<br/>• Cấu hình đường dẫn và quyền truy cập<br/>• Kiểm tra kết nối NAS<br/>• Lưu cấu hình<br/><br/><color=Gray>Lưu ý:</color> NAS được sử dụng để lưu trữ các file lớn như hình ảnh, tài liệu."
                    );
                }

                // Quản lý kho - Định danh sản phẩm hàng hóa
                if (DinhDanhSpHhBarButtonItem != null)
                {
                    SuperToolTipHelper.SetBarButtonSuperTip(
                        DinhDanhSpHhBarButtonItem,
                        title: "<b><color=Teal>🏷️ Định danh sản phẩm hàng hóa</color></b>",
                        content:
                        "Quản lý định danh (identifier) cho các sản phẩm hàng hóa trong hệ thống.<br/><br/><b>Chức năng:</b><br/>• Xem danh sách định danh sản phẩm<br/>• Thêm, sửa, xóa định danh<br/>• Quản lý mã QR code cho định danh<br/>• Xem lịch sử giao dịch của định danh<br/>• Tìm kiếm và lọc định danh<br/><br/><color=Gray>Lưu ý:</color> Định danh giúp theo dõi và quản lý từng sản phẩm hàng hóa riêng lẻ trong kho."
                    );
                }
            }
            catch (Exception ex)
            {
                // Ignore lỗi setup SuperToolTip để không chặn form
                System.Diagnostics.Debug.WriteLine($"Lỗi setup SuperToolTip: {ex.Message}");
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

            // Lấy connection string an toàn (không có password)
            var safeConnectionString = ConnectionStringHelper.GetSafeConnectionString(connectionString);

            return $"Thông tin Database:\n\n" +
                   $"Server: {builder.DataSource ?? "Unknown"}\n" +
                   $"Database: {builder.InitialCatalog ?? "Unknown"}\n" +
                   $"User: {builder.UserID ?? "Windows Authentication"}\n" +
                   $"Connection Timeout: {builder.ConnectTimeout} giây\n" +
                   $"Command Timeout: {connectionManager.CommandTimeout} giây\n" +
                   $"Trạng thái: {connectionManager.State}\n" +
                   $"Kết nối hoạt động: {(connectionManager.IsHealthy() ? "Có" : "Không")}\n\n" +
                   $"Connection String:\n{safeConnectionString}";
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

                ApplicationSystemUtils.ShowOrActivateForm<FrmEmployeeDto>(this);

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

        /// <summary>
        /// Xử lý sự kiện click nút Attribute - hiển thị form quản lý thuộc tính
        /// </summary>
        /// <param name="sender">Đối tượng gửi sự kiện</param>
        /// <param name="e">Thông tin sự kiện</param>
        private void AttributeBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                ApplicationSystemUtils.ShowOrActivateForm<FrmAttribute>(this);

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý thuộc tính");
            }
        }

        #endregion

        #endregion

        #endregion

        #region Inventory - Nhập Xuất Tồn Kho

        #region Nhập kho

        /// <summary>
        /// Xử lý sự kiện click nút Nhập bảo hành
        /// </summary>
        private void NhapBaoHanhBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmNhapBaoHanh>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form nhập bảo hành");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Nhập hàng thương mại
        /// </summary>
        private void NhapHangThuongMaiBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmNhapKhoThuongMai>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form nhập hàng thương mại");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Nhập lưu chuyển kho
        /// </summary>
        private void NhapLuuChuyenKhoBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmNhapLuuChuyenKho>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form nhập lưu chuyển kho");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Nhập nội bộ
        /// </summary>
        private void NhapNoiBoBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmNhapNoiBo>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form nhập nội bộ");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Nhập thiết bị mượn - thuê
        /// </summary>
        private void NhapThietBiMuonBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmNhapThietBiMuon>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form nhập thiết bị mượn - thuê");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Nhập lắp ráp
        /// </summary>
        private void NhapLapRapBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmNhapLapRap>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form nhập lắp ráp");
            }
        }

        #endregion

        #region Xuất kho

        /// <summary>
        /// Xử lý sự kiện click nút Xuất bảo hành
        /// </summary>
        private void XuatBaoHanhBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmXuatBaoHanh>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form xuất bảo hành");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Xuất hàng thương mại
        /// </summary>
        private void XuatHangThuongMaiBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmXuatKhoThuongMai>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form xuất hàng thương mại");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Xuất lưu chuyển kho
        /// </summary>
        private void XuatLuuChuyenKhoBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmXuatLuuChuyenKho>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form xuất lưu chuyển kho");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Xuất nội bộ
        /// </summary>
        private void XuatNoiBoBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmXuatNoiBo>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form xuất nội bộ");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Xuất thiết bị mượn - thuê
        /// </summary>
        private void XuatChoThueMuonBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmXuatThietBiChoThueMuon>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form xuất thiết bị mượn - thuê");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Xuất linh kiện lắp ráp
        /// </summary>
        private void XuatLapRapBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmXuatLapRap>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form xuất linh kiện lắp ráp");
            }
        }

        #endregion

        #region Quản lý kho & Truy vấn

        /// <summary>
        /// Xử lý sự kiện click nút Tồn kho theo tháng
        /// </summary>
        private void InventoryBalanceBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmInventoryBalanceDto>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form tồn kho theo tháng");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Phiếu xuất kho (Lịch sử phiếu nhập/xuất)
        /// </summary>
        private void StockInOutMasterHistoryBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmStockInOutMasterHistory>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form lịch sử phiếu nhập/xuất kho");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Sản phẩm - Dịch vụ (Lịch sử sản phẩm nhập/xuất)
        /// </summary>
        private void StockInOutProductHistoryBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmStockInOutProductHistory>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form lịch sử sản phẩm nhập/xuất kho");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Bảo hành
        /// </summary>
        private void WarrantyCheckBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmWarrantyCheck>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form kiểm tra bảo hành");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Hình ảnh
        /// </summary>
        private void StockInOutImagesBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmStockInOutImageLookup>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý hình ảnh");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Chứng từ
        /// </summary>
        private void StockInOutDocumentBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmStockInOutDocumentDtoLookup>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý chứng từ");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Danh sách Thiết bị - Tài sản
        /// </summary>
        private void DeviceDtoMangementBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmDeviceDtoMangement>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý thiết bị - tài sản");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút Định danh sản phẩm hàng hóa
        /// </summary>
        private void DinhDanhSpHhBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmProductVariantIdentifier>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý định danh sản phẩm hàng hóa");
            }
        }

        #endregion

        #endregion

        #region Version and User Management

        /// <summary>
        /// Xử lý sự kiện click nút AllowedMacAddress - hiển thị form quản lý MAC address được phép
        /// </summary>
        private void AllowedMacAddressBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmAllowedMacAddressDto>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý MAC address");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút ApplicationUser - hiển thị form quản lý người dùng
        /// </summary>
        private void ApplicationUserBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmApplicationUserDto>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý người dùng");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút ApplicationVersion - hiển thị form quản lý phiên bản ứng dụng
        /// </summary>
        private void ApplicationVersionBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();
                ApplicationSystemUtils.ShowOrActivateForm<FrmApplicationVersionDto>(this);
                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form quản lý phiên bản ứng dụng");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút DatabaseConfig - hiển thị form cấu hình database
        /// </summary>
        private void DatabaseConfigBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ShowDatabaseConfigForm();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form cấu hình database");
            }
        }

        /// <summary>
        /// Xử lý sự kiện click nút NasConfig - hiển thị form cấu hình NAS
        /// </summary>
        private void NasConfigBarButtonItem_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                ShowNASConfigForm();
            }
            catch (Exception ex)
            {
                MsgBox.ShowException(ex, "Lỗi hiển thị form cấu hình NAS");
            }
        }

        /// <summary>
        /// Hiển thị form cấu hình NAS và xử lý kết quả
        /// </summary>
        private void ShowNASConfigForm()
        {
            using (OverlayManager.ShowScope(this))
            {
                using (var configForm = new FrmNASConfig())
                {
                    var result = configForm.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        MsgBox.ShowSuccess("Cấu hình NAS đã được cập nhật.", "Thông báo");
                    }
                }
            }
        }

        #endregion

    }
}