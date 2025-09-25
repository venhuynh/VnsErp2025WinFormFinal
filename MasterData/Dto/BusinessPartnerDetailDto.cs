using System;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Dto
{
    public class BusinessPartnerDetailDto
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
        [Required(ErrorMessage = "Loại đối tác không được để trống")]
        public int PartnerType { get; set; } // Enum value

        [Display(Name = "Loại đối tác")]
        public string PartnerTypeName { get; set; } // Text hiển thị

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

        [Display(Name = "Website")]
        [Url(ErrorMessage = "Website không đúng định dạng")]
        [StringLength(200, ErrorMessage = "Website không được vượt quá 200 ký tự")]
        public string Website { get; set; }

        [Display(Name = "Địa chỉ")]
        [StringLength(500, ErrorMessage = "Địa chỉ không được vượt quá 500 ký tự")]
        public string Address { get; set; }

        [Display(Name = "Thành phố")]
        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [Display(Name = "Quốc gia")]
        [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
        public string Country { get; set; }

        [Display(Name = "Người liên hệ")]
        [StringLength(100, ErrorMessage = "Tên người liên hệ không được vượt quá 100 ký tự")]
        public string ContactPerson { get; set; }

        [Display(Name = "Chức vụ")]
        [StringLength(100, ErrorMessage = "Chức vụ không được vượt quá 100 ký tự")]
        public string ContactPosition { get; set; }

        [Display(Name = "Số tài khoản ngân hàng")]
        [StringLength(50, ErrorMessage = "Số tài khoản ngân hàng không được vượt quá 50 ký tự")]
        public string BankAccount { get; set; }

        [Display(Name = "Tên ngân hàng")]
        [StringLength(200, ErrorMessage = "Tên ngân hàng không được vượt quá 200 ký tự")]
        public string BankName { get; set; }

        [Display(Name = "Hạn mức tín dụng")]
        [DataType(DataType.Currency)]
        [Range(0, double.MaxValue, ErrorMessage = "Hạn mức tín dụng phải lớn hơn hoặc bằng 0")]
        public decimal? CreditLimit { get; set; }

        [Display(Name = "Điều khoản thanh toán")]
        [StringLength(200, ErrorMessage = "Điều khoản thanh toán không được vượt quá 200 ký tự")]
        public string PaymentTerm { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Display(Name = "Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "Ngày cập nhật")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }
    }
}