-- =============================================
-- Script tạo dữ liệu mẫu cho BusinessPartner_BusinessPartnerCategory (Mapping)
-- Tạo bởi: AI Assistant
-- Ngày tạo: 2025
-- Mô tả: Script SQL để tạo mapping giữa BusinessPartner và BusinessPartnerCategory
-- Lưu ý: Script idempotent - kiểm tra tồn tại trước khi INSERT
-- =============================================

USE [VnsErp2025Final]
GO

SET NOCOUNT ON;
BEGIN TRY
    BEGIN TRAN;

    -- Insert dữ liệu mapping giữa BusinessPartner và BusinessPartnerCategory
    
    -- 1. CUS001 -> Khách hàng doanh nghiệp
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.BusinessPartner_BusinessPartnerCategory m
        JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = 'CUS001'
        JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = N'Khách hàng doanh nghiệp')
    BEGIN
        INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
        SELECT p.Id, c.Id
        FROM dbo.BusinessPartner p
        JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = N'Khách hàng doanh nghiệp'
        WHERE p.PartnerCode = 'CUS001';
    END

    -- 2. VEN001 -> Nhà cung cấp dịch vụ
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.BusinessPartner_BusinessPartnerCategory m
        JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = 'VEN001'
        JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = N'Nhà cung cấp dịch vụ')
    BEGIN
        INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
        SELECT p.Id, c.Id
        FROM dbo.BusinessPartner p
        JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = N'Nhà cung cấp dịch vụ'
        WHERE p.PartnerCode = 'VEN001';
    END

    -- 3. BOTH001 -> Khách hàng doanh nghiệp + Nhà cung cấp dịch vụ
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.BusinessPartner_BusinessPartnerCategory m
        JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = 'BOTH001'
        JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = N'Khách hàng doanh nghiệp')
    BEGIN
        INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
        SELECT p.Id, c.Id
        FROM dbo.BusinessPartner p
        JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = N'Khách hàng doanh nghiệp'
        WHERE p.PartnerCode = 'BOTH001';
    END

    IF NOT EXISTS (
        SELECT 1
        FROM dbo.BusinessPartner_BusinessPartnerCategory m
        JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = 'BOTH001'
        JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = N'Nhà cung cấp dịch vụ')
    BEGIN
        INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
        SELECT p.Id, c.Id
        FROM dbo.BusinessPartner p
        JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = N'Nhà cung cấp dịch vụ'
        WHERE p.PartnerCode = 'BOTH001';
    END

    -- 4. Bulk mapping cho các TEST partners (TEST001-TEST100)
    DECLARE @i INT = 1;
    WHILE (@i <= 100)
    BEGIN
        DECLARE @code NVARCHAR(20) = CONCAT('TEST', RIGHT('000' + CAST(@i AS VARCHAR(3)), 3));
        DECLARE @type INT = ((@i - 1) % 3) + 1; -- 1..3
        
        -- Mapping dựa trên PartnerType
        IF (@type = 1) -- Khách hàng
        BEGIN
            -- Khách hàng cá nhân hoặc doanh nghiệp
            DECLARE @categoryName NVARCHAR(100) = CASE WHEN (@i % 2 = 0) THEN N'Khách hàng doanh nghiệp' ELSE N'Khách hàng cá nhân' END;
            
            IF NOT EXISTS (
                SELECT 1
                FROM dbo.BusinessPartner_BusinessPartnerCategory m
                JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = @code
                JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = @categoryName)
            BEGIN
                INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
                SELECT p.Id, c.Id
                FROM dbo.BusinessPartner p
                JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = @categoryName
                WHERE p.PartnerCode = @code;
            END
        END
        ELSE IF (@type = 2) -- Nhà cung cấp
        BEGIN
            -- Nhà cung cấp (ngẫu nhiên loại)
            DECLARE @supplierCategory NVARCHAR(100) = CASE (@i % 4)
                WHEN 0 THEN N'Nhà cung cấp dịch vụ'
                WHEN 1 THEN N'Nhà cung cấp nguyên vật liệu'
                WHEN 2 THEN N'Nhà cung cấp thiết bị'
                ELSE N'Nhà cung cấp công nghệ' END;
            
            IF NOT EXISTS (
                SELECT 1
                FROM dbo.BusinessPartner_BusinessPartnerCategory m
                JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = @code
                JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = @supplierCategory)
            BEGIN
                INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
                SELECT p.Id, c.Id
                FROM dbo.BusinessPartner p
                JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = @supplierCategory
                WHERE p.PartnerCode = @code;
            END
        END
        ELSE IF (@type = 3) -- Cả hai
        BEGIN
            -- Mapping cho cả khách hàng và nhà cung cấp
            -- Khách hàng doanh nghiệp
            IF NOT EXISTS (
                SELECT 1
                FROM dbo.BusinessPartner_BusinessPartnerCategory m
                JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = @code
                JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = N'Khách hàng doanh nghiệp')
            BEGIN
                INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
                SELECT p.Id, c.Id
                FROM dbo.BusinessPartner p
                JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = N'Khách hàng doanh nghiệp'
                WHERE p.PartnerCode = @code;
            END

            -- Nhà cung cấp dịch vụ
            IF NOT EXISTS (
                SELECT 1
                FROM dbo.BusinessPartner_BusinessPartnerCategory m
                JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = @code
                JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = N'Nhà cung cấp dịch vụ')
            BEGIN
                INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
                SELECT p.Id, c.Id
                FROM dbo.BusinessPartner p
                JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = N'Nhà cung cấp dịch vụ'
                WHERE p.PartnerCode = @code;
            END
        END

        SET @i = @i + 1;
    END

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(N'Lỗi khi seed BusinessPartner_BusinessPartnerCategory mapping data: %s', 16, 1, @ErrMsg);
END CATCH

GO

-- Kiểm tra dữ liệu mapping đã insert
SELECT 
    p.[PartnerCode],
    p.[PartnerName],
    p.[PartnerType],
    c.[CategoryName],
    c.[Description] as CategoryDescription
FROM [dbo].[BusinessPartner_BusinessPartnerCategory] m
JOIN [dbo].[BusinessPartner] p ON p.Id = m.PartnerId
JOIN [dbo].[BusinessPartnerCategory] c ON c.Id = m.CategoryId
ORDER BY p.[PartnerCode], c.[CategoryName]

GO

-- Thống kê mapping theo category
SELECT 
    c.[CategoryName],
    COUNT(m.[PartnerId]) as PartnerCount,
    STRING_AGG(p.[PartnerCode], ', ') as PartnerCodes
FROM [dbo].[BusinessPartnerCategory] c
LEFT JOIN [dbo].[BusinessPartner_BusinessPartnerCategory] m ON m.CategoryId = c.Id
LEFT JOIN [dbo].[BusinessPartner] p ON p.Id = m.PartnerId
GROUP BY c.[CategoryName]
ORDER BY PartnerCount DESC, c.[CategoryName]

GO

-- Thống kê mapping theo partner type
SELECT 
    CASE p.[PartnerType]
        WHEN 1 THEN N'Khách hàng'
        WHEN 2 THEN N'Nhà cung cấp'
        WHEN 3 THEN N'Cả hai'
        ELSE N'Không xác định'
    END as PartnerTypeName,
    COUNT(DISTINCT p.[Id]) as PartnerCount,
    COUNT(m.[CategoryId]) as TotalMappings
FROM [dbo].[BusinessPartner] p
LEFT JOIN [dbo].[BusinessPartner_BusinessPartnerCategory] m ON m.PartnerId = p.Id
GROUP BY p.[PartnerType]
ORDER BY p.[PartnerType]

GO

PRINT 'Đã hoàn thành việc tạo dữ liệu mapping cho BusinessPartner_BusinessPartnerCategory'
