-- =============================================
-- Script tạo bảng lịch sử giao dịch thiết bị (Device Transaction History)
-- Database: VnsErp2025Final
-- Ngày tạo: 2025
-- Mô tả: Bảng để ghi lại lịch sử nhập, xuất, cấp phát, thu hồi và các thao tác khác của thiết bị
--        Bao gồm 2 cột: Information (thông tin text) và HtmlInformation (thông tin HTML)
-- =============================================

USE [VnsErp2025Final]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

BEGIN TRANSACTION
BEGIN TRY

    PRINT 'Bắt đầu tạo bảng DeviceTransactionHistory...'
    
    -- =============================================
    -- Bảng DeviceTransactionHistory (Lịch sử giao dịch thiết bị)
    -- =============================================
    IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DeviceTransactionHistory]') AND type in (N'U'))
    BEGIN
        CREATE TABLE [dbo].[DeviceTransactionHistory](
            [Id] [uniqueidentifier] NOT NULL,
            [DeviceId] [uniqueidentifier] NOT NULL, -- Thiết bị được thao tác
            
            -- Thông tin thao tác
            [OperationType] [int] NOT NULL, -- Loại thao tác: 0=Import/Nhập, 1=Export/Xuất, 2=Allocation/Cấp phát, 3=Recovery/Thu hồi, 4=Transfer/Chuyển giao, 5=Maintenance/Bảo trì, 6=StatusChange/Đổi trạng thái, 7=Other/Khác
            [OperationDate] [datetime] NOT NULL, -- Ngày thực hiện thao tác
            
            -- Thông tin tham chiếu
            [ReferenceId] [uniqueidentifier] NULL, -- ID tham chiếu (ví dụ: StockInOutDetailId, DeviceTransferId, v.v.)
            [ReferenceType] [int] NULL, -- Loại tham chiếu: 0=StockInOutDetail, 1=DeviceTransfer, 2=Warranty, 3=Other
            
            -- Thông tin chi tiết (2 cột theo yêu cầu)
            [Information] [nvarchar](max) NULL, -- Thông tin dạng text (mô tả chi tiết thao tác)
            [HtmlInformation] [nvarchar](max) NULL, -- Thông tin dạng HTML (để hiển thị đẹp trên UI)
            
            -- Thông tin người thực hiện
            [PerformedBy] [uniqueidentifier] NULL, -- Người thực hiện thao tác (EmployeeId hoặc ApplicationUserId)
            
            -- Ghi chú
            [Notes] [nvarchar](1000) NULL, -- Ghi chú bổ sung
            
            -- Audit fields
            [CreatedDate] [datetime] NOT NULL DEFAULT(GETDATE()),
            [CreatedBy] [uniqueidentifier] NULL,
            
            CONSTRAINT [PK_DeviceTransactionHistory] PRIMARY KEY CLUSTERED ([Id] ASC),
            CONSTRAINT [FK_DeviceTransactionHistory_Device] FOREIGN KEY([DeviceId])
                REFERENCES [dbo].[Device] ([Id]) ON DELETE CASCADE
        )
        
        -- Index cho DeviceId và OperationDate (để query lịch sử theo thiết bị)
        CREATE NONCLUSTERED INDEX [IX_DeviceTransactionHistory_DeviceId_OperationDate] 
            ON [dbo].[DeviceTransactionHistory] ([DeviceId] ASC, [OperationDate] DESC)
        
        -- Index cho OperationType (để filter theo loại thao tác)
        CREATE NONCLUSTERED INDEX [IX_DeviceTransactionHistory_OperationType] 
            ON [dbo].[DeviceTransactionHistory] ([OperationType] ASC)
        
        -- Index cho ReferenceId và ReferenceType (để query theo tham chiếu)
        CREATE NONCLUSTERED INDEX [IX_DeviceTransactionHistory_Reference] 
            ON [dbo].[DeviceTransactionHistory] ([ReferenceId] ASC, [ReferenceType] ASC) 
            WHERE [ReferenceId] IS NOT NULL
        
        -- Index cho PerformedBy (để query theo người thực hiện)
        CREATE NONCLUSTERED INDEX [IX_DeviceTransactionHistory_PerformedBy] 
            ON [dbo].[DeviceTransactionHistory] ([PerformedBy] ASC) 
            WHERE [PerformedBy] IS NOT NULL
        
        PRINT 'Bảng DeviceTransactionHistory đã được tạo thành công.'
    END
    ELSE
    BEGIN
        PRINT 'Bảng DeviceTransactionHistory đã tồn tại.'
    END
    
    COMMIT TRANSACTION
    PRINT 'Hoàn tất tạo bảng DeviceTransactionHistory.'
    
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0
    BEGIN
        ROLLBACK TRANSACTION
        PRINT 'Tạo bảng thất bại! Đã rollback.'
    END
    
    DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE()
    DECLARE @ErrorSeverity INT = ERROR_SEVERITY()
    DECLARE @ErrorState INT = ERROR_STATE()
    
    PRINT 'Lỗi: ' + @ErrorMessage
    RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState)
END CATCH
GO

PRINT 'Script hoàn tất.'
GO
