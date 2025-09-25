using System;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Dto
{
    public class BusinessPartnerListDto
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Mã đối tác")]
        [Required(ErrorMessage = "Mã đối tác không được để trống")]
        [StringLength(20, ErrorMessage = "Mã đối tác không được vượt quá 20 ký tự")]
        public string PartnerCode { get; set; }

        [Display(Name = "Tên đối tác")]
        [Required(ErrorMessage = "Tên đối tác không được để trống")]
        [StringLength(200, ErrorMessage = "Tên đối tác không được vượt quá 200 ký tự")]
        public string PartnerName { get; set; }

        [Display(Name = "Loại đối tác")]
        public int PartnerType { get; set; }

        [Display(Name = "Loại đối tác")]
        public string PartnerTypeName { get; set; } // Customer / Vendor / Both

        [Display(Name = "Mã số thuế")]
        [StringLength(20, ErrorMessage = "Mã số thuế không được vượt quá 20 ký tự")]
        public string TaxCode { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(20, ErrorMessage = "Số điện thoại không được vượt quá 20 ký tự")]
        public string Phone { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }

        [Display(Name = "Thành phố")]
        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
    }
}