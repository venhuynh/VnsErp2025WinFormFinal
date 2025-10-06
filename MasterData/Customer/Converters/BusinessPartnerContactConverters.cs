using System;
using Dal.DataContext;
using MasterData.Customer.Dto;

namespace MasterData.Customer.Converters
{
    public static class BusinessPartnerContactConverters
    {
        public static BusinessPartnerContactDto ToDto(this BusinessPartnerContact entity, string siteName = null)
        {
            if (entity == null) return null;
            return new BusinessPartnerContactDto
            {
                Id = entity.Id,
                SiteId = entity.SiteId,
                SiteName = siteName,
                FullName = entity.FullName,
                Position = entity.Position,
                Phone = entity.Phone,
                Email = entity.Email,
                IsPrimary = entity.IsPrimary,
                Avatar = entity.Avatar?.ToArray(),
                IsActive = entity.IsActive
            };
        }

        public static BusinessPartnerContact ToEntity(this BusinessPartnerContactDto dto, BusinessPartnerContact destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new BusinessPartnerContact();
            if (dto.Id != Guid.Empty) entity.Id = dto.Id;
            entity.SiteId = dto.SiteId;
            entity.FullName = dto.FullName;
            entity.Position = dto.Position;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.IsPrimary = dto.IsPrimary;
            entity.Avatar = dto.Avatar != null ? new System.Data.Linq.Binary(dto.Avatar) : null;
            entity.IsActive = dto.IsActive;
            return entity;
        }
    }
}


