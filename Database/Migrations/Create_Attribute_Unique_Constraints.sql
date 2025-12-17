-- =============================================
-- Script tạo UNIQUE CONSTRAINT cho bảng Attribute
-- Đảm bảo Name và DataType là duy nhất
-- =============================================

USE [VnsErp2025Final]
GO

-- Kiểm tra xem bảng Attribute có tồn tại không
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Attribute]') AND type in (N'U'))
BEGIN
    PRINT 'Bảng Attribute chưa tồn tại. Vui lòng tạo bảng trước khi chạy script này.'
    RETURN
END
GO

-- Kiểm tra và xóa constraint cũ nếu tồn tại (nếu cần sửa đổi)
-- Xóa constraint cho Name riêng lẻ (nếu có)
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Attribute_Name' AND object_id = OBJECT_ID('dbo.Attribute'))
BEGIN
    ALTER TABLE [dbo].[Attribute] DROP CONSTRAINT [UQ_Attribute_Name]
    PRINT 'Đã xóa constraint UQ_Attribute_Name cũ (không cần thiết)'
END
GO

-- Xóa constraint cho DataType riêng lẻ (nếu có) - ĐÂY LÀ NGUYÊN NHÂN GÂY LỖI
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Attribute_DataType' AND object_id = OBJECT_ID('dbo.Attribute'))
BEGIN
    ALTER TABLE [dbo].[Attribute] DROP CONSTRAINT [UQ_Attribute_DataType]
    PRINT 'Đã xóa constraint UQ_Attribute_DataType cũ (không cần thiết - nhiều thuộc tính có thể có cùng DataType)'
END
GO

-- Xóa constraint cho cặp (Name, DataType) nếu cần tạo lại
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Attribute_Name_DataType' AND object_id = OBJECT_ID('dbo.Attribute'))
BEGIN
    ALTER TABLE [dbo].[Attribute] DROP CONSTRAINT [UQ_Attribute_Name_DataType]
    PRINT 'Đã xóa constraint UQ_Attribute_Name_DataType cũ'
END
GO

-- Kiểm tra dữ liệu trùng lặp trước khi tạo constraint
PRINT ''
PRINT '========================================'
PRINT 'Kiểm tra dữ liệu trùng lặp:'
PRINT '========================================'

-- Chỉ kiểm tra cặp (Name, DataType) trùng lặp
-- Lưu ý: Nhiều thuộc tính có thể có cùng DataType (ví dụ: nhiều thuộc tính có DataType = "String")
-- Nhưng mỗi cặp (Name, DataType) phải là duy nhất
IF EXISTS (SELECT [Name], [DataType], COUNT(*) AS [Count] 
           FROM [dbo].[Attribute] 
           GROUP BY [Name], [DataType] 
           HAVING COUNT(*) > 1)
BEGIN
    PRINT '⚠ CẢNH BÁO: Có cặp (Name, DataType) trùng lặp trong bảng Attribute:'
    SELECT [Name], [DataType], COUNT(*) AS [Số lượng trùng]
    FROM [dbo].[Attribute]
    GROUP BY [Name], [DataType]
    HAVING COUNT(*) > 1
    PRINT 'Vui lòng xử lý dữ liệu trùng lặp trước khi tạo constraint!'
    RETURN
END

PRINT '✓ Không có dữ liệu trùng lặp'
GO

-- Chỉ tạo UNIQUE CONSTRAINT cho cặp (Name, DataType)
-- Điều này cho phép:
-- - Cùng một Name với các DataType khác nhau (ví dụ: "Màu sắc" với String và "Màu sắc" với Integer)
-- - Cùng một DataType với các Name khác nhau (ví dụ: "Màu sắc" với String và "Kích thước" với String)
-- Nhưng không cho phép: Cùng một cặp (Name, DataType) xuất hiện 2 lần
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'UQ_Attribute_Name_DataType' AND object_id = OBJECT_ID('dbo.Attribute'))
BEGIN
    ALTER TABLE [dbo].[Attribute]
    ADD CONSTRAINT [UQ_Attribute_Name_DataType] UNIQUE NONCLUSTERED ([Name] ASC, [DataType] ASC)
    PRINT '✓ Đã tạo UNIQUE CONSTRAINT cho (Name, DataType): UQ_Attribute_Name_DataType'
END
ELSE
BEGIN
    PRINT 'Constraint UQ_Attribute_Name_DataType đã tồn tại'
END
GO

-- Hiển thị thông tin các constraint đã tạo
PRINT ''
PRINT '========================================'
PRINT 'Danh sách UNIQUE CONSTRAINT trên bảng Attribute:'
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

PRINT ''
PRINT 'Hoàn tất script tạo UNIQUE CONSTRAINT cho bảng Attribute'
GO
