using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto
{
    /// <summary>
    /// DTO cho quản lý UserRole (Liên kết User và Role)
    /// </summary>
    public class UserRoleDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("ID Người dùng")]
        [Required(ErrorMessage = "ID Người dùng không được để trống")]
        public Guid UserId { get; set; }

        [DisplayName("Tên người dùng")]
        public string UserName { get; set; }

        [DisplayName("ID Vai trò")]
        [Required(ErrorMessage = "ID Vai trò không được để trống")]
        public Guid RoleId { get; set; }

        [DisplayName("Tên vai trò")]
        public string RoleName { get; set; }

        [DisplayName("Mô tả vai trò")]
        public string RoleDescription { get; set; }

        [DisplayName("Đang hoạt động")]
        public bool IsActive { get; set; }

        [DisplayName("Ngày gán")]
        public DateTime? AssignedDate { get; set; }

        [DisplayName("Người gán")]
        public Guid? AssignedBy { get; set; }

        [DisplayName("Tên người gán")]
        public string AssignedByName { get; set; }

        /// <summary>
        /// Thông tin UserRole dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin liên kết User-Role dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var userName = UserName ?? "N/A";
                var roleName = RoleName ?? "N/A";
                var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";

                var html = $"<b><color='blue'>{userName}</color></b> → <b><color='green'>{roleName}</color></b>";

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

                if (!string.IsNullOrWhiteSpace(RoleDescription))
                {
                    infoParts.Add($"<color='#757575'>Mô tả:</color> {RoleDescription}");
                }

                if (AssignedDate.HasValue)
                {
                    infoParts.Add($"<color='#757575'>Ngày gán:</color> <b>{AssignedDate.Value:dd/MM/yyyy HH:mm}</b>");
                }

                if (!string.IsNullOrWhiteSpace(AssignedByName))
                {
                    infoParts.Add($"<color='#757575'>Người gán:</color> <b>{AssignedByName}</b>");
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

    /// <summary>
    /// Extension methods cho UserRole entities và DTOs
    /// </summary>
    public static class UserRoleDtoExtensions
    {
        /// <summary>
        /// Convert UserRole entity to UserRoleDto
        /// </summary>
        public static UserRoleDto ToDto(this UserRole entity)
        {
            if (entity == null)
                return null;

            var dto = new UserRoleDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                RoleId = entity.RoleId,
                IsActive = entity.IsActive,
                AssignedDate = entity.AssignedDate,
                AssignedBy = entity.AssignedBy
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

            // Load thông tin Role
            try
            {
                var role = entity.Role;
                if (role != null)
                {
                    dto.RoleName = role.Name;
                    dto.RoleDescription = role.Description;
                }
            }
            catch
            {
                // Ignore nếu không thể load
            }

            // Load thông tin AssignedBy
            if (entity.AssignedBy.HasValue)
            {
                try
                {
                    var assignedByUser = entity.ApplicationUser1; // Navigation property từ AssignedBy
                    if (assignedByUser != null)
                    {
                        dto.AssignedByName = assignedByUser.UserName;
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
        /// Convert UserRoleDto to UserRole entity
        /// </summary>
        public static UserRole ToEntity(this UserRoleDto dto, UserRole existingEntity = null)
        {
            if (dto == null)
                return null;

            UserRole entity;
            if (existingEntity != null)
            {
                entity = existingEntity;
            }
            else
            {
                entity = new UserRole();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            entity.UserId = dto.UserId;
            entity.RoleId = dto.RoleId;
            entity.IsActive = dto.IsActive;
            entity.AssignedBy = dto.AssignedBy;

            return entity;
        }

        /// <summary>
        /// Convert collection of UserRole entities to UserRoleDto list
        /// </summary>
        public static List<UserRoleDto> ToDtos(this IEnumerable<UserRole> entities)
        {
            if (entities == null)
                return new List<UserRoleDto>();

            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// Convert collection of UserRoleDto to UserRole entities list
        /// </summary>
        public static List<UserRole> ToEntities(this IEnumerable<UserRoleDto> dtos)
        {
            if (dtos == null)
                return new List<UserRole>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }
    }
}
