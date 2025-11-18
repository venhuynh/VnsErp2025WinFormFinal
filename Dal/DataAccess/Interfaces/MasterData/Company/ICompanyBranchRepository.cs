using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.Company
{
    /// <summary>
    /// Interface cho Data Access Layer quản lý chi nhánh công ty.
    /// Cung cấp các phương thức truy cập dữ liệu cho chi nhánh công ty.
    /// </summary>
    public interface ICompanyBranchRepository
    {
        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Lấy chi nhánh công ty theo ID.
        /// </summary>
        /// <param name="id">ID của chi nhánh công ty</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        CompanyBranch GetById(Guid id);

        /// <summary>
        /// Lấy chi nhánh công ty theo mã chi nhánh.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        CompanyBranch GetByBranchCode(string branchCode);

        /// <summary>
        /// Lấy chi nhánh công ty theo mã chi nhánh và CompanyId.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh</param>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        CompanyBranch GetByBranchCodeAndCompany(string branchCode, Guid companyId);

        /// <summary>
        /// Lấy tất cả chi nhánh của một công ty.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Danh sách chi nhánh của công ty</returns>
        List<CompanyBranch> GetByCompanyId(Guid companyId);

        /// <summary>
        /// Lấy trụ sở chính của công ty.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Trụ sở chính hoặc null nếu không tìm thấy</returns>
        CompanyBranch GetMainBranchByCompanyId(Guid companyId);

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động (Async).
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        Task<List<CompanyBranch>> GetActiveBranchesAsync();

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động (Sync).
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        List<CompanyBranch> GetActiveBranches();

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thêm mới chi nhánh công ty.
        /// </summary>
        /// <param name="companyBranch">Chi nhánh công ty cần thêm</param>
        /// <returns>ID của chi nhánh công ty vừa thêm</returns>
        Guid Insert(CompanyBranch companyBranch);

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
        /// Kiểm tra mã chi nhánh có tồn tại trong cùng công ty không.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh cần kiểm tra</param>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ (tùy chọn)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        bool IsBranchCodeExistsInCompany(string branchCode, Guid companyId, Guid? excludeId = null);

        /// <summary>
        /// Kiểm tra công ty có trụ sở chính chưa.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>True nếu đã có trụ sở chính, False nếu chưa</returns>
        bool HasMainBranch(Guid companyId);

        /// <summary>
        /// Đảm bảo công ty có ít nhất một trụ sở chính.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>True nếu có trụ sở chính hoặc đã tạo, False nếu không thể tạo</returns>
        bool EnsureMainBranchExists(Guid companyId);

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
