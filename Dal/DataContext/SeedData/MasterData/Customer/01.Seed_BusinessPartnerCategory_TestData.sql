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
    
    -- Tạo các category chính trước (không có ParentId)
    
    -- 0. Chưa phân loại (category mặc định - phải tồn tại đầu tiên)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Chưa phân loại')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        VALUES (NEWID(), N'Chưa phân loại', N'Danh mục mặc định cho các đối tác chưa được phân loại', NULL);
    END
    
    -- 1. Khách hàng doanh nghiệp (category chính)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Khách hàng doanh nghiệp')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        VALUES (NEWID(), N'Khách hàng doanh nghiệp', N'Đối tác là doanh nghiệp, công ty mua hàng cho hoạt động kinh doanh', NULL);
    END
    
    -- 2. Nhà cung cấp dịch vụ (category chính)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp dịch vụ')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        VALUES (NEWID(), N'Nhà cung cấp dịch vụ', N'Nhà cung cấp các dịch vụ cho doanh nghiệp', NULL);
    END
    
    -- Tạo các sub-category (có ParentId)
    
    -- 3. Khách hàng cá nhân (sub-category của Khách hàng doanh nghiệp)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Khách hàng cá nhân')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Khách hàng cá nhân', N'Đối tác là cá nhân, mua hàng cho nhu cầu cá nhân', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Khách hàng doanh nghiệp';
    END
    
    -- 4. Nhà cung cấp nguyên vật liệu (sub-category của Nhà cung cấp dịch vụ)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp nguyên vật liệu')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Nhà cung cấp nguyên vật liệu', N'Nhà cung cấp các loại nguyên vật liệu, phụ liệu cho sản xuất', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Nhà cung cấp dịch vụ';
    END
    
    -- 5. Nhà cung cấp thiết bị (sub-category của Nhà cung cấp dịch vụ)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp thiết bị')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Nhà cung cấp thiết bị', N'Nhà cung cấp máy móc, thiết bị, công cụ sản xuất', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Nhà cung cấp dịch vụ';
    END
    
    -- 6. Đại lý phân phối (sub-category của Khách hàng doanh nghiệp)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Đại lý phân phối')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Đại lý phân phối', N'Đối tác làm đại lý phân phối sản phẩm của công ty', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Khách hàng doanh nghiệp';
    END
    
    -- 7. Nhà bán lẻ (sub-category của Khách hàng doanh nghiệp)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà bán lẻ')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Nhà bán lẻ', N'Đối tác bán lẻ sản phẩm trực tiếp cho người tiêu dùng cuối', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Khách hàng doanh nghiệp';
    END
    
    -- 8. Nhà bán buôn (sub-category của Khách hàng doanh nghiệp)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà bán buôn')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Nhà bán buôn', N'Đối tác bán buôn sản phẩm cho các nhà bán lẻ khác', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Khách hàng doanh nghiệp';
    END
    
    -- 9. Đối tác chiến lược (category chính)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Đối tác chiến lược')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        VALUES (NEWID(), N'Đối tác chiến lược', N'Đối tác hợp tác chiến lược dài hạn, cùng phát triển', NULL);
    END
    
    -- 10. Khách hàng tiềm năng (sub-category của Khách hàng doanh nghiệp)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Khách hàng tiềm năng')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Khách hàng tiềm năng', N'Khách hàng có khả năng mua hàng trong tương lai', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Khách hàng doanh nghiệp';
    END
    
    -- 11. Nhà cung cấp công nghệ (sub-category của Nhà cung cấp dịch vụ)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp công nghệ')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Nhà cung cấp công nghệ', N'Nhà cung cấp các giải pháp công nghệ, phần mềm, hệ thống', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Nhà cung cấp dịch vụ';
    END
    
    -- 12. Đối tác tài chính (category chính)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Đối tác tài chính')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        VALUES (NEWID(), N'Đối tác tài chính', N'Ngân hàng, tổ chức tài chính, nhà đầu tư', NULL);
    END
    
    -- 13. Nhà cung cấp marketing (sub-category của Nhà cung cấp dịch vụ)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp marketing')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Nhà cung cấp marketing', N'Nhà cung cấp dịch vụ quảng cáo, marketing, PR', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Nhà cung cấp dịch vụ';
    END
    
    -- 14. Đối tác logistics (sub-category của Nhà cung cấp dịch vụ)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Đối tác logistics')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Đối tác logistics', N'Đối tác cung cấp dịch vụ vận chuyển, kho bãi, phân phối', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Nhà cung cấp dịch vụ';
    END
    
    -- 15. Nhà cung cấp năng lượng (sub-category của Nhà cung cấp dịch vụ)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp năng lượng')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description], ParentId)
        SELECT NEWID(), N'Nhà cung cấp năng lượng', N'Nhà cung cấp điện, nước, gas, nhiên liệu cho hoạt động sản xuất', c.Id
        FROM dbo.BusinessPartnerCategory c WHERE c.CategoryName = N'Nhà cung cấp dịch vụ';
    END

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(N'Lỗi khi seed BusinessPartnerCategory data: %s', 16, 1, @ErrMsg);
END CATCH

GO

-- Kiểm tra dữ liệu đã insert với cấu trúc hierarchical
SELECT 
    [Id],
    [CategoryName],
    [Description],
    [ParentId],
    CASE 
        WHEN [ParentId] IS NULL THEN N'Category chính'
        ELSE N'Sub-category'
    END as CategoryType,
    COUNT(*) OVER() as TotalRecords
FROM [dbo].[BusinessPartnerCategory]
ORDER BY 
    CASE WHEN [ParentId] IS NULL THEN 0 ELSE 1 END,
    [CategoryName]

GO

-- Hiển thị cấu trúc hierarchical
SELECT 
    c1.[CategoryName] as ParentCategory,
    c2.[CategoryName] as SubCategory,
    c2.[Description] as SubDescription
FROM [dbo].[BusinessPartnerCategory] c1
LEFT JOIN [dbo].[BusinessPartnerCategory] c2 ON c2.[ParentId] = c1.[Id]
WHERE c1.[ParentId] IS NULL
ORDER BY c1.[CategoryName], c2.[CategoryName]

GO

-- Thống kê số lượng category theo loại
SELECT 
    CASE 
        WHEN [ParentId] IS NULL THEN N'Category chính'
        ELSE N'Sub-category'
    END as CategoryType,
    COUNT(*) as Count
FROM [dbo].[BusinessPartnerCategory]
GROUP BY 
    CASE 
        WHEN [ParentId] IS NULL THEN N'Category chính'
        ELSE N'Sub-category'
    END

UNION ALL

SELECT 
    'Tổng số danh mục đối tác' as CategoryType,
    COUNT(*) as Count
FROM [dbo].[BusinessPartnerCategory]

ORDER BY CategoryType

GO

PRINT 'Đã hoàn thành việc tạo dữ liệu mẫu cho BusinessPartnerCategory'
PRINT 'Tổng số bản ghi đã thêm: 16 danh mục đối tác (5 category chính + 11 sub-category)'
PRINT 'Bao gồm category "Chưa phân loại" làm danh mục mặc định'
