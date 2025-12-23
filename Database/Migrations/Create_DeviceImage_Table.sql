-- =============================================
-- Script: Tạo bảng DeviceImage
-- Dựa trên cấu trúc bảng ProductImage và StockInOutImage
-- =============================================
-- Mô tả: Tạo bảng DeviceImage để quản lý hình ảnh của thiết bị (Device)
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
PRINT 'Bắt đầu tạo bảng DeviceImage';
PRINT '========================================';
GO

-- Kiểm tra và xóa bảng nếu đã tồn tại
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'DeviceImage' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '⚠️  Bảng DeviceImage đã tồn tại. Đang xóa...';
    
    -- Xóa Extended Properties
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'MS_Description' AND minor_id = 0)
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage';
    
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.DeviceImage') AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'MigrationStatus') AND name = 'MS_Description')
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'MigrationStatus';
    
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.DeviceImage') AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'StorageType') AND name = 'MS_Description')
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'StorageType';
    
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.DeviceImage') AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'RelativePath') AND name = 'MS_Description')
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'RelativePath';
    
    -- Xóa Foreign Keys
    IF EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_DeviceImage_Device' AND parent_object_id = OBJECT_ID(N'dbo.DeviceImage'))
        ALTER TABLE [dbo].[DeviceImage] DROP CONSTRAINT [FK_DeviceImage_Device];
    
    -- Xóa Default Constraints
    DECLARE @sql NVARCHAR(MAX) = '';
    SELECT @sql += 'ALTER TABLE [dbo].[DeviceImage] DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.DeviceImage');
    IF @sql <> '' EXEC sp_executesql @sql;
    
    -- Xóa bảng
    DROP TABLE [dbo].[DeviceImage];
    PRINT '✅ Đã xóa bảng DeviceImage cũ';
END
GO

-- =============================================
-- Tạo bảng DeviceImage
-- =============================================
CREATE TABLE [dbo].[DeviceImage](
	[Id] [uniqueidentifier] NOT NULL,
	[DeviceId] [uniqueidentifier] NOT NULL,
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
	[Caption] [nvarchar](255) NULL,
	[AltText] [nvarchar](255) NULL,
	[IsPrimary] [bit] NULL DEFAULT ((0)),
	[DisplayOrder] [int] NULL DEFAULT ((0)),
	[IsActive] [bit] NULL DEFAULT ((1)),
 CONSTRAINT [PK_DeviceImage] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
GO

SET ANSI_PADDING OFF;
GO

PRINT '✅ Đã tạo bảng DeviceImage';
GO

-- =============================================
-- Tạo Foreign Key Constraints
-- =============================================

-- Foreign Key: DeviceImage -> Device
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'Device' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    ALTER TABLE [dbo].[DeviceImage] WITH CHECK ADD CONSTRAINT [FK_DeviceImage_Device] 
    FOREIGN KEY([DeviceId])
    REFERENCES [dbo].[Device] ([Id]);
    
    ALTER TABLE [dbo].[DeviceImage] CHECK CONSTRAINT [FK_DeviceImage_Device];
    PRINT '✅ Đã tạo Foreign Key FK_DeviceImage_Device';
END
ELSE
BEGIN
    PRINT '⚠️  Bảng Device chưa tồn tại, bỏ qua Foreign Key';
END
GO

-- =============================================
-- Tạo Indexes (tương tự ProductImage và StockInOutImage)
-- =============================================

-- Đảm bảo ANSI_PADDING ON cho filtered indexes
SET ANSI_PADDING ON;
GO

-- Index cho DeviceId
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_DeviceId')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_DeviceId] 
    ON [dbo].[DeviceImage]([DeviceId])
    WHERE [DeviceId] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_DeviceImage_DeviceId';
END
GO

-- Index cho RelativePath
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_RelativePath')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_RelativePath] 
    ON [dbo].[DeviceImage]([RelativePath])
    WHERE [RelativePath] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_DeviceImage_RelativePath';
END
GO

-- Index cho FileExists
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_FileExists')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_FileExists] 
    ON [dbo].[DeviceImage]([FileExists])
    WHERE [FileExists] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_DeviceImage_FileExists';
END
GO

-- Index cho StorageType
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_StorageType')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_StorageType] 
    ON [dbo].[DeviceImage]([StorageType])
    WHERE [StorageType] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_DeviceImage_StorageType';
END
GO

-- Index cho MigrationStatus
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_MigrationStatus')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_MigrationStatus] 
    ON [dbo].[DeviceImage]([MigrationStatus])
    WHERE [MigrationStatus] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_DeviceImage_MigrationStatus';
END
GO

-- Index cho FileName
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_FileName')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_FileName] 
    ON [dbo].[DeviceImage]([FileName])
    WHERE [FileName] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_DeviceImage_FileName';
END
GO

-- Index cho IsPrimary (để tìm hình ảnh chính của thiết bị)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_IsPrimary')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_IsPrimary] 
    ON [dbo].[DeviceImage]([DeviceId], [IsPrimary])
    WHERE [IsPrimary] = 1;
    PRINT '✅ Đã tạo index IX_DeviceImage_IsPrimary';
END
GO

-- Index cho DisplayOrder (để sắp xếp hình ảnh)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_DisplayOrder')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_DisplayOrder] 
    ON [dbo].[DeviceImage]([DeviceId], [DisplayOrder])
    WHERE [DisplayOrder] IS NOT NULL;
    PRINT '✅ Đã tạo index IX_DeviceImage_DisplayOrder';
END
GO

-- Index cho IsActive
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.DeviceImage') AND name = 'IX_DeviceImage_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_DeviceImage_IsActive] 
    ON [dbo].[DeviceImage]([DeviceId], [IsActive])
    WHERE [IsActive] = 1;
    PRINT '✅ Đã tạo index IX_DeviceImage_IsActive';
END
GO

SET ANSI_PADDING OFF;
GO

-- =============================================
-- Thêm Extended Properties (Comments)
-- =============================================

-- Comment cho bảng
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảng lưu trữ hình ảnh của thiết bị (Device). Hỗ trợ lưu trữ trên NAS hoặc Database.', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage';
GO

-- Comment cho các cột quan trọng
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Đường dẫn tương đối từ root của NAS share', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'RelativePath';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Loại storage: NAS, Local, Cloud, Database', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'StorageType';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Trạng thái migration: Pending, Migrated, Failed', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'MigrationStatus';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Đánh dấu hình ảnh chính của thiết bị', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'IsPrimary';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Thứ tự hiển thị hình ảnh (số nhỏ hơn hiển thị trước)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'DisplayOrder';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Mô tả ngắn gọn về hình ảnh', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'Caption';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Text thay thế cho hình ảnh (dùng cho SEO và accessibility)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'DeviceImage', @level2type=N'COLUMN',@level2name=N'AltText';
GO

PRINT '✅ Đã thêm extended properties';
GO

PRINT '';
PRINT '========================================';
PRINT 'Hoàn thành tạo bảng DeviceImage';
PRINT '========================================';
PRINT '';
PRINT 'Tóm tắt:';
PRINT '- Đã tạo bảng DeviceImage với cấu trúc tương tự ProductImage và StockInOutImage';
PRINT '- Các cột: Id, DeviceId, ImageData, CreateDate, CreateBy, ModifiedDate, ModifiedBy, FileName, RelativePath, FullPath, StorageType, FileSize, FileExtension, MimeType, Checksum, FileExists, LastVerified, MigrationStatus, Caption, AltText, IsPrimary, DisplayOrder, IsActive';
PRINT '- Default Constraints: StorageType (NAS), FileExists (1), MigrationStatus (Pending), IsPrimary (0), DisplayOrder (0), IsActive (1)';
PRINT '- Đã tạo Foreign Key: FK_DeviceImage_Device';
PRINT '- Đã tạo các indexes:';
PRINT '  * IX_DeviceImage_DeviceId';
PRINT '  * IX_DeviceImage_RelativePath';
PRINT '  * IX_DeviceImage_FileExists';
PRINT '  * IX_DeviceImage_StorageType';
PRINT '  * IX_DeviceImage_MigrationStatus';
PRINT '  * IX_DeviceImage_FileName';
PRINT '  * IX_DeviceImage_IsPrimary';
PRINT '  * IX_DeviceImage_DisplayOrder';
PRINT '  * IX_DeviceImage_IsActive';
PRINT '- Đã thêm extended properties (comments)';
PRINT '';
PRINT 'Lưu ý:';
PRINT '1. Bảng đã được tạo với cấu trúc đầy đủ, sẵn sàng sử dụng';
PRINT '2. Cần refresh DataContext trong Visual Studio';
PRINT '3. Cần regenerate Entity từ DBML hoặc designer.cs';
PRINT '4. Bảng hỗ trợ lưu trữ hình ảnh trên NAS hoặc Database (ImageData)';
PRINT '5. Có thể đánh dấu hình ảnh chính bằng IsPrimary = 1';
PRINT '6. Sắp xếp hình ảnh bằng DisplayOrder (số nhỏ hơn hiển thị trước)';
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

