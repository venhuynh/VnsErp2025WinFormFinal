using System;

namespace Dal.DataAccess.Implementations.Inventory.StockIn;

/// <summary>
/// Query criteria cho lịch sử nhập xuất kho
/// Class này nằm trong Dal namespace để tránh circular dependency với DTO
/// </summary>
public class StockInHistoryQueryCriteria
{
    #region Properties - Filter theo thời gian (Bắt buộc)

    /// <summary>
    /// Từ ngày (bắt buộc)
    /// </summary>
    public DateTime FromDate { get; set; }

    /// <summary>
    /// Đến ngày (bắt buộc)
    /// </summary>
    public DateTime ToDate { get; set; }

    #endregion

    #region Properties - Filter theo kho

    /// <summary>
    /// ID kho (optional - nếu null thì lấy tất cả kho)
    /// </summary>
    public Guid? WarehouseId { get; set; }

    /// <summary>
    /// Mã kho (optional - để search nhanh)
    /// </summary>
    public string WarehouseCode { get; set; }

    #endregion

    #region Properties - Filter theo loại và trạng thái

    /// <summary>
    /// Loại nhập kho (int - optional - nếu null thì lấy tất cả loại)
    /// </summary>
    public int? LoaiNhapKho { get; set; }

    /// <summary>
    /// Trạng thái phiếu nhập (int - optional - nếu null thì lấy tất cả trạng thái)
    /// </summary>
    public int? TrangThai { get; set; }

    /// <summary>
    /// Danh sách trạng thái (optional - nếu có thì filter theo danh sách này, ưu tiên hơn TrangThai)
    /// </summary>
    public int[] TrangThaiList { get; set; }

    #endregion

    #region Properties - Filter theo đối tác

    /// <summary>
    /// ID nhà cung cấp/khách hàng (optional)
    /// </summary>
    public Guid? SupplierId { get; set; }

    /// <summary>
    /// Mã nhà cung cấp/khách hàng (optional - để search nhanh)
    /// </summary>
    public string SupplierCode { get; set; }

    #endregion

    #region Properties - Filter theo đơn hàng

    /// <summary>
    /// ID đơn mua hàng (PO) (optional)
    /// </summary>
    public Guid? PurchaseOrderId { get; set; }

    /// <summary>
    /// Số đơn mua hàng (PO) (optional - để search)
    /// </summary>
    public string PurchaseOrderNumber { get; set; }

    #endregion

    #region Properties - Search text

    /// <summary>
    /// Từ khóa tìm kiếm (optional - tìm trong số phiếu, ghi chú)
    /// </summary>
    public string SearchText { get; set; }

    /// <summary>
    /// Số phiếu cụ thể (optional - nếu có thì tìm chính xác số phiếu này)
    /// </summary>
    public string StockInNumber { get; set; }

    #endregion

    #region Properties - Sorting

    /// <summary>
    /// Cột để sắp xếp (mặc định: "StockInDate")
    /// Các giá trị hợp lệ: "StockInDate", "StockInNumber", "TotalAmount", "TotalQuantity", "CreatedDate"
    /// </summary>
    public string OrderBy { get; set; } = "StockInDate";

    /// <summary>
    /// Hướng sắp xếp: "ASC" hoặc "DESC" (mặc định: "DESC")
    /// </summary>
    public string OrderDirection { get; set; } = "DESC";

    #endregion

    #region Properties - Pagination (Optional)

    /// <summary>
    /// Số trang (bắt đầu từ 1, mặc định: null)
    /// Nếu null hoặc <= 0 thì không phân trang, trả về tất cả
    /// </summary>
    public int? PageIndex { get; set; }

    /// <summary>
    /// Số bản ghi mỗi trang (mặc định: null)
    /// Nếu null hoặc <= 0 thì không phân trang, trả về tất cả
    /// </summary>
    public int? PageSize { get; set; }

    #endregion

    #region Helper Properties

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

