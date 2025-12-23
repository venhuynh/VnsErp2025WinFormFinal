using Authentication.Form;
using Bll.Common;
using Common.Appconfig;
using Common.Utils;
using Dal.Connection;
using Inventory.Management;
using Inventory.OverlayForm;
using Inventory.StockIn.NhapHangThuongMai;
using MasterData.ProductService;
using Microsoft.Win32;
using System;
using System.Windows.Forms;
using Inventory.Management.DeviceMangement;
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
            
            // 1) Kiểm tra xem có thông tin cấu hình database trong Registry không
            if (!KiemTraThongTinTuRegistry())
            {
                // Không có thông tin trong Registry -> hiển thị màn hình cài đặt
                MessageBox.Show(
                    @"Chưa có cấu hình cơ sở dữ liệu.\nVui lòng cài đặt thông tin kết nối cơ sở dữ liệu.",
                    @"Cấu hình cơ sở dữ liệu",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                
                using (var configForm = new FrmDatabaseConfig())
                {
                    if (configForm.ShowDialog() != DialogResult.OK)
                    {
                        // Người dùng hủy cấu hình -> thoát ứng dụng
                        Application.Exit();
                        return;
                    }
                }
                
                // Kiểm tra lại sau khi người dùng cấu hình
                if (!KiemTraThongTinTuRegistry())
                {
                    MessageBox.Show(
                        @"Không thể đọc thông tin cấu hình từ Registry.\nVui lòng kiểm tra lại quyền truy cập Registry.",
                        @"Lỗi cấu hình",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    Application.Exit();
                    return;
                }
            }
            
            // 2) Tải connection string từ Registry
            var connectionManager = new ConnectionManager();

            // 3) Kiểm tra kết nối
            if (!connectionManager.TestConnection())
            {
                // Không kết nối được -> mở màn hình cấu hình DB để sửa
                MessageBox.Show(
                    @"Không thể kết nối đến cơ sở dữ liệu.\nVui lòng kiểm tra lại thông tin kết nối.",
                    @"Lỗi kết nối",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                
                using (var configForm = new FrmDatabaseConfig())
                {
                    if (configForm.ShowDialog() != DialogResult.OK)
                    {
                        // Người dùng hủy -> thoát
                        Application.Exit();
                        return;
                    }
                }

                // Thử kiểm tra lại sau khi người dùng lưu cấu hình
                connectionManager = new ConnectionManager();
                if (!connectionManager.TestConnection())
                {
                    // Không kết nối được sau cấu hình -> thoát
                    MessageBox.Show(
                        @"Vẫn không thể kết nối đến cơ sở dữ liệu sau khi cấu hình.\nVui lòng kiểm tra lại thông tin kết nối.",
                        @"Lỗi kết nối",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
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
            


            Application.Run(new FrmDeviceDtoMangement());

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

        /// <summary>
        /// Kiểm tra xem có thể đọc được thông tin cấu hình database từ Registry không
        /// Tất cả thông tin được giải mã khi kiểm tra
        /// </summary>
        /// <returns>True nếu đọc được thông tin hợp lệ từ Registry, False nếu không</returns>
        private static bool KiemTraThongTinTuRegistry()
        {
            try
            {
                const string registryKey = @"HKEY_CURRENT_USER\Software\Software\VietNhatSolutions\VnsErp2025";
                
                // Đọc các giá trị đã mã hóa từ Registry
                var encryptedDns = (string)Registry.GetValue(registryKey, "dns", null);
                var encryptedDatabase = (string)Registry.GetValue(registryKey, "database", null);
                var encryptedUsername = (string)Registry.GetValue(registryKey, "username", null);
                var encryptedPassword = (string)Registry.GetValue(registryKey, "password", null);
                
                // Kiểm tra xem có các giá trị không
                if (string.IsNullOrEmpty(encryptedDns) || string.IsNullOrEmpty(encryptedDatabase))
                {
                    return false;
                }
                
                // Kiểm tra xem có username và password không (bắt buộc vì dùng SQL Authentication)
                if (string.IsNullOrEmpty(encryptedUsername) || string.IsNullOrEmpty(encryptedPassword))
                {
                    return false;
                }
                
                // Thử giải mã tất cả các trường để đảm bảo chúng hợp lệ
                try
                {
                    var decryptedDns = VntaCrypto.Decrypt(encryptedDns);
                    var decryptedDatabase = VntaCrypto.Decrypt(encryptedDatabase);
                    var decryptedUsername = VntaCrypto.Decrypt(encryptedUsername);
                    var decryptedPassword = VntaCrypto.Decrypt(encryptedPassword);
                    
                    // Kiểm tra xem sau khi giải mã có giá trị hợp lệ không
                    if (string.IsNullOrEmpty(decryptedDns) || string.IsNullOrEmpty(decryptedDatabase) ||
                        string.IsNullOrEmpty(decryptedUsername) || string.IsNullOrEmpty(decryptedPassword))
                    {
                        return false;
                    }
                }
                catch
                {
                    // Không thể giải mã -> có thể là dữ liệu cũ chưa mã hóa, thử kiểm tra trực tiếp
                    // Nếu không phải định dạng mã hóa, VntaCrypto.Decrypt sẽ trả về nguyên vẹn
                    // Nhưng để đảm bảo an toàn, chúng ta yêu cầu tất cả phải được mã hóa
                    return false;
                }
                
                return true;
            }
            catch (Exception)
            {
                // Có lỗi khi đọc Registry -> không hợp lệ
                return false;
            }
        }

    }
}