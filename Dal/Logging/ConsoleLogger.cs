using System;

namespace Dal.Logging
{
    /// <summary>
    /// Console logger implementation
    /// </summary>
    public class ConsoleLogger : ILogger
    {
        #region thuocTinhDonGian

        private readonly bool _enableDebug;

        #endregion

        #region phuongThuc

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsoleLogger(bool enableDebug = false)
        {
            _enableDebug = enableDebug;
        }

        /// <summary>
        /// Log information message
        /// </summary>
        public void LogInfo(string message, params object[] args)
        {
            var formattedMessage = FormatMessage("INFO", message, args);
            Console.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Log warning message
        /// </summary>
        public void LogWarning(string message, params object[] args)
        {
            var formattedMessage = FormatMessage("WARN", message, args);
            Console.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Log error message
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
        /// Log debug message
        /// </summary>
        public void LogDebug(string message, params object[] args)
        {
            if (!_enableDebug) return;

            var formattedMessage = FormatMessage("DEBUG", message, args);
            Console.WriteLine(formattedMessage);
        }

        /// <summary>
        /// Log performance message
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
        /// Format log message with timestamp and level
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
