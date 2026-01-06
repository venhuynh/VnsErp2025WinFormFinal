-- =============================================
-- Script tạo bảng quản lý định danh cho ProductVariant
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Bảng ProductVariantIdentifier để quản lý các loại định danh cho ProductVariant
--        Tương tự như bảng Device nhưng quản lý rộng hơn, không chỉ giới hạn cho thiết bị
--        Hỗ trợ nhiều loại định danh: SerialNumber, Barcode, QRCode, SKU, RFID, MAC, IMEI, AssetTag, LicenseKey, v.v.
-- =============================================

USE [VnsErp2025Final]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

BEGIN TRANSACTION
BEGIN TRY

    PRINT 'Bắt đầu tạo bảng ProductVariantIdentifier...'
    
    -- =============================================
    -- Bảng ProductVariantIdentifier (Định danh ProductVariant)
    -- =============================================
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantIdentifier]') AND type in (N'U'))
    BEGIN
        CREATE TABLE [dbo].[ProductVariantIdentifier](
            [Id] [uniqueidentifier] NOT NULL,
            [ProductVariantId] [uniqueidentifier] NOT NULL, -- Biến thể sản phẩm cần định danh
            
            -- Thông tin định danh (từng loại định danh được khai báo riêng)
            [SerialNumber] [nvarchar](100) NULL, -- Số serial
            [Barcode] [nvarchar](255) NULL, -- Mã vạch
            [QRCode] [nvarchar](500) NULL, -- Mã QR
            [QRCodeImagePath] [nvarchar](500) NULL, -- Đường dẫn hình ảnh QR code (relative path)
            [QRCodeImageFullPath] [nvarchar](1000) NULL, -- Đường dẫn đầy đủ hình ảnh QR code (full UNC path)
            [QRCodeImageFileName] [nvarchar](255) NULL, -- Tên file hình ảnh QR code
            [QRCodeImageStorageType] [nvarchar](20) NULL DEFAULT('NAS'), -- Loại lưu trữ: NAS, Local, Cloud
            [QRCodeImageLocked] [bit] NOT NULL DEFAULT(0), -- Khóa hình ảnh QR code (không cho chỉnh sửa/xóa)
            [QRCodeImageLockedDate] [datetime] NULL, -- Ngày khóa hình ảnh
            [QRCodeImageLockedBy] [uniqueidentifier] NULL, -- Người khóa hình ảnh
            [SKU] [nvarchar](100) NULL, -- Stock Keeping Unit (mã sản phẩm)
            [RFID] [nvarchar](100) NULL, -- Radio Frequency Identification
            [MACAddress] [nvarchar](50) NULL, -- Media Access Control Address
            [IMEI] [nvarchar](50) NULL, -- International Mobile Equipment Identity
            [AssetTag] [nvarchar](50) NULL, -- Mã tài sản nội bộ
            [LicenseKey] [nvarchar](255) NULL, -- Khóa bản quyền (cho phần mềm)
            [UPC] [nvarchar](50) NULL, -- Universal Product Code
            [EAN] [nvarchar](50) NULL, -- European Article Number
            [ISBN] [nvarchar](50) NULL, -- International Standard Book Number
            [OtherIdentifier] [nvarchar](255) NULL, -- Loại định danh khác
            
            -- Thông tin bổ sung
            [IsActive] [bit] NOT NULL DEFAULT(1), -- Còn sử dụng không
            
            -- Thông tin tình trạng hàng hóa/sản phẩm
            [Status] [int] NOT NULL DEFAULT(0), -- Tình trạng: 0=Tại kho VNS, 1=Đã xuất cho KH, 2=Đang lắp đặt tại site KH, 3=Đang gửi Bảo hành NCC, 4=Đã hư hỏng (Tại kho VNS), 5=Đã thanh lý
            [StatusDate] [datetime] NULL, -- Ngày thay đổi trạng thái
            [StatusChangedBy] [uniqueidentifier] NULL, -- Người thay đổi trạng thái
            [StatusNotes] [nvarchar](1000) NULL, -- Ghi chú về trạng thái
            
            -- Thông tin nguồn gốc (nếu có)
            [SourceType] [int] NULL, -- Nguồn: 0=Manual, 1=Import, 2=AutoGenerate, 3=Scanner, 4=Other
            [SourceReference] [nvarchar](255) NULL, -- Tham chiếu nguồn (ví dụ: file import, device scanner, v.v.)
            
            -- Thông tin ngày tháng
            [ValidFrom] [datetime] NULL, -- Ngày bắt đầu có hiệu lực
            [ValidTo] [datetime] NULL, -- Ngày hết hiệu lực (nếu có)
            
            -- Ghi chú
            [Notes] [nvarchar](1000) NULL, -- Ghi chú bổ sung
            
            -- Audit fields
            [CreatedDate] [datetime] NOT NULL DEFAULT(GETDATE()),
            [UpdatedDate] [datetime] NULL,
            [CreatedBy] [uniqueidentifier] NULL,
            [UpdatedBy] [uniqueidentifier] NULL,
            
            CONSTRAINT [PK_ProductVariantIdentifier] PRIMARY KEY CLUSTERED ([Id] ASC),
            CONSTRAINT [FK_ProductVariantIdentifier_ProductVariant] FOREIGN KEY([ProductVariantId])
                REFERENCES [dbo].[ProductVariant] ([Id]) ON DELETE CASCADE
        )
        
        -- Index cho ProductVariantId
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_ProductVariantId] 
            ON [dbo].[ProductVariantIdentifier] ([ProductVariantId] ASC, [IsActive] ASC)
        
        -- Unique Index cho SerialNumber
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_SerialNumber] 
            ON [dbo].[ProductVariantIdentifier] ([SerialNumber] ASC) 
            WHERE [SerialNumber] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho Barcode
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_Barcode] 
            ON [dbo].[ProductVariantIdentifier] ([Barcode] ASC) 
            WHERE [Barcode] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho QRCode
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_QRCode] 
            ON [dbo].[ProductVariantIdentifier] ([QRCode] ASC) 
            WHERE [QRCode] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho SKU
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_SKU] 
            ON [dbo].[ProductVariantIdentifier] ([SKU] ASC) 
            WHERE [SKU] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho RFID
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_RFID] 
            ON [dbo].[ProductVariantIdentifier] ([RFID] ASC) 
            WHERE [RFID] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho MACAddress
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_MACAddress] 
            ON [dbo].[ProductVariantIdentifier] ([MACAddress] ASC) 
            WHERE [MACAddress] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho IMEI
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_IMEI] 
            ON [dbo].[ProductVariantIdentifier] ([IMEI] ASC) 
            WHERE [IMEI] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho AssetTag
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_AssetTag] 
            ON [dbo].[ProductVariantIdentifier] ([AssetTag] ASC) 
            WHERE [AssetTag] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho LicenseKey
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_LicenseKey] 
            ON [dbo].[ProductVariantIdentifier] ([LicenseKey] ASC) 
            WHERE [LicenseKey] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho UPC
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_UPC] 
            ON [dbo].[ProductVariantIdentifier] ([UPC] ASC) 
            WHERE [UPC] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho EAN
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_EAN] 
            ON [dbo].[ProductVariantIdentifier] ([EAN] ASC) 
            WHERE [EAN] IS NOT NULL AND [IsActive] = 1
        
        -- Unique Index cho ISBN
        CREATE UNIQUE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_ISBN] 
            ON [dbo].[ProductVariantIdentifier] ([ISBN] ASC) 
            WHERE [ISBN] IS NOT NULL AND [IsActive] = 1
        
        -- Index cho OtherIdentifier (không unique vì có thể trùng)
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_OtherIdentifier] 
            ON [dbo].[ProductVariantIdentifier] ([OtherIdentifier] ASC) 
            WHERE [OtherIdentifier] IS NOT NULL AND [IsActive] = 1
        
        -- Index cho ValidFrom và ValidTo (để query theo thời gian hiệu lực)
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_ValidDate] 
            ON [dbo].[ProductVariantIdentifier] ([ValidFrom] ASC, [ValidTo] ASC) 
            WHERE [IsActive] = 1
        
        -- Index cho QRCodeImagePath
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_QRCodeImagePath] 
            ON [dbo].[ProductVariantIdentifier] ([QRCodeImagePath] ASC) 
            WHERE [QRCodeImagePath] IS NOT NULL
        
        -- Index cho QRCodeImageLocked
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_QRCodeImageLocked] 
            ON [dbo].[ProductVariantIdentifier] ([QRCodeImageLocked] ASC) 
            WHERE [QRCodeImageLocked] = 1
        
        -- Index cho Status (để query theo tình trạng)
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_Status] 
            ON [dbo].[ProductVariantIdentifier] ([Status] ASC, [IsActive] ASC)
        
        -- Index cho StatusDate (để query theo ngày thay đổi trạng thái)
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifier_StatusDate] 
            ON [dbo].[ProductVariantIdentifier] ([StatusDate] DESC) 
            WHERE [StatusDate] IS NOT NULL
        
        -- Thêm Extended Properties để mô tả bảng
        EXEC sys.sp_addextendedproperty 
            @name = N'MS_Description', 
            @value = N'Bảng quản lý các loại định danh cho ProductVariant. Mỗi loại định danh được khai báo thành cột riêng: SerialNumber, Barcode, QRCode, SKU, RFID, MACAddress, IMEI, AssetTag, LicenseKey, UPC, EAN, ISBN, OtherIdentifier. Tương tự như bảng Device nhưng quản lý rộng hơn, không chỉ giới hạn cho thiết bị. Mỗi ProductVariant có thể có một bản ghi với nhiều loại định danh khác nhau.', 
            @level0type = N'SCHEMA', @level0name = N'dbo', 
            @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier'
        
        -- Thêm Extended Properties cho các cột định danh
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Số serial của sản phẩm', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'SerialNumber'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Mã vạch của sản phẩm', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'Barcode'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Mã QR của sản phẩm', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'QRCode'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Stock Keeping Unit - Mã sản phẩm', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'SKU'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Radio Frequency Identification - Mã RFID', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'RFID'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Media Access Control Address - Địa chỉ MAC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'MACAddress'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'International Mobile Equipment Identity - Số IMEI', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'IMEI'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Mã tài sản nội bộ', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'AssetTag'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Khóa bản quyền (cho phần mềm)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'LicenseKey'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Universal Product Code - Mã UPC', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'UPC'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'European Article Number - Mã EAN', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'EAN'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'International Standard Book Number - Mã ISBN', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'ISBN'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Loại định danh khác (không thuộc các loại trên)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'OtherIdentifier'
        
        -- Thêm Extended Properties cho các cột quản lý hình ảnh QR code
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Đường dẫn tương đối hình ảnh QR code (relative path trên NAS)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'QRCodeImagePath'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Đường dẫn đầy đủ hình ảnh QR code (full UNC path)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'QRCodeImageFullPath'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Tên file hình ảnh QR code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'QRCodeImageFileName'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Loại lưu trữ hình ảnh: NAS, Local, Cloud', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'QRCodeImageStorageType'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Khóa hình ảnh QR code (không cho chỉnh sửa/xóa khi = 1)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'QRCodeImageLocked'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Ngày khóa hình ảnh QR code', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'QRCodeImageLockedDate'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Người khóa hình ảnh QR code (EmployeeId hoặc ApplicationUserId)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'QRCodeImageLockedBy'
        
        -- Thêm Extended Properties cho các cột quản lý tình trạng
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Tình trạng hàng hóa/sản phẩm: 0=Tại kho VNS, 1=Đã xuất cho KH, 2=Đang lắp đặt tại site KH, 3=Đang gửi Bảo hành NCC, 4=Đã hư hỏng (Tại kho VNS), 5=Đã thanh lý', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'Status'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Ngày thay đổi trạng thái hàng hóa/sản phẩm', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'StatusDate'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Người thay đổi trạng thái (EmployeeId hoặc ApplicationUserId)', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'StatusChangedBy'
        
        EXEC sys.sp_addextendedproperty @name = N'MS_Description', @value = N'Ghi chú về trạng thái hàng hóa/sản phẩm', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'ProductVariantIdentifier', @level2type = N'COLUMN', @level2name = N'StatusNotes'
        
        PRINT 'Bảng ProductVariantIdentifier đã được tạo thành công.'
    END
    ELSE
    BEGIN
        PRINT 'Bảng ProductVariantIdentifier đã tồn tại.'
    END

    -- =============================================
    -- Bảng ProductVariantIdentifierHistory (Lịch sử thay đổi định danh)
    -- =============================================
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ProductVariantIdentifierHistory]') AND type in (N'U'))
    BEGIN
        CREATE TABLE [dbo].[ProductVariantIdentifierHistory](
            [Id] [uniqueidentifier] NOT NULL,
            [ProductVariantIdentifierId] [uniqueidentifier] NOT NULL, -- Định danh được thay đổi
            [ProductVariantId] [uniqueidentifier] NOT NULL, -- ProductVariant (để query nhanh)
            
            -- Thông tin thay đổi
            [ChangeType] [int] NOT NULL, -- Loại thay đổi: 0=Created, 1=Updated, 2=Activated, 3=Deactivated, 4=Deleted
            [ChangeDate] [datetime] NOT NULL,
            [ChangedBy] [uniqueidentifier] NULL,
            
            -- Giá trị cũ và mới
            [OldValue] [nvarchar](500) NULL, -- Giá trị cũ
            [NewValue] [nvarchar](500) NULL, -- Giá trị mới
            [FieldName] [nvarchar](100) NULL, -- Tên trường thay đổi
            
            -- Mô tả
            [Description] [nvarchar](1000) NULL, -- Mô tả thay đổi
            [Notes] [nvarchar](1000) NULL, -- Ghi chú bổ sung
            
            CONSTRAINT [PK_ProductVariantIdentifierHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
            CONSTRAINT [FK_ProductVariantIdentifierHistory_ProductVariantIdentifier] FOREIGN KEY([ProductVariantIdentifierId])
                REFERENCES [dbo].[ProductVariantIdentifier] ([Id]) ON DELETE CASCADE,
            CONSTRAINT [FK_ProductVariantIdentifierHistory_ProductVariant] FOREIGN KEY([ProductVariantId])
                REFERENCES [dbo].[ProductVariant] ([Id])
        )
        
        -- Index cho ProductVariantIdentifierId
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifierHistory_ProductVariantIdentifierId] 
            ON [dbo].[ProductVariantIdentifierHistory] ([ProductVariantIdentifierId] ASC, [ChangeDate] DESC)
        
        -- Index cho ProductVariantId (để query lịch sử theo ProductVariant)
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifierHistory_ProductVariantId] 
            ON [dbo].[ProductVariantIdentifierHistory] ([ProductVariantId] ASC, [ChangeDate] DESC)
        
        -- Index cho ChangeType
        CREATE NONCLUSTERED INDEX [IX_ProductVariantIdentifierHistory_ChangeType] 
            ON [dbo].[ProductVariantIdentifierHistory] ([ChangeType] ASC)
        
        -- Thêm Extended Properties
        EXEC sys.sp_addextendedproperty 
            @name = N'MS_Description', 
            @value = N'Bảng lưu trữ lịch sử thay đổi của các định danh ProductVariant', 
            @level0type = N'SCHEMA', @level0name = N'dbo', 
            @level1type = N'TABLE', @level1name = N'ProductVariantIdentifierHistory'
        
        PRINT 'Bảng ProductVariantIdentifierHistory đã được tạo thành công.'
    END
    ELSE
    BEGIN
        PRINT 'Bảng ProductVariantIdentifierHistory đã tồn tại.'
    END

    COMMIT TRANSACTION
    PRINT 'Hoàn tất tạo các bảng quản lý định danh ProductVariant.'
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION
    
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
    DECLARE @ErrorState INT = ERROR_STATE()
    
    PRINT 'Lỗi khi tạo bảng: ' + @ErrorMessage
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
GO

-- =============================================
-- Ghi chú về các cột định danh:
-- =============================================
-- SerialNumber: Số serial của sản phẩm
-- Barcode: Mã vạch của sản phẩm
-- QRCode: Mã QR của sản phẩm
-- SKU: Stock Keeping Unit (mã sản phẩm)
-- RFID: Radio Frequency Identification
-- MACAddress: Media Access Control Address (địa chỉ MAC)
-- IMEI: International Mobile Equipment Identity
-- AssetTag: Mã tài sản nội bộ
-- LicenseKey: Khóa bản quyền (cho phần mềm)
-- UPC: Universal Product Code
-- EAN: European Article Number
-- ISBN: International Standard Book Number
-- OtherIdentifier: Loại định danh khác
-- =============================================
-- Lưu ý: Tất cả các cột định danh đều có unique index (trừ OtherIdentifier)
-- để đảm bảo tính duy nhất của từng loại định danh
-- =============================================

-- =============================================
-- Ghi chú về quản lý hình ảnh QR code:
-- =============================================
-- QRCodeImagePath: Đường dẫn tương đối hình ảnh QR code (relative path trên NAS)
-- QRCodeImageFullPath: Đường dẫn đầy đủ hình ảnh QR code (full UNC path)
-- QRCodeImageFileName: Tên file hình ảnh QR code
-- QRCodeImageStorageType: Loại lưu trữ (NAS, Local, Cloud) - mặc định là NAS
-- QRCodeImageLocked: Khóa hình ảnh (bit) - khi = 1 thì không cho chỉnh sửa/xóa
-- QRCodeImageLockedDate: Ngày khóa hình ảnh
-- QRCodeImageLockedBy: Người khóa hình ảnh (EmployeeId hoặc ApplicationUserId)
-- =============================================
-- Lưu ý: Khi QRCodeImageLocked = 1, hệ thống sẽ không cho phép:
-- - Xóa hình ảnh
-- - Thay đổi đường dẫn hình ảnh
-- - Cập nhật QRCodeImagePath, QRCodeImageFullPath, QRCodeImageFileName
-- =============================================

-- =============================================
-- Ghi chú về SourceType:
-- =============================================
-- 0 = Manual: Nhập thủ công
-- 1 = Import: Import từ file
-- 2 = AutoGenerate: Tự động sinh
-- 3 = Scanner: Quét từ thiết bị
-- 4 = Other: Nguồn khác
-- =============================================

-- =============================================
-- Ghi chú về ChangeType (trong History):
-- =============================================
-- 0 = Created: Tạo mới
-- 1 = Updated: Cập nhật
-- 2 = Activated: Kích hoạt
-- 3 = Deactivated: Vô hiệu hóa
-- 4 = Deleted: Xóa
-- =============================================

-- =============================================
-- Ghi chú về Status (Tình trạng hàng hóa/sản phẩm):
-- =============================================
-- 0 = Tại kho VNS: Hàng hóa đang ở kho VNS
-- 1 = Đã xuất cho KH: Đã xuất hàng cho khách hàng
-- 2 = Đang lắp đặt tại site KH: Đang được lắp đặt tại công trường/site của khách hàng
-- 3 = Đang gửi Bảo hành NCC: Đang gửi đi bảo hành tại nhà cung cấp
-- 4 = Đã hư hỏng (Tại kho VNS): Hàng hóa đã bị hư hỏng và đang ở kho VNS
-- 5 = Đã thanh lý: Hàng hóa đã được thanh lý
-- =============================================
-- Lưu ý: 
-- - Status mặc định = 0 (Tại kho VNS) khi tạo mới
-- - Mỗi lần thay đổi Status nên cập nhật StatusDate và StatusChangedBy
-- - Có thể ghi chú thêm vào StatusNotes khi thay đổi trạng thái
-- =============================================
