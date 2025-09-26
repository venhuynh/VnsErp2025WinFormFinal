-- =============================================
-- Script tạo dữ liệu BusinessPartner và Mapping
-- Tạo bởi: AI Assistant
-- Ngày tạo: 2025
-- Mô tả: Script SQL để tạo dữ liệu BusinessPartner và mapping với BusinessPartnerCategory
-- =============================================

USE [VnsErp2025Final]
GO

SET NOCOUNT ON;
BEGIN TRY
    BEGIN TRAN;

    PRINT '=== TẠO DỮ LIỆU BUSINESS PARTNER VÀ MAPPING ==='

    -- 1. Kiểm tra dữ liệu hiện có
    DECLARE @PartnerCount INT = (SELECT COUNT(*) FROM dbo.BusinessPartner);
    DECLARE @CategoryCount INT = (SELECT COUNT(*) FROM dbo.BusinessPartnerCategory);
    DECLARE @MappingCount INT = (SELECT COUNT(*) FROM dbo.BusinessPartner_BusinessPartnerCategory);

    PRINT 'Dữ liệu hiện có:'
    PRINT '  - BusinessPartner: ' + CAST(@PartnerCount AS VARCHAR(10))
    PRINT '  - BusinessPartnerCategory: ' + CAST(@CategoryCount AS VARCHAR(10))
    PRINT '  - Mapping: ' + CAST(@MappingCount AS VARCHAR(10))

    -- 2. Tạo BusinessPartner nếu chưa có
    IF @PartnerCount = 0
    BEGIN
        PRINT ''
        PRINT 'Tạo dữ liệu BusinessPartner...'
        
        -- Tạo 10 BusinessPartner test
        INSERT INTO dbo.BusinessPartner (Id, PartnerCode, PartnerName, PartnerType, TaxCode, Phone, Email, Address, City, Country, ContactPerson, IsActive, CreatedDate)
        VALUES 
            -- Khách hàng (PartnerType = 1)
            (NEWID(), 'CUS001', 'Công ty TNHH ABC', 1, '0123456789', '024-1234567', 'info@abc.com', '123 Đường ABC, Quận 1', 'TP.HCM', 'Việt Nam', 'Nguyễn Văn A', 1, GETDATE()),
            (NEWID(), 'CUS002', 'Công ty Cổ phần XYZ', 1, '0123456790', '024-1234568', 'info@xyz.com', '456 Đường XYZ, Quận 2', 'TP.HCM', 'Việt Nam', 'Trần Thị B', 1, GETDATE()),
            (NEWID(), 'CUS003', 'Doanh nghiệp DEF', 1, '0123456791', '024-1234569', 'info@def.com', '789 Đường DEF, Quận 3', 'TP.HCM', 'Việt Nam', 'Lê Văn C', 1, GETDATE()),
            
            -- Nhà cung cấp (PartnerType = 2)
            (NEWID(), 'SUP001', 'Nhà cung cấp GHI', 2, '0123456792', '024-1234570', 'info@ghi.com', '321 Đường GHI, Quận 4', 'TP.HCM', 'Việt Nam', 'Phạm Thị D', 1, GETDATE()),
            (NEWID(), 'SUP002', 'Công ty JKL', 2, '0123456793', '024-1234571', 'info@jkl.com', '654 Đường JKL, Quận 5', 'TP.HCM', 'Việt Nam', 'Hoàng Văn E', 1, GETDATE()),
            (NEWID(), 'SUP003', 'Nhà cung cấp MNO', 2, '0123456794', '024-1234572', 'info@mno.com', '987 Đường MNO, Quận 6', 'TP.HCM', 'Việt Nam', 'Võ Thị F', 1, GETDATE()),
            
            -- Cả hai (PartnerType = 3)
            (NEWID(), 'BOTH001', 'Đối tác PQR', 3, '0123456795', '024-1234573', 'info@pqr.com', '147 Đường PQR, Quận 7', 'TP.HCM', 'Việt Nam', 'Đặng Văn G', 1, GETDATE()),
            (NEWID(), 'BOTH002', 'Công ty STU', 3, '0123456796', '024-1234574', 'info@stu.com', '258 Đường STU, Quận 8', 'TP.HCM', 'Việt Nam', 'Bùi Thị H', 1, GETDATE()),
            
            -- Khách hàng cá nhân (PartnerType = 1)
            (NEWID(), 'CUS004', 'Nguyễn Văn I', 1, NULL, '0901234567', 'nguyenvani@email.com', '369 Đường VWX, Quận 9', 'TP.HCM', 'Việt Nam', 'Nguyễn Văn I', 1, GETDATE()),
            (NEWID(), 'CUS005', 'Trần Thị K', 1, NULL, '0901234568', 'tranthik@email.com', '741 Đường YZA, Quận 10', 'TP.HCM', 'Việt Nam', 'Trần Thị K', 1, GETDATE());
            
        PRINT 'Đã tạo 10 BusinessPartner test'
    END
    ELSE
    BEGIN
        PRINT ''
        PRINT 'BusinessPartner đã có dữ liệu, bỏ qua tạo mới'
    END

    -- 3. Xóa mapping cũ nếu có
    IF @MappingCount > 0
    BEGIN
        PRINT ''
        PRINT 'Xóa mapping cũ...'
        DELETE FROM dbo.BusinessPartner_BusinessPartnerCategory;
        PRINT 'Đã xóa ' + CAST(@@ROWCOUNT AS VARCHAR(10)) + ' mapping cũ'
    END

    -- 4. Tạo mapping mới
    PRINT ''
    PRINT 'Tạo mapping mới...'
    
    -- Lấy ID của các categories
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

    DECLARE @IndividualCustomerId UNIQUEIDENTIFIER = (
        SELECT Id FROM dbo.BusinessPartnerCategory 
        WHERE CategoryName = N'Khách hàng cá nhân'
    );

    -- Lấy ID của một số sub-categories để test
    DECLARE @RetailCategoryId UNIQUEIDENTIFIER = (
        SELECT Id FROM dbo.BusinessPartnerCategory 
        WHERE CategoryName = N'Nhà bán lẻ'
    );
    
    DECLARE @WholesaleCategoryId UNIQUEIDENTIFIER = (
        SELECT Id FROM dbo.BusinessPartnerCategory 
        WHERE CategoryName = N'Nhà bán buôn'
    );
    
    DECLARE @TechnologyCategoryId UNIQUEIDENTIFIER = (
        SELECT Id FROM dbo.BusinessPartnerCategory 
        WHERE CategoryName = N'Nhà cung cấp công nghệ'
    );

    -- Tạo mapping cho tất cả BusinessPartner
    INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
    SELECT 
        p.Id,
        CASE 
            WHEN p.PartnerType = 1 AND p.TaxCode IS NOT NULL THEN @CustomerCategoryId  -- Khách hàng doanh nghiệp
            WHEN p.PartnerType = 1 AND p.TaxCode IS NULL THEN @IndividualCustomerId    -- Khách hàng cá nhân
            WHEN p.PartnerType = 2 THEN @SupplierCategoryId  -- Nhà cung cấp
            WHEN p.PartnerType = 3 THEN @UncategorizedId     -- Cả hai
            ELSE @UncategorizedId
        END
    FROM dbo.BusinessPartner p
    WHERE NOT EXISTS (
        SELECT 1 FROM dbo.BusinessPartner_BusinessPartnerCategory m 
        WHERE m.PartnerId = p.Id
    );

    DECLARE @NewMappingCount INT = @@ROWCOUNT;
    PRINT 'Đã tạo ' + CAST(@NewMappingCount AS VARCHAR(10)) + ' mapping mới'

    -- Tạo thêm một số mapping cho sub-categories để test
    PRINT 'Tạo mapping cho sub-categories...'
    
    -- Mapping một số partners vào sub-categories
    INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
    SELECT TOP 2 p.Id, @RetailCategoryId
    FROM dbo.BusinessPartner p
    WHERE p.PartnerType = 1 AND p.TaxCode IS NOT NULL
    AND NOT EXISTS (
        SELECT 1 FROM dbo.BusinessPartner_BusinessPartnerCategory m 
        WHERE m.PartnerId = p.Id AND m.CategoryId = @RetailCategoryId
    );

    INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
    SELECT TOP 2 p.Id, @WholesaleCategoryId
    FROM dbo.BusinessPartner p
    WHERE p.PartnerType = 1 AND p.TaxCode IS NOT NULL
    AND NOT EXISTS (
        SELECT 1 FROM dbo.BusinessPartner_BusinessPartnerCategory m 
        WHERE m.PartnerId = p.Id AND m.CategoryId = @WholesaleCategoryId
    );

    INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
    SELECT TOP 1 p.Id, @TechnologyCategoryId
    FROM dbo.BusinessPartner p
    WHERE p.PartnerType = 2
    AND NOT EXISTS (
        SELECT 1 FROM dbo.BusinessPartner_BusinessPartnerCategory m 
        WHERE m.PartnerId = p.Id AND m.CategoryId = @TechnologyCategoryId
    );

    DECLARE @SubMappingCount INT = @@ROWCOUNT;
    PRINT 'Đã tạo thêm ' + CAST(@SubMappingCount AS VARCHAR(10)) + ' mapping cho sub-categories'

    -- 5. Hiển thị kết quả
    PRINT ''
    PRINT '=== KẾT QUẢ MAPPING ==='
    
    SELECT 
        p.PartnerCode,
        p.PartnerName,
        CASE 
            WHEN p.PartnerType = 1 THEN 'Khách hàng'
            WHEN p.PartnerType = 2 THEN 'Nhà cung cấp'
            WHEN p.PartnerType = 3 THEN 'Cả hai'
            ELSE 'Khác'
        END as PartnerTypeText,
        c.CategoryName,
        CASE 
            WHEN p.TaxCode IS NOT NULL THEN 'Doanh nghiệp'
            ELSE 'Cá nhân'
        END as PartnerSubType
    FROM dbo.BusinessPartner_BusinessPartnerCategory m
    JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId
    JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId
    ORDER BY p.PartnerCode;

    -- 6. Đếm theo category
    PRINT ''
    PRINT '=== ĐẾM THEO CATEGORY ==='
    
    SELECT 
        c.CategoryName,
        COUNT(m.PartnerId) as PartnerCount
    FROM dbo.BusinessPartnerCategory c
    LEFT JOIN dbo.BusinessPartner_BusinessPartnerCategory m ON m.CategoryId = c.Id
    GROUP BY c.Id, c.CategoryName
    ORDER BY PartnerCount DESC, c.CategoryName;

    -- 7. Thống kê tổng quan
    PRINT ''
    PRINT '=== THỐNG KÊ TỔNG QUAN ==='
    
    SELECT 
        'BusinessPartner' as TableName,
        COUNT(*) as RecordCount
    FROM dbo.BusinessPartner
    
    UNION ALL
    
    SELECT 
        'BusinessPartnerCategory' as TableName,
        COUNT(*) as RecordCount
    FROM dbo.BusinessPartnerCategory
    
    UNION ALL
    
    SELECT 
        'BusinessPartner_BusinessPartnerCategory' as TableName,
        COUNT(*) as RecordCount
    FROM dbo.BusinessPartner_BusinessPartnerCategory;

    COMMIT TRAN;
    PRINT ''
    PRINT '✓ Hoàn thành tạo dữ liệu BusinessPartner và mapping';

END TRY
BEGIN CATCH
    ROLLBACK TRAN;
    PRINT '✗ Lỗi: ' + ERROR_MESSAGE();
    THROW;
END CATCH

GO
