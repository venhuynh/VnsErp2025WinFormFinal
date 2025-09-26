-- =============================================
-- Script tạo dữ liệu mapping test cho BusinessPartner_BusinessPartnerCategory
-- Tạo bởi: AI Assistant
-- Ngày tạo: 2025
-- Mô tả: Script SQL để tạo mapping test giữa BusinessPartner và BusinessPartnerCategory
-- =============================================

USE [VnsErp2025Final]
GO

SET NOCOUNT ON;
BEGIN TRY
    BEGIN TRAN;

    PRINT '=== TẠO DỮ LIỆU MAPPING TEST ==='

    -- Kiểm tra số lượng BusinessPartner hiện có
    DECLARE @PartnerCount INT = (SELECT COUNT(*) FROM dbo.BusinessPartner);
    PRINT 'Số lượng BusinessPartner hiện có: ' + CAST(@PartnerCount AS VARCHAR(10));

    -- Kiểm tra số lượng BusinessPartnerCategory hiện có
    DECLARE @CategoryCount INT = (SELECT COUNT(*) FROM dbo.BusinessPartnerCategory);
    PRINT 'Số lượng BusinessPartnerCategory hiện có: ' + CAST(@CategoryCount AS VARCHAR(10));

    -- Nếu không có BusinessPartner, tạo một số test
    IF @PartnerCount = 0
    BEGIN
        PRINT 'Tạo BusinessPartner test...';
        
        -- Tạo 5 BusinessPartner test
        INSERT INTO dbo.BusinessPartner (Id, PartnerCode, PartnerName, PartnerType, IsActive, CreatedDate, ModifiedDate)
        VALUES 
            (NEWID(), 'TEST001', 'Công ty ABC', 1, 1, GETDATE(), GETDATE()),
            (NEWID(), 'TEST002', 'Công ty XYZ', 1, 1, GETDATE(), GETDATE()),
            (NEWID(), 'SUP001', 'Nhà cung cấp DEF', 2, 1, GETDATE(), GETDATE()),
            (NEWID(), 'SUP002', 'Nhà cung cấp GHI', 2, 1, GETDATE(), GETDATE()),
            (NEWID(), 'BOTH001', 'Đối tác JKL', 3, 1, GETDATE(), GETDATE());
            
        PRINT 'Đã tạo 5 BusinessPartner test';
    END

    -- Tạo mapping giữa BusinessPartner và BusinessPartnerCategory
    PRINT 'Tạo mapping...';
    
    -- Xóa mapping cũ nếu có
    DELETE FROM dbo.BusinessPartner_BusinessPartnerCategory;
    
    -- Lấy ID của một số categories
    DECLARE @CustomerCategoryId UNIQUEIDENTIFIER = (
        SELECT Id FROM dbo.BusinessPartnerCategory 
        WHERE CategoryName = N'Khách hàng doanh nghiệp'
    );
    
    DECLARE @SupplierCategoryId UNIQUEIDENTIFIER = (
        SELECT Id FROM dbo.BusinessPartnerCategory 
        WHERE CategoryName = N'Nhà cung cấp dịch vụ'
    );
    
    DECLARE @UncategorizedId UNIQUEIDENTIFIER = (
        SELECT Id FROM dbo.BusinessPartnerCategory 
        WHERE CategoryName = N'Chưa phân loại'
    );

    -- Tạo mapping cho tất cả BusinessPartner
    INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
    SELECT 
        p.Id,
        CASE 
            WHEN p.PartnerType = 1 THEN @CustomerCategoryId  -- Khách hàng
            WHEN p.PartnerType = 2 THEN @SupplierCategoryId  -- Nhà cung cấp
            WHEN p.PartnerType = 3 THEN @UncategorizedId     -- Cả hai
            ELSE @UncategorizedId
        END
    FROM dbo.BusinessPartner p
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.BusinessPartner_BusinessPartnerCategory m 
        WHERE m.PartnerId = p.Id
    );

    DECLARE @MappingCount INT = @@ROWCOUNT;
    PRINT 'Đã tạo ' + CAST(@MappingCount AS VARCHAR(10)) + ' mapping';

    -- Hiển thị kết quả
    PRINT ''
    PRINT '=== KẾT QUẢ MAPPING ==='
    
    SELECT 
        p.PartnerCode,
        p.PartnerName,
        p.PartnerType,
        c.CategoryName,
        CASE 
            WHEN p.PartnerType = 1 THEN 'Khách hàng'
            WHEN p.PartnerType = 2 THEN 'Nhà cung cấp'
            WHEN p.PartnerType = 3 THEN 'Cả hai'
            ELSE 'Khác'
        END as PartnerTypeText
    FROM dbo.BusinessPartner_BusinessPartnerCategory m
    JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId
    JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId
    ORDER BY p.PartnerCode;

    -- Đếm theo category
    PRINT ''
    PRINT '=== ĐẾM THEO CATEGORY ==='
    
    SELECT 
        c.CategoryName,
        COUNT(m.PartnerId) as PartnerCount
    FROM dbo.BusinessPartnerCategory c
    LEFT JOIN dbo.BusinessPartner_BusinessPartnerCategory m ON m.CategoryId = c.Id
    GROUP BY c.Id, c.CategoryName
    ORDER BY PartnerCount DESC, c.CategoryName;

    COMMIT TRAN;
    PRINT ''
    PRINT '✓ Hoàn thành tạo dữ liệu mapping test';

END TRY
BEGIN CATCH
    ROLLBACK TRAN;
    PRINT '✗ Lỗi: ' + ERROR_MESSAGE();
    THROW;
END CATCH

GO
