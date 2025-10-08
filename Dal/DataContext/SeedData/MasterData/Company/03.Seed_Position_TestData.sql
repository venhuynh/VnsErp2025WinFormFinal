-- =============================================
-- Script: Tạo dữ liệu test cho bảng Positions
-- Description: Tạo dữ liệu mẫu cho các chức vụ
-- Author: VNS ERP Team
-- Created: 2025-01-07
-- =============================================

PRINT '👔 Bắt đầu tạo dữ liệu Positions...'

-- Tạo Positions
INSERT INTO Positions (Id, CompanyId, PositionCode, PositionName, Description, IsManagerLevel, IsActive, CreatedDate, UpdatedDate)
VALUES 
    -- VNS Technology positions
    ('66666666-6666-6666-6666-666666666666', '11111111-1111-1111-1111-111111111111', 'CEO', 'Tổng Giám Đốc', 'Lãnh đạo cao nhất của công ty', 1, 1, GETDATE()-365, GETDATE()-30),
    ('77777777-7777-7777-7777-777777777777', '11111111-1111-1111-1111-111111111111', 'CTO', 'Giám Đốc Công Nghệ', 'Quản lý công nghệ và phát triển sản phẩm', 1, 1, GETDATE()-350, GETDATE()-25),
    ('88888888-8888-8888-8888-888888888888', '11111111-1111-1111-1111-111111111111', 'DEV', 'Lập Trình Viên', 'Phát triển phần mềm và ứng dụng', 0, 1, GETDATE()-300, GETDATE()-20),
    ('99999999-9999-9999-9999-999999999999', '11111111-1111-1111-1111-111111111111', 'HR', 'Chuyên Viên Nhân Sự', 'Quản lý nhân sự và tuyển dụng', 0, 1, GETDATE()-280, GETDATE()-15),
    -- VNS Solutions positions
    ('AAAAAAAA-AAAA-AAAA-AAAA-AAAAAAAAAAAA', '22222222-2222-2222-2222-222222222222', 'CEO', 'Tổng Giám Đốc', 'Lãnh đạo cao nhất của công ty', 1, 1, GETDATE()-300, GETDATE()-20),
    ('BBBBBBBB-BBBB-BBBB-BBBB-BBBBBBBBBBBB', '22222222-2222-2222-2222-222222222222', 'SALES', 'Nhân Viên Kinh Doanh', 'Bán hàng và phát triển khách hàng', 0, 1, GETDATE()-250, GETDATE()-10)

PRINT '✅ Đã tạo 6 Positions'
