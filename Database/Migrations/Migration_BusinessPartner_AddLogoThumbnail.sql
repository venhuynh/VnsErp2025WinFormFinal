-- =============================================
-- Migration Script: Add Logo Thumbnail Columns to BusinessPartner Table
-- Description: Thêm các cột để lưu thumbnail của logo BusinessPartner
-- Reference: Tương tự như các cột Logo, nhưng cho thumbnail (kích thước nhỏ hơn)
-- Date: 2025
-- =============================================

USE [VnsErp2025Final];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

-- Kiểm tra và bắt đầu transaction
IF @@TRANCOUNT = 0
BEGIN
    BEGIN TRANSACTION;
END
GO

BEGIN TRY
    PRINT '========================================';
    PRINT 'Bắt đầu migration: Thêm cột Logo Thumbnail cho BusinessPartner';
    PRINT '========================================';
    PRINT '';

    -- =============================================
    -- 1. Thêm cột LogoThumbnailImage (Binary data)
    -- =============================================
    -- Note: Tên cột là LogoThumbnailImage (không phải LogoThumbnail) để nhất quán với pattern
    -- Tham khảo: ProductService.ThumbnailImage, ProductVariant.ThumbnailImage
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'LogoThumbnailImage')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        ADD LogoThumbnailImage VARBINARY(MAX) NULL;
        
        PRINT '✓ Đã thêm cột LogoThumbnailImage (VARBINARY(MAX))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột LogoThumbnailImage đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 2. Thêm cột LogoThumbnailFileName
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'LogoThumbnailFileName')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        ADD LogoThumbnailFileName NVARCHAR(255) NULL;
        
        PRINT '✓ Đã thêm cột LogoThumbnailFileName (NVARCHAR(255))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột LogoThumbnailFileName đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 3. Thêm cột LogoThumbnailRelativePath
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'LogoThumbnailRelativePath')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        ADD LogoThumbnailRelativePath NVARCHAR(500) NULL;
        
        PRINT '✓ Đã thêm cột LogoThumbnailRelativePath (NVARCHAR(500))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột LogoThumbnailRelativePath đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 4. Thêm cột LogoThumbnailFullPath
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'LogoThumbnailFullPath')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        ADD LogoThumbnailFullPath NVARCHAR(1000) NULL;
        
        PRINT '✓ Đã thêm cột LogoThumbnailFullPath (NVARCHAR(1000))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột LogoThumbnailFullPath đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 5. Thêm cột LogoThumbnailStorageType
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'LogoThumbnailStorageType')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        ADD LogoThumbnailStorageType NVARCHAR(20) NULL;
        
        PRINT '✓ Đã thêm cột LogoThumbnailStorageType (NVARCHAR(20))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột LogoThumbnailStorageType đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 6. Thêm cột LogoThumbnailFileSize
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'LogoThumbnailFileSize')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        ADD LogoThumbnailFileSize BIGINT NULL;
        
        PRINT '✓ Đã thêm cột LogoThumbnailFileSize (BIGINT)';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột LogoThumbnailFileSize đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 7. Thêm cột LogoThumbnailChecksum
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'LogoThumbnailChecksum')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        ADD LogoThumbnailChecksum NVARCHAR(64) NULL;
        
        PRINT '✓ Đã thêm cột LogoThumbnailChecksum (NVARCHAR(64))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột LogoThumbnailChecksum đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 8. Thêm index cho LogoThumbnailFileName (nếu cần tìm kiếm)
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.indexes 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'IX_BusinessPartner_LogoThumbnailFileName')
    BEGIN
        CREATE NONCLUSTERED INDEX IX_BusinessPartner_LogoThumbnailFileName
        ON dbo.BusinessPartner(LogoThumbnailFileName)
        WHERE LogoThumbnailFileName IS NOT NULL;
        
        PRINT '✓ Đã tạo index IX_BusinessPartner_LogoThumbnailFileName';
    END
    ELSE
    BEGIN
        PRINT '⚠ Index IX_BusinessPartner_LogoThumbnailFileName đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 9. Thêm index cho LogoThumbnailStorageType (nếu cần filter)
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.indexes 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
                   AND name = 'IX_BusinessPartner_LogoThumbnailStorageType')
    BEGIN
        CREATE NONCLUSTERED INDEX IX_BusinessPartner_LogoThumbnailStorageType
        ON dbo.BusinessPartner(LogoThumbnailStorageType)
        WHERE LogoThumbnailStorageType IS NOT NULL;
        
        PRINT '✓ Đã tạo index IX_BusinessPartner_LogoThumbnailStorageType';
    END
    ELSE
    BEGIN
        PRINT '⚠ Index IX_BusinessPartner_LogoThumbnailStorageType đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 10. Verify: Kiểm tra các cột đã được thêm
    -- =============================================
    PRINT '';
    PRINT '========================================';
    PRINT 'Kiểm tra kết quả migration:';
    PRINT '========================================';
    
    SELECT 
        c.name AS ColumnName,
        t.name AS DataType,
        c.max_length AS MaxLength,
        c.is_nullable AS IsNullable
    FROM sys.columns c
    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.BusinessPartner')
      AND (c.name LIKE 'LogoThumbnail%' OR c.name = 'LogoThumbnailImage')
    ORDER BY c.name;
    
    PRINT '';
    PRINT '========================================';
    PRINT 'Migration hoàn tất thành công!';
    PRINT '========================================';

    -- Commit transaction nếu đã bắt đầu
    IF @@TRANCOUNT > 0
    BEGIN
        COMMIT TRANSACTION;
        PRINT 'Transaction đã được commit.';
    END

END TRY
BEGIN CATCH
    -- Rollback transaction nếu có lỗi
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION;
        PRINT 'Transaction đã được rollback do lỗi.';
    END

    -- Hiển thị thông tin lỗi
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
    DECLARE @ErrorState INT = ERROR_STATE();
    
    PRINT '';
    PRINT '========================================';
    PRINT 'LỖI KHI THỰC HIỆN MIGRATION:';
    PRINT '========================================';
    PRINT 'Error Message: ' + @ErrorMessage;
    PRINT 'Error Severity: ' + CAST(@ErrorSeverity AS NVARCHAR(10));
    PRINT 'Error State: ' + CAST(@ErrorState AS NVARCHAR(10));
    PRINT '';
    
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
END CATCH
GO

-- Đảm bảo transaction được commit nếu chưa commit
IF @@TRANCOUNT > 0
BEGIN
    COMMIT TRANSACTION;
END
GO

