using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{
    /// <summary>
    /// DTO cho thông tin hình ảnh sản phẩm
    /// </summary>
    public class ProductImageDto
    {

        /// <summary>
        /// Mã sản phẩm
        /// </summary>
        [Display(Name = "Mã sản phẩm")]
        public string ProductCode { get; set; }

        /// <summary>
        /// Tên sản phẩm
        /// </summary>
        [Display(Name = "Tên sản phẩm")]
        public string ProductName { get; set; }

        /// <summary>
        /// Mã biến thể sản phẩm
        /// </summary>
        [Display(Name = "Mã biến thể")]
        public string VariantCode { get; set; }

        /// <summary>
        /// Tên biến thể sản phẩm đầy đủ
        /// </summary>
        [Display(Name = "Tên biến thể")]
        public string VariantFullName { get; set; }

        /// <summary>
        /// Tên đơn vị tính
        /// </summary>
        [Display(Name = "Đơn vị tính")]
        public string UnitName { get; set; }

        /// <summary>
        /// Tên file hình ảnh
        /// </summary>
        [Display(Name = "Tên file")]
        public string FileName { get; set; }

        /// <summary>
        /// Kích thước file dạng hiển thị (KB, MB)
        /// </summary>
        [Display(Name = "Kích thước")]
        public string FormattedImageSize { get; set; }

        /// <summary>
        /// Kích thước hình ảnh dạng hiển thị (WxH)
        /// </summary>
        [Display(Name = "Kích thước hình")]
        public string FormattedImageDimensions { get; set; }

        /// <summary>
        /// Đường dẫn đầy đủ của hình ảnh
        /// </summary>
        public string FullImagePath { get; set; }

        /// <summary>
        /// Tên hiển thị đầy đủ cho sản phẩm/biến thể
        /// </summary>
        [Display(Name = "Sản phẩm")]
        public string ProductDisplayName { get; set; }

        /// <summary>
        /// Tên hiển thị cho hình ảnh
        /// </summary>
        [Display(Name = "Hình ảnh")]
        public string ImageDisplayName { get; set; }

        /// <summary>
        /// Trạng thái hiển thị
        /// </summary>
        [Display(Name = "Trạng thái")]
        public string StatusDisplay { get; set; }
    }
}