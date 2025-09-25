/*
  Seed_BusinessPartner_TestData.sql
  Mục đích: Seed dữ liệu mẫu cho BusinessPartner, BusinessPartnerCategory, Contact và Mapping.
  Lưu ý: Script an toàn (idempotent) - có kiểm tra tồn tại trước khi INSERT để tránh trùng.
*/

SET NOCOUNT ON;
BEGIN TRY
    BEGIN TRAN;

    /* 1) Insert Category */
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Khách hàng lẻ')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Khách hàng lẻ', N'Cá nhân mua hàng trực tiếp');
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Khách hàng doanh nghiệp')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Khách hàng doanh nghiệp', N'Công ty, tổ chức');
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà cung cấp dịch vụ')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà cung cấp dịch vụ', N'Cung cấp dịch vụ CNTT, vận tải...');
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerCategory WHERE CategoryName = N'Nhà phân phối')
    BEGIN
        INSERT INTO dbo.BusinessPartnerCategory (Id, CategoryName, [Description])
        VALUES (NEWID(), N'Nhà phân phối', N'Cung cấp số lượng lớn');
    END

    /* 2) Insert BusinessPartner */
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartner WHERE PartnerCode = 'CUS001')
    BEGIN
        INSERT INTO dbo.BusinessPartner (
            Id, PartnerCode, PartnerName, PartnerType, TaxCode, Phone, Email, City, Country,
            Website, [Address], ContactPerson, ContactPosition, BankAccount, BankName,
            CreditLimit, PaymentTerm, IsActive, CreatedDate, UpdatedDate
        )
        VALUES (
            NEWID(), 'CUS001', N'Công ty TNHH ABC', 1, '0312345678', '0909000001', 'contact@abc.com', N'HCM', N'Việt Nam',
            NULL, NULL, NULL, NULL, NULL, NULL,
            NULL, NULL, 1, GETDATE(), NULL
        );
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartner WHERE PartnerCode = 'VEN001')
    BEGIN
        INSERT INTO dbo.BusinessPartner (
            Id, PartnerCode, PartnerName, PartnerType, TaxCode, Phone, Email, City, Country,
            Website, [Address], ContactPerson, ContactPosition, BankAccount, BankName,
            CreditLimit, PaymentTerm, IsActive, CreatedDate, UpdatedDate
        )
        VALUES (
            NEWID(), 'VEN001', N'Công ty TNHH XYZ', 2, '0209876543', '0909000002', 'info@xyz.com', N'Hà Nội', N'Việt Nam',
            NULL, NULL, NULL, NULL, NULL, NULL,
            NULL, NULL, 1, GETDATE(), NULL
        );
    END

    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartner WHERE PartnerCode = 'BOTH001')
    BEGIN
        INSERT INTO dbo.BusinessPartner (
            Id, PartnerCode, PartnerName, PartnerType, TaxCode, Phone, Email, City, Country,
            Website, [Address], ContactPerson, ContactPosition, BankAccount, BankName,
            CreditLimit, PaymentTerm, IsActive, CreatedDate, UpdatedDate
        )
        VALUES (
            NEWID(), 'BOTH001', N'Công ty TNHH Song Phương', 3, '0505551234', '0909000003', 'sp@both.com', N'Đà Nẵng', N'Việt Nam',
            NULL, NULL, NULL, NULL, NULL, NULL,
            NULL, NULL, 1, GETDATE(), NULL
        );
    END

    /* 3) Insert Contact cho CUS001 */
    IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartnerContact bc JOIN dbo.BusinessPartner p ON p.Id = bc.PartnerId WHERE p.PartnerCode = 'CUS001')
    BEGIN
        INSERT INTO dbo.BusinessPartnerContact (Id, PartnerId, FullName, [Position], Phone, Email, IsPrimary)
        SELECT NEWID(), p.Id, N'Nguyễn Văn A', N'Giám đốc', '0909000004', 'a.nguyen@abc.com', 1
        FROM dbo.BusinessPartner p WHERE p.PartnerCode = 'CUS001';
    END

    /* 4) Insert Mapping Category: CUS001 -> Khách hàng doanh nghiệp */
    IF NOT EXISTS (
        SELECT 1
        FROM dbo.BusinessPartner_BusinessPartnerCategory m
        JOIN dbo.BusinessPartner p ON p.Id = m.PartnerId AND p.PartnerCode = 'CUS001'
        JOIN dbo.BusinessPartnerCategory c ON c.Id = m.CategoryId AND c.CategoryName = N'Khách hàng doanh nghiệp')
    BEGIN
        INSERT INTO dbo.BusinessPartner_BusinessPartnerCategory (PartnerId, CategoryId)
        SELECT p.Id, c.Id
        FROM dbo.BusinessPartner p
        JOIN dbo.BusinessPartnerCategory c ON c.CategoryName = N'Khách hàng doanh nghiệp'
        WHERE p.PartnerCode = 'CUS001';
    END

    /* 5) Bulk seed 100 BusinessPartner test rows: TEST001..TEST100 */
    DECLARE @i INT = 1;
    WHILE (@i <= 100)
    BEGIN
        DECLARE @code NVARCHAR(20) = CONCAT('TEST', RIGHT('000' + CAST(@i AS VARCHAR(3)), 3));
        IF NOT EXISTS (SELECT 1 FROM dbo.BusinessPartner WHERE PartnerCode = @code)
        BEGIN
            DECLARE @type INT = ((@i - 1) % 3) + 1; -- 1..3
            DECLARE @city NVARCHAR(100) = CASE (@i % 3)
                                               WHEN 0 THEN N'Hà Nội'
                                               WHEN 1 THEN N'HCM'
                                               ELSE N'Đà Nẵng' END;
            DECLARE @active BIT = CASE WHEN (@i % 7) = 0 THEN 0 ELSE 1 END; -- một vài dòng không active
            DECLARE @name NVARCHAR(255) = N'Đối tác thử nghiệm ' + CAST(@i AS NVARCHAR(10))
                                          + N' - tên rất dài để kiểm tra xuống dòng trong Grid (wrap test).';

            INSERT INTO dbo.BusinessPartner (
                Id, PartnerCode, PartnerName, PartnerType, TaxCode, Phone, Email, City, Country,
                Website, [Address], ContactPerson, ContactPosition, BankAccount, BankName,
                CreditLimit, PaymentTerm, IsActive, CreatedDate, UpdatedDate
            )
            VALUES (
                NEWID(), @code, @name, @type,
                CONCAT('09', RIGHT('00000000' + CAST(@i AS VARCHAR(8)), 8)),
                CONCAT('0909', RIGHT('000000' + CAST(@i AS VARCHAR(6)), 6)),
                CONCAT('test', @i, '@mail.com'),
                @city, N'Việt Nam',
                NULL,
                N'Địa chỉ rất dài dùng để kiểm tra hiển thị xuống dòng của GridControl, lặp lại lần ' + CAST(@i AS NVARCHAR(10)),
                NULL, NULL, NULL, NULL,
                NULL, NULL, @active, GETDATE(), NULL
            );
        END
        SET @i = @i + 1;
    END

    COMMIT TRAN;
END TRY
BEGIN CATCH
    IF @@TRANCOUNT > 0 ROLLBACK TRAN;
    DECLARE @ErrMsg NVARCHAR(4000) = ERROR_MESSAGE();
    RAISERROR(N'Lỗi khi seed test data: %s', 16, 1, @ErrMsg);
END CATCH


