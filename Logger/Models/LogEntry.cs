using System;
using Logger.Configuration;

namespace Logger.Models;

/// <summary>
/// Model đại diện cho một log entry
/// </summary>
public class LogEntry
{
    /// <summary>
    /// Timestamp của log entry
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Mức độ logging
    /// </summary>
    public LogLevel Level { get; set; }

    /// <summary>
    /// Category của log (UI, BLL, DAL, etc.)
    /// </summary>
    public string Category { get; set; }

    /// <summary>
    /// Nội dung message
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// Exception nếu có
    /// </summary>
    public Exception Exception { get; set; }

    /// <summary>
    /// Thread ID
    /// </summary>
    public int ThreadId { get; set; }

    /// <summary>
    /// Constructor
    /// </summary>
    public LogEntry()
    {
        Timestamp = DateTime.Now;
        ThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
    }

    /// <summary>
    /// Constructor với parameters
    /// </summary>
    public LogEntry(LogLevel level, string category, string message, Exception exception = null)
        : this()
    {
        Level = level;
        Category = category;
        Message = message;
        Exception = exception;
    }

    /// <summary>
    /// Format log entry thành string
    /// </summary>
    public string ToFormattedString()
    {
        var timestamp = Timestamp.ToString("yyyy-MM-dd HH:mm:ss.fff");
        var level = Level.GetDisplayName();
        var category = string.IsNullOrEmpty(Category) ? "SYSTEM" : Category;
        var threadId = ThreadId;

        var result = $"[{timestamp}] [{level}] [{category}] [T:{threadId}] {Message}";

        if (Exception != null)
        {
            result += $"\nException: {Exception}";
        }

        return result;
    }

    /// <summary>
    /// Format log entry thành string ngắn gọn cho console
    /// </summary>
    public string ToConsoleString()
    {
        var timestamp = Timestamp.ToString("HH:mm:ss.fff");
        var level = Level.GetDisplayName();
        var category = string.IsNullOrEmpty(Category) ? "SYS" : Category;

        return $"[{timestamp}] [{level}] [{category}] {Message}";
    }
}