using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Dal.DataContext;

namespace DTO.MasterData.Company;

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
    /// Thông tin chi nhánh dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    public string ThongTinHtml
    {
        get
        {
            var branchName = BranchName ?? string.Empty;
            var branchCode = BranchCode ?? string.Empty;
            var address = Address ?? string.Empty;
            var phone = Phone ?? string.Empty;
            var email = Email ?? string.Empty;
            var managerName = ManagerName ?? string.Empty;
            var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
            var statusColor = IsActive ? "#4CAF50" : "#F44336";

            // Format chuyên nghiệp với visual hierarchy rõ ràng
            // - Tên chi nhánh: font lớn, bold, màu xanh đậm (primary)
            // - Mã chi nhánh: font nhỏ hơn, màu xám
            // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

            var html = $"<size=12><b><color='blue'>{branchName}</color></b></size>";

            if (!string.IsNullOrWhiteSpace(branchCode))
            {
                html += $" <size=9><color='#757575'>({branchCode})</color></size>";
            }

            html += "<br>";

            if (!string.IsNullOrWhiteSpace(address))
            {
                html += $"<size=9><color='#757575'>Địa chỉ:</color></size> <size=10><color='#212121'><b>{address}</b></color></size><br>";
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                html += $"<size=9><color='#757575'>Điện thoại:</color></size> <size=10><color='#212121'><b>{phone}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!string.IsNullOrWhiteSpace(phone))
                    html += " | ";
                html += $"<size=9><color='#757575'>Email:</color></size> <size=10><color='#212121'><b>{email}</b></color></size>";
            }

            if (!string.IsNullOrWhiteSpace(phone) || !string.IsNullOrWhiteSpace(email))
            {
                html += "<br>";
            }

            if (!string.IsNullOrWhiteSpace(managerName))
            {
                html += $"<size=9><color='#757575'>Quản lý:</color></size> <size=10><color='#212121'><b>{managerName}</b></color></size><br>";
            }

            html += $"<size=9><color='#757575'>Trạng thái:</color></size> <size=10><color='{statusColor}'><b>{statusText}</b></color></size>";

            return html;
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



/// <summary>
/// Converters giữa Entity và DTO cho chi nhánh công ty.
/// Cung cấp các phương thức chuyển đổi dữ liệu giữa các layer.
/// </summary>
public static class CompanyBranchConverters
{
    #region ========== CONVERTERS ==========

    /// <summary>
    /// Chuyển đổi từ Entity sang DTO.
    /// </summary>
    /// <param name="entity">Entity chi nhánh công ty</param>
    /// <returns>DTO chi nhánh công ty</returns>
    public static CompanyBranchDto ToDto(this CompanyBranch entity)
    {
        if (entity == null)
            return null;

        return new CompanyBranchDto
        {
            Id = entity.Id,
            CompanyId = entity.CompanyId,
            BranchCode = entity.BranchCode,
            BranchName = entity.BranchName,
            Address = entity.Address,
            Phone = entity.Phone,
            Email = entity.Email,
            ManagerName = entity.ManagerName,
            IsActive = entity.IsActive,
            // Entity không có CreatedDate/ModifiedDate, sử dụng giá trị mặc định
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    /// <summary>
    /// Chuyển đổi từ DTO sang Entity.
    /// </summary>
    /// <param name="dto">DTO chi nhánh công ty</param>
    /// <returns>Entity chi nhánh công ty</returns>
    public static CompanyBranch ToEntity(this CompanyBranchDto dto)
    {
        if (dto == null)
            return null;

        return new CompanyBranch
        {
            Id = dto.Id,
            CompanyId = dto.CompanyId,
            BranchCode = dto.BranchCode,
            BranchName = dto.BranchName,
            Address = dto.Address,
            Phone = dto.Phone,
            Email = dto.Email,
            ManagerName = dto.ManagerName,
            IsActive = dto.IsActive
            // Entity không có CreatedDate/ModifiedDate properties
        };
    }

    /// <summary>
    /// Chuyển đổi từ DTO sang Entity với thông tin cập nhật.
    /// </summary>
    /// <param name="dto">DTO chi nhánh công ty</param>
    /// <param name="existingEntity">Entity hiện tại (để giữ nguyên thông tin cũ)</param>
    /// <returns>Entity chi nhánh công ty đã cập nhật</returns>
    public static CompanyBranch ToEntity(this CompanyBranchDto dto, CompanyBranch existingEntity)
    {
        if (dto == null)
            return null;

        if (existingEntity == null)
            return dto.ToEntity();

        // Cập nhật thông tin từ DTO
        existingEntity.CompanyId = dto.CompanyId;
        existingEntity.BranchCode = dto.BranchCode;
        existingEntity.BranchName = dto.BranchName;
        existingEntity.Address = dto.Address;
        existingEntity.Phone = dto.Phone;
        existingEntity.Email = dto.Email;
        existingEntity.ManagerName = dto.ManagerName;
        existingEntity.IsActive = dto.IsActive;
        // Entity không có ModifiedDate property

        return existingEntity;
    }

    #endregion
}