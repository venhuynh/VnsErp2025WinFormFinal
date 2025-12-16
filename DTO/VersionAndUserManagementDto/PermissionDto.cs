using Dal.DataContext;
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

        [DisplayName("Đang hoạt động")]
        public bool IsActive { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime? CreatedDate { get; set; }

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

    /// <summary>
    /// Extension methods cho Permission entities và DTOs
    /// </summary>
    public static class PermissionDtoExtensions
    {
        /// <summary>
        /// Convert Permission entity to PermissionDto
        /// </summary>
        public static PermissionDto ToDto(this Permission entity)
        {
            if (entity == null)
                return null;

            return new PermissionDto
            {
                Id = entity.Id,
                EntityName = entity.EntityName,
                Action = entity.Action,
                Description = entity.Description,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate
            };
        }

        /// <summary>
        /// Convert PermissionDto to Permission entity
        /// </summary>
        public static Permission ToEntity(this PermissionDto dto, Permission existingEntity = null)
        {
            if (dto == null)
                return null;

            Permission entity;
            if (existingEntity != null)
            {
                entity = existingEntity;
            }
            else
            {
                entity = new Permission();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            entity.EntityName = dto.EntityName;
            entity.Action = dto.Action;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;

            return entity;
        }

        /// <summary>
        /// Convert collection of Permission entities to PermissionDto list
        /// </summary>
        public static List<PermissionDto> ToDtos(this IEnumerable<Permission> entities)
        {
            if (entities == null)
                return new List<PermissionDto>();

            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// Convert collection of PermissionDto to Permission entities list
        /// </summary>
        public static List<Permission> ToEntities(this IEnumerable<PermissionDto> dtos)
        {
            if (dtos == null)
                return new List<Permission>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }
    }
}
