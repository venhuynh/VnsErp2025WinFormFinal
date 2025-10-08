-- =============================================
-- Script: Tạo dữ liệu test cho bảng Companies
-- Description: Tạo dữ liệu mẫu cho các công ty
-- Author: VNS ERP Team
-- Created: 2025-01-07
-- =============================================

PRINT '🏢 Bắt đầu tạo dữ liệu Companies...'

-- Tạo Companies
INSERT INTO Companies (Id, CompanyCode, CompanyName, TaxCode, Phone, Email, Website, Address, Country, LogoPath, IsActive, CreatedDate, UpdatedDate)
VALUES 
    ('11111111-1111-1111-1111-111111111111', 'VNS001', 'Công ty TNHH VNS Technology', '0123456789', '028-1234-5678', 'info@vnstech.com', 'www.vnstech.com', '123 Nguyễn Huệ, Quận 1, TP.HCM', 'Việt Nam', '/images/logos/vns-logo.png', 1, GETDATE()-365, GETDATE()-30),
    ('22222222-2222-2222-2222-222222222222', 'VNS002', 'Công ty TNHH VNS Solutions', '0987654321', '028-8765-4321', 'contact@vnssolutions.com', 'www.vnssolutions.com', '456 Lê Lợi, Quận 3, TP.HCM', 'Việt Nam', '/images/logos/vns-solutions-logo.png', 1, GETDATE()-300, GETDATE()-15)

PRINT '✅ Đã tạo 2 Companies'
