using System;

namespace Dal.Logging
{
    /// <summary>
    /// Giao diện logger đơn giản cho ứng dụng.
    /// Định nghĩa các mức log cơ bản: Info/Warning/Error/Debug và Performance.
    /// </summary>
    public interface ILogger
    {
        #region Methods

        /// <summary>
        /// Ghi log mức thông tin.
        /// </summary>
        /// <param name="message">Thông điệp</param>
        /// <param name="args">Tham số format</param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// Ghi log mức cảnh báo.
        /// </summary>
        /// <param name="message">Thông điệp</param>
        /// <param name="args">Tham số format</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// Ghi log mức lỗi.
        /// </summary>
        /// <param name="message">Thông điệp</param>
        /// <param name="ex">Exception (tùy chọn)</param>
        /// <param name="args">Tham số format</param>
        void LogError(string message, Exception ex = null, params object[] args);

        /// <summary>
        /// Ghi log mức debug.
        /// </summary>
        /// <param name="message">Thông điệp</param>
        /// <param name="args">Tham số format</param>
        void LogDebug(string message, params object[] args);

        /// <summary>
        /// Ghi log hiệu năng (tên tác vụ và thời gian thực thi).
        /// </summary>
        /// <param name="operationName">Tên tác vụ</param>
        /// <param name="elapsedMs">Thời gian thực thi (ms)</param>
        /// <param name="args">Tham số bổ sung</param>
        void LogPerformance(string operationName, long elapsedMs, params object[] args);

        #endregion
    }
}
