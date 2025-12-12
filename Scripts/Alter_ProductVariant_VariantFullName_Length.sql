USE [VnsErp2025Final]
GO

-- =============================================
-- Script: Thay đổi độ dài cột VariantFullName trong bảng ProductVariant
-- Mô tả: Thay đổi từ nvarchar(500) sang nvarchar(max) để lưu trữ HTML đầy đủ
-- Ngày tạo: 12/12/2025
-- =============================================

-- Kiểm tra xem cột có tồn tại không
IF EXISTS (
    SELECT 1 
    FROM INFORMATION_SCHEMA.COLUMNS 
    WHERE TABLE_SCHEMA = 'dbo' 
    AND TABLE_NAME = 'ProductVariant' 
    AND COLUMN_NAME = 'VariantFullName'
)
BEGIN
    PRINT 'Đang thay đổi độ dài cột VariantFullName từ nvarchar(500) sang nvarchar(max)...'
    
    -- Thay đổi kiểu dữ liệu của cột VariantFullName
    ALTER TABLE [dbo].[ProductVariant]
    ALTER COLUMN [VariantFullName] [nvarchar](max) NULL
    
    PRINT 'Đã thay đổi thành công cột VariantFullName sang nvarchar(max)'
END
ELSE
BEGIN
    PRINT 'Cảnh báo: Cột VariantFullName không tồn tại trong bảng ProductVariant'
END
GO

-- Kiểm tra lại kết quả
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_SCHEMA = 'dbo' 
    AND TABLE_NAME = 'ProductVariant' 
    AND COLUMN_NAME = 'VariantFullName'
GO

PRINT 'Script hoàn tất!'
GO
