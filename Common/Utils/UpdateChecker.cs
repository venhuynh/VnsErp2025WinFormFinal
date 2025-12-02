using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Common.Common;
using Timer = System.Threading.Timer;

namespace Common.Utils
{
    /// <summary>
    /// Class kiểm tra cập nhật tự động
    /// </summary>
    public class UpdateChecker
    {
        #region ========== FIELDS ==========

        private readonly ApplicationVersionService _versionService;
        private Timer _checkTimer;
        private bool _isChecking;

        #endregion

        #region ========== PROPERTIES ==========

        /// <summary>
        /// Khoảng thời gian kiểm tra (giờ)
        /// </summary>
        public int CheckIntervalHours { get; set; } = 24;

        /// <summary>
        /// Kiểm tra khi khởi động
        /// </summary>
        public bool CheckOnStartup { get; set; } = true;

        /// <summary>
        /// Tự động cập nhật (không hỏi người dùng)
        /// </summary>
        public bool AutoUpdateEnabled { get; set; } = false;

        #endregion

        #region ========== EVENTS ==========

        /// <summary>
        /// Sự kiện khi phát hiện bản cập nhật mới
        /// </summary>
        public event EventHandler<UpdateAvailableEventArgs> UpdateAvailable;

        /// <summary>
        /// Sự kiện khi kiểm tra cập nhật hoàn tất
        /// </summary>
        public event EventHandler<UpdateCheckCompletedEventArgs> CheckCompleted;

        #endregion

        #region ========== CONSTRUCTOR ==========

        public UpdateChecker()
        {
            _versionService = new ApplicationVersionService();
            LoadSettings();
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Bắt đầu kiểm tra cập nhật định kỳ
        /// </summary>
        public void StartPeriodicCheck()
        {
            if (_checkTimer != null)
                return;

            var interval = TimeSpan.FromHours(CheckIntervalHours);
            _checkTimer = new Timer(OnTimerTick, null, interval, interval);
        }

        /// <summary>
        /// Dừng kiểm tra cập nhật định kỳ
        /// </summary>
        public void StopPeriodicCheck()
        {
            _checkTimer?.Dispose();
            _checkTimer = null;
        }

        /// <summary>
        /// Kiểm tra cập nhật ngay lập tức
        /// </summary>
        public async Task<bool> CheckNowAsync(bool showNoUpdateMessage = false)
        {
            if (_isChecking)
                return false;

            _isChecking = true;

            try
            {
                var hasUpdate = await _versionService.CheckForUpdatesAsync();
                
                if (hasUpdate)
                {
                    var latestVersionInfo = await _versionService.GetLatestVersionInfoAsync();
                    if (latestVersionInfo != null)
                    {
                        OnUpdateAvailable(latestVersionInfo);
                        return true;
                    }
                }
                else if (showNoUpdateMessage)
                {
                    MsgBox.ShowSuccess("Bạn đang sử dụng phiên bản mới nhất.", "Kiểm tra cập nhật");
                }

                OnCheckCompleted(hasUpdate);
                return hasUpdate;
            }
            catch (Exception ex)
            {
                // Chỉ log ở Debug level, không spam log khi không có update server
                System.Diagnostics.Debug.WriteLine($"Lỗi kiểm tra cập nhật: {ex.Message}");
                
                // Chỉ hiển thị thông báo nếu user yêu cầu và lỗi không phải là network error
                if (showNoUpdateMessage && !(ex is System.Net.Http.HttpRequestException || ex is System.Net.WebException))
                {
                    MsgBox.ShowWarning("Không thể kiểm tra cập nhật. Vui lòng thử lại sau.", "Lỗi kiểm tra cập nhật");
                }
                return false;
            }
            finally
            {
                _isChecking = false;
            }
        }

        /// <summary>
        /// Kiểm tra khi khởi động ứng dụng
        /// </summary>
        public async Task CheckOnStartupAsync()
        {
            if (!CheckOnStartup)
                return;

            // Đợi một chút để form chính load xong
            await Task.Delay(2000);

            // Kiểm tra ở background, không block UI
            _ = Task.Run(async () =>
            {
                try
                {
                    await CheckNowAsync(false);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi kiểm tra cập nhật khi khởi động: {ex.Message}");
                }
            });
        }

        #endregion

        #region ========== PRIVATE METHODS ==========

        private void OnTimerTick(object state)
        {
            _ = Task.Run(async () =>
            {
                try
                {
                    await CheckNowAsync(false);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Lỗi kiểm tra cập nhật định kỳ: {ex.Message}");
                }
            });
        }

        private void OnUpdateAvailable(VersionInfo versionInfo)
        {
            UpdateAvailable?.Invoke(this, new UpdateAvailableEventArgs
            {
                VersionInfo = versionInfo,
                CurrentVersion = _versionService.GetCurrentVersionString()
            });
        }

        private void OnCheckCompleted(bool hasUpdate)
        {
            CheckCompleted?.Invoke(this, new UpdateCheckCompletedEventArgs
            {
                HasUpdate = hasUpdate,
                CurrentVersion = _versionService.GetCurrentVersionString()
            });
        }

        private void LoadSettings()
        {
            try
            {
                var checkInterval = System.Configuration.ConfigurationManager.AppSettings["UpdateCheckInterval"];
                if (!string.IsNullOrEmpty(checkInterval) && int.TryParse(checkInterval, out var hours))
                {
                    CheckIntervalHours = hours;
                }

                var checkOnStartup = System.Configuration.ConfigurationManager.AppSettings["CheckUpdateOnStartup"];
                if (!string.IsNullOrEmpty(checkOnStartup) && bool.TryParse(checkOnStartup, out var onStartup))
                {
                    CheckOnStartup = onStartup;
                }

                var autoUpdate = System.Configuration.ConfigurationManager.AppSettings["AutoUpdateEnabled"];
                if (!string.IsNullOrEmpty(autoUpdate) && bool.TryParse(autoUpdate, out var auto))
                {
                    AutoUpdateEnabled = auto;
                }
            }
            catch
            {
                // Sử dụng giá trị mặc định
            }
        }

        #endregion
    }

    #region ========== EVENT ARGS ==========

    public class UpdateAvailableEventArgs : EventArgs
    {
        public VersionInfo VersionInfo { get; set; }
        public string CurrentVersion { get; set; }
    }

    public class UpdateCheckCompletedEventArgs : EventArgs
    {
        public bool HasUpdate { get; set; }
        public string CurrentVersion { get; set; }
    }

    #endregion
}

