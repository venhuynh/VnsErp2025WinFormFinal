using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.CustomerPartner
{
    public class BusinessPartnerCategoryDto
    {
        [DisplayName("ID")] public Guid Id { get; set; }

        [DisplayName("Tên phân loại")]
        [Required(ErrorMessage = "Tên phân loại không được để trống")]
        [StringLength(100, ErrorMessage = "Tên phân loại không được vượt quá 100 ký tự")]
        public string CategoryName { get; set; }

        [DisplayName("Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string Description { get; set; }

        [DisplayName("Danh mục cha")]
        [Description("ID của danh mục cha (null nếu là danh mục gốc)")]
        public Guid? ParentId { get; set; }

        [DisplayName("Tên danh mục cha")]
        [Description("Tên của danh mục cha (để hiển thị)")]
        public string ParentCategoryName { get; set; }

        [DisplayName("Loại danh mục")]
        [Description("Category chính hoặc Sub-category")]
        public string CategoryType { get; set; }

        [DisplayName("Số lượng đối tác")]
        [Description("Tổng số đối tác thuộc phân loại này")]
        public int PartnerCount { get; set; }

        [DisplayName("Cấp độ")]
        [Description("Cấp độ trong cây phân cấp (0 = gốc, 1 = con, ...)")]
        public int Level { get; set; }

        [DisplayName("Có danh mục con")]
        [Description("Có danh mục con hay không")]
        public bool HasChildren { get; set; }

        [DisplayName("Đường dẫn đầy đủ")]
        [Description("Đường dẫn đầy đủ từ gốc đến danh mục này")]
        public string FullPath { get; set; }

        [DisplayName("Mã danh mục")]
        [StringLength(50, ErrorMessage = "Mã danh mục không được vượt quá 50 ký tự")]
        public string CategoryCode { get; set; }

        [DisplayName("Trạng thái hoạt động")]
        [Description("Danh mục có đang hoạt động hay không")]
        public bool IsActive { get; set; } = true;

        [DisplayName("Thứ tự sắp xếp")]
        [Description("Thứ tự hiển thị trong danh sách (số nhỏ hơn hiển thị trước)")]
        public int? SortOrder { get; set; }

        [DisplayName("Ngày tạo")]
        [Description("Ngày giờ tạo danh mục")]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Người tạo")]
        [Description("ID người tạo danh mục")]
        public Guid? CreatedBy { get; set; }

        [DisplayName("Tên người tạo")]
        [Description("Tên người dùng đã tạo danh mục")]
        public string CreatedByName { get; set; }

        [DisplayName("Ngày cập nhật")]
        [Description("Ngày giờ cập nhật danh mục lần cuối")]
        public DateTime? ModifiedDate { get; set; }

        [DisplayName("Người cập nhật")]
        [Description("ID người cập nhật danh mục")]
        public Guid? ModifiedBy { get; set; }

        [DisplayName("Tên người cập nhật")]
        [Description("Tên người dùng đã cập nhật danh mục")]
        public string ModifiedByName { get; set; }

        /// <summary>
        /// Thông tin danh mục dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
        /// Format giống BusinessPartnerListDto.ThongTinHtml (không dùng &lt;size&gt;)
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin danh mục dưới dạng HTML")]
        public string CategoryInfoHtml
        {
            get
            {
                var html = string.Empty;

                // Tên danh mục (màu xanh, đậm)
                if (!string.IsNullOrWhiteSpace(CategoryName))
                {
                    html += $"<b><color='blue'>{CategoryName}</color></b>";
                }

                // Mã danh mục (nếu có, màu xám)
                if (!string.IsNullOrWhiteSpace(CategoryCode))
                {
                    if (!string.IsNullOrWhiteSpace(html))
                        html += " ";
                    html += $"<color='#757575'>({CategoryCode})</color>";
                }

                html += "<br>";

                // Thông tin bổ sung
                var additionalInfo = new List<string>();

                // Trạng thái hoạt động
                var statusText = IsActive ? "Hoạt động" : "Không hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";
                additionalInfo.Add(
                    $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>");

                // Số lượng đối tác
                if (PartnerCount > 0)
                {
                    additionalInfo.Add(
                        $"<color='#757575'>Số đối tác:</color> <b>{PartnerCount}</b>");
                }

                // Thứ tự sắp xếp
                if (SortOrder.HasValue)
                {
                    additionalInfo.Add(
                        $"<color='#757575'>Thứ tự:</color> <b>{SortOrder.Value}</b>");
                }

                if (additionalInfo.Any())
                {
                    html += string.Join(" | ", additionalInfo);
                }

                return html;
            }
        }

        /// <summary>
        /// Đường dẫn đầy đủ dưới dạng HTML theo format DevExpress
        /// Format giống BusinessPartnerListDto.FullPathHtml (không dùng &lt;size&gt;)
        /// </summary>
        [DisplayName("Đường dẫn HTML")]
        [Description("Đường dẫn đầy đủ từ gốc đến danh mục này dưới dạng HTML")]
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

                    // Format giống BusinessPartnerListDto: không dùng <size>, chỉ dùng <b> và <color>
                    htmlParts.Add($"{weight}<color='{color}'>{parts[i]}</color>{weightClose}");
                }

                return string.Join(" <color='#757575'>></color> ", htmlParts);
            }
        }

        /// <summary>
        /// Thông tin audit (người tạo/sửa) dưới dạng HTML theo format DevExpress
        /// Format giống BusinessPartnerListDto.AuditInfoHtml (không dùng &lt;size&gt;)
        /// </summary>
        [DisplayName("Thông tin audit HTML")]
        [Description("Thông tin người tạo và cập nhật dưới dạng HTML")]
        public string AuditInfoHtml
        {
            get
            {
                var html = string.Empty;
                var infoParts = new List<string>();

                // Người tạo
                if (CreatedDate != default(DateTime))
                {
                    var createdInfo = $"<color='#757575'>Tạo:</color> <b>{CreatedDate:dd/MM/yyyy HH:mm}</b>";
                    if (!string.IsNullOrWhiteSpace(CreatedByName))
                    {
                        createdInfo += $" <color='#757575'>bởi</color> <b>{CreatedByName}</b>";
                    }

                    infoParts.Add(createdInfo);
                }

                // Người cập nhật
                if (ModifiedDate.HasValue)
                {
                    var modifiedInfo = $"<color='#757575'>Sửa:</color> <b>{ModifiedDate.Value:dd/MM/yyyy HH:mm}</b>";
                    if (!string.IsNullOrWhiteSpace(ModifiedByName))
                    {
                        modifiedInfo += $" <color='#757575'>bởi</color> <b>{ModifiedByName}</b>";
                    }

                    infoParts.Add(modifiedInfo);
                }

                if (infoParts.Any())
                {
                    html = string.Join("<br>", infoParts);
                }

                return html;
            }
        }
    }
}