using System;
using System.Collections.Generic;
using System.Configuration;

namespace Dal.Connection
{
    /// <summary>
    /// Cấu hình Database cấp ứng dụng: lưu thông số kết nối, pooling, cache và môi trường.
    /// - Cung cấp API nạp/ghi cấu hình từ App.config, tạo connection string theo profile.
    /// - Hỗ trợ singleton truy cập nhanh và đảm bảo giá trị hợp lệ trước khi sử dụng.
    /// </summary>
    public class DatabaseConfig
    {
        #region Fields & Properties

        private static DatabaseConfig _instance;
        private static readonly object _lockObject = new object();

        /// <summary>
        /// Tên server database
        /// </summary>
        public string ServerName { get; set; } = "localhost";

        /// <summary>
        /// Tên database
        /// </summary>
        public string DatabaseName { get; set; } = "VnsErp2025";

        /// <summary>
        /// Sử dụng Windows Authentication
        /// </summary>
        public bool UseIntegratedSecurity { get; set; } = true;

        /// <summary>
        /// User ID (nếu không dùng Windows Auth)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Password (nếu không dùng Windows Auth)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Connection timeout (giây)
        /// </summary>
        public int ConnectionTimeout { get; set; } = 15;

        /// <summary>
        /// Command timeout (giây)
        /// </summary>
        public int CommandTimeout { get; set; } = 30;

        /// <summary>
        /// Enable connection pooling
        /// </summary>
        public bool EnablePooling { get; set; } = true;

        /// <summary>
        /// Minimum pool size
        /// </summary>
        public int MinPoolSize { get; set; } = 1;

        /// <summary>
        /// Maximum pool size
        /// </summary>
        public int MaxPoolSize { get; set; } = 100;

        /// <summary>
        /// Connection lifetime (giây)
        /// </summary>
        public int ConnectionLifetime { get; set; } = 0;

        /// <summary>
        /// Enable enlist in distributed transaction
        /// </summary>
        public bool EnlistInDistributedTransaction { get; set; } = true;

        /// <summary>
        /// Environment hiện tại
        /// </summary>
        public string Environment { get; set; } = "Development";

        /// <summary>
        /// Enable logging SQL queries
        /// </summary>
        public bool EnableSqlLogging { get; set; } = true;

        /// <summary>
        /// Log level (1=Error, 2=Warning, 3=Info, 4=Debug)
        /// </summary>
        public int LogLevel { get; set; } = 3;

        /// <summary>
        /// Enable performance monitoring
        /// </summary>
        public bool EnablePerformanceMonitoring { get; set; } = true;

        /// <summary>
        /// Slow query threshold (ms)
        /// </summary>
        public int SlowQueryThreshold { get; set; } = 1000;

        /// <summary>
        /// Enable caching
        /// </summary>
        public bool EnableCaching { get; set; } = true;

        /// <summary>
        /// Cache timeout (phút)
        /// </summary>
        public int CacheTimeout { get; set; } = 30;

        /// <summary>
        /// Maximum cache size (MB)
        /// </summary>
        public int MaxCacheSize { get; set; } = 100;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public DatabaseConfig()
        {
            LoadFromConfig();
        }

        /// <summary>
        /// Constructor với tham số cơ bản
        /// </summary>
        public DatabaseConfig(string serverName, string databaseName, bool useIntegratedSecurity = true)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UseIntegratedSecurity = useIntegratedSecurity;
        }

        #endregion

        #region Singleton Accessor

        /// <summary>
        /// Singleton instance
        /// </summary>
        public static DatabaseConfig Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseConfig();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region Loaders

        /// <summary>
        /// Tải cấu hình từ App.config
        /// </summary>
        public void LoadFromConfig()
        {
            try
            {
                ServerName = GetConfigValue("DatabaseServer", ServerName);
                DatabaseName = GetConfigValue("DatabaseName", DatabaseName);
                UseIntegratedSecurity = bool.Parse(GetConfigValue("UseIntegratedSecurity", UseIntegratedSecurity.ToString()));
                UserId = GetConfigValue("DatabaseUserId", UserId);
                Password = GetConfigValue("DatabasePassword", Password);
                
                ConnectionTimeout = int.Parse(GetConfigValue("ConnectionTimeout", ConnectionTimeout.ToString()));
                CommandTimeout = int.Parse(GetConfigValue("CommandTimeout", CommandTimeout.ToString()));
                
                EnablePooling = bool.Parse(GetConfigValue("EnablePooling", EnablePooling.ToString()));
                MinPoolSize = int.Parse(GetConfigValue("MinPoolSize", MinPoolSize.ToString()));
                MaxPoolSize = int.Parse(GetConfigValue("MaxPoolSize", MaxPoolSize.ToString()));
                ConnectionLifetime = int.Parse(GetConfigValue("ConnectionLifetime", ConnectionLifetime.ToString()));
                EnlistInDistributedTransaction = bool.Parse(GetConfigValue("EnlistInDistributedTransaction", EnlistInDistributedTransaction.ToString()));
                
                Environment = GetConfigValue("Environment", Environment);
                
                EnableSqlLogging = bool.Parse(GetConfigValue("EnableSqlLogging", EnableSqlLogging.ToString()));
                LogLevel = int.Parse(GetConfigValue("LogLevel", LogLevel.ToString()));
                
                EnablePerformanceMonitoring = bool.Parse(GetConfigValue("EnablePerformanceMonitoring", EnablePerformanceMonitoring.ToString()));
                SlowQueryThreshold = int.Parse(GetConfigValue("SlowQueryThreshold", SlowQueryThreshold.ToString()));
                
                EnableCaching = bool.Parse(GetConfigValue("EnableCaching", EnableCaching.ToString()));
                CacheTimeout = int.Parse(GetConfigValue("CacheTimeout", CacheTimeout.ToString()));
                MaxCacheSize = int.Parse(GetConfigValue("MaxCacheSize", MaxCacheSize.ToString()));
            }
            catch (Exception)
            {
                // TODO: log nếu cần; không throw để không chặn khởi tạo ứng dụng
            }
        }

        /// <summary>
        /// Lấy giá trị cấu hình từ AppSettings, có fallback mặc định.
        /// </summary>
        private string GetConfigValue(string key, string defaultValue)
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

        #endregion

        #region Factory Methods

        /// <summary>
        /// Lấy connection string hiện tại theo cấu hình.
        /// </summary>
        public string GetConnectionString()
        {
            return ConnectionStringHelper.BuildDetailedConnectionString(
                ServerName, DatabaseName, UseIntegratedSecurity, UserId, Password,
                ConnectionTimeout, CommandTimeout, EnablePooling, MinPoolSize, MaxPoolSize);
        }

        #endregion

        #region Environment Profiles

        /// <summary>
        /// Thiết lập profile môi trường (Development/Testing/Staging/Production).
        /// </summary>
        public void SetEnvironment(string environment)
        {
            Environment = environment ?? "Development";
            
            switch (Environment.ToLower())
            {
                case "development":
                case "dev":
                    ConfigureDevelopment();
                    break;
                    
                case "testing":
                case "test":
                    ConfigureTesting();
                    break;
                    
                case "staging":
                    ConfigureStaging();
                    break;
                    
                case "production":
                case "prod":
                    ConfigureProduction();
                    break;
                    
                default:
                    ConfigureDevelopment();
                    break;
            }
        }

        /// <summary>
        /// Thiết lập cấu hình cho Development
        /// </summary>
        private void ConfigureDevelopment()
        {
            ServerName = "localhost";
            DatabaseName = "VnsErp2025_Dev";
            UseIntegratedSecurity = true;
            EnableSqlLogging = true;
            LogLevel = 4; // Debug
            EnablePerformanceMonitoring = true;
            SlowQueryThreshold = 500;
            EnableCaching = false;
        }

        /// <summary>
        /// Thiết lập cấu hình cho Testing
        /// </summary>
        private void ConfigureTesting()
        {
            ServerName = "localhost";
            DatabaseName = "VnsErp2025_Test";
            UseIntegratedSecurity = true;
            EnableSqlLogging = true;
            LogLevel = 3; // Info
            EnablePerformanceMonitoring = true;
            SlowQueryThreshold = 1000;
            EnableCaching = true;
            CacheTimeout = 15;
        }

        /// <summary>
        /// Thiết lập cấu hình cho Staging
        /// </summary>
        private void ConfigureStaging()
        {
            ServerName = "staging-server";
            DatabaseName = "VnsErp2025_Staging";
            UseIntegratedSecurity = false;
            EnableSqlLogging = true;
            LogLevel = 2; // Warning
            EnablePerformanceMonitoring = true;
            SlowQueryThreshold = 2000;
            EnableCaching = true;
            CacheTimeout = 30;
        }

        /// <summary>
        /// Thiết lập cấu hình cho Production
        /// </summary>
        private void ConfigureProduction()
        {
            ServerName = "production-server";
            DatabaseName = "VnsErp2025_Production";
            UseIntegratedSecurity = false;
            EnableSqlLogging = false;
            LogLevel = 1; // Error only
            EnablePerformanceMonitoring = true;
            SlowQueryThreshold = 5000;
            EnableCaching = true;
            CacheTimeout = 60;
            MaxPoolSize = 200;
        }

        #endregion

        #region Validation & Utilities

        /// <summary>
        /// Kiểm tra cấu hình có hợp lệ không.
        /// </summary>
        public bool IsValid()
        {
            try
            {
                if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName))
                    return false;

                if (!UseIntegratedSecurity && string.IsNullOrEmpty(UserId))
                    return false;

                if (ConnectionTimeout <= 0 || CommandTimeout <= 0)
                    return false;

                if (EnablePooling && (MinPoolSize < 0 || MaxPoolSize <= 0 || MinPoolSize > MaxPoolSize))
                    return false;

                if (EnableCaching && (CacheTimeout <= 0 || MaxCacheSize <= 0))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Reset về cấu hình mặc định
        /// </summary>
        public void ResetToDefaults()
        {
            ServerName = "localhost";
            DatabaseName = "VnsErp2025";
            UseIntegratedSecurity = true;
            UserId = null;
            Password = null;
            ConnectionTimeout = 15;
            CommandTimeout = 30;
            EnablePooling = true;
            MinPoolSize = 1;
            MaxPoolSize = 100;
            ConnectionLifetime = 0;
            EnlistInDistributedTransaction = true;
            Environment = "Development";
            EnableSqlLogging = true;
            LogLevel = 3;
            EnablePerformanceMonitoring = true;
            SlowQueryThreshold = 1000;
            EnableCaching = true;
            CacheTimeout = 30;
            MaxCacheSize = 100;
        }

        /// <summary>
        /// Clone cấu hình hiện tại
        /// </summary>
        public DatabaseConfig Clone()
        {
            return new DatabaseConfig
            {
                ServerName = this.ServerName,
                DatabaseName = this.DatabaseName,
                UseIntegratedSecurity = this.UseIntegratedSecurity,
                UserId = this.UserId,
                Password = this.Password,
                ConnectionTimeout = this.ConnectionTimeout,
                CommandTimeout = this.CommandTimeout,
                EnablePooling = this.EnablePooling,
                MinPoolSize = this.MinPoolSize,
                MaxPoolSize = this.MaxPoolSize,
                ConnectionLifetime = this.ConnectionLifetime,
                EnlistInDistributedTransaction = this.EnlistInDistributedTransaction,
                Environment = this.Environment,
                EnableSqlLogging = this.EnableSqlLogging,
                LogLevel = this.LogLevel,
                EnablePerformanceMonitoring = this.EnablePerformanceMonitoring,
                SlowQueryThreshold = this.SlowQueryThreshold,
                EnableCaching = this.EnableCaching,
                CacheTimeout = this.CacheTimeout,
                MaxCacheSize = this.MaxCacheSize
            };
        }

        /// <summary>
        /// Chuyển cấu hình thành dictionary (hỗ trợ hiển thị/logging).
        /// </summary>
        public Dictionary<string, object> ToDictionary()
        {
            return new Dictionary<string, object>
            {
                { "ServerName", ServerName },
                { "DatabaseName", DatabaseName },
                { "UseIntegratedSecurity", UseIntegratedSecurity },
                { "UserId", UserId ?? string.Empty },
                { "Password", Password ?? string.Empty },
                { "ConnectionTimeout", ConnectionTimeout },
                { "CommandTimeout", CommandTimeout },
                { "EnablePooling", EnablePooling },
                { "MinPoolSize", MinPoolSize },
                { "MaxPoolSize", MaxPoolSize },
                { "ConnectionLifetime", ConnectionLifetime },
                { "EnlistInDistributedTransaction", EnlistInDistributedTransaction },
                { "Environment", Environment },
                { "EnableSqlLogging", EnableSqlLogging },
                { "LogLevel", LogLevel },
                { "EnablePerformanceMonitoring", EnablePerformanceMonitoring },
                { "SlowQueryThreshold", SlowQueryThreshold },
                { "EnableCaching", EnableCaching },
                { "CacheTimeout", CacheTimeout },
                { "MaxCacheSize", MaxCacheSize }
            };
        }

        #endregion

        #region Obsolete Aliases (Backward Compatibility)

        [Obsolete("Use LoadFromConfig() instead")]
        public void TaiCauHinhTuConfig() => LoadFromConfig();

        [Obsolete("Use GetConfigValue(string,string) instead")]
        private string LayGiaTriConfig(string key, string defaultValue) => GetConfigValue(key, defaultValue);

        [Obsolete("Use GetConnectionString() instead")]
        public string LayConnectionString() => GetConnectionString();

        [Obsolete("Use SetEnvironment(string) instead")]
        public void ThietLapEnvironment(string environment) => SetEnvironment(environment);

        [Obsolete("Use IsValid() instead")]
        public bool KiemTraCauHinhHopLe() => IsValid();

        [Obsolete("Use ResetToDefaults() instead")]
        public void ResetVeCauHinhMacDinh() => ResetToDefaults();

        #endregion
    }
}
