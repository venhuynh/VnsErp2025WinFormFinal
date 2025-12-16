using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto
{
    /// <summary>
    /// DTO cho quản lý UserPermission (Quyền trực tiếp cho User - Override)
    /// </summary>
    public class UserPermissionDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("ID Người dùng")]
        [Required(ErrorMessage = "ID Người dùng không được để trống")]
        public Guid UserId { get; set; }

        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }

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

        [DisplayName("Là Override")]
        [Description("Quyền này override quyền từ Role")]
        public bool IsOverride { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

        [DisplayName("Người tạo")]
        public Guid? CreatedBy { get; set; }

        [DisplayName("Tên người tạo")]
        public string CreatedByName { get; set; }

        /// <summary>
        /// Thông tin UserPermission dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin quyền người dùng dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var userName = UserName ?? "N/A";
                var permissionName = PermissionFullName ?? "N/A";
                var grantedText = IsGranted ? "Cho phép" : "Từ chối";
                var grantedColor = IsGranted ? "#4CAF50" : "#F44336";
                var overrideText = IsOverride ? " <color='#FF9800'><b>[Override]</b></color>" : "";

                var html = $"<b><color='blue'>{userName}</color></b> → <b><color='green'>{permissionName}</color></b>{overrideText}";

                html += $" <color='{grantedColor}'><b>●</b></color>";

                html += "<br>";

                var infoParts = new List<string>();

                infoParts.Add($"<color='#757575'>Quyền:</color> <color='{grantedColor}'><b>{grantedText}</b></color>");

                if (CreatedDate.HasValue)
                {
                    infoParts.Add($"<color='#757575'>Ngày tạo:</color> <b>{CreatedDate.Value:dd/MM/yyyy HH:mm}</b>");
                }

                if (!string.IsNullOrWhiteSpace(CreatedByName))
                {
                    infoParts.Add($"<color='#757575'>Người tạo:</color> <b>{CreatedByName}</b>");
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
    /// Extension methods cho UserPermission entities và DTOs
    /// </summary>
    public static class UserPermissionDtoExtensions
    {
        /// <summary>
        /// Convert UserPermission entity to UserPermissionDto
        /// </summary>
        public static UserPermissionDto ToDto(this UserPermission entity)
        {
            if (entity == null)
                return null;

            var dto = new UserPermissionDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                PermissionId = entity.PermissionId,
                IsGranted = entity.IsGranted,
                IsOverride = entity.IsOverride,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy
            };

            // Load thông tin User
            try
            {
                var user = entity.ApplicationUser;
                if (user != null)
                {
                    dto.UserName = user.UserName;
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

            // Load thông tin CreatedBy
            if (entity.CreatedBy.HasValue)
            {
                try
                {
                    var createdByUser = entity.ApplicationUser1; // Navigation property từ CreatedBy
                    if (createdByUser != null)
                    {
                        dto.CreatedByName = createdByUser.UserName;
                    }
                }
                catch
                {
                    // Ignore nếu không thể load
                }
            }

            return dto;
        }

        /// <summary>
        /// Convert UserPermissionDto to UserPermission entity
        /// </summary>
        public static UserPermission ToEntity(this UserPermissionDto dto, UserPermission existingEntity = null)
        {
            if (dto == null)
                return null;

            UserPermission entity;
            if (existingEntity != null)
            {
                entity = existingEntity;
            }
            else
            {
                entity = new UserPermission();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            entity.UserId = dto.UserId;
            entity.PermissionId = dto.PermissionId;
            entity.IsGranted = dto.IsGranted;
            entity.IsOverride = dto.IsOverride;
            entity.CreatedBy = dto.CreatedBy;

            return entity;
        }

        /// <summary>
        /// Convert collection of UserPermission entities to UserPermissionDto list
        /// </summary>
        public static List<UserPermissionDto> ToDtos(this IEnumerable<UserPermission> entities)
        {
            if (entities == null)
                return new List<UserPermissionDto>();

            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// Convert collection of UserPermissionDto to UserPermission entities list
        /// </summary>
        public static List<UserPermission> ToEntities(this IEnumerable<UserPermissionDto> dtos)
        {
            if (dtos == null)
                return new List<UserPermission>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }
    }
}
