using Dal.DataContext;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Common.Utils;
using DTO.VersionAndUserManagementDto;

namespace Bll.Common
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
        private static ApplicationUserDto _currentUser;

        #endregion

        #region phuongThuc

        /// <summary>
        /// Thiết lập user hiện tại đã đăng nhập
        /// </summary>
        /// <param name="user">Thông tin user</param>
        public static void SetCurrentUser(ApplicationUserDto user)
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
                _currentUser = new ApplicationUserDto
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
        public static ApplicationUserDto GetCurrentUser()
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
            using var stream = new MemoryStream();
            // Save the image to the stream
            image.Save(stream, ImageFormat.Png);

            // Convert the stream data to a byte array
            byte[] bytes = stream.ToArray();

            // Encode the byte array as a base64 string
            string base64String = Convert.ToBase64String(bytes);

            return base64String;
        }

        public static void ShowOrActivateForm<T>(Form mdiParent) where T : Form, new()
        {
            try
            {
                // Đảm bảo thao tác UI được thực hiện trên UI thread
                if (mdiParent.InvokeRequired)
                {
                    mdiParent.Invoke(new Action(() => ShowOrActivateForm<T>(mdiParent)));
                    return;
                }

                

                bool isActive = false;
                foreach (Form form in Application.OpenForms.OfType<Form>().ToList())
                {
                    if (form.GetType() == typeof(T))
                    {
                        form.Activate();
                        isActive = true;
                        break;
                    }
                }

                if (!isActive)
                {
                    T frm = new T();
                    
                    // Thiết lập icon cho form
                    ApplicationIconHelper.SetFormIcon(frm);
                    
                    // Chỉ thiết lập MdiParent nếu form parent là MDI Container
                    if (mdiParent.IsMdiContainer)
                    {
                        frm.MdiParent = mdiParent;
                    }
                    
                    frm.Show();
                }

                
            }
            catch (Exception ex)
            {
                
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