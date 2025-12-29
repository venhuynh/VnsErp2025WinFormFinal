using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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
