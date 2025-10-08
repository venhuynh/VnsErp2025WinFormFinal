-- =============================================
-- Script: Tạo dữ liệu test cho bảng Departments
-- Description: Tạo dữ liệu mẫu cho các phòng ban với cấu trúc phân cấp
-- Author: VNS ERP Team
-- Created: 2025-01-07
-- =============================================

PRINT '🏢 Bắt đầu tạo dữ liệu Departments...'

-- Tạo Departments
INSERT INTO Departments (Id, CompanyId, BranchId, DepartmentCode, DepartmentName, ParentId, Description, IsActive, CreatedDate, UpdatedDate)
VALUES 
    -- VNS Technology - Cấp 1 (Phòng ban chính)
    ('CCCCCCCC-CCCC-CCCC-CCCC-CCCCCCCCCCCC', '11111111-1111-1111-1111-111111111111', NULL, 'CEO', 'Ban Giám Đốc', NULL, 'Ban lãnh đạo cao nhất của công ty', 1, GETDATE()-365, GETDATE()-30),
    ('DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD', '11111111-1111-1111-1111-111111111111', NULL, 'IT', 'Phòng Công Nghệ Thông Tin', NULL, 'Phát triển và bảo trì hệ thống công nghệ', 1, GETDATE()-350, GETDATE()-25),
    ('EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE', '11111111-1111-1111-1111-111111111111', NULL, 'HR', 'Phòng Nhân Sự', NULL, 'Quản lý nhân sự và tuyển dụng', 1, GETDATE()-340, GETDATE()-20),
    
    -- VNS Technology - Cấp 2 (Phòng ban con)
    ('FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF', '11111111-1111-1111-1111-111111111111', NULL, 'IT-DEV', 'Bộ phận Phát Triển', 'DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD', 'Phát triển phần mềm và ứng dụng', 1, GETDATE()-300, GETDATE()-15),
    ('11111111-1111-1111-1111-111111111112', '11111111-1111-1111-1111-111111111111', NULL, 'IT-SUPPORT', 'Bộ phận Hỗ Trợ', 'DDDDDDDD-DDDD-DDDD-DDDD-DDDDDDDDDDDD', 'Hỗ trợ kỹ thuật và bảo trì hệ thống', 1, GETDATE()-280, GETDATE()-10),
    ('22222222-2222-2222-2222-222222222223', '11111111-1111-1111-1111-111111111111', NULL, 'HR-REC', 'Bộ phận Tuyển Dụng', 'EEEEEEEE-EEEE-EEEE-EEEE-EEEEEEEEEEEE', 'Tuyển dụng và phỏng vấn nhân viên', 1, GETDATE()-260, GETDATE()-5),
    
    -- VNS Solutions - Cấp 1 (Phòng ban chính)
    ('33333333-3333-3333-3333-333333333334', '22222222-2222-2222-2222-222222222222', NULL, 'CEO', 'Ban Giám Đốc', NULL, 'Ban lãnh đạo cao nhất của công ty', 1, GETDATE()-300, GETDATE()-20),
    ('44444444-4444-4444-4444-444444444445', '22222222-2222-2222-2222-222222222222', NULL, 'SALES', 'Phòng Kinh Doanh', NULL, 'Bán hàng và phát triển khách hàng', 1, GETDATE()-250, GETDATE()-10)

PRINT '✅ Đã tạo 8 Departments (có cấu trúc phân cấp)'
