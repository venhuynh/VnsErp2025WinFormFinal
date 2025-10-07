using System;
using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.Customer.Dto;

namespace MasterData.Customer.Converters
{
    /// <summary>
    /// Converter cho BusinessPartner Entity và DTO
    /// </summary>
    public static class BusinessPartnerConverters
    {
        /// <summary>
        /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerListDto
        /// </summary>
        /// <param name="entity">BusinessPartner Entity</param>
        /// <returns>BusinessPartnerListDto</returns>
        public static BusinessPartnerListDto ToListDto(this BusinessPartner entity)
        {
            if (entity == null) return null;

            return new BusinessPartnerListDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
                PartnerTypeName = ResolvePartnerTypeName(entity.PartnerType),
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                City = entity.City,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        /// <summary>
        /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerListDto với tên loại đối tác
        /// </summary>
        /// <param name="entity">BusinessPartner Entity</param>
        /// <param name="partnerTypeNameResolver">Function để resolve tên loại đối tác</param>
        /// <returns>BusinessPartnerListDto</returns>
        public static BusinessPartnerListDto ToListDto(this BusinessPartner entity, Func<int, string> partnerTypeNameResolver)
        {
            if (entity == null) return null;

            return new BusinessPartnerListDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
                PartnerTypeName = partnerTypeNameResolver?.Invoke(entity.PartnerType) ?? ResolvePartnerTypeName(entity.PartnerType),
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                City = entity.City,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartner Entity sang danh sách BusinessPartnerListDto
        /// </summary>
        /// <param name="entities">Danh sách BusinessPartner Entity</param>
        /// <returns>Danh sách BusinessPartnerListDto</returns>
        public static IEnumerable<BusinessPartnerListDto> ToBusinessPartnerListDtos(this IEnumerable<BusinessPartner> entities)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerListDto>();

            return entities.Select(entity => entity.ToListDto());
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartner Entity sang danh sách BusinessPartnerListDto với resolver
        /// </summary>
        /// <param name="entities">Danh sách BusinessPartner Entity</param>
        /// <param name="partnerTypeNameResolver">Function để resolve tên loại đối tác</param>
        /// <returns>Danh sách BusinessPartnerListDto</returns>
        public static IEnumerable<BusinessPartnerListDto> ToBusinessPartnerListDtos(this IEnumerable<BusinessPartner> entities, Func<int, string> partnerTypeNameResolver)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerListDto>();

            return entities.Select(entity => entity.ToListDto(partnerTypeNameResolver));
        }

        /// <summary>
        /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerDetailDto
        /// </summary>
        /// <param name="entity">BusinessPartner Entity</param>
        /// <returns>BusinessPartnerDetailDto</returns>
        public static BusinessPartnerDetailDto ToDetailDto(this BusinessPartner entity)
        {
            if (entity == null) return null;

            return new BusinessPartnerDetailDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
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

        /// <summary>
        /// Chuyển đổi BusinessPartnerDetailDto sang BusinessPartner Entity
        /// </summary>
        /// <param name="dto">BusinessPartnerDetailDto</param>
        /// <param name="existingEntity">Entity hiện tại (cho update)</param>
        /// <returns>BusinessPartner Entity</returns>
        public static BusinessPartner ToEntity(this BusinessPartnerDetailDto dto, BusinessPartner existingEntity = null)
        {
            if (dto == null) return null;

            var entity = existingEntity ?? new BusinessPartner();
            
            // Chỉ set ID nếu là entity mới
            if (existingEntity == null && dto.Id != Guid.Empty)
            {
                entity.Id = dto.Id;
            }
            
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
            entity.CreatedDate = dto.CreatedDate;
            entity.UpdatedDate = dto.UpdatedDate;

            return entity;
        }

        /// <summary>
        /// Resolve tên loại đối tác từ PartnerType
        /// </summary>
        /// <param name="partnerType">PartnerType (int)</param>
        /// <returns>Tên loại đối tác</returns>
        private static string ResolvePartnerTypeName(int partnerType)
        {
            return partnerType switch
            {
                1 => "Khách hàng",
                2 => "Nhà cung cấp", 
                3 => "Cả hai",
                _ => "Không xác định"
            };
        }
    }
}