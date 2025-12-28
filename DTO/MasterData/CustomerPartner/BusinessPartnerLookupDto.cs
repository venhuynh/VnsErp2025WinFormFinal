using System;
using System.ComponentModel;

namespace DTO.MasterData.CustomerPartner
{
    /// <summary>
    /// DTO tối giản cho SearchLookUpEdit - chỉ chứa thông tin cần thiết để hiển thị và chọn đối tác
    /// </summary>
    public class BusinessPartnerLookupDto
    {
        [DisplayName("ID")] public Guid Id { get; set; }

        [DisplayName("Mã đối tác")] public string PartnerCode { get; set; }

        [DisplayName("Loại đối tác")] public int PartnerType { get; set; }

        [DisplayName("Tên đối tác")] public string PartnerName { get; set; }

        /// <summary>
        /// Logo thumbnail (byte array) để hiển thị trong SearchLookUpEdit
        /// </summary>
        [DisplayName("Logo")]
        public byte[] LogoThumbnailData { get; set; }

        /// <summary>
        /// Thông tin đối tác dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin đối tác HTML")]
        [Description("Thông tin đối tác dưới dạng HTML")]
        public string BusinessPartnerInfoHtml
        {
            get
            {
                var partnerName = PartnerName ?? string.Empty;
                var partnerCode = PartnerCode ?? string.Empty;
                var partnerTypeName = ResolvePartnerTypeName(PartnerType);

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Tên đối tác: font lớn, bold, màu xanh đậm (primary)
                // - Mã đối tác: font nhỏ hơn, màu xám
                // - Loại đối tác: highlight với màu khác nhau

                var html = $"<b><color='blue'>{partnerName}</color></b>";

                if (!string.IsNullOrWhiteSpace(partnerCode))
                {
                    html += $" <color='#757575'>({partnerCode})</color>";
                }

                html += "<br>";

                if (!string.IsNullOrWhiteSpace(partnerTypeName))
                {
                    var typeColor = PartnerType switch
                    {
                        1 => "#2196F3", // Khách hàng: Blue
                        2 => "#FF9800", // Nhà cung cấp: Orange
                        3 => "#9C27B0", // Cả hai: Purple
                        _ => "#757575" // Khác: Gray
                    };
                    html += $"<color='#757575'>Loại:</color> <color='{typeColor}'><b>{partnerTypeName}</b></color>";
                }

                return html;
            }
        }

        /// <summary>
        /// Resolve tên loại đối tác từ PartnerType (int)
        /// </summary>
        private static string ResolvePartnerTypeName(int partnerType)
        {
            return partnerType switch
            {
                1 => "Khách hàng",
                2 => "Nhà cung cấp",
                3 => "Cả hai",
                _ => "Không xác định"
            };
        }
    }
}