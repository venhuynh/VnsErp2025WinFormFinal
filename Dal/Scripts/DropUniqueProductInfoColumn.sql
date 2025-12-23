-- =============================================
-- Script xóa cột UniqueProductInfo khỏi bảng Warranty
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Xóa cột UniqueProductInfo không còn sử dụng trong bảng Warranty
--        Cột này đã được thay thế bằng DeviceId và DeviceInfo
-- =============================================

USE [VnsErp2025Final]
GO

-- =============================================
-- 1. Xóa constraint phụ thuộc vào cột UniqueProductInfo
-- =============================================

-- Xóa constraint UQ_Warranty_WarrantyType_UniqueProductInfo nếu tồn tại
IF EXISTS (SELECT * FROM sys.objects 
           WHERE object_id = OBJECT_ID(N'[dbo].[UQ_Warranty_WarrantyType_UniqueProductInfo]') 
           AND type = 'UQ')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    DROP CONSTRAINT [UQ_Warranty_WarrantyType_UniqueProductInfo];
    
    PRINT 'Đã xóa constraint UQ_Warranty_WarrantyType_UniqueProductInfo';
END
ELSE
BEGIN
    PRINT 'Constraint UQ_Warranty_WarrantyType_UniqueProductInfo không tồn tại';
END
GO

-- Kiểm tra và xóa các constraint khác có thể phụ thuộc vào cột UniqueProductInfo
DECLARE @ConstraintName NVARCHAR(200);
DECLARE @SQL NVARCHAR(MAX);

-- Tìm tất cả các unique constraint có sử dụng cột UniqueProductInfo
DECLARE constraint_cursor CURSOR FOR
SELECT DISTINCT kc.name
FROM sys.key_constraints kc
INNER JOIN sys.index_columns ic ON kc.parent_object_id = ic.object_id AND kc.unique_index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE kc.parent_object_id = OBJECT_ID(N'[dbo].[Warranty]')
  AND c.name = 'UniqueProductInfo';

OPEN constraint_cursor;
FETCH NEXT FROM constraint_cursor INTO @ConstraintName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = N'ALTER TABLE [dbo].[Warranty] DROP CONSTRAINT [' + @ConstraintName + N']';
    EXEC sp_executesql @SQL;
    PRINT 'Đã xóa constraint: ' + @ConstraintName;
    
    FETCH NEXT FROM constraint_cursor INTO @ConstraintName;
END;

CLOSE constraint_cursor;
DEALLOCATE constraint_cursor;
GO

-- =============================================
-- 2. Kiểm tra và xóa cột UniqueProductInfo
-- =============================================

IF EXISTS (SELECT * FROM sys.columns 
           WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') 
           AND name = 'UniqueProductInfo')
BEGIN
    -- Xóa cột UniqueProductInfo
    ALTER TABLE [dbo].[Warranty]
    DROP COLUMN [UniqueProductInfo];
    
    PRINT 'Đã xóa cột UniqueProductInfo khỏi bảng Warranty';
END
ELSE
BEGIN
    PRINT 'Cột UniqueProductInfo không tồn tại trong bảng Warranty';
END
GO

-- =============================================
-- Kiểm tra kết quả
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') 
               AND name = 'UniqueProductInfo')
BEGIN
    PRINT 'Xác nhận: Cột UniqueProductInfo đã được xóa thành công';
END
ELSE
BEGIN
    PRINT 'CẢNH BÁO: Cột UniqueProductInfo vẫn còn tồn tại sau khi thực hiện xóa';
END
GO

