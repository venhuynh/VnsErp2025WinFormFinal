using Dal.BaseDataAccess;
using Dal.DataAccess.Interfaces.MasterData.Company;
using Dal.DataContext;
using Dal.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.DataAccess.Implementations.MasterData.Company
{
    /// <summary>
    /// Data Access Layer cho Department
    /// Cung cấp các phương thức truy cập dữ liệu cho Department
    /// </summary>
    public class DepartmentRepository : IDepartmentRepository
    {
        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo DepartmentDataAccess
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public DepartmentRepository(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Khởi tạo với connection string
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public DepartmentRepository(string connectionString, ILogger logger = null) : base(connectionString, logger)
        {
        }

        #endregion

        #region ========== CRUD OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả departments với thông tin chi nhánh
        /// </summary>
        /// <returns>Danh sách tất cả departments</returns>
        public override List<Department> GetAll()
        {
            try
            {
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Department>(d => d.CompanyBranch);
                loadOptions.LoadWith<Department>(d => d.Company);


                context.LoadOptions = loadOptions;

                // Load tất cả departments với relationships đã được include
                var departments = context.Departments
                    .OrderBy(d => d.DepartmentName)
                    .ToList();

                return departments;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy danh sách departments: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Logger?.LogError($"Lỗi khi lấy danh sách departments: {ex.Message}", ex);
                throw new DataAccessException($"Lỗi khi lấy danh sách departments: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả departments với thông tin chi nhánh (Async)
        /// </summary>
        /// <returns>Danh sách tất cả departments</returns>
        public override async Task<List<Department>> GetAllAsync()
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
                using var context = CreateContext();
                // Sử dụng DataLoadOptions để include relationships (tránh circular reference)
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Department>(d => d.CompanyBranch);
                loadOptions.LoadWith<Department>(d => d.Company);
                // Không load Department1 để tránh circular reference
                context.LoadOptions = loadOptions;

                var department = context.Departments
                    .FirstOrDefault(d => d.Id == id);
                
                return department;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy department theo ID {id}: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Logger?.LogError($"Lỗi khi lấy department theo ID {id}: {ex.Message}", ex);
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
                // Set default values
                department.Id = Guid.NewGuid();

                await base.AddAsync(department);
                return department;
            }
            catch (Exception ex)
            {
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
                base.Update(department);
                return department;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi cập nhật department: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                System.Diagnostics.Debug.WriteLine($"DepartmentDataAccess.UpdateDepartmentAsync - Department.BranchId: {department.BranchId}");
                System.Diagnostics.Debug.WriteLine($"DepartmentDataAccess.UpdateDepartmentAsync - Department.ParentId: {department.ParentId}");
                System.Diagnostics.Debug.WriteLine($"DepartmentDataAccess.UpdateDepartmentAsync - Department.Id: {department.Id}");

                using var context = CreateContext();
                
                // Debug: Kiểm tra tất cả departments với cùng CompanyId và DepartmentCode
                var duplicateDepartments = context.Departments
                    .Where(d => d.CompanyId == department.CompanyId && d.DepartmentCode == department.DepartmentCode)
                    .ToList();
                
                System.Diagnostics.Debug.WriteLine($"Found {duplicateDepartments.Count} departments with CompanyId={department.CompanyId} and DepartmentCode={department.DepartmentCode}");
                foreach (var dup in duplicateDepartments)
                {
                    System.Diagnostics.Debug.WriteLine($"  - Department ID: {dup.Id}, Code: {dup.DepartmentCode}, Name: {dup.DepartmentName}");
                }
                
                // Tìm department hiện tại từ database
                var existingDepartment = context.Departments.FirstOrDefault(d => d.Id == department.Id);
                
                if (existingDepartment != null)
                {
                    System.Diagnostics.Debug.WriteLine("DepartmentDataAccess.UpdateDepartmentAsync - Found existing department, updating properties");
                    
                    // Kiểm tra xem có cần cập nhật DepartmentCode không
                    // Nếu DepartmentCode thay đổi, cần kiểm tra duplicate
                    if (existingDepartment.DepartmentCode != department.DepartmentCode)
                    {
                        System.Diagnostics.Debug.WriteLine($"DepartmentCode changed from '{existingDepartment.DepartmentCode}' to '{department.DepartmentCode}'");
                        
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
                    
                    // Sử dụng approach khác - tạo DataContext mới và attach entity
                    using var updateContext = CreateContext();
                    
                    // Attach entity vào context mới
                    updateContext.Departments.Attach(department);
                    
                    // Đánh dấu entity là modified
                    updateContext.Refresh(RefreshMode.KeepCurrentValues, department);
                    
                    System.Diagnostics.Debug.WriteLine($"DepartmentDataAccess.UpdateDepartmentAsync - Attached department with BranchId: {department.BranchId}");
                    System.Diagnostics.Debug.WriteLine($"DepartmentDataAccess.UpdateDepartmentAsync - Attached department with ParentId: {department.ParentId}");
                    
                    // Submit changes
                    await Task.Run(() => updateContext.SubmitChanges());
                    
                    System.Diagnostics.Debug.WriteLine("DepartmentDataAccess.UpdateDepartmentAsync - LINQ to SQL UPDATE completed successfully");
                    
                    // Trả về department đã được cập nhật
                    return department;
                }
                else
                {
                    throw new DataAccessException($"Không tìm thấy department với ID {department.Id} để cập nhật");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DepartmentDataAccess.UpdateDepartmentAsync - Error: {ex.Message}");
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
                using var context = CreateContext();
                
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

                return Task.FromResult(base.DeleteById(id));
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                
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
                
                return true;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi xóa departments: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                Logger?.LogError($"Lỗi khi xóa departments: {ex.Message}", ex);
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
            try
            {
                using var context = CreateContext();
                
                var departments = context.Departments.Where(d => ids.Contains(d.Id)).ToList();
                if (!departments.Any())
                {
                    throw new DataAccessException("Không tìm thấy departments cần xóa");
                }

                // Kiểm tra constraints trước khi xóa
                foreach (var department in departments)
                {
                    var childDepartments = context.Departments.Where(d => d.ParentId == department.Id).ToList();
                    if (childDepartments.Any())
                    {
                        throw new DataAccessException($"Không thể xóa department '{department.DepartmentName}' vì còn {childDepartments.Count} department con");
                    }

                    var employees = context.Employees.Where(e => e.DepartmentId == department.Id).ToList();
                    if (employees.Any())
                    {
                        throw new DataAccessException($"Không thể xóa department '{department.DepartmentName}' vì còn {employees.Count} employee thuộc department này");
                    }
                }

                // Xóa từng department
                foreach (var id in ids)
                {
                    base.DeleteById(id);
                }

                return await Task.FromResult(true);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa departments: {ex.Message}", ex);
            }
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
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Department>(d => d.CompanyBranch);
                loadOptions.LoadWith<Department>(d => d.Company);
                // Không load Department1 để tránh circular reference
                context.LoadOptions = loadOptions;

                var departments = context.Departments
                    .Where(d => d.CompanyId == companyId)
                    .OrderBy(d => d.DepartmentName)
                    .ToList();

                return departments;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy departments theo company ID {companyId}: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Department>(d => d.CompanyBranch);
                loadOptions.LoadWith<Department>(d => d.Company);
                // Không load Department1 để tránh circular reference
                context.LoadOptions = loadOptions;

                var departments = context.Departments
                    .Where(d => d.BranchId == branchId)
                    .OrderBy(d => d.DepartmentName)
                    .ToList();

                return departments;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy departments theo branch ID {branchId}: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Department>(d => d.CompanyBranch);
                loadOptions.LoadWith<Department>(d => d.Company);
                // Không load Department1 để tránh circular reference
                context.LoadOptions = loadOptions;

                var departments = context.Departments
                    .Where(d => d.ParentId == parentId)
                    .OrderBy(d => d.DepartmentName)
                    .ToList();

                return departments;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy departments theo parent ID {parentId}: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();

                // Sử dụng DataLoadOptions để include relationships
                var loadOptions = new DataLoadOptions();
                loadOptions.LoadWith<Department>(d => d.CompanyBranch);
                loadOptions.LoadWith<Department>(d => d.Company);
                // Không load Department1 để tránh circular reference
                context.LoadOptions = loadOptions;

                var departments = context.Departments
                    .Where(d => d.ParentId == null)
                    .OrderBy(d => d.DepartmentName)
                    .ToList();

                return departments;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi lấy root departments: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
                using var context = CreateContext();
                
                // Debug: Kiểm tra tất cả departments trong database
                var allDepartments = context.Departments.ToList();
                Logger?.LogInfo($"All departments in DB: {string.Join(", ", allDepartments.Select(d => $"Id={d.Id}, Code='{d.DepartmentCode}'"))}");
                
                var query = context.Departments.Where(d => d.DepartmentCode == departmentCode);
                if (excludeId.HasValue)
                {
                    query = query.Where(d => d.Id != excludeId.Value);
                }
                
                // Debug: Lấy tất cả departments có cùng code để kiểm tra
                var allMatchingDepartments = query.ToList();
                Logger?.LogInfo($"All matching departments: {string.Join(", ", allMatchingDepartments.Select(d => $"Id={d.Id}, Code='{d.DepartmentCode}'"))}");
                
                var result = query.Any();
                
                // Debug logging
                Logger?.LogInfo($"IsDepartmentCodeExists: Code='{departmentCode}', ExcludeId={excludeId}, Result={result}");
                
                return result;
            }
            catch (SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi kiểm tra department code: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
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
}
