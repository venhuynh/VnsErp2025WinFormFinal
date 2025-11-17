using System;
using Logger.Configuration;

namespace Logger.Examples
{
    /// <summary>
    /// Ví dụ sử dụng hệ thống Logger
    /// </summary>
    public class LoggerUsageExample
    {
        /// <summary>
        /// Ví dụ cơ bản sử dụng Logger
        /// </summary>
        public static void BasicExample()
        {
            // Tạo logger với category mặc định
            var logger = LoggerFactory.CreateLogger();

            logger.Info("Ứng dụng VNTA.NET 2025 đã khởi động");
            logger.Warning("Cảnh báo: Cấu hình chưa được thiết lập hoàn toàn");
            logger.Error("Lỗi kết nối database");
        }

        /// <summary>
        /// Ví dụ sử dụng Logger với categories cụ thể
        /// </summary>
        public static void CategoryExample()
        {
            // Tạo logger cho từng layer
            var uiLogger = LoggerFactory.CreateLogger(LogCategory.UI);
            var bllLogger = LoggerFactory.CreateLogger(LogCategory.BLL);
            var dalLogger = LoggerFactory.CreateLogger(LogCategory.DAL);

            uiLogger.Info("Form FrmMsSqlConfig được khởi tạo thành công");
            bllLogger.Info("Business logic xử lý cấu hình database");
            dalLogger.Info("Kết nối database thành công");
        }

        /// <summary>
        /// Ví dụ logging exceptions
        /// </summary>
        public static void ExceptionExample()
        {
            var logger = LoggerFactory.CreateLogger(LogCategory.DAL);

            try
            {
                // Simulate database operation
                throw new InvalidOperationException("Kết nối database thất bại");
            }
            catch (Exception ex)
            {
                logger.Error("Lỗi khi thực hiện thao tác database: {0}", ex, ex.Message);
                logger.Fatal("Lỗi nghiêm trọng không thể khôi phục", ex);
            }
        }

        /// <summary>
        /// Ví dụ logging với configuration tùy chỉnh
        /// </summary>
        public static void CustomConfigExample()
        {
            // Tạo cấu hình tùy chỉnh
            var config = new LogConfiguration();
            config.MinimumLevel = LogLevel.Debug;
            config.EnableConsole = true;
            config.EnableFile = true;
            config.LogDirectory = @"D:\CustomLogs";

            // Tạo logger với cấu hình tùy chỉnh
            var customLogger = LoggerFactory.CreateLogger(config, LogCategory.Security);

            customLogger.Debug("Debug message với cấu hình tùy chỉnh");
            customLogger.Info("Custom logger đang hoạt động");
        }

        /// <summary>
        /// Ví dụ logging performance timing
        /// </summary>
        public static void PerformanceExample()
        {
            var logger = LoggerFactory.CreateLogger(LogCategory.BLL);

            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            
            // Simulate some work
            System.Threading.Thread.Sleep(100);
            
            stopwatch.Stop();
            
            logger.Info("Thao tác hoàn thành trong {0}ms", stopwatch.ElapsedMilliseconds);
        }

        /// <summary>
        /// Chạy tất cả ví dụ
        /// </summary>
        public static void RunAllExamples()
        {
            Console.WriteLine("=== VNTA.NET 2025 Logger Examples ===");
            
            Console.WriteLine("\n1. Basic Example:");
            BasicExample();
            
            Console.WriteLine("\n2. Category Example:");
            CategoryExample();
            
            Console.WriteLine("\n3. Exception Example:");
            ExceptionExample();
            
            Console.WriteLine("\n4. Custom Config Example:");
            CustomConfigExample();
            
            Console.WriteLine("\n5. Performance Example:");
            PerformanceExample();
            
            Console.WriteLine("\n=== Examples completed ===");
        }
    }
}