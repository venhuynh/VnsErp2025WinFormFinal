using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto
{
    /// <summary>
    /// DTO cho quản lý Role (Vai trò người dùng)
    /// </summary>
    public class RoleDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("Tên vai trò")]
        [Required(ErrorMessage = "Tên vai trò không được để trống")]
        [StringLength(100, ErrorMessage = "Tên vai trò không được vượt quá 100 ký tự")]
        public string Name { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
        public string Description { get; set; }

        [DisplayName("Là vai trò hệ thống")]
        [Description("Vai trò hệ thống không thể xóa")]
        public bool IsSystemRole { get; set; }

        [DisplayName("Đang hoạt động")] public bool IsActive { get; set; }

        [DisplayName("Ngày tạo")] public DateTime? CreatedDate { get; set; }

        [DisplayName("Người tạo")] public Guid? CreatedBy { get; set; }

        [DisplayName("Tên người tạo")] public string CreatedByName { get; set; }

        [DisplayName("Ngày cập nhật")] public DateTime? ModifiedDate { get; set; }

        [DisplayName("Người cập nhật")] public Guid? ModifiedBy { get; set; }

        [DisplayName("Tên người cập nhật")] public string ModifiedByName { get; set; }

        [DisplayName("Số lượng người dùng")]
        [Description("Số lượng người dùng có vai trò này")]
        public int UserCount { get; set; }

        [DisplayName("Số lượng quyền")]
        [Description("Số lượng quyền được gán cho vai trò này")]
        public int PermissionCount { get; set; }

        /// <summary>
        /// Thông tin vai trò dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin vai trò dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var name = Name ?? string.Empty;
                var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";
                var systemRoleText = IsSystemRole ? " <color='#FF9800'><b>[Hệ thống]</b></color>" : "";

                var html = $"<b><color='blue'>{name}</color></b>{systemRoleText}";

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

                if (UserCount > 0)
                {
                    infoParts.Add($"<color='#757575'>Người dùng:</color> <b>{UserCount}</b>");
                }

                if (PermissionCount > 0)
                {
                    infoParts.Add($"<color='#757575'>Quyền:</color> <b>{PermissionCount}</b>");
                }

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