using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MasterData.ProductService.Dto
{
    /// <summary>
    /// DTO cho thuộc tính biến thể sản phẩm.
    /// Thể hiện đầy đủ các thuộc tính của Entity VariantAttribute.
    /// </summary>
    public class ProductVariantAttributeDto
    {
        /// <summary>
        /// ID của biến thể sản phẩm
        /// </summary>
        [DisplayName("ID Biến thể")]
        public Guid VariantId { get; set; }

        /// <summary>
        /// ID của thuộc tính
        /// </summary>
        [DisplayName("ID Thuộc tính")]
        public Guid AttributeId { get; set; }

        /// <summary>
        /// ID của giá trị thuộc tính
        /// </summary>
        [DisplayName("ID Giá trị")]
        public Guid AttributeValueId { get; set; }

        /// <summary>
        /// Tên thuộc tính (từ bảng Attribute)
        /// </summary>
        [DisplayName("Tên thuộc tính")]
        [Required(ErrorMessage = "Tên thuộc tính không được để trống")]
        [StringLength(100, ErrorMessage = "Tên thuộc tính không được vượt quá 100 ký tự")]
        public string AttributeName { get; set; }

        /// <summary>
        /// Kiểu dữ liệu của thuộc tính (từ bảng Attribute)
        /// </summary>
        [DisplayName("Kiểu dữ liệu")]
        [StringLength(50, ErrorMessage = "Kiểu dữ liệu không được vượt quá 50 ký tự")]
        public string AttributeDataType { get; set; }

        /// <summary>
        /// Giá trị thuộc tính (từ bảng AttributeValue)
        /// </summary>
        [DisplayName("Giá trị")]
        [Required(ErrorMessage = "Giá trị thuộc tính không được để trống")]
        [StringLength(255, ErrorMessage = "Giá trị thuộc tính không được vượt quá 255 ký tự")]
        public string AttributeValue { get; set; }

        /// <summary>
        /// Mô tả thuộc tính (từ bảng Attribute)
        /// </summary>
        [DisplayName("Mô tả")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự")]
        public string Description { get; set; }

        #region Helper Methods

        /// <summary>
        /// Tạo bản sao
        /// </summary>
        /// <returns>Bản sao mới</returns>
        public ProductVariantAttributeDto Clone()
        {
            return new ProductVariantAttributeDto
            {
                VariantId = this.VariantId,
                AttributeId = this.AttributeId,
                AttributeValueId = this.AttributeValueId,
                AttributeName = this.AttributeName,
                AttributeDataType = this.AttributeDataType,
                AttributeValue = this.AttributeValue,
                Description = this.Description
            };
        }

        /// <summary>
        /// Cập nhật từ DTO khác
        /// </summary>
        /// <param name="source">DTO nguồn</param>
        public void UpdateFrom(ProductVariantAttributeDto source)
        {
            if (source == null) return;

            VariantId = source.VariantId;
            AttributeId = source.AttributeId;
            AttributeValueId = source.AttributeValueId;
            AttributeName = source.AttributeName;
            AttributeDataType = source.AttributeDataType;
            AttributeValue = source.AttributeValue;
            Description = source.Description;
        }

        /// <summary>
        /// Reset về giá trị mặc định
        /// </summary>
        public void Reset()
        {
            VariantId = Guid.Empty;
            AttributeId = Guid.Empty;
            AttributeValueId = Guid.Empty;
            AttributeName = string.Empty;
            AttributeDataType = string.Empty;
            AttributeValue = string.Empty;
            Description = string.Empty;
        }

        /// <summary>
        /// Kiểm tra tính hợp lệ của DTO
        /// </summary>
        /// <returns>True nếu hợp lệ</returns>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(AttributeName) &&
                   AttributeId != Guid.Empty &&
                   AttributeValueId != Guid.Empty &&
                   !string.IsNullOrWhiteSpace(AttributeValue);
        }

        /// <summary>
        /// ToString override để hiển thị thông tin
        /// </summary>
        /// <returns>Chuỗi hiển thị</returns>
        public override string ToString()
        {
            return $"{AttributeName}: {AttributeValue}";
        }

        #endregion
    }
}
