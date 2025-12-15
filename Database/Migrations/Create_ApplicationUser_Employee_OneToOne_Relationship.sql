-- =============================================
-- Script: Tạo liên kết 1-1 giữa ApplicationUser và Employee
-- Mục đích: Mỗi Employee có thể liên kết với một ApplicationUser (1-1 relationship)
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
PRINT 'Bắt đầu tạo liên kết 1-1 giữa ApplicationUser và Employee';
PRINT '========================================';
GO

-- =============================================
-- Kiểm tra và thêm cột ApplicationUserId vào bảng Employee
-- =============================================

IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'dbo.Employee') 
    AND name = 'ApplicationUserId'
)
BEGIN
    PRINT '📝 Đang thêm cột ApplicationUserId vào bảng Employee...';
    
    ALTER TABLE [dbo].[Employee]
    ADD [ApplicationUserId] UNIQUEIDENTIFIER NULL;
    
    PRINT '✅ Đã thêm cột ApplicationUserId vào bảng Employee';
END
ELSE
BEGIN
    PRINT '⚠️  Cột ApplicationUserId đã tồn tại trong bảng Employee';
END
GO

-- =============================================
-- Thêm Extended Property cho cột ApplicationUserId
-- =============================================

IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'dbo.Employee') 
    AND name = 'ApplicationUserId'
)
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM sys.extended_properties 
        WHERE major_id = OBJECT_ID(N'dbo.Employee') 
        AND minor_id = (SELECT column_id FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.Employee') AND name = 'ApplicationUserId')
        AND name = 'MS_Description'
    )
    BEGIN
        EXEC sys.sp_addextendedproperty 
            @name = N'MS_Description',
            @value = N'ID người dùng ứng dụng liên kết với nhân viên này (1-1 relationship)',
            @level0type = N'SCHEMA', @level0name = N'dbo',
            @level1type = N'TABLE', @level1name = N'Employee',
            @level2type = N'COLUMN', @level2name = N'ApplicationUserId';
        
        PRINT '✅ Đã thêm Extended Property cho cột ApplicationUserId';
    END
    ELSE
    BEGIN
        PRINT '⚠️  Extended Property cho cột ApplicationUserId đã tồn tại';
    END
END
GO

-- =============================================
-- Tạo Foreign Key Constraint: Employee.ApplicationUserId -> ApplicationUser.Id
-- =============================================

IF NOT EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE name = 'FK_Employee_ApplicationUser'
    AND parent_object_id = OBJECT_ID(N'dbo.Employee')
)
BEGIN
    PRINT '🔗 Đang tạo Foreign Key Constraint FK_Employee_ApplicationUser...';
    
    ALTER TABLE [dbo].[Employee]
    ADD CONSTRAINT [FK_Employee_ApplicationUser]
    FOREIGN KEY ([ApplicationUserId])
    REFERENCES [dbo].[ApplicationUser] ([Id])
    ON DELETE SET NULL
    ON UPDATE CASCADE;
    
    PRINT '✅ Đã tạo Foreign Key Constraint FK_Employee_ApplicationUser';
END
ELSE
BEGIN
    PRINT '⚠️  Foreign Key Constraint FK_Employee_ApplicationUser đã tồn tại';
END
GO

-- =============================================
-- Tạo Unique Constraint để đảm bảo 1-1 relationship
-- (Mỗi ApplicationUser chỉ có thể liên kết với 1 Employee)
-- =============================================

IF NOT EXISTS (
    SELECT 1 
    FROM sys.indexes 
    WHERE name = 'UQ_Employee_ApplicationUserId'
    AND object_id = OBJECT_ID(N'dbo.Employee')
)
BEGIN
    PRINT '🔒 Đang tạo Unique Constraint UQ_Employee_ApplicationUserId...';
    
    -- Tạo unique index (chỉ áp dụng cho giá trị không NULL)
    CREATE UNIQUE NONCLUSTERED INDEX [UQ_Employee_ApplicationUserId]
    ON [dbo].[Employee] ([ApplicationUserId])
    WHERE [ApplicationUserId] IS NOT NULL;
    
    PRINT '✅ Đã tạo Unique Constraint UQ_Employee_ApplicationUserId';
END
ELSE
BEGIN
    PRINT '⚠️  Unique Constraint UQ_Employee_ApplicationUserId đã tồn tại';
END
GO

-- =============================================
-- Thêm Extended Property cho Foreign Key
-- =============================================

IF EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE name = 'FK_Employee_ApplicationUser'
    AND parent_object_id = OBJECT_ID(N'dbo.Employee')
)
BEGIN
    IF NOT EXISTS (
        SELECT 1 
        FROM sys.extended_properties 
        WHERE major_id = OBJECT_ID(N'dbo.Employee') 
        AND minor_id = 0
        AND name = 'FK_Employee_ApplicationUser_Description'
    )
    BEGIN
        EXEC sys.sp_addextendedproperty 
            @name = N'FK_Employee_ApplicationUser_Description',
            @value = N'Foreign Key: Employee.ApplicationUserId -> ApplicationUser.Id (1-1 relationship)',
            @level0type = N'SCHEMA', @level0name = N'dbo',
            @level1type = N'TABLE', @level1name = N'Employee';
        
        PRINT '✅ Đã thêm Extended Property cho Foreign Key';
    END
END
GO

-- =============================================
-- Kiểm tra kết quả
-- =============================================

PRINT '';
PRINT '========================================';
PRINT 'Kiểm tra kết quả:';
PRINT '========================================';

-- Kiểm tra cột
IF EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'dbo.Employee') 
    AND name = 'ApplicationUserId'
)
BEGIN
    PRINT '✅ Cột ApplicationUserId đã được tạo thành công';
END
ELSE
BEGIN
    PRINT '❌ LỖI: Cột ApplicationUserId chưa được tạo';
END

-- Kiểm tra Foreign Key
IF EXISTS (
    SELECT 1 
    FROM sys.foreign_keys 
    WHERE name = 'FK_Employee_ApplicationUser'
    AND parent_object_id = OBJECT_ID(N'dbo.Employee')
)
BEGIN
    PRINT '✅ Foreign Key FK_Employee_ApplicationUser đã được tạo thành công';
END
ELSE
BEGIN
    PRINT '❌ LỖI: Foreign Key FK_Employee_ApplicationUser chưa được tạo';
END

-- Kiểm tra Unique Index
IF EXISTS (
    SELECT 1 
    FROM sys.indexes 
    WHERE name = 'UQ_Employee_ApplicationUserId'
    AND object_id = OBJECT_ID(N'dbo.Employee')
)
BEGIN
    PRINT '✅ Unique Index UQ_Employee_ApplicationUserId đã được tạo thành công';
END
ELSE
BEGIN
    PRINT '❌ LỖI: Unique Index UQ_Employee_ApplicationUserId chưa được tạo';
END

PRINT '';
PRINT '========================================';
PRINT 'Hoàn thành tạo liên kết 1-1 giữa ApplicationUser và Employee';
PRINT '========================================';
GO

-- Commit transaction
COMMIT TRANSACTION;
GO

PRINT '';
PRINT '✅ Migration hoàn tất thành công!';
PRINT '';
PRINT 'Lưu ý:';
PRINT '  - Cột ApplicationUserId trong bảng Employee cho phép NULL (không bắt buộc)';
PRINT '  - Unique constraint đảm bảo mỗi ApplicationUser chỉ có thể liên kết với 1 Employee';
PRINT '  - Foreign Key có ON DELETE SET NULL để tự động xóa liên kết khi ApplicationUser bị xóa';
PRINT '  - Foreign Key có ON UPDATE CASCADE để tự động cập nhật khi ApplicationUser.Id thay đổi';
GO
