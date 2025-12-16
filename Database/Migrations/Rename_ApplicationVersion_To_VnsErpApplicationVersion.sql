-- =============================================
-- Script: Đổi tên bảng ApplicationVersion thành VnsErpApplicationVersion
-- Mục đích: Đồng bộ tên bảng với LINQ to SQL mapping trong .dbml
-- =============================================

USE [VnsErp2025Final];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

BEGIN TRANSACTION;
GO

PRINT '========================================';
PRINT 'Bắt đầu đổi tên bảng ApplicationVersion thành VnsErpApplicationVersion';
PRINT '========================================';
GO

-- Kiểm tra xem bảng ApplicationVersion có tồn tại không
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ApplicationVersion' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '⚠️  Bảng ApplicationVersion không tồn tại.';
    
    -- Kiểm tra xem bảng VnsErpApplicationVersion đã tồn tại chưa
    IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VnsErpApplicationVersion' AND schema_id = SCHEMA_ID('dbo'))
    BEGIN
        PRINT '✅ Bảng VnsErpApplicationVersion đã tồn tại. Không cần đổi tên.';
        COMMIT TRANSACTION;
        RETURN;
    END
    ELSE
    BEGIN
        PRINT '❌ Cả hai bảng đều không tồn tại. Vui lòng chạy script Create_ApplicationVersion_And_AllowedMacAddress_Tables.sql trước.';
        ROLLBACK TRANSACTION;
        RETURN;
    END
END
GO

-- Kiểm tra xem bảng VnsErpApplicationVersion đã tồn tại chưa
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VnsErpApplicationVersion' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '⚠️  Bảng VnsErpApplicationVersion đã tồn tại. Không thể đổi tên.';
    PRINT '⚠️  Có thể bảng đã được đổi tên trước đó hoặc có xung đột tên.';
    
    -- Kiểm tra xem có dữ liệu trong bảng ApplicationVersion không
    DECLARE @RowCount INT;
    SELECT @RowCount = COUNT(*) FROM [dbo].[ApplicationVersion];
    
    IF @RowCount > 0
    BEGIN
        PRINT '⚠️  Bảng ApplicationVersion có ' + CAST(@RowCount AS NVARCHAR(10)) + ' bản ghi.';
        PRINT '⚠️  Cần kiểm tra và merge dữ liệu nếu cần thiết.';
    END
    
    ROLLBACK TRANSACTION;
    RETURN;
END
GO

-- Đổi tên bảng từ ApplicationVersion sang VnsErpApplicationVersion
EXEC sp_rename 'dbo.ApplicationVersion', 'VnsErpApplicationVersion';
GO

PRINT '✅ Đã đổi tên bảng từ ApplicationVersion sang VnsErpApplicationVersion';
GO

-- Đổi tên Primary Key constraint nếu cần
IF EXISTS (SELECT 1 FROM sys.key_constraints WHERE parent_object_id = OBJECT_ID(N'dbo.VnsErpApplicationVersion') AND name = 'PK_ApplicationVersion')
BEGIN
    EXEC sp_rename 'dbo.VnsErpApplicationVersion.PK_ApplicationVersion', 'PK_VnsErpApplicationVersion';
    PRINT '✅ Đã đổi tên Primary Key constraint';
END
GO

-- Đổi tên các indexes nếu cần
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.VnsErpApplicationVersion') AND name = 'IX_ApplicationVersion_Version')
BEGIN
    EXEC sp_rename 'dbo.VnsErpApplicationVersion.IX_ApplicationVersion_Version', 'IX_VnsErpApplicationVersion_Version';
    PRINT '✅ Đã đổi tên index IX_ApplicationVersion_Version';
END
GO

IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.VnsErpApplicationVersion') AND name = 'IX_ApplicationVersion_IsActive')
BEGIN
    EXEC sp_rename 'dbo.VnsErpApplicationVersion.IX_ApplicationVersion_IsActive', 'IX_VnsErpApplicationVersion_IsActive';
    PRINT '✅ Đã đổi tên index IX_ApplicationVersion_IsActive';
END
GO

-- Cập nhật Extended Properties
IF EXISTS (SELECT 1 FROM sys.extended_properties WHERE major_id = OBJECT_ID(N'dbo.VnsErpApplicationVersion') AND name = 'MS_Description' AND minor_id = 0)
BEGIN
    EXEC sys.sp_dropextendedproperty 
        @name=N'MS_Description', 
        @level0type=N'SCHEMA',
        @level0name=N'dbo', 
        @level1type=N'TABLE',
        @level1name=N'ApplicationVersion';
END
GO

EXEC sys.sp_addextendedproperty 
    @name=N'MS_Description', 
    @value=N'Bảng quản lý phiên bản ứng dụng. Chỉ có một phiên bản Active tại một thời điểm.', 
    @level0type=N'SCHEMA',
    @level0name=N'dbo', 
    @level1type=N'TABLE',
    @level1name=N'VnsErpApplicationVersion';
GO

PRINT '';
PRINT '========================================';
PRINT 'Hoàn thành đổi tên bảng ApplicationVersion thành VnsErpApplicationVersion';
PRINT '========================================';
PRINT '';
PRINT 'Tóm tắt:';
PRINT '- Đã đổi tên bảng từ ApplicationVersion sang VnsErpApplicationVersion';
PRINT '- Đã đổi tên các constraints và indexes liên quan';
PRINT '- Đã cập nhật Extended Properties';
PRINT '';
PRINT 'Lưu ý:';
PRINT '1. Tên bảng hiện đã khớp với LINQ to SQL mapping trong .dbml';
PRINT '2. Có thể cần rebuild project để đảm bảo DataContext được cập nhật';
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

