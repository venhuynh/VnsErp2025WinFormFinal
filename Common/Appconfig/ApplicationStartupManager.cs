namespace Common.Appconfig;

/// <summary>
/// Manager quản lý quá trình khởi động ứng dụng.
/// Đảm bảo các thành phần quan trọng được khởi tạo đúng thứ tự.
/// </summary>
public sealed class ApplicationStartupManager
{
    #region Singleton Pattern

    private static volatile ApplicationStartupManager _instance;
    private static readonly object Lock = new object();

    private ApplicationStartupManager()
    {
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
            return true;
        }

        lock (_initLock)
        {
            if (_isInitialized)
            {
                return true;
            }

            // Bước 1: Khởi tạo DatabaseConnectionManager
            if (!DatabaseConnectionManager.Instance.Initialize())
            {
                return false;
            }

            // Bước 2: Kiểm tra kết nối database
            if (!DatabaseConnectionManager.Instance.TestConnection())
            {
                return false;
            }

            // Bước 3: Khởi tạo các manager khác (nếu có)
            InitializeOtherManagers();

            _isInitialized = true;

            return true;
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
            return true;
        }

        lock (_initLock)
        {
            if (_isInitialized)
            {
                return true;
            }

            // Bước 1: Khởi tạo DatabaseConnectionManager với config tùy chỉnh
            if (!DatabaseConnectionManager.Instance.Initialize(customDbConfig))
            {
                return false;
            }

            // Bước 2: Kiểm tra kết nối database
            if (!DatabaseConnectionManager.Instance.TestConnection())
            {
                return false;
            }

            // Bước 3: Khởi tạo các manager khác (nếu có)
            InitializeOtherManagers();

            _isInitialized = true;

            return true;
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
    /// </summary>
    /// <returns>Connection string hoặc null nếu chưa khởi tạo</returns>
    public string GetGlobalConnectionString()
    {
        if (!IsApplicationReady())
        {
            return null;
        }

        return GlobalConnectionString;
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