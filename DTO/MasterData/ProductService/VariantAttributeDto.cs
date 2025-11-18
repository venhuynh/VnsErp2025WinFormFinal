using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.MasterData.ProductService;

/// <summary>
/// DTO đại diện cho bản ghi mapping giữa ProductVariant - Attribute - AttributeValue
/// Tương ứng bảng dbo.VariantAttribute (khóa chính tổng hợp VariantId + AttributeId + AttributeValueId)
/// </summary>
public class VariantAttributeDto
    {
        [DisplayName("ID Biến thể")]
        [Display(Order = -1)]
        [Required]
        public Guid VariantId { get; set; }

        [DisplayName("ID Thuộc tính")]
        [Display(Order = -1)]
        [Required]
        public Guid AttributeId { get; set; }

        [DisplayName("ID Giá trị thuộc tính")]
        [Display(Order = -1)]
        [Required]
        public Guid AttributeValueId { get; set; }

        // Thông tin hiển thị mở rộng, phục vụ UI
        [DisplayName("Tên thuộc tính")]
        public string AttributeName { get; set; }

        [DisplayName("Giá trị")]
        public string AttributeValue { get; set; }

        public VariantAttributeDto() {}

        public VariantAttributeDto(Guid variantId, Guid attributeId, Guid attributeValueId)
        {
            VariantId = variantId;
            AttributeId = attributeId;
            AttributeValueId = attributeValueId;
        }

        public string GetDisplay()
        {
            if (string.IsNullOrWhiteSpace(AttributeName)) return AttributeValue;
            return $"{AttributeName}: {AttributeValue}";
        }

        public override string ToString()
        {
            return GetDisplay();
        }

        public override bool Equals(object obj)
        {
            if (obj is VariantAttributeDto other)
            {
                return VariantId == other.VariantId && AttributeId == other.AttributeId && AttributeValueId == other.AttributeValueId;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + VariantId.GetHashCode();
                hash = hash * 23 + AttributeId.GetHashCode();
                hash = hash * 23 + AttributeValueId.GetHashCode();
                return hash;
            }
        }
    }