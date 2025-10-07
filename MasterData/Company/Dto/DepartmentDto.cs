using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Company.Dto
{
    public class DepartmentDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID phòng ban không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("ID công ty")]
        [Required(ErrorMessage = "ID công ty không được để trống")]
        public Guid CompanyId { get; set; }

        [DisplayName("ID chi nhánh")]
        public Guid? BranchId { get; set; }

        [DisplayName("Mã phòng ban")]
        [Required(ErrorMessage = "Mã phòng ban không được để trống")]
        [StringLength(50, ErrorMessage = "Mã phòng ban không được vượt quá 50 ký tự")]
        public string DepartmentCode { get; set; }

        [DisplayName("Tên phòng ban")]
        [Required(ErrorMessage = "Tên phòng ban không được để trống")]
        [StringLength(255, ErrorMessage = "Tên phòng ban không được vượt quá 255 ký tự")]
        public string DepartmentName { get; set; }

        [DisplayName("ID phòng ban cha")]
        public Guid? ParentId { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string Description { get; set; }

        [DisplayName("Trạng thái hoạt động")]
        public bool IsActive { get; set; }


        [DisplayName("Tên chi nhánh")]
        public string BranchName { get; set; }

        [DisplayName("Phòng ban cha")]
        public string ParentDepartmentName { get; set; }

        [DisplayName("Số nhân viên")]
        public int EmployeeCount { get; set; }

        [DisplayName("Số phòng ban con")]
        public int SubDepartmentCount { get; set; }
    }
}
