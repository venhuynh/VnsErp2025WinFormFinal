-- Script để thêm trường ImageData vào bảng ProductImage
-- Date: 28/09/2025
-- Description: Thêm khả năng lưu trữ dữ liệu hình ảnh trực tiếp trong database

USE [VnsErp2025Final]
GO

-- Bước 1: Thêm cột ImageData để lưu binary data của ảnh
ALTER TABLE [dbo].[ProductImage]
ADD [ImageData] [varbinary](max) NULL
GO

-- Bước 2: Thêm cột ImageType để lưu loại file ảnh (jpg, png, gif, bmp)
ALTER TABLE [dbo].[ProductImage]
ADD [ImageType] [nvarchar](10) NULL
GO

-- Bước 3: Thêm cột ImageSize để lưu kích thước file ảnh (bytes)
ALTER TABLE [dbo].[ProductImage]
ADD [ImageSize] [bigint] NULL
GO

-- Bước 4: Thêm cột ImageWidth và ImageHeight để lưu kích thước pixel
ALTER TABLE [dbo].[ProductImage]
ADD [ImageWidth] [int] NULL
GO

ALTER TABLE [dbo].[ProductImage]
ADD [ImageHeight] [int] NULL
GO

-- Bước 5: Thêm cột Caption để lưu mô tả ảnh
ALTER TABLE [dbo].[ProductImage]
ADD [Caption] [nvarchar](255) NULL
GO

-- Bước 6: Thêm cột AltText để SEO và accessibility
ALTER TABLE [dbo].[ProductImage]
ADD [AltText] [nvarchar](255) NULL
GO

-- Bước 7: Thêm cột IsActive để enable/disable ảnh
ALTER TABLE [dbo].[ProductImage]
ADD [IsActive] [bit] NULL DEFAULT (1)
GO

-- Bước 8: Thêm cột CreatedDate và ModifiedDate để tracking
ALTER TABLE [dbo].[ProductImage]
ADD [CreatedDate] [datetime] NULL DEFAULT (getdate())
GO

ALTER TABLE [dbo].[ProductImage]
ADD [ModifiedDate] [datetime] NULL
GO

-- Bước 9: Thêm comment cho các cột mới
EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Dữ liệu binary của hình ảnh (varbinary(max))', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'ImageData'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Loại file ảnh (jpg, png, gif, bmp)', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'ImageType'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Kích thước file ảnh tính bằng bytes', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'ImageSize'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Chiều rộng ảnh tính bằng pixel', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'ImageWidth'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Chiều cao ảnh tính bằng pixel', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'ImageHeight'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Mô tả/chú thích cho hình ảnh', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'Caption'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Text thay thế cho accessibility và SEO', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'AltText'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Trạng thái hoạt động của ảnh (1=active, 0=inactive)', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'IsActive'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Ngày tạo ảnh', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'CreatedDate'
GO

EXEC sys.sp_addextendedproperty 
    @name = N'MS_Description', 
    @value = N'Ngày cập nhật ảnh lần cuối', 
    @level0type = N'SCHEMA', @level0name = N'dbo', 
    @level1type = N'TABLE', @level1name = N'ProductImage', 
    @level2type = N'COLUMN', @level2name = N'ModifiedDate'
GO

-- Bước 10: Tạo index để tối ưu performance
CREATE NONCLUSTERED INDEX [IX_ProductImage_ProductId_IsActive_SortOrder] 
ON [dbo].[ProductImage] ([ProductId] ASC, [IsActive] ASC, [SortOrder] ASC)
INCLUDE ([Id], [ImagePath], [ImageData], [ImageType], [IsPrimary])
GO

CREATE NONCLUSTERED INDEX [IX_ProductImage_VariantId_IsActive_SortOrder] 
ON [dbo].[ProductImage] ([VariantId] ASC, [IsActive] ASC, [SortOrder] ASC)
INCLUDE ([Id], [ImagePath], [ImageData], [ImageType], [IsPrimary])
GO

-- Bước 11: Tạo constraint để đảm bảo ImageType hợp lệ
ALTER TABLE [dbo].[ProductImage]
ADD CONSTRAINT [CK_ProductImage_ImageType] 
CHECK ([ImageType] IS NULL OR [ImageType] IN ('jpg', 'jpeg', 'png', 'gif', 'bmp', 'webp'))
GO

-- Bước 12: Tạo constraint để đảm bảo ImageSize hợp lệ
ALTER TABLE [dbo].[ProductImage]
ADD CONSTRAINT [CK_ProductImage_ImageSize] 
CHECK ([ImageSize] IS NULL OR [ImageSize] > 0)
GO

-- Bước 13: Tạo constraint để đảm bảo kích thước pixel hợp lệ
ALTER TABLE [dbo].[ProductImage]
ADD CONSTRAINT [CK_ProductImage_ImageDimensions] 
CHECK (([ImageWidth] IS NULL AND [ImageHeight] IS NULL) OR ([ImageWidth] > 0 AND [ImageHeight] > 0))
GO

-- Bước 14: Tạo constraint để đảm bảo SortOrder hợp lệ
ALTER TABLE [dbo].[ProductImage]
ADD CONSTRAINT [CK_ProductImage_SortOrder] 
CHECK ([SortOrder] IS NULL OR [SortOrder] >= 0)
GO

PRINT 'Đã thêm thành công các trường mới vào bảng ProductImage:'
PRINT '- ImageData: Lưu trữ dữ liệu binary của ảnh'
PRINT '- ImageType: Loại file ảnh (jpg, png, gif, bmp, webp)'
PRINT '- ImageSize: Kích thước file tính bằng bytes'
PRINT '- ImageWidth: Chiều rộng ảnh (pixel)'
PRINT '- ImageHeight: Chiều cao ảnh (pixel)'
PRINT '- Caption: Mô tả/chú thích ảnh'
PRINT '- AltText: Text thay thế cho accessibility'
PRINT '- IsActive: Trạng thái hoạt động'
PRINT '- CreatedDate: Ngày tạo'
PRINT '- ModifiedDate: Ngày cập nhật'
PRINT 'Đã tạo index và constraints để tối ưu performance và đảm bảo data integrity'
GO
