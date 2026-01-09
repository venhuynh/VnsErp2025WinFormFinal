using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

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
    [DisplayName("Thông tin chi nhánh công ty")]
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
            // - Tên chi nhánh: bold, màu xanh đậm (primary)
            // - Mã chi nhánh: màu xám
            // - Thông tin chi tiết: màu xám cho label, đen cho value
            // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

            var html = $"<b><color='blue'>{branchName}</color></b>";

            if (!string.IsNullOrWhiteSpace(branchCode))
            {
                html += $" <color='#757575'>({branchCode})</color>";
            }

            html += "<br>";

            if (!string.IsNullOrWhiteSpace(address))
            {
                html += $"<color='#757575'>Địa chỉ:</color> <color='#212121'><b>{address}</b></color><br>";
            }

            if (!string.IsNullOrWhiteSpace(phone))
            {
                html += $"<color='#757575'>Điện thoại:</color> <color='#212121'><b>{phone}</b></color>";
            }

            if (!string.IsNullOrWhiteSpace(email))
            {
                if (!string.IsNullOrWhiteSpace(phone))
                    html += " | ";
                html += $"<color='#757575'>Email:</color> <color='#212121'><b>{email}</b></color>";
            }

            if (!string.IsNullOrWhiteSpace(phone) || !string.IsNullOrWhiteSpace(email))
            {
                html += "<br>";
            }

            if (!string.IsNullOrWhiteSpace(managerName))
            {
                html += $"<color='#757575'>Quản lý:</color> <color='#212121'><b>{managerName}</b></color><br>";
            }

            html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

            return html;
        }
    }

    #endregion

}


