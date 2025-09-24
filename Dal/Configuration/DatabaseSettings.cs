using System;
using Dal.Connection;

namespace Dal.Configuration
{
    /// <summary>
    /// Database settings configuration
    /// </summary>
    public class DatabaseSettings
    {
        #region thuocTinhDonGian

        /// <summary>
        /// Connection string
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Command timeout in seconds
        /// </summary>
        public int CommandTimeout { get; set; } = 30;

        /// <summary>
        /// Connection timeout in seconds
        /// </summary>
        public int ConnectionTimeout { get; set; } = 15;

        /// <summary>
        /// Enable retry on failure
        /// </summary>
        public bool EnableRetryOnFailure { get; set; } = true;

        /// <summary>
        /// Maximum retry count
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// Retry delay in milliseconds
        /// </summary>
        public int RetryDelayMs { get; set; } = 1000;

        /// <summary>
        /// Enable detailed error logging
        /// </summary>
        public bool EnableDetailedErrors { get; set; } = false;

        /// <summary>
        /// Enable sensitive data logging (for debugging only)
        /// </summary>
        public bool EnableSensitiveDataLogging { get; set; } = false;

        /// <summary>
        /// Enable performance monitoring
        /// </summary>
        public bool EnablePerformanceMonitoring { get; set; } = true;

        /// <summary>
        /// Performance threshold in milliseconds (log if operation takes longer)
        /// </summary>
        public long PerformanceThresholdMs { get; set; } = 1000;

        #endregion

        #region phuongThuc

        /// <summary>
        /// Validate settings
        /// </summary>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ConnectionString))
                throw new InvalidOperationException("ConnectionString is required");

            if (CommandTimeout <= 0)
                throw new InvalidOperationException("CommandTimeout must be greater than 0");

            if (ConnectionTimeout <= 0)
                throw new InvalidOperationException("ConnectionTimeout must be greater than 0");

            if (MaxRetryCount < 0)
                throw new InvalidOperationException("MaxRetryCount cannot be negative");

            if (RetryDelayMs <= 0)
                throw new InvalidOperationException("RetryDelayMs must be greater than 0");

            if (PerformanceThresholdMs <= 0)
                throw new InvalidOperationException("PerformanceThresholdMs must be greater than 0");
        }

        /// <summary>
        /// Create default settings
        /// </summary>
        public static DatabaseSettings CreateDefault()
        {
            return new DatabaseSettings
            {
                ConnectionString = ConnectionStringHelper.LayConnectionStringMacDinh(),
                CommandTimeout = 30,
                ConnectionTimeout = 15,
                EnableRetryOnFailure = true,
                MaxRetryCount = 3,
                RetryDelayMs = 1000,
                EnableDetailedErrors = false,
                EnableSensitiveDataLogging = false,
                EnablePerformanceMonitoring = true,
                PerformanceThresholdMs = 1000
            };
        }

        #endregion
    }
}
