# Kiến Trúc Refactor Lưu Trữ Hình Ảnh/File - VNS ERP 2025

## 📋 Mục Lục

1. [Tổng Quan](#tổng-quan)
2. [Kiến Trúc Hiện Tại](#kiến-trúc-hiện-tại)
3. [Kiến Trúc Đề Xuất](#kiến-trúc-đề-xuất)
4. [Database Schema Changes](#database-schema-changes)
5. [Service Layer Architecture](#service-layer-architecture)
6. [Configuration](#configuration)
7. [Migration Plan](#migration-plan)
8. [Implementation Steps](#implementation-steps)
9. [Best Practices](#best-practices)
10. [Testing Strategy](#testing-strategy)
11. [Rollback Plan](#rollback-plan)

---

## 📌 Tổng Quan

### Mục Tiêu

Refactor hệ thống lưu trữ hình ảnh/file từ **lưu trực tiếp trong database (VarBinary(MAX))** sang **lưu trên NAS Synology với đường dẫn trong database**.

### Lợi Ích

- ✅ **Performance**: Giảm kích thước database, tăng tốc query
- ✅ **Scalability**: Dễ dàng mở rộng storage mà không ảnh hưởng database
- ✅ **Cost**: Chi phí thấp, không phụ thuộc cloud bandwidth
- ✅ **Maintenance**: Dễ dàng backup, quản lý file riêng biệt
- ✅ **Security**: Kiểm soát truy cập tốt hơn với NAS permissions

### Phạm Vi

Các bảng cần refactor:
- `ProductImage` (ImageData → NAS Storage)
- `ProductService` (ThumbnailImage → NAS Storage)
- `ProductVariant` (ThumbnailImage → NAS Storage)
- `StockInOutImage` (ImageData → NAS Storage)
- `Company` (Logo → NAS Storage)
- `BusinessPartnerContact` (Avatar → NAS Storage)

---

## 🏗️ Kiến Trúc Hiện Tại

### Vấn Đề

```
┌─────────────────┐
│  SQL Server     │
│                 │
│  ProductImage   │
│  ├─ ImageData   │ ← VarBinary(MAX) - RẤT LỚN
│  └─ ImagePath   │ ← Đường dẫn local (không dùng)
│                 │
│  StockInOutImage│
│  └─ ImageData   │ ← VarBinary(MAX) - RẤT LỚN
│                 │
│  Company        │
│  └─ Logo        │ ← VarBinary(MAX)
└─────────────────┘
```

**Vấn đề:**
- Database size tăng nhanh
- Query chậm khi load entities có image
- Backup/restore tốn thời gian
- Khó scale

---

## 🎯 Kiến Trúc Đề Xuất

### Kiến Trúc Mới

```
┌─────────────────┐
│  Application    │
│   Server(s)     │
└────────┬────────┘
         │
         │ IImageStorageService
         │
┌────────▼────────┐      ┌─────────────────┐
│  Image Storage  │      │   SQL Server    │
│     Service     │      │                 │
│                 │      │  ProductImage   │
│  ┌──────────┐   │      │  ├─ RelativePath│
│  │   NAS    │   │◄─────┤  ├─ FileName    │
│  │ Synology │   │      │  ├─ FileSize    │
│  │          │   │      │  └─ Checksum    │
│  └──────────┘   │      │                 │
│                 │      │  StockInOutImage│
│  Storage Layer  │      │  └─ RelativePath│
└─────────────────┘      └─────────────────┘
```

### Flow Diagram

```
Upload Image
    │
    ├─► Validate (size, type, format)
    │
    ├─► Generate Unique File Name
    │   Format: {EntityType}_{EntityId}_{ImageId}_{Timestamp}.{ext}
    │
    ├─► Save to NAS
    │   Path: \\NAS_SERVER\ERP_Images\{Category}\{Year}\{Month}\{FileName}
    │
    ├─► Generate Thumbnail (optional)
    │   Save: {FileName}_thumb.jpg
    │
    ├─► Calculate Checksum (MD5/SHA256)
    │
    └─► Save Metadata to Database
        - RelativePath
        - FileName
        - FileSize
        - Checksum
        - StorageType = 'NAS'
```

---

## 🗄️ Database Schema Changes

### 1. ProductImage Table

```sql
-- Bước 1: Thêm các cột mới
ALTER TABLE [dbo].[ProductImage]
ADD 
    -- File Information
    [FileName] NVARCHAR(255) NULL,
    [RelativePath] NVARCHAR(500) NULL,  -- Relative to NAS root
    [FullPath] NVARCHAR(1000) NULL,      -- Full UNC path
    [NASShareName] NVARCHAR(100) NULL DEFAULT 'ERP_Images',
    
    -- Storage Information
    [StorageType] NVARCHAR(20) NULL DEFAULT 'NAS', -- NAS, Local, Cloud
    [StorageProvider] NVARCHAR(50) NULL, -- Synology, QNAP, etc.
    
    -- File Metadata
    [FileExtension] NVARCHAR(10) NULL,
    [MimeType] NVARCHAR(100) NULL,
    [Checksum] NVARCHAR(64) NULL, -- MD5/SHA256
    
    -- Thumbnail Information
    [HasThumbnail] BIT NULL DEFAULT 0,
    [ThumbnailPath] NVARCHAR(500) NULL,
    [ThumbnailFileName] NVARCHAR(255) NULL,
    
    -- Access Information
    [IsPublic] BIT NULL DEFAULT 0,
    [AccessUrl] NVARCHAR(1000) NULL, -- If using web server
    
    -- Status
    [FileExists] BIT NULL DEFAULT 1,
    [LastVerified] DATETIME NULL,
    [MigrationStatus] NVARCHAR(20) NULL DEFAULT 'Pending'; -- Pending, Migrated, Failed

-- Bước 2: Tạo Indexes
CREATE INDEX [IX_ProductImage_RelativePath] ON [dbo].[ProductImage]([RelativePath]);
CREATE INDEX [IX_ProductImage_FileExists] ON [dbo].[ProductImage]([FileExists]);
CREATE INDEX [IX_ProductImage_StorageType] ON [dbo].[ProductImage]([StorageType]);
CREATE INDEX [IX_ProductImage_MigrationStatus] ON [dbo].[ProductImage]([MigrationStatus]);

-- Bước 3: Thêm Comments
EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Đường dẫn tương đối từ root của NAS share', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'RelativePath';

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Loại storage: NAS, Local, Cloud', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'StorageType';
```

### 2. StockInOutImage Table

```sql
ALTER TABLE [dbo].[StockInOutImage]
ADD 
    [FileName] NVARCHAR(255) NULL,
    [RelativePath] NVARCHAR(500) NULL,
    [FullPath] NVARCHAR(1000) NULL,
    [StorageType] NVARCHAR(20) NULL DEFAULT 'NAS',
    [FileSize] BIGINT NULL,
    [FileExtension] NVARCHAR(10) NULL,
    [MimeType] NVARCHAR(100) NULL,
    [Checksum] NVARCHAR(64) NULL,
    [FileExists] BIT NULL DEFAULT 1,
    [LastVerified] DATETIME NULL,
    [MigrationStatus] NVARCHAR(20) NULL DEFAULT 'Pending';

CREATE INDEX [IX_StockInOutImage_RelativePath] ON [dbo].[StockInOutImage]([RelativePath]);
CREATE INDEX [IX_StockInOutImage_FileExists] ON [dbo].[StockInOutImage]([FileExists]);
```

### 3. Company Table

```sql
ALTER TABLE [dbo].[Company]
ADD 
    [LogoFileName] NVARCHAR(255) NULL,
    [LogoRelativePath] NVARCHAR(500) NULL,
    [LogoFullPath] NVARCHAR(1000) NULL,
    [LogoStorageType] NVARCHAR(20) NULL DEFAULT 'NAS',
    [LogoFileSize] BIGINT NULL,
    [LogoChecksum] NVARCHAR(64) NULL;
```

### 4. ProductService & ProductVariant

```sql
-- ProductService
ALTER TABLE [dbo].[ProductService]
ADD 
    [ThumbnailFileName] NVARCHAR(255) NULL,
    [ThumbnailRelativePath] NVARCHAR(500) NULL,
    [ThumbnailFullPath] NVARCHAR(1000) NULL,
    [ThumbnailStorageType] NVARCHAR(20) NULL DEFAULT 'NAS',
    [ThumbnailFileSize] BIGINT NULL,
    [ThumbnailChecksum] NVARCHAR(64) NULL;

-- ProductVariant
ALTER TABLE [dbo].[ProductVariant]
ADD 
    [ThumbnailFileName] NVARCHAR(255) NULL,
    [ThumbnailRelativePath] NVARCHAR(500) NULL,
    [ThumbnailFullPath] NVARCHAR(1000) NULL,
    [ThumbnailStorageType] NVARCHAR(20) NULL DEFAULT 'NAS',
    [ThumbnailFileSize] BIGINT NULL,
    [ThumbnailChecksum] NVARCHAR(64) NULL;
```

### 5. Migration Tracking Table (Optional)

```sql
CREATE TABLE [dbo].[ImageMigrationLog]
(
    [Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY DEFAULT NEWID(),
    [TableName] NVARCHAR(100) NOT NULL,
    [RecordId] UNIQUEIDENTIFIER NOT NULL,
    [OldStorageType] NVARCHAR(20) NULL, -- Database
    [NewStorageType] NVARCHAR(20) NOT NULL, -- NAS
    [SourcePath] NVARCHAR(1000) NULL,
    [DestinationPath] NVARCHAR(1000) NOT NULL,
    [FileSize] BIGINT NULL,
    [MigrationDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [MigrationStatus] NVARCHAR(20) NOT NULL, -- Success, Failed, Pending
    [ErrorMessage] NVARCHAR(MAX) NULL,
    [MigratedBy] NVARCHAR(100) NULL
);

CREATE INDEX [IX_ImageMigrationLog_TableName_RecordId] 
ON [dbo].[ImageMigrationLog]([TableName], [RecordId]);
CREATE INDEX [IX_ImageMigrationLog_MigrationStatus] 
ON [dbo].[ImageMigrationLog]([MigrationStatus]);
```

---

## 🔧 Service Layer Architecture

### 1. Interface Definition

```csharp
// Bll/Common/ImageStorage/IImageStorageService.cs

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Interface cho Image Storage Service
    /// Hỗ trợ nhiều loại storage: NAS, Local, Cloud
    /// </summary>
    public interface IImageStorageService
    {
        /// <summary>
        /// Lưu hình ảnh vào storage
        /// </summary>
        Task<ImageStorageResult> SaveImageAsync(
            byte[] imageData, 
            string fileName, 
            ImageCategory category,
            Guid? entityId = null,
            bool generateThumbnail = false);

        /// <summary>
        /// Lấy hình ảnh từ storage
        /// </summary>
        Task<byte[]> GetImageAsync(string relativePath);

        /// <summary>
        /// Lấy thumbnail
        /// </summary>
        Task<byte[]> GetThumbnailAsync(string relativePath);

        /// <summary>
        /// Xóa hình ảnh
        /// </summary>
        Task<bool> DeleteImageAsync(string relativePath);

        /// <summary>
        /// Kiểm tra file tồn tại
        /// </summary>
        Task<bool> ImageExistsAsync(string relativePath);

        /// <summary>
        /// Verify file integrity
        /// </summary>
        Task<bool> VerifyImageAsync(string relativePath, string checksum);

        /// <summary>
        /// Generate thumbnail từ original image
        /// </summary>
        Task<string> GenerateThumbnailAsync(string originalRelativePath, int width = 200, int height = 200);
    }

    /// <summary>
    /// Kết quả lưu trữ hình ảnh
    /// </summary>
    public class ImageStorageResult
    {
        public bool Success { get; set; }
        public string RelativePath { get; set; }
        public string FullPath { get; set; }
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string Checksum { get; set; }
        public string ThumbnailRelativePath { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Danh mục hình ảnh
    /// </summary>
    public enum ImageCategory
    {
        Product,
        ProductVariant,
        StockInOut,
        Company,
        Avatar,
        Temp
    }
}
```

### 2. NAS Implementation

```csharp
// Bll/Common/ImageStorage/NASImageStorageService.cs

namespace Bll.Common.ImageStorage
{
    /// <summary>
    /// Implementation cho NAS Storage (Synology)
    /// </summary>
    public class NASImageStorageService : IImageStorageService
    {
        private readonly string _nasBasePath;
        private readonly string _nasShareName;
        private readonly ILogger _logger;
        private readonly ImageStorageConfiguration _config;

        public NASImageStorageService(ImageStorageConfiguration config, ILogger logger)
        {
            _config = config;
            _logger = logger;
            _nasBasePath = config.NASBasePath; // \\192.168.1.100\ERP_Images
            _nasShareName = config.NASShareName;
        }

        public async Task<ImageStorageResult> SaveImageAsync(
            byte[] imageData, 
            string fileName, 
            ImageCategory category,
            Guid? entityId = null,
            bool generateThumbnail = false)
        {
            try
            {
                // 1. Validate
                ValidateImage(imageData, fileName);

                // 2. Generate file path
                var relativePath = GenerateRelativePath(category, fileName, entityId);
                var fullPath = Path.Combine(_nasBasePath, relativePath);

                // 3. Ensure directory exists
                var directory = Path.GetDirectoryName(fullPath);
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                // 4. Save file
                await File.WriteAllBytesAsync(fullPath, imageData);

                // 5. Calculate checksum
                var checksum = CalculateChecksum(imageData);

                // 6. Generate thumbnail if requested
                string thumbnailPath = null;
                if (generateThumbnail)
                {
                    thumbnailPath = await GenerateThumbnailAsync(relativePath);
                }

                return new ImageStorageResult
                {
                    Success = true,
                    RelativePath = relativePath,
                    FullPath = fullPath,
                    FileName = fileName,
                    FileSize = imageData.Length,
                    Checksum = checksum,
                    ThumbnailRelativePath = thumbnailPath
                };
            }
            catch (Exception ex)
            {
                _logger.Error($"SaveImageAsync failed: {ex.Message}", ex);
                return new ImageStorageResult
                {
                    Success = false,
                    ErrorMessage = ex.Message
                };
            }
        }

        private string GenerateRelativePath(ImageCategory category, string fileName, Guid? entityId)
        {
            var year = DateTime.Now.Year;
            var month = DateTime.Now.Month.ToString("00");

            return category switch
            {
                ImageCategory.Product => $"Products/{entityId}/{year}/{month}/{fileName}",
                ImageCategory.StockInOut => $"StockInOut/{year}/{month}/{fileName}",
                ImageCategory.Company => $"Company/{fileName}",
                ImageCategory.Avatar => $"Avatars/{fileName}",
                _ => $"Temp/{fileName}"
            };
        }

        // Implement other methods...
    }
}
```

### 3. Configuration Class

```csharp
// Bll/Common/ImageStorage/ImageStorageConfiguration.cs

namespace Bll.Common.ImageStorage
{
    public class ImageStorageConfiguration
    {
        public string StorageType { get; set; } // NAS, Local, Cloud
        public string NASServerName { get; set; }
        public string NASShareName { get; set; }
        public string NASBasePath { get; set; }
        public string NASUsername { get; set; }
        public string NASPassword { get; set; }
        public string NASProtocol { get; set; } // SMB, NFS
        
        public bool EnableThumbnailGeneration { get; set; }
        public int ThumbnailWidth { get; set; }
        public int ThumbnailHeight { get; set; }
        
        public bool EnableImageCompression { get; set; }
        public int ImageQuality { get; set; }
        
        public long MaxFileSize { get; set; }
        public string[] AllowedExtensions { get; set; }
    }
}
```

---

## ⚙️ Configuration

### App.config

```xml
<appSettings>
    <!-- Storage Type: NAS, Local, Cloud -->
    <add key="ImageStorage.StorageType" value="NAS" />
    
    <!-- NAS Configuration -->
    <add key="ImageStorage.NAS.ServerName" value="\\192.168.1.100" />
    <add key="ImageStorage.NAS.ShareName" value="ERP_Images" />
    <add key="ImageStorage.NAS.BasePath" value="\\192.168.1.100\ERP_Images" />
    <add key="ImageStorage.NAS.Username" value="erp_user" />
    <add key="ImageStorage.NAS.Password" value="[Encrypted]" />
    <add key="ImageStorage.NAS.Protocol" value="SMB" />
    <add key="ImageStorage.NAS.ConnectionTimeout" value="30" />
    <add key="ImageStorage.NAS.RetryAttempts" value="3" />
    
    <!-- Path Configuration -->
    <add key="ImageStorage.Path.Products" value="Products" />
    <add key="ImageStorage.Path.StockInOut" value="StockInOut" />
    <add key="ImageStorage.Path.Company" value="Company" />
    <add key="ImageStorage.Path.Avatars" value="Avatars" />
    <add key="ImageStorage.Path.Temp" value="Temp" />
    
    <!-- Thumbnail Configuration -->
    <add key="ImageStorage.Thumbnail.Enable" value="true" />
    <add key="ImageStorage.Thumbnail.Width" value="200" />
    <add key="ImageStorage.Thumbnail.Height" value="200" />
    <add key="ImageStorage.Thumbnail.Quality" value="80" />
    
    <!-- Image Processing -->
    <add key="ImageStorage.Compression.Enable" value="true" />
    <add key="ImageStorage.Compression.Quality" value="80" />
    <add key="ImageStorage.MaxFileSize" value="10485760" /> <!-- 10MB -->
    <add key="ImageStorage.AllowedExtensions" value="jpg,jpeg,png,gif,bmp,webp" />
    
    <!-- File Management -->
    <add key="ImageStorage.Verification.Enable" value="true" />
    <add key="ImageStorage.Verification.IntervalHours" value="24" />
    <add key="ImageStorage.Cleanup.Enable" value="true" />
    <add key="ImageStorage.Cleanup.OrphanedFileRetentionDays" value="30" />
    
    <!-- Performance -->
    <add key="ImageStorage.Cache.Enable" value="true" />
    <add key="ImageStorage.Cache.SizeMB" value="500" />
    <add key="ImageStorage.Async.Enable" value="true" />
    <add key="ImageStorage.Async.MaxConcurrent" value="10" />
</appSettings>
```

---

## 📦 Migration Plan

### Phase 1: Preparation (Week 1)

1. **Setup NAS Synology**
   - Cài đặt và cấu hình NAS
   - Tạo share folder `ERP_Images`
   - Tạo user `erp_user` với quyền Read/Write
   - Cấu hình RAID (RAID 5 hoặc RAID 6)
   - Enable snapshot (7 days retention)
   - Test kết nối từ application server

2. **Database Schema Update**
   - Chạy migration scripts để thêm các cột mới
   - Tạo indexes
   - Backup database trước khi migration

3. **Code Preparation**
   - Tạo `IImageStorageService` interface
   - Implement `NASImageStorageService`
   - Update configuration

### Phase 2: Implementation (Week 2-3)

1. **Service Layer**
   - Implement `NASImageStorageService`
   - Create `ImageStorageFactory` (Factory pattern)
   - Update `ImageService` để sử dụng storage service
   - Unit tests

2. **Repository Layer**
   - Update `ProductImageRepository`
   - Update `StockInOutImageRepository`
   - Update các repository khác

3. **BLL Layer**
   - Update `ProductImageBll`
   - Update `StockInOutImageBll`
   - Update các BLL khác

### Phase 3: Migration Data (Week 4)

1. **Migration Script**
   ```csharp
   // Scripts/MigrateImagesToNAS.cs
   public class ImageMigrationService
   {
       public async Task MigrateProductImagesAsync()
       {
           // 1. Get all images with ImageData
           // 2. For each image:
           //    - Save ImageData to NAS
           //    - Update RelativePath, FileName, etc.
           //    - Set MigrationStatus = 'Migrated'
           //    - Log to ImageMigrationLog
       }
   }
   ```

2. **Migration Process**
   - Run migration script in batches (100 images/batch)
   - Verify files exist on NAS
   - Update database records
   - Monitor progress

3. **Verification**
   - Verify all files migrated
   - Check file integrity (checksum)
   - Test image loading

### Phase 4: Cleanup (Week 5)

1. **Remove ImageData Columns**
   ```sql
   -- After verification, remove ImageData columns
   ALTER TABLE [dbo].[ProductImage] DROP COLUMN [ImageData];
   ALTER TABLE [dbo].[StockInOutImage] DROP COLUMN [ImageData];
   ALTER TABLE [dbo].[Company] DROP COLUMN [Logo];
   -- etc.
   ```

2. **Optimize Database**
   - Rebuild indexes
   - Update statistics
   - Shrink database if needed

3. **Update Application**
   - Remove old code references to ImageData
   - Update all image loading logic
   - Update UI components

---

## 🚀 Implementation Steps

### Step 1: Create Storage Service Interface

**File**: `Bll/Common/ImageStorage/IImageStorageService.cs`

```csharp
// Interface definition (see Service Layer Architecture section)
```

### Step 2: Implement NAS Storage Service

**File**: `Bll/Common/ImageStorage/NASImageStorageService.cs`

```csharp
// Implementation (see Service Layer Architecture section)
```

### Step 3: Create Factory Pattern

**File**: `Bll/Common/ImageStorage/ImageStorageFactory.cs`

```csharp
public static class ImageStorageFactory
{
    public static IImageStorageService Create(ImageStorageConfiguration config, ILogger logger)
    {
        return config.StorageType.ToUpper() switch
        {
            "NAS" => new NASImageStorageService(config, logger),
            "LOCAL" => new LocalImageStorageService(config, logger),
            "CLOUD" => new CloudImageStorageService(config, logger),
            _ => throw new NotSupportedException($"Storage type {config.StorageType} not supported")
        };
    }
}
```

### Step 4: Update ProductImageBll

**File**: `Bll/MasterData/ProductServiceBll/ProductImageBll.cs`

```csharp
public class ProductImageBll
{
    private readonly IImageStorageService _imageStorage;
    
    public ProductImageBll()
    {
        var config = LoadImageStorageConfiguration();
        _imageStorage = ImageStorageFactory.Create(config, _logger);
    }
    
    public async Task<ProductImage> SaveImageAsync(
        Guid productId, 
        string imageFilePath, 
        bool isPrimary = false,
        string caption = null,
        string altText = null)
    {
        // 1. Read image file
        var imageData = await File.ReadAllBytesAsync(imageFilePath);
        
        // 2. Generate file name
        var fileName = GenerateFileName(productId, Path.GetExtension(imageFilePath));
        
        // 3. Save to NAS
        var storageResult = await _imageStorage.SaveImageAsync(
            imageData, 
            fileName, 
            ImageCategory.Product,
            productId,
            generateThumbnail: true);
        
        if (!storageResult.Success)
        {
            throw new Exception($"Failed to save image: {storageResult.ErrorMessage}");
        }
        
        // 4. Get image metadata
        var imageInfo = GetImageInfo(imageFilePath);
        
        // 5. Create ProductImage entity (NO ImageData)
        var productImage = new ProductImage
        {
            Id = Guid.NewGuid(),
            ProductId = productId,
            ImagePath = storageResult.RelativePath, // Keep for backward compatibility
            FileName = storageResult.FileName,
            RelativePath = storageResult.RelativePath,
            FullPath = storageResult.FullPath,
            StorageType = "NAS",
            FileSize = storageResult.FileSize,
            FileExtension = Path.GetExtension(fileName).TrimStart('.'),
            Checksum = storageResult.Checksum,
            HasThumbnail = !string.IsNullOrEmpty(storageResult.ThumbnailRelativePath),
            ThumbnailPath = storageResult.ThumbnailRelativePath,
            ImageType = Path.GetExtension(fileName).TrimStart('.').ToLower(),
            ImageSize = storageResult.FileSize,
            ImageWidth = imageInfo.Width,
            ImageHeight = imageInfo.Height,
            Caption = caption,
            AltText = altText,
            IsActive = true,
            CreatedDate = DateTime.Now,
            FileExists = true
        };
        
        // 6. Save to database
        GetDataAccess().SaveOrUpdate(productImage);
        
        return productImage;
    }
    
    public async Task<byte[]> GetImageAsync(Guid imageId)
    {
        var productImage = GetDataAccess().GetById(imageId);
        if (productImage == null)
            throw new NotFoundException($"Image not found: {imageId}");
        
        // Load from NAS
        return await _imageStorage.GetImageAsync(productImage.RelativePath);
    }
}
```

### Step 5: Update StockInOutImageBll

**File**: `Bll/Inventory/InventoryManagement/StockInOutImageBll.cs`

```csharp
// Similar pattern to ProductImageBll
```

---

## ✅ Best Practices

### 1. File Naming Convention

```
Format: {EntityType}_{EntityId}_{ImageId}_{Timestamp}.{Extension}

Examples:
- Product_123e4567-e89b-12d3-a456-426614174000_abc123_20250101120000.jpg
- StockInOut_456e7890-e89b-12d3-a456-426614174000_def456_20250101120000.png
- Company_789e0123-e89b-12d3-a456-426614174000_logo.jpg
```

### 2. Directory Structure

```
\\NAS_SERVER\ERP_Images\
├── Products\
│   ├── {ProductId}\
│   │   ├── {Year}\
│   │   │   ├── {Month}\
│   │   │   │   ├── {FileName}.jpg
│   │   │   │   └── {FileName}_thumb.jpg
├── StockInOut\
│   ├── {Year}\
│   │   ├── {Month}\
│   │   │   └── {StockInOutMasterId}_{ImageId}_{Timestamp}.jpg
├── Company\
│   └── {CompanyId}_logo.jpg
├── Avatars\
│   └── {UserId}_avatar.jpg
└── Temp\
    └── (staging area for uploads)
```

### 3. Error Handling

```csharp
try
{
    // Save to NAS
}
catch (UnauthorizedAccessException ex)
{
    // Handle NAS permission error
    _logger.Error("NAS access denied", ex);
    throw new StorageException("Cannot access NAS storage", ex);
}
catch (IOException ex)
{
    // Handle network/file system error
    _logger.Error("NAS I/O error", ex);
    throw new StorageException("Storage I/O error", ex);
}
```

### 4. Caching Strategy

```csharp
// Cache frequently accessed images
private readonly MemoryCache _imageCache = new MemoryCache(new MemoryCacheOptions
{
    SizeLimit = 500 * 1024 * 1024, // 500MB
    CompactionPercentage = 0.25
});

public async Task<byte[]> GetImageAsync(string relativePath)
{
    // Check cache first
    if (_imageCache.TryGetValue(relativePath, out byte[] cachedImage))
    {
        return cachedImage;
    }
    
    // Load from NAS
    var image = await _imageStorage.GetImageAsync(relativePath);
    
    // Cache for 1 hour
    _imageCache.Set(relativePath, image, TimeSpan.FromHours(1));
    
    return image;
}
```

### 5. Security

- ✅ Validate file extensions
- ✅ Scan for malware (optional)
- ✅ Limit file size
- ✅ Sanitize file names
- ✅ Use checksum for integrity
- ✅ Encrypt NAS credentials in config

---

## 🧪 Testing Strategy

### Unit Tests

```csharp
[TestClass]
public class NASImageStorageServiceTests
{
    [TestMethod]
    public async Task SaveImageAsync_ValidImage_ReturnsSuccess()
    {
        // Arrange
        var service = new NASImageStorageService(config, logger);
        var imageData = GetTestImageBytes();
        
        // Act
        var result = await service.SaveImageAsync(
            imageData, 
            "test.jpg", 
            ImageCategory.Product);
        
        // Assert
        Assert.IsTrue(result.Success);
        Assert.IsNotNull(result.RelativePath);
        Assert.IsTrue(File.Exists(result.FullPath));
    }
    
    [TestMethod]
    public async Task GetImageAsync_ExistingFile_ReturnsImageData()
    {
        // Test image retrieval
    }
    
    [TestMethod]
    public async Task DeleteImageAsync_ExistingFile_DeletesSuccessfully()
    {
        // Test image deletion
    }
}
```

### Integration Tests

- Test NAS connectivity
- Test file operations (save, read, delete)
- Test thumbnail generation
- Test error handling (network failure, permission denied)

### Migration Tests

- Test migration script
- Verify all images migrated
- Verify file integrity
- Test rollback procedure

---

## 🔄 Rollback Plan

### If Migration Fails

1. **Keep ImageData Columns**
   - Don't drop `ImageData` columns until migration verified
   - Keep both old and new paths during transition

2. **Rollback Script**
   ```sql
   -- Revert to ImageData if needed
   UPDATE ProductImage 
   SET MigrationStatus = 'RolledBack'
   WHERE MigrationStatus = 'Migrated';
   ```

3. **Code Rollback**
   - Keep old code that uses `ImageData`
   - Use feature flag to switch between old/new implementation

### Feature Flag

```csharp
public class ImageService
{
    private readonly bool _useNASStorage;
    
    public ImageService()
    {
        _useNASStorage = bool.Parse(ConfigurationManager.AppSettings["UseNASStorage"] ?? "false");
    }
    
    public async Task<byte[]> GetImageAsync(Guid imageId)
    {
        if (_useNASStorage)
        {
            // Use NAS storage
            return await GetImageFromNASAsync(imageId);
        }
        else
        {
            // Use database storage (old way)
            return await GetImageFromDatabaseAsync(imageId);
        }
    }
}
```

---

## 📊 Monitoring & Maintenance

### Metrics to Monitor

- Storage usage on NAS
- File count by category
- Average file size
- Upload/download performance
- Error rates
- Cache hit rates

### Maintenance Tasks

- **Daily**: Monitor storage usage
- **Weekly**: Verify file integrity (checksum)
- **Monthly**: Cleanup orphaned files
- **Quarterly**: Review and optimize directory structure

### Alerts

- Storage usage > 80%
- NAS connectivity issues
- High error rates
- File verification failures

---

## 📝 Checklist

### Pre-Migration
- [ ] NAS Synology setup and configured
- [ ] Database schema updated
- [ ] Service layer implemented
- [ ] Configuration updated
- [ ] Unit tests written
- [ ] Integration tests passed

### Migration
- [ ] Backup database
- [ ] Run migration script
- [ ] Verify all files migrated
- [ ] Test image loading
- [ ] Update application code

### Post-Migration
- [ ] Remove ImageData columns
- [ ] Optimize database
- [ ] Update documentation
- [ ] Train team on new architecture
- [ ] Monitor for issues

---

## 📚 References

- [Synology NAS Documentation](https://www.synology.com/en-global/knowledgebase/DSM/help)
- [SMB/CIFS Protocol](https://docs.microsoft.com/en-us/windows/win32/fileio/microsoft-smb-protocol-and-cifs-protocol-overview)
- [File Storage Best Practices](https://docs.microsoft.com/en-us/azure/storage/common/storage-performance-checklist)

---

## 👥 Team Responsibilities

- **Backend Developer**: Implement service layer, update repositories/BLL
- **Database Admin**: Run migration scripts, optimize database
- **DevOps**: Setup NAS, configure network, monitoring
- **QA**: Test migration, verify functionality
- **PM**: Coordinate, track progress, manage risks

---

**Document Version**: 1.0  
**Last Updated**: 2025-01-XX  
**Author**: Development Team  
**Status**: Draft - Pending Review

