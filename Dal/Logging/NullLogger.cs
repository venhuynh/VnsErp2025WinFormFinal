namespace Dal.Logging
{
    /// <summary>
    /// Null logger implementation (no-op)
    /// </summary>
    public class NullLogger : ILogger
    {
        #region phuongThuc

        /// <summary>
        /// Log information message (no-op)
        /// </summary>
        public void LogInfo(string message, params object[] args)
        {
            // No operation
        }

        /// <summary>
        /// Log warning message (no-op)
        /// </summary>
        public void LogWarning(string message, params object[] args)
        {
            // No operation
        }

        /// <summary>
        /// Log error message (no-op)
        /// </summary>
        public void LogError(string message, System.Exception ex = null, params object[] args)
        {
            // No operation
        }

        /// <summary>
        /// Log debug message (no-op)
        /// </summary>
        public void LogDebug(string message, params object[] args)
        {
            // No operation
        }

        /// <summary>
        /// Log performance message (no-op)
        /// </summary>
        public void LogPerformance(string operationName, long elapsedMs, params object[] args)
        {
            // No operation
        }

        #endregion
    }
}
