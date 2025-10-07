using System;
using System.Linq;
using Dal.DataContext;
using MasterData.Company.Dto;

namespace MasterData.Company.Converters
{
    /// <summary>
    /// Converter giữa Department Entity và DepartmentDto
    /// </summary>
    public static class DepartmentConverters
    {
        /// <summary>
        /// Chuyển đổi từ Department Entity sang DepartmentDto
        /// </summary>
        /// <param name="entity">Department Entity</param>
        /// <returns>DepartmentDto</returns>
        public static DepartmentDto ToDto(this Department entity)
        {
            if (entity == null)
                return null;

            return new DepartmentDto
            {
                Id = entity.Id,
                CompanyId = entity.CompanyId,
                BranchId = entity.BranchId,
                DepartmentCode = entity.DepartmentCode,
                DepartmentName = entity.DepartmentName,
                ParentId = entity.ParentId,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = DateTime.Now, // Default value since entity doesn't have this field
                ModifiedDate = DateTime.Now, // Default value since entity doesn't have this field
                
                // Navigation properties
                CompanyName = entity.Company?.CompanyName,
                BranchName = entity.CompanyBranch?.BranchName,
                ParentDepartmentName = entity.Department1?.DepartmentName,
                EmployeeCount = entity.Employees?.Count ?? 0,
                SubDepartmentCount = entity.Departments?.Count ?? 0
            };
        }

        /// <summary>
        /// Chuyển đổi từ DepartmentDto sang Department Entity
        /// </summary>
        /// <param name="dto">DepartmentDto</param>
        /// <returns>Department Entity</returns>
        public static Department ToEntity(this DepartmentDto dto)
        {
            if (dto == null)
                return null;

            return new Department
            {
                Id = dto.Id,
                CompanyId = dto.CompanyId,
                BranchId = dto.BranchId,
                DepartmentCode = dto.DepartmentCode,
                DepartmentName = dto.DepartmentName,
                ParentId = dto.ParentId,
                Description = dto.Description,
                IsActive = dto.IsActive
            };
        }

        /// <summary>
        /// Cập nhật Department Entity từ DepartmentDto
        /// </summary>
        /// <param name="entity">Department Entity cần cập nhật</param>
        /// <param name="dto">DepartmentDto chứa dữ liệu mới</param>
        public static void UpdateFromDto(this Department entity, DepartmentDto dto)
        {
            if (entity == null || dto == null)
                return;

            entity.CompanyId = dto.CompanyId;
            entity.BranchId = dto.BranchId;
            entity.DepartmentCode = dto.DepartmentCode;
            entity.DepartmentName = dto.DepartmentName;
            entity.ParentId = dto.ParentId;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
        }

        /// <summary>
        /// Chuyển đổi danh sách Department Entity sang danh sách DepartmentDto
        /// </summary>
        /// <param name="entities">Danh sách Department Entity</param>
        /// <returns>Danh sách DepartmentDto</returns>
        public static System.Collections.Generic.List<DepartmentDto> ToDtoList(this System.Collections.Generic.IEnumerable<Department> entities)
        {
            if (entities == null)
                return new System.Collections.Generic.List<DepartmentDto>();

            return entities.Select(e => e.ToDto()).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách DepartmentDto sang danh sách Department Entity
        /// </summary>
        /// <param name="dtos">Danh sách DepartmentDto</param>
        /// <returns>Danh sách Department Entity</returns>
        public static System.Collections.Generic.List<Department> ToEntityList(this System.Collections.Generic.IEnumerable<DepartmentDto> dtos)
        {
            if (dtos == null)
                return new System.Collections.Generic.List<Department>();

            return dtos.Select(d => d.ToEntity()).ToList();
        }

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Tạo DepartmentDto mới với thông tin cơ bản
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <param name="branchId">ID chi nhánh (tùy chọn)</param>
        /// <param name="parentId">ID phòng ban cha (tùy chọn)</param>
        /// <returns>DepartmentDto mới</returns>
        public static DepartmentDto CreateNew(Guid companyId, Guid? branchId = null, Guid? parentId = null)
        {
            return new DepartmentDto
            {
                Id = Guid.NewGuid(),
                CompanyId = companyId,
                BranchId = branchId,
                ParentId = parentId,
                IsActive = true,
                CreatedDate = DateTime.Now
            };
        }

        /// <summary>
        /// Tạo DepartmentDto cho phòng ban cấp cao (không có phòng ban cha)
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <param name="branchId">ID chi nhánh (tùy chọn)</param>
        /// <returns>DepartmentDto cho phòng ban cấp cao</returns>
        public static DepartmentDto CreateTopLevel(Guid companyId, Guid? branchId = null)
        {
            return CreateNew(companyId, branchId, null);
        }

        /// <summary>
        /// Tạo DepartmentDto cho phòng ban con
        /// </summary>
        /// <param name="companyId">ID công ty</param>
        /// <param name="parentId">ID phòng ban cha</param>
        /// <param name="branchId">ID chi nhánh (tùy chọn)</param>
        /// <returns>DepartmentDto cho phòng ban con</returns>
        public static DepartmentDto CreateSubDepartment(Guid companyId, Guid parentId, Guid? branchId = null)
        {
            return CreateNew(companyId, branchId, parentId);
        }

        /// <summary>
        /// Kiểm tra DepartmentDto có hợp lệ không
        /// </summary>
        /// <param name="dto">DepartmentDto cần kiểm tra</param>
        /// <returns>True nếu hợp lệ, False nếu không</returns>
        public static bool IsValid(this DepartmentDto dto)
        {
            if (dto == null)
                return false;

            return !string.IsNullOrWhiteSpace(dto.DepartmentCode) &&
                   !string.IsNullOrWhiteSpace(dto.DepartmentName) &&
                   dto.CompanyId != Guid.Empty;
        }

        /// <summary>
        /// Lấy thông tin phân cấp của DepartmentDto
        /// </summary>
        /// <param name="dto">DepartmentDto</param>
        /// <returns>Thông tin phân cấp</returns>
        public static string GetHierarchyInfo(this DepartmentDto dto)
        {
            if (dto == null)
                return string.Empty;

            if (dto.HasParent)
                return $"Phòng ban con của {dto.ParentDepartmentName}";
            else
                return "Phòng ban cấp cao";
        }

        #endregion
    }
}
