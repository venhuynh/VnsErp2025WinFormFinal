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
    /// <returns>ProductServiceDto</returns>
    public static ProductServiceDto ToDto(this Dal.DataContext.ProductService entity,
        Func<Guid?, string> categoryNameResolver,
        Func<Guid, int> variantCountResolver = null,
        Func<Guid, int> imageCountResolver = null)
    {
        if (entity == null) return null;

        return new ProductServiceDto
        {
            Id = entity.Id,
            Code = entity.Code,
            Name = entity.Name,
            CategoryId = entity.CategoryId,
            CategoryName = categoryNameResolver?.Invoke(entity.CategoryId),
            IsService = entity.IsService,
            Description = entity.Description,
            IsActive = entity.IsActive,
            ThumbnailImage = entity.ThumbnailImage?.ToArray(),
            VariantCount = variantCountResolver?.Invoke(entity.Id) ?? 0,
            ImageCount = imageCountResolver?.Invoke(entity.Id) ?? 0,
        };
    }

    /// <summary>
    /// Chuyển đổi danh sách ProductService entities sang danh sách ProductServiceDto
    /// </summary>
    /// <param name="entities">Danh sách ProductService entities</param>
    /// <param name="categoryNameResolver">Hàm resolve tên danh mục từ CategoryId</param>
    /// <param name="variantCountResolver">Hàm resolve số lượng biến thể từ ProductServiceId</param>
    /// <param name="imageCountResolver">Hàm resolve số lượng hình ảnh từ ProductServiceId</param>
    /// <returns>Danh sách ProductServiceDto</returns>
    public static List<ProductServiceDto> ToDtoList(this IEnumerable<Dal.DataContext.ProductService> entities,
        Func<Guid?, string> categoryNameResolver = null,
        Func<Guid, int> variantCountResolver = null,
        Func<Guid, int> imageCountResolver = null)
    {
        if (entities == null) return new List<ProductServiceDto>();

        return entities.Select(x => x.ToDto(categoryNameResolver, variantCountResolver, imageCountResolver)).ToList();
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