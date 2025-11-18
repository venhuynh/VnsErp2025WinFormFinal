using Logger.Interfaces;
using Logger.Models;
using System;
using System.IO;

namespace Logger.Implementations;

/// <summary>
/// Target logging ghi ra file
/// </summary>
public class FileLogTarget : ILogTarget
{
    #region Fields

    private readonly ILogConfiguration _config;
    private readonly object _lockObject = new object();
    private string _currentLogFile;
    private long _currentFileSize;

    #endregion

    #region Properties

    public string Name => "FileTarget";
    public bool IsEnabled { get; set; } = true;

    #endregion

    #region Constructor

    public FileLogTarget(ILogConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
        InitializeLogFile();
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Ghi log entry ra file
    /// </summary>
    public void Write(LogEntry entry)
    {
        if (!IsEnabled || !_config.EnableFile)
            return;

        try
        {
            lock (_lockObject)
            {
                CheckAndRotateLogFile();
                    
                var logLine = entry.ToFormattedString();
                File.AppendAllText(_currentLogFile, logLine + Environment.NewLine);
                    
                _currentFileSize += logLine.Length + Environment.NewLine.Length;
            }
        }
        catch (Exception ex)
        {
            // Fallback to console if file writing fails
            System.Diagnostics.Debug.WriteLine($"Error writing to log file: {ex.Message}");
            Console.WriteLine($"[ERROR] Failed to write to log file: {ex.Message}");
        }
    }

    /// <summary>
    /// Flush buffer (không cần thiết cho File.AppendAllText)
    /// </summary>
    public void Flush()
    {
        // File.AppendAllText automatically flushes
    }

    /// <summary>
    /// Đóng target
    /// </summary>
    public void Close()
    {
        // Nothing to close for file target
    }

    #endregion

    #region Private Methods

    /// <summary>
    /// Khởi tạo file log hiện tại
    /// </summary>
    private void InitializeLogFile()
    {
        try
        {
            _currentLogFile = _config.GetCurrentLogFilePath();
            if (File.Exists(_currentLogFile))
            {
                var fileInfo = new FileInfo(_currentLogFile);
                _currentFileSize = fileInfo.Length;
            }
            else
            {
                _currentFileSize = 0;
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error initializing log file: {ex.Message}");
            _currentLogFile = null;
            _currentFileSize = 0;
        }
    }

    /// <summary>
    /// Kiểm tra và rotate log file nếu cần
    /// </summary>
    private void CheckAndRotateLogFile()
    {
        if (string.IsNullOrEmpty(_currentLogFile))
        {
            InitializeLogFile();
            return;
        }

        var maxSizeBytes = _config.MaxFileSizeMB * 1024 * 1024;
            
        if (_currentFileSize >= maxSizeBytes)
        {
            RotateLogFile();
        }
    }

    /// <summary>
    /// Rotate log file
    /// </summary>
    private void RotateLogFile()
    {
        try
        {
            // Tạo tên file mới với timestamp
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var directory = Path.GetDirectoryName(_currentLogFile);
            var fileName = Path.GetFileNameWithoutExtension(_currentLogFile);
            var extension = Path.GetExtension(_currentLogFile);
                
            var rotatedFileName = $"{fileName}_{timestamp}{extension}";
            var rotatedFilePath = Path.Combine(directory, rotatedFileName);
                
            // Rename current file
            if (File.Exists(_currentLogFile))
            {
                File.Move(_currentLogFile, rotatedFilePath);
            }
                
            // Cleanup old files
            CleanupOldLogFiles();
                
            // Reset current file
            _currentFileSize = 0;
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error rotating log file: {ex.Message}");
        }
    }

    /// <summary>
    /// Dọn dẹp các file log cũ
    /// </summary>
    private void CleanupOldLogFiles()
    {
        try
        {
            var directory = Path.GetDirectoryName(_currentLogFile);
            if (!Directory.Exists(directory))
                return;

            var logFiles = Directory.GetFiles(directory, "VNTA-QuangVienPrinting_*.log");
                
            if (logFiles.Length > _config.MaxFiles)
            {
                // Sort by creation time (oldest first)
                Array.Sort(logFiles, (x, y) => File.GetCreationTime(x).CompareTo(File.GetCreationTime(y)));
                    
                // Delete oldest files
                var filesToDelete = logFiles.Length - _config.MaxFiles;
                for (int i = 0; i < filesToDelete; i++)
                {
                    try
                    {
                        File.Delete(logFiles[i]);
                    }
                    catch
                    {
                        // Ignore deletion errors
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error cleaning up old log files: {ex.Message}");
        }
    }

    #endregion
}