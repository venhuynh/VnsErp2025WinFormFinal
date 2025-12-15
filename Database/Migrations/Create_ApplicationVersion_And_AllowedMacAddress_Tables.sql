-- =============================================
-- Script: Tạo bảng ApplicationVersion và AllowedMacAddress
-- Mục đích: Quản lý phiên bản ứng dụng và danh sách MAC address được phép
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
PRINT 'Bắt đầu tạo bảng ApplicationVersion và AllowedMacAddress';
PRINT '========================================';
GO

-- =============================================
-- Tạo bảng ApplicationVersion
-- =============================================

-- Kiểm tra và xóa bảng nếu đã tồn tại
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ApplicationVersion' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '⚠️  Bảng ApplicationVersion đã tồn tại. Đang xóa...';
    
    -- Xóa Extended Properties
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.ApplicationVersion') AND name = 'MS_Description' AND minor_id = 0)
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationVersion';
    
    -- Xóa Default Constraints
    DECLARE @sql1 NVARCHAR(MAX) = '';
    SELECT @sql1 += 'ALTER TABLE [dbo].[ApplicationVersion] DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.ApplicationVersion');
    IF @sql1 <> '' EXEC sp_executesql @sql1;
    
    -- Xóa bảng
    DROP TABLE [dbo].[ApplicationVersion];
    PRINT '✅ Đã xóa bảng ApplicationVersion cũ';
END
GO

-- Tạo bảng ApplicationVersion
CREATE TABLE [dbo].[ApplicationVersion](
    [Id] [uniqueidentifier] NOT NULL,
    [Version] [nvarchar](50) NOT NULL,
    [ReleaseDate] [datetime] NOT NULL,
    [IsActive] [bit] NOT NULL DEFAULT ((1)),
    [Description] [nvarchar](500) NULL,
    [CreateDate] [datetime] NOT NULL DEFAULT (GETDATE()),
    [CreateBy] [uniqueidentifier] NULL,
    [ModifiedDate] [datetime] NULL,
    [ModifiedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_ApplicationVersion] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

SET ANSI_PADDING OFF;
GO

PRINT '✅ Đã tạo bảng ApplicationVersion';
GO

-- Index cho Version
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ApplicationVersion') AND name = 'IX_ApplicationVersion_Version')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_ApplicationVersion_Version] 
    ON [dbo].[ApplicationVersion]([Version]);
    PRINT '✅ Đã tạo index IX_ApplicationVersion_Version';
END
GO

-- Index cho IsActive
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.ApplicationVersion') AND name = 'IX_ApplicationVersion_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ApplicationVersion_IsActive] 
    ON [dbo].[ApplicationVersion]([IsActive]);
    PRINT '✅ Đã tạo index IX_ApplicationVersion_IsActive';
END
GO

-- Extended Properties
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảng quản lý phiên bản ứng dụng. Chỉ có một phiên bản Active tại một thời điểm.', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationVersion';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phiên bản ứng dụng (ví dụ: 1.0.0.0)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationVersion', @level2type=N'COLUMN',@level2name=N'Version';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ngày phát hành phiên bản', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationVersion', @level2type=N'COLUMN',@level2name=N'ReleaseDate';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Phiên bản có đang hoạt động không (chỉ có một phiên bản Active)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ApplicationVersion', @level2type=N'COLUMN',@level2name=N'IsActive';
GO

-- =============================================
-- Tạo bảng AllowedMacAddress
-- =============================================

-- Kiểm tra và xóa bảng nếu đã tồn tại
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'AllowedMacAddress' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '⚠️  Bảng AllowedMacAddress đã tồn tại. Đang xóa...';
    
    -- Xóa Extended Properties
    IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.AllowedMacAddress') AND name = 'MS_Description' AND minor_id = 0)
        EXEC sys.sp_dropextendedproperty @name=N'MS_Description', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllowedMacAddress';
    
    -- Xóa Default Constraints
    DECLARE @sql2 NVARCHAR(MAX) = '';
    SELECT @sql2 += 'ALTER TABLE [dbo].[AllowedMacAddress] DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13)
    FROM sys.default_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.AllowedMacAddress');
    IF @sql2 <> '' EXEC sp_executesql @sql2;
    
    -- Xóa bảng
    DROP TABLE [dbo].[AllowedMacAddress];
    PRINT '✅ Đã xóa bảng AllowedMacAddress cũ';
END
GO

SET ANSI_PADDING ON;
GO

-- Tạo bảng AllowedMacAddress
CREATE TABLE [dbo].[AllowedMacAddress](
    [Id] [uniqueidentifier] NOT NULL,
    [MacAddress] [nvarchar](50) NOT NULL,
    [ComputerName] [nvarchar](255) NULL,
    [Description] [nvarchar](500) NULL,
    [IsActive] [bit] NOT NULL DEFAULT ((1)),
    [CreateDate] [datetime] NOT NULL DEFAULT (GETDATE()),
    [CreateBy] [uniqueidentifier] NULL,
    [ModifiedDate] [datetime] NULL,
    [ModifiedBy] [uniqueidentifier] NULL,
 CONSTRAINT [PK_AllowedMacAddress] PRIMARY KEY CLUSTERED 
(
    [Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY];
GO

SET ANSI_PADDING OFF;
GO

PRINT '✅ Đã tạo bảng AllowedMacAddress';
GO

-- Index cho MacAddress (Unique)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.AllowedMacAddress') AND name = 'IX_AllowedMacAddress_MacAddress')
BEGIN
    CREATE UNIQUE NONCLUSTERED INDEX [IX_AllowedMacAddress_MacAddress] 
    ON [dbo].[AllowedMacAddress]([MacAddress]);
    PRINT '✅ Đã tạo index IX_AllowedMacAddress_MacAddress';
END
GO

-- Index cho IsActive
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.AllowedMacAddress') AND name = 'IX_AllowedMacAddress_IsActive')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AllowedMacAddress_IsActive] 
    ON [dbo].[AllowedMacAddress]([IsActive]);
    PRINT '✅ Đã tạo index IX_AllowedMacAddress_IsActive';
END
GO

-- Extended Properties
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bảng quản lý danh sách MAC address được phép sử dụng ứng dụng', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllowedMacAddress';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Địa chỉ MAC của máy tính (format: XX-XX-XX-XX-XX-XX hoặc XXXXXXXXXXXX)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllowedMacAddress', @level2type=N'COLUMN',@level2name=N'MacAddress';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tên máy tính (tùy chọn)', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllowedMacAddress', @level2type=N'COLUMN',@level2name=N'ComputerName';
GO

EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MAC address có đang được phép sử dụng không', @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AllowedMacAddress', @level2type=N'COLUMN',@level2name=N'IsActive';
GO

PRINT '';
PRINT '========================================';
PRINT 'Hoàn thành tạo bảng ApplicationVersion và AllowedMacAddress';
PRINT '========================================';
PRINT '';
PRINT 'Tóm tắt:';
PRINT '- Đã tạo bảng ApplicationVersion để quản lý phiên bản ứng dụng';
PRINT '- Đã tạo bảng AllowedMacAddress để quản lý MAC address được phép';
PRINT '- Đã tạo các indexes cần thiết';
PRINT '- Đã thêm extended properties (comments)';
PRINT '';
PRINT 'Lưu ý:';
PRINT '1. Cần refresh DataContext trong Visual Studio';
PRINT '2. Cần regenerate Entity từ DBML hoặc designer.cs';
PRINT '3. Cần insert phiên bản đầu tiên vào bảng ApplicationVersion';
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
