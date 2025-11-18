using Logger.Configuration;
using Logger.Interfaces;
using Logger.Models;
using System;

namespace Logger.Implementations;

/// <summary>
/// Implementation chính của ILogger
/// </summary>
public class Logger : ILogger
{
    #region Fields

    private readonly ILogTarget _target;
    private readonly ILogConfiguration _config;
    private readonly string _category;

    #endregion

    #region Constructor

    public Logger(ILogTarget target, ILogConfiguration config, string category = null)
    {
        _target = target ?? throw new ArgumentNullException(nameof(target));
        _config = config ?? throw new ArgumentNullException(nameof(config));
        _category = category ?? LogCategory.System;
    }

    #endregion

    #region ILogger Implementation

    public void Trace(string message, params object[] args)
    {
        Log(LogLevel.Trace, _category, FormatMessage(message, args));
    }

    public void Debug(string message, params object[] args)
    {
        Log(LogLevel.Debug, _category, FormatMessage(message, args));
    }

    public void Info(string message, params object[] args)
    {
        Log(LogLevel.Info, _category, FormatMessage(message, args));
    }

    public void Warning(string message, params object[] args)
    {
        Log(LogLevel.Warning, _category, FormatMessage(message, args));
    }

    public void Error(string message, params object[] args)
    {
        Log(LogLevel.Error, _category, FormatMessage(message, args));
    }

    public void Fatal(string message, params object[] args)
    {
        Log(LogLevel.Fatal, _category, FormatMessage(message, args));
    }

    public void Error(string message, Exception exception, params object[] args)
    {
        Log(LogLevel.Error, _category, FormatMessage(message, args), exception);
    }

    public void Fatal(string message, Exception exception, params object[] args)
    {
        Log(LogLevel.Fatal, _category, FormatMessage(message, args), exception);
    }

    public void Log(LogLevel level, string category, string message, params object[] args)
    {
        var formattedMessage = FormatMessage(message, args);
        var entry = new LogEntry(level, category, formattedMessage, null);
        _target.Write(entry);
    }

    public void Log(LogLevel level, string category, string message, Exception exception, params object[] args)
    {
        if (!IsEnabled(level))
            return;

        var entry = new LogEntry(level, category, FormatMessage(message, args), exception);
        _target.Write(entry);
    }

    public bool IsEnabled(LogLevel level)
    {
        return level.IsEnabled(_config.MinimumLevel);
    }

    public ILogger CreateChildLogger(string category)
    {
        return new Logger(_target, _config, category);
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Format message với parameters
    /// </summary>
    private string FormatMessage(string message, params object[] args)
    {
        if (args == null || args.Length == 0)
            return message;

        try
        {
            return string.Format(message, args);
        }
        catch (FormatException)
        {
            // Fallback to original message if formatting fails
            return message;
        }
    }

    /// <summary>
    /// Đóng logger
    /// </summary>
    public void Close()
    {
        _target?.Close();
    }

    #endregion
}