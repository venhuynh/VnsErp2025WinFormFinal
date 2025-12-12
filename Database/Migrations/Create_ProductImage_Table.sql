-- =============================================
-- Script: Tạo bảng ProductImage
-- Dựa trên cấu trúc bảng StockInOutImage
-- =============================================
-- Mô tả: Tạo lại bảng ProductImage với cấu trúc tương tự StockInOutImage
-- =============================================

USE [VnsErp2025Final];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

SET ANSI_PADDING ON;
GO

BEGIN TRANSACTION;
GO

PRINT '========================================';
PRINT 'Bắt đầu tạo bảng ProductImage';
PRINT '========================================';
GO

-- Kiểm tra và xóa bảng nếu đã tồn tại
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ProductImage' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '⚠️  Bảng ProductImage đã tồn tại. Đang xóa...';
    
    -- Xóa Extended Properties
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'MS_Description' AND minor_id = 0)
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductImage';
    
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.ProductImage') AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'MigrationStatus') AND name = 'MS_Description')
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductImage', @level2type=N'COLUMN',@level2name=N'MigrationStatus';
    
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.ProductImage') AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'StorageType') AND name = 'MS_Description')
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductImage', @level2type=N'COLUMN',@level2name=N'StorageType';
    
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.ProductImage') AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'RelativePath') AND name = 'MS_Description')
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductImage', @level2type=N'COLUMN',@level2name=N'RelativePath';
    
    -- Xóa Foreign Keys
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ProductImage_ProductVariant' AND parent_object_id = OBJECT_ID(N'dbo.ProductImage'))
        ALTER TABLE [dbo].[ProductImage] DROP CONSTRAINT [FK_ProductImage_ProductVariant];
    
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_ProductImage_ProductService' AND parent_object_id = OBJECT_ID(N'dbo.ProductImage'))
        ALTER TABLE [dbo].[ProductImage] DROP CONSTRAINT [FK_ProductImage_ProductService];
    
    -- Xóa Default Constraints
    DECLARE @sql NVARCHAR(MAX) = '';
    SELECT @sql += 'ALTER TABLE [dbo].[ProductImage] DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.ProductImage');
    IF @sql <> '' EXEC sp_executesql @sql;
    
    -- Xóa bảng
    DROP TABLE [dbo].[ProductImage];
    PRINT '✅ Đã xóa bảng ProductImage cũ';
END
GO

-- =============================================
-- Tạo bảng ProductImage
-- =============================================
CREATE TABLE [dbo].[ProductImage](
	[Id] [uniqueidentifier] NOT NULL,
	[ProductId] [uniqueidentifier] NULL,
	[ImageData] [varbinary](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[CreateBy] [uniqueidentifier] NOT NULL,
	[ModifiedDate] [datetime] NULL,
	[ModifiedBy] [uniqueidentifier] NOT NULL,
	[FileName] [nvarchar](255) NULL,
	[RelativePath] [nvarchar](500) NULL,
	[FullPath] [nvarchar](1000) NULL,
	[StorageType] [nvarchar](20) NULL DEFAULT ('NAS'),
	[FileSize] [bigint] NULL,
	[FileExtension] [nvarchar](10) NULL,
	[MimeType] [nvarchar](100) NULL,
	[Checksum] [nvarchar](64) NULL,
	[FileExists] [bit] NULL DEFAULT ((1)),
	[LastVerified] [datetime] NULL,
	[MigrationStatus] [nvarchar](20) NULL DEFAULT ('Pending'),
 CONSTRAINT [PK_ProductImage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

SET ANSI_PADDING OFF;
GO

PRINT '✅ Đã tạo bảng ProductImage';
GO

-- =============================================
-- Tạo Foreign Key Constraints
-- =============================================

-- Foreign Key: ProductImage -> ProductService
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ProductService' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    ALTER TABLE [dbo].[ProductImage] WITH CHECK ADD CONSTRAINT [FK_ProductImage_ProductService] 
    FOREIGN KEY([ProductId])
    REFERENCES [dbo].[ProductService] ([Id]);
    
    ALTER TABLE [dbo].[ProductImage] CHECK CONSTRAINT [FK_ProductImage_ProductService];
    PRINT '✅ Đã tạo Foreign Key FK_ProductImage_ProductService';
END
ELSE
BEGIN
    PRINT '⚠️  Bảng ProductService chưa tồn tại, bỏ qua Foreign Key';
END
GO

-- =============================================
-- Tạo Indexes (tương tự StockInOutImage)
-- =============================================

-- Đảm bảo ANSI_PADDING ON cho filtered indexes
SET ANSI_PADDING ON;
GO

-- Index cho ProductId
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_ProductId')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_ProductId] 
    ON [dbo].[ProductImage]([ProductId])
    WHERE [ProductId] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_ProductId';
END
GO

-- Index cho RelativePath (tương tự StockInOutImage)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_RelativePath')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_RelativePath] 
    ON [dbo].[ProductImage]([RelativePath])
    WHERE [RelativePath] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_RelativePath';
END
GO

-- Index cho FileExists (tương tự StockInOutImage)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_FileExists')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_FileExists] 
    ON [dbo].[ProductImage]([FileExists])
    WHERE [FileExists] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_FileExists';
END
GO

-- Index cho StorageType (tương tự StockInOutImage)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_StorageType')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_StorageType] 
    ON [dbo].[ProductImage]([StorageType])
    WHERE [StorageType] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_StorageType';
END
GO

-- Index cho MigrationStatus (tương tự StockInOutImage)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_MigrationStatus')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_MigrationStatus] 
    ON [dbo].[ProductImage]([MigrationStatus])
    WHERE [MigrationStatus] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_MigrationStatus';
END
GO

-- Index cho FileName (tương tự StockInOutImage)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ProductImage') AND name = 'IX_ProductImage_FileName')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_FileName] 
    ON [dbo].[ProductImage]([FileName])
    WHERE [FileName] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_ProductImage_FileName';
END
GO

SET ANSI_PADDING OFF;
GO

-- =============================================
-- Thêm Extended Properties (Comments)
-- =============================================

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Đường dẫn tương đối từ root của NAS share', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductImage', @level2type=N'COLUMN',@level2name=N'RelativePath';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loại storage: NAS, Local, Cloud, Database', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductImage', @level2type=N'COLUMN',@level2name=N'StorageType';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Trạng thái migration: Pending, Migrated, Failed', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductImage', @level2type=N'COLUMN',@level2name=N'MigrationStatus';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảng lưu trữ hình ảnh của Product. Hỗ trợ lưu trữ trên NAS hoặc Database.', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ProductImage';
GO

PRINT '✅ Đã thêm extended properties';
GO

PRINT '';
PRINT '========================================';
PRINT 'Hoàn thành tạo bảng ProductImage';
PRINT '========================================';
PRINT '';
PRINT 'Tóm tắt:';
PRINT '- Đã tạo bảng ProductImage với cấu trúc tương tự StockInOutImage';
PRINT '- Các cột: Id, ProductId, ImageData, CreateDate, CreateBy, ModifiedDate, ModifiedBy, FileName, RelativePath, FullPath, StorageType, FileSize, FileExtension, MimeType, Checksum, FileExists, LastVerified, MigrationStatus';
PRINT '- Default Constraints đã được định nghĩa trong CREATE TABLE: StorageType, FileExists, MigrationStatus';
PRINT '- Đã tạo Foreign Key: FK_ProductImage_ProductService';
PRINT '- Đã tạo các indexes: IX_ProductImage_ProductId, IX_ProductImage_RelativePath, IX_ProductImage_FileExists, IX_ProductImage_StorageType, IX_ProductImage_MigrationStatus, IX_ProductImage_FileName';
PRINT '- Đã thêm extended properties (comments)';
PRINT '';
PRINT 'Lưu ý:';
PRINT '1. Bảng đã được tạo với cấu trúc đầy đủ, sẵn sàng sử dụng';
PRINT '2. Cần refresh DataContext trong Visual Studio';
PRINT '3. Cần regenerate Entity từ DBML hoặc designer.cs';
PRINT '';

IF @@TRANCOUNT > 0
BEGIN
    COMMIT TRANSACTION;
    PRINT '✅ Transaction đã được commit thành công!';
END
ELSE
BEGIN
    PRINT '⚠️  Không có transaction để commit';
END
GO
