using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Authentication.Form;
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
            
            // Khởi tạo DevExpress Look and Feel
            DevExpress.UserSkins.BonusSkins.Register();
            DevExpress.Skins.SkinManager.EnableFormSkins();
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Office 2019 Colorful");
            
            // Hiển thị form đăng nhập
            using (var loginForm = new FrmLogin())
            {
                if (loginForm.ShowDialog() == DialogResult.OK)
                {
                    // Đăng nhập thành công, hiển thị form chính
                    Application.Run(new FormMain());
                }
                else
                {
                    // Người dùng hủy đăng nhập, thoát ứng dụng
                    Application.Exit();
                }
            }
        }
    }
}
