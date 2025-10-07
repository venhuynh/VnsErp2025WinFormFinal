-- =============================================
-- Script: Tạo dữ liệu test cho bảng CompanyBranches
-- Description: Tạo dữ liệu mẫu cho các chi nhánh
-- Author: VNS ERP Team
-- Created: 2025-01-07
-- =============================================

PRINT '🏢 Bắt đầu tạo dữ liệu CompanyBranches...'

-- Tạo CompanyBranches
INSERT INTO CompanyBranches (Id, CompanyId, BranchCode, BranchName, Address, Phone, Email, ManagerName, IsActive, CreatedDate, UpdatedDate)
VALUES 
    -- Chi nhánh của VNS Technology
    ('33333333-3333-3333-3333-333333333333', '11111111-1111-1111-1111-111111111111', 'VNS001-HN', 'Chi nhánh Hà Nội', '789 Tràng Tiền, Hoàn Kiếm, Hà Nội', '024-1234-5678', 'hanoi@vnstech.com', 'Nguyễn Văn A', 1, GETDATE()-300, GETDATE()-20),
    ('44444444-4444-4444-4444-444444444444', '11111111-1111-1111-1111-111111111111', 'VNS001-DN', 'Chi nhánh Đà Nẵng', '321 Lê Duẩn, Hải Châu, Đà Nẵng', '0236-1234-5678', 'danang@vnstech.com', 'Trần Thị B', 1, GETDATE()-250, GETDATE()-10),
    -- Chi nhánh của VNS Solutions
    ('55555555-5555-5555-5555-555555555555', '22222222-2222-2222-2222-222222222222', 'VNS002-HN', 'Chi nhánh Hà Nội', '654 Ba Đình, Ba Đình, Hà Nội', '024-8765-4321', 'hanoi@vnssolutions.com', 'Lê Văn C', 1, GETDATE()-200, GETDATE()-5)

PRINT '✅ Đã tạo 3 CompanyBranches'
