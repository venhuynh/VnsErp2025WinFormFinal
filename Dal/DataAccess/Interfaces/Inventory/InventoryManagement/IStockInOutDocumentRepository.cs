using Dal.DataContext;
using System;
using System.Collections.Generic;

namespace Dal.DataAccess.Interfaces.Inventory.InventoryManagement;

/// <summary>
/// Repository interface cho StockInOutDocument
/// Quản lý các thao tác CRUD với bảng StockInOutDocument
/// </summary>
public interface IStockInOutDocumentRepository
{
    /// <summary>
    /// Lưu hoặc cập nhật chứng từ nhập/xuất kho
    /// </summary>
    /// <param name="stockInOutDocument">Entity chứng từ cần lưu</param>
    void SaveOrUpdate(StockInOutDocument stockInOutDocument);

    /// <summary>
    /// Lấy chứng từ theo ID
    /// </summary>
    /// <param name="id">ID chứng từ</param>
    /// <returns>StockInOutDocument hoặc null</returns>
    StockInOutDocument GetById(Guid id);

    /// <summary>
    /// Xóa chứng từ theo ID (soft delete)
    /// </summary>
    /// <param name="id">ID chứng từ cần xóa</param>
    /// <param name="deletedBy">ID người xóa</param>
    void Delete(Guid id, Guid deletedBy);

    /// <summary>
    /// Lấy danh sách chứng từ theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách chứng từ</returns>
    List<StockInOutDocument> GetByStockInOutMasterId(Guid stockInOutMasterId);

    /// <summary>
    /// Lấy danh sách chứng từ theo BusinessPartnerId
    /// </summary>
    /// <param name="businessPartnerId">ID đối tác</param>
    /// <returns>Danh sách chứng từ</returns>
    List<StockInOutDocument> GetByBusinessPartnerId(Guid businessPartnerId);

    /// <summary>
    /// Query chứng từ theo nhiều tiêu chí
    /// </summary>
    /// <param name="fromDate">Từ ngày (DocumentDate hoặc CreateDate)</param>
    /// <param name="toDate">Đến ngày (DocumentDate hoặc CreateDate)</param>
    /// <param name="documentType">Loại chứng từ (null = tất cả)</param>
    /// <param name="documentCategory">Danh mục chứng từ (null = tất cả)</param>
    /// <param name="keyword">Từ khóa tìm kiếm (tên file, số chứng từ, mô tả)</param>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho (null = tất cả)</param>
    /// <param name="businessPartnerId">ID đối tác (null = tất cả)</param>
    /// <returns>Danh sách chứng từ</returns>
    List<StockInOutDocument> QueryDocuments(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? documentType = null,
        int? documentCategory = null,
        string keyword = null,
        Guid? stockInOutMasterId = null,
        Guid? businessPartnerId = null);

    /// <summary>
    /// Lấy danh sách chứng từ cần xác minh (IsVerified = false)
    /// </summary>
    /// <returns>Danh sách chứng từ cần xác minh</returns>
    List<StockInOutDocument> GetUnverifiedDocuments();

    /// <summary>
    /// Cập nhật trạng thái xác minh của chứng từ
    /// </summary>
    /// <param name="id">ID chứng từ</param>
    /// <param name="isVerified">Đã xác minh</param>
    /// <param name="verifiedBy">ID người xác minh</param>
    void UpdateVerificationStatus(Guid id, bool isVerified, Guid verifiedBy);
}

