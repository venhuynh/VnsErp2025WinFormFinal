using System;
using System.Data.SqlClient;

namespace Common.Appconfig
{
    /// <summary>
    /// DTO cho cấu hình database
    /// </summary>
    public class DatabaseConfigDto
    {
        /// <summary>
        /// Tên server database
        /// </summary>
        public string ServerName { get; set; }

        /// <summary>
        /// Tên database
        /// </summary>
        public string DatabaseName { get; set; }

        /// <summary>
        /// Phương thức xác thực
        /// </summary>
        public AuthenticationMethod AuthenticationMethod { get; set; } = AuthenticationMethod.WindowsAuthentication;

        /// <summary>
        /// User ID (nếu dùng SQL Server Authentication)
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Password (nếu dùng SQL Server Authentication)
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Connection timeout (giây)
        /// </summary>
        public int ConnectionTimeout { get; set; } = 15;

        /// <summary>
        /// Trust Server Certificate
        /// </summary>
        public bool TrustServerCertificate { get; set; } = false;

        /// <summary>
        /// Tạo connection string từ cấu hình
        /// </summary>
        /// <returns>Connection string</returns>
        public string GetConnectionString()
        {
            var builder = new SqlConnectionStringBuilder
            {
                DataSource = ServerName ?? "localhost",
                InitialCatalog = DatabaseName ?? "VnsErp2025",
                ConnectTimeout = ConnectionTimeout,
                TrustServerCertificate = TrustServerCertificate
            };

            if (AuthenticationMethod == AuthenticationMethod.SqlServerAuthentication)
            {
                builder.IntegratedSecurity = false;
                builder.UserID = UserId ?? string.Empty;
                builder.Password = Password ?? string.Empty;
            }
            else
            {
                builder.IntegratedSecurity = true;
            }

            return builder.ToString();
        }

        /// <summary>
        /// Clone cấu hình
        /// </summary>
        /// <returns>Bản sao của cấu hình</returns>
        public DatabaseConfigDto Clone()
        {
            return new DatabaseConfigDto
            {
                ServerName = ServerName,
                DatabaseName = DatabaseName,
                AuthenticationMethod = AuthenticationMethod,
                UserId = UserId,
                Password = Password,
                ConnectionTimeout = ConnectionTimeout,
                TrustServerCertificate = TrustServerCertificate
            };
        }

        /// <summary>
        /// Kiểm tra cấu hình có hợp lệ không
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public bool IsValid()
        {
            if (string.IsNullOrWhiteSpace(ServerName) || string.IsNullOrWhiteSpace(DatabaseName))
                return false;

            if (AuthenticationMethod == AuthenticationMethod.SqlServerAuthentication)
            {
                if (string.IsNullOrWhiteSpace(UserId))
                    return false;
            }

            if (ConnectionTimeout <= 0)
                return false;

            return true;
        }
    }
}

