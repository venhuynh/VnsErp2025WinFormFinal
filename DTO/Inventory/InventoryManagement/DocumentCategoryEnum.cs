using System.ComponentModel;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Enum định nghĩa danh mục chứng từ
/// </summary>
public enum DocumentCategoryEnum
{
    /// <summary>
    /// Tài chính
    /// </summary>
    [Description("Tài chính")]
    Financial = 1,

    /// <summary>
    /// Pháp lý
    /// </summary>
    [Description("Pháp lý")]
    Legal = 2,

    /// <summary>
    /// Hành chính
    /// </summary>
    [Description("Hành chính")]
    Administrative = 3,

    /// <summary>
    /// Kho hàng
    /// </summary>
    [Description("Kho hàng")]
    Inventory = 4,

    /// <summary>
    /// Mua hàng
    /// </summary>
    [Description("Mua hàng")]
    Procurement = 5,

    /// <summary>
    /// Bán hàng
    /// </summary>
    [Description("Bán hàng")]
    Sales = 6,

    /// <summary>
    /// Khác
    /// </summary>
    [Description("Khác")]
    Other = 99
}

