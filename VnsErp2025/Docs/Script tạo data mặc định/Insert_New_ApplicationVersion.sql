-- =============================================
-- Script: Insert phiên bản mới vào bảng VnsErpApplicationVersion
-- Mục đích: Thêm phiên bản mới và tự động set các phiên bản cũ thành IsActive = false
-- Lưu ý: Chỉ có một phiên bản Active tại một thời điểm
-- 
-- ⚠️ QUAN TRỌNG: Script này được cập nhật tự động từ AssemblyInfo.cs
-- Phiên bản hiện tại: 2.0.0.0 (từ AssemblyVersion trong AssemblyInfo.cs)
-- Vui lòng cập nhật @Description và @ReleaseNote với thông tin chi tiết về các thay đổi
-- =============================================

USE [VnsErp2025Final];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

BEGIN TRANSACTION;
GO

PRINT '========================================';
PRINT 'Bắt đầu insert phiên bản mới vào bảng VnsErpApplicationVersion';
PRINT '========================================';
GO

-- Kiểm tra xem bảng VnsErpApplicationVersion có tồn tại không
IF NOT EXISTS (SELECT 1 FROM sys.tables WHERE name = 'VnsErpApplicationVersion' AND schema_id = SCHEMA_ID('dbo'))
BEGIN
    PRINT '❌ Bảng VnsErpApplicationVersion không tồn tại. Vui lòng chạy script Create_ApplicationVersion_And_AllowedMacAddress_Tables.sql và Rename_ApplicationVersion_To_VnsErpApplicationVersion.sql trước.';
    ROLLBACK TRANSACTION;
    RETURN;
END
GO

-- =============================================
-- CẤU HÌNH PHIÊN BẢN MỚI - ĐƯỢC CẬP NHẬT TỪ AssemblyInfo.cs
-- Phiên bản trong AssemblyInfo.cs: 2.0.0.0
-- =============================================

DECLARE @NewVersion NVARCHAR(50) = N'2.0.0.0';  -- ✅ Phiên bản từ AssemblyInfo.cs (AssemblyVersion: 2.0.0.0)
DECLARE @ReleaseDate DATETIME = GETDATE();      -- ⚠️ THAY ĐỔI: Ngày phát hành (hoặc dùng GETDATE() cho ngày hiện tại)
DECLARE @Description NVARCHAR(500) = N'Phiên bản 2.0.0.0 - Cập nhật mới';  -- ⚠️ THAY ĐỔI: Mô tả ngắn
DECLARE @ReleaseNote NVARCHAR(1000) = N'Chi tiết các thay đổi trong phiên bản 2.0.0.0:
- Cập nhật phiên bản lên 2.0.0.0
- Sửa lỗi và cải thiện hiệu suất
- Cập nhật các tính năng mới';  -- ⚠️ THAY ĐỔI: Ghi chú phát hành chi tiết
DECLARE @CreateBy UNIQUEIDENTIFIER = NULL;       -- ⚠️ THAY ĐỔI: ID người tạo (hoặc NULL)

-- =============================================
-- KIỂM TRA VÀ XỬ LÝ
-- =============================================

-- Kiểm tra xem phiên bản đã tồn tại chưa
IF EXISTS (SELECT 1 FROM [dbo].[VnsErpApplicationVersion] WHERE [Version] = @NewVersion)
BEGIN
    PRINT '⚠️  Phiên bản ' + @NewVersion + ' đã tồn tại trong bảng.';
    PRINT '   Vui lòng kiểm tra lại hoặc sử dụng phiên bản khác.';
    
    -- Hiển thị thông tin phiên bản hiện tại
    SELECT 
        [Version],
        [ReleaseDate],
        [IsActive],
        [Description]
    FROM [dbo].[VnsErpApplicationVersion]
    WHERE [Version] = @NewVersion;
    
    ROLLBACK TRANSACTION;
    RETURN;
END

-- Set tất cả các phiên bản cũ thành IsActive = false
UPDATE [dbo].[VnsErpApplicationVersion]
SET [IsActive] = 0,
    [ModifiedDate] = GETDATE(),
    [ModifiedBy] = @CreateBy
WHERE [IsActive] = 1;

DECLARE @UpdatedCount INT = @@ROWCOUNT;
IF @UpdatedCount > 0
BEGIN
    PRINT '✅ Đã set ' + CAST(@UpdatedCount AS NVARCHAR(10)) + ' phiên bản cũ thành IsActive = false';
END
ELSE
BEGIN
    PRINT 'ℹ️  Không có phiên bản nào đang Active (có thể đây là phiên bản đầu tiên)';
END

-- =============================================
-- INSERT PHIÊN BẢN MỚI
-- =============================================

INSERT INTO [dbo].[VnsErpApplicationVersion]
(
    [Id],
    [Version],
    [ReleaseDate],
    [IsActive],
    [Description],
    [ReleaseNote],
    [CreateDate],
    [CreateBy],
    [ModifiedDate],
    [ModifiedBy]
)
VALUES
(
    NEWID(),                    -- Id: Tự động tạo GUID mới
    @NewVersion,                -- Version: Phiên bản mới
    @ReleaseDate,               -- ReleaseDate: Ngày phát hành
    1,                          -- IsActive: true (phiên bản mới là Active)
    @Description,               -- Description: Mô tả ngắn
    @ReleaseNote,               -- ReleaseNote: Ghi chú phát hành chi tiết
    GETDATE(),                  -- CreateDate: Ngày tạo
    @CreateBy,                  -- CreateBy: ID người tạo (hoặc NULL)
    NULL,                       -- ModifiedDate: NULL (chưa sửa)
    NULL                        -- ModifiedBy: NULL (chưa sửa)
);

PRINT '✅ Đã insert phiên bản mới: ' + @NewVersion;
PRINT '   - ReleaseDate: ' + CONVERT(NVARCHAR(20), @ReleaseDate, 120);
PRINT '   - IsActive: true';
PRINT '   - Description: ' + @Description;
GO

-- =============================================
-- KIỂM TRA KẾT QUẢ
-- =============================================

PRINT '';
PRINT '========================================';
PRINT 'Danh sách tất cả các phiên bản:';
PRINT '========================================';

SELECT 
    [Version] AS [Phiên bản],
    [ReleaseDate] AS [Ngày phát hành],
    CASE 
        WHEN [IsActive] = 1 THEN N'✓ Active'
        ELSE N'✗ Inactive'
    END AS [Trạng thái],
    [Description] AS [Mô tả],
    [CreateDate] AS [Ngày tạo]
FROM [dbo].[VnsErpApplicationVersion]
ORDER BY [ReleaseDate] DESC, [CreateDate] DESC;

PRINT '';
PRINT '========================================';
PRINT 'Hoàn thành insert phiên bản mới';
PRINT '========================================';
PRINT '';
PRINT 'Lưu ý:';
PRINT '- Chỉ có một phiên bản Active tại một thời điểm';
PRINT '- Các phiên bản cũ đã được set thành IsActive = false';
PRINT '- Vui lòng kiểm tra lại thông tin phiên bản vừa insert';
GO

IF @@TRANCOUNT > 0
BEGIN
    COMMIT TRANSACTION;
    PRINT '✅ Transaction đã được commit thành công!';
END
ELSE
BEGIN
    PRINT '⚠️  Không có transaction để commit';
END
GO
