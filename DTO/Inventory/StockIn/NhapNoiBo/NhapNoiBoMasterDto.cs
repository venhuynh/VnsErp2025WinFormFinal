using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DTO.Inventory.StockIn.NhapNoiBo;

/// <summary>
/// Data Transfer Object cho danh sách phiếu nhập kho
/// Dùng cho GridControl (danh sách)
/// </summary>
public class NhapNoiBoMasterDtoListDto
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

    #endregion

    #region Properties - Tổng hợp

    /// <summary>
    /// Tổng số lượng nhập
    /// </summary>
    [DisplayName("Tổng SL")]
    [Display(Order = 20)]
    public decimal TotalQuantity { get; set; }

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
public class NhapThietBiMuonMasterDto
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
    [Required(ErrorMessage = "Loại nhập kho không được để trống")]
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
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    public string WarehouseCode { get; set; }

    /// <summary>
    /// Tên kho nhập hàng (để hiển thị)
    /// </summary>
    [DisplayName("Tên kho")]
    [Display(Order = 12)]
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    public string WarehouseName { get; set; }


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
    /// Người xuất hàng
    /// </summary>
    [DisplayName("Người xuất hàng")]
    [Display(Order = 21)]
    [StringLength(500, ErrorMessage = "Người xuất hàng không được vượt quá 500 ký tự")]
    public string NguoiXuatHang { get; set; }

    /// <summary>
    /// Người nhận hàng
    /// </summary>
    [DisplayName("Người nhận hàng")]
    [Display(Order = 22)]
    [StringLength(500, ErrorMessage = "Người nhận hàng không được vượt quá 500 ký tự")]
    public string NguoiNhanHang { get; set; }

    #endregion

    #region Private Fields - Tổng hợp

    private decimal _totalQuantity;

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

    #endregion

    #region Public Methods - Cập nhật tổng hợp

    /// <summary>
    /// Cập nhật các giá trị tổng hợp từ detail
    /// Method này được gọi từ UcStockInMaster khi có thay đổi trong Detail
    /// </summary>
    /// <param name="totalQuantity">Tổng số lượng</param>
    public void SetTotals(decimal totalQuantity)
    {
        _totalQuantity = totalQuantity;
    }

    #endregion
}