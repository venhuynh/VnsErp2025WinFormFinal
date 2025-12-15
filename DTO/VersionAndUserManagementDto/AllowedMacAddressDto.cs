using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto;

/// <summary>
/// DTO cho quản lý MAC address được phép sử dụng ứng dụng
/// </summary>
public class AllowedMacAddressDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("Địa chỉ MAC")]
    [Required(ErrorMessage = "Địa chỉ MAC không được để trống")]
    [StringLength(50, ErrorMessage = "Địa chỉ MAC không được vượt quá 50 ký tự")]
    [RegularExpression(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$|^[0-9A-Fa-f]{12}$", 
        ErrorMessage = "Địa chỉ MAC không đúng định dạng (ví dụ: XX-XX-XX-XX-XX-XX hoặc XXXXXXXXXXXX)")]
    public string MacAddress { get; set; }

    [DisplayName("Tên máy tính")]
    [StringLength(255, ErrorMessage = "Tên máy tính không được vượt quá 255 ký tự")]
    public string ComputerName { get; set; }

    [DisplayName("Mô tả")]
    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
    public string Description { get; set; }

    [DisplayName("Đang hoạt động")]
    public bool IsActive { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime CreateDate { get; set; }

    [DisplayName("Người tạo")]
    public Guid? CreateBy { get; set; }

    [DisplayName("Ngày cập nhật")]
    public DateTime? ModifiedDate { get; set; }

    [DisplayName("Người cập nhật")]
    public Guid? ModifiedBy { get; set; }

    /// <summary>
    /// Thông tin MAC address dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin MAC address dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var macAddress = MacAddress ?? string.Empty;
            var computerName = ComputerName ?? string.Empty;
            var description = Description ?? string.Empty;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - MAC address: font lớn, bold, màu xanh đậm (primary)
            // - Tên máy tính: highlight với màu khác nhau
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

            var html = $"<b><color='blue'>{macAddress}</color></b>";

            if (IsActive)
            {
                html += " <color='#4CAF50'><b>●</b></color>";
            }
            else
            {
                html += " <color='#F44336'><b>●</b></color>";
            }

            html += "<br>";

            var infoParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(computerName))
            {
                infoParts.Add($"<color='#757575'>Máy tính:</color> <b>{computerName}</b>");
            }

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                html += $"<color='#757575'>Mô tả:</color> <b>{description}</b><br>";
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
            if (CreateDate != default(DateTime))
            {
                var createdInfo = $"<color='#757575'>Tạo:</color> <b>{CreateDate:dd/MM/yyyy HH:mm}</b>";
                infoParts.Add(createdInfo);
            }

            // Ngày cập nhật
            if (ModifiedDate.HasValue)
            {
                var modifiedInfo = $"<color='#757575'>Sửa:</color> <b>{ModifiedDate.Value:dd/MM/yyyy HH:mm}</b>";
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
