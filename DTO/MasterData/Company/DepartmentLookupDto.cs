using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DTO.MasterData.Company
{
    /// <summary>
    /// DTO tối giản cho SearchLookUpEdit - chỉ chứa thông tin cần thiết để hiển thị và chọn phòng ban
    /// </summary>
    public class DepartmentLookupDto
    {
        [DisplayName("ID")] public Guid Id { get; set; }

        [DisplayName("ID công ty")] public Guid CompanyId { get; set; }

        [DisplayName("ID chi nhánh")] public Guid? BranchId { get; set; }

        [DisplayName("Mã phòng ban")] public string DepartmentCode { get; set; }

        [DisplayName("Tên phòng ban")] public string DepartmentName { get; set; }

        [DisplayName("ID phòng ban cha")] public Guid? ParentId { get; set; }

        [DisplayName("Trạng thái hoạt động")] public bool IsActive { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ từ gốc đến phòng ban này
        /// Ví dụ: "Phòng ban A > Phòng ban A1"
        /// </summary>
        [DisplayName("Đường dẫn đầy đủ")]
        [Description("Đường dẫn đầy đủ từ gốc đến phòng ban này")]
        public string FullPath { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ dưới dạng HTML theo format DevExpress
        /// Format giống BusinessPartnerCategoryDto.FullPathHtml (không dùng &lt;size&gt;)
        /// </summary>
        [DisplayName("Đường dẫn HTML")]
        [Description("Đường dẫn đầy đủ từ gốc đến phòng ban này dưới dạng HTML")]
        public string FullPathHtml
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullPath))
                    return string.Empty;

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

                    // Format giống BusinessPartnerCategoryDto: không dùng <size>, chỉ dùng <b> và <color>
                    htmlParts.Add($"{weight}<color='{color}'>{parts[i]}</color>{weightClose}");
                }

                return string.Join(" <color='#757575'>></color> ", htmlParts);
            }
        }

        /// <summary>
        /// Thông tin phòng ban dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Format giống CompanyBranchLookupDto.BranchInfoHtml (không dùng &lt;size&gt;)
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin phòng ban HTML")]
        [Description("Thông tin phòng ban dưới dạng HTML")]
        public string DepartmentInfoHtml
        {
            get
            {
                var departmentName = DepartmentName ?? string.Empty;
                var departmentCode = DepartmentCode ?? string.Empty;
                var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Tên phòng ban: bold, màu xanh đậm (primary)
                // - Mã phòng ban: màu xám
                // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

                var html = $"<b><color='blue'>{departmentName}</color></b>";

                if (!string.IsNullOrWhiteSpace(departmentCode))
                {
                    html += $" <color='#757575'>({departmentCode})</color>";
                }

                html += "<br>";

                html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

                return html;
            }
        }
    }
}