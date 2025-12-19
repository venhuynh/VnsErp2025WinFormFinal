-- =============================================
-- Script điều chỉnh bảng Device - Xóa các cột không cần thiết
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Xóa các cột không cần thiết và chỉ giữ lại các cột theo yêu cầu
-- =============================================

USE [VnsErp2025Final]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

BEGIN TRANSACTION
BEGIN TRY

    PRINT 'Bắt đầu migration bảng Device...'
    
    -- =============================================
    -- Bước 1: Xóa các Foreign Key Constraints liên quan đến các cột sẽ bị xóa
    -- =============================================
    
    -- Xóa FK_Device_Company
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Device_Company')
    BEGIN
        ALTER TABLE [dbo].[Device] DROP CONSTRAINT [FK_Device_Company]
        PRINT 'Đã xóa constraint FK_Device_Company'
    END
    
    -- Xóa FK_Device_CompanyBranch
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Device_CompanyBranch')
    BEGIN
        ALTER TABLE [dbo].[Device] DROP CONSTRAINT [FK_Device_CompanyBranch]
        PRINT 'Đã xóa constraint FK_Device_CompanyBranch'
    END
    
    -- Xóa FK_Device_Department
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Device_Department')
    BEGIN
        ALTER TABLE [dbo].[Device] DROP CONSTRAINT [FK_Device_Department]
        PRINT 'Đã xóa constraint FK_Device_Department'
    END
    
    -- Xóa FK_Device_Employee
    IF EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_Device_Employee')
    BEGIN
        ALTER TABLE [dbo].[Device] DROP CONSTRAINT [FK_Device_Employee]
        PRINT 'Đã xóa constraint FK_Device_Employee'
    END
    
    -- =============================================
    -- Bước 2: Xóa các Indexes liên quan đến các cột sẽ bị xóa
    -- =============================================
    
    -- Xóa IX_Device_AssignedEmployeeId
    IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Device_AssignedEmployeeId' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        DROP INDEX [IX_Device_AssignedEmployeeId] ON [dbo].[Device]
        PRINT 'Đã xóa index IX_Device_AssignedEmployeeId'
    END
    
    -- Xóa IX_Device_DepartmentId
    IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Device_DepartmentId' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        DROP INDEX [IX_Device_DepartmentId] ON [dbo].[Device]
        PRINT 'Đã xóa index IX_Device_DepartmentId'
    END
    
    -- =============================================
    -- Bước 3: Xóa các cột không cần thiết
    -- =============================================
    
    -- Xóa cột CompanyId
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'CompanyId' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [CompanyId]
        PRINT 'Đã xóa cột CompanyId'
    END
    
    -- Xóa cột BranchId
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'BranchId' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [BranchId]
        PRINT 'Đã xóa cột BranchId'
    END
    
    -- Xóa cột DepartmentId
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'DepartmentId' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [DepartmentId]
        PRINT 'Đã xóa cột DepartmentId'
    END
    
    -- Xóa cột AssignedEmployeeId
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'AssignedEmployeeId' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [AssignedEmployeeId]
        PRINT 'Đã xóa cột AssignedEmployeeId'
    END
    
    -- Xóa cột Location
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'Location' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [Location]
        PRINT 'Đã xóa cột Location'
    END
    
    -- Xóa cột PurchaseDate
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'PurchaseDate' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [PurchaseDate]
        PRINT 'Đã xóa cột PurchaseDate'
    END
    
    -- Xóa cột InstallationDate
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'InstallationDate' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [InstallationDate]
        PRINT 'Đã xóa cột InstallationDate'
    END
    
    -- Xóa cột LastMaintenanceDate
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'LastMaintenanceDate' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [LastMaintenanceDate]
        PRINT 'Đã xóa cột LastMaintenanceDate'
    END
    
    -- Xóa cột NextMaintenanceDate
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'NextMaintenanceDate' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [NextMaintenanceDate]
        PRINT 'Đã xóa cột NextMaintenanceDate'
    END
    
    -- Xóa cột Manufacturer
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'Manufacturer' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [Manufacturer]
        PRINT 'Đã xóa cột Manufacturer'
    END
    
    -- Xóa cột Model
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'Model' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [Model]
        PRINT 'Đã xóa cột Model'
    END
    
    -- Xóa cột Specifications
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'Specifications' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [Specifications]
        PRINT 'Đã xóa cột Specifications'
    END
    
    -- Xóa cột Configuration
    IF EXISTS (SELECT * FROM sys.columns WHERE name = 'Configuration' AND object_id = OBJECT_ID('dbo.Device'))
    BEGIN
        ALTER TABLE [dbo].[Device] DROP COLUMN [Configuration]
        PRINT 'Đã xóa cột Configuration'
    END
    
    -- =============================================
    -- Bước 4: Kiểm tra và đảm bảo các cột còn lại có đúng kiểu dữ liệu
    -- =============================================
    
    -- Kiểm tra và điều chỉnh các cột còn lại nếu cần
    -- (Các cột này đã có đúng kiểu dữ liệu từ script tạo ban đầu)
    
    PRINT 'Hoàn tất migration bảng Device.'
    PRINT 'Các cột còn lại:'
    PRINT '  - Id (uniqueidentifier NOT NULL)'
    PRINT '  - ProductVariantId (uniqueidentifier NOT NULL)'
    PRINT '  - StockInOutDetailId (uniqueidentifier NULL)'
    PRINT '  - WarrantyId (uniqueidentifier NULL)'
    PRINT '  - SerialNumber (nvarchar(100) NULL)'
    PRINT '  - MACAddress (nvarchar(50) NULL)'
    PRINT '  - IMEI (nvarchar(50) NULL)'
    PRINT '  - AssetTag (nvarchar(50) NULL)'
    PRINT '  - LicenseKey (nvarchar(255) NULL)'
    PRINT '  - HostName (nvarchar(100) NULL)'
    PRINT '  - IPAddress (nvarchar(50) NULL)'
    PRINT '  - Status (int NOT NULL)'
    PRINT '  - DeviceType (int NOT NULL)'
    PRINT '  - Notes (nvarchar(1000) NULL)'
    PRINT '  - IsActive (bit NOT NULL)'
    PRINT '  - CreatedDate (datetime NOT NULL)'
    PRINT '  - UpdatedDate (datetime NULL)'
    PRINT '  - CreatedBy (uniqueidentifier NULL)'
    PRINT '  - UpdatedBy (uniqueidentifier NULL)'
    
    COMMIT TRANSACTION
    PRINT 'Migration thành công!'
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION
        PRINT 'Migration thất bại! Đã rollback.'
    END
    
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
    DECLARE @ErrorState INT = ERROR_STATE()
    
    PRINT 'Lỗi: ' + @ErrorMessage
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
GO
