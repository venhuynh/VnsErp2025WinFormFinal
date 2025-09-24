using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security;
using System.Text;

namespace Dal.Connection
{
    /// <summary>
    /// Helper class để quản lý connection string
    /// </summary>
    public static class ConnectionStringHelper
    {
        #region thuocTinhDonGian

        private const string DEFAULT_CONNECTION_NAME = "VnsErp2025ConnectionString";
        private const string DEFAULT_SERVER = "localhost";
        private const string DEFAULT_DATABASE = "VnsErp2025Final";
        private const int DEFAULT_TIMEOUT = 30;
        private const int DEFAULT_CONNECTION_TIMEOUT = 15;

        #endregion

        #region phuongThuc

        /// <summary>
        /// Lấy connection string mặc định từ config
        /// </summary>
        /// <returns>Connection string</returns>
        public static string LayConnectionStringMacDinh()
        {
            try
            {
                var connectionString = ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTION_NAME]?.ConnectionString;
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    // Tạo connection string mặc định nếu không tìm thấy trong config
                    connectionString = TaoConnectionStringMacDinh();
                }

                return connectionString;
            }
            catch (Exception)
            {
                // Fallback về connection string mặc định
                return TaoConnectionStringMacDinh();
            }
        }

        /// <summary>
        /// Lấy connection string theo tên
        /// </summary>
        /// <param name="connectionName">Tên connection string trong config</param>
        /// <returns>Connection string</returns>
        public static string LayConnectionString(string connectionName)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionName))
                    throw new ArgumentException("Tên connection string không được null hoặc rỗng", nameof(connectionName));

                var connectionString = ConfigurationManager.ConnectionStrings[connectionName]?.ConnectionString;
                
                if (string.IsNullOrEmpty(connectionString))
                    throw new ConfigurationErrorsException($"Không tìm thấy connection string với tên: {connectionName}");

                return connectionString;
            }
            catch (Exception ex)
            {
                throw new ConfigurationErrorsException($"Lỗi lấy connection string: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Tạo connection string mặc định
        /// </summary>
        /// <returns>Connection string mặc định</returns>
        public static string TaoConnectionStringMacDinh()
        {
            return TaoConnectionString(DEFAULT_SERVER, DEFAULT_DATABASE, true, null, null);
        }

        /// <summary>
        /// Tạo connection string với thông tin cơ bản
        /// </summary>
        /// <param name="server">Tên server</param>
        /// <param name="database">Tên database</param>
        /// <param name="integratedSecurity">Sử dụng Windows Authentication</param>
        /// <param name="userId">User ID (nếu không dùng Windows Auth)</param>
        /// <param name="password">Password (nếu không dùng Windows Auth)</param>
        /// <returns>Connection string</returns>
        public static string TaoConnectionString(string server, string database, bool integratedSecurity = true, 
            string userId = null, string password = null)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server ?? DEFAULT_SERVER,
                InitialCatalog = database ?? DEFAULT_DATABASE,
                IntegratedSecurity = integratedSecurity,
                ConnectTimeout = DEFAULT_CONNECTION_TIMEOUT,
                Pooling = true,
                MinPoolSize = 1,
                MaxPoolSize = 100,
                Enlist = true
            };

            if (!integratedSecurity && !string.IsNullOrEmpty(userId))
            {
                builder.UserID = userId;
                builder.Password = password ?? string.Empty;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Tạo connection string với SqlConnectionStringBuilder
        /// </summary>
        /// <param name="server">Tên server</param>
        /// <param name="database">Tên database</param>
        /// <param name="integratedSecurity">Sử dụng Windows Authentication</param>
        /// <param name="userId">User ID</param>
        /// <param name="password">Password</param>
        /// <param name="timeout">Connection timeout</param>
        /// <param name="commandTimeout">Command timeout</param>
        /// <param name="pooling">Enable connection pooling</param>
        /// <param name="minPoolSize">Minimum pool size</param>
        /// <param name="maxPoolSize">Maximum pool size</param>
        /// <returns>Connection string</returns>
        public static string TaoConnectionStringChiTiet(string server, string database, bool integratedSecurity = true,
            string userId = null, string password = null, int timeout = DEFAULT_CONNECTION_TIMEOUT,
            int commandTimeout = DEFAULT_TIMEOUT, bool pooling = true, int minPoolSize = 1, int maxPoolSize = 100)
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                IntegratedSecurity = integratedSecurity,
                ConnectTimeout = timeout,
                Pooling = pooling,
                MinPoolSize = minPoolSize,
                MaxPoolSize = maxPoolSize,
                Enlist = true
            };

            if (!integratedSecurity && !string.IsNullOrEmpty(userId))
            {
                builder.UserID = userId;
                builder.Password = password ?? string.Empty;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Parse connection string thành các thành phần
        /// </summary>
        /// <param name="connectionString">Connection string cần parse</param>
        /// <returns>ConnectionStringInfo object</returns>
        public static ConnectionStringInfo PhanTichConnectionString(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentException("Connection string không được null hoặc rỗng", nameof(connectionString));

                var builder = new SqlConnectionStringBuilder(connectionString);
                
                return new ConnectionStringInfo
                {
                    Server = builder.DataSource,
                    Database = builder.InitialCatalog,
                    IntegratedSecurity = builder.IntegratedSecurity,
                    UserId = builder.UserID,
                    Password = builder.Password,
                    ConnectionTimeout = builder.ConnectTimeout,
                    CommandTimeout = DEFAULT_TIMEOUT, // Default value since SqlConnectionStringBuilder doesn't have this property
                    Pooling = builder.Pooling,
                    MinPoolSize = builder.MinPoolSize,
                    MaxPoolSize = builder.MaxPoolSize
                };
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Không thể parse connection string: {ex.Message}", nameof(connectionString), ex);
            }
        }

        /// <summary>
        /// Kiểm tra connection string có hợp lệ không
        /// </summary>
        /// <param name="connectionString">Connection string cần kiểm tra</param>
        /// <returns>True nếu hợp lệ</returns>
        public static bool KiemTraConnectionString(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                    return false;

                var builder = new SqlConnectionStringBuilder(connectionString);
                
                // Kiểm tra các thông tin bắt buộc
                if (string.IsNullOrEmpty(builder.DataSource) || string.IsNullOrEmpty(builder.InitialCatalog))
                    return false;

                // Kiểm tra authentication
                if (!builder.IntegratedSecurity && string.IsNullOrEmpty(builder.UserID))
                    return false;

                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Mã hóa connection string
        /// </summary>
        /// <param name="connectionString">Connection string cần mã hóa</param>
        /// <returns>Connection string đã mã hóa</returns>
        public static string MaHoaConnectionString(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                    return string.Empty;

                var bytes = Encoding.UTF8.GetBytes(connectionString);
                return Convert.ToBase64String(bytes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thể mã hóa connection string: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Giải mã connection string
        /// </summary>
        /// <param name="encryptedConnectionString">Connection string đã mã hóa</param>
        /// <returns>Connection string đã giải mã</returns>
        public static string GiaiMaConnectionString(string encryptedConnectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(encryptedConnectionString))
                    return string.Empty;

                var bytes = Convert.FromBase64String(encryptedConnectionString);
                return Encoding.UTF8.GetString(bytes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Không thể giải mã connection string: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy connection string an toàn (ẩn password)
        /// </summary>
        /// <param name="connectionString">Connection string gốc</param>
        /// <returns>Connection string đã ẩn password</returns>
        public static string LayConnectionStringAnToan(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                    return string.Empty;

                var builder = new SqlConnectionStringBuilder(connectionString);
                
                if (!string.IsNullOrEmpty(builder.Password))
                {
                    builder.Password = "***";
                }

                return builder.ToString();
            }
            catch
            {
                // Nếu không thể parse, trả về connection string gốc
                return connectionString;
            }
        }

        /// <summary>
        /// Tạo connection string cho environment khác nhau
        /// </summary>
        /// <param name="environment">Environment (Development, Testing, Production)</param>
        /// <returns>Connection string phù hợp với environment</returns>
        public static string TaoConnectionStringTheoEnvironment(string environment)
        {
            switch (environment?.ToLower())
            {
                case "development":
                case "dev":
                    return TaoConnectionString("localhost", "VnsErp2025_Dev", true);
                
                case "testing":
                case "test":
                    return TaoConnectionString("localhost", "VnsErp2025_Test", true);
                
                case "production":
                case "prod":
                    return LayConnectionString("VnsErp2025_Production");
                
                default:
                    return LayConnectionStringMacDinh();
            }
        }

        #endregion

        #region lopHoTro

        /// <summary>
        /// Class chứa thông tin connection string
        /// </summary>
        public class ConnectionStringInfo
        {
            #region thuocTinhDonGian

            /// <summary>
            /// Tên server
            /// </summary>
            public string Server { get; set; }

            /// <summary>
            /// Tên database
            /// </summary>
            public string Database { get; set; }

            /// <summary>
            /// Sử dụng Windows Authentication
            /// </summary>
            public bool IntegratedSecurity { get; set; }

            /// <summary>
            /// User ID
            /// </summary>
            public string UserId { get; set; }

            /// <summary>
            /// Password
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Connection timeout
            /// </summary>
            public int ConnectionTimeout { get; set; }

            /// <summary>
            /// Command timeout
            /// </summary>
            public int CommandTimeout { get; set; }

            /// <summary>
            /// Enable pooling
            /// </summary>
            public bool Pooling { get; set; }

            /// <summary>
            /// Minimum pool size
            /// </summary>
            public int MinPoolSize { get; set; }

            /// <summary>
            /// Maximum pool size
            /// </summary>
            public int MaxPoolSize { get; set; }

            #endregion

            #region phuongThuc

            /// <summary>
            /// Chuyển đổi thành connection string
            /// </summary>
            /// <returns>Connection string</returns>
            public override string ToString()
            {
                return ConnectionStringHelper.TaoConnectionStringChiTiet(
                    Server, Database, IntegratedSecurity, UserId, Password,
                    ConnectionTimeout, CommandTimeout, Pooling, MinPoolSize, MaxPoolSize);
            }

            #endregion
        }

        #endregion
    }
}
