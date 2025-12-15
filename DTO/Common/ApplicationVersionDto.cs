using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Common;

/// <summary>
/// DTO cho quản lý phiên bản ứng dụng
/// </summary>
public class ApplicationVersionDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("Phiên bản")]
    [Required(ErrorMessage = "Phiên bản không được để trống")]
    [StringLength(50, ErrorMessage = "Phiên bản không được vượt quá 50 ký tự")]
    public string Version { get; set; }

    [DisplayName("Ngày phát hành")]
    [Required(ErrorMessage = "Ngày phát hành không được để trống")]
    public DateTime ReleaseDate { get; set; }

    [DisplayName("Đang hoạt động")]
    public bool IsActive { get; set; }

    [DisplayName("Mô tả")]
    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
    public string Description { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime CreateDate { get; set; }

    [DisplayName("Người tạo")]
    public Guid? CreateBy { get; set; }

    [DisplayName("Ngày cập nhật")]
    public DateTime? ModifiedDate { get; set; }

    [DisplayName("Người cập nhật")]
    public Guid? ModifiedBy { get; set; }
}
