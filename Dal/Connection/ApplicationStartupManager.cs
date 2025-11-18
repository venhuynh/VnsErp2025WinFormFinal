using System;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using Common.Appconfig;

namespace Dal.Connection;

/// <summary>
/// Manager quản lý quá trình khởi động ứng dụng.
/// Đảm bảo các thành phần quan trọng được khởi tạo đúng thứ tự.
/// Quản lý connection string toàn cục cho DAL layer.
/// </summary>
public sealed class ApplicationStartupManager
{
    #region Singleton Pattern

    private static volatile ApplicationStartupManager _instance;
    private static readonly object Lock = new object();

    private ApplicationStartupManager()
    {
        // Khởi tạo logger
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger?.Info("ApplicationStartupManager instance được tạo");
    }

    /// <summary>
    /// Instance duy nhất của ApplicationStartupManager
    /// </summary>
    public static ApplicationStartupManager Instance
    {
        get
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    if (_instance == null)
                    {
                        _instance = new ApplicationStartupManager();
                    }
                }
            }
            return _instance;
        }
    }

    #endregion

    #region Private Fields

    /// <summary>
    /// Flag đánh dấu đã khởi tạo hay chưa
    /// </summary>
    private bool _isInitialized;

    /// <summary>
    /// Lock object để đảm bảo thread safety
    /// </summary>
    private readonly object _initLock = new object();

    /// <summary>
    /// Logger để ghi log
    /// </summary>
    private readonly ILogger _logger;

    #endregion

    #region Public Properties

    /// <summary>
    /// Kiểm tra xem đã khởi tạo hay chưa
    /// </summary>
    public bool IsInitialized => _isInitialized;

    /// <summary>
    /// Connection string toàn cục (chỉ đọc)
    /// </summary>
    public string GlobalConnectionString => DatabaseConnectionManager.Instance.ConnectionString;

    /// <summary>
    /// Database configuration toàn cục (chỉ đọc)
    /// </summary>
    public object GlobalDatabaseConfig => DatabaseConnectionManager.Instance.DatabaseConfig;

    #endregion

    #region Public Methods

    /// <summary>
    /// Khởi tạo ứng dụng (chỉ được gọi 1 lần duy nhất khi chương trình khởi động)
    /// </summary>
    /// <returns>True nếu khởi tạo thành công</returns>
    public bool InitializeApplication()
    {
        if (_isInitialized)
        {
            _logger?.Info("ApplicationStartupManager đã được khởi tạo trước đó");
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
                _logger?.Info("Bắt đầu khởi tạo ApplicationStartupManager");

                // Bước 1: Khởi tạo DatabaseConnectionManager
                _logger?.Info("Bước 1: Khởi tạo DatabaseConnectionManager");
                if (!DatabaseConnectionManager.Instance.Initialize())
                {
                    _logger?.Error("Không thể khởi tạo DatabaseConnectionManager");
                    return false;
                }
                _logger?.Info("DatabaseConnectionManager đã được khởi tạo thành công");

                // Bước 2: Kiểm tra kết nối database
                _logger?.Info("Bước 2: Kiểm tra kết nối database");
                if (!DatabaseConnectionManager.Instance.TestConnection())
                {
                    _logger?.Error("Không thể kết nối đến database");
                    return false;
                }
                _logger?.Info("Kết nối database thành công");

                // Bước 3: Khởi tạo các manager khác (nếu có)
                _logger?.Info("Bước 3: Khởi tạo các manager khác");
                InitializeOtherManagers();

                _isInitialized = true;
                _logger?.Info($"ApplicationStartupManager đã được khởi tạo thành công. Connection String: {DatabaseConnectionManager.Instance.ConnectionString?.Substring(0, Math.Min(50, DatabaseConnectionManager.Instance.ConnectionString?.Length ?? 0))}...");

                return true;
            }
            catch (Exception ex)
            {
                _logger?.Error($"Lỗi khi khởi tạo ApplicationStartupManager: {ex.Message}", ex);
                return false;
            }
        }
    }

    /// <summary>
    /// Khởi tạo ứng dụng với database configuration tùy chỉnh
    /// </summary>
    /// <param name="customDbConfig">Database configuration tùy chỉnh</param>
    /// <returns>True nếu khởi tạo thành công</returns>
    public bool InitializeApplication(DatabaseConfigDto customDbConfig)
    {
        if (_isInitialized)
        {
            _logger?.Info("ApplicationStartupManager đã được khởi tạo trước đó");
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
                _logger?.Info("Bắt đầu khởi tạo ApplicationStartupManager với config tùy chỉnh");

                // Bước 1: Khởi tạo DatabaseConnectionManager với config tùy chỉnh
                _logger?.Info($"Bước 1: Khởi tạo DatabaseConnectionManager với Server: {customDbConfig?.ServerName}, Database: {customDbConfig?.DatabaseName}");
                if (!DatabaseConnectionManager.Instance.Initialize(customDbConfig))
                {
                    _logger?.Error("Không thể khởi tạo DatabaseConnectionManager với config tùy chỉnh");
                    return false;
                }
                _logger?.Info("DatabaseConnectionManager đã được khởi tạo thành công với config tùy chỉnh");

                // Bước 2: Kiểm tra kết nối database
                _logger?.Info("Bước 2: Kiểm tra kết nối database");
                if (!DatabaseConnectionManager.Instance.TestConnection())
                {
                    _logger?.Error("Không thể kết nối đến database");
                    return false;
                }
                _logger?.Info("Kết nối database thành công");

                // Bước 3: Khởi tạo các manager khác (nếu có)
                _logger?.Info("Bước 3: Khởi tạo các manager khác");
                InitializeOtherManagers();

                _isInitialized = true;
                _logger?.Info($"ApplicationStartupManager đã được khởi tạo thành công với config tùy chỉnh. Connection String: {DatabaseConnectionManager.Instance.ConnectionString?.Substring(0, Math.Min(50, DatabaseConnectionManager.Instance.ConnectionString?.Length ?? 0))}...");

                return true;
            }
            catch (Exception ex)
            {
                _logger?.Error($"Lỗi khi khởi tạo ApplicationStartupManager với config tùy chỉnh: {ex.Message}", ex);
                return false;
            }
        }
    }

    /// <summary>
    /// Kiểm tra trạng thái ứng dụng
    /// </summary>
    /// <returns>True nếu ứng dụng đã sẵn sàng</returns>
    public bool IsApplicationReady()
    {
        return _isInitialized && DatabaseConnectionManager.Instance.IsInitialized;
    }

    /// <summary>
    /// Lấy connection string toàn cục
    /// Cho phép lấy connection string từ DatabaseConnectionManager ngay cả khi ApplicationStartupManager chưa được initialized
    /// DatabaseConnectionManager sẽ tự động khởi tạo nếu chưa được khởi tạo
    /// </summary>
    /// <returns>Connection string hoặc null nếu không thể lấy được</returns>
    public string GetGlobalConnectionString()
    {
        try
        {
            // Nếu ApplicationStartupManager đã được initialized, sử dụng GlobalConnectionString
            if (IsApplicationReady())
            {
                _logger?.Debug("Lấy connection string từ ApplicationStartupManager (đã initialized)");
                return GlobalConnectionString;
            }

            // Nếu chưa initialized, thử lấy trực tiếp từ DatabaseConnectionManager
            // DatabaseConnectionManager có thể tự động khởi tạo thông qua EnsureInitialized()
            var dbManager = DatabaseConnectionManager.Instance;
            if (dbManager.IsInitialized)
            {
                _logger?.Debug("Lấy connection string từ DatabaseConnectionManager (đã initialized)");
                return dbManager.ConnectionString;
            }

            // Thử tự động khởi tạo DatabaseConnectionManager nếu chưa được khởi tạo
            // Điều này cho phép lấy connection string ngay cả khi ApplicationStartupManager chưa được gọi InitializeApplication()
            _logger?.Info("ApplicationStartupManager chưa được initialized, thử tự động khởi tạo DatabaseConnectionManager");
            if (dbManager.Initialize())
            {
                _logger?.Info("Đã tự động khởi tạo DatabaseConnectionManager thành công");
                return dbManager.ConnectionString;
            }

            // Nếu không thể khởi tạo, trả về null
            _logger?.Warning("Không thể lấy connection string - DatabaseConnectionManager không thể được khởi tạo");
            return null;
        }
        catch (Exception ex)
        {
            // Nếu có lỗi, log và trả về null
            _logger?.Error($"Lỗi khi lấy connection string: {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Kiểm tra kết nối database toàn cục
    /// </summary>
    /// <returns>True nếu kết nối thành công</returns>
    public bool TestGlobalDatabaseConnection()
    {
        if (!IsApplicationReady())
        {
            return false;
        }

        return DatabaseConnectionManager.Instance.TestConnection();
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Khởi tạo các manager khác (nếu có)
    /// </summary>
    private void InitializeOtherManagers()
    {
        // TODO: Khởi tạo các manager khác ở đây
        // Ví dụ: UserManager, CacheManager, etc.
    }

    /// <summary>
    /// Khởi tạo ứng dụng tối ưu hóa (chỉ test connection 1 lần)
    /// </summary>
    /// <returns>True nếu khởi tạo thành công</returns>
    public bool InitializeApplicationOptimized()
    {
        if (_isInitialized)
        {
            return true;
        }

        lock (_initLock)
        {
            if (_isInitialized)
            {
                return true;
            }

            // Bước 1: Khởi tạo DatabaseConnectionManager (đã test connection)
            if (!DatabaseConnectionManager.Instance.Initialize())
            {
                return false;
            }

            // Bước 2: Khởi tạo các manager khác (nếu có)
            InitializeOtherManagers();

            _isInitialized = true;

            return true;
        }
    }

    #endregion
}

