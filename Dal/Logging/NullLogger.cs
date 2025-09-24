namespace Dal.Logging
{
    /// <summary>
    /// Trình ghi log rỗng (không thực hiện gì) dùng khi không muốn ghi log.
    /// Hữu ích cho test hoặc vô hiệu hóa logging theo cấu hình.
    /// </summary>
    public class NullLogger : ILogger
    {
        #region Methods

        /// <summary>
        /// Ghi log mức thông tin (no-op).
        /// </summary>
        public void LogInfo(string message, params object[] args)
        {
            // No operation
        }

        /// <summary>
        /// Ghi log mức cảnh báo (no-op).
        /// </summary>
        public void LogWarning(string message, params object[] args)
        {
            // No operation
        }

        /// <summary>
        /// Ghi log mức lỗi (no-op).
        /// </summary>
        public void LogError(string message, System.Exception ex = null, params object[] args)
        {
            // No operation
        }

        /// <summary>
        /// Ghi log mức debug (no-op).
        /// </summary>
        public void LogDebug(string message, params object[] args)
        {
            // No operation
        }

        /// <summary>
        /// Ghi log hiệu năng (no-op).
        /// </summary>
        public void LogPerformance(string operationName, long elapsedMs, params object[] args)
        {
            // No operation
        }

        #endregion
    }
}
