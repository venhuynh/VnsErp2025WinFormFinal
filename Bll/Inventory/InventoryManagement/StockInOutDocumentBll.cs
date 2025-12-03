using Dal.Connection;
using Dal.DataAccess.Implementations.Inventory.InventoryManagement;
using Dal.DataAccess.Interfaces.Inventory.InventoryManagement;
using Dal.DataContext;
using Logger;
using Logger.Configuration;
using Logger.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Bll.Common.ImageStorage;
// ReSharper disable UnusedParameter.Local

namespace Bll.Inventory.InventoryManagement;

/// <summary>
/// Business Logic Layer cho StockInOutDocument
/// Sử dụng ImageStorageService để lưu trữ chứng từ trên NAS/Local thay vì database
/// </summary>
public class StockInOutDocumentBll
{
    #region Fields

    private IStockInOutDocumentRepository _dataAccess;
    private readonly IImageStorageService _imageStorage;
    private readonly ILogger _logger;
    private readonly object _lockObject = new();

    #endregion

    #region Constructors

    /// <summary>
    /// Constructor mặc định
    /// </summary>
    public StockInOutDocumentBll()
    {
        _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        _imageStorage = ImageStorageFactory.CreateFromConfig(_logger);
    }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Lấy hoặc khởi tạo Repository (lazy initialization)
    /// </summary>
    private IStockInOutDocumentRepository GetDataAccess()
    {
        if (_dataAccess == null)
        {
            lock (_lockObject)
            {
                if (_dataAccess == null)
                {
                    try
                    {
                        // Sử dụng global connection string từ ApplicationStartupManager
                        var globalConnectionString = ApplicationStartupManager.Instance.GetGlobalConnectionString();
                        if (string.IsNullOrEmpty(globalConnectionString))
                        {
                            throw new InvalidOperationException(
                                "Không có global connection string. Ứng dụng chưa được khởi tạo hoặc chưa sẵn sàng.");
                        }

                        _dataAccess = new StockInOutDocumentRepository(globalConnectionString);
                    }
                    catch (Exception ex)
                    {
                        _logger.Error($"Lỗi khởi tạo StockInOutDocumentRepository: {ex.Message}", ex);
                        throw;
                    }
                }
            }
        }

        return _dataAccess;
    }

    /// <summary>
    /// Lấy MIME type từ file extension
    /// </summary>
    private string GetMimeType(string fileExtension)
    {
        if (string.IsNullOrEmpty(fileExtension))
            return "application/octet-stream";

        var ext = fileExtension.TrimStart('.').ToLower();
        return ext switch
        {
            "pdf" => "application/pdf",
            "doc" => "application/msword",
            "docx" => "application/vnd.openxmlformats-officedocument.wordprocessingml.document",
            "xls" => "application/vnd.ms-excel",
            "xlsx" => "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "jpg" or "jpeg" => "image/jpeg",
            "png" => "image/png",
            "gif" => "image/gif",
            "txt" => "text/plain",
            "zip" => "application/zip",
            "rar" => "application/x-rar-compressed",
            _ => "application/octet-stream"
        };
    }

    /// <summary>
    /// Tính checksum MD5 của file
    /// </summary>
    private string CalculateChecksum(byte[] data)
    {
        if (data == null || data.Length == 0)
            return null;

        using var md5 = MD5.Create();
        var hash = md5.ComputeHash(data);
        return BitConverter.ToString(hash).Replace("-", "").ToLowerInvariant();
    }

    /// <summary>
    /// Chuyển đổi FileCategory từ DocumentType và entity
    /// </summary>
    private FileCategory GetFileCategory(int documentType, Guid? stockInOutMasterId, Guid? businessPartnerId)
    {
        if (stockInOutMasterId.HasValue)
            return FileCategory.StockInOutDocument;
        
        if (businessPartnerId.HasValue)
            return FileCategory.BusinessPartnerDocument;

        return FileCategory.Document;
    }

    /// <summary>
    /// Loại bỏ các ký tự không hợp lệ trong tên file
    /// </summary>
    private string SanitizeFileName(string fileName)
    {
        if (string.IsNullOrWhiteSpace(fileName))
            return "PHIEU";

        // Loại bỏ các ký tự không hợp lệ: < > : " / \ | ? *
        var invalidChars = Path.GetInvalidFileNameChars();
        var sanitized = fileName;
        
        foreach (var c in invalidChars)
        {
            sanitized = sanitized.Replace(c, '_');
        }

        // Loại bỏ khoảng trắng ở đầu và cuối
        sanitized = sanitized.Trim();

        // Nếu sau khi sanitize trống, trả về giá trị mặc định
        if (string.IsNullOrWhiteSpace(sanitized))
            return "PHIEU";

        return sanitized;
    }

    #endregion

    #region Business Methods

    /// <summary>
    /// Lưu chứng từ từ file vào storage (NAS/Local) và metadata vào database
    /// </summary>
    public async Task<StockInOutDocument> SaveDocumentFromFileAsync(
        Guid? stockInOutMasterId,
        Guid? businessPartnerId,
        string documentFilePath,
        int documentType,
        int? documentCategory = null,
        string documentNumber = null,
        DateTime? documentDate = null,
        string displayName = null,
        string description = null)
    {
        try
        {
            if (!File.Exists(documentFilePath))
            {
                throw new FileNotFoundException($"File chứng từ không tồn tại: {documentFilePath}");
            }

            // 1. Đọc file
            var fileData = await Task.Run(() => File.ReadAllBytes(documentFilePath));

            // 2. Tạo tên file mới theo format: "Số phiếu (số thứ tự file)"
            var fileExtension = Path.GetExtension(documentFilePath);
            var originalFileName = Path.GetFileName(documentFilePath);
            string fileName;

            if (stockInOutMasterId.HasValue)
            {
                // Lấy thông tin StockInOutMaster để lấy VocherNumber
                var dataAccess = GetDataAccess();
                var existingDocuments = dataAccess.GetByStockInOutMasterId(stockInOutMasterId.Value);
                
                // Tính số thứ tự file (bắt đầu từ 1)
                var sequenceNumber = existingDocuments.Count + 1;
                
                // Lấy VocherNumber từ StockInOutMaster
                string vocherNumber = "PHIEU";
                try
                {
                    using (var context = new VnsErp2025DataContext(ApplicationStartupManager.Instance.GetGlobalConnectionString()))
                    {
                        var master = context.StockInOutMasters.FirstOrDefault(m => m.Id == stockInOutMasterId.Value);
                        if (master != null && !string.IsNullOrWhiteSpace(master.VocherNumber))
                        {
                            vocherNumber = master.VocherNumber;
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Warning($"Không thể lấy VocherNumber từ StockInOutMaster: {ex.Message}");
                }
                
                // Tạo tên file: VocherNumber_SequenceNumber.extension
                // Loại bỏ các ký tự không hợp lệ trong tên file
                var safeVocherNumber = SanitizeFileName(vocherNumber);
                fileName = $"{safeVocherNumber}_{sequenceNumber:D3}{fileExtension}";
            }
            else
            {
                // Nếu không có stockInOutMasterId, dùng format cũ
                fileName = $"Document_{businessPartnerId ?? Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}_{Guid.NewGuid():N}{fileExtension}";
            }

            // 3. Xác định FileCategory
            var fileCategory = GetFileCategory(documentType, stockInOutMasterId, businessPartnerId);

            // 4. Lưu vào storage (NAS/Local) thông qua ImageStorageService
            // Sử dụng IFileStorageService interface (IImageStorageService kế thừa IFileStorageService)
            if (_imageStorage is not IFileStorageService fileStorage)
            {
                throw new InvalidOperationException("ImageStorageService không implement IFileStorageService");
            }

            var storageResult = await fileStorage.SaveFileAsync(
                fileData: fileData,
                fileName: fileName,
                category: fileCategory,
                entityId: stockInOutMasterId ?? businessPartnerId,
                generateThumbnail: false // Documents thường không cần thumbnail
            );

            if (!storageResult.Success)
            {
                throw new InvalidOperationException(
                    $"Không thể lưu chứng từ vào storage: {storageResult.ErrorMessage}");
            }

            // 5. Lấy thông tin user hiện tại
            var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
            var createBy = currentUser?.Id ?? Guid.Empty;

            // 6. Tính checksum
            var checksum = CalculateChecksum(fileData);

            // 7. Tạo StockInOutDocument entity
            var stockInOutDocument = new StockInOutDocument
            {
                Id = Guid.NewGuid(),
                StockInOutMasterId = stockInOutMasterId,
                BusinessPartnerId = businessPartnerId,
                PurchaseOrderId = null,
                RelatedEntityType = stockInOutMasterId.HasValue ? "StockInOutMaster" : 
                                   businessPartnerId.HasValue ? "BusinessPartner" : null,
                RelatedEntityId = stockInOutMasterId ?? businessPartnerId,
                DocumentType = documentType,
                DocumentCategory = documentCategory,
                DocumentSubType = null,
                FileName = fileName, // Lưu tên file mới (VocherNumber_SequenceNumber.extension)
                DisplayName = displayName ?? fileName, // Hiển thị tên file mới để trực quan hơn
                Description = description,
                RelativePath = storageResult.RelativePath,
                FullPath = storageResult.FullPath,
                NASShareName = "ERP_Documents",
                StorageType = "NAS",
                StorageProvider = null,
                FileExtension = EnsureFileExtensionFormat(fileExtension),
                MimeType = storageResult.MimeType ?? GetMimeType(fileExtension),
                FileSize = fileData.Length,
                Checksum = storageResult.Checksum ?? checksum,
                FileVersion = 1,
                DocumentNumber = documentNumber,
                DocumentDate = documentDate,
                IssueDate = documentDate,
                ExpiryDate = null,
                Amount = null,
                Currency = null,
                IsPublic = false,
                IsConfidential = false,
                AccessLevel = 1, // Internal
                AccessUrl = null,
                PasswordHash = null,
                FileExists = true,
                LastVerified = null,
                IsVerified = false,
                VerifiedBy = null,
                VerifiedDate = null,
                MigrationStatus = "Migrated",
                HasThumbnail = false,
                ThumbnailPath = null,
                ThumbnailFileName = null,
                Tags = null,
                Keywords = null,
                CreateDate = DateTime.Now,
                CreateBy = createBy,
                ModifiedDate = null,
                ModifiedBy = null,
                IsActive = true,
                IsDeleted = false,
                DeletedDate = null,
                DeletedBy = null
            };

            // 8. Lưu metadata vào database
            GetDataAccess().SaveOrUpdate(stockInOutDocument);

            _logger.Info($"Đã lưu chứng từ, StockInOutMasterId={stockInOutMasterId}, BusinessPartnerId={businessPartnerId}, DocumentId={stockInOutDocument.Id}, RelativePath={storageResult.RelativePath}");

            return stockInOutDocument;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lưu chứng từ từ file '{documentFilePath}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy dữ liệu chứng từ từ storage (NAS/Local) bằng RelativePath
    /// </summary>
    public async Task<byte[]> GetDocumentDataByRelativePathAsync(string relativePath)
    {
        try
        {
            if (string.IsNullOrEmpty(relativePath))
            {
                _logger.Warning("RelativePath is null or empty");
                return null;
            }

            // Lấy trực tiếp từ storage sử dụng RelativePath
            // Sử dụng IFileStorageService interface (IImageStorageService kế thừa IFileStorageService)
            if (_imageStorage is not IFileStorageService fileStorage)
            {
                _logger.Warning("ImageStorageService không implement IFileStorageService");
                return null;
            }

            var documentData = await fileStorage.GetFileAsync(relativePath);
            
            if (documentData == null)
            {
                _logger.Warning($"Không thể đọc file từ storage, RelativePath={relativePath}");
            }
            else
            {
                _logger.Debug($"Đã load chứng từ từ storage, RelativePath={relativePath}, Size={documentData.Length} bytes");
            }

            return documentData;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy dữ liệu chứng từ từ RelativePath '{relativePath}': {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Đảm bảo FileExtension có format đúng theo constraint: bắt đầu bằng dấu chấm và lowercase
    /// Constraint yêu cầu: LIKE '.[a-z][a-z0-9][a-z0-9][a-z0-9]%'
    /// </summary>
    private string EnsureFileExtensionFormat(string fileExtension)
    {
        if (string.IsNullOrWhiteSpace(fileExtension))
        {
            return ".unknown";
        }

        // Loại bỏ dấu chấm ở đầu nếu có, sau đó thêm lại và chuyển thành lowercase
        var extension = fileExtension.TrimStart('.').ToLower();
        
        // Đảm bảo có ít nhất 1 ký tự
        if (string.IsNullOrEmpty(extension))
        {
            return ".unknown";
        }

        // Thêm dấu chấm ở đầu và trả về
        return "." + extension;
    }

    /// <summary>
    /// Lấy dữ liệu chứng từ từ storage (NAS/Local) bằng DocumentId
    /// </summary>
    public async Task<byte[]> GetDocumentDataAsync(Guid documentId)
    {
        try
        {
            // 1. Query database để lấy metadata (RelativePath)
            var document = GetDataAccess().GetById(documentId);
            if (document == null)
            {
                _logger.Warning($"Không tìm thấy chứng từ với ID '{documentId}'");
                return null;
            }

            // 2. Lấy RelativePath từ metadata
            var relativePath = document.RelativePath;
            
            if (string.IsNullOrEmpty(relativePath))
            {
                _logger.Warning($"Chứng từ '{documentId}' không có RelativePath");
                return null;
            }

            // 3. Load từ storage sử dụng RelativePath
            var documentData = await GetDocumentDataByRelativePathAsync(relativePath);
            
            return documentData;
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi lấy dữ liệu chứng từ '{documentId}': {ex.Message}", ex);
            return null;
        }
    }

    /// <summary>
    /// Xóa chứng từ (database + file từ storage)
    /// </summary>
    public async Task DeleteDocumentAsync(Guid documentId)
    {
        try
        {
            // 1. Lấy thông tin chứng từ
            var document = GetDataAccess().GetById(documentId);
            if (document == null)
            {
                _logger.Warning($"Không tìm thấy chứng từ với ID '{documentId}'");
                throw new InvalidOperationException($"Không tìm thấy chứng từ với ID: {documentId}");
            }

            // 2. Xóa file từ storage nếu có RelativePath
            if (!string.IsNullOrEmpty(document.RelativePath))
            {
                try
                {
                    // Sử dụng IFileStorageService interface (IImageStorageService kế thừa IFileStorageService)
                    if (_imageStorage is IFileStorageService fileStorage)
                    {
                        var deleted = await fileStorage.DeleteFileAsync(document.RelativePath);
                        if (deleted)
                        {
                            _logger.Info($"Đã xóa file chứng từ từ storage, RelativePath={document.RelativePath}");
                        }
                        else
                        {
                            _logger.Warning($"Không thể xóa file chứng từ từ storage, RelativePath={document.RelativePath}");
                        }
                    }
                    else
                    {
                        // Fallback: sử dụng DeleteImageAsync nếu không có IFileStorageService
                        var deleted = await _imageStorage.DeleteImageAsync(document.RelativePath);
                        if (deleted)
                        {
                            _logger.Info($"Đã xóa file chứng từ từ storage (fallback), RelativePath={document.RelativePath}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.Error($"Lỗi khi xóa file chứng từ từ storage: {ex.Message}", ex);
                    // Tiếp tục xóa database record dù file xóa thất bại
                }
            }

            // 3. Xóa record trong database (soft delete)
            var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
            var deletedBy = currentUser?.Id ?? Guid.Empty;
            GetDataAccess().Delete(documentId, deletedBy);

            _logger.Info($"Đã xóa chứng từ, DocumentId={documentId}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi xóa chứng từ '{documentId}': {ex.Message}", ex);
            throw;
        }
    }

    /// <summary>
    /// Lấy chứng từ theo ID
    /// </summary>
    public StockInOutDocument GetById(Guid id)
    {
        return GetDataAccess().GetById(id);
    }

    /// <summary>
    /// Lấy danh sách chứng từ theo StockInOutMasterId
    /// </summary>
    public List<StockInOutDocument> GetByStockInOutMasterId(Guid stockInOutMasterId)
    {
        return GetDataAccess().GetByStockInOutMasterId(stockInOutMasterId);
    }

    /// <summary>
    /// Lấy danh sách chứng từ theo BusinessPartnerId
    /// </summary>
    public List<StockInOutDocument> GetByBusinessPartnerId(Guid businessPartnerId)
    {
        return GetDataAccess().GetByBusinessPartnerId(businessPartnerId);
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
        return GetDataAccess().QueryDocuments(
            fromDate, toDate, documentType, documentCategory, keyword, 
            stockInOutMasterId, businessPartnerId);
    }

    /// <summary>
    /// Lấy danh sách chứng từ cần xác minh
    /// </summary>
    public List<StockInOutDocument> GetUnverifiedDocuments()
    {
        return GetDataAccess().GetUnverifiedDocuments();
    }

    /// <summary>
    /// Cập nhật trạng thái xác minh của chứng từ
    /// </summary>
    public Task UpdateVerificationStatusAsync(Guid id, bool isVerified)
    {
        try
        {
            var currentUser = Common.ApplicationSystemUtils.GetCurrentUser();
            var verifiedBy = currentUser?.Id ?? Guid.Empty;

            GetDataAccess().UpdateVerificationStatus(id, isVerified, verifiedBy);

            _logger.Info($"Đã cập nhật trạng thái xác minh, DocumentId={id}, IsVerified={isVerified}");
        }
        catch (Exception ex)
        {
            _logger.Error($"Lỗi khi cập nhật trạng thái xác minh '{id}': {ex.Message}", ex);
            throw;
        }

        return Task.CompletedTask;
    }

    /// <summary>
    /// Lưu chứng từ từ file (synchronous version - backward compatibility)
    /// </summary>
    public StockInOutDocument SaveDocumentFromFile(
        Guid? stockInOutMasterId,
        Guid? businessPartnerId,
        string documentFilePath,
        int documentType,
        int? documentCategory = null,
        string documentNumber = null,
        DateTime? documentDate = null,
        string displayName = null,
        string description = null)
    {
        return SaveDocumentFromFileAsync(
            stockInOutMasterId,
            businessPartnerId,
            documentFilePath,
            documentType,
            documentCategory,
            documentNumber,
            documentDate,
            displayName,
            description).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Xóa chứng từ (synchronous version - backward compatibility)
    /// </summary>
    public void DeleteDocument(Guid documentId)
    {
        DeleteDocumentAsync(documentId).GetAwaiter().GetResult();
    }

    #endregion
}
