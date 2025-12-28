using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTO.MasterData.CustomerPartner
{
    public class BusinessPartnerListDto
    {
        [DisplayName("ID")] public Guid Id { get; set; }

        [DisplayName("Mã đối tác")]
        [Required(ErrorMessage = "Mã đối tác không được để trống")]
        [StringLength(50, ErrorMessage = "Mã đối tác không được vượt quá 50 ký tự")]
        public string PartnerCode { get; set; }

        [DisplayName("Tên đối tác")]
        [Required(ErrorMessage = "Tên đối tác không được để trống")]
        [StringLength(255, ErrorMessage = "Tên đối tác không được vượt quá 255 ký tự")]
        public string PartnerName { get; set; }

        [DisplayName("Loại đối tác")] public int PartnerType { get; set; }

        [DisplayName("Loại đối tác")] public string PartnerTypeName { get; set; } // Customer / Vendor / Both

        [DisplayName("Mã số thuế")]
        [StringLength(50, ErrorMessage = "Mã số thuế không được vượt quá 50 ký tự")]
        public string TaxCode { get; set; }

        [DisplayName("Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không đúng định dạng")]
        [StringLength(50, ErrorMessage = "Số điện thoại không được vượt quá 50 ký tự")]
        public string Phone { get; set; }

        [DisplayName("Email")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        [StringLength(100, ErrorMessage = "Email không được vượt quá 100 ký tự")]
        public string Email { get; set; }

        [DisplayName("Website")]
        [StringLength(100, ErrorMessage = "Website không được vượt quá 100 ký tự")]
        public string Website { get; set; }

        [DisplayName("Địa chỉ")]
        [StringLength(255, ErrorMessage = "Địa chỉ không được vượt quá 255 ký tự")]
        public string Address { get; set; }

        [DisplayName("Thành phố")]
        [StringLength(100, ErrorMessage = "Thành phố không được vượt quá 100 ký tự")]
        public string City { get; set; }

        [DisplayName("Quốc gia")]
        [StringLength(100, ErrorMessage = "Quốc gia không được vượt quá 100 ký tự")]
        public string Country { get; set; }

        [DisplayName("Trạng thái")] public bool IsActive { get; set; }

        [DisplayName("Ngày tạo")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [DisplayName("Ngày cập nhật")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedDate { get; set; }

        [DisplayName("Người tạo ID")] public Guid? CreatedBy { get; set; }

        [DisplayName("Người tạo")] public string CreatedByName { get; set; }

        [DisplayName("Người cập nhật ID")] public Guid? ModifiedBy { get; set; }

        [DisplayName("Người cập nhật")] public string ModifiedByName { get; set; }

        [DisplayName("Danh sách danh mục")]
        [Description("Danh sách tên các danh mục phân loại (ngăn cách bởi dấu phẩy)")]
        public string CategoryNames { get; set; }

        /// <summary>
        /// Danh sách đường dẫn đầy đủ của các danh mục phân loại (ngăn cách bởi dấu phẩy)
        /// Ví dụ: "Danh mục A > Danh mục A1, Danh mục B > Danh mục B1"
        /// </summary>
        [DisplayName("Đường dẫn danh mục")]
        [Description("Danh sách đường dẫn đầy đủ của các danh mục phân loại (ngăn cách bởi dấu phẩy)")]
        public string CategoryPaths { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ từ gốc đến danh mục này
        /// Nếu có nhiều danh mục, lấy đường dẫn đầu tiên
        /// </summary>
        [DisplayName("Đường dẫn đầy đủ")]
        [Description("Đường dẫn đầy đủ từ gốc đến danh mục này")]
        public string FullPath { get; set; }

        /// <summary>
        /// Đường dẫn phân loại đầy đủ dưới dạng HTML theo format DevExpress
        /// Hiển thị tất cả các đường dẫn phân loại với màu sắc và format chuyên nghiệp
        /// </summary>
        [DisplayName("Đường dẫn phân loại HTML")]
        [Description("Đường dẫn đầy đủ của các danh mục phân loại dưới dạng HTML")]
        public string CategoryPathHtml
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CategoryPaths))
                {
                    System.Diagnostics.Debug.WriteLine($"[CategoryPathHtml] CategoryPaths is null or empty");
                    return string.Empty;
                }

                System.Diagnostics.Debug.WriteLine($"[CategoryPathHtml] CategoryPaths input: {CategoryPaths}");

                // Tách các đường dẫn phân loại (ngăn cách bởi dấu phẩy)
                var paths = CategoryPaths.Split([", "], StringSplitOptions.RemoveEmptyEntries);
                System.Diagnostics.Debug.WriteLine($"[CategoryPathHtml] Split thành {paths.Length} paths");
                var htmlPaths = new List<string>();

                foreach (var path in paths)
                {
                    var trimmedPath = path.Trim();
                    if (string.IsNullOrWhiteSpace(trimmedPath))
                        continue;

                    System.Diagnostics.Debug.WriteLine($"[CategoryPathHtml] Đang xử lý path: {trimmedPath}");

                    // Tách đường dẫn thành các phần (ngăn cách bởi " > ")
                    var parts = trimmedPath.Split(new[] { " > ", ">", " >", "> " }, StringSplitOptions.None)
                        .Where(p => !string.IsNullOrWhiteSpace(p))
                        .Select(p => p.Trim())
                        .ToArray();

                    System.Diagnostics.Debug.WriteLine(
                        $"[CategoryPathHtml] Path split thành {parts.Length} parts: {string.Join(" | ", parts)}");
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

                    // Kết hợp các phần với dấu " > " (format HTML giống ThongTinHtml)
                    var htmlPath = string.Join(" <color='#757575'>></color> ", htmlParts);
                    htmlPaths.Add(htmlPath);
                    System.Diagnostics.Debug.WriteLine($"[CategoryPathHtml] HTML path: {htmlPath}");
                }

                // Kết hợp tất cả các đường dẫn, mỗi đường dẫn trên một dòng
                var result = string.Join("<br>", htmlPaths);
                System.Diagnostics.Debug.WriteLine($"[CategoryPathHtml] Result HTML: {result}");
                return result;
            }
        }

        /// <summary>
        /// Đường dẫn đầy đủ dưới dạng HTML theo format DevExpress
        /// </summary>
        [DisplayName("Đường dẫn HTML")]
        [Description("Đường dẫn đầy đủ từ gốc đến danh mục này dưới dạng HTML")]
        public string FullPathHtml
        {
            get
            {
                if (string.IsNullOrWhiteSpace(FullPath))
                {
                    System.Diagnostics.Debug.WriteLine($"[FullPathHtml] FullPath is null or empty");
                    return string.Empty;
                }

                System.Diagnostics.Debug.WriteLine($"[FullPathHtml] FullPath input: {FullPath}");

                // Tách đường dẫn và format với màu sắc
                // Hỗ trợ nhiều format: " > ", ">", " >" hoặc "> "
                var parts = FullPath.Split(new[] { " > ", ">", " >", "> " }, StringSplitOptions.None)
                    .Where(p => !string.IsNullOrWhiteSpace(p))
                    .Select(p => p.Trim())
                    .ToArray();
                System.Diagnostics.Debug.WriteLine(
                    $"[FullPathHtml] Split thành {parts.Length} parts: {string.Join(" | ", parts)}");

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
                System.Diagnostics.Debug.WriteLine($"[FullPathHtml] Result HTML: {result}");
                return result;
            }
        }

        // Logo metadata fields (để hiển thị và load logo từ NAS)
        [DisplayName("Tên file logo")]
        [StringLength(255, ErrorMessage = "Tên file logo không được vượt quá 255 ký tự")]
        public string LogoFileName { get; set; }

        [DisplayName("Đường dẫn tương đối logo")]
        [StringLength(500, ErrorMessage = "Đường dẫn tương đối logo không được vượt quá 500 ký tự")]
        public string LogoRelativePath { get; set; }

        [DisplayName("Đường dẫn đầy đủ logo")]
        [StringLength(1000, ErrorMessage = "Đường dẫn đầy đủ logo không được vượt quá 1000 ký tự")]
        public string LogoFullPath { get; set; }

        [DisplayName("Loại storage logo")]
        [StringLength(20, ErrorMessage = "Loại storage logo không được vượt quá 20 ký tự")]
        public string LogoStorageType { get; set; }

        [DisplayName("Kích thước file logo")] public long? LogoFileSize { get; set; }

        [DisplayName("Checksum logo")]
        [StringLength(64, ErrorMessage = "Checksum logo không được vượt quá 64 ký tự")]
        public string LogoChecksum { get; set; }

        /// <summary>
        /// Dữ liệu binary của logo thumbnail (để hiển thị nhanh trong gridview)
        /// Lưu ý: Chỉ lưu thumbnail trong database, logo gốc lưu trên NAS
        /// </summary>
        [DisplayName("Dữ liệu thumbnail logo")]
        [Description("Dữ liệu binary của logo thumbnail (để hiển thị nhanh trong gridview)")]
        public byte[] LogoThumbnailData { get; set; }

        /// <summary>
        /// Địa chỉ đầy đủ được tính từ Address, City, Country
        /// </summary>
        [DisplayName("Địa chỉ đầy đủ")]
        [Description("Địa chỉ đầy đủ được tính từ Address, City, Country")]
        public string FullAddressName
        {
            get
            {
                var addressParts = new List<string>();
                if (!string.IsNullOrWhiteSpace(Address))
                    addressParts.Add(Address);
                if (!string.IsNullOrWhiteSpace(City))
                    addressParts.Add(City);
                if (!string.IsNullOrWhiteSpace(Country))
                    addressParts.Add(Country);
                return string.Join(", ", addressParts);
            }
        }

        /// <summary>
        /// Thông tin đối tác dưới dạng HTML theo format DevExpress
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin đối tác dưới dạng HTML")]
        public string ThongTinHtml
        {
            get
            {
                var partnerName = PartnerName ?? string.Empty;
                var partnerCode = PartnerCode ?? string.Empty;
                var partnerTypeName = PartnerTypeName ?? string.Empty;
                var taxCode = TaxCode ?? string.Empty;
                var phone = Phone ?? string.Empty;
                var email = Email ?? string.Empty;
                var website = Website ?? string.Empty;
                var fullAddress = FullAddressName;
                var categoryNames = CategoryNames ?? string.Empty;
                var statusText = IsActive ? "Đang hoạt động" : "Ngừng hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";

                // Format chuyên nghiệp với visual hierarchy rõ ràng
                // - Tên đối tác: font lớn, bold, màu xanh đậm (primary)
                // - Mã đối tác: font nhỏ hơn, màu xám
                // - Loại đối tác: highlight với màu khác nhau
                // - Thông tin chi tiết: font nhỏ hơn, màu xám cho label, đen cho value
                // - Trạng thái: highlight với màu xanh (active) hoặc đỏ (inactive)

                var html = $"<b><color='blue'>{partnerName}</color></b>";

                if (!string.IsNullOrWhiteSpace(partnerCode))
                {
                    html += $" <color='#757575'>({partnerCode})</color>";
                }

                html += "<br>";

                var infoParts = new List<string>();

                if (!string.IsNullOrWhiteSpace(partnerTypeName))
                {
                    var typeColor =
                        PartnerType == 1 ? "#2196F3" :
                        PartnerType == 2 ? "#FF9800" : "#9C27B0"; // Customer: Blue, Vendor: Orange, Both: Purple
                    infoParts.Add(
                        $"<color='#757575'>Loại:</color> <color='{typeColor}'><b>{partnerTypeName}</b></color>");
                }

                if (!string.IsNullOrWhiteSpace(categoryNames))
                {
                    infoParts.Add($"<color='#757575'>Danh mục:</color> <b>{categoryNames}</b>");
                }

                if (infoParts.Any())
                {
                    html += string.Join(" | ", infoParts) + "<br>";
                }

                if (!string.IsNullOrWhiteSpace(fullAddress))
                {
                    html += $"<color='#757575'>Địa chỉ:</color> <b>{fullAddress}</b><br>";
                }

                var contactParts = new List<string>();
                if (!string.IsNullOrWhiteSpace(taxCode))
                {
                    contactParts.Add($"<color='#757575'>MST:</color> <b>{taxCode}</b>");
                }

                if (!string.IsNullOrWhiteSpace(phone))
                {
                    contactParts.Add($"<color='#757575'>ĐT:</color> <b>{phone}</b>");
                }

                if (!string.IsNullOrWhiteSpace(email))
                {
                    contactParts.Add($"<color='#757575'>Email:</color> <b>{email}</b>");
                }

                if (!string.IsNullOrWhiteSpace(website))
                {
                    contactParts.Add($"<color='#757575'>Web:</color> <b>{website}</b>");
                }

                if (contactParts.Any())
                {
                    html += string.Join(" | ", contactParts) + "<br>";
                }

                html += $"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>";

                return html;
            }
        }

        /// <summary>
        /// Thông tin audit (người tạo/sửa) dưới dạng HTML theo format DevExpress
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
                if (UpdatedDate.HasValue)
                {
                    var modifiedInfo = $"<color='#757575'>Sửa:</color> <b>{UpdatedDate.Value:dd/MM/yyyy HH:mm}</b>";
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