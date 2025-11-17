using Logger.Models;

namespace Logger.Interfaces;

/// <summary>
/// Interface cho các target logging (File, Console, etc.)
/// </summary>
public interface ILogTarget
{
    /// <summary>
    /// Tên của target
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Ghi log entry
    /// </summary>
    void Write(LogEntry entry);

    /// <summary>
    /// Flush buffer nếu có
    /// </summary>
    void Flush();

    /// <summary>
    /// Đóng target
    /// </summary>
    void Close();

    /// <summary>
    /// Kiểm tra xem target có được enable không
    /// </summary>
    bool IsEnabled { get; set; }
}