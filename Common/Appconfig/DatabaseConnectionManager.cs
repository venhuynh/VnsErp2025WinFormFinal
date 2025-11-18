using System;
using Common.Utils;

namespace Common.Appconfig
{
    /// <summary>
    /// Manager quản lý connection string toàn cục cho ứng dụng VNTA.NET 2025.
    /// Đảm bảo LoadDatabaseConfig chỉ được gọi 1 lần duy nhất khi khởi động.
    /// Sử dụng pattern Singleton với thread-safe implementation.
    /// </summary>
    public sealed class DatabaseConnectionManager : IDisposable
    {
        #region ========== SINGLETON PATTERN ==========

        private static volatile DatabaseConnectionManager _instance;
        private static readonly object Lock = new();

        private DatabaseConnectionManager()
        {
            // Private constructor để đảm bảo singleton pattern
        }

        /// <summary>
        /// Instance duy nhất của DatabaseConnectionManager (Thread-safe Singleton)
        /// </summary>
        public static DatabaseConnectionManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (Lock)
                    {
                        if (_instance == null)
                        {
                            _instance = new DatabaseConnectionManager();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        #region ========== PRIVATE FIELDS ==========

        /// <summary>
        /// Connection string đã được cache
        /// </summary>
        private string _cachedConnectionString;

        /// <summary>
        /// Database configuration đã được cache
        /// </summary>
        private DatabaseConfigDto _cachedDatabaseConfig;

        /// <summary>
        /// Flag đánh dấu đã khởi tạo hay chưa
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// Lock object để đảm bảo thread safety cho việc khởi tạo
        /// </summary>
        private readonly object _initLock = new();

        /// <summary>
        /// Flag đánh dấu đã dispose hay chưa
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// Timestamp của lần khởi tạo cuối cùng
        /// </summary>
        private DateTime? _lastInitialized;

        #endregion

        #region ========== PUBLIC PROPERTIES ==========

        /// <summary>
        /// Connection string đã được cache (chỉ đọc)
        /// Tự động khởi tạo nếu chưa được khởi tạo
        /// </summary>
        public string ConnectionString
        {
            get
            {
                EnsureInitialized();
                return _cachedConnectionString;
            }
        }

        /// <summary>
        /// Database configuration đã được cache (chỉ đọc)
        /// Tự động khởi tạo nếu chưa được khởi tạo
        /// </summary>
        public DatabaseConfigDto DatabaseConfig
        {
            get
            {
                EnsureInitialized();
                return _cachedDatabaseConfig?.Clone(); // Trả về bản sao để tránh modification
            }
        }

        /// <summary>
        /// Kiểm tra xem đã khởi tạo hay chưa
        /// </summary>
        public bool IsInitialized => _isInitialized;

        /// <summary>
        /// Thời gian khởi tạo cuối cùng
        /// </summary>
        public DateTime? LastInitialized => _lastInitialized;

        /// <summary>
        /// Thông tin tóm tắt về kết nối hiện tại
        /// </summary>
        public string ConnectionSummary
        {
            get
            {
                if (!_isInitialized || _cachedDatabaseConfig == null)
                    return "Chưa khởi tạo";
                
                return $"Server: {_cachedDatabaseConfig.ServerName}, Auth: {_cachedDatabaseConfig.AuthenticationMethod}";
            }
        }

        #endregion

        #region ========== PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo DatabaseConnectionManager (chỉ được gọi 1 lần duy nhất)
        /// Tự động load cấu hình từ file XML
        /// </summary>
        /// <returns>True nếu khởi tạo thành công</returns>
        public bool Initialize()
        {
            if (_isInitialized)
            {
                LogMessage("DatabaseConnectionManager đã được khởi tạo trước đó");
                return true;
            }

            lock (_initLock)
            {
                if (_isInitialized)
                {
                    return true;
                }

                try
                {
                    LogMessage("Bắt đầu khởi tạo DatabaseConnectionManager");

                    // Load database configuration từ file XML
                    var dbHelper = DatabaseConnectionHelper.Instance;
                    var dbConfig = dbHelper.LoadDatabaseConfig();
                    
                    if (dbConfig == null || !dbConfig.IsValid())
                    {
                        LogError("Không thể load database configuration hoặc config không hợp lệ");
                        return false;
                    }

                    // Cache database config
                    _cachedDatabaseConfig = dbConfig;

                    // Tạo connection string
                    _cachedConnectionString = dbConfig.GetConnectionString();
                    if (string.IsNullOrEmpty(_cachedConnectionString))
                    {
                        LogError("Không thể tạo connection string");
                        return false;
                    }

                    // Test connection để đảm bảo có thể kết nối
                    if (!dbHelper.TestDatabaseConnection(dbConfig))
                    {
                        LogError("Không thể kết nối đến database");
                        return false;
                    }

                    _isInitialized = true;
                    _lastInitialized = DateTime.Now;
                    
                    LogMessage($"Khởi tạo DatabaseConnectionManager thành công - Server: {dbConfig.ServerName}, Auth: {dbConfig.AuthenticationMethod}");
                    return true;
                }
                catch (Exception ex)
                {
                    LogError($"Lỗi khi khởi tạo DatabaseConnectionManager: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Khởi tạo với database configuration cụ thể (cho testing hoặc custom config)
        /// </summary>
        /// <param name="dbConfig">Database configuration</param>
        /// <returns>True nếu khởi tạo thành công</returns>
        public bool Initialize(DatabaseConfigDto dbConfig)
        {
            if (_isInitialized)
            {
                LogMessage("DatabaseConnectionManager đã được khởi tạo trước đó");
                return true;
            }

            if (dbConfig == null || !dbConfig.IsValid())
            {
                LogError("Database configuration không hợp lệ");
                return false;
            }

            lock (_initLock)
            {
                if (_isInitialized)
                {
                    return true;
                }

                try
                {
                    LogMessage("Bắt đầu khởi tạo DatabaseConnectionManager với config tùy chỉnh");

                    // Cache database config
                    _cachedDatabaseConfig = dbConfig.Clone();

                    // Tạo connection string
                    _cachedConnectionString = dbConfig.GetConnectionString();
                    if (string.IsNullOrEmpty(_cachedConnectionString))
                    {
                        LogError("Không thể tạo connection string");
                        return false;
                    }

                    // Test connection để đảm bảo có thể kết nối
                    var dbHelper = DatabaseConnectionHelper.Instance;
                    if (!dbHelper.TestDatabaseConnection(dbConfig))
                    {
                        LogError("Không thể kết nối đến database");
                        return false;
                    }

                    _isInitialized = true;
                    _lastInitialized = DateTime.Now;
                    
                    LogMessage($"Khởi tạo DatabaseConnectionManager thành công với config tùy chỉnh - Server: {dbConfig.ServerName}, Auth: {dbConfig.AuthenticationMethod}");
                    return true;
                }
                catch (Exception ex)
                {
                    LogError($"Lỗi khi khởi tạo DatabaseConnectionManager với config tùy chỉnh: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Reset và khởi tạo lại (chỉ dùng khi cần thiết - ví dụ: thay đổi config)
        /// </summary>
        /// <returns>True nếu reset thành công</returns>
        public bool Reset()
        {
            lock (_initLock)
            {
                try
                {
                    LogMessage("Reset DatabaseConnectionManager");

                    _cachedConnectionString = null;
                    _cachedDatabaseConfig = null;
                    _isInitialized = false;
                    _lastInitialized = null;

                    LogMessage("Reset DatabaseConnectionManager thành công");
                    return true;
                }
                catch (Exception ex)
                {
                    LogError($"Lỗi khi reset DatabaseConnectionManager: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Refresh connection string từ file XML hiện tại
        /// </summary>
        /// <returns>True nếu refresh thành công</returns>
        public bool RefreshConnectionString()
        {
            lock (_initLock)
            {
                try
                {
                    LogMessage("Refresh connection string từ file XML");

                    // Load lại database configuration từ file XML
                    var dbHelper = DatabaseConnectionHelper.Instance;
                    var newDbConfig = dbHelper.LoadDatabaseConfig();
                    
                    if (newDbConfig == null || !newDbConfig.IsValid())
                    {
                        LogError("Không thể load database configuration hoặc config không hợp lệ khi refresh");
                        return false;
                    }

                    // Cập nhật cache
                    _cachedDatabaseConfig = newDbConfig;
                    _cachedConnectionString = newDbConfig.GetConnectionString();
                    
                    if (string.IsNullOrEmpty(_cachedConnectionString))
                    {
                        LogError("Không thể tạo connection string khi refresh");
                        return false;
                    }

                    _isInitialized = true;
                    _lastInitialized = DateTime.Now;
                    
                    LogMessage($"Refresh connection string thành công - Server: {newDbConfig.ServerName}, Auth: {newDbConfig.AuthenticationMethod}");
                    return true;
                }
                catch (Exception ex)
                {
                    LogError($"Lỗi khi refresh connection string: {ex.Message}");
                    return false;
                }
            }
        }

        /// <summary>
        /// Kiểm tra xem có thể kết nối database hay không
        /// </summary>
        /// <returns>True nếu có thể kết nối</returns>
        public bool TestConnection()
        {
            EnsureInitialized();
            
            try
            {
                var dbHelper = DatabaseConnectionHelper.Instance;
                return dbHelper.TestDatabaseConnection(_cachedDatabaseConfig);
            }
            catch (Exception ex)
            {
                LogError($"Lỗi khi test connection: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Hiển thị thông tin chi tiết về kết nối hiện tại cho user
        /// </summary>
        public void ShowConnectionInfo()
        {
            try
            {
                if (!_isInitialized)
                {
                    MsgBox.ShowWarning("DatabaseConnectionManager chưa được khởi tạo.", "Thông tin kết nối");
                    return;
                }

                string info = $"Thông tin kết nối database:\n\n" +
                             $"Server: {_cachedDatabaseConfig.ServerName}\n" +
                             $"Database: {_cachedDatabaseConfig.DatabaseName}\n" +
                             $"Xác thực: {_cachedDatabaseConfig.AuthenticationMethod}\n" +
                             $"User ID: {(_cachedDatabaseConfig.AuthenticationMethod == AuthenticationMethod.SqlServerAuthentication ? _cachedDatabaseConfig.UserId : "Windows Authentication")}\n" +
                             $"Timeout: {_cachedDatabaseConfig.ConnectionTimeout} giây\n" +
                             $"Trust Server Certificate: {(_cachedDatabaseConfig.TrustServerCertificate ? "Có" : "Không")}\n";

                if (_lastInitialized.HasValue)
                {
                    info += $"Khởi tạo lần cuối: {_lastInitialized.Value:yyyy-MM-dd HH:mm:ss}\n";
                }

                // Test connection để kiểm tra trạng thái hiện tại
                bool canConnect = TestConnection();
                info += $"Trạng thái kết nối: {(canConnect ? "Kết nối thành công" : "Kết nối thất bại")}";

                if (canConnect)
                {
                    MsgBox.ShowSuccess(info, "Thông tin kết nối database");
                }
                else
                {
                    MsgBox.ShowWarning(info, "Thông tin kết nối database");
                }
            }
            catch (Exception ex)
            {
                MsgBox.ShowError($"Lỗi khi hiển thị thông tin kết nối: {ex.Message}");
            }
        }

        #endregion

        #region ========== PRIVATE METHODS ==========

        /// <summary>
        /// Đảm bảo DatabaseConnectionManager đã được khởi tạo
        /// Tự động khởi tạo nếu chưa được khởi tạo
        /// </summary>
        private void EnsureInitialized()
        {
            if (!_isInitialized)
            {
                // Thử khởi tạo tự động
                if (!Initialize())
                {
                    throw new InvalidOperationException("DatabaseConnectionManager chưa được khởi tạo và không thể tự động khởi tạo. Hãy kiểm tra cấu hình database.");
                }
            }
        }

        /// <summary>
        /// Ghi log thông tin (có thể thay thế bằng MsgBox khi cần debug)
        /// </summary>
        /// <param name="message">Thông điệp</param>
        private void LogMessage(string message)
        {
            // Debug mode: hiển thị thông tin bằng Debug
            if (System.Diagnostics.Debugger.IsAttached)
            {
                System.Diagnostics.Debug.WriteLine($"[DatabaseConnectionManager] {DateTime.Now:yyyy-MM-dd HH:mm:ss} INFO: {message}");
            }
        }

        /// <summary>
        /// Ghi log lỗi (sử dụng Debug output)
        /// </summary>
        /// <param name="message">Thông điệp lỗi</param>
        /// <param name="showToUser">Có hiển thị cho user không (mặc định: false)</param>
        private void LogError(string message, bool showToUser = false)
        {
            // Luôn ghi vào debug output
            System.Diagnostics.Debug.WriteLine($"[DatabaseConnectionManager] {DateTime.Now:yyyy-MM-dd HH:mm:ss} ERROR: {message}");
            
            // Hiển thị cho user nếu được yêu cầu
            if (showToUser)
            {
                MsgBox.ShowError(message, "Lỗi Database Connection Manager");
            }
        }

        #endregion

        #region ========== IDISPOSABLE IMPLEMENTATION ==========

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        /// <param name="disposing">True nếu đang dispose</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed && disposing)
            {
                try
                {
                    LogMessage("Dispose DatabaseConnectionManager");

                    lock (_initLock)
                    {
                        _cachedConnectionString = null;
                        _cachedDatabaseConfig = null;
                        _isInitialized = false;
                        _lastInitialized = null;
                        _disposed = true;
                    }

                    LogMessage("Dispose DatabaseConnectionManager thành công");
                }
                catch (Exception ex)
                {
                    LogError($"Lỗi khi dispose DatabaseConnectionManager: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Finalizer
        /// </summary>
        ~DatabaseConnectionManager()
        {
            Dispose(false);
        }

        #endregion
    }
}