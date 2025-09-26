-- =============================================
-- Script tổng hợp tạo dữ liệu mẫu cho toàn bộ BusinessPartner System
-- Tạo bởi: AI Assistant
-- Ngày tạo: 2025
-- Mô tả: Script SQL tổng hợp để tạo dữ liệu mẫu cho tất cả bảng liên quan đến BusinessPartner
-- Thứ tự thực hiện:
--   1. BusinessPartnerCategory (danh mục đối tác)
--   2. BusinessPartner (đối tác chính)
--   3. BusinessPartnerContact (liên hệ đối tác)
--   4. BusinessPartner_BusinessPartnerCategory (mapping đối tác - danh mục)
-- =============================================

USE [VnsErp2025Final]
GO

PRINT '============================================='
PRINT 'Bắt đầu tạo dữ liệu mẫu cho BusinessPartner System'
PRINT 'Thời gian: ' + CONVERT(VARCHAR(19), GETDATE(), 120)
PRINT '============================================='

-- =============================================
-- 1. TẠO DỮ LIỆU MẪU CHO BusinessPartnerCategory
-- =============================================
PRINT ''
PRINT '1. Đang tạo dữ liệu mẫu cho BusinessPartnerCategory...'

-- Chạy script tạo BusinessPartnerCategory
:r "Seed_BusinessPartnerCategory_TestData.sql"

PRINT '   ✓ Hoàn thành tạo BusinessPartnerCategory'

-- =============================================
-- 2. TẠO DỮ LIỆU MẪU CHO BusinessPartner
-- =============================================
PRINT ''
PRINT '2. Đang tạo dữ liệu mẫu cho BusinessPartner...'

-- Chạy script tạo BusinessPartner
:r "Seed_BusinessPartner_TestData.sql"

PRINT '   ✓ Hoàn thành tạo BusinessPartner'

-- =============================================
-- 3. TẠO DỮ LIỆU MẪU CHO BusinessPartnerContact
-- =============================================
PRINT ''
PRINT '3. Đang tạo dữ liệu mẫu cho BusinessPartnerContact...'

-- Chạy script tạo BusinessPartnerContact
:r "Seed_BusinessPartnerContact_TestData.sql"

PRINT '   ✓ Hoàn thành tạo BusinessPartnerContact'

-- =============================================
-- 4. TẠO DỮ LIỆU MẪU CHO BusinessPartner_BusinessPartnerCategory MAPPING
-- =============================================
PRINT ''
PRINT '4. Đang tạo dữ liệu mapping cho BusinessPartner_BusinessPartnerCategory...'

-- Chạy script tạo mapping
:r "Seed_BusinessPartnerCategoryMapping_TestData.sql"

PRINT '   ✓ Hoàn thành tạo BusinessPartner_BusinessPartnerCategory mapping'

-- =============================================
-- 5. THỐNG KÊ TỔNG QUAN
-- =============================================
PRINT ''
PRINT '============================================='
PRINT 'THỐNG KÊ TỔNG QUAN DỮ LIỆU ĐÃ TẠO'
PRINT '============================================='

-- Thống kê BusinessPartnerCategory
SELECT 
    'BusinessPartnerCategory' as TableName,
    COUNT(*) as RecordCount
FROM [dbo].[BusinessPartnerCategory]

UNION ALL

-- Thống kê BusinessPartner
SELECT 
    'BusinessPartner' as TableName,
    COUNT(*) as RecordCount
FROM [dbo].[BusinessPartner]

UNION ALL

-- Thống kê BusinessPartnerContact
SELECT 
    'BusinessPartnerContact' as TableName,
    COUNT(*) as RecordCount
FROM [dbo].[BusinessPartnerContact]

UNION ALL

-- Thống kê BusinessPartner_BusinessPartnerCategory
SELECT 
    'BusinessPartner_BusinessPartnerCategory' as TableName,
    COUNT(*) as RecordCount
FROM [dbo].[BusinessPartner_BusinessPartnerCategory]

ORDER BY TableName

-- Thống kê chi tiết theo PartnerType
PRINT ''
PRINT 'Thống kê theo loại đối tác:'
SELECT 
    CASE [PartnerType]
        WHEN 1 THEN N'Khách hàng'
        WHEN 2 THEN N'Nhà cung cấp'
        WHEN 3 THEN N'Cả hai'
        ELSE N'Không xác định'
    END as PartnerTypeName,
    COUNT(*) as PartnerCount,
    SUM(CASE WHEN [IsActive] = 1 THEN 1 ELSE 0 END) as ActiveCount,
    SUM(CASE WHEN [IsActive] = 0 THEN 1 ELSE 0 END) as InactiveCount
FROM [dbo].[BusinessPartner]
GROUP BY [PartnerType]
ORDER BY [PartnerType]

-- Thống kê contact theo partner
PRINT ''
PRINT 'Thống kê liên hệ theo đối tác:'
SELECT 
    'Tổng số đối tác có liên hệ' as Description,
    COUNT(DISTINCT p.[Id]) as PartnerCount
FROM [dbo].[BusinessPartner] p
WHERE EXISTS (SELECT 1 FROM [dbo].[BusinessPartnerContact] bc WHERE bc.PartnerId = p.Id)

UNION ALL

SELECT 
    'Tổng số liên hệ' as Description,
    COUNT(*) as ContactCount
FROM [dbo].[BusinessPartnerContact]

UNION ALL

SELECT 
    'Số liên hệ chính' as Description,
    COUNT(*) as PrimaryContactCount
FROM [dbo].[BusinessPartnerContact]
WHERE [IsPrimary] = 1

-- Thống kê mapping theo category
PRINT ''
PRINT 'Thống kê mapping theo danh mục:'
SELECT TOP 10
    c.[CategoryName],
    COUNT(m.[PartnerId]) as PartnerCount
FROM [dbo].[BusinessPartnerCategory] c
LEFT JOIN [dbo].[BusinessPartner_BusinessPartnerCategory] m ON m.CategoryId = c.Id
GROUP BY c.[CategoryName]
ORDER BY PartnerCount DESC, c.[CategoryName]

PRINT ''
PRINT '============================================='
PRINT 'HOÀN THÀNH TẠO DỮ LIỆU MẪU'
PRINT 'Thời gian: ' + CONVERT(VARCHAR(19), GETDATE(), 120)
PRINT '============================================='

-- Kiểm tra tính toàn vẹn dữ liệu
PRINT ''
PRINT 'Kiểm tra tính toàn vẹn dữ liệu:'

-- Kiểm tra BusinessPartner không có contact
SELECT 
    'Đối tác không có liên hệ' as CheckType,
    COUNT(*) as Count
FROM [dbo].[BusinessPartner] p
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BusinessPartnerContact] bc WHERE bc.PartnerId = p.Id)

UNION ALL

-- Kiểm tra BusinessPartner không có category
SELECT 
    'Đối tác không có danh mục' as CheckType,
    COUNT(*) as Count
FROM [dbo].[BusinessPartner] p
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BusinessPartner_BusinessPartnerCategory] m WHERE m.PartnerId = p.Id)

UNION ALL

-- Kiểm tra Contact không có partner
SELECT 
    'Liên hệ không có đối tác' as CheckType,
    COUNT(*) as Count
FROM [dbo].[BusinessPartnerContact] bc
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BusinessPartner] p WHERE p.Id = bc.PartnerId)

UNION ALL

-- Kiểm tra Mapping không có partner
SELECT 
    'Mapping không có đối tác' as CheckType,
    COUNT(*) as Count
FROM [dbo].[BusinessPartner_BusinessPartnerCategory] m
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BusinessPartner] p WHERE p.Id = m.PartnerId)

UNION ALL

-- Kiểm tra Mapping không có category
SELECT 
    'Mapping không có danh mục' as CheckType,
    COUNT(*) as Count
FROM [dbo].[BusinessPartner_BusinessPartnerCategory] m
WHERE NOT EXISTS (SELECT 1 FROM [dbo].[BusinessPartnerCategory] c WHERE c.Id = m.CategoryId)

PRINT ''
PRINT 'Dữ liệu mẫu đã được tạo thành công!'
PRINT 'Bạn có thể sử dụng các form BusinessPartner để kiểm tra dữ liệu.'
