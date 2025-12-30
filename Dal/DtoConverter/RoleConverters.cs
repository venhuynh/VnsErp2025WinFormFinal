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

        /// <summary>
        /// Chuyển đổi danh sách Role entities thành danh sách RoleDto (alias cho ToDtos)
        /// </summary>
        /// <param name="entities">Danh sách Role entities</param>
        /// <returns>Danh sách RoleDto</returns>
        public static List<RoleDto> ToDtoList(this IEnumerable<Role> entities)
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
                // Tạo mới entity
                var entity = new Role
                {
                    // ID: Sử dụng dto.Id nếu không rỗng, ngược lại tạo mới
                    Id = dto.Id != Guid.Empty ? dto.Id : Guid.NewGuid(),

                    // Thông tin cơ bản
                    Name = dto.Name,
                    Description = dto.Description,
                    IsSystemRole = dto.IsSystemRole,
                    IsActive = dto.IsActive,

                    // Thông tin hệ thống - Create
                    CreatedDate = dto.CreatedDate ?? DateTime.Now,
                    CreatedBy = dto.CreatedBy,

                    // Thông tin hệ thống - Modified (null cho entity mới)
                    ModifiedDate = null,
                    ModifiedBy = null
                };

                return entity;
            }
            else
            {
                // Cập nhật entity hiện có
                // Không thay đổi Id và CreatedDate, CreatedBy (thông tin audit tạo)
                // Không thay đổi IsSystemRole vì đây là property hệ thống
                // IsSystemRole chỉ được set khi tạo mới, không được thay đổi khi update

                destination.Name = dto.Name;
                destination.Description = dto.Description;
                destination.IsActive = dto.IsActive;

                // ModifiedDate và ModifiedBy sẽ được set bởi repository layer
                // Không set ở đây để tránh duplicate logic

                return destination;
            }
        }

        /// <summary>
        /// Chuyển đổi danh sách RoleDto sang danh sách Role entities
        /// </summary>
        /// <param name="dtos">Danh sách RoleDto</param>
        /// <returns>Danh sách Role entities</returns>
        public static List<Role> ToEntityList(this IEnumerable<RoleDto> dtos)
        {
            if (dtos == null) return new List<Role>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        /// <summary>
        /// Chuyển đổi danh sách RoleDto sang danh sách Role entities (alias cho ToEntityList)
        /// </summary>
        /// <param name="dtos">Danh sách RoleDto</param>
        /// <returns>Danh sách Role entities</returns>
        public static List<Role> ToEntities(this IEnumerable<RoleDto> dtos)
        {
            if (dtos == null) return new List<Role>();
            return dtos.Select(d => d.ToEntity()).Where(e => e != null).ToList();
        }

        #endregion
    }
}

