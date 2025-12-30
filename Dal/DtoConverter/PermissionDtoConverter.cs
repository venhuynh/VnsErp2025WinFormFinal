using Dal.DataContext;
using DTO.VersionAndUserManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter
{

    /// <summary>
    /// Extension methods cho Permission entities và DTOs
    /// </summary>
    public static class PermissionDtoConverter
    {
        /// <summary>
        /// Convert Permission entity to PermissionDto
        /// </summary>
        public static PermissionDto ToDto(this Permission entity)
        {
            if (entity == null)
                return null;

            return new PermissionDto
            {
                Id = entity.Id,
                EntityName = entity.EntityName,
                Action = entity.Action,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        /// <summary>
        /// Convert PermissionDto to Permission entity
        /// </summary>
        public static Permission ToEntity(this PermissionDto dto, Permission existingEntity = null)
        {
            if (dto == null)
                return null;

            Permission entity;
            if (existingEntity != null)
            {
                entity = existingEntity;
            }
            else
            {
                entity = new Permission();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            entity.EntityName = dto.EntityName;
            entity.Action = dto.Action;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;

            return entity;
        }

        /// <summary>
        /// Convert collection of Permission entities to PermissionDto list
        /// </summary>
        public static List<PermissionDto> ToDtos(this IEnumerable<Permission> entities)
        {
            if (entities == null)
                return new List<PermissionDto>();

            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// Convert collection of PermissionDto to Permission entities list
        /// </summary>
        public static List<Permission> ToEntities(this IEnumerable<PermissionDto> dtos)
        {
            if (dtos == null)
                return new List<Permission>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }
    }
}
