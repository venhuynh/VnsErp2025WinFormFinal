-- Script để thay đổi cột PartnerSiteId từ NOT NULL sang cho phép NULL
-- Lý do: Nhập xuất hàng nội bộ hoặc lưu chuyển kho không cần thông tin khách hàng

USE [VnsErp2025Final]
GO

-- Thay đổi cột PartnerSiteId cho phép NULL
ALTER TABLE [dbo].[StockInOutMaster]
ALTER COLUMN [PartnerSiteId] [uniqueidentifier] NULL;
GO

-- Lưu ý: Foreign key constraint FK_StockInOutMaster_BusinessPartnerSite 
-- vẫn hoạt động bình thường với giá trị NULL
-- Không cần thay đổi constraint vì SQL Server cho phép NULL trong foreign key

