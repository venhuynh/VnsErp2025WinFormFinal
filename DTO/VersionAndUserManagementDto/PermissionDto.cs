using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto
{
    /// <summary>
    /// DTO cho quản lý Permission (Quyền truy cập)
    /// </summary>
    public class PermissionDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("Tên Entity")]
        [Required(ErrorMessage = "Tên Entity không được để trống")]
        [StringLength(100, ErrorMessage = "Tên Entity không được vượt quá 100 ký tự")]
        public string EntityName { get; set; }

        [DisplayName("Hành động")]
        [Required(ErrorMessage = "Hành động không được để trống")]
        [StringLength(50, ErrorMessage = "Hành động không được vượt quá 50 ký tự")]
        public string Action { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        [DisplayName("Đang hoạt động")] public bool IsActive { get; set; }

        [DisplayName("Ngày tạo")] public DateTime? CreatedDate { get; set; }

        [DisplayName("Tên đầy đủ")]
        [Description("EntityName.Action")]
        public string FullName => $"{EntityName}.{Action}";

        /// <summary>
        /// Thông tin quyền dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin quyền dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var entityName = EntityName ?? string.Empty;
                var action = Action ?? string.Empty;
                var fullName = FullName;
                var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";

                var html = $"<b><color='blue'>{fullName}</color></b>";

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

                if (!string.IsNullOrWhiteSpace(Description))
                {
                    infoParts.Add($"<color='#757575'>Mô tả:</color> {Description}");
                }

                infoParts.Add($"<color='#757575'>Entity:</color> <b>{entityName}</b>");
                infoParts.Add($"<color='#757575'>Action:</color> <b>{action}</b>");

                if (infoParts.Any())
                {
                    html += string.Join(" | ", infoParts) + "<br>";
                }

                html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

                return html;
            }
        }
    }
}