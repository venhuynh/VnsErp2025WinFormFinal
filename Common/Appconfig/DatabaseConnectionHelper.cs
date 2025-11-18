using System;
using System.Data.SqlClient;
using System.Text;
using Common.Helpers;

namespace Common.Appconfig
{
    /// <summary>
    /// Helper class để quản lý cấu hình database trong file XML
    /// </summary>
    public class DatabaseConnectionHelper
    {
        #region ========== SINGLETON PATTERN ==========

        private static readonly Lazy<DatabaseConnectionHelper> _instance = new(() => new DatabaseConnectionHelper());

        /// <summary>
        /// Instance singleton của DatabaseConnectionHelper
        /// </summary>
        public static DatabaseConnectionHelper Instance => _instance.Value;

        #endregion

        #region ========== CONSTANTS ==========

        // Constants are now defined in ApplicationConstants class

        #endregion

        #region ========== PRIVATE CONSTRUCTOR ==========

        /// <summary>
        /// Private constructor cho singleton pattern
        /// </summary>
        private DatabaseConnectionHelper()
        {
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Kiểm tra xem có cấu hình database trong file XML không
        /// </summary>
        /// <returns>True nếu có cấu hình, False nếu không có</returns>
        public bool HasDatabaseConfig()
        {
            try
            {
                var configFilePath = XmlHelper.GetDatabaseConfigFilePath();
                if (string.IsNullOrEmpty(configFilePath) || !XmlHelper.XmlFileExists(configFilePath))
                    return false;

                var config = XmlHelper.LoadFromXmlToObject<DatabaseConfigDto>(configFilePath);
                if (config == null)
                    return false;

                // Kiểm tra xem có ít nhất ServerName và DatabaseName không
                return !string.IsNullOrWhiteSpace(config.ServerName) && !string.IsNullOrWhiteSpace(config.DatabaseName);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Load cấu hình database từ file XML
        /// </summary>
        /// <returns>DatabaseConfigDto chứa thông tin cấu hình</returns>
        public DatabaseConfigDto LoadDatabaseConfig()
        {
            try
            {
                var configFilePath = XmlHelper.GetDatabaseConfigFilePath();
                if (string.IsNullOrEmpty(configFilePath))
                {
                    throw new InvalidOperationException("Không thể lấy đường dẫn file cấu hình database");
                }

                if (!XmlHelper.XmlFileExists(configFilePath))
                {
                    throw new InvalidOperationException("Không tìm thấy file cấu hình database");
                }

                var config = XmlHelper.LoadFromXmlToObject<DatabaseConfigDto>(configFilePath);
                if (config == null)
                {
                    throw new InvalidOperationException("Không thể đọc cấu hình database từ file XML");
                }

                // Decrypt password
                if (!string.IsNullOrEmpty(config.Password))
                {
                    config.Password = DecryptPassword(config.Password);
                }

                return config;
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi đọc cấu hình database từ file XML: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lưu cấu hình database vào file XML
        /// </summary>
        /// <param name="config">Cấu hình database cần lưu</param>
        public void SaveDatabaseConfig(DatabaseConfigDto config)
        {
            if (config == null)
                throw new ArgumentNullException(nameof(config));

            try
            {
                var configFilePath = XmlHelper.GetDatabaseConfigFilePath();
                if (string.IsNullOrEmpty(configFilePath))
                {
                    throw new InvalidOperationException("Không thể lấy đường dẫn file cấu hình database");
                }

                // Clone config để không thay đổi object gốc
                var configToSave = config.Clone();
                
                // Re-encrypt password nếu đang ở định dạng cũ
                if (!string.IsNullOrEmpty(configToSave.Password))
                {
                    configToSave.Password = EnsurePasswordIsReEncrypted(configToSave.Password);
                }

                // Lưu vào file XML
                bool saveResult = XmlHelper.SaveToXml(configFilePath, configToSave);
                if (!saveResult)
                {
                    throw new InvalidOperationException("Không thể lưu cấu hình database vào file XML");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lưu cấu hình database vào file XML: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa cấu hình database (xóa file XML)
        /// </summary>
        public void DeleteDatabaseConfig()
        {
            try
            {
                var configFilePath = XmlHelper.GetDatabaseConfigFilePath();
                if (!string.IsNullOrEmpty(configFilePath))
                {
                    XmlHelper.DeleteXmlFile(configFilePath);
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa cấu hình database: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Test kết nối database với cấu hình đã cho
        /// </summary>
        /// <param name="config">Cấu hình database để test</param>
        /// <returns>True nếu kết nối thành công, False nếu thất bại</returns>
        public bool TestDatabaseConnection(DatabaseConfigDto config)
        {
            if (config == null || !config.IsValid())
                return false;

            try
            {
                var connectionString = config.GetConnectionString();
                
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    
                    // Thực hiện query đơn giản để test
                    using (var command = new SqlCommand("SELECT 1", connection))
                    {
                        var result = command.ExecuteScalar();
                        return result != null && result.ToString() == "1";
                    }
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Lấy connection string từ cấu hình hiện tại trong file XML
        /// </summary>
        /// <returns>Connection string hoặc empty string nếu không có cấu hình</returns>
        public string GetConnectionString()
        {
            try
            {
                if (!HasDatabaseConfig())
                    return string.Empty;

                var config = LoadDatabaseConfig();
                return config.GetConnectionString();
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        #endregion

        #region ========== PRIVATE METHODS ==========

        /// <summary>
        /// Đảm bảo password được re-encrypt nếu đang ở định dạng cũ
        /// </summary>
        /// <param name="password">Password cần kiểm tra và re-encrypt</param>
        /// <returns>Password đã được encrypt với VntaCrypto</returns>
        private string EnsurePasswordIsReEncrypted(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            try
            {
                // Kiểm tra xem password có đang ở định dạng VntaCrypto không
                if (VntaCrypto.IsEncrypted(password))
                {
                    // Đã được encrypt bằng VntaCrypto, return nguyên vẹn
                    return password;
                }

                // Có thể là password cũ (Base64 hoặc plain text)
                // Thử decrypt để lấy plain text, sau đó encrypt lại
                string plainPassword;
                
                // Thử decrypt như password cũ (Base64)
                try
                {
                    byte[] data = Convert.FromBase64String(password);
                    plainPassword = Encoding.UTF8.GetString(data);
                }
                catch
                {
                    // Không phải Base64, có thể là plain text
                    plainPassword = password;
                }

                // Re-encrypt bằng VntaCrypto
                return VntaCrypto.Encrypt(plainPassword);
            }
            catch (Exception ex)
            {
                // Log error và fallback về password gốc
                System.Diagnostics.Debug.WriteLine($"Lỗi re-encrypt password: {ex.Message}");
                return EncryptPassword(password); // Fallback về encrypt thông thường
            }
        }

        /// <summary>
        /// Mã hóa password sử dụng VntaCrypto
        /// </summary>
        /// <param name="password">Password cần mã hóa</param>
        /// <returns>Password đã mã hóa</returns>
        private string EncryptPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                return string.Empty;

            try
            {
                // Sử dụng VntaCrypto với key mặc định
                return VntaCrypto.Encrypt(password);
            }
            catch (Exception ex)
            {
                // Log error và fallback về password gốc (không an toàn nhưng đảm bảo ứng dụng hoạt động)
                System.Diagnostics.Debug.WriteLine($"Lỗi mã hóa password: {ex.Message}");
                return password; // Fallback về password gốc nếu không encrypt được
            }
        }

        /// <summary>
        /// Giải mã password sử dụng VntaCrypto
        /// </summary>
        /// <param name="encryptedPassword">Password đã mã hóa</param>
        /// <returns>Password gốc</returns>
        private string DecryptPassword(string encryptedPassword)
        {
            if (string.IsNullOrEmpty(encryptedPassword))
                return string.Empty;

            try
            {
                // Kiểm tra xem có phải dữ liệu đã mã hóa bằng VntaCrypto không
                if (VntaCrypto.IsEncrypted(encryptedPassword))
                {
                    return VntaCrypto.Decrypt(encryptedPassword);
                }
                else
                {
                    // Có thể là password cũ được mã hóa bằng Base64, thử giải mã theo cách cũ
                    try
                    {
                        var bytes = Convert.FromBase64String(encryptedPassword);
                        var oldPassword = Encoding.UTF8.GetString(bytes);
                        
                        // Re-encrypt bằng VntaCrypto để migration
                        System.Diagnostics.Debug.WriteLine("Migrating old password encryption to VntaCrypto");
                        return oldPassword;
                    }
                    catch
                    {
                        // Nếu không phải Base64, có thể là plaintext
                        return encryptedPassword;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi giải mã password: {ex.Message}");
                return encryptedPassword; // Fallback về encrypted password nếu không decrypt được
            }
        }

        #endregion
    }
}