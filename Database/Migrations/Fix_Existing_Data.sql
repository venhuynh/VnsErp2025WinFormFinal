-- ============================================================================
-- FIX EXISTING DATA SCRIPT: BusinessPartner Tables
-- Description: Fix dữ liệu hiện có để có thể chạy migration thành công
-- Run this script if you encountered errors during migration
-- ============================================================================

USE [VnsErp2025Final];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

BEGIN TRANSACTION;
GO

PRINT '========================================';
PRINT 'Fixing Existing BusinessPartner Data...';
PRINT '========================================';
GO

-- ============================================================================
-- SECTION 1: FIX EMAIL FORMAT ISSUES
-- ============================================================================

PRINT '';
PRINT '--- Section 1: Fixing Email Format Issues ---';
GO

-- 1.1. Fix BusinessPartnerSite.Email
DECLARE @FixedCount INT = 0;

-- Check if constraint exists and drop it temporarily if needed
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BusinessPartnerSite_EmailFormat')
BEGIN
    ALTER TABLE dbo.BusinessPartnerSite
    DROP CONSTRAINT CK_BusinessPartnerSite_EmailFormat;
    PRINT 'Temporarily dropped CK_BusinessPartnerSite_EmailFormat constraint';
END

UPDATE dbo.BusinessPartnerSite
SET Email = NULL
WHERE Email IS NOT NULL 
  AND (Email NOT LIKE '%@%.%' OR LEN(Email) < 5);

SET @FixedCount = @@ROWCOUNT;
PRINT 'Fixed ' + CAST(@FixedCount AS VARCHAR(10)) + ' invalid emails in BusinessPartnerSite';

-- Recreate constraint
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BusinessPartnerSite_EmailFormat')
BEGIN
    ALTER TABLE dbo.BusinessPartnerSite
    ADD CONSTRAINT CK_BusinessPartnerSite_EmailFormat
    CHECK (Email IS NULL OR Email LIKE '%@%.%');
    PRINT 'Recreated CK_BusinessPartnerSite_EmailFormat constraint';
END
GO

-- 1.2. Fix BusinessPartnerContact.Email
DECLARE @FixedCount INT = 0;

-- Check if constraint exists and drop it temporarily if needed
IF EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BusinessPartnerContact_EmailFormat')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    DROP CONSTRAINT CK_BusinessPartnerContact_EmailFormat;
    PRINT 'Temporarily dropped CK_BusinessPartnerContact_EmailFormat constraint';
END

UPDATE dbo.BusinessPartnerContact
SET Email = NULL
WHERE Email IS NOT NULL 
  AND (Email NOT LIKE '%@%.%' OR LEN(Email) < 5);

SET @FixedCount = @@ROWCOUNT;
PRINT 'Fixed ' + CAST(@FixedCount AS VARCHAR(10)) + ' invalid emails in BusinessPartnerContact';

-- Recreate constraint
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BusinessPartnerContact_EmailFormat')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD CONSTRAINT CK_BusinessPartnerContact_EmailFormat
    CHECK (Email IS NULL OR Email LIKE '%@%.%');
    PRINT 'Recreated CK_BusinessPartnerContact_EmailFormat constraint';
END
GO

-- ============================================================================
-- SECTION 2: FIX MULTIPLE DEFAULT SITES PER PARTNER
-- ============================================================================

PRINT '';
PRINT '--- Section 2: Fixing Multiple Default Sites ---';
GO

-- 2.1. Drop index if exists
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerSite_OneDefaultPerPartner')
BEGIN
    DROP INDEX IX_BusinessPartnerSite_OneDefaultPerPartner ON dbo.BusinessPartnerSite;
    PRINT 'Dropped IX_BusinessPartnerSite_OneDefaultPerPartner index';
END
GO

-- 2.2. Find and fix duplicate defaults
WITH RankedSites AS (
    SELECT 
        Id, 
        PartnerId, 
        IsDefault,
        ROW_NUMBER() OVER (
            PARTITION BY PartnerId 
            ORDER BY CreatedDate ASC, Id ASC
        ) AS rn
    FROM dbo.BusinessPartnerSite
    WHERE PartnerId IN (
        SELECT PartnerId 
        FROM dbo.BusinessPartnerSite 
        WHERE IsDefault = 1
        GROUP BY PartnerId 
        HAVING COUNT(*) > 1
    )
    AND IsDefault = 1
)
UPDATE dbo.BusinessPartnerSite
SET IsDefault = NULL
WHERE Id IN (
    SELECT Id FROM RankedSites WHERE rn > 1
);

DECLARE @FixedSites INT = @@ROWCOUNT;
PRINT 'Fixed ' + CAST(@FixedSites AS VARCHAR(10)) + ' duplicate default sites (kept oldest, set others to NULL)';
GO

-- 2.3. Set default for partners with no default
UPDATE s
SET s.IsDefault = 1
FROM dbo.BusinessPartnerSite s
INNER JOIN (
    SELECT 
        PartnerId,
        MIN(Id) AS FirstSiteId
    FROM dbo.BusinessPartnerSite
    WHERE PartnerId NOT IN (
        SELECT DISTINCT PartnerId 
        FROM dbo.BusinessPartnerSite 
        WHERE IsDefault = 1
    )
    GROUP BY PartnerId
) AS noDefault ON s.Id = noDefault.FirstSiteId;

DECLARE @SetDefaults INT = @@ROWCOUNT;
PRINT 'Set ' + CAST(@SetDefaults AS VARCHAR(10)) + ' sites as default for partners with no default';
GO

-- 2.4. Recreate index
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerSite_OneDefaultPerPartner')
BEGIN
    CREATE UNIQUE INDEX IX_BusinessPartnerSite_OneDefaultPerPartner 
    ON dbo.BusinessPartnerSite(PartnerId, IsDefault) 
    WHERE IsDefault = 1;
    PRINT 'Recreated IX_BusinessPartnerSite_OneDefaultPerPartner index';
END
GO

-- ============================================================================
-- SECTION 3: FIX MULTIPLE PRIMARY CONTACTS PER SITE
-- ============================================================================

PRINT '';
PRINT '--- Section 3: Fixing Multiple Primary Contacts ---';
GO

-- 3.1. Drop index if exists
IF EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerContact_OnePrimaryPerSite')
BEGIN
    DROP INDEX IX_BusinessPartnerContact_OnePrimaryPerSite ON dbo.BusinessPartnerContact;
    PRINT 'Dropped IX_BusinessPartnerContact_OnePrimaryPerSite index';
END
GO

-- 3.2. Find and fix duplicate primaries
WITH RankedContacts AS (
    SELECT 
        Id, 
        SiteId, 
        IsPrimary,
        ROW_NUMBER() OVER (
            PARTITION BY SiteId 
            ORDER BY 
                CASE WHEN CreatedDate IS NOT NULL THEN CreatedDate ELSE '1900-01-01' END ASC,
                Id ASC
        ) AS rn
    FROM dbo.BusinessPartnerContact
    WHERE SiteId IN (
        SELECT SiteId 
        FROM dbo.BusinessPartnerContact 
        WHERE IsPrimary = 1
        GROUP BY SiteId 
        HAVING COUNT(*) > 1
    )
    AND IsPrimary = 1
)
UPDATE dbo.BusinessPartnerContact
SET IsPrimary = 0
WHERE Id IN (
    SELECT Id FROM RankedContacts WHERE rn > 1
);

DECLARE @FixedContacts INT = @@ROWCOUNT;
PRINT 'Fixed ' + CAST(@FixedContacts AS VARCHAR(10)) + ' duplicate primary contacts (kept oldest, set others to 0)';
GO

-- 3.3. Set primary for sites with no primary
UPDATE c
SET c.IsPrimary = 1
FROM dbo.BusinessPartnerContact c
INNER JOIN (
    SELECT 
        SiteId,
        MIN(Id) AS FirstContactId
    FROM dbo.BusinessPartnerContact
    WHERE SiteId NOT IN (
        SELECT DISTINCT SiteId 
        FROM dbo.BusinessPartnerContact 
        WHERE IsPrimary = 1
    )
    GROUP BY SiteId
) AS noPrimary ON c.Id = noPrimary.FirstContactId;

DECLARE @SetPrimaries INT = @@ROWCOUNT;
PRINT 'Set ' + CAST(@SetPrimaries AS VARCHAR(10)) + ' contacts as primary for sites with no primary';
GO

-- 3.4. Recreate index
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerContact_OnePrimaryPerSite')
BEGIN
    CREATE UNIQUE INDEX IX_BusinessPartnerContact_OnePrimaryPerSite 
    ON dbo.BusinessPartnerContact(SiteId, IsPrimary) 
    WHERE IsPrimary = 1;
    PRINT 'Recreated IX_BusinessPartnerContact_OnePrimaryPerSite index';
END
GO

-- ============================================================================
-- SECTION 4: VERIFICATION
-- ============================================================================

PRINT '';
PRINT '--- Section 4: Verification ---';
GO

-- 4.1. Check for remaining email issues
DECLARE @BadEmails INT = 0;

SELECT @BadEmails = COUNT(*)
FROM (
    SELECT Email FROM dbo.BusinessPartnerSite WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%'
    UNION ALL
    SELECT Email FROM dbo.BusinessPartnerContact WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%'
) AS AllEmails;

IF @BadEmails > 0
    PRINT 'WARNING: Still found ' + CAST(@BadEmails AS VARCHAR(10)) + ' invalid emails';
ELSE
    PRINT '✓ All emails are valid or NULL';
GO

-- 4.2. Check for remaining multiple defaults
DECLARE @MultipleDefaults INT = 0;

SELECT @MultipleDefaults = COUNT(*)
FROM (
    SELECT PartnerId
    FROM dbo.BusinessPartnerSite
    WHERE IsDefault = 1
    GROUP BY PartnerId
    HAVING COUNT(*) > 1
) AS MultiDefaults;

IF @MultipleDefaults > 0
    PRINT 'WARNING: Still found ' + CAST(@MultipleDefaults AS VARCHAR(10)) + ' partners with multiple default sites';
ELSE
    PRINT '✓ All partners have at most 1 default site';
GO

-- 4.3. Check for remaining multiple primaries
DECLARE @MultiplePrimaries INT = 0;

SELECT @MultiplePrimaries = COUNT(*)
FROM (
    SELECT SiteId
    FROM dbo.BusinessPartnerContact
    WHERE IsPrimary = 1
    GROUP BY SiteId
    HAVING COUNT(*) > 1
) AS MultiPrimaries;

IF @MultiplePrimaries > 0
    PRINT 'WARNING: Still found ' + CAST(@MultiplePrimaries AS VARCHAR(10)) + ' sites with multiple primary contacts';
ELSE
    PRINT '✓ All sites have at most 1 primary contact';
GO

-- ============================================================================
-- COMMIT TRANSACTION
-- ============================================================================

PRINT '';
PRINT '========================================';
PRINT 'Data fix completed!';
PRINT '========================================';
PRINT '';
PRINT 'You can now re-run Migration_BusinessPartner_Improvements.sql';
PRINT '';

IF @@TRANCOUNT > 0
BEGIN
    COMMIT TRANSACTION;
    PRINT 'Transaction committed successfully.';
END
ELSE
BEGIN
    PRINT 'WARNING: No active transaction to commit.';
END
GO

