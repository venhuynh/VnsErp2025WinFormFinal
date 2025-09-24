using System;
using Dal.Connection;

namespace Dal.Configuration
{
    /// <summary>
    /// Trình quản lý cấu hình tập trung (Configuration Manager) cho DAL.
    /// - Chịu trách nhiệm tải, cung cấp và cho phép override cấu hình database.
    /// - Đảm bảo thread-safe khi truy xuất cấu hình.
    /// </summary>
    public static class ConfigurationManager
    {
        #region Fields & Properties

        private static DatabaseSettings _databaseSettings;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Cấu hình database (singleton, thread-safe).
        /// </summary>
        public static DatabaseSettings DatabaseSettings
        {
            get
            {
                if (_databaseSettings == null)
                {
                    lock (_lockObject)
                    {
                        if (_databaseSettings == null)
                        {
                            _databaseSettings = LoadDatabaseSettings();
                        }
                    }
                }
                return _databaseSettings;
            }
        }

        #endregion

        #region Loaders

        /// <summary>
        /// Nạp cấu hình database từ AppSettings và hợp lệ hóa.
        /// </summary>
        private static DatabaseSettings LoadDatabaseSettings()
        {
            try
            {
                var settings = new DatabaseSettings
                {
                    ConnectionString = ConnectionStringHelper.LayConnectionStringMacDinh(),
                    CommandTimeout = GetIntFromConfig("Database.CommandTimeout", 30),
                    ConnectionTimeout = GetIntFromConfig("Database.ConnectionTimeout", 15),
                    EnableRetryOnFailure = GetBoolFromConfig("Database.EnableRetryOnFailure", true),
                    MaxRetryCount = GetIntFromConfig("Database.MaxRetryCount", 3),
                    RetryDelayMs = GetIntFromConfig("Database.RetryDelayMs", 1000),
                    EnableDetailedErrors = GetBoolFromConfig("Database.EnableDetailedErrors", false),
                    EnableSensitiveDataLogging = GetBoolFromConfig("Database.EnableSensitiveDataLogging", false),
                    EnablePerformanceMonitoring = GetBoolFromConfig("Database.EnablePerformanceMonitoring", true),
                    PerformanceThresholdMs = GetLongFromConfig("Database.PerformanceThresholdMs", 1000)
                };

                settings.Validate();
                return settings;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to load database settings: {ex.Message}", ex);
            }
        }

        #endregion

        #region Parsing Helpers

        /// <summary>
        /// Đọc giá trị int từ AppSettings với giá trị mặc định.
        /// </summary>
        private static int GetIntFromConfig(string key, int defaultValue)
        {
            try
            {
                var value = System.Configuration.ConfigurationManager.AppSettings[key];
                return string.IsNullOrWhiteSpace(value) ? defaultValue : int.Parse(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Đọc giá trị long từ AppSettings với giá trị mặc định.
        /// </summary>
        private static long GetLongFromConfig(string key, long defaultValue)
        {
            try
            {
                var value = System.Configuration.ConfigurationManager.AppSettings[key];
                return string.IsNullOrWhiteSpace(value) ? defaultValue : long.Parse(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Đọc giá trị bool từ AppSettings với giá trị mặc định.
        /// </summary>
        private static bool GetBoolFromConfig(string key, bool defaultValue)
        {
            try
            {
                var value = System.Configuration.ConfigurationManager.AppSettings[key];
                return string.IsNullOrWhiteSpace(value) ? defaultValue : bool.Parse(value);
            }
            catch
            {
                return defaultValue;
            }
        }

        #endregion

        #region Management

        /// <summary>
        /// Reload toàn bộ cấu hình (hữu ích cho test hoặc cập nhật động).
        /// Lần truy cập tiếp theo vào <see cref="DatabaseSettings"/> sẽ tự động nạp lại.
        /// </summary>
        public static void ReloadConfiguration()
        {
            lock (_lockObject)
            {
                _databaseSettings = null;
            }
        }

        /// <summary>
        /// Ghi đè cấu hình hiện tại (hữu ích cho test).
        /// </summary>
        /// <param name="settings">Đối tượng cấu hình hợp lệ</param>
        /// <exception cref="ArgumentNullException">Khi <paramref name="settings"/> null</exception>
        public static void OverrideSettings(DatabaseSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            settings.Validate();

            lock (_lockObject)
            {
                _databaseSettings = settings;
            }
        }

        #endregion
    }
}