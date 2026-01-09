using Dal.DataContext;
using DTO.VersionAndUserManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter
{
    
    /// <summary>
    /// Extension methods cho ApplicationVersion entities và DTOs.
    /// Cung cấp conversion, transformation, và utility methods.
    /// </summary>
    public static class ApplicationVersionDtoConverter
    {
        #region ========== CONVERSION METHODS ==========

        /// <summary>
        /// Convert VnsErpApplicationVersion entity to ApplicationVersionDto.
        /// </summary>
        /// <param name="entity">VnsErpApplicationVersion entity</param>
        /// <returns>ApplicationVersionDto</returns>
        public static ApplicationVersionDto ToDto(this VnsErpApplicationVersion entity)
        {
            if (entity == null)
                return null;

            return new ApplicationVersionDto
            {
                Id = entity.Id,
                Version = entity.Version,
                ReleaseDate = entity.ReleaseDate,
                IsActive = entity.IsActive,
                Description = entity.Description,
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy,
                ReleaseNote = entity.ReleaseNote
            };
        }

        /// <summary>
        /// Convert ApplicationVersionDto to VnsErpApplicationVersion entity.
        /// </summary>
        /// <param name="dto">ApplicationVersionDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>VnsErpApplicationVersion entity</returns>
        public static VnsErpApplicationVersion ToEntity(this ApplicationVersionDto dto, VnsErpApplicationVersion existingEntity = null)
        {
            if (dto == null)
                return null;

            VnsErpApplicationVersion entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new VnsErpApplicationVersion();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            // Map properties
            entity.Version = dto.Version;
            entity.ReleaseDate = dto.ReleaseDate;
            entity.IsActive = dto.IsActive;
            entity.Description = dto.Description;
            entity.CreateDate = dto.CreateDate;
            entity.CreateBy = dto.CreateBy;
            entity.ModifiedDate = dto.ModifiedDate;
            entity.ModifiedBy = dto.ModifiedBy;
            entity.ReleaseNote = dto.ReleaseNote;

            return entity;
        }

        /// <summary>
        /// Convert collection of VnsErpApplicationVersion entities to ApplicationVersionDto list.
        /// </summary>
        /// <param name="entities">Collection of VnsErpApplicationVersion entities</param>
        /// <returns>List of ApplicationVersionDto</returns>
        public static List<ApplicationVersionDto> ToDtos(this IEnumerable<VnsErpApplicationVersion> entities)
        {
            if (entities == null)
                return new List<ApplicationVersionDto>();

            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// Convert collection of ApplicationVersionDto to VnsErpApplicationVersion entities list.
        /// </summary>
        /// <param name="dtos">Collection of ApplicationVersionDto</param>
        /// <returns>List of VnsErpApplicationVersion entities</returns>
        public static List<VnsErpApplicationVersion> ToEntities(this IEnumerable<ApplicationVersionDto> dtos)
        {
            if (dtos == null)
                return new List<VnsErpApplicationVersion>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }

        #endregion
    }

}
