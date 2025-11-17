using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.CustomerPartner;

    

    /// <summary>
    /// DTO cho chi tiết chi nhánh đối tác
    /// </summary>
    public class BusinessPartnerSiteDto
    {
        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("ID Đối tác")]
        [Required(ErrorMessage = "Vui lòng chọn đối tác")]
        public Guid PartnerId { get; set; }

        [DisplayName("Mã chi nhánh")]
        [Required(ErrorMessage = "Mã chi nhánh không được để trống")]
        [StringLength(50, ErrorMessage = "Mã chi nhánh không được vượt quá 50 ký tự")]
        public string SiteCode { get; set; }

        [DisplayName("Tên chi nhánh")]
        [Required(ErrorMessage = "Tên chi nhánh không được để trống")]
        [StringLength(255, ErrorMessage = "Tên chi nhánh không được vượt quá 255 ký tự")]
        public string SiteName { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Address { get; set; }

        [DisplayName("Thành phố")]
        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [DisplayName("Tỉnh/Thành phố")]
        [StringLength(100, ErrorMessage = "Tỉnh/Thành phố không được vượt quá 100 ký tự")]
        public string Province { get; set; }

        [DisplayName("Quốc gia")]
        [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
        public string Country { get; set; }

        [DisplayName("Người liên hệ")]
        [StringLength(100, ErrorMessage = "Người liên hệ không được vượt quá 100 ký tự")]
        public string ContactPerson { get; set; }

        [DisplayName("Số điện thoại")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [DisplayName("Mặc định")]
        public bool? IsDefault { get; set; }

        [DisplayName("Trạng thái")]
        public bool IsActive { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }
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