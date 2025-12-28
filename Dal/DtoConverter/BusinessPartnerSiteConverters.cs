using Dal.DataContext;
using DTO.MasterData.CustomerPartner;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Dal.DtoConverter
{

    public static class BusinessPartnerSiteListDtoConveter
    {
        /// <summary>
        /// Chuyển đổi BusinessPartnerSite Entity sang BusinessPartnerSiteListDto
        /// </summary>
        /// <param name="entity">BusinessPartnerSite Entity</param>
        /// <param name="partnerName">Tên đối tác (từ eager loading)</param>
        /// <param name="partnerCode">Mã đối tác (từ eager loading)</param>
        /// <param name="partnerType">Loại đối tác (từ eager loading)</param>
        /// <param name="partnerTaxCode">Mã số thuế đối tác (từ eager loading)</param>
        /// <param name="partnerPhone">Số điện thoại đối tác (từ eager loading)</param>
        /// <param name="partnerEmail">Email đối tác (từ eager loading)</param>
        /// <param name="partnerWebsite">Website đối tác (từ eager loading)</param>
        /// <returns>BusinessPartnerSiteListDto</returns>
        private static BusinessPartnerSiteListDto ToListDto(
            this BusinessPartnerSite entity,
            string partnerName = null,
            string partnerCode = null,
            int? partnerType = null,
            string partnerTaxCode = null,
            string partnerPhone = null,
            string partnerEmail = null,
            string partnerWebsite = null)
        {
            if (entity == null) return null;

            var dto = new BusinessPartnerSiteListDto
            {
                Id = entity.Id,
                PartnerId = entity.PartnerId,
                SiteCode = entity.SiteCode,
                SiteName = entity.SiteName,
                Address = entity.Address,
                City = entity.City,
                Province = entity.Province,
                Country = entity.Country,
                PostalCode = entity.PostalCode,
                District = entity.District,
                Phone = entity.Phone,
                Email = entity.Email,
                IsDefault = entity.IsDefault,
                IsActive = entity.IsActive,
                SiteType = entity.SiteType,
                Notes = entity.Notes,
                GoogleMapUrl = entity.GoogleMapUrl,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                PartnerName = partnerName,
                PartnerCode = partnerCode,
                PartnerType = partnerType,
                PartnerTypeName = partnerType.HasValue ? ResolvePartnerTypeName(partnerType.Value) : null,
                PartnerTaxCode = partnerTaxCode,
                PartnerPhone = partnerPhone,
                PartnerEmail = partnerEmail,
                PartnerWebsite = partnerWebsite
            };

            // Resolve SiteTypeName
            dto.SiteTypeName = ResolveSiteTypeName(entity.SiteType);

            return dto;
        }

        /// <summary>
        /// Resolve tên loại địa điểm từ SiteType
        /// </summary>
        /// <param name="siteType">SiteType (int?)</param>
        /// <returns>Tên loại địa điểm</returns>
        private static string ResolveSiteTypeName(int? siteType)
        {
            if (!siteType.HasValue) return string.Empty;

            return siteType.Value switch
            {
                1 => "Trụ sở chính",
                2 => "Chi nhánh",
                3 => "Kho hàng",
                4 => "Văn phòng đại diện",
                _ => "Khác"
            };
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
                3 => "Khách hàng & Nhà cung cấp",
                _ => "Không xác định"
            };
        }


        /// <summary>
        /// Chuyển đổi danh sách BusinessPartnerSite Entity sang danh sách BusinessPartnerSiteListDto
        /// </summary>
        /// <param name="entities">Danh sách BusinessPartnerSite Entity</param>
        /// <returns>Danh sách BusinessPartnerSiteListDto</returns>
        public static IEnumerable<BusinessPartnerSiteListDto> ToSiteListDtos(this IEnumerable<BusinessPartnerSite> entities)
        {
            if (entities == null) return [];

            return entities.Select(entity =>
            {
                // Lấy thông tin partner từ navigation property trước khi context bị dispose
                string partnerName = null;
                string partnerCode = null;
                int? partnerType = null;
                string partnerTaxCode = null;
                string partnerPhone = null;
                string partnerEmail = null;
                string partnerWebsite = null;

                try
                {
                    var businessPartner = entity.BusinessPartner;
                    if (businessPartner != null)
                    {
                        partnerName = businessPartner.PartnerName;
                        partnerCode = businessPartner.PartnerCode;
                        partnerType = businessPartner.PartnerType;
                        partnerTaxCode = businessPartner.TaxCode;
                        partnerPhone = businessPartner.Phone;
                        partnerEmail = businessPartner.Email;
                        partnerWebsite = businessPartner.Website;
                    }
                }
                catch
                {
                    // Navigation property chưa được load hoặc đã bị dispose
                }

                return entity.ToListDto(partnerName, partnerCode, partnerType, partnerTaxCode, partnerPhone, partnerEmail, partnerWebsite);
            });
        }

    }

}


/// <summary>
/// Converter cho BusinessPartnerSite Entity và DTO
/// </summary>
public static class BusinessPartnerSiteConverters
{
    /// <summary>
    /// Chuyển đổi BusinessPartnerSite Entity sang BusinessPartnerSiteDto
    /// </summary>
    /// <param name="entity">BusinessPartnerSite Entity</param>
    /// <param name="partnerName">Tên đối tác (từ eager loading)</param>
    /// <param name="partnerCode">Mã đối tác (từ eager loading)</param>
    /// <param name="partnerType">Loại đối tác (từ eager loading)</param>
    /// <param name="partnerTaxCode">Mã số thuế đối tác (từ eager loading)</param>
    /// <param name="partnerPhone">Số điện thoại đối tác (từ eager loading)</param>
    /// <param name="partnerEmail">Email đối tác (từ eager loading)</param>
    /// <param name="partnerWebsite">Website đối tác (từ eager loading)</param>
    /// <returns>BusinessPartnerSiteDto</returns>
    public static BusinessPartnerSiteDto ToSiteDto(
        this BusinessPartnerSite entity,
        string partnerName = null,
        string partnerCode = null,
        int? partnerType = null,
        string partnerTaxCode = null,
        string partnerPhone = null,
        string partnerEmail = null,
        string partnerWebsite = null)
    {
        if (entity == null) return null;

        var dto = new BusinessPartnerSiteDto
        {
            Id = entity.Id,
            PartnerId = entity.PartnerId,
            SiteCode = entity.SiteCode,
            SiteName = entity.SiteName,
            Address = entity.Address,
            City = entity.City,
            Province = entity.Province,
            Country = entity.Country,
            PostalCode = entity.PostalCode,
            District = entity.District,
            Phone = entity.Phone,
            Email = entity.Email,
            IsDefault = entity.IsDefault,
            IsActive = entity.IsActive,
            SiteType = entity.SiteType,
            Notes = entity.Notes,
            GoogleMapUrl = entity.GoogleMapUrl,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate,
            PartnerName = partnerName,
            PartnerCode = partnerCode,
            PartnerType = partnerType,
            PartnerTypeName = partnerType.HasValue ? ResolvePartnerTypeName(partnerType.Value) : null,
            PartnerTaxCode = partnerTaxCode,
            PartnerPhone = partnerPhone,
            PartnerEmail = partnerEmail,
            PartnerWebsite = partnerWebsite
        };

        // Resolve SiteTypeName
        dto.SiteTypeName = ResolveSiteTypeName(entity.SiteType);

        return dto;
    }

    /// <summary>
    /// Chuyển đổi BusinessPartnerSiteDto sang BusinessPartnerSite Entity
    /// </summary>
    /// <param name="dto">BusinessPartnerSiteDto</param>
    /// <param name="existingEntity">Entity hiện tại (cho update)</param>
    /// <returns>BusinessPartnerSite Entity</returns>
    public static BusinessPartnerSite ToEntity(this BusinessPartnerSiteDto dto, BusinessPartnerSite existingEntity = null)
    {
        if (dto == null) return null;

        var entity = existingEntity ?? new BusinessPartnerSite();

        // Chỉ set ID nếu là entity đã tồn tại (edit mode)
        // Khi tạo mới (existingEntity == null), không set Id từ dto để đảm bảo Id = Guid.Empty
        if (existingEntity != null)
        {
            // Edit mode: giữ nguyên Id của existing entity
            // Không cần set lại vì entity đã là existingEntity
        }
        else
        {
            // Create mode: đảm bảo Id = Guid.Empty (default của new BusinessPartnerSite())
            entity.Id = Guid.Empty;
        }

        entity.PartnerId = dto.PartnerId;
        entity.SiteCode = dto.SiteCode;
        entity.SiteName = dto.SiteName;
        entity.Address = dto.Address;
        entity.City = dto.City;
        entity.Province = dto.Province;
        entity.Country = dto.Country;
        entity.PostalCode = dto.PostalCode;
        entity.District = dto.District;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.IsDefault = dto.IsDefault;
        entity.IsActive = dto.IsActive;
        entity.SiteType = dto.SiteType;
        entity.Notes = dto.Notes;
        entity.GoogleMapUrl = dto.GoogleMapUrl;
        entity.CreatedDate = dto.CreatedDate;
        entity.UpdatedDate = dto.UpdatedDate;

        return entity;
    }

    /// <summary>
    /// Chuyển đổi danh sách BusinessPartnerSite Entity sang danh sách BusinessPartnerSiteDto
    /// </summary>
    /// <param name="entities">Danh sách BusinessPartnerSite Entity</param>
    /// <returns>Danh sách BusinessPartnerSiteDto</returns>
    public static IEnumerable<BusinessPartnerSiteDto> ToSiteDtos(this IEnumerable<BusinessPartnerSite> entities)
    {
        if (entities == null) return [];

        return entities.Select(entity =>
        {
            // Lấy thông tin partner từ navigation property trước khi context bị dispose
            string partnerName = null;
            string partnerCode = null;
            int? partnerType = null;
            string partnerTaxCode = null;
            string partnerPhone = null;
            string partnerEmail = null;
            string partnerWebsite = null;

            try
            {
                var businessPartner = entity.BusinessPartner;
                if (businessPartner != null)
                {
                    partnerName = businessPartner.PartnerName;
                    partnerCode = businessPartner.PartnerCode;
                    partnerType = businessPartner.PartnerType;
                    partnerTaxCode = businessPartner.TaxCode;
                    partnerPhone = businessPartner.Phone;
                    partnerEmail = businessPartner.Email;
                    partnerWebsite = businessPartner.Website;
                }
            }
            catch
            {
                // Navigation property chưa được load hoặc đã bị dispose
            }

            return entity.ToSiteDto(partnerName, partnerCode, partnerType, partnerTaxCode, partnerPhone, partnerEmail, partnerWebsite);
        });
    }

    /// <summary>
    /// Resolve tên loại địa điểm từ SiteType
    /// </summary>
    /// <param name="siteType">SiteType (int?)</param>
    /// <returns>Tên loại địa điểm</returns>
    private static string ResolveSiteTypeName(int? siteType)
    {
        if (!siteType.HasValue) return string.Empty;

        return siteType.Value switch
        {
            1 => "Trụ sở chính",
            2 => "Chi nhánh",
            3 => "Kho hàng",
            4 => "Văn phòng đại diện",
            _ => "Khác"
        };
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
            3 => "Khách hàng & Nhà cung cấp",
            _ => "Không xác định"
        };
    }
}