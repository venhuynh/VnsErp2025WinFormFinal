using System;

namespace Dal.Logging
{
    /// <summary>
    /// Simple logger interface
    /// </summary>
    public interface ILogger
    {
        #region phuongThuc

        /// <summary>
        /// Log information message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="args">Format arguments</param>
        void LogInfo(string message, params object[] args);

        /// <summary>
        /// Log warning message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="args">Format arguments</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// Log error message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="ex">Exception (optional)</param>
        /// <param name="args">Format arguments</param>
        void LogError(string message, Exception ex = null, params object[] args);

        /// <summary>
        /// Log debug message
        /// </summary>
        /// <param name="message">Message to log</param>
        /// <param name="args">Format arguments</param>
        void LogDebug(string message, params object[] args);

        /// <summary>
        /// Log performance message
        /// </summary>
        /// <param name="operationName">Operation name</param>
        /// <param name="elapsedMs">Elapsed time in milliseconds</param>
        /// <param name="args">Additional arguments</param>
        void LogPerformance(string operationName, long elapsedMs, params object[] args);

        #endregion
    }
}
