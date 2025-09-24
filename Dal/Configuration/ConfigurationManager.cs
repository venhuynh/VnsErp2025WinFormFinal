using System;
using Dal.Connection;

namespace Dal.Configuration
{
    /// <summary>
    /// Centralized configuration manager
    /// </summary>
    public static class ConfigurationManager
    {
        #region thuocTinhDonGian

        private static DatabaseSettings _databaseSettings;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Database settings (thread-safe singleton)
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

        #region phuongThuc

        /// <summary>
        /// Load database settings from configuration
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

        /// <summary>
        /// Get integer value from configuration with default
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
        /// Get long value from configuration with default
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
        /// Get boolean value from configuration with default
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

        /// <summary>
        /// Reload configuration (useful for testing or dynamic updates)
        /// </summary>
        public static void ReloadConfiguration()
        {
            lock (_lockObject)
            {
                _databaseSettings = null; // Will be reloaded on next access
            }
        }

        /// <summary>
        /// Override settings (useful for testing)
        /// </summary>
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
