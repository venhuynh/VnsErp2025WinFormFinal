-- =============================================
-- Script: Thêm cột ReleaseNote vào bảng ApplicationVersion
-- Mục đích: Bổ sung cột ReleaseNote để lưu ghi chú phát hành chi tiết
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
PRINT 'Bắt đầu thêm cột ReleaseNote vào bảng ApplicationVersion';
PRINT '========================================';
GO

-- Kiểm tra xem bảng ApplicationVersion có tồn tại không
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ApplicationVersion' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '⚠️  Bảng ApplicationVersion không tồn tại. Vui lòng chạy script Create_ApplicationVersion_And_AllowedMacAddress_Tables.sql trước.';
    ROLLBACK TRANSACTION;
    RETURN;
END
GO

-- Kiểm tra xem cột ReleaseNote đã tồn tại chưa
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID(N'dbo.ApplicationVersion') AND name = 'ReleaseNote')
BEGIN
    PRINT '⚠️  Cột ReleaseNote đã tồn tại trong bảng ApplicationVersion. Không cần thêm.';
END
ELSE
BEGIN
    -- Thêm cột ReleaseNote
    ALTER TABLE [dbo].[ApplicationVersion]
    ADD [ReleaseNote] [nvarchar](1000) NULL;
    
    PRINT '✅ Đã thêm cột ReleaseNote vào bảng ApplicationVersion';
    
    -- Thêm Extended Property
    EXEC sys.sp_addextendedproperty 
        @name=N'MS_Description', 
        @value=N'Ghi chú phát hành chi tiết về phiên bản (tối đa 1000 ký tự)', 
        @level0type=N'SCHEMA',
        @level0name=N'dbo', 
        @level1type=N'TABLE',
        @level1name=N'ApplicationVersion', 
        @level2type=N'COLUMN',
        @level2name=N'ReleaseNote';
    
    PRINT '✅ Đã thêm Extended Property cho cột ReleaseNote';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'Hoàn thành thêm cột ReleaseNote vào bảng ApplicationVersion';
PRINT '========================================';
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


