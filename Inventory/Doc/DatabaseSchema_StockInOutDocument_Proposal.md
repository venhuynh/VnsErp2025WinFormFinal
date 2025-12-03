# Đề Xuất Bảng Lưu Trữ File Chứng Từ - StockInOutDocument

## 📋 Tổng Quan

Bảng `StockInOutDocument` được thiết kế để lưu trữ thông tin các file chứng từ và tài liệu liên quan đến quá trình nhập xuất kho, được lưu trữ trên NAS thay vì trong database.

## 🎯 Mục Đích

- Lưu trữ các file chứng từ: Hóa đơn, Phiếu xuất kho, Phiếu nhập kho (PDF, Word, Excel)
- Lưu trữ các file đính kèm: Hợp đồng, Biên bản, Báo cáo, Chứng từ khác
- Hỗ trợ nhiều loại file: PDF, DOCX, XLSX, Images, ZIP, etc.
- Liên kết linh hoạt với nhiều entity khác nhau
- Quản lý metadata đầy đủ và phân loại rõ ràng

## 🗄️ Database Schema

### Bảng: StockInOutDocument

```sql
CREATE TABLE [dbo].[StockInOutDocument] (
    -- Primary Key
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    
    -- Foreign Keys - Liên kết với các entity
    [StockInOutMasterId] UNIQUEIDENTIFIER NULL,  -- Liên kết với phiếu nhập/xuất kho
    [BusinessPartnerId] UNIQUEIDENTIFIER NULL,    -- Liên kết với đối tác (nếu là chứng từ của đối tác)
    [PurchaseOrderId] UNIQUEIDENTIFIER NULL,      -- Liên kết với đơn đặt hàng (nếu có)
    [RelatedEntityType] NVARCHAR(50) NULL,         -- Loại entity liên quan (StockInOutMaster, BusinessPartner, PurchaseOrder, etc.)
    [RelatedEntityId] UNIQUEIDENTIFIER NULL,     -- ID của entity liên quan (generic foreign key)
    
    -- Document Classification
    [DocumentType] INT NOT NULL DEFAULT 1,       -- Loại chứng từ (Invoice, Receipt, Contract, etc.)
    [DocumentCategory] INT NULL,                 -- Danh mục (Financial, Legal, Administrative, etc.)
    [DocumentSubType] NVARCHAR(100) NULL,         -- Phân loại chi tiết (Hóa đơn VAT, Hóa đơn không VAT, etc.)
    
    -- File Information
    [FileName] NVARCHAR(255) NOT NULL,            -- Tên file gốc
    [DisplayName] NVARCHAR(255) NULL,             -- Tên hiển thị (có thể khác với FileName)
    [Description] NVARCHAR(1000) NULL,             -- Mô tả file
    
    -- Storage Information (tương tự StockInOutImage)
    [RelativePath] NVARCHAR(500) NOT NULL,         -- Đường dẫn tương đối trên NAS
    [FullPath] NVARCHAR(1000) NULL,               -- Đường dẫn đầy đủ (UNC path)
    [NASShareName] NVARCHAR(100) NULL DEFAULT 'ERP_Documents', -- Tên share trên NAS
    [StorageType] NVARCHAR(20) NULL DEFAULT 'NAS', -- NAS, Local, Cloud
    [StorageProvider] NVARCHAR(50) NULL,          -- Synology, QNAP, etc.
    
    -- File Metadata
    [FileExtension] NVARCHAR(10) NOT NULL,        -- .pdf, .docx, .xlsx, etc.
    [MimeType] NVARCHAR(100) NULL,                -- application/pdf, application/vnd.openxmlformats-officedocument.wordprocessingml.document, etc.
    [FileSize] BIGINT NULL,                       -- Kích thước file (bytes)
    [Checksum] NVARCHAR(64) NULL,                 -- MD5/SHA256 checksum để verify integrity
    [FileVersion] INT NULL DEFAULT 1,             -- Phiên bản file (nếu có nhiều version)
    
    -- Document Metadata
    [DocumentNumber] NVARCHAR(100) NULL,           -- Số chứng từ (nếu có)
    [DocumentDate] DATETIME NULL,                 -- Ngày chứng từ
    [IssueDate] DATETIME NULL,                    -- Ngày phát hành
    [ExpiryDate] DATETIME NULL,                   -- Ngày hết hạn (nếu có)
    [Amount] DECIMAL(18,2) NULL,                  -- Số tiền (nếu là chứng từ tài chính)
    [Currency] NVARCHAR(10) NULL,                 -- Loại tiền tệ
    
    -- Access & Security
    [IsPublic] BIT NULL DEFAULT 0,                -- Có công khai không
    [IsConfidential] BIT NULL DEFAULT 0,           -- Tài liệu mật
    [AccessLevel] INT NULL DEFAULT 0,             -- Mức độ truy cập (0=Public, 1=Internal, 2=Confidential, 3=Secret)
    [AccessUrl] NVARCHAR(1000) NULL,              -- URL truy cập (nếu có web server)
    [PasswordHash] NVARCHAR(255) NULL,             -- Hash mật khẩu (nếu file được bảo vệ)
    
    -- Status & Verification
    [FileExists] BIT NULL DEFAULT 1,              -- File có tồn tại trên NAS không
    [LastVerified] DATETIME NULL,                 -- Lần cuối kiểm tra file
    [IsVerified] BIT NULL DEFAULT 0,              -- Đã được xác minh chưa
    [VerifiedBy] UNIQUEIDENTIFIER NULL,           -- Người xác minh
    [VerifiedDate] DATETIME NULL,                  -- Ngày xác minh
    [MigrationStatus] NVARCHAR(20) NULL DEFAULT 'Pending', -- Pending, Migrated, Failed
    
    -- Thumbnail/Preview (cho file PDF, Word có thể tạo preview)
    [HasThumbnail] BIT NULL DEFAULT 0,
    [ThumbnailPath] NVARCHAR(500) NULL,
    [ThumbnailFileName] NVARCHAR(255) NULL,
    
    -- Tags & Search
    [Tags] NVARCHAR(500) NULL,                     -- Tags để tìm kiếm (comma-separated)
    [Keywords] NVARCHAR(1000) NULL,                -- Từ khóa tìm kiếm
    
    -- Audit Fields
    [CreateDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [CreateBy] UNIQUEIDENTIFIER NOT NULL,
    [ModifiedDate] DATETIME NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [IsDeleted] BIT NOT NULL DEFAULT 0,
    [DeletedDate] DATETIME NULL,
    [DeletedBy] UNIQUEIDENTIFIER NULL,
    
    -- Constraints
    CONSTRAINT [FK_StockInOutDocument_StockInOutMaster] 
        FOREIGN KEY ([StockInOutMasterId]) 
        REFERENCES [dbo].[StockInOutMaster]([Id]) 
        ON DELETE NO ACTION,
    
    CONSTRAINT [FK_StockInOutDocument_BusinessPartner] 
        FOREIGN KEY ([BusinessPartnerId]) 
        REFERENCES [dbo].[BusinessPartner]([Id]) 
        ON DELETE NO ACTION,
    
    CONSTRAINT [CHK_StockInOutDocument_FileExtension] 
        CHECK ([FileExtension] LIKE '.[a-z][a-z0-9][a-z0-9][a-z0-9]%'),
    
    CONSTRAINT [CHK_StockInOutDocument_FileSize] 
        CHECK ([FileSize] IS NULL OR [FileSize] >= 0)
);

-- Indexes
CREATE INDEX [IX_StockInOutDocument_StockInOutMasterId] 
    ON [dbo].[StockInOutDocument]([StockInOutMasterId]);
    
CREATE INDEX [IX_StockInOutDocument_BusinessPartnerId] 
    ON [dbo].[StockInOutDocument]([BusinessPartnerId]);
    
CREATE INDEX [IX_StockInOutDocument_RelatedEntity] 
    ON [dbo].[StockInOutDocument]([RelatedEntityType], [RelatedEntityId]);
    
CREATE INDEX [IX_StockInOutDocument_DocumentType] 
    ON [dbo].[StockInOutDocument]([DocumentType]);
    
CREATE INDEX [IX_StockInOutDocument_RelativePath] 
    ON [dbo].[StockInOutDocument]([RelativePath]);
    
CREATE INDEX [IX_StockInOutDocument_FileExists] 
    ON [dbo].[StockInOutDocument]([FileExists]);
    
CREATE INDEX [IX_StockInOutDocument_DocumentDate] 
    ON [dbo].[StockInOutDocument]([DocumentDate]);
    
CREATE INDEX [IX_StockInOutDocument_CreateDate] 
    ON [dbo].[StockInOutDocument]([CreateDate]);
    
CREATE INDEX [IX_StockInOutDocument_IsActive_IsDeleted] 
    ON [dbo].[StockInOutDocument]([IsActive], [IsDeleted]);
```

## 📊 Enum Definitions

### DocumentType Enum

```csharp
public enum DocumentTypeEnum
{
    /// <summary>
    /// Hóa đơn
    /// </summary>
    [Description("Hóa đơn")]
    Invoice = 1,
    
    /// <summary>
    /// Phiếu nhập kho
    /// </summary>
    [Description("Phiếu nhập kho")]
    StockInVoucher = 2,
    
    /// <summary>
    /// Phiếu xuất kho
    /// </summary>
    [Description("Phiếu xuất kho")]
    StockOutVoucher = 3,
    
    /// <summary>
    /// Hợp đồng
    /// </summary>
    [Description("Hợp đồng")]
    Contract = 4,
    
    /// <summary>
    /// Biên bản
    /// </summary>
    [Description("Biên bản")]
    Minutes = 5,
    
    /// <summary>
    /// Báo cáo
    /// </summary>
    [Description("Báo cáo")]
    Report = 6,
    
    /// <summary>
    /// Chứng từ thanh toán
    /// </summary>
    [Description("Chứng từ thanh toán")]
    PaymentVoucher = 7,
    
    /// <summary>
    /// Giấy tờ pháp lý
    /// </summary>
    [Description("Giấy tờ pháp lý")]
    LegalDocument = 8,
    
    /// <summary>
    /// Chứng từ khác
    /// </summary>
    [Description("Chứng từ khác")]
    Other = 99
}
```

### DocumentCategory Enum

```csharp
public enum DocumentCategoryEnum
{
    /// <summary>
    /// Tài chính
    /// </summary>
    [Description("Tài chính")]
    Financial = 1,
    
    /// <summary>
    /// Pháp lý
    /// </summary>
    [Description("Pháp lý")]
    Legal = 2,
    
    /// <summary>
    /// Hành chính
    /// </summary>
    [Description("Hành chính")]
    Administrative = 3,
    
    /// <summary>
    /// Kho hàng
    /// </summary>
    [Description("Kho hàng")]
    Inventory = 4,
    
    /// <summary>
    /// Mua hàng
    /// </summary>
    [Description("Mua hàng")]
    Procurement = 5,
    
    /// <summary>
    /// Bán hàng
    /// </summary>
    [Description("Bán hàng")]
    Sales = 6,
    
    /// <summary>
    /// Khác
    /// </summary>
    [Description("Khác")]
    Other = 99
}
```

### AccessLevel Enum

```csharp
public enum DocumentAccessLevelEnum
{
    /// <summary>
    /// Công khai
    /// </summary>
    [Description("Công khai")]
    Public = 0,
    
    /// <summary>
    /// Nội bộ
    /// </summary>
    [Description("Nội bộ")]
    Internal = 1,
    
    /// <summary>
    /// Mật
    /// </summary>
    [Description("Mật")]
    Confidential = 2,
    
    /// <summary>
    /// Tuyệt mật
    /// </summary>
    [Description("Tuyệt mật")]
    Secret = 3
}
```

## 🔗 Relationships

### 1. StockInOutMaster (1-N)
- Một phiếu nhập/xuất kho có thể có nhiều file chứng từ
- Foreign Key: `StockInOutMasterId`

### 2. BusinessPartner (1-N)
- Một đối tác có thể có nhiều file chứng từ
- Foreign Key: `BusinessPartnerId`

### 3. PurchaseOrder (1-N) - Tương lai
- Một đơn đặt hàng có thể có nhiều file chứng từ
- Foreign Key: `PurchaseOrderId`

### 4. Generic Relationship
- `RelatedEntityType` + `RelatedEntityId`: Cho phép liên kết với bất kỳ entity nào
- Ví dụ: Employee, Device, Warranty, etc.

## 📁 File Organization trên NAS

### Cấu trúc thư mục đề xuất:

```
\\NAS_SERVER\ERP_Documents\
├── StockInOut\
│   ├── 2025\
│   │   ├── 01\
│   │   │   ├── Invoice\
│   │   │   │   └── Invoice_{StockInOutMasterId}_{DocumentId}_{Timestamp}.pdf
│   │   │   ├── Contract\
│   │   │   │   └── Contract_{BusinessPartnerId}_{DocumentId}_{Timestamp}.pdf
│   │   │   └── Other\
│   │   └── 02\
│   └── 2024\
├── BusinessPartner\
│   ├── {BusinessPartnerId}\
│   │   ├── Contracts\
│   │   ├── Invoices\
│   │   └── Legal\
└── PurchaseOrder\
    └── {PurchaseOrderId}\
        └── PO_{PurchaseOrderId}_{DocumentId}_{Timestamp}.pdf
```

### RelativePath Format:

```
StockInOut\{Year}\{Month}\{DocumentType}\{FileName}
BusinessPartner\{BusinessPartnerId}\{DocumentCategory}\{FileName}
PurchaseOrder\{PurchaseOrderId}\{FileName}
```

## 💡 Use Cases

### 1. Lưu Hóa Đơn Kèm Phiếu Nhập Kho
```csharp
var document = new StockInOutDocument
{
    Id = Guid.NewGuid(),
    StockInOutMasterId = stockInOutMasterId,
    DocumentType = (int)DocumentTypeEnum.Invoice,
    DocumentCategory = (int)DocumentCategoryEnum.Financial,
    FileName = "Invoice_2025_001.pdf",
    DisplayName = "Hóa đơn số 2025/001",
    DocumentNumber = "2025/001",
    DocumentDate = DateTime.Now,
    RelativePath = $"StockInOut\\2025\\{DateTime.Now.Month:D2}\\Invoice\\Invoice_{stockInOutMasterId}_{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}.pdf",
    FileExtension = ".pdf",
    MimeType = "application/pdf",
    FileSize = fileSize,
    Checksum = checksum,
    CreateBy = currentUserId,
    CreateDate = DateTime.Now
};
```

### 2. Lưu Hợp Đồng của Đối Tác
```csharp
var document = new StockInOutDocument
{
    Id = Guid.NewGuid(),
    BusinessPartnerId = businessPartnerId,
    DocumentType = (int)DocumentTypeEnum.Contract,
    DocumentCategory = (int)DocumentCategoryEnum.Legal,
    FileName = "Contract_2025_ABC.pdf",
    DisplayName = "Hợp đồng mua hàng 2025",
    RelativePath = $"BusinessPartner\\{businessPartnerId}\\Contracts\\Contract_{businessPartnerId}_{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}.pdf",
    IssueDate = DateTime.Now,
    ExpiryDate = DateTime.Now.AddYears(1),
    IsConfidential = true,
    AccessLevel = (int)DocumentAccessLevelEnum.Confidential
};
```

### 3. Lưu Chứng Từ Thanh Toán
```csharp
var document = new StockInOutDocument
{
    Id = Guid.NewGuid(),
    StockInOutMasterId = stockInOutMasterId,
    DocumentType = (int)DocumentTypeEnum.PaymentVoucher,
    DocumentCategory = (int)DocumentCategoryEnum.Financial,
    FileName = "Payment_Receipt_001.pdf",
    DocumentNumber = "PT001",
    DocumentDate = DateTime.Now,
    Amount = 1000000,
    Currency = "VND",
    RelativePath = $"StockInOut\\2025\\{DateTime.Now.Month:D2}\\Payment\\Payment_{stockInOutMasterId}_{Guid.NewGuid()}_{DateTime.Now:yyyyMMddHHmmss}.pdf"
};
```

## 🔍 Query Examples

### 1. Lấy tất cả chứng từ của một phiếu nhập/xuất
```sql
SELECT * 
FROM StockInOutDocument 
WHERE StockInOutMasterId = @StockInOutMasterId 
  AND IsActive = 1 
  AND IsDeleted = 0
ORDER BY DocumentDate DESC, CreateDate DESC;
```

### 2. Tìm kiếm chứng từ theo loại và khoảng thời gian
```sql
SELECT * 
FROM StockInOutDocument 
WHERE DocumentType = @DocumentType
  AND DocumentDate BETWEEN @FromDate AND @ToDate
  AND IsActive = 1 
  AND IsDeleted = 0;
```

### 3. Lấy các file chứng từ cần xác minh
```sql
SELECT * 
FROM StockInOutDocument 
WHERE IsVerified = 0 
  AND FileExists = 1
  AND IsActive = 1 
  AND IsDeleted = 0
ORDER BY CreateDate DESC;
```

## ✅ Lợi Ích

1. **Tách biệt dữ liệu**: File lưu trên NAS, metadata lưu trong database
2. **Linh hoạt**: Có thể liên kết với nhiều entity khác nhau
3. **Phân loại rõ ràng**: DocumentType, DocumentCategory giúp quản lý dễ dàng
4. **Bảo mật**: AccessLevel, IsConfidential kiểm soát truy cập
5. **Tìm kiếm**: Tags, Keywords hỗ trợ tìm kiếm nhanh
6. **Audit đầy đủ**: Track được ai tạo, sửa, xóa
7. **Versioning**: FileVersion hỗ trợ quản lý nhiều phiên bản
8. **Verification**: Có thể xác minh tính toàn vẹn file

## 🔄 Migration từ StockInOutImage (nếu cần)

Nếu muốn chuyển một số hình ảnh sang dạng document:

```sql
-- Tạo bản ghi Document từ Image (nếu hình ảnh là chứng từ)
INSERT INTO StockInOutDocument (
    Id, StockInOutMasterId, DocumentType, DocumentCategory,
    FileName, RelativePath, FullPath, StorageType,
    FileExtension, MimeType, FileSize, Checksum,
    CreateDate, CreateBy, IsActive
)
SELECT 
    NEWID(),
    StockInOutMasterId,
    99, -- Other
    4,  -- Inventory
    FileName,
    RelativePath,
    FullPath,
    StorageType,
    FileExtension,
    MimeType,
    FileSize,
    Checksum,
    CreateDate,
    CreateBy,
    1
FROM StockInOutImage
WHERE -- Điều kiện để chuyển (ví dụ: file có kích thước lớn, là PDF, etc.)
```

## 📝 Notes

- Bảng này tương tự `StockInOutImage` nhưng tập trung vào **chứng từ và tài liệu** thay vì hình ảnh
- Có thể mở rộng thêm các trường nếu cần (OCR text, Digital signature, etc.)
- Nên tạo service layer tương tự `ImageStorageService` để quản lý document storage
- Có thể tích hợp với hệ thống OCR để extract text từ PDF/Image documents

