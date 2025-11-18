using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using Dal.DataContext;

namespace DTO.MasterData.CustomerPartner;

public class BusinessPartnerContactDto
{
    [DisplayName("ID")]
    public Guid Id { get; set; }

    [DisplayName("ID chi nhánh")]
    [Required(ErrorMessage = "ID chi nhánh không được để trống")]
    public Guid SiteId { get; set; }

    [DisplayName("Tên chi nhánh")]
    [Required(ErrorMessage = "Tên chi nhánh không được để trống")]
    [Description("Tên chi nhánh mà contact này thuộc về (chỉ để hiển thị)")]
    public string SiteName { get; set; }

    [DisplayName("Họ và tên")]
    [Required(ErrorMessage = "Họ và tên không được để trống")]
    [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
    public string FullName { get; set; }

    [DisplayName("Chức vụ")]
    [StringLength(100, ErrorMessage = "Chức vụ không được vượt quá 100 ký tự")]
    public string Position { get; set; }

    [DisplayName("Số điện thoại")]
    [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
    [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
    public string Phone { get; set; }

    [DisplayName("Email")]
    [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
    public string Email { get; set; }

    [DisplayName("Liên hệ chính")]
    public bool IsPrimary { get; set; }

    [DisplayName("Ảnh đại diện")]
    [Description("Ảnh đại diện của liên hệ")]
    public byte[] Avatar { get; set; }

    [DisplayName("Trạng thái")]
    public bool IsActive { get; set; }
}


public static class BusinessPartnerContactConverters
{
    public static BusinessPartnerContactDto ToDto(this BusinessPartnerContact entity, string siteName = null)
    {
        if (entity == null) return null;

        // Sử dụng siteName từ parameter hoặc từ navigation property (đã được include)
        var finalSiteName = siteName ?? entity.BusinessPartnerSite?.SiteName;

        return new BusinessPartnerContactDto
        {
            Id = entity.Id,
            SiteId = entity.SiteId,
            SiteName = finalSiteName,
            FullName = entity.FullName,
            Position = entity.Position,
            Phone = entity.Phone,
            Email = entity.Email,
            IsPrimary = entity.IsPrimary,
            Avatar = entity.Avatar?.ToArray(),
            IsActive = entity.IsActive
        };
    }

    public static BusinessPartnerContact ToEntity(this BusinessPartnerContactDto dto,
        BusinessPartnerContact destination = null)
    {
        if (dto == null) return null;
        var entity = destination ?? new BusinessPartnerContact();
        if (dto.Id != Guid.Empty) entity.Id = dto.Id;
        entity.SiteId = dto.SiteId;
        entity.FullName = dto.FullName;
        entity.Position = dto.Position;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.IsPrimary = dto.IsPrimary;
        entity.Avatar = dto.Avatar != null ? new Binary(dto.Avatar) : null;
        entity.IsActive = dto.IsActive;
        return entity;
    }
}
