using System.ComponentModel;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Enum định nghĩa các loại tham chiếu trong bảng DeviceTransactionHistory
/// </summary>
public enum DeviceReferenceTypeEnum
{
    /// <summary>
    /// Tham chiếu đến StockInOutDetail (Phiếu nhập/xuất chi tiết)
    /// </summary>
    [Description("Phiếu nhập/xuất")]
    StockInOutDetail = 0,

    /// <summary>
    /// Tham chiếu đến DeviceTransfer (Chuyển giao thiết bị)
    /// </summary>
    [Description("Chuyển giao thiết bị")]
    DeviceTransfer = 1,

    /// <summary>
    /// Tham chiếu đến Warranty (Bảo hành)
    /// </summary>
    [Description("Bảo hành")]
    Warranty = 2,

    /// <summary>
    /// Khác (Other)
    /// </summary>
    [Description("Khác")]
    Other = 3
}
