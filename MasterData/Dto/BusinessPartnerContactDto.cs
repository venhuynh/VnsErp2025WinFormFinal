using System;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Dto
{
    public class BusinessPartnerContactDto
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "ID đối tác")]
        [Required(ErrorMessage = "ID đối tác không được để trống")]
        public Guid PartnerId { get; set; }

        [Display(Name = "Họ và tên")]
        [Required(ErrorMessage = "Họ và tên không được để trống")]
        [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
        public string FullName { get; set; }

        [Display(Name = "Chức vụ")]
        [StringLength(100, ErrorMessage = "Chức vụ không được vượt quá 100 ký tự")]
        public string Position { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }

        [Display(Name = "Liên hệ chính")]
        public bool IsPrimary { get; set; }
    }
}