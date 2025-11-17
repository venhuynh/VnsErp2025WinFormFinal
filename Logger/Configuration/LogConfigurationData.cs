using System;
using System.Xml.Serialization;
using Common;

namespace Logger.Configuration;

/// <summary>
/// Data class để serialize/deserialize cấu hình logging vào XML
/// </summary>
[XmlRoot("LogConfiguration")]
public class LogConfigurationData
{
    /// <summary>
    /// Log level tối thiểu
    /// </summary>
    [XmlElement]
    public LogLevel MinimumLevel { get; set; } = LogLevel.Info;

    /// <summary>
    /// Có enable console logging không
    /// </summary>
    [XmlElement]
    public bool EnableConsole { get; set; } = ApplicationConstants.LOG_DEFAULT_ENABLE_CONSOLE;

    /// <summary>
    /// Có enable file logging không
    /// </summary>
    [XmlElement]
    public bool EnableFile { get; set; } = ApplicationConstants.LOG_DEFAULT_ENABLE_FILE;

    /// <summary>
    /// Thư mục chứa file log
    /// </summary>
    [XmlElement]
    public string LogDirectory { get; set; } = ApplicationConstants.LOGS_FOLDER;

    /// <summary>
    /// Pattern tên file log
    /// </summary>
    [XmlElement]
    public string LogFilePattern { get; set; } = ApplicationConstants.LOG_FILE_PATTERN;

    /// <summary>
    /// Kích thước tối đa của file log (MB)
    /// </summary>
    [XmlElement]
    public int MaxFileSizeMB { get; set; } = ApplicationConstants.LOG_MAX_FILE_SIZE_MB;

    /// <summary>
    /// Số lượng file log tối đa giữ lại
    /// </summary>
    [XmlElement]
    public int MaxFiles { get; set; } = ApplicationConstants.LOG_MAX_FILES;

    /// <summary>
    /// Hiển thị timestamp trên console
    /// </summary>
    [XmlElement]
    public bool ShowTimestampOnConsole { get; set; } = ApplicationConstants.LOG_DEFAULT_SHOW_TIMESTAMP;

    /// <summary>
    /// Hiển thị category trên console
    /// </summary>
    [XmlElement]
    public bool ShowCategoryOnConsole { get; set; } = ApplicationConstants.LOG_DEFAULT_SHOW_CATEGORY;

    /// <summary>
    /// Thời gian cập nhật cuối cùng
    /// </summary>
    [XmlElement]
    public DateTime LastUpdated { get; set; } = DateTime.Now;

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public LogConfigurationData()
    {
    }
}