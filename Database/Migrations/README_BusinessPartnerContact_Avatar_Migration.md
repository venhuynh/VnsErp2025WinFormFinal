# Migration: Refactor Avatar Columns cho BusinessPartnerContact

## Mô tả

Migration script này refactor cấu trúc lưu trữ Avatar của `BusinessPartnerContact` để tương tự như cách `BusinessPartner` lưu Logo. Thay vì chỉ có `Avatar` (Binary) và `AvatarPath` (string), bây giờ sẽ có cấu trúc đầy đủ với metadata và thumbnail.

## Các thay đổi

### Các cột được thêm

| Tên cột | Kiểu dữ liệu | Nullable | Mô tả |
|---------|--------------|----------|-------|
| `AvatarFileName` | `NVARCHAR(255)` | NULL | Tên file avatar |
| `AvatarRelativePath` | `NVARCHAR(500)` | NULL | Đường dẫn tương đối của avatar |
| `AvatarFullPath` | `NVARCHAR(1000)` | NULL | Đường dẫn đầy đủ của avatar |
| `AvatarStorageType` | `NVARCHAR(20)` | NULL | Loại storage (Database, FileSystem, NAS, etc.) |
| `AvatarFileSize` | `BIGINT` | NULL | Kích thước file avatar (bytes) |
| `AvatarChecksum` | `NVARCHAR(64)` | NULL | Checksum để verify integrity |
| `AvatarThumbnailData` | `VARBINARY(MAX)` | NULL | Dữ liệu binary của avatar thumbnail (thay thế cho Avatar) |

### Các cột được xóa

| Tên cột | Kiểu dữ liệu | Lý do |
|---------|--------------|-------|
| `Avatar` | `VARBINARY(MAX)` | Thay thế bằng `AvatarThumbnailData` (chỉ lưu thumbnail, file gốc lưu trên NAS) |
| `AvatarPath` | `NVARCHAR(500)` | Thay thế bằng `AvatarFullPath` (đầy đủ hơn) |

### Indexes được tạo

- `IX_BusinessPartnerContact_AvatarFileName`: Index trên `AvatarFileName` (filtered index, chỉ index các record có AvatarFileName)
- `IX_BusinessPartnerContact_AvatarStorageType`: Index trên `AvatarStorageType` (filtered index, chỉ index các record có AvatarStorageType)

## Cách chạy

1. **Backup database** trước khi chạy migration
2. **Kiểm tra dữ liệu**: Script sẽ cảnh báo nếu có dữ liệu Avatar/AvatarPath hiện có
3. Mở SQL Server Management Studio
4. Kết nối đến database `VnsErp2025Final`
5. Mở file `Migration_BusinessPartnerContact_RefactorAvatar.sql`
6. Execute script

## Lưu ý quan trọng

⚠️ **CẢNH BÁO**: Script sẽ **XÓA** cột `Avatar` (Binary). Nếu có dữ liệu quan trọng trong cột này, hãy backup trước khi chạy migration.

Script sẽ tự động migrate dữ liệu từ `AvatarPath` sang `AvatarFullPath` trước khi xóa cột cũ.

## Rollback

Nếu cần rollback, chạy script sau:

```sql
USE [VnsErp2025Final];
GO

BEGIN TRANSACTION;
GO

-- Xóa indexes
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'IX_BusinessPartnerContact_AvatarStorageType')
    DROP INDEX IX_BusinessPartnerContact_AvatarStorageType ON dbo.BusinessPartnerContact;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'IX_BusinessPartnerContact_AvatarFileName')
    DROP INDEX IX_BusinessPartnerContact_AvatarFileName ON dbo.BusinessPartnerContact;
GO

-- Xóa các cột mới
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'AvatarThumbnailData')
    ALTER TABLE dbo.BusinessPartnerContact DROP COLUMN AvatarThumbnailData;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'AvatarChecksum')
    ALTER TABLE dbo.BusinessPartnerContact DROP COLUMN AvatarChecksum;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'AvatarFileSize')
    ALTER TABLE dbo.BusinessPartnerContact DROP COLUMN AvatarFileSize;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'AvatarStorageType')
    ALTER TABLE dbo.BusinessPartnerContact DROP COLUMN AvatarStorageType;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'AvatarFullPath')
    ALTER TABLE dbo.BusinessPartnerContact DROP COLUMN AvatarFullPath;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'AvatarRelativePath')
    ALTER TABLE dbo.BusinessPartnerContact DROP COLUMN AvatarRelativePath;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'AvatarFileName')
    ALTER TABLE dbo.BusinessPartnerContact DROP COLUMN AvatarFileName;
GO

-- Khôi phục các cột cũ (nếu cần)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'Avatar')
    ALTER TABLE dbo.BusinessPartnerContact ADD Avatar VARBINARY(MAX) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') AND name = 'AvatarPath')
    ALTER TABLE dbo.BusinessPartnerContact ADD AvatarPath NVARCHAR(500) NULL;
GO

COMMIT TRANSACTION;
GO
```

## Sau khi migration

Sau khi chạy migration thành công, cần:

1. **Refresh DataContext** trong Visual Studio
2. **Regenerate Entity** trong `VnsErp2025.designer.cs` (hoặc regenerate từ DBML)
3. **Cập nhật DTO** để thêm các properties Avatar mới
4. **Cập nhật Repository và BLL** để xử lý Avatar theo pattern mới (tương tự như Logo của BusinessPartner)

## Tham khảo

- Bảng `BusinessPartner` có cấu trúc tương tự cho Logo
- Bảng `Company` có cấu trúc tương tự cho Logo
- Bảng `ProductService` và `ProductVariant` có cấu trúc tương tự cho ThumbnailImage

## Pattern sử dụng

Sau khi migration, pattern sử dụng sẽ tương tự như `BusinessPartner.Logo`:

- **File gốc**: Lưu trên NAS với metadata đầy đủ (FileName, RelativePath, FullPath, StorageType, FileSize, Checksum)
- **Thumbnail**: Lưu trong database (`AvatarThumbnailData`) để hiển thị nhanh trong GridView/ListView

