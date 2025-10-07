using System.Collections.Generic;
using System.Linq;
using Dal.DataContext;
using MasterData.Customer.Dto;

namespace MasterData.Customer.Converters
{
    /// <summary>
    /// Converter cho BusinessPartnerSite Entity và DTO
    /// </summary>
    public static class BusinessPartnerSiteConverters
    {
        /// <summary>
        /// Chuyển đổi BusinessPartnerSite Entity sang BusinessPartnerSiteListDto
        /// </summary>
        /// <param name="entity">BusinessPartnerSite Entity</param>
        /// <returns>BusinessPartnerSiteListDto</returns>
        public static BusinessPartnerSiteListDto ToListDto(this BusinessPartnerSite entity)
        {
            if (entity == null) return null;

               return new BusinessPartnerSiteListDto
               {
                   Id = entity.Id,
                   PartnerId = entity.PartnerId,
                   SiteCode = entity.SiteCode,
                   PartnerName = entity.BusinessPartner?.PartnerName,
                   SiteName = entity.SiteName,
                   Address = entity.Address,
                   City = entity.City,
                   Province = entity.Province,
                   Country = entity.Country,
                   ContactPerson = entity.ContactPerson,
                   Phone = entity.Phone,
                   Email = entity.Email,
                   IsDefault = entity.IsDefault,
                   IsActive = entity.IsActive,
                   CreatedDate = entity.CreatedDate,
                   UpdatedDate = entity.UpdatedDate
               };
        }

        /// <summary>
        /// Chuyển đổi BusinessPartnerSiteListDto sang BusinessPartnerSite Entity
        /// </summary>
        /// <param name="dto">BusinessPartnerSiteListDto</param>
        /// <returns>BusinessPartnerSite Entity</returns>
        public static BusinessPartnerSite ToEntity(this BusinessPartnerSiteListDto dto)
        {
            if (dto == null) return null;

               return new BusinessPartnerSite
               {
                   Id = dto.Id,
                   PartnerId = dto.PartnerId,
                   SiteCode = dto.SiteCode,
                   SiteName = dto.SiteName,
                   Address = dto.Address,
                   City = dto.City,
                   Province = dto.Province,
                   Country = dto.Country,
                   ContactPerson = dto.ContactPerson,
                   Phone = dto.Phone,
                   Email = dto.Email,
                   IsDefault = dto.IsDefault,
                   IsActive = dto.IsActive,
                   CreatedDate = dto.CreatedDate,
                   UpdatedDate = dto.UpdatedDate
                   // Note: PartnerName không được map ngược vì nó chỉ để hiển thị
               };
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartnerSite Entity sang danh sách BusinessPartnerSiteListDto
        /// </summary>
        /// <param name="entities">Danh sách BusinessPartnerSite Entity</param>
        /// <returns>Danh sách BusinessPartnerSiteListDto</returns>
        public static IEnumerable<BusinessPartnerSiteListDto> ToSiteListDtos(this IEnumerable<BusinessPartnerSite> entities)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerSiteListDto>();

            return entities.Select(entity => entity.ToListDto());
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartnerSiteListDto sang danh sách BusinessPartnerSite Entity
        /// </summary>
        /// <param name="dtos">Danh sách BusinessPartnerSiteListDto</param>
        /// <returns>Danh sách BusinessPartnerSite Entity</returns>
        public static IEnumerable<BusinessPartnerSite> ToEntities(this IEnumerable<BusinessPartnerSiteListDto> dtos)
        {
            if (dtos == null) return Enumerable.Empty<BusinessPartnerSite>();

            return dtos.Select(dto => dto.ToEntity());
        }

        /// <summary>
        /// Chuyển đổi BusinessPartnerSite Entity sang BusinessPartnerSiteDto
        /// </summary>
        /// <param name="entity">BusinessPartnerSite Entity</param>
        /// <returns>BusinessPartnerSiteDto</returns>
        public static BusinessPartnerSiteDto ToSiteDto(this BusinessPartnerSite entity)
        {
            if (entity == null) return null;

            return new BusinessPartnerSiteDto
            {
                Id = entity.Id,
                PartnerId = entity.PartnerId,
                SiteCode = entity.SiteCode,
                SiteName = entity.SiteName,
                Address = entity.Address,
                City = entity.City,
                Province = entity.Province,
                Country = entity.Country,
                ContactPerson = entity.ContactPerson,
                Phone = entity.Phone,
                Email = entity.Email,
                IsDefault = entity.IsDefault,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate
            };
        }

        /// <summary>
        /// Chuyển đổi BusinessPartnerSiteDto sang BusinessPartnerSite Entity
        /// </summary>
        /// <param name="dto">BusinessPartnerSiteDto</param>
        /// <returns>BusinessPartnerSite Entity</returns>
        public static BusinessPartnerSite ToEntity(this BusinessPartnerSiteDto dto)
        {
            if (dto == null) return null;

            return new BusinessPartnerSite
            {
                Id = dto.Id,
                PartnerId = dto.PartnerId,
                SiteCode = dto.SiteCode,
                SiteName = dto.SiteName,
                Address = dto.Address,
                City = dto.City,
                Province = dto.Province,
                Country = dto.Country,
                ContactPerson = dto.ContactPerson,
                Phone = dto.Phone,
                Email = dto.Email,
                IsDefault = dto.IsDefault,
                IsActive = dto.IsActive,
                CreatedDate = dto.CreatedDate,
                UpdatedDate = dto.UpdatedDate
            };
        }
    }
}
