using System;
using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{
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

        [Display(Name = "Tên đầy đủ")]
        public string FullName => $"{VariantCode} - {ProductName} {VariantFullName} ({UnitName})";

        [Display(Name = "Hiển thị")]
        public string DisplayText => $"{ProductCode}-{VariantCode} - {ProductName} ({UnitName})";
    }
    
}
