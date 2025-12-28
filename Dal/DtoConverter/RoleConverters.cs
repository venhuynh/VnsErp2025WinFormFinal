using Dal.DataContext;
using DTO.VersionAndUserManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa Role entity và RoleDto
    /// </summary>
    public static class RoleConverters
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi Role entity thành RoleDto
        /// </summary>
        /// <param name="entity">Role entity</param>
        /// <param name="createdByName">Tên người tạo (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <param name="modifiedByName">Tên người cập nhật (tùy chọn, để tránh DataContext disposed errors)</param>
        /// <returns>RoleDto</returns>
        public static RoleDto ToDto(this Role entity, string createdByName = null, string modifiedByName = null)
        {
            if (entity == null) return null;

            var dto = new RoleDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsSystemRole = entity.IsSystemRole,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy,
                CreatedByName = createdByName,
                ModifiedByName = modifiedByName
            };

            // Đếm số lượng UserRole (nếu có navigation property)
            try
            {
                dto.UserCount = entity.UserRoles?.Count(ur => ur.IsActive) ?? 0;
            }
            catch
            {
                dto.UserCount = 0;
            }

            // Đếm số lượng RolePermission (nếu có navigation property)
            try
            {
                dto.PermissionCount = entity.RolePermissions?.Count(rp => rp.IsGranted) ?? 0;
            }
            catch
            {
                dto.PermissionCount = 0;
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Role entities thành danh sách RoleDto
        /// </summary>
        /// <param name="entities">Danh sách Role entities</param>
        /// <returns>Danh sách RoleDto</returns>
        public static List<RoleDto> ToDtos(this IEnumerable<Role> entities)
        {
            if (entities == null) return new List<RoleDto>();

            return entities.Select(entity => entity.ToDto()).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi RoleDto thành Role entity
        /// </summary>
        /// <param name="dto">RoleDto</param>
        /// <param name="destination">Entity đích để cập nhật (nếu null thì tạo mới)</param>
        /// <returns>Role entity</returns>
        public static Role ToEntity(this RoleDto dto, Role destination = null)
        {
            if (dto == null) return null;

            if (destination == null)
            {
                // Tạo mới
                return new Role
                {
                    Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                    Name = dto.Name,
                    Description = dto.Description,
                    IsSystemRole = dto.IsSystemRole,
                    IsActive = dto.IsActive,
                    CreatedDate = dto.CreatedDate ?? DateTime.Now,
                    CreatedBy = dto.CreatedBy,
                    ModifiedDate = DateTime.Now,
                    ModifiedBy = dto.ModifiedBy
                };
            }
            else
            {
                // Cập nhật
                destination.Name = dto.Name;
                destination.Description = dto.Description;
                destination.IsSystemRole = dto.IsSystemRole;
                destination.IsActive = dto.IsActive;
                destination.ModifiedDate = DateTime.Now;
                destination.ModifiedBy = dto.ModifiedBy;
                return destination;
            }
        }

        #endregion
    }
}

