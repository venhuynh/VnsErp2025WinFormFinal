using System;
using Logger.Configuration;

namespace Logger.Interfaces;

/// <summary>
/// Interface chính cho hệ thống logging
/// </summary>
public interface ILogger
{
    /// <summary>
    /// Ghi log với level Trace
    /// </summary>
    void Trace(string message, params object[] args);

    /// <summary>
    /// Ghi log với level Debug
    /// </summary>
    void Debug(string message, params object[] args);

    /// <summary>
    /// Ghi log với level Info
    /// </summary>
    void Info(string message, params object[] args);

    /// <summary>
    /// Ghi log với level Warning
    /// </summary>
    void Warning(string message, params object[] args);

    /// <summary>
    /// Ghi log với level Error
    /// </summary>
    void Error(string message, params object[] args);

    /// <summary>
    /// Ghi log với level Fatal
    /// </summary>
    void Fatal(string message, params object[] args);

    /// <summary>
    /// Ghi log với exception
    /// </summary>
    void Error(string message, Exception exception, params object[] args);

    /// <summary>
    /// Ghi log với exception
    /// </summary>
    void Fatal(string message, Exception exception, params object[] args);

    /// <summary>
    /// Ghi log với level và category tùy chỉnh
    /// </summary>
    void Log(LogLevel level, string category, string message, params object[] args);

    /// <summary>
    /// Ghi log với level, category và exception
    /// </summary>
    void Log(LogLevel level, string category, string message, Exception exception, params object[] args);

    /// <summary>
    /// Kiểm tra xem level có được enable không
    /// </summary>
    bool IsEnabled(LogLevel level);

    /// <summary>
    /// Tạo child logger với category cụ thể
    /// </summary>
    ILogger CreateChildLogger(string category);
}