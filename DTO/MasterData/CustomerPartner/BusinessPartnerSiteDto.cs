using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.CustomerPartner
{
    /// <summary>
    /// DTO cho chi tiết chi nhánh đối tác
    /// </summary>
    public class BusinessPartnerSiteDto
    {
        [DisplayName("ID")] public Guid Id { get; set; }

        [DisplayName("ID Đối tác")]
        [Required(ErrorMessage = "Vui lòng chọn đối tác")]
        public Guid PartnerId { get; set; }

        [DisplayName("Tên đối tác")]
        [Description("Tên đối tác (từ navigation property)")]
        public string PartnerName { get; set; }

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

        [DisplayName("Mã bưu điện")]
        [StringLength(20, ErrorMessage = "Mã bưu điện không được vượt quá 20 ký tự")]
        public string PostalCode { get; set; }

        [DisplayName("Quận/Huyện")]
        [StringLength(100, ErrorMessage = "Quận/Huyện không được vượt quá 100 ký tự")]
        public string District { get; set; }

        [DisplayName("Số điện thoại")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [DisplayName("Mặc định")] public bool? IsDefault { get; set; }

        [DisplayName("Trạng thái")] public bool IsActive { get; set; }

        [DisplayName("Loại địa điểm")]
        [Description("Loại địa điểm (1: Trụ sở chính, 2: Chi nhánh, 3: Kho hàng, ...)")]
        public int? SiteType { get; set; }

        [DisplayName("Loại địa điểm")]
        [Description("Tên loại địa điểm")]
        public string SiteTypeName { get; set; }

        [DisplayName("Ghi chú")]
        [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
        public string Notes { get; set; }

        [DisplayName("Google Map URL")]
        [StringLength(1000, ErrorMessage = "Google Map URL không được vượt quá 1000 ký tự")]
        public string GoogleMapUrl { get; set; }

        [DisplayName("Ngày tạo")] public DateTime CreatedDate { get; set; }

        [DisplayName("Ngày cập nhật")] public DateTime? UpdatedDate { get; set; }

        /// <summary>
        /// Địa chỉ đầy đủ được tính từ Address, District, City, Province, Country, PostalCode
        /// </summary>
        [DisplayName("Địa chỉ đầy đủ")]
        [Description("Địa chỉ đầy đủ được tính từ các thành phần địa chỉ")]
        public string FullAddressName
        {
            get
            {
                var addressParts = new List<string>();
                if (!string.IsNullOrWhiteSpace(Address))
                    addressParts.Add(Address);
                if (!string.IsNullOrWhiteSpace(District))
                    addressParts.Add(District);
                if (!string.IsNullOrWhiteSpace(City))
                    addressParts.Add(City);
                if (!string.IsNullOrWhiteSpace(Province))
                    addressParts.Add(Province);
                if (!string.IsNullOrWhiteSpace(Country))
                    addressParts.Add(Country);
                if (!string.IsNullOrWhiteSpace(PostalCode))
                    addressParts.Add(PostalCode);
                return string.Join(", ", addressParts);
            }
        }

        /// <summary>
        /// Thông tin chi nhánh dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin chi nhánh dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var siteName = SiteName ?? string.Empty;
                var siteCode = SiteCode ?? string.Empty;
                var partnerName = PartnerName ?? string.Empty;
                var fullAddress = FullAddressName;
                var phone = Phone ?? string.Empty;
                var email = Email ?? string.Empty;
                var siteTypeName = SiteTypeName ?? string.Empty;
                var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";
                var isDefaultText = IsDefault == true ? " (Mặc định)" : string.Empty;

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Tên chi nhánh: font lớn, bold, màu xanh đậm (primary)
                // - Mã chi nhánh: font nhỏ hơn, màu xám
                // - Tên đối tác: highlight với màu khác nhau
                // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
                // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

                var html = $"<b><color='blue'>{siteName}</color></b>{isDefaultText}";

                if (!string.IsNullOrWhiteSpace(siteCode))
                {
                    html += $" <color='#757575'>({siteCode})</color>";
                }

                html += "<br>";

                var infoParts = new List<string>();

                if (!string.IsNullOrWhiteSpace(partnerName))
                {
                    infoParts.Add($"<color='#757575'>Đối tác:</color> <b>{partnerName}</b>");
                }

                if (!string.IsNullOrWhiteSpace(siteTypeName))
                {
                    var typeColor = SiteType switch
                    {
                        1 => "#2196F3", // Trụ sở chính: Blue
                        2 => "#FF9800", // Chi nhánh: Orange
                        3 => "#9C27B0", // Kho hàng: Purple
                        _ => "#757575"  // Khác: Gray
                    };
                    infoParts.Add($"<color='#757575'>Loại:</color> <color='{typeColor}'><b>{siteTypeName}</b></color>");
                }

                if (infoParts.Any())
                {
                    html += string.Join(" | ", infoParts) + "<br>";
                }

                if (!string.IsNullOrWhiteSpace(fullAddress))
                {
                    html += $"<color='#757575'>Địa chỉ:</color> <b>{fullAddress}</b><br>";
                }

                var contactParts = new List<string>();
                if (!string.IsNullOrWhiteSpace(phone))
                {
                    contactParts.Add($"<color='#757575'>ĐT:</color> <b>{phone}</b>");
                }

                if (!string.IsNullOrWhiteSpace(email))
                {
                    contactParts.Add($"<color='#757575'>Email:</color> <b>{email}</b>");
                }

                if (!string.IsNullOrWhiteSpace(GoogleMapUrl))
                {
                    contactParts.Add($"<color='#757575'>Map:</color> <b><u>Xem bản đồ</u></b>");
                }

                if (contactParts.Any())
                {
                    html += string.Join(" | ", contactParts) + "<br>";
                }

                if (!string.IsNullOrWhiteSpace(Notes))
                {
                    html += $"<color='#757575'>Ghi chú:</color> <b>{Notes}</b><br>";
                }

                html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

                return html;
            }
        }

        /// <summary>
        /// Thông tin audit (ngày tạo/cập nhật) dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Thông tin audit HTML")]
        [Description("Thông tin ngày tạo và cập nhật dưới dạng HTML")]
        public string AuditInfoHtml
        {
            get
            {
                var html = string.Empty;
                var infoParts = new List<string>();

                // Ngày tạo
                if (CreatedDate != default(DateTime))
                {
                    var createdInfo = $"<color='#757575'>Tạo:</color> <b>{CreatedDate:dd/MM/yyyy HH:mm}</b>";
                    infoParts.Add(createdInfo);
                }

                // Ngày cập nhật
                if (UpdatedDate.HasValue)
                {
                    var modifiedInfo = $"<color='#757575'>Sửa:</color> <b>{UpdatedDate.Value:dd/MM/yyyy HH:mm}</b>";
                    infoParts.Add(modifiedInfo);
                }

                if (infoParts.Any())
                {
                    html = string.Join("<br>", infoParts);
                }

                return html;
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
        /// Sử dụng navigation properties đã được load trong repository
        /// </summary>
        /// <param name="entity">BusinessPartnerSite Entity</param>
        /// <returns>BusinessPartnerSiteDto</returns>
        public static BusinessPartnerSiteDto ToSiteDto(this BusinessPartnerSite entity)
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
                UpdatedDate = entity.UpdatedDate
            };

            // Lấy PartnerName từ navigation property
            try
            {
                var businessPartner = entity.BusinessPartner;
                dto.PartnerName = businessPartner?.PartnerName;
            }
            catch
            {
                // Navigation property chưa được load hoặc đã bị dispose
                dto.PartnerName = null;
            }

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

            // Chỉ set ID nếu là entity mới
            if (existingEntity == null && dto.Id != Guid.Empty)
            {
                entity.Id = dto.Id;
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

            return entities.Select(entity => entity.ToSiteDto());
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
    }
}