using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using CustomLogger = Logger.Interfaces.ILogger;

namespace Dal.DataAccess.Implementations.Inventory.InventoryManagement;

/// <summary>
/// Repository implementation cho StockInOutDocument
/// Quản lý các thao tác CRUD với bảng StockInOutDocument
/// </summary>
public class StockInOutDocumentRepository : IStockInOutDocumentRepository
{
    #region Private Fields

    /// <summary>
    /// Chuỗi kết nối cơ sở dữ liệu để tạo DataContext mới cho mỗi operation.
    /// </summary>
    private readonly string _connectionString;

    /// <summary>
    /// Instance logger để theo dõi các thao tác và lỗi
    /// </summary>
    private readonly CustomLogger _logger;

    #endregion

    #region Constructor

    /// <summary>
    /// Khởi tạo một instance mới của class StockInOutDocumentRepository.
    /// </summary>
    /// <param name="connectionString">Chuỗi kết nối cơ sở dữ liệu</param>
    /// <exception cref="ArgumentNullException">Ném ra khi connectionString là null</exception>
    public StockInOutDocumentRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = LoggerFactory.CreateLogger(LogCategory.DAL);
        _logger.Info("StockInOutDocumentRepository được khởi tạo với connection string");
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Tạo DataContext mới cho mỗi operation để tránh cache issue
    /// </summary>
    /// <returns>DataContext mới</returns>
    private VnsErp2025DataContext CreateNewContext()
    {
        var context = new VnsErp2025DataContext(_connectionString);

        // Configure eager loading cho navigation properties
        var loadOptions = new DataLoadOptions();
        context.LoadOptions = loadOptions;

        return context;
    }

    #endregion

    #region Save Operations

    /// <summary>
    /// Lưu hoặc cập nhật chứng từ nhập/xuất kho
    /// </summary>
    /// <param name="stockInOutDocument">Entity chứng từ cần lưu</param>
    public void SaveOrUpdate(StockInOutDocument stockInOutDocument)
    {
        if (stockInOutDocument == null)
            throw new ArgumentNullException(nameof(stockInOutDocument));

        using var context = CreateNewContext();
        try
        {
            _logger.Debug("SaveOrUpdate: Bắt đầu lưu chứng từ, Id={0}, StockInOutMasterId={1}", 
                stockInOutDocument.Id, stockInOutDocument.StockInOutMasterId);

            var existing = stockInOutDocument.Id != Guid.Empty ? 
                context.StockInOutDocuments.FirstOrDefault(x => x.Id == stockInOutDocument.Id) : null;

            if (existing == null)
            {
                // Thêm mới
                if (stockInOutDocument.Id == Guid.Empty)
                    stockInOutDocument.Id = Guid.NewGuid();
                
                // Thiết lập giá trị mặc định
                stockInOutDocument.CreateDate = DateTime.Now;

                context.StockInOutDocuments.InsertOnSubmit(stockInOutDocument);
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã thêm mới chứng từ, Id={0}", stockInOutDocument.Id);
            }
            else
            {
                // Cập nhật
                existing.StockInOutMasterId = stockInOutDocument.StockInOutMasterId;
                existing.BusinessPartnerId = stockInOutDocument.BusinessPartnerId;
                existing.PurchaseOrderId = stockInOutDocument.PurchaseOrderId;
                existing.RelatedEntityType = stockInOutDocument.RelatedEntityType;
                existing.RelatedEntityId = stockInOutDocument.RelatedEntityId;
                existing.DocumentType = stockInOutDocument.DocumentType;
                existing.DocumentCategory = stockInOutDocument.DocumentCategory;
                existing.DocumentSubType = stockInOutDocument.DocumentSubType;
                existing.FileName = stockInOutDocument.FileName;
                existing.DisplayName = stockInOutDocument.DisplayName;
                existing.Description = stockInOutDocument.Description;
                existing.RelativePath = stockInOutDocument.RelativePath;
                existing.FullPath = stockInOutDocument.FullPath;
                existing.NASShareName = stockInOutDocument.NASShareName;
                existing.StorageType = stockInOutDocument.StorageType;
                existing.StorageProvider = stockInOutDocument.StorageProvider;
                existing.FileExtension = stockInOutDocument.FileExtension;
                existing.MimeType = stockInOutDocument.MimeType;
                existing.FileSize = stockInOutDocument.FileSize;
                existing.Checksum = stockInOutDocument.Checksum;
                existing.FileVersion = stockInOutDocument.FileVersion;
                existing.DocumentNumber = stockInOutDocument.DocumentNumber;
                existing.DocumentDate = stockInOutDocument.DocumentDate;
                existing.IssueDate = stockInOutDocument.IssueDate;
                existing.ExpiryDate = stockInOutDocument.ExpiryDate;
                existing.Amount = stockInOutDocument.Amount;
                existing.Currency = stockInOutDocument.Currency;
                existing.IsPublic = stockInOutDocument.IsPublic;
                existing.IsConfidential = stockInOutDocument.IsConfidential;
                existing.AccessLevel = stockInOutDocument.AccessLevel;
                existing.AccessUrl = stockInOutDocument.AccessUrl;
                existing.PasswordHash = stockInOutDocument.PasswordHash;
                existing.FileExists = stockInOutDocument.FileExists;
                existing.LastVerified = stockInOutDocument.LastVerified;
                existing.IsVerified = stockInOutDocument.IsVerified;
                existing.VerifiedBy = stockInOutDocument.VerifiedBy;
                existing.VerifiedDate = stockInOutDocument.VerifiedDate;
                existing.MigrationStatus = stockInOutDocument.MigrationStatus;
                existing.HasThumbnail = stockInOutDocument.HasThumbnail;
                existing.ThumbnailPath = stockInOutDocument.ThumbnailPath;
                existing.ThumbnailFileName = stockInOutDocument.ThumbnailFileName;
                existing.Tags = stockInOutDocument.Tags;
                existing.Keywords = stockInOutDocument.Keywords;
                existing.ModifiedDate = DateTime.Now;
                existing.ModifiedBy = stockInOutDocument.ModifiedBy;
                existing.IsActive = stockInOutDocument.IsActive;
                
                context.SubmitChanges();
                
                _logger.Info("SaveOrUpdate: Đã cập nhật chứng từ, Id={0}", existing.Id);
            }
        }
        catch (Exception ex)
        {
            _logger.Error($"SaveOrUpdate: Lỗi lưu chứng từ: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Query Operations

    /// <summary>
    /// Lấy chứng từ theo ID
    /// </summary>
    /// <param name="id">ID chứng từ</param>
    /// <returns>StockInOutDocument hoặc null</returns>
    public StockInOutDocument GetById(Guid id)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetById: Lấy chứng từ, Id={0}", id);

            var document = context.StockInOutDocuments.FirstOrDefault(x => x.Id == id);

            if (document == null)
            {
                _logger.Warning("GetById: Không tìm thấy chứng từ với Id={0}", id);
            }
            else
            {
                _logger.Info("GetById: Đã lấy chứng từ, Id={0}, StockInOutMasterId={1}", 
                    id, document.StockInOutMasterId);
            }

            return document;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetById: Lỗi lấy chứng từ: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách chứng từ theo StockInOutMasterId
    /// </summary>
    /// <param name="stockInOutMasterId">ID phiếu nhập/xuất kho</param>
    /// <returns>Danh sách chứng từ</returns>
    public List<StockInOutDocument> GetByStockInOutMasterId(Guid stockInOutMasterId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByStockInOutMasterId: Lấy danh sách chứng từ, StockInOutMasterId={0}", 
                stockInOutMasterId);

            var documents = context.StockInOutDocuments
                .Where(x => x.StockInOutMasterId == stockInOutMasterId)
                .OrderBy(x => x.CreateDate)
                .ToList();
            
            // Load StockInOutMaster cho tất cả documents để có thông tin phiếu
            foreach (var document in documents)
            {
                if (document.StockInOutMaster == null && document.StockInOutMasterId.HasValue)
                {
                    document.StockInOutMaster = context.StockInOutMasters
                        .FirstOrDefault(m => m.Id == document.StockInOutMasterId.Value);
                }
            }

            _logger.Info("GetByStockInOutMasterId: Lấy được {0} chứng từ", documents.Count);
            return documents;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByStockInOutMasterId: Lỗi lấy danh sách chứng từ: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách chứng từ theo BusinessPartnerId
    /// </summary>
    /// <param name="businessPartnerId">ID đối tác</param>
    /// <returns>Danh sách chứng từ</returns>
    public List<StockInOutDocument> GetByBusinessPartnerId(Guid businessPartnerId)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetByBusinessPartnerId: Lấy danh sách chứng từ, BusinessPartnerId={0}", 
                businessPartnerId);

            var documents = context.StockInOutDocuments
                .Where(x => x.BusinessPartnerId == businessPartnerId)
                .OrderBy(x => x.CreateDate)
                .ToList();

            _logger.Info("GetByBusinessPartnerId: Lấy được {0} chứng từ", documents.Count);
            return documents;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetByBusinessPartnerId: Lỗi lấy danh sách chứng từ: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Query chứng từ theo nhiều tiêu chí
    /// </summary>
    public List<StockInOutDocument> QueryDocuments(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? documentType = null,
        int? documentCategory = null,
        string keyword = null,
        Guid? stockInOutMasterId = null,
        Guid? businessPartnerId = null)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("QueryDocuments: Bắt đầu query chứng từ, FromDate={0}, ToDate={1}, DocumentType={2}, Keyword={3}", 
                fromDate, toDate, documentType, keyword ?? "null");

            // Bắt đầu query từ StockInOutDocument
            var queryable = context.StockInOutDocuments.AsQueryable();

            // Filter theo thời gian (CreateDate)
            if (fromDate.HasValue && toDate.HasValue)
            {
                queryable = queryable.Where(x => x.CreateDate >= fromDate.Value.Date && 
                                                x.CreateDate <= toDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            // Filter theo loại chứng từ
            if (documentType.HasValue)
            {
                queryable = queryable.Where(x => x.DocumentType == documentType.Value);
            }

            // Filter theo danh mục
            if (documentCategory.HasValue)
            {
                queryable = queryable.Where(x => x.DocumentCategory == documentCategory.Value);
            }

            // Filter theo StockInOutMasterId
            if (stockInOutMasterId.HasValue)
            {
                queryable = queryable.Where(x => x.StockInOutMasterId == stockInOutMasterId.Value);
            }

            // Filter theo BusinessPartnerId
            if (businessPartnerId.HasValue)
            {
                queryable = queryable.Where(x => x.BusinessPartnerId == businessPartnerId.Value);
            }

            // Filter theo từ khóa
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                var searchText = keyword.Trim();
                queryable = queryable.Where(x =>
                    // Tìm trong tên file
                    (x.FileName != null && x.FileName.Contains(searchText)) ||
                    // Tìm trong tên hiển thị
                    (x.DisplayName != null && x.DisplayName.Contains(searchText)) ||
                    // Tìm trong số chứng từ
                    (x.DocumentNumber != null && x.DocumentNumber.Contains(searchText)) ||
                    // Tìm trong mô tả
                    (x.Description != null && x.Description.Contains(searchText)) ||
                    // Tìm trong đường dẫn tương đối
                    (x.RelativePath != null && x.RelativePath.Contains(searchText)) ||
                    // Tìm trong đường dẫn đầy đủ
                    (x.FullPath != null && x.FullPath.Contains(searchText))
                );
            }

            // Sắp xếp theo ngày tạo (mới nhất trước)
            var result = queryable
                .OrderByDescending(x => x.CreateDate)
                .ThenByDescending(x => x.FileName ?? string.Empty)
                .ToList();

            // Load StockInOutMaster cho tất cả documents để có thông tin phiếu
            var masterIds = result.Where(x => x.StockInOutMasterId.HasValue)
                .Select(x => x.StockInOutMasterId.Value)
                .Distinct()
                .ToList();
            
            if (masterIds.Any())
            {
                var masters = context.StockInOutMasters
                    .Where(m => masterIds.Contains(m.Id))
                    .ToDictionary(m => m.Id);

                foreach (var document in result)
                {
                    if (document.StockInOutMaster == null && 
                        document.StockInOutMasterId.HasValue && 
                        masters.TryGetValue(document.StockInOutMasterId.Value, out var master))
                    {
                        document.StockInOutMaster = master;
                    }
                }
            }

            _logger.Info("QueryDocuments: Query thành công, ResultCount={0}", result.Count);
            return result;
        }
        catch (Exception ex)
        {
            _logger.Error($"QueryDocuments: Lỗi query chứng từ: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy danh sách chứng từ cần xác minh (IsVerified = false)
    /// </summary>
    /// <returns>Danh sách chứng từ cần xác minh</returns>
    public List<StockInOutDocument> GetUnverifiedDocuments()
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("GetUnverifiedDocuments: Lấy danh sách chứng từ cần xác minh");

            var documents = context.StockInOutDocuments
                .Where(x => (x.IsVerified == null || x.IsVerified == false) &&
                           (x.FileExists == null || x.FileExists == true))
                .OrderByDescending(x => x.CreateDate)
                .ToList();

            _logger.Info("GetUnverifiedDocuments: Lấy được {0} chứng từ cần xác minh", documents.Count);
            return documents;
        }
        catch (Exception ex)
        {
            _logger.Error($"GetUnverifiedDocuments: Lỗi lấy danh sách chứng từ cần xác minh: {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Cập nhật trạng thái xác minh của chứng từ
    /// </summary>
    /// <param name="id">ID chứng từ</param>
    /// <param name="isVerified">Đã xác minh</param>
    /// <param name="verifiedBy">ID người xác minh</param>
    public void UpdateVerificationStatus(Guid id, bool isVerified, Guid verifiedBy)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("UpdateVerificationStatus: Cập nhật trạng thái xác minh, Id={0}, IsVerified={1}", 
                id, isVerified);

            var document = context.StockInOutDocuments.FirstOrDefault(x => x.Id == id);
            if (document == null)
            {
                _logger.Warning("UpdateVerificationStatus: Không tìm thấy chứng từ với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy chứng từ với ID: {id}");
            }

            document.IsVerified = isVerified;
            document.VerifiedBy = verifiedBy;
            document.VerifiedDate = DateTime.Now;
            document.LastVerified = DateTime.Now;

            context.SubmitChanges();

            _logger.Info("UpdateVerificationStatus: Đã cập nhật trạng thái xác minh, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"UpdateVerificationStatus: Lỗi cập nhật trạng thái xác minh: {ex.Message}", ex);
            throw;
        }
    }

    #endregion

    #region Delete Operations

    /// <summary>
    /// Xóa chứng từ theo ID
    /// </summary>
    /// <param name="id">ID chứng từ cần xóa</param>
    /// <param name="deletedBy">ID người xóa (optional, để tương thích với BLL)</param>
    public void Delete(Guid id, Guid deletedBy = default)
    {
        using var context = CreateNewContext();
        try
        {
            _logger.Debug("Delete: Bắt đầu xóa chứng từ, Id={0}", id);

            var document = context.StockInOutDocuments.FirstOrDefault(x => x.Id == id);
            if (document == null)
            {
                _logger.Warning("Delete: Không tìm thấy chứng từ với Id={0}", id);
                throw new InvalidOperationException($"Không tìm thấy chứng từ với ID: {id}");
            }

            context.StockInOutDocuments.DeleteOnSubmit(document);
            context.SubmitChanges();

            _logger.Info("Delete: Đã xóa chứng từ, Id={0}", id);
        }
        catch (Exception ex)
        {
            _logger.Error($"Delete: Lỗi xóa chứng từ: {ex.Message}", ex);
            throw;
        }
    }

    #endregion
}
