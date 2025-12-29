using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto
{
    /// <summary>
    /// DTO cho quản lý RolePermission (Liên kết Role và Permission)
    /// </summary>
    public class RolePermissionDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("ID Vai trò")]
        [Required(ErrorMessage = "ID Vai trò không được để trống")]
        public Guid RoleId { get; set; }

        [DisplayName("Tên vai trò")] public string RoleName { get; set; }

        [DisplayName("ID Quyền")]
        [Required(ErrorMessage = "ID Quyền không được để trống")]
        public Guid PermissionId { get; set; }

        [DisplayName("Tên Entity")] public string EntityName { get; set; }

        [DisplayName("Hành động")] public string Action { get; set; }

        [DisplayName("Tên đầy đủ quyền")] public string PermissionFullName => $"{EntityName}.{Action}";

        [DisplayName("Được cấp quyền")] public bool IsGranted { get; set; }

        [DisplayName("Ngày tạo")] public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Thông tin RolePermission dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin liên kết Role-Permission dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var roleName = RoleName ?? "N/A";
                var permissionName = PermissionFullName ?? "N/A";
                var grantedText = IsGranted ? "Cho phép" : "Từ chối";
                var grantedColor = IsGranted ? "#4CAF50" : "#F44336";

                var html = $"<b><color='blue'>{roleName}</color></b> → <b><color='green'>{permissionName}</color></b>";

                html += $" <color='{grantedColor}'><b>●</b></color>";

                html += "<br>";

                var infoParts = new List<string>();

                infoParts.Add($"<color='#757575'>Quyền:</color> <color='{grantedColor}'><b>{grantedText}</b></color>");

                if (CreatedDate.HasValue)
                {
                    infoParts.Add($"<color='#757575'>Ngày tạo:</color> <b>{CreatedDate.Value:dd/MM/yyyy HH:mm}</b>");
                }

                if (infoParts.Any())
                {
                    html += string.Join(" | ", infoParts);
                }

                return html;
            }
        }
    }
}