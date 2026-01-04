using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// Data Transfer Object cho lịch sử sản phẩm nhập xuất kho (StockInOutDetail)
/// Dùng để hiển thị thông tin lịch sử các sản phẩm/dịch vụ trong phiếu nhập xuất kho
/// </summary>
public class StockInOutProductHistoryDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của chi tiết phiếu nhập xuất kho
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID phiếu nhập xuất kho (Master)
    /// </summary>
    [DisplayName("ID Phiếu")]
    [Display(Order = 1)]
    public Guid StockInOutMasterId { get; set; }

    /// <summary>
    /// Số phiếu nhập xuất kho (từ Master)
    /// </summary>
    [DisplayName("Số phiếu")]
    [Display(Order = 2)]
    public string VocherNumber { get; set; }

    /// <summary>
    /// Ngày nhập xuất kho (từ Master)
    /// </summary>
    [DisplayName("Ngày nhập xuất")]
    [Display(Order = 3)]
    public DateTime StockInOutDate { get; set; }

    /// <summary>
    /// Loại nhập xuất kho (Enum)
    /// </summary>
    [DisplayName("Loại nhập xuất-Enum")]
    [Display(Order = 4)]
    public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

    /// <summary>
    /// Tên loại nhập xuất kho (hiển thị)
    /// </summary>
    [DisplayName("Loại nhập xuất")]
    [Display(Order = 5)]
    public string LoaiNhapXuatKhoName { get; set; }

    /// <summary>
    /// ID biến thể sản phẩm
    /// </summary>
    [DisplayName("ID Biến thể")]
    [Display(Order = 6)]
    public Guid ProductVariantId { get; set; }

    #endregion

    #region Properties - Thông tin sản phẩm

    /// <summary>
    /// Mã sản phẩm/dịch vụ (từ ProductService)
    /// </summary>
    [DisplayName("Mã sản phẩm")]
    [Display(Order = 10)]
    public string ProductCode { get; set; }

    /// <summary>
    /// Tên sản phẩm/dịch vụ (từ ProductService)
    /// </summary>
    [DisplayName("Tên sản phẩm")]
    [Display(Order = 11)]
    public string ProductName { get; set; }

    /// <summary>
    /// Mã biến thể sản phẩm
    /// </summary>
    [DisplayName("Mã biến thể")]
    [Display(Order = 12)]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên biến thể sản phẩm đầy đủ
    /// </summary>
    [DisplayName("Tên biến thể")]
    [Display(Order = 13)]
    public string ProductVariantFullName { get; set; }

    /// <summary>
    /// ID đơn vị tính
    /// </summary>
    [DisplayName("ID ĐVT")]
    [Display(Order = 15)]
    public Guid? UnitOfMeasureId { get; set; }

    /// <summary>
    /// Mã đơn vị tính
    /// </summary>
    [DisplayName("Mã ĐVT")]
    [Display(Order = 16)]
    public string UnitOfMeasureCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 17)]
    public string UnitOfMeasureName { get; set; }

    #endregion

    #region Properties - Số lượng và giá

    /// <summary>
    /// Số lượng nhập
    /// </summary>
    [DisplayName("SL nhập")]
    [Display(Order = 20)]
    public decimal StockInQty { get; set; }

    /// <summary>
    /// Số lượng xuất
    /// </summary>
    [DisplayName("SL xuất")]
    [Display(Order = 21)]
    public decimal StockOutQty { get; set; }

    /// <summary>
    /// Đơn giá
    /// </summary>
    [DisplayName("Đơn giá")]
    [Display(Order = 22)]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Thuế suất VAT (%)
    /// </summary>
    [DisplayName("VAT (%)")]
    [Display(Order = 23)]
    public decimal Vat { get; set; }

    /// <summary>
    /// Số tiền VAT
    /// </summary>
    [DisplayName("Tiền VAT")]
    [Display(Order = 24)]
    public decimal VatAmount { get; set; }

    /// <summary>
    /// Thành tiền (chưa VAT)
    /// </summary>
    [DisplayName("Thành tiền")]
    [Display(Order = 25)]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Tổng tiền bao gồm VAT
    /// </summary>
    [DisplayName("Tổng tiền gồm VAT")]
    [Display(Order = 26)]
    public decimal TotalAmountIncludedVat { get; set; }

    #endregion

    #region Properties - Thông tin liên kết

    /// <summary>
    /// ID kho (từ Master)
    /// </summary>
    [DisplayName("ID Kho")]
    [Display(Order = 30)]
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Tên kho nhập xuất (từ Master)
    /// </summary>
    [DisplayName("Kho nhập xuất")]
    [Display(Order = 31)]
    public string WarehouseName { get; set; }

    /// <summary>
    /// Thông tin kho nhập xuất dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// Format tương tự CompanyBranchDto.ThongTinHtml để đảm bảo consistency
    /// </summary>
    [DisplayName("Thông tin kho HTML")]
    [Display(Order = 32)]
    [Description("Thông tin kho nhập xuất dưới dạng HTML")]
    public string WarehouseNameHtml
    {
        get
        {
            var warehouseName = WarehouseName ?? string.Empty;

            if (string.IsNullOrWhiteSpace(warehouseName))
                return string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng (tham khảo CompanyBranchDto.ThongTinHtml)
            // - Tên kho: font lớn, bold, màu xanh đậm (primary)
            // - Loại nhập xuất: font nhỏ hơn, màu xám, không bold (secondary)
            var html = $"<b><color='blue'>{warehouseName}</color></b>";

            // Thêm loại nhập xuất nếu có
            var loaiNhapXuatKhoName = LoaiNhapXuatKhoName ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(loaiNhapXuatKhoName))
            {
                html += "<br>";
                html += $"<color='#757575'>Loại:</color> <color='#757575'><i>{loaiNhapXuatKhoName}</i></color>";
                html += "<br>";
                html += $"<color='#757575'>Số phiếu:</color> <color='#757575'><i>{VocherNumber}</i></color>";
            }

            return html;
        }
    }

    /// <summary>
    /// Tên khách hàng (từ Master)
    /// </summary>
    [DisplayName("Tên khách hàng")]
    [Display(Order = 33)]
    public string CustomerName { get; set; }

    /// <summary>
    /// Thông tin khách hàng dưới dạng HTML theo format DevExpress
    /// Sử dụng các tag HTML chuẩn của DevExpress: &lt;b&gt;, &lt;i&gt;, &lt;color&gt;
    /// Tham khảo: https://docs.devexpress.com/WindowsForms/4874/common-features/html-text-formatting
    /// Format tương tự BusinessPartnerListDto.ThongTinHtml để đảm bảo consistency
    /// </summary>
    [DisplayName("Thông tin khách hàng HTML")]
    [Display(Order = 34)]
    [Description("Thông tin khách hàng dưới dạng HTML")]
    public string CustomerNameHtml
    {
        get
        {
            var customerName = CustomerName ?? string.Empty;

            if (string.IsNullOrWhiteSpace(customerName))
                return string.Empty;

            // Format chuyên nghiệp với visual hierarchy rõ ràng (tham khảo BusinessPartnerListDto.ThongTinHtml)
            // - Tên khách hàng: font lớn, bold, màu xanh đậm (primary)
            var html = $"<b><color='blue'>{customerName}</color></b>";

            return html;
        }
    }

    #endregion

}
