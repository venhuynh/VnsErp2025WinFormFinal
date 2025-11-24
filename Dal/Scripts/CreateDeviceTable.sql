-- =============================================
-- Script tạo bảng quản lý thiết bị (Device/Equipment)
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Bảng Device để quản lý từng thiết bị/phần mềm/sản phẩm cụ thể
--        Liên kết với ProductVariant (loại thiết bị) và Warranty (bảo hành)
-- =============================================

USE [VnsErp2025Final]
GO

-- =============================================
-- Bảng Device (Thiết bị/Sản phẩm cụ thể)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Device]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[Device](
        [Id] [uniqueidentifier] NOT NULL,
        [ProductVariantId] [uniqueidentifier] NOT NULL, -- Loại thiết bị (Laptop Dell XPS 15)
        [StockInOutDetailId] [uniqueidentifier] NULL, -- Phiếu nhập kho (để biết thiết bị này được nhập từ đâu)
        [WarrantyId] [uniqueidentifier] NULL, -- Thông tin bảo hành (nếu có)
        
        -- Thông tin định danh duy nhất
        [SerialNumber] [nvarchar](100) NULL, -- Số serial
        [MACAddress] [nvarchar](50) NULL, -- Địa chỉ MAC (cho thiết bị mạng)
        [IMEI] [nvarchar](50) NULL, -- IMEI (cho thiết bị di động)
        [AssetTag] [nvarchar](50) NULL, -- Mã tài sản (mã nội bộ)
        [LicenseKey] [nvarchar](255) NULL, -- License key (cho phần mềm)
        [HostName] [nvarchar](100) NULL, -- Tên máy/hostname
        [IPAddress] [nvarchar](50) NULL, -- Địa chỉ IP (nếu có)
        
        -- Thông tin vị trí và người sử dụng
        [CompanyId] [uniqueidentifier] NULL, -- Thuộc công ty nào
        [BranchId] [uniqueidentifier] NULL, -- Thuộc chi nhánh nào
        [DepartmentId] [uniqueidentifier] NULL, -- Thuộc phòng ban nào
        [AssignedEmployeeId] [uniqueidentifier] NULL, -- Người được gán sử dụng
        [Location] [nvarchar](500) NULL, -- Vị trí cụ thể (phòng, tầng, v.v.)
        
        -- Thông tin trạng thái
        [Status] [int] NOT NULL DEFAULT(0), -- 0: Available, 1: InUse, 2: Maintenance, 3: Broken, 4: Disposed, 5: Reserved
        [DeviceType] [int] NOT NULL DEFAULT(0), -- 0: Hardware, 1: Software, 2: Network, 3: Other
        
        -- Thông tin ngày tháng
        [PurchaseDate] [datetime] NULL, -- Ngày mua
        [InstallationDate] [datetime] NULL, -- Ngày lắp đặt/cài đặt
        [LastMaintenanceDate] [datetime] NULL, -- Ngày bảo trì cuối
        [NextMaintenanceDate] [datetime] NULL, -- Ngày bảo trì tiếp theo
        
        -- Thông tin bổ sung
        [Manufacturer] [nvarchar](255) NULL, -- Nhà sản xuất (có thể khác với ProductVariant)
        [Model] [nvarchar](255) NULL, -- Model cụ thể
        [Specifications] [nvarchar](2000) NULL, -- Thông số kỹ thuật chi tiết
        [Configuration] [nvarchar](2000) NULL, -- Cấu hình (cho phần mềm)
        [Notes] [nvarchar](1000) NULL, -- Ghi chú
        
        -- Audit fields
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [CreatedDate] [datetime] NOT NULL,
        [UpdatedDate] [datetime] NULL,
        [CreatedBy] [uniqueidentifier] NULL,
        [UpdatedBy] [uniqueidentifier] NULL,
        
        CONSTRAINT [PK_Device] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_Device_ProductVariant] FOREIGN KEY([ProductVariantId])
            REFERENCES [dbo].[ProductVariant] ([Id]),
        CONSTRAINT [FK_Device_StockInOutDetail] FOREIGN KEY([StockInOutDetailId])
            REFERENCES [dbo].[StockInOutDetail] ([Id]),
        CONSTRAINT [FK_Device_Warranty] FOREIGN KEY([WarrantyId])
            REFERENCES [dbo].[Warranty] ([Id]),
        CONSTRAINT [FK_Device_Company] FOREIGN KEY([CompanyId])
            REFERENCES [dbo].[Company] ([Id]),
        CONSTRAINT [FK_Device_CompanyBranch] FOREIGN KEY([BranchId])
            REFERENCES [dbo].[CompanyBranch] ([Id]),
        CONSTRAINT [FK_Device_Department] FOREIGN KEY([DepartmentId])
            REFERENCES [dbo].[Department] ([Id]),
        CONSTRAINT [FK_Device_Employee] FOREIGN KEY([AssignedEmployeeId])
            REFERENCES [dbo].[Employee] ([Id])
    )
    
    -- Index cho SerialNumber (Unique nếu có)
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Device_SerialNumber] 
        ON [dbo].[Device] ([SerialNumber] ASC) WHERE [SerialNumber] IS NOT NULL
    
    -- Index cho AssetTag (Unique nếu có)
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Device_AssetTag] 
        ON [dbo].[Device] ([AssetTag] ASC) WHERE [AssetTag] IS NOT NULL
    
    -- Index cho IMEI (Unique nếu có)
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Device_IMEI] 
        ON [dbo].[Device] ([IMEI] ASC) WHERE [IMEI] IS NOT NULL
    
    -- Index cho MACAddress (Unique nếu có)
    CREATE UNIQUE NONCLUSTERED INDEX [IX_Device_MACAddress] 
        ON [dbo].[Device] ([MACAddress] ASC) WHERE [MACAddress] IS NOT NULL
    
    -- Index cho ProductVariantId
    CREATE NONCLUSTERED INDEX [IX_Device_ProductVariantId] 
        ON [dbo].[Device] ([ProductVariantId] ASC)
    
    -- Index cho StockInOutDetailId
    CREATE NONCLUSTERED INDEX [IX_Device_StockInOutDetailId] 
        ON [dbo].[Device] ([StockInOutDetailId] ASC) WHERE [StockInOutDetailId] IS NOT NULL
    
    -- Index cho WarrantyId
    CREATE NONCLUSTERED INDEX [IX_Device_WarrantyId] 
        ON [dbo].[Device] ([WarrantyId] ASC) WHERE [WarrantyId] IS NOT NULL
    
    -- Index cho Status và IsActive (để query nhanh)
    CREATE NONCLUSTERED INDEX [IX_Device_Status_IsActive] 
        ON [dbo].[Device] ([Status] ASC, [IsActive] ASC)
    
    -- Index cho AssignedEmployeeId
    CREATE NONCLUSTERED INDEX [IX_Device_AssignedEmployeeId] 
        ON [dbo].[Device] ([AssignedEmployeeId] ASC) WHERE [AssignedEmployeeId] IS NOT NULL
    
    -- Index cho DepartmentId
    CREATE NONCLUSTERED INDEX [IX_Device_DepartmentId] 
        ON [dbo].[Device] ([DepartmentId] ASC) WHERE [DepartmentId] IS NOT NULL
    
    PRINT 'Bảng Device đã được tạo thành công.'
END
ELSE
BEGIN
    PRINT 'Bảng Device đã tồn tại.'
END
GO

-- =============================================
-- Bảng DeviceHistory (Lịch sử thay đổi thiết bị)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceHistory]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DeviceHistory](
        [Id] [uniqueidentifier] NOT NULL,
        [DeviceId] [uniqueidentifier] NOT NULL,
        [ChangeType] [int] NOT NULL, -- 0: Created, 1: Updated, 2: StatusChanged, 3: Assigned, 4: Transferred, 5: Maintenance
        [ChangeDate] [datetime] NOT NULL,
        [ChangedBy] [uniqueidentifier] NULL,
        [OldValue] [nvarchar](500) NULL, -- Giá trị cũ
        [NewValue] [nvarchar](500) NULL, -- Giá trị mới
        [FieldName] [nvarchar](100) NULL, -- Tên trường thay đổi
        [Description] [nvarchar](1000) NULL, -- Mô tả thay đổi
        [Notes] [nvarchar](1000) NULL, -- Ghi chú bổ sung
        CONSTRAINT [PK_DeviceHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_DeviceHistory_Device] FOREIGN KEY([DeviceId])
            REFERENCES [dbo].[Device] ([Id]) ON DELETE CASCADE
    )
    
    -- Index cho DeviceId
    CREATE NONCLUSTERED INDEX [IX_DeviceHistory_DeviceId] 
        ON [dbo].[DeviceHistory] ([DeviceId] ASC, [ChangeDate] DESC)
    
    -- Index cho ChangeType
    CREATE NONCLUSTERED INDEX [IX_DeviceHistory_ChangeType] 
        ON [dbo].[DeviceHistory] ([ChangeType] ASC)
    
    PRINT 'Bảng DeviceHistory đã được tạo thành công.'
END
ELSE
BEGIN
    PRINT 'Bảng DeviceHistory đã tồn tại.'
END
GO

-- =============================================
-- Bảng DeviceTransfer (Lịch sử chuyển giao thiết bị)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceTransfer]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[DeviceTransfer](
        [Id] [uniqueidentifier] NOT NULL,
        [DeviceId] [uniqueidentifier] NOT NULL,
        [TransferDate] [datetime] NOT NULL,
        [FromBranchId] [uniqueidentifier] NULL,
        [FromDepartmentId] [uniqueidentifier] NULL,
        [FromEmployeeId] [uniqueidentifier] NULL,
        [FromLocation] [nvarchar](500) NULL,
        [ToBranchId] [uniqueidentifier] NULL,
        [ToDepartmentId] [uniqueidentifier] NULL,
        [ToEmployeeId] [uniqueidentifier] NULL,
        [ToLocation] [nvarchar](500) NULL,
        [TransferReason] [nvarchar](500) NULL,
        [TransferBy] [uniqueidentifier] NULL,
        [Status] [int] NOT NULL DEFAULT(0), -- 0: Pending, 1: Approved, 2: Completed, 3: Cancelled
        [Notes] [nvarchar](1000) NULL,
        [CreatedDate] [datetime] NOT NULL,
        [UpdatedDate] [datetime] NULL,
        [CreatedBy] [uniqueidentifier] NULL,
        [UpdatedBy] [uniqueidentifier] NULL,
        CONSTRAINT [PK_DeviceTransfer] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_DeviceTransfer_Device] FOREIGN KEY([DeviceId])
            REFERENCES [dbo].[Device] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_DeviceTransfer_FromBranch] FOREIGN KEY([FromBranchId])
            REFERENCES [dbo].[CompanyBranch] ([Id]),
        CONSTRAINT [FK_DeviceTransfer_ToBranch] FOREIGN KEY([ToBranchId])
            REFERENCES [dbo].[CompanyBranch] ([Id]),
        CONSTRAINT [FK_DeviceTransfer_FromDepartment] FOREIGN KEY([FromDepartmentId])
            REFERENCES [dbo].[Department] ([Id]),
        CONSTRAINT [FK_DeviceTransfer_ToDepartment] FOREIGN KEY([ToDepartmentId])
            REFERENCES [dbo].[Department] ([Id]),
        CONSTRAINT [FK_DeviceTransfer_FromEmployee] FOREIGN KEY([FromEmployeeId])
            REFERENCES [dbo].[Employee] ([Id]),
        CONSTRAINT [FK_DeviceTransfer_ToEmployee] FOREIGN KEY([ToEmployeeId])
            REFERENCES [dbo].[Employee] ([Id])
    )
    
    -- Index cho DeviceId
    CREATE NONCLUSTERED INDEX [IX_DeviceTransfer_DeviceId] 
        ON [dbo].[DeviceTransfer] ([DeviceId] ASC, [TransferDate] DESC)
    
    PRINT 'Bảng DeviceTransfer đã được tạo thành công.'
END
ELSE
BEGIN
    PRINT 'Bảng DeviceTransfer đã tồn tại.'
END
GO

PRINT 'Hoàn tất tạo các bảng quản lý thiết bị.'
GO

