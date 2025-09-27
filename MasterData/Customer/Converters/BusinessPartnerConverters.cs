using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.Dto.Customer;

namespace MasterData.Converters.Customer
{
    public static class BusinessPartnerConverters
    {
        public static BusinessPartnerListDto ToListDto(this BusinessPartner entity, Func<int, string> partnerTypeNameResolver = null)
        {
            if (entity == null) return null;

            return new BusinessPartnerListDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
                PartnerTypeName = ResolvePartnerTypeName(entity.PartnerType, partnerTypeNameResolver),
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                City = entity.City,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        public static IEnumerable<BusinessPartnerListDto> ToListDtos(this IEnumerable<BusinessPartner> entities, Func<int, string> partnerTypeNameResolver = null)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerListDto>();
            return entities.Select(e => e.ToListDto(partnerTypeNameResolver));
        }

        public static BusinessPartnerDetailDto ToDetailDto(this BusinessPartner entity, Func<int, string> partnerTypeNameResolver = null)
        {
            if (entity == null) return null;

            return new BusinessPartnerDetailDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
                PartnerTypeName = ResolvePartnerTypeName(entity.PartnerType, partnerTypeNameResolver),
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                Website = entity.Website,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                ContactPerson = entity.ContactPerson,
                ContactPosition = entity.ContactPosition,
                BankAccount = entity.BankAccount,
                BankName = entity.BankName,
                CreditLimit = entity.CreditLimit,
                PaymentTerm = entity.PaymentTerm,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        public static BusinessPartner ToEntity(this BusinessPartnerDetailDto dto, BusinessPartner destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new BusinessPartner();

            entity.Id = dto.Id == Guid.Empty ? entity.Id : dto.Id;
            entity.PartnerCode = dto.PartnerCode;
            entity.PartnerName = dto.PartnerName;
            entity.PartnerType = dto.PartnerType;
            entity.TaxCode = dto.TaxCode;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.Website = dto.Website;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Country = dto.Country;
            entity.ContactPerson = dto.ContactPerson;
            entity.ContactPosition = dto.ContactPosition;
            entity.BankAccount = dto.BankAccount;
            entity.BankName = dto.BankName;
            entity.CreditLimit = dto.CreditLimit;
            entity.PaymentTerm = dto.PaymentTerm;
            entity.IsActive = dto.IsActive;
            entity.CreatedDate = dto.CreatedDate == default(DateTime) ? entity.CreatedDate : dto.CreatedDate;
            entity.UpdatedDate = dto.UpdatedDate;

            return entity;
        }

        private static string ResolvePartnerTypeName(int partnerType, Func<int, string> resolver)
        {
            if (resolver != null) return resolver(partnerType);

            switch (partnerType)
            {
                case 1: return "Khách hàng";
                case 2: return "Nhà cung cấp";
                case 3: return "Khách hàng & Nhà cung cấp";
                default: return "Không xác định";
            }
        }
    }
}


