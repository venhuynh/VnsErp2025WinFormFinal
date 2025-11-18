using Dal.Configuration;
using Dal.DataAccess.Interfaces;
using System;
using Common.Appconfig;
using Dal.Connection;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Factories;

/// <summary>
/// Factory implementation cho việc tạo DataContext
/// Giải quyết vấn đề CreateContext() được gọi quá nhiều lần
/// </summary>
public class DataContextFactory : IDataContextFactory
{
    #region Fields & Properties

    private readonly CustomLogger _logger;
    private readonly DatabaseSettings _settings;
    private readonly string _defaultConnectionString;

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định - sử dụng connection string toàn cục
    /// </summary>
    public DataContextFactory()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _settings = new DatabaseSettings();

        try
        {
            // Ưu tiên sử dụng connection string toàn cục từ ApplicationStartupManager
            var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
            if (!string.IsNullOrEmpty(globalConnectionString))
            {
                _defaultConnectionString = globalConnectionString;
                _logger.Info("DataContextFactory được khởi tạo với connection string toàn cục");
            }
            else
            {
                // Fallback về Registry nếu không có global connection string
                var databaseHelper = DatabaseConnectionHelper.Instance;
                if (databaseHelper.HasDatabaseConfig())
                {
                    var databaseConfig = databaseHelper.LoadDatabaseConfig();
                    _defaultConnectionString = databaseConfig.GetConnectionString();
                    _logger.Info("DataContextFactory được khởi tạo với connection string từ Registry (fallback)");
                }
                else
                {
                    _defaultConnectionString = null;
                    _logger.Warning("DataContextFactory không có cấu hình database");
                }
            }
        }
        catch (Exception ex)
        {
            _defaultConnectionString = null;
            _logger.Error($"Lỗi khi lấy connection string cho DataContextFactory: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Constructor với connection string tùy chỉnh
    /// </summary>
    /// <param name="connectionString">Connection string</param>
    public DataContextFactory(string connectionString)
    {
        _defaultConnectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _settings = new DatabaseSettings();
        _logger.Info("DataContextFactory được khởi tạo với connection string tùy chỉnh");
    }

    #endregion

    #region IDataContextFactory Implementation

    /// <summary>
    /// Tạo VntaDataContext với connection string mặc định
    /// </summary>
    /// <returns>VntaDataContext instance</returns>
    public VnsErp2025DataContext CreateContext()
    {
        try
        {
            if (string.IsNullOrEmpty(_defaultConnectionString))
            {
                throw new InvalidOperationException("Default connection string is not available. Please ensure database configuration is properly set up.");
            }

            _logger.Debug("Tạo VntaDataContext với connection string mặc định");

            return new VnsErp2025DataContext(_defaultConnectionString)
            {
                CommandTimeout = _settings.CommandTimeout
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo VntaDataContext với connection string mặc định: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Tạo VntaDataContext với connection string tùy chỉnh
    /// </summary>
    /// <param name="connectionString">Connection string</param>
    /// <returns>VntaDataContext instance</returns>
    public VnsErp2025DataContext CreateContext(string connectionString)
    {
        try
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentException(@"Connection string cannot be null or empty", nameof(connectionString));
            }

            if (!IsValidConnectionString(connectionString))
            {
                throw new ArgumentException(@"Invalid connection string format", nameof(connectionString));
            }

            _logger.Debug("Tạo VntaDataContext với connection string tùy chỉnh");

            return new VnsErp2025DataContext(connectionString)
            {
                CommandTimeout = _settings.CommandTimeout
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo VntaDataContext với connection string tùy chỉnh: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Kiểm tra connection string có hợp lệ không
    /// </summary>
    /// <param name="connectionString">Connection string cần kiểm tra</param>
    /// <returns>True nếu hợp lệ</returns>
    public bool IsValidConnectionString(string connectionString)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                return false;
            }

            // Kiểm tra các thành phần cơ bản của connection string
            var requiredComponents = new[] { "Data Source", "Server", "Initial Catalog", "Database" };
            var hasRequiredComponent = false;

            foreach (var component in requiredComponents)
            {
                if (connectionString.IndexOf(component, StringComparison.OrdinalIgnoreCase) >= 0)
                {
                    hasRequiredComponent = true;
                    break;
                }
            }

            return hasRequiredComponent;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra connection string: {ex.Message}", ex);
            return false;
        }
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy connection string mặc định
    /// </summary>
    /// <returns>Connection string mặc định</returns>
    public string GetDefaultConnectionString()
    {
        return _defaultConnectionString;
    }

    /// <summary>
    /// Kiểm tra factory có sẵn sàng không
    /// </summary>
    /// <returns>True nếu sẵn sàng</returns>
    public bool IsReady()
    {
        return !string.IsNullOrEmpty(_defaultConnectionString);
    }

    #endregion
}