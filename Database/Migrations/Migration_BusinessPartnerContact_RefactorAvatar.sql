-- =============================================
-- Migration Script: Refactor Avatar Columns in BusinessPartnerContact Table
-- Description: Refactor cấu trúc lưu trữ Avatar của BusinessPartnerContact tương tự như BusinessPartner (Logo)
-- Reference: Tương tự như BusinessPartner (LogoFileName, LogoRelativePath, LogoFullPath, LogoStorageType, LogoFileSize, LogoChecksum, LogoThumbnailData)
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
    PRINT 'Bắt đầu migration: Refactor Avatar cho BusinessPartnerContact';
    PRINT '========================================';
    PRINT '';

    -- =============================================
    -- BƯỚC 1: Backup dữ liệu Avatar hiện có (nếu cần)
    -- =============================================
    PRINT 'Bước 1: Kiểm tra dữ liệu Avatar hiện có...';
    
    DECLARE @AvatarCount INT;
    SELECT @AvatarCount = COUNT(*) 
    FROM dbo.BusinessPartnerContact 
    WHERE Avatar IS NOT NULL OR AvatarPath IS NOT NULL;
    
    IF @AvatarCount > 0
    BEGIN
        PRINT '⚠ Cảnh báo: Có ' + CAST(@AvatarCount AS NVARCHAR(10)) + ' record có dữ liệu Avatar/AvatarPath';
        PRINT '   Lưu ý: Dữ liệu Avatar (Binary) sẽ bị mất khi xóa cột.';
        PRINT '   Nếu cần giữ lại, hãy backup dữ liệu trước khi tiếp tục.';
        PRINT '';
    END
    ELSE
    BEGIN
        PRINT '✓ Không có dữ liệu Avatar hiện có, an toàn để tiếp tục.';
        PRINT '';
    END
    GO

    -- =============================================
    -- BƯỚC 2: Thêm các cột mới (trước khi xóa cột cũ)
    -- =============================================
    PRINT 'Bước 2: Thêm các cột mới...';
    PRINT '';

    -- 2.1. Thêm cột AvatarFileName
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'AvatarFileName')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        ADD AvatarFileName NVARCHAR(255) NULL;
        
        PRINT '✓ Đã thêm cột AvatarFileName (NVARCHAR(255))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột AvatarFileName đã tồn tại, bỏ qua';
    END
    GO

    -- 2.2. Thêm cột AvatarRelativePath
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'AvatarRelativePath')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        ADD AvatarRelativePath NVARCHAR(500) NULL;
        
        PRINT '✓ Đã thêm cột AvatarRelativePath (NVARCHAR(500))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột AvatarRelativePath đã tồn tại, bỏ qua';
    END
    GO

    -- 2.3. Thêm cột AvatarFullPath
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'AvatarFullPath')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        ADD AvatarFullPath NVARCHAR(1000) NULL;
        
        PRINT '✓ Đã thêm cột AvatarFullPath (NVARCHAR(1000))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột AvatarFullPath đã tồn tại, bỏ qua';
    END
    GO

    -- 2.4. Thêm cột AvatarStorageType
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'AvatarStorageType')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        ADD AvatarStorageType NVARCHAR(20) NULL;
        
        PRINT '✓ Đã thêm cột AvatarStorageType (NVARCHAR(20))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột AvatarStorageType đã tồn tại, bỏ qua';
    END
    GO

    -- 2.5. Thêm cột AvatarFileSize
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'AvatarFileSize')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        ADD AvatarFileSize BIGINT NULL;
        
        PRINT '✓ Đã thêm cột AvatarFileSize (BIGINT)';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột AvatarFileSize đã tồn tại, bỏ qua';
    END
    GO

    -- 2.6. Thêm cột AvatarChecksum
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'AvatarChecksum')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        ADD AvatarChecksum NVARCHAR(64) NULL;
        
        PRINT '✓ Đã thêm cột AvatarChecksum (NVARCHAR(64))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột AvatarChecksum đã tồn tại, bỏ qua';
    END
    GO

    -- 2.7. Thêm cột AvatarThumbnailData (thay thế cho Avatar)
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'AvatarThumbnailData')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        ADD AvatarThumbnailData VARBINARY(MAX) NULL;
        
        PRINT '✓ Đã thêm cột AvatarThumbnailData (VARBINARY(MAX))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột AvatarThumbnailData đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- BƯỚC 3: Migrate dữ liệu từ AvatarPath sang AvatarFullPath (nếu có)
    -- =============================================
    PRINT '';
    PRINT 'Bước 3: Migrate dữ liệu từ AvatarPath sang AvatarFullPath...';
    
    UPDATE dbo.BusinessPartnerContact
    SET AvatarFullPath = AvatarPath
    WHERE AvatarPath IS NOT NULL 
      AND AvatarFullPath IS NULL;
    
    DECLARE @MigratedCount INT = @@ROWCOUNT;
    IF @MigratedCount > 0
    BEGIN
        PRINT '✓ Đã migrate ' + CAST(@MigratedCount AS NVARCHAR(10)) + ' record từ AvatarPath sang AvatarFullPath';
    END
    ELSE
    BEGIN
        PRINT '✓ Không có dữ liệu cần migrate';
    END
    GO

    -- =============================================
    -- BƯỚC 4: Xóa các cột cũ
    -- =============================================
    PRINT '';
    PRINT 'Bước 4: Xóa các cột cũ...';
    PRINT '';

    -- 4.1. Xóa cột Avatar (Binary)
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
               AND name = 'Avatar')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        DROP COLUMN Avatar;
        
        PRINT '✓ Đã xóa cột Avatar (VARBINARY(MAX))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột Avatar không tồn tại, bỏ qua';
    END
    GO

    -- 4.2. Xóa cột AvatarPath
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
               AND name = 'AvatarPath')
    BEGIN
        ALTER TABLE dbo.BusinessPartnerContact
        DROP COLUMN AvatarPath;
        
        PRINT '✓ Đã xóa cột AvatarPath (NVARCHAR(500))';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột AvatarPath không tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- BƯỚC 5: Tạo indexes (nếu cần tìm kiếm)
    -- =============================================
    PRINT '';
    PRINT 'Bước 5: Tạo indexes...';
    PRINT '';

    -- 5.1. Index cho AvatarFileName
    IF NOT EXISTS (SELECT 1 FROM sys.indexes 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'IX_BusinessPartnerContact_AvatarFileName')
    BEGIN
        CREATE NONCLUSTERED INDEX IX_BusinessPartnerContact_AvatarFileName
        ON dbo.BusinessPartnerContact(AvatarFileName)
        WHERE AvatarFileName IS NOT NULL;
        
        PRINT '✓ Đã tạo index IX_BusinessPartnerContact_AvatarFileName';
    END
    ELSE
    BEGIN
        PRINT '⚠ Index IX_BusinessPartnerContact_AvatarFileName đã tồn tại, bỏ qua';
    END
    GO

    -- 5.2. Index cho AvatarStorageType
    IF NOT EXISTS (SELECT 1 FROM sys.indexes 
                   WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
                   AND name = 'IX_BusinessPartnerContact_AvatarStorageType')
    BEGIN
        CREATE NONCLUSTERED INDEX IX_BusinessPartnerContact_AvatarStorageType
        ON dbo.BusinessPartnerContact(AvatarStorageType)
        WHERE AvatarStorageType IS NOT NULL;
        
        PRINT '✓ Đã tạo index IX_BusinessPartnerContact_AvatarStorageType';
    END
    ELSE
    BEGIN
        PRINT '⚠ Index IX_BusinessPartnerContact_AvatarStorageType đã tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- BƯỚC 6: Verify: Kiểm tra các cột đã được thêm/xóa
    -- =============================================
    PRINT '';
    PRINT '========================================';
    PRINT 'Kiểm tra kết quả migration:';
    PRINT '========================================';
    
    -- Kiểm tra các cột mới
    PRINT '';
    PRINT 'Các cột Avatar mới:';
    SELECT 
        c.name AS ColumnName,
        t.name AS DataType,
        c.max_length AS MaxLength,
        c.is_nullable AS IsNullable
    FROM sys.columns c
    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.BusinessPartnerContact')
      AND c.name LIKE 'Avatar%'
    ORDER BY c.name;
    
    -- Kiểm tra các cột cũ đã bị xóa
    PRINT '';
    PRINT 'Kiểm tra các cột cũ đã bị xóa:';
    DECLARE @OldColumnsExist INT = 0;
    
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
               AND name = 'Avatar')
        SET @OldColumnsExist = @OldColumnsExist + 1;
    
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartnerContact') 
               AND name = 'AvatarPath')
        SET @OldColumnsExist = @OldColumnsExist + 1;
    
    IF @OldColumnsExist = 0
    BEGIN
        PRINT '✓ Các cột cũ (Avatar, AvatarPath) đã được xóa thành công';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cảnh báo: Vẫn còn ' + CAST(@OldColumnsExist AS NVARCHAR(10)) + ' cột cũ chưa được xóa';
    END
    
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

