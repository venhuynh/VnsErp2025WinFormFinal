using DevExpress.XtraSplashScreen;
using System;
using System.Threading.Tasks;
using Bll.Common;


    /// <summary>
    /// Helper class để quản lý splash screen với async/await support và progress tracking
    /// </summary>
    public static class SplashScreenHelper
    {
        #region Fields

        /// <summary>
        /// Splash screen hiện tại
        /// </summary>
        private static SplashScreenManager _currentSplashScreen;

        #endregion

        #region Basic Operations

        /// <summary>
        /// Đóng màn hình chờ hiện tại
        /// </summary>
        public static void CloseSplashScreen()
        {
            try
            {
                if (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible)
                {
                    SplashScreenManager.CloseForm();
                }

                if (_currentSplashScreen != null && _currentSplashScreen.IsSplashFormVisible)
                {
                    SplashScreenManager.CloseForm();
                    _currentSplashScreen = null;
                }
            }
            catch (Exception ex)
            {
                // Silent error handling
            }
        }

        /// <summary>
        /// Kiểm tra xem có splash screen nào đang hiển thị không
        /// </summary>
        /// <returns>True nếu có splash screen đang hiển thị</returns>
        public static bool IsSplashScreenVisible()
        {
            return (SplashScreenManager.Default != null && SplashScreenManager.Default.IsSplashFormVisible) ||
                   (_currentSplashScreen != null && _currentSplashScreen.IsSplashFormVisible);
        }

        #endregion

        #region VNS Splash Screen

        /// <summary>
        /// Hiển thị màn hình chờ của VNS
        /// </summary>
        /// <param name="title">Tiêu đề splash screen</param>
        /// <param name="subtitle">Phụ đề</param>
        public static void ShowVnsSplashScreen(string title = "VnsErp2025", string subtitle = "Hệ thống quản lý doanh nghiệp")
        {
            try
            {
                // Đóng splash screen hiện tại trước
                CloseSplashScreen();

                // Hiển thị VNS splash screen
                SplashScreenManager.ShowForm(typeof(Bll.Common.VnsSplashScreen));
            }
            catch (Exception ex)
            {
                // Silent error handling
            }
        }

        /// <summary>
        /// Hiển thị màn hình chờ của VNS với progress tracking
        /// </summary>
        /// <param name="title">Tiêu đề splash screen</param>
        /// <param name="subtitle">Phụ đề</param>
        /// <param name="showProgress">Hiển thị progress bar</param>
        public static void ShowVnsSplashScreenWithProgress(string title = "VnsErp2025", string subtitle = "Đang khởi tạo hệ thống...", bool showProgress = true)
        {
            try
            {
                // Đóng splash screen hiện tại trước
                CloseSplashScreen();

                // Hiển thị VNS splash screen
                SplashScreenManager.ShowForm(typeof(Bll.Common.VnsSplashScreen));
            }
            catch (Exception ex)
            {
                // Silent error handling
            }
        }

        #endregion


        #region Async Operations

        /// <summary>
        /// Thực hiện operation với splash screen
        /// </summary>
        /// <param name="operation">Operation cần thực hiện</param>
        /// <param name="title">Tiêu đề splash screen</param>
        /// <param name="description">Mô tả</param>
        /// <param name="showProgress">Hiển thị progress bar</param>
        public static async Task ExecuteWithSplashScreenAsync(
            Func<Task> operation,
            string title = "VnsErp2025",
            string description = "Đang xử lý...",
            bool showProgress = true)
        {
            try
            {
                // Hiển thị splash screen
                ShowVnsSplashScreenWithProgress(title, description, showProgress);
                
                // Thực hiện operation
                await operation();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // Đóng splash screen
                CloseSplashScreen();
            }
        }

        /// <summary>
        /// Thực hiện operation với splash screen và progress tracking
        /// </summary>
        /// <param name="operation">Operation cần thực hiện với progress callback</param>
        /// <param name="title">Tiêu đề splash screen</param>
        /// <param name="description">Mô tả</param>
        public static async Task ExecuteWithProgressAsync(
            Func<Action<int, string>, Task> operation,
            string title = "VnsErp2025",
            string description = "Đang xử lý...")
        {
            try
            {
                // Hiển thị splash screen với progress
                ShowVnsSplashScreenWithProgress(title, description, true);
                
                // Tạo progress callback (không làm gì)
                Action<int, string> progressCallback = (progress, status) =>
                {
                    // Silent progress handling
                };
                
                // Thực hiện operation
                await operation(progressCallback);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                // Đóng splash screen
                CloseSplashScreen();
            }
        }

        #endregion

        #region Legacy Support

        /// <summary>
        /// Hiển thị màn hình chờ xử lý dữ liệu (legacy)
        /// </summary>
        [Obsolete("Sử dụng ShowVnsSplashScreen thay thế")]
        public static void ShowWaitingSplashScreen()
        {
            try
            {
                // Đóng trước khi mở cái mới
                CloseSplashScreen();
                SplashScreenManager.ShowForm(typeof(Bll.Common.WaitForm1));
            }
            catch (Exception ex)
            {
                // Silent error handling
            }
        }

        #endregion
    }

