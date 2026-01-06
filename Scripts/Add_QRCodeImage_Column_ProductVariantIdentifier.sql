-- =============================================
-- Script: Thêm cột QRCodeImage vào bảng ProductVariantIdentifier
-- Description: Thêm cột QRCodeImage kiểu varbinary(MAX) để lưu trữ dữ liệu ảnh QR Code trực tiếp trong database
-- Date: 2025
-- =============================================

USE [YourDatabaseName] -- Thay đổi tên database phù hợp
GO

-- Kiểm tra xem cột đã tồn tại chưa
IF NOT EXISTS (
    SELECT 1 
    FROM sys.columns 
    WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantIdentifier]') 
    AND name = 'QRCodeImage'
)
BEGIN
    -- Thêm cột QRCodeImage kiểu varbinary(MAX)
    ALTER TABLE [dbo].[ProductVariantIdentifier]
    ADD [QRCodeImage] [varbinary](MAX) NULL;
    
    PRINT 'Đã thêm cột QRCodeImage vào bảng ProductVariantIdentifier thành công.';
END
ELSE
BEGIN
    PRINT 'Cột QRCodeImage đã tồn tại trong bảng ProductVariantIdentifier.';
END
GO

-- Kiểm tra kết quả
SELECT 
    COLUMN_NAME,
    DATA_TYPE,
    CHARACTER_MAXIMUM_LENGTH,
    IS_NULLABLE
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'ProductVariantIdentifier'
    AND COLUMN_NAME = 'QRCodeImage';
GO
