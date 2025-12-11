-- Script để cải tiến bảng ProductServiceCategory
-- Dựa trên cấu trúc BusinessPartnerCategory để đảm bảo tính nhất quán
-- Date: 11/12/2025
-- Mục đích: Thêm các cột audit, cải tiến ràng buộc, tối ưu performance

USE [VnsErp2025Final]
GO

-- ============================================================================
-- PHẦN 1: KIỂM TRA VÀ LƯU DỮ LIỆU BACKUP
-- ============================================================================

-- Backup dữ liệu hiện có
SELECT * INTO [ProductServiceCategory_Backup_20251211]
FROM [dbo].[ProductServiceCategory]
PRINT 'Backup dữ liệu ProductServiceCategory thành công'
GO

-- ============================================================================
-- PHẦN 2: KIỂM TRA VÀ XÓA CONSTRAINTS CŨ
-- ============================================================================

-- Xóa Foreign Key constraints
IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProductService_ProductServiceCategory')
BEGIN
    ALTER TABLE [dbo].[ProductService] DROP CONSTRAINT [FK_ProductService_ProductServiceCategory]
    PRINT 'Đã xóa FK_ProductService_ProductServiceCategory'
END
GO

-- Xóa Check constraints
IF EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_ProductServiceCategory_CategoryCode_Format')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory] DROP CONSTRAINT [CK_ProductServiceCategory_CategoryCode_Format]
    PRINT 'Đã xóa CK_ProductServiceCategory_CategoryCode_Format'
END
GO

-- Xóa Unique constraints
IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'UK_ProductServiceCategory_CategoryCode')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory] DROP CONSTRAINT [UK_ProductServiceCategory_CategoryCode]
    PRINT 'Đã xóa UK_ProductServiceCategory_CategoryCode'
END
GO

-- Xóa Unique constraints trên CategoryName
IF EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'UK_ProductServiceCategory_CategoryName')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory] DROP CONSTRAINT [UK_ProductServiceCategory_CategoryName]
    PRINT 'Đã xóa UK_ProductServiceCategory_CategoryName'
END
GO

-- ============================================================================
-- PHẦN 3: KIỂM TRA VÀ XÓA INDEXES CŨ
-- ============================================================================

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductServiceCategory_CategoryCode' AND object_id = OBJECT_ID('dbo.ProductServiceCategory'))
BEGIN
    DROP INDEX [IX_ProductServiceCategory_CategoryCode] ON [dbo].[ProductServiceCategory]
    PRINT 'Đã xóa IX_ProductServiceCategory_CategoryCode'
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductServiceCategory_CategoryName' AND object_id = OBJECT_ID('dbo.ProductServiceCategory'))
BEGIN
    DROP INDEX [IX_ProductServiceCategory_CategoryName] ON [dbo].[ProductServiceCategory]
    PRINT 'Đã xóa IX_ProductServiceCategory_CategoryName'
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductServiceCategory_ParentId' AND object_id = OBJECT_ID('dbo.ProductServiceCategory'))
BEGIN
    DROP INDEX [IX_ProductServiceCategory_ParentId] ON [dbo].[ProductServiceCategory]
    PRINT 'Đã xóa IX_ProductServiceCategory_ParentId'
END
GO

IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductServiceCategory_IsActive' AND object_id = OBJECT_ID('dbo.ProductServiceCategory'))
BEGIN
    DROP INDEX [IX_ProductServiceCategory_IsActive] ON [dbo].[ProductServiceCategory]
    PRINT 'Đã xóa IX_ProductServiceCategory_IsActive'
END
GO

-- ============================================================================
-- PHẦN 4: KIỂM TRA VÀ THÊM CỘT MỚI (NẾU CHƯA CÓ)
-- ============================================================================

-- Kiểm tra và thêm cột IsActive
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductServiceCategory' AND COLUMN_NAME = 'IsActive')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD [IsActive] [bit] NOT NULL DEFAULT 1
    PRINT 'Đã thêm cột IsActive'
END
ELSE
BEGIN
    PRINT 'Cột IsActive đã tồn tại'
END
GO

-- Kiểm tra và thêm cột SortOrder
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductServiceCategory' AND COLUMN_NAME = 'SortOrder')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD [SortOrder] [int] NULL
    PRINT 'Đã thêm cột SortOrder'
END
ELSE
BEGIN
    PRINT 'Cột SortOrder đã tồn tại'
END
GO

-- Kiểm tra và thêm cột CreatedDate
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductServiceCategory' AND COLUMN_NAME = 'CreatedDate')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD [CreatedDate] [datetime] NOT NULL DEFAULT GETUTCDATE()
    PRINT 'Đã thêm cột CreatedDate'
END
ELSE
BEGIN
    PRINT 'Cột CreatedDate đã tồn tại'
END
GO

-- Kiểm tra và thêm cột CreatedBy
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductServiceCategory' AND COLUMN_NAME = 'CreatedBy')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD [CreatedBy] [uniqueidentifier] NULL
    PRINT 'Đã thêm cột CreatedBy'
END
ELSE
BEGIN
    PRINT 'Cột CreatedBy đã tồn tại'
END
GO

-- Kiểm tra và thêm cột ModifiedDate
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductServiceCategory' AND COLUMN_NAME = 'ModifiedDate')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD [ModifiedDate] [datetime] NULL
    PRINT 'Đã thêm cột ModifiedDate'
END
ELSE
BEGIN
    PRINT 'Cột ModifiedDate đã tồn tại'
END
GO

-- Kiểm tra và thêm cột ModifiedBy
IF NOT EXISTS (SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = 'ProductServiceCategory' AND COLUMN_NAME = 'ModifiedBy')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD [ModifiedBy] [uniqueidentifier] NULL
    PRINT 'Đã thêm cột ModifiedBy'
END
ELSE
BEGIN
    PRINT 'Cột ModifiedBy đã tồn tại'
END
GO

-- ============================================================================
-- PHẦN 5: CẬP NHẬT DỮ LIỆU
-- ============================================================================

-- Cập nhật SortOrder nếu NULL
UPDATE [dbo].[ProductServiceCategory]
SET [SortOrder] = ROW_NUMBER() OVER (ORDER BY [CategoryName] ASC)
WHERE [SortOrder] IS NULL
PRINT 'Đã cập nhật SortOrder'
GO

-- Cập nhật CreatedDate nếu NULL hoặc mặc định
UPDATE [dbo].[ProductServiceCategory]
SET [CreatedDate] = GETUTCDATE()
WHERE [CreatedDate] IS NULL OR [CreatedDate] = '1900-01-01'
PRINT 'Đã cập nhật CreatedDate'
GO

-- ============================================================================
-- PHẦN 6: TÓM TẮT CẤU TRÚC BẢNG HIỆN TẠI
-- ============================================================================

PRINT '--- HIỆN TẠI BẢNG PRODUCTSERVICECATEGORY CÓ CÁC CỘT: ---'
SELECT 
    ORDINAL_POSITION as 'Thứ tự',
    COLUMN_NAME as 'Tên cột',
    DATA_TYPE as 'Kiểu dữ liệu',
    CASE WHEN IS_NULLABLE = 'NO' THEN 'NOT NULL' ELSE 'NULL' END as 'Null',
    COLUMN_DEFAULT as 'Mặc định'
FROM INFORMATION_SCHEMA.COLUMNS
WHERE TABLE_NAME = 'ProductServiceCategory'
ORDER BY ORDINAL_POSITION
GO

-- ============================================================================
-- PHẦN 7: TẠO FOREIGN KEY CONSTRAINTS MỚI
-- ============================================================================

-- Tạo Foreign Key cho CreatedBy
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProductServiceCategory_CreatedBy')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD CONSTRAINT [FK_ProductServiceCategory_CreatedBy]
    FOREIGN KEY ([CreatedBy]) REFERENCES [dbo].[ApplicationUser] ([Id])
    PRINT 'Đã tạo FK_ProductServiceCategory_CreatedBy'
END
ELSE
BEGIN
    PRINT 'FK_ProductServiceCategory_CreatedBy đã tồn tại'
END
GO

-- Tạo Foreign Key cho ModifiedBy
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProductServiceCategory_ModifiedBy')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD CONSTRAINT [FK_ProductServiceCategory_ModifiedBy]
    FOREIGN KEY ([ModifiedBy]) REFERENCES [dbo].[ApplicationUser] ([Id])
    PRINT 'Đã tạo FK_ProductServiceCategory_ModifiedBy'
END
ELSE
BEGIN
    PRINT 'FK_ProductServiceCategory_ModifiedBy đã tồn tại'
END
GO

-- ============================================================================
-- PHẦN 8: TẠO UNIQUE CONSTRAINTS
-- ============================================================================

-- Unique constraint cho CategoryCode
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'UK_ProductServiceCategory_CategoryCode')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD CONSTRAINT [UK_ProductServiceCategory_CategoryCode] 
    UNIQUE ([CategoryCode])
    PRINT 'Đã tạo UK_ProductServiceCategory_CategoryCode'
END
ELSE
BEGIN
    PRINT 'UK_ProductServiceCategory_CategoryCode đã tồn tại'
END
GO

-- Unique constraint cho CategoryName trong cùng ParentId (phân cấp)
IF NOT EXISTS (SELECT * FROM sys.key_constraints WHERE name = 'UK_ProductServiceCategory_Name_Parent')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD CONSTRAINT [UK_ProductServiceCategory_Name_Parent] 
    UNIQUE ([CategoryName], [ParentId])
    PRINT 'Đã tạo UK_ProductServiceCategory_Name_Parent'
END
ELSE
BEGIN
    PRINT 'UK_ProductServiceCategory_Name_Parent đã tồn tại'
END
GO

-- ============================================================================
-- PHẦN 9: TẠO CHECK CONSTRAINTS
-- ============================================================================

-- Check constraint cho CategoryCode (chỉ chữ, số, và gạch ngang)
IF NOT EXISTS (SELECT * FROM sys.check_constraints WHERE name = 'CK_ProductServiceCategory_CategoryCode_Format')
BEGIN
    ALTER TABLE [dbo].[ProductServiceCategory]
    ADD CONSTRAINT [CK_ProductServiceCategory_CategoryCode_Format]
    CHECK ([CategoryCode] IS NULL OR [CategoryCode] LIKE '^[A-Z0-9-]*$')
    PRINT 'Đã tạo CK_ProductServiceCategory_CategoryCode_Format'
END
ELSE
BEGIN
    PRINT 'CK_ProductServiceCategory_CategoryCode_Format đã tồn tại'
END
GO

-- ============================================================================
-- PHẦN 10: TẠO INDEXES ĐỂ CẢI THIỆN PERFORMANCE
-- ============================================================================

-- Index cho CategoryCode (giúp tìm kiếm và unique constraint)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductServiceCategory_CategoryCode' AND object_id = OBJECT_ID('dbo.ProductServiceCategory'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductServiceCategory_CategoryCode] 
    ON [dbo].[ProductServiceCategory] ([CategoryCode])
    INCLUDE ([CategoryName], [IsActive])
    PRINT 'Đã tạo IX_ProductServiceCategory_CategoryCode'
END
ELSE
BEGIN
    PRINT 'IX_ProductServiceCategory_CategoryCode đã tồn tại'
END
GO

-- Index cho ParentId (giúp truy vấn phân cấp)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductServiceCategory_ParentId' AND object_id = OBJECT_ID('dbo.ProductServiceCategory'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductServiceCategory_ParentId] 
    ON [dbo].[ProductServiceCategory] ([ParentId])
    INCLUDE ([CategoryName], [IsActive], [SortOrder])
    PRINT 'Đã tạo IX_ProductServiceCategory_ParentId'
END
ELSE
BEGIN
    PRINT 'IX_ProductServiceCategory_ParentId đã tồn tại'
END
GO

-- Index cho IsActive (giúp truy vấn danh mục active)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductServiceCategory_IsActive' AND object_id = OBJECT_ID('dbo.ProductServiceCategory'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductServiceCategory_IsActive] 
    ON [dbo].[ProductServiceCategory] ([IsActive])
    INCLUDE ([CategoryName], [ParentId], [SortOrder])
    PRINT 'Đã tạo IX_ProductServiceCategory_IsActive'
END
ELSE
BEGIN
    PRINT 'IX_ProductServiceCategory_IsActive đã tồn tại'
END
GO

-- Index combo cho tìm kiếm nâng cao
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductServiceCategory_ParentIsActive_SortOrder' AND object_id = OBJECT_ID('dbo.ProductServiceCategory'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductServiceCategory_ParentIsActive_SortOrder] 
    ON [dbo].[ProductServiceCategory] ([ParentId], [IsActive], [SortOrder])
    INCLUDE ([CategoryName], [CategoryCode], [Description])
    PRINT 'Đã tạo IX_ProductServiceCategory_ParentIsActive_SortOrder'
END
ELSE
BEGIN
    PRINT 'IX_ProductServiceCategory_ParentIsActive_SortOrder đã tồn tại'
END
GO

-- ============================================================================
-- PHẦN 11: TẠO FOREIGN KEY CHO PRODUCTSERVICE
-- ============================================================================

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProductService_ProductServiceCategory')
BEGIN
    ALTER TABLE [dbo].[ProductService]
    ADD CONSTRAINT [FK_ProductService_ProductServiceCategory]
    FOREIGN KEY ([CategoryId]) REFERENCES [dbo].[ProductServiceCategory] ([Id])
    PRINT 'Đã tạo FK_ProductService_ProductServiceCategory'
END
ELSE
BEGIN
    PRINT 'FK_ProductService_ProductServiceCategory đã tồn tại'
END
GO

-- ============================================================================
-- PHẦN 12: THỐNG KÊ VÀ KIỂM TRA
-- ============================================================================

PRINT '=== THỐNG KÊ BẢNG PRODUCTSERVICECATEGORY ==='
SELECT 
    'Tổng số danh mục' as 'Chỉ số',
    COUNT(*) as 'Giá trị'
FROM [dbo].[ProductServiceCategory]
UNION ALL
SELECT 
    'Danh mục Active',
    COUNT(*)
FROM [dbo].[ProductServiceCategory]
WHERE [IsActive] = 1
UNION ALL
SELECT 
    'Danh mục cấp 1 (ParentId IS NULL)',
    COUNT(*)
FROM [dbo].[ProductServiceCategory]
WHERE [ParentId] IS NULL
UNION ALL
SELECT 
    'Danh mục có CategoryCode',
    COUNT(*)
FROM [dbo].[ProductServiceCategory]
WHERE [CategoryCode] IS NOT NULL
GO

PRINT '=== CÁC CONSTRAINT TRÊN BẢNG PRODUCTSERVICECATEGORY ==='
SELECT 
    name as 'Tên constraint',
    type_desc as 'Loại'
FROM sys.objects
WHERE parent_object_id = OBJECT_ID('ProductServiceCategory')
    AND type IN ('UQ', 'F', 'C', 'PK')
ORDER BY type_desc
GO

PRINT '=== CÁC INDEX TRÊN BẢNG PRODUCTSERVICECATEGORY ==='
SELECT 
    name as 'Tên index',
    type_desc as 'Loại index'
FROM sys.indexes
WHERE object_id = OBJECT_ID('ProductServiceCategory')
    AND name IS NOT NULL
ORDER BY name
GO

PRINT '=== SCRIPT CẢI TIẾN PRODUCTSERVICECATEGORY HOÀN THÀNH ==='
PRINT 'Đã thêm: IsActive, SortOrder, CreatedDate, CreatedBy, ModifiedDate, ModifiedBy'
PRINT 'Đã tạo: Constraints, Foreign Keys, Indexes'
PRINT 'Dữ liệu Backup: ProductServiceCategory_Backup_20251211'
GO
