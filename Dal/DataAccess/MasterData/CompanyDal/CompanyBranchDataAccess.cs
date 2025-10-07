using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;

namespace Dal.DataAccess.MasterData.CompanyDal
{
    /// <summary>
    /// Data Access Layer cho quản lý chi nhánh công ty.
    /// Cung cấp các phương thức truy cập dữ liệu cho chi nhánh công ty.
    /// </summary>
    public class CompanyBranchDataAccess : BaseDataAccess<CompanyBranch>
    {
        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo Data Access Layer cho chi nhánh công ty.
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public CompanyBranchDataAccess(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Khởi tạo với connection string.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public CompanyBranchDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        #endregion

        #region ========== QUẢN LÝ DỮ LIỆU ==========

        /// <summary>
        /// Lấy chi nhánh công ty theo ID.
        /// </summary>
        /// <param name="id">ID của chi nhánh công ty</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        public CompanyBranch GetById(Guid id)
        {
            try
            {
                using var context = CreateContext();
                return context.CompanyBranches.FirstOrDefault(x => x.Id == id);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy thông tin chi nhánh công ty", ex);
            }
        }

        /// <summary>
        /// Lấy chi nhánh công ty theo mã chi nhánh.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        public CompanyBranch GetByBranchCode(string branchCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchCode))
                    return null;

                using var context = CreateContext();
                return context.CompanyBranches.FirstOrDefault(x => x.BranchCode == branchCode.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy thông tin chi nhánh công ty theo mã", ex);
            }
        }

        /// <summary>
        /// Lấy chi nhánh công ty theo mã chi nhánh và CompanyId.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh</param>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
        public CompanyBranch GetByBranchCodeAndCompany(string branchCode, Guid companyId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchCode))
                    return null;

                using var context = CreateContext();
                
                // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
                if (companyId == Guid.Empty)
                {
                    var company = context.Companies.FirstOrDefault();
                    if (company == null)
                        return null;
                    companyId = company.Id;
                }

                return context.CompanyBranches.FirstOrDefault(x => 
                    x.BranchCode == branchCode.Trim() && x.CompanyId == companyId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy thông tin chi nhánh công ty theo mã và công ty", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả chi nhánh của một công ty.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Danh sách chi nhánh của công ty</returns>
        public List<CompanyBranch> GetByCompanyId(Guid companyId)
        {
            try
            {
                using var context = CreateContext();
                
                // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
                if (companyId == Guid.Empty)
                {
                    var company = context.Companies.FirstOrDefault();
                    if (company == null)
                        return new List<CompanyBranch>();
                    companyId = company.Id;
                }

                return context.CompanyBranches.Where(x => x.CompanyId == companyId).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy danh sách chi nhánh theo công ty", ex);
            }
        }

        /// <summary>
        /// Lấy trụ sở chính của công ty.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>Trụ sở chính hoặc null nếu không tìm thấy</returns>
        public CompanyBranch GetMainBranchByCompanyId(Guid companyId)
        {
            try
            {
                using var context = CreateContext();
                
                // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
                if (companyId == Guid.Empty)
                {
                    var company = context.Companies.FirstOrDefault();
                    if (company == null)
                        return null;
                    companyId = company.Id;
                }

                // Tìm trụ sở chính (có thể dựa vào tên hoặc mã đặc biệt)
                return context.CompanyBranches.FirstOrDefault(x => 
                    x.CompanyId == companyId && 
                    (x.BranchCode.ToUpper().Contains("MAIN") || 
                     x.BranchCode.ToUpper().Contains("HEAD") ||
                     x.BranchName.ToUpper().Contains("TRỤ SỞ CHÍNH") ||
                     x.BranchName.ToUpper().Contains("HEADQUARTERS")));
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy trụ sở chính của công ty", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động (Async).
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        public async Task<List<CompanyBranch>> GetActiveBranchesAsync()
        {
            try
            {
                using var context = CreateContext();
                return await Task.Run(() => context.CompanyBranches.Where(x => x.IsActive).ToList());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy danh sách chi nhánh công ty đang hoạt động", ex);
            }
        }

        /// <summary>
        /// Lấy danh sách chi nhánh công ty đang hoạt động (Sync).
        /// </summary>
        /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
        public List<CompanyBranch> GetActiveBranches()
        {
            try
            {
                using var context = CreateContext();
                return context.CompanyBranches.Where(x => x.IsActive).ToList();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi lấy danh sách chi nhánh công ty đang hoạt động", ex);
            }
        }

        #endregion

        #region ========== XỬ LÝ DỮ LIỆU ==========

        /// <summary>
        /// Thêm mới chi nhánh công ty.
        /// </summary>
        /// <param name="companyBranch">Chi nhánh công ty cần thêm</param>
        /// <returns>ID của chi nhánh công ty vừa thêm</returns>
        public Guid Insert(CompanyBranch companyBranch)
        {
            try
            {
                Add(companyBranch);
                return companyBranch.Id;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi thêm mới chi nhánh công ty", ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã chi nhánh có tồn tại không.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchCodeExists(string branchCode)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchCode))
                    return false;

                using var context = CreateContext();
                return context.CompanyBranches.Any(x => x.BranchCode == branchCode.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra mã chi nhánh", ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã chi nhánh có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchCodeExists(string branchCode, Guid excludeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchCode))
                    return false;

                using var context = CreateContext();
                return context.CompanyBranches.Any(x => x.BranchCode == branchCode.Trim() && x.Id != excludeId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra mã chi nhánh", ex);
            }
        }

        /// <summary>
        /// Kiểm tra mã chi nhánh có tồn tại trong cùng công ty không.
        /// </summary>
        /// <param name="branchCode">Mã chi nhánh cần kiểm tra</param>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ (tùy chọn)</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchCodeExistsInCompany(string branchCode, Guid companyId, Guid? excludeId = null)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchCode))
                    return false;

                using var context = CreateContext();
                
                // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
                if (companyId == Guid.Empty)
                {
                    var company = context.Companies.FirstOrDefault();
                    if (company == null)
                        return false;
                    companyId = company.Id;
                }

                var query = context.CompanyBranches.Where(x => 
                    x.BranchCode == branchCode.Trim() && x.CompanyId == companyId);
                
                if (excludeId.HasValue)
                {
                    query = query.Where(x => x.Id != excludeId.Value);
                }
                
                return query.Any();
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra mã chi nhánh trong công ty", ex);
            }
        }

        /// <summary>
        /// Kiểm tra công ty có trụ sở chính chưa.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>True nếu đã có trụ sở chính, False nếu chưa</returns>
        public bool HasMainBranch(Guid companyId)
        {
            try
            {
                var mainBranch = GetMainBranchByCompanyId(companyId);
                return mainBranch != null;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra trụ sở chính", ex);
            }
        }

        /// <summary>
        /// Đảm bảo công ty có ít nhất một trụ sở chính.
        /// </summary>
        /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
        /// <returns>True nếu có trụ sở chính hoặc đã tạo, False nếu không thể tạo</returns>
        public bool EnsureMainBranchExists(Guid companyId)
        {
            try
            {
                using var context = CreateContext();
                
                // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
                if (companyId == Guid.Empty)
                {
                    var company = context.Companies.FirstOrDefault();
                    if (company == null)
                        return false;
                    companyId = company.Id;
                }

                // Kiểm tra đã có trụ sở chính chưa
                if (HasMainBranch(companyId))
                    return true;

                // Tạo trụ sở chính mặc định
                var mainBranch = new CompanyBranch
                {
                    Id = Guid.NewGuid(),
                    CompanyId = companyId,
                    BranchCode = "MAIN-001",
                    BranchName = "Trụ sở chính",
                    Address = "Địa chỉ trụ sở chính",
                    IsActive = true
                };

                Insert(mainBranch);
                return true;
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi đảm bảo trụ sở chính tồn tại", ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên chi nhánh có tồn tại không.
        /// </summary>
        /// <param name="branchName">Tên chi nhánh cần kiểm tra</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchNameExists(string branchName)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchName))
                    return false;

                using var context = CreateContext();
                return context.CompanyBranches.Any(x => x.BranchName == branchName.Trim());
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra tên chi nhánh", ex);
            }
        }

        /// <summary>
        /// Kiểm tra tên chi nhánh có tồn tại không (trừ bản ghi hiện tại).
        /// </summary>
        /// <param name="branchName">Tên chi nhánh cần kiểm tra</param>
        /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
        /// <returns>True nếu tồn tại, False nếu không</returns>
        public bool IsBranchNameExists(string branchName, Guid excludeId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(branchName))
                    return false;

                using var context = CreateContext();
                return context.CompanyBranches.Any(x => x.BranchName == branchName.Trim() && x.Id != excludeId);
            }
            catch (Exception ex)
            {
                throw new DataAccessException("Lỗi kiểm tra tên chi nhánh", ex);
            }
        }

        #endregion
    }
}
