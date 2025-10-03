-- Script tối ưu hóa database indexes cho image storage
-- Date: 28/09/2025
-- Description: Tạo indexes tối ưu cho performance

USE [VnsErp2025Final]
GO

-- =============================================
-- 1. Tối ưu hóa indexes cho ProductImage table
-- =============================================

-- Index cho ProductId (được sử dụng nhiều nhất)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductImage_ProductId_IsActive' AND object_id = OBJECT_ID('dbo.ProductImage'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_ProductId_IsActive] 
    ON [dbo].[ProductImage] ([ProductId], [IsActive])
    INCLUDE ([Id], [ImagePath], [SortOrder], [IsPrimary], [ImageType], [ImageSize], [ImageWidth], [ImageHeight], [Caption], [AltText], [CreatedDate])
    PRINT 'Đã tạo index IX_ProductImage_ProductId_IsActive'
END
GO

-- Index cho IsPrimary (để tìm hình ảnh chính nhanh)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductImage_IsPrimary_ProductId' AND object_id = OBJECT_ID('dbo.ProductImage'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_IsPrimary_ProductId] 
    ON [dbo].[ProductImage] ([IsPrimary], [ProductId])
    WHERE [IsPrimary] = 1 AND [IsActive] = 1
    INCLUDE ([Id], [ImagePath], [ImageType], [ImageSize], [ImageWidth], [ImageHeight])
    PRINT 'Đã tạo index IX_ProductImage_IsPrimary_ProductId'
END
GO

-- Index cho SortOrder (để sắp xếp hình ảnh)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductImage_ProductId_SortOrder' AND object_id = OBJECT_ID('dbo.ProductImage'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_ProductId_SortOrder] 
    ON [dbo].[ProductImage] ([ProductId], [SortOrder])
    WHERE [IsActive] = 1
    INCLUDE ([Id], [ImagePath], [IsPrimary], [ImageType], [ImageSize])
    PRINT 'Đã tạo index IX_ProductImage_ProductId_SortOrder'
END
GO

-- Index cho CreatedDate (để cleanup old files)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductImage_CreatedDate' AND object_id = OBJECT_ID('dbo.ProductImage'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_CreatedDate] 
    ON [dbo].[ProductImage] ([CreatedDate])
    INCLUDE ([Id], [ImagePath], [IsActive])
    PRINT 'Đã tạo index IX_ProductImage_CreatedDate'
END
GO

-- Index cho ImageType (để filter theo loại ảnh)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductImage_ImageType' AND object_id = OBJECT_ID('dbo.ProductImage'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductImage_ImageType] 
    ON [dbo].[ProductImage] ([ImageType])
    WHERE [IsActive] = 1
    INCLUDE ([Id], [ProductId], [ImagePath], [ImageSize])
    PRINT 'Đã tạo index IX_ProductImage_ImageType'
END
GO

-- =============================================
-- 2. Tối ưu hóa indexes cho ProductService table
-- =============================================

-- Index cho ThumbnailImage (để check có thumbnail không)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductService_HasThumbnail' AND object_id = OBJECT_ID('dbo.ProductService'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductService_HasThumbnail] 
    ON [dbo].[ProductService] ([Id])
    WHERE [ThumbnailImage] IS NOT NULL
    INCLUDE ([Code], [Name], [CategoryId], [IsActive])
    PRINT 'Đã tạo index IX_ProductService_HasThumbnail'
END
GO

-- =============================================
-- 3. Tạo statistics để tối ưu query plan
-- =============================================

-- Statistics cho ProductImage
IF NOT EXISTS (SELECT * FROM sys.stats WHERE name = 'ST_ProductImage_ProductId' AND object_id = OBJECT_ID('dbo.ProductImage'))
BEGIN
    CREATE STATISTICS [ST_ProductImage_ProductId] ON [dbo].[ProductImage] ([ProductId])
    PRINT 'Đã tạo statistics ST_ProductImage_ProductId'
END
GO

IF NOT EXISTS (SELECT * FROM sys.stats WHERE name = 'ST_ProductImage_IsActive' AND object_id = OBJECT_ID('dbo.ProductImage'))
BEGIN
    CREATE STATISTICS [ST_ProductImage_IsActive] ON [dbo].[ProductImage] ([IsActive])
    PRINT 'Đã tạo statistics ST_ProductImage_IsActive'
END
GO

-- =============================================
-- 4. Tạo views để tối ưu queries thường dùng
-- =============================================

-- View cho danh sách hình ảnh active
IF EXISTS (SELECT * FROM sys.views WHERE name = 'VW_ProductImage_Active')
    DROP VIEW [dbo].[VW_ProductImage_Active]
GO

CREATE VIEW [dbo].[VW_ProductImage_Active]
AS
SELECT 
    [Id],
    [ProductId],
    [VariantId],
    [ImagePath],
    [SortOrder],
    [IsPrimary],
    [ImageType],
    [ImageSize],
    [ImageWidth],
    [ImageHeight],
    [Caption],
    [AltText],
    [CreatedDate],
    [ModifiedDate]
FROM [dbo].[ProductImage]
WHERE [IsActive] = 1
GO

PRINT 'Đã tạo view VW_ProductImage_Active'
GO

-- View cho hình ảnh chính
IF EXISTS (SELECT * FROM sys.views WHERE name = 'VW_ProductImage_Primary')
    DROP VIEW [dbo].[VW_ProductImage_Primary']
GO

CREATE VIEW [dbo].[VW_ProductImage_Primary]
AS
SELECT 
    [Id],
    [ProductId],
    [ImagePath],
    [ImageType],
    [ImageSize],
    [ImageWidth],
    [ImageHeight],
    [Caption],
    [AltText]
FROM [dbo].[ProductImage]
WHERE [IsPrimary] = 1 AND [IsActive] = 1
GO

PRINT 'Đã tạo view VW_ProductImage_Primary'
GO

-- =============================================
-- 5. Tạo stored procedures tối ưu
-- =============================================

-- SP để lấy danh sách hình ảnh của product
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetProductImages')
    DROP PROCEDURE [dbo].[SP_GetProductImages]
GO

CREATE PROCEDURE [dbo].[SP_GetProductImages]
    @ProductId UNIQUEIDENTIFIER,
    @IncludeImageData BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @IncludeImageData = 1
    BEGIN
        -- Lấy cả ImageData (chậm hơn)
        SELECT 
            [Id],
            [ProductId],
            [VariantId],
            [ImagePath],
            [SortOrder],
            [IsPrimary],
            [ImageData],
            [ImageType],
            [ImageSize],
            [ImageWidth],
            [ImageHeight],
            [Caption],
            [AltText],
            [CreatedDate],
            [ModifiedDate]
        FROM [dbo].[ProductImage]
        WHERE [ProductId] = @ProductId AND [IsActive] = 1
        ORDER BY [SortOrder], [CreatedDate]
    END
    ELSE
    BEGIN
        -- Chỉ lấy metadata (nhanh hơn)
        SELECT 
            [Id],
            [ProductId],
            [VariantId],
            [ImagePath],
            [SortOrder],
            [IsPrimary],
            [ImageType],
            [ImageSize],
            [ImageWidth],
            [ImageHeight],
            [Caption],
            [AltText],
            [CreatedDate],
            [ModifiedDate]
        FROM [dbo].[ProductImage]
        WHERE [ProductId] = @ProductId AND [IsActive] = 1
        ORDER BY [SortOrder], [CreatedDate]
    END
END
GO

PRINT 'Đã tạo stored procedure SP_GetProductImages'
GO

-- SP để lấy hình ảnh chính
IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'SP_GetPrimaryImage')
    DROP PROCEDURE [dbo].[SP_GetPrimaryImage']
GO

CREATE PROCEDURE [dbo].[SP_GetPrimaryImage]
    @ProductId UNIQUEIDENTIFIER,
    @IncludeImageData BIT = 0
AS
BEGIN
    SET NOCOUNT ON;
    
    IF @IncludeImageData = 1
    BEGIN
        SELECT 
            [Id],
            [ProductId],
            [ImagePath],
            [ImageData],
            [ImageType],
            [ImageSize],
            [ImageWidth],
            [ImageHeight],
            [Caption],
            [AltText]
        FROM [dbo].[ProductImage]
        WHERE [ProductId] = @ProductId AND [IsPrimary] = 1 AND [IsActive] = 1
    END
    ELSE
    BEGIN
        SELECT 
            [Id],
            [ProductId],
            [ImagePath],
            [ImageType],
            [ImageSize],
            [ImageWidth],
            [ImageHeight],
            [Caption],
            [AltText]
        FROM [dbo].[ProductImage]
        WHERE [ProductId] = @ProductId AND [IsPrimary] = 1 AND [IsActive] = 1
    END
END
GO

PRINT 'Đã tạo stored procedure SP_GetPrimaryImage'
GO

-- =============================================
-- 6. Cập nhật statistics
-- =============================================

-- Cập nhật statistics cho tất cả tables
UPDATE STATISTICS [dbo].[ProductImage] WITH FULLSCAN
UPDATE STATISTICS [dbo].[ProductService] WITH FULLSCAN

PRINT 'Đã cập nhật statistics cho tất cả tables'
GO

-- =============================================
-- 7. Tạo maintenance plan
-- =============================================

-- Tạo job để rebuild indexes định kỳ
IF EXISTS (SELECT * FROM msdb.dbo.sysjobs WHERE name = 'Maintenance_ImageIndexes')
    EXEC msdb.dbo.sp_delete_job @job_name = 'Maintenance_ImageIndexes'
GO

EXEC msdb.dbo.sp_add_job
    @job_name = 'Maintenance_ImageIndexes',
    @description = 'Maintenance job cho image indexes',
    @enabled = 1
GO

EXEC msdb.dbo.sp_add_jobstep
    @job_name = 'Maintenance_ImageIndexes',
    @step_name = 'Rebuild Image Indexes',
    @command = 'ALTER INDEX ALL ON [VnsErp2025Final].[dbo].[ProductImage] REBUILD WITH (ONLINE = OFF)',
    @database_name = 'VnsErp2025Final'
GO

EXEC msdb.dbo.sp_add_schedule
    @schedule_name = 'Weekly_ImageMaintenance',
    @freq_type = 8, -- Weekly
    @freq_interval = 1, -- Sunday
    @freq_recurrence_factor = 1,
    @active_start_time = 020000 -- 2:00 AM
GO

EXEC msdb.dbo.sp_attach_schedule
    @job_name = 'Maintenance_ImageIndexes',
    @schedule_name = 'Weekly_ImageMaintenance'
GO

PRINT 'Đã tạo maintenance job cho image indexes'
GO

PRINT 'Hoàn thành tối ưu hóa database indexes cho image storage'
