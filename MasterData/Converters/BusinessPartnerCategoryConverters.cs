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
                Description = entity.Description,
                PartnerCount = 0 // Sẽ được cập nhật bởi method có đếm
            };
        }

        public static BusinessPartnerCategoryDto ToDtoWithCount(this BusinessPartnerCategory entity, int partnerCount)
        {
            if (entity == null) return null;
            return new BusinessPartnerCategoryDto
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                PartnerCount = partnerCount
            };
        }

        public static IEnumerable<BusinessPartnerCategoryDto> ToDtos(this IEnumerable<BusinessPartnerCategory> entities)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDto());
        }

        public static IEnumerable<BusinessPartnerCategoryDto> ToDtosWithCount(this IEnumerable<BusinessPartnerCategory> entities, 
            Func<Guid, int> partnerCountResolver)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDtoWithCount(partnerCountResolver?.Invoke(e.Id) ?? 0));
        }

        /// <summary>
        /// Chuyển đổi BusinessPartnerCategory sang DTO với đếm số lượng đối tác từ mapping table.
        /// </summary>
        /// <param name="entity">Entity BusinessPartnerCategory</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng đối tác theo CategoryId</param>
        /// <returns>BusinessPartnerCategoryDto với PartnerCount</returns>
        public static BusinessPartnerCategoryDto ToDtoWithMappingCount(this BusinessPartnerCategory entity, 
            Dictionary<Guid, int> mappingCounts)
        {
            if (entity == null) return null;
            var partnerCount = mappingCounts?.ContainsKey(entity.Id) == true ? mappingCounts[entity.Id] : 0;
            return entity.ToDtoWithCount(partnerCount);
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartnerCategory sang DTO với đếm số lượng đối tác.
        /// </summary>
        /// <param name="entities">Danh sách entities</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng đối tác theo CategoryId</param>
        /// <returns>Danh sách BusinessPartnerCategoryDto với PartnerCount</returns>
        public static IEnumerable<BusinessPartnerCategoryDto> ToDtosWithMappingCount(this IEnumerable<BusinessPartnerCategory> entities, 
            Dictionary<Guid, int> mappingCounts)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDtoWithMappingCount(mappingCounts));
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


