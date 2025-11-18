using Dal.BaseDataAccess;
using Dal.DataAccess.Interfaces.MasterData.Company;
using Dal.Exceptions;
using System;
using System.Linq;

namespace Dal.DataAccess.Implementations.MasterData.Company
{
    /// <summary>
    /// Data Access cho thực thể Company (LINQ to SQL trên DataContext).
    /// Cung cấp các truy vấn/biến đổi phổ biến: lấy thông tin công ty, đảm bảo có công ty mặc định, cập nhật thông tin.
    /// </summary>
    public class CompanyRepository : ICompanyRepository
    {
        #region Constructors

        /// <summary>
        /// Khởi tạo mặc định.
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ICompanyRepository(ILogger logger = null) : base(logger)
        {
            EnsureDefaultCompany();
        }

        /// <summary>
        /// Khởi tạo với connection string.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public ICompanyRepository(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
            EnsureDefaultCompany();
        }

        #endregion

        /// <summary>
        /// Đảm bảo có thông tin công ty mặc định
        /// </summary>
        public void EnsureDefaultCompany()
        {
            try
            {
                using var context = CreateContext();
                
                // Kiểm tra xem đã có công ty nào chưa
                var existingCompany = context.Companies.FirstOrDefault();
                
                if (existingCompany == null)
                {
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
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi đảm bảo công ty mặc định: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi đảm bảo công ty mặc định: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy thông tin công ty từ database
        /// </summary>
        /// <returns>Company entity</returns>
        public DataContext.Company GetCompany()
        {
            try
            {
                using var context = CreateContext();
                return context.Companies.FirstOrDefault();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy thông tin công ty: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy thông tin công ty: {ex.Message}", ex);
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
                using var context = CreateContext();
                
                if (company is DataContext.Company companyEntity)
                {
                    // Tìm công ty hiện tại
                    var existingCompany = context.Companies.FirstOrDefault();
                    
                    if (existingCompany != null)
                    {
                        // Cập nhật thông tin
                        existingCompany.CompanyCode = companyEntity.CompanyCode;
                        existingCompany.CompanyName = companyEntity.CompanyName;
                        existingCompany.TaxCode = companyEntity.TaxCode;
                        existingCompany.Phone = companyEntity.Phone;
                        existingCompany.Email = companyEntity.Email;
                        existingCompany.Website = companyEntity.Website;
                        existingCompany.Address = companyEntity.Address;
                        existingCompany.Country = companyEntity.Country;
                        existingCompany.Logo = companyEntity.Logo;
                        existingCompany.UpdatedDate = companyEntity.UpdatedDate;
                        
                        context.SubmitChanges();
                    }
                    else
                    {
                        throw new DataAccessException("Không tìm thấy công ty để cập nhật");
                    }
                }
                else
                {
                    throw new DataAccessException("Đối tượng không phải là Company entity");
                }
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi cập nhật thông tin công ty: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi cập nhật thông tin công ty: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Dispose resources
        /// </summary>
        public void Dispose()
        {
            // CompanyDataAccess không cần dispose gì vì sử dụng using var context
            // Method này chỉ để implement IDisposable pattern
        }
    }
}
