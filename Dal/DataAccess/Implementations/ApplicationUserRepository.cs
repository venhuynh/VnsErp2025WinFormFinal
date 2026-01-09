using Dal.DataAccess.Interfaces;
using Dal.DataContext;
using Dal.DtoConverter;
using Dal.Exceptions;
using DTO.VersionAndUserManagementDto;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using Dal.DtoConverter.VersionAndUserManagement;

namespace Dal.DataAccess.Implementations
{
    /// <summary>
    /// Data Access cho thực thể ApplicationUser (LINQ to SQL trên DataContext).
    /// Cung cấp các truy vấn/biến đổi phổ biến: lấy theo UserName, active/inactive, tìm kiếm, đổi mật khẩu, kích hoạt/vô hiệu.
    /// </summary>
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        #region Private Fields

        /// <summary>
        /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
        /// </summary>
        private readonly string _connectionString;

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo một instance mới của class ApplicationUserRepository.
        /// </summary>
        /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
        /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
        public ApplicationUserRepository(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
            var logger = LoggerFactory.CreateLogger(LogCategory.DAL);
            logger.Info("ApplicationUserRepository được khởi tạo với connection string");
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Tạo DataContext mới cho mỗi operation để tránh cache issue
        /// </summary>
        /// <returns>DataContext mới</returns>
        private VnsErp2025DataContext CreateNewContext()
        {
            var context = new VnsErp2025DataContext(_connectionString);

            // Configure eager loading cho navigation properties
            var loadOptions = new DataLoadOptions();
            // Load Employee và các navigation properties liên quan
            loadOptions.LoadWith<ApplicationUser>(u => u.Employee);
            loadOptions.LoadWith<Employee>(e => e.Department);
            loadOptions.LoadWith<Employee>(e => e.Position);
            context.LoadOptions = loadOptions;

            return context;
        }

        /// <summary>
        /// Tạo dictionary chứa thông tin Employee để truyền vào converter
        /// </summary>
        private Dictionary<Guid, (string EmployeeCode, string EmployeeFullName, string DepartmentName, string PositionName)> GetEmployeeDict(VnsErp2025DataContext context, IEnumerable<ApplicationUser> users)
        {
            var employeeDict = new Dictionary<Guid, (string, string, string, string)>();
            
            var employeeIds = users
                .Where(u => u.EmployeeId.HasValue)
                .Select(u => u.EmployeeId.Value)
                .Distinct()
                .ToList();

            if (employeeIds.Any())
            {
                var employees = context.Employees
                    .Where(e => employeeIds.Contains(e.Id))
                    .ToList();

                foreach (var employee in employees)
                {
                    string departmentName = null;
                    string positionName = null;

                    if (employee.DepartmentId.HasValue)
                    {
                        try
                        {
                            var department = context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId.Value);
                            departmentName = department?.DepartmentName;
                        }
                        catch
                        {
                            // Ignore
                        }
                    }

                    if (employee.PositionId.HasValue)
                    {
                        try
                        {
                            var position = context.Positions.FirstOrDefault(p => p.Id == employee.PositionId.Value);
                            positionName = position?.PositionName;
                        }
                        catch
                        {
                            // Ignore
                        }
                    }

                    employeeDict[employee.Id] = (employee.EmployeeCode, employee.FullName, departmentName, positionName);
                }
            }

            return employeeDict;
        }

        #endregion

        #region ========== READ OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả người dùng
        /// </summary>
        public List<ApplicationUserDto> GetAll()
        {
            try
            {
                using var context = CreateNewContext();
                var entities = context.ApplicationUsers
                    .OrderBy(u => u.UserName)
                    .ToList();
                
                // Tạo dictionary chứa thông tin Employee
                var employeeDict = GetEmployeeDict(context, entities);
                
                return entities.ToDtos(employeeDict);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy tất cả người dùng: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy user theo ID
        /// </summary>
        public ApplicationUserDto GetById(Guid id)
        {
            try
            {
                using var context = CreateNewContext();
                var entity = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
                
                if (entity == null)
                    return null;

                // Load Employee info nếu có
                string employeeCode = null;
                string employeeFullName = null;
                string departmentName = null;
                string positionName = null;

                if (entity.EmployeeId.HasValue)
                {
                    try
                    {
                        var employee = context.Employees.FirstOrDefault(e => e.Id == entity.EmployeeId.Value);
                        if (employee != null)
                        {
                            employeeCode = employee.EmployeeCode;
                            employeeFullName = employee.FullName;

                            if (employee.DepartmentId.HasValue)
                            {
                                var department = context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId.Value);
                                departmentName = department?.DepartmentName;
                            }

                            if (employee.PositionId.HasValue)
                            {
                                var position = context.Positions.FirstOrDefault(p => p.Id == employee.PositionId.Value);
                                positionName = position?.PositionName;
                            }
                        }
                    }
                    catch
                    {
                        // Ignore nếu không thể load Employee
                    }
                }

                return entity.ToDto(employeeCode, employeeFullName, departmentName, positionName);
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi lấy user theo ID: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== CREATE OPERATIONS ==========

        /// <summary>
        /// Tạo user mới
        /// </summary>
        public ApplicationUserDto Create(ApplicationUserDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                // Validate UserName
                if (string.IsNullOrWhiteSpace(dto.UserName))
                    throw new ArgumentException("UserName không được để trống", nameof(dto));

                using var context = CreateNewContext();
                
                // Kiểm tra duplicate UserName
                if (context.ApplicationUsers.Any(u => u.UserName == dto.UserName))
                    throw new DataAccessException($"UserName '{dto.UserName}' đã tồn tại");

                // Convert DTO to Entity
                var entity = dto.ToEntity();
                
                if (entity.Id == Guid.Empty)
                    entity.Id = Guid.NewGuid();

                context.ApplicationUsers.InsertOnSubmit(entity);
                context.SubmitChanges();
                
                // Load Employee info để trả về DTO đầy đủ
                string employeeCode = null;
                string employeeFullName = null;
                string departmentName = null;
                string positionName = null;

                if (entity.EmployeeId.HasValue)
                {
                    try
                    {
                        var employee = context.Employees.FirstOrDefault(e => e.Id == entity.EmployeeId.Value);
                        if (employee != null)
                        {
                            employeeCode = employee.EmployeeCode;
                            employeeFullName = employee.FullName;

                            if (employee.DepartmentId.HasValue)
                            {
                                var department = context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId.Value);
                                departmentName = department?.DepartmentName;
                            }

                            if (employee.PositionId.HasValue)
                            {
                                var position = context.Positions.FirstOrDefault(p => p.Id == employee.PositionId.Value);
                                positionName = position?.PositionName;
                            }
                        }
                    }
                    catch
                    {
                        // Ignore nếu không thể load Employee
                    }
                }
                
                return entity.ToDto(employeeCode, employeeFullName, departmentName, positionName);
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Duplicate key
            {
                throw new DataAccessException($"UserName '{dto.UserName}' đã tồn tại trong hệ thống", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi tạo user '{dto.UserName}': {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi tạo user mới: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== UPDATE OPERATIONS ==========

        /// <summary>
        /// Cập nhật user
        /// </summary>
        public ApplicationUserDto Update(ApplicationUserDto dto)
        {
            try
            {
                if (dto == null)
                    throw new ArgumentNullException(nameof(dto));

                using var context = CreateNewContext();
                var existing = context.ApplicationUsers.FirstOrDefault(u => u.Id == dto.Id);

                if (existing == null)
                    throw new DataAccessException($"Không tìm thấy user với ID: {dto.Id}");

                // Validate UserName nếu thay đổi
                if (existing.UserName != dto.UserName && context.ApplicationUsers.Any(u => u.UserName == dto.UserName))
                    throw new DataAccessException($"UserName '{dto.UserName}' đã tồn tại");

                // Convert DTO to Entity (update existing)
                dto.ToEntity(existing);

                context.SubmitChanges();
                
                // Load Employee info để trả về DTO đầy đủ
                string employeeCode = null;
                string employeeFullName = null;
                string departmentName = null;
                string positionName = null;

                if (existing.EmployeeId.HasValue)
                {
                    try
                    {
                        var employee = context.Employees.FirstOrDefault(e => e.Id == existing.EmployeeId.Value);
                        if (employee != null)
                        {
                            employeeCode = employee.EmployeeCode;
                            employeeFullName = employee.FullName;

                            if (employee.DepartmentId.HasValue)
                            {
                                var department = context.Departments.FirstOrDefault(d => d.Id == employee.DepartmentId.Value);
                                departmentName = department?.DepartmentName;
                            }

                            if (employee.PositionId.HasValue)
                            {
                                var position = context.Positions.FirstOrDefault(p => p.Id == employee.PositionId.Value);
                                positionName = position?.PositionName;
                            }
                        }
                    }
                    catch
                    {
                        // Ignore nếu không thể load Employee
                    }
                }
                
                return existing.ToDto(employeeCode, employeeFullName, departmentName, positionName);
            }
            catch (System.Data.SqlClient.SqlException sqlEx) when (sqlEx.Number == 2627) // Duplicate key
            {
                throw new DataAccessException($"UserName '{dto.UserName}' đã tồn tại trong hệ thống", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (System.Data.SqlClient.SqlException sqlEx)
            {
                throw new DataAccessException($"Lỗi SQL khi cập nhật user: {sqlEx.Message}", sqlEx)
                {
                    SqlErrorNumber = sqlEx.Number,
                    ThoiGianLoi = DateTime.Now
                };
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi cập nhật user: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== DELETE OPERATIONS ==========

        /// <summary>
        /// Xóa user
        /// </summary>
        public void Delete(Guid id)
        {
            try
            {
                using var context = CreateNewContext();
                var user = context.ApplicationUsers.FirstOrDefault(u => u.Id == id);

                if (user == null)
                    throw new DataAccessException($"Không tìm thấy user với ID: {id}");

                context.ApplicationUsers.DeleteOnSubmit(user);
                context.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new DataAccessException($"Lỗi khi xóa user: {ex.Message}", ex);
            }
        }

        #endregion
    }
}