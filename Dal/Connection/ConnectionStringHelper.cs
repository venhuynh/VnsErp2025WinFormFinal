using System;
using System.Data.SqlClient;
using System.Security;
using Microsoft.Win32;
using Common.Appconfig;

namespace Dal.Connection
{
    /// <summary>
    /// Helper quản lý Connection String cho toàn bộ ứng dụng.
    /// - Đọc cấu hình từ Registry (HKEY_CURRENT_USER\Software\Software\VietNhatSolutions\VnsErp2025).
    /// - Nếu không tìm thấy trong Registry, sẽ trả về chuỗi mặc định build sẵn.
    /// - Hỗ trợ tạo, phân tích, kiểm tra, mã hóa/giải mã, và ẩn mật khẩu.
    /// - Sử dụng VntaCrypto để mã hóa/giải mã tất cả thông tin (dns, database, username, password) khi lưu/đọc từ Registry.
    /// </summary>
    public static class ConnectionStringHelper
    {
        #region Constants

        private const string DEFAULT_CONNECTION_NAME = "VnsErp2025ConnectionString";
        private const string DEFAULT_SERVER = "localhost";
        private const string DEFAULT_DATABASE = "VnsErp2025Final";
        private const int DEFAULT_TIMEOUT = 30;
        private const int DEFAULT_CONNECTION_TIMEOUT = 15;
        private const string REGISTRY_KEY = @"HKEY_CURRENT_USER\Software\Software\VietNhatSolutions\VnsErp2025";

        #endregion

        #region Public API

        /// <summary>
        /// Lấy Connection String mặc định từ Registry.
        /// Nếu không tìm thấy trong Registry, sẽ trả về chuỗi mặc định build sẵn.
        /// </summary>
        public static string GetDefaultConnectionString()
        {
            try
            {
                var dbConfig = GetMsSqlServerInfo();
                if (dbConfig != null && !string.IsNullOrEmpty(dbConfig.Dns))
                {
                    var connectionString = $@"Data Source={dbConfig.Dns};Initial Catalog={dbConfig.Database};Persist Security Info=True;User ID={dbConfig.Username};Password={dbConfig.Password}";
                    return connectionString;
                }
            }
            catch (Exception)
            {
                // Fallback to default connection string
            }

            return BuildDefaultConnectionString();
        }

        /// <summary>
        /// Tạo Connection String mặc định (localhost, DB mặc định, Windows Authentication).
        /// </summary>
        private static string BuildDefaultConnectionString()
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
        private static string BuildConnectionString(string server, string database, bool integratedSecurity = true, 
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
        /// Ẩn mật khẩu trong Connection String để ghi log/hiển thị an toàn.
        /// </summary>
        /// <param name="connectionString">Connection string cần ẩn password</param>
        /// <returns>Connection string với password được thay bằng ***</returns>
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
                // Nếu không thể parse, trả về connection string gốc (không an toàn nhưng đảm bảo không lỗi)
                return connectionString;
            }
        }

        /// <summary>
        /// Lưu thông tin kết nối của MS Sql Server vào Registry
        /// Tất cả thông tin được mã hóa trước khi lưu vào Registry
        /// </summary>
        /// <param name="dns">Tên server/DNS</param>
        /// <param name="database">Tên database</param>
        /// <param name="username">Username</param>
        /// <param name="password">Password</param>
        public static void SetDbConfig(string dns, string database, string username, string password)
        {
            try
            {
                //dns - mã hóa trước khi lưu
                var encryptedDns = VntaCrypto.Encrypt(dns ?? string.Empty);
                Registry.SetValue(REGISTRY_KEY, "dns", encryptedDns);

                //database - mã hóa trước khi lưu
                var encryptedDatabase = VntaCrypto.Encrypt(database ?? string.Empty);
                Registry.SetValue(REGISTRY_KEY, "database", encryptedDatabase);

                //username - mã hóa trước khi lưu
                var encryptedUsername = VntaCrypto.Encrypt(username ?? string.Empty);
                Registry.SetValue(REGISTRY_KEY, "username", encryptedUsername);

                //password - mã hóa trước khi lưu
                var encryptedPassword = VntaCrypto.Encrypt(password ?? string.Empty);
                Registry.SetValue(REGISTRY_KEY, "password", encryptedPassword);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu cấu hình vào Registry: {ex.Message}", ex);
            }
        }

        #endregion

        #region Internal Helpers

        /// <summary>
        /// Đọc thông tin kết nối của MS Sql Server từ Registry
        /// Tất cả thông tin được giải mã khi đọc từ Registry
        /// </summary>
        /// <returns>DbConfigInfo chứa thông tin kết nối, hoặc null nếu không tìm thấy</returns>
        private static DbConfigInfo GetMsSqlServerInfo()
        {
            try
            {
                // Đọc và giải mã tất cả các trường từ Registry
                string encryptedDns = (string)Registry.GetValue(REGISTRY_KEY, "dns", "") ?? string.Empty;
                string encryptedDatabase = (string)Registry.GetValue(REGISTRY_KEY, "database", "") ?? string.Empty;
                string encryptedUsername = (string)Registry.GetValue(REGISTRY_KEY, "username", "") ?? string.Empty;
                string encryptedPassword = (string)Registry.GetValue(REGISTRY_KEY, "password", "") ?? string.Empty;

                var model = new DbConfigInfo
                {
                    Dns = string.Empty,
                    Database = string.Empty,
                    Username = string.Empty,
                    Password = string.Empty
                };

                // Giải mã dns
                if (!string.IsNullOrEmpty(encryptedDns))
                {
                    try
                    {
                        model.Dns = VntaCrypto.Decrypt(encryptedDns);
                    }
                    catch
                    {
                        // Nếu không thể giải mã, có thể là dữ liệu cũ chưa mã hóa, thử dùng trực tiếp
                        model.Dns = encryptedDns;
                    }
                }

                // Giải mã database
                if (!string.IsNullOrEmpty(encryptedDatabase))
                {
                    try
                    {
                        model.Database = VntaCrypto.Decrypt(encryptedDatabase);
                    }
                    catch
                    {
                        // Nếu không thể giải mã, có thể là dữ liệu cũ chưa mã hóa, thử dùng trực tiếp
                        model.Database = encryptedDatabase;
                    }
                }

                // Giải mã username
                if (!string.IsNullOrEmpty(encryptedUsername))
                {
                    try
                    {
                        model.Username = VntaCrypto.Decrypt(encryptedUsername);
                    }
                    catch
                    {
                        // Nếu không thể giải mã, có thể là dữ liệu cũ chưa mã hóa, thử dùng trực tiếp
                        model.Username = encryptedUsername;
                    }
                }

                // Giải mã password
                if (!string.IsNullOrEmpty(encryptedPassword))
                {
                    try
                    {
                        model.Password = VntaCrypto.Decrypt(encryptedPassword);
                    }
                    catch
                    {
                        // Nếu không thể giải mã, có thể là dữ liệu cũ chưa mã hóa, thử dùng trực tiếp
                        model.Password = encryptedPassword;
                    }
                }

                if (string.IsNullOrEmpty(model.Dns))
                    return null;

                return model;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        

        #region Nested Types

        /// <summary>
        /// Cấu trúc thông tin database config từ Registry
        /// </summary>
        private class DbConfigInfo
        {
            public string Dns { get; set; }
            public string Database { get; set; }
            public string Username { get; set; }
            public string Password { get; set; }
        }

        #endregion
    }
}
