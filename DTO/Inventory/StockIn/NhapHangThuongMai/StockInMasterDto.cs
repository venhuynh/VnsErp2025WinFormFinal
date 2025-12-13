using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DTO.Inventory.InventoryManagement;

namespace DTO.Inventory.StockIn.NhapHangThuongMai;

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
    public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

    /// <summary>
    /// Trạng thái phiếu nhập
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 4)]
    public TrangThaiPhieuNhapEnum TrangThai { get; set; }

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
    /// Tên nhà cung cấp
    /// </summary>
    [DisplayName("Tên NCC")]
    [Display(Order = 13)]
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

/// <summary>
/// Data Transfer Object cho phiếu nhập kho (chi tiết)
/// Dùng cho form nhập/sửa phiếu nhập kho
/// </summary>
public class StockInMasterDto
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
    [DisplayName("Số phiếu nhập")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Số phiếu nhập không được để trống")]
    [StringLength(50, ErrorMessage = "Số phiếu nhập không được vượt quá 50 ký tự")]
    public string StockInNumber { get; set; }

    /// <summary>
    /// Ngày nhập kho
    /// </summary>
    [DisplayName("Ngày nhập")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Ngày nhập không được để trống")]
    public DateTime StockInDate { get; set; }

    /// <summary>
    /// Loại nhập kho
    /// </summary>
    [DisplayName("Loại nhập")]
    [Display(Order = 3)]
    
    public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

    /// <summary>
    /// Trạng thái phiếu nhập
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 4)]
    public TrangThaiPhieuNhapEnum TrangThai { get; set; }

    #endregion

    #region Properties - Thông tin liên kết

    /// <summary>
    /// ID kho nhập hàng
    /// </summary>
    [DisplayName("ID Kho")]
    [Display(Order = 10)]
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    public Guid WarehouseId { get; set; }

    /// <summary>
    /// Mã kho nhập hàng (để hiển thị)
    /// </summary>
    [DisplayName("Mã kho")]
    [Display(Order = 11)]
    public string WarehouseCode { get; set; }

    /// <summary>
    /// Tên kho nhập hàng (để hiển thị)
    /// </summary>
    [DisplayName("Tên kho")]
    [Display(Order = 12)]
    public string WarehouseName { get; set; }

    /// <summary>
    /// ID đơn mua hàng (PO)
    /// </summary>
    [DisplayName("ID PO")]
    [Display(Order = 13)]
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>
    /// Số đơn mua hàng (PO) (để hiển thị)
    /// </summary>
    [DisplayName("Số PO")]
    [Display(Order = 14)]
    public string PurchaseOrderNumber { get; set; }

    /// <summary>
    /// ID nhà cung cấp (PartnerSiteId)
    /// </summary>
    [DisplayName("ID NCC")]
    [Display(Order = 15)]
    [Required(ErrorMessage = "Nhà cung cấp hoặc khách hàng không được để trống")]
    public Guid? SupplierId { get; set; }

    /// <summary>
    /// Tên nhà cung cấp (để hiển thị)
    /// </summary>
    [DisplayName("Tên NCC")]
    [Display(Order = 16)]
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
    /// Người nhận hàng
    /// </summary>
    [DisplayName("Người nhận hàng")]
    [Display(Order = 21)]
    [StringLength(500, ErrorMessage = "Người nhận hàng không được vượt quá 500 ký tự")]
    public string NguoiNhanHang { get; set; }

    /// <summary>
    /// Người giao hàng
    /// </summary>
    [DisplayName("Người giao hàng")]
    [Display(Order = 22)]
    [StringLength(500, ErrorMessage = "Người giao hàng không được vượt quá 500 ký tự")]
    public string NguoiGiaoHang { get; set; }

    #endregion

    #region Private Fields - Tổng hợp

    private decimal _totalQuantity;
    private decimal _totalAmount;
    private decimal _totalVat;
    private decimal _totalAmountIncludedVat;

    #endregion

    #region Properties - Tổng hợp (Computed)

    /// <summary>
    /// Tổng số lượng nhập - Computed property
    /// Tính toán từ tổng StockInQty của tất cả các dòng detail
    /// Map với: StockInOutMaster.TotalQuantity (lưu vào DB khi save)
    /// </summary>
    [DisplayName("Tổng SL")]
    [Display(Order = 30)]
    public decimal TotalQuantity => _totalQuantity;

    /// <summary>
    /// Tổng giá trị nhập (chưa VAT) - Computed property
    /// Tính toán từ tổng TotalAmount của tất cả các dòng detail
    /// Map với: StockInOutMaster.TotalAmount (lưu vào DB khi save)
    /// </summary>
    [DisplayName("Tổng tiền chưa VAT")]
    [Display(Order = 31)]
    public decimal TotalAmount => _totalAmount;

    /// <summary>
    /// Tổng VAT - Computed property
    /// Tính toán từ tổng VatAmount của tất cả các dòng detail
    /// Map với: StockInOutMaster.TotalVat (lưu vào DB khi save)
    /// </summary>
    [DisplayName("VAT")]
    [Display(Order = 32)]
    public decimal TotalVat => _totalVat;

    /// <summary>
    /// Tổng tiền bao gồm VAT - Computed property
    /// Tính toán từ tổng TotalAmountIncludedVat của tất cả các dòng detail
    /// Map với: StockInOutMaster.TotalAmountIncludedVat (lưu vào DB khi save)
    /// </summary>
    [DisplayName("Tổng tiền bao gồm VAT")]
    [Display(Order = 33)]
    public decimal TotalAmountIncludedVat => _totalAmountIncludedVat;

    #endregion

    #region Public Methods - Cập nhật tổng hợp

    /// <summary>
    /// Cập nhật các giá trị tổng hợp từ detail
    /// Method này được gọi từ UcStockInMaster khi có thay đổi trong Detail
    /// </summary>
    /// <param name="totalQuantity">Tổng số lượng</param>
    /// <param name="totalAmount">Tổng tiền chưa VAT</param>
    /// <param name="totalVat">Tổng VAT</param>
    /// <param name="totalAmountIncludedVat">Tổng tiền bao gồm VAT</param>
    public void SetTotals(decimal totalQuantity, decimal totalAmount, decimal totalVat, decimal totalAmountIncludedVat)
    {
        _totalQuantity = totalQuantity;
        _totalAmount = totalAmount;
        _totalVat = totalVat;
        _totalAmountIncludedVat = totalAmountIncludedVat;
    }

    #endregion
}