using System;
using System.Data;
using System.Data.SqlClient;
using Dal.Exceptions;
using Dal.Helpers;

namespace Dal.Connection
{
    /// <summary>
    /// Ví dụ sử dụng ConnectionManager và các class liên quan
    /// </summary>
    public class ConnectionManagerExample
    {
        #region phuongThuc

        /// <summary>
        /// Ví dụ sử dụng ConnectionManager cơ bản
        /// </summary>
        public static void ViDuSuDungCoBan()
        {
            // Sử dụng connection string mặc định
            using (var connectionManager = new ConnectionManager())
            {
                try
                {
                    // Mở kết nối
                    if (connectionManager.MoKetNoi())
                    {
                        Console.WriteLine("Kết nối thành công!");

                        // Test kết nối
                        if (connectionManager.KiemTraHoatDong())
                        {
                            Console.WriteLine("Kết nối hoạt động bình thường");
                        }

                        // Thực hiện query đơn giản
                        using (var command = connectionManager.TaoCommand("SELECT GETDATE() AS CurrentTime"))
                        {
                            var result = command.ExecuteScalar();
                            Console.WriteLine($"Thời gian hiện tại: {result}");
                        }
                    }
                }
                catch (ConnectionException ex)
                {
                    Console.WriteLine($"Lỗi kết nối: {ex.Message}");
                    Console.WriteLine($"Loại lỗi: {ex.LoaiLoi}");
                    
                    if (ex.CoTheRetry())
                    {
                        Console.WriteLine("Có thể thử lại kết nối");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi khác: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Ví dụ sử dụng với connection string tùy chỉnh
        /// </summary>
        public static void ViDuSuDungVoiConnectionStringTuyChinh()
        {
            // Tạo connection string tùy chỉnh
            var connectionString = ConnectionStringHelper.TaoConnectionString(
                server: "localhost",
                database: "VnsErp2025_Test",
                integratedSecurity: true
            );

            using (var connectionManager = new ConnectionManager(connectionString))
            {
                // Đăng ký event handlers
                connectionManager.KetNoiMo += (sender, e) => 
                    Console.WriteLine($"Kết nối đã mở lúc: {e.ThoiGian}");
                
                connectionManager.KetNoiDong += (sender, e) => 
                    Console.WriteLine($"Kết nối đã đóng lúc: {e.ThoiGian}");
                
                connectionManager.LoiKetNoi += (sender, e) => 
                    Console.WriteLine($"Lỗi kết nối: {e.MoTaLoi}, Mức độ: {e.MucDoNghiemTrong}");

                try
                {
                    // Mở kết nối và thực hiện operations
                    connectionManager.MoKetNoi();
                    
                    // Sử dụng stored procedure
                    using (var command = connectionManager.TaoStoredProcedureCommand("sp_GetSystemInfo"))
                    {
                        command.Parameters.Add(new SqlParameter("@InfoType", "Version"));
                        command.Parameters.Add(SqlHelper.TaoOutputParameter("@Version", SqlDbType.VarChar, 100));
                        
                        command.ExecuteNonQuery();
                        var version = command.Parameters["@Version"].Value.ToString();
                        Console.WriteLine($"Phiên bản hệ thống: {version}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Ví dụ sử dụng DatabaseConfig
        /// </summary>
        public static void ViDuSuDungDatabaseConfig()
        {
            // Lấy instance singleton
            var config = DatabaseConfig.Instance;
            
            // Thiết lập environment
            config.ThietLapEnvironment("Development");
            
            Console.WriteLine($"Server: {config.ServerName}");
            Console.WriteLine($"Database: {config.DatabaseName}");
            Console.WriteLine($"Environment: {config.Environment}");
            Console.WriteLine($"Enable Logging: {config.EnableSqlLogging}");
            Console.WriteLine($"Log Level: {config.LogLevel}");
            
            // Kiểm tra cấu hình
            if (config.KiemTraCauHinhHopLe())
            {
                Console.WriteLine("Cấu hình hợp lệ");
                
                // Lấy connection string từ config
                var connectionString = config.LayConnectionString();
                Console.WriteLine($"Connection String: {ConnectionStringHelper.LayConnectionStringAnToan(connectionString)}");
            }
            else
            {
                Console.WriteLine("Cấu hình không hợp lệ");
            }
        }

        /// <summary>
        /// Ví dụ sử dụng ConnectionStringHelper
        /// </summary>
        public static void ViDuSuDungConnectionStringHelper()
        {
            // Tạo connection string chi tiết
            var connectionString = ConnectionStringHelper.TaoConnectionStringChiTiet(
                server: "localhost",
                database: "VnsErp2025",
                integratedSecurity: true,
                timeout: 30,
                commandTimeout: 60,
                pooling: true,
                minPoolSize: 2,
                maxPoolSize: 50
            );
            
            Console.WriteLine($"Connection String: {ConnectionStringHelper.LayConnectionStringAnToan(connectionString)}");
            
            // Parse connection string
            var connectionInfo = ConnectionStringHelper.PhanTichConnectionString(connectionString);
            Console.WriteLine($"Server: {connectionInfo.Server}");
            Console.WriteLine($"Database: {connectionInfo.Database}");
            Console.WriteLine($"Integrated Security: {connectionInfo.IntegratedSecurity}");
            Console.WriteLine($"Connection Timeout: {connectionInfo.ConnectionTimeout}");
            Console.WriteLine($"Command Timeout: {connectionInfo.CommandTimeout}");
            Console.WriteLine($"Pooling: {connectionInfo.Pooling}");
            Console.WriteLine($"Min Pool Size: {connectionInfo.MinPoolSize}");
            Console.WriteLine($"Max Pool Size: {connectionInfo.MaxPoolSize}");
            
            // Kiểm tra connection string hợp lệ
            if (ConnectionStringHelper.KiemTraConnectionString(connectionString))
            {
                Console.WriteLine("Connection string hợp lệ");
            }
            else
            {
                Console.WriteLine("Connection string không hợp lệ");
            }
            
            // Tạo connection string theo environment
            var devConnectionString = ConnectionStringHelper.TaoConnectionStringTheoEnvironment("Development");
            var prodConnectionString = ConnectionStringHelper.TaoConnectionStringTheoEnvironment("Production");
            
            Console.WriteLine($"Dev Connection String: {ConnectionStringHelper.LayConnectionStringAnToan(devConnectionString)}");
            Console.WriteLine($"Prod Connection String: {ConnectionStringHelper.LayConnectionStringAnToan(prodConnectionString)}");
        }

        /// <summary>
        /// Ví dụ xử lý lỗi và retry
        /// </summary>
        public static void ViDuXuLyLoiVaRetry()
        {
            var maxRetries = 3;
            var retryDelay = 1000; // 1 giây
            
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    using (var connectionManager = new ConnectionManager())
                    {
                        connectionManager.MoKetNoi();
                        
                        // Thực hiện operation
                        using (var command = connectionManager.TaoCommand("SELECT COUNT(*) FROM INFORMATION_SCHEMA.TABLES"))
                        {
                            var tableCount = command.ExecuteScalar();
                            Console.WriteLine($"Số lượng bảng: {tableCount}");
                        }
                        
                        // Thành công, thoát khỏi retry loop
                        break;
                    }
                }
                catch (ConnectionException ex)
                {
                    Console.WriteLine($"Lần thử {attempt}: {ex.Message}");
                    
                    if (!ex.CoTheRetry() || attempt == maxRetries)
                    {
                        Console.WriteLine("Không thể retry hoặc đã hết số lần thử");
                        throw;
                    }
                    
                    Console.WriteLine($"Chờ {retryDelay}ms trước khi thử lại...");
                    System.Threading.Thread.Sleep(retryDelay);
                    retryDelay *= 2; // Exponential backoff
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Lỗi không thể retry: {ex.Message}");
                    throw;
                }
            }
        }

        /// <summary>
        /// Ví dụ sử dụng với transaction
        /// </summary>
        public static void ViDuSuDungVoiTransaction()
        {
            using (var connectionManager = new ConnectionManager())
            {
                var connection = connectionManager.LayKetNoi();
                
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Thực hiện các operations trong transaction
                        using (var command1 = new SqlCommand("INSERT INTO TestTable (Name) VALUES ('Test1')", connection, transaction))
                        {
                            command1.ExecuteNonQuery();
                        }
                        
                        using (var command2 = new SqlCommand("INSERT INTO TestTable (Name) VALUES ('Test2')", connection, transaction))
                        {
                            command2.ExecuteNonQuery();
                        }
                        
                        // Commit transaction
                        transaction.Commit();
                        Console.WriteLine("Transaction committed successfully");
                    }
                    catch (Exception ex)
                    {
                        // Rollback transaction
                        transaction.Rollback();
                        Console.WriteLine($"Transaction rolled back: {ex.Message}");
                        throw;
                    }
                }
            }
        }

        #endregion
    }
}
