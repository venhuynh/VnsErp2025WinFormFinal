using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.Dto;

namespace MasterData.Converters
{
    public static class BusinessPartnerContactConverters
    {
        public static BusinessPartnerContactDto ToDto(this BusinessPartnerContact entity)
        {
            if (entity == null) return null;
            return new BusinessPartnerContactDto
            {
                Id = entity.Id,
                PartnerId = entity.PartnerId,
                FullName = entity.FullName,
                Position = entity.Position,
                Phone = entity.Phone,
                Email = entity.Email,
                IsPrimary = entity.IsPrimary
            };
        }

        public static IEnumerable<BusinessPartnerContactDto> ToDtos(this IEnumerable<BusinessPartnerContact> entities)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerContactDto>();
            return entities.Select(e => e.ToDto());
        }

        public static BusinessPartnerContact ToEntity(this BusinessPartnerContactDto dto, BusinessPartnerContact destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new BusinessPartnerContact();

            entity.Id = dto.Id == Guid.Empty ? entity.Id : dto.Id;
            entity.PartnerId = dto.PartnerId;
            entity.FullName = dto.FullName;
            entity.Position = dto.Position;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.IsPrimary = dto.IsPrimary;

            return entity;
        }
    }
}


