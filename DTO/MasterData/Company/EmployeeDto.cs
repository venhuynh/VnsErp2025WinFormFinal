using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.Company;

public class EmployeeDto
{
    [DisplayName("ID")]
    [Required(ErrorMessage = "ID nhân viên không được để trống")]
    public Guid Id { get; set; }

    [DisplayName("ID công ty")]
    [Required(ErrorMessage = "ID công ty không được để trống")]
    public Guid CompanyId { get; set; }

    [DisplayName("ID chi nhánh")]
    public Guid? BranchId { get; set; }

    [DisplayName("ID phòng ban")]
    public Guid? DepartmentId { get; set; }

    [DisplayName("ID chức vụ")]
    public Guid? PositionId { get; set; }

    [DisplayName("Mã nhân viên")]
    [Required(ErrorMessage = "Mã nhân viên không được để trống")]
    [StringLength(50, ErrorMessage = "Mã nhân viên không được vượt quá 50 ký tự")]
    public string EmployeeCode { get; set; }

    [DisplayName("Họ và tên")]
    [Required(ErrorMessage = "Họ và tên không được để trống")]
    [StringLength(100, ErrorMessage = "Họ và tên không được vượt quá 100 ký tự")]
    public string FullName { get; set; }

    [DisplayName("Giới tính")]
    [StringLength(10, ErrorMessage = "Giới tính không được vượt quá 10 ký tự")]
    public string Gender { get; set; }

    [DisplayName("Ngày sinh")]
    public DateTime? BirthDate { get; set; }

    [DisplayName("Số điện thoại")]
    [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
    public string Phone { get; set; }

    [DisplayName("Email")]
    [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
    public string Email { get; set; }

    [DisplayName("Ngày vào làm")]
    public DateTime? HireDate { get; set; }

    [DisplayName("Ngày nghỉ việc")]
    public DateTime? ResignDate { get; set; }

    [DisplayName("Đường dẫn ảnh đại diện")]
    [StringLength(500, ErrorMessage = "Đường dẫn ảnh đại diện không được vượt quá 500 ký tự")]
    public string AvatarPath { get; set; }

    [DisplayName("Trạng thái hoạt động")]
    public bool IsActive { get; set; }

    [DisplayName("Số điện thoại di động")]
    [StringLength(50, ErrorMessage = "Số điện thoại di động không được vượt quá 50 ký tự")]
    public string Mobile { get; set; }

    [DisplayName("Số fax")]
    [StringLength(50, ErrorMessage = "Số fax không được vượt quá 50 ký tự")]
    public string Fax { get; set; }

    [DisplayName("LinkedIn")]
    [StringLength(255, ErrorMessage = "LinkedIn không được vượt quá 255 ký tự")]
    public string LinkedIn { get; set; }

    [DisplayName("Skype")]
    [StringLength(100, ErrorMessage = "Skype không được vượt quá 100 ký tự")]
    public string Skype { get; set; }

    [DisplayName("WeChat")]
    [StringLength(100, ErrorMessage = "WeChat không được vượt quá 100 ký tự")]
    public string WeChat { get; set; }

    [DisplayName("Ghi chú")]
    [StringLength(1000, ErrorMessage = "Ghi chú không được vượt quá 1000 ký tự")]
    public string Notes { get; set; }

    [DisplayName("Ngày tạo")]
    public DateTime CreatedDate { get; set; }

    [DisplayName("Ngày cập nhật")]
    public DateTime? ModifiedDate { get; set; }

    [DisplayName("Tên file Avatar")]
    [StringLength(255, ErrorMessage = "Tên file Avatar không được vượt quá 255 ký tự")]
    public string AvatarFileName { get; set; }

    [DisplayName("Đường dẫn tương đối Avatar")]
    [StringLength(500, ErrorMessage = "Đường dẫn tương đối Avatar không được vượt quá 500 ký tự")]
    public string AvatarRelativePath { get; set; }

    [DisplayName("Đường dẫn đầy đủ Avatar")]
    [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ Avatar không được vượt quá 1000 ký tự")]
    public string AvatarFullPath { get; set; }

    [DisplayName("Loại lưu trữ Avatar")]
    [StringLength(20, ErrorMessage = "Loại lưu trữ Avatar không được vượt quá 20 ký tự")]
    public string AvatarStorageType { get; set; }

    [DisplayName("Kích thước file Avatar")]
    public long? AvatarFileSize { get; set; }

    [DisplayName("Checksum Avatar")]
    [StringLength(64, ErrorMessage = "Checksum Avatar không được vượt quá 64 ký tự")]
    public string AvatarChecksum { get; set; }

    [DisplayName("Dữ liệu thumbnail Avatar")]
    public byte[] AvatarThumbnailData { get; set; }

    // Navigation properties for display purposes
    [DisplayName("Tên công ty")]
    public string CompanyName { get; set; }

    [DisplayName("Tên chi nhánh")]
    public string BranchName { get; set; }

    [DisplayName("Tên phòng ban")]
    public string DepartmentName { get; set; }

    [DisplayName("Tên chức vụ")]
    public string PositionName { get; set; }

    /// <summary>
    /// Thông tin nhân viên dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Format giống DepartmentDto.ThongTinHtml (không dùng &lt;size&gt;)
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin nhân viên dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var html = string.Empty;

            // Họ và tên (màu xanh, đậm)
            if (!string.IsNullOrWhiteSpace(FullName))
            {
                html += $"<b><color='blue'>{FullName}</color></b>";
            }

            // Mã nhân viên (nếu có, màu xám)
            if (!string.IsNullOrWhiteSpace(EmployeeCode))
            {
                if (!string.IsNullOrWhiteSpace(html))
                    html += " ";
                html += $"<color='#757575'>({EmployeeCode})</color>";
            }

            html += "<br>";

            // Thông tin bổ sung
            var additionalInfo = new List<string>();

            // Trạng thái hoạt động
            var statusText = IsActive ? "Hoạt động" : "Không hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";
            additionalInfo.Add(
                $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>");

            // Chức vụ
            if (!string.IsNullOrWhiteSpace(PositionName))
            {
                additionalInfo.Add(
                    $"<color='#757575'>Chức vụ:</color> <b>{PositionName}</b>");
            }

            // Phòng ban
            if (!string.IsNullOrWhiteSpace(DepartmentName))
            {
                additionalInfo.Add(
                    $"<color='#757575'>Phòng ban:</color> <b>{DepartmentName}</b>");
            }

            // Chi nhánh
            if (!string.IsNullOrWhiteSpace(BranchName))
            {
                additionalInfo.Add(
                    $"<color='#757575'>Chi nhánh:</color> <b>{BranchName}</b>");
            }

            // Email
            if (!string.IsNullOrWhiteSpace(Email))
            {
                additionalInfo.Add(
                    $"<color='#757575'>Email:</color> <b>{Email}</b>");
            }

            // Số điện thoại
            if (!string.IsNullOrWhiteSpace(Phone))
            {
                additionalInfo.Add(
                    $"<color='#757575'>Điện thoại:</color> <b>{Phone}</b>");
            }

            // Ngày vào làm
            if (HireDate.HasValue)
            {
                additionalInfo.Add(
                    $"<color='#757575'>Ngày vào làm:</color> <b>{HireDate.Value:dd/MM/yyyy}</b>");
            }

            if (additionalInfo.Any())
            {
                html += string.Join(" | ", additionalInfo);
            }

            return html;
        }
    }
}

/// <summary>
/// Converter giữa Employee Entity và EmployeeDto
/// </summary>
public static class EmployeeConverters
{
    /// <summary>
    /// Chuyển đổi từ Employee Entity sang EmployeeDto
    /// </summary>
    /// <param name="entity">Employee Entity</param>
    /// <param name="companyName">Tên công ty (tùy chọn, nếu đã có sẵn)</param>
    /// <param name="branchName">Tên chi nhánh (tùy chọn, nếu đã có sẵn)</param>
    /// <param name="departmentName">Tên phòng ban (tùy chọn, nếu đã có sẵn)</param>
    /// <param name="positionName">Tên chức vụ (tùy chọn, nếu đã có sẵn)</param>
    /// <returns>EmployeeDto</returns>
    public static EmployeeDto ToDto(this Employee entity, string companyName = null, string branchName = null, string departmentName = null, string positionName = null)
    {
        if (entity == null)
            return null;

        // QUAN TRỌNG: KHÔNG truy cập navigation properties (entity.Company, entity.CompanyBranch, entity.Department, entity.Position)
        // vì DataContext đã bị dispose sau khi repository method kết thúc
        // Chỉ sử dụng tham số truyền vào

        return new EmployeeDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            BranchId = entity.BranchId,
            DepartmentId = entity.DepartmentId,
            PositionId = entity.PositionId,
            EmployeeCode = entity.EmployeeCode,
            FullName = entity.FullName,
            Gender = entity.Gender,
            BirthDate = entity.BirthDate,
            Phone = entity.Phone,
            Email = entity.Email,
            HireDate = entity.HireDate,
            ResignDate = entity.ResignDate,
            AvatarPath = entity.AvatarPath,
            IsActive = entity.IsActive,
            Mobile = entity.Mobile,
            Fax = entity.Fax,
            LinkedIn = entity.LinkedIn,
            Skype = entity.Skype,
            WeChat = entity.WeChat,
            Notes = entity.Notes,
            CreatedDate = entity.CreatedDate,
            ModifiedDate = entity.ModifiedDate,
            AvatarFileName = entity.AvatarFileName,
            AvatarRelativePath = entity.AvatarRelativePath,
            AvatarFullPath = entity.AvatarFullPath,
            AvatarStorageType = entity.AvatarStorageType,
            AvatarFileSize = entity.AvatarFileSize,
            AvatarChecksum = entity.AvatarChecksum,
            AvatarThumbnailData = entity.AvatarThumbnailData?.ToArray(),

            // Navigation properties - sử dụng giá trị từ tham số truyền vào
            CompanyName = companyName,
            BranchName = branchName,
            DepartmentName = departmentName,
            PositionName = positionName
        };
    }

    /// <summary>
    /// Chuyển đổi danh sách Employee Entity sang danh sách EmployeeDto
    /// </summary>
    /// <param name="entities">Danh sách Employee Entity</param>
    /// <returns>Danh sách EmployeeDto</returns>
    public static IEnumerable<EmployeeDto> ToEmployeeDtos(this IEnumerable<Employee> entities)
    {
        if (entities == null) return [];

        return entities.Select(entity => entity.ToDto());
    }

    /// <summary>
    /// Chuyển đổi từ EmployeeDto sang Employee Entity
    /// </summary>
    /// <param name="dto">EmployeeDto</param>
    /// <param name="destination">Employee Entity đích (tùy chọn, cho update)</param>
    /// <returns>Employee Entity</returns>
    public static Employee ToEntity(this EmployeeDto dto, Employee destination = null)
    {
        if (dto == null) return null;
        var entity = destination ?? new Employee();
        if (dto.Id != Guid.Empty) entity.Id = dto.Id;
        entity.CompanyId = dto.CompanyId;
        entity.BranchId = dto.BranchId;
        entity.DepartmentId = dto.DepartmentId;
        entity.PositionId = dto.PositionId;
        entity.EmployeeCode = dto.EmployeeCode;
        entity.FullName = dto.FullName;
        entity.Gender = dto.Gender;
        entity.BirthDate = dto.BirthDate;
        entity.Phone = dto.Phone;
        entity.Email = dto.Email;
        entity.HireDate = dto.HireDate;
        entity.ResignDate = dto.ResignDate;
        entity.AvatarPath = dto.AvatarPath;
        entity.IsActive = dto.IsActive;
        entity.Mobile = dto.Mobile;
        entity.Fax = dto.Fax;
        entity.LinkedIn = dto.LinkedIn;
        entity.Skype = dto.Skype;
        entity.WeChat = dto.WeChat;
        entity.Notes = dto.Notes;
        entity.CreatedDate = dto.CreatedDate;
        entity.ModifiedDate = dto.ModifiedDate;
        entity.AvatarFileName = dto.AvatarFileName;
        entity.AvatarRelativePath = dto.AvatarRelativePath;
        entity.AvatarFullPath = dto.AvatarFullPath;
        entity.AvatarStorageType = dto.AvatarStorageType;
        entity.AvatarFileSize = dto.AvatarFileSize;
        entity.AvatarChecksum = dto.AvatarChecksum;
        if (dto.AvatarThumbnailData != null)
        {
            entity.AvatarThumbnailData = new System.Data.Linq.Binary(dto.AvatarThumbnailData);
        }
        return entity;
    }
}
