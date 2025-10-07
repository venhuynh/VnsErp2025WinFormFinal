using Dal.BaseDataAccess;
using Dal.DataContext;
using Dal.Exceptions;
using Dal.Logging;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Dal.DataAccess.MasterData.CompanyDal
{
    /// <summary>
    /// Data Access Layer cho Department
    /// Cung cấp các phương thức truy cập dữ liệu cho Department
    /// </summary>
    public class DepartmentDataAccess : BaseDataAccess<Department>
    {
        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo DepartmentDataAccess
        /// </summary>
        /// <param name="logger">Logger (tùy chọn)</param>
        public DepartmentDataAccess(ILogger logger = null) : base(logger)
        {
        }

        /// <summary>
        /// Khởi tạo với connection string
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối</param>
        /// <param name="logger">Logger (tùy chọn)</param>
        public DepartmentDataAccess(string connectionString, ILogger logger = null) : base(connectionString, logger)
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
            return await Task.Run(() => GetAll());
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
                var loadOptions = new System.Data.Linq.DataLoadOptions();
                loadOptions.LoadWith<Department>(d => d.CompanyBranch);
                loadOptions.LoadWith<Department>(d => d.Company);
                // Không load Department1 để tránh circular reference
                context.LoadOptions = loadOptions;

                var department = context.Departments
                    .FirstOrDefault(d => d.Id == id);
                
                return department;
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
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
            catch (System.Data.SqlClient.SqlException sqlEx)
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
        /// Cập nhật department (async)
        /// </summary>
        /// <param name="department">Department cần cập nhật</param>
        /// <returns>Department đã được cập nhật</returns>
        public async Task<Department> UpdateDepartmentAsync(Department department)
        {
            try
            {
                await base.UpdateAsync(department);
                return department;
            }
            catch (Exception ex)
            {
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
        /// Xóa nhiều departments
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
                var loadOptions = new System.Data.Linq.DataLoadOptions();
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
            catch (System.Data.SqlClient.SqlException sqlEx)
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
                var loadOptions = new System.Data.Linq.DataLoadOptions();
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
            catch (System.Data.SqlClient.SqlException sqlEx)
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
                var loadOptions = new System.Data.Linq.DataLoadOptions();
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
            catch (System.Data.SqlClient.SqlException sqlEx)
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
                var loadOptions = new System.Data.Linq.DataLoadOptions();
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
            catch (System.Data.SqlClient.SqlException sqlEx)
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
                var query = context.Departments.Where(d => d.DepartmentCode == departmentCode);
                if (excludeId.HasValue)
                {
                    query = query.Where(d => d.Id != excludeId.Value);
                }
                return query.Any();
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
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
