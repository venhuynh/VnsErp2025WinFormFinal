using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using DTO.Inventory.InventoryManagement;
using DTO.Inventory.StockIn;

namespace DTO.Inventory.StockOut.XuatLuuChuyenKho;

/// <summary>
/// Data Transfer Object cho danh sách phiếu xuất lưu chuyển kho
/// Dùng cho GridControl (danh sách)
/// </summary>
public class XuatLuuChuyenKhoMasterDtoListDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của phiếu xuất kho
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// Số phiếu xuất kho
    /// </summary>
    [DisplayName("Số phiếu")]
    [Display(Order = 1)]
    public string StockOutNumber { get; set; }

    /// <summary>
    /// Ngày xuất kho
    /// </summary>
    [DisplayName("Ngày xuất")]
    [Display(Order = 2)]
    public DateTime StockOutDate { get; set; }

    /// <summary>
    /// Loại xuất kho
    /// </summary>
    [DisplayName("Loại xuất")]
    [Display(Order = 3)]
    public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

    /// <summary>
    /// Trạng thái phiếu xuất
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 4)]
    public TrangThaiPhieuNhapEnum TrangThai { get; set; }

    #endregion

    #region Properties - Thông tin liên kết

    /// <summary>
    /// Mã kho xuất hàng (kho nguồn)
    /// </summary>
    [DisplayName("Mã kho xuất")]
    [Display(Order = 10)]
    public string WarehouseStockOutCode { get; set; }

    /// <summary>
    /// Tên kho xuất hàng (kho nguồn)
    /// </summary>
    [DisplayName("Tên kho xuất")]
    [Display(Order = 11)]
    public string WarehouseStockOutName { get; set; }

    /// <summary>
    /// Mã kho nhập hàng (kho đích)
    /// </summary>
    [DisplayName("Mã kho nhập")]
    [Display(Order = 12)]
    public string WarehouseStockInCode { get; set; }

    /// <summary>
    /// Tên kho nhập hàng (kho đích)
    /// </summary>
    [DisplayName("Tên kho nhập")]
    [Display(Order = 13)]
    public string WarehouseStockInName { get; set; }

    #endregion

    #region Properties - Tổng hợp

    /// <summary>
    /// Tổng số lượng xuất
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
/// Data Transfer Object cho phiếu xuất lưu chuyển kho (chi tiết)
/// Dùng cho form nhập/sửa phiếu xuất lưu chuyển kho
/// </summary>
public class XuatLuuChuyenKhoMasterDto
{
    #region Properties - Thông tin cơ bản

    /// <summary>
    /// ID duy nhất của phiếu xuất kho
    /// </summary>
    [DisplayName("ID")]
    [Display(Order = -1)]
    public Guid Id { get; set; }

    /// <summary>
    /// Số phiếu xuất kho
    /// </summary>
    [DisplayName("Số phiếu xuất")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Số phiếu xuất không được để trống")]
    [StringLength(50, ErrorMessage = "Số phiếu xuất không được vượt quá 50 ký tự")]
    public string StockOutNumber { get; set; }

    /// <summary>
    /// Ngày xuất kho
    /// </summary>
    [DisplayName("Ngày xuất")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Ngày xuất không được để trống")]
    public DateTime StockOutDate { get; set; }

    /// <summary>
    /// Loại xuất kho
    /// </summary>
    [DisplayName("Loại xuất")]
    [Display(Order = 3)]
    [Required(ErrorMessage = "Loại xuất kho không được để trống")]
    public LoaiNhapXuatKhoEnum LoaiNhapXuatKho { get; set; }

    /// <summary>
    /// Trạng thái phiếu xuất
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 4)]
    public TrangThaiPhieuNhapEnum TrangThai { get; set; }

    #endregion

    #region Properties - Thông tin liên kết

    /// <summary>
    /// ID kho xuất hàng (kho nguồn)
    /// </summary>
    [DisplayName("ID Kho xuất")]
    [Display(Order = 10)]
    [Required(ErrorMessage = "Kho xuất không được để trống")]
    public Guid WarehouseStockOutId { get; set; }

    /// <summary>
    /// Mã kho xuất hàng (kho nguồn, để hiển thị)
    /// </summary>
    [DisplayName("Mã kho xuất")]
    [Display(Order = 11)]
    [Required(ErrorMessage = "Kho xuất không được để trống")]
    public string WarehouseStockOutCode { get; set; }

    /// <summary>
    /// Tên kho xuất hàng (kho nguồn, để hiển thị)
    /// </summary>
    [DisplayName("Tên kho xuất")]
    [Display(Order = 12)]
    [Required(ErrorMessage = "Kho xuất không được để trống")]
    public string WarehouseStockOutName { get; set; }

    /// <summary>
    /// ID kho nhập hàng (kho đích)
    /// </summary>
    [DisplayName("ID Kho nhập")]
    [Display(Order = 13)]
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    public Guid WarehouseStockInId { get; set; }

    /// <summary>
    /// Mã kho nhập hàng (kho đích, để hiển thị)
    /// </summary>
    [DisplayName("Mã kho nhập")]
    [Display(Order = 14)]
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    public string WarehouseStockInCode { get; set; }

    /// <summary>
    /// Tên kho nhập hàng (kho đích, để hiển thị)
    /// </summary>
    [DisplayName("Tên kho nhập")]
    [Display(Order = 15)]
    [Required(ErrorMessage = "Kho nhập không được để trống")]
    public string WarehouseStockInName { get; set; }


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
    /// Người giao hàng
    /// </summary>
    [DisplayName("Người giao hàng")]
    [Display(Order = 21)]
    [StringLength(500, ErrorMessage = "Người giao hàng không được vượt quá 500 ký tự")]
    [Required(ErrorMessage = "Người giao hàng không được để trống")]
    public string NguoiGiaoHang { get; set; }

    /// <summary>
    /// Người nhận hàng
    /// </summary>
    [DisplayName("Người nhận hàng")]
    [Display(Order = 22)]
    [StringLength(500, ErrorMessage = "Người nhận hàng không được vượt quá 500 ký tự")]
    [Required(ErrorMessage = "Người nhận hàng không được để trống")]
    public string NguoiNhanHang { get; set; }

    #endregion

    #region Private Fields - Tổng hợp

    private decimal _totalQuantity;

    #endregion

    #region Properties - Tổng hợp (Computed)

    /// <summary>
    /// Tổng số lượng xuất - Computed property
    /// Tính toán từ tổng StockOutQty của tất cả các dòng detail
    /// Map với: StockInOutMaster.TotalQuantity (lưu vào DB khi save)
    /// </summary>
    [DisplayName("Tổng SL")]
    [Display(Order = 30)]
    public decimal TotalQuantity => _totalQuantity;

    #endregion

    #region Public Methods - Cập nhật tổng hợp

    /// <summary>
    /// Cập nhật các giá trị tổng hợp từ detail
    /// Method này được gọi từ UcStockOutMaster khi có thay đổi trong Detail
    /// </summary>
    /// <param name="totalQuantity">Tổng số lượng xuất</param>
    public void SetTotals(decimal totalQuantity)
    {
        _totalQuantity = totalQuantity;
    }

    #endregion
}