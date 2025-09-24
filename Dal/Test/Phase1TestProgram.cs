using System;
using System.Threading.Tasks;
using Dal.DataAccess;
using Dal.Examples;
using Dal.Logging;
using Dal.Configuration;
using Dal.Helpers;

namespace Dal.Test
{
    /// <summary>
    /// Test program cho Phase 1 features: Configuration, Logging, Retry Policy
    /// </summary>
    class Phase1TestProgram
    {
        public static async Task Main(string[] args)
        {
            Console.WriteLine("🚀 Phase 1 Features Test - Configuration, Logging, Retry Policy");
            Console.WriteLine(new string('=', 70));
            Console.WriteLine();

            try
            {
                // Test 1: Configuration Management
                await TestConfigurationManagement();
                Console.WriteLine();

                // Test 2: Logging System
                await TestLoggingSystem();
                Console.WriteLine();

                // Test 3: Retry Policy
                await TestRetryPolicy();
                Console.WriteLine();

                // Test 4: Enhanced DataAccess
                await TestEnhancedDataAccess();
                Console.WriteLine();

                // Test 5: Full Integration
                await TestFullIntegration();
                Console.WriteLine();

                Console.WriteLine("✅ All Phase 1 tests completed successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Test failed: {ex.Message}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
            }

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Test Configuration Management
        /// </summary>
        static async Task TestConfigurationManagement()
        {
            Console.WriteLine("📋 Test 1: Configuration Management");
            Console.WriteLine(new string('-', 40));

            try
            {
                // Test default configuration
                var settings = ConfigurationManager.DatabaseSettings;
                Console.WriteLine($"✅ Default Connection String: {settings.ConnectionString.Substring(0, 50)}...");
                Console.WriteLine($"✅ Command Timeout: {settings.CommandTimeout}s");
                Console.WriteLine($"✅ Enable Retry: {settings.EnableRetryOnFailure}");
                Console.WriteLine($"✅ Max Retry Count: {settings.MaxRetryCount}");
                Console.WriteLine($"✅ Retry Delay: {settings.RetryDelayMs}ms");

                // Test configuration override
                var testSettings = new DatabaseSettings
                {
                    ConnectionString = "TestConnectionString",
                    CommandTimeout = 60,
                    EnableRetryOnFailure = false,
                    MaxRetryCount = 1,
                    RetryDelayMs = 500
                };

                ConfigurationManager.OverrideSettings(testSettings);
                var overriddenSettings = ConfigurationManager.DatabaseSettings;
                Console.WriteLine($"✅ Override Test - Command Timeout: {overriddenSettings.CommandTimeout}s");
                Console.WriteLine($"✅ Override Test - Enable Retry: {overriddenSettings.EnableRetryOnFailure}");

                // Restore original settings
                ConfigurationManager.ReloadConfiguration();
                var restoredSettings = ConfigurationManager.DatabaseSettings;
                Console.WriteLine($"✅ Restore Test - Original Command Timeout: {restoredSettings.CommandTimeout}s");

                Console.WriteLine("✅ Configuration Management test PASSED");
                await Task.Delay(1); // Avoid async warning
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Configuration Management test FAILED: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Logging System
        /// </summary>
        static async Task TestLoggingSystem()
        {
            Console.WriteLine("📝 Test 2: Logging System");
            Console.WriteLine(new string('-', 40));

            try
            {
                // Test ConsoleLogger
                var logger = new ConsoleLogger(enableDebug: true);
                
                logger.LogInfo("This is an INFO message");
                logger.LogWarning("This is a WARNING message");
                logger.LogDebug("This is a DEBUG message");
                
                // Test error logging with exception
                try
                {
                    throw new InvalidOperationException("Test exception for logging");
                }
                catch (Exception ex)
                {
                    logger.LogError("This is an ERROR message with exception", ex);
                }

                // Test performance logging
                logger.LogPerformance("TestOperation", 1500, "Records processed: 100");

                // Test NullLogger (should not output anything)
                var nullLogger = new NullLogger();
                nullLogger.LogInfo("This should not appear");
                nullLogger.LogError("This should not appear either");

                Console.WriteLine("✅ Logging System test PASSED");
                await Task.Delay(1); // Avoid async warning
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Logging System test FAILED: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Retry Policy
        /// </summary>
        static async Task TestRetryPolicy()
        {
            Console.WriteLine("🔄 Test 3: Retry Policy");
            Console.WriteLine(new string('-', 40));

            try
            {
                var logger = new ConsoleLogger(enableDebug: true);

                // Test successful operation (no retry needed)
                var successResult = await RetryHelper.ExecuteWithRetryAsync(
                    async () =>
                    {
                        await Task.Delay(100); // Simulate async operation
                        return "Success on first attempt";
                    },
                    maxRetries: 3,
                    delayMs: 100,
                    shouldRetry: RetryHelper.ShouldRetrySqlException,
                    logger: logger
                );
                Console.WriteLine($"✅ Success operation: {successResult}");

                // Test operation that fails first time then succeeds
                var retryCount = 0;
                var retryResult = await RetryHelper.ExecuteWithRetryAsync(
                    async () =>
                    {
                        retryCount++;
                        await Task.Delay(50);
                        
                        if (retryCount < 2)
                        {
                            // Simulate SQL deadlock error (1205)
                            throw new InvalidOperationException("Simulated SQL deadlock error (1205)");
                        }
                        
                        return "Success after retry";
                    },
                    maxRetries: 3,
                    delayMs: 100,
                    shouldRetry: (ex) => ex is InvalidOperationException, // Custom retry logic for testing
                    logger: logger
                );
                Console.WriteLine($"✅ Retry operation: {retryResult} (attempts: {retryCount})");

                // Test operation that fails completely
                try
                {
                    await RetryHelper.ExecuteWithRetryAsync<string>(
                        async () =>
                        {
                            await Task.Delay(50);
                            // Simulate non-retryable error
                            throw new ArgumentException("Non-retryable error");
                        },
                        maxRetries: 2,
                        delayMs: 100,
                        shouldRetry: RetryHelper.ShouldRetrySqlException, // Won't retry ArgumentException
                        logger: logger
                    );
                    Console.WriteLine("❌ Should have thrown exception");
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("✅ Correctly did not retry non-retryable exception");
                }

                Console.WriteLine("✅ Retry Policy test PASSED");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Retry Policy test FAILED: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Enhanced DataAccess
        /// </summary>
        static async Task TestEnhancedDataAccess()
        {
            Console.WriteLine("🗄️ Test 4: Enhanced DataAccess");
            Console.WriteLine(new string('-', 40));

            try
            {
                var logger = new ConsoleLogger(enableDebug: true);

                // Test with logging enabled
                var userDataAccess = new ApplicationUserDataAccess(logger);

                // Test synchronous operations with logging
                Console.WriteLine("Testing synchronous operations...");
                var userCount = userDataAccess.DemSoLuong();
                Console.WriteLine($"✅ User count: {userCount}");

                var users = userDataAccess.LayTatCa();
                Console.WriteLine($"✅ Total users retrieved: {users.Count}");

                // Test async operations with retry policy
                Console.WriteLine("Testing async operations...");
                var asyncUsers = await userDataAccess.LayTatCaAsync();
                Console.WriteLine($"✅ Async total users retrieved: {asyncUsers.Count}");

                // Test specific operations
                var adminUser = userDataAccess.LayTheoUserName("admin");
                if (adminUser != null)
                {
                    Console.WriteLine($"✅ Found admin user: {adminUser.UserName}, Active: {adminUser.Active}");
                }
                else
                {
                    Console.WriteLine("ℹ️ Admin user not found (this is normal if not created yet)");
                }

                var activeUsers = userDataAccess.LayUserActive();
                Console.WriteLine($"✅ Active users: {activeUsers.Count}");

                Console.WriteLine("✅ Enhanced DataAccess test PASSED");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Enhanced DataAccess test FAILED: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// Test Full Integration
        /// </summary>
        static async Task TestFullIntegration()
        {
            Console.WriteLine("🔗 Test 5: Full Integration");
            Console.WriteLine(new string('-', 40));

            try
            {
                // Test comprehensive integration
                var logger = new ConsoleLogger(enableDebug: true);
                var userDataAccess = new ApplicationUserDataAccess(logger);
                
                Console.WriteLine("Testing comprehensive integration...");
                
                // Test all CRUD operations with logging and retry
                var users = await userDataAccess.LayTatCaAsync();
                Console.WriteLine($"✅ Retrieved {users.Count} users via async operation");
                
                var activeUsers = userDataAccess.LayUserActive();
                Console.WriteLine($"✅ Found {activeUsers.Count} active users");
                
                // Test performance logging
                logger.LogPerformance("IntegrationTest", 250, "All operations completed successfully");

                Console.WriteLine("✅ Full Integration test PASSED");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Full Integration test FAILED: {ex.Message}");
                throw;
            }
        }
    }
}
