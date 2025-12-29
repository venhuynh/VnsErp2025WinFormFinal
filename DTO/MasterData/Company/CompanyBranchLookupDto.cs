using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace DTO.MasterData.Company
{
    /// <summary>
    /// DTO tối giản cho SearchLookUpEdit - chỉ chứa thông tin cần thiết để hiển thị và chọn chi nhánh
    /// </summary>
    public class CompanyBranchLookupDto
    {
        [DisplayName("ID")] public Guid Id { get; set; }

        [DisplayName("ID công ty")] public Guid CompanyId { get; set; }

        [DisplayName("Mã chi nhánh")] public string BranchCode { get; set; }

        [DisplayName("Tên chi nhánh")] public string BranchName { get; set; }

        [DisplayName("Trạng thái hoạt động")] public bool IsActive { get; set; }

        /// <summary>
        /// Thông tin chi nhánh dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin chi nhánh HTML")]
        [Description("Thông tin chi nhánh dưới dạng HTML")]
        public string BranchInfoHtml
        {
            get
            {
                var branchName = BranchName ?? string.Empty;
                var branchCode = BranchCode ?? string.Empty;
                var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Tên chi nhánh: font lớn, bold, màu xanh đậm (primary)
                // - Mã chi nhánh: font nhỏ hơn, màu xám
                // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

                var html = $"<b><color='blue'>{branchName}</color></b>";

                if (!string.IsNullOrWhiteSpace(branchCode))
                {
                    html += $" <color='#757575'>({branchCode})</color>";
                }

                html += "<br>";

                html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

                return html;
            }
        }
    }
}