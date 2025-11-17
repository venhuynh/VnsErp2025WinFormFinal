using DevExpress.LookAndFeel;
using DevExpress.Skins;
using DevExpress.UserSkins;
using System;

namespace Bll.Utils
{
    /// <summary>
    /// Helper class để quản lý DevExpress Skin cho toàn bộ ứng dụng
    /// </summary>
    public static class SkinHelper
    {
        #region thuocTinhDonGian

        private static bool _daKhoiTao = false;
        private static readonly object _lockObject = new object();

        #endregion

        #region phuongThuc

        /// <summary>
        /// Khởi tạo DevExpress Skin cho ứng dụng
        /// </summary>
        /// <param name="skinName">Tên skin (mặc định: WXI)</param>
        public static void KhoiTaoSkin(string skinName = "WXI")
        {
            lock (_lockObject)
            {
                if (_daKhoiTao)
                    return;

                try
                {
                    // Đăng ký Bonus Skins
                    BonusSkins.Register();
                    
                    // Thiết lập skin mặc định
                    UserLookAndFeel.Default.SetSkinStyle(skinName);
                    
                    // Cấu hình thêm nếu cần
                    UserLookAndFeel.Default.UseWindowsXPTheme = false;
                    
                    _daKhoiTao = true;
                    
                    System.Diagnostics.Debug.WriteLine($"DevExpress Skin '{skinName}' đã được khởi tạo thành công.");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi khởi tạo DevExpress Skin: {ex.Message}");
                    throw new InvalidOperationException($"Không thể khởi tạo DevExpress Skin '{skinName}': {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Thiết lập skin cho form cụ thể
        /// </summary>
        /// <param name="form">Form cần thiết lập skin</param>
        /// <param name="skinName">Tên skin</param>
        public static void ThietLapSkinChoForm(System.Windows.Forms.Form form, string skinName = "WXI")
        {
            try
            {
                if (form == null)
                    return;

                // Đảm bảo skin đã được khởi tạo
                if (!_daKhoiTao)
                {
                    KhoiTaoSkin(skinName);
                }

                // Thiết lập skin cho form
                if (form is DevExpress.XtraEditors.XtraForm xtraForm)
                {
                    xtraForm.LookAndFeel.SetSkinStyle(skinName);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi thiết lập skin cho form: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy danh sách các skin có sẵn
        /// </summary>
        /// <returns>Mảng tên các skin</returns>
        public static string[] LayDanhSachSkin()
        {
            try
            {
                var skinContainers = SkinManager.Default.Skins;
                var skinNames = new string[skinContainers.Count];
                for (int i = 0; i < skinContainers.Count; i++)
                {
                    skinNames[i] = skinContainers[i].SkinName;
                }
                return skinNames;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi lấy danh sách skin: {ex.Message}");
                return new string[] { "WXI", "Office 2019 Colorful", "Office 2019 Dark Gray", "Office 2019 White" };
            }
        }

        /// <summary>
        /// Kiểm tra skin có tồn tại không
        /// </summary>
        /// <param name="skinName">Tên skin cần kiểm tra</param>
        /// <returns>True nếu skin tồn tại</returns>
        public static bool KiemTraSkinTonTai(string skinName)
        {
            try
            {
                var skins = LayDanhSachSkin();
                return Array.Exists(skins, skin => skin.Equals(skinName, StringComparison.OrdinalIgnoreCase));
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Thiết lập skin từ cấu hình
        /// </summary>
        public static void ThietLapSkinTuCauHinh()
        {
            try
            {
                // Đọc skin từ App.config hoặc Settings
                string skinName = "WXI"; // Mặc định
                
                // Có thể đọc từ ConfigurationManager.AppSettings["DefaultSkin"]
                // hoặc từ Settings.Default.DefaultSkin
                
                KhoiTaoSkin(skinName);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi thiết lập skin từ cấu hình: {ex.Message}");
                // Fallback về skin mặc định
                KhoiTaoSkin("WXI");
            }
        }

        /// <summary>
        /// Reset về skin mặc định
        /// </summary>
        public static void ResetVeSkinMacDinh()
        {
            try
            {
                UserLookAndFeel.Default.SetSkinStyle("WXI");
                System.Diagnostics.Debug.WriteLine("Đã reset về skin mặc định WXI.");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi reset skin: {ex.Message}");
            }
        }

        /// <summary>
        /// Lấy thông tin skin hiện tại
        /// </summary>
        /// <returns>Thông tin skin hiện tại</returns>
        public static string LayThongTinSkinHienTai()
        {
            try
            {
                return UserLookAndFeel.Default.SkinName;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi lấy thông tin skin: {ex.Message}");
                return "Unknown";
            }
        }

        #endregion
    }
}
