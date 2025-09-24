using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security;
using System.Text;
using System.Linq;
using System.Reflection;

namespace Dal.Connection
{
    /// <summary>
    /// Helper quản lý Connection String cho toàn bộ ứng dụng.
    /// - Ưu tiên nguồn cấu hình: User Settings (do UI lưu) → ConnectionStrings (App.config) → Mặc định.
    /// - Hỗ trợ tạo, phân tích, kiểm tra, mã hóa/giải mã, và ẩn mật khẩu.
    /// - Đọc User Settings qua reflection để không phụ thuộc dự án UI.
    /// </summary>
    public static class ConnectionStringHelper
    {
        #region Constants

        private const string DEFAULT_CONNECTION_NAME = "VnsErp2025ConnectionString";
        private const string DEFAULT_SERVER = "localhost";
        private const string DEFAULT_DATABASE = "VnsErp2025Final";
        private const int DEFAULT_TIMEOUT = 30;
        private const int DEFAULT_CONNECTION_TIMEOUT = 15;

        #endregion

        #region Public API

        /// <summary>
        /// Lấy Connection String mặc định theo luồng ưu tiên:
        /// 1) User Settings (UI) → 2) ConnectionStrings trong cấu hình → 3) Chuỗi mặc định build sẵn.
        /// </summary>
        public static string GetDefaultConnectionString()
        {
            try
            {
                if (TryGetFromUserSettings(out var csFromSettings))
                {
                    return csFromSettings;
                }

                var connectionString = ConfigurationManager.ConnectionStrings[DEFAULT_CONNECTION_NAME]?.ConnectionString;
                
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = BuildDefaultConnectionString();
                }

                return connectionString;
            }
            catch (Exception)
            {
                return BuildDefaultConnectionString();
            }
        }

        /// <summary>
        /// Lấy Connection String theo tên trong ConnectionStrings của cấu hình ứng dụng.
        /// </summary>
        /// <param name="connectionName">Tên Connection String trong App.config/Web.config</param>
        /// <returns>Connection String tương ứng</returns>
        /// <exception cref="ConfigurationErrorsException">Ném lỗi khi không tìm thấy hoặc cấu hình không hợp lệ</exception>
        public static string GetConnectionStringByName(string connectionName)
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
        /// Tạo Connection String mặc định (localhost, DB mặc định, Windows Authentication).
        /// </summary>
        public static string BuildDefaultConnectionString()
        {
            return BuildConnectionString(DEFAULT_SERVER, DEFAULT_DATABASE, true, null, null);
        }

        /// <summary>
        /// Tạo Connection String với thông tin cơ bản.
        /// </summary>
        /// <param name="server">Tên server</param>
        /// <param name="database">Tên database</param>
        /// <param name="integratedSecurity">Sử dụng Windows Authentication</param>
        /// <param name="userId">User ID (nếu không dùng Windows Auth)</param>
        /// <param name="password">Password (nếu không dùng Windows Auth)</param>
        public static string BuildConnectionString(string server, string database, bool integratedSecurity = true, 
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
        /// Tạo Connection String chi tiết bằng SqlConnectionStringBuilder.
        /// </summary>
        public static string BuildDetailedConnectionString(string server, string database, bool integratedSecurity = true,
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
        /// Phân tích Connection String thành cấu trúc thông tin.
        /// </summary>
        public static ConnectionStringInfo ParseConnectionString(string connectionString)
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
                    CommandTimeout = DEFAULT_TIMEOUT, // SqlConnectionStringBuilder không có property này
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
        /// Kiểm tra Connection String có hợp lệ (server/database + thông tin xác thực nếu cần).
        /// </summary>
        public static bool IsValidConnectionString(string connectionString)
        {
            try
            {
                if (string.IsNullOrEmpty(connectionString))
                    return false;

                var builder = new SqlConnectionStringBuilder(connectionString);
                
                if (string.IsNullOrEmpty(builder.DataSource) || string.IsNullOrEmpty(builder.InitialCatalog))
                    return false;

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
        /// Mã hóa Connection String (Base64) để lưu trữ an toàn hơn.
        /// </summary>
        public static string EncodeConnectionString(string connectionString)
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
        /// Giải mã Connection String đã mã hóa bằng Base64.
        /// </summary>
        public static string DecodeConnectionString(string encryptedConnectionString)
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
        /// Ẩn mật khẩu trong Connection String để ghi log/hiển thị an toàn.
        /// </summary>
        public static string GetSafeConnectionString(string connectionString)
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
                return connectionString;
            }
        }

        /// <summary>
        /// Tạo Connection String theo môi trường (Development/Testing/Production).
        /// </summary>
        public static string BuildByEnvironment(string environment)
        {
            switch (environment?.ToLower())
            {
                case "development":
                case "dev":
                    return BuildConnectionString("localhost", "VnsErp2025_Dev", true);
                
                case "testing":
                case "test":
                    return BuildConnectionString("localhost", "VnsErp2025_Test", true);
                
                case "production":
                case "prod":
                    return GetConnectionStringByName("VnsErp2025_Production");
                
                default:
                    return GetDefaultConnectionString();
            }
        }

        #endregion

        #region Internal Helpers

        /// <summary>
        /// Thử lấy Connection String từ User Settings (Properties.Settings) bằng reflection.
        /// Kỳ vọng các khóa: DatabaseServer, DatabaseName, UseIntegratedSecurity, DatabaseUserId, DatabasePassword.
        /// </summary>
        private static bool TryGetFromUserSettings(out string connectionString)
        {
            connectionString = null;
            try
            {
                var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in assemblies)
                {
                    var settingsType = asm.GetTypes()
                        .FirstOrDefault(t => t.Name == "Settings" && t.Namespace != null && t.Namespace.EndsWith(".Properties", StringComparison.OrdinalIgnoreCase));
                    if (settingsType == null) continue;

                    var defaultProp = settingsType.GetProperty("Default", BindingFlags.Public | BindingFlags.Static);
                    if (defaultProp == null) continue;
                    var settingsInstance = defaultProp.GetValue(null);
                    if (settingsInstance == null) continue;

                    string server = ReadSetting<string>(settingsInstance, "DatabaseServer");
                    string database = ReadSetting<string>(settingsInstance, "DatabaseName");
                    bool? useIntegrated = ReadSetting<bool?>(settingsInstance, "UseIntegratedSecurity");
                    string userId = ReadSetting<string>(settingsInstance, "DatabaseUserId");
                    string encPwd = ReadSetting<string>(settingsInstance, "DatabasePassword");
                    string password = string.IsNullOrEmpty(encPwd) ? null : DecodeConnectionString(encPwd);

                    if (!string.IsNullOrEmpty(server) && !string.IsNullOrEmpty(database))
                    {
                        bool integrated = useIntegrated ?? true;
                        connectionString = BuildDetailedConnectionString(
                            server,
                            database,
                            integrated,
                            integrated ? null : userId,
                            integrated ? null : password,
                            DEFAULT_CONNECTION_TIMEOUT,
                            DEFAULT_TIMEOUT,
                            true,
                            1,
                            100
                        );
                        return true;
                    }
                }
            }
            catch
            {
                // Bỏ qua và fallback
            }
            return false;
        }

        /// <summary>
        /// Đọc một thuộc tính từ đối tượng Settings (Properties.Settings.Default) một cách an toàn.
        /// </summary>
        private static T ReadSetting<T>(object settingsInstance, string propertyName)
        {
            try
            {
                var prop = settingsInstance.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
                if (prop == null) return default;
                var value = prop.GetValue(settingsInstance);
                if (value == null) return default;
                return (T)value;
            }
            catch
            {
                return default;
            }
        }

        #endregion

        #region Obsolete Aliases (Backward Compatibility)

        [Obsolete("Use GetDefaultConnectionString() instead")]
        public static string LayConnectionStringMacDinh() => GetDefaultConnectionString();

        [Obsolete("Use GetConnectionStringByName(string) instead")]
        public static string LayConnectionString(string connectionName) => GetConnectionStringByName(connectionName);

        [Obsolete("Use BuildDefaultConnectionString() instead")]
        public static string TaoConnectionStringMacDinh() => BuildDefaultConnectionString();

        [Obsolete("Use BuildConnectionString(string,string,bool,string,string) instead")]
        public static string TaoConnectionString(string server, string database, bool integratedSecurity = true, string userId = null, string password = null)
            => BuildConnectionString(server, database, integratedSecurity, userId, password);

        [Obsolete("Use BuildDetailedConnectionString(...) instead")]
        public static string TaoConnectionStringChiTiet(string server, string database, bool integratedSecurity = true,
            string userId = null, string password = null, int timeout = DEFAULT_CONNECTION_TIMEOUT,
            int commandTimeout = DEFAULT_TIMEOUT, bool pooling = true, int minPoolSize = 1, int maxPoolSize = 100)
            => BuildDetailedConnectionString(server, database, integratedSecurity, userId, password, timeout, commandTimeout, pooling, minPoolSize, maxPoolSize);

        [Obsolete("Use ParseConnectionString(string) instead")]
        public static ConnectionStringInfo PhanTichConnectionString(string connectionString) => ParseConnectionString(connectionString);

        [Obsolete("Use IsValidConnectionString(string) instead")]
        public static bool KiemTraConnectionString(string connectionString) => IsValidConnectionString(connectionString);

        [Obsolete("Use EncodeConnectionString(string) instead")]
        public static string MaHoaConnectionString(string connectionString) => EncodeConnectionString(connectionString);

        [Obsolete("Use DecodeConnectionString(string) instead")]
        public static string GiaiMaConnectionString(string encryptedConnectionString) => DecodeConnectionString(encryptedConnectionString);

        [Obsolete("Use GetSafeConnectionString(string) instead")]
        public static string LayConnectionStringAnToan(string connectionString) => GetSafeConnectionString(connectionString);

        [Obsolete("Use BuildByEnvironment(string) instead")]
        public static string TaoConnectionStringTheoEnvironment(string environment) => BuildByEnvironment(environment);

        #endregion

        #region Nested Types

        /// <summary>
        /// Cấu trúc thông tin connection string (để parse/hiển thị).
        /// </summary>
        public class ConnectionStringInfo
        {
            #region Fields & Properties

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

            #region Methods

            /// <summary>
            /// Chuyển đổi thành connection string chi tiết.
            /// </summary>
            public override string ToString()
            {
                return ConnectionStringHelper.BuildDetailedConnectionString(
                    Server, Database, IntegratedSecurity, UserId, Password,
                    ConnectionTimeout, CommandTimeout, Pooling, MinPoolSize, MaxPoolSize);
            }

            #endregion
        }

        #endregion
    }
}
