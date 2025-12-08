using Dal.DataContext;
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
                    
                    System.Diagnostics.Debug.WriteLine($"[CategoryPathHtml] Path split thành {parts.Length} parts: {string.Join(" | ", parts)}");
                    var htmlParts = new List<string>();

                    for (int i = 0; i < parts.Length; i++)
                    {
                        var isLast = i == parts.Length - 1;
                        var color = isLast ? "blue" : "#757575";
                        var size = isLast ? "12" : "10";
                        var weight = isLast ? "<b>" : "";
                        var weightClose = isLast ? "</b>" : "";

                        htmlParts.Add($"<size={size}>{weight}<color='{color}'>{parts[i]}</color>{weightClose}</size>");
                    }

                    // Kết hợp các phần với dấu " > " (format HTML giống FullPathHtml)
                    var htmlPath = string.Join(" <size=9><color='#757575'>></color></size> ", htmlParts);
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
                System.Diagnostics.Debug.WriteLine($"[FullPathHtml] Split thành {parts.Length} parts: {string.Join(" | ", parts)}");
                
                var htmlParts = new List<string>();

                for (int i = 0; i < parts.Length; i++)
                {
                    var isLast = i == parts.Length - 1;
                    var color = isLast ? "blue" : "#757575";
                    var size = isLast ? "12" : "10";
                    var weight = isLast ? "<b>" : "";
                    var weightClose = isLast ? "</b>" : "";

                    htmlParts.Add($"<size={size}>{weight}<color='{color}'>{parts[i]}</color>{weightClose}</size>");
                }

                var result = string.Join(" <size=9><color='#757575'>></color></size> ", htmlParts);
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

    /// <summary>
    /// Converter cho BusinessPartner Entity và DTO
    /// </summary>
    public static class BusinessPartnerConverters
    {
        /// <summary>
        /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerListDto
        /// Sử dụng navigation properties đã được load trong repository
        /// </summary>
        /// <param name="entity">BusinessPartner Entity</param>
        /// <param name="categoryDict">Dictionary chứa tất cả BusinessPartnerCategory entities để tính FullPath (optional, chỉ cần khi tính full path của category)</param>
        /// <returns>BusinessPartnerListDto</returns>
        public static BusinessPartnerListDto ToListDto(this BusinessPartner entity,
            Dictionary<Guid, BusinessPartnerCategory> categoryDict = null)
        {
            if (entity == null) return null;

            System.Diagnostics.Debug.WriteLine($"[ToListDto] Bắt đầu convert partner {entity.Id} ({entity.PartnerCode})");

            var dto = new BusinessPartnerListDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
                PartnerTypeName = ResolvePartnerTypeName(entity.PartnerType),
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                Website = entity.Website,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                CreatedBy = entity.CreatedBy,
                ModifiedBy = entity.ModifiedBy,
                LogoFileName = entity.LogoFileName,
                LogoRelativePath = entity.LogoRelativePath,
                LogoFullPath = entity.LogoFullPath,
                LogoStorageType = entity.LogoStorageType,
                LogoFileSize = entity.LogoFileSize,
                LogoChecksum = entity.LogoChecksum,
                LogoThumbnailData = entity.LogoThumbnailData?.ToArray()
            };

            // Sử dụng navigation properties trực tiếp với try-catch chi tiết
            try
            {
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Đang truy cập ApplicationUser cho partner {entity.Id}");
                var createdByUser = entity.ApplicationUser;
                dto.CreatedByName = createdByUser?.UserName;
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Đã lấy CreatedByName: {dto.CreatedByName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ToListDto] LỖI khi truy cập ApplicationUser cho partner {entity.Id}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ToListDto] StackTrace: {ex.StackTrace}");
                dto.CreatedByName = null;
            }

            try
            {
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Đang truy cập ApplicationUser2 cho partner {entity.Id}");
                var modifiedByUser = entity.ApplicationUser2;
                dto.ModifiedByName = modifiedByUser?.UserName;
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Đã lấy ModifiedByName: {dto.ModifiedByName}");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ToListDto] LỖI khi truy cập ApplicationUser2 cho partner {entity.Id}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ToListDto] StackTrace: {ex.StackTrace}");
                dto.ModifiedByName = null;
            }

            // Tính CategoryNames và CategoryPaths từ navigation properties
            try
            {
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Đang truy cập BusinessPartner_BusinessPartnerCategories cho partner {entity.Id}");
                var categories = entity.BusinessPartner_BusinessPartnerCategories;
                System.Diagnostics.Debug.WriteLine($"[ToListDto] Categories is null: {categories == null}");
                
                if (categories != null && categories.Count > 0)
                {
                    System.Diagnostics.Debug.WriteLine($"[ToListDto] Có {categories.Count} categories");
                    var categoryNames = new List<string>();
                    var categoryPaths = new List<string>();

                    int index = 0;
                    foreach (var mapping in categories)
                    {
                        try
                        {
                            System.Diagnostics.Debug.WriteLine($"[ToListDto] Đang xử lý category mapping {index}");
                            var category = mapping.BusinessPartnerCategory;
                            if (category == null)
                            {
                                System.Diagnostics.Debug.WriteLine($"[ToListDto] Category là null cho mapping {index}");
                                continue;
                            }

                            System.Diagnostics.Debug.WriteLine($"[ToListDto] Category name: {category.CategoryName}");
                            categoryNames.Add(category.CategoryName);

                            // Tính FullPath: chỉ dùng categoryDict nếu có, nếu không chỉ dùng CategoryName
                            // KHÔNG tính từ navigation properties vì sẽ gây lỗi "Cannot access a disposed object"
                            string fullPath;
                            if (categoryDict != null && categoryDict.ContainsKey(category.Id))
                            {
                                fullPath = CalculateCategoryFullPath(category, categoryDict);
                                System.Diagnostics.Debug.WriteLine($"[ToListDto] Full path từ categoryDict: {fullPath}");
                            }
                            else
                            {
                                // Không có categoryDict, chỉ dùng CategoryName
                                // KHÔNG cố tính từ navigation properties vì DataContext đã bị dispose
                                fullPath = category.CategoryName;
                                System.Diagnostics.Debug.WriteLine($"[ToListDto] Không có categoryDict, dùng CategoryName: {fullPath}");
                            }
                            categoryPaths.Add(fullPath);
                            index++;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"[ToListDto] LỖI khi xử lý category mapping {index}: {ex.Message}");
                            System.Diagnostics.Debug.WriteLine($"[ToListDto] StackTrace: {ex.StackTrace}");
                        }
                    }

                    dto.CategoryNames = string.Join(", ", categoryNames);
                    dto.CategoryPaths = string.Join(", ", categoryPaths);
                    dto.FullPath = categoryPaths.FirstOrDefault();
                    System.Diagnostics.Debug.WriteLine($"[ToListDto] Đã xử lý xong categories cho partner {entity.Id}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine($"[ToListDto] Không có categories cho partner {entity.Id}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"[ToListDto] LỖI TỔNG QUÁT khi xử lý categories cho partner {entity.Id}: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"[ToListDto] StackTrace: {ex.StackTrace}");
            }

            System.Diagnostics.Debug.WriteLine($"[ToListDto] Hoàn thành convert partner {entity.Id}");
            return dto;
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartner Entity sang danh sách BusinessPartnerListDto
        /// </summary>
        /// <param name="entities">Danh sách BusinessPartner Entity</param>
        /// <param name="categoryDict">Dictionary chứa tất cả BusinessPartnerCategory entities để tính FullPath (optional)</param>
        /// <returns>Danh sách BusinessPartnerListDto</returns>
        public static IEnumerable<BusinessPartnerListDto> ToBusinessPartnerListDtos(
            this IEnumerable<BusinessPartner> entities,
            Dictionary<Guid, BusinessPartnerCategory> categoryDict = null)
        {
            if (entities == null) return [];

            return entities.Select(entity => entity.ToListDto(categoryDict));
        }

        /// <summary>
        /// Chuyển đổi BusinessPartner Entity sang BusinessPartnerDetailDto
        /// </summary>
        /// <param name="entity">BusinessPartner Entity</param>
        /// <returns>BusinessPartnerDetailDto</returns>
        public static BusinessPartnerDetailDto ToDetailDto(this BusinessPartner entity)
        {
            if (entity == null) return null;

            return new BusinessPartnerDetailDto
            {
                Id = entity.Id,
                PartnerCode = entity.PartnerCode,
                PartnerName = entity.PartnerName,
                PartnerType = entity.PartnerType,
                TaxCode = entity.TaxCode,
                Phone = entity.Phone,
                Email = entity.Email,
                Website = entity.Website,
                Address = entity.Address,
                City = entity.City,
                Country = entity.Country,
                IsActive = entity.IsActive,
                CreatedDate = entity.CreatedDate,
                UpdatedDate = entity.UpdatedDate,
                LogoFileName = entity.LogoFileName,
                LogoRelativePath = entity.LogoRelativePath,
                LogoFullPath = entity.LogoFullPath,
                LogoStorageType = entity.LogoStorageType,
                LogoFileSize = entity.LogoFileSize,
                LogoChecksum = entity.LogoChecksum,
                LogoThumbnailData = entity.LogoThumbnailData?.ToArray()
            };
        }

        /// <summary>
        /// Chuyển đổi BusinessPartnerDetailDto sang BusinessPartner Entity
        /// </summary>
        /// <param name="dto">BusinessPartnerDetailDto</param>
        /// <param name="existingEntity">Entity hiện tại (cho update)</param>
        /// <returns>BusinessPartner Entity</returns>
        public static BusinessPartner ToEntity(this BusinessPartnerDetailDto dto, BusinessPartner existingEntity = null)
        {
            if (dto == null) return null;

            var entity = existingEntity ?? new BusinessPartner();

            // Chỉ set ID nếu là entity mới
            if (existingEntity == null && dto.Id != Guid.Empty)
            {
                entity.Id = dto.Id;
            }

            entity.PartnerCode = dto.PartnerCode;
            entity.PartnerName = dto.PartnerName;
            entity.PartnerType = dto.PartnerType;
            entity.TaxCode = dto.TaxCode;
            entity.Phone = dto.Phone;
            entity.Email = dto.Email;
            entity.Website = dto.Website;
            entity.Address = dto.Address;
            entity.City = dto.City;
            entity.Country = dto.Country;
            entity.IsActive = dto.IsActive;
            entity.CreatedDate = dto.CreatedDate;
            entity.UpdatedDate = dto.UpdatedDate;

            // Copy Logo fields (metadata only)
            entity.LogoFileName = dto.LogoFileName;
            entity.LogoRelativePath = dto.LogoRelativePath;
            entity.LogoFullPath = dto.LogoFullPath;
            entity.LogoStorageType = dto.LogoStorageType;
            entity.LogoFileSize = dto.LogoFileSize;
            entity.LogoChecksum = dto.LogoChecksum;

            // Copy LogoThumbnailData (binary)
            if (dto.LogoThumbnailData != null && dto.LogoThumbnailData.Length > 0)
            {
                entity.LogoThumbnailData = new System.Data.Linq.Binary(dto.LogoThumbnailData);
            }
            else
            {
                entity.LogoThumbnailData = null;
            }

            return entity;
        }

        /// <summary>
        /// Resolve tên loại đối tác từ PartnerType
        /// </summary>
        /// <param name="partnerType">PartnerType (int)</param>
        /// <returns>Tên loại đối tác</returns>
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

        /// <summary>
        /// Tính toán đường dẫn đầy đủ từ gốc đến category sử dụng categoryDict
        /// </summary>
        /// <param name="category">BusinessPartnerCategory entity</param>
        /// <param name="categoryDict">Dictionary chứa tất cả BusinessPartnerCategory entities</param>
        /// <returns>Đường dẫn đầy đủ (ví dụ: "Danh mục A > Danh mục A1")</returns>
        private static string CalculateCategoryFullPath(BusinessPartnerCategory category,
            Dictionary<Guid, BusinessPartnerCategory> categoryDict)
        {
            if (category == null) return string.Empty;

            var pathParts = new List<string> { category.CategoryName };
            var current = category;

            while (current.ParentId.HasValue && categoryDict.ContainsKey(current.ParentId.Value))
            {
                current = categoryDict[current.ParentId.Value];
                pathParts.Insert(0, current.CategoryName);
                if (pathParts.Count > 10) break; // Tránh infinite loop
            }

            return string.Join(" > ", pathParts);
        }

        /// <summary>
        /// Tính toán đường dẫn đầy đủ từ gốc đến category sử dụng navigation properties
        /// Chỉ hoạt động nếu navigation properties đã được load đầy đủ
        /// </summary>
        /// <param name="category">BusinessPartnerCategory entity</param>
        /// <returns>Đường dẫn đầy đủ (ví dụ: "Danh mục A > Danh mục A1") hoặc CategoryName nếu không thể tính đầy đủ</returns>
        private static string CalculateCategoryFullPathFromNavigation(BusinessPartnerCategory category)
        {
            if (category == null) return string.Empty;

            var pathParts = new List<string> { category.CategoryName };
            var current = category;
            var visited = new HashSet<Guid> { category.Id }; // Tránh circular reference

            // Thử tính từ navigation properties nếu đã được load
            while (current.ParentId.HasValue)
            {
                // Kiểm tra circular reference
                if (visited.Contains(current.ParentId.Value))
                    break;

                // Kiểm tra navigation property có được load không
                if (current.BusinessPartnerCategory1 == null)
                {
                    // Navigation property chưa được load, trả về path đã tính được
                    break;
                }

                current = current.BusinessPartnerCategory1;
                visited.Add(current.Id);
                pathParts.Insert(0, current.CategoryName);

                if (pathParts.Count > 10) break; // Tránh infinite loop
            }

            return string.Join(" > ", pathParts);
        }
    }
}