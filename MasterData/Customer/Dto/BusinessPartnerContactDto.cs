using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Customer.Dto
{
    public class BusinessPartnerContactDto
    {
        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("ID chi nhánh")]
        [Required(ErrorMessage = "ID chi nhánh không được để trống")]
        public Guid SiteId { get; set; }

        [DisplayName("Tên chi nhánh")]
        [Required(ErrorMessage = "Tên chi nhánh không được để trống")]
        [Description("Tên chi nhánh mà contact này thuộc về (chỉ để hiển thị)")]
        public string SiteName { get; set; }

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

        [DisplayName("Liên hệ chính")]
        public bool IsPrimary { get; set; }

        [DisplayName("Ảnh đại diện")]
        [Description("Ảnh đại diện của liên hệ")]
        public byte[] Avatar { get; set; }

        [DisplayName("Trạng thái")]
        public bool IsActive { get; set; }
    }
}