using Authentication.Form;
using Bll.Utils;
using Dal.Connection;
using Dal.DataContext.SeedData.MasterData.ProductService;
using MasterData.Customer;
using MasterData.ProductService;
using System;
using System.Windows.Forms;
using Dal.DataContext.SeedData.MasterData.Company;
using Dal.DataContext.SeedData.MasterData.Customer;
using DevExpress.XtraEditors;
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

            #region Dành cho debug

            //SeedData_Master_Customer.DeleteAllPartnerData();
            //SeedData_Master_Customer.SeedAllData();

            //SeedData_Master_ProductService.DeleteAllProductServiceData();
            //SeedData_Master_ProductService.SeedAllData();

            //SeedData_Master_Company.DeleteAllCompanyData();
            //SeedData_Master_Company.CreateAllCompanyData();
            


            Application.Run(new FormMain());

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