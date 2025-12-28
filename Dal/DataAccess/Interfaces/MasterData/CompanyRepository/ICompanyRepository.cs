using DTO.MasterData.Company;

namespace Dal.DataAccess.Interfaces.MasterData.CompanyRepository;

/// <summary>
/// Data Access cho thực thể Company (LINQ to SQL trên DataContext).
/// Cung cấp các truy vấn/biến đổi phổ biến: lấy thông tin công ty, đảm bảo có công ty mặc định, cập nhật thông tin.
/// </summary>
public interface ICompanyRepository
{
    /// <summary>
    /// Đảm bảo có thông tin công ty mặc định
    /// </summary>
    void EnsureDefaultCompany();

    /// <summary>
    /// Lấy thông tin công ty từ database
    /// </summary>
    /// <returns>CompanyDto</returns>
    CompanyDto GetCompany();

    /// <summary>
    /// Cập nhật thông tin công ty
    /// </summary>
    /// <param name="company">CompanyDto cần cập nhật</param>
    void UpdateCompany(CompanyDto company);

}