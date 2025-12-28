using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.Company;

public class DepartmentDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID phòng ban không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("ID công ty")]
    [Required(ErrorMessage = "ID công ty không được để trống")]
    public Guid CompanyId { get; set; }

    [DisplayName("ID chi nhánh")]
    [Required(ErrorMessage = "Chi nhánh không được để trống")]
    public Guid? BranchId { get; set; }

    [DisplayName("Mã phòng ban")]
    [Required(ErrorMessage = "Mã phòng ban không được để trống")]
    [StringLength(50, ErrorMessage = "Mã phòng ban không được vượt quá 50 ký tự")]
    public string DepartmentCode { get; set; }

    [DisplayName("Tên phòng ban")]
    [Required(ErrorMessage = "Tên phòng ban không được để trống")]
    [StringLength(255, ErrorMessage = "Tên phòng ban không được vượt quá 255 ký tự")]
    public string DepartmentName { get; set; }

    [DisplayName("ID phòng ban cha")]
    public Guid? ParentId { get; set; }

    [DisplayName("Mô tả")]
    [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
    public string Description { get; set; }

    [DisplayName("Trạng thái hoạt động")]
    public bool IsActive { get; set; }


    [DisplayName("Tên chi nhánh")]
    public string BranchName { get; set; }

    [DisplayName("Phòng ban cha")]
    public string ParentDepartmentName { get; set; }

    [DisplayName("Số nhân viên")]
    public int EmployeeCount { get; set; }

    [DisplayName("Số phòng ban con")]
    public int SubDepartmentCount { get; set; }

    /// <summary>
    /// Đường dẫn đầy đủ từ gốc đến phòng ban này
    /// Ví dụ: "Phòng ban A > Phòng ban A1"
    /// </summary>
    [DisplayName("Đường dẫn đầy đủ")]
    [Description("Đường dẫn đầy đủ từ gốc đến phòng ban này")]
    public string FullPath { get; set; }

    /// <summary>
    /// Đường dẫn đầy đủ dưới dạng HTML theo format DevExpress
    /// Format giống BusinessPartnerCategoryDto.FullPathHtml (không dùng &lt;size&gt;)
    /// </summary>
    [DisplayName("Đường dẫn HTML")]
    [Description("Đường dẫn đầy đủ từ gốc đến phòng ban này dưới dạng HTML")]
    public string FullPathHtml
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FullPath))
                return string.Empty;

            // Tách đường dẫn và format với màu sắc
            // Hỗ trợ nhiều format: " > ", ">", " >" hoặc "> "
            var parts = FullPath.Split(new[] { " > ", ">", " >", "> " }, StringSplitOptions.None)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim())
                .ToArray();
            
            var htmlParts = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                var isLast = i == parts.Length - 1;
                var color = isLast ? "blue" : "#757575";
                var weight = isLast ? "<b>" : "";
                var weightClose = isLast ? "</b>" : "";

                // Format giống BusinessPartnerCategoryDto: không dùng <size>, chỉ dùng <b> và <color>
                htmlParts.Add($"{weight}<color='{color}'>{parts[i]}</color>{weightClose}");
            }

            return string.Join(" <color='#757575'>></color> ", htmlParts);
        }
    }

    /// <summary>
    /// Thông tin phòng ban dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Format giống BusinessPartnerCategoryDto.CategoryInfoHtml (không dùng &lt;size&gt;)
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin phòng ban dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var html = string.Empty;

            // Tên phòng ban (màu xanh, đậm)
            if (!string.IsNullOrWhiteSpace(DepartmentName))
            {
                html += $"<b><color='blue'>{DepartmentName}</color></b>";
            }

            // Mã phòng ban (nếu có, màu xám)
            if (!string.IsNullOrWhiteSpace(DepartmentCode))
            {
                if (!string.IsNullOrWhiteSpace(html))
                    html += " ";
                html += $"<color='#757575'>({DepartmentCode})</color>";
            }

            html += "<br>";

            // Thông tin bổ sung
            var additionalInfo = new List<string>();

            // Trạng thái hoạt động
            var statusText = IsActive ? "Hoạt động" : "Không hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";
            additionalInfo.Add(
                $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>");

            // Số nhân viên
            if (EmployeeCount > 0)
            {
                additionalInfo.Add(
                    $"<color='#757575'>Nhân viên:</color> <b>{EmployeeCount}</b>");
            }

            // Số phòng ban con
            if (SubDepartmentCount > 0)
            {
                additionalInfo.Add(
                    $"<color='#757575'>Phòng ban con:</color> <b>{SubDepartmentCount}</b>");
            }

            // Chi nhánh
            if (!string.IsNullOrWhiteSpace(BranchName))
            {
                additionalInfo.Add(
                    $"<color='#757575'>Chi nhánh:</color> <b>{BranchName}</b>");
            }

            // Phòng ban cha
            if (!string.IsNullOrWhiteSpace(ParentDepartmentName) && ParentDepartmentName != "Không xác định")
            {
                additionalInfo.Add(
                    $"<color='#757575'>Phòng ban cha:</color> <b>{ParentDepartmentName}</b>");
            }

            if (additionalInfo.Any())
            {
                html += string.Join(" | ", additionalInfo);
            }

            return html;
        }
    }
}
