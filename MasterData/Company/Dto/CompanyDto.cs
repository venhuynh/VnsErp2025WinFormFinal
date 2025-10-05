using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Company.Dto
{
    public class CompanyDto
    {
        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("Mã công ty")]
        [Required(ErrorMessage = "Mã công ty không được để trống")]
        [StringLength(50, ErrorMessage = "Mã công ty không được vượt quá 50 ký tự")]
        public string CompanyCode { get; set; }

        [DisplayName("Tên công ty")]
        [Required(ErrorMessage = "Tên công ty không được để trống")]
        [StringLength(255, ErrorMessage = "Tên công ty không được vượt quá 255 ký tự")]
        public string CompanyName { get; set; }

        [DisplayName("Mã số thuế")]
        [StringLength(50, ErrorMessage = "Mã số thuế không được vượt quá 50 ký tự")]
        public string TaxCode { get; set; }

        [DisplayName("Số điện thoại")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [DisplayName("Website")]
        [StringLength(100, ErrorMessage = "Website không được vượt quá 100 ký tự")]
        public string Website { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Address { get; set; }

        [DisplayName("Quốc gia")]
        [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
        public string Country { get; set; }

        [DisplayName("Đường dẫn logo")]
        [StringLength(500, ErrorMessage = "Đường dẫn logo không được vượt quá 500 ký tự")]
        public string LogoPath { get; set; }
    }
}
