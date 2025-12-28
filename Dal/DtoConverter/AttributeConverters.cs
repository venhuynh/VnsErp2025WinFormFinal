using DTO.MasterData.ProductService;
using System;
using System.Collections.Generic;
using System.Linq;
using Attribute = Dal.DataContext.Attribute;

namespace Dal.DtoConverter
{
    /// <summary>
    /// Converter giữa Attribute/AttributeValue/VariantAttribute (Dal) và DTO tương ứng
    /// </summary>
    public static class AttributeConverters
    {
        #region Attribute -> AttributeDto

        /// <summary>
        /// Chuyển đổi Attribute Entity sang AttributeDto
        /// </summary>
        /// <param name="entity">Attribute Entity</param>
        /// <returns>AttributeDto</returns>
        public static AttributeDto ToDto(this Attribute entity)
        {
            if (entity == null) return null;

            var dto = new AttributeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                DataType = entity.DataType,
                Description = entity.Description
            };

            // Load AttributeValues từ navigation property trước khi context bị dispose
            try
            {
                if (entity.AttributeValues != null)
                {
                    dto.AttributeValues = entity.AttributeValues
                        .Select(v => AttributeValueConverters.ToDto(v, entity))
                        .ToList();
                }
                else
                {
                    dto.AttributeValues = new List<AttributeValueDto>();
                }
            }
            catch
            {
                // Navigation property chưa được load hoặc đã bị dispose
                dto.AttributeValues = new List<AttributeValueDto>();
            }

            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách Attribute Entity sang danh sách AttributeDto
        /// </summary>
        /// <param name="entities">Danh sách Attribute Entity</param>
        /// <returns>Danh sách AttributeDto</returns>
        public static List<AttributeDto> ToDtoList(this IEnumerable<Attribute> entities)
        {
            if (entities == null) return new List<AttributeDto>();
            return entities.Select(e => e.ToDto()).ToList();
        }

        #endregion

        #region AttributeDto -> Attribute

        /// <summary>
        /// Chuyển đổi AttributeDto sang Attribute Entity
        /// </summary>
        /// <param name="dto">AttributeDto</param>
        /// <param name="existingEntity">Entity hiện tại (cho update)</param>
        /// <returns>Attribute Entity</returns>
        public static Attribute ToEntity(this AttributeDto dto, Attribute existingEntity = null)
        {
            if (dto == null) return null;

            var entity = existingEntity ?? new Attribute();

            // Chỉ set ID nếu là entity đã tồn tại (edit mode)
            // Khi tạo mới (existingEntity == null), không set Id từ dto để đảm bảo Id = Guid.Empty
            if (existingEntity != null)
            {
                // Edit mode: giữ nguyên Id của existing entity
                // Không cần set lại vì entity đã là existingEntity
            }
            else
            {
                // Create mode: đảm bảo Id = Guid.Empty (default của new Attribute())
                entity.Id = Guid.Empty;
            }

            entity.Name = dto.Name;
            entity.DataType = dto.DataType;
            entity.Description = dto.Description;

            // Note: AttributeValues không được set ở đây vì chúng được quản lý riêng
            // thông qua AttributeValueRepository

            return entity;
        }

        #endregion
    }
}
