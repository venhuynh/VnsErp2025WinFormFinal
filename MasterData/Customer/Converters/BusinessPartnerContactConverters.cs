using System;
using Dal.DataContext;
using MasterData.Customer.Dto;

namespace MasterData.Customer.Converters
{
    public static class BusinessPartnerContactConverters
    {
        public static BusinessPartnerContactDto ToDto(this BusinessPartnerContact entity, string partnerName = null)
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
                IsPrimary = entity.IsPrimary,
                PartnerName = partnerName
            };
        }

        public static BusinessPartnerContact ToEntity(this BusinessPartnerContactDto dto, BusinessPartnerContact destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new BusinessPartnerContact();
            if (dto.Id != Guid.Empty) entity.Id = dto.Id;
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


