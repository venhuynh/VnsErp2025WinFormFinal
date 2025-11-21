using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using DTO.Inventory.StockIn;

namespace DTO.Inventory.InventoryManagement;

/// <summary>
/// DTO cho truy vấn lịch sử nhập xuất kho
/// Hỗ trợ filter theo nhiều tiêu chí và pagination
/// </summary>
public class StockInHistoryQueryDto
{
    #region Properties - Filter theo thời gian (Bắt buộc)

    /// <summary>
    /// Từ ngày (bắt buộc)
    /// </summary>
    [DisplayName("Từ ngày")]
    [Display(Order = 1)]
    [Required(ErrorMessage = "Từ ngày không được để trống")]
    public DateTime FromDate { get; set; }

    /// <summary>
    /// Đến ngày (bắt buộc)
    /// </summary>
    [DisplayName("Đến ngày")]
    [Display(Order = 2)]
    [Required(ErrorMessage = "Đến ngày không được để trống")]
    public DateTime ToDate { get; set; }

    #endregion

    #region Properties - Filter theo kho

    /// <summary>
    /// ID kho (optional - nếu null thì lấy tất cả kho)
    /// </summary>
    [DisplayName("ID Kho")]
    [Display(Order = 10)]
    public Guid? WarehouseId { get; set; }

    /// <summary>
    /// Mã kho (optional - để search nhanh)
    /// </summary>
    [DisplayName("Mã kho")]
    [Display(Order = 11)]
    public string WarehouseCode { get; set; }

    #endregion

    #region Properties - Filter theo loại và trạng thái

    /// <summary>
    /// Loại nhập kho (optional - nếu null thì lấy tất cả loại)
    /// </summary>
    [DisplayName("Loại nhập kho")]
    [Display(Order = 20)]
    public LoaiNhapKhoEnum? LoaiNhapKho { get; set; }

    /// <summary>
    /// Trạng thái phiếu nhập (optional - nếu null thì lấy tất cả trạng thái)
    /// </summary>
    [DisplayName("Trạng thái")]
    [Display(Order = 21)]
    public TrangThaiPhieuNhapEnum? TrangThai { get; set; }

    /// <summary>
    /// Danh sách trạng thái (optional - nếu có thì filter theo danh sách này, ưu tiên hơn TrangThai)
    /// </summary>
    [DisplayName("Danh sách trạng thái")]
    [Display(Order = 22)]
    public TrangThaiPhieuNhapEnum[] TrangThaiList { get; set; }

    #endregion

    #region Properties - Filter theo đối tác

    /// <summary>
    /// ID nhà cung cấp/khách hàng (optional)
    /// </summary>
    [DisplayName("ID Đối tác")]
    [Display(Order = 30)]
    public Guid? SupplierId { get; set; }

    /// <summary>
    /// Mã nhà cung cấp/khách hàng (optional - để search nhanh)
    /// </summary>
    [DisplayName("Mã đối tác")]
    [Display(Order = 31)]
    public string SupplierCode { get; set; }

    #endregion

    #region Properties - Filter theo đơn hàng

    /// <summary>
    /// ID đơn mua hàng (PO) (optional)
    /// </summary>
    [DisplayName("ID PO")]
    [Display(Order = 40)]
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>
    /// Số đơn mua hàng (PO) (optional - để search)
    /// </summary>
    [DisplayName("Số PO")]
    [Display(Order = 41)]
    public string PurchaseOrderNumber { get; set; }

    #endregion

    #region Properties - Search text

    /// <summary>
    /// Từ khóa tìm kiếm (optional - tìm trong số phiếu, ghi chú)
    /// </summary>
    [DisplayName("Từ khóa tìm kiếm")]
    [Display(Order = 50)]
    [StringLength(200, ErrorMessage = "Từ khóa tìm kiếm không được vượt quá 200 ký tự")]
    public string SearchText { get; set; }

    /// <summary>
    /// Số phiếu cụ thể (optional - nếu có thì tìm chính xác số phiếu này)
    /// </summary>
    [DisplayName("Số phiếu")]
    [Display(Order = 51)]
    [StringLength(50, ErrorMessage = "Số phiếu không được vượt quá 50 ký tự")]
    public string StockInNumber { get; set; }

    #endregion

    #region Properties - Sorting

    /// <summary>
    /// Cột để sắp xếp (mặc định: "StockInDate")
    /// Các giá trị hợp lệ: "StockInDate", "StockInNumber", "TotalAmount", "TotalQuantity", "CreatedDate"
    /// </summary>
    [DisplayName("Sắp xếp theo")]
    [Display(Order = 60)]
    public string OrderBy { get; set; } = "StockInDate";

    /// <summary>
    /// Hướng sắp xếp: "ASC" hoặc "DESC" (mặc định: "DESC")
    /// </summary>
    [DisplayName("Thứ tự")]
    [Display(Order = 61)]
    public string OrderDirection { get; set; } = "DESC";

    #endregion

    #region Properties - Pagination (Optional)

    /// <summary>
    /// Số trang (bắt đầu từ 1, mặc định: 1)
    /// Nếu null hoặc <= 0 thì không phân trang, trả về tất cả
    /// </summary>
    [DisplayName("Số trang")]
    [Display(Order = 70)]
    [Range(1, int.MaxValue, ErrorMessage = "Số trang phải lớn hơn 0")]
    public int? PageIndex { get; set; }

    /// <summary>
    /// Số bản ghi mỗi trang (mặc định: 100)
    /// Nếu null hoặc <= 0 thì không phân trang, trả về tất cả
    /// </summary>
    [DisplayName("Số bản ghi mỗi trang")]
    [Display(Order = 71)]
    [Range(1, 1000, ErrorMessage = "Số bản ghi mỗi trang phải từ 1 đến 1000")]
    public int? PageSize { get; set; }

    #endregion

    #region Methods - Validation và Helper

    /// <summary>
    /// Validate query DTO
    /// </summary>
    /// <returns>True nếu hợp lệ, False nếu không hợp lệ</returns>
    public bool Validate(out string errorMessage)
    {
        errorMessage = string.Empty;

        // Validate FromDate và ToDate
        if (FromDate == default(DateTime))
        {
            errorMessage = "Từ ngày không được để trống";
            return false;
        }

        if (ToDate == default(DateTime))
        {
            errorMessage = "Đến ngày không được để trống";
            return false;
        }

        if (FromDate > ToDate)
        {
            errorMessage = "Từ ngày không được lớn hơn đến ngày";
            return false;
        }

        // Validate OrderBy
        var validOrderByFields = new[] { "StockInDate", "StockInNumber", "TotalAmount", "TotalQuantity", "CreatedDate" };
        if (!string.IsNullOrWhiteSpace(OrderBy) && !validOrderByFields.Contains(OrderBy, StringComparer.OrdinalIgnoreCase))
        {
            errorMessage = $"Cột sắp xếp không hợp lệ. Các giá trị hợp lệ: {string.Join(", ", validOrderByFields)}";
            return false;
        }

        // Validate OrderDirection
        if (!string.IsNullOrWhiteSpace(OrderDirection) && 
            !OrderDirection.Equals("ASC", StringComparison.OrdinalIgnoreCase) && 
            !OrderDirection.Equals("DESC", StringComparison.OrdinalIgnoreCase))
        {
            errorMessage = "Hướng sắp xếp phải là 'ASC' hoặc 'DESC'";
            return false;
        }

        // Validate PageSize
        if (PageSize.HasValue && (PageSize.Value < 1 || PageSize.Value > 1000))
        {
            errorMessage = "Số bản ghi mỗi trang phải từ 1 đến 1000";
            return false;
        }

        return true;
    }

    /// <summary>
    /// Kiểm tra có sử dụng pagination không
    /// </summary>
    public bool UsePagination => PageIndex.HasValue && PageIndex.Value > 0 && 
                                 PageSize.HasValue && PageSize.Value > 0;

    /// <summary>
    /// Tính toán số bản ghi bỏ qua (skip) cho pagination
    /// </summary>
    public int SkipCount => UsePagination ? (PageIndex.Value - 1) * PageSize.Value : 0;

    /// <summary>
    /// Tính toán số bản ghi lấy (take) cho pagination
    /// </summary>
    public int TakeCount => UsePagination ? PageSize.Value : int.MaxValue;

    #endregion
}

/// <summary>
/// DTO cho kết quả truy vấn lịch sử nhập xuất kho (có pagination info)
/// </summary>
public class StockInHistoryResultDto
{
    /// <summary>
    /// Danh sách phiếu nhập kho
    /// </summary>
    [DisplayName("Danh sách phiếu nhập")]
    public List<StockInListDto> Items { get; set; } = new List<StockInListDto>();

    /// <summary>
    /// Tổng số bản ghi (không phân trang)
    /// </summary>
    [DisplayName("Tổng số bản ghi")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Số trang hiện tại
    /// </summary>
    [DisplayName("Số trang")]
    public int? PageIndex { get; set; }

    /// <summary>
    /// Số bản ghi mỗi trang
    /// </summary>
    [DisplayName("Số bản ghi mỗi trang")]
    public int? PageSize { get; set; }

    /// <summary>
    /// Tổng số trang
    /// </summary>
    [DisplayName("Tổng số trang")]
    public int? TotalPages => PageSize.HasValue && PageSize.Value > 0 
        ? (int)Math.Ceiling((double)TotalCount / PageSize.Value) 
        : null;

    /// <summary>
    /// Có trang tiếp theo không
    /// </summary>
    [DisplayName("Có trang tiếp theo")]
    public bool HasNextPage => PageIndex.HasValue && TotalPages.HasValue && PageIndex.Value < TotalPages.Value;

    /// <summary>
    /// Có trang trước đó không
    /// </summary>
    [DisplayName("Có trang trước")]
    public bool HasPreviousPage => PageIndex.HasValue && PageIndex.Value > 1;
}

