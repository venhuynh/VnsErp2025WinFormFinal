using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Company.Dto
{
    public class PositionDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID chức vụ không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("ID Công ty")]
        [Required(ErrorMessage = "ID công ty không được để trống")]
        public Guid CompanyId { get; set; }

        [DisplayName("Mã chức vụ")]
        [Required(ErrorMessage = "Mã chức vụ không được để trống")]
        [StringLength(50, ErrorMessage = "Mã chức vụ không được vượt quá 50 ký tự")]
        public string PositionCode { get; set; }

        [DisplayName("Tên chức vụ")]
        [Required(ErrorMessage = "Tên chức vụ không được để trống")]
        [StringLength(255, ErrorMessage = "Tên chức vụ không được vượt quá 255 ký tự")]
        public string PositionName { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string Description { get; set; }

        [DisplayName("Cấp quản lý")]
        public bool? IsManagerLevel { get; set; }

        [DisplayName("Trạng thái hoạt động")]
        [Required(ErrorMessage = "Trạng thái hoạt động không được để trống")]
        public bool IsActive { get; set; }

        // Navigation properties for display purposes
        [DisplayName("Tên công ty")]
        public string CompanyName { get; set; }
    }
}
