using Dal.DataContext;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bll.Inventory.InventoryManagement;

/// <summary>
/// Business Logic Layer interface cho StockInOutDocument
/// Quản lý các thao tác với chứng từ nhập/xuất kho
/// </summary>
public interface IStockInOutDocumentBll
{
    /// <summary>
    /// Lưu chứng từ từ file vào storage (NAS/Local) và metadata vào database
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho (nullable)</param>
    /// <param name="businessPartnerId">ID đối tác (nullable)</param>
    /// <param name="documentFilePath">Đường dẫn file chứng từ</param>
    /// <param name="documentType">Loại chứng từ</param>
    /// <param name="documentCategory">Danh mục chứng từ (nullable)</param>
    /// <param name="documentNumber">Số chứng từ (nullable)</param>
    /// <param name="documentDate">Ngày chứng từ (nullable)</param>
    /// <param name="displayName">Tên hiển thị (nullable)</param>
    /// <param name="description">Mô tả (nullable)</param>
    /// <returns>StockInOutDocument đã lưu</returns>
    Task<StockInOutDocument> SaveDocumentFromFileAsync(
        Guid? stockInOutMasterId,
        Guid? businessPartnerId,
        string documentFilePath,
        int documentType,
        int? documentCategory = null,
        string documentNumber = null,
        DateTime? documentDate = null,
        string displayName = null,
        string description = null);

    /// <summary>
    /// Lấy dữ liệu chứng từ từ storage (NAS/Local) bằng RelativePath
    /// </summary>
    /// <param name="relativePath">Đường dẫn tương đối của chứng từ trong storage</param>
    /// <returns>Dữ liệu file (byte array) hoặc null</returns>
    Task<byte[]> GetDocumentDataByRelativePathAsync(string relativePath);

    /// <summary>
    /// Lấy dữ liệu chứng từ từ storage (NAS/Local) bằng DocumentId
    /// </summary>
    /// <param name="documentId">ID chứng từ</param>
    /// <returns>Dữ liệu file (byte array) hoặc null</returns>
    Task<byte[]> GetDocumentDataAsync(Guid documentId);

    /// <summary>
    /// Xóa chứng từ (database + file từ storage)
    /// </summary>
    /// <param name="documentId">ID chứng từ</param>
    Task DeleteDocumentAsync(Guid documentId);

    /// <summary>
    /// Lấy chứng từ theo ID
    /// </summary>
    /// <param name="id">ID chứng từ</param>
    /// <returns>StockInOutDocument hoặc null</returns>
    StockInOutDocument GetById(Guid id);

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
    /// <param name="fromDate">Từ ngày (nullable)</param>
    /// <param name="toDate">Đến ngày (nullable)</param>
    /// <param name="documentType">Loại chứng từ (nullable)</param>
    /// <param name="documentCategory">Danh mục chứng từ (nullable)</param>
    /// <param name="keyword">Từ khóa tìm kiếm (nullable)</param>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho (nullable)</param>
    /// <param name="businessPartnerId">ID đối tác (nullable)</param>
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
    /// Lấy danh sách chứng từ cần xác minh
    /// </summary>
    /// <returns>Danh sách chứng từ cần xác minh</returns>
    List<StockInOutDocument> GetUnverifiedDocuments();

    /// <summary>
    /// Cập nhật trạng thái xác minh của chứng từ
    /// </summary>
    /// <param name="id">ID chứng từ</param>
    /// <param name="isVerified">Đã xác minh</param>
    Task UpdateVerificationStatusAsync(Guid id, bool isVerified);
}

