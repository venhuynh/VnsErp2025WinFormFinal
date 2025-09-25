using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.Dto;

namespace MasterData.Converters
{
    public static class BusinessPartnerCategoryConverters
    {
        public static BusinessPartnerCategoryDto ToDto(this BusinessPartnerCategory entity)
        {
            if (entity == null) return null;
            return new BusinessPartnerCategoryDto
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                Description = entity.Description
            };
        }

        public static IEnumerable<BusinessPartnerCategoryDto> ToDtos(this IEnumerable<BusinessPartnerCategory> entities)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDto());
        }

        public static BusinessPartnerCategory ToEntity(this BusinessPartnerCategoryDto dto, BusinessPartnerCategory destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new BusinessPartnerCategory();

            entity.Id = dto.Id == Guid.Empty ? entity.Id : dto.Id;
            entity.CategoryName = dto.CategoryName;
            entity.Description = dto.Description;

            return entity;
        }
    }
}


