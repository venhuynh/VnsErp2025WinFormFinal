using Logger.Configuration;
using Logger.Implementations;
using Logger.Interfaces;

namespace Logger;

/// <summary>
/// Factory class để tạo logger instances
/// </summary>
public static class LoggerFactory
{
    #region Fields

    private static ILogger _defaultLogger;
    private static readonly object _lockObject = new object();

    #endregion

    #region Public Methods

    /// <summary>
    /// Tạo logger với category cụ thể
    /// </summary>
    public static ILogger CreateLogger(string category = LogCategory.System)
    {
        lock (_lockObject)
        {
            if (_defaultLogger == null)
            {
                InitializeDefaultLogger();
            }

            return _defaultLogger.CreateChildLogger(category);
        }
    }

    /// <summary>
    /// Tạo logger tùy chỉnh
    /// </summary>
    public static ILogger CreateLogger(ILogConfiguration config, string category = null)
    {
        var target = CreateLogTarget(config);
        return new global::Logger.Implementations.Logger(target, config, category);
    }

    /// <summary>
    /// Khởi tạo logger mặc định
    /// </summary>
    public static void InitializeDefaultLogger()
    {
        lock (_lockObject)
        {
            if (_defaultLogger != null)
                return;

            var config = new LogConfiguration();
            if (!config.Validate())
            {
                // Fallback to basic console logging if config is invalid
                config = (LogConfiguration)CreateDefaultConfig();
            }

            var target = CreateLogTarget(config);
            _defaultLogger = new global::Logger.Implementations.Logger(target, config, LogCategory.System);
        }
    }

    /// <summary>
    /// Tạo log target dựa trên cấu hình
    /// </summary>
    public static ILogTarget CreateLogTarget(ILogConfiguration config)
    {
        var compositeTarget = new CompositeLogTarget();

        if (config.EnableFile)
        {
            var fileTarget = new FileLogTarget(config);
            compositeTarget.AddTarget(fileTarget);
        }

        if (config.EnableConsole)
        {
            var consoleTarget = new ConsoleLogTarget(config);
            compositeTarget.AddTarget(consoleTarget);
        }

        // Luôn thêm Debug target khi chạy trong Visual Studio
        if (System.Diagnostics.Debugger.IsAttached)
        {
            var debugTarget = new DebugLogTarget(config);
            compositeTarget.AddTarget(debugTarget);
        }

        return compositeTarget;
    }

    /// <summary>
    /// Tạo cấu hình mặc định
    /// </summary>
    public static ILogConfiguration CreateDefaultConfig()
    {
        return new LogConfiguration
        {
            MinimumLevel = LogLevel.Info,
            EnableConsole = true,
            EnableFile = true,
            LogDirectory = "Logs",
            LogFilePattern = "VNTA-QuangVienPrinting_{date}.log",
            MaxFileSizeMB = 10,
            MaxFiles = 30,
            ShowTimestampOnConsole = true,
            ShowCategoryOnConsole = true
        };
    }

    /// <summary>
    /// Đóng tất cả loggers
    /// </summary>
    public static void Shutdown()
    {
        lock (_lockObject)
        {
            if (_defaultLogger is global::Logger.Implementations.Logger logger)
            {
                logger.Close();
            }
            _defaultLogger = null;
        }
    }

    #endregion
}