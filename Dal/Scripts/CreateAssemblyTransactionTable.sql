-- =============================================
-- Script tạo bảng AssemblyTransaction (Lịch sử lắp ráp)
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Bảng lưu lịch sử các giao dịch lắp ráp sản phẩm
--        Liên kết phiếu xuất linh kiện và phiếu nhập sản phẩm
-- =============================================

USE [VnsErp2025Final]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Bảng AssemblyTransaction (Lịch sử lắp ráp)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AssemblyTransaction]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[AssemblyTransaction](
        -- Primary Key
        [Id] [uniqueidentifier] NOT NULL,
        
        -- Thông tin lắp ráp
        [AssemblyDate] [datetime] NOT NULL,            -- Ngày lắp ráp
        [ProductVariantId] [uniqueidentifier] NOT NULL, -- Sản phẩm lắp ráp
        [Quantity] [decimal](18, 2) NOT NULL,          -- Số lượng lắp ráp
        
        -- Liên kết với phiếu nhập/xuất
        [StockOutMasterId] [uniqueidentifier] NOT NULL,  -- Phiếu xuất linh kiện
        [StockInMasterId] [uniqueidentifier] NOT NULL,  -- Phiếu nhập sản phẩm hoàn chỉnh
        
        -- Thông tin kho
        [WarehouseId] [uniqueidentifier] NOT NULL,      -- Kho thực hiện lắp ráp
        
        -- Thông tin tài chính
        [TotalCost] [decimal](18, 2) NOT NULL,         -- Tổng giá thành (từ linh kiện)
        [UnitCost] [decimal](18, 2) NOT NULL,           -- Giá thành đơn vị
        
        -- Ghi chú
        [Notes] [nvarchar](1000) NULL,
        
        -- Audit Fields
        [CreatedBy] [uniqueidentifier] NULL,
        [CreatedDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] [datetime] NULL,
        [ModifiedBy] [uniqueidentifier] NULL,
        
        -- Constraints
        CONSTRAINT [PK_AssemblyTransaction] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                  IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
                  ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    
    -- Default Constraints
    ALTER TABLE [dbo].[AssemblyTransaction] ADD DEFAULT (NEWID()) FOR [Id]
    ALTER TABLE [dbo].[AssemblyTransaction] ADD DEFAULT (GETDATE()) FOR [CreatedDate]
    ALTER TABLE [dbo].[AssemblyTransaction] ADD DEFAULT (0) FOR [TotalCost]
    ALTER TABLE [dbo].[AssemblyTransaction] ADD DEFAULT (0) FOR [UnitCost]
    
    PRINT 'Bảng AssemblyTransaction đã được tạo thành công!'
END
ELSE
BEGIN
    PRINT 'Bảng AssemblyTransaction đã tồn tại!'
END
GO

-- Foreign Key Constraints (chạy sau khi bảng đã được tạo)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AssemblyTransaction_ProductVariant')
BEGIN
    ALTER TABLE [dbo].[AssemblyTransaction] WITH CHECK 
        ADD CONSTRAINT [FK_AssemblyTransaction_ProductVariant] 
        FOREIGN KEY([ProductVariantId]) REFERENCES [dbo].[ProductVariant] ([Id])
    ALTER TABLE [dbo].[AssemblyTransaction] CHECK CONSTRAINT [FK_AssemblyTransaction_ProductVariant]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AssemblyTransaction_StockOutMaster')
BEGIN
    ALTER TABLE [dbo].[AssemblyTransaction] WITH CHECK 
        ADD CONSTRAINT [FK_AssemblyTransaction_StockOutMaster] 
        FOREIGN KEY([StockOutMasterId]) REFERENCES [dbo].[StockInOutMaster] ([Id])
    ALTER TABLE [dbo].[AssemblyTransaction] CHECK CONSTRAINT [FK_AssemblyTransaction_StockOutMaster]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AssemblyTransaction_StockInMaster')
BEGIN
    ALTER TABLE [dbo].[AssemblyTransaction] WITH CHECK 
        ADD CONSTRAINT [FK_AssemblyTransaction_StockInMaster] 
        FOREIGN KEY([StockInMasterId]) REFERENCES [dbo].[StockInOutMaster] ([Id])
    ALTER TABLE [dbo].[AssemblyTransaction] CHECK CONSTRAINT [FK_AssemblyTransaction_StockInMaster]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_AssemblyTransaction_Warehouse')
BEGIN
    ALTER TABLE [dbo].[AssemblyTransaction] WITH CHECK 
        ADD CONSTRAINT [FK_AssemblyTransaction_Warehouse] 
        FOREIGN KEY([WarehouseId]) REFERENCES [dbo].[CompanyBranch] ([Id])
    ALTER TABLE [dbo].[AssemblyTransaction] CHECK CONSTRAINT [FK_AssemblyTransaction_Warehouse]
END
GO

-- Indexes (chạy sau khi bảng đã được tạo)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AssemblyTransaction_ProductVariantId' AND object_id = OBJECT_ID('dbo.AssemblyTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AssemblyTransaction_ProductVariantId] 
        ON [dbo].[AssemblyTransaction]([ProductVariantId] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AssemblyTransaction_AssemblyDate' AND object_id = OBJECT_ID('dbo.AssemblyTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AssemblyTransaction_AssemblyDate] 
        ON [dbo].[AssemblyTransaction]([AssemblyDate] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AssemblyTransaction_StockOutMasterId' AND object_id = OBJECT_ID('dbo.AssemblyTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AssemblyTransaction_StockOutMasterId] 
        ON [dbo].[AssemblyTransaction]([StockOutMasterId] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_AssemblyTransaction_StockInMasterId' AND object_id = OBJECT_ID('dbo.AssemblyTransaction'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_AssemblyTransaction_StockInMasterId] 
        ON [dbo].[AssemblyTransaction]([StockInMasterId] ASC)
END
GO

