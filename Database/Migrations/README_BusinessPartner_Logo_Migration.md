# Migration: Thêm Logo Columns cho BusinessPartner

## Mô tả

Migration script này thêm các cột để lưu trữ logo của BusinessPartner vào bảng `BusinessPartner`, tương tự như cách bảng `Company` lưu logo.

## Các cột được thêm

| Tên cột | Kiểu dữ liệu | Nullable | Mô tả |
|---------|--------------|----------|-------|
| `Logo` | `VARBINARY(MAX)` | NULL | Dữ liệu binary của logo |
| `LogoFileName` | `NVARCHAR(255)` | NULL | Tên file logo |
| `LogoRelativePath` | `NVARCHAR(500)` | NULL | Đường dẫn tương đối của logo |
| `LogoFullPath` | `NVARCHAR(1000)` | NULL | Đường dẫn đầy đủ của logo |
| `LogoStorageType` | `NVARCHAR(20)` | NULL | Loại storage (Database, FileSystem, NAS, etc.) |
| `LogoFileSize` | `BIGINT` | NULL | Kích thước file logo (bytes) |
| `LogoChecksum` | `NVARCHAR(64)` | NULL | Checksum để verify integrity |

## Indexes được tạo

- `IX_BusinessPartner_LogoFileName`: Index trên `LogoFileName` (filtered index, chỉ index các record có LogoFileName)
- `IX_BusinessPartner_LogoStorageType`: Index trên `LogoStorageType` (filtered index, chỉ index các record có LogoStorageType)

## Cách chạy

1. **Backup database** trước khi chạy migration
2. Mở SQL Server Management Studio
3. Kết nối đến database `VnsErp2025Final`
4. Mở file `Migration_BusinessPartner_AddLogo.sql`
5. Execute script

## Rollback

Nếu cần rollback, chạy script sau:

```sql
USE [VnsErp2025Final];
GO

BEGIN TRANSACTION;
GO

-- Xóa indexes
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'IX_BusinessPartner_LogoStorageType')
    DROP INDEX IX_BusinessPartner_LogoStorageType ON dbo.BusinessPartner;
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'IX_BusinessPartner_LogoFileName')
    DROP INDEX IX_BusinessPartner_LogoFileName ON dbo.BusinessPartner;
GO

-- Xóa các cột
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'LogoChecksum')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoChecksum;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'LogoFileSize')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoFileSize;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'LogoStorageType')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoStorageType;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'LogoFullPath')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoFullPath;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'LogoRelativePath')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoRelativePath;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'LogoFileName')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoFileName;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') AND name = 'Logo')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN Logo;
GO

COMMIT TRANSACTION;
GO
```

## Lưu ý

- Tất cả các cột đều là NULL, không ảnh hưởng đến dữ liệu hiện có
- Script có thể chạy lại nhiều lần (idempotent) - sẽ bỏ qua nếu cột đã tồn tại
- Sau khi chạy migration, cần:
  1. Refresh DataContext trong Visual Studio
  2. Cập nhật Entity trong `VnsErp2025.designer.cs` (hoặc regenerate từ DBML)
  3. Cập nhật DTO để thêm các properties Logo
  4. Cập nhật Repository và BLL để xử lý Logo

## Tham khảo

- Bảng `Company` có cấu trúc tương tự cho Logo
- Bảng `StockInOutImage` có cấu trúc tương tự cho ImageData

