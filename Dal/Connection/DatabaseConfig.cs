using System;
using System.Configuration;
using System.Collections.Generic;
using System.ComponentModel;

namespace Dal.Connection
{
    /// <summary>
    /// Class cấu hình database
    /// </summary>
    public class DatabaseConfig
    {
        #region thuocTinhDonGian

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

        #region phuongThuc

        /// <summary>
        /// Constructor mặc định
        /// </summary>
        public DatabaseConfig()
        {
            TaiCauHinhTuConfig();
        }

        /// <summary>
        /// Constructor với tham số
        /// </summary>
        /// <param name="serverName">Tên server</param>
        /// <param name="databaseName">Tên database</param>
        /// <param name="useIntegratedSecurity">Sử dụng Windows Auth</param>
        public DatabaseConfig(string serverName, string databaseName, bool useIntegratedSecurity = true)
        {
            ServerName = serverName;
            DatabaseName = databaseName;
            UseIntegratedSecurity = useIntegratedSecurity;
        }

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

        /// <summary>
        /// Tải cấu hình từ App.config
        /// </summary>
        public void TaiCauHinhTuConfig()
        {
            try
            {
                // Đọc từ appSettings
                ServerName = LayGiaTriConfig("DatabaseServer", ServerName);
                DatabaseName = LayGiaTriConfig("DatabaseName", DatabaseName);
                UseIntegratedSecurity = bool.Parse(LayGiaTriConfig("UseIntegratedSecurity", UseIntegratedSecurity.ToString()));
                UserId = LayGiaTriConfig("DatabaseUserId", UserId);
                Password = LayGiaTriConfig("DatabasePassword", Password);
                
                ConnectionTimeout = int.Parse(LayGiaTriConfig("ConnectionTimeout", ConnectionTimeout.ToString()));
                CommandTimeout = int.Parse(LayGiaTriConfig("CommandTimeout", CommandTimeout.ToString()));
                
                EnablePooling = bool.Parse(LayGiaTriConfig("EnablePooling", EnablePooling.ToString()));
                MinPoolSize = int.Parse(LayGiaTriConfig("MinPoolSize", MinPoolSize.ToString()));
                MaxPoolSize = int.Parse(LayGiaTriConfig("MaxPoolSize", MaxPoolSize.ToString()));
                ConnectionLifetime = int.Parse(LayGiaTriConfig("ConnectionLifetime", ConnectionLifetime.ToString()));
                EnlistInDistributedTransaction = bool.Parse(LayGiaTriConfig("EnlistInDistributedTransaction", EnlistInDistributedTransaction.ToString()));
                
                Environment = LayGiaTriConfig("Environment", Environment);
                
                EnableSqlLogging = bool.Parse(LayGiaTriConfig("EnableSqlLogging", EnableSqlLogging.ToString()));
                LogLevel = int.Parse(LayGiaTriConfig("LogLevel", LogLevel.ToString()));
                
                EnablePerformanceMonitoring = bool.Parse(LayGiaTriConfig("EnablePerformanceMonitoring", EnablePerformanceMonitoring.ToString()));
                SlowQueryThreshold = int.Parse(LayGiaTriConfig("SlowQueryThreshold", SlowQueryThreshold.ToString()));
                
                EnableCaching = bool.Parse(LayGiaTriConfig("EnableCaching", EnableCaching.ToString()));
                CacheTimeout = int.Parse(LayGiaTriConfig("CacheTimeout", CacheTimeout.ToString()));
                MaxCacheSize = int.Parse(LayGiaTriConfig("MaxCacheSize", MaxCacheSize.ToString()));
            }
            catch (Exception)
            {
                // Log error nhưng không throw exception
                // Có thể sử dụng logging framework ở đây
            }
        }

        /// <summary>
        /// Lấy giá trị từ config với fallback
        /// </summary>
        /// <param name="key">Key trong config</param>
        /// <param name="defaultValue">Giá trị mặc định</param>
        /// <returns>Giá trị từ config hoặc default</returns>
        private string LayGiaTriConfig(string key, string defaultValue)
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
        /// Lấy connection string từ cấu hình
        /// </summary>
        /// <returns>Connection string</returns>
        public string LayConnectionString()
        {
            return ConnectionStringHelper.TaoConnectionStringChiTiet(
                ServerName, DatabaseName, UseIntegratedSecurity, UserId, Password,
                ConnectionTimeout, CommandTimeout, EnablePooling, MinPoolSize, MaxPoolSize);
        }

        /// <summary>
        /// Thiết lập cấu hình cho environment
        /// </summary>
        /// <param name="environment">Environment</param>
        public void ThietLapEnvironment(string environment)
        {
            Environment = environment ?? "Development";
            
            switch (Environment.ToLower())
            {
                case "development":
                case "dev":
                    ThietLapCauHinhDevelopment();
                    break;
                    
                case "testing":
                case "test":
                    ThietLapCauHinhTesting();
                    break;
                    
                case "staging":
                    ThietLapCauHinhStaging();
                    break;
                    
                case "production":
                case "prod":
                    ThietLapCauHinhProduction();
                    break;
                    
                default:
                    ThietLapCauHinhDevelopment();
                    break;
            }
        }

        /// <summary>
        /// Thiết lập cấu hình cho Development
        /// </summary>
        private void ThietLapCauHinhDevelopment()
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
        private void ThietLapCauHinhTesting()
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
        private void ThietLapCauHinhStaging()
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
        private void ThietLapCauHinhProduction()
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

        /// <summary>
        /// Kiểm tra cấu hình có hợp lệ không
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public bool KiemTraCauHinhHopLe()
        {
            try
            {
                // Kiểm tra thông tin bắt buộc
                if (string.IsNullOrEmpty(ServerName) || string.IsNullOrEmpty(DatabaseName))
                    return false;

                // Kiểm tra authentication
                if (!UseIntegratedSecurity && string.IsNullOrEmpty(UserId))
                    return false;

                // Kiểm tra timeout values
                if (ConnectionTimeout <= 0 || CommandTimeout <= 0)
                    return false;

                // Kiểm tra pool settings
                if (EnablePooling && (MinPoolSize < 0 || MaxPoolSize <= 0 || MinPoolSize > MaxPoolSize))
                    return false;

                // Kiểm tra cache settings
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
        public void ResetVeCauHinhMacDinh()
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
        /// <returns>DatabaseConfig mới</returns>
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
        /// Chuyển đổi thành dictionary
        /// </summary>
        /// <returns>Dictionary chứa tất cả cấu hình</returns>
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
    }
}
