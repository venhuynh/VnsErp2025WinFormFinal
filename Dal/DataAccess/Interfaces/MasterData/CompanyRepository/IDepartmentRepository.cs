using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DTO.MasterData.Company;

namespace Dal.DataAccess.Interfaces.MasterData.CompanyRepository
{
    /// <summary>
    /// Data Access Layer cho Department
    /// Cung cấp các phương thức truy cập dữ liệu cho Department
    /// </summary>
    public interface IDepartmentRepository
    {
        #region ========== CRUD OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả departments với thông tin chi nhánh
        /// </summary>
        /// <returns>Danh sách tất cả departments</returns>
        List<DepartmentDto> GetAll();

        /// <summary>
        /// Lấy tất cả departments với thông tin chi nhánh (Async)
        /// </summary>
        /// <returns>Danh sách tất cả departments</returns>
        Task<List<DepartmentDto>> GetAllAsync();

        /// <summary>
        /// Lấy department theo ID với thông tin chi nhánh (synchronous)
        /// </summary>
        /// <param name="id">ID của department</param>
        /// <returns>DepartmentDto hoặc null nếu không tìm thấy</returns>
        DepartmentDto GetById(Guid id);

        /// <summary>
        /// Lấy department theo ID với thông tin chi nhánh (async)
        /// </summary>
        /// <param name="id">ID của department</param>
        /// <returns>DepartmentDto hoặc null nếu không tìm thấy</returns>
        Task<DepartmentDto> GetByIdAsync(Guid id);

        /// <summary>
        /// Tạo mới department
        /// </summary>
        /// <param name="department">DepartmentDto cần tạo</param>
        /// <returns>DepartmentDto đã được tạo</returns>
        Task<DepartmentDto> CreateAsync(DepartmentDto department);

        /// <summary>
        /// Cập nhật department (synchronous)
        /// </summary>
        /// <param name="department">DepartmentDto cần cập nhật</param>
        /// <returns>DepartmentDto đã được cập nhật</returns>
        DepartmentDto UpdateDepartment(DepartmentDto department);

        /// <summary>
        /// Cập nhật department (async) - Sử dụng logic update riêng
        /// </summary>
        /// <param name="department">DepartmentDto cần cập nhật</param>
        /// <returns>DepartmentDto đã được cập nhật</returns>
        Task<DepartmentDto> UpdateDepartmentAsync(DepartmentDto department);

        /// <summary>
        /// Xóa department
        /// </summary>
        /// <param name="id">ID của department cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Xóa nhiều departments (Synchronous)
        /// </summary>
        /// <param name="ids">Danh sách ID của departments cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        bool DeleteMultiple(List<Guid> ids);

        /// <summary>
        /// Xóa nhiều departments (Async)
        /// </summary>
        /// <param name="ids">Danh sách ID của departments cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        Task<bool> DeleteMultipleAsync(List<Guid> ids);

        #endregion

        #region ========== BUSINESS LOGIC ==========

        /// <summary>
        /// Lấy departments theo company ID với thông tin chi nhánh
        /// </summary>
        /// <param name="companyId">ID của company</param>
        /// <returns>Danh sách departments thuộc company</returns>
        List<DepartmentDto> GetByCompanyId(Guid companyId);

        /// <summary>
        /// Lấy departments theo company ID với thông tin chi nhánh (Async)
        /// </summary>
        /// <param name="companyId">ID của company</param>
        /// <returns>Danh sách departments thuộc company</returns>
        Task<List<DepartmentDto>> GetByCompanyIdAsync(Guid companyId);

        /// <summary>
        /// Lấy departments theo branch ID với thông tin chi nhánh
        /// </summary>
        /// <param name="branchId">ID của branch</param>
        /// <returns>Danh sách departments thuộc branch</returns>
        List<DepartmentDto> GetByBranchId(Guid branchId);

        /// <summary>
        /// Lấy departments theo branch ID với thông tin chi nhánh (Async)
        /// </summary>
        /// <param name="branchId">ID của branch</param>
        /// <returns>Danh sách departments thuộc branch</returns>
        Task<List<DepartmentDto>> GetByBranchIdAsync(Guid branchId);

        /// <summary>
        /// Lấy departments theo parent ID với thông tin chi nhánh
        /// </summary>
        /// <param name="parentId">ID của parent department</param>
        /// <returns>Danh sách departments con</returns>
        List<DepartmentDto> GetByParentId(Guid parentId);

        /// <summary>
        /// Lấy departments theo parent ID với thông tin chi nhánh (Async)
        /// </summary>
        /// <param name="parentId">ID của parent department</param>
        /// <returns>Danh sách departments con</returns>
        Task<List<DepartmentDto>> GetByParentIdAsync(Guid parentId);

        /// <summary>
        /// Lấy root departments (không có parent) với thông tin chi nhánh
        /// </summary>
        /// <returns>Danh sách root departments</returns>
        List<DepartmentDto> GetRootDepartments();

        /// <summary>
        /// Lấy root departments (không có parent) với thông tin chi nhánh (Async)
        /// </summary>
        /// <returns>Danh sách root departments</returns>
        Task<List<DepartmentDto>> GetRootDepartmentsAsync();

        /// <summary>
        /// Kiểm tra department code có tồn tại không (synchronous)
        /// </summary>
        /// <param name="departmentCode">Mã department</param>
        /// <param name="excludeId">ID cần loại trừ (cho update)</param>
        /// <returns>True nếu tồn tại</returns>
        bool IsDepartmentCodeExists(string departmentCode, Guid? excludeId = null);

        /// <summary>
        /// Kiểm tra department code có tồn tại không (async)
        /// </summary>
        /// <param name="departmentCode">Mã department</param>
        /// <param name="excludeId">ID cần loại trừ (cho update)</param>
        /// <returns>True nếu tồn tại</returns>
        Task<bool> IsDepartmentCodeExistsAsync(string departmentCode, Guid? excludeId = null);

        #endregion
    }
}