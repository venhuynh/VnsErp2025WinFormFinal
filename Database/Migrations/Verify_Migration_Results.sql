-- ============================================================================
-- VERIFICATION SCRIPT: BusinessPartner Migration Results
-- Description: Kiểm tra xem tất cả các thay đổi đã được áp dụng đúng chưa
-- ============================================================================

USE [VnsErp2025Final];
GO

SET ANSI_NULLS ON;
GO

SET QUOTED_IDENTIFIER ON;
GO

PRINT '========================================';
PRINT 'Verifying BusinessPartner Migration...';
PRINT '========================================';
PRINT '';

-- ============================================================================
-- SECTION 1: VERIFY BUSINESS PARTNER COLUMNS
-- ============================================================================

PRINT '--- Section 1: BusinessPartner Columns ---';
GO

DECLARE @Results TABLE (
    TableName NVARCHAR(255),
    ColumnName NVARCHAR(255),
    ExpectedDataType NVARCHAR(100),
    ActualDataType NVARCHAR(100),
    Status NVARCHAR(50)
);

-- Check BusinessPartner audit fields
INSERT INTO @Results
SELECT 
    'BusinessPartner' AS TableName,
    c.name AS ColumnName,
    CASE c.name
        WHEN 'CreatedBy' THEN 'UNIQUEIDENTIFIER'
        WHEN 'ModifiedBy' THEN 'UNIQUEIDENTIFIER'
        WHEN 'DeletedBy' THEN 'UNIQUEIDENTIFIER'
        WHEN 'IsDeleted' THEN 'BIT'
        WHEN 'DeletedDate' THEN 'DATETIME'
    END AS ExpectedDataType,
    ty.name + CASE 
        WHEN ty.name IN ('nvarchar', 'varchar', 'char', 'nchar') 
        THEN '(' + CASE WHEN c.max_length = -1 THEN 'MAX' ELSE CAST(c.max_length AS VARCHAR) END + ')'
        WHEN ty.name IN ('decimal', 'numeric') 
        THEN '(' + CAST(c.precision AS VARCHAR) + ',' + CAST(c.scale AS VARCHAR) + ')'
        ELSE ''
    END AS ActualDataType,
    CASE 
        WHEN c.name IN ('CreatedBy', 'ModifiedBy', 'DeletedBy', 'IsDeleted', 'DeletedDate')
        THEN '✓ FOUND'
        ELSE '✗ MISSING'
    END AS Status
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
WHERE t.name = 'BusinessPartner'
  AND c.name IN ('CreatedBy', 'ModifiedBy', 'DeletedBy', 'IsDeleted', 'DeletedDate');

SELECT * FROM @Results WHERE TableName = 'BusinessPartner';
GO

-- ============================================================================
-- SECTION 2: VERIFY BUSINESS PARTNER CATEGORY COLUMNS
-- ============================================================================

PRINT '';
PRINT '--- Section 2: BusinessPartnerCategory Columns ---';
GO

DECLARE @Results TABLE (
    TableName NVARCHAR(255),
    ColumnName NVARCHAR(255),
    Status NVARCHAR(50)
);

INSERT INTO @Results
SELECT 
    'BusinessPartnerCategory' AS TableName,
    c.name AS ColumnName,
    CASE 
        WHEN c.name IN ('CategoryCode', 'IsActive', 'SortOrder', 'CreatedDate', 'CreatedBy', 'ModifiedDate', 'ModifiedBy')
        THEN '✓ FOUND'
        ELSE '✗ MISSING'
    END AS Status
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
WHERE t.name = 'BusinessPartnerCategory'
  AND c.name IN ('CategoryCode', 'IsActive', 'SortOrder', 'CreatedDate', 'CreatedBy', 'ModifiedDate', 'ModifiedBy');

SELECT * FROM @Results;
GO

-- ============================================================================
-- SECTION 3: VERIFY BUSINESS PARTNER SITE COLUMNS
-- ============================================================================

PRINT '';
PRINT '--- Section 3: BusinessPartnerSite Columns ---';
GO

DECLARE @Results TABLE (
    TableName NVARCHAR(255),
    ColumnName NVARCHAR(255),
    Status NVARCHAR(50)
);

INSERT INTO @Results
SELECT 
    'BusinessPartnerSite' AS TableName,
    c.name AS ColumnName,
    CASE 
        WHEN c.name IN ('PostalCode', 'District', 'Latitude', 'Longitude', 'SiteType', 'Notes')
        THEN '✓ FOUND'
        ELSE '✗ MISSING'
    END AS Status
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
WHERE t.name = 'BusinessPartnerSite'
  AND c.name IN ('PostalCode', 'District', 'Latitude', 'Longitude', 'SiteType', 'Notes');

SELECT * FROM @Results;
GO

-- ============================================================================
-- SECTION 4: VERIFY BUSINESS PARTNER CONTACT COLUMNS
-- ============================================================================

PRINT '';
PRINT '--- Section 4: BusinessPartnerContact Columns ---';
GO

DECLARE @Results TABLE (
    TableName NVARCHAR(255),
    ColumnName NVARCHAR(255),
    Status NVARCHAR(50)
);

INSERT INTO @Results
SELECT 
    'BusinessPartnerContact' AS TableName,
    c.name AS ColumnName,
    CASE 
        WHEN c.name IN ('Mobile', 'Fax', 'Department', 'BirthDate', 'Gender', 
                       'LinkedIn', 'Skype', 'WeChat', 'Notes', 'AvatarPath', 
                       'CreatedDate', 'ModifiedDate')
        THEN '✓ FOUND'
        ELSE '✗ MISSING'
    END AS Status
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
WHERE t.name = 'BusinessPartnerContact'
  AND c.name IN ('Mobile', 'Fax', 'Department', 'BirthDate', 'Gender', 
                 'LinkedIn', 'Skype', 'WeChat', 'Notes', 'AvatarPath', 
                 'CreatedDate', 'ModifiedDate');

SELECT * FROM @Results;
GO

-- ============================================================================
-- SECTION 5: VERIFY CONSTRAINTS
-- ============================================================================

PRINT '';
PRINT '--- Section 5: Check Constraints ---';
GO

SELECT 
    'Check Constraints' AS ConstraintType,
    cc.name AS ConstraintName,
    OBJECT_NAME(cc.parent_object_id) AS TableName,
    cc.definition AS Definition,
    CASE 
        WHEN cc.name IN ('CK_BusinessPartner_EmailFormat', 
                        'CK_BusinessPartner_CreditLimit',
                        'CK_BusinessPartnerSite_EmailFormat',
                        'CK_BusinessPartnerContact_EmailFormat')
        THEN '✓ FOUND'
        ELSE '? UNEXPECTED'
    END AS Status
FROM sys.check_constraints cc
WHERE OBJECT_NAME(cc.parent_object_id) IN ('BusinessPartner', 'BusinessPartnerSite', 'BusinessPartnerContact')
  AND cc.name IN ('CK_BusinessPartner_EmailFormat', 
                  'CK_BusinessPartner_CreditLimit',
                  'CK_BusinessPartnerSite_EmailFormat',
                  'CK_BusinessPartnerContact_EmailFormat')
ORDER BY OBJECT_NAME(cc.parent_object_id), cc.name;
GO

-- ============================================================================
-- SECTION 6: VERIFY FOREIGN KEYS
-- ============================================================================

PRINT '';
PRINT '--- Section 6: Foreign Key Constraints ---';
GO

SELECT 
    'Foreign Keys' AS ConstraintType,
    fk.name AS ConstraintName,
    OBJECT_NAME(fk.parent_object_id) AS TableName,
    COL_NAME(fc.parent_object_id, fc.parent_column_id) AS ColumnName,
    OBJECT_NAME(fk.referenced_object_id) AS ReferencedTable,
    CASE 
        WHEN fk.name LIKE 'FK_BusinessPartner%CreatedBy%' 
          OR fk.name LIKE 'FK_BusinessPartner%ModifiedBy%'
          OR fk.name LIKE 'FK_BusinessPartner%DeletedBy%'
          OR fk.name LIKE 'FK_BusinessPartnerCategory%CreatedBy%'
          OR fk.name LIKE 'FK_BusinessPartnerCategory%ModifiedBy%'
          OR fk.name LIKE 'FK_BusinessPartner_BusinessPartnerCategory%CreatedBy%'
        THEN '✓ FOUND'
        ELSE '? OTHER'
    END AS Status
FROM sys.foreign_keys fk
INNER JOIN sys.foreign_key_columns fc ON fk.object_id = fc.constraint_object_id
WHERE OBJECT_NAME(fk.parent_object_id) IN ('BusinessPartner', 'BusinessPartnerCategory', 
                                           'BusinessPartner_BusinessPartnerCategory')
  AND (fk.name LIKE '%CreatedBy%' OR fk.name LIKE '%ModifiedBy%' OR fk.name LIKE '%DeletedBy%')
ORDER BY OBJECT_NAME(fk.parent_object_id), fk.name;
GO

-- ============================================================================
-- SECTION 7: VERIFY UNIQUE INDEXES
-- ============================================================================

PRINT '';
PRINT '--- Section 7: Unique Indexes ---';
GO

SELECT 
    'Unique Indexes' AS IndexType,
    i.name AS IndexName,
    OBJECT_NAME(i.object_id) AS TableName,
    CASE 
        WHEN i.name IN ('IX_BusinessPartnerSite_OneDefaultPerPartner',
                       'IX_BusinessPartnerContact_OnePrimaryPerSite')
        THEN '✓ FOUND'
        ELSE '? OTHER'
    END AS Status,
    i.is_unique AS IsUnique,
    i.filter_definition AS FilterDefinition
FROM sys.indexes i
WHERE OBJECT_NAME(i.object_id) IN ('BusinessPartnerSite', 'BusinessPartnerContact')
  AND i.name IN ('IX_BusinessPartnerSite_OneDefaultPerPartner',
                 'IX_BusinessPartnerContact_OnePrimaryPerSite')
ORDER BY OBJECT_NAME(i.object_id), i.name;
GO

-- ============================================================================
-- SECTION 8: VERIFY PERFORMANCE INDEXES
-- ============================================================================

PRINT '';
PRINT '--- Section 8: Performance Indexes ---';
GO

SELECT 
    'Performance Indexes' AS IndexType,
    i.name AS IndexName,
    OBJECT_NAME(i.object_id) AS TableName,
    CASE 
        WHEN i.name LIKE 'IX_BusinessPartner%' 
          OR i.name LIKE 'IX_BusinessPartnerSite%'
          OR i.name LIKE 'IX_BusinessPartnerContact%'
          OR i.name LIKE 'IX_BusinessPartnerCategory%'
        THEN '✓ FOUND'
        ELSE '? OTHER'
    END AS Status
FROM sys.indexes i
WHERE OBJECT_NAME(i.object_id) IN ('BusinessPartner', 'BusinessPartnerSite', 
                                   'BusinessPartnerContact', 'BusinessPartnerCategory')
  AND i.name LIKE 'IX_%'
  AND i.is_primary_key = 0
  AND i.is_unique_constraint = 0
ORDER BY OBJECT_NAME(i.object_id), i.name;
GO

-- ============================================================================
-- SECTION 9: SUMMARY REPORT
-- ============================================================================

PRINT '';
PRINT '========================================';
PRINT 'SUMMARY REPORT';
PRINT '========================================';
PRINT '';

-- Count columns
DECLARE @BusinessPartnerColumns INT = (
    SELECT COUNT(*) 
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = 'BusinessPartner'
      AND c.name IN ('CreatedBy', 'ModifiedBy', 'DeletedBy', 'IsDeleted', 'DeletedDate')
);

DECLARE @CategoryColumns INT = (
    SELECT COUNT(*) 
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = 'BusinessPartnerCategory'
      AND c.name IN ('CategoryCode', 'IsActive', 'SortOrder', 'CreatedDate', 'CreatedBy', 'ModifiedDate', 'ModifiedBy')
);

DECLARE @SiteColumns INT = (
    SELECT COUNT(*) 
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = 'BusinessPartnerSite'
      AND c.name IN ('PostalCode', 'District', 'Latitude', 'Longitude', 'SiteType', 'Notes')
);

DECLARE @ContactColumns INT = (
    SELECT COUNT(*) 
    FROM sys.columns c
    INNER JOIN sys.tables t ON c.object_id = t.object_id
    WHERE t.name = 'BusinessPartnerContact'
      AND c.name IN ('Mobile', 'Fax', 'Department', 'BirthDate', 'Gender', 
                     'LinkedIn', 'Skype', 'WeChat', 'Notes', 'AvatarPath', 
                     'CreatedDate', 'ModifiedDate')
);

DECLARE @CheckConstraints INT = (
    SELECT COUNT(*) 
    FROM sys.check_constraints cc
    WHERE OBJECT_NAME(cc.parent_object_id) IN ('BusinessPartner', 'BusinessPartnerSite', 'BusinessPartnerContact')
      AND cc.name IN ('CK_BusinessPartner_EmailFormat', 
                     'CK_BusinessPartner_CreditLimit',
                     'CK_BusinessPartnerSite_EmailFormat',
                     'CK_BusinessPartnerContact_EmailFormat')
);

DECLARE @UniqueIndexes INT = (
    SELECT COUNT(*) 
    FROM sys.indexes i
    WHERE OBJECT_NAME(i.object_id) IN ('BusinessPartnerSite', 'BusinessPartnerContact')
      AND i.name IN ('IX_BusinessPartnerSite_OneDefaultPerPartner',
                     'IX_BusinessPartnerContact_OnePrimaryPerSite')
);

DECLARE @PerformanceIndexes INT = (
    SELECT COUNT(*) 
    FROM sys.indexes i
    WHERE OBJECT_NAME(i.object_id) IN ('BusinessPartner', 'BusinessPartnerSite', 
                                      'BusinessPartnerContact', 'BusinessPartnerCategory')
      AND i.name LIKE 'IX_%'
      AND i.is_primary_key = 0
      AND i.is_unique_constraint = 0
);

PRINT 'BusinessPartner Audit Columns: ' + CAST(@BusinessPartnerColumns AS VARCHAR) + '/5';
PRINT 'BusinessPartnerCategory Columns: ' + CAST(@CategoryColumns AS VARCHAR) + '/7';
PRINT 'BusinessPartnerSite Columns: ' + CAST(@SiteColumns AS VARCHAR) + '/6';
PRINT 'BusinessPartnerContact Columns: ' + CAST(@ContactColumns AS VARCHAR) + '/12';
PRINT 'Check Constraints: ' + CAST(@CheckConstraints AS VARCHAR) + '/4';
PRINT 'Unique Indexes: ' + CAST(@UniqueIndexes AS VARCHAR) + '/2';
PRINT 'Performance Indexes: ' + CAST(@PerformanceIndexes AS VARCHAR);
PRINT '';

IF @BusinessPartnerColumns = 5 
   AND @CategoryColumns = 7 
   AND @SiteColumns = 6 
   AND @ContactColumns = 12
   AND @CheckConstraints = 4
   AND @UniqueIndexes = 2
BEGIN
    PRINT '✓✓✓ ALL CHECKS PASSED - Migration successful! ✓✓✓';
END
ELSE
BEGIN
    PRINT '⚠⚠⚠ SOME CHECKS FAILED - Please review the details above ⚠⚠⚠';
END

PRINT '';
PRINT '========================================';
GO

