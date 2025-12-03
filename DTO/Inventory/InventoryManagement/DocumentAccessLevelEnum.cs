using System.ComponentModel;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Enum định nghĩa mức độ truy cập chứng từ
/// </summary>
public enum DocumentAccessLevelEnum
{
    /// <summary>
    /// Công khai
    /// </summary>
    [Description("Công khai")]
    Public = 0,

    /// <summary>
    /// Nội bộ
    /// </summary>
    [Description("Nội bộ")]
    Internal = 1,

    /// <summary>
    /// Mật
    /// </summary>
    [Description("Mật")]
    Confidential = 2,

    /// <summary>
    /// Tuyệt mật
    /// </summary>
    [Description("Tuyệt mật")]
    Secret = 3
}

