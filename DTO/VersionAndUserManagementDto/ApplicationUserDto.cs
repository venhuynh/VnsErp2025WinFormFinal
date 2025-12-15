using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto
{
    /// <summary>
    /// DTO cho quản lý người dùng ứng dụng
    /// </summary>
    public class ApplicationUserDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        public string UserName { get; set; }

        [DisplayName("Mật khẩu (Hash)")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(500, ErrorMessage = "Mật khẩu không được vượt quá 500 ký tự")]
        public string HashPassword { get; set; }

        [DisplayName("Đang hoạt động")]
        public bool Active { get; set; }

        /// <summary>
        /// Thông tin người dùng dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin người dùng dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var userName = UserName ?? string.Empty;
                var statusText = Active ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = Active ? "#4CAF50" : "#F44336";

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Tên đăng nhập: font lớn, bold, màu xanh đậm (primary)
                // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

                var html = $"<b><color='blue'>{userName}</color></b>";

                if (Active)
                {
                    html += " <color='#4CAF50'><b>●</b></color>";
                }
                else
                {
                    html += " <color='#F44336'><b>●</b></color>";
                }

                html += "<br>";

                html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

                return html;
            }
        }

        /// <summary>
        /// Thông tin audit (ngày tạo/cập nhật) dưới dạng HTML theo format DevExpress
        /// Lưu ý: ApplicationUser entity không có các trường CreateDate, CreateBy, ModifiedDate, ModifiedBy
        /// Nên thuộc tính này trả về chuỗi rỗng hoặc có thể được mở rộng trong tương lai
        /// </summary>
        [DisplayName("Thông tin audit HTML")]
        [Description("Thông tin ngày tạo và cập nhật dưới dạng HTML")]
        public string AuditInfoHtml
        {
            get
            {
                // ApplicationUser entity hiện tại không có các trường audit
                // Trả về chuỗi rỗng hoặc có thể mở rộng trong tương lai
                return string.Empty;
            }
        }
    }

    /// <summary>
    /// Extension methods cho ApplicationUser entities và DTOs.
    /// Cung cấp conversion, transformation, và utility methods.
    /// </summary>
    public static class ApplicationUserDtoExtensions
    {
        #region ========== CONVERSION METHODS ==========

        /// <summary>
        /// Convert ApplicationUser entity to ApplicationUserDto.
        /// </summary>
        /// <param name="entity">ApplicationUser entity</param>
        /// <returns>ApplicationUserDto</returns>
        public static ApplicationUserDto ToDto(this ApplicationUser entity)
        {
            if (entity == null)
                return null;

            return new ApplicationUserDto
            {
                Id = entity.Id,
                UserName = entity.UserName,
                HashPassword = entity.HashPassword,
                Active = entity.Active
            };
        }

        /// <summary>
        /// Convert ApplicationUserDto to ApplicationUser entity.
        /// </summary>
        /// <param name="dto">ApplicationUserDto</param>
        /// <param name="existingEntity">Existing entity to update (optional, for edit mode)</param>
        /// <returns>ApplicationUser entity</returns>
        public static ApplicationUser ToEntity(this ApplicationUserDto dto, ApplicationUser existingEntity = null)
        {
            if (dto == null)
                return null;

            ApplicationUser entity;
            if (existingEntity != null)
            {
                // Update existing entity
                entity = existingEntity;
            }
            else
            {
                // Create new entity
                entity = new ApplicationUser();
                if (dto.Id != Guid.Empty)
                {
                    entity.Id = dto.Id;
                }
            }

            // Map properties
            entity.UserName = dto.UserName;
            entity.HashPassword = dto.HashPassword;
            entity.Active = dto.Active;

            return entity;
        }

        /// <summary>
        /// Convert collection of ApplicationUser entities to ApplicationUserDto list.
        /// </summary>
        /// <param name="entities">Collection of ApplicationUser entities</param>
        /// <returns>List of ApplicationUserDto</returns>
        public static List<ApplicationUserDto> ToDtos(this IEnumerable<ApplicationUser> entities)
        {
            if (entities == null)
                return new List<ApplicationUserDto>();

            return entities.Select(ToDto).ToList();
        }

        /// <summary>
        /// Convert collection of ApplicationUserDto to ApplicationUser entities list.
        /// </summary>
        /// <param name="dtos">Collection of ApplicationUserDto</param>
        /// <returns>List of ApplicationUser entities</returns>
        public static List<ApplicationUser> ToEntities(this IEnumerable<ApplicationUserDto> dtos)
        {
            if (dtos == null)
                return new List<ApplicationUser>();

            return dtos.Select(dto => dto.ToEntity()).ToList();
        }

        #endregion
    }
}
