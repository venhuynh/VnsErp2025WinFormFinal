using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.ProductService.Dto;
using AttributeEntity = Dal.DataContext.Attribute;
using AttributeValueEntity = Dal.DataContext.AttributeValue;

namespace MasterData.ProductService.Converters
{
    /// <summary>
    /// Converter giữa Attribute/AttributeValue/VariantAttribute (Dal) và DTO tương ứng
    /// </summary>
    public static class AttributeConverters
    {
        #region Attribute -> AttributeDto
        public static AttributeDto ToDto(this AttributeEntity entity)
        {
            if (entity == null) return null;
            return new AttributeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                DataType = entity.DataType,
                Description = entity.Description,
                AttributeValues = entity.AttributeValues?.Select(v => v.ToDto(entity.Name)).ToList()
            };
        }

        public static List<AttributeDto> ToDtoList(this IEnumerable<AttributeEntity> entities)
        {
            if (entities == null) return new List<AttributeDto>();
            return entities.Select(e => e.ToDto()).ToList();
        }

        public static AttributeEntity ToEntity(this AttributeDto dto)
        {
            if (dto == null) return null;
            return new AttributeEntity
            {
                Id = dto.Id,
                Name = dto.Name,
                DataType = dto.DataType,
                Description = dto.Description
            };
        }
        #endregion

        #region AttributeValue -> AttributeValueDto
        public static AttributeValueDto ToDto(this AttributeValueEntity entity)
        {
            if (entity == null) return null;
            return new AttributeValueDto
            {
                Id = entity.Id,
                AttributeId = entity.AttributeId,
                Value = entity.Value,
                AttributeName = entity.Attribute?.Name
            };
        }

        /// <summary>
        /// Chuyển đổi AttributeValue sang DTO với truyền sẵn AttributeName để tránh lazy-load khi DataContext đã dispose
        /// </summary>
        public static AttributeValueDto ToDto(this AttributeValueEntity entity, string attributeName)
        {
            if (entity == null) return null;
            return new AttributeValueDto
            {
                Id = entity.Id,
                AttributeId = entity.AttributeId,
                Value = entity.Value,
                AttributeName = attributeName ?? entity.Attribute?.Name
            };
        }

        public static List<AttributeValueDto> ToDtoList(this IEnumerable<AttributeValueEntity> entities)
        {
            if (entities == null) return new List<AttributeValueDto>();
            return entities.Select(e => e.ToDto()).ToList();
        }

        public static AttributeValueEntity ToEntity(this AttributeValueDto dto)
        {
            if (dto == null) return null;
            return new AttributeValueEntity
            {
                Id = dto.Id,
                AttributeId = dto.AttributeId,
                Value = dto.Value
            };
        }
        #endregion

        #region VariantAttribute -> VariantAttributeDto
        public static VariantAttributeDto ToDto(this VariantAttribute entity)
        {
            if (entity == null) return null;
            return new VariantAttributeDto
            {
                VariantId = entity.VariantId,
                AttributeId = entity.AttributeId,
                AttributeValueId = entity.AttributeValueId,
                AttributeName = entity.Attribute?.Name,
                AttributeValue = entity.AttributeValue?.Value
            };
        }

        public static List<VariantAttributeDto> ToDtoList(this IEnumerable<VariantAttribute> entities)
        {
            if (entities == null) return new List<VariantAttributeDto>();
            return entities.Select(e => e.ToDto()).ToList();
        }

        public static VariantAttribute ToEntity(this VariantAttributeDto dto)
        {
            if (dto == null) return null;
            return new VariantAttribute
            {
                VariantId = dto.VariantId,
                AttributeId = dto.AttributeId,
                AttributeValueId = dto.AttributeValueId
            };
        }
        #endregion
    }
}

