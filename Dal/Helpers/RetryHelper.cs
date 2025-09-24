using System;
using System.Threading;
using System.Threading.Tasks;
using Dal.Logging;

namespace Dal.Helpers
{
    /// <summary>
    /// Retry helper for robust operations
    /// </summary>
    public static class RetryHelper
    {
        #region phuongThuc

        /// <summary>
        /// Execute operation with retry (synchronous)
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="operation">Operation to execute</param>
        /// <param name="maxRetries">Maximum retry count</param>
        /// <param name="delayMs">Initial delay in milliseconds</param>
        /// <param name="shouldRetry">Function to determine if should retry</param>
        /// <param name="logger">Logger for retry attempts</param>
        /// <returns>Operation result</returns>
        public static T ExecuteWithRetry<T>(
            Func<T> operation,
            int maxRetries = 3,
            int delayMs = 1000,
            Func<Exception, bool> shouldRetry = null,
            ILogger logger = null)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var retryCount = 0;
            Exception lastException = null;
            logger = logger ?? new NullLogger();

            while (retryCount <= maxRetries)
            {
                try
                {
                    logger.LogDebug("Executing operation, attempt {0}/{1}", retryCount + 1, maxRetries + 1);
                    var result = operation();
                    
                    if (retryCount > 0)
                    {
                        logger.LogInfo("Operation succeeded after {0} retries", retryCount);
                    }
                    
                    return result;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    logger.LogWarning("Operation failed on attempt {0}/{1}: {2}", retryCount + 1, maxRetries + 1, ex.Message);

                    // Check if should retry
                    if (shouldRetry != null && !shouldRetry(ex))
                    {
                        logger.LogDebug("Should not retry based on exception type");
                        throw;
                    }

                    // Check if max retries reached
                    if (retryCount == maxRetries)
                    {
                        logger.LogError("Operation failed after {0} attempts", ex, maxRetries + 1);
                        throw;
                    }

                    // Calculate delay with exponential backoff
                    var currentDelay = delayMs * (int)Math.Pow(2, retryCount);
                    logger.LogDebug("Waiting {0}ms before retry", currentDelay);
                    
                    Thread.Sleep(currentDelay);
                    retryCount++;
                }
            }

            throw lastException;
        }

        /// <summary>
        /// Execute operation with retry (asynchronous)
        /// </summary>
        /// <typeparam name="T">Return type</typeparam>
        /// <param name="operation">Operation to execute</param>
        /// <param name="maxRetries">Maximum retry count</param>
        /// <param name="delayMs">Initial delay in milliseconds</param>
        /// <param name="shouldRetry">Function to determine if should retry</param>
        /// <param name="logger">Logger for retry attempts</param>
        /// <returns>Operation result</returns>
        public static async Task<T> ExecuteWithRetryAsync<T>(
            Func<Task<T>> operation,
            int maxRetries = 3,
            int delayMs = 1000,
            Func<Exception, bool> shouldRetry = null,
            ILogger logger = null)
        {
            if (operation == null)
                throw new ArgumentNullException(nameof(operation));

            var retryCount = 0;
            Exception lastException = null;
            logger = logger ?? new NullLogger();

            while (retryCount <= maxRetries)
            {
                try
                {
                    logger.LogDebug("Executing async operation, attempt {0}/{1}", retryCount + 1, maxRetries + 1);
                    var result = await operation();
                    
                    if (retryCount > 0)
                    {
                        logger.LogInfo("Async operation succeeded after {0} retries", retryCount);
                    }
                    
                    return result;
                }
                catch (Exception ex)
                {
                    lastException = ex;
                    logger.LogWarning("Async operation failed on attempt {0}/{1}: {2}", retryCount + 1, maxRetries + 1, ex.Message);

                    // Check if should retry
                    if (shouldRetry != null && !shouldRetry(ex))
                    {
                        logger.LogDebug("Should not retry based on exception type");
                        throw;
                    }

                    // Check if max retries reached
                    if (retryCount == maxRetries)
                    {
                        logger.LogError("Async operation failed after {0} attempts", ex, maxRetries + 1);
                        throw;
                    }

                    // Calculate delay with exponential backoff
                    var currentDelay = delayMs * (int)Math.Pow(2, retryCount);
                    logger.LogDebug("Waiting {0}ms before retry", currentDelay);
                    
                    await Task.Delay(currentDelay);
                    retryCount++;
                }
            }

            throw lastException;
        }

        /// <summary>
        /// Default retry condition for SQL exceptions
        /// </summary>
        /// <param name="ex">Exception to check</param>
        /// <returns>True if should retry</returns>
        public static bool ShouldRetrySqlException(Exception ex)
        {
            if (ex is System.Data.SqlClient.SqlException sqlEx)
            {
                // Retry for transient errors
                return sqlEx.Number == 1205 || // Deadlock
                       sqlEx.Number == -2 ||   // Timeout
                       sqlEx.Number == 2 ||    // Connection timeout
                       sqlEx.Number == 53 ||   // Network path not found
                       sqlEx.Number == 121 ||  // Semaphore timeout
                       sqlEx.Number == 1204 || // Lock request timeout
                       sqlEx.Number == 1222;   // Lock request timeout
            }
            return false;
        }

        #endregion
    }
}
