using System;
using System.Data.Linq;
using Dal.DataContext;
using DTO.MasterData.CustomerPartner;

namespace Dal.DtoConverter.MasterData.Customer
{


    public static class BusinessPartnerContactConverters
    {
        /// <summary>
        /// Chuyển đổi từ BusinessPartnerContact Entity sang BusinessPartnerContactDto
        /// </summary>
        /// <param name="entity">BusinessPartnerContact Entity</param>
        /// <param name="siteName">Tên chi nhánh (tùy chọn, nếu đã có sẵn)</param>
        /// <param name="partnerName">Tên đối tác (tùy chọn, nếu đã có sẵn)</param>
        /// <returns>BusinessPartnerContactDto</returns>
        public static BusinessPartnerContactDto ToDto(this BusinessPartnerContact entity, string siteName = null, string partnerName = null)
        {
            if (entity == null) return null;

            // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.BusinessPartnerSite, entity.BusinessPartnerSite.BusinessPartner)
            // vì DataContext đã bị dispose sau khi repository method kết thúc
            // Chỉ sử dụng tham số truyền vào

            return new BusinessPartnerContactDto
            {
                Id = entity.Id,
                SiteId = entity.SiteId,
                SiteName = siteName, // Sử dụng tham số truyền vào thay vì navigation property
                PartnerName = partnerName, // Sử dụng tham số truyền vào thay vì navigation property
                FullName = entity.FullName,
                Position = entity.Position,
                Phone = entity.Phone,
                Email = entity.Email,
                IsPrimary = entity.IsPrimary,
                Avatar = entity.AvatarThumbnailData?.ToArray(),
                IsActive = entity.IsActive
            };
        }

        /// <summary>
        /// Chuyển đổi từ BusinessPartnerContactDto sang BusinessPartnerContact Entity
        /// </summary>
        /// <param name="dto">BusinessPartnerContactDto</param>
        /// <param name="destination">BusinessPartnerContact Entity đích (tùy chọn, cho update)</param>
        /// <returns>BusinessPartnerContact Entity</returns>
        public static BusinessPartnerContact ToEntity(this BusinessPartnerContactDto dto,
            BusinessPartnerContact destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new BusinessPartnerContact();
            
            // Chỉ set ID nếu là entity mới
            if (destination == null && dto.Id != Guid.Empty)
            {
                entity.Id = dto.Id;
            }

            entity.SiteId = dto.SiteId;
            entity.FullName = dto.FullName;
            entity.Position = dto.Position;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.IsPrimary = dto.IsPrimary;
            entity.AvatarThumbnailData = dto.Avatar != null ? new Binary(dto.Avatar) : null;
            entity.IsActive = dto.IsActive;
            
            return entity;
        }
    }
}