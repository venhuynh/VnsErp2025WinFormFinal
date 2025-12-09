-- ============================================================================
-- DATA CLEANUP SCRIPT: BusinessPartner Tables
-- Description: Cleanup dữ liệu trước khi chạy migration
-- Run this script BEFORE running Migration_BusinessPartner_Improvements.sql
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
PRINT 'Starting BusinessPartner Data Cleanup...';
PRINT '========================================';
GO

-- ============================================================================
-- SECTION 1: FIX EMAIL FORMAT ISSUES
-- ============================================================================

PRINT '';
PRINT '--- Section 1: Fixing Email Format Issues ---';
GO

-- 1.1. Fix BusinessPartner.Email
DECLARE @FixedCount INT = 0;

UPDATE dbo.BusinessPartner
SET Email = NULL
WHERE Email IS NOT NULL 
  AND (Email NOT LIKE '%@%.%' OR LEN(Email) < 5);

SET @FixedCount = @@ROWCOUNT;
PRINT 'Fixed ' + CAST(@FixedCount AS VARCHAR(10)) + ' invalid emails in BusinessPartner';
GO

-- 1.2. Fix BusinessPartnerSite.Email
DECLARE @FixedCount INT = 0;

UPDATE dbo.BusinessPartnerSite
SET Email = NULL
WHERE Email IS NOT NULL 
  AND (Email NOT LIKE '%@%.%' OR LEN(Email) < 5);

SET @FixedCount = @@ROWCOUNT;
PRINT 'Fixed ' + CAST(@FixedCount AS VARCHAR(10)) + ' invalid emails in BusinessPartnerSite';
GO

-- 1.3. Fix BusinessPartnerContact.Email
DECLARE @FixedCount INT = 0;

UPDATE dbo.BusinessPartnerContact
SET Email = NULL
WHERE Email IS NOT NULL 
  AND (Email NOT LIKE '%@%.%' OR LEN(Email) < 5);

SET @FixedCount = @@ROWCOUNT;
PRINT 'Fixed ' + CAST(@FixedCount AS VARCHAR(10)) + ' invalid emails in BusinessPartnerContact';
GO

-- ============================================================================
-- SECTION 2: FIX MULTIPLE DEFAULT SITES PER PARTNER
-- ============================================================================

PRINT '';
PRINT '--- Section 2: Fixing Multiple Default Sites ---';
GO

-- 2.1. Find partners with multiple default sites
DECLARE @MultipleDefaults TABLE (
    PartnerId UNIQUEIDENTIFIER,
    Count INT
);

INSERT INTO @MultipleDefaults
SELECT PartnerId, COUNT(*) AS Count
FROM dbo.BusinessPartnerSite
WHERE IsDefault = 1
GROUP BY PartnerId
HAVING COUNT(*) > 1;

DECLARE @PartnerCount INT = (SELECT COUNT(*) FROM @MultipleDefaults);
PRINT 'Found ' + CAST(@PartnerCount AS VARCHAR(10)) + ' partners with multiple default sites';
GO

-- 2.2. Keep the oldest default site, set others to NULL
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

-- 2.3. If still NULL, set the first site as default for partners with no default
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

-- ============================================================================
-- SECTION 3: FIX MULTIPLE PRIMARY CONTACTS PER SITE
-- ============================================================================

PRINT '';
PRINT '--- Section 3: Fixing Multiple Primary Contacts ---';
GO

-- 3.1. Find sites with multiple primary contacts
DECLARE @MultiplePrimaries TABLE (
    SiteId UNIQUEIDENTIFIER,
    Count INT
);

INSERT INTO @MultiplePrimaries
SELECT SiteId, COUNT(*) AS Count
FROM dbo.BusinessPartnerContact
WHERE IsPrimary = 1
GROUP BY SiteId
HAVING COUNT(*) > 1;

DECLARE @SiteCount INT = (SELECT COUNT(*) FROM @MultiplePrimaries);
PRINT 'Found ' + CAST(@SiteCount AS VARCHAR(10)) + ' sites with multiple primary contacts';
GO

-- 3.2. Keep the oldest primary contact, set others to 0
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

-- 3.3. If no primary contact, set the first contact as primary for sites with no primary
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
    SELECT Email FROM dbo.BusinessPartner WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%'
    UNION ALL
    SELECT Email FROM dbo.BusinessPartnerSite WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%'
    UNION ALL
    SELECT Email FROM dbo.BusinessPartnerContact WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%'
) AS AllEmails;

IF @BadEmails > 0
    PRINT 'WARNING: Still found ' + CAST(@BadEmails AS VARCHAR(10)) + ' invalid emails';
ELSE
    PRINT 'All emails are valid or NULL';
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
    PRINT 'All partners have at most 1 default site';
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
    PRINT 'All sites have at most 1 primary contact';
GO

-- ============================================================================
-- COMMIT TRANSACTION
-- ============================================================================

PRINT '';
PRINT '========================================';
PRINT 'Data cleanup completed!';
PRINT '========================================';
PRINT '';
PRINT 'Next steps:';
PRINT '1. Review the changes above';
PRINT '2. Run Migration_BusinessPartner_Improvements.sql';
PRINT '';

COMMIT TRANSACTION;
GO

