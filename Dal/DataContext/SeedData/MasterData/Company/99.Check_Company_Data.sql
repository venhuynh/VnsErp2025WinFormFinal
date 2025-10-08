-- =============================================
-- Script: Kiểm tra dữ liệu Company entities
-- Description: Kiểm tra và hiển thị thống kê dữ liệu đã tạo
-- Author: VNS ERP Team
-- Created: 2025-01-07
-- =============================================

PRINT '📊 Kiểm tra dữ liệu Company entities...'

-- Kiểm tra Companies
SELECT 'Companies' AS TableName, COUNT(*) AS RecordCount FROM Companies
UNION ALL
SELECT 'CompanyBranches' AS TableName, COUNT(*) AS RecordCount FROM CompanyBranches
UNION ALL
SELECT 'Positions' AS TableName, COUNT(*) AS RecordCount FROM Positions
UNION ALL
SELECT 'Departments' AS TableName, COUNT(*) AS RecordCount FROM Departments
UNION ALL
SELECT 'Employees' AS TableName, COUNT(*) AS RecordCount FROM Employees

PRINT '📋 Chi tiết dữ liệu:'

-- Chi tiết Companies
PRINT '🏢 Companies:'
SELECT CompanyCode, CompanyName, TaxCode, Phone, Email, IsActive FROM Companies

-- Chi tiết CompanyBranches
PRINT '🏢 CompanyBranches:'
SELECT cb.BranchCode, cb.BranchName, c.CompanyName, cb.ManagerName, cb.IsActive 
FROM CompanyBranches cb
INNER JOIN Companies c ON cb.CompanyId = c.Id

-- Chi tiết Positions
PRINT '👔 Positions:'
SELECT p.PositionCode, p.PositionName, c.CompanyName, p.IsManagerLevel, p.IsActive
FROM Positions p
INNER JOIN Companies c ON p.CompanyId = c.Id

-- Chi tiết Departments (với hierarchy)
PRINT '🏢 Departments (Hierarchy):'
SELECT 
    d.DepartmentCode,
    d.DepartmentName,
    c.CompanyName,
    CASE 
        WHEN d.ParentId IS NULL THEN 'Cấp 1 (Root)'
        ELSE 'Cấp 2 (Sub)'
    END AS HierarchyLevel,
    pd.DepartmentName AS ParentDepartment,
    d.IsActive
FROM Departments d
INNER JOIN Companies c ON d.CompanyId = c.Id
LEFT JOIN Departments pd ON d.ParentId = pd.Id

-- Chi tiết Employees
PRINT '👥 Employees:'
SELECT 
    e.EmployeeCode,
    e.FullName,
    c.CompanyName,
    d.DepartmentName,
    p.PositionName,
    e.HireDate,
    e.IsActive
FROM Employees e
INNER JOIN Companies c ON e.CompanyId = c.Id
LEFT JOIN Departments d ON e.DepartmentId = d.Id
LEFT JOIN Positions p ON e.PositionId = p.Id

PRINT '✅ Hoàn thành kiểm tra dữ liệu!'
