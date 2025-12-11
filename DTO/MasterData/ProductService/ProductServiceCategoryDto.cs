using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;

namespace DTO.MasterData.ProductService;

public class ProductServiceCategoryDto
{
    [DisplayName("ID")]
    public Guid Id { get; set; }

    [DisplayName("Mã danh mục")]
    [Required(ErrorMessage = "Mã danh mục không được để trống")]
    [StringLength(200, ErrorMessage = "Mã danh mục không được vượt quá 200 ký tự")]
    public string CategoryCode { get; set; }

    [DisplayName("Tên danh mục")]
    [Required(ErrorMessage = "Tên danh mục không được để trống")]
    [StringLength(200, ErrorMessage = "Tên danh mục không được vượt quá 200 ký tự")]
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

    [DisplayName("Cấp độ")]
    [Description("Cấp độ trong cây phân cấp (0 = gốc, 1 = con, ...)")]
    public int Level { get; set; }

    [DisplayName("Có danh mục con")]
    [Description("Có danh mục con hay không")]
    public bool HasChildren { get; set; }

    [DisplayName("Đường dẫn đầy đủ")]
    [Description("Đường dẫn đầy đủ từ gốc đến danh mục này")]
    public string FullPath { get; set; }

    [DisplayName("Số lượng sản phẩm/dịch vụ")]
    [Description("Tổng số sản phẩm/dịch vụ thuộc danh mục này")]
    public int ProductCount { get; set; }

    [DisplayName("Loại danh mục")]
    [Description("Category chính hoặc Sub-category")]
    public string CategoryType { get; set; }

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
    /// Format giống BusinessPartnerCategoryDto.CategoryInfoHtml (không dùng &lt;size&gt;)
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

            // Số lượng sản phẩm/dịch vụ
            if (ProductCount > 0)
            {
                additionalInfo.Add(
                    $"<color='#757575'>Số sản phẩm/dịch vụ:</color> <b>{ProductCount}</b>");
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
    /// Format giống BusinessPartnerCategoryDto.FullPathHtml (không dùng &lt;size&gt;)
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

                // Format giống BusinessPartnerCategoryDto: không dùng <size>, chỉ dùng <b> và <color>
                htmlParts.Add($"{weight}<color='{color}'>{parts[i]}</color>{weightClose}");
            }

            return string.Join(" <color='#757575'>></color> ", htmlParts);
        }
    }

    /// <summary>
    /// Thông tin audit (người tạo/sửa) dưới dạng HTML theo format DevExpress
    /// Format giống BusinessPartnerCategoryDto.AuditInfoHtml (không dùng &lt;size&gt;)
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

public static class ProductServiceCategoryConverters
{
    public static ProductServiceCategoryDto ToDto(this ProductServiceCategory entity)
    {
        if (entity == null) return null;
        return new ProductServiceCategoryDto
        {
            Id = entity.Id,
            CategoryCode = entity.CategoryCode,
            CategoryName = entity.CategoryName,
            Description = entity.Description,
            ParentId = entity.ParentId,
            CategoryType = entity.ParentId == null ? "Category chính" : "Sub-category",
            ProductCount = 0, // Sẽ được cập nhật bởi method có đếm
            Level = 0, // Sẽ được tính toán
            HasChildren = false, // Sẽ được cập nhật
            FullPath = entity.CategoryName, // Sẽ được cập nhật với đường dẫn đầy đủ
            IsActive = entity.IsActive,
            SortOrder = entity.SortOrder,
            CreatedDate = entity.CreatedDate,
            CreatedBy = entity.CreatedBy,
            ModifiedDate = entity.ModifiedDate,
            ModifiedBy = entity.ModifiedBy
        };
    }

    public static ProductServiceCategoryDto ToDtoWithCount(this ProductServiceCategory entity, int productCount)
    {
        if (entity == null) return null;
        return new ProductServiceCategoryDto
        {
            Id = entity.Id,
            CategoryCode = entity.CategoryCode,
            CategoryName = entity.CategoryName,
            Description = entity.Description,
            ParentId = entity.ParentId,
            CategoryType = entity.ParentId == null ? "Category chính" : "Sub-category",
            ProductCount = productCount,
            Level = 0, // Sẽ được tính toán
            HasChildren = false, // Sẽ được cập nhật
            FullPath = entity.CategoryName, // Sẽ được cập nhật với đường dẫn đầy đủ
            IsActive = entity.IsActive,
            SortOrder = entity.SortOrder,
            CreatedDate = entity.CreatedDate,
            CreatedBy = entity.CreatedBy,
            ModifiedDate = entity.ModifiedDate,
            ModifiedBy = entity.ModifiedBy
        };
    }

    public static IEnumerable<ProductServiceCategoryDto> ToDtos(this IEnumerable<ProductServiceCategory> entities)
    {
        if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();
        return entities.Select(e => e.ToDto());
    }

    public static IEnumerable<ProductServiceCategoryDto> ToDtosWithCount(this IEnumerable<ProductServiceCategory> entities,
        Func<Guid, int> productCountResolver)
    {
        if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();
        return entities.Select(e => e.ToDtoWithCount(productCountResolver?.Invoke(e.Id) ?? 0));
    }

    /// <summary>
    /// Chuyển đổi ProductServiceCategory sang DTO với đếm số lượng sản phẩm/dịch vụ từ mapping table.
    /// </summary>
    /// <param name="entity">Entity ProductServiceCategory</param>
    /// <param name="mappingCounts">Dictionary chứa số lượng sản phẩm/dịch vụ theo CategoryId</param>
    /// <returns>ProductServiceCategoryDto với ProductCount</returns>
    public static ProductServiceCategoryDto ToDtoWithMappingCount(this ProductServiceCategory entity,
        Dictionary<Guid, int> mappingCounts)
    {
        if (entity == null) return null;
        var productCount = mappingCounts?.ContainsKey(entity.Id) == true ? mappingCounts[entity.Id] : 0;

        // Debug logging
        Debug.WriteLine($"ToDtoWithMappingCount - Category: {entity.CategoryName}, ID: {entity.Id}, ProductCount: {productCount}");

        return entity.ToDtoWithCount(productCount);
    }

    /// <summary>
    /// Chuyển đổi danh sách ProductServiceCategory sang DTO với đếm số lượng sản phẩm/dịch vụ.
    /// </summary>
    /// <param name="entities">Danh sách entities</param>
    /// <param name="mappingCounts">Dictionary chứa số lượng sản phẩm/dịch vụ theo CategoryId</param>
    /// <returns>Danh sách ProductServiceCategoryDto với ProductCount</returns>
    public static IEnumerable<ProductServiceCategoryDto> ToDtosWithMappingCount(this IEnumerable<ProductServiceCategory> entities,
        Dictionary<Guid, int> mappingCounts)
    {
        if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();
        return entities.Select(e => e.ToDtoWithMappingCount(mappingCounts));
    }

    public static ProductServiceCategory ToEntity(this ProductServiceCategoryDto dto, ProductServiceCategory destination = null)
    {
        if (dto == null) return null;
        var entity = destination ?? new ProductServiceCategory();

        entity.Id = dto.Id == Guid.Empty ? entity.Id : dto.Id;
        entity.CategoryCode = dto.CategoryCode;
        entity.CategoryName = dto.CategoryName;
        entity.Description = dto.Description;
        entity.ParentId = dto.ParentId;
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
    /// <param name="mappingCounts">Dictionary chứa số lượng sản phẩm/dịch vụ theo CategoryId</param>
    /// <returns>Danh sách DTOs với thông tin hierarchical, sắp xếp theo cấu trúc cây</returns>
    public static IEnumerable<ProductServiceCategoryDto> ToDtosWithHierarchy(this IEnumerable<ProductServiceCategory> entities,
        Dictionary<Guid, int> mappingCounts)
    {
        if (entities == null) return Enumerable.Empty<ProductServiceCategoryDto>();

        var entityList = entities.ToList();
        var entityDict = entityList.ToDictionary(e => e.Id);

        // Tạo DTOs với số lượng ban đầu (chỉ sản phẩm/dịch vụ trực tiếp)
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
            if (entity.ParentId.HasValue && entityDict.ContainsKey(entity.ParentId.Value))
            {
                dto.ParentCategoryName = entityDict[entity.ParentId.Value].CategoryName;
            }

            // Map các thuộc tính audit (nếu có navigation properties)
            // Note: CreatedByName và ModifiedByName sẽ được set từ ApplicationUser nếu có
            // Để đơn giản, ở đây chỉ map các thuộc tính cơ bản

            return dto;
        }).ToList();

        // Cộng tổng số lượng từ các danh mục con
        CalculateTotalProductCounts(dtoList, entityDict);

        // Sắp xếp theo cấu trúc cây: parent trước, children sau
        return SortHierarchical(dtoList);
    }

    /// <summary>
    /// Sắp xếp danh sách DTO theo cấu trúc cây
    /// </summary>
    private static IEnumerable<ProductServiceCategoryDto> SortHierarchical(List<ProductServiceCategoryDto> dtoList)
    {
        var result = new List<ProductServiceCategoryDto>();
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
    private static void AddChildrenRecursive(ProductServiceCategoryDto parent,
        Dictionary<Guid, ProductServiceCategoryDto> dtoDict, List<ProductServiceCategoryDto> result)
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
    private static int CalculateLevel(ProductServiceCategory entity, Dictionary<Guid, ProductServiceCategory> entityDict)
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
    private static string CalculateFullPath(ProductServiceCategory entity, Dictionary<Guid, ProductServiceCategory> entityDict)
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

    /// <summary>
    /// Tính toán tổng số lượng sản phẩm/dịch vụ bao gồm cả các danh mục con
    /// </summary>
    /// <param name="dtoList">Danh sách DTOs</param>
    /// <param name="entityDict">Dictionary entities để xác định quan hệ parent-child</param>
    private static void CalculateTotalProductCounts(List<ProductServiceCategoryDto> dtoList, Dictionary<Guid, ProductServiceCategory> entityDict)
    {
        var dtoDict = dtoList.ToDictionary(d => d.Id);

        // Lưu trữ số lượng trực tiếp để tránh bị ghi đè
        var directCounts = dtoList.ToDictionary(d => d.Id, d => d.ProductCount);

        // Tính tổng số lượng cho mỗi danh mục (bao gồm cả con)
        foreach (var dto in dtoList)
        {
            var totalCount = CalculateTotalCountForCategory(dto.Id, dtoDict, entityDict, directCounts);
            dto.ProductCount = totalCount;

            // Debug logging
            Debug.WriteLine($"CalculateTotalProductCounts - Category: {dto.CategoryName}, Direct: {directCounts[dto.Id]}, Total: {totalCount}");
        }
    }

    /// <summary>
    /// Tính tổng số lượng sản phẩm/dịch vụ cho một danh mục và tất cả các danh mục con
    /// </summary>
    /// <param name="categoryId">ID của danh mục</param>
    /// <param name="dtoDict">Dictionary DTOs</param>
    /// <param name="entityDict">Dictionary entities</param>
    /// <param name="directCounts">Dictionary chứa số lượng trực tiếp của mỗi danh mục</param>
    /// <returns>Tổng số lượng sản phẩm/dịch vụ</returns>
    private static int CalculateTotalCountForCategory(Guid categoryId, Dictionary<Guid, ProductServiceCategoryDto> dtoDict,
        Dictionary<Guid, ProductServiceCategory> entityDict, Dictionary<Guid, int> directCounts)
    {
        if (!dtoDict.ContainsKey(categoryId))
            return 0;

        var directCount = directCounts.ContainsKey(categoryId) ? directCounts[categoryId] : 0;

        // Tìm tất cả danh mục con
        var childCategories = entityDict.Values.Where(e => e.ParentId == categoryId);

        var childCount = 0;
        foreach (var child in childCategories)
        {
            childCount += CalculateTotalCountForCategory(child.Id, dtoDict, entityDict, directCounts);
        }

        var totalCount = directCount + childCount;

        // Debug logging
        var dto = dtoDict[categoryId];
        Debug.WriteLine($"  CalculateTotalCountForCategory - {dto.CategoryName}: Direct={directCount}, Children={childCount}, Total={totalCount}");

        return totalCount;
    }
}