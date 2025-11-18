using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;

namespace DTO.MasterData.Company;

public class CompanyDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID công ty không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("Mã công ty")]
    [Required(ErrorMessage = "Mã công ty không được để trống")]
    [StringLength(50, ErrorMessage = "Mã công ty không được vượt quá 50 ký tự")]
    public string CompanyCode { get; set; }

    [DisplayName("Tên công ty")]
    [Required(ErrorMessage = "Tên công ty không được để trống")]
    [StringLength(255, ErrorMessage = "Tên công ty không được vượt quá 255 ký tự")]
    public string CompanyName { get; set; }

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
    [StringLength(100, ErrorMessage = "Website không được vượt quá 100 ký tự")]
    public string Website { get; set; }

    [DisplayName("Địa chỉ")]
    [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
    public string Address { get; set; }

    [DisplayName("Quốc gia")]
    [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
    public string Country { get; set; }

    [DisplayName("Ngày tạo")]
    [Required(ErrorMessage = "Ngày tạo không được để trống")]
    public DateTime CreatedDate { get; set; }

    [DisplayName("Ngày cập nhật")]
    public DateTime? UpdatedDate { get; set; }

    [DisplayName("Logo")]
    public byte[] Logo { get; set; }
}

/// <summary>
/// Converter để chuyển đổi giữa Company Entity và CompanyDto
/// </summary>
public static class CompanyConverter
{
    /// <summary>
    /// Chuyển đổi Company Entity sang CompanyDto
    /// </summary>
    /// <param name="entity">Company entity</param>
    /// <returns>CompanyDto</returns>
    public static CompanyDto ToDto(this Dal.DataContext.Company entity)
    {
        if (entity == null)
            return null;

        return new CompanyDto
        {
            Id = entity.Id,
            CompanyCode = entity.CompanyCode,
            CompanyName = entity.CompanyName,
            TaxCode = entity.TaxCode,
            Phone = entity.Phone,
            Email = entity.Email,
            Website = entity.Website,
            Address = entity.Address,
            Country = entity.Country,
            CreatedDate = entity.CreatedDate,
            UpdatedDate = entity.UpdatedDate,
            Logo = entity.Logo?.ToArray() // Convert Binary sang byte[]
        };
    }

    /// <summary>
    /// Chuyển đổi CompanyDto sang Company Entity
    /// </summary>
    /// <param name="dto">CompanyDto</param>
    /// <returns>Company entity</returns>
    public static Dal.DataContext.Company ToEntity(this CompanyDto dto)
    {
        if (dto == null)
            return null;

        return new Dal.DataContext.Company
        {
            Id = dto.Id,
            CompanyCode = dto.CompanyCode,
            CompanyName = dto.CompanyName,
            TaxCode = dto.TaxCode,
            Phone = dto.Phone,
            Email = dto.Email,
            Website = dto.Website,
            Address = dto.Address,
            Country = dto.Country,
            CreatedDate = dto.CreatedDate,
            UpdatedDate = dto.UpdatedDate,
            Logo = dto.Logo != null ? new Binary(dto.Logo) : null
        };
    }
}