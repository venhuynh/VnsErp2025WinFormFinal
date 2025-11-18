using Logger.Configuration;
using Logger.Models;
using System;
using System.Diagnostics;
using Logger.Interfaces;

namespace Logger.Implementations;

/// <summary>
/// Target logging ghi ra Visual Studio Debug Output
/// </summary>
public class DebugLogTarget : ILogTarget
{
    #region Fields

    private readonly ILogConfiguration _config;
    private readonly object _lockObject = new object();

    #endregion

    #region Properties

    public string Name => "DebugTarget";
    public bool IsEnabled { get; set; } = true;

    #endregion

    #region Constructor

    public DebugLogTarget(ILogConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Ghi log entry ra Debug Output
    /// </summary>
    public void Write(LogEntry entry)
    {
        if (!IsEnabled)
            return;

        try
        {
            lock (_lockObject)
            {
                // Format message cho Debug Output (không có màu sắc)
                var message = FormatDebugMessage(entry);
                    
                // Ghi ra Debug Output
                Debug.WriteLine(message);
                    
                // Ghi exception details nếu có
                if (entry.Exception != null)
                {
                    Debug.WriteLine($"Exception: {entry.Exception}");
                }
            }
        }
        catch (Exception)
        {
            // Fallback nếu Debug output bị lỗi
            try
            {
                Debug.WriteLine($"[{entry.Level.GetDisplayName()}] {entry.Message}");
            }
            catch
            {
                // Last resort - không làm gì cả
            }
        }
    }

    /// <summary>
    /// Flush buffer (không cần thiết cho Debug output)
    /// </summary>
    public void Flush()
    {
        // Debug output tự động flush
    }

    /// <summary>
    /// Đóng target
    /// </summary>
    public void Close()
    {
        // Nothing to close for debug target
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Format message cho Debug Output
    /// </summary>
    private string FormatDebugMessage(LogEntry entry)
    {
        var timestamp = entry.Timestamp.ToString("HH:mm:ss.fff");
        var level = entry.Level.GetDisplayName();
        var category = string.IsNullOrEmpty(entry.Category) ? "SYS" : entry.Category;
        var threadId = entry.ThreadId;

        return $"[{timestamp}] [{level}] [{category}] [T:{threadId}] {entry.Message}";
    }

    #endregion
}