-- =============================================
-- Script tạo bảng quản lý kiểm kho hàng tháng
-- Tác giả: System
-- Ngày tạo: 2025
-- Mô tả: Tạo bảng InventoryStocktaking và InventoryStocktakingDetail
--         để quản lý quy trình kiểm kho hàng tháng
-- =============================================

USE [VnsErp2025]
GO

-- =============================================
-- 1. Bảng chính: InventoryStocktaking (Phiếu kiểm kho)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InventoryStocktaking]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[InventoryStocktaking](
        [Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [StocktakingCode] [nvarchar](50) NOT NULL,                    -- Mã phiếu kiểm kho (VD: KK202501001)
        [StocktakingName] [nvarchar](255) NULL,                       -- Tên phiếu kiểm kho
        [WarehouseId] [uniqueidentifier] NOT NULL,                      -- ID kho (FK -> CompanyBranch)
        [PeriodYear] [int] NOT NULL,                                   -- Năm kiểm kho (VD: 2025)
        [PeriodMonth] [int] NOT NULL,                                  -- Tháng kiểm kho (1-12)
        [StocktakingDate] [datetime] NOT NULL,                         -- Ngày kiểm kho
        [StartDate] [datetime] NULL,                                   -- Ngày bắt đầu kiểm kho
        [EndDate] [datetime] NULL,                                     -- Ngày kết thúc kiểm kho
        
        -- Trạng thái
        [Status] [int] NOT NULL DEFAULT 0,                             -- 0: Nháp, 1: Đang kiểm, 2: Hoàn thành, 3: Đã xác nhận, 4: Đã hủy
        [IsLocked] [bit] NOT NULL DEFAULT 0,                           -- Đã khóa chưa
        [LockedDate] [datetime] NULL,                                  -- Ngày khóa
        [LockedBy] [uniqueidentifier] NULL,                            -- ID người khóa (FK -> ApplicationUser)
        [LockReason] [nvarchar](500) NULL,                             -- Lý do khóa
        
        -- Thông tin người thực hiện
        [CreatedBy] [uniqueidentifier] NOT NULL,                       -- ID người tạo (FK -> ApplicationUser)
        [CreateDate] [datetime] NOT NULL DEFAULT GETDATE(),            -- Ngày tạo
        [ModifiedBy] [uniqueidentifier] NULL,                           -- ID người sửa (FK -> ApplicationUser)
        [ModifiedDate] [datetime] NULL,                                -- Ngày sửa
        [ApprovedBy] [uniqueidentifier] NULL,                           -- ID người phê duyệt (FK -> ApplicationUser)
        [ApprovedDate] [datetime] NULL,                                -- Ngày phê duyệt
        
        -- Ghi chú
        [Notes] [nvarchar](1000) NULL,                                 -- Ghi chú chung
        [ApprovalNotes] [nvarchar](1000) NULL,                         -- Ghi chú phê duyệt
        
        -- Audit
        [IsActive] [bit] NOT NULL DEFAULT 1,                           -- Trạng thái hoạt động
        [IsDeleted] [bit] NOT NULL DEFAULT 0,                         -- Đã xóa chưa
        [DeletedBy] [uniqueidentifier] NULL,                            -- ID người xóa (FK -> ApplicationUser)
        [DeletedDate] [datetime] NULL,                                 -- Ngày xóa
        
        CONSTRAINT [PK_InventoryStocktaking] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_InventoryStocktaking_Warehouse] FOREIGN KEY ([WarehouseId]) 
            REFERENCES [dbo].[CompanyBranch] ([Id]),
        CONSTRAINT [FK_InventoryStocktaking_CreatedBy] FOREIGN KEY ([CreatedBy]) 
            REFERENCES [dbo].[ApplicationUser] ([Id]),
        CONSTRAINT [FK_InventoryStocktaking_ModifiedBy] FOREIGN KEY ([ModifiedBy]) 
            REFERENCES [dbo].[ApplicationUser] ([Id]),
        CONSTRAINT [FK_InventoryStocktaking_ApprovedBy] FOREIGN KEY ([ApprovedBy]) 
            REFERENCES [dbo].[ApplicationUser] ([Id]),
        CONSTRAINT [FK_InventoryStocktaking_LockedBy] FOREIGN KEY ([LockedBy]) 
            REFERENCES [dbo].[ApplicationUser] ([Id]),
        CONSTRAINT [FK_InventoryStocktaking_DeletedBy] FOREIGN KEY ([DeletedBy]) 
            REFERENCES [dbo].[ApplicationUser] ([Id]),
        CONSTRAINT [CK_InventoryStocktaking_PeriodMonth] CHECK ([PeriodMonth] >= 1 AND [PeriodMonth] <= 12),
        CONSTRAINT [CK_InventoryStocktaking_PeriodYear] CHECK ([PeriodYear] >= 2000 AND [PeriodYear] <= 9999),
        CONSTRAINT [CK_InventoryStocktaking_Status] CHECK ([Status] >= 0 AND [Status] <= 4)
    )
    
    -- Tạo index cho các trường thường query
    CREATE UNIQUE NONCLUSTERED INDEX [IX_InventoryStocktaking_Code] 
        ON [dbo].[InventoryStocktaking] ([StocktakingCode] ASC)
    
    CREATE NONCLUSTERED INDEX [IX_InventoryStocktaking_Warehouse_Period] 
        ON [dbo].[InventoryStocktaking] ([WarehouseId], [PeriodYear], [PeriodMonth])
    
    CREATE NONCLUSTERED INDEX [IX_InventoryStocktaking_Status] 
        ON [dbo].[InventoryStocktaking] ([Status], [IsDeleted], [IsActive])
    
    CREATE NONCLUSTERED INDEX [IX_InventoryStocktaking_Date] 
        ON [dbo].[InventoryStocktaking] ([StocktakingDate])
    
    PRINT 'Bảng InventoryStocktaking đã được tạo thành công.'
END
ELSE
BEGIN
    PRINT 'Bảng InventoryStocktaking đã tồn tại.'
END
GO

-- =============================================
-- 2. Bảng chi tiết: InventoryStocktakingDetail (Chi tiết kiểm kho)
-- =============================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[InventoryStocktakingDetail]') AND type in (N'U'))
BEGIN
    CREATE TABLE [dbo].[InventoryStocktakingDetail](
        [Id] [uniqueidentifier] NOT NULL DEFAULT NEWID(),
        [StocktakingId] [uniqueidentifier] NOT NULL,                   -- ID phiếu kiểm kho (FK -> InventoryStocktaking)
        [ProductVariantId] [uniqueidentifier] NOT NULL,                 -- ID biến thể sản phẩm (FK -> ProductVariant)
        
        -- Thông tin sản phẩm (denormalized để tăng tốc query)
        [ProductCode] [nvarchar](50) NULL,                              -- Mã sản phẩm (SKU có thể dùng field này)
        [ProductVariantCode] [nvarchar](50) NULL,                      -- Mã biến thể (hoặc dùng field này làm SKU)
        [ProductName] [nvarchar](500) NULL,                            -- Tên sản phẩm
        [UnitOfMeasureId] [uniqueidentifier] NULL,                     -- ID đơn vị tính (FK -> UnitOfMeasure)
        [UnitOfMeasureName] [nvarchar](100) NULL,                     -- Tên đơn vị tính
        
        -- Số lượng
        [BookQuantity] [decimal](18, 4) NOT NULL DEFAULT 0,           -- Số lượng sổ sách (từ InventoryBalance)
        [PhysicalQuantity] [decimal](18, 4) NULL,                      -- Số lượng thực tế (đếm được)
        [DifferenceQuantity] [decimal](18, 4) NULL,                    -- Chênh lệch = PhysicalQuantity - BookQuantity
        
        -- Giá trị
        [UnitPrice] [decimal](18, 4) NULL,                             -- Đơn giá (từ InventoryBalance hoặc giá trung bình)
        [BookValue] [decimal](18, 4) NULL,                             -- Giá trị sổ sách = BookQuantity * UnitPrice
        [PhysicalValue] [decimal](18, 4) NULL,                         -- Giá trị thực tế = PhysicalQuantity * UnitPrice
        [DifferenceValue] [decimal](18, 4) NULL,                       -- Chênh lệch giá trị = PhysicalValue - BookValue
        
        -- Thông tin kiểm kho
        [CountedBy] [uniqueidentifier] NULL,                           -- ID người đếm (FK -> ApplicationUser)
        [CountedDate] [datetime] NULL,                                 -- Ngày đếm
        [CountedTimes] [int] NOT NULL DEFAULT 1,                       -- Số lần đếm (để xử lý đếm lại)
        [IsReCount] [bit] NOT NULL DEFAULT 0,                          -- Có phải đếm lại không
        
        -- Xử lý chênh lệch
        [AdjustmentType] [int] NULL,                                   -- Loại điều chỉnh: 0=Không điều chỉnh, 1=Điều chỉnh tăng, 2=Điều chỉnh giảm
        [AdjustmentQuantity] [decimal](18, 4) NULL,                    -- Số lượng điều chỉnh
        [AdjustmentReason] [nvarchar](500) NULL,                      -- Lý do điều chỉnh
        [IsAdjusted] [bit] NOT NULL DEFAULT 0,                         -- Đã điều chỉnh chưa
        
        -- Ghi chú
        [Notes] [nvarchar](1000) NULL,                                 -- Ghi chú chi tiết
        
        -- Thứ tự hiển thị
        [DisplayOrder] [int] NOT NULL DEFAULT 0,                       -- Thứ tự hiển thị
        
        -- Audit
        [IsActive] [bit] NOT NULL DEFAULT 1,
        [IsDeleted] [bit] NOT NULL DEFAULT 0,
        [CreateDate] [datetime] NOT NULL DEFAULT GETDATE(),
        [CreatedBy] [uniqueidentifier] NOT NULL,
        [ModifiedDate] [datetime] NULL,
        [ModifiedBy] [uniqueidentifier] NULL,
        
        CONSTRAINT [PK_InventoryStocktakingDetail] PRIMARY KEY CLUSTERED ([Id] ASC),
        CONSTRAINT [FK_InventoryStocktakingDetail_Stocktaking] FOREIGN KEY ([StocktakingId]) 
            REFERENCES [dbo].[InventoryStocktaking] ([Id]) ON DELETE CASCADE,
        CONSTRAINT [FK_InventoryStocktakingDetail_ProductVariant] FOREIGN KEY ([ProductVariantId]) 
            REFERENCES [dbo].[ProductVariant] ([Id]),
        CONSTRAINT [FK_InventoryStocktakingDetail_UnitOfMeasure] FOREIGN KEY ([UnitOfMeasureId]) 
            REFERENCES [dbo].[UnitOfMeasure] ([Id]),
        CONSTRAINT [FK_InventoryStocktakingDetail_CountedBy] FOREIGN KEY ([CountedBy]) 
            REFERENCES [dbo].[ApplicationUser] ([Id]),
        CONSTRAINT [FK_InventoryStocktakingDetail_CreatedBy] FOREIGN KEY ([CreatedBy]) 
            REFERENCES [dbo].[ApplicationUser] ([Id]),
        CONSTRAINT [FK_InventoryStocktakingDetail_ModifiedBy] FOREIGN KEY ([ModifiedBy]) 
            REFERENCES [dbo].[ApplicationUser] ([Id]),
        CONSTRAINT [CK_InventoryStocktakingDetail_AdjustmentType] CHECK ([AdjustmentType] IS NULL OR ([AdjustmentType] >= 0 AND [AdjustmentType] <= 2))
    )
    
    -- Tạo index cho các trường thường query
    CREATE NONCLUSTERED INDEX [IX_InventoryStocktakingDetail_Stocktaking] 
        ON [dbo].[InventoryStocktakingDetail] ([StocktakingId])
    
    CREATE NONCLUSTERED INDEX [IX_InventoryStocktakingDetail_ProductVariant] 
        ON [dbo].[InventoryStocktakingDetail] ([ProductVariantId])
    
    CREATE NONCLUSTERED INDEX [IX_InventoryStocktakingDetail_ProductCode] 
        ON [dbo].[InventoryStocktakingDetail] ([ProductCode])
    
    CREATE NONCLUSTERED INDEX [IX_InventoryStocktakingDetail_ProductVariantCode] 
        ON [dbo].[InventoryStocktakingDetail] ([ProductVariantCode])
    
    -- Index composite để tìm nhanh sản phẩm trong phiếu kiểm kho
    CREATE NONCLUSTERED INDEX [IX_InventoryStocktakingDetail_Stocktaking_Product] 
        ON [dbo].[InventoryStocktakingDetail] ([StocktakingId], [ProductVariantId])
    
    PRINT 'Bảng InventoryStocktakingDetail đã được tạo thành công.'
END
ELSE
BEGIN
    PRINT 'Bảng InventoryStocktakingDetail đã tồn tại.'
END
GO

-- =============================================
-- 3. Tạo trigger để tự động tính chênh lệch
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TR_InventoryStocktakingDetail_CalculateDifference]') AND type = 'TR')
    DROP TRIGGER [dbo].[TR_InventoryStocktakingDetail_CalculateDifference]
GO

CREATE TRIGGER [dbo].[TR_InventoryStocktakingDetail_CalculateDifference]
ON [dbo].[InventoryStocktakingDetail]
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE d
    SET 
        [DifferenceQuantity] = ISNULL(d.[PhysicalQuantity], 0) - d.[BookQuantity],
        [BookValue] = d.[BookQuantity] * ISNULL(d.[UnitPrice], 0),
        [PhysicalValue] = ISNULL(d.[PhysicalQuantity], 0) * ISNULL(d.[UnitPrice], 0),
        [DifferenceValue] = (ISNULL(d.[PhysicalQuantity], 0) - d.[BookQuantity]) * ISNULL(d.[UnitPrice], 0)
    FROM [dbo].[InventoryStocktakingDetail] d
    INNER JOIN inserted i ON d.[Id] = i.[Id]
    WHERE d.[PhysicalQuantity] IS NOT NULL;
END
GO

PRINT 'Trigger TR_InventoryStocktakingDetail_CalculateDifference đã được tạo thành công.'
GO

-- =============================================
-- 4. Tạo view tổng hợp thông tin kiểm kho
-- =============================================
IF EXISTS (SELECT * FROM sys.views WHERE object_id = OBJECT_ID(N'[dbo].[vw_InventoryStocktakingSummary]'))
    DROP VIEW [dbo].[vw_InventoryStocktakingSummary]
GO

CREATE VIEW [dbo].[vw_InventoryStocktakingSummary]
AS
SELECT 
    s.[Id],
    s.[StocktakingCode],
    s.[StocktakingName],
    s.[WarehouseId],
    w.[BranchName] AS [WarehouseName],
    w.[BranchCode] AS [WarehouseCode],
    s.[PeriodYear],
    s.[PeriodMonth],
    s.[StocktakingDate],
    s.[StartDate],
    s.[EndDate],
    s.[Status],
    s.[IsLocked],
    s.[LockedDate],
    s.[LockedBy],
    s.[CreatedBy],
    s.[CreateDate],
    s.[ApprovedBy],
    s.[ApprovedDate],
    -- Tổng hợp từ detail
    COUNT(d.[Id]) AS [TotalItems],                                    -- Tổng số sản phẩm
    SUM(d.[BookQuantity]) AS [TotalBookQuantity],                     -- Tổng số lượng sổ sách
    SUM(ISNULL(d.[PhysicalQuantity], 0)) AS [TotalPhysicalQuantity], -- Tổng số lượng thực tế
    SUM(ISNULL(d.[DifferenceQuantity], 0)) AS [TotalDifferenceQuantity], -- Tổng chênh lệch số lượng
    SUM(ISNULL(d.[BookValue], 0)) AS [TotalBookValue],                -- Tổng giá trị sổ sách
    SUM(ISNULL(d.[PhysicalValue], 0)) AS [TotalPhysicalValue],       -- Tổng giá trị thực tế
    SUM(ISNULL(d.[DifferenceValue], 0)) AS [TotalDifferenceValue],   -- Tổng chênh lệch giá trị
    COUNT(CASE WHEN d.[PhysicalQuantity] IS NULL THEN 1 END) AS [UncountedItems], -- Số sản phẩm chưa đếm
    COUNT(CASE WHEN d.[DifferenceQuantity] <> 0 THEN 1 END) AS [ItemsWithDifference] -- Số sản phẩm có chênh lệch
FROM [dbo].[InventoryStocktaking] s
LEFT JOIN [dbo].[CompanyBranch] w ON s.[WarehouseId] = w.[Id]
LEFT JOIN [dbo].[InventoryStocktakingDetail] d ON s.[Id] = d.[StocktakingId] 
    AND d.[IsDeleted] = 0 AND d.[IsActive] = 1
WHERE s.[IsDeleted] = 0 AND s.[IsActive] = 1
GROUP BY 
    s.[Id], s.[StocktakingCode], s.[StocktakingName], s.[WarehouseId],
    w.[BranchName], w.[BranchCode], s.[PeriodYear], s.[PeriodMonth],
    s.[StocktakingDate], s.[StartDate], s.[EndDate], s.[Status],
    s.[IsLocked], s.[LockedDate], s.[LockedBy], s.[CreatedBy],
    s.[CreateDate], s.[ApprovedBy], s.[ApprovedDate]
GO

PRINT 'View vw_InventoryStocktakingSummary đã được tạo thành công.'
GO

-- =============================================
-- 5. Tạo stored procedure để tạo phiếu kiểm kho từ InventoryBalance
-- =============================================
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_CreateStocktakingFromBalance]') AND type = 'P')
    DROP PROCEDURE [dbo].[sp_CreateStocktakingFromBalance]
GO

CREATE PROCEDURE [dbo].[sp_CreateStocktakingFromBalance]
    @WarehouseId uniqueidentifier,
    @PeriodYear int,
    @PeriodMonth int,
    @StocktakingDate datetime = NULL,
    @CreatedBy uniqueidentifier,
    @StocktakingCode nvarchar(50) OUTPUT,
    @StocktakingId uniqueidentifier OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    
    BEGIN TRY
        BEGIN TRANSACTION;
        
        -- Kiểm tra kỳ đã có phiếu kiểm kho chưa
        IF EXISTS (
            SELECT 1 FROM [dbo].[InventoryStocktaking]
            WHERE [WarehouseId] = @WarehouseId
                AND [PeriodYear] = @PeriodYear
                AND [PeriodMonth] = @PeriodMonth
                AND [IsDeleted] = 0
        )
        BEGIN
            RAISERROR('Kỳ này đã có phiếu kiểm kho. Vui lòng kiểm tra lại.', 16, 1);
            RETURN;
        END
        
        -- Tạo mã phiếu kiểm kho tự động
        DECLARE @YearMonth nvarchar(6) = CAST(@PeriodYear AS nvarchar(4)) + RIGHT('0' + CAST(@PeriodMonth AS nvarchar(2)), 2);
        DECLARE @MaxCode int;
        
        SELECT @MaxCode = ISNULL(MAX(CAST(SUBSTRING([StocktakingCode], 9, LEN([StocktakingCode])) AS int)), 0)
        FROM [dbo].[InventoryStocktaking]
        WHERE [StocktakingCode] LIKE 'KK' + @YearMonth + '%';
        
        SET @StocktakingCode = 'KK' + @YearMonth + RIGHT('000' + CAST(@MaxCode + 1 AS nvarchar(3)), 3);
        
        -- Tạo phiếu kiểm kho
        SET @StocktakingId = NEWID();
        SET @StocktakingDate = ISNULL(@StocktakingDate, GETDATE());
        
        INSERT INTO [dbo].[InventoryStocktaking] (
            [Id], [StocktakingCode], [StocktakingName], [WarehouseId],
            [PeriodYear], [PeriodMonth], [StocktakingDate],
            [Status], [CreatedBy], [CreateDate]
        )
        VALUES (
            @StocktakingId, @StocktakingCode, 
            N'Kiểm kho tháng ' + CAST(@PeriodMonth AS nvarchar(2)) + '/' + CAST(@PeriodYear AS nvarchar(4)),
            @WarehouseId, @PeriodYear, @PeriodMonth, @StocktakingDate,
            0, @CreatedBy, GETDATE()
        );
        
        -- Tạo chi tiết từ InventoryBalance
        INSERT INTO [dbo].[InventoryStocktakingDetail] (
            [Id], [StocktakingId], [ProductVariantId],
            [ProductCode], [ProductVariantCode], [ProductName],
            [UnitOfMeasureId], [UnitOfMeasureName],
            [BookQuantity], [UnitPrice],
            [CreatedBy], [CreateDate], [DisplayOrder]
        )
        SELECT 
            NEWID(),
            @StocktakingId,
            ib.[ProductVariantId],
            ib.[ProductCode],
            ib.[ProductVariantCode],
            ib.[ProductName],
            ib.[UnitOfMeasureId],
            ib.[UnitOfMeasureName],
            ib.[ClosingBalance], -- Số lượng sổ sách = Tồn cuối kỳ
            CASE 
                WHEN ib.[ClosingBalance] > 0 
                THEN ISNULL(ib.[ClosingValue], 0) / ib.[ClosingBalance]
                ELSE 0 
            END AS [UnitPrice], -- Đơn giá trung bình
            @CreatedBy,
            GETDATE(),
            ROW_NUMBER() OVER (ORDER BY ib.[ProductCode], ib.[ProductVariantCode])
        FROM [dbo].[InventoryBalance] ib
        WHERE ib.[WarehouseId] = @WarehouseId
            AND ib.[PeriodYear] = @PeriodYear
            AND ib.[PeriodMonth] = @PeriodMonth
            AND ib.[IsDeleted] = 0
            AND ib.[IsActive] = 1
            AND ib.[ClosingBalance] > 0; -- Chỉ lấy sản phẩm có tồn kho
        
        COMMIT TRANSACTION;
        
        PRINT 'Đã tạo phiếu kiểm kho thành công: ' + @StocktakingCode;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        
        DECLARE @ErrorMessage nvarchar(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity int = ERROR_SEVERITY();
        DECLARE @ErrorState int = ERROR_STATE();
        
        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
    END CATCH
END
GO

PRINT 'Stored procedure sp_CreateStocktakingFromBalance đã được tạo thành công.'
GO

-- =============================================
-- Kết thúc script
-- =============================================
PRINT '============================================='
PRINT 'Hoàn thành tạo bảng quản lý kiểm kho hàng tháng'
PRINT '============================================='
PRINT ''
PRINT 'Các bảng đã tạo:'
PRINT '  1. InventoryStocktaking - Phiếu kiểm kho'
PRINT '  2. InventoryStocktakingDetail - Chi tiết kiểm kho'
PRINT ''
PRINT 'Các đối tượng hỗ trợ:'
PRINT '  1. View: vw_InventoryStocktakingSummary - Tổng hợp thông tin kiểm kho'
PRINT '  2. Trigger: TR_InventoryStocktakingDetail_CalculateDifference - Tự động tính chênh lệch'
PRINT '  3. Stored Procedure: sp_CreateStocktakingFromBalance - Tạo phiếu từ InventoryBalance'
PRINT ''
PRINT 'Lưu ý về SKU:'
PRINT '  - Có thể sử dụng ProductCode hoặc ProductVariantCode làm mã SKU'
PRINT '  - Cả hai trường đều đã được index để tìm kiếm nhanh'
PRINT '  - ProductCode: Mã sản phẩm chính (từ ProductService)'
PRINT '  - ProductVariantCode: Mã biến thể (từ ProductVariant)'
PRINT ''
GO
