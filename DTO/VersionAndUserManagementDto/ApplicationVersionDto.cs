using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto;

/// <summary>
/// DTO cho quản lý phiên bản ứng dụng
/// </summary>
public class ApplicationVersionDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("Phiên bản")]
    [Required(ErrorMessage = "Phiên bản không được để trống")]
    [StringLength(50, ErrorMessage = "Phiên bản không được vượt quá 50 ký tự")]
    public string Version { get; set; }

    [DisplayName("Ngày phát hành")]
    [Required(ErrorMessage = "Ngày phát hành không được để trống")]
    public DateTime ReleaseDate { get; set; }

    [DisplayName("Đang hoạt động")]
    public bool IsActive { get; set; }

    [DisplayName("Mô tả")]
    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
    public string Description { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime CreateDate { get; set; }

    [DisplayName("Người tạo")]
    public Guid? CreateBy { get; set; }

    [DisplayName("Ngày cập nhật")]
    public DateTime? ModifiedDate { get; set; }

    [DisplayName("Người cập nhật")]
    public Guid? ModifiedBy { get; set; }

    [DisplayName("Ghi chú phát hành")]
    [StringLength(1000, ErrorMessage = "Ghi chú phát hành không được vượt quá 1000 ký tự")]
    public string ReleaseNote { get; set; }

    /// <summary>
    /// Thông tin phiên bản dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin phiên bản dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var version = Version ?? string.Empty;
            var description = Description ?? string.Empty;
            var releaseDate = ReleaseDate;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#757575";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Phiên bản: font lớn, bold, màu xanh đậm (primary)
            // - Trạng thái: highlight với màu xanh (active) hoặc xám (inactive)
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value

            var html = $"<b><color='blue'>{version}</color></b>";

            if (IsActive)
            {
                html += " <color='#4CAF50'><b>●</b></color>";
            }

            html += "<br>";

            var infoParts = new List<string>();

            if (releaseDate != default(DateTime))
            {
                infoParts.Add($"<color='#757575'>Phát hành:</color> <b>{releaseDate:dd/MM/yyyy}</b>");
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
