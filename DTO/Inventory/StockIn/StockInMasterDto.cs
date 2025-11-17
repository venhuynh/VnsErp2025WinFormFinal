using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace DTO.Inventory.StockIn;

/// <summary>
/// Data Transfer Object cho phiếu nhập kho (Master)
/// Dùng cho DataLayoutControl và truyền dữ liệu giữa Service ↔ WinForms
/// </summary>
public class StockInMasterDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của phiếu nhập kho
    /// </summary>
    [DisplayName("ID")]
    public Guid Id { get; set; }

    /// <summary>
    /// Số phiếu nhập kho (GR Number - Goods Receipt)
    /// </summary>
    [DisplayName("Số phiếu nhập")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Số phiếu nhập không được để trống")]
    [StringLength(50, ErrorMessage = "Số phiếu nhập không được vượt quá 50 ký tự")]
    public string StockInNumber { get; set; }

    /// <summary>
    /// Ngày nhập kho
    /// </summary>
    [DisplayName("Ngày nhập kho")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Ngày nhập kho không được để trống")]
    public DateTime StockInDate { get; set; }

    /// <summary>
    /// Loại nhập kho
    /// </summary>
    [DisplayName("Loại nhập kho")]
    [Display(Order = 3)]
    [Required(ErrorMessage = "Loại nhập kho không được để trống")]
    public LoaiNhapKhoEnum LoaiNhapKho { get; set; }

    /// <summary>
    /// Tên loại nhập kho (hiển thị)
    /// </summary>
    [DisplayName("Loại nhập kho")]
    [Display(Order = 4)]
    public string LoaiNhapKhoName { get; set; }

    /// <summary>
    /// Trạng thái phiếu nhập
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 7)]
    [Required(ErrorMessage = "Trạng thái không được để trống")]
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
    /// ID kho nhập hàng
    /// </summary>
    [DisplayName("Kho nhập")]
    [Display(Order = 10)]
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Mã kho nhập hàng
    /// </summary>
    [DisplayName("Mã kho")]
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    [Display(Order = 11)]
    public string WarehouseCode { get; set; }

    /// <summary>
    /// Tên kho nhập hàng
    /// </summary>
    [DisplayName("Tên kho")]
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    [Display(Order = 12)]
    public string WarehouseName { get; set; }

    /// <summary>
    /// ID đơn mua hàng (Purchase Order - PO) - Tùy chọn
    /// </summary>
    [DisplayName("Đơn mua hàng (PO)")]
    [Display(Order = 13)]
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>
    /// Số đơn mua hàng
    /// </summary>
    [DisplayName("Số PO")]
    [Display(Order = 14)]
    public string PurchaseOrderNumber { get; set; }


    /// <summary>
    /// ID nhà cung cấp (PartnerSiteId trong database)
    /// </summary>
    [DisplayName("Nhà cung cấp")]
    [Display(Order = 17)]
    [Required(ErrorMessage = "Nhà cung cấp không được để trống")]
    public Guid SupplierId { get; set; }

    /// <summary>
    /// Tên nhà cung cấp
    /// </summary>
    [DisplayName("Tên NCC")]
    [Required(ErrorMessage = "Tên NCC không được để trống")]
    [Display(Order = 19)]
    public string SupplierName { get; set; }

    #endregion

    #region Properties - Thông tin bổ sung

    /// <summary>
    /// Ghi chú
    /// </summary>
    [DisplayName("Ghi chú")]
    [Display(Order = 20)]
    [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự")]
    public string Notes { get; set; }

    /// <summary>
    /// Tổng số lượng nhập
    /// </summary>
    [DisplayName("Tổng số lượng")]
    [Display(Order = 23)]
    public decimal TotalQuantity { get; set; }

    /// <summary>
    /// Tổng giá trị nhập
    /// </summary>
    [DisplayName("Tổng giá trị")]
    [Display(Order = 24)]
    public decimal TotalAmount { get; set; }


    /// <summary>
    /// Tổng thuế VAT
    /// </summary>
    [DisplayName("Tổng thuế VAT")]
    [Display(Order = 24)]
    public decimal TotalVat { get; set; }

    /// <summary>
    /// Tổng tiền bao gồm thuế VAT
    /// </summary>
    [DisplayName("Tổng tiền bao gồm thuế VAT")]
    [Display(Order = 24)]
    public decimal TotalAmountIncludedVat { get; set; }

    #endregion

    #region Properties - Thông tin hệ thống

    /// <summary>
    /// ID người tạo
    /// </summary>
    [DisplayName("Người tạo")]
    [Display(Order = 30)]
    public Guid? CreatedBy { get; set; }

    /// <summary>
    /// Ngày tạo
    /// </summary>
    [DisplayName("Ngày tạo")]
    [Display(Order = 32)]
    public DateTime? CreatedDate { get; set; }

    /// <summary>
    /// ID người cập nhật
    /// </summary>
    [DisplayName("Người cập nhật")]
    [Display(Order = 33)]
    public Guid? UpdatedBy { get; set; }

    /// <summary>
    /// Ngày cập nhật
    /// </summary>
    [DisplayName("Ngày cập nhật")]
    [Display(Order = 35)]
    public DateTime? UpdatedDate { get; set; }

    #endregion

    #region Properties - Danh sách chi tiết

    /// <summary>
    /// Danh sách chi tiết phiếu nhập
    /// </summary>
    public List<StockInDetailDto> Details { get; set; } = [];

    #endregion
}