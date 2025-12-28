using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.VersionAndUserManagementDto
{
    /// <summary>
    /// DTO cho quản lý người dùng ứng dụng
    /// </summary>
    public class ApplicationUserDto
    {
        [DisplayName("ID")]
        [Required(ErrorMessage = "ID không được để trống")]
        public Guid Id { get; set; }

        [DisplayName("Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống")]
        [StringLength(50, ErrorMessage = "Tên đăng nhập không được vượt quá 50 ký tự")]
        public string UserName { get; set; }

        [DisplayName("Mật khẩu (Hash)")]
        [Required(ErrorMessage = "Mật khẩu không được để trống")]
        [StringLength(500, ErrorMessage = "Mật khẩu không được vượt quá 500 ký tự")]
        public string HashPassword { get; set; }

        [DisplayName("Đang hoạt động")] public bool Active { get; set; }

        [DisplayName("ID Nhân viên")]
        [Description("ID nhân viên liên kết với người dùng này (1-1 relationship)")]
        public Guid? EmployeeId { get; set; }

        [DisplayName("Mã nhân viên")]
        [Description("Mã nhân viên (từ Employee)")]
        public string EmployeeCode { get; set; }

        [DisplayName("Họ tên nhân viên")]
        [Description("Họ tên nhân viên (từ Employee)")]
        public string EmployeeFullName { get; set; }

        [DisplayName("Phòng ban")]
        [Description("Tên phòng ban (từ Employee.Department)")]
        public string DepartmentName { get; set; }

        [DisplayName("Vị trí")]
        [Description("Tên vị trí (từ Employee.Position)")]
        public string PositionName { get; set; }

        /// <summary>
        /// Thông tin người dùng dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin người dùng dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var userName = UserName ?? string.Empty;
                var statusText = Active ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = Active ? "#4CAF50" : "#F44336";

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Tên đăng nhập: font lớn, bold, màu xanh đậm (primary)
                // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)
                // - Thông tin nhân viên: hiển thị EmployeeId nếu có

                var html = $"<b><color='blue'>{userName}</color></b>";

                if (Active)
                {
                    html += " <color='#4CAF50'><b>●</b></color>";
                }
                else
                {
                    html += " <color='#F44336'><b>●</b></color>";
                }

                html += "<br>";

                var infoParts = new List<string>();

                // Hiển thị thông tin nhân viên nếu có
                if (!string.IsNullOrWhiteSpace(EmployeeCode) || !string.IsNullOrWhiteSpace(EmployeeFullName))
                {
                    var employeeInfo = new List<string>();
                    if (!string.IsNullOrWhiteSpace(EmployeeCode))
                    {
                        employeeInfo.Add($"<b>{EmployeeCode}</b>");
                    }

                    if (!string.IsNullOrWhiteSpace(EmployeeFullName))
                    {
                        employeeInfo.Add(EmployeeFullName);
                    }

                    if (employeeInfo.Any())
                    {
                        infoParts.Add($"<color='#757575'>Nhân viên:</color> {string.Join(" - ", employeeInfo)}");
                    }
                }

                if (!string.IsNullOrWhiteSpace(DepartmentName))
                {
                    infoParts.Add($"<color='#757575'>Phòng ban:</color> <b>{DepartmentName}</b>");
                }

                if (!string.IsNullOrWhiteSpace(PositionName))
                {
                    infoParts.Add($"<color='#757575'>Vị trí:</color> <b>{PositionName}</b>");
                }

                if (infoParts.Any())
                {
                    html += string.Join(" | ", infoParts) + "<br>";
                }

                html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

                return html;
            }
        }

        /// <summary>
        /// Thông tin audit (ngày tạo/cập nhật) dưới dạng HTML theo format DevExpress
        /// Lưu ý: ApplicationUser entity không có các trường CreateDate, CreateBy, ModifiedDate, ModifiedBy
        /// Nên thuộc tính này trả về chuỗi rỗng hoặc có thể được mở rộng trong tương lai
        /// </summary>
        [DisplayName("Thông tin audit HTML")]
        [Description("Thông tin ngày tạo và cập nhật dưới dạng HTML")]
        public string AuditInfoHtml
        {
            get
            {
                // ApplicationUser entity hiện tại không có các trường audit
                // Trả về chuỗi rỗng hoặc có thể mở rộng trong tương lai
                return string.Empty;
            }
        }
    }
}