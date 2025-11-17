using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.CustomerPartner;

/// <summary>
/// DTO cho danh sách chi nhánh đối tác
/// </summary>
public class BusinessPartnerSiteListDto
{
    [DisplayName("ID")] public Guid Id { get; set; }

    [DisplayName("ID Đối tác")] public Guid PartnerId { get; set; }

    [DisplayName("Mã chi nhánh")]
    [Required(ErrorMessage = "Mã chi nhánh không được để trống")]
    [StringLength(50, ErrorMessage = "Mã chi nhánh không được vượt quá 50 ký tự")]
    public string SiteCode { get; set; }

    [DisplayName("Tên đối tác")] public string PartnerName { get; set; }

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
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
    public string Phone { get; set; }

    [DisplayName("Email")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; }

    [DisplayName("Mặc định")] public bool? IsDefault { get; set; }

    [DisplayName("Trạng thái")] public bool IsActive { get; set; }

    [DisplayName("Ngày tạo")] public DateTime CreatedDate { get; set; }

    [DisplayName("Ngày cập nhật")] public DateTime? UpdatedDate { get; set; }

    [DisplayName("Địa chỉ đầy đủ")]
    public string SiteFullAddress
    {
        get
        {
            // Gộp các thuộc tính địa chỉ thành địa chỉ đầy đủ
            var addressParts = new List<string>();

            if (!string.IsNullOrEmpty(Address))
                addressParts.Add(Address);
            if (!string.IsNullOrEmpty(City))
                addressParts.Add(City);
            if (!string.IsNullOrEmpty(Province))
                addressParts.Add(Province);
            if (!string.IsNullOrEmpty(Country))
                addressParts.Add(Country);

            return string.Join(", ", addressParts);
        }
    }

    /// <summary>
    /// Thông tin chi nhánh đối tác dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    public string ThongTinHtml
    {
        get
        {
            var siteName = SiteName ?? string.Empty;
            var siteCode = SiteCode ?? string.Empty;
            var partnerName = PartnerName ?? string.Empty;
            var address = Address ?? string.Empty;
            var city = City ?? string.Empty;
            var province = Province ?? string.Empty;
            var country = Country ?? string.Empty;
            var contactPerson = ContactPerson ?? string.Empty;
            var phone = Phone ?? string.Empty;
            var email = Email ?? string.Empty;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";
            var defaultText = IsDefault == true ? "Mặc định" : string.Empty;
            var defaultColor = "#FF9800"; // Màu cam cho mặc định

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên chi nhánh: font lớn, bold, màu xanh đậm (primary)
            // - Mã chi nhánh: font nhỏ hơn, màu xám
            // - Tên đối tác: hiển thị nếu có
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)
            // - Mặc định: highlight với màu cam nếu là chi nhánh mặc định

            var html = $"<size=12><b><color='blue'>{siteName}</color></b></size>";

            if (!string.IsNullOrWhiteSpace(siteCode))
            {
                html += $" <size=9><color='#757575'>({siteCode})</color></size>";
            }

            html += "<br>";

            if (!string.IsNullOrWhiteSpace(partnerName))
            {
                html += $"<size=9><color='#757575'>Đối tác:</color></size> <size=10><color='#212121'><b>{partnerName}</b></color></size><br>";
            }

            // Hiển thị địa chỉ đầy đủ hoặc từng phần
            var fullAddress = SiteFullAddress;
            if (!string.IsNullOrWhiteSpace(fullAddress))
            {
                html += $"<size=9><color='#757575'>Địa chỉ:</color></size> <size=10><color='#212121'><b>{fullAddress}</b></color></size><br>";
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                html += $"<size=9><color='#757575'>Điện thoại:</color></size> <size=10><color='#212121'><b>{phone}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!string.IsNullOrWhiteSpace(phone))
                    html += " | ";
                html += $"<size=9><color='#757575'>Email:</color></size> <size=10><color='#212121'><b>{email}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(phone) || !string.IsNullOrWhiteSpace(email))
            {
                html += "<br>";
            }

            if (!string.IsNullOrWhiteSpace(contactPerson))
            {
                html += $"<size=9><color='#757575'>Người liên hệ:</color></size> <size=10><color='#212121'><b>{contactPerson}</b></color></size><br>";
            }

            // Hiển thị trạng thái mặc định nếu có
            if (!string.IsNullOrWhiteSpace(defaultText))
            {
                html += $"<size=9><color='#757575'>Loại:</color></size> <size=10><color='{defaultColor}'><b>{defaultText}</b></color></size> ";
            }

            html += $"<size=9><color='#757575'>Trạng thái:</color></size> <size=10><color='{statusColor}'><b>{statusText}</b></color></size>";

            return html;
        }
    }
}

public static class BusinessPartnerSiteListDtoConveter
{
    /// <summary>
    /// Chuyển đổi BusinessPartnerSite Entity sang BusinessPartnerSiteListDto
    /// </summary>
    /// <param name="entity">BusinessPartnerSite Entity</param>
    /// <returns>BusinessPartnerSiteListDto</returns>
    private static BusinessPartnerSiteListDto ToListDto(this BusinessPartnerSite entity)
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
    /// Chuyển đổi danh sách BusinessPartnerSite Entity sang danh sách BusinessPartnerSiteListDto
    /// </summary>
    /// <param name="entities">Danh sách BusinessPartnerSite Entity</param>
    /// <returns>Danh sách BusinessPartnerSiteListDto</returns>
    public static IEnumerable<BusinessPartnerSiteListDto> ToSiteListDtos(this IEnumerable<BusinessPartnerSite> entities)
    {
        return entities == null ? [] : entities.Select(entity => entity.ToListDto());
    }

}
