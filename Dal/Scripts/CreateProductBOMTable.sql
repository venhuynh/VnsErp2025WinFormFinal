-- =============================================
-- Script tạo bảng ProductBOM (Bill of Materials)
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Bảng định nghĩa cấu trúc sản phẩm lắp ráp
--        Một sản phẩm hoàn chỉnh gồm nhiều linh kiện
-- =============================================

USE [VnsErp2025Final]
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Bảng ProductBOM (Cấu trúc sản phẩm)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductBOM]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[ProductBOM](
        -- Primary Key
        [Id] [uniqueidentifier] NOT NULL,
        
        -- Thông tin sản phẩm hoàn chỉnh
        [ProductVariantId] [uniqueidentifier] NOT NULL,  -- Sản phẩm hoàn chỉnh (ví dụ: Bộ máy tính PC-001)
        
        -- Thông tin linh kiện
        [ComponentVariantId] [uniqueidentifier] NOT NULL, -- Linh kiện (ví dụ: CPU, RAM, Ổ cứng)
        
        -- Số lượng và đơn vị
        [Quantity] [decimal](18, 2) NOT NULL,            -- Số lượng linh kiện cần cho 1 sản phẩm
        [UnitId] [uniqueidentifier] NOT NULL,            -- Đơn vị tính
        
        -- Thông tin bổ sung
        [Notes] [nvarchar](1000) NULL,                  -- Ghi chú
        
        -- Trạng thái
        [IsActive] [bit] NOT NULL DEFAULT 1,            -- Còn sử dụng
        
        -- Audit Fields
        [CreatedDate] [datetime2] NOT NULL DEFAULT GETDATE(),
        [ModifiedDate] [datetime2] NOT NULL DEFAULT GETDATE(),
        
        -- Constraints
        CONSTRAINT [PK_ProductBOM] PRIMARY KEY CLUSTERED ([Id] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                  IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
                  ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
        
        -- Unique constraint: Một sản phẩm không thể có 2 dòng BOM giống nhau cho cùng 1 linh kiện
        CONSTRAINT [UQ_ProductBOM_Product_Component] UNIQUE NONCLUSTERED 
            ([ProductVariantId] ASC, [ComponentVariantId] ASC)
            WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, 
                  IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, 
                  ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY]
    
    -- Default Constraints
    ALTER TABLE [dbo].[ProductBOM] ADD DEFAULT (NEWID()) FOR [Id]
    ALTER TABLE [dbo].[ProductBOM] ADD DEFAULT (1) FOR [IsActive]
    ALTER TABLE [dbo].[ProductBOM] ADD DEFAULT (GETDATE()) FOR [CreatedDate]
    ALTER TABLE [dbo].[ProductBOM] ADD DEFAULT (GETDATE()) FOR [ModifiedDate]
    
    PRINT 'Bảng ProductBOM đã được tạo thành công!'
END
ELSE
BEGIN
    PRINT 'Bảng ProductBOM đã tồn tại!'
END
GO

-- Foreign Key Constraints (chạy sau khi bảng đã được tạo)
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProductBOM_ProductVariant')
BEGIN
    ALTER TABLE [dbo].[ProductBOM] WITH CHECK 
        ADD CONSTRAINT [FK_ProductBOM_ProductVariant] 
        FOREIGN KEY([ProductVariantId]) REFERENCES [dbo].[ProductVariant] ([Id])
    ALTER TABLE [dbo].[ProductBOM] CHECK CONSTRAINT [FK_ProductBOM_ProductVariant]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProductBOM_ComponentVariant')
BEGIN
    ALTER TABLE [dbo].[ProductBOM] WITH CHECK 
        ADD CONSTRAINT [FK_ProductBOM_ComponentVariant] 
        FOREIGN KEY([ComponentVariantId]) REFERENCES [dbo].[ProductVariant] ([Id])
    ALTER TABLE [dbo].[ProductBOM] CHECK CONSTRAINT [FK_ProductBOM_ComponentVariant]
END
GO

IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE name = 'FK_ProductBOM_UnitOfMeasure')
BEGIN
    ALTER TABLE [dbo].[ProductBOM] WITH CHECK 
        ADD CONSTRAINT [FK_ProductBOM_UnitOfMeasure] 
        FOREIGN KEY([UnitId]) REFERENCES [dbo].[UnitOfMeasure] ([Id])
    ALTER TABLE [dbo].[ProductBOM] CHECK CONSTRAINT [FK_ProductBOM_UnitOfMeasure]
END
GO

-- Indexes (chạy sau khi bảng đã được tạo)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductBOM_ProductVariantId' AND object_id = OBJECT_ID('dbo.ProductBOM'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductBOM_ProductVariantId] 
        ON [dbo].[ProductBOM]([ProductVariantId] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductBOM_ComponentVariantId' AND object_id = OBJECT_ID('dbo.ProductBOM'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductBOM_ComponentVariantId] 
        ON [dbo].[ProductBOM]([ComponentVariantId] ASC)
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_ProductBOM_IsActive' AND object_id = OBJECT_ID('dbo.ProductBOM'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_ProductBOM_IsActive] 
        ON [dbo].[ProductBOM]([IsActive] ASC)
END
GO

