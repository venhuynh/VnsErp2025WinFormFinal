using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.Company.Dto
{
    /// <summary>
    /// Data Transfer Object cho chi nhánh công ty.
    /// Chứa thông tin chi nhánh công ty để hiển thị và xử lý.
    /// </summary>
    public class CompanyBranchDto
    {
        #region ========== KHAI BÁO BIẾN ==========

        /// <summary>
        /// ID của chi nhánh công ty
        /// </summary>
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID chi nhánh không được để trống")]
        public Guid Id { get; set; }

        /// <summary>
        /// ID của công ty (bắt buộc)
        /// </summary>
        [DisplayName("ID công ty")]
        [Required(ErrorMessage = "ID công ty không được để trống")]
        public Guid CompanyId { get; set; }

        /// <summary>
        /// Mã chi nhánh (bắt buộc, tối đa 50 ký tự)
        /// </summary>
        [DisplayName("Mã chi nhánh")]
        [Required(ErrorMessage = "Mã chi nhánh không được để trống")]
        [StringLength(50, ErrorMessage = "Mã chi nhánh không được vượt quá 50 ký tự")]
        public string BranchCode { get; set; }

        /// <summary>
        /// Tên chi nhánh (bắt buộc, tối đa 255 ký tự)
        /// </summary>
        [DisplayName("Tên chi nhánh")]
        [Required(ErrorMessage = "Tên chi nhánh không được để trống")]
        [StringLength(255, ErrorMessage = "Tên chi nhánh không được vượt quá 255 ký tự")]
        public string BranchName { get; set; }

        /// <summary>
        /// Địa chỉ (tối đa 255 ký tự)
        /// </summary>
        [DisplayName("Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Address { get; set; }

        /// <summary>
        /// Số điện thoại (tối đa 50 ký tự)
        /// </summary>
        [DisplayName("Số điện thoại")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        /// <summary>
        /// Email (tối đa 100 ký tự)
        /// </summary>
        [DisplayName("Email")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; }

        /// <summary>
        /// Tên người quản lý (tối đa 100 ký tự)
        /// </summary>
        [DisplayName("Tên người quản lý")]
        [StringLength(100, ErrorMessage = "Tên người quản lý không được vượt quá 100 ký tự")]
        public string ManagerName { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        [DisplayName("Trạng thái hoạt động")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        [DisplayName("Ngày tạo")]
        [Required(ErrorMessage = "Ngày tạo không được để trống")]
        public DateTime CreatedDate { get; set; }

        /// <summary>
        /// Ngày cập nhật
        /// </summary>
        [DisplayName("Ngày cập nhật")]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Địa chỉ đầy đủ (computed property)
        /// </summary>
        public string FullAddress
        {
            get
            {
                var addressParts = new List<string>();
                
                if (!string.IsNullOrWhiteSpace(Address))
                    addressParts.Add(Address.Trim());
                
                if (!string.IsNullOrWhiteSpace(Phone))
                    addressParts.Add($"ĐT: {Phone.Trim()}");
                
                if (!string.IsNullOrWhiteSpace(Email))
                    addressParts.Add($"Email: {Email.Trim()}");
                
                return string.Join(" | ", addressParts);
            }
        }

        /// <summary>
        /// Thông tin chi nhánh đầy đủ (computed property)
        /// </summary>
        public string FullBranchInfo
        {
            get
            {
                var infoParts = new List<string>();
                
                if (!string.IsNullOrWhiteSpace(BranchName))
                    infoParts.Add(BranchName.Trim());
                
                if (!string.IsNullOrWhiteSpace(ManagerName))
                    infoParts.Add($"QL: {ManagerName.Trim()}");
                
                if (!string.IsNullOrWhiteSpace(FullAddress))
                    infoParts.Add(FullAddress);
                
                return string.Join(" - ", infoParts);
            }
        }

        #endregion

        #region ========== CONSTRUCTOR & PUBLIC METHODS ==========

        /// <summary>
        /// Khởi tạo DTO cho chi nhánh công ty.
        /// </summary>
        public CompanyBranchDto()
        {
            Id = Guid.Empty;
            CompanyId = Guid.Empty;
            BranchCode = string.Empty;
            BranchName = string.Empty;
            Address = string.Empty;
            Phone = string.Empty;
            Email = string.Empty;
            ManagerName = string.Empty;
            IsActive = true;
            CreatedDate = DateTime.Now;
            ModifiedDate = DateTime.Now;
        }

        #endregion
    }
}