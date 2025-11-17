using Dal.DataContext;

namespace Dal.DataAccess.Interfaces;

/// <summary>
/// Interface cho factory tạo DataContext
/// Giải quyết vấn đề tight coupling và tạo điều kiện cho dependency injection
/// </summary>
public interface IDataContextFactory
{
    /// <summary>
    /// Tạo VnsErp2025DataContext mới
    /// </summary>
    /// <returns>VnsErp2025DataContext instance</returns>
    VnsErp2025DataContext CreateContext();

    /// <summary>
    /// Tạo VnsErp2025DataContext với connection string tùy chỉnh
    /// </summary>
    /// <param name="connectionString">Connection string</param>
    /// <returns>VnsErp2025DataContext instance</returns>
    VnsErp2025DataContext CreateContext(string connectionString);

    /// <summary>
    /// Kiểm tra connection string có hợp lệ không
    /// </summary>
    /// <param name="connectionString">Connection string cần kiểm tra</param>
    /// <returns>True nếu hợp lệ</returns>
    bool IsValidConnectionString(string connectionString);

    /// <summary>
    /// Lấy connection string mặc định
    /// </summary>
    /// <returns>Connection string mặc định</returns>
    string GetDefaultConnectionString();

    /// <summary>
    /// Kiểm tra factory có sẵn sàng không
    /// </summary>
    /// <returns>True nếu sẵn sàng</returns>
    bool IsReady();
}