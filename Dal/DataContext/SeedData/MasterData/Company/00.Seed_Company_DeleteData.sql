-- =============================================
-- Script: Xóa tất cả dữ liệu Company entities
-- Description: Xóa dữ liệu theo thứ tự ngược lại để tránh foreign key constraint
-- Author: VNS ERP Team
-- Created: 2025-01-07
-- =============================================

PRINT '🗑️ Bắt đầu xóa dữ liệu Company entities...'

-- Xóa Employees trước (có foreign key đến các bảng khác)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Employees')
BEGIN
    DELETE FROM Employees
    PRINT '✅ Đã xóa dữ liệu từ bảng Employees'
END

-- Xóa Departments (có foreign key đến Company và self-reference)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Departments')
BEGIN
    DELETE FROM Departments
    PRINT '✅ Đã xóa dữ liệu từ bảng Departments'
END

-- Xóa Positions (có foreign key đến Company)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Positions')
BEGIN
    DELETE FROM Positions
    PRINT '✅ Đã xóa dữ liệu từ bảng Positions'
END

-- Xóa CompanyBranches (có foreign key đến Company)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'CompanyBranches')
BEGIN
    DELETE FROM CompanyBranches
    PRINT '✅ Đã xóa dữ liệu từ bảng CompanyBranches'
END

-- Xóa Companies cuối cùng (root entity)
IF EXISTS (SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'Companies')
BEGIN
    DELETE FROM Companies
    PRINT '✅ Đã xóa dữ liệu từ bảng Companies'
END

PRINT '🎉 Hoàn thành xóa dữ liệu Company entities!'
