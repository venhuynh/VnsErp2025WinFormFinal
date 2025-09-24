using System;
using System.Threading;
using System.Threading.Tasks;
using Dal.Logging;

namespace Dal.Helpers
{
    /// <summary>
    /// Helper thực thi tác vụ với cơ chế retry (thử lại) an toàn.
    /// Mục tiêu: tăng độ bền cho các thao tác dễ lỗi tạm thời (transient) như truy vấn DB, gọi mạng.
    /// </summary>
    public static class RetryHelper
    {
        #region Methods

        /// <summary>
        /// Thực thi một operation với retry (đồng bộ).
        /// </summary>
        /// <typeparam name="T">Kiểu kết quả trả về</typeparam>
        /// <param name="operation">Hàm cần thực thi</param>
        /// <param name="maxRetries">Số lần thử lại tối đa</param>
        /// <param name="delayMs">Độ trễ ban đầu giữa các lần thử (ms), có exponential backoff</param>
        /// <param name="shouldRetry">Hàm xác định có nên retry dựa trên exception</param>
        /// <param name="logger">Logger (tùy chọn) để theo dõi retry</param>
        /// <returns>Kết quả thực thi</returns>
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

                    // Quyết định có retry hay không
                    if (shouldRetry != null && !shouldRetry(ex))
                    {
                        logger.LogDebug("Should not retry based on exception type");
                        throw;
                    }

                    // Đã đạt tối đa số lần thử
                    if (retryCount == maxRetries)
                    {
                        logger.LogError("Operation failed after {0} attempts", ex, maxRetries + 1);
                        throw;
                    }

                    // Exponential backoff
                    var currentDelay = delayMs * (int)Math.Pow(2, retryCount);
                    logger.LogDebug("Waiting {0}ms before retry", currentDelay);
                    
                    Thread.Sleep(currentDelay);
                    retryCount++;
                }
            }

            throw lastException;
        }

        /// <summary>
        /// Thực thi một operation với retry (bất đồng bộ).
        /// </summary>
        /// <typeparam name="T">Kiểu kết quả trả về</typeparam>
        /// <param name="operation">Hàm async cần thực thi</param>
        /// <param name="maxRetries">Số lần thử lại tối đa</param>
        /// <param name="delayMs">Độ trễ ban đầu giữa các lần thử (ms), có exponential backoff</param>
        /// <param name="shouldRetry">Hàm xác định có nên retry dựa trên exception</param>
        /// <param name="logger">Logger (tùy chọn) để theo dõi retry</param>
        /// <returns>Task trả về kết quả</returns>
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

                    if (shouldRetry != null && !shouldRetry(ex))
                    {
                        logger.LogDebug("Should not retry based on exception type");
                        throw;
                    }

                    if (retryCount == maxRetries)
                    {
                        logger.LogError("Async operation failed after {0} attempts", ex, maxRetries + 1);
                        throw;
                    }

                    var currentDelay = delayMs * (int)Math.Pow(2, retryCount);
                    logger.LogDebug("Waiting {0}ms before retry", currentDelay);
                    
                    await Task.Delay(currentDelay);
                    retryCount++;
                }
            }

            throw lastException;
        }

        /// <summary>
        /// Điều kiện retry mặc định cho SQL Server: true nếu là lỗi tạm thời (transient) có thể retry.
        /// </summary>
        /// <param name="ex">Exception cần kiểm tra</param>
        /// <returns>true nếu nên retry</returns>
        public static bool ShouldRetrySqlException(Exception ex)
        {
            if (ex is System.Data.SqlClient.SqlException sqlEx)
            {
                // Các mã lỗi tạm thời phổ biến
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
