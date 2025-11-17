using Common;
using Logger.Interfaces;
using System;
using System.IO;

namespace Logger.Configuration;

/// <summary>
/// Class cấu hình logging
/// </summary>
public class LogConfiguration : ILogConfiguration, ILogConfiguration
{
    #region Properties

    public LogLevel MinimumLevel { get; set; } = LogLevel.Info;
    public bool EnableConsole { get; set; } = ApplicationConstants.LOG_DEFAULT_ENABLE_CONSOLE;
    public bool EnableFile { get; set; } = ApplicationConstants.LOG_DEFAULT_ENABLE_FILE;
    private string _logDirectory = ApplicationConstants.LOGS_FOLDER;
    public string LogDirectory 
    { 
        get => _logDirectory; 
        set 
        { 
            _logDirectory = value;
            // Cập nhật XML config khi LogDirectory thay đổi
            UpdateLogDirectoryToXml();
        } 
    }
    public string LogFilePattern { get; set; } = ApplicationConstants.LOG_FILE_PATTERN;
    public int MaxFileSizeMB { get; set; } = ApplicationConstants.LOG_MAX_FILE_SIZE_MB;
    public int MaxFiles { get; set; } = ApplicationConstants.LOG_MAX_FILES;
    public bool ShowTimestampOnConsole { get; set; } = ApplicationConstants.LOG_DEFAULT_SHOW_TIMESTAMP;
    public bool ShowCategoryOnConsole { get; set; } = ApplicationConstants.LOG_DEFAULT_SHOW_CATEGORY;

    #endregion

    #region Constructor

    public LogConfiguration()
    {
        LoadFromConfig();
        UpdateLogDirectoryToXml();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Load cấu hình từ App.config và XML
    /// </summary>
    public void LoadFromConfig()
    {
        try
        {
            // Ưu tiên load từ XML trước
            LoadConfigFromXml();

            // Sau đó load từ appSettings (sẽ override XML nếu có)
            var minimumLevelStr = ConfigurationManager.AppSettings["Logging.MinimumLevel"];
            if (!string.IsNullOrEmpty(minimumLevelStr) && Enum.TryParse<LogLevel>(minimumLevelStr, true, out var level))
            {
                MinimumLevel = level;
            }

            var enableConsoleStr = ConfigurationManager.AppSettings["Logging.EnableConsole"];
            if (!string.IsNullOrEmpty(enableConsoleStr) && bool.TryParse(enableConsoleStr, out var enableConsole))
            {
                EnableConsole = enableConsole;
            }

            var enableFileStr = ConfigurationManager.AppSettings["Logging.EnableFile"];
            if (!string.IsNullOrEmpty(enableFileStr) && bool.TryParse(enableFileStr, out var enableFile))
            {
                EnableFile = enableFile;
            }

            var logDirectory = ConfigurationManager.AppSettings["Logging.LogDirectory"];
            if (!string.IsNullOrEmpty(logDirectory))
            {
                LogDirectory = logDirectory;
            }

            var logFilePattern = ConfigurationManager.AppSettings["Logging.LogFilePattern"];
            if (!string.IsNullOrEmpty(logFilePattern))
            {
                LogFilePattern = logFilePattern;
            }

            var maxFileSizeStr = ConfigurationManager.AppSettings["Logging.MaxFileSizeMB"];
            if (!string.IsNullOrEmpty(maxFileSizeStr) && int.TryParse(maxFileSizeStr, out var maxFileSize))
            {
                MaxFileSizeMB = maxFileSize;
            }

            var maxFilesStr = ConfigurationManager.AppSettings["Logging.MaxFiles"];
            if (!string.IsNullOrEmpty(maxFilesStr) && int.TryParse(maxFilesStr, out var maxFiles))
            {
                MaxFiles = maxFiles;
            }

            var showTimestampStr = ConfigurationManager.AppSettings["Logging.ShowTimestampOnConsole"];
            if (!string.IsNullOrEmpty(showTimestampStr) && bool.TryParse(showTimestampStr, out var showTimestamp))
            {
                ShowTimestampOnConsole = showTimestamp;
            }

            var showCategoryStr = ConfigurationManager.AppSettings["Logging.ShowCategoryOnConsole"];
            if (!string.IsNullOrEmpty(showCategoryStr) && bool.TryParse(showCategoryStr, out var showCategory))
            {
                ShowCategoryOnConsole = showCategory;
            }
        }
        catch (Exception ex)
        {
            // Fallback to default values if config loading fails
            System.Diagnostics.Debug.WriteLine($"Error loading logging configuration: {ex.Message}");
        }
    }

    /// <summary>
    /// Validate cấu hình
    /// </summary>
    public bool Validate()
    {
        try
        {
            // Validate log directory
            if (EnableFile)
            {
                if (string.IsNullOrEmpty(LogDirectory))
                    return false;

                // Try to create directory if it doesn't exist
                if (!Directory.Exists(LogDirectory))
                {
                    Directory.CreateDirectory(LogDirectory);
                }
            }

            // Validate numeric values
            if (MaxFileSizeMB <= 0 || MaxFiles <= 0)
                return false;

            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Lấy đường dẫn file log hiện tại
    /// </summary>
    public string GetCurrentLogFilePath()
    {
        if (!EnableFile)
            return null;

        var fileName = LogFilePattern.Replace("{date}", DateTime.Now.ToString("yyyy-MM-dd"));
        return Path.Combine(LogDirectory, fileName);
    }

    /// <summary>
    /// Lấy đường dẫn file log theo ngày
    /// </summary>
    public string GetLogFilePath(DateTime date)
    {
        if (!EnableFile)
            return null;

        var fileName = LogFilePattern.Replace("{date}", date.ToString("yyyy-MM-dd"));
        return Path.Combine(LogDirectory, fileName);
    }

    /// <summary>
    /// Cập nhật đường dẫn log directory vào XML config
    /// </summary>
    private void UpdateLogDirectoryToXml()
    {
        try
        {
            // Đảm bảo đường dẫn log directory là absolute path
            string absoluteLogDirectory = GetAbsoluteLogDirectory();
                
            // Lưu vào XML
            SaveConfigToXml();
                
            System.Diagnostics.Debug.WriteLine($"Log directory updated to XML: {absoluteLogDirectory}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error updating log directory to XML: {ex.Message}");
        }
    }

    /// <summary>
    /// Lấy đường dẫn log directory tuyệt đối
    /// </summary>
    /// <returns>Đường dẫn tuyệt đối của log directory</returns>
    private string GetAbsoluteLogDirectory()
    {
        try
        {
            // Nếu LogDirectory đã là absolute path, trả về
            if (Path.IsPathRooted(LogDirectory))
            {
                return Path.GetFullPath(LogDirectory);
            }
                
            // Nếu là relative path, kết hợp với application directory
            string applicationDirectory = AppDomain.CurrentDomain.BaseDirectory;
            return Path.GetFullPath(Path.Combine(applicationDirectory, LogDirectory));
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error getting absolute log directory: {ex.Message}");
            // Fallback to default
            return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
        }
    }

    /// <summary>
    /// Đường dẫn file XML config
    /// </summary>
    private string ConfigFilePath => ApplicationConstants.LogConfigFilePath;

    /// <summary>
    /// Lưu cấu hình hiện tại vào file XML
    /// </summary>
    private void SaveConfigToXml()
    {
        try
        {
            var configData = new LogConfigurationData
            {
                LogDirectory = LogDirectory,
                MinimumLevel = MinimumLevel,
                EnableConsole = EnableConsole,
                EnableFile = EnableFile,
                LogFilePattern = LogFilePattern,
                MaxFileSizeMB = MaxFileSizeMB,
                MaxFiles = MaxFiles,
                ShowTimestampOnConsole = ShowTimestampOnConsole,
                ShowCategoryOnConsole = ShowCategoryOnConsole,
                LastUpdated = DateTime.Now
            };

            XmlHelper.SaveToXml(ConfigFilePath, configData);
            System.Diagnostics.Debug.WriteLine($"Log configuration saved to XML: {ConfigFilePath}");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error saving log configuration to XML: {ex.Message}");
        }
    }

    /// <summary>
    /// Load cấu hình từ file XML (nếu tồn tại)
    /// </summary>
    private void LoadConfigFromXml()
    {
        try
        {
            if (!File.Exists(ConfigFilePath))
            {
                System.Diagnostics.Debug.WriteLine($"XML config file not found: {ConfigFilePath}");
                return;
            }

            var configData = XmlHelper.LoadFromXmlToObject<LogConfigurationData>(ConfigFilePath);
            if (configData != null)
            {
                LogDirectory = configData.LogDirectory ?? _logDirectory;
                MinimumLevel = configData.MinimumLevel;
                EnableConsole = configData.EnableConsole;
                EnableFile = configData.EnableFile;
                LogFilePattern = configData.LogFilePattern ?? LogFilePattern;
                MaxFileSizeMB = configData.MaxFileSizeMB;
                MaxFiles = configData.MaxFiles;
                ShowTimestampOnConsole = configData.ShowTimestampOnConsole;
                ShowCategoryOnConsole = configData.ShowCategoryOnConsole;
                
                System.Diagnostics.Debug.WriteLine($"Log configuration loaded from XML: {ConfigFilePath}");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading log configuration from XML: {ex.Message}");
        }
    }

    #endregion
}