-- =============================================
-- Script tạo dữ liệu mẫu cho BusinessPartnerContact
-- Tạo bởi: AI Assistant
-- Ngày tạo: 2025
-- Mô tả: Script SQL để insert dữ liệu mẫu vào bảng BusinessPartnerContact
-- Lưu ý: Script idempotent - kiểm tra tồn tại trước khi INSERT
-- =============================================

USE [VnsErp2025Final]
GO

SET NOCOUNT ON;
BEGIN TRY
    BEGIN TRAN;

    -- Insert dữ liệu mẫu cho BusinessPartnerContact
    
    -- 1. Contact cho CUS001 (Công ty TNHH ABC)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerContact bc 
                   JOIN dbo.BusinessPartner p ON p.Id = bc.PartnerId 
                   WHERE p.PartnerCode = 'CUS001' AND bc.FullName = N'Nguyễn Văn A')
    BEGIN
        INSERT INTO dbo.BusinessPartnerContact (Id, PartnerId, FullName, [Position], Phone, Email, IsPrimary)
        SELECT NEWID(), p.Id, N'Nguyễn Văn A', N'Giám đốc', '0909000004', 'a.nguyen@abc.com', 1
        FROM dbo.BusinessPartner p WHERE p.PartnerCode = 'CUS001';
    END

    -- 2. Contact thứ 2 cho CUS001
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerContact bc 
                   JOIN dbo.BusinessPartner p ON p.Id = bc.PartnerId 
                   WHERE p.PartnerCode = 'CUS001' AND bc.FullName = N'Trần Thị B')
    BEGIN
        INSERT INTO dbo.BusinessPartnerContact (Id, PartnerId, FullName, [Position], Phone, Email, IsPrimary)
        SELECT NEWID(), p.Id, N'Trần Thị B', N'Trưởng phòng Kinh doanh', '0909000005', 'b.tran@abc.com', 0
        FROM dbo.BusinessPartner p WHERE p.PartnerCode = 'CUS001';
    END

    -- 3. Contact cho VEN001 (Công ty TNHH XYZ)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerContact bc 
                   JOIN dbo.BusinessPartner p ON p.Id = bc.PartnerId 
                   WHERE p.PartnerCode = 'VEN001' AND bc.FullName = N'Lê Văn C')
    BEGIN
        INSERT INTO dbo.BusinessPartnerContact (Id, PartnerId, FullName, [Position], Phone, Email, IsPrimary)
        SELECT NEWID(), p.Id, N'Lê Văn C', N'Giám đốc', '0909000006', 'c.le@xyz.com', 1
        FROM dbo.BusinessPartner p WHERE p.PartnerCode = 'VEN001';
    END

    -- 4. Contact cho BOTH001 (Công ty TNHH Song Phương)
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerContact bc 
                   JOIN dbo.BusinessPartner p ON p.Id = bc.PartnerId 
                   WHERE p.PartnerCode = 'BOTH001' AND bc.FullName = N'Phạm Thị D')
    BEGIN
        INSERT INTO dbo.BusinessPartnerContact (Id, PartnerId, FullName, [Position], Phone, Email, IsPrimary)
        SELECT NEWID(), p.Id, N'Phạm Thị D', N'Giám đốc', '0909000007', 'd.pham@both.com', 1
        FROM dbo.BusinessPartner p WHERE p.PartnerCode = 'BOTH001';
    END

    -- 5. Contact thứ 2 cho BOTH001
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerContact bc 
                   JOIN dbo.BusinessPartner p ON p.Id = bc.PartnerId 
                   WHERE p.PartnerCode = 'BOTH001' AND bc.FullName = N'Hoàng Văn E')
    BEGIN
        INSERT INTO dbo.BusinessPartnerContact (Id, PartnerId, FullName, [Position], Phone, Email, IsPrimary)
        SELECT NEWID(), p.Id, N'Hoàng Văn E', N'Trưởng phòng Mua hàng', '0909000008', 'e.hoang@both.com', 0
        FROM dbo.BusinessPartner p WHERE p.PartnerCode = 'BOTH001';
    END

    -- 6. Bulk insert contacts cho các TEST partners (TEST001-TEST100)
    DECLARE @i INT = 1;
    WHILE (@i <= 100)
    BEGIN
        DECLARE @code NVARCHAR(20) = CONCAT('TEST', RIGHT('000' + CAST(@i AS VARCHAR(3)), 3));
        
        -- Contact chính
        IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerContact bc 
                       JOIN dbo.BusinessPartner p ON p.Id = bc.PartnerId 
                       WHERE p.PartnerCode = @code AND bc.IsPrimary = 1)
        BEGIN
            INSERT INTO dbo.BusinessPartnerContact (Id, PartnerId, FullName, [Position], Phone, Email, IsPrimary)
            SELECT NEWID(), p.Id, 
                   N'Người liên hệ chính ' + CAST(@i AS NVARCHAR(10)), 
                   N'Giám đốc', 
                   CONCAT('0909', RIGHT('000000' + CAST((@i + 100) AS VARCHAR(6)), 6)),
                   CONCAT('contact', @i, '@test.com'), 
                   1
            FROM dbo.BusinessPartner p WHERE p.PartnerCode = @code;
        END

        -- Contact phụ (chỉ cho một số partner)
        IF (@i % 3 = 0) -- Mỗi partner thứ 3 có thêm 1 contact phụ
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerContact bc 
                           JOIN dbo.BusinessPartner p ON p.Id = bc.PartnerId 
                           WHERE p.PartnerCode = @code AND bc.IsPrimary = 0)
            BEGIN
                INSERT INTO dbo.BusinessPartnerContact (Id, PartnerId, FullName, [Position], Phone, Email, IsPrimary)
                SELECT NEWID(), p.Id, 
                       N'Người liên hệ phụ ' + CAST(@i AS NVARCHAR(10)), 
                       N'Trưởng phòng', 
                       CONCAT('0909', RIGHT('000000' + CAST((@i + 200) AS VARCHAR(6)), 6)),
                       CONCAT('sub', @i, '@test.com'), 
                       0
                FROM dbo.BusinessPartner p WHERE p.PartnerCode = @code;
            END
        END

        SET @i = @i + 1;
    END

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(N'Lỗi khi seed BusinessPartnerContact data: %s', 16, 1, @ErrMsg);
END CATCH

GO

-- Kiểm tra dữ liệu đã insert
SELECT 
    bc.[Id],
    p.[PartnerCode],
    p.[PartnerName],
    bc.[FullName],
    bc.[Position],
    bc.[Phone],
    bc.[Email],
    bc.[IsPrimary],
    COUNT(*) OVER() as TotalRecords
FROM [dbo].[BusinessPartnerContact] bc
JOIN [dbo].[BusinessPartner] p ON p.Id = bc.PartnerId
ORDER BY p.[PartnerCode], bc.[IsPrimary] DESC, bc.[FullName]

GO

-- Thống kê số lượng contact
SELECT 
    'Tổng số liên hệ đối tác' as Description,
    COUNT(*) as Count
FROM [dbo].[BusinessPartnerContact]

GO

-- Thống kê theo partner
SELECT 
    p.[PartnerCode],
    p.[PartnerName],
    COUNT(bc.[Id]) as ContactCount,
    SUM(CASE WHEN bc.[IsPrimary] = 1 THEN 1 ELSE 0 END) as PrimaryContactCount
FROM [dbo].[BusinessPartner] p
LEFT JOIN [dbo].[BusinessPartnerContact] bc ON bc.PartnerId = p.Id
GROUP BY p.[PartnerCode], p.[PartnerName]
ORDER BY p.[PartnerCode]

GO

PRINT 'Đã hoàn thành việc tạo dữ liệu mẫu cho BusinessPartnerContact'
PRINT 'Tổng số bản ghi đã thêm: Contacts cho 103 đối tác (3 đối tác mẫu + 100 đối tác test)'
