using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Bll.Utils;
using Bll.Authentication;
using Dal.DataAccess;
using Dal.DataContext;

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
        #endregion
    }
}