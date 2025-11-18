using Dal.Logging;
using System;
using Dal.DataAccess.Implementations.MasterData.Company;

namespace Bll.MasterData.Company
{
    /// <summary>
    /// Business Logic Layer cho Company
    /// </summary>
    public class CompanyBll
    {
        private readonly ICompanyRepository _companyDataAccess;
        private readonly ILogger _logger;

        public CompanyBll(ILogger logger = null)
        {
            _logger = logger ?? new NullLogger();
            _companyDataAccess = new CompanyDataAccess(_logger);
        }

        public CompanyBll(string connectionString, ILogger logger = null)
        {
            _logger = logger ?? new NullLogger();
            _companyDataAccess = new CompanyDataAccess(connectionString, _logger);
        }

        /// <summary>
        /// Đảm bảo chỉ có 1 công ty trong database
        /// </summary>
        public void EnsureSingleCompany()
        {
            try
            {
                _logger?.LogInfo("Bắt đầu đảm bảo chỉ có 1 công ty trong database");
                _companyDataAccess.EnsureDefaultCompany();
                _logger?.LogInfo("Hoàn thành đảm bảo chỉ có 1 công ty trong database");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi đảm bảo chỉ có 1 công ty: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Lấy thông tin công ty từ database
        /// </summary>
        /// <returns>Company entity</returns>
        public object GetCompany()
        {
            try
            {
                _logger?.LogInfo("Bắt đầu lấy thông tin công ty từ database");
                _companyDataAccess.EnsureDefaultCompany();
                var company = _companyDataAccess.GetCompany();
                _logger?.LogInfo("Hoàn thành lấy thông tin công ty từ database");
                return company;
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi lấy thông tin công ty: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Cập nhật thông tin công ty
        /// </summary>
        /// <param name="company">Company entity cần cập nhật</param>
        public void UpdateCompany(object company)
        {
            try
            {
                _logger?.LogInfo("Bắt đầu cập nhật thông tin công ty");
                _companyDataAccess.UpdateCompany(company);
                _logger?.LogInfo("Hoàn thành cập nhật thông tin công ty");
            }
            catch (Exception ex)
            {
                _logger?.LogError($"Lỗi khi cập nhật thông tin công ty: {ex.Message}", ex);
                throw;
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            _companyDataAccess?.Dispose();
        }
    }
}