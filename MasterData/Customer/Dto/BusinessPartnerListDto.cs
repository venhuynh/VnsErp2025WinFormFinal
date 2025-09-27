using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Customer.Dto
{
    public class BusinessPartnerListDto
    {
        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("Mã đối tác")]
        [Required(ErrorMessage = "Mã đối tác không được để trống")]
        [StringLength(50, ErrorMessage = "Mã đối tác không được vượt quá 50 ký tự")]
        public string PartnerCode { get; set; }

        [DisplayName("Tên đối tác")]
        [Required(ErrorMessage = "Tên đối tác không được để trống")]
        [StringLength(255, ErrorMessage = "Tên đối tác không được vượt quá 255 ký tự")]
        public string PartnerName { get; set; }

        [DisplayName("Loại đối tác")]
        public int PartnerType { get; set; }

        [DisplayName("Loại đối tác")]
        public string PartnerTypeName { get; set; } // Customer / Vendor / Both

        [DisplayName("Mã số thuế")]
        [StringLength(50, ErrorMessage = "Mã số thuế không được vượt quá 50 ký tự")]
        public string TaxCode { get; set; }

        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }

        [DisplayName("Thành phố")]
        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [DisplayName("Trạng thái")]
        public bool IsActive { get; set; }

        [DisplayName("Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }
    }
}