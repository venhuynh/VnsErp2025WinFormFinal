using System;
using Common.Appconfig;
using Dal.DataAccess.Implementations.MasterData.CompanyRepository;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;

namespace Bll.MasterData.CompanyBll;

/// <summary>
/// Business Logic Layer cho Company
/// </summary>
public class CompanyBll
{
    #region Fields

    private ICompanyRepository _dataAccess;
    private readonly ILogger _logger;
    private readonly object _lockObject = new object();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public CompanyBll()
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
    private ICompanyRepository GetDataAccess()
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

                        _dataAccess = new CompanyRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error("Lỗi khi khởi tạo CompanyRepository: {0}", ex, ex.Message);
                        throw new InvalidOperationException(
                            "Không thể khởi tạo kết nối database. Vui lòng kiểm tra cấu hình database.", ex);
                    }
                }
            }
        }

        return _dataAccess;
    }

    #endregion
    
    /// <summary>
    /// Đảm bảo chỉ có 1 công ty trong database
    /// </summary>
    public void EnsureSingleCompany()
    {
        try
        {
            _logger?.Info("Bắt đầu đảm bảo chỉ có 1 công ty trong database");
            GetDataAccess().EnsureDefaultCompany();
            _logger?.Info("Hoàn thành đảm bảo chỉ có 1 công ty trong database");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi đảm bảo chỉ có 1 công ty: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy thông tin công ty từ database
    /// </summary>
    /// <returns>Company entity</returns>
    public Dal.DataContext.Company GetCompany()
    {
        try
        {
            _logger?.Info("Bắt đầu lấy thông tin công ty từ database");
            var dataAccess = GetDataAccess();
            dataAccess.EnsureDefaultCompany();
            var company = dataAccess.GetCompany();
            _logger?.Info("Hoàn thành lấy thông tin công ty từ database");
            return company;
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi lấy thông tin công ty: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật thông tin công ty
    /// </summary>
    /// <param name="company">Company entity cần cập nhật</param>
    public void UpdateCompany(Dal.DataContext.Company company)
    {
        try
        {
            _logger?.Info("Bắt đầu cập nhật thông tin công ty");
            GetDataAccess().UpdateCompany(company);
            _logger?.Info("Hoàn thành cập nhật thông tin công ty");
        }
        catch (Exception ex)
        {
            _logger?.Error($"Lỗi khi cập nhật thông tin công ty: {ex.Message}", ex);
            throw;
        }
    }

}