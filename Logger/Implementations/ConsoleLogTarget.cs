using Logger.Configuration;
using Logger.Interfaces;
using Logger.Models;
using System;

namespace Logger.Implementations;

/// <summary>
/// Target logging hiển thị trên console
/// </summary>
public class ConsoleLogTarget : ILogTarget
{
    #region Fields

    private readonly ILogConfiguration _config;
    private readonly object _lockObject = new object();

    #endregion

    #region Properties

    public string Name => "ConsoleTarget";
    public bool IsEnabled { get; set; } = true;

    #endregion

    #region Constructor

    public ConsoleLogTarget(ILogConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Ghi log entry ra console
    /// </summary>
    public void Write(LogEntry entry)
    {
        if (!IsEnabled || !_config.EnableConsole)
            return;

        try
        {
            lock (_lockObject)
            {
                var originalColor = Console.ForegroundColor;
                    
                try
                {
                    // Set color based on log level
                    SetConsoleColor(entry.Level);
                        
                    // Format message for console
                    var message = FormatConsoleMessage(entry);
                        
                    // Write with proper encoding
                    Console.Out.WriteLine(message);
                        
                    // Write exception details if present
                    if (entry.Exception != null)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Out.WriteLine($"Exception: {entry.Exception}");
                    }
                }
                finally
                {
                    Console.ForegroundColor = originalColor;
                }
            }
        }
        catch (Exception ex)
        {
            // Fallback to basic console output
            System.Diagnostics.Debug.WriteLine($"Error writing to console: {ex.Message}");
            try
            {
                Console.WriteLine($"[{entry.Level.GetDisplayName()}] {entry.Message}");
            }
            catch
            {
                // Last resort - use Debug output
                System.Diagnostics.Debug.WriteLine($"[{entry.Level.GetDisplayName()}] {entry.Message}");
            }
        }
    }

    /// <summary>
    /// Flush console buffer
    /// </summary>
    public void Flush()
    {
        try
        {
            Console.Out.Flush();
        }
        catch
        {
            // Ignore flush errors
        }
    }

    /// <summary>
    /// Đóng target
    /// </summary>
    public void Close()
    {
        // Nothing to close for console target
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Set màu console dựa trên log level
    /// </summary>
    private void SetConsoleColor(LogLevel level)
    {
        Console.ForegroundColor = level switch
        {
            LogLevel.Trace => ConsoleColor.Gray,
            LogLevel.Debug => ConsoleColor.Cyan,
            LogLevel.Info => ConsoleColor.White,
            LogLevel.Warning => ConsoleColor.Yellow,
            LogLevel.Error => ConsoleColor.Red,
            LogLevel.Fatal => ConsoleColor.Magenta,
            _ => ConsoleColor.White
        };
    }

    /// <summary>
    /// Format message cho console
    /// </summary>
    private string FormatConsoleMessage(LogEntry entry)
    {
        if (_config.ShowTimestampOnConsole && _config.ShowCategoryOnConsole)
        {
            return entry.ToConsoleString();
        }
        else if (_config.ShowTimestampOnConsole)
        {
            var timestamp = entry.Timestamp.ToString("HH:mm:ss.fff");
            return $"[{timestamp}] [{entry.Level.GetDisplayName()}] {entry.Message}";
        }
        else if (_config.ShowCategoryOnConsole)
        {
            var category = string.IsNullOrEmpty(entry.Category) ? "SYS" : entry.Category;
            return $"[{entry.Level.GetDisplayName()}] [{category}] {entry.Message}";
        }
        else
        {
            return $"[{entry.Level.GetDisplayName()}] {entry.Message}";
        }
    }

    /// <summary>
    /// Cấu hình console encoding để hiển thị tiếng Việt đúng
    /// </summary>
    private void ConfigureConsoleEncoding()
    {
        try
        {
            // Cấu hình console encoding để hiển thị tiếng Việt
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.InputEncoding = System.Text.Encoding.UTF8;
        }
        catch
        {
            // Ignore encoding errors
        }
    }

    #endregion
}