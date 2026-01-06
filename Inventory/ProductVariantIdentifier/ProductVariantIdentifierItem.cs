using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DTO.Inventory.InventoryManagement;

namespace Inventory.ProductVariantIdentifier
{
    /// <summary>
    /// Class đơn giản để bind vào ProductVariantIdentifierGridView
    /// </summary>
    public class ProductVariantIdentifierItem
    {
        /// <summary>
        /// ID duy nhất của item
        /// </summary>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Loại định danh (ProductVariantIdentifierEnum)
        /// </summary>
        [DisplayName("Kiểu định danh")]
        [Required(ErrorMessage = "Vui lòng chọn kiểu định danh")]
        public ProductVariantIdentifierEnum IdentifierType { get; set; }

        /// <summary>
        /// Giá trị định danh
        /// </summary>
        [DisplayName("Giá trị")]
        [Required(ErrorMessage = "Vui lòng nhập giá trị định danh")]
        [StringLength(500, ErrorMessage = "Giá trị định danh không được vượt quá 500 ký tự")]
        public string Value { get; set; }
    }
}
