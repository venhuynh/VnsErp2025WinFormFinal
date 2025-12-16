-- =============================================
-- Script: Kiểm tra bảng ApplicationVersion/VnsErpApplicationVersion
-- Mục đích: Xác định tên bảng thực tế trong database
-- =============================================

USE [VnsErp2025Final];
GO

PRINT '========================================';
PRINT 'Kiểm tra bảng ApplicationVersion/VnsErpApplicationVersion';
PRINT '========================================';
PRINT '';

-- Kiểm tra bảng ApplicationVersion
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'ApplicationVersion' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '✅ Bảng ApplicationVersion TỒN TẠI trong schema dbo';
    
    DECLARE @RowCount1 INT;
    SELECT @RowCount1 = COUNT(*) FROM [dbo].[ApplicationVersion];
    PRINT '   Số bản ghi: ' + CAST(@RowCount1 AS NVARCHAR(10));
    
    -- Liệt kê các cột
    PRINT '   Các cột:';
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        IS_NULLABLE
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'ApplicationVersion'
    ORDER BY ORDINAL_POSITION;
END
ELSE
BEGIN
    PRINT '❌ Bảng ApplicationVersion KHÔNG TỒN TẠI trong schema dbo';
END
GO

PRINT '';

-- Kiểm tra bảng VnsErpApplicationVersion
IF EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VnsErpApplicationVersion' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '✅ Bảng VnsErpApplicationVersion TỒN TẠI trong schema dbo';
    
    DECLARE @RowCount2 INT;
    SELECT @RowCount2 = COUNT(*) FROM [dbo].[VnsErpApplicationVersion];
    PRINT '   Số bản ghi: ' + CAST(@RowCount2 AS NVARCHAR(10));
    
    -- Liệt kê các cột
    PRINT '   Các cột:';
    SELECT 
        COLUMN_NAME,
        DATA_TYPE,
        IS_NULLABLE
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_SCHEMA = 'dbo' AND TABLE_NAME = 'VnsErpApplicationVersion'
    ORDER BY ORDINAL_POSITION;
END
ELSE
BEGIN
    PRINT '❌ Bảng VnsErpApplicationVersion KHÔNG TỒN TẠI trong schema dbo';
END
GO

PRINT '';

-- Liệt kê tất cả các bảng có chứa "ApplicationVersion" hoặc "Version" trong tên
PRINT 'Tất cả các bảng có chứa "ApplicationVersion" hoặc "Version" trong tên:';
SELECT 
    TABLE_SCHEMA,
    TABLE_NAME
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_NAME LIKE '%ApplicationVersion%' 
   OR TABLE_NAME LIKE '%Version%'
ORDER BY TABLE_SCHEMA, TABLE_NAME;
GO

PRINT '';
PRINT '========================================';
PRINT 'Kết thúc kiểm tra';
PRINT '========================================';
GO
