using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Threading.Tasks;
using Dal.DataAccess.Interfaces.MasterData.CompanyRepository;
using Dal.DataContext;
using Dal.Exceptions;
using Logger;
using Logger.Configuration;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.MasterData.CompanyRepository;

/// <summary>
/// Data Access Layer cho quản lý nhân viên.
/// Cung cấp các phương thức truy cập dữ liệu cho nhân viên.
/// </summary>
public class EmployeeRepository : IEmployeeRepository
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
    /// Khởi tạo một instance mới của class EmployeeRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public EmployeeRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("EmployeeRepository được khởi tạo với connection string");
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
        loadOptions.LoadWith<Employee>(e => e.Company);
        loadOptions.LoadWith<Employee>(e => e.CompanyBranch);
        loadOptions.LoadWith<Employee>(e => e.Department);
        loadOptions.LoadWith<Employee>(e => e.Position);
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region ========== QUẢN LÝ DỮ LIỆU ==========

    /// <summary>
    /// Lấy nhân viên theo ID.
    /// </summary>
    /// <param name="id">ID của nhân viên</param>
    /// <returns>Nhân viên hoặc null nếu không tìm thấy</returns>
    public Employee GetById(Guid id)
    {
        try
        {
            using var context = CreateNewContext();
            var employee = context.Employees.FirstOrDefault(x => x.Id == id);
            
            if (employee != null)
            {
                _logger.Debug($"Đã lấy nhân viên theo ID: {id} - {employee.FullName}");
            }
            
            return employee;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy thông tin nhân viên: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy thông tin nhân viên", ex);
        }
    }

    /// <summary>
    /// Lấy nhân viên theo mã nhân viên.
    /// </summary>
    /// <param name="employeeCode">Mã nhân viên</param>
    /// <returns>Nhân viên hoặc null nếu không tìm thấy</returns>
    public Employee GetByEmployeeCode(string employeeCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
                return null;

            using var context = CreateNewContext();
            var employee = context.Employees.FirstOrDefault(x => x.EmployeeCode == employeeCode.Trim());
            
            if (employee != null)
            {
                _logger.Debug($"Đã lấy nhân viên theo mã: {employeeCode} - {employee.FullName}");
            }
            
            return employee;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy thông tin nhân viên theo mã: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy thông tin nhân viên theo mã", ex);
        }
    }

    /// <summary>
    /// Lấy nhân viên theo mã nhân viên và CompanyId.
    /// </summary>
    /// <param name="employeeCode">Mã nhân viên</param>
    /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
    /// <returns>Nhân viên hoặc null nếu không tìm thấy</returns>
    public Employee GetByEmployeeCodeAndCompany(string employeeCode, Guid companyId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
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

            var employee = context.Employees.FirstOrDefault(x => x.EmployeeCode == employeeCode.Trim() && x.CompanyId == companyId);
            
            if (employee != null)
            {
                _logger.Debug($"Đã lấy nhân viên theo mã và công ty: {employeeCode} - {employee.FullName}");
            }
            
            return employee;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy thông tin nhân viên theo mã và công ty: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy thông tin nhân viên theo mã và công ty", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả nhân viên (Async).
    /// </summary>
    /// <returns>Danh sách tất cả nhân viên</returns>
    public async Task<List<Employee>> GetAllAsync()
    {
        try
        {
            using var context = CreateNewContext();

            var employees = await Task.Run(() => context.Employees
                .OrderBy(e => e.FullName)
                .ToList());
            
            _logger.Debug($"Đã lấy {employees.Count} nhân viên (async)");
            return employees;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách nhân viên: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách nhân viên", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả nhân viên (Sync).
    /// </summary>
    /// <returns>Danh sách tất cả nhân viên</returns>
    public List<Employee> GetAll()
    {
        try
        {
            using var context = CreateNewContext();

            var employees = context.Employees
                .OrderBy(e => e.FullName)
                .ToList();
            
            _logger.Debug($"Đã lấy {employees.Count} nhân viên");
            return employees;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách nhân viên: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách nhân viên", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách nhân viên đang hoạt động (Async).
    /// </summary>
    /// <returns>Danh sách nhân viên đang hoạt động</returns>
    public async Task<List<Employee>> GetActiveEmployeesAsync()
    {
        try
        {
            using var context = CreateNewContext();

            var employees = await Task.Run(() => context.Employees
                .Where(x => x.IsActive)
                .OrderBy(e => e.FullName)
                .ToList());
            
            _logger.Debug($"Đã lấy {employees.Count} nhân viên đang hoạt động (async)");
            return employees;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách nhân viên đang hoạt động: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách nhân viên đang hoạt động", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách nhân viên đang hoạt động (Sync).
    /// </summary>
    /// <returns>Danh sách nhân viên đang hoạt động</returns>
    public List<Employee> GetActiveEmployees()
    {
        try
        {
            using var context = CreateNewContext();

            var employees = context.Employees
                .Where(x => x.IsActive)
                .OrderBy(e => e.FullName)
                .ToList();
            
            _logger.Debug($"Đã lấy {employees.Count} nhân viên đang hoạt động");
            return employees;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách nhân viên đang hoạt động: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách nhân viên đang hoạt động", ex);
        }
    }

    /// <summary>
    /// Lấy tất cả nhân viên của một công ty.
    /// </summary>
    /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
    /// <returns>Danh sách nhân viên của công ty</returns>
    public List<Employee> GetByCompanyId(Guid companyId)
    {
        try
        {
            using var context = CreateNewContext();
            
            // Nếu companyId là Guid.Empty, lấy từ Company duy nhất
            if (companyId == Guid.Empty)
            {
                var company = context.Companies.FirstOrDefault();
                if (company == null)
                    return new List<Employee>();
                companyId = company.Id;
            }

            var employees = context.Employees
                .Where(x => x.CompanyId == companyId)
                .OrderBy(e => e.FullName)
                .ToList();
            
            _logger.Debug($"Đã lấy {employees.Count} nhân viên theo công ty: {companyId}");
            return employees;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách nhân viên theo công ty: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách nhân viên theo công ty", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách nhân viên theo chi nhánh.
    /// </summary>
    /// <param name="branchId">ID chi nhánh</param>
    /// <returns>Danh sách nhân viên của chi nhánh</returns>
    public List<Employee> GetByBranchId(Guid branchId)
    {
        try
        {
            using var context = CreateNewContext();

            var employees = context.Employees
                .Where(x => x.BranchId == branchId)
                .OrderBy(e => e.FullName)
                .ToList();
            
            _logger.Debug($"Đã lấy {employees.Count} nhân viên theo chi nhánh: {branchId}");
            return employees;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách nhân viên theo chi nhánh: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách nhân viên theo chi nhánh", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách nhân viên theo phòng ban.
    /// </summary>
    /// <param name="departmentId">ID phòng ban</param>
    /// <returns>Danh sách nhân viên của phòng ban</returns>
    public List<Employee> GetByDepartmentId(Guid departmentId)
    {
        try
        {
            using var context = CreateNewContext();

            var employees = context.Employees
                .Where(x => x.DepartmentId == departmentId)
                .OrderBy(e => e.FullName)
                .ToList();
            
            _logger.Debug($"Đã lấy {employees.Count} nhân viên theo phòng ban: {departmentId}");
            return employees;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách nhân viên theo phòng ban: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách nhân viên theo phòng ban", ex);
        }
    }

    /// <summary>
    /// Lấy danh sách nhân viên theo chức vụ.
    /// </summary>
    /// <param name="positionId">ID chức vụ</param>
    /// <returns>Danh sách nhân viên có chức vụ</returns>
    public List<Employee> GetByPositionId(Guid positionId)
    {
        try
        {
            using var context = CreateNewContext();

            var employees = context.Employees
                .Where(x => x.PositionId == positionId)
                .OrderBy(e => e.FullName)
                .ToList();
            
            _logger.Debug($"Đã lấy {employees.Count} nhân viên theo chức vụ: {positionId}");
            return employees;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi lấy danh sách nhân viên theo chức vụ: {ex.Message}", ex);
            throw new DataAccessException("Lỗi lấy danh sách nhân viên theo chức vụ", ex);
        }
    }

    #endregion

    #region ========== XỬ LÝ DỮ LIỆU ==========

    /// <summary>
    /// Thêm mới nhân viên.
    /// </summary>
    /// <param name="employee">Nhân viên cần thêm</param>
    /// <returns>ID của nhân viên vừa thêm</returns>
    public Guid Insert(Employee employee)
    {
        try
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            using var context = CreateNewContext();
            
            // Set CreatedDate nếu chưa có
            if (employee.CreatedDate == default(DateTime))
            {
                employee.CreatedDate = DateTime.Now;
            }

            context.Employees.InsertOnSubmit(employee);
            context.SubmitChanges();
            
            _logger.Info($"Đã thêm mới nhân viên: {employee.EmployeeCode} - {employee.FullName}");
            return employee.Id;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi thêm mới nhân viên: {ex.Message}", ex);
            throw new DataAccessException("Lỗi thêm mới nhân viên", ex);
        }
    }

    /// <summary>
    /// Cập nhật nhân viên.
    /// </summary>
    /// <param name="employee">Nhân viên cần cập nhật</param>
    public void Update(Employee employee)
    {
        try
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            using var context = CreateNewContext();
            var existingEmployee = context.Employees.FirstOrDefault(x => x.Id == employee.Id);
            
            if (existingEmployee == null)
            {
                throw new DataAccessException("Không tìm thấy nhân viên để cập nhật");
            }

            // Cập nhật các thuộc tính cơ bản
            existingEmployee.CompanyId = employee.CompanyId;
            existingEmployee.BranchId = employee.BranchId;
            existingEmployee.DepartmentId = employee.DepartmentId;
            existingEmployee.PositionId = employee.PositionId;
            existingEmployee.EmployeeCode = employee.EmployeeCode;
            existingEmployee.FullName = employee.FullName;
            existingEmployee.Gender = employee.Gender;
            existingEmployee.BirthDate = employee.BirthDate;
            existingEmployee.Phone = employee.Phone;
            existingEmployee.Email = employee.Email;
            existingEmployee.HireDate = employee.HireDate;
            existingEmployee.ResignDate = employee.ResignDate;
            existingEmployee.AvatarPath = employee.AvatarPath;
            existingEmployee.IsActive = employee.IsActive;

            // Cập nhật các thuộc tính từ BusinessPartnerContact
            existingEmployee.Mobile = employee.Mobile;
            existingEmployee.Fax = employee.Fax;
            existingEmployee.LinkedIn = employee.LinkedIn;
            existingEmployee.Skype = employee.Skype;
            existingEmployee.WeChat = employee.WeChat;
            existingEmployee.Notes = employee.Notes;
            existingEmployee.ModifiedDate = DateTime.Now;

            // Cập nhật các thuộc tính Avatar
            existingEmployee.AvatarFileName = employee.AvatarFileName;
            existingEmployee.AvatarRelativePath = employee.AvatarRelativePath;
            existingEmployee.AvatarFullPath = employee.AvatarFullPath;
            existingEmployee.AvatarStorageType = employee.AvatarStorageType;
            existingEmployee.AvatarFileSize = employee.AvatarFileSize;
            existingEmployee.AvatarChecksum = employee.AvatarChecksum;
            existingEmployee.AvatarThumbnailData = employee.AvatarThumbnailData;

            context.SubmitChanges();
            
            _logger.Info($"Đã cập nhật nhân viên: {existingEmployee.EmployeeCode} - {existingEmployee.FullName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi cập nhật nhân viên: {ex.Message}", ex);
            throw new DataAccessException("Lỗi cập nhật nhân viên", ex);
        }
    }

    /// <summary>
    /// Xóa nhân viên.
    /// </summary>
    /// <param name="employee">Nhân viên cần xóa</param>
    public void Delete(Employee employee)
    {
        try
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));

            using var context = CreateNewContext();
            var existingEmployee = context.Employees.FirstOrDefault(x => x.Id == employee.Id);
            
            if (existingEmployee == null)
            {
                throw new DataAccessException("Không tìm thấy nhân viên để xóa");
            }

            // Kiểm tra ràng buộc dữ liệu trước khi xóa
            // Kiểm tra xem có Asset nào được gán cho nhân viên này không
            var assets = context.Assets.Where(a => a.AssignedEmployeeId == employee.Id).ToList();
            if (assets.Any())
            {
                throw new DataAccessException($"Không thể xóa nhân viên vì còn {assets.Count} tài sản được gán cho nhân viên này");
            }

            // Kiểm tra xem có Device nào được gán cho nhân viên này không
            var devices = context.Devices.Where(d => d.AssignedEmployeeId == employee.Id).ToList();
            if (devices.Any())
            {
                throw new DataAccessException($"Không thể xóa nhân viên vì còn {devices.Count} thiết bị được gán cho nhân viên này");
            }

            context.Employees.DeleteOnSubmit(existingEmployee);
            context.SubmitChanges();
            
            _logger.Info($"Đã xóa nhân viên: {existingEmployee.EmployeeCode} - {existingEmployee.FullName}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi xóa nhân viên: {ex.Message}", ex);
            throw new DataAccessException("Lỗi xóa nhân viên", ex);
        }
    }

    #endregion

    #region ========== KIỂM TRA DỮ LIỆU ==========

    /// <summary>
    /// Kiểm tra mã nhân viên có tồn tại không.
    /// </summary>
    /// <param name="employeeCode">Mã nhân viên cần kiểm tra</param>
    /// <returns>True nếu tồn tại, False nếu không</returns>
    public bool IsEmployeeCodeExists(string employeeCode)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
                return false;

            using var context = CreateNewContext();
            var result = context.Employees.Any(x => x.EmployeeCode == employeeCode.Trim());
            
            _logger.Debug($"IsEmployeeCodeExists: Code='{employeeCode}', Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi kiểm tra mã nhân viên: {ex.Message}", ex);
            throw new DataAccessException("Lỗi kiểm tra mã nhân viên", ex);
        }
    }

    /// <summary>
    /// Kiểm tra mã nhân viên có tồn tại không (trừ bản ghi hiện tại).
    /// </summary>
    /// <param name="employeeCode">Mã nhân viên cần kiểm tra</param>
    /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
    /// <returns>True nếu tồn tại, False nếu không</returns>
    public bool IsEmployeeCodeExists(string employeeCode, Guid excludeId)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(employeeCode))
                return false;

            using var context = CreateNewContext();
            var result = context.Employees.Any(x => x.EmployeeCode == employeeCode.Trim() && x.Id != excludeId);
            
            _logger.Debug($"IsEmployeeCodeExists: Code='{employeeCode}', ExcludeId={excludeId}, Result={result}");
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi kiểm tra mã nhân viên: {ex.Message}", ex);
            throw new DataAccessException("Lỗi kiểm tra mã nhân viên", ex);
        }
    }

    #endregion
}
