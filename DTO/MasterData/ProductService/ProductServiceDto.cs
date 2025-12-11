using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Linq;
using System.Linq;

namespace DTO.MasterData.ProductService;

/// <summary>
/// Data Transfer Object cho ProductService.
/// Chuyển đổi dữ liệu giữa Entity và UI layer.
/// </summary>
public class ProductServiceDto
{
    #region Properties

    [DisplayName("ID")]
    public Guid Id { get; set; }

    [DisplayName("Mã sản phẩm/dịch vụ")]
    [Required(ErrorMessage = "Mã sản phẩm/dịch vụ không được để trống")]
    [StringLength(50, ErrorMessage = "Mã sản phẩm/dịch vụ không được vượt quá 50 ký tự")]
    public string Code { get; set; }

    [DisplayName("Tên sản phẩm/dịch vụ")]
    [Required(ErrorMessage = "Tên sản phẩm/dịch vụ không được để trống")]
    [StringLength(200, ErrorMessage = "Tên sản phẩm/dịch vụ không được vượt quá 200 ký tự")]
    public string Name { get; set; }

    [DisplayName("Danh mục")]
    [Description("ID của danh mục sản phẩm/dịch vụ")]
    public Guid? CategoryId { get; set; }

    [DisplayName("Tên danh mục")]
    [Description("Tên của danh mục sản phẩm/dịch vụ (để hiển thị)")]
    public string CategoryName { get; set; }

    [DisplayName("Đường dẫn danh mục đầy đủ")]
    [Description("Đường dẫn đầy đủ từ gốc đến danh mục này (ví dụ: Danh mục cha > Danh mục con)")]
    public string CategoryFullPath { get; set; }

    [DisplayName("Loại")]
    [Description("Có phải là dịch vụ không (true = dịch vụ, false = sản phẩm)")]
    public bool IsService { get; set; }

    [DisplayName("Loại hiển thị")]
    [Description("Loại sản phẩm/dịch vụ (hiển thị dạng text)")]
    public string TypeDisplay => IsService ? "Dịch vụ" : "Sản phẩm";

    [DisplayName("Mô tả")]
    [StringLength(500, ErrorMessage = "Mô tả không được vượt quá 500 ký tự")]
    public string Description { get; set; }

    [DisplayName("Đang hoạt động")]
    [Description("Trạng thái hoạt động")]
    public bool IsActive { get; set; }

    [DisplayName("Trạng thái hiển thị")]
    [Description("Trạng thái hoạt động (hiển thị dạng text)")]
    public string StatusDisplay => IsActive ? "Hoạt động" : "Không hoạt động";

    [DisplayName("Ảnh đại diện")]
    [Description("Ảnh đại diện của sản phẩm/dịch vụ")]
    public byte[] ThumbnailImage { get; set; }

    [DisplayName("Có ảnh đại diện")]
    [Description("Kiểm tra xem có ảnh đại diện không")]
    public bool HasThumbnailImage => ThumbnailImage != null && ThumbnailImage.Length > 0;

    [DisplayName("Số biến thể")]
    [Description("Số lượng biến thể sản phẩm")]
    public int VariantCount { get; set; }

    [DisplayName("Số ảnh")]
    [Description("Số lượng ảnh sản phẩm")]
    public int ImageCount { get; set; }

    /// <summary>
    /// Thông tin sản phẩm/dịch vụ dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Format giống CompanyDto.ThongTinHtml (không dùng &lt;size&gt;)
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// </summary>
    [DisplayName("Thông tin HTML")]
    [Description("Thông tin sản phẩm/dịch vụ dưới dạng HTML")]
    public string ThongTinHtml
    {
        get
        {
            var html = string.Empty;

            // Tên sản phẩm/dịch vụ (màu xanh, đậm)
            var productName = Name ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productName))
            {
                html += $"<b><color='blue'>{productName}</color></b>";
            }

            // Mã sản phẩm/dịch vụ (nếu có, màu xám)
            var productCode = Code ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(productCode))
            {
                if (!string.IsNullOrWhiteSpace(html))
                    html += " ";
                html += $"<color='#757575'>({productCode})</color>";
            }

            html += "<br>";

            // Thông tin bổ sung
            var infoParts = new List<string>();

            // Loại (Sản phẩm/Dịch vụ)
            var typeText = TypeDisplay;
            var typeColor = IsService ? "#FF9800" : "#2196F3"; // Màu cam cho dịch vụ, xanh cho sản phẩm
            infoParts.Add($"<color='#757575'>Loại:</color> <color='{typeColor}'><b>{typeText}</b></color>");

            // Danh mục (nếu có)
            if (!string.IsNullOrWhiteSpace(CategoryName))
            {
                infoParts.Add($"<color='#757575'>Danh mục:</color> <b>{CategoryName}</b>");
            }

            // Trạng thái hoạt động
            var statusText = StatusDisplay;
            var statusColor = IsActive ? "#4CAF50" : "#F44336";
            infoParts.Add($"<color='#757575'>Trạng thái:</color> <color='{statusColor}'><b>{statusText}</b></color>");

            if (infoParts.Any())
            {
                html += string.Join(" | ", infoParts) + "<br>";
            }

            // Thông tin số lượng (biến thể, ảnh)
            var countParts = new List<string>();

            if (VariantCount > 0)
            {
                countParts.Add($"<color='#757575'>Biến thể:</color> <b>{VariantCount}</b>");
            }

            if (ImageCount > 0)
            {
                countParts.Add($"<color='#757575'>Ảnh:</color> <b>{ImageCount}</b>");
            }

            if (countParts.Any())
            {
                html += string.Join(" | ", countParts);
            }

            return html;
        }
    }

    /// <summary>
    /// Đường dẫn danh mục đầy đủ dưới dạng HTML theo format DevExpress
    /// Format giống ProductServiceCategoryDto.FullPathHtml (không dùng &lt;size&gt;)
    /// </summary>
    [DisplayName("Đường dẫn danh mục HTML")]
    [Description("Đường dẫn đầy đủ từ gốc đến danh mục này dưới dạng HTML")]
    public string CategoryFullPathHtml
    {
        get
        {
            if (string.IsNullOrWhiteSpace(CategoryFullPath))
                return string.Empty;

            // Tách đường dẫn và format với màu sắc
            // Hỗ trợ nhiều format: " > ", ">", " >" hoặc "> "
            var parts = CategoryFullPath.Split(new[] { " > ", ">", " >", "> " }, StringSplitOptions.None)
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

                // Format giống ProductServiceCategoryDto: không dùng <size>, chỉ dùng <b> và <color>
                htmlParts.Add($"{weight}<color='{color}'>{parts[i]}</color>{weightClose}");
            }

            return string.Join(" <color='#757575'>></color> ", htmlParts);
        }
    }

        
    #endregion
}

/// <summary>
/// Converter để chuyển đổi giữa ProductService Entity và ProductServiceDto
/// </summary>
public static class ProductServiceConverters
{
    /// <summary>
    /// Chuyển đổi ProductService entity sang ProductServiceDto
    /// </summary>
    /// <param name="entity">ProductService entity</param>
    /// <returns>ProductServiceDto</returns>
    public static ProductServiceDto ToDto(this Dal.DataContext.ProductService entity)
    {
        if (entity == null) return null;

        return new ProductServiceDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            CategoryId = entity.CategoryId,
            CategoryName = null, // Sẽ được resolve riêng nếu cần
            IsService = entity.IsService,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ThumbnailImage = entity.ThumbnailImage?.ToArray(),
            VariantCount = 0, // Sẽ được tính toán riêng nếu cần
            ImageCount = 0,   // Sẽ được tính toán riêng nếu cần
        };
    }

    /// <summary>
    /// Chuyển đổi ProductService entity sang ProductServiceDto với resolver cho tên danh mục
    /// </summary>
    /// <param name="entity">ProductService entity</param>
    /// <param name="categoryNameResolver">Hàm resolve tên danh mục từ CategoryId</param>
    /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
    /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
    /// <param name="categoryFullPathResolver">Hàm resolve đường dẫn đầy đủ của danh mục từ CategoryId</param>
    /// <returns>ProductServiceDto</returns>
    public static ProductServiceDto ToDto(this Dal.DataContext.ProductService entity,
        Func<Guid?, string> categoryNameResolver,
        Func<Guid, int> variantCountResolver = null,
        Func<Guid, int> imageCountResolver = null,
        Func<Guid?, string> categoryFullPathResolver = null)
    {
        if (entity == null) return null;

        return new ProductServiceDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            CategoryId = entity.CategoryId,
            CategoryName = categoryNameResolver?.Invoke(entity.CategoryId),
            CategoryFullPath = categoryFullPathResolver?.Invoke(entity.CategoryId),
            IsService = entity.IsService,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ThumbnailImage = entity.ThumbnailImage?.ToArray(),
            VariantCount = variantCountResolver?.Invoke(entity.Id) ?? 0,
            ImageCount = imageCountResolver?.Invoke(entity.Id) ?? 0,
        };
    }

    /// <summary>
    /// Chuyển đổi ProductService entity sang ProductServiceDto với category dictionary để tối ưu hiệu suất
    /// </summary>
    /// <param name="entity">ProductService entity</param>
    /// <param name="categoryDict">Dictionary chứa tất cả ProductServiceCategory entities (key = CategoryId)</param>
    /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
    /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
    /// <returns>ProductServiceDto</returns>
    public static ProductServiceDto ToDto(this Dal.DataContext.ProductService entity,
        Dictionary<Guid, Dal.DataContext.ProductServiceCategory> categoryDict = null,
        Func<Guid, int> variantCountResolver = null,
        Func<Guid, int> imageCountResolver = null)
    {
        if (entity == null) return null;

        string categoryName = null;
        string categoryFullPath = null;

        if (entity.CategoryId.HasValue && categoryDict != null)
        {
            if (categoryDict.TryGetValue(entity.CategoryId.Value, out var category))
            {
                categoryName = category.CategoryName;
                categoryFullPath = CalculateCategoryFullPath(category, categoryDict);
            }
        }

        return new ProductServiceDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            CategoryId = entity.CategoryId,
            CategoryName = categoryName,
            CategoryFullPath = categoryFullPath,
            IsService = entity.IsService,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ThumbnailImage = entity.ThumbnailImage?.ToArray(),
            VariantCount = variantCountResolver?.Invoke(entity.Id) ?? 0,
            ImageCount = imageCountResolver?.Invoke(entity.Id) ?? 0,
        };
    }

    /// <summary>
    /// Tính toán đường dẫn đầy đủ của category từ dictionary
    /// </summary>
    private static string CalculateCategoryFullPath(Dal.DataContext.ProductServiceCategory category, 
        Dictionary<Guid, Dal.DataContext.ProductServiceCategory> categoryDict)
    {
        if (category == null || categoryDict == null) return null;

        try
        {
            var pathParts = new List<string> { category.CategoryName };
            var current = category;

            // Đi ngược lên parent categories
            while (current.ParentId.HasValue)
            {
                if (!categoryDict.TryGetValue(current.ParentId.Value, out current))
                    break;
                pathParts.Insert(0, current.CategoryName);
                
                // Tránh infinite loop
                if (pathParts.Count > 10)
                    break;
            }

            return string.Join(" > ", pathParts);
        }
        catch
        {
            return category.CategoryName;
        }
    }

    /// <summary>
    /// Chuyển đổi danh sách ProductService entities sang danh sách ProductServiceDto
    /// </summary>
    /// <param name="entities">Danh sách ProductService entities</param>
    /// <param name="categoryNameResolver">Hàm resolve tên danh mục từ CategoryId</param>
    /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
    /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
    /// <param name="categoryFullPathResolver">Hàm resolve đường dẫn đầy đủ của danh mục từ CategoryId</param>
    /// <returns>Danh sách ProductServiceDto</returns>
    public static List<ProductServiceDto> ToDtoList(this IEnumerable<Dal.DataContext.ProductService> entities,
        Func<Guid?, string> categoryNameResolver = null,
        Func<Guid, int> variantCountResolver = null,
        Func<Guid, int> imageCountResolver = null,
        Func<Guid?, string> categoryFullPathResolver = null)
    {
        if (entities == null) return new List<ProductServiceDto>();

        return entities.Select(x => x.ToDto(categoryNameResolver, variantCountResolver, imageCountResolver, categoryFullPathResolver)).ToList();
    }

    /// <summary>
    /// Chuyển đổi danh sách ProductService entities sang danh sách ProductServiceDto với category dictionary để tối ưu hiệu suất
    /// </summary>
    /// <param name="entities">Danh sách ProductService entities</param>
    /// <param name="categoryDict">Dictionary chứa tất cả ProductServiceCategory entities (key = CategoryId)</param>
    /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
    /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
    /// <returns>Danh sách ProductServiceDto</returns>
    public static List<ProductServiceDto> ToDtoList(this IEnumerable<Dal.DataContext.ProductService> entities,
        Dictionary<Guid, Dal.DataContext.ProductServiceCategory> categoryDict = null,
        Func<Guid, int> variantCountResolver = null,
        Func<Guid, int> imageCountResolver = null)
    {
        if (entities == null) return new List<ProductServiceDto>();

        return entities.Select(x => x.ToDto(categoryDict, variantCountResolver, imageCountResolver)).ToList();
    }

    /// <summary>
    /// Chuyển đổi ProductServiceDto sang ProductService entity
    /// </summary>
    /// <param name="dto">ProductServiceDto</param>
    /// <returns>ProductService entity</returns>
    public static Dal.DataContext.ProductService ToEntity(this ProductServiceDto dto)
    {
        if (dto == null) return null;

        return new Dal.DataContext.ProductService
        {
            Id = dto.Id,
            Code = dto.Code,
            Name = dto.Name,
            CategoryId = dto.CategoryId,
            IsService = dto.IsService,
            Description = dto.Description,
            IsActive = dto.IsActive,
            ThumbnailImage = dto.ThumbnailImage != null ? new Binary(dto.ThumbnailImage) : null,
        };
    }
}