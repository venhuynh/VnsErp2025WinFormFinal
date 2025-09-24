using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Bll.Common;

namespace Bll.Utils
{
    internal class ApplicationSystemUtils
    {
        #region Khai báo singleton

        private static ApplicationSystemUtils _instance;

        // Để ngăn việc tạo thể hiện từ bên ngoài lớp
        private ApplicationSystemUtils()
        {
        }

        // Thực hiện Singleton bằng cách sử dụng thuộc tính Instance
        public static ApplicationSystemUtils Instance => _instance ??= new ApplicationSystemUtils();

        #endregion




        public string ConvertImageToBase64(Image image)
        {
            using (var stream = new MemoryStream())
            {
                // Save the image to the stream
                image.Save(stream, ImageFormat.Png);

                // Convert the stream data to a byte array
                byte[] bytes = stream.ToArray();

                // Encode the byte array as a base64 string
                string base64String = Convert.ToBase64String(bytes);

                return base64String;
            }
        }

        public static void ShowOrActivateForm<T>(Form mdiParent) where T : Form, new()
        {
            try
            {
                SplashScreenHelper.ShowVnsSplashScreen();

                bool isActive = false;
                foreach (Form form in Application.OpenForms.OfType<Form>().ToList())
                {
                    if (form.GetType() == typeof(T))
                    {
                        form.Activate();
                        isActive = true;
                    }
                }

                if (!isActive)
                {
                    T frm = new T();
                    frm.MdiParent = mdiParent;
                    frm.Show();

                }

                SplashScreenHelper.CloseSplashScreen();
            }
            catch (Exception ex)
            {
                SplashScreenHelper.CloseSplashScreen();
                MsgBox.ShowException(ex);
            }
        }


        public static string GetApplicationVersion()
        {
            return Application.ProductVersion;
        }

    }
}