using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho InventoryBalance
/// Quản lý các thao tác CRUD với bảng InventoryBalance (Tồn kho theo tháng)
/// </summary>
public interface IInventoryBalanceRepository
{
    /// <summary>
    /// Lưu hoặc cập nhật tồn kho
    /// </summary>
    /// <param name="inventoryBalance">Entity tồn kho cần lưu</param>
    void SaveOrUpdate(InventoryBalance inventoryBalance);

    /// <summary>
    /// Lấy tồn kho theo ID
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <returns>InventoryBalance hoặc null</returns>
    InventoryBalance GetById(Guid id);

    /// <summary>
    /// Lấy tồn kho theo kho, sản phẩm và kỳ
    /// </summary>
    /// <param name="warehouseId">ID kho</param>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <param name="periodYear">Năm</param>
    /// <param name="periodMonth">Tháng (1-12)</param>
    /// <returns>InventoryBalance hoặc null</returns>
    InventoryBalance GetByPeriod(Guid warehouseId, Guid productVariantId, int periodYear, int periodMonth);

    /// <summary>
    /// Lấy danh sách tồn kho theo kho
    /// </summary>
    /// <param name="warehouseId">ID kho</param>
    /// <returns>Danh sách tồn kho</returns>
    List<InventoryBalance> GetByWarehouseId(Guid warehouseId);

    /// <summary>
    /// Lấy danh sách tồn kho theo sản phẩm
    /// </summary>
    /// <param name="productVariantId">ID biến thể sản phẩm</param>
    /// <returns>Danh sách tồn kho</returns>
    List<InventoryBalance> GetByProductVariantId(Guid productVariantId);

    /// <summary>
    /// Lấy danh sách tồn kho theo kỳ
    /// </summary>
    /// <param name="periodYear">Năm</param>
    /// <param name="periodMonth">Tháng (1-12)</param>
    /// <returns>Danh sách tồn kho</returns>
    List<InventoryBalance> GetByPeriod(int periodYear, int periodMonth);

    /// <summary>
    /// Query tồn kho theo nhiều tiêu chí
    /// </summary>
    /// <param name="warehouseId">ID kho (null = tất cả)</param>
    /// <param name="productVariantId">ID biến thể sản phẩm (null = tất cả)</param>
    /// <param name="periodYear">Năm (null = tất cả)</param>
    /// <param name="periodMonth">Tháng (null = tất cả)</param>
    /// <param name="fromDate">Từ ngày (CreateDate) (null = không filter)</param>
    /// <param name="toDate">Đến ngày (CreateDate) (null = không filter)</param>
    /// <param name="isLocked">Đã khóa (null = tất cả)</param>
    /// <param name="isVerified">Đã xác thực (null = tất cả)</param>
    /// <param name="isApproved">Đã phê duyệt (null = tất cả)</param>
    /// <param name="status">Trạng thái (null = tất cả)</param>
    /// <returns>Danh sách tồn kho</returns>
    List<InventoryBalance> QueryBalances(
        Guid? warehouseId = null,
        Guid? productVariantId = null,
        int? periodYear = null,
        int? periodMonth = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        bool? isLocked = null,
        bool? isVerified = null,
        bool? isApproved = null,
        int? status = null);

    /// <summary>
    /// Lấy danh sách tồn kho cần xác thực (IsVerified = false)
    /// </summary>
    /// <returns>Danh sách tồn kho cần xác thực</returns>
    List<InventoryBalance> GetUnverifiedBalances();

    /// <summary>
    /// Lấy danh sách tồn kho cần phê duyệt (IsVerified = true, IsApproved = false)
    /// </summary>
    /// <returns>Danh sách tồn kho cần phê duyệt</returns>
    List<InventoryBalance> GetUnapprovedBalances();

    /// <summary>
    /// Cập nhật trạng thái khóa của tồn kho
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <param name="isLocked">Đã khóa</param>
    /// <param name="lockedBy">ID người khóa</param>
    /// <param name="lockReason">Lý do khóa</param>
    void UpdateLockStatus(Guid id, bool isLocked, Guid lockedBy, string lockReason = null);

    /// <summary>
    /// Cập nhật trạng thái xác thực của tồn kho
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <param name="isVerified">Đã xác thực</param>
    /// <param name="verifiedBy">ID người xác thực</param>
    /// <param name="verificationNotes">Ghi chú xác thực</param>
    void UpdateVerificationStatus(Guid id, bool isVerified, Guid verifiedBy, string verificationNotes = null);

    /// <summary>
    /// Cập nhật trạng thái phê duyệt của tồn kho
    /// </summary>
    /// <param name="id">ID tồn kho</param>
    /// <param name="isApproved">Đã phê duyệt</param>
    /// <param name="approvedBy">ID người phê duyệt</param>
    /// <param name="approvalNotes">Ghi chú phê duyệt</param>
    void UpdateApprovalStatus(Guid id, bool isApproved, Guid approvedBy, string approvalNotes = null);

    /// <summary>
    /// Xóa tồn kho theo ID
    /// </summary>
    /// <param name="id">ID tồn kho cần xóa</param>
    /// <param name="deletedBy">ID người xóa (optional, để tương thích với BLL)</param>
    void Delete(Guid id, Guid deletedBy = default);

    /// <summary>
    /// Tính lại tổng kết tồn kho cho kỳ được chỉ định
    /// Tính toán từ StockInOutDetail và cập nhật lại InventoryBalance
    /// </summary>
    /// <param name="periodYear">Năm kỳ</param>
    /// <param name="periodMonth">Tháng kỳ (1-12)</param>
    /// <returns>Số lượng tồn kho đã được cập nhật</returns>
    int RecalculateSummary(int periodYear, int periodMonth);

    /// <summary>
    /// Kết chuyển dữ liệu tồn kho từ kỳ hiện tại sang kỳ tiếp theo
    /// Tạo tồn kho mới cho kỳ tiếp theo với OpeningBalance = ClosingBalance của kỳ hiện tại
    /// </summary>
    /// <param name="fromPeriodYear">Năm kỳ nguồn</param>
    /// <param name="fromPeriodMonth">Tháng kỳ nguồn (1-12)</param>
    /// <param name="overwriteExisting">Nếu true, ghi đè dữ liệu đã tồn tại ở kỳ đích. Nếu false, báo lỗi nếu đã có dữ liệu</param>
    /// <returns>Số lượng tồn kho đã được kết chuyển</returns>
    int ForwardBalance(int fromPeriodYear, int fromPeriodMonth, bool overwriteExisting = false);
}

