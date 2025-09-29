-- Script để cập nhật ProductService table: bỏ ThumbnailPath, thêm ThumbnailImage
-- Date: 28/09/2025

USE [VnsErp2025Final]
GO

-- Kiểm tra và xóa tất cả foreign key constraints liên quan đến ProductService
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProductService_Category')
BEGIN
    ALTER TABLE [dbo].[ProductService] DROP CONSTRAINT [FK_ProductService_Category]
    PRINT 'Đã xóa FK_ProductService_Category'
END
GO

-- Kiểm tra và xóa tất cả indexes trước
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductService_CategoryId' AND object_id = OBJECT_ID('dbo.ProductService'))
BEGIN
    DROP INDEX [IX_ProductService_CategoryId] ON [dbo].[ProductService]
    PRINT 'Đã xóa IX_ProductService_CategoryId'
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductService_IsActive' AND object_id = OBJECT_ID('dbo.ProductService'))
BEGIN
    DROP INDEX [IX_ProductService_IsActive] ON [dbo].[ProductService]
    PRINT 'Đã xóa IX_ProductService_IsActive'
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductService_IsService' AND object_id = OBJECT_ID('dbo.ProductService'))
BEGIN
    DROP INDEX [IX_ProductService_IsService] ON [dbo].[ProductService]
    PRINT 'Đã xóa IX_ProductService_IsService'
END
GO

-- Backup dữ liệu hiện tại
SELECT * INTO ProductService_Backup_$(FORMAT(GETDATE(), 'yyyyMMdd_HHmmss')) FROM ProductService
GO

-- Drop table hiện tại
DROP TABLE [dbo].[ProductService]
GO

-- Tạo lại ProductService table với ThumbnailImage thay vì ThumbnailPath
CREATE TABLE [dbo].[ProductService](
	[Id] [uniqueidentifier] NOT NULL DEFAULT (newid()),
	[Code] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[CategoryId] [uniqueidentifier] NULL,
	[IsService] [bit] NOT NULL DEFAULT ((0)),
	[Description] [nvarchar](500) NULL,
	[IsActive] [bit] NOT NULL DEFAULT ((1)),
	[ThumbnailImage] [varbinary](max) NULL,
 CONSTRAINT [PK_ProductService] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ_ProductService_Code] UNIQUE NONCLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Thêm lại foreign key constraint
ALTER TABLE [dbo].[ProductService]  WITH CHECK ADD  CONSTRAINT [FK_ProductService_Category] FOREIGN KEY([CategoryId])
REFERENCES [dbo].[ProductServiceCategory] ([Id])
GO

ALTER TABLE [dbo].[ProductService] CHECK CONSTRAINT [FK_ProductService_Category]
GO

-- Thêm lại indexes cho performance
CREATE INDEX [IX_ProductService_CategoryId] ON [dbo].[ProductService] ([CategoryId])
GO

CREATE INDEX [IX_ProductService_IsActive] ON [dbo].[ProductService] ([IsActive])
GO

CREATE INDEX [IX_ProductService_IsService] ON [dbo].[ProductService] ([IsService])
GO

-- Restore dữ liệu từ backup (loại bỏ ThumbnailPath column)
DECLARE @BackupTableName NVARCHAR(255)
SELECT TOP 1 @BackupTableName = name FROM sys.tables WHERE name LIKE 'ProductService_Backup_%' ORDER BY create_date DESC

IF @BackupTableName IS NOT NULL
BEGIN
    EXEC('INSERT INTO ProductService (Id, Code, Name, CategoryId, IsService, Description, IsActive)
          SELECT Id, Code, Name, CategoryId, IsService, Description, IsActive 
          FROM [' + @BackupTableName + ']')
    PRINT 'Đã restore dữ liệu từ backup table: ' + @BackupTableName
END
ELSE
BEGIN
    PRINT 'Không tìm thấy backup table để restore dữ liệu'
END
GO

PRINT 'ProductService table đã được cập nhật thành công - ThumbnailPath đã được thay thế bằng ThumbnailImage';
