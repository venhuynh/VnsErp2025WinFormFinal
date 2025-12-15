using Authentication.Form;
using Bll.Common;
using Common.Utils;
using Dal.Connection;
using Inventory.Management;
using Inventory.OverlayForm;
using Inventory.Query;
using Inventory.StockOut.XuatLapRap;
using Inventory.StockOut.XuatNoiBo;
using MasterData.Customer;
using System;
using System.Windows.Forms;
using VersionAndUserManagement.AllowedMacAddress;
using VnsErp2025.Form;


namespace VnsErp2025
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Cấu hình DevExpress Skin
            SkinHelper.KhoiTaoSkin("WXI");

            // 1) Tải connection string từ Settings (ConnectionManager default sẽ ưu tiên User Settings)
            var connectionManager = new ConnectionManager();

            // 2) Kiểm tra kết nối
            if (!connectionManager.TestConnection())
            {
                // 2a) Không kết nối được -> mở màn hình cấu hình DB
                using (var configForm = new FrmDatabaseConfig())
                {
                    configForm.ShowDialog();
                }

                // Thử kiểm tra lại sau khi người dùng lưu cấu hình
                connectionManager = new ConnectionManager();
                if (!connectionManager.TestConnection())
                {
                    // Không kết nối được sau cấu hình -> thoát
                    Application.Exit();
                    return;
                }
            }

            // 3) Sau khi test connection thành công, khởi tạo ApplicationStartupManager
            // Đảm bảo connection string được load vào ứng dụng để các BLL có thể sử dụng
            try
            {
                // Khởi tạo ApplicationStartupManager để load connection string vào ứng dụng
                // DatabaseConnectionManager sẽ tự động load từ file XML hoặc Registry
                if (!ApplicationStartupManager.Instance.InitializeApplication())
                {
                    // Nếu không thể khởi tạo, thử với connection string từ ConnectionManager
                    var connectionString = connectionManager.ConnectionString;
                    if (string.IsNullOrEmpty(connectionString))
                    {
                        MessageBox.Show(
                            @"Không thể khởi tạo connection string. Vui lòng kiểm tra lại cấu hình database.",
                            @"Lỗi khởi tạo",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                        Application.Exit();
                        return;
                    }
                    
                    System.Diagnostics.Debug.WriteLine($"Connection string từ ConnectionManager: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");
                }
                
                // Kiểm tra lại ApplicationStartupManager đã sẵn sàng chưa
                if (!ApplicationStartupManager.Instance.IsApplicationReady())
                {
                    MessageBox.Show(
                        @"Ứng dụng chưa sẵn sàng. Vui lòng khởi động lại.",
                        @"Lỗi khởi tạo",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $@"Lỗi khi khởi tạo ApplicationStartupManager: {ex.Message}",
                    @"Lỗi khởi tạo",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            // 4) Kiểm tra bảo mật ứng dụng (version và MAC address)
            try
            {
                var securityService = new ApplicationSecurityService();
                var securityResult = securityService.CheckSecurity();

                if (!securityResult.IsValid)
                {
                    securityService.ShowErrorAndExit(securityResult);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $@"Lỗi khi kiểm tra bảo mật ứng dụng: {ex.Message}\n\n" +
                    @"Vui lòng liên hệ quản trị viên để được hỗ trợ.",
                    @"Lỗi bảo mật",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                Application.Exit();
                return;
            }

            #region Dành cho debug

            //SeedData_Master_Customer.DeleteAllPartnerData();
            //SeedData_Master_Customer.SeedAllData();

            //SeedData_Master_ProductService.DeleteAllProductServiceData();
            //SeedData_Master_ProductService.SeedAllData();

            //SeedData_Master_Company.DeleteAllCompanyData();
            //SeedData_Master_Company.CreateAllCompanyData();
            


            Application.Run(new FrmAllowedMacAddressDto());

            #endregion

            #region Dành cho Release - bỏ comment

            // 3) Kết nối OK -> hiển thị màn hình đăng nhập
            //using (var loginForm = new FrmLogin())
            //{
            //    if (loginForm.ShowDialog() == DialogResult.OK)
            //    {
            //        // Đăng nhập thành công, hiển thị form chính
            //        Application.Run(new FormMain());
            //    }
            //    else
            //    {
            //        // Người dùng hủy đăng nhập, thoát ứng dụng
            //        Application.Exit();
            //    }
            //}


            #endregion

        }

    }
}