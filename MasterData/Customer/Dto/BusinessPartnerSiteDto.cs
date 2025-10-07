using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Customer.Dto
{
    /// <summary>
    /// DTO cho chi tiết chi nhánh đối tác
    /// </summary>
    public class BusinessPartnerSiteDto
    {
        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("ID Đối tác")]
        [Required(ErrorMessage = "Vui lòng chọn đối tác")]
        public Guid PartnerId { get; set; }

        [DisplayName("Mã chi nhánh")]
        [Required(ErrorMessage = "Mã chi nhánh không được để trống")]
        [StringLength(50, ErrorMessage = "Mã chi nhánh không được vượt quá 50 ký tự")]
        public string SiteCode { get; set; }

        [DisplayName("Tên chi nhánh")]
        [Required(ErrorMessage = "Tên chi nhánh không được để trống")]
        [StringLength(255, ErrorMessage = "Tên chi nhánh không được vượt quá 255 ký tự")]
        public string SiteName { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Address { get; set; }

        [DisplayName("Thành phố")]
        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [DisplayName("Tỉnh/Thành phố")]
        [StringLength(100, ErrorMessage = "Tỉnh/Thành phố không được vượt quá 100 ký tự")]
        public string Province { get; set; }

        [DisplayName("Quốc gia")]
        [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
        public string Country { get; set; }

        [DisplayName("Người liên hệ")]
        [StringLength(100, ErrorMessage = "Người liên hệ không được vượt quá 100 ký tự")]
        public string ContactPerson { get; set; }

        [DisplayName("Số điện thoại")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [DisplayName("Mặc định")]
        public bool? IsDefault { get; set; }

        [DisplayName("Trạng thái")]
        public bool IsActive { get; set; }

        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Ngày cập nhật")]
        public DateTime? UpdatedDate { get; set; }
    }
}
