-- =============================================
-- Migration Script: Add Discount Columns to StockInOutMaster Table
-- Description: Thêm các cột chiết khấu vào bảng StockInOutMaster
-- Columns:
--   - DiscountPercentage: % chiết khấu (Decimal(18,2))
--   - DiscountAmount: Số tiền chiết khấu (Decimal(18,2))
--   - TotalAmountAfterDiscount: Thành tiền sau chiết khấu (Decimal(18,2))
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
    PRINT 'Bắt đầu migration: Thêm cột chiết khấu cho StockInOutMaster';
    PRINT '========================================';
    PRINT '';

    -- =============================================
    -- 1. Thêm cột DiscountPercentage (% chiết khấu)
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.StockInOutMaster') 
                   AND name = 'DiscountPercentage')
    BEGIN
        ALTER TABLE dbo.StockInOutMaster
        ADD DiscountPercentage DECIMAL(18,2) NULL;
        
        PRINT '✓ Đã thêm cột DiscountPercentage (DECIMAL(18,2)) - % chiết khấu';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột DiscountPercentage đã tồn tại, bỏ qua';
    END

    -- =============================================
    -- 2. Thêm cột DiscountAmount (Số tiền chiết khấu)
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.StockInOutMaster') 
                   AND name = 'DiscountAmount')
    BEGIN
        ALTER TABLE dbo.StockInOutMaster
        ADD DiscountAmount DECIMAL(18,2) NULL;
        
        PRINT '✓ Đã thêm cột DiscountAmount (DECIMAL(18,2)) - Số tiền chiết khấu';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột DiscountAmount đã tồn tại, bỏ qua';
    END

    -- =============================================
    -- 3. Thêm cột TotalAmountAfterDiscount (Thành tiền sau chiết khấu)
    -- =============================================
    IF NOT EXISTS (SELECT 1 FROM sys.columns 
                   WHERE object_id = OBJECT_ID(N'dbo.StockInOutMaster') 
                   AND name = 'TotalAmountAfterDiscount')
    BEGIN
        ALTER TABLE dbo.StockInOutMaster
        ADD TotalAmountAfterDiscount DECIMAL(18,2) NULL;
        
        PRINT '✓ Đã thêm cột TotalAmountAfterDiscount (DECIMAL(18,2)) - Thành tiền sau chiết khấu';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột TotalAmountAfterDiscount đã tồn tại, bỏ qua';
    END

    -- =============================================
    -- 4. Verify: Kiểm tra các cột đã được thêm
    -- =============================================
    PRINT '';
    PRINT '========================================';
    PRINT 'Kiểm tra kết quả migration:';
    PRINT '========================================';
    
    SELECT 
        c.name AS ColumnName,
        t.name AS DataType,
        c.precision AS Precision,
        c.scale AS Scale,
        c.is_nullable AS IsNullable
    FROM sys.columns c
    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.StockInOutMaster')
      AND c.name IN ('DiscountPercentage', 'DiscountAmount', 'TotalAmountAfterDiscount')
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
