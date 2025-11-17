namespace Logger.Configuration;

/// <summary>
/// Enum định nghĩa các mức độ logging
/// </summary>
public enum LogLevel
{
    /// <summary>
    /// Chi tiết nhất - dành cho debugging
    /// </summary>
    Trace = 0,

    /// <summary>
    /// Thông tin debug
    /// </summary>
    Debug = 1,

    /// <summary>
    /// Thông tin chung
    /// </summary>
    Info = 2,

    /// <summary>
    /// Cảnh báo
    /// </summary>
    Warning = 3,

    /// <summary>
    /// Lỗi
    /// </summary>
    Error = 4,

    /// <summary>
    /// Lỗi nghiêm trọng
    /// </summary>
    Fatal = 5
}

/// <summary>
/// Extension methods cho LogLevel
/// </summary>
public static class LogLevelExtensions
{
    /// <summary>
    /// Lấy tên hiển thị của LogLevel
    /// </summary>
    public static string GetDisplayName(this LogLevel level)
    {
        return level switch
        {
            LogLevel.Trace => "TRACE",
            LogLevel.Debug => "DEBUG",
            LogLevel.Info => "INFO",
            LogLevel.Warning => "WARNING",
            LogLevel.Error => "ERROR",
            LogLevel.Fatal => "FATAL",
            _ => level.ToString().ToUpper()
        };
    }

    /// <summary>
    /// Kiểm tra xem level có >= threshold không
    /// </summary>
    public static bool IsEnabled(this LogLevel level, LogLevel threshold)
    {
        return level >= threshold;
    }
}