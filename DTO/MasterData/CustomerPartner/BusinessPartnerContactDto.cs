using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.CustomerPartner
{
    public class BusinessPartnerContactDto
    {
        [DisplayName("ID")] public Guid Id { get; set; }

        [DisplayName("ID chi nhánh")]
        [Required(ErrorMessage = "ID chi nhánh không được để trống")]
        public Guid SiteId { get; set; }

        [DisplayName("Tên chi nhánh")]
        [Required(ErrorMessage = "Tên chi nhánh không được để trống")]
        [Description("Tên chi nhánh mà contact này thuộc về (chỉ để hiển thị)")]
        public string SiteName { get; set; }

        [DisplayName("Tên đối tác")]
        [Description("Tên đối tác mà contact này thuộc về (để group)")]
        public string PartnerName { get; set; }

        [DisplayName("Họ và tên")]
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; }

        [DisplayName("Chức vụ")]
        [StringLength(100, ErrorMessage = "Chức vụ không được vượt quá 100 ký tự")]
        public string Position { get; set; }

        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }

        [DisplayName("Liên hệ chính")] public bool IsPrimary { get; set; }

        /// <summary>
        /// Dữ liệu binary của avatar thumbnail (để hiển thị nhanh trong gridview)
        /// Lưu ý: Chỉ lưu thumbnail trong database, avatar gốc lưu trên NAS
        /// </summary>
        [DisplayName("Ảnh đại diện")]
        [Description("Ảnh đại diện thumbnail của liên hệ (để hiển thị nhanh)")]
        public byte[] Avatar { get; set; }

        /// <summary>
        /// Avatar dưới dạng base64 data URI để hiển thị trong HTML template
        /// </summary>
        [DisplayName("Ảnh đại diện (Base64)")]
        [Description("Ảnh đại diện dưới dạng base64 data URI cho HTML template")]
        public string AvatarBase64
        {
            get
            {
                if (Avatar == null || Avatar.Length == 0)
                    return string.Empty;

                try
                {
                    return $"data:image/png;base64,{Convert.ToBase64String(Avatar)}";
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        [DisplayName("Trạng thái")] public bool IsActive { get; set; }

        /// <summary>
        /// Trạng thái selected (để hiển thị checkbox trong template)
        /// </summary>
        [DisplayName("Đã chọn")]
        [Description("Trạng thái selected để hiển thị checkbox")]
        public bool Selected { get; set; }

        /// <summary>
        /// Thông tin liên hệ dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Format giống BusinessPartnerCategoryDto.CategoryInfoHtml (không dùng &lt;size&gt;)
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin liên hệ HTML")]
        [Description("Thông tin liên hệ dưới dạng HTML")]
        public string ContactInfoHtml
        {
            get
            {
                var html = string.Empty;

                // Tên liên hệ (màu xanh, đậm)
                if (!string.IsNullOrWhiteSpace(FullName))
                {
                    html += $"<b><color='blue'>{FullName}</color></b>";
                }

                // Chức vụ (nếu có, màu xám)
                if (!string.IsNullOrWhiteSpace(Position))
                {
                    if (!string.IsNullOrWhiteSpace(html))
                        html += " ";
                    html += $"<color='#757575'>({Position})</color>";
                }

                // Liên hệ chính (badge)
                if (IsPrimary)
                {
                    if (!string.IsNullOrWhiteSpace(html))
                        html += " ";
                    html += $"<color='#4CAF50'><b>[Liên hệ chính]</b></color>";
                }

                html += "<br>";

                // Thông tin bổ sung
                var additionalInfo = new List<string>();

                // Chi nhánh
                if (!string.IsNullOrWhiteSpace(SiteName))
                {
                    additionalInfo.Add(
                        $"<color='#757575'>Chi nhánh:</color> <b>{SiteName}</b>");
                }

                // Trạng thái hoạt động
                var statusText = IsActive ? "Hoạt động" : "Không hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";
                additionalInfo.Add(
                    $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>");

                if (additionalInfo.Any())
                {
                    html += string.Join(" | ", additionalInfo);
                }

                return html;
            }
        }

        /// <summary>
        /// Thông tin liên lạc dưới dạng HTML theo format DevExpress
        /// Hiển thị Phone, Email, Mobile, Fax, v.v.
        /// </summary>
        [DisplayName("Thông tin liên lạc HTML")]
        [Description("Thông tin liên lạc (điện thoại, email, v.v.) dưới dạng HTML")]
        public string ContactDetailsHtml
        {
            get
            {
                var contactParts = new List<string>();

                // Số điện thoại
                if (!string.IsNullOrWhiteSpace(Phone))
                {
                    contactParts.Add($"<color='#757575'>ĐT:</color> <b>{Phone}</b>");
                }

                // Email
                if (!string.IsNullOrWhiteSpace(Email))
                {
                    contactParts.Add($"<color='#757575'>Email:</color> <b>{Email}</b>");
                }

                if (contactParts.Any())
                {
                    return string.Join(" | ", contactParts);
                }

                return string.Empty;
            }
        }
    }
}
