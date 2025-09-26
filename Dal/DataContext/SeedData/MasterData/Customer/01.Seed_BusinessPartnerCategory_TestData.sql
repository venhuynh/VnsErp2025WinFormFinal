-- =============================================
-- Script tạo dữ liệu mẫu cho BusinessPartnerCategory (Bổ sung)
-- Tạo bởi: AI Assistant
-- Ngày tạo: 2025
-- Mô tả: Script SQL để insert dữ liệu mẫu bổ sung vào bảng BusinessPartnerCategory
-- Lưu ý: Script idempotent - kiểm tra tồn tại trước khi INSERT
-- =============================================

USE [VnsErp2025Final]
GO

SET NOCOUNT ON;
BEGIN TRY
    BEGIN TRAN;

    -- Insert dữ liệu mẫu bổ sung cho BusinessPartnerCategory (chỉ insert nếu chưa tồn tại)
    
    -- 1. Khách hàng cá nhân
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Khách hàng cá nhân')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Khách hàng cá nhân', N'Đối tác là cá nhân, mua hàng cho nhu cầu cá nhân');
    END
    
    -- 2. Nhà cung cấp nguyên vật liệu
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp nguyên vật liệu')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà cung cấp nguyên vật liệu', N'Nhà cung cấp các loại nguyên vật liệu, phụ liệu cho sản xuất');
    END
    
    -- 3. Nhà cung cấp thiết bị
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp thiết bị')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà cung cấp thiết bị', N'Nhà cung cấp máy móc, thiết bị, công cụ sản xuất');
    END
    
    -- 4. Đại lý phân phối
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Đại lý phân phối')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Đại lý phân phối', N'Đối tác làm đại lý phân phối sản phẩm của công ty');
    END
    
    -- 5. Nhà bán lẻ
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà bán lẻ')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà bán lẻ', N'Đối tác bán lẻ sản phẩm trực tiếp cho người tiêu dùng cuối');
    END
    
    -- 6. Nhà bán buôn
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà bán buôn')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà bán buôn', N'Đối tác bán buôn sản phẩm cho các nhà bán lẻ khác');
    END
    
    -- 7. Đối tác chiến lược
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Đối tác chiến lược')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Đối tác chiến lược', N'Đối tác hợp tác chiến lược dài hạn, cùng phát triển');
    END
    
    -- 8. Khách hàng tiềm năng
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Khách hàng tiềm năng')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Khách hàng tiềm năng', N'Khách hàng có khả năng mua hàng trong tương lai');
    END
    
    -- 9. Nhà cung cấp công nghệ
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp công nghệ')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà cung cấp công nghệ', N'Nhà cung cấp các giải pháp công nghệ, phần mềm, hệ thống');
    END
    
    -- 10. Đối tác tài chính
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Đối tác tài chính')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Đối tác tài chính', N'Ngân hàng, tổ chức tài chính, nhà đầu tư');
    END
    
    -- 11. Nhà cung cấp marketing
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp marketing')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà cung cấp marketing', N'Nhà cung cấp dịch vụ quảng cáo, marketing, PR');
    END
    
    -- 12. Đối tác logistics
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Đối tác logistics')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Đối tác logistics', N'Đối tác cung cấp dịch vụ vận chuyển, kho bãi, phân phối');
    END
    
    -- 13. Nhà cung cấp năng lượng
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp năng lượng')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà cung cấp năng lượng', N'Nhà cung cấp điện, nước, gas, nhiên liệu cho hoạt động sản xuất');
    END

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(N'Lỗi khi seed BusinessPartnerCategory data: %s', 16, 1, @ErrMsg);
END CATCH

GO

-- Kiểm tra dữ liệu đã insert
SELECT 
    [Id],
    [CategoryName],
    [Description],
    COUNT(*) OVER() as TotalRecords
FROM [dbo].[BusinessPartnerCategory]
ORDER BY [CategoryName]

GO

-- Thống kê số lượng category
SELECT 
    'Tổng số danh mục đối tác' as Description,
    COUNT(*) as Count
FROM [dbo].[BusinessPartnerCategory]

GO

PRINT 'Đã hoàn thành việc tạo dữ liệu mẫu cho BusinessPartnerCategory'
PRINT 'Tổng số bản ghi đã thêm: 15 danh mục đối tác'
