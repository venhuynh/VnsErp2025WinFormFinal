using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dal.DataContext;

namespace Dal.DataAccess.Interfaces.MasterData.CompanyRepository;

/// <summary>
/// Data Access Layer cho quản lý nhân viên.
/// Cung cấp các phương thức truy cập dữ liệu cho nhân viên.
/// </summary>
public interface IEmployeeRepository
{
    #region ========== QUẢN LÝ DỮ LIỆU ==========

    /// <summary>
    /// Lấy nhân viên theo ID.
    /// </summary>
    /// <param name="id">ID của nhân viên</param>
    /// <returns>Nhân viên hoặc null nếu không tìm thấy</returns>
    Employee GetById(Guid id);

    /// <summary>
    /// Lấy nhân viên theo mã nhân viên.
    /// </summary>
    /// <param name="employeeCode">Mã nhân viên</param>
    /// <returns>Nhân viên hoặc null nếu không tìm thấy</returns>
    Employee GetByEmployeeCode(string employeeCode);

    /// <summary>
    /// Lấy nhân viên theo mã nhân viên và CompanyId.
    /// </summary>
    /// <param name="employeeCode">Mã nhân viên</param>
    /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
    /// <returns>Nhân viên hoặc null nếu không tìm thấy</returns>
    Employee GetByEmployeeCodeAndCompany(string employeeCode, Guid companyId);

    /// <summary>
    /// Lấy tất cả nhân viên (Async).
    /// </summary>
    /// <returns>Danh sách tất cả nhân viên</returns>
    Task<List<Employee>> GetAllAsync();

    /// <summary>
    /// Lấy tất cả nhân viên (Sync).
    /// </summary>
    /// <returns>Danh sách tất cả nhân viên</returns>
    List<Employee> GetAll();

    /// <summary>
    /// Lấy danh sách nhân viên đang hoạt động (Async).
    /// </summary>
    /// <returns>Danh sách nhân viên đang hoạt động</returns>
    Task<List<Employee>> GetActiveEmployeesAsync();

    /// <summary>
    /// Lấy danh sách nhân viên đang hoạt động (Sync).
    /// </summary>
    /// <returns>Danh sách nhân viên đang hoạt động</returns>
    List<Employee> GetActiveEmployees();

    /// <summary>
    /// Lấy tất cả nhân viên của một công ty.
    /// </summary>
    /// <param name="companyId">ID công ty (nếu Guid.Empty thì lấy từ Company duy nhất)</param>
    /// <returns>Danh sách nhân viên của công ty</returns>
    List<Employee> GetByCompanyId(Guid companyId);

    /// <summary>
    /// Lấy danh sách nhân viên theo chi nhánh.
    /// </summary>
    /// <param name="branchId">ID chi nhánh</param>
    /// <returns>Danh sách nhân viên của chi nhánh</returns>
    List<Employee> GetByBranchId(Guid branchId);

    /// <summary>
    /// Lấy danh sách nhân viên theo phòng ban.
    /// </summary>
    /// <param name="departmentId">ID phòng ban</param>
    /// <returns>Danh sách nhân viên của phòng ban</returns>
    List<Employee> GetByDepartmentId(Guid departmentId);

    /// <summary>
    /// Lấy danh sách nhân viên theo chức vụ.
    /// </summary>
    /// <param name="positionId">ID chức vụ</param>
    /// <returns>Danh sách nhân viên có chức vụ</returns>
    List<Employee> GetByPositionId(Guid positionId);

    #endregion

    #region ========== XỬ LÝ DỮ LIỆU ==========

    /// <summary>
    /// Thêm mới nhân viên.
    /// </summary>
    /// <param name="employee">Nhân viên cần thêm</param>
    /// <returns>ID của nhân viên vừa thêm</returns>
    Guid Insert(Employee employee);

    /// <summary>
    /// Cập nhật nhân viên.
    /// </summary>
    /// <param name="employee">Nhân viên cần cập nhật</param>
    public void Update(Employee employee);

    /// <summary>
    /// Xóa nhân viên.
    /// </summary>
    /// <param name="employee">Nhân viên cần xóa</param>
    void Delete(Employee employee);

    #endregion

    #region ========== KIỂM TRA DỮ LIỆU ==========

    /// <summary>
    /// Kiểm tra mã nhân viên có tồn tại không.
    /// </summary>
    /// <param name="employeeCode">Mã nhân viên cần kiểm tra</param>
    /// <returns>True nếu tồn tại, False nếu không</returns>
    bool IsEmployeeCodeExists(string employeeCode);

    /// <summary>
    /// Kiểm tra mã nhân viên có tồn tại không (trừ bản ghi hiện tại).
    /// </summary>
    /// <param name="employeeCode">Mã nhân viên cần kiểm tra</param>
    /// <param name="excludeId">ID của bản ghi cần loại trừ</param>
    /// <returns>True nếu tồn tại, False nếu không</returns>
    bool IsEmployeeCodeExists(string employeeCode, Guid excludeId);

    #endregion
}
