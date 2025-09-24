﻿using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Bll.Common;
using Bll.Authentication;
using Dal.DataAccess;
using Dal.DataContext;

namespace Bll.Utils
{
    public class ApplicationSystemUtils
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

        #region thuocTinhDonGian

        /// <summary>
        /// Thông tin user hiện tại đã đăng nhập
        /// </summary>
        private static ApplicationUser _currentUser;

        #endregion

        #region phuongThuc

        /// <summary>
        /// Thiết lập user hiện tại đã đăng nhập
        /// </summary>
        /// <param name="user">Thông tin user</param>
        public static void SetCurrentUser(ApplicationUser user)
        {
            _currentUser = user;
        }

        /// <summary>
        /// Thiết lập user hiện tại từ UserInfo (từ LoginBll)
        /// </summary>
        /// <param name="userInfo">Thông tin user từ LoginBll</param>
        public static void SetCurrentUser(Bll.Authentication.UserInfo userInfo)
        {
            if (userInfo != null)
            {
                _currentUser = new ApplicationUser
                {
                    Id = userInfo.Id,
                    UserName = userInfo.UserName,
                    Active = userInfo.Active,
                    HashPassword = string.Empty // Không lưu password vào session
                };
            }
        }

        /// <summary>
        /// Lấy thông tin user hiện tại
        /// </summary>
        /// <returns>Thông tin user hiện tại hoặc null nếu chưa đăng nhập</returns>
        public static ApplicationUser GetCurrentUser()
        {
            return _currentUser;
        }

        /// <summary>
        /// Kiểm tra xem có user nào đang đăng nhập không
        /// </summary>
        /// <returns>True nếu có user đăng nhập, False nếu không</returns>
        public static bool IsUserLoggedIn()
        {
            return _currentUser != null && _currentUser.Active;
        }

        /// <summary>
        /// Đăng xuất user hiện tại
        /// </summary>
        public static void LogoutCurrentUser()
        {
            _currentUser = null;
        }

        /// <summary>
        /// Lấy tên user hiện tại
        /// </summary>
        /// <returns>Tên user hoặc "Unknown" nếu chưa đăng nhập</returns>
        public static string GetCurrentUserName()
        {
            return _currentUser?.UserName ?? "Unknown";
        }

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

        #endregion
    }
}