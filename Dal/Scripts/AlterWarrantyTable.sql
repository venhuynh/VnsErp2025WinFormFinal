-- =============================================
-- Script điều chỉnh bảng Warranty (Bảo hành)
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Điều chỉnh bảng Warranty để phù hợp với pattern của bảng Device
--        Thêm các trường audit (IsActive, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy)
--        Thêm trường Notes và DeviceId làm FK đến bảng Device
--        Xóa các cột không cần thiết: StockInOutDetailId, UniqueProductInfo, WarrantyProvider, WarrantyNumber, WarrantyContact
-- =============================================

USE [VnsErp2025Final]
GO

-- =============================================
-- 1. Kiểm tra và thêm các cột mới (nếu chưa tồn tại)
-- =============================================

-- Thêm cột IsActive
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'IsActive')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    ADD [IsActive] [bit] NOT NULL DEFAULT(1);
    
    PRINT 'Đã thêm cột IsActive vào bảng Warranty';
END
GO

-- Thêm cột CreatedDate
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'CreatedDate')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    ADD [CreatedDate] [datetime] NOT NULL DEFAULT(GETDATE());
    
    PRINT 'Đã thêm cột CreatedDate vào bảng Warranty';
END
GO

-- Thêm cột UpdatedDate
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'UpdatedDate')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    ADD [UpdatedDate] [datetime] NULL;
    
    PRINT 'Đã thêm cột UpdatedDate vào bảng Warranty';
END
GO

-- Thêm cột CreatedBy
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'CreatedBy')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    ADD [CreatedBy] [uniqueidentifier] NULL;
    
    PRINT 'Đã thêm cột CreatedBy vào bảng Warranty';
END
GO

-- Thêm cột UpdatedBy
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'UpdatedBy')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    ADD [UpdatedBy] [uniqueidentifier] NULL;
    
    PRINT 'Đã thêm cột UpdatedBy vào bảng Warranty';
END
GO

-- Thêm cột Notes
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'Notes')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    ADD [Notes] [nvarchar](1000) NULL;
    
    PRINT 'Đã thêm cột Notes vào bảng Warranty';
END
GO

-- =============================================
-- 2. Xóa cột StockInOutDetailId và Foreign Key liên quan
--    Thêm cột DeviceId làm Foreign Key đến bảng Device
-- =============================================

-- Xóa Foreign Key constraint của StockInOutDetailId nếu tồn tại
IF EXISTS (
    SELECT * FROM sys.foreign_keys 
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[Warranty]') 
    AND referenced_object_id = OBJECT_ID(N'[dbo].[StockInOutDetail]')
)
BEGIN
    DECLARE @FKName NVARCHAR(200);
    
    SELECT @FKName = name
    FROM sys.foreign_keys
    WHERE parent_object_id = OBJECT_ID(N'[dbo].[Warranty]')
    AND referenced_object_id = OBJECT_ID(N'[dbo].[StockInOutDetail]');
    
    IF @FKName IS NOT NULL
    BEGIN
        EXEC('ALTER TABLE [dbo].[Warranty] DROP CONSTRAINT ' + @FKName);
        PRINT 'Đã xóa Foreign Key constraint: ' + @FKName;
    END
END
GO

-- Xóa Index của StockInOutDetailId nếu tồn tại
IF EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Warranty_StockInOutDetailId' AND object_id = OBJECT_ID(N'[dbo].[Warranty]'))
BEGIN
    DROP INDEX [IX_Warranty_StockInOutDetailId] ON [dbo].[Warranty];
    PRINT 'Đã xóa Index IX_Warranty_StockInOutDetailId';
END
GO

-- Xóa cột StockInOutDetailId nếu tồn tại
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'StockInOutDetailId')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    DROP COLUMN [StockInOutDetailId];
    
    PRINT 'Đã xóa cột StockInOutDetailId khỏi bảng Warranty';
END
GO

-- Xóa cột UniqueProductInfo nếu tồn tại
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'UniqueProductInfo')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    DROP COLUMN [UniqueProductInfo];
    
    PRINT 'Đã xóa cột UniqueProductInfo khỏi bảng Warranty';
END
GO

-- Xóa cột WarrantyProvider nếu tồn tại
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'WarrantyProvider')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    DROP COLUMN [WarrantyProvider];
    
    PRINT 'Đã xóa cột WarrantyProvider khỏi bảng Warranty';
END
GO

-- Xóa cột WarrantyNumber nếu tồn tại
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'WarrantyNumber')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    DROP COLUMN [WarrantyNumber];
    
    PRINT 'Đã xóa cột WarrantyNumber khỏi bảng Warranty';
END
GO

-- Xóa cột WarrantyContact nếu tồn tại
IF EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'WarrantyContact')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    DROP COLUMN [WarrantyContact];
    
    PRINT 'Đã xóa cột WarrantyContact khỏi bảng Warranty';
END
GO

-- Thêm cột DeviceId (Foreign Key đến bảng Device)
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Warranty]') AND name = 'DeviceId')
BEGIN
    ALTER TABLE [dbo].[Warranty]
    ADD [DeviceId] [uniqueidentifier] NULL;
    
    PRINT 'Đã thêm cột DeviceId vào bảng Warranty';
END
GO

-- Thêm Foreign Key constraint từ DeviceId đến Device.Id
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'Device')
BEGIN
    IF NOT EXISTS (
        SELECT * FROM sys.foreign_keys 
        WHERE parent_object_id = OBJECT_ID(N'[dbo].[Warranty]') 
        AND name = 'FK_Warranty_Device'
    )
    BEGIN
        ALTER TABLE [dbo].[Warranty]
        ADD CONSTRAINT [FK_Warranty_Device]
        FOREIGN KEY([DeviceId])
        REFERENCES [dbo].[Device]([Id]);
        
        PRINT 'Đã thêm Foreign Key FK_Warranty_Device';
    END
END
GO

-- =============================================
-- 3. Thêm Foreign Key constraint cho CreatedBy và UpdatedBy
--    (nếu bảng ApplicationUser tồn tại)
-- =============================================

-- Foreign Key cho CreatedBy
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ApplicationUser')
BEGIN
    IF NOT EXISTS (
        SELECT * FROM sys.foreign_keys 
        WHERE parent_object_id = OBJECT_ID(N'[dbo].[Warranty]') 
        AND name = 'FK_Warranty_CreatedBy_ApplicationUser'
    )
    BEGIN
        ALTER TABLE [dbo].[Warranty]
        ADD CONSTRAINT [FK_Warranty_CreatedBy_ApplicationUser]
        FOREIGN KEY([CreatedBy])
        REFERENCES [dbo].[ApplicationUser]([Id]);
        
        PRINT 'Đã thêm Foreign Key FK_Warranty_CreatedBy_ApplicationUser';
    END
END
GO

-- Foreign Key cho UpdatedBy
IF EXISTS (SELECT * FROM sys.tables WHERE name = 'ApplicationUser')
BEGIN
    IF NOT EXISTS (
        SELECT * FROM sys.foreign_keys 
        WHERE parent_object_id = OBJECT_ID(N'[dbo].[Warranty]') 
        AND name = 'FK_Warranty_UpdatedBy_ApplicationUser'
    )
    BEGIN
        ALTER TABLE [dbo].[Warranty]
        ADD CONSTRAINT [FK_Warranty_UpdatedBy_ApplicationUser]
        FOREIGN KEY([UpdatedBy])
        REFERENCES [dbo].[ApplicationUser]([Id]);
        
        PRINT 'Đã thêm Foreign Key FK_Warranty_UpdatedBy_ApplicationUser';
    END
END
GO

-- =============================================
-- 4. Tạo Index để tối ưu truy vấn
-- =============================================

-- Index cho WarrantyStatus
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Warranty_WarrantyStatus' AND object_id = OBJECT_ID(N'[dbo].[Warranty]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Warranty_WarrantyStatus]
    ON [dbo].[Warranty]([WarrantyStatus] ASC);
    
    PRINT 'Đã tạo Index IX_Warranty_WarrantyStatus';
END
GO

-- Index cho WarrantyUntil (để tìm warranty sắp hết hạn)
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Warranty_WarrantyUntil' AND object_id = OBJECT_ID(N'[dbo].[Warranty]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Warranty_WarrantyUntil]
    ON [dbo].[Warranty]([WarrantyUntil] ASC)
    WHERE [WarrantyUntil] IS NOT NULL;
    
    PRINT 'Đã tạo Index IX_Warranty_WarrantyUntil';
END
GO

-- Index cho IsActive
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Warranty_IsActive' AND object_id = OBJECT_ID(N'[dbo].[Warranty]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Warranty_IsActive]
    ON [dbo].[Warranty]([IsActive] ASC);
    
    PRINT 'Đã tạo Index IX_Warranty_IsActive';
END
GO

-- Index cho DeviceId
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Warranty_DeviceId' AND object_id = OBJECT_ID(N'[dbo].[Warranty]'))
BEGIN
    CREATE NONCLUSTERED INDEX [IX_Warranty_DeviceId]
    ON [dbo].[Warranty]([DeviceId] ASC)
    WHERE [DeviceId] IS NOT NULL;
    
    PRINT 'Đã tạo Index IX_Warranty_DeviceId';
END
GO

-- =============================================
-- 5. Migration dữ liệu: Cập nhật DeviceId từ quan hệ ngược
--    (Nếu Device có WarrantyId, cập nhật ngược lại DeviceId trong Warranty)
-- =============================================

-- Cập nhật DeviceId từ quan hệ ngược (Device.WarrantyId -> Warranty.Id)
-- Chỉ cập nhật nếu DeviceId chưa có giá trị
UPDATE w
SET w.[DeviceId] = d.[Id]
FROM [dbo].[Warranty] w
INNER JOIN [dbo].[Device] d ON d.[WarrantyId] = w.[Id]
WHERE w.[DeviceId] IS NULL;

IF @@ROWCOUNT > 0
BEGIN
    PRINT 'Đã cập nhật DeviceId cho ' + CAST(@@ROWCOUNT AS NVARCHAR(10)) + ' warranty từ quan hệ Device.WarrantyId';
END
GO

-- Cập nhật CreatedDate cho các record chưa có giá trị
UPDATE [dbo].[Warranty]
SET [CreatedDate] = GETDATE()
WHERE [CreatedDate] IS NULL;
GO

-- Cập nhật IsActive = 1 cho các record chưa có giá trị
UPDATE [dbo].[Warranty]
SET [IsActive] = 1
WHERE [IsActive] IS NULL;
GO

-- =============================================
-- 6. Tạo Trigger để tự động cập nhật UpdatedDate
--    (tương tự pattern của Device nếu có)
-- =============================================

IF EXISTS (SELECT * FROM sys.triggers WHERE name = 'TR_Warranty_UpdateDate')
BEGIN
    DROP TRIGGER [dbo].[TR_Warranty_UpdateDate];
END
GO

CREATE TRIGGER [dbo].[TR_Warranty_UpdateDate]
ON [dbo].[Warranty]
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE [dbo].[Warranty]
    SET [UpdatedDate] = GETDATE()
    FROM [dbo].[Warranty] w
    INNER JOIN inserted i ON w.Id = i.Id
    WHERE w.[UpdatedDate] IS NULL OR w.[UpdatedDate] < i.[UpdatedDate];
END
GO

PRINT 'Đã tạo Trigger TR_Warranty_UpdateDate';
GO

-- =============================================
-- 7. Tạo View để xem thông tin Warranty kèm Device
-- =============================================

IF EXISTS (SELECT * FROM sys.views WHERE name = 'vw_WarrantyWithDevice')
BEGIN
    DROP VIEW [dbo].[vw_WarrantyWithDevice];
END
GO

CREATE VIEW [dbo].[vw_WarrantyWithDevice]
AS
SELECT 
    w.[Id],
    w.[DeviceId],
    w.[WarrantyType],
    w.[WarrantyFrom],
    w.[MonthOfWarranty],
    w.[WarrantyUntil],
    w.[WarrantyStatus],
    w.[Notes],
    w.[IsActive],
    w.[CreatedDate],
    w.[UpdatedDate],
    w.[CreatedBy],
    w.[UpdatedBy],
    -- Thông tin Device
    d.[Id] AS DeviceId_Full,
    d.[SerialNumber] AS DeviceSerialNumber,
    d.[MACAddress] AS DeviceMACAddress,
    d.[IMEI] AS DeviceIMEI,
    d.[AssetTag] AS DeviceAssetTag,
    d.[Status] AS DeviceStatus,
    d.[DeviceType] AS DeviceType,
    -- Thông tin ProductVariant
    pv.[Name] AS ProductVariantName,
    pv.[Code] AS ProductVariantCode,
    -- Thông tin người tạo
    cu.[UserName] AS CreatedByUserName,
    -- Thông tin người cập nhật
    uu.[UserName] AS UpdatedByUserName
FROM [dbo].[Warranty] w
LEFT JOIN [dbo].[Device] d ON w.[DeviceId] = d.[Id]
LEFT JOIN [dbo].[ProductVariant] pv ON d.[ProductVariantId] = pv.[Id]
LEFT JOIN [dbo].[ApplicationUser] cu ON w.[CreatedBy] = cu.[Id]
LEFT JOIN [dbo].[ApplicationUser] uu ON w.[UpdatedBy] = uu.[Id]
WHERE w.[IsActive] = 1;
GO

PRINT 'Đã tạo View vw_WarrantyWithDevice';
GO

-- =============================================
-- 8. Tạo Stored Procedure để tự động tính WarrantyUntil
--    dựa trên WarrantyFrom và MonthOfWarranty
-- =============================================

IF EXISTS (SELECT * FROM sys.procedures WHERE name = 'sp_UpdateWarrantyUntil')
BEGIN
    DROP PROCEDURE [dbo].[sp_UpdateWarrantyUntil];
END
GO

CREATE PROCEDURE [dbo].[sp_UpdateWarrantyUntil]
    @WarrantyId UNIQUEIDENTIFIER = NULL
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Cập nhật WarrantyUntil cho một warranty cụ thể hoặc tất cả
    UPDATE [dbo].[Warranty]
    SET [WarrantyUntil] = DATEADD(MONTH, [MonthOfWarranty], [WarrantyFrom])
    WHERE 
        ([WarrantyFrom] IS NOT NULL)
        AND ([MonthOfWarranty] > 0)
        AND ([WarrantyUntil] IS NULL OR [WarrantyUntil] <> DATEADD(MONTH, [MonthOfWarranty], [WarrantyFrom]))
        AND (@WarrantyId IS NULL OR [Id] = @WarrantyId);
    
    RETURN @@ROWCOUNT;
END
GO

PRINT 'Đã tạo Stored Procedure sp_UpdateWarrantyUntil';
GO

-- =============================================
-- 9. Chạy Stored Procedure để cập nhật WarrantyUntil cho dữ liệu hiện có
-- =============================================

EXEC [dbo].[sp_UpdateWarrantyUntil];
GO

PRINT 'Đã cập nhật WarrantyUntil cho tất cả warranty';
GO

-- =============================================
-- Hoàn thành
-- =============================================
PRINT '=============================================';
PRINT 'Đã hoàn thành điều chỉnh bảng Warranty';
PRINT 'Các thay đổi:';
PRINT '  - Thêm các trường audit: IsActive, CreatedDate, UpdatedDate, CreatedBy, UpdatedBy';
PRINT '  - Thêm trường Notes';
PRINT '  - Xóa các cột: StockInOutDetailId, UniqueProductInfo, WarrantyProvider, WarrantyNumber, WarrantyContact';
PRINT '  - Thêm cột DeviceId làm Foreign Key đến bảng Device';
PRINT '  - Tạo các Index để tối ưu truy vấn';
PRINT '  - Tạo Trigger tự động cập nhật UpdatedDate';
PRINT '  - Tạo View vw_WarrantyWithDevice';
PRINT '  - Tạo Stored Procedure sp_UpdateWarrantyUntil';
PRINT '';
PRINT 'Cấu trúc bảng Warranty sau khi điều chỉnh:';
PRINT '  - Id (PK)';
PRINT '  - WarrantyType';
PRINT '  - WarrantyFrom';
PRINT '  - MonthOfWarranty';
PRINT '  - WarrantyUntil';
PRINT '  - WarrantyStatus';
PRINT '  - IsActive';
PRINT '  - CreatedDate';
PRINT '  - UpdatedDate';
PRINT '  - CreatedBy';
PRINT '  - UpdatedBy';
PRINT '  - Notes';
PRINT '  - DeviceId (FK -> Device)';
PRINT '=============================================';
GO

