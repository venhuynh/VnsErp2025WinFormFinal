using Dal.DataContext;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.MasterData.Company;

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

/// <summary>
/// Converter giữa Department Entity và DepartmentDto
/// </summary>
public static class DepartmentConverters
{
    /// <summary>
    /// Chuyển đổi từ Department Entity sang DepartmentDto
    /// </summary>
    /// <param name="entity">Department Entity</param>
    /// <param name="companyName">Tên công ty (tùy chọn, nếu đã có sẵn)</param>
    /// <param name="branchName">Tên chi nhánh (tùy chọn, nếu đã có sẵn)</param>
    /// <param name="parentDepartmentName">Tên phòng ban cha (tùy chọn, nếu đã có sẵn)</param>
    /// <returns>DepartmentDto</returns>
    public static DepartmentDto ToDto(this Department entity, string companyName = null, string branchName = null, string parentDepartmentName = null)
    {
        if (entity == null)
            return null;

        // Sử dụng tham số hoặc navigation properties (đã được include)

        var finalBranchName = branchName ?? entity.CompanyBranch?.BranchName;
        var finalParentDepartmentName = parentDepartmentName ?? entity.Department1?.DepartmentName ?? "Không xác định";

        return new DepartmentDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            BranchId = entity.BranchId,
            DepartmentCode = entity.DepartmentCode,
            DepartmentName = entity.DepartmentName,
            ParentId = entity.ParentId,
            Description = entity.Description,
            IsActive = entity.IsActive,

            // Navigation properties - sử dụng giá trị mặc định để tránh DataContext disposed
            BranchName = finalBranchName,
            ParentDepartmentName = finalParentDepartmentName,
            EmployeeCount = 0, // Sẽ được tính toán riêng nếu cần
            SubDepartmentCount = 0 // Sẽ được tính toán riêng nếu cần
        };
    }

    /// <summary>
    /// Chuyển đổi từ DepartmentDto sang Department Entity
    /// </summary>
    /// <param name="dto">DepartmentDto</param>
    /// <param name="destination">Department Entity đích (tùy chọn, cho update)</param>
    /// <returns>Department Entity</returns>
    public static Department ToEntity(this DepartmentDto dto, Department destination = null)
    {
        if (dto == null) return null;
        var entity = destination ?? new Department();
        if (dto.Id != Guid.Empty) entity.Id = dto.Id;
        entity.CompanyId = dto.CompanyId;
        entity.BranchId = dto.BranchId;
        entity.DepartmentCode = dto.DepartmentCode;
        entity.DepartmentName = dto.DepartmentName;
        entity.ParentId = dto.ParentId;
        entity.Description = dto.Description;
        entity.IsActive = dto.IsActive;
        return entity;
    }


}