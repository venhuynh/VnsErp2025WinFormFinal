using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DTO.Inventory.StockIn;

namespace Inventory.StockIn.Dto;

/// <summary>
/// Data Transfer Object cho danh sách phiếu nhập kho
/// Dùng cho GridControl (danh sách)
/// </summary>
public class StockInListDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của phiếu nhập kho
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// Số phiếu nhập kho
    /// </summary>
    [DisplayName("Số phiếu")]
    [Display(Order = 1)]
    public string StockInNumber { get; set; }

    /// <summary>
    /// Ngày nhập kho
    /// </summary>
    [DisplayName("Ngày nhập")]
    [Display(Order = 2)]
    public DateTime StockInDate { get; set; }

    /// <summary>
    /// Loại nhập kho
    /// </summary>
    [DisplayName("Loại nhập")]
    [Display(Order = 3)]
    public LoaiNhapKhoEnum LoaiNhapKho { get; set; }

    /// <summary>
    /// Tên loại nhập kho (hiển thị)
    /// </summary>
    [DisplayName("Loại nhập")]
    [Display(Order = 4)]
    public string LoaiNhapKhoName { get; set; }

    /// <summary>
    /// Phương thức nhập kho
    /// </summary>
    [DisplayName("Phương thức")]
    [Display(Order = 5)]
    public PhuongThucNhapKhoEnum PhuongThucNhapKho { get; set; }

    /// <summary>
    /// Tên phương thức nhập kho (hiển thị)
    /// </summary>
    [DisplayName("Phương thức")]
    [Display(Order = 6)]
    public string PhuongThucNhapKhoName { get; set; }

    /// <summary>
    /// Trạng thái phiếu nhập
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 7)]
    public TrangThaiPhieuNhapEnum TrangThai { get; set; }

    /// <summary>
    /// Tên trạng thái (hiển thị)
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 8)]
    public string TrangThaiName { get; set; }

    #endregion

    #region Properties - Thông tin liên kết

    /// <summary>
    /// Mã kho nhập hàng
    /// </summary>
    [DisplayName("Mã kho")]
    [Display(Order = 10)]
    public string WarehouseCode { get; set; }

    /// <summary>
    /// Tên kho nhập hàng
    /// </summary>
    [DisplayName("Tên kho")]
    [Display(Order = 11)]
    public string WarehouseName { get; set; }

    /// <summary>
    /// Số đơn mua hàng (PO)
    /// </summary>
    [DisplayName("Số PO")]
    [Display(Order = 12)]
    public string PurchaseOrderNumber { get; set; }

    /// <summary>
    /// Số đơn đặt hàng khách
    /// </summary>
    [DisplayName("Số đơn khách")]
    [Display(Order = 13)]
    public string SalesOrderNumber { get; set; }

    /// <summary>
    /// Mã nhà cung cấp
    /// </summary>
    [DisplayName("Mã NCC")]
    [Display(Order = 14)]
    public string SupplierCode { get; set; }

    /// <summary>
    /// Tên nhà cung cấp
    /// </summary>
    [DisplayName("Tên NCC")]
    [Display(Order = 15)]
    public string SupplierName { get; set; }

    #endregion

    #region Properties - Tổng hợp

    /// <summary>
    /// Tổng số lượng nhập
    /// </summary>
    [DisplayName("Tổng SL")]
    [Display(Order = 20)]
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// Tổng giá trị nhập
    /// </summary>
    [DisplayName("Tổng giá trị")]
    [Display(Order = 21)]
    public decimal TotalAmount { get; set; }

    #endregion

    #region Properties - Thông tin hệ thống

    /// <summary>
    /// Tên người tạo
    /// </summary>
    [DisplayName("Người tạo")]
    [Display(Order = 30)]
    public string CreatedByName { get; set; }

    /// <summary>
    /// Ngày tạo
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 31)]
    public DateTime? CreatedDate { get; set; }

    #endregion
}