-- =============================================
-- Migration Script: Refactor ProductImage Table
-- Tương tự như StockInOutImage refactoring
-- =============================================
-- Mô tả: Thêm các cột và indexes để hỗ trợ lưu trữ hình ảnh trên NAS
-- Thay vì lưu trực tiếp ImageData trong database
-- =============================================

USE [VnsErp2025Final];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

BEGIN TRANSACTION;
GO

PRINT '========================================';
PRINT 'Bắt đầu refactor bảng ProductImage';
PRINT '========================================';
GO

-- =============================================
-- Bước 1: Thêm các cột mới (nếu chưa tồn tại)
-- =============================================

-- FileName
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileName')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [FileName] NVARCHAR(255) NULL;
    PRINT '✅ Đã thêm cột FileName';
END
ELSE
BEGIN
    PRINT '⏭️  Cột FileName đã tồn tại, bỏ qua';
END
GO

-- RelativePath
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'RelativePath')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [RelativePath] NVARCHAR(500) NULL;
    PRINT '✅ Đã thêm cột RelativePath';
END
ELSE
BEGIN
    PRINT '⏭️  Cột RelativePath đã tồn tại, bỏ qua';
END
GO

-- FullPath
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FullPath')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [FullPath] NVARCHAR(1000) NULL;
    PRINT '✅ Đã thêm cột FullPath';
END
ELSE
BEGIN
    PRINT '⏭️  Cột FullPath đã tồn tại, bỏ qua';
END
GO

-- StorageType
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'StorageType')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [StorageType] NVARCHAR(20) NULL DEFAULT 'NAS';
    PRINT '✅ Đã thêm cột StorageType';
END
ELSE
BEGIN
    PRINT '⏭️  Cột StorageType đã tồn tại, bỏ qua';
END
GO

-- FileSize (sử dụng ImageSize nếu đã có, hoặc thêm FileSize mới)
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileSize')
BEGIN
    -- Kiểm tra xem có cột ImageSize không
    IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'ImageSize')
    BEGIN
        PRINT '⏭️  Cột ImageSize đã tồn tại, sử dụng ImageSize thay cho FileSize';
    END
    ELSE
    BEGIN
        ALTER TABLE [dbo].[ProductImage]
        ADD [FileSize] BIGINT NULL;
        PRINT '✅ Đã thêm cột FileSize';
    END
END
ELSE
BEGIN
    PRINT '⏭️  Cột FileSize đã tồn tại, bỏ qua';
END
GO

-- FileExtension
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileExtension')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [FileExtension] NVARCHAR(10) NULL;
    PRINT '✅ Đã thêm cột FileExtension';
END
ELSE
BEGIN
    PRINT '⏭️  Cột FileExtension đã tồn tại, bỏ qua';
END
GO

-- MimeType
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'MimeType')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [MimeType] NVARCHAR(100) NULL;
    PRINT '✅ Đã thêm cột MimeType';
END
ELSE
BEGIN
    PRINT '⏭️  Cột MimeType đã tồn tại, bỏ qua';
END
GO

-- Checksum
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'Checksum')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [Checksum] NVARCHAR(64) NULL;
    PRINT '✅ Đã thêm cột Checksum';
END
ELSE
BEGIN
    PRINT '⏭️  Cột Checksum đã tồn tại, bỏ qua';
END
GO

-- FileExists
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileExists')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [FileExists] BIT NULL DEFAULT 1;
    PRINT '✅ Đã thêm cột FileExists';
END
ELSE
BEGIN
    PRINT '⏭️  Cột FileExists đã tồn tại, bỏ qua';
END
GO

-- LastVerified
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'LastVerified')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [LastVerified] DATETIME NULL;
    PRINT '✅ Đã thêm cột LastVerified';
END
ELSE
BEGIN
    PRINT '⏭️  Cột LastVerified đã tồn tại, bỏ qua';
END
GO

-- MigrationStatus
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'MigrationStatus')
BEGIN
    ALTER TABLE [dbo].[ProductImage]
    ADD [MigrationStatus] NVARCHAR(20) NULL DEFAULT 'Pending';
    PRINT '✅ Đã thêm cột MigrationStatus';
END
ELSE
BEGIN
    PRINT '⏭️  Cột MigrationStatus đã tồn tại, bỏ qua';
END
GO

-- =============================================
-- Bước 2: Tạo Indexes (tương tự StockInOutImage)
-- =============================================

-- Index cho RelativePath
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_RelativePath')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_RelativePath] 
    ON [dbo].[ProductImage]([RelativePath])
    WHERE [RelativePath] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_RelativePath';
END
ELSE
BEGIN
    PRINT '⏭️  Index IX_ProductImage_RelativePath đã tồn tại, bỏ qua';
END
GO

-- Index cho FileExists
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_FileExists')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_FileExists] 
    ON [dbo].[ProductImage]([FileExists])
    WHERE [FileExists] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_FileExists';
END
ELSE
BEGIN
    PRINT '⏭️  Index IX_ProductImage_FileExists đã tồn tại, bỏ qua';
END
GO

-- Index cho StorageType
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_StorageType')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_StorageType] 
    ON [dbo].[ProductImage]([StorageType])
    WHERE [StorageType] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_StorageType';
END
ELSE
BEGIN
    PRINT '⏭️  Index IX_ProductImage_StorageType đã tồn tại, bỏ qua';
END
GO

-- Index cho MigrationStatus
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_MigrationStatus')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_MigrationStatus] 
    ON [dbo].[ProductImage]([MigrationStatus])
    WHERE [MigrationStatus] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_MigrationStatus';
END
ELSE
BEGIN
    PRINT '⏭️  Index IX_ProductImage_MigrationStatus đã tồn tại, bỏ qua';
END
GO

-- Index cho FileName (filtered index)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_FileName')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_FileName] 
    ON [dbo].[ProductImage]([FileName])
    WHERE [FileName] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_FileName';
END
ELSE
BEGIN
    PRINT '⏭️  Index IX_ProductImage_FileName đã tồn tại, bỏ qua';
END
GO

-- =============================================
-- Bước 3: Thêm Extended Properties (Comments)
-- =============================================

-- Comment cho RelativePath
IF NOT EXISTS (
    SELECT 1 
    FROM sys.extended_properties 
    WHERE major_id = OBJECT_ID(N'dbo.ProductImage') 
    AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'RelativePath')
    AND name = 'MS_Description'
)
BEGIN
    EXEC sys.sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Đường dẫn tương đối từ root của NAS share', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'ProductImage', 
        @level2type = N'COLUMN', @level2name = N'RelativePath';
    PRINT '✅ Đã thêm comment cho RelativePath';
END
ELSE
BEGIN
    PRINT '⏭️  Comment cho RelativePath đã tồn tại, bỏ qua';
END
GO

-- Comment cho StorageType
IF NOT EXISTS (
    SELECT 1 
    FROM sys.extended_properties 
    WHERE major_id = OBJECT_ID(N'dbo.ProductImage') 
    AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'StorageType')
    AND name = 'MS_Description'
)
BEGIN
    EXEC sys.sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Loại storage: NAS, Local, Cloud, Database', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'ProductImage', 
        @level2type = N'COLUMN', @level2name = N'StorageType';
    PRINT '✅ Đã thêm comment cho StorageType';
END
ELSE
BEGIN
    PRINT '⏭️  Comment cho StorageType đã tồn tại, bỏ qua';
END
GO

-- Comment cho FileName
IF NOT EXISTS (
    SELECT 1 
    FROM sys.extended_properties 
    WHERE major_id = OBJECT_ID(N'dbo.ProductImage') 
    AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileName')
    AND name = 'MS_Description'
)
BEGIN
    EXEC sys.sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Tên file hình ảnh được lưu trên NAS', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'ProductImage', 
        @level2type = N'COLUMN', @level2name = N'FileName';
    PRINT '✅ Đã thêm comment cho FileName';
END
ELSE
BEGIN
    PRINT '⏭️  Comment cho FileName đã tồn tại, bỏ qua';
END
GO

-- Comment cho FullPath
IF NOT EXISTS (
    SELECT 1 
    FROM sys.extended_properties 
    WHERE major_id = OBJECT_ID(N'dbo.ProductImage') 
    AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FullPath')
    AND name = 'MS_Description'
)
BEGIN
    EXEC sys.sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Đường dẫn đầy đủ (UNC path) đến file trên NAS', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'ProductImage', 
        @level2type = N'COLUMN', @level2name = N'FullPath';
    PRINT '✅ Đã thêm comment cho FullPath';
END
ELSE
BEGIN
    PRINT '⏭️  Comment cho FullPath đã tồn tại, bỏ qua';
END
GO

-- Comment cho Checksum
IF NOT EXISTS (
    SELECT 1 
    FROM sys.extended_properties 
    WHERE major_id = OBJECT_ID(N'dbo.ProductImage') 
    AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'Checksum')
    AND name = 'MS_Description'
)
BEGIN
    EXEC sys.sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Checksum (MD5/SHA256) để verify integrity của file', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'ProductImage', 
        @level2type = N'COLUMN', @level2name = N'Checksum';
    PRINT '✅ Đã thêm comment cho Checksum';
END
ELSE
BEGIN
    PRINT '⏭️  Comment cho Checksum đã tồn tại, bỏ qua';
END
GO

-- Comment cho FileExists
IF NOT EXISTS (
    SELECT 1 
    FROM sys.extended_properties 
    WHERE major_id = OBJECT_ID(N'dbo.ProductImage') 
    AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'FileExists')
    AND name = 'MS_Description'
)
BEGIN
    EXEC sys.sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Flag để đánh dấu file có tồn tại trên NAS hay không', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'ProductImage', 
        @level2type = N'COLUMN', @level2name = N'FileExists';
    PRINT '✅ Đã thêm comment cho FileExists';
END
ELSE
BEGIN
    PRINT '⏭️  Comment cho FileExists đã tồn tại, bỏ qua';
END
GO

-- Comment cho MigrationStatus
IF NOT EXISTS (
    SELECT 1 
    FROM sys.extended_properties 
    WHERE major_id = OBJECT_ID(N'dbo.ProductImage') 
    AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'MigrationStatus')
    AND name = 'MS_Description'
)
BEGIN
    EXEC sys.sp_addextendedproperty 
        @name = N'MS_Description', 
        @value = N'Trạng thái migration: Pending, Migrated, Failed', 
        @level0type = N'SCHEMA', @level0name = N'dbo', 
        @level1type = N'TABLE', @level1name = N'ProductImage', 
        @level2type = N'COLUMN', @level2name = N'MigrationStatus';
    PRINT '✅ Đã thêm comment cho MigrationStatus';
END
ELSE
BEGIN
    PRINT '⏭️  Comment cho MigrationStatus đã tồn tại, bỏ qua';
END
GO

-- =============================================
-- Bước 4: Cập nhật giá trị mặc định cho các record hiện có
-- =============================================

-- Cập nhật MigrationStatus = 'Pending' cho các record chưa có giá trị
UPDATE [dbo].[ProductImage]
SET [MigrationStatus] = 'Pending'
WHERE [MigrationStatus] IS NULL;
GO

-- Cập nhật FileExists = 1 cho các record chưa có giá trị
UPDATE [dbo].[ProductImage]
SET [FileExists] = 1
WHERE [FileExists] IS NULL;
GO

-- Cập nhật StorageType = 'Database' cho các record có ImageData nhưng chưa có StorageType
UPDATE [dbo].[ProductImage]
SET [StorageType] = 'Database'
WHERE [ImageData] IS NOT NULL 
  AND [StorageType] IS NULL;
GO

PRINT '';
PRINT '========================================';
PRINT 'Hoàn thành refactor bảng ProductImage';
PRINT '========================================';
PRINT '';
PRINT 'Tóm tắt:';
PRINT '- Đã thêm các cột: FileName, RelativePath, FullPath, StorageType, FileExtension, MimeType, Checksum, FileExists, LastVerified, MigrationStatus';
PRINT '- Đã tạo các indexes: IX_ProductImage_RelativePath, IX_ProductImage_FileExists, IX_ProductImage_StorageType, IX_ProductImage_MigrationStatus, IX_ProductImage_FileName';
PRINT '- Đã thêm extended properties (comments) cho các cột';
PRINT '- Đã cập nhật giá trị mặc định cho các record hiện có';
PRINT '';
PRINT 'Lưu ý:';
PRINT '1. Script này có thể chạy lại nhiều lần (idempotent)';
PRINT '2. Sau khi chạy script, cần refresh DataContext trong Visual Studio';
PRINT '3. Cần cập nhật Entity trong VnsErp2025.designer.cs (hoặc regenerate từ DBML)';
PRINT '4. Cần cập nhật BLL/Repository để sử dụng các cột mới';
PRINT '';

COMMIT TRANSACTION;
GO

PRINT '✅ Transaction đã được commit thành công!';
GO
