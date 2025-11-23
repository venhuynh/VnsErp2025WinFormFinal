using System;
using System.Configuration;

namespace Common.Helpers
{
    /// <summary>
    /// Helper class để đọc và ghi App.config
    /// </summary>
    public static class AppConfigHelper
    {
        /// <summary>
        /// Lấy giá trị từ App.config
        /// </summary>
        /// <param name="key">Key trong appSettings</param>
        /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
        /// <returns>Giá trị hoặc defaultValue</returns>
        public static string GetAppSetting(string key, string defaultValue = "")
        {
            try
            {
                var value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Cập nhật giá trị trong App.config
        /// </summary>
        /// <param name="key">Key trong appSettings</param>
        /// <param name="value">Giá trị mới</param>
        /// <returns>True nếu thành công</returns>
        public static bool UpdateAppSetting(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;

                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value ?? string.Empty;
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể cập nhật App.config: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật nhiều giá trị cùng lúc
        /// </summary>
        /// <param name="settings">Dictionary chứa key-value pairs</param>
        /// <returns>True nếu thành công</returns>
        public static bool UpdateAppSettings(System.Collections.Generic.Dictionary<string, string> settings)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var appSettings = configFile.AppSettings.Settings;

                foreach (var setting in settings)
                {
                    if (appSettings[setting.Key] == null)
                    {
                        appSettings.Add(setting.Key, setting.Value ?? string.Empty);
                    }
                    else
                    {
                        appSettings[setting.Key].Value = setting.Value ?? string.Empty;
                    }
                }

                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");

                return true;
            }
            catch (Exception ex)
            {
                throw new Exception($"Không thể cập nhật App.config: {ex.Message}", ex);
            }
        }
    }
}

