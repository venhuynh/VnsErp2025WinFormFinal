using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.Company;

public class PositionDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID chức vụ không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("ID Công ty")]
    [Required(ErrorMessage = "ID công ty không được để trống")]
    public Guid CompanyId { get; set; }

    [DisplayName("Mã chức vụ")]
    [Required(ErrorMessage = "Mã chức vụ không được để trống")]
    [StringLength(50, ErrorMessage = "Mã chức vụ không được vượt quá 50 ký tự")]
    public string PositionCode { get; set; }

    [DisplayName("Tên chức vụ")]
    [Required(ErrorMessage = "Tên chức vụ không được để trống")]
    [StringLength(255, ErrorMessage = "Tên chức vụ không được vượt quá 255 ký tự")]
    public string PositionName { get; set; }

    [DisplayName("Mô tả")]
    [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
    public string Description { get; set; }

    [DisplayName("Cấp quản lý")]
    public bool? IsManagerLevel { get; set; }

    [DisplayName("Trạng thái hoạt động")]
    [Required(ErrorMessage = "Trạng thái hoạt động không được để trống")]
    public bool IsActive { get; set; }

    // Navigation properties for display purposes
    [DisplayName("Tên công ty")]
    public string CompanyName { get; set; }

    /// <summary>
    /// Thông tin chức vụ dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Format giống DepartmentDto.ThongTinHtml (không dùng &lt;size&gt;)
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin chức vụ dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var html = string.Empty;

            // Tên chức vụ (màu xanh, đậm)
            if (!string.IsNullOrWhiteSpace(PositionName))
            {
                html += $"<b><color='blue'>{PositionName}</color></b>";
            }

            // Mã chức vụ (nếu có, màu xám)
            if (!string.IsNullOrWhiteSpace(PositionCode))
            {
                if (!string.IsNullOrWhiteSpace(html))
                    html += " ";
                html += $"<color='#757575'>({PositionCode})</color>";
            }

            html += "<br>";

            // Thông tin bổ sung
            var additionalInfo = new List<string>();

            // Trạng thái hoạt động
            var statusText = IsActive ? "Hoạt động" : "Không hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";
            additionalInfo.Add(
                $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>");

            // Cấp quản lý
            if (IsManagerLevel.HasValue)
            {
                var managerText = IsManagerLevel.Value ? "Có" : "Không";
                var managerColor = IsManagerLevel.Value ? "#4CAF50" : "#757575";
                additionalInfo.Add(
                    $"<color='#757575'>Cấp quản lý:</color> <color='{managerColor}'><b>{managerText}</b></color>");
            }

            // Tên công ty
            if (!string.IsNullOrWhiteSpace(CompanyName))
            {
                additionalInfo.Add(
                    $"<color='#757575'>Công ty:</color> <b>{CompanyName}</b>");
            }

            if (additionalInfo.Any())
            {
                html += string.Join(" | ", additionalInfo);
            }

            return html;
        }
    }
}
