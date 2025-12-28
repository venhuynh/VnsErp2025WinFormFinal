using Dal.DataContext;
using DTO.VersionAndUserManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal.DtoConverter
{

    /// <summary>
    /// Extension methods cho ApplicationUser entities và DTOs.
    /// Cung cấp conversion, transformation, và utility methods.
    /// </summary>
    public static class ApplicationUserConverter
    {
        #region ========== CONVERSION METHODS ==========

        /// <summary>
        /// Convert ApplicationUser entity to ApplicationUserDto.
        /// </summary>
        /// <param name="entity">ApplicationUser entity</param>
        /// <param name="employeeCode">Mã nhân viên (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="employeeFullName">Họ tên nhân viên (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="departmentName">Tên phòng ban (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="positionName">Tên vị trí (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <returns>ApplicationUserDto</returns>
        public static ApplicationUserDto ToDto(this ApplicationUser entity, string employeeCode = null, string employeeFullName = null, string departmentName = null, string positionName = null)
        {
            if (entity == null)
                return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.Employee, entity.Employee.Department, entity.Employee.Position)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            return new ApplicationUserDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                HashPassword = entity.HashPassword,
                Active = entity.Active,
                EmployeeId = entity.EmployeeId,
                EmployeeCode = employeeCode,
                EmployeeFullName = employeeFullName,
                DepartmentName = departmentName,
                PositionName = positionName
            };
        }

        /// <summary>
        /// Convert ApplicationUserDto to ApplicationUser entity.
        /// </summary>
        /// <param name="dto">ApplicationUserDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>ApplicationUser entity</returns>
        public static ApplicationUser ToEntity(this ApplicationUserDto dto, ApplicationUser existingEntity = null)
        {
            if (dto == null)
                return null;

            ApplicationUser entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new ApplicationUser();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            // Map properties
            entity.UserName = dto.UserName;
            entity.HashPassword = dto.HashPassword;
            entity.Active = dto.Active;
            entity.EmployeeId = dto.EmployeeId;

            return entity;
        }

        /// <summary>
        /// Convert collection of ApplicationUser entities to ApplicationUserDto list.
        /// </summary>
        /// <param name="entities">Collection of ApplicationUser entities</param>
        /// <param name="employeeDict">Dictionary chứa thông tin Employee (key: EmployeeId, value: (EmployeeCode, EmployeeFullName, DepartmentName, PositionName))</param>
        /// <returns>List of ApplicationUserDto</returns>
        public static List<ApplicationUserDto> ToDtos(this IEnumerable<ApplicationUser> entities, Dictionary<Guid, (string EmployeeCode, string EmployeeFullName, string DepartmentName, string PositionName)> employeeDict = null)
        {
            if (entities == null)
                return new List<ApplicationUserDto>();

            return entities.Select(entity =>
            {
                if (entity.EmployeeId.HasValue && employeeDict != null && employeeDict.TryGetValue(entity.EmployeeId.Value, out var employeeInfo))
                {
                    return entity.ToDto(employeeInfo.EmployeeCode, employeeInfo.EmployeeFullName, employeeInfo.DepartmentName, employeeInfo.PositionName);
                }
                return entity.ToDto();
            }).ToList();
        }

        /// <summary>
        /// Convert collection of ApplicationUserDto to ApplicationUser entities list.
        /// </summary>
        /// <param name="dtos">Collection of ApplicationUserDto</param>
        /// <returns>List of ApplicationUser entities</returns>
        public static List<ApplicationUser> ToEntities(this IEnumerable<ApplicationUserDto> dtos)
        {
            if (dtos == null)
                return new List<ApplicationUser>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }

        #endregion
    }
}

}
