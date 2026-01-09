-- =============================================
-- Script tạo các bảng Kiểm kho (Stocktaking)
-- Dựa trên cấu trúc hiện tại của StockInOutMaster, StockInOutDetail, InventoryBalance
-- =============================================

USE [VnsErp2025] -- Thay đổi tên database nếu cần
GO

-- =============================================
-- 1. Bảng chính kiểm kho: StocktakingMaster
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StocktakingMaster]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StocktakingMaster] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [StocktakingDate] [datetime] NOT NULL,                    -- Ngày kiểm kho
        [VoucherNumber] [nvarchar](50) NOT NULL,                  -- Số phiếu kiểm kho
        [StocktakingType] [int] NOT NULL,                         -- Loại kiểm kho (0: Định kỳ, 1: Đột xuất, 2: Toàn bộ, 3: Theo sản phẩm)
        [StocktakingStatus] [int] NOT NULL DEFAULT(0),             -- Trạng thái (0: Nháp, 1: Đang kiểm, 2: Hoàn thành, 3: Đã duyệt, 4: Đã hủy)
        [WarehouseId] [uniqueidentifier] NOT NULL,                -- Kho kiểm
        [CompanyBranchId] [uniqueidentifier] NULL,                 -- Chi nhánh
        [StartDate] [datetime] NULL,                              -- Ngày bắt đầu kiểm
        [EndDate] [datetime] NULL,                                -- Ngày kết thúc kiểm
        [CountedBy] [uniqueidentifier] NULL,                       -- Người kiểm
        [CountedDate] [datetime] NULL,                            -- Ngày kiểm
        [ReviewedBy] [uniqueidentifier] NULL,                     -- Người rà soát
        [ReviewedDate] [datetime] NULL,                           -- Ngày rà soát
        [ApprovedBy] [uniqueidentifier] NULL,                     -- Người duyệt
        [ApprovedDate] [datetime] NULL,                           -- Ngày duyệt
        [Notes] [nvarchar](1000) NULL,                            -- Ghi chú
        [Reason] [nvarchar](500) NULL,                            -- Lý do kiểm kho
        [IsLocked] [bit] NOT NULL DEFAULT(0),                     -- Đã khóa
        [LockedDate] [datetime] NULL,
        [LockedBy] [uniqueidentifier] NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [IsDeleted] [bit] NOT NULL DEFAULT(0),
        [CreatedBy] [uniqueidentifier] NULL,
        [CreatedDate] [datetime] NULL DEFAULT GETDATE(),
        [UpdatedBy] [uniqueidentifier] NULL,
        [UpdatedDate] [datetime] NULL,
        [DeletedBy] [uniqueidentifier] NULL,
        [DeletedDate] [datetime] NULL,
        
        CONSTRAINT [FK_StocktakingMaster_Warehouse] 
            FOREIGN KEY ([WarehouseId]) REFERENCES [dbo].[CompanyBranch]([Id]),
        CONSTRAINT [FK_StocktakingMaster_CompanyBranch] 
            FOREIGN KEY ([CompanyBranchId]) REFERENCES [dbo].[CompanyBranch]([Id]),
        CONSTRAINT [FK_StocktakingMaster_CountedBy] 
            FOREIGN KEY ([CountedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
        CONSTRAINT [FK_StocktakingMaster_ReviewedBy] 
            FOREIGN KEY ([ReviewedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
        CONSTRAINT [FK_StocktakingMaster_ApprovedBy] 
            FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[ApplicationUser]([Id])
    );
    
    -- Tạo index
    CREATE INDEX [IX_StocktakingMaster_WarehouseId] ON [dbo].[StocktakingMaster]([WarehouseId]);
    CREATE INDEX [IX_StocktakingMaster_StocktakingDate] ON [dbo].[StocktakingMaster]([StocktakingDate]);
    CREATE INDEX [IX_StocktakingMaster_StocktakingStatus] ON [dbo].[StocktakingMaster]([StocktakingStatus]);
    CREATE INDEX [IX_StocktakingMaster_VoucherNumber] ON [dbo].[StocktakingMaster]([VoucherNumber]);
    
    PRINT 'Table StocktakingMaster created successfully.';
END
ELSE
BEGIN
    PRINT 'Table StocktakingMaster already exists.';
END
GO

-- =============================================
-- 2. Bảng chi tiết kiểm kho: StocktakingDetail
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StocktakingDetail]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StocktakingDetail] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [StocktakingMasterId] [uniqueidentifier] NOT NULL,        -- Liên kết với StocktakingMaster
        [ProductVariantId] [uniqueidentifier] NOT NULL,            -- Sản phẩm
        [SystemQuantity] [decimal](18,2) NOT NULL DEFAULT(0),     -- Số lượng hệ thống (tồn kho theo sổ sách)
        [CountedQuantity] [decimal](18,2) NULL,                   -- Số lượng thực tế đếm được
        [DifferenceQuantity] [decimal](18,2) NOT NULL DEFAULT(0), -- Chênh lệch (CountedQuantity - SystemQuantity)
        [SystemValue] [decimal](18,2) NULL,                       -- Giá trị theo sổ sách
        [CountedValue] [decimal](18,2) NULL,                      -- Giá trị thực tế
        [DifferenceValue] [decimal](18,2) NULL,                   -- Chênh lệch giá trị
        [UnitPrice] [decimal](18,2) NULL,                         -- Đơn giá (để tính giá trị)
        [AdjustmentType] [int] NULL,                              -- Loại điều chỉnh (0: Tăng, 1: Giảm, 2: Không điều chỉnh)
        [AdjustmentReason] [nvarchar](500) NULL,                 -- Lý do điều chỉnh
        [IsCounted] [bit] NOT NULL DEFAULT(0),                    -- Đã đếm chưa
        [CountedBy] [uniqueidentifier] NULL,                       -- Người đếm
        [CountedDate] [datetime] NULL,                            -- Ngày đếm
        [IsReviewed] [bit] NOT NULL DEFAULT(0),                   -- Đã rà soát
        [ReviewedBy] [uniqueidentifier] NULL,
        [ReviewedDate] [datetime] NULL,
        [ReviewNotes] [nvarchar](1000) NULL,                     -- Ghi chú rà soát
        [IsApproved] [bit] NOT NULL DEFAULT(0),                   -- Đã duyệt
        [ApprovedBy] [uniqueidentifier] NULL,
        [ApprovedDate] [datetime] NULL,
        [Notes] [nvarchar](1000) NULL,                           -- Ghi chú
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [IsDeleted] [bit] NOT NULL DEFAULT(0),
        [CreatedBy] [uniqueidentifier] NULL,
        [CreatedDate] [datetime] NULL DEFAULT GETDATE(),
        [UpdatedBy] [uniqueidentifier] NULL,
        [UpdatedDate] [datetime] NULL,
        
        CONSTRAINT [FK_StocktakingDetail_StocktakingMaster] 
            FOREIGN KEY ([StocktakingMasterId]) REFERENCES [dbo].[StocktakingMaster]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_StocktakingDetail_ProductVariant] 
            FOREIGN KEY ([ProductVariantId]) REFERENCES [dbo].[ProductVariant]([Id]),
        CONSTRAINT [FK_StocktakingDetail_CountedBy] 
            FOREIGN KEY ([CountedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
        CONSTRAINT [FK_StocktakingDetail_ReviewedBy] 
            FOREIGN KEY ([ReviewedBy]) REFERENCES [dbo].[ApplicationUser]([Id]),
        CONSTRAINT [FK_StocktakingDetail_ApprovedBy] 
            FOREIGN KEY ([ApprovedBy]) REFERENCES [dbo].[ApplicationUser]([Id])
    );
    
    -- Tạo index
    CREATE INDEX [IX_StocktakingDetail_StocktakingMasterId] ON [dbo].[StocktakingDetail]([StocktakingMasterId]);
    CREATE INDEX [IX_StocktakingDetail_ProductVariantId] ON [dbo].[StocktakingDetail]([ProductVariantId]);
    CREATE INDEX [IX_StocktakingDetail_IsCounted] ON [dbo].[StocktakingDetail]([IsCounted]);
    
    PRINT 'Table StocktakingDetail created successfully.';
END
ELSE
BEGIN
    PRINT 'Table StocktakingDetail already exists.';
END
GO

-- =============================================
-- 2.1. Bảng chi tiết kiểm kho theo từng định danh: StocktakingIdentifier
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StocktakingIdentifier]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StocktakingIdentifier] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [StocktakingMasterId] [uniqueidentifier] NOT NULL,        -- Liên kết với StocktakingMaster
        [StocktakingDetailId] [uniqueidentifier] NULL,            -- Liên kết với StocktakingDetail (để dễ query)
        [ProductVariantId] [uniqueidentifier] NOT NULL,            -- Sản phẩm (để tăng tốc query)
        [ProductVariantIdentifierId] [uniqueidentifier] NULL,     -- Định danh đã có trong hệ thống (nếu quét đúng)
        [IdentifierType] [int] NOT NULL,                         -- Loại định danh quét được (0: SerialNumber, 1: Barcode, 2: QRCode, 3: RFID, 4: IMEI, 5: MACAddress, 6: Other)
        [IdentifierValue] [nvarchar](500) NOT NULL,              -- Giá trị định danh đã quét
        [IsFoundInSystem] [bit] NOT NULL DEFAULT(0),               -- Định danh có tồn tại trong hệ thống không
        [IsExpectedInWarehouse] [bit] NOT NULL DEFAULT(0),         -- Định danh có được mong đợi ở kho này không
        [IsCounted] [bit] NOT NULL DEFAULT(1),                     -- Đã được đếm (mặc định true khi insert)
        [CountedBy] [uniqueidentifier] NULL,                       -- Người quét/đếm
        [CountedDate] [datetime] NULL DEFAULT GETDATE(),          -- Thời gian quét
        [CountMethod] [int] NULL,                                  -- Phương thức quét (0: Manual, 1: Barcode Scanner, 2: RFID Reader, 3: QR Code Scanner, 4: Import)
        [Notes] [nvarchar](1000) NULL,                            -- Ghi chú (ví dụ: định danh không tìm thấy, định danh ở kho khác)
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [IsDeleted] [bit] NOT NULL DEFAULT(0),
        [CreatedBy] [uniqueidentifier] NULL,
        [CreatedDate] [datetime] NULL DEFAULT GETDATE(),
        
        CONSTRAINT [FK_StocktakingIdentifier_StocktakingMaster] 
            FOREIGN KEY ([StocktakingMasterId]) REFERENCES [dbo].[StocktakingMaster]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_StocktakingIdentifier_StocktakingDetail] 
            FOREIGN KEY ([StocktakingDetailId]) REFERENCES [dbo].[StocktakingDetail]([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_StocktakingIdentifier_ProductVariant] 
            FOREIGN KEY ([ProductVariantId]) REFERENCES [dbo].[ProductVariant]([Id]),
        CONSTRAINT [FK_StocktakingIdentifier_ProductVariantIdentifier] 
            FOREIGN KEY ([ProductVariantIdentifierId]) REFERENCES [dbo].[ProductVariantIdentifier]([Id]),
        CONSTRAINT [FK_StocktakingIdentifier_CountedBy] 
            FOREIGN KEY ([CountedBy]) REFERENCES [dbo].[ApplicationUser]([Id])
    );
    
    -- Tạo index
    CREATE INDEX [IX_StocktakingIdentifier_StocktakingMasterId] ON [dbo].[StocktakingIdentifier]([StocktakingMasterId]);
    CREATE INDEX [IX_StocktakingIdentifier_StocktakingDetailId] ON [dbo].[StocktakingIdentifier]([StocktakingDetailId]);
    CREATE INDEX [IX_StocktakingIdentifier_ProductVariantId] ON [dbo].[StocktakingIdentifier]([ProductVariantId]);
    CREATE INDEX [IX_StocktakingIdentifier_ProductVariantIdentifierId] ON [dbo].[StocktakingIdentifier]([ProductVariantIdentifierId]);
    CREATE INDEX [IX_StocktakingIdentifier_IdentifierValue] ON [dbo].[StocktakingIdentifier]([IdentifierValue]);
    CREATE INDEX [IX_StocktakingIdentifier_IdentifierType] ON [dbo].[StocktakingIdentifier]([IdentifierType]);
    CREATE INDEX [IX_StocktakingIdentifier_IsFoundInSystem] ON [dbo].[StocktakingIdentifier]([IsFoundInSystem]);
    
    -- Unique constraint để tránh quét trùng trong cùng phiếu kiểm kho
    CREATE UNIQUE NONCLUSTERED INDEX [IX_StocktakingIdentifier_Unique] 
        ON [dbo].[StocktakingIdentifier]([StocktakingMasterId], [IdentifierType], [IdentifierValue])
        WHERE [IsDeleted] = 0;
    
    PRINT 'Table StocktakingIdentifier created successfully.';
END
ELSE
BEGIN
    PRINT 'Table StocktakingIdentifier already exists.';
END
GO

-- =============================================
-- 3. Bảng điều chỉnh tồn kho: StocktakingAdjustment
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StocktakingAdjustment]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StocktakingAdjustment] (
        [Id] [uniqueidentifier] NOT NULL PRIMARY KEY DEFAULT NEWID(),
        [StocktakingMasterId] [uniqueidentifier] NOT NULL,        -- Liên kết với phiếu kiểm kho
        [StocktakingDetailId] [uniqueidentifier] NOT NULL,       -- Chi tiết kiểm kho
        [StockInOutMasterId] [uniqueidentifier] NULL,            -- Liên kết với phiếu điều chỉnh (nếu tạo)
        [ProductVariantId] [uniqueidentifier] NOT NULL,          -- Sản phẩm
        [AdjustmentQuantity] [decimal](18,2) NOT NULL,          -- Số lượng điều chỉnh (có thể âm)
        [AdjustmentValue] [decimal](18,2) NULL,                  -- Giá trị điều chỉnh
        [UnitPrice] [decimal](18,2) NULL,                        -- Đơn giá điều chỉnh
        [AdjustmentType] [int] NOT NULL,                         -- Loại điều chỉnh (0: Tăng, 1: Giảm)
        [AdjustmentReason] [nvarchar](500) NULL,                -- Lý do điều chỉnh
        [AdjustmentDate] [datetime] NOT NULL DEFAULT GETDATE(),   -- Ngày điều chỉnh
        [AdjustedBy] [uniqueidentifier] NULL,                     -- Người thực hiện điều chỉnh
        [Notes] [nvarchar](1000) NULL,
        [IsApplied] [bit] NOT NULL DEFAULT(0),                   -- Đã áp dụng vào tồn kho chưa
        [AppliedDate] [datetime] NULL,
        [IsActive] [bit] NOT NULL DEFAULT(1),
        [IsDeleted] [bit] NOT NULL DEFAULT(0),
        [CreatedBy] [uniqueidentifier] NULL,
        [CreatedDate] [datetime] NULL DEFAULT GETDATE(),
        [UpdatedBy] [uniqueidentifier] NULL,
        [UpdatedDate] [datetime] NULL,
        
        CONSTRAINT [FK_StocktakingAdjustment_StocktakingMaster] 
            FOREIGN KEY ([StocktakingMasterId]) REFERENCES [dbo].[StocktakingMaster]([Id]),
        CONSTRAINT [FK_StocktakingAdjustment_StocktakingDetail] 
            FOREIGN KEY ([StocktakingDetailId]) REFERENCES [dbo].[StocktakingDetail]([Id]),
        CONSTRAINT [FK_StocktakingAdjustment_StockInOutMaster] 
            FOREIGN KEY ([StockInOutMasterId]) REFERENCES [dbo].[StockInOutMaster]([Id]),
        CONSTRAINT [FK_StocktakingAdjustment_ProductVariant] 
            FOREIGN KEY ([ProductVariantId]) REFERENCES [dbo].[ProductVariant]([Id]),
        CONSTRAINT [FK_StocktakingAdjustment_AdjustedBy] 
            FOREIGN KEY ([AdjustedBy]) REFERENCES [dbo].[ApplicationUser]([Id])
    );
    
    -- Tạo index
    CREATE INDEX [IX_StocktakingAdjustment_StocktakingMasterId] ON [dbo].[StocktakingAdjustment]([StocktakingMasterId]);
    CREATE INDEX [IX_StocktakingAdjustment_StocktakingDetailId] ON [dbo].[StocktakingAdjustment]([StocktakingDetailId]);
    CREATE INDEX [IX_StocktakingAdjustment_IsApplied] ON [dbo].[StocktakingAdjustment]([IsApplied]);
    CREATE INDEX [IX_StocktakingAdjustment_ProductVariantId] ON [dbo].[StocktakingAdjustment]([ProductVariantId]);
    
    PRINT 'Table StocktakingAdjustment created successfully.';
END
ELSE
BEGIN
    PRINT 'Table StocktakingAdjustment already exists.';
END
GO

-- =============================================
-- 4. Bảng hình ảnh kiểm kho: StocktakingImage
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[StocktakingImage]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[StocktakingImage] (
        [Id] [uniqueidentifier] NOT NULL,
        [StocktakingMasterId] [uniqueidentifier] NULL,           -- Hình ảnh của phiếu kiểm kho
        [StocktakingDetailId] [uniqueidentifier] NULL,          -- Hình ảnh của chi tiết kiểm kho
        [ImageData] [varbinary](max) NULL,                        -- Dữ liệu hình ảnh
        [CreateDate] [datetime] NOT NULL,
        [CreateBy] [uniqueidentifier] NOT NULL,
        [ModifiedDate] [datetime] NULL,
        [ModifiedBy] [uniqueidentifier] NOT NULL,
        [FileName] [nvarchar](255) NULL,
        [RelativePath] [nvarchar](500) NULL,
        [FullPath] [nvarchar](1000) NULL,
        [StorageType] [nvarchar](20) NULL,
        [FileSize] [bigint] NULL,
        [FileExtension] [nvarchar](10) NULL,
        [MimeType] [nvarchar](100) NULL,
        [Checksum] [nvarchar](64) NULL,
        [FileExists] [bit] NULL,
        [LastVerified] [datetime] NULL,
        [MigrationStatus] [nvarchar](20) NULL,
        CONSTRAINT [PK_StocktakingImage] PRIMARY KEY CLUSTERED 
        (
            [Id] ASC
        )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
    ) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY];
    
    -- Thêm default values
    ALTER TABLE [dbo].[StocktakingImage] ADD DEFAULT ('NAS') FOR [StorageType];
    ALTER TABLE [dbo].[StocktakingImage] ADD DEFAULT ((1)) FOR [FileExists];
    ALTER TABLE [dbo].[StocktakingImage] ADD DEFAULT ('Pending') FOR [MigrationStatus];
    
    -- Thêm foreign keys
    ALTER TABLE [dbo].[StocktakingImage] WITH CHECK ADD CONSTRAINT [FK_StocktakingImage_StocktakingMaster] 
        FOREIGN KEY ([StocktakingMasterId]) REFERENCES [dbo].[StocktakingMaster]([Id]) ON DELETE CASCADE;
    ALTER TABLE [dbo].[StocktakingImage] CHECK CONSTRAINT [FK_StocktakingImage_StocktakingMaster];
    
    ALTER TABLE [dbo].[StocktakingImage] WITH CHECK ADD CONSTRAINT [FK_StocktakingImage_StocktakingDetail] 
        FOREIGN KEY ([StocktakingDetailId]) REFERENCES [dbo].[StocktakingDetail]([Id]) ON DELETE CASCADE;
    ALTER TABLE [dbo].[StocktakingImage] CHECK CONSTRAINT [FK_StocktakingImage_StocktakingDetail];
    
    -- Tạo index
    CREATE INDEX [IX_StocktakingImage_StocktakingMasterId] ON [dbo].[StocktakingImage]([StocktakingMasterId]);
    CREATE INDEX [IX_StocktakingImage_StocktakingDetailId] ON [dbo].[StocktakingImage]([StocktakingDetailId]);
    
    PRINT 'Table StocktakingImage created successfully.';
END
ELSE
BEGIN
    PRINT 'Table StocktakingImage already exists.';
END
GO

-- =============================================
-- 5. Trigger tự động tính DifferenceQuantity khi CountedQuantity thay đổi
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TRG_StocktakingDetail_CalculateDifference]') AND type = 'TR')
BEGIN
    DROP TRIGGER [dbo].[TRG_StocktakingDetail_CalculateDifference];
END
GO

CREATE TRIGGER [dbo].[TRG_StocktakingDetail_CalculateDifference]
ON [dbo].[StocktakingDetail]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE sd
    SET 
        [DifferenceQuantity] = ISNULL(sd.[CountedQuantity], 0) - sd.[SystemQuantity],
        [CountedValue] = ISNULL(sd.[CountedQuantity], 0) * ISNULL(sd.[UnitPrice], 0),
        [DifferenceValue] = (ISNULL(sd.[CountedQuantity], 0) - sd.[SystemQuantity]) * ISNULL(sd.[UnitPrice], 0)
    FROM [dbo].[StocktakingDetail] sd
    INNER JOIN inserted i ON sd.[Id] = i.[Id]
    WHERE i.[CountedQuantity] IS NOT NULL;
END
GO

PRINT 'Trigger TRG_StocktakingDetail_CalculateDifference created successfully.';
GO

-- =============================================
-- 5.1. Trigger tự động cập nhật CountedQuantity khi có định danh mới được quét
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TRG_StocktakingIdentifier_UpdateCountedQuantity]') AND type = 'TR')
BEGIN
    DROP TRIGGER [dbo].[TRG_StocktakingIdentifier_UpdateCountedQuantity];
END
GO

CREATE TRIGGER [dbo].[TRG_StocktakingIdentifier_UpdateCountedQuantity]
ON [dbo].[StocktakingIdentifier]
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cập nhật CountedQuantity cho các StocktakingDetail liên quan
    UPDATE sd
    SET 
        [CountedQuantity] = (
            SELECT COUNT(DISTINCT si.[ProductVariantIdentifierId])
            FROM [dbo].[StocktakingIdentifier] si
            WHERE si.[StocktakingDetailId] = sd.[Id]
                AND si.[IsDeleted] = 0
                AND si.[IsCounted] = 1
                AND si.[ProductVariantIdentifierId] IS NOT NULL
        ),
        [IsCounted] = CASE 
            WHEN (
                SELECT COUNT(DISTINCT si.[ProductVariantIdentifierId])
                FROM [dbo].[StocktakingIdentifier] si
                WHERE si.[StocktakingDetailId] = sd.[Id]
                    AND si.[IsDeleted] = 0
                    AND si.[IsCounted] = 1
                    AND si.[ProductVariantIdentifierId] IS NOT NULL
            ) >= sd.[SystemQuantity] THEN 1
            ELSE 0
        END
    FROM [dbo].[StocktakingDetail] sd
    WHERE sd.[Id] IN (
        SELECT DISTINCT [StocktakingDetailId]
        FROM inserted
        WHERE [StocktakingDetailId] IS NOT NULL
        UNION
        SELECT DISTINCT [StocktakingDetailId]
        FROM deleted
        WHERE [StocktakingDetailId] IS NOT NULL
    );
END
GO

PRINT 'Trigger TRG_StocktakingIdentifier_UpdateCountedQuantity created successfully.';
GO

-- =============================================
-- 6. Stored Procedure: Tự động sinh số phiếu kiểm kho
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GenerateStocktakingVoucherNumber]') AND type = 'P')
BEGIN
    DROP PROCEDURE [dbo].[SP_GenerateStocktakingVoucherNumber];
END
GO

CREATE PROCEDURE [dbo].[SP_GenerateStocktakingVoucherNumber]
    @Year INT,
    @VoucherNumber NVARCHAR(50) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    DECLARE @Prefix NVARCHAR(10) = 'KK-';
    DECLARE @MaxNumber INT;
    
    -- Lấy số lớn nhất trong năm
    SELECT @MaxNumber = ISNULL(MAX(CAST(SUBSTRING([VoucherNumber], LEN(@Prefix) + LEN(CAST(@Year AS NVARCHAR(4))) + 2, 10) AS INT)), 0)
    FROM [dbo].[StocktakingMaster]
    WHERE [VoucherNumber] LIKE @Prefix + CAST(@Year AS NVARCHAR(4)) + '-%'
        AND ISNUMERIC(SUBSTRING([VoucherNumber], LEN(@Prefix) + LEN(CAST(@Year AS NVARCHAR(4))) + 2, 10)) = 1;
    
    SET @MaxNumber = @MaxNumber + 1;
    SET @VoucherNumber = @Prefix + CAST(@Year AS NVARCHAR(4)) + '-' + RIGHT('000000' + CAST(@MaxNumber AS NVARCHAR(10)), 6);
END
GO

PRINT 'Stored Procedure SP_GenerateStocktakingVoucherNumber created successfully.';
GO

-- =============================================
-- 7. Stored Procedure: Lấy số lượng hệ thống tại thời điểm kiểm kho
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_GetSystemQuantityAtDate]') AND type = 'P')
BEGIN
    DROP PROCEDURE [dbo].[SP_GetSystemQuantityAtDate];
END
GO

CREATE PROCEDURE [dbo].[SP_GetSystemQuantityAtDate]
    @ProductVariantId UNIQUEIDENTIFIER,
    @WarehouseId UNIQUEIDENTIFIER,
    @StocktakingDate DATETIME,
    @SystemQuantity DECIMAL(18,2) OUTPUT,
    @SystemValue DECIMAL(18,2) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Đếm số lượng định danh (ProductVariantIdentifier) có trong kho
    SELECT 
        @SystemQuantity = COUNT(DISTINCT pvi.[Id])
    FROM [dbo].[ProductVariantIdentifier] pvi
    WHERE pvi.[ProductVariantId] = @ProductVariantId
        AND pvi.[IsActive] = 1
        AND pvi.[Status] = 1; -- Status = 1: In Stock (cần điều chỉnh theo enum thực tế)
    
    -- Tính giá trị (cần lấy đơn giá từ ProductVariant hoặc StockInOutDetail)
    SET @SystemValue = @SystemQuantity * (
        SELECT TOP 1 ISNULL(pv.[StandardPrice], 0)
        FROM [dbo].[ProductVariant] pv
        WHERE pv.[Id] = @ProductVariantId
    );
    
    -- Nếu không có giá, lấy từ StockInOutDetail gần nhất
    IF @SystemValue = 0
    BEGIN
        SET @SystemValue = @SystemQuantity * (
            SELECT TOP 1 [UnitPrice]
            FROM [dbo].[StockInOutDetail] sod
            INNER JOIN [dbo].[StockInOutMaster] som ON sod.[StockInOutMasterId] = som.[Id]
            WHERE sod.[ProductVariantId] = @ProductVariantId
                AND som.[WarehouseId] = @WarehouseId
                AND som.[StockInOutDate] <= @StocktakingDate
                AND sod.[UnitPrice] > 0
            ORDER BY som.[StockInOutDate] DESC
        );
    END
    
    SET @SystemQuantity = ISNULL(@SystemQuantity, 0);
    SET @SystemValue = ISNULL(@SystemValue, 0);
END
GO

PRINT 'Stored Procedure SP_GetSystemQuantityAtDate created successfully.';
GO

-- =============================================
-- 7.1. Stored Procedure: Tìm ProductVariantIdentifier theo định danh
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SP_FindProductVariantIdentifier]') AND type = 'P')
BEGIN
    DROP PROCEDURE [dbo].[SP_FindProductVariantIdentifier];
END
GO

CREATE PROCEDURE [dbo].[SP_FindProductVariantIdentifier]
    @IdentifierType INT,              -- 0: SerialNumber, 1: Barcode, 2: QRCode, 3: RFID, 4: IMEI, 5: MACAddress, 6: Other
    @IdentifierValue NVARCHAR(500),
    @ProductVariantIdentifierId UNIQUEIDENTIFIER OUTPUT,
    @ProductVariantId UNIQUEIDENTIFIER OUTPUT,
    @IsFound BIT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    SET @IsFound = 0;
    SET @ProductVariantIdentifierId = NULL;
    SET @ProductVariantId = NULL;
    
    -- Tìm theo loại định danh
    IF @IdentifierType = 0 -- SerialNumber
    BEGIN
        SELECT TOP 1
            @ProductVariantIdentifierId = [Id],
            @ProductVariantId = [ProductVariantId],
            @IsFound = 1
        FROM [dbo].[ProductVariantIdentifier]
        WHERE [SerialNumber] = @IdentifierValue
            AND [IsActive] = 1;
    END
    ELSE IF @IdentifierType = 1 -- Barcode
    BEGIN
        SELECT TOP 1
            @ProductVariantIdentifierId = [Id],
            @ProductVariantId = [ProductVariantId],
            @IsFound = 1
        FROM [dbo].[ProductVariantIdentifier]
        WHERE [Barcode] = @IdentifierValue
            AND [IsActive] = 1;
    END
    ELSE IF @IdentifierType = 2 -- QRCode
    BEGIN
        SELECT TOP 1
            @ProductVariantIdentifierId = [Id],
            @ProductVariantId = [ProductVariantId],
            @IsFound = 1
        FROM [dbo].[ProductVariantIdentifier]
        WHERE [QRCode] = @IdentifierValue
            AND [IsActive] = 1;
    END
    ELSE IF @IdentifierType = 3 -- RFID
    BEGIN
        SELECT TOP 1
            @ProductVariantIdentifierId = [Id],
            @ProductVariantId = [ProductVariantId],
            @IsFound = 1
        FROM [dbo].[ProductVariantIdentifier]
        WHERE [RFID] = @IdentifierValue
            AND [IsActive] = 1;
    END
    ELSE IF @IdentifierType = 4 -- IMEI
    BEGIN
        SELECT TOP 1
            @ProductVariantIdentifierId = [Id],
            @ProductVariantId = [ProductVariantId],
            @IsFound = 1
        FROM [dbo].[ProductVariantIdentifier]
        WHERE [IMEI] = @IdentifierValue
            AND [IsActive] = 1;
    END
    ELSE IF @IdentifierType = 5 -- MACAddress
    BEGIN
        SELECT TOP 1
            @ProductVariantIdentifierId = [Id],
            @ProductVariantId = [ProductVariantId],
            @IsFound = 1
        FROM [dbo].[ProductVariantIdentifier]
        WHERE [MACAddress] = @IdentifierValue
            AND [IsActive] = 1;
    END
    ELSE -- Other hoặc tìm tất cả các trường
    BEGIN
        SELECT TOP 1
            @ProductVariantIdentifierId = [Id],
            @ProductVariantId = [ProductVariantId],
            @IsFound = 1
        FROM [dbo].[ProductVariantIdentifier]
        WHERE (
            [SerialNumber] = @IdentifierValue
            OR [Barcode] = @IdentifierValue
            OR [QRCode] = @IdentifierValue
            OR [RFID] = @IdentifierValue
            OR [IMEI] = @IdentifierValue
            OR [MACAddress] = @IdentifierValue
            OR [OtherIdentifier] = @IdentifierValue
        )
        AND [IsActive] = 1;
    END
END
GO

PRINT 'Stored Procedure SP_FindProductVariantIdentifier created successfully.';
GO

PRINT '========================================';
PRINT 'All Stocktaking tables and objects created successfully!';
PRINT '========================================';
GO
