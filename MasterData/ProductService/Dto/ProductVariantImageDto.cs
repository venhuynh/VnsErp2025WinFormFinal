using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{

    /// <summary>
    /// DTO cho hình ảnh biến thể
    /// </summary>
    public class ProductVariantImageDto
    {
        /// <summary>
        /// ID của hình ảnh
        /// </summary>
        [DisplayName("ID Hình ảnh")]
        public Guid ImageId { get; set; }

        /// <summary>
        /// Đường dẫn hình ảnh
        /// </summary>
        [DisplayName("Đường dẫn")]
        [StringLength(500, ErrorMessage = "Đường dẫn hình ảnh không được vượt quá 500 ký tự")]
        public string ImagePath { get; set; }

        /// <summary>
        /// Dữ liệu hình ảnh (byte array)
        /// </summary>
        [DisplayName("Dữ liệu hình ảnh")]
        [Description("Dữ liệu hình ảnh dạng byte array")]
        public byte[] ImageData { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        [DisplayName("Thứ tự")]
        [Description("Thứ tự hiển thị của hình ảnh")]
        [Range(0, int.MaxValue, ErrorMessage = "Thứ tự không được âm")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Có phải ảnh chính không
        /// </summary>
        [DisplayName("Ảnh chính")]
        [Description("Có phải ảnh chính của biến thể không")]
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Loại hình ảnh
        /// </summary>
        [DisplayName("Loại hình ảnh")]
        [StringLength(10, ErrorMessage = "Loại hình ảnh không được vượt quá 10 ký tự")]
        public string ImageType { get; set; }

        /// <summary>
        /// Kích thước hình ảnh (bytes)
        /// </summary>
        [DisplayName("Kích thước")]
        [Description("Kích thước hình ảnh tính bằng bytes")]
        [Range(0, long.MaxValue, ErrorMessage = "Kích thước không được âm")]
        public long? ImageSize { get; set; }

        /// <summary>
        /// Chiều rộng hình ảnh (pixels)
        /// </summary>
        [DisplayName("Chiều rộng")]
        [Description("Chiều rộng hình ảnh tính bằng pixels")]
        [Range(0, int.MaxValue, ErrorMessage = "Chiều rộng không được âm")]
        public int? ImageWidth { get; set; }

        /// <summary>
        /// Chiều cao hình ảnh (pixels)
        /// </summary>
        [DisplayName("Chiều cao")]
        [Description("Chiều cao hình ảnh tính bằng pixels")]
        [Range(0, int.MaxValue, ErrorMessage = "Chiều cao không được âm")]
        public int? ImageHeight { get; set; }

        /// <summary>
        /// Chú thích hình ảnh
        /// </summary>
        [DisplayName("Chú thích")]
        [StringLength(255, ErrorMessage = "Chú thích không được vượt quá 255 ký tự")]
        public string Caption { get; set; }

        /// <summary>
        /// Text thay thế
        /// </summary>
        [DisplayName("Text thay thế")]
        [StringLength(255, ErrorMessage = "Text thay thế không được vượt quá 255 ký tự")]
        public string AltText { get; set; }

        /// <summary>
        /// Trạng thái hoạt động
        /// </summary>
        [DisplayName("Trạng thái")]
        [Description("Trạng thái hoạt động của hình ảnh")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Tạo bản sao
        /// </summary>
        /// <returns>Bản sao mới</returns>
        public ProductVariantImageDto Clone()
        {
            return new ProductVariantImageDto
            {
                ImageId = ImageId,
                ImagePath = ImagePath,
                ImageData = ImageData?.Clone() as byte[],
                SortOrder = SortOrder,
                IsPrimary = IsPrimary,
                ImageType = ImageType,
                ImageSize = ImageSize,
                ImageWidth = ImageWidth,
                ImageHeight = ImageHeight,
                Caption = Caption,
                AltText = AltText,
                IsActive = IsActive
            };
        }
    }
}


