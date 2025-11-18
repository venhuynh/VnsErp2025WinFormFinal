using Dal.DataAccess.Interfaces.MasterData.Company;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.Company;

/// <summary>
/// Data Access Layer cho Department
/// Cung cấp các phương thức truy cập dữ liệu cho Department
/// </summary>
public class DepartmentRepository : IDepartmentRepository
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
    /// Khởi tạo một instance mới của class DepartmentRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public DepartmentRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("DepartmentRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<Department>(d => d.CompanyBranch);
        loadOptions.LoadWith<Department>(d => d.Company);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region ========== CRUD OPERATIONS ==========

    /// <summary>
    /// Lấy tất cả departments với thông tin chi nhánh
    /// </summary>
    /// <returns>Danh sách tất cả departments</returns>
    public List<Department> GetAll()
    {
        try
        {
            using var context = CreateNewContext();

            // Load tất cả departments với relationships đã được include
            var departments = context.Departments
                .OrderBy(d => d.DepartmentName)
                .ToList();

            _logger.Debug($"Đã lấy {departments.Count} departments");
            return departments;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy danh sách departments: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy danh sách departments: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy danh sách departments: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy danh sách departments: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả departments với thông tin chi nhánh (Async)
    /// </summary>
    /// <returns>Danh sách tất cả departments</returns>
    public async Task<List<Department>> GetAllAsync()
    {
        return await Task.Run(GetAll);
    }

    /// <summary>
    /// Lấy department theo ID với thông tin chi nhánh (synchronous)
    /// </summary>
    /// <param name="id">ID của department</param>
    /// <returns>Department hoặc null nếu không tìm thấy</returns>
    public Department GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();

            var department = context.Departments
                .FirstOrDefault(d => d.Id == id);
            
            if (department != null)
            {
                _logger.Debug($"Đã lấy department theo ID: {id} - {department.DepartmentName}");
            }
            
            return department;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy department theo ID {id}: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy department theo ID {id}: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy department theo ID {id}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy department theo ID {id}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy department theo ID với thông tin chi nhánh (async)
    /// </summary>
    /// <param name="id">ID của department</param>
    /// <returns>Department hoặc null nếu không tìm thấy</returns>
    public async Task<Department> GetByIdAsync(Guid id)
    {
        return await Task.Run(() => GetById(id));
    }

    /// <summary>
    /// Tạo mới department
    /// </summary>
    /// <param name="department">Department cần tạo</param>
    /// <returns>Department đã được tạo</returns>
    public async Task<Department> CreateAsync(Department department)
    {
        try
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            // Set default values
            if (department.Id == Guid.Empty)
            {
                department.Id = Guid.NewGuid();
            }

            using var context = CreateNewContext();
            context.Departments.InsertOnSubmit(department);
            await Task.Run(() => context.SubmitChanges());
            
            _logger.Info($"Đã tạo mới department: {department.DepartmentCode} - {department.DepartmentName}");
            return department;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi tạo department: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi tạo department: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Cập nhật department (synchronous)
    /// </summary>
    /// <param name="department">Department cần cập nhật</param>
    /// <returns>Department đã được cập nhật</returns>
    public Department UpdateDepartment(Department department)
    {
        try
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            using var context = CreateNewContext();
            var existingDepartment = context.Departments.FirstOrDefault(d => d.Id == department.Id);
            
            if (existingDepartment == null)
            {
                throw new DataAccessException($"Không tìm thấy department với ID {department.Id} để cập nhật");
            }

            // Cập nhật các thuộc tính
            existingDepartment.CompanyId = department.CompanyId;
            existingDepartment.BranchId = department.BranchId;
            existingDepartment.ParentId = department.ParentId;
            existingDepartment.DepartmentCode = department.DepartmentCode;
            existingDepartment.DepartmentName = department.DepartmentName;
            existingDepartment.Description = department.Description;
            existingDepartment.IsActive = department.IsActive;

            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật department: {existingDepartment.DepartmentCode} - {existingDepartment.DepartmentName}");
            return existingDepartment;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi cập nhật department: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi cập nhật department: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật department: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật department: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Cập nhật department (async) - Sử dụng logic update riêng
    /// </summary>
    /// <param name="department">Department cần cập nhật</param>
    /// <returns>Department đã được cập nhật</returns>
    public async Task<Department> UpdateDepartmentAsync(Department department)
    {
        try
        {
            if (department == null)
                throw new ArgumentNullException(nameof(department));

            using var context = CreateNewContext();
            
            // Tìm department hiện tại từ database
            var existingDepartment = context.Departments.FirstOrDefault(d => d.Id == department.Id);
            
            if (existingDepartment != null)
            {
                // Kiểm tra xem có cần cập nhật DepartmentCode không
                // Nếu DepartmentCode thay đổi, cần kiểm tra duplicate
                if (existingDepartment.DepartmentCode != department.DepartmentCode)
                {
                    // Kiểm tra xem DepartmentCode mới có bị trùng không
                    var duplicateCheck = context.Departments
                        .FirstOrDefault(d => d.CompanyId == department.CompanyId && 
                                             d.DepartmentCode == department.DepartmentCode && 
                                             d.Id != department.Id);
                    
                    if (duplicateCheck != null)
                    {
                        throw new DataAccessException($"Mã phòng ban '{department.DepartmentCode}' đã tồn tại trong công ty");
                    }
                }
                
                // Cập nhật các thuộc tính
                existingDepartment.CompanyId = department.CompanyId;
                existingDepartment.BranchId = department.BranchId;
                existingDepartment.ParentId = department.ParentId;
                existingDepartment.DepartmentCode = department.DepartmentCode;
                existingDepartment.DepartmentName = department.DepartmentName;
                existingDepartment.Description = department.Description;
                existingDepartment.IsActive = department.IsActive;
                
                await Task.Run(() => context.SubmitChanges());
                
                _logger.Info($"Đã cập nhật department (async): {existingDepartment.DepartmentCode} - {existingDepartment.DepartmentName}");
                
                // Trả về department đã được cập nhật
                return existingDepartment;
            }
            else
            {
                throw new DataAccessException($"Không tìm thấy department với ID {department.Id} để cập nhật");
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật department (async): {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi cập nhật department: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa department
    /// </summary>
    /// <param name="id">ID của department cần xóa</param>
    /// <returns>True nếu xóa thành công</returns>
    public Task<bool> DeleteAsync(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Kiểm tra xem có department con không
            var childDepartments = context.Departments.Where(d => d.ParentId == id).ToList();
            if (childDepartments.Any())
            {
                throw new DataAccessException($"Không thể xóa department vì còn {childDepartments.Count} department con");
            }

            // Kiểm tra xem có employee nào thuộc department này không
            var employees = context.Employees.Where(e => e.DepartmentId == id).ToList();
            if (employees.Any())
            {
                throw new DataAccessException($"Không thể xóa department vì còn {employees.Count} employee thuộc department này");
            }

            var department = context.Departments.FirstOrDefault(d => d.Id == id);
            if (department == null)
            {
                throw new DataAccessException($"Không tìm thấy department với ID {id} để xóa");
            }

            context.Departments.DeleteOnSubmit(department);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa department: {department.DepartmentCode} - {department.DepartmentName}");
            return Task.FromResult(true);
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa department: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa department: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa nhiều departments (Synchronous)
    /// </summary>
    /// <param name="ids">Danh sách ID của departments cần xóa</param>
    /// <returns>True nếu xóa thành công</returns>
    public bool DeleteMultiple(List<Guid> ids)
    {
        try
        {
            using var context = CreateNewContext();
            
            var departments = context.Departments.Where(d => ids.Contains(d.Id)).ToList();
            if (!departments.Any())
            {
                throw new DataAccessException("Không tìm thấy departments cần xóa");
            }

            // Kiểm tra constraints trước khi xóa
            foreach (var department in departments)
            {
                // Kiểm tra có phòng ban con không
                var hasChildren = context.Departments.Any(d => d.ParentId == department.Id);
                if (hasChildren)
                {
                    throw new DataAccessException($"Không thể xóa phòng ban '{department.DepartmentName}' vì còn có phòng ban con");
                }

                // Kiểm tra có nhân viên không
                var hasEmployees = context.Employees.Any(e => e.DepartmentId == department.Id);
                if (hasEmployees)
                {
                    throw new DataAccessException($"Không thể xóa phòng ban '{department.DepartmentName}' vì còn có nhân viên");
                }
            }

            // Xóa departments
            context.Departments.DeleteAllOnSubmit(departments);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa {departments.Count} departments");
            return true;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi xóa departments: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi xóa departments: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa departments: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi xóa departments: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Xóa nhiều departments (Async)
    /// </summary>
    /// <param name="ids">Danh sách ID của departments cần xóa</param>
    /// <returns>True nếu xóa thành công</returns>
    public async Task<bool> DeleteMultipleAsync(List<Guid> ids)
    {
        return await Task.Run(() => DeleteMultiple(ids));
    }

    #endregion

    #region ========== BUSINESS LOGIC ==========

    /// <summary>
    /// Lấy departments theo company ID với thông tin chi nhánh
    /// </summary>
    /// <param name="companyId">ID của company</param>
    /// <returns>Danh sách departments thuộc company</returns>
    public List<Department> GetByCompanyId(Guid companyId)
    {
        try
        {
            using var context = CreateNewContext();

            var departments = context.Departments
                .Where(d => d.CompanyId == companyId)
                .OrderBy(d => d.DepartmentName)
                .ToList();

            _logger.Debug($"Đã lấy {departments.Count} departments theo company ID: {companyId}");
            return departments;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy departments theo company ID {companyId}: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy departments theo company ID {companyId}: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy departments theo company ID {companyId}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy departments theo company ID {companyId}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy departments theo company ID với thông tin chi nhánh (Async)
    /// </summary>
    /// <param name="companyId">ID của company</param>
    /// <returns>Danh sách departments thuộc company</returns>
    public async Task<List<Department>> GetByCompanyIdAsync(Guid companyId)
    {
        return await Task.Run(() => GetByCompanyId(companyId));
    }

    /// <summary>
    /// Lấy departments theo branch ID với thông tin chi nhánh
    /// </summary>
    /// <param name="branchId">ID của branch</param>
    /// <returns>Danh sách departments thuộc branch</returns>
    public List<Department> GetByBranchId(Guid branchId)
    {
        try
        {
            using var context = CreateNewContext();

            var departments = context.Departments
                .Where(d => d.BranchId == branchId)
                .OrderBy(d => d.DepartmentName)
                .ToList();

            _logger.Debug($"Đã lấy {departments.Count} departments theo branch ID: {branchId}");
            return departments;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy departments theo branch ID {branchId}: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy departments theo branch ID {branchId}: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy departments theo branch ID {branchId}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy departments theo branch ID {branchId}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy departments theo branch ID với thông tin chi nhánh (Async)
    /// </summary>
    /// <param name="branchId">ID của branch</param>
    /// <returns>Danh sách departments thuộc branch</returns>
    public async Task<List<Department>> GetByBranchIdAsync(Guid branchId)
    {
        return await Task.Run(() => GetByBranchId(branchId));
    }

    /// <summary>
    /// Lấy departments theo parent ID với thông tin chi nhánh
    /// </summary>
    /// <param name="parentId">ID của parent department</param>
    /// <returns>Danh sách departments con</returns>
    public List<Department> GetByParentId(Guid parentId)
    {
        try
        {
            using var context = CreateNewContext();

            var departments = context.Departments
                .Where(d => d.ParentId == parentId)
                .OrderBy(d => d.DepartmentName)
                .ToList();

            _logger.Debug($"Đã lấy {departments.Count} departments con theo parent ID: {parentId}");
            return departments;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy departments theo parent ID {parentId}: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy departments theo parent ID {parentId}: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy departments theo parent ID {parentId}: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy departments theo parent ID {parentId}: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy departments theo parent ID với thông tin chi nhánh (Async)
    /// </summary>
    /// <param name="parentId">ID của parent department</param>
    /// <returns>Danh sách departments con</returns>
    public async Task<List<Department>> GetByParentIdAsync(Guid parentId)
    {
        return await Task.Run(() => GetByParentId(parentId));
    }

    /// <summary>
    /// Lấy root departments (không có parent) với thông tin chi nhánh
    /// </summary>
    /// <returns>Danh sách root departments</returns>
    public List<Department> GetRootDepartments()
    {
        try
        {
            using var context = CreateNewContext();

            var departments = context.Departments
                .Where(d => d.ParentId == null)
                .OrderBy(d => d.DepartmentName)
                .ToList();

            _logger.Debug($"Đã lấy {departments.Count} root departments");
            return departments;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi lấy root departments: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi lấy root departments: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy root departments: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi lấy root departments: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Lấy root departments (không có parent) với thông tin chi nhánh (Async)
    /// </summary>
    /// <returns>Danh sách root departments</returns>
    public async Task<List<Department>> GetRootDepartmentsAsync()
    {
        return await Task.Run(() => GetRootDepartments());
    }

    /// <summary>
    /// Kiểm tra department code có tồn tại không (synchronous)
    /// </summary>
    /// <param name="departmentCode">Mã department</param>
    /// <param name="excludeId">ID cần loại trừ (cho update)</param>
    /// <returns>True nếu tồn tại</returns>
    public bool IsDepartmentCodeExists(string departmentCode, Guid? excludeId = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(departmentCode))
                return false;

            using var context = CreateNewContext();
            
            var query = context.Departments.Where(d => d.DepartmentCode == departmentCode);
            if (excludeId.HasValue)
            {
                query = query.Where(d => d.Id != excludeId.Value);
            }
            
            var result = query.Any();
            _logger.Debug($"IsDepartmentCodeExists: Code='{departmentCode}', ExcludeId={excludeId}, Result={result}");
            
            return result;
        }
        catch (SqlException sqlEx)
        {
            _logger.Error($"Lỗi SQL khi kiểm tra department code: {sqlEx.Message}", sqlEx);
            throw new DataAccessException($"Lỗi SQL khi kiểm tra department code: {sqlEx.Message}", sqlEx)
            {
                SqlErrorNumber = sqlEx.Number,
                ThoiGianLoi = DateTime.Now
            };
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi kiểm tra department code: {ex.Message}", ex);
            throw new DataAccessException($"Lỗi khi kiểm tra department code: {ex.Message}", ex);
        }
    }

    /// <summary>
    /// Kiểm tra department code có tồn tại không (async)
    /// </summary>
    /// <param name="departmentCode">Mã department</param>
    /// <param name="excludeId">ID cần loại trừ (cho update)</param>
    /// <returns>True nếu tồn tại</returns>
    public async Task<bool> IsDepartmentCodeExistsAsync(string departmentCode, Guid? excludeId = null)
    {
        return await Task.Run(() => IsDepartmentCodeExists(departmentCode, excludeId));
    }

    #endregion
}
