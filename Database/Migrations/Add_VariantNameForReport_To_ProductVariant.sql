-- =============================================
-- Migration Script: Add VariantNameForReport Column to ProductVariant Table
-- =============================================
-- Mô tả: Thêm cột VariantNameForReport để lưu tên biến thể sản phẩm 
-- đã loại bỏ HTML tags, phục vụ cho việc hiển thị trong report
-- =============================================
-- Ngày tạo: 2025
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
PRINT 'Bắt đầu thêm cột VariantNameForReport vào bảng ProductVariant';
PRINT '========================================';
GO

-- =============================================
-- Bước 1: Thêm cột VariantNameForReport (nếu chưa tồn tại)
-- =============================================

IF NOT EXISTS (SELECT 1 FROM sys.columns 
               WHERE object_id = OBJECT_ID(N'dbo.ProductVariant') 
               AND name = 'VariantNameForReport')
BEGIN
    ALTER TABLE [dbo].[ProductVariant]
    ADD [VariantNameForReport] NVARCHAR(MAX) NULL;
    
    PRINT '✅ Đã thêm cột VariantNameForReport vào bảng ProductVariant';
END
ELSE
BEGIN
    PRINT '⏭️  Cột VariantNameForReport đã tồn tại, bỏ qua';
END
GO

-- =============================================
-- Bước 2: Cập nhật dữ liệu cho các record hiện có (nếu cần)
-- Lưu ý: Có thể để NULL hoặc copy từ VariantFullName và loại bỏ HTML tags
-- =============================================

-- Script này chỉ thêm cột, việc populate dữ liệu sẽ được thực hiện ở application layer
-- hoặc có thể tạo script riêng để update dữ liệu

PRINT '========================================';
PRINT 'Hoàn thành thêm cột VariantNameForReport';
PRINT '========================================';
PRINT '';
PRINT 'Lưu ý: Cột VariantNameForReport đã được thêm với giá trị NULL.';
PRINT 'Cần cập nhật dữ liệu từ VariantFullName (đã loại bỏ HTML tags)';
PRINT 'thông qua application code hoặc script update riêng.';
PRINT '========================================';
GO

-- =============================================
-- Commit transaction
-- =============================================

COMMIT TRANSACTION;
GO

PRINT '';
PRINT '✅ Migration hoàn thành thành công!';
GO

