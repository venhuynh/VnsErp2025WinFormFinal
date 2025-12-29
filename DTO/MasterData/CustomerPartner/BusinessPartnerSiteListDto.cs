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

    [DisplayName("Mã đối tác")] public string PartnerCode { get; set; }

    [DisplayName("Loại đối tác")] public int? PartnerType { get; set; }

    [DisplayName("Loại đối tác")] public string PartnerTypeName { get; set; }

    [DisplayName("Mã số thuế đối tác")] public string PartnerTaxCode { get; set; }

    [DisplayName("Website đối tác")] public string PartnerWebsite { get; set; }

    [DisplayName("Số điện thoại đối tác")] public string PartnerPhone { get; set; }

    [DisplayName("Email đối tác")] public string PartnerEmail { get; set; }

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
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
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

    [DisplayName("Địa chỉ đầy đủ")]
    public string SiteFullAddress
    {
        get
        {
            // Gộp các thuộc tính địa chỉ thành địa chỉ đầy đủ
            var addressParts = new List<string>();

            if (!string.IsNullOrEmpty(Address))
                addressParts.Add(Address);
            if (!string.IsNullOrEmpty(District))
                addressParts.Add(District);
            if (!string.IsNullOrEmpty(City))
                addressParts.Add(City);
            if (!string.IsNullOrEmpty(Province))
                addressParts.Add(Province);
            if (!string.IsNullOrEmpty(Country))
                addressParts.Add(Country);
            if (!string.IsNullOrEmpty(PostalCode))
                addressParts.Add(PostalCode);

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
            var phone = Phone ?? string.Empty;
            var email = Email ?? string.Empty;
            var siteTypeName = SiteTypeName ?? string.Empty;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";
            var defaultText = IsDefault == true ? " (Mặc định)" : string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên chi nhánh: font lớn, bold, màu xanh đậm (primary)
            // - Mã chi nhánh: font nhỏ hơn, màu xám
            // - Tên đối tác: hiển thị nếu có
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

            var html = $"<b><color='blue'>{siteName}</color></b>{defaultText}";

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

            // Hiển thị địa chỉ đầy đủ
            var fullAddress = SiteFullAddress;
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
    /// Thông tin về Đối tác dưới dạng HTML theo format DevExpress
    /// Hiển thị thông tin đối tác liên quan đến chi nhánh này
    /// </summary>
    [DisplayName("Thông tin Đối tác HTML")]
    [Description("Thông tin về đối tác dưới dạng HTML")]
    public string PartnerInfoHtml
    {
        get
        {
            var partnerName = PartnerName ?? string.Empty;
            var partnerCode = PartnerCode ?? string.Empty;
            var partnerTypeName = PartnerTypeName ?? string.Empty;
            var partnerTaxCode = PartnerTaxCode ?? string.Empty;
            var partnerPhone = PartnerPhone ?? string.Empty;
            var partnerEmail = PartnerEmail ?? string.Empty;
            var partnerWebsite = PartnerWebsite ?? string.Empty;

            if (string.IsNullOrWhiteSpace(partnerName))
            {
                return string.Empty;
            }

            var html = $"<b><color='blue'>{partnerName}</color></b>";

            if (!string.IsNullOrWhiteSpace(partnerCode))
            {
                html += $" <color='#757575'>({partnerCode})</color>";
            }

            html += "<br>";

            var infoParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(partnerTypeName))
            {
                var typeColor = PartnerType switch
                {
                    1 => "#2196F3", // Khách hàng: Blue
                    2 => "#FF9800", // Nhà cung cấp: Orange
                    3 => "#9C27B0", // Cả hai: Purple
                    _ => "#757575"  // Khác: Gray
                };
                infoParts.Add($"<color='#757575'>Loại:</color> <color='{typeColor}'><b>{partnerTypeName}</b></color>");
            }

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
            }

            var contactParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(partnerTaxCode))
            {
                contactParts.Add($"<color='#757575'>MST:</color> <b>{partnerTaxCode}</b>");
            }

            if (!string.IsNullOrWhiteSpace(partnerPhone))
            {
                contactParts.Add($"<color='#757575'>ĐT:</color> <b>{partnerPhone}</b>");
            }

            if (!string.IsNullOrWhiteSpace(partnerEmail))
            {
                contactParts.Add($"<color='#757575'>Email:</color> <b>{partnerEmail}</b>");
            }

            if (!string.IsNullOrWhiteSpace(partnerWebsite))
            {
                contactParts.Add($"<color='#757575'>Web:</color> <b>{partnerWebsite}</b>");
            }

            if (contactParts.Any())
            {
                html += string.Join(" | ", contactParts);
            }

            return html;
        }
    }

    /// <summary>
    /// Thông tin về địa chỉ liên hệ dưới dạng HTML theo format DevExpress
    /// Hiển thị địa chỉ đầy đủ và thông tin liên hệ của chi nhánh
    /// </summary>
    [DisplayName("Thông tin Địa chỉ liên hệ HTML")]
    [Description("Thông tin về địa chỉ liên hệ dưới dạng HTML")]
    public string ContactAddressHtml
    {
        get
        {
            var fullAddress = SiteFullAddress;
            var phone = Phone ?? string.Empty;
            var email = Email ?? string.Empty;
            var googleMapUrl = GoogleMapUrl ?? string.Empty;
            var siteName = SiteName ?? string.Empty;
            var siteCode = SiteCode ?? string.Empty;
            var siteTypeName = SiteTypeName ?? string.Empty;

            var html = string.Empty;

            // Tiêu đề: Tên chi nhánh
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                html += $"<b><color='blue'>{siteName}</color></b>";
                if (!string.IsNullOrWhiteSpace(siteCode))
                {
                    html += $" <color='#757575'>({siteCode})</color>";
                }
                html += "<br>";
            }

            // Loại địa điểm
            if (!string.IsNullOrWhiteSpace(siteTypeName))
            {
                var typeColor = SiteType switch
                {
                    1 => "#2196F3", // Trụ sở chính: Blue
                    2 => "#FF9800", // Chi nhánh: Orange
                    3 => "#9C27B0", // Kho hàng: Purple
                    _ => "#757575"  // Khác: Gray
                };
                html += $"<color='#757575'>Loại:</color> <color='{typeColor}'><b>{siteTypeName}</b></color><br>";
            }

            // Địa chỉ đầy đủ
            if (!string.IsNullOrWhiteSpace(fullAddress))
            {
                html += $"<color='#757575'>📍 Địa chỉ:</color> <b>{fullAddress}</b><br>";
            }

            // Thông tin liên hệ
            var contactParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(phone))
            {
                contactParts.Add($"<color='#757575'>📞 ĐT:</color> <b>{phone}</b>");
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                contactParts.Add($"<color='#757575'>✉️ Email:</color> <b>{email}</b>");
            }

            if (!string.IsNullOrWhiteSpace(googleMapUrl))
            {
                contactParts.Add($"<color='#757575'>🗺️ Map:</color> <b><u>Xem bản đồ</u></b>");
            }

            if (contactParts.Any())
            {
                html += string.Join(" | ", contactParts);
            }

            return html;
        }
    }
}
