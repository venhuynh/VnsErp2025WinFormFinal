USE [VnsErp2025Final]
GO

/****** Object: Table [dbo].[Asset] Script Date: 12/04/2025 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- Drop table if exists (for development/testing)
IF OBJECT_ID('dbo.Asset', 'U') IS NOT NULL
    DROP TABLE [dbo].[Asset]
GO

CREATE TABLE [dbo].[Asset](
    -- Primary Key
    [Id] [uniqueidentifier] NOT NULL,
    
    -- Thông tin cơ bản
    [AssetCode] [nvarchar](50) NOT NULL,                    -- Mã tài sản (unique)
    [AssetName] [nvarchar](255) NOT NULL,                    -- Tên tài sản
    [AssetType] [int] NOT NULL,                               -- Loại tài sản (0: Cố định, 1: Lưu động, 2: Vô hình)
    [AssetCategory] [int] NOT NULL,                          -- Danh mục tài sản (0: Máy móc, 1: Thiết bị, 2: Phương tiện, 3: Nhà xưởng, 4: Khác)
    [Description] [nvarchar](1000) NULL,                     -- Mô tả
    
    -- Thông tin sản phẩm (nếu tài sản là sản phẩm trong hệ thống)
    [ProductVariantId] [uniqueidentifier] NULL,              -- Liên kết với ProductVariant (nếu có)
    
    -- Thông tin định vị
    [CompanyId] [uniqueidentifier] NOT NULL,                 -- Công ty
    [BranchId] [uniqueidentifier] NULL,                       -- Chi nhánh
    [DepartmentId] [uniqueidentifier] NULL,                  -- Phòng ban
    [AssignedEmployeeId] [uniqueidentifier] NULL,            -- Nhân viên phụ trách
    [Location] [nvarchar](500) NULL,                         -- Vị trí cụ thể
    
    -- Thông tin tài chính
    [PurchasePrice] [decimal](18, 2) NOT NULL,              -- Giá mua
    [PurchaseDate] [datetime] NULL,                          -- Ngày mua
    [SupplierName] [nvarchar](255) NULL,                     -- Nhà cung cấp
    [InvoiceNumber] [nvarchar](100) NULL,                    -- Số hóa đơn
    [InvoiceDate] [datetime] NULL,                          -- Ngày hóa đơn
    
    -- Thông tin khấu hao
    [DepreciationMethod] [int] NOT NULL DEFAULT 0,          -- Phương pháp khấu hao (0: Đường thẳng, 1: Số dư giảm dần, 2: Không khấu hao)
    [DepreciationRate] [decimal](5, 2) NULL,                 -- Tỷ lệ khấu hao (%/năm)
    [UsefulLife] [int] NULL,                                 -- Thời gian sử dụng (tháng)
    [DepreciationStartDate] [datetime] NULL,                -- Ngày bắt đầu khấu hao
    [AccumulatedDepreciation] [decimal](18, 2) NOT NULL DEFAULT 0, -- Khấu hao lũy kế
    [CurrentValue] [decimal](18, 2) NULL,                    -- Giá trị hiện tại (tự động tính: PurchasePrice - AccumulatedDepreciation)
    
    -- Thông tin trạng thái
    [Status] [int] NOT NULL DEFAULT 0,                      -- Trạng thái (0: Mới, 1: Đang sử dụng, 2: Bảo trì, 3: Ngừng sử dụng, 4: Thanh lý)
    [Condition] [int] NOT NULL DEFAULT 0,                   -- Tình trạng (0: Tốt, 1: Khá, 2: Trung bình, 3: Kém, 4: Hỏng)
    [IsActive] [bit] NOT NULL DEFAULT 1,                    -- Đang hoạt động
    [IsDeleted] [bit] NOT NULL DEFAULT 0,                   -- Đã xóa
    
    -- Thông tin bảo hành
    [WarrantyId] [uniqueidentifier] NULL,                    -- Liên kết với Warranty (nếu có)
    [WarrantyExpiryDate] [datetime] NULL,                    -- Ngày hết hạn bảo hành
    
    -- Thông tin bổ sung
    [SerialNumber] [nvarchar](100) NULL,                     -- Số seri
    [Manufacturer] [nvarchar](255) NULL,                    -- Nhà sản xuất
    [Model] [nvarchar](255) NULL,                            -- Model
    [Specifications] [nvarchar](2000) NULL,                  -- Thông số kỹ thuật
    [Notes] [nvarchar](1000) NULL,                           -- Ghi chú
    
    -- Audit Fields
    [CreateDate] [datetime] NOT NULL DEFAULT GETDATE(),     -- Ngày tạo
    [CreateBy] [uniqueidentifier] NULL,                     -- Người tạo
    [ModifiedDate] [datetime] NULL,                          -- Ngày sửa
    [ModifiedBy] [uniqueidentifier] NULL,                    -- Người sửa
    [DeletedDate] [datetime] NULL,                          -- Ngày xóa
    [DeletedBy] [uniqueidentifier] NULL,                    -- Người xóa
    
    -- Constraints
    CONSTRAINT [PK_Asset] PRIMARY KEY CLUSTERED ([Id] ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
              IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
              ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
    
    CONSTRAINT [UQ_Asset_AssetCode] UNIQUE NONCLUSTERED ([AssetCode] ASC, [IsDeleted] ASC)
        WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
              IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
              ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

-- Default Constraints
ALTER TABLE [dbo].[Asset] ADD DEFAULT (NEWID()) FOR [Id]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [AssetType]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [AssetCategory]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [DepreciationMethod]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [AccumulatedDepreciation]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [Status]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [Condition]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (1) FOR [IsActive]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (0) FOR [IsDeleted]
GO

ALTER TABLE [dbo].[Asset] ADD DEFAULT (GETDATE()) FOR [CreateDate]
GO

-- Foreign Key Constraints
ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_Company] 
    FOREIGN KEY([CompanyId]) REFERENCES [dbo].[Company] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Company]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_CompanyBranch] 
    FOREIGN KEY([BranchId]) REFERENCES [dbo].[CompanyBranch] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_CompanyBranch]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_Department] 
    FOREIGN KEY([DepartmentId]) REFERENCES [dbo].[Department] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Department]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_Employee] 
    FOREIGN KEY([AssignedEmployeeId]) REFERENCES [dbo].[Employee] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Employee]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_ProductVariant] 
    FOREIGN KEY([ProductVariantId]) REFERENCES [dbo].[ProductVariant] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_ProductVariant]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_Warranty] 
    FOREIGN KEY([WarrantyId]) REFERENCES [dbo].[Warranty] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_Warranty]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_CreateBy] 
    FOREIGN KEY([CreateBy]) REFERENCES [dbo].[ApplicationUser] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_CreateBy]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_ModifiedBy] 
    FOREIGN KEY([ModifiedBy]) REFERENCES [dbo].[ApplicationUser] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_ModifiedBy]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [FK_Asset_DeletedBy] 
    FOREIGN KEY([DeletedBy]) REFERENCES [dbo].[ApplicationUser] ([Id])
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [FK_Asset_DeletedBy]
GO

-- Check Constraints
ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_AssetType] 
    CHECK (([AssetType] >= 0 AND [AssetType] <= 2))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_AssetType]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_AssetCategory] 
    CHECK (([AssetCategory] >= 0 AND [AssetCategory] <= 4))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_AssetCategory]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_DepreciationMethod] 
    CHECK (([DepreciationMethod] >= 0 AND [DepreciationMethod] <= 2))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_DepreciationMethod]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_Status] 
    CHECK (([Status] >= 0 AND [Status] <= 4))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_Status]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_Condition] 
    CHECK (([Condition] >= 0 AND [Condition] <= 4))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_Condition]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_PurchasePrice] 
    CHECK (([PurchasePrice] >= 0))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_PurchasePrice]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_AccumulatedDepreciation] 
    CHECK (([AccumulatedDepreciation] >= 0))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_AccumulatedDepreciation]
GO

ALTER TABLE [dbo].[Asset] WITH CHECK 
    ADD CONSTRAINT [CHK_Asset_CurrentValue] 
    CHECK (([CurrentValue] IS NULL OR [CurrentValue] >= 0))
GO

ALTER TABLE [dbo].[Asset] CHECK CONSTRAINT [CHK_Asset_CurrentValue]
GO

-- Indexes
CREATE NONCLUSTERED INDEX [IX_Asset_AssetCode] 
    ON [dbo].[Asset] ([AssetCode] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_CompanyId] 
    ON [dbo].[Asset] ([CompanyId] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_BranchId] 
    ON [dbo].[Asset] ([BranchId] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_DepartmentId] 
    ON [dbo].[Asset] ([DepartmentId] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_AssignedEmployeeId] 
    ON [dbo].[Asset] ([AssignedEmployeeId] ASC)
    WHERE [IsDeleted] = 0
GO

CREATE NONCLUSTERED INDEX [IX_Asset_Status] 
    ON [dbo].[Asset] ([Status] ASC)
    WHERE [IsDeleted] = 0 AND [IsActive] = 1
GO

CREATE NONCLUSTERED INDEX [IX_Asset_ProductVariantId] 
    ON [dbo].[Asset] ([ProductVariantId] ASC)
    WHERE [ProductVariantId] IS NOT NULL AND [IsDeleted] = 0
GO

PRINT 'Table [dbo].[Asset] created successfully!'
GO

