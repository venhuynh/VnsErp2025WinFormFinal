using Dal.DataAccess.MasterData.CompanyDal;
using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bll.MasterData.Company
{
    /// <summary>
    /// Business Logic Layer cho Department
    /// Cung cấp các phương thức xử lý nghiệp vụ cho Department
    /// </summary>
    public class DepartmentBll
    {
        #region ========== FIELDS ==========

        private readonly DepartmentDataAccess _departmentDataAccess;

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo DepartmentBll
        /// </summary>
        public DepartmentBll()
        {
            _departmentDataAccess = new DepartmentDataAccess();
        }

        #endregion

        #region ========== CRUD OPERATIONS ==========

        /// <summary>
        /// Lấy tất cả departments (synchronous)
        /// </summary>
        /// <returns>Danh sách Department</returns>
        public List<Department> GetAll()
        {
            try
            {
                return _departmentDataAccess.GetAll();
            }
            catch (Exception ex)
            {
                // Log error và throw exception để UI xử lý
                throw new Exception($"Lỗi khi lấy danh sách departments: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy tất cả departments (async)
        /// </summary>
        /// <returns>Danh sách Department</returns>
        public async Task<List<Department>> GetAllAsync()
        {
            try
            {
                return await _departmentDataAccess.GetAllAsync();
            }
            catch (Exception ex)
            {
                // Log error và throw exception để UI xử lý
                throw new Exception($"Lỗi khi lấy danh sách departments: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy department theo ID (synchronous)
        /// </summary>
        /// <param name="id">ID của department</param>
        /// <returns>Department hoặc null nếu không tìm thấy</returns>
        public Department GetById(Guid id)
        {
            try
            {
                return _departmentDataAccess.GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy department theo ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Lấy department theo ID (async)
        /// </summary>
        /// <param name="id">ID của department</param>
        /// <returns>Department hoặc null nếu không tìm thấy</returns>
        public async Task<Department> GetByIdAsync(Guid id)
        {
            try
            {
                return await _departmentDataAccess.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi lấy department theo ID {id}: {ex.Message}", ex);
            }
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
                // Validation
                await ValidateDepartmentAsync(department);

                return await _departmentDataAccess.CreateAsync(department);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi tạo department: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật department (synchronous)
        /// </summary>
        /// <param name="department">Department cần cập nhật</param>
        /// <returns>Department đã được cập nhật</returns>
        public Department Update(Department department)
        {
            try
            {
                
                return _departmentDataAccess.UpdateDepartment(department);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi cập nhật department: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Cập nhật department (async)
        /// </summary>
        /// <param name="department">Department cần cập nhật</param>
        /// <returns>Department đã được cập nhật</returns>
        public async Task<Department> UpdateAsync(Department department)
        {
            try
            {
                System.Diagnostics.Debug.WriteLine($"DepartmentBll.UpdateAsync - Department.BranchId: {department.BranchId}");
                System.Diagnostics.Debug.WriteLine($"DepartmentBll.UpdateAsync - Department.ParentId: {department.ParentId}");
                System.Diagnostics.Debug.WriteLine($"DepartmentBll.UpdateAsync - Department.Id: {department.Id}");

                // Validation
                await ValidateDepartmentAsync(department, department.Id);

                System.Diagnostics.Debug.WriteLine("DepartmentBll.UpdateAsync - Validation passed, calling UpdateDepartmentAsync");

                return await _departmentDataAccess.UpdateDepartmentAsync(department);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"DepartmentBll.UpdateAsync - Error: {ex.Message}");
                throw new Exception($"Lỗi khi cập nhật department: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Xóa department
        /// </summary>
        /// <param name="id">ID của department cần xóa</param>
        /// <returns>True nếu xóa thành công</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                return await _departmentDataAccess.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa department: {ex.Message}", ex);
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
                return _departmentDataAccess.DeleteMultiple(ids);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa departments: {ex.Message}", ex);
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
                return await _departmentDataAccess.DeleteMultipleAsync(ids);
            }
            catch (Exception ex)
            {
                throw new Exception($"Lỗi khi xóa departments: {ex.Message}", ex);
            }
        }

        #endregion

        #region ========== VALIDATION ==========

        /// <summary>
        /// Validate department trước khi lưu
        /// </summary>
        /// <param name="department">Department cần validate</param>
        /// <param name="excludeId">ID cần loại trừ (cho update)</param>
        private async Task ValidateDepartmentAsync(Department department, Guid? excludeId = null)
        {
            if (department == null)
                throw new ArgumentException("Department không được null");

            if (string.IsNullOrWhiteSpace(department.DepartmentCode))
                throw new ArgumentException("Mã phòng ban không được để trống");

            if (string.IsNullOrWhiteSpace(department.DepartmentName))
                throw new ArgumentException("Tên phòng ban không được để trống");

            if (department.CompanyId == Guid.Empty)
                throw new ArgumentException("Company ID không được để trống");

            // Kiểm tra department code trùng lặp - chỉ khi tạo mới
            if (!excludeId.HasValue) // Chỉ kiểm tra khi tạo mới (excludeId = null)
            {
                var isCodeExists = await _departmentDataAccess.IsDepartmentCodeExistsAsync(department.DepartmentCode, excludeId);
                if (isCodeExists)
                {
                    throw new ArgumentException($"Mã phòng ban '{department.DepartmentCode}' đã tồn tại");
                }
            }

            // Kiểm tra parent department có tồn tại không (nếu có)
            if (department.ParentId.HasValue)
            {
                var parentDepartment = await _departmentDataAccess.GetByIdAsync(department.ParentId.Value);
                if (parentDepartment == null)
                    throw new ArgumentException($"Parent department với ID {department.ParentId.Value} không tồn tại");
            }
        }

        #endregion
    }
}
