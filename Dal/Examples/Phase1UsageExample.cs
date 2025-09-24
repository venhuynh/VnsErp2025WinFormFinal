using System;
using System.Threading.Tasks;
using Dal.DataAccess;
using Dal.Exceptions;
using Dal.Logging;
using Dal.Configuration;

namespace Dal.Examples
{
    /// <summary>
    /// Ví dụ sử dụng Phase 1 improvements: Configuration, Logging, Retry Policy
    /// </summary>
    public class Phase1UsageExample
    {
        #region thuocTinhDonGian

        private readonly ILogger _logger;

        #endregion

        #region phuongThuc

        public Phase1UsageExample()
        {
            // Sử dụng ConsoleLogger với debug enabled
            _logger = new ConsoleLogger(enableDebug: true);
        }

        /// <summary>
        /// Demo Configuration Management
        /// </summary>
        public void DemonstrateConfiguration()
        {
            Console.WriteLine("=== Configuration Management Demo ===");
            
            // Lấy settings từ ConfigurationManager
            var settings = ConfigurationManager.DatabaseSettings;
            
            Console.WriteLine($"Connection String: {settings.ConnectionString}");
            Console.WriteLine($"Command Timeout: {settings.CommandTimeout}s");
            Console.WriteLine($"Enable Retry: {settings.EnableRetryOnFailure}");
            Console.WriteLine($"Max Retry Count: {settings.MaxRetryCount}");
            Console.WriteLine($"Retry Delay: {settings.RetryDelayMs}ms");
            Console.WriteLine($"Enable Performance Monitoring: {settings.EnablePerformanceMonitoring}");
            Console.WriteLine();
        }

        /// <summary>
        /// Demo Logging
        /// </summary>
        public void DemonstrateLogging()
        {
            Console.WriteLine("=== Logging Demo ===");
            
            _logger.LogInfo("This is an info message");
            _logger.LogWarning("This is a warning message");
            _logger.LogDebug("This is a debug message");
            _logger.LogError("This is an error message");
            
            try
            {
                throw new InvalidOperationException("Sample exception for logging");
            }
            catch (Exception ex)
            {
                _logger.LogError("Caught an exception", ex);
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// Demo Enhanced DataAccess với Logging
        /// </summary>
        public void DemonstrateEnhancedDataAccess()
        {
            Console.WriteLine("=== Enhanced DataAccess Demo ===");
            
            try
            {
                // Tạo DataAccess với logger
                var userDataAccess = new ApplicationUserDataAccess(_logger);
                
                // Lấy danh sách users (sẽ có logging)
                var users = userDataAccess.LayTatCa();
                Console.WriteLine($"Found {users.Count} users");
                
                // Lấy user theo UserName (sẽ có logging)
                var user = userDataAccess.LayTheoUserName("admin");
                if (user != null)
                {
                    Console.WriteLine($"Found user: {user.UserName}, Active: {user.Active}");
                }
                else
                {
                    Console.WriteLine("User 'admin' not found");
                }
                
                // Kiểm tra UserName tồn tại (sẽ có logging)
                var exists = userDataAccess.UserNameTonTai("admin");
                Console.WriteLine($"UserName 'admin' exists: {exists}");
            }
            catch (DataAccessException ex)
            {
                _logger.LogError("DataAccess error: {0}", ex, ex.Message);
                if (ex.SqlErrorNumber.HasValue)
                {
                    _logger.LogError("SQL Error Number: {0}", ex.SqlErrorNumber);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error: {0}", ex, ex.Message);
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// Demo Retry Policy
        /// </summary>
        public async Task DemonstrateRetryPolicy()
        {
            Console.WriteLine("=== Retry Policy Demo ===");
            
            try
            {
                // Tạo DataAccess với logger
                var userDataAccess = new ApplicationUserDataAccess(_logger);
                
                // Async operation với retry policy
                var users = await userDataAccess.LayTatCaAsync();
                Console.WriteLine($"Async operation completed. Found {users.Count} users");
                
                // Async operation với retry policy
                var user = await userDataAccess.LayTheoUserNameAsync("admin");
                if (user != null)
                {
                    Console.WriteLine($"Async found user: {user.UserName}");
                }
                
                // Async operation với retry policy
                var exists = await userDataAccess.UserNameTonTaiAsync("admin");
                Console.WriteLine($"Async check - UserName 'admin' exists: {exists}");
            }
            catch (DataAccessException ex)
            {
                _logger.LogError("DataAccess error with retry: {0}", ex, ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError("Unexpected error with retry: {0}", ex, ex.Message);
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// Demo Configuration Override (useful for testing)
        /// </summary>
        public void DemonstrateConfigurationOverride()
        {
            Console.WriteLine("=== Configuration Override Demo ===");
            
            try
            {
                // Override settings cho testing
                var testSettings = new DatabaseSettings
                {
                    ConnectionString = "TestConnectionString",
                    CommandTimeout = 60,
                    EnableRetryOnFailure = false,
                    MaxRetryCount = 1,
                    RetryDelayMs = 500,
                    EnablePerformanceMonitoring = false
                };
                
                ConfigurationManager.OverrideSettings(testSettings);
                
                // Lấy settings đã override
                var currentSettings = ConfigurationManager.DatabaseSettings;
                Console.WriteLine($"Overridden Connection String: {currentSettings.ConnectionString}");
                Console.WriteLine($"Overridden Command Timeout: {currentSettings.CommandTimeout}s");
                Console.WriteLine($"Overridden Enable Retry: {currentSettings.EnableRetryOnFailure}");
                
                // Reload original settings
                ConfigurationManager.ReloadConfiguration();
                var originalSettings = ConfigurationManager.DatabaseSettings;
                Console.WriteLine($"Reloaded - Original Command Timeout: {originalSettings.CommandTimeout}s");
            }
            catch (Exception ex)
            {
                _logger.LogError("Configuration override error: {0}", ex, ex.Message);
            }
            
            Console.WriteLine();
        }

        /// <summary>
        /// Run tất cả demos
        /// </summary>
        public async Task RunAllDemos()
        {
            Console.WriteLine("🚀 Phase 1 Improvements Demo - Configuration, Logging, Retry Policy");
            Console.WriteLine(new string('=', 80));
            Console.WriteLine();
            
            try
            {
                DemonstrateConfiguration();
                DemonstrateLogging();
                DemonstrateEnhancedDataAccess();
                await DemonstrateRetryPolicy();
                DemonstrateConfigurationOverride();
                
                Console.WriteLine("✅ All demos completed successfully!");
            }
            catch (Exception ex)
            {
                _logger.LogError("Demo failed: {0}", ex, ex.Message);
            }
        }

        #endregion
    }
}
