-- =============================================
-- Script kiểm tra dữ liệu BusinessPartner và BusinessPartnerCategory
-- Tạo bởi: AI Assistant
-- Ngày tạo: 2025
-- Mô tả: Script SQL để kiểm tra dữ liệu hiện tại trong database
-- =============================================

USE [VnsErp2025Final]
GO

SET NOCOUNT ON;

PRINT '=== KIỂM TRA DỮ LIỆU BUSINESS PARTNER CATEGORY ==='

-- 1. Kiểm tra số lượng categories
SELECT 
    'Total Categories' as Info,
    COUNT(*) as Count
FROM dbo.BusinessPartnerCategory

UNION ALL

-- 2. Kiểm tra số lượng partners
SELECT 
    'Total Partners' as Info,
    COUNT(*) as Count
FROM dbo.BusinessPartner

UNION ALL

-- 3. Kiểm tra số lượng mappings
SELECT 
    'Total Mappings' as Info,
    COUNT(*) as Count
FROM dbo.BusinessPartner_BusinessPartnerCategory

ORDER BY Info

GO

PRINT '=== CHI TIẾT CATEGORIES ==='

-- 4. Chi tiết categories
SELECT 
    c.Id,
    c.CategoryName,
    c.Description,
    c.ParentId,
    CASE 
        WHEN c.ParentId IS NULL THEN 'Root Category'
        ELSE 'Sub Category'
    END as CategoryType
FROM dbo.BusinessPartnerCategory c
ORDER BY c.ParentId, c.CategoryName

GO

PRINT '=== CHI TIẾT MAPPINGS ==='

-- 5. Chi tiết mappings với tên category và partner
SELECT 
    m.PartnerId,
    p.PartnerCode,
    p.PartnerName,
    m.CategoryId,
    c.CategoryName
FROM dbo.BusinessPartner_BusinessPartnerCategory m
JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId
JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId
ORDER BY c.CategoryName, p.PartnerCode

GO

PRINT '=== ĐẾM SỐ LƯỢNG PARTNERS THEO CATEGORY ==='

-- 6. Đếm số lượng partners theo category (giống logic trong code)
SELECT 
    c.Id as CategoryId,
    c.CategoryName,
    COUNT(m.PartnerId) as PartnerCount
FROM dbo.BusinessPartnerCategory c
LEFT JOIN dbo.BusinessPartner_BusinessPartnerCategory m ON m.CategoryId = c.Id
GROUP BY c.Id, c.CategoryName
ORDER BY PartnerCount DESC, c.CategoryName

GO

PRINT '=== KIỂM TRA HOÀN THÀNH ==='
