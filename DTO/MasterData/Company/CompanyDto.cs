using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.Company;

public class CompanyDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID công ty không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("Mã công ty")]
    [Required(ErrorMessage = "Mã công ty không được để trống")]
    [StringLength(50, ErrorMessage = "Mã công ty không được vượt quá 50 ký tự")]
    public string CompanyCode { get; set; }

    [DisplayName("Tên công ty")]
    [Required(ErrorMessage = "Tên công ty không được để trống")]
    [StringLength(255, ErrorMessage = "Tên công ty không được vượt quá 255 ký tự")]
    public string CompanyName { get; set; }

    [DisplayName("Mã số thuế")]
    [StringLength(50, ErrorMessage = "Mã số thuế không được vượt quá 50 ký tự")]
    public string TaxCode { get; set; }

    [DisplayName("Số điện thoại")]
    [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
    public string Phone { get; set; }

    [DisplayName("Email")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    public string Email { get; set; }

    [DisplayName("Website")]
    [StringLength(100, ErrorMessage = "Website không được vượt quá 100 ký tự")]
    public string Website { get; set; }

    [DisplayName("Địa chỉ")]
    [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
    public string Address { get; set; }

    [DisplayName("Quốc gia")]
    [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
    public string Country { get; set; }

    [DisplayName("Ngày tạo")]
    [Required(ErrorMessage = "Ngày tạo không được để trống")]
    public DateTime CreatedDate { get; set; }

    [DisplayName("Ngày cập nhật")] public DateTime? UpdatedDate { get; set; }

    [DisplayName("Logo")] public byte[] Logo { get; set; }

    // Logo metadata fields (để hiển thị và load logo từ NAS)
    [DisplayName("Tên file logo")]
    [StringLength(255, ErrorMessage = "Tên file logo không được vượt quá 255 ký tự")]
    public string LogoFileName { get; set; }

    [DisplayName("Đường dẫn tương đối logo")]
    [StringLength(500, ErrorMessage = "Đường dẫn tương đối logo không được vượt quá 500 ký tự")]
    public string LogoRelativePath { get; set; }

    [DisplayName("Đường dẫn đầy đủ logo")]
    [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ logo không được vượt quá 1000 ký tự")]
    public string LogoFullPath { get; set; }

    [DisplayName("Loại storage logo")]
    [StringLength(20, ErrorMessage = "Loại storage logo không được vượt quá 20 ký tự")]
    public string LogoStorageType { get; set; }

    [DisplayName("Kích thước file logo")] public long? LogoFileSize { get; set; }

    [DisplayName("Checksum logo")]
    [StringLength(64, ErrorMessage = "Checksum logo không được vượt quá 64 ký tự")]
    public string LogoChecksum { get; set; }

    /// <summary>
    /// Địa chỉ đầy đủ được tính từ Address, Country
    /// </summary>
    [DisplayName("Địa chỉ đầy đủ")]
    [Description("Địa chỉ đầy đủ được tính từ Address, Country")]
    public string FullAddressName
    {
        get
        {
            var addressParts = new List<string>();
            if (!string.IsNullOrWhiteSpace(Address))
                addressParts.Add(Address);
            if (!string.IsNullOrWhiteSpace(Country))
                addressParts.Add(Country);
            return string.Join(", ", addressParts);
        }
    }

    /// <summary>
    /// Thông tin công ty dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin công ty dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var companyName = CompanyName ?? string.Empty;
            var companyCode = CompanyCode ?? string.Empty;
            var taxCode = TaxCode ?? string.Empty;
            var phone = Phone ?? string.Empty;
            var email = Email ?? string.Empty;
            var website = Website ?? string.Empty;
            var fullAddress = FullAddressName;

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên công ty: font lớn, bold, màu xanh đậm (primary)
            // - Mã công ty: font nhỏ hơn, màu xám
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value

            var html = $"<b><color='blue'>{companyName}</color></b>";

            if (!string.IsNullOrWhiteSpace(companyCode))
            {
                html += $" <color='#757575'>({companyCode})</color>";
            }

            html += "<br>";

            var infoParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(taxCode))
            {
                infoParts.Add($"<color='#757575'>MST:</color> <b>{taxCode}</b>");
            }

            if (!string.IsNullOrWhiteSpace(fullAddress))
            {
                infoParts.Add($"<color='#757575'>Địa chỉ:</color> <b>{fullAddress}</b>");
            }

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
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

            if (!string.IsNullOrWhiteSpace(website))
            {
                contactParts.Add($"<color='#757575'>Web:</color> <b>{website}</b>");
            }

            if (contactParts.Any())
            {
                html += string.Join(" | ", contactParts) + "<br>";
            }

            // Thông tin ngày tạo/cập nhật
            var dateParts = new List<string>();
            if (CreatedDate != default(DateTime))
            {
                dateParts.Add($"<color='#757575'>Tạo:</color> <b>{CreatedDate:dd/MM/yyyy}</b>");
            }

            if (UpdatedDate.HasValue)
            {
                dateParts.Add($"<color='#757575'>Cập nhật:</color> <b>{UpdatedDate.Value:dd/MM/yyyy}</b>");
            }

            if (dateParts.Any())
            {
                html += string.Join(" | ", dateParts);
            }

            return html;
        }
    }
}