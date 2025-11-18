using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockIn;

/// <summary>
/// Data Transfer Object cho chi tiết phiếu nhập kho
/// Dùng cho GridControl và truyền dữ liệu giữa Service ↔ WinForms
/// </summary>
public class StockInDetailDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của chi tiết phiếu nhập
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// ID phiếu nhập kho (Master)
    /// </summary>
    [DisplayName("ID Phiếu nhập")]
    [Display(Order = 0)]
    [Required(ErrorMessage = "ID phiếu nhập không được để trống")]
    public Guid StockInMasterId { get; set; }

    /// <summary>
    /// Thứ tự dòng
    /// </summary>
    [DisplayName("STT")]
    [Display(Order = 1)]
    public int LineNumber { get; set; }

    #endregion

    #region Properties - Thông tin hàng hóa

    /// <summary>
    /// ID hàng hóa (Item/Product)
    /// </summary>
    [DisplayName("Hàng hóa")]
    [Display(Order = 10)]
    [Required(ErrorMessage = "Hàng hóa không được để trống")]
    public Guid ItemId { get; set; }

    /// <summary>
    /// Mã hàng hóa
    /// </summary>
    [DisplayName("Mã hàng")]
    [Display(Order = 11)]
    public string ItemCode { get; set; }

    /// <summary>
    /// Tên hàng hóa
    /// </summary>
    [DisplayName("Tên hàng")]
    [Display(Order = 12)]
    public string ItemName { get; set; }

    /// <summary>
    /// ID đơn vị tính
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 13)]
    [Required(ErrorMessage = "Đơn vị tính không được để trống")]
    public Guid UnitOfMeasureId { get; set; }

    /// <summary>
    /// Mã đơn vị tính
    /// </summary>
    [DisplayName("Mã ĐVT")]
    [Display(Order = 14)]
    public string UnitOfMeasureCode { get; set; }

    /// <summary>
    /// Tên đơn vị tính
    /// </summary>
    [DisplayName("Đơn vị tính")]
    [Display(Order = 15)]
    public string UnitOfMeasureName { get; set; }

    #endregion

    #region Properties - Số lượng và giá

    /// <summary>
    /// Số lượng nhập (thực tế)
    /// </summary>
    [DisplayName("SL nhập")]
    [Display(Order = 22)]
    [Required(ErrorMessage = "Số lượng nhập không được để trống")]
    [Range(0.0001, double.MaxValue, ErrorMessage = "Số lượng nhập phải lớn hơn 0")]
    public decimal StockInQuantity { get; set; }

    /// <summary>
    /// Đơn giá nhập
    /// </summary>
    [DisplayName("Đơn giá")]
    [Display(Order = 23)]
    [Range(0, double.MaxValue, ErrorMessage = "Đơn giá phải lớn hơn hoặc bằng 0")]
    public decimal UnitPrice { get; set; }



    /// <summary>
    /// VAT
    /// </summary>
    [DisplayName("VAT")]
    [Display(Order = 23)]
    [Range(0, double.MaxValue, ErrorMessage = "VAT phải lớn hơn hoặc bằng 0")]
    public decimal Vat { get; set; }



    /// <summary>
    /// Số tiền VAT
    /// </summary>
    [DisplayName("Số tiền VAT")]
    [Display(Order = 23)]
    [Range(0, double.MaxValue, ErrorMessage = "Số tiền VAT phải lớn hơn hoặc bằng 0")]
    public decimal VatAmount { get; set; }


    /// <summary>
    /// Tổng tiền gồm VAT
    /// </summary>
    [DisplayName("Tổng tiền gồm VAT")]
    [Display(Order = 23)]
    [Range(0, double.MaxValue, ErrorMessage = "Tổng tiền gồm VAT phải lớn hơn hoặc bằng 0")]
    public decimal TotalAmountIncludedVat { get; set; }



    #endregion
}
