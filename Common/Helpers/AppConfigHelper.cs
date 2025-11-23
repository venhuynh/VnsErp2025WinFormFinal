using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Common.Appconfig;

namespace Common.Helpers
{
    /// <summary>
    /// Helper class để đọc và ghi App.config và NAS Config XML
    /// </summary>
    public static class AppConfigHelper
    {
        /// <summary>
        /// Đường dẫn file NAS Config XML
        /// </summary>
        private static string GetNASConfigFilePath()
        {
            try
            {
                var appDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var configDirectory = Path.Combine(appDirectory, "VNTA_Config");
                
                if (!Directory.Exists(configDirectory))
                {
                    Directory.CreateDirectory(configDirectory);
                }
                
                return Path.Combine(configDirectory, "NASConfig.xml");
            }
            catch
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "NASConfig.xml");
            }
        }
        /// <summary>
        /// Lấy đường dẫn file config thực tế
        /// </summary>
        private static string GetConfigFilePath()
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                return configFile.FilePath;
            }
            catch
            {
                // Fallback: sử dụng AppDomain.BaseDirectory
                var exePath = System.Reflection.Assembly.GetExecutingAssembly().Location;
                var exeConfigPath = exePath + ".config";
                return exeConfigPath;
            }
        }

        /// <summary>
        /// Lấy giá trị từ App.config hoặc NAS Config XML (ưu tiên XML)
        /// </summary>
        /// <param name="key">Key trong appSettings</param>
        /// <param name="defaultValue">Giá trị mặc định nếu không tìm thấy</param>
        /// <returns>Giá trị hoặc defaultValue</returns>
        public static string GetAppSetting(string key, string defaultValue = "")
        {
            try
            {
                // Nếu là NAS config key, ưu tiên đọc từ XML
                if (key.StartsWith("ImageStorage.NAS.") || key == "ImageStorage.StorageType")
                {
                    var nasConfig = LoadNASConfig();
                    if (nasConfig != null)
                    {
                        switch (key)
                        {
                            case "ImageStorage.StorageType":
                                return nasConfig.StorageType ?? defaultValue;
                            case "ImageStorage.NAS.ServerName":
                                return nasConfig.ServerName ?? defaultValue;
                            case "ImageStorage.NAS.ShareName":
                                return nasConfig.ShareName ?? defaultValue;
                            case "ImageStorage.NAS.BasePath":
                                return nasConfig.BasePath ?? defaultValue;
                            case "ImageStorage.NAS.Username":
                                return nasConfig.Username ?? defaultValue;
                            case "ImageStorage.NAS.Password":
                                return nasConfig.Password ?? defaultValue;
                            case "ImageStorage.NAS.Protocol":
                                return nasConfig.Protocol ?? defaultValue;
                            case "ImageStorage.NAS.ConnectionTimeout":
                                return nasConfig.ConnectionTimeout.ToString();
                            case "ImageStorage.NAS.RetryAttempts":
                                return nasConfig.RetryAttempts.ToString();
                            case "ImageStorage.NAS.EnableNASCache":
                                return nasConfig.EnableCache.ToString();
                            case "ImageStorage.NAS.NASCacheSize":
                                return nasConfig.CacheSize.ToString();
                        }
                    }
                }

                // Fallback: đọc từ App.config
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var setting = configFile.AppSettings.Settings[key];
                
                if (setting != null && !string.IsNullOrEmpty(setting.Value))
                {
                    return setting.Value;
                }
                
                // Fallback: thử đọc từ ConfigurationManager.AppSettings
                ConfigurationManager.RefreshSection("appSettings");
                var value = ConfigurationManager.AppSettings[key];
                return string.IsNullOrEmpty(value) ? defaultValue : value;
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Load NAS Config từ XML file
        /// </summary>
        public static NASConfigData LoadNASConfig()
        {
            try
            {
                var configFilePath = GetNASConfigFilePath();
                if (!File.Exists(configFilePath))
                {
                    return null;
                }

                return XmlHelper.LoadFromXmlToObject<NASConfigData>(configFilePath);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Save NAS Config vào XML file
        /// </summary>
        public static bool SaveNASConfig(NASConfigData config)
        {
            try
            {
                if (config == null)
                    return false;

                config.LastUpdated = DateTime.Now;
                var configFilePath = GetNASConfigFilePath();
                return XmlHelper.SaveToXml(configFilePath, config);
            }
            catch
            {
                return false;
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
        /// Ưu tiên lưu NAS config vào XML file để tránh bị ghi đè khi rebuild
        /// </summary>
        /// <param name="settings">Dictionary chứa key-value pairs</param>
        /// <returns>True nếu thành công</returns>
        public static bool UpdateAppSettings(System.Collections.Generic.Dictionary<string, string> settings)
        {
            try
            {
                // Kiểm tra xem có phải NAS config không
                bool isNASConfig = settings.ContainsKey("ImageStorage.StorageType") || 
                                   settings.Keys.Any(k => k.StartsWith("ImageStorage.NAS."));

                if (isNASConfig)
                {
                    // Lưu vào XML file (không bị ghi đè khi rebuild)
                    var nasConfig = LoadNASConfig() ?? new NASConfigData();
                    
                    foreach (var setting in settings)
                    {
                        switch (setting.Key)
                        {
                            case "ImageStorage.StorageType":
                                nasConfig.StorageType = setting.Value ?? "NAS";
                                break;
                            case "ImageStorage.NAS.ServerName":
                                nasConfig.ServerName = setting.Value ?? "";
                                break;
                            case "ImageStorage.NAS.ShareName":
                                nasConfig.ShareName = setting.Value ?? "ERP_Images";
                                break;
                            case "ImageStorage.NAS.BasePath":
                                nasConfig.BasePath = setting.Value ?? "";
                                break;
                            case "ImageStorage.NAS.Username":
                                nasConfig.Username = setting.Value ?? "";
                                break;
                            case "ImageStorage.NAS.Password":
                                nasConfig.Password = setting.Value ?? "";
                                break;
                            case "ImageStorage.NAS.Protocol":
                                nasConfig.Protocol = setting.Value ?? "SMB";
                                break;
                            case "ImageStorage.NAS.ConnectionTimeout":
                                if (int.TryParse(setting.Value, out int timeout))
                                    nasConfig.ConnectionTimeout = timeout;
                                break;
                            case "ImageStorage.NAS.RetryAttempts":
                                if (int.TryParse(setting.Value, out int retry))
                                    nasConfig.RetryAttempts = retry;
                                break;
                            case "ImageStorage.NAS.EnableNASCache":
                                if (bool.TryParse(setting.Value, out bool enableCache))
                                    nasConfig.EnableCache = enableCache;
                                break;
                            case "ImageStorage.NAS.NASCacheSize":
                                if (int.TryParse(setting.Value, out int cacheSize))
                                    nasConfig.CacheSize = cacheSize;
                                break;
                        }
                    }

                    var xmlFilePath = GetNASConfigFilePath();
                    var saved = SaveNASConfig(nasConfig);
                    System.Diagnostics.Debug.WriteLine($"AppConfigHelper: Đã lưu NAS config vào XML file: {xmlFilePath}, Success: {saved}");
                    
                    if (saved)
                    {
                        return true;
                    }
                }

                // Fallback: Lưu vào App.config (cho các config khác)
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
                System.Diagnostics.Debug.WriteLine($"AppConfigHelper: Lỗi khi cập nhật config: {ex.Message}");
                throw new Exception($"Không thể cập nhật config: {ex.Message}", ex);
            }
        }
    }
}


