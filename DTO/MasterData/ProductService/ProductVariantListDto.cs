using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;

namespace DTO.MasterData.ProductService;

/// <summary>
/// DTO danh sách cho ProductVariant - Sử dụng cho danh sách, dropdown, combo
/// </summary>
public class ProductVariantListDto
{
    [Display(Name = "ID")]
    public Guid Id { get; set; }

    [Display(Name = "Mã sản phẩm")]
    public string ProductCode { get; set; }

    [Display(Name = "Tên sản phẩm")]
    public string ProductName { get; set; }

    [Display(Name = "Mã biến thể")]
    public string VariantCode { get; set; }

    [Display(Name = "Tên biến thể đầy đủ")]
    public string VariantFullName { get; set; }

    [Display(Name = "Đơn vị tính")]
    public string UnitName { get; set; }

    [Display(Name = "Trạng thái")]
    public bool IsActive { get; set; }

        

    [Display(Name = "Số hình ảnh")]
    public int ImageCount { get; set; }

    [Display(Name = "Hình ảnh thumbnail")]
    [Description("Hình ảnh thumbnail của biến thể sản phẩm")]
    public byte[] ThumbnailImage { get; set; }

    [Display(Name = "Tên đầy đủ")]
    public string FullName => $"{VariantCode} - {ProductName} {VariantFullName} ({UnitName})";

    [Display(Name = "Hiển thị")]
    public string DisplayText => $"{ProductCode}-{VariantCode} - {ProductName} ({UnitName})";

    /// <summary>
    /// Thông tin biến thể đầy đủ dưới dạng ProductVariantDto
    /// Chứa đầy đủ thông tin về biến thể bao gồm thuộc tính, hình ảnh, v.v.
    /// </summary>
    [Display(Name = "Thông tin biến thể đầy đủ")]
    [Description("Thông tin biến thể đầy đủ dưới dạng ProductVariantDto")]
    public ProductVariantDto FullVariantInfo { get; set; }
}

/// <summary>
/// Converter giữa ProductVariant entity và ProductVariantDto
/// </summary>
public static class ProductVariantConverters
{
    #region Entity to DTO

    /// <summary>
    /// Chuyển đổi ProductVariant entity thành ProductVariantDto
    /// </summary>
    /// <param name="entity">ProductVariant entity</param>
    /// <param name="getProductName">Function để lấy tên sản phẩm</param>
    /// <param name="getProductCode">Function để lấy mã sản phẩm</param>
    /// <param name="getUnitName">Function để lấy tên đơn vị</param>
    /// <param name="getUnitCode">Function để lấy mã đơn vị</param>
    /// <returns>ProductVariantDto</returns>
    public static ProductVariantDto ToDto(
        this ProductVariant entity,
        Func<Guid, string> getProductName = null,
        Func<Guid, string> getProductCode = null,
        Func<Guid, string> getUnitName = null,
        Func<Guid, string> getUnitCode = null)
    {
        if (entity == null) return null;

        var dto = new ProductVariantDto
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            ProductCode = getProductCode?.Invoke(entity.ProductId),
            ProductName = getProductName?.Invoke(entity.ProductId),
            VariantCode = entity.VariantCode,
            UnitId = entity.UnitId,
            UnitCode = getUnitCode?.Invoke(entity.UnitId),
            UnitName = getUnitName?.Invoke(entity.UnitId),
            IsActive = entity.IsActive,
            ThumbnailImage = entity.ThumbnailImage?.ToArray(),
            AttributeCount = entity.VariantAttributes?.Count ?? 0,
            ImageCount = 0 // ProductVariant không còn ProductImages navigation property, ProductImage không còn VariantId
        };

        // Convert attributes
        if (entity.VariantAttributes != null && entity.VariantAttributes.Any())
        {
            dto.Attributes = entity.VariantAttributes.Select(va => new ProductVariantAttributeDto
            {
                AttributeId = va.AttributeId,
                AttributeName = va.Attribute?.Name,
                AttributeValueId = va.AttributeValueId,
                AttributeValue = va.AttributeValue?.Value,
                Description = va.Attribute?.Description,
            }).ToList();
        }

        // Convert images - ProductVariant không còn ProductImages navigation property
        // ProductImage không còn VariantId, chỉ có ProductId
        dto.Images = new List<ProductImageDto>();

        return dto;
    }

    /// <summary>
    /// Chuyển đổi danh sách ProductVariant entities thành danh sách ProductVariantDto
    /// </summary>
    /// <param name="entities">Danh sách ProductVariant entities</param>
    /// <param name="getProductName">Function để lấy tên sản phẩm</param>
    /// <param name="getProductCode">Function để lấy mã sản phẩm</param>
    /// <param name="getUnitName">Function để lấy tên đơn vị</param>
    /// <param name="getUnitCode">Function để lấy mã đơn vị</param>
    /// <returns>Danh sách ProductVariantDto</returns>
    public static List<ProductVariantDto> ToDtoList(
        this IEnumerable<ProductVariant> entities,
        Func<Guid, string> getProductName = null,
        Func<Guid, string> getProductCode = null,
        Func<Guid, string> getUnitName = null,
        Func<Guid, string> getUnitCode = null)
    {
        if (entities == null) return [];

        return entities.Select(entity => ToDto(entity, getProductName,
            getProductCode,
            getUnitName,
            getUnitCode
        )).ToList();
    }

    #endregion

    #region DTO to Entity

    /// <summary>
    /// Chuyển đổi ProductVariantDto thành ProductVariant entity
    /// </summary>
    /// <param name="dto">ProductVariantDto</param>
    /// <returns>ProductVariant entity</returns>
    public static ProductVariant ToEntity(this ProductVariantDto dto)
    {
        if (dto == null) return null;

        var entity = new ProductVariant
        {
            Id = dto.Id,
            ProductId = dto.ProductId,
            VariantCode = dto.VariantCode,
            UnitId = dto.UnitId,
            IsActive = dto.IsActive,
            ThumbnailImage = dto.ThumbnailImage
        };

        return entity;
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo ProductVariantDto mới với thông tin cơ bản
    /// </summary>
    /// <param name="productId">ID sản phẩm gốc</param>
    /// <param name="variantCode">Mã biến thể</param>
    /// <param name="unitId">ID đơn vị tính</param>
    /// <param name="getProductName">Function để lấy tên sản phẩm</param>
    /// <param name="getProductCode">Function để lấy mã sản phẩm</param>
    /// <param name="getUnitName">Function để lấy tên đơn vị</param>
    /// <param name="getUnitCode">Function để lấy mã đơn vị</param>
    /// <returns>ProductVariantDto mới</returns>
    public static ProductVariantDto CreateNew(
        Guid productId,
        string variantCode,
        Guid unitId,
        Func<Guid, string> getProductName = null,
        Func<Guid, string> getProductCode = null,
        Func<Guid, string> getUnitName = null,
        Func<Guid, string> getUnitCode = null)
    {
        var dto = new ProductVariantDto(productId, variantCode, unitId)
        {
            ProductCode = getProductCode?.Invoke(productId),
            ProductName = getProductName?.Invoke(productId),
            UnitCode = getUnitCode?.Invoke(unitId),
            UnitName = getUnitName?.Invoke(unitId)
        };

        return dto;
    }

    /// <summary>
    /// Tạo danh sách ProductVariantDto từ template
    /// </summary>
    /// <param name="productId">ID sản phẩm gốc</param>
    /// <param name="unitId">ID đơn vị tính</param>
    /// <param name="variantCodes">Danh sách mã biến thể</param>
    /// <param name="getProductName">Function để lấy tên sản phẩm</param>
    /// <param name="getProductCode">Function để lấy mã sản phẩm</param>
    /// <param name="getUnitName">Function để lấy tên đơn vị</param>
    /// <param name="getUnitCode">Function để lấy mã đơn vị</param>
    /// <returns>Danh sách ProductVariantDto</returns>
    public static List<ProductVariantDto> CreateBulk(
        Guid productId,
        Guid unitId,
        IEnumerable<string> variantCodes,
        Func<Guid, string> getProductName = null,
        Func<Guid, string> getProductCode = null,
        Func<Guid, string> getUnitName = null,
        Func<Guid, string> getUnitCode = null)
    {
        if (variantCodes == null) return new List<ProductVariantDto>();

        return Enumerable.ToList(Enumerable.Select(variantCodes, variantCode => CreateNew(
            productId,
            variantCode,
            unitId,
            getProductName,
            getProductCode,
            getUnitName,
            getUnitCode
        )));
    }

    /// <summary>
    /// Tạo ProductVariantDto từ ProductVariant entity với thông tin đầy đủ
    /// </summary>
    /// <param name="entity">ProductVariant entity</param>
    /// <returns>ProductVariantDto với thông tin đầy đủ</returns>
    public static ProductVariantDto ToFullDto(this ProductVariant entity)
    {
        if (entity == null) return null;

        var dto = new ProductVariantDto
        {
            Id = entity.Id,
            ProductId = entity.ProductId,
            VariantCode = entity.VariantCode,
            UnitId = entity.UnitId,
            IsActive = entity.IsActive,
            ThumbnailImage = entity.ThumbnailImage?.ToArray()
        };

        // Load product information
        if (entity.ProductService != null)
        {
            dto.ProductCode = entity.ProductService.Code;
            dto.ProductName = entity.ProductService.Name;
        }

        // Load unit information
        if (entity.UnitOfMeasure != null)
        {
            var unitDto = UnitOfMeasureConverters.ToDto(entity.UnitOfMeasure);
            dto.UnitCode = unitDto.Code;
            dto.UnitName = unitDto.Name;
        }

        // Convert attributes
        if (entity.VariantAttributes != null && Enumerable.Any(entity.VariantAttributes))
        {
            dto.Attributes = Enumerable.ToList(Enumerable.Select(entity.VariantAttributes, va => new ProductVariantAttributeDto
            {
                AttributeId = va.AttributeId,
                AttributeName = va.Attribute?.Name,
                AttributeValueId = va.AttributeValueId,
                AttributeValue = va.AttributeValue?.Value,
                Description = va.Attribute?.Description,
            }));
            dto.AttributeCount = dto.Attributes.Count;
        }

        // Convert images - ProductVariant không còn ProductImages navigation property
        // ProductImage không còn VariantId, chỉ có ProductId
        dto.Images = new List<ProductImageDto>();
        dto.ImageCount = 0;

        return dto;
    }

    #endregion
}
