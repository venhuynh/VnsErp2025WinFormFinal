# Migration: Add Logo Thumbnail Fields to BusinessPartner Table

This document outlines the migration to add logo thumbnail-related fields to the `dbo.BusinessPartner` table.

**Date:** 2025
**Version:** 1.0

---

## Purpose

To enable storing and managing logo thumbnails (smaller versions of logos) directly within the `BusinessPartner` table. Thumbnails are useful for:
- Faster loading in list views and grids
- Reduced bandwidth consumption
- Better user experience in data-intensive screens

---

## Changes Introduced

The `Migration_BusinessPartner_AddLogoThumbnail.sql` script adds the following columns to the `dbo.BusinessPartner` table:

| Column Name                  | Data Type          | Nullable | Description                                   |
|------------------------------|--------------------|----------|-----------------------------------------------|
| `LogoThumbnailImage`         | `VARBINARY(MAX)`   | YES      | Binary data of the logo thumbnail image.      |
| `LogoThumbnailFileName`      | `NVARCHAR(255)`    | YES      | Original file name of the logo thumbnail.    |
| `LogoThumbnailRelativePath` | `NVARCHAR(500)`    | YES      | Relative path to the thumbnail file (if stored externally). |
| `LogoThumbnailFullPath`      | `NVARCHAR(1000)`   | YES      | Full path to the thumbnail file (if stored externally).     |
| `LogoThumbnailStorageType`   | `NVARCHAR(20)`     | YES      | Type of storage (e.g., 'Database', 'FileSystem', 'NAS'). |
| `LogoThumbnailFileSize`      | `BIGINT`           | YES      | Size of the thumbnail file in bytes.          |
| `LogoThumbnailChecksum`      | `NVARCHAR(64)`     | YES      | SHA256 checksum of the thumbnail file for integrity verification. |

**Note:** Pattern này tương tự như `ProductService.ThumbnailImage` và `ProductVariant.ThumbnailImage` để đảm bảo tính nhất quán trong hệ thống.

---

## Migration Script

The migration script is located at:
`Database/Migrations/Migration_BusinessPartner_AddLogoThumbnail.sql`

---

## How to Run the Migration

1.  **Backup your database:** Always perform a full backup of your `VnsErp2025Final` database before running any migration script.
2.  **Execute the script:** Run the `Migration_BusinessPartner_AddLogoThumbnail.sql` script against your `VnsErp2025Final` database using SQL Server Management Studio (SSMS) or a similar tool.
3.  **Verify:** After execution, check the `dbo.BusinessPartner` table schema to ensure the new columns have been added. You can use the following query:
    ```sql
    SELECT COLUMN_NAME, DATA_TYPE, CHARACTER_MAXIMUM_LENGTH, IS_NULLABLE
    FROM INFORMATION_SCHEMA.COLUMNS
    WHERE TABLE_NAME = 'BusinessPartner' AND (COLUMN_NAME LIKE 'LogoThumbnail%' OR COLUMN_NAME = 'LogoThumbnailImage');
    ```

---

## Thumbnail Generation

Thumbnails should typically be generated when a logo is uploaded. Recommended practices:

1. **Size:** Common thumbnail sizes are 150x150, 200x200, or 300x300 pixels
2. **Format:** Usually JPEG for smaller file size, or PNG for transparency
3. **Quality:** Lower quality (70-80%) is acceptable for thumbnails to reduce file size
4. **Aspect Ratio:** Maintain the original aspect ratio to avoid distortion

---

## Rollback Plan (if needed)

If you need to revert these changes, you can use the following SQL commands. **Only do this if absolutely necessary and after restoring your database from a backup.**

```sql
BEGIN TRANSACTION;
GO

IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'LogoThumbnailChecksum')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoThumbnailChecksum;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'LogoThumbnailFileSize')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoThumbnailFileSize;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'LogoThumbnailStorageType')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoThumbnailStorageType;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'LogoThumbnailFullPath')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoThumbnailFullPath;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'LogoThumbnailRelativePath')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoThumbnailRelativePath;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'LogoThumbnailFileName')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoThumbnailFileName;
IF EXISTS (SELECT 1 FROM sys.columns WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'LogoThumbnailImage')
    ALTER TABLE dbo.BusinessPartner DROP COLUMN LogoThumbnailImage;

-- Drop indexes
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'IX_BusinessPartner_LogoThumbnailStorageType')
    DROP INDEX IX_BusinessPartner_LogoThumbnailStorageType ON dbo.BusinessPartner;
IF EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.BusinessPartner') AND name = 'IX_BusinessPartner_LogoThumbnailFileName')
    DROP INDEX IX_BusinessPartner_LogoThumbnailFileName ON dbo.BusinessPartner;

COMMIT TRANSACTION;
GO
```

---

**This document is automatically generated and reflects the current state of the migration script.**

