using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Company.Dto
{
    /// <summary>
    /// Data Transfer Object cho Department (Phòng ban)
    /// </summary>
    public class DepartmentDto
    {
        /// <summary>
        /// ID phòng ban
        /// </summary>
        [Required(ErrorMessage = "ID phòng ban không được để trống")]
        public Guid Id { get; set; }

        /// <summary>
        /// ID công ty
        /// </summary>
        [Required(ErrorMessage = "ID công ty không được để trống")]
        public Guid CompanyId { get; set; }

        /// <summary>
        /// ID chi nhánh (tùy chọn)
        /// </summary>
        public Guid? BranchId { get; set; }

        /// <summary>
        /// Mã phòng ban
        /// </summary>
        [Required(ErrorMessage = "Mã phòng ban không được để trống")]
        [StringLength(50, ErrorMessage = "Mã phòng ban không được vượt quá 50 ký tự")]
        [DisplayName("Mã phòng ban")]
        public string DepartmentCode { get; set; }

        /// <summary>
        /// Tên phòng ban
        /// </summary>
        [Required(ErrorMessage = "Tên phòng ban không được để trống")]
        [StringLength(255, ErrorMessage = "Tên phòng ban không được vượt quá 255 ký tự")]
        [DisplayName("Tên phòng ban")]
        public string DepartmentName { get; set; }

        /// <summary>
        /// ID phòng ban cha (tùy chọn)
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        [DisplayName("Mô tả")]
        public string Description { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        [DisplayName("Trạng thái hoạt động")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        [DisplayName("Ngày tạo")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        [DisplayName("Ngày cập nhật")]
        public DateTime? ModifiedDate { get; set; }

        #region ========== NAVIGATION PROPERTIES ==========

        /// <summary>
        /// Tên công ty (để hiển thị)
        /// </summary>
        [DisplayName("Tên công ty")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Tên chi nhánh (để hiển thị)
        /// </summary>
        [DisplayName("Tên chi nhánh")]
        public string BranchName { get; set; }

        /// <summary>
        /// Tên phòng ban cha (để hiển thị)
        /// </summary>
        [DisplayName("Phòng ban cha")]
        public string ParentDepartmentName { get; set; }

        /// <summary>
        /// Số lượng nhân viên
        /// </summary>
        [DisplayName("Số nhân viên")]
        public int EmployeeCount { get; set; }

        /// <summary>
        /// Số lượng phòng ban con
        /// </summary>
        [DisplayName("Số phòng ban con")]
        public int SubDepartmentCount { get; set; }

        #endregion

        #region ========== CONSTRUCTOR ==========

        /// <summary>
        /// Khởi tạo DTO mặc định
        /// </summary>
        public DepartmentDto()
        {
            Id = Guid.NewGuid();
            IsActive = true;
            CreatedDate = DateTime.Now;
        }

        #endregion

        #region ========== HELPER METHODS ==========

        /// <summary>
        /// Kiểm tra có phòng ban con không
        /// </summary>
        public bool HasSubDepartments => SubDepartmentCount > 0;

        /// <summary>
        /// Kiểm tra có nhân viên không
        /// </summary>
        public bool HasEmployees => EmployeeCount > 0;

        /// <summary>
        /// Kiểm tra có phòng ban cha không
        /// </summary>
        public bool HasParent => ParentId.HasValue;

        /// <summary>
        /// Lấy tên hiển thị đầy đủ
        /// </summary>
        public string FullDisplayName => $"{DepartmentCode} - {DepartmentName}";

        /// <summary>
        /// Lấy thông tin phân cấp
        /// </summary>
        public string HierarchyInfo
        {
            get
            {
                if (HasParent)
                    return $"Phòng ban con của {ParentDepartmentName}";
                else
                    return "Phòng ban cấp cao";
            }
        }

        #endregion
    }
}
