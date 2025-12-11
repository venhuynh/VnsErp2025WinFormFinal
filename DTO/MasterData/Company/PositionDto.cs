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

/// <summary>
/// Converter để chuyển đổi giữa Position Entity và PositionDto
/// </summary>
public static class PositionConverter
{
    /// <summary>
    /// Chuyển đổi Position Entity sang PositionDto
    /// </summary>
    /// <param name="entity">Position entity</param>
    /// <returns>PositionDto</returns>
    public static PositionDto ToDto(this Dal.DataContext.Position entity)
    {
        if (entity == null)
            return null;

        return new PositionDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            PositionCode = entity.PositionCode,
            PositionName = entity.PositionName,
            Description = entity.Description,
            IsManagerLevel = entity.IsManagerLevel,
            IsActive = entity.IsActive,
            CompanyName = entity.Company?.CompanyName // Lấy tên công ty từ navigation property
        };
    }

    /// <summary>
    /// Chuyển đổi PositionDto sang Position Entity
    /// </summary>
    /// <param name="dto">PositionDto</param>
    /// <returns>Position entity</returns>
    public static Dal.DataContext.Position ToEntity(this PositionDto dto)
    {
        if (dto == null)
            return null;

        return new Dal.DataContext.Position
        {
            Id = dto.Id,
            CompanyId = dto.CompanyId,
            PositionCode = dto.PositionCode,
            PositionName = dto.PositionName,
            Description = dto.Description,
            IsManagerLevel = dto.IsManagerLevel,
            IsActive = dto.IsActive
        };
    }
}