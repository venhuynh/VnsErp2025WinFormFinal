-- ============================================================================
-- MIGRATION SCRIPT: BusinessPartner Tables Improvements
-- Description: Cải tiến các bảng BusinessPartner, BusinessPartnerCategory,
--              BusinessPartnerSite, BusinessPartnerContact
-- Created: 2025
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
PRINT 'Starting BusinessPartner Migration...';
PRINT '========================================';
GO

-- ============================================================================
-- SECTION 1: BUSINESS PARTNER - AUDIT FIELDS & IMPROVEMENTS
-- ============================================================================

PRINT '';
PRINT '--- Section 1: BusinessPartner Table Improvements ---';
GO

-- 1.1. Add Audit Fields to BusinessPartner
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'CreatedBy')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD CreatedBy UNIQUEIDENTIFIER NULL;
    PRINT 'Added CreatedBy column to BusinessPartner';
END
ELSE
    PRINT 'CreatedBy column already exists in BusinessPartner';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'ModifiedBy')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD ModifiedBy UNIQUEIDENTIFIER NULL;
    PRINT 'Added ModifiedBy column to BusinessPartner';
END
ELSE
    PRINT 'ModifiedBy column already exists in BusinessPartner';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'DeletedBy')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD DeletedBy UNIQUEIDENTIFIER NULL;
    PRINT 'Added DeletedBy column to BusinessPartner';
END
ELSE
    PRINT 'DeletedBy column already exists in BusinessPartner';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'IsDeleted')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD IsDeleted BIT NOT NULL DEFAULT 0;
    PRINT 'Added IsDeleted column to BusinessPartner';
END
ELSE
    PRINT 'IsDeleted column already exists in BusinessPartner';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'DeletedDate')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD DeletedDate DATETIME NULL;
    PRINT 'Added DeletedDate column to BusinessPartner';
END
ELSE
    PRINT 'DeletedDate column already exists in BusinessPartner';
GO

-- 1.2. Add Foreign Key Constraints for Audit Fields
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'CreatedBy')
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BusinessPartner_CreatedBy_ApplicationUser')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD CONSTRAINT FK_BusinessPartner_CreatedBy_ApplicationUser
    FOREIGN KEY (CreatedBy) REFERENCES dbo.ApplicationUser(Id);
    PRINT 'Added FK constraint for CreatedBy';
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'ModifiedBy')
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BusinessPartner_ModifiedBy_ApplicationUser')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD CONSTRAINT FK_BusinessPartner_ModifiedBy_ApplicationUser
    FOREIGN KEY (ModifiedBy) REFERENCES dbo.ApplicationUser(Id);
    PRINT 'Added FK constraint for ModifiedBy';
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'DeletedBy')
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BusinessPartner_DeletedBy_ApplicationUser')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD CONSTRAINT FK_BusinessPartner_DeletedBy_ApplicationUser
    FOREIGN KEY (DeletedBy) REFERENCES dbo.ApplicationUser(Id);
    PRINT 'Added FK constraint for DeletedBy';
END
GO

-- 1.3. Add Email Validation Constraint
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BusinessPartner_EmailFormat')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD CONSTRAINT CK_BusinessPartner_EmailFormat
    CHECK (Email IS NULL OR Email LIKE '%@%.%');
    PRINT 'Added Email format validation constraint';
END
ELSE
    PRINT 'Email validation constraint already exists';
GO

-- 1.4. Add CreditLimit Validation Constraint
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BusinessPartner_CreditLimit')
BEGIN
    ALTER TABLE dbo.BusinessPartner
    ADD CONSTRAINT CK_BusinessPartner_CreditLimit
    CHECK (CreditLimit IS NULL OR CreditLimit >= 0);
    PRINT 'Added CreditLimit validation constraint';
END
ELSE
    PRINT 'CreditLimit validation constraint already exists';
GO

-- ============================================================================
-- SECTION 2: BUSINESS PARTNER CATEGORY - IMPROVEMENTS
-- ============================================================================

PRINT '';
PRINT '--- Section 2: BusinessPartnerCategory Table Improvements ---';
GO

-- 2.1. Add CategoryCode
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'CategoryCode')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD CategoryCode NVARCHAR(50) NULL;
    PRINT 'Added CategoryCode column to BusinessPartnerCategory';
END
ELSE
    PRINT 'CategoryCode column already exists in BusinessPartnerCategory';
GO

-- 2.2. Add IsActive
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'IsActive')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD IsActive BIT NOT NULL DEFAULT 1;
    PRINT 'Added IsActive column to BusinessPartnerCategory';
END
ELSE
    PRINT 'IsActive column already exists in BusinessPartnerCategory';
GO

-- 2.3. Add SortOrder
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'SortOrder')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD SortOrder INT NULL;
    PRINT 'Added SortOrder column to BusinessPartnerCategory';
END
ELSE
    PRINT 'SortOrder column already exists in BusinessPartnerCategory';
GO

-- 2.4. Add Audit Fields
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'CreatedDate')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD CreatedDate DATETIME NOT NULL DEFAULT GETDATE();
    PRINT 'Added CreatedDate column to BusinessPartnerCategory';
END
ELSE
    PRINT 'CreatedDate column already exists in BusinessPartnerCategory';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'CreatedBy')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD CreatedBy UNIQUEIDENTIFIER NULL;
    PRINT 'Added CreatedBy column to BusinessPartnerCategory';
END
ELSE
    PRINT 'CreatedBy column already exists in BusinessPartnerCategory';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'ModifiedDate')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD ModifiedDate DATETIME NULL;
    PRINT 'Added ModifiedDate column to BusinessPartnerCategory';
END
ELSE
    PRINT 'ModifiedDate column already exists in BusinessPartnerCategory';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'ModifiedBy')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD ModifiedBy UNIQUEIDENTIFIER NULL;
    PRINT 'Added ModifiedBy column to BusinessPartnerCategory';
END
ELSE
    PRINT 'ModifiedBy column already exists in BusinessPartnerCategory';
GO

-- 2.5. Add Foreign Key Constraints for Audit Fields
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'CreatedBy')
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BusinessPartnerCategory_CreatedBy_ApplicationUser')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD CONSTRAINT FK_BusinessPartnerCategory_CreatedBy_ApplicationUser
    FOREIGN KEY (CreatedBy) REFERENCES dbo.ApplicationUser(Id);
    PRINT 'Added FK constraint for BusinessPartnerCategory.CreatedBy';
END
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'ModifiedBy')
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BusinessPartnerCategory_ModifiedBy_ApplicationUser')
BEGIN
    ALTER TABLE dbo.BusinessPartnerCategory
    ADD CONSTRAINT FK_BusinessPartnerCategory_ModifiedBy_ApplicationUser
    FOREIGN KEY (ModifiedBy) REFERENCES dbo.ApplicationUser(Id);
    PRINT 'Added FK constraint for BusinessPartnerCategory.ModifiedBy';
END
GO

-- ============================================================================
-- SECTION 3: BUSINESS PARTNER SITE - IMPROVEMENTS
-- ============================================================================

PRINT '';
PRINT '--- Section 3: BusinessPartnerSite Table Improvements ---';
GO

-- 3.1. Add Additional Address Fields
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerSite') AND name = 'PostalCode')
BEGIN
    ALTER TABLE dbo.BusinessPartnerSite
    ADD PostalCode NVARCHAR(20) NULL;
    PRINT 'Added PostalCode column to BusinessPartnerSite';
END
ELSE
    PRINT 'PostalCode column already exists in BusinessPartnerSite';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerSite') AND name = 'District')
BEGIN
    ALTER TABLE dbo.BusinessPartnerSite
    ADD District NVARCHAR(100) NULL;
    PRINT 'Added District column to BusinessPartnerSite';
END
ELSE
    PRINT 'District column already exists in BusinessPartnerSite';
GO

-- 3.2. Add Geographic Coordinates
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerSite') AND name = 'Latitude')
BEGIN
    ALTER TABLE dbo.BusinessPartnerSite
    ADD Latitude DECIMAL(10,8) NULL;
    PRINT 'Added Latitude column to BusinessPartnerSite';
END
ELSE
    PRINT 'Latitude column already exists in BusinessPartnerSite';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerSite') AND name = 'Longitude')
BEGIN
    ALTER TABLE dbo.BusinessPartnerSite
    ADD Longitude DECIMAL(11,8) NULL;
    PRINT 'Added Longitude column to BusinessPartnerSite';
END
ELSE
    PRINT 'Longitude column already exists in BusinessPartnerSite';
GO

-- 3.3. Add SiteType
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerSite') AND name = 'SiteType')
BEGIN
    ALTER TABLE dbo.BusinessPartnerSite
    ADD SiteType INT NULL; -- 0: Trụ sở, 1: Chi nhánh, 2: Kho hàng, 3: Văn phòng đại diện
    PRINT 'Added SiteType column to BusinessPartnerSite';
END
ELSE
    PRINT 'SiteType column already exists in BusinessPartnerSite';
GO

-- 3.4. Add Notes
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerSite') AND name = 'Notes')
BEGIN
    ALTER TABLE dbo.BusinessPartnerSite
    ADD Notes NVARCHAR(1000) NULL;
    PRINT 'Added Notes column to BusinessPartnerSite';
END
ELSE
    PRINT 'Notes column already exists in BusinessPartnerSite';
GO

-- 3.5. Add Email Validation Constraint (with data cleanup first)
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BusinessPartnerSite_EmailFormat')
BEGIN
    -- Clean up invalid emails first
    DECLARE @InvalidEmails INT = 0;
    SELECT @InvalidEmails = COUNT(*) 
    FROM dbo.BusinessPartnerSite 
    WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%';
    
    IF @InvalidEmails > 0
    BEGIN
        UPDATE dbo.BusinessPartnerSite
        SET Email = NULL
        WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%';
        PRINT 'Cleaned up ' + CAST(@InvalidEmails AS VARCHAR(10)) + ' invalid emails in BusinessPartnerSite before adding constraint';
    END
    
    ALTER TABLE dbo.BusinessPartnerSite
    ADD CONSTRAINT CK_BusinessPartnerSite_EmailFormat
    CHECK (Email IS NULL OR Email LIKE '%@%.%');
    PRINT 'Added Email format validation constraint for BusinessPartnerSite';
END
ELSE
    PRINT 'Email validation constraint already exists for BusinessPartnerSite';
GO

-- 3.6. Create Unique Index for IsDefault (Only one default site per partner)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerSite_OneDefaultPerPartner')
BEGIN
    -- Clean up duplicate defaults first
    DECLARE @DuplicateDefaults INT = 0;
    SELECT @DuplicateDefaults = COUNT(*)
    FROM (
        SELECT PartnerId
        FROM dbo.BusinessPartnerSite
        WHERE IsDefault = 1
        GROUP BY PartnerId
        HAVING COUNT(*) > 1
    ) AS Duplicates;
    
    IF @DuplicateDefaults > 0
    BEGIN
        -- Keep the oldest default site, set others to NULL
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
        PRINT 'Cleaned up ' + CAST(@DuplicateDefaults AS VARCHAR(10)) + ' duplicate default sites before creating index';
    END
    
    CREATE UNIQUE INDEX IX_BusinessPartnerSite_OneDefaultPerPartner 
    ON dbo.BusinessPartnerSite(PartnerId, IsDefault) 
    WHERE IsDefault = 1;
    PRINT 'Created unique index for IsDefault constraint';
END
ELSE
    PRINT 'Unique index for IsDefault already exists';
GO

-- ============================================================================
-- SECTION 4: BUSINESS PARTNER CONTACT - IMPROVEMENTS
-- ============================================================================

PRINT '';
PRINT '--- Section 4: BusinessPartnerContact Table Improvements ---';
GO

-- 4.1. Add Additional Contact Fields
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'Mobile')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD Mobile NVARCHAR(50) NULL;
    PRINT 'Added Mobile column to BusinessPartnerContact';
END
ELSE
    PRINT 'Mobile column already exists in BusinessPartnerContact';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'Fax')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD Fax NVARCHAR(50) NULL;
    PRINT 'Added Fax column to BusinessPartnerContact';
END
ELSE
    PRINT 'Fax column already exists in BusinessPartnerContact';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'Department')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD Department NVARCHAR(100) NULL;
    PRINT 'Added Department column to BusinessPartnerContact';
END
ELSE
    PRINT 'Department column already exists in BusinessPartnerContact';
GO

-- 4.2. Add Personal Information Fields
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'BirthDate')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD BirthDate DATE NULL;
    PRINT 'Added BirthDate column to BusinessPartnerContact';
END
ELSE
    PRINT 'BirthDate column already exists in BusinessPartnerContact';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'Gender')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD Gender NVARCHAR(10) NULL; -- 'Male', 'Female', 'Other'
    PRINT 'Added Gender column to BusinessPartnerContact';
END
ELSE
    PRINT 'Gender column already exists in BusinessPartnerContact';
GO

-- 4.3. Add Social Media Fields
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'LinkedIn')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD LinkedIn NVARCHAR(255) NULL;
    PRINT 'Added LinkedIn column to BusinessPartnerContact';
END
ELSE
    PRINT 'LinkedIn column already exists in BusinessPartnerContact';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'Skype')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD Skype NVARCHAR(100) NULL;
    PRINT 'Added Skype column to BusinessPartnerContact';
END
ELSE
    PRINT 'Skype column already exists in BusinessPartnerContact';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'WeChat')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD WeChat NVARCHAR(100) NULL;
    PRINT 'Added WeChat column to BusinessPartnerContact';
END
ELSE
    PRINT 'WeChat column already exists in BusinessPartnerContact';
GO

-- 4.4. Add Notes and Avatar Path
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'Notes')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD Notes NVARCHAR(1000) NULL;
    PRINT 'Added Notes column to BusinessPartnerContact';
END
ELSE
    PRINT 'Notes column already exists in BusinessPartnerContact';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'AvatarPath')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD AvatarPath NVARCHAR(500) NULL;
    PRINT 'Added AvatarPath column to BusinessPartnerContact';
END
ELSE
    PRINT 'AvatarPath column already exists in BusinessPartnerContact';
GO

-- 4.5. Add Audit Fields
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'CreatedDate')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD CreatedDate DATETIME NOT NULL DEFAULT GETDATE();
    PRINT 'Added CreatedDate column to BusinessPartnerContact';
END
ELSE
    PRINT 'CreatedDate column already exists in BusinessPartnerContact';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'ModifiedDate')
BEGIN
    ALTER TABLE dbo.BusinessPartnerContact
    ADD ModifiedDate DATETIME NULL;
    PRINT 'Added ModifiedDate column to BusinessPartnerContact';
END
ELSE
    PRINT 'ModifiedDate column already exists in BusinessPartnerContact';
GO

-- 4.6. Add Email Validation Constraint (with data cleanup first)
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_BusinessPartnerContact_EmailFormat')
BEGIN
    -- Clean up invalid emails first
    DECLARE @InvalidEmails INT = 0;
    SELECT @InvalidEmails = COUNT(*) 
    FROM dbo.BusinessPartnerContact 
    WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%';
    
    IF @InvalidEmails > 0
    BEGIN
        UPDATE dbo.BusinessPartnerContact
        SET Email = NULL
        WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%';
        PRINT 'Cleaned up ' + CAST(@InvalidEmails AS VARCHAR(10)) + ' invalid emails in BusinessPartnerContact before adding constraint';
    END
    
    ALTER TABLE dbo.BusinessPartnerContact
    ADD CONSTRAINT CK_BusinessPartnerContact_EmailFormat
    CHECK (Email IS NULL OR Email LIKE '%@%.%');
    PRINT 'Added Email format validation constraint for BusinessPartnerContact';
END
ELSE
    PRINT 'Email validation constraint already exists for BusinessPartnerContact';
GO

-- 4.7. Create Unique Index for IsPrimary (Only one primary contact per site)
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerContact_OnePrimaryPerSite')
BEGIN
    -- Clean up duplicate primaries first
    DECLARE @DuplicatePrimaries INT = 0;
    SELECT @DuplicatePrimaries = COUNT(*)
    FROM (
        SELECT SiteId
        FROM dbo.BusinessPartnerContact
        WHERE IsPrimary = 1
        GROUP BY SiteId
        HAVING COUNT(*) > 1
    ) AS Duplicates;
    
    IF @DuplicatePrimaries > 0
    BEGIN
        -- Keep the oldest primary contact, set others to 0
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
        PRINT 'Cleaned up ' + CAST(@DuplicatePrimaries AS VARCHAR(10)) + ' duplicate primary contacts before creating index';
    END
    
    CREATE UNIQUE INDEX IX_BusinessPartnerContact_OnePrimaryPerSite 
    ON dbo.BusinessPartnerContact(SiteId, IsPrimary) 
    WHERE IsPrimary = 1;
    PRINT 'Created unique index for IsPrimary constraint';
END
ELSE
    PRINT 'Unique index for IsPrimary already exists';
GO

-- ============================================================================
-- SECTION 5: BUSINESS PARTNER - BUSINESS PARTNER CATEGORY JUNCTION TABLE
-- ============================================================================

PRINT '';
PRINT '--- Section 5: BusinessPartner_BusinessPartnerCategory Junction Table Improvements ---';
GO

-- 5.1. Add Audit Fields
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner_BusinessPartnerCategory') AND name = 'CreatedDate')
BEGIN
    ALTER TABLE dbo.BusinessPartner_BusinessPartnerCategory
    ADD CreatedDate DATETIME NOT NULL DEFAULT GETDATE();
    PRINT 'Added CreatedDate column to BusinessPartner_BusinessPartnerCategory';
END
ELSE
    PRINT 'CreatedDate column already exists in BusinessPartner_BusinessPartnerCategory';
GO

IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner_BusinessPartnerCategory') AND name = 'CreatedBy')
BEGIN
    ALTER TABLE dbo.BusinessPartner_BusinessPartnerCategory
    ADD CreatedBy UNIQUEIDENTIFIER NULL;
    PRINT 'Added CreatedBy column to BusinessPartner_BusinessPartnerCategory';
END
ELSE
    PRINT 'CreatedBy column already exists in BusinessPartner_BusinessPartnerCategory';
GO

-- 5.2. Add Foreign Key Constraint for CreatedBy
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner_BusinessPartnerCategory') AND name = 'CreatedBy')
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = 'FK_BusinessPartner_BusinessPartnerCategory_CreatedBy_ApplicationUser')
BEGIN
    ALTER TABLE dbo.BusinessPartner_BusinessPartnerCategory
    ADD CONSTRAINT FK_BusinessPartner_BusinessPartnerCategory_CreatedBy_ApplicationUser
    FOREIGN KEY (CreatedBy) REFERENCES dbo.ApplicationUser(Id);
    PRINT 'Added FK constraint for BusinessPartner_BusinessPartnerCategory.CreatedBy';
END
GO

-- ============================================================================
-- SECTION 6: PERFORMANCE INDEXES
-- ============================================================================

PRINT '';
PRINT '--- Section 6: Creating Performance Indexes ---';
GO

-- 6.1. BusinessPartner Indexes
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartner_PartnerCode')
BEGIN
    CREATE INDEX IX_BusinessPartner_PartnerCode ON dbo.BusinessPartner(PartnerCode);
    PRINT 'Created index IX_BusinessPartner_PartnerCode';
END
ELSE
    PRINT 'Index IX_BusinessPartner_PartnerCode already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartner_PartnerType')
BEGIN
    CREATE INDEX IX_BusinessPartner_PartnerType ON dbo.BusinessPartner(PartnerType);
    PRINT 'Created index IX_BusinessPartner_PartnerType';
END
ELSE
    PRINT 'Index IX_BusinessPartner_PartnerType already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartner_IsActive')
BEGIN
    CREATE INDEX IX_BusinessPartner_IsActive ON dbo.BusinessPartner(IsActive);
    PRINT 'Created index IX_BusinessPartner_IsActive';
END
ELSE
    PRINT 'Index IX_BusinessPartner_IsActive already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartner_IsDeleted')
BEGIN
    CREATE INDEX IX_BusinessPartner_IsDeleted ON dbo.BusinessPartner(IsDeleted);
    PRINT 'Created index IX_BusinessPartner_IsDeleted';
END
ELSE
    PRINT 'Index IX_BusinessPartner_IsDeleted already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartner_TaxCode')
BEGIN
    CREATE INDEX IX_BusinessPartner_TaxCode ON dbo.BusinessPartner(TaxCode) WHERE TaxCode IS NOT NULL;
    PRINT 'Created filtered index IX_BusinessPartner_TaxCode';
END
ELSE
    PRINT 'Index IX_BusinessPartner_TaxCode already exists';
GO

-- 6.2. BusinessPartnerSite Indexes
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerSite_PartnerId')
BEGIN
    CREATE INDEX IX_BusinessPartnerSite_PartnerId ON dbo.BusinessPartnerSite(PartnerId);
    PRINT 'Created index IX_BusinessPartnerSite_PartnerId';
END
ELSE
    PRINT 'Index IX_BusinessPartnerSite_PartnerId already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerSite_IsActive')
BEGIN
    CREATE INDEX IX_BusinessPartnerSite_IsActive ON dbo.BusinessPartnerSite(IsActive);
    PRINT 'Created index IX_BusinessPartnerSite_IsActive';
END
ELSE
    PRINT 'Index IX_BusinessPartnerSite_IsActive already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerSite_SiteType')
BEGIN
    CREATE INDEX IX_BusinessPartnerSite_SiteType ON dbo.BusinessPartnerSite(SiteType) WHERE SiteType IS NOT NULL;
    PRINT 'Created filtered index IX_BusinessPartnerSite_SiteType';
END
ELSE
    PRINT 'Index IX_BusinessPartnerSite_SiteType already exists';
GO

-- 6.3. BusinessPartnerContact Indexes
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerContact_SiteId')
BEGIN
    CREATE INDEX IX_BusinessPartnerContact_SiteId ON dbo.BusinessPartnerContact(SiteId);
    PRINT 'Created index IX_BusinessPartnerContact_SiteId';
END
ELSE
    PRINT 'Index IX_BusinessPartnerContact_SiteId already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerContact_IsActive')
BEGIN
    CREATE INDEX IX_BusinessPartnerContact_IsActive ON dbo.BusinessPartnerContact(IsActive);
    PRINT 'Created index IX_BusinessPartnerContact_IsActive';
END
ELSE
    PRINT 'Index IX_BusinessPartnerContact_IsActive already exists';
GO

-- 6.4. BusinessPartnerCategory Indexes
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerCategory_ParentId')
BEGIN
    CREATE INDEX IX_BusinessPartnerCategory_ParentId ON dbo.BusinessPartnerCategory(ParentId) WHERE ParentId IS NOT NULL;
    PRINT 'Created filtered index IX_BusinessPartnerCategory_ParentId';
END
ELSE
    PRINT 'Index IX_BusinessPartnerCategory_ParentId already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerCategory_CategoryCode')
BEGIN
    CREATE INDEX IX_BusinessPartnerCategory_CategoryCode ON dbo.BusinessPartnerCategory(CategoryCode) WHERE CategoryCode IS NOT NULL;
    PRINT 'Created filtered index IX_BusinessPartnerCategory_CategoryCode';
END
ELSE
    PRINT 'Index IX_BusinessPartnerCategory_CategoryCode already exists';
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE name = 'IX_BusinessPartnerCategory_IsActive')
BEGIN
    CREATE INDEX IX_BusinessPartnerCategory_IsActive ON dbo.BusinessPartnerCategory(IsActive);
    PRINT 'Created index IX_BusinessPartnerCategory_IsActive';
END
ELSE
    PRINT 'Index IX_BusinessPartnerCategory_IsActive already exists';
GO

-- ============================================================================
-- SECTION 7: DATA MIGRATION (Optional - Uncomment if needed)
-- ============================================================================

PRINT '';
PRINT '--- Section 7: Data Migration (Optional) ---';
PRINT 'Note: Uncomment and modify these sections if you need to migrate existing data';
GO

/*
-- 7.1. Migrate existing BusinessPartner data to set default values
-- Update IsDeleted to 0 for existing records
UPDATE dbo.BusinessPartner
SET IsDeleted = 0
WHERE IsDeleted IS NULL;

-- 7.2. Set default SiteType for existing BusinessPartnerSite records
-- Assuming existing sites with IsDefault = 1 are headquarters
UPDATE dbo.BusinessPartnerSite
SET SiteType = 0 -- Trụ sở
WHERE IsDefault = 1 AND SiteType IS NULL;

-- 7.3. Set default IsActive for existing BusinessPartnerCategory records
UPDATE dbo.BusinessPartnerCategory
SET IsActive = 1
WHERE IsActive IS NULL;

-- 7.4. Generate CategoryCode for existing BusinessPartnerCategory if needed
-- This is just an example - adjust based on your business rules
UPDATE dbo.BusinessPartnerCategory
SET CategoryCode = 'CAT' + RIGHT('0000' + CAST(ROW_NUMBER() OVER (ORDER BY CreatedDate) AS VARCHAR), 4)
WHERE CategoryCode IS NULL;
*/

-- ============================================================================
-- SECTION 8: VERIFICATION
-- ============================================================================

PRINT '';
PRINT '--- Section 8: Verification ---';
GO

-- Check if all columns were added successfully
DECLARE @MissingColumns TABLE (TableName NVARCHAR(255), ColumnName NVARCHAR(255));

-- BusinessPartner
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'CreatedBy')
    INSERT INTO @MissingColumns VALUES ('BusinessPartner', 'CreatedBy');
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'IsDeleted')
    INSERT INTO @MissingColumns VALUES ('BusinessPartner', 'IsDeleted');

-- BusinessPartnerCategory
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'CategoryCode')
    INSERT INTO @MissingColumns VALUES ('BusinessPartnerCategory', 'CategoryCode');
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerCategory') AND name = 'IsActive')
    INSERT INTO @MissingColumns VALUES ('BusinessPartnerCategory', 'IsActive');

-- BusinessPartnerSite
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerSite') AND name = 'PostalCode')
    INSERT INTO @MissingColumns VALUES ('BusinessPartnerSite', 'PostalCode');
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerSite') AND name = 'SiteType')
    INSERT INTO @MissingColumns VALUES ('BusinessPartnerSite', 'SiteType');

-- BusinessPartnerContact
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'Mobile')
    INSERT INTO @MissingColumns VALUES ('BusinessPartnerContact', 'Mobile');
IF NOT EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartnerContact') AND name = 'CreatedDate')
    INSERT INTO @MissingColumns VALUES ('BusinessPartnerContact', 'CreatedDate');

IF EXISTS (SELECT 1 FROM @MissingColumns)
BEGIN
    PRINT 'WARNING: Some columns may not have been added:';
    SELECT TableName, ColumnName FROM @MissingColumns;
END
ELSE
    PRINT 'All expected columns verified successfully';
GO

-- ============================================================================
-- COMMIT TRANSACTION
-- ============================================================================

PRINT '';
PRINT '========================================';
PRINT 'Migration completed successfully!';
PRINT '========================================';
PRINT '';
PRINT 'Summary of changes:';
PRINT '1. Added audit fields (CreatedBy, ModifiedBy, DeletedBy, IsDeleted, DeletedDate)';
PRINT '2. Added CategoryCode, IsActive, SortOrder to BusinessPartnerCategory';
PRINT '3. Added PostalCode, District, Latitude, Longitude, SiteType to BusinessPartnerSite';
PRINT '4. Added Mobile, Fax, Department, Social fields to BusinessPartnerContact';
PRINT '5. Added validation constraints (Email format, CreditLimit)';
PRINT '6. Created unique indexes for IsDefault and IsPrimary';
PRINT '7. Created performance indexes for frequently queried columns';
PRINT '';
PRINT 'Please review the changes and test thoroughly before using in production.';
PRINT '';

-- Check if transaction is still active before committing
IF @@TRANCOUNT > 0
BEGIN
    COMMIT TRANSACTION;
    PRINT 'Transaction committed successfully.';
END
ELSE
BEGIN
    PRINT 'WARNING: No active transaction to commit. Some operations may have failed.';
    PRINT 'Please review error messages above.';
END
GO

