-- =============================================
-- Script: Điều chỉnh bảng ProductVariantIdentifierHistory
-- Description: 
--   - Xóa các cột: OldValue, NewValue, FieldName, Description
--   - Thêm cột: Value (nvarchar(1000) NULL)
-- Date: 2026-01-07
-- =============================================

USE [VnsErp2025Final]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

BEGIN TRANSACTION
BEGIN TRY

    PRINT 'Bắt đầu điều chỉnh bảng ProductVariantIdentifierHistory...'
    
    -- =============================================
    -- Xóa các cột cũ: OldValue, NewValue, FieldName, Description
    -- =============================================
    
    -- Xóa cột OldValue
    IF EXISTS (
        SELECT 1 
        FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantIdentifierHistory]') 
        AND name = 'OldValue'
    )
    BEGIN
        ALTER TABLE [dbo].[ProductVariantIdentifierHistory]
        DROP COLUMN [OldValue];
        
        PRINT 'Đã xóa cột OldValue.';
    END
    ELSE
    BEGIN
        PRINT 'Cột OldValue không tồn tại.';
    END
    
    -- Xóa cột NewValue
    IF EXISTS (
        SELECT 1 
        FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantIdentifierHistory]') 
        AND name = 'NewValue'
    )
    BEGIN
        ALTER TABLE [dbo].[ProductVariantIdentifierHistory]
        DROP COLUMN [NewValue];
        
        PRINT 'Đã xóa cột NewValue.';
    END
    ELSE
    BEGIN
        PRINT 'Cột NewValue không tồn tại.';
    END
    
    -- Xóa cột FieldName
    IF EXISTS (
        SELECT 1 
        FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantIdentifierHistory]') 
        AND name = 'FieldName'
    )
    BEGIN
        ALTER TABLE [dbo].[ProductVariantIdentifierHistory]
        DROP COLUMN [FieldName];
        
        PRINT 'Đã xóa cột FieldName.';
    END
    ELSE
    BEGIN
        PRINT 'Cột FieldName không tồn tại.';
    END
    
    -- Xóa cột Description
    IF EXISTS (
        SELECT 1 
        FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantIdentifierHistory]') 
        AND name = 'Description'
    )
    BEGIN
        ALTER TABLE [dbo].[ProductVariantIdentifierHistory]
        DROP COLUMN [Description];
        
        PRINT 'Đã xóa cột Description.';
    END
    ELSE
    BEGIN
        PRINT 'Cột Description không tồn tại.';
    END
    
    -- =============================================
    -- Thêm cột mới: Value
    -- =============================================
    
    IF NOT EXISTS (
        SELECT 1 
        FROM sys.columns 
        WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantIdentifierHistory]') 
        AND name = 'Value'
    )
    BEGIN
        ALTER TABLE [dbo].[ProductVariantIdentifierHistory]
        ADD [Value] [nvarchar](1000) NULL;
        
        -- Thêm Extended Property cho cột Value
        EXEC sys.sp_addextendedproperty 
            @name = N'MS_Description', 
            @value = N'Giá trị thay đổi (thay thế cho OldValue/NewValue/FieldName/Description)', 
            @level0type = N'SCHEMA', @level0name = N'dbo', 
            @level1type = N'TABLE', @level1name = N'ProductVariantIdentifierHistory', 
            @level2type = N'COLUMN', @level2name = N'Value';
        
        PRINT 'Đã thêm cột Value.';
    END
    ELSE
    BEGIN
        PRINT 'Cột Value đã tồn tại.';
    END
    
    COMMIT TRANSACTION
    PRINT 'Hoàn tất điều chỉnh bảng ProductVariantIdentifierHistory.'
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION
    
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
    DECLARE @ErrorState INT = ERROR_STATE()
    
    PRINT 'Lỗi khi điều chỉnh bảng: ' + @ErrorMessage
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
GO

-- =============================================
-- Kiểm tra kết quả
-- =============================================
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'ProductVariantIdentifierHistory'
ORDER BY ORDINAL_POSITION;
GO
