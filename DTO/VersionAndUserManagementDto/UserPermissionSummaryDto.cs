using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DTO.VersionAndUserManagementDto
{
    /// <summary>
    /// DTO tổng hợp thông tin quyền của User
    /// Bao gồm: Roles, Permissions từ Role, Permissions trực tiếp (Override)
    /// </summary>
    public class UserPermissionSummaryDto
    {
        [DisplayName("ID Người dùng")]
        public Guid UserId { get; set; }

        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }

        [DisplayName("Danh sách Roles")]
        public List<RoleDto> Roles { get; set; } = new List<RoleDto>();

        [DisplayName("Danh sách Permissions từ Role")]
        public List<PermissionDto> PermissionsFromRole { get; set; } = new List<PermissionDto>();

        [DisplayName("Danh sách Permissions trực tiếp (Override)")]
        public List<PermissionDto> PermissionsFromUser { get; set; } = new List<PermissionDto>();

        [DisplayName("Tất cả Permissions")]
        [Description("Tổng hợp tất cả quyền (từ Role + Override, loại bỏ duplicate)")]
        public List<PermissionDto> AllPermissions { get; set; } = new List<PermissionDto>();

        [DisplayName("Số lượng Roles")]
        public int RoleCount => Roles?.Count ?? 0;

        [DisplayName("Số lượng Permissions")]
        public int PermissionCount => AllPermissions?.Count ?? 0;

        /// <summary>
        /// Lấy quyền theo Entity
        /// </summary>
        public List<PermissionDto> GetPermissionsByEntity(string entityName)
        {
            if (string.IsNullOrWhiteSpace(entityName))
                return new List<PermissionDto>();

            return AllPermissions?
                .Where(p => p.EntityName == entityName)
                .ToList() ?? new List<PermissionDto>();
        }

        /// <summary>
        /// Kiểm tra user có quyền không
        /// </summary>
        public bool HasPermission(string entityName, string action)
        {
            if (string.IsNullOrWhiteSpace(entityName) || string.IsNullOrWhiteSpace(action))
                return false;

            return AllPermissions?
                .Any(p => p.EntityName == entityName && p.Action == action && p.IsActive) ?? false;
        }

        /// <summary>
        /// Kiểm tra user có quyền Read
        /// </summary>
        public bool CanRead(string entityName)
        {
            return HasPermission(entityName, "Read");
        }

        /// <summary>
        /// Kiểm tra user có quyền Create
        /// </summary>
        public bool CanCreate(string entityName)
        {
            return HasPermission(entityName, "Create");
        }

        /// <summary>
        /// Kiểm tra user có quyền Update
        /// </summary>
        public bool CanUpdate(string entityName)
        {
            return HasPermission(entityName, "Update");
        }

        /// <summary>
        /// Kiểm tra user có quyền Delete
        /// </summary>
        public bool CanDelete(string entityName)
        {
            return HasPermission(entityName, "Delete");
        }

        /// <summary>
        /// Kiểm tra user có quyền Approve
        /// </summary>
        public bool CanApprove(string entityName)
        {
            return HasPermission(entityName, "Approve");
        }

        /// <summary>
        /// Thông tin tổng hợp dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin quyền người dùng dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var userName = UserName ?? "N/A";
                var html = $"<b><color='blue'>{userName}</color></b><br>";

                var infoParts = new List<string>();

                if (RoleCount > 0)
                {
                    var roleNames = string.Join(", ", Roles.Select(r => r.Name));
                    infoParts.Add($"<color='#757575'>Roles ({RoleCount}):</color> <b>{roleNames}</b>");
                }

                if (PermissionCount > 0)
                {
                    infoParts.Add($"<color='#757575'>Permissions ({PermissionCount}):</color> <b>{PermissionCount} quyền</b>");
                }

                if (PermissionsFromUser.Count > 0)
                {
                    infoParts.Add($"<color='#FF9800'>Override ({PermissionsFromUser.Count}):</color> <b>{PermissionsFromUser.Count} quyền</b>");
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
