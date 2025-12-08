using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using Dal.DataContext;

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
        /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;, &lt;size&gt;
        /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
        /// </summary>
        [DisplayName("Thông tin HTML")]
        [Description("Thông tin danh mục dưới dạng HTML")]
        public string CategoryInfoHtml
        {
            get
            {
                var html = string.Empty;

                // Tên danh mục (màu xanh, đậm, size 12)
                if (!string.IsNullOrWhiteSpace(CategoryName))
                {
                    html += $"<size=12><b><color='blue'>{CategoryName}</color></b></size>";
                }

                // Mã danh mục (nếu có, màu xám, size 9)
                if (!string.IsNullOrWhiteSpace(CategoryCode))
                {
                    if (!string.IsNullOrWhiteSpace(html))
                        html += " ";
                    html += $"<size=9><color='#757575'>({CategoryCode})</color></size>";
                }

                html += "<br>";

                // Thông tin bổ sung
                var additionalInfo = new List<string>();

                // Trạng thái hoạt động
                var statusText = IsActive ? "Hoạt động" : "Không hoạt động";
                var statusColor = IsActive ? "#4CAF50" : "#F44336";
                additionalInfo.Add(
                    $"<size=9><color='#757575'>Trạng thái:</color></size> <size=10><color='{statusColor}'><b>{statusText}</b></color></size>");

                // Số lượng đối tác
                if (PartnerCount > 0)
                {
                    additionalInfo.Add(
                        $"<size=9><color='#757575'>Số đối tác:</color></size> <size=10><color='#212121'><b>{PartnerCount}</b></color></size>");
                }

                // Thứ tự sắp xếp
                if (SortOrder.HasValue)
                {
                    additionalInfo.Add(
                        $"<size=9><color='#757575'>Thứ tự:</color></size> <size=10><color='#212121'><b>{SortOrder.Value}</b></color></size>");
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
                var parts = FullPath.Split([" > "], StringSplitOptions.None);
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

                return string.Join(" <size=9><color='#757575'>></color></size> ", htmlParts);
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
                    var createdInfo =
                        $"<size=9><color='#757575'>Tạo:</color></size> <size=10><color='#212121'>{CreatedDate:dd/MM/yyyy HH:mm}</color></size>";
                    if (!string.IsNullOrWhiteSpace(CreatedByName))
                    {
                        createdInfo +=
                            $" <size=9><color='#757575'>bởi</color></size> <size=10><color='#212121'><b>{CreatedByName}</b></color></size>";
                    }

                    infoParts.Add(createdInfo);
                }

                // Người cập nhật
                if (ModifiedDate.HasValue)
                {
                    var modifiedInfo =
                        $"<size=9><color='#757575'>Sửa:</color></size> <size=10><color='#212121'>{ModifiedDate.Value:dd/MM/yyyy HH:mm}</color></size>";
                    if (!string.IsNullOrWhiteSpace(ModifiedByName))
                    {
                        modifiedInfo +=
                            $" <size=9><color='#757575'>bởi</color></size> <size=10><color='#212121'><b>{ModifiedByName}</b></color></size>";
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

    public static class BusinessPartnerCategoryConverters
    {
        public static BusinessPartnerCategoryDto ToDto(this BusinessPartnerCategory entity)
        {
            if (entity == null) return null;
            return new BusinessPartnerCategoryDto
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentId = entity.ParentId,
                CategoryType = entity.ParentId == null ? "Category chính" : "Sub-category",
                PartnerCount = 0, // Sẽ được cập nhật bởi method có đếm
                Level = 0, // Sẽ được tính toán
                HasChildren = false, // Sẽ được cập nhật
                FullPath = entity.CategoryName, // Sẽ được cập nhật với đường dẫn đầy đủ
                CategoryCode = entity.CategoryCode,
                IsActive = entity.IsActive,
                SortOrder = entity.SortOrder,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };
        }

        public static BusinessPartnerCategoryDto ToDtoWithCount(this BusinessPartnerCategory entity, int partnerCount)
        {
            if (entity == null) return null;
            return new BusinessPartnerCategoryDto
            {
                Id = entity.Id,
                CategoryName = entity.CategoryName,
                Description = entity.Description,
                ParentId = entity.ParentId,
                CategoryType = entity.ParentId == null ? "Category chính" : "Sub-category",
                PartnerCount = partnerCount,
                Level = 0, // Sẽ được tính toán
                HasChildren = false, // Sẽ được cập nhật
                FullPath = entity.CategoryName, // Sẽ được cập nhật với đường dẫn đầy đủ
                CategoryCode = entity.CategoryCode,
                IsActive = entity.IsActive,
                SortOrder = entity.SortOrder,
                CreatedDate = entity.CreatedDate,
                CreatedBy = entity.CreatedBy,
                ModifiedDate = entity.ModifiedDate,
                ModifiedBy = entity.ModifiedBy
            };
        }

        public static IEnumerable<BusinessPartnerCategoryDto> ToDtos(this IEnumerable<BusinessPartnerCategory> entities)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDto());
        }

        public static IEnumerable<BusinessPartnerCategoryDto> ToDtosWithCount(
            this IEnumerable<BusinessPartnerCategory> entities,
            Func<Guid, int> partnerCountResolver)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDtoWithCount(partnerCountResolver?.Invoke(e.Id) ?? 0));
        }

        /// <summary>
        /// Chuyển đổi BusinessPartnerCategory sang DTO với đếm số lượng đối tác từ mapping table.
        /// </summary>
        /// <param name="entity">Entity BusinessPartnerCategory</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng đối tác theo CategoryId</param>
        /// <returns>BusinessPartnerCategoryDto với PartnerCount</returns>
        public static BusinessPartnerCategoryDto ToDtoWithMappingCount(this BusinessPartnerCategory entity,
            Dictionary<Guid, int> mappingCounts)
        {
            if (entity == null) return null;
            var partnerCount = mappingCounts?.ContainsKey(entity.Id) == true ? mappingCounts[entity.Id] : 0;

            // Debug logging
            Debug.WriteLine(
                $"ToDtoWithMappingCount - Category: {entity.CategoryName}, ID: {entity.Id}, PartnerCount: {partnerCount}");

            return entity.ToDtoWithCount(partnerCount);
        }

        /// <summary>
        /// Chuyển đổi danh sách BusinessPartnerCategory sang DTO với đếm số lượng đối tác.
        /// </summary>
        /// <param name="entities">Danh sách entities</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng đối tác theo CategoryId</param>
        /// <returns>Danh sách BusinessPartnerCategoryDto với PartnerCount</returns>
        public static IEnumerable<BusinessPartnerCategoryDto> ToDtosWithMappingCount(
            this IEnumerable<BusinessPartnerCategory> entities,
            Dictionary<Guid, int> mappingCounts)
        {
            if (entities == null) return Enumerable.Empty<BusinessPartnerCategoryDto>();
            return entities.Select(e => e.ToDtoWithMappingCount(mappingCounts));
        }

        public static BusinessPartnerCategory ToEntity(this BusinessPartnerCategoryDto dto,
            BusinessPartnerCategory destination = null)
        {
            if (dto == null) return null;
            var entity = destination ?? new BusinessPartnerCategory();

            entity.Id = dto.Id == Guid.Empty ? entity.Id : dto.Id;
            entity.CategoryName = dto.CategoryName;
            entity.Description = dto.Description;
            entity.ParentId = dto.ParentId;
            entity.CategoryCode = dto.CategoryCode;
            entity.IsActive = dto.IsActive;
            entity.SortOrder = dto.SortOrder;

            // Chỉ cập nhật CreatedDate và CreatedBy nếu là entity mới
            if (destination == null)
            {
                entity.CreatedDate = dto.CreatedDate != default(DateTime) ? dto.CreatedDate : DateTime.Now;
                entity.CreatedBy = dto.CreatedBy;
            }
            else
            {
                // Khi update, chỉ cập nhật ModifiedDate và ModifiedBy
                entity.ModifiedDate = DateTime.Now;
                entity.ModifiedBy = dto.ModifiedBy;
            }

            return entity;
        }

        /// <summary>
        /// Chuyển đổi danh sách categories thành hierarchical DTOs với thông tin đầy đủ
        /// </summary>
        /// <param name="entities">Danh sách entities</param>
        /// <param name="mappingCounts">Dictionary chứa số lượng đối tác theo CategoryId</param>
        /// <returns>Danh sách DTOs với thông tin hierarchical, sắp xếp theo cấu trúc cây</returns>
        public static IEnumerable<BusinessPartnerCategoryDto> ToDtosWithHierarchy(
            this IEnumerable<BusinessPartnerCategory> entities,
            Dictionary<Guid, int> mappingCounts)
        {
            if (entities == null) return [];

            var entityList = entities.ToList();
            var entityDict = entityList.ToDictionary(e => e.Id);

            var dtoList = entityList.Select(entity =>
            {
                var dto = entity.ToDtoWithMappingCount(mappingCounts);

                // Tính toán Level
                dto.Level = CalculateLevel(entity, entityDict);

                // Tính toán HasChildren (chỉ sub-categories)
                var hasSubCategories = entityList.Any(e => e.ParentId == entity.Id);
                dto.HasChildren = hasSubCategories;

                // Tính toán FullPath
                dto.FullPath = CalculateFullPath(entity, entityDict);

                // Lấy tên parent category
                if (entity.ParentId.HasValue && entityDict.TryGetValue(entity.ParentId.Value, out var value))
                {
                    dto.ParentCategoryName = value.CategoryName;
                }

                return dto;
            }).ToList();

            // Sắp xếp theo cấu trúc cây: parent trước, children sau
            return SortHierarchical(dtoList);
        }


        /// <summary>
        /// Sắp xếp danh sách DTO theo cấu trúc cây
        /// </summary>
        private static IEnumerable<BusinessPartnerCategoryDto> SortHierarchical(
            List<BusinessPartnerCategoryDto> dtoList)
        {
            var result = new List<BusinessPartnerCategoryDto>();
            var dtoDict = dtoList.ToDictionary(d => d.Id);

            // Thêm các root categories trước (ParentId = null)
            var rootCategories = dtoList.Where(d => !d.ParentId.HasValue).OrderBy(d => d.CategoryName);

            foreach (var root in rootCategories)
            {
                result.Add(root);
                AddChildrenRecursive(root, dtoDict, result);
            }

            return result;
        }

        /// <summary>
        /// Thêm children một cách đệ quy
        /// </summary>
        private static void AddChildrenRecursive(BusinessPartnerCategoryDto parent,
            Dictionary<Guid, BusinessPartnerCategoryDto> dtoDict, List<BusinessPartnerCategoryDto> result)
        {
            var children = dtoDict.Values
                .Where(d => d.ParentId == parent.Id)
                .OrderBy(d => d.CategoryName);

            foreach (var child in children)
            {
                result.Add(child);
                AddChildrenRecursive(child, dtoDict, result);
            }
        }

        /// <summary>
        /// Tính toán cấp độ của category trong cây phân cấp
        /// </summary>
        private static int CalculateLevel(BusinessPartnerCategory entity,
            Dictionary<Guid, BusinessPartnerCategory> entityDict)
        {
            int level = 0;
            var current = entity;
            while (current.ParentId.HasValue && entityDict.ContainsKey(current.ParentId.Value))
            {
                level++;
                current = entityDict[current.ParentId.Value];
                if (level > 10) break; // Tránh infinite loop
            }

            return level;
        }

        /// <summary>
        /// Tính toán đường dẫn đầy đủ từ gốc đến category
        /// </summary>
        private static string CalculateFullPath(BusinessPartnerCategory entity,
            Dictionary<Guid, BusinessPartnerCategory> entityDict)
        {
            var pathParts = new List<string> { entity.CategoryName };
            var current = entity;

            while (current.ParentId.HasValue && entityDict.ContainsKey(current.ParentId.Value))
            {
                current = entityDict[current.ParentId.Value];
                pathParts.Insert(0, current.CategoryName);
                if (pathParts.Count > 10) break; // Tránh infinite loop
            }

            return string.Join(" > ", pathParts);
        }
    }
}