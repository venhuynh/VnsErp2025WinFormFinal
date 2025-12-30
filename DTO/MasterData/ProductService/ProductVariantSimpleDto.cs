using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.MasterData.ProductService
{
    public class ProductVariantSimpleDto
    {
        [Display(Name = "ID")]
        public Guid Id { get; set; }
        
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }


        [Display(Name = "Tên biến thể đầy đủ (HTML)")]
        public string VariantFullName { get; set; }

        [Display(Name = "Tên biến thể đầy đủ (Không HTML)")]
        public string VariantFullNamePlain { get; set; }
        
        [Display(Name = "Đơn vị tính")]
        public string UnitName { get; set; }
        
        [Display(Name = "Hình ảnh thumbnail")]
        [Description("Hình ảnh thumbnail của biến thể sản phẩm")]
        public byte[] ThumbnailImage { get; set; }

        [Display(Name = "Còn hoạt động")]
        public bool IsActive { get; set; }
    }
}
