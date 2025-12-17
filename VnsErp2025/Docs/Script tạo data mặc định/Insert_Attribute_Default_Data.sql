-- =============================================
-- Script INSERT dữ liệu mặc định cho bảng Attribute
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

-- Kiểm tra và xóa dữ liệu cũ nếu cần (tùy chọn - comment nếu muốn giữ dữ liệu cũ)
-- DELETE FROM [dbo].[Attribute]
-- GO

-- Insert dữ liệu mặc định cho bảng Attribute
-- Sử dụng MERGE để tránh trùng lặp dựa trên Name và DataType

-- 1. Màu sắc (Color)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Màu sắc' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Màu sắc', N'String', N'Thuộc tính màu sắc của sản phẩm (ví dụ: Đỏ, Xanh, Vàng, v.v.)')
    PRINT 'Đã thêm: Màu sắc'
END
GO

-- 2. Kích thước (Size)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Kích thước' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Kích thước', N'String', N'Kích thước của sản phẩm (ví dụ: S, M, L, XL, XXL, v.v.)')
    PRINT 'Đã thêm: Kích thước'
END
GO

-- 3. Chất liệu (Material)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Chất liệu' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Chất liệu', N'String', N'Chất liệu làm nên sản phẩm (ví dụ: Cotton, Polyester, Gỗ, Nhựa, v.v.)')
    PRINT 'Đã thêm: Chất liệu'
END
GO

-- 4. Trọng lượng (Weight)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Trọng lượng' AND [DataType] = N'Decimal')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Trọng lượng', N'Decimal', N'Trọng lượng của sản phẩm (đơn vị: kg hoặc gram)')
    PRINT 'Đã thêm: Trọng lượng'
END
GO

-- 5. Chiều dài (Length)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Chiều dài' AND [DataType] = N'Decimal')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Chiều dài', N'Decimal', N'Chiều dài của sản phẩm (đơn vị: cm hoặc m)')
    PRINT 'Đã thêm: Chiều dài'
END
GO

-- 6. Chiều rộng (Width)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Chiều rộng' AND [DataType] = N'Decimal')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Chiều rộng', N'Decimal', N'Chiều rộng của sản phẩm (đơn vị: cm hoặc m)')
    PRINT 'Đã thêm: Chiều rộng'
END
GO

-- 7. Chiều cao (Height)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Chiều cao' AND [DataType] = N'Decimal')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Chiều cao', N'Decimal', N'Chiều cao của sản phẩm (đơn vị: cm hoặc m)')
    PRINT 'Đã thêm: Chiều cao'
END
GO

-- 8. Dung lượng (Capacity)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Dung lượng' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Dung lượng', N'String', N'Dung lượng của sản phẩm (ví dụ: 64GB, 128GB, 500ml, 1L, v.v.)')
    PRINT 'Đã thêm: Dung lượng'
END
GO

-- 9. Model (Model)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Model' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Model', N'String', N'Model hoặc phiên bản của sản phẩm')
    PRINT 'Đã thêm: Model'
END
GO

-- 10. Thương hiệu (Brand)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Thương hiệu' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Thương hiệu', N'String', N'Thương hiệu của sản phẩm')
    PRINT 'Đã thêm: Thương hiệu'
END
GO

-- 11. Xuất xứ (Origin)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Xuất xứ' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Xuất xứ', N'String', N'Nước xuất xứ của sản phẩm (ví dụ: Việt Nam, Trung Quốc, Nhật Bản, v.v.)')
    PRINT 'Đã thêm: Xuất xứ'
END
GO

-- 12. Bảo hành (Warranty)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Bảo hành' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Bảo hành', N'String', N'Thời gian bảo hành (ví dụ: 12 tháng, 24 tháng, v.v.)')
    PRINT 'Đã thêm: Bảo hành'
END
GO

-- 13. Điện áp (Voltage)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Điện áp' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Điện áp', N'String', N'Điện áp hoạt động (ví dụ: 220V, 110V, v.v.)')
    PRINT 'Đã thêm: Điện áp'
END
GO

-- 14. Công suất (Power)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Công suất' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Công suất', N'String', N'Công suất tiêu thụ (ví dụ: 1000W, 2000W, v.v.)')
    PRINT 'Đã thêm: Công suất'
END
GO

-- 15. Số lượng (Quantity)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Số lượng' AND [DataType] = N'Integer')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Số lượng', N'Integer', N'Số lượng trong một đơn vị (ví dụ: 1, 2, 10, v.v.)')
    PRINT 'Đã thêm: Số lượng'
END
GO

-- 16. Đơn vị đóng gói (Packaging Unit)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Đơn vị đóng gói' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Đơn vị đóng gói', N'String', N'Đơn vị đóng gói (ví dụ: Hộp, Thùng, Túi, v.v.)')
    PRINT 'Đã thêm: Đơn vị đóng gói'
END
GO

-- 17. Hạn sử dụng (Expiry Date)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Hạn sử dụng' AND [DataType] = N'DateTime')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Hạn sử dụng', N'DateTime', N'Ngày hết hạn sử dụng của sản phẩm')
    PRINT 'Đã thêm: Hạn sử dụng'
END
GO

-- 18. Ngày sản xuất (Manufacturing Date)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Ngày sản xuất' AND [DataType] = N'DateTime')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Ngày sản xuất', N'DateTime', N'Ngày sản xuất của sản phẩm')
    PRINT 'Đã thêm: Ngày sản xuất'
END
GO

-- 19. Trạng thái (Status)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Trạng thái' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Trạng thái', N'String', N'Trạng thái của sản phẩm (ví dụ: Mới, Đã qua sử dụng, Tái chế, v.v.)')
    PRINT 'Đã thêm: Trạng thái'
END
GO

-- 20. Ghi chú (Notes)
IF NOT EXISTS (SELECT 1 FROM [dbo].[Attribute] WHERE [Name] = N'Ghi chú' AND [DataType] = N'String')
BEGIN
    INSERT INTO [dbo].[Attribute] ([Id], [Name], [DataType], [Description])
    VALUES (NEWID(), N'Ghi chú', N'String', N'Ghi chú hoặc mô tả bổ sung về sản phẩm')
    PRINT 'Đã thêm: Ghi chú'
END
GO

-- Kiểm tra kết quả
PRINT ''
PRINT '========================================'
PRINT 'Kết quả INSERT dữ liệu Attribute:'
PRINT '========================================'
SELECT 
    [Name] AS [Tên thuộc tính],
    [DataType] AS [Kiểu dữ liệu],
    [Description] AS [Mô tả]
FROM [dbo].[Attribute]
ORDER BY [Name]
GO

-- Kiểm tra tính duy nhất của Name và DataType
PRINT ''
PRINT '========================================'
PRINT 'Kiểm tra tính duy nhất của Name và DataType:'
PRINT '========================================'
SELECT 
    [Name],
    [DataType],
    COUNT(*) AS [Số lượng]
FROM [dbo].[Attribute]
GROUP BY [Name], [DataType]
HAVING COUNT(*) > 1
GO

IF @@ROWCOUNT = 0
BEGIN
    PRINT '✓ Không có bản ghi trùng lặp - Name và DataType là duy nhất'
END
ELSE
BEGIN
    PRINT '⚠ CẢNH BÁO: Có bản ghi trùng lặp Name và DataType!'
END
GO

PRINT ''
PRINT 'Hoàn tất script INSERT dữ liệu Attribute'
GO
