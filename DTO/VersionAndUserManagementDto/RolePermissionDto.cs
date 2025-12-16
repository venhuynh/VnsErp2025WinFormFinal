using Dal.DataContext;
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

        [DisplayName("Tên vai trò")]
        public string RoleName { get; set; }

        [DisplayName("ID Quyền")]
        [Required(ErrorMessage = "ID Quyền không được để trống")]
        public Guid PermissionId { get; set; }

        [DisplayName("Tên Entity")]
        public string EntityName { get; set; }

        [DisplayName("Hành động")]
        public string Action { get; set; }

        [DisplayName("Tên đầy đủ quyền")]
        public string PermissionFullName => $"{EntityName}.{Action}";

        [DisplayName("Được cấp quyền")]
        public bool IsGranted { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

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

    /// <summary>
    /// Extension methods cho RolePermission entities và DTOs
    /// </summary>
    public static class RolePermissionDtoExtensions
    {
        /// <summary>
        /// Convert RolePermission entity to RolePermissionDto
        /// </summary>
        public static RolePermissionDto ToDto(this RolePermission entity)
        {
            if (entity == null)
                return null;

            var dto = new RolePermissionDto
            {
                Id = entity.Id,
                RoleId = entity.RoleId,
                PermissionId = entity.PermissionId,
                IsGranted = entity.IsGranted,
                CreatedDate = entity.CreatedDate
            };

            // Load thông tin Role
            try
            {
                var role = entity.Role;
                if (role != null)
                {
                    dto.RoleName = role.Name;
                }
            }
            catch
            {
                // Ignore nếu không thể load
            }

            // Load thông tin Permission
            try
            {
                var permission = entity.Permission;
                if (permission != null)
                {
                    dto.EntityName = permission.EntityName;
                    dto.Action = permission.Action;
                }
            }
            catch
            {
                // Ignore nếu không thể load
            }

            return dto;
        }

        /// <summary>
        /// Convert RolePermissionDto to RolePermission entity
        /// </summary>
        public static RolePermission ToEntity(this RolePermissionDto dto, RolePermission existingEntity = null)
        {
            if (dto == null)
                return null;

            RolePermission entity;
            if (existingEntity != null)
            {
                entity = existingEntity;
            }
            else
            {
                entity = new RolePermission();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            entity.RoleId = dto.RoleId;
            entity.PermissionId = dto.PermissionId;
            entity.IsGranted = dto.IsGranted;

            return entity;
        }

        /// <summary>
        /// Convert collection of RolePermission entities to RolePermissionDto list
        /// </summary>
        public static List<RolePermissionDto> ToDtos(this IEnumerable<RolePermission> entities)
        {
            if (entities == null)
                return new List<RolePermissionDto>();

            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// Convert collection of RolePermissionDto to RolePermission entities list
        /// </summary>
        public static List<RolePermission> ToEntities(this IEnumerable<RolePermissionDto> dtos)
        {
            if (dtos == null)
                return new List<RolePermission>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }
    }
}
