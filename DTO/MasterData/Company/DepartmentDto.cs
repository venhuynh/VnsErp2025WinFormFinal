using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

    /// <summary>
    /// Đường dẫn đầy đủ từ gốc đến phòng ban này
    /// Ví dụ: "Phòng ban A > Phòng ban A1"
    /// </summary>
    [DisplayName("Đường dẫn đầy đủ")]
    [Description("Đường dẫn đầy đủ từ gốc đến phòng ban này")]
    public string FullPath { get; set; }

    /// <summary>
    /// Đường dẫn đầy đủ dưới dạng HTML theo format DevExpress
    /// </summary>
    [DisplayName("Đường dẫn HTML")]
    [Description("Đường dẫn đầy đủ từ gốc đến phòng ban này dưới dạng HTML")]
    public string FullPathHtml
    {
        get
        {
            if (string.IsNullOrWhiteSpace(FullPath))
            {
                return string.Empty;
            }

            // Tách đường dẫn và format với màu sắc
            // Hỗ trợ nhiều format: " > ", ">", " >" hoặc "> "
            var parts = FullPath.Split(new[] { " > ", ">", " >", "> " }, StringSplitOptions.None)
                .Where(p => !string.IsNullOrWhiteSpace(p))
                .Select(p => p.Trim())
                .ToArray();
            
            var htmlParts = new List<string>();

            for (int i = 0; i < parts.Length; i++)
            {
                var isLast = i == parts.Length - 1;
                var color = isLast ? "blue" : "#757575";
                var weight = isLast ? "<b>" : "";
                var weightClose = isLast ? "</b>" : "";

                // Format giống ThongTinHtml: không dùng <size>, chỉ dùng <b> và <color>
                htmlParts.Add($"{weight}<color='{color}'>{parts[i]}</color>{weightClose}");
            }

            var result = string.Join(" <color='#757575'>></color> ", htmlParts);
            return result;
        }
    }

    /// <summary>
    /// Thông tin phòng ban dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin phòng ban dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var departmentName = DepartmentName ?? string.Empty;
            var departmentCode = DepartmentCode ?? string.Empty;
            var branchName = BranchName ?? string.Empty;
            var parentDepartmentName = ParentDepartmentName ?? string.Empty;
            var description = Description ?? string.Empty;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên phòng ban: font lớn, bold, màu xanh đậm (primary)
            // - Mã phòng ban: font nhỏ hơn, màu xám
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

            var html = $"<b><color='blue'>{departmentName}</color></b>";

            if (!string.IsNullOrWhiteSpace(departmentCode))
            {
                html += $" <color='#757575'>({departmentCode})</color>";
            }

            html += "<br>";

            var infoParts = new List<string>();

            if (!string.IsNullOrWhiteSpace(branchName))
            {
                infoParts.Add($"<color='#757575'>Chi nhánh:</color> <b>{branchName}</b>");
            }

            if (!string.IsNullOrWhiteSpace(parentDepartmentName) && parentDepartmentName != "Không xác định")
            {
                infoParts.Add($"<color='#757575'>Phòng ban cha:</color> <b>{parentDepartmentName}</b>");
            }

            if (EmployeeCount > 0)
            {
                infoParts.Add($"<color='#757575'>Nhân viên:</color> <b>{EmployeeCount}</b>");
            }

            if (SubDepartmentCount > 0)
            {
                infoParts.Add($"<color='#757575'>Phòng ban con:</color> <b>{SubDepartmentCount}</b>");
            }

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
            }

            if (!string.IsNullOrWhiteSpace(description))
            {
                html += $"<color='#757575'>Mô tả:</color> <b>{description}</b><br>";
            }

            html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

            return html;
        }
    }
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
    /// <param name="departmentDict">Dictionary chứa tất cả Department entities để tính FullPath (optional, chỉ cần khi tính full path)</param>
    /// <returns>DepartmentDto</returns>
    public static DepartmentDto ToDto(this Department entity, string companyName = null, string branchName = null, string parentDepartmentName = null, Dictionary<Guid, Department> departmentDict = null)
    {
        if (entity == null)
            return null;

        // Sử dụng tham số hoặc navigation properties (đã được include)

        var finalBranchName = branchName ?? entity.CompanyBranch?.BranchName;
        var finalParentDepartmentName = parentDepartmentName ?? entity.Department1?.DepartmentName ?? "Không xác định";

        var dto = new DepartmentDto
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

        // Tính FullPath: chỉ dùng departmentDict nếu có, nếu không chỉ dùng DepartmentName
        // KHÔNG tính từ navigation properties vì sẽ gây lỗi "Cannot access a disposed object"
        if (departmentDict != null && departmentDict.ContainsKey(entity.Id))
        {
            dto.FullPath = CalculateDepartmentFullPath(entity, departmentDict);
        }
        else
        {
            // Không có departmentDict, chỉ dùng DepartmentName
            // KHÔNG cố tính từ navigation properties vì DataContext đã bị dispose
            dto.FullPath = entity.DepartmentName;
        }

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách Department Entity sang danh sách DepartmentDto
    /// </summary>
    /// <param name="entities">Danh sách Department Entity</param>
    /// <param name="departmentDict">Dictionary chứa tất cả Department entities để tính FullPath (optional)</param>
    /// <returns>Danh sách DepartmentDto</returns>
    public static IEnumerable<DepartmentDto> ToDepartmentDtos(
        this IEnumerable<Department> entities,
        Dictionary<Guid, Department> departmentDict = null)
    {
        if (entities == null) return [];

        return entities.Select(entity => entity.ToDto(departmentDict: departmentDict));
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

    /// <summary>
    /// Tính toán đường dẫn đầy đủ từ gốc đến department sử dụng departmentDict
    /// </summary>
    /// <param name="department">Department entity</param>
    /// <param name="departmentDict">Dictionary chứa tất cả Department entities</param>
    /// <returns>Đường dẫn đầy đủ (ví dụ: "Phòng ban A > Phòng ban A1")</returns>
    private static string CalculateDepartmentFullPath(Department department,
        Dictionary<Guid, Department> departmentDict)
    {
        if (department == null) return string.Empty;

        var pathParts = new List<string> { department.DepartmentName };
        var current = department;

        while (current.ParentId.HasValue && departmentDict.ContainsKey(current.ParentId.Value))
        {
            current = departmentDict[current.ParentId.Value];
            pathParts.Insert(0, current.DepartmentName);
            if (pathParts.Count > 10) break; // Tránh infinite loop
        }

        return string.Join(" > ", pathParts);
    }
}