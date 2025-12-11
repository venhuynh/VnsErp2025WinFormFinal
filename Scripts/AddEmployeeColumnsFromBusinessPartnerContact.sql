-- =============================================
-- Script: Thêm các cột từ BusinessPartnerContact vào bảng Employee
-- Mô tả: Điều chỉnh bảng Employee có các cột tương tự BusinessPartnerContact
-- Ngày tạo: 2025
-- =============================================

USE [VnsErp2025Final]
GO

-- Kiểm tra và thêm cột Mobile
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'Mobile')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [Mobile] NVARCHAR(50) NULL;
    PRINT 'Đã thêm cột Mobile vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột Mobile đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột Fax
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'Fax')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [Fax] NVARCHAR(50) NULL;
    PRINT 'Đã thêm cột Fax vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột Fax đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột LinkedIn
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'LinkedIn')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [LinkedIn] NVARCHAR(255) NULL;
    PRINT 'Đã thêm cột LinkedIn vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột LinkedIn đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột Skype
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'Skype')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [Skype] NVARCHAR(100) NULL;
    PRINT 'Đã thêm cột Skype vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột Skype đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột WeChat
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'WeChat')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [WeChat] NVARCHAR(100) NULL;
    PRINT 'Đã thêm cột WeChat vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột WeChat đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột Notes
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'Notes')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [Notes] NVARCHAR(1000) NULL;
    PRINT 'Đã thêm cột Notes vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột Notes đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột CreatedDate
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'CreatedDate')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE();
    PRINT 'Đã thêm cột CreatedDate vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột CreatedDate đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột ModifiedDate
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'ModifiedDate')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [ModifiedDate] DATETIME NULL;
    PRINT 'Đã thêm cột ModifiedDate vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột ModifiedDate đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột AvatarFileName
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'AvatarFileName')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [AvatarFileName] NVARCHAR(255) NULL;
    PRINT 'Đã thêm cột AvatarFileName vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột AvatarFileName đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột AvatarRelativePath
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'AvatarRelativePath')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [AvatarRelativePath] NVARCHAR(500) NULL;
    PRINT 'Đã thêm cột AvatarRelativePath vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột AvatarRelativePath đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột AvatarFullPath
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'AvatarFullPath')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [AvatarFullPath] NVARCHAR(1000) NULL;
    PRINT 'Đã thêm cột AvatarFullPath vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột AvatarFullPath đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột AvatarStorageType
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'AvatarStorageType')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [AvatarStorageType] NVARCHAR(20) NULL;
    PRINT 'Đã thêm cột AvatarStorageType vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột AvatarStorageType đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột AvatarFileSize
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'AvatarFileSize')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [AvatarFileSize] BIGINT NULL;
    PRINT 'Đã thêm cột AvatarFileSize vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột AvatarFileSize đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột AvatarChecksum
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'AvatarChecksum')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [AvatarChecksum] NVARCHAR(64) NULL;
    PRINT 'Đã thêm cột AvatarChecksum vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột AvatarChecksum đã tồn tại trong bảng Employee';
END
GO

-- Kiểm tra và thêm cột AvatarThumbnailData
IF NOT EXISTS (SELECT * FROM sys.columns WHERE object_id = OBJECT_ID(N'[dbo].[Employee]') AND name = 'AvatarThumbnailData')
BEGIN
    ALTER TABLE [dbo].[Employee]
    ADD [AvatarThumbnailData] VARBINARY(MAX) NULL;
    PRINT 'Đã thêm cột AvatarThumbnailData vào bảng Employee';
END
ELSE
BEGIN
    PRINT 'Cột AvatarThumbnailData đã tồn tại trong bảng Employee';
END
GO

-- =============================================
-- Tùy chọn: Migrate dữ liệu từ AvatarPath sang AvatarRelativePath (nếu cần)
-- =============================================
/*
-- Nếu muốn migrate dữ liệu từ AvatarPath (cũ) sang AvatarRelativePath (mới)
-- Chỉ chạy script này nếu AvatarPath có dữ liệu và muốn chuyển sang AvatarRelativePath
UPDATE [dbo].[Employee]
SET [AvatarRelativePath] = [AvatarPath]
WHERE [AvatarPath] IS NOT NULL 
  AND [AvatarRelativePath] IS NULL;
GO
*/

PRINT 'Hoàn thành: Đã thêm tất cả các cột từ BusinessPartnerContact vào bảng Employee';
GO

