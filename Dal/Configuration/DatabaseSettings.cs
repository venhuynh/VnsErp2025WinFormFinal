using System;
using Dal.Connection;

namespace Dal.Configuration
{
    /// <summary>
    /// Cấu hình cho Database/DataContext dùng trong DAL.
    /// - Quản lý các tham số kết nối, timeout, retry, logging và performance.
    /// - Cung cấp hàm Validate để đảm bảo cấu hình hợp lệ trước khi sử dụng.
    /// </summary>
    public class DatabaseSettings
    {
        #region Fields & Properties

        /// <summary>
        /// Connection string dùng để khởi tạo DataContext/kết nối DB.
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// Thời gian timeout cho lệnh (giây).
        /// </summary>
        public int CommandTimeout { get; set; } = 30;

        /// <summary>
        /// Thời gian timeout kết nối (giây).
        /// </summary>
        public int ConnectionTimeout { get; set; } = 15;

        /// <summary>
        /// Bật cơ chế retry khi lỗi kết nối/tạm thời.
        /// </summary>
        public bool EnableRetryOnFailure { get; set; } = true;

        /// <summary>
        /// Số lần retry tối đa.
        /// </summary>
        public int MaxRetryCount { get; set; } = 3;

        /// <summary>
        /// Thời gian chờ giữa các lần retry (milliseconds).
        /// </summary>
        public int RetryDelayMs { get; set; } = 1000;

        /// <summary>
        /// Bật log chi tiết lỗi.
        /// </summary>
        public bool EnableDetailedErrors { get; set; } = false;

        /// <summary>
        /// Bật log dữ liệu nhạy cảm (chỉ nên bật khi debug).
        /// </summary>
        public bool EnableSensitiveDataLogging { get; set; } = false;

        /// <summary>
        /// Bật theo dõi hiệu năng truy vấn.
        /// </summary>
        public bool EnablePerformanceMonitoring { get; set; } = true;

        /// <summary>
        /// Ngưỡng thời gian (ms) để cảnh báo log chậm.
        /// </summary>
        public long PerformanceThresholdMs { get; set; } = 1000;

        #endregion

        #region Validation & Utilities

        /// <summary>
        /// Kiểm tra tính hợp lệ của cấu hình. Ném exception nếu có giá trị không hợp lệ.
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

        #endregion

        #region Factory Methods

        /// <summary>
        /// Tạo cấu hình mặc định dựa trên AppSettings và giá trị gợi ý.
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