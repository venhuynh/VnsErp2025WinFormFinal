using System;
using System.Data.Linq;
using System.Linq;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.CompanyRepository;

/// <summary>
/// Data Access cho thực thể Company (LINQ to SQL trên DataContext).
/// Cung cấp các truy vấn/biến đổi phổ biến: lấy thông tin công ty, đảm bảo có công ty mặc định, cập nhật thông tin.
/// </summary>
public class CompanyRepository : ICompanyRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class CompanyRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public CompanyRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("CompanyRepository được khởi tạo với connection string");
        
        // Đảm bảo có công ty mặc định khi khởi tạo
        EnsureDefaultCompany();
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Đảm bảo có thông tin công ty mặc định
    /// </summary>
    public void EnsureDefaultCompany()
    {
        try
        {
            using var context = CreateNewContext();
            
            // Kiểm tra xem đã có công ty nào chưa
            var existingCompany = context.Companies.FirstOrDefault();
            
            if (existingCompany == null)
            {
                _logger.Info("Không tìm thấy công ty nào, tạo công ty mặc định");
                
                // Tạo công ty mặc định
                var defaultCompany = new DataContext.Company
                {
                    Id = Guid.NewGuid(),
                    CompanyCode = "VNS001",
                    CompanyName = "CÔNG TY TNHH VIỆT NHẬT SOLUTIONS",
                    TaxCode = "3702887941",
                    Address = "2/3 Khu phố Bình Phước A, Phường An Phú, Thành phố Hồ Chí Minh, Việt Nam",
                    Phone = "",
                    Email = "",
                    Website = "",
                    Country = "Việt Nam",
                    CreatedDate = DateTime.Now,
                    UpdatedDate = null,
                    Logo = null
                };

                context.Companies.InsertOnSubmit(defaultCompany);
                context.SubmitChanges();
                
                _logger.Info($"Đã tạo công ty mặc định: {defaultCompany.CompanyCode} - {defaultCompany.CompanyName}");
            }
        }
        catch (System.Data.SqlClient.SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi đảm bảo công ty mặc định: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi đảm bảo công ty mặc định: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi đảm bảo công ty mặc định: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi đảm bảo công ty mặc định: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy thông tin công ty từ database
    /// </summary>
    /// <returns>Company entity</returns>
    public Company GetCompany()
    {
        try
        {
            using var context = CreateNewContext();
            var company = context.Companies.FirstOrDefault();
            
            if (company != null)
            {
                _logger.Debug($"Đã lấy thông tin công ty: {company.CompanyCode} - {company.CompanyName}");
            }
            else
            {
                _logger.Warning("Không tìm thấy công ty nào trong database");
            }
            
            return company;
        }
        catch (System.Data.SqlClient.SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy thông tin công ty: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy thông tin công ty: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy thông tin công ty: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy thông tin công ty: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Cập nhật thông tin công ty
    /// </summary>
    /// <param name="company">Company entity cần cập nhật</param>
    public void UpdateCompany(Company company)
    {
        try
        {
            if (company == null)
                throw new ArgumentNullException(nameof(company));

            using var context = CreateNewContext();
            
            // Tìm công ty hiện tại
            var existingCompany = context.Companies.FirstOrDefault();
            
            if (existingCompany != null)
            {
                // Cập nhật thông tin
                existingCompany.CompanyCode = company.CompanyCode;
                existingCompany.CompanyName = company.CompanyName;
                existingCompany.TaxCode = company.TaxCode;
                existingCompany.Phone = company.Phone;
                existingCompany.Email = company.Email;
                existingCompany.Website = company.Website;
                existingCompany.Address = company.Address;
                existingCompany.Country = company.Country;
                existingCompany.Logo = company.Logo;
                existingCompany.UpdatedDate = DateTime.Now;
                
                context.SubmitChanges();
                
                _logger.Info($"Đã cập nhật thông tin công ty: {existingCompany.CompanyCode} - {existingCompany.CompanyName}");
            }
            else
            {
                throw new DataAccessException("Không tìm thấy công ty để cập nhật");
            }
        }
        catch (System.Data.SqlClient.SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi cập nhật thông tin công ty: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi cập nhật thông tin công ty: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật thông tin công ty: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật thông tin công ty: {ex.Message}", ex);
        }
    }

    #endregion
}
