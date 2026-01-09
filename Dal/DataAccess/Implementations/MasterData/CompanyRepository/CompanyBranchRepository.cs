using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Dal.DataContext;
using Dal.Exceptions;
using DTO.MasterData.Company;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DtoConverter;
using Dal.DtoConverter.MasterData.Company;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.CompanyRepository;

/// <summary>
/// Data Access Layer cho quản lý chi nhánh công ty.
/// Cung cấp các phương thức truy cập dữ liệu cho chi nhánh công ty.
/// </summary>
public class CompanyBranchRepository : ICompanyBranchRepository
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
    /// Khởi tạo một instance mới của class CompanyBranchRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public CompanyBranchRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("CompanyBranchRepository được khởi tạo với connection string");
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

    /// <summary>
    /// Tạo DataContext mới cho Lookup (không load navigation properties để tối ưu hiệu năng)
    /// </summary>
    /// <returns>DataContext mới không có LoadOptions</returns>
    private VnsErp2025DataContext CreateLookupContext()
    {
        // Tạo context mới nhưng KHÔNG set LoadOptions để tránh load navigation properties không cần thiết
        return new VnsErp2025DataContext(_connectionString);
    }

    #endregion

    #region Create


    /// <summary>
    /// Thêm mới chi nhánh công ty.
    /// </summary>
    /// <param name="companyBranch">Chi nhánh công ty cần thêm</param>
    /// <returns>ID của chi nhánh công ty vừa thêm</returns>
    public Guid Insert(CompanyBranchDto companyBranch)
    {
        try
        {
            if (companyBranch == null)
                throw new ArgumentNullException(nameof(companyBranch));

            // Chuyển đổi DTO sang Entity
            var entity = companyBranch.ToEntity();

            using var context = CreateNewContext();
            context.CompanyBranches.InsertOnSubmit(entity);
            context.SubmitChanges();

            _logger.Info($"Đã thêm mới chi nhánh công ty: {entity.BranchCode} - {entity.BranchName}");
            return entity.Id;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi thêm mới chi nhánh công ty: {ex.Message}", ex);
            throw new DataAccessException("Lỗi thêm mới chi nhánh công ty", ex);
        }
    }

    #endregion

    #region Retrieve


    /// <summary>
    /// Lấy chi nhánh công ty theo ID.
    /// </summary>
    /// <param name="id">ID của chi nhánh công ty</param>
    /// <returns>Chi nhánh công ty hoặc null nếu không tìm thấy</returns>
    public CompanyBranchDto GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            return context.CompanyBranches.FirstOrDefault(x => x.Id == id)?.ToDto();
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

            using var context = CreateNewContext();
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
    public CompanyBranchDto GetByBranchCodeAndCompany(string branchCode, Guid companyId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(branchCode))
                return null;

            using var context = CreateNewContext();

            // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
            if (companyId == Guid.Empty)
            {
                var company = context.Companies.FirstOrDefault();
                if (company == null)
                    return null;
                companyId = company.Id;
            }

            var entity = context.CompanyBranches.FirstOrDefault(x =>
                x.BranchCode == branchCode.Trim() && x.CompanyId == companyId);
            return entity?.ToDto();
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
    public List<CompanyBranchDto> GetByCompanyId(Guid companyId)
    {
        try
        {
            using var context = CreateNewContext();

            // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
            if (companyId == Guid.Empty)
            {
                var company = context.Companies.FirstOrDefault();
                if (company == null)
                    return new List<CompanyBranchDto>();
                companyId = company.Id;
            }

            var result = context.CompanyBranches.Where(x => x.CompanyId == companyId).ToList();
            return result.Select(x => x.ToDto()).ToList();
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
            using var context = CreateNewContext();

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
    public async Task<List<CompanyBranchDto>> GetActiveBranchesAsync()
    {
        try
        {
            using var context = CreateNewContext();
            var entities = await Task.Run(() => context.CompanyBranches.Where(x => x.IsActive).ToList());
            return entities.Select(x => x.ToDto()).ToList();
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
    public List<CompanyBranchDto> GetActiveBranches()
    {
        try
        {
            using var context = CreateNewContext();
            var entities = context.CompanyBranches.Where(x => x.IsActive).ToList();
            return entities.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Lỗi lấy danh sách chi nhánh công ty đang hoạt động", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách chi nhánh công ty đang hoạt động cho Lookup (chỉ load các trường cần thiết).
    /// Không load navigation properties để tối ưu hiệu năng.
    /// LINQ to SQL sẽ tự động chỉ select các trường được sử dụng (Id, CompanyId, BranchCode, BranchName, IsActive).
    /// </summary>
    /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
    public List<CompanyBranchDto> GetActiveBranchesForLookup()
    {
        try
        {
            using var context = CreateLookupContext();

            // Query CompanyBranch entities nhưng không load navigation properties
            // LINQ to SQL sẽ chỉ select các trường được truy cập sau khi ToList()
            var branches = context.CompanyBranches
                .Where(x => x.IsActive)
                .ToList();

            // Materialize chỉ các trường cần thiết để đảm bảo chúng được load từ DB
            // Các trường khác sẽ là null/default nhưng không ảnh hưởng vì ta chỉ dùng các trường này
            foreach (var branch in branches)
            {
                // Force materialize các trường cần thiết cho LookupDto
                var _ = branch.Id;
                var __ = branch.CompanyId;
                var ___ = branch.BranchCode;
                var ____ = branch.BranchName;
                var _____ = branch.IsActive;
            }

            // Chuyển đổi sang DTO
            return branches.Select(x => x.ToDto()).ToList();
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Lỗi lấy danh sách chi nhánh công ty cho lookup", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách chi nhánh công ty đang hoạt động cho Lookup (Async) - chỉ load các trường cần thiết.
    /// </summary>
    /// <returns>Danh sách chi nhánh công ty đang hoạt động</returns>
    public async Task<List<CompanyBranchDto>> GetActiveBranchesForLookupAsync()
    {
        try
        {
            return await Task.Run(() =>
            {
                using var context = CreateLookupContext();

                // Query CompanyBranch entities nhưng không load navigation properties
                var branches = context.CompanyBranches
                    .Where(x => x.IsActive)
                    .ToList();

                // Materialize chỉ các trường cần thiết
                foreach (var branch in branches)
                {
                    var _ = branch.Id;
                    var __ = branch.CompanyId;
                    var ___ = branch.BranchCode;
                    var ____ = branch.BranchName;
                    var _____ = branch.IsActive;
                }

                // Chuyển đổi sang DTO
                return branches.Select(x => x.ToDto()).ToList();
            });
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Lỗi lấy danh sách chi nhánh công ty cho lookup (async)", ex);
        }
    }

    /// <summary>
    /// Retrieves all company branches from the data source.
    /// </summary>
    /// <returns>
    /// A list of <see cref="CompanyBranchDto"/> representing all company branches.
    /// </returns>
    /// <exception cref="Exception">
    /// Thrown when an error occurs while retrieving the company branches.
    /// </exception>
    public List<CompanyBranchDto> GetAll()
    {
        using var context = CreateNewContext();
        return context.CompanyBranches.ToList().Select(x => x.ToDto()).ToList();
    }

    #endregion

    #region Update


    /// <summary>
    /// Cập nhật chi nhánh công ty.
    /// </summary>
    /// <param name="companyBranch">Chi nhánh công ty cần cập nhật</param>
    public void Update(CompanyBranchDto companyBranch)
    {
        try
        {
            if (companyBranch == null)
                throw new ArgumentNullException(nameof(companyBranch));

            using var context = CreateNewContext();
            var existingBranch = context.CompanyBranches.FirstOrDefault(x => x.Id == companyBranch.Id);

            if (existingBranch == null)
            {
                throw new DataAccessException("Không tìm thấy chi nhánh công ty để cập nhật");
            }

            // Sử dụng converter để cập nhật entity từ DTO
            companyBranch.ToEntity(existingBranch);

            context.SubmitChanges();

            _logger.Info($"Đã cập nhật chi nhánh công ty: {existingBranch.BranchCode} - {existingBranch.BranchName}");
        }
        catch (DataAccessException)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi cập nhật chi nhánh công ty: {ex.Message}", ex);
            throw new DataAccessException("Lỗi cập nhật chi nhánh công ty", ex);
        }
    }


    #endregion

    #region Delete



    #endregion

    #region ========== QUẢN LÝ DỮ LIỆU ==========

    #endregion

    #region ========== XỬ LÝ DỮ LIỆU ==========


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

            using var context = CreateNewContext();
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

            using var context = CreateNewContext();
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

            using var context = CreateNewContext();
                
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
            using var context = CreateNewContext();
                
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
            var mainBranchDto = new CompanyBranchDto
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                BranchCode = "MAIN-001",
                BranchName = "Trụ sở chính",
                Address = "Địa chỉ trụ sở chính",
                IsActive = true
            };

            Insert(mainBranchDto);
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

            using var context = CreateNewContext();
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

            using var context = CreateNewContext();
            return context.CompanyBranches.Any(x => x.BranchName == branchName.Trim() && x.Id != excludeId);
        }
        catch (Exception ex)
        {
            throw new DataAccessException("Lỗi kiểm tra tên chi nhánh", ex);
        }
    }

    #endregion
}