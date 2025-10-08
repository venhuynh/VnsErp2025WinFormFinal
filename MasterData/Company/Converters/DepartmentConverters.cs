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
        /// <param name="companyName">Tên công ty (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="branchName">Tên chi nhánh (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="parentDepartmentName">Tên phòng ban cha (tùy chọn, nếu đã có sẵn)</param>
        /// <returns>DepartmentDto</returns>
        public static DepartmentDto ToDto(this Department entity, string companyName = null, string branchName = null, string parentDepartmentName = null)
        {
            if (entity == null)
                return null;

            // Sử dụng tham số hoặc navigation properties (đã được include)
            
            var finalBranchName = branchName ?? entity.CompanyBranch?.BranchName;
            var finalParentDepartmentName = parentDepartmentName ?? entity.Department1?.DepartmentName ?? "Không xác định";

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
                
                // Navigation properties - sử dụng giá trị mặc định để tránh DataContext disposed
                BranchName = finalBranchName,
                ParentDepartmentName = finalParentDepartmentName,
                EmployeeCount = 0, // Sẽ được tính toán riêng nếu cần
                SubDepartmentCount = 0 // Sẽ được tính toán riêng nếu cần
            };
        }

        /// <summary>
        /// Chuyển đổi từ DepartmentDto sang Department Entity
        /// </summary>
        /// <param name="dto">DepartmentDto</param>
        /// <param name="destination">Department Entity đích (tùy chọn, cho update)</param>
        /// <returns>Department Entity</returns>
        public static Department ToEntity(this DepartmentDto dto, Department destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new Department();
            if (dto.Id != Guid.Empty) entity.Id = dto.Id;
            entity.CompanyId = dto.CompanyId;
            entity.BranchId = dto.BranchId;
            entity.DepartmentCode = dto.DepartmentCode;
            entity.DepartmentName = dto.DepartmentName;
            entity.ParentId = dto.ParentId;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            return entity;
        }


    }
}
