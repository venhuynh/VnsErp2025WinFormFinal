using System;

namespace Dal.Logging
{
    /// <summary>
    /// Trình ghi log ra Console đơn giản.
    /// Mục tiêu: ghi nhanh các mức INFO/WARN/ERROR/DEBUG/PERF với timestamp.
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        #region Fields & Properties

        private readonly bool _enableDebug;

        #endregion

        #region Methods

        /// <summary>
        /// Khởi tạo logger.
        /// </summary>
        /// <param name="enableDebug">Bật ghi DEBUG và StackTrace cho lỗi</param>
        public ConsoleLogger(bool enableDebug = false)
        {
            _enableDebug = enableDebug;
        }

        /// <summary>
        /// Ghi log mức thông tin (INFO).
        /// </summary>
        public void LogInfo(string message, params object[] args)
        {
            var formattedMessage = FormatMessage("INFO", message, args);
            Console.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Ghi log mức cảnh báo (WARN).
        /// </summary>
        public void LogWarning(string message, params object[] args)
        {
            var formattedMessage = FormatMessage("WARN", message, args);
            Console.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Ghi log mức lỗi (ERROR). In kèm Exception và StackTrace (nếu bật DEBUG).
        /// </summary>
        public void LogError(string message, Exception ex = null, params object[] args)
        {
            var formattedMessage = FormatMessage("ERROR", message, args);
            Console.WriteLine(formattedMessage);

            if (ex != null)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                if (_enableDebug)
                {
                    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
                }
            }
        }

        /// <summary>
        /// Ghi log mức DEBUG (chỉ khi bật).
        /// </summary>
        public void LogDebug(string message, params object[] args)
        {
            if (!_enableDebug) return;

            var formattedMessage = FormatMessage("DEBUG", message, args);
            Console.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Ghi log hiệu năng (PERF) kèm thời gian thực thi.
        /// </summary>
        public void LogPerformance(string operationName, long elapsedMs, params object[] args)
        {
            var message = $"Operation '{operationName}' completed in {elapsedMs}ms";
            if (args.Length > 0)
            {
                message += $" - {string.Join(", ", args)}";
            }

            var formattedMessage = FormatMessage("PERF", message);
            Console.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Chuẩn hóa định dạng log với timestamp và level.
        /// </summary>
        private string FormatMessage(string level, string message, params object[] args)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            var formattedMessage = args.Length > 0 ? string.Format(message, args) : message;
            return $"[{timestamp}] [{level}] {formattedMessage}";
        }

        #endregion
    }
}
