using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MasterData.Company;

namespace Dal.DataAccess.Interfaces.MasterData.CompanyRepository
{
    /// <summary>
    /// Interface cho Data Access Layer quản lý chi nhánh công ty.
    /// Cung cấp các phương thức truy cập dữ liệu cho chi nhánh công ty.
    /// </summary>
    public interface ICompanyBranchRepository
    {
        #region Create

        /// <summary>
        /// Thêm mới chi nhánh công ty.
        /// </summary>
        /// <param name="companyBranch">Chi nhánh công ty cần thêm</param>
        /// <returns>ID của chi nhánh công ty vừa thêm</returns>
        Guid Insert(CompanyBranchDto companyBranch);

        #endregion

        #region Retreive

        /// <summary>
        /// Lấy chi nhánh công ty theo ID.
        /// </summary>
        /// <param name="id">ID của chi nhánh công ty</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        CompanyBranchDto GetById(Guid id);

        /// <summary>
        /// Lấy chi nhánh công ty theo mã chi nhánh và CompanyId.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh</param>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        CompanyBranchDto GetByBranchCodeAndCompany(string branchCode, Guid companyId);

        /// <summary>
        /// Lấy tất cả chi nhánh của một công ty.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Danh sách chi nhánh của công ty</returns>
        List<CompanyBranchDto> GetByCompanyId(Guid companyId);

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động (Async).
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        Task<List<CompanyBranchDto>> GetActiveBranchesAsync();

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động (Sync).
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        List<CompanyBranchDto> GetActiveBranches();

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động cho Lookup (chỉ load các trường cần thiết).
        /// Tối ưu hiệu năng bằng cách không load navigation properties.
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        List<CompanyBranchDto> GetActiveBranchesForLookup();

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động cho Lookup (Async) - chỉ load các trường cần thiết.
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        Task<List<CompanyBranchDto>> GetActiveBranchesForLookupAsync();

        /// <summary>
        /// Lấy danh sách tất cả các chi nhánh công ty.
        /// </summary>
        /// <returns>Danh sách tất cả các chi nhánh công ty.</returns>
        List<CompanyBranchDto> GetAll();

        #endregion


        #region Update


        /// <summary>
        /// Cập nhật chi nhánh công ty.
        /// </summary>
        /// <param name="companyBranch">Chi nhánh công ty cần cập nhật</param>
        void Update(CompanyBranchDto companyBranch);


        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Kiểm tra mã chi nhánh có tồn tại không.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsBranchCodeExists(string branchCode);

        /// <summary>
        /// Kiểm tra mã chi nhánh có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsBranchCodeExists(string branchCode, Guid excludeId);

        /// <summary>
        /// Kiểm tra tên chi nhánh có tồn tại không.
        /// </summary>
        /// <param name="branchName">Tên chi nhánh cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsBranchNameExists(string branchName);

        /// <summary>
        /// Kiểm tra tên chi nhánh có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="branchName">Tên chi nhánh cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsBranchNameExists(string branchName, Guid excludeId);

        #endregion

        
    }
}
