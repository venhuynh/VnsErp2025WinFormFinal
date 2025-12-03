using System.ComponentModel;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Enum định nghĩa loại chứng từ
/// </summary>
public enum DocumentTypeEnum
{
    /// <summary>
    /// Hóa đơn
    /// </summary>
    [Description("Hóa đơn")]
    Invoice = 1,

    /// <summary>
    /// Phiếu nhập kho
    /// </summary>
    [Description("Phiếu nhập kho")]
    StockInVoucher = 2,

    /// <summary>
    /// Phiếu xuất kho
    /// </summary>
    [Description("Phiếu xuất kho")]
    StockOutVoucher = 3,

    /// <summary>
    /// Hợp đồng
    /// </summary>
    [Description("Hợp đồng")]
    Contract = 4,

    /// <summary>
    /// Biên bản
    /// </summary>
    [Description("Biên bản")]
    Minutes = 5,

    /// <summary>
    /// Báo cáo
    /// </summary>
    [Description("Báo cáo")]
    Report = 6,

    /// <summary>
    /// Chứng từ thanh toán
    /// </summary>
    [Description("Chứng từ thanh toán")]
    PaymentVoucher = 7,

    /// <summary>
    /// Giấy tờ pháp lý
    /// </summary>
    [Description("Giấy tờ pháp lý")]
    LegalDocument = 8,

    /// <summary>
    /// Chứng từ khác
    /// </summary>
    [Description("Chứng từ khác")]
    Other = 99
}

