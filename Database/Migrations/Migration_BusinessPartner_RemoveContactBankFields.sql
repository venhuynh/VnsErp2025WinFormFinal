-- =============================================
-- Migration Script: Remove Contact, Bank, and Payment Fields from BusinessPartner Table
-- Description: Xóa các cột liên hệ, ngân hàng, thanh toán khỏi bảng BusinessPartner
--              vì sẽ tách ra làm bảng riêng
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
    PRINT 'Bắt đầu migration: Xóa các cột liên hệ, ngân hàng, thanh toán khỏi BusinessPartner';
    PRINT '========================================';
    PRINT '';
    PRINT 'LƯU Ý: Các cột sau sẽ bị XÓA vĩnh viễn:';
    PRINT '  - ContactPerson (Người liên hệ)';
    PRINT '  - ContactPosition (Chức vụ)';
    PRINT '  - BankAccount (Số tài khoản)';
    PRINT '  - BankName (Tên ngân hàng)';
    PRINT '  - CreditLimit (Hạn mức tín dụng)';
    PRINT '  - PaymentTerm (Điều khoản thanh toán)';
    PRINT '';
    PRINT 'Vui lòng đảm bảo đã backup dữ liệu trước khi chạy script này!';
    PRINT '';

    -- =============================================
    -- 0. Xóa constraint CK_BusinessPartner_CreditLimit trước (nếu có)
    -- =============================================
    PRINT '';
    PRINT '--- Bước 0: Xóa constraint liên quan đến CreditLimit ---';
    
    DECLARE @CreditLimitConstraintName NVARCHAR(255);
    
    -- Tìm constraint liên quan đến CreditLimit
    SELECT @CreditLimitConstraintName = name
    FROM sys.check_constraints
    WHERE parent_object_id = OBJECT_ID(N'dbo.BusinessPartner')
      AND (name = 'CK_BusinessPartner_CreditLimit' OR definition LIKE '%CreditLimit%');
    
    IF @CreditLimitConstraintName IS NOT NULL
    BEGIN
        BEGIN TRY
            DECLARE @DropConstraintSQL NVARCHAR(MAX) = 
                'ALTER TABLE dbo.BusinessPartner DROP CONSTRAINT [' + @CreditLimitConstraintName + ']';
            EXEC sp_executesql @DropConstraintSQL;
            PRINT '✓ Đã xóa constraint: ' + @CreditLimitConstraintName;
        END TRY
        BEGIN CATCH
            DECLARE @ErrorMsg NVARCHAR(4000) = ERROR_MESSAGE();
            PRINT '⚠ Lỗi khi xóa constraint ' + @CreditLimitConstraintName + ': ' + @ErrorMsg;
            -- Không throw error, tiếp tục xử lý
        END CATCH
    END
    ELSE
    BEGIN
        PRINT '⚠ Không tìm thấy constraint liên quan đến CreditLimit, bỏ qua';
    END
    GO

    -- =============================================
    -- 1. Xóa cột ContactPerson
    -- =============================================
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
               AND name = 'ContactPerson')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        DROP COLUMN ContactPerson;
        
        PRINT '✓ Đã xóa cột ContactPerson';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột ContactPerson không tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 2. Xóa cột ContactPosition
    -- =============================================
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
               AND name = 'ContactPosition')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        DROP COLUMN ContactPosition;
        
        PRINT '✓ Đã xóa cột ContactPosition';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột ContactPosition không tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 3. Xóa cột BankAccount
    -- =============================================
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
               AND name = 'BankAccount')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        DROP COLUMN BankAccount;
        
        PRINT '✓ Đã xóa cột BankAccount';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột BankAccount không tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 4. Xóa cột BankName
    -- =============================================
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
               AND name = 'BankName')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        DROP COLUMN BankName;
        
        PRINT '✓ Đã xóa cột BankName';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột BankName không tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 5. Xóa cột CreditLimit
    -- =============================================
    -- Lưu ý: Constraint đã được xóa ở Bước 0
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
               AND name = 'CreditLimit')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        DROP COLUMN CreditLimit;
        
        PRINT '✓ Đã xóa cột CreditLimit';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột CreditLimit không tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 6. Xóa cột PaymentTerm
    -- =============================================
    IF EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.BusinessPartner') 
               AND name = 'PaymentTerm')
    BEGIN
        ALTER TABLE dbo.BusinessPartner
        DROP COLUMN PaymentTerm;
        
        PRINT '✓ Đã xóa cột PaymentTerm';
    END
    ELSE
    BEGIN
        PRINT '⚠ Cột PaymentTerm không tồn tại, bỏ qua';
    END
    GO

    -- =============================================
    -- 7. Verify: Kiểm tra các cột đã được xóa
    -- =============================================
    PRINT '';
    PRINT '========================================';
    PRINT 'Kiểm tra kết quả migration:';
    PRINT '========================================';
    
    -- Kiểm tra các cột còn lại trong bảng
    SELECT 
        c.name AS ColumnName,
        t.name AS DataType,
        c.max_length AS MaxLength,
        c.is_nullable AS IsNullable
    FROM sys.columns c
    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.BusinessPartner')
      AND c.name IN ('ContactPerson', 'ContactPosition', 'BankAccount', 'BankName', 'CreditLimit', 'PaymentTerm')
    ORDER BY c.name;
    
    IF @@ROWCOUNT = 0
    BEGIN
        PRINT '✓ Tất cả các cột đã được xóa thành công!';
    END
    ELSE
    BEGIN
        PRINT '⚠ Còn một số cột chưa được xóa (xem danh sách trên)';
    END
    
    PRINT '';
    PRINT '========================================';
    PRINT 'Danh sách các cột còn lại trong bảng BusinessPartner:';
    PRINT '========================================';
    
    SELECT 
        c.name AS ColumnName,
        t.name AS DataType,
        c.max_length AS MaxLength,
        c.is_nullable AS IsNullable
    FROM sys.columns c
    INNER JOIN sys.types t ON c.user_type_id = t.user_type_id
    WHERE c.object_id = OBJECT_ID(N'dbo.BusinessPartner')
    ORDER BY c.column_id;
    
    PRINT '';
    PRINT '========================================';
    PRINT 'Migration hoàn tất!';
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
    PRINT 'Lưu ý: Nếu lỗi liên quan đến constraint hoặc foreign key,';
    PRINT '       vui lòng xóa các constraint/foreign key trước khi chạy lại script.';
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

