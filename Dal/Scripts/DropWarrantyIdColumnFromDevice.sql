-- =============================================
-- Script xóa cột WarrantyId khỏi bảng Device
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Xóa cột WarrantyId không còn sử dụng trong bảng Device
--        Cột này đã được thay thế bằng quan hệ ngược lại: Warranty.DeviceId -> Device.Id
--        Bảng Warranty liên kết với Device thông qua DeviceId (Foreign Key đến Device.Id)
-- =============================================

USE [VnsErp2025Final]
GO

-- =============================================
-- 1. Xóa constraint phụ thuộc vào cột WarrantyId
-- =============================================

-- Xóa Foreign Key constraint nếu tồn tại
DECLARE @ForeignKeyName NVARCHAR(200);
DECLARE @SQL NVARCHAR(MAX);

-- Tìm tất cả các Foreign Key constraint có sử dụng cột WarrantyId
DECLARE fk_cursor CURSOR FOR
SELECT DISTINCT fk.name
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fkc ON fk.object_id = fkc.constraint_object_id
INNER JOIN sys.columns c ON fkc.parent_object_id = c.object_id AND fkc.parent_column_id = c.column_id
WHERE fkc.parent_object_id = OBJECT_ID(N'[dbo].[Device]')
  AND c.name = 'WarrantyId';

OPEN fk_cursor;
FETCH NEXT FROM fk_cursor INTO @ForeignKeyName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @SQL = N'ALTER TABLE [dbo].[Device] DROP CONSTRAINT [' + @ForeignKeyName + N']';
    EXEC sp_executesql @SQL;
    PRINT 'Đã xóa Foreign Key constraint: ' + @ForeignKeyName;
    
    FETCH NEXT FROM fk_cursor INTO @ForeignKeyName;
END;

CLOSE fk_cursor;
DEALLOCATE fk_cursor;
GO

-- Xóa Index nếu tồn tại
IF EXISTS (SELECT * FROM sys.indexes 
           WHERE name LIKE '%WarrantyId%' 
           AND object_id = OBJECT_ID(N'[dbo].[Device]'))
BEGIN
    DECLARE @IndexName NVARCHAR(200);
    DECLARE @IndexSQL NVARCHAR(MAX);
    
    DECLARE index_cursor CURSOR FOR
    SELECT name
    FROM sys.indexes
    WHERE object_id = OBJECT_ID(N'[dbo].[Device]')
      AND name LIKE '%WarrantyId%';
    
    OPEN index_cursor;
    FETCH NEXT FROM index_cursor INTO @IndexName;
    
    WHILE @@FETCH_STATUS = 0
    BEGIN
        SET @IndexSQL = N'DROP INDEX [' + @IndexName + N'] ON [dbo].[Device]';
        EXEC sp_executesql @IndexSQL;
        PRINT 'Đã xóa Index: ' + @IndexName;
        
        FETCH NEXT FROM index_cursor INTO @IndexName;
    END;
    
    CLOSE index_cursor;
    DEALLOCATE index_cursor;
END
ELSE
BEGIN
    PRINT 'Không có Index nào liên quan đến WarrantyId';
END
GO

-- Xóa Default constraint nếu tồn tại
DECLARE @DefaultConstraintName NVARCHAR(200);
DECLARE @DefaultSQL NVARCHAR(MAX);

DECLARE default_cursor CURSOR FOR
SELECT DISTINCT dc.name
FROM sys.default_constraints dc
INNER JOIN sys.columns c ON dc.parent_object_id = c.object_id AND dc.parent_column_id = c.column_id
WHERE dc.parent_object_id = OBJECT_ID(N'[dbo].[Device]')
  AND c.name = 'WarrantyId';

OPEN default_cursor;
FETCH NEXT FROM default_cursor INTO @DefaultConstraintName;

WHILE @@FETCH_STATUS = 0
BEGIN
    SET @DefaultSQL = N'ALTER TABLE [dbo].[Device] DROP CONSTRAINT [' + @DefaultConstraintName + N']';
    EXEC sp_executesql @DefaultSQL;
    PRINT 'Đã xóa Default constraint: ' + @DefaultConstraintName;
    
    FETCH NEXT FROM default_cursor INTO @DefaultConstraintName;
END;

CLOSE default_cursor;
DEALLOCATE default_cursor;
GO

-- =============================================
-- 2. Kiểm tra và xóa cột WarrantyId
-- =============================================

IF EXISTS (SELECT * FROM sys.columns 
           WHERE object_id = OBJECT_ID(N'[dbo].[Device]') 
           AND name = 'WarrantyId')
BEGIN
    -- Xóa cột WarrantyId
    ALTER TABLE [dbo].[Device]
    DROP COLUMN [WarrantyId];
    
    PRINT 'Đã xóa cột WarrantyId khỏi bảng Device';
END
ELSE
BEGIN
    PRINT 'Cột WarrantyId không tồn tại trong bảng Device';
END
GO

-- =============================================
-- 3. Kiểm tra kết quả
-- =============================================

IF NOT EXISTS (SELECT * FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'[dbo].[Device]') 
               AND name = 'WarrantyId')
BEGIN
    PRINT 'Xác nhận: Cột WarrantyId đã được xóa thành công';
END
ELSE
BEGIN
    PRINT 'CẢNH BÁO: Cột WarrantyId vẫn còn tồn tại sau khi thực hiện xóa';
END
GO

