-- =============================================
-- Script XÓA các UNIQUE CONSTRAINT cũ trên bảng Attribute
-- Chạy script này trước khi chạy Insert_Attribute_Default_Data.sql
-- nếu bạn đã chạy Create_Attribute_Unique_Constraints.sql trước đó
-- =============================================

USE [VnsErp2025Final]
GO

-- Kiểm tra xem bảng Attribute có tồn tại không
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Attribute]') AND type in (N'U'))
BEGIN
    PRINT 'Bảng Attribute chưa tồn tại.'
    RETURN
END
GO

PRINT '========================================'
PRINT 'Xóa các UNIQUE CONSTRAINT trên bảng Attribute:'
PRINT '========================================'

-- Xóa constraint cho Name riêng lẻ (nếu có)
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Attribute_Name' AND object_id = OBJECT_ID('dbo.Attribute'))
BEGIN
    ALTER TABLE [dbo].[Attribute] DROP CONSTRAINT [UQ_Attribute_Name]
    PRINT '✓ Đã xóa constraint UQ_Attribute_Name'
END
ELSE
BEGIN
    PRINT '  Constraint UQ_Attribute_Name không tồn tại'
END
GO

-- Xóa constraint cho DataType riêng lẻ (nếu có) - ĐÂY LÀ NGUYÊN NHÂN GÂY LỖI
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Attribute_DataType' AND object_id = OBJECT_ID('dbo.Attribute'))
BEGIN
    ALTER TABLE [dbo].[Attribute] DROP CONSTRAINT [UQ_Attribute_DataType]
    PRINT '✓ Đã xóa constraint UQ_Attribute_DataType (nhiều thuộc tính có thể có cùng DataType)'
END
ELSE
BEGIN
    PRINT '  Constraint UQ_Attribute_DataType không tồn tại'
END
GO

-- Xóa constraint cho cặp (Name, DataType) (nếu có)
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Attribute_Name_DataType' AND object_id = OBJECT_ID('dbo.Attribute'))
BEGIN
    ALTER TABLE [dbo].[Attribute] DROP CONSTRAINT [UQ_Attribute_Name_DataType]
    PRINT '✓ Đã xóa constraint UQ_Attribute_Name_DataType'
END
ELSE
BEGIN
    PRINT '  Constraint UQ_Attribute_Name_DataType không tồn tại'
END
GO

-- Hiển thị các constraint còn lại
PRINT ''
PRINT '========================================'
PRINT 'Các UNIQUE CONSTRAINT còn lại trên bảng Attribute:'
PRINT '========================================'
SELECT 
    i.name AS [Constraint Name],
    STRING_AGG(c.name, ', ') WITHIN GROUP (ORDER BY ic.key_ordinal) AS [Columns]
FROM sys.indexes i
INNER JOIN sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
INNER JOIN sys.columns c ON ic.object_id = c.object_id AND ic.column_id = c.column_id
WHERE i.object_id = OBJECT_ID('dbo.Attribute')
  AND i.is_unique = 1
  AND i.is_primary_key = 0
GROUP BY i.name
ORDER BY i.name
GO

IF @@ROWCOUNT = 0
BEGIN
    PRINT '  Không còn UNIQUE CONSTRAINT nào (ngoài Primary Key)'
END

PRINT ''
PRINT 'Hoàn tất script xóa UNIQUE CONSTRAINT'
GO
