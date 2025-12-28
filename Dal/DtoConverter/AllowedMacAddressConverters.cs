using Dal.DataContext;
using DTO.VersionAndUserManagementDto;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa AllowedMacAddress entity và AllowedMacAddressDto
    /// </summary>
    public static class AllowedMacAddressConverters
    {
        #region Entity to DTO

        /// <summary>
        /// Chuyển đổi AllowedMacAddress entity thành AllowedMacAddressDto
        /// </summary>
        /// <param name="entity">AllowedMacAddress entity</param>
        /// <returns>AllowedMacAddressDto</returns>
        public static AllowedMacAddressDto ToDto(this AllowedMacAddress entity)
        {
            if (entity == null) return null;

            return new AllowedMacAddressDto
            {
                Id = entity.Id,
                MacAddress = entity.MacAddress,
                ComputerName = entity.ComputerName,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreateDate = entity.CreateDate,
                CreateBy = entity.CreateBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };
        }

        /// <summary>
        /// Chuyển đổi danh sách AllowedMacAddress entities thành danh sách AllowedMacAddressDto
        /// </summary>
        /// <param name="entities">Danh sách AllowedMacAddress entities</param>
        /// <returns>Danh sách AllowedMacAddressDto</returns>
        public static List<AllowedMacAddressDto> ToDtos(this IEnumerable<AllowedMacAddress> entities)
        {
            if (entities == null) return new List<AllowedMacAddressDto>();

            return entities.Select(entity => entity.ToDto()).ToList();
        }

        #endregion

        #region DTO to Entity

        /// <summary>
        /// Chuyển đổi AllowedMacAddressDto thành AllowedMacAddress entity
        /// </summary>
        /// <param name="dto">AllowedMacAddressDto</param>
        /// <param name="destination">Entity đích để cập nhật (nếu null thì tạo mới)</param>
        /// <returns>AllowedMacAddress entity</returns>
        public static AllowedMacAddress ToEntity(this AllowedMacAddressDto dto, AllowedMacAddress destination = null)
        {
            if (dto == null) return null;

            if (destination == null)
            {
                // Tạo mới
                return new AllowedMacAddress
                {
                    Id = dto.Id == Guid.Empty ? Guid.NewGuid() : dto.Id,
                    MacAddress = dto.MacAddress,
                    ComputerName = dto.ComputerName,
                    Description = dto.Description,
                    IsActive = dto.IsActive,
                    CreateDate = dto.CreateDate == default(DateTime) ? DateTime.Now : dto.CreateDate,
                    CreateBy = dto.CreateBy,
                    ModifiedDate = dto.ModifiedDate,
                    ModifiedBy = dto.ModifiedBy
                };
            }
            else
            {
                // Cập nhật
                destination.MacAddress = dto.MacAddress;
                destination.ComputerName = dto.ComputerName;
                destination.Description = dto.Description;
                destination.IsActive = dto.IsActive;
                destination.ModifiedDate = DateTime.Now;
                destination.ModifiedBy = dto.ModifiedBy;
                return destination;
            }
        }

        #endregion
    }
}

