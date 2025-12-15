using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Common;

/// <summary>
/// DTO cho quản lý MAC address được phép sử dụng ứng dụng
/// </summary>
public class AllowedMacAddressDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("Địa chỉ MAC")]
    [Required(ErrorMessage = "Địa chỉ MAC không được để trống")]
    [StringLength(50, ErrorMessage = "Địa chỉ MAC không được vượt quá 50 ký tự")]
    [RegularExpression(@"^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$|^[0-9A-Fa-f]{12}$", 
        ErrorMessage = "Địa chỉ MAC không đúng định dạng (ví dụ: XX-XX-XX-XX-XX-XX hoặc XXXXXXXXXXXX)")]
    public string MacAddress { get; set; }

    [DisplayName("Tên máy tính")]
    [StringLength(255, ErrorMessage = "Tên máy tính không được vượt quá 255 ký tự")]
    public string ComputerName { get; set; }

    [DisplayName("Mô tả")]
    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
    public string Description { get; set; }

    [DisplayName("Đang hoạt động")]
    public bool IsActive { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime CreateDate { get; set; }

    [DisplayName("Người tạo")]
    public Guid? CreateBy { get; set; }

    [DisplayName("Ngày cập nhật")]
    public DateTime? ModifiedDate { get; set; }

    [DisplayName("Người cập nhật")]
    public Guid? ModifiedBy { get; set; }
}
