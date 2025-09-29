using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{

    /// <summary>
    /// DTO cho thuộc tính biến thể
    /// </summary>
    public class ProductVariantAttributeDto
    {
        /// <summary>
        /// ID của thuộc tính
        /// </summary>
        [DisplayName("ID Thuộc tính")]
        public Guid AttributeId { get; set; }

        /// <summary>
        /// Tên thuộc tính
        /// </summary>
        [DisplayName("Tên thuộc tính")]
        [Required(ErrorMessage = "Tên thuộc tính không được để trống")]
        [StringLength(100, ErrorMessage = "Tên thuộc tính không được vượt quá 100 ký tự")]
        public string AttributeName { get; set; }

        /// <summary>
        /// ID của giá trị thuộc tính
        /// </summary>
        [DisplayName("ID Giá trị")]
        [Required(ErrorMessage = "Giá trị thuộc tính không được để trống")]
        public Guid AttributeValueId { get; set; }

        /// <summary>
        /// Giá trị thuộc tính
        /// </summary>
        [DisplayName("Giá trị")]
        [Required(ErrorMessage = "Giá trị thuộc tính không được để trống")]
        [StringLength(255, ErrorMessage = "Giá trị thuộc tính không được vượt quá 255 ký tự")]
        public string AttributeValue { get; set; }

        /// <summary>
        /// Mô tả thuộc tính
        /// </summary>
        [DisplayName("Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string Description { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        [DisplayName("Thứ tự")]
        [Description("Thứ tự hiển thị của thuộc tính")]
        [Range(0, int.MaxValue, ErrorMessage = "Thứ tự không được âm")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Tạo bản sao
        /// </summary>
        /// <returns>Bản sao mới</returns>
        public ProductVariantAttributeDto Clone()
        {
            return new ProductVariantAttributeDto
            {
                AttributeId = this.AttributeId,
                AttributeName = this.AttributeName,
                AttributeValueId = this.AttributeValueId,
                AttributeValue = this.AttributeValue,
                Description = this.Description,
                SortOrder = this.SortOrder
            };
        }
    }
}
