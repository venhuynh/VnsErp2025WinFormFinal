using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Customer.Dto
{
    /// <summary>
    /// DTO cho chi tiết đối tác (đầy đủ thông tin để lưu)
    /// </summary>
    public class BusinessPartnerDetailDto
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
        [StringLength(255, ErrorMessage = "Website không được vượt quá 255 ký tự")]
        public string Website { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Address { get; set; }

        [DisplayName("Thành phố")]
        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [DisplayName("Quốc gia")]
        [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
        public string Country { get; set; }

        [DisplayName("Người liên hệ")]
        [StringLength(100, ErrorMessage = "Người liên hệ không được vượt quá 100 ký tự")]
        public string ContactPerson { get; set; }

        [DisplayName("Chức vụ")]
        [StringLength(100, ErrorMessage = "Chức vụ không được vượt quá 100 ký tự")]
        public string ContactPosition { get; set; }

        [DisplayName("Số tài khoản")]
        [StringLength(50, ErrorMessage = "Số tài khoản không được vượt quá 50 ký tự")]
        public string BankAccount { get; set; }

        [DisplayName("Tên ngân hàng")]
        [StringLength(255, ErrorMessage = "Tên ngân hàng không được vượt quá 255 ký tự")]
        public string BankName { get; set; }

        [DisplayName("Hạn mức tín dụng")]
        public decimal? CreditLimit { get; set; }

        [DisplayName("Điều khoản thanh toán")]
        [StringLength(255, ErrorMessage = "Điều khoản thanh toán không được vượt quá 255 ký tự")]
        public string PaymentTerm { get; set; }

        [DisplayName("Trạng thái")]
        public bool IsActive { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }
    }
}