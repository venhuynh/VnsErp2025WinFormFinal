using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.MasterData.Company;

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

/// <summary>
/// Converter để chuyển đổi giữa Position Entity và PositionDto
/// </summary>
public static class PositionConverter
{
    /// <summary>
    /// Chuyển đổi Position Entity sang PositionDto
    /// </summary>
    /// <param name="entity">Position entity</param>
    /// <returns>PositionDto</returns>
    public static PositionDto ToDto(this Dal.DataContext.Position entity)
    {
        if (entity == null)
            return null;

        return new PositionDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            PositionCode = entity.PositionCode,
            PositionName = entity.PositionName,
            Description = entity.Description,
            IsManagerLevel = entity.IsManagerLevel,
            IsActive = entity.IsActive,
            CompanyName = entity.Company?.CompanyName // Lấy tên công ty từ navigation property
        };
    }

    /// <summary>
    /// Chuyển đổi PositionDto sang Position Entity
    /// </summary>
    /// <param name="dto">PositionDto</param>
    /// <returns>Position entity</returns>
    public static Dal.DataContext.Position ToEntity(this PositionDto dto)
    {
        if (dto == null)
            return null;

        return new Dal.DataContext.Position
        {
            Id = dto.Id,
            CompanyId = dto.CompanyId,
            PositionCode = dto.PositionCode,
            PositionName = dto.PositionName,
            Description = dto.Description,
            IsManagerLevel = dto.IsManagerLevel,
            IsActive = dto.IsActive
        };
    }
}