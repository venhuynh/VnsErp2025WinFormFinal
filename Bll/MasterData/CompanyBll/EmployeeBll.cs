using Dal.Connection;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using Dal.DataAccess.Implementations.MasterData.CompanyRepository;

namespace Bll.MasterData.CompanyBll;

internal class EmployeeBll
{
    #region Fields

    private IEmployeeRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public EmployeeBll()
    {
        // Khởi tạo logger trước để đảm bảo không null trong khối catch
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        // Repository sẽ được khởi tạo lazy khi cần sử dụng
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IEmployeeRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    try
                    {
                        // Sử dụng global connection string từ ApplicationStartupManager
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataAccess = new EmployeeRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Lỗi khi khởi tạo EmployeeRepository: {0}", ex, ex.Message);
                        throw new InvalidOperationException(
                            "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                    }
                }
            }
        }

        return _dataAccess;
    }

    #endregion
}