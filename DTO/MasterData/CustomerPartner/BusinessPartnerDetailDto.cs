using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.MasterData.CustomerPartner;

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

    [DisplayName("Trạng thái")]
    public bool IsActive { get; set; }

    [DisplayName("Tên file logo")]
    [StringLength(255, ErrorMessage = "Tên file logo không được vượt quá 255 ký tự")]
    public string LogoFileName { get; set; }

    [DisplayName("Đường dẫn tương đối logo")]
    [StringLength(500, ErrorMessage = "Đường dẫn tương đối logo không được vượt quá 500 ký tự")]
    public string LogoRelativePath { get; set; }

    [DisplayName("Đường dẫn đầy đủ logo")]
    [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ logo không được vượt quá 1000 ký tự")]
    public string LogoFullPath { get; set; }

    [DisplayName("Loại storage logo")]
    [StringLength(20, ErrorMessage = "Loại storage logo không được vượt quá 20 ký tự")]
    public string LogoStorageType { get; set; }

    [DisplayName("Kích thước file logo")]
    public long? LogoFileSize { get; set; }

    [DisplayName("Checksum logo")]
    [StringLength(64, ErrorMessage = "Checksum logo không được vượt quá 64 ký tự")]
    public string LogoChecksum { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime CreatedDate { get; set; }

    [DisplayName("Ngày cập nhật")]
    public DateTime? UpdatedDate { get; set; }
}