using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockIn;

/// <summary>
/// Data Transfer Object cho chi tiết phiếu nhập kho
/// Dùng cho GridControl và truyền dữ liệu giữa Service ↔ WinForms
/// Map với bảng StockInOutDetail trong database
/// </summary>
public class StockInDetailDto
{
    #region Properties - Thông tin cơ bản (map với DB)

    /// <summary>
    /// ID duy nhất của chi tiết phiếu nhập
    /// Map với: StockInOutDetail.Id
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID phiếu nhập kho (Master)
    /// Map với: StockInOutDetail.StockInOutMasterId
    /// </summary>
    [DisplayName("ID Phiếu nhập")]
    [Display(Order = 0)]
    [Required(ErrorMessage = "ID phiếu nhập không được để trống")]
    public Guid StockInOutMasterId { get; set; }

    /// <summary>
    /// ID biến thể sản phẩm (ProductVariant)
    /// Map với: StockInOutDetail.ProductVariantId
    /// </summary>
    [DisplayName("ID Biến thể")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Biến thể sản phẩm không được để trống")]
    public Guid ProductVariantId { get; set; }

    /// <summary>
    /// Thứ tự dòng (dùng cho UI, không có trong DB)
    /// </summary>
    [DisplayName("STT")]
    [Display(Order = 2)]
    public int LineNumber { get; set; }

    #endregion

    #region Properties - Thông tin hàng hóa (hiển thị, lấy từ ProductVariant)

    /// <summary>
    /// Mã biến thể sản phẩm (để hiển thị)
    /// Lấy từ ProductVariant.VariantCode
    /// </summary>
    [DisplayName("Mã hàng")]
    [Display(Order = 10)]
    public string ProductVariantCode { get; set; }

    /// <summary>
    /// Tên biến thể sản phẩm (để hiển thị)
    /// Lấy từ ProductVariant.VariantFullName hoặc ProductService.Name
    /// </summary>
    [DisplayName("Tên hàng")]
    [Display(Order = 11)]
    public string ProductVariantName { get; set; }

    /// <summary>
    /// ID đơn vị tính (để hiển thị)
    /// Lấy từ ProductVariant.UnitId
    /// </summary>
    [DisplayName("ID ĐVT")]
    [Display(Order = 12)]
    public Guid? UnitOfMeasureId { get; set; }

    /// <summary>
    /// Mã đơn vị tính (để hiển thị)
    /// Lấy từ UnitOfMeasure.Code
    /// </summary>
    [DisplayName("Mã ĐVT")]
    [Display(Order = 13)]
    public string UnitOfMeasureCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính (để hiển thị)
    /// Lấy từ UnitOfMeasure.Name
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 14)]
    public string UnitOfMeasureName { get; set; }

    #endregion

    #region Properties - Số lượng và giá (map với DB)

    /// <summary>
    /// Số lượng nhập
    /// Map với: StockInOutDetail.StockInQty
    /// </summary>
    [DisplayName("SL nhập")]
    [Display(Order = 20)]
    [Required(ErrorMessage = "Số lượng nhập không được để trống")]
    [Range(0, double.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn hoặc bằng 0")]
    public decimal StockInQty { get; set; }

    /// <summary>
    /// Số lượng xuất (dùng cho phiếu xuất kho, mặc định = 0 cho phiếu nhập)
    /// Map với: StockInOutDetail.StockOutQty
    /// </summary>
    [DisplayName("SL xuất")]
    [Display(Order = 21)]
    [Range(0, double.MaxValue, ErrorMessage = "Số lượng xuất phải lớn hơn hoặc bằng 0")]
    public decimal StockOutQty { get; set; }

    /// <summary>
    /// Đơn giá
    /// Map với: StockInOutDetail.UnitPrice
    /// </summary>
    [DisplayName("Đơn giá")]
    [Display(Order = 22)]
    [Required(ErrorMessage = "Đơn giá không được để trống")]
    [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// Tỷ lệ VAT (%)
    /// Map với: StockInOutDetail.Vat
    /// </summary>
    [DisplayName("VAT (%)")]
    [Display(Order = 23)]
    [Range(0, 100, ErrorMessage = "VAT phải từ 0 đến 100")]
    public decimal Vat { get; set; }

    /// <summary>
    /// Số tiền VAT
    /// Map với: StockInOutDetail.VatAmount
    /// </summary>
    [DisplayName("Số tiền VAT")]
    [Display(Order = 24)]
    [Range(0, double.MaxValue, ErrorMessage = "Số tiền VAT phải lớn hơn hoặc bằng 0")]
    public decimal VatAmount { get; set; }

    /// <summary>
    /// Tổng tiền (chưa VAT)
    /// Map với: StockInOutDetail.TotalAmount
    /// </summary>
    [DisplayName("Tổng tiền")]
    [Display(Order = 25)]
    [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền phải lớn hơn hoặc bằng 0")]
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// Tổng tiền bao gồm VAT
    /// Map với: StockInOutDetail.TotalAmountIncludedVat
    /// </summary>
    [DisplayName("Tổng tiền gồm VAT")]
    [Display(Order = 26)]
    [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền gồm VAT phải lớn hơn hoặc bằng 0")]
    public decimal TotalAmountIncludedVat { get; set; }

    #endregion
}
