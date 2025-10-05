using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Company.Dto.Company
{
    public class CompanyBranchDto
    {
        [DisplayName("ID")]
        public Guid Id { get; set; }

        [DisplayName("ID công ty")]
        [Required(ErrorMessage = "ID công ty không được để trống")]
        public Guid CompanyId { get; set; }

        [DisplayName("Mã chi nhánh")]
        [Required(ErrorMessage = "Mã chi nhánh không được để trống")]
        [StringLength(50, ErrorMessage = "Mã chi nhánh không được vượt quá 50 ký tự")]
        public string BranchCode { get; set; }

        [DisplayName("Tên chi nhánh")]
        [Required(ErrorMessage = "Tên chi nhánh không được để trống")]
        [StringLength(255, ErrorMessage = "Tên chi nhánh không được vượt quá 255 ký tự")]
        public string BranchName { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Address { get; set; }

        [DisplayName("Số điện thoại")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        [DisplayName("Tên quản lý")]
        [StringLength(100, ErrorMessage = "Tên quản lý không được vượt quá 100 ký tự")]
        public string ManagerName { get; set; }

        [DisplayName("Trạng thái hoạt động")]
        public bool IsActive { get; set; }
    }
}
