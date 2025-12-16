# Migration: Refactor ProductImage Table

## Mô tả

Migration script này refactor bảng `ProductImage` để hỗ trợ lưu trữ hình ảnh trên NAS (Network Attached Storage) thay vì lưu trực tiếp trong database, tương tự như cách bảng `StockInOutImage` đã được refactor.

## Mục tiêu

- ✅ Thêm các cột metadata để quản lý file trên NAS
- ✅ Tạo indexes để tối ưu query performance
- ✅ Chuẩn bị cho việc migration dữ liệu từ ImageData sang NAS storage
- ✅ Hỗ trợ nhiều loại storage: Database, NAS, Local, Cloud

## Các cột được thêm

| Tên cột | Kiểu dữ liệu | Nullable | Default | Mô tả |
|---------|--------------|----------|---------|-------|
| `FileName` | `NVARCHAR(255)` | NULL | - | Tên file hình ảnh được lưu trên NAS |
| `RelativePath` | `NVARCHAR(500)` | NULL | - | Đường dẫn tương đối từ root của NAS share |
| `FullPath` | `NVARCHAR(1000)` | NULL | - | Đường dẫn đầy đủ (UNC path) đến file trên NAS |
| `StorageType` | `NVARCHAR(20)` | NULL | 'NAS' | Loại storage: NAS, Local, Cloud, Database |
| `FileSize` | `BIGINT` | NULL | - | Kích thước file (bytes) - sử dụng ImageSize nếu đã có |
| `FileExtension` | `NVARCHAR(10)` | NULL | - | Phần mở rộng file (jpg, png, etc.) |
| `MimeType` | `NVARCHAR(100)` | NULL | - | MIME type của file |
| `Checksum` | `NVARCHAR(64)` | NULL | - | Checksum (MD5/SHA256) để verify integrity |
| `FileExists` | `BIT` | NULL | 1 | Flag đánh dấu file có tồn tại trên NAS |
| `LastVerified` | `DATETIME` | NULL | - | Thời gian verify file lần cuối |
| `MigrationStatus` | `NVARCHAR(20)` | NULL | 'Pending' | Trạng thái migration: Pending, Migrated, Failed |

## Indexes được tạo

- `IX_ProductImage_RelativePath`: Index trên `RelativePath` (filtered index, chỉ index các record có RelativePath)
- `IX_ProductImage_FileExists`: Index trên `FileExists` (filtered index)
- `IX_ProductImage_StorageType`: Index trên `StorageType` (filtered index)
- `IX_ProductImage_MigrationStatus`: Index trên `MigrationStatus` (filtered index)
- `IX_ProductImage_FileName`: Index trên `FileName` (filtered index)

## Cách chạy

1. **Backup database** trước khi chạy migration
2. Mở SQL Server Management Studio
3. Kết nối đến database `VnsErp2025Final`
4. Mở file `Migration_ProductImage_Refactor.sql`
5. Execute script

## Script Idempotent

Script này có thể chạy lại nhiều lần một cách an toàn:
- Kiểm tra sự tồn tại của cột trước khi thêm
- Kiểm tra sự tồn tại của index trước khi tạo
- Kiểm tra sự tồn tại của extended property trước khi thêm

## Rollback

Nếu cần rollback, chạy script sau:

```sql
USE [VnsErp2025Final];
GO

BEGIN TRANSACTION;
GO

-- Xóa indexes
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_FileName')
    DROP INDEX IX_ProductImage_FileName ON dbo.ProductImage;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_MigrationStatus')
    DROP INDEX IX_ProductImage_MigrationStatus ON dbo.ProductImage;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_StorageType')
    DROP INDEX IX_ProductImage_StorageType ON dbo.ProductImage;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_FileExists')
    DROP INDEX IX_ProductImage_FileExists ON dbo.ProductImage;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_RelativePath')
    DROP INDEX IX_ProductImage_RelativePath ON dbo.ProductImage;
GO

-- Xóa các cột (CHỈ chạy nếu chắc chắn không cần dữ liệu)
-- LƯU Ý: Chỉ xóa cột nếu đã migrate xong và không cần rollback
/*
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'MigrationStatus')
    ALTER TABLE dbo.ProductImage DROP COLUMN MigrationStatus;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'LastVerified')
    ALTER TABLE dbo.ProductImage DROP COLUMN LastVerified;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileExists')
    ALTER TABLE dbo.ProductImage DROP COLUMN FileExists;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'Checksum')
    ALTER TABLE dbo.ProductImage DROP COLUMN Checksum;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'MimeType')
    ALTER TABLE dbo.ProductImage DROP COLUMN MimeType;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileExtension')
    ALTER TABLE dbo.ProductImage DROP COLUMN FileExtension;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileSize')
    ALTER TABLE dbo.ProductImage DROP COLUMN FileSize;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'StorageType')
    ALTER TABLE dbo.ProductImage DROP COLUMN StorageType;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FullPath')
    ALTER TABLE dbo.ProductImage DROP COLUMN FullPath;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'RelativePath')
    ALTER TABLE dbo.ProductImage DROP COLUMN RelativePath;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileName')
    ALTER TABLE dbo.ProductImage DROP COLUMN FileName;
GO
*/

COMMIT TRANSACTION;
GO
```

## Lưu ý

- Tất cả các cột đều là NULL, không ảnh hưởng đến dữ liệu hiện có
- Script có thể chạy lại nhiều lần (idempotent) - sẽ bỏ qua nếu cột/index đã tồn tại
- Sau khi chạy migration, cần:
  1. Refresh DataContext trong Visual Studio
  2. Cập nhật Entity trong `VnsErp2025.designer.cs` (hoặc regenerate từ DBML)
  3. Cập nhật DTO để thêm các properties mới
  4. Cập nhật Repository và BLL để xử lý các cột mới
  5. Cập nhật ImageStorageService để migrate dữ liệu từ ImageData sang NAS

## So sánh với StockInOutImage

Bảng `ProductImage` có cấu trúc tương tự `StockInOutImage` sau khi refactor:

| Feature | StockInOutImage | ProductImage |
|---------|----------------|--------------|
| FileName | ✅ | ✅ |
| RelativePath | ✅ | ✅ |
| FullPath | ✅ | ✅ |
| StorageType | ✅ | ✅ |
| FileSize | ✅ | ✅ (hoặc ImageSize) |
| FileExtension | ✅ | ✅ |
| MimeType | ✅ | ✅ |
| Checksum | ✅ | ✅ |
| FileExists | ✅ | ✅ |
| LastVerified | ✅ | ✅ |
| MigrationStatus | ✅ | ✅ |

## Tham khảo

- Bảng `StockInOutImage` đã được refactor tương tự
- Tài liệu: `Docs/ImageStorageRefactoringArchitecture.md`
- Migration tương tự: `Migration_BusinessPartner_AddLogo.sql`

## Các bước tiếp theo

1. **Chạy migration script** để thêm các cột và indexes
2. **Refresh DataContext** trong Visual Studio
3. **Update BLL/Repository** để sử dụng các cột mới
4. **Tạo migration script** để migrate dữ liệu từ ImageData sang NAS
5. **Test** việc load/save image từ NAS
6. **Sau khi verify**, có thể xóa cột ImageData (nếu muốn)

---

**Document Version**: 1.0  
**Created**: 2025-01-XX  
**Author**: Development Team  
**Status**: Ready for Review



