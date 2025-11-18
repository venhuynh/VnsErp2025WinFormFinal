using System;
using Logger.Configuration;

namespace Logger.Interfaces;

/// <summary>
/// Interface cho cấu hình logging
/// </summary>
public interface ILogConfiguration
{
    /// <summary>
    /// Log level tối thiểu
    /// </summary>
    LogLevel MinimumLevel { get; set; }

    /// <summary>
    /// Có enable console logging không
    /// </summary>
    bool EnableConsole { get; set; }

    /// <summary>
    /// Có enable file logging không
    /// </summary>
    bool EnableFile { get; set; }

    /// <summary>
    /// Đường dẫn thư mục log
    /// </summary>
    string LogDirectory { get; set; }

    /// <summary>
    /// Pattern tên file log
    /// </summary>
    string LogFilePattern { get; set; }

    /// <summary>
    /// Kích thước tối đa của file log (MB)
    /// </summary>
    int MaxFileSizeMB { get; set; }

    /// <summary>
    /// Số lượng file log tối đa
    /// </summary>
    int MaxFiles { get; set; }

    /// <summary>
    /// Có hiển thị timestamp trên console không
    /// </summary>
    bool ShowTimestampOnConsole { get; set; }

    /// <summary>
    /// Có hiển thị category trên console không
    /// </summary>
    bool ShowCategoryOnConsole { get; set; }

    /// <summary>
    /// Load cấu hình từ App.config
    /// </summary>
    void LoadFromConfig();

    /// <summary>
    /// Validate cấu hình
    /// </summary>
    bool Validate();

    /// <summary>
    /// Lấy đường dẫn file log hiện tại
    /// </summary>
    string GetCurrentLogFilePath();

    /// <summary>
    /// Lấy đường dẫn file log theo ngày
    /// </summary>
    string GetLogFilePath(DateTime date);
}