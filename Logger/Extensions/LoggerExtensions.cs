using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Diagnostics;
using LogLevel = Logger.Configuration.LogLevel;

namespace Logger.Extensions;

/// <summary>
/// Extension methods cho ILogger
/// </summary>
public static class LoggerExtensions
{
    /// <summary>
    /// Log performance với execution time
    /// </summary>
    public static void LogPerformance(this ILogger logger, string operation, Action action)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            logger.Debug($"Bắt đầu thực hiện: {operation}");
            action();
            stopwatch.Stop();
            logger.Info($"Hoàn thành {operation} trong {stopwatch.ElapsedMilliseconds}ms");
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.Error($"Lỗi khi thực hiện {operation} sau {stopwatch.ElapsedMilliseconds}ms", ex);
            throw;
        }
    }

    /// <summary>
    /// Log performance với execution time và return value
    /// </summary>
    public static T LogPerformance<T>(this ILogger logger, string operation, Func<T> func)
    {
        var stopwatch = Stopwatch.StartNew();
        try
        {
            logger.Debug($"Bắt đầu thực hiện: {operation}");
            var result = func();
            stopwatch.Stop();
            logger.Info($"Hoàn thành {operation} trong {stopwatch.ElapsedMilliseconds}ms");
            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            logger.Error($"Lỗi khi thực hiện {operation} sau {stopwatch.ElapsedMilliseconds}ms", ex);
            throw;
        }
    }

    /// <summary>
    /// Log method entry
    /// </summary>
    public static void LogMethodEntry(this ILogger logger, string methodName, params object[] parameters)
    {
        var paramStr = parameters?.Length > 0 ? string.Join(", ", parameters) : "không có tham số";
        logger.Trace($"Vào method: {methodName}({paramStr})");
    }

    /// <summary>
    /// Log method exit
    /// </summary>
    public static void LogMethodExit(this ILogger logger, string methodName, object returnValue = null)
    {
        var returnStr = returnValue != null ? $" = {returnValue}" : "";
        logger.Trace($"Ra khỏi method: {methodName}{returnStr}");
    }

    /// <summary>
    /// Log database operation
    /// </summary>
    public static void LogDatabaseOperation(this ILogger logger, string operation, string query = null)
    {
        if (!string.IsNullOrEmpty(query))
        {
            logger.Debug($"Database {operation}: {query}");
        }
        else
        {
            logger.Debug($"Database {operation}");
        }
    }

    /// <summary>
    /// Log security event
    /// </summary>
    public static void LogSecurityEvent(this ILogger logger, string eventType, string details = null)
    {
        var message = $"Security Event: {eventType}";
        if (!string.IsNullOrEmpty(details))
        {
            message += $" - {details}";
        }
        logger.Warning(message);
    }

    /// <summary>
    /// Log audit trail
    /// </summary>
    public static void LogAudit(this ILogger logger, string action, string entity, string entityId = null, string userId = null)
    {
        var message = $"Audit: {action} {entity}";
        if (!string.IsNullOrEmpty(entityId))
        {
            message += $" (ID: {entityId})";
        }
        if (!string.IsNullOrEmpty(userId))
        {
            message += $" by User: {userId}";
        }
        logger.Info(message);
    }

    /// <summary>
    /// Log configuration change
    /// </summary>
    public static void LogConfigChange(this ILogger logger, string configKey, string oldValue, string newValue)
    {
        logger.Info($"Configuration changed: {configKey} from '{oldValue}' to '{newValue}'");
    }

    /// <summary>
    /// Log với structured data
    /// </summary>
    public static void LogStructured(this ILogger logger, LogLevel level, string message, object data)
    {
        var dataStr = data != null ? data.ToString() : "null";
        logger.Log(level, LogCategory.System, $"{message} | Data: {dataStr}");
    }

    
}