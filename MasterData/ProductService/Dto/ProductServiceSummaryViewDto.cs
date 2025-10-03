using System;
using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{
    /// <summary>
    /// DTO tóm tắt cho ProductService - Sử dụng cho danh sách, dropdown, combo
    /// </summary>
    public class ProductServiceSummaryViewDto
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public string Code { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Danh mục")]
        public string CategoryName { get; set; }

        [Display(Name = "Là dịch vụ")]
        public bool IsService { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Display(Name = "Số biến thể")]
        public int VariantCount { get; set; }

        [Display(Name = "Hiển thị")]
        public string DisplayText => $"{Code} - {Name}";
    }

    /// <summary>
    /// DTO tóm tắt cho ProductVariant - Sử dụng cho danh sách, dropdown, combo
    /// </summary>
    public class ProductVariantSummaryViewDto
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }

        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Mã biến thể")]
        public string VariantCode { get; set; }

        [Display(Name = "Đơn vị")]
        public string UnitName { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        [Display(Name = "Hiển thị")]
        public string DisplayText => $"{ProductCode}-{VariantCode} - {ProductName} ({UnitName})";
    }

    /// <summary>
    /// DTO cho tìm kiếm và lọc ProductService
    /// </summary>
    public class ProductServiceSearchViewDto
    {
        [Display(Name = "Mã sản phẩm")]
        public string Code { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string Name { get; set; }

        [Display(Name = "Danh mục")]
        public Guid? CategoryId { get; set; }

        [Display(Name = "Là dịch vụ")]
        public bool? IsService { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? IsActive { get; set; }

        [Display(Name = "Có biến thể")]
        public bool? HasVariants { get; set; }
    }

    /// <summary>
    /// DTO cho tìm kiếm và lọc ProductVariant
    /// </summary>
    public class ProductVariantSearchViewDto
    {
        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; }

        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        [Display(Name = "Mã biến thể")]
        public string VariantCode { get; set; }

        [Display(Name = "Đơn vị")]
        public Guid? UnitId { get; set; }

        [Display(Name = "Trạng thái")]
        public bool? IsActive { get; set; }

        [Display(Name = "Có thuộc tính")]
        public bool? HasAttributes { get; set; }
    }
}
