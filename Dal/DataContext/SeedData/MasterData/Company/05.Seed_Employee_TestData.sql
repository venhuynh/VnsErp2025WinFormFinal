-- =============================================
-- Script: Tạo dữ liệu test cho bảng Employees
-- Description: Tạo dữ liệu mẫu cho các nhân viên
-- Author: VNS ERP Team
-- Created: 2025-01-07
-- =============================================

PRINT '👥 Bắt đầu tạo dữ liệu Employees...'

-- Tạo Employees
INSERT INTO Employees (Id, CompanyId, BranchId, DepartmentId, PositionId, EmployeeCode, FullName, Gender, BirthDate, Phone, Email, HireDate, ResignDate, AvatarPath, IsActive, CreatedDate, UpdatedDate)
VALUES 
    -- VNS Technology employees
    ('55555555-5555-5555-5555-555555555556', '11111111-1111-1111-1111-111111111111', NULL, 'CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC', '66666666-6666-6666-6666-666666666666', 'VNS001-001', 'Nguyễn Văn Giám Đốc', 'Nam', '1975-05-15', '0901-234-567', 'ceo@vnstech.com', '2020-01-01', NULL, '/images/avatars/ceo.jpg', 1, GETDATE()-365, GETDATE()-30),
    ('66666666-6666-6666-6666-666666666667', '11111111-1111-1111-1111-111111111111', NULL, 'DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD', '77777777-7777-7777-7777-777777777777', 'VNS001-002', 'Trần Thị Công Nghệ', 'Nữ', '1980-08-20', '0902-345-678', 'cto@vnstech.com', '2020-03-01', NULL, '/images/avatars/cto.jpg', 1, GETDATE()-350, GETDATE()-25),
    ('77777777-7777-7777-7777-777777777778', '11111111-1111-1111-1111-111111111111', NULL, 'FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF', '88888888-8888-8888-8888-888888888888', 'VNS001-003', 'Lê Văn Lập Trình', 'Nam', '1990-12-10', '0903-456-789', 'dev@vnstech.com', '2021-06-01', NULL, '/images/avatars/dev.jpg', 1, GETDATE()-300, GETDATE()-20),
    ('88888888-8888-8888-8888-888888888889', '11111111-1111-1111-1111-111111111111', NULL, '22222222-2222-2222-2222-222222222223', '99999999-9999-9999-9999-999999999999', 'VNS001-004', 'Phạm Thị Nhân Sự', 'Nữ', '1985-03-25', '0904-567-890', 'hr@vnstech.com', '2021-09-01', NULL, '/images/avatars/hr.jpg', 1, GETDATE()-260, GETDATE()-15),
    
    -- VNS Solutions employees
    ('99999999-9999-9999-9999-99999999999A', '22222222-2222-2222-2222-222222222222', NULL, '33333333-3333-3333-3333-333333333334', 'AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', 'VNS002-001', 'Hoàng Văn Giám Đốc', 'Nam', '1978-07-12', '0905-678-901', 'ceo@vnssolutions.com', '2021-01-01', NULL, '/images/avatars/ceo2.jpg', 1, GETDATE()-300, GETDATE()-20),
    ('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAB', '22222222-2222-2222-2222-222222222222', NULL, '44444444-4444-4444-4444-444444444445', 'BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB', 'VNS002-002', 'Võ Thị Kinh Doanh', 'Nữ', '1988-11-05', '0906-789-012', 'sales@vnssolutions.com', '2021-04-01', NULL, '/images/avatars/sales.jpg', 1, GETDATE()-250, GETDATE()-10)

PRINT '✅ Đã tạo 6 Employees'
