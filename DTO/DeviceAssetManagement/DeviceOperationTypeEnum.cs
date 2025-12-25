using System.ComponentModel;

namespace DTO.DeviceAssetManagement;

/// <summary>
/// Enum định nghĩa các loại thao tác với thiết bị trong bảng DeviceTransactionHistory
/// </summary>
public enum DeviceOperationTypeEnum
{
    /// <summary>
    /// Nhập kho (Import)
    /// </summary>
    [Description("Nhập kho")]
    Import = 0,

    /// <summary>
    /// Xuất kho (Export)
    /// </summary>
    [Description("Xuất kho")]
    Export = 1,

    /// <summary>
    /// Cấp phát (Allocation)
    /// </summary>
    [Description("Cấp phát")]
    Allocation = 2,

    /// <summary>
    /// Thu hồi (Recovery)
    /// </summary>
    [Description("Thu hồi")]
    Recovery = 3,

    /// <summary>
    /// Chuyển giao (Transfer)
    /// </summary>
    [Description("Chuyển giao")]
    Transfer = 4,

    /// <summary>
    /// Bảo trì (Maintenance)
    /// </summary>
    [Description("Bảo trì")]
    Maintenance = 5,

    /// <summary>
    /// Đổi trạng thái (Status Change)
    /// </summary>
    [Description("Đổi trạng thái")]
    StatusChange = 6,

    /// <summary>
    /// Khác (Other)
    /// </summary>
    [Description("Khác")]
    Other = 7
}
