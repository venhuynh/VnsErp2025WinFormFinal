# BusinessPartner Tables Migration Guide

## Tổng quan

Script migration này cải tiến các bảng BusinessPartner và các bảng liên quan với các tính năng:
- Audit trail (theo dõi người tạo/sửa/xóa)
- Soft delete (xóa mềm)
- Validation constraints
- Performance indexes
- Các trường dữ liệu bổ sung

## Các bảng được cải tiến

1. **BusinessPartner** - Bảng đối tác chính
2. **BusinessPartnerCategory** - Danh mục đối tác
3. **BusinessPartnerSite** - Địa điểm/chi nhánh
4. **BusinessPartnerContact** - Người liên hệ
5. **BusinessPartner_BusinessPartnerCategory** - Junction table

## Các cải tiến chính

### 1. Audit Trail Fields
- `CreatedBy`, `ModifiedBy`, `DeletedBy` (UNIQUEIDENTIFIER)
- `IsDeleted` (BIT) - Soft delete flag
- `DeletedDate` (DATETIME)
- `CreatedDate`, `ModifiedDate` cho các bảng cần thiết

### 2. BusinessPartner Improvements
- ✅ Audit fields (CreatedBy, ModifiedBy, DeletedBy, IsDeleted, DeletedDate)
- ✅ Email format validation
- ✅ CreditLimit validation (>= 0)
- ✅ Performance indexes

### 3. BusinessPartnerCategory Improvements
- ✅ `CategoryCode` (NVARCHAR(50)) - Mã danh mục
- ✅ `IsActive` (BIT) - Trạng thái hoạt động
- ✅ `SortOrder` (INT) - Thứ tự sắp xếp
- ✅ Audit fields (CreatedDate, CreatedBy, ModifiedDate, ModifiedBy)

### 4. BusinessPartnerSite Improvements
- ✅ `PostalCode` (NVARCHAR(20)) - Mã bưu điện
- ✅ `District` (NVARCHAR(100)) - Quận/Huyện
- ✅ `Latitude`, `Longitude` (DECIMAL) - Tọa độ địa lý
- ✅ `SiteType` (INT) - Loại địa điểm (0: Trụ sở, 1: Chi nhánh, 2: Kho hàng, 3: Văn phòng đại diện)
- ✅ `Notes` (NVARCHAR(1000))
- ✅ Email format validation
- ✅ Unique constraint: Chỉ 1 site mặc định mỗi partner

### 5. BusinessPartnerContact Improvements
- ✅ `Mobile` (NVARCHAR(50)) - Số điện thoại di động
- ✅ `Fax` (NVARCHAR(50)) - Số fax
- ✅ `Department` (NVARCHAR(100)) - Phòng ban
- ✅ `BirthDate` (DATE) - Ngày sinh
- ✅ `Gender` (NVARCHAR(10)) - Giới tính
- ✅ `LinkedIn`, `Skype`, `WeChat` - Social media
- ✅ `Notes` (NVARCHAR(1000))
- ✅ `AvatarPath` (NVARCHAR(500)) - Đường dẫn ảnh đại diện
- ✅ `CreatedDate`, `ModifiedDate` - Audit fields
- ✅ Email format validation
- ✅ Unique constraint: Chỉ 1 primary contact mỗi site

### 6. Performance Indexes
- ✅ `IX_BusinessPartner_PartnerCode`
- ✅ `IX_BusinessPartner_PartnerType`
- ✅ `IX_BusinessPartner_IsActive`
- ✅ `IX_BusinessPartner_IsDeleted`
- ✅ `IX_BusinessPartner_TaxCode` (filtered)
- ✅ `IX_BusinessPartnerSite_PartnerId`
- ✅ `IX_BusinessPartnerSite_IsActive`
- ✅ `IX_BusinessPartnerSite_SiteType` (filtered)
- ✅ `IX_BusinessPartnerContact_SiteId`
- ✅ `IX_BusinessPartnerContact_IsActive`
- ✅ `IX_BusinessPartnerCategory_ParentId` (filtered)
- ✅ `IX_BusinessPartnerCategory_CategoryCode` (filtered)
- ✅ `IX_BusinessPartnerCategory_IsActive`

## Cách sử dụng

### Bước 1: Backup Database
```sql
BACKUP DATABASE [VnsErp2025Final] 
TO DISK = 'C:\Backups\VnsErp2025Final_Before_BusinessPartner_Migration.bak'
WITH COMPRESSION, INIT;
```

### Bước 2: Cleanup Data (Khuyến nghị)
Chạy script cleanup data trước để tránh lỗi:
```sql
-- Option 1: Chạy Cleanup_BusinessPartner_Data.sql (cho lần đầu)
-- Option 2: Nếu đã chạy migration và gặp lỗi, chạy Fix_Existing_Data.sql
```

**Scripts có sẵn:**
- `Cleanup_BusinessPartner_Data.sql` - Cleanup data trước khi migration (khuyến nghị)
- `Fix_Existing_Data.sql` - Fix data nếu đã gặp lỗi trong migration

### Bước 3: Review Script
- Mở file `Migration_BusinessPartner_Improvements.sql`
- Review các thay đổi
- Kiểm tra tên database (mặc định: `VnsErp2025Final`)

### Bước 4: Chạy Script Migration
```sql
-- Option 1: Chạy trực tiếp trong SQL Server Management Studio
-- Mở file và Execute (F5)

-- Option 2: Chạy từ command line
sqlcmd -S ServerName -d VnsErp2025Final -i Migration_BusinessPartner_Improvements.sql
```

**Lưu ý:** Script migration đã được cập nhật để tự động cleanup data trước khi tạo constraints/indexes, nhưng vẫn nên chạy cleanup script trước để đảm bảo.

### Bước 4: Verify Changes
```sql
-- Kiểm tra các cột mới đã được thêm
SELECT 
    t.name AS TableName,
    c.name AS ColumnName,
    ty.name AS DataType,
    c.max_length AS MaxLength
FROM sys.tables t
INNER JOIN sys.columns c ON t.object_id = c.object_id
INNER JOIN sys.types ty ON c.user_type_id = ty.user_type_id
WHERE t.name IN ('BusinessPartner', 'BusinessPartnerCategory', 'BusinessPartnerSite', 'BusinessPartnerContact')
  AND c.name IN ('CreatedBy', 'IsDeleted', 'CategoryCode', 'PostalCode', 'SiteType', 'Mobile')
ORDER BY t.name, c.name;

-- Kiểm tra indexes
SELECT 
    t.name AS TableName,
    i.name AS IndexName,
    i.type_desc AS IndexType
FROM sys.tables t
INNER JOIN sys.indexes i ON t.object_id = i.object_id
WHERE t.name IN ('BusinessPartner', 'BusinessPartnerCategory', 'BusinessPartnerSite', 'BusinessPartnerContact')
  AND i.name LIKE 'IX_%'
ORDER BY t.name, i.name;
```

### Bước 5: Data Migration (Optional)
Nếu cần migrate dữ liệu hiện có, uncomment Section 7 trong script và điều chỉnh theo nhu cầu.

## Rollback Plan

Nếu cần rollback, tạo script riêng để:
1. Drop các indexes mới
2. Drop các constraints mới
3. Drop các cột mới (cẩn thận với dữ liệu)

```sql
-- Example Rollback Script (tạo riêng nếu cần)
BEGIN TRANSACTION;

-- Drop indexes
DROP INDEX IF EXISTS IX_BusinessPartner_PartnerCode ON dbo.BusinessPartner;
-- ... (thêm các DROP INDEX khác)

-- Drop constraints
ALTER TABLE dbo.BusinessPartner DROP CONSTRAINT IF EXISTS CK_BusinessPartner_EmailFormat;
-- ... (thêm các DROP CONSTRAINT khác)

-- Drop columns (CẨN THẬN - sẽ mất dữ liệu)
-- ALTER TABLE dbo.BusinessPartner DROP COLUMN IF EXISTS CreatedBy;
-- ... (chỉ làm nếu chắc chắn)

COMMIT TRANSACTION;
```

## Lưu ý quan trọng

### ⚠️ Trước khi chạy:
1. **Backup database** - Bắt buộc!
2. **Test trên môi trường DEV/STAGING** trước
3. **Kiểm tra dependencies** - Đảm bảo ApplicationUser table tồn tại
4. **Thời gian chạy** - Script có thể mất vài phút tùy số lượng dữ liệu

### ⚠️ Sau khi chạy:
1. **Update Application Code** - Cập nhật code để sử dụng các trường mới
2. **Update DTOs** - Thêm các properties mới vào DTO classes
3. **Update Business Logic** - Xử lý IsDeleted, audit fields
4. **Test thoroughly** - Test tất cả các chức năng liên quan

### ⚠️ Constraints:
- Unique constraint `IX_BusinessPartnerSite_OneDefaultPerPartner`: Chỉ 1 site có `IsDefault = 1` mỗi partner
- Unique constraint `IX_BusinessPartnerContact_OnePrimaryPerSite`: Chỉ 1 contact có `IsPrimary = 1` mỗi site
- Email validation: Format phải chứa '@' và '.'
- CreditLimit: Phải >= 0

## SiteType Values

| Value | Description |
|-------|-------------|
| 0 | Trụ sở chính |
| 1 | Chi nhánh |
| 2 | Kho hàng |
| 3 | Văn phòng đại diện |
| NULL | Chưa xác định |

## Gender Values

| Value | Description |
|-------|-------------|
| 'Male' | Nam |
| 'Female' | Nữ |
| 'Other' | Khác |
| NULL | Chưa xác định |

## Cập nhật Code sau Migration

### 1. Update Entity Classes (nếu dùng LINQ to SQL)
- Regenerate `.dbml` file hoặc update designer
- Hoặc manually thêm properties vào partial classes

### 2. Update DTOs
```csharp
// Example: BusinessPartnerDto.cs
public Guid? CreatedBy { get; set; }
public bool IsDeleted { get; set; }
public DateTime? DeletedDate { get; set; }
```

### 3. Update Repository/BLL
```csharp
// Example: Soft delete method
public void SoftDelete(Guid partnerId, Guid deletedBy)
{
    var partner = GetById(partnerId);
    partner.IsDeleted = true;
    partner.DeletedDate = DateTime.Now;
    partner.DeletedBy = deletedBy;
    context.SubmitChanges();
}
```

### 4. Update Queries
```csharp
// Filter out deleted records
var activePartners = context.BusinessPartners
    .Where(p => !p.IsDeleted && p.IsActive)
    .ToList();
```

## Troubleshooting

### Lỗi: Email format validation fails
**Lỗi:** `Msg 547, Level 16, State 0 - The ALTER TABLE statement conflicted with the CHECK constraint`

**Nguyên nhân**: Có dữ liệu email không đúng format trong BusinessPartnerSite hoặc BusinessPartnerContact

**Giải pháp**: 
1. Chạy script `Fix_Existing_Data.sql` để tự động fix
2. Hoặc chạy thủ công:
```sql
-- Fix invalid emails in BusinessPartnerSite
UPDATE BusinessPartnerSite
SET Email = NULL
WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%';

-- Fix invalid emails in BusinessPartnerContact
UPDATE BusinessPartnerContact
SET Email = NULL
WHERE Email IS NOT NULL AND Email NOT LIKE '%@%.%';
```

### Lỗi: Unique index violation - Multiple default sites
**Lỗi:** `Msg 1505, Level 16, State 1 - duplicate key was found... IX_BusinessPartnerSite_OneDefaultPerPartner`

**Nguyên nhân**: Có nhiều hơn 1 record với IsDefault = 1 cho cùng một PartnerId

**Giải pháp**: 
1. Chạy script `Fix_Existing_Data.sql` để tự động fix
2. Hoặc chạy thủ công:
```sql
-- Fix multiple default sites (keep oldest, set others to NULL)
WITH RankedSites AS (
    SELECT Id, PartnerId, IsDefault,
           ROW_NUMBER() OVER (PARTITION BY PartnerId ORDER BY CreatedDate ASC, Id ASC) AS rn
    FROM BusinessPartnerSite
    WHERE PartnerId IN (
        SELECT PartnerId 
        FROM BusinessPartnerSite 
        WHERE IsDefault = 1
        GROUP BY PartnerId 
        HAVING COUNT(*) > 1
    )
    AND IsDefault = 1
)
UPDATE BusinessPartnerSite
SET IsDefault = NULL
WHERE Id IN (SELECT Id FROM RankedSites WHERE rn > 1);
```

### Lỗi: Unique index violation - Multiple primary contacts
**Lỗi:** `Msg 1505, Level 16, State 1 - duplicate key was found... IX_BusinessPartnerContact_OnePrimaryPerSite`

**Nguyên nhân**: Có nhiều hơn 1 record với IsPrimary = 1 cho cùng một SiteId

**Giải pháp**: 
1. Chạy script `Fix_Existing_Data.sql` để tự động fix
2. Hoặc chạy thủ công:
```sql
-- Fix multiple primary contacts (keep oldest, set others to 0)
WITH RankedContacts AS (
    SELECT Id, SiteId, IsPrimary,
           ROW_NUMBER() OVER (
               PARTITION BY SiteId 
               ORDER BY 
                   CASE WHEN CreatedDate IS NOT NULL THEN CreatedDate ELSE '1900-01-01' END ASC,
                   Id ASC
           ) AS rn
    FROM BusinessPartnerContact
    WHERE SiteId IN (
        SELECT SiteId 
        FROM BusinessPartnerContact 
        WHERE IsPrimary = 1
        GROUP BY SiteId 
        HAVING COUNT(*) > 1
    )
    AND IsPrimary = 1
)
UPDATE BusinessPartnerContact
SET IsPrimary = 0
WHERE Id IN (SELECT Id FROM RankedContacts WHERE rn > 1);
```

### Lỗi: COMMIT TRANSACTION request has no corresponding BEGIN TRANSACTION
**Lỗi:** `Msg 3902, Level 16, State 1 - The COMMIT TRANSACTION request has no corresponding BEGIN TRANSACTION`

**Nguyên nhân**: Transaction đã bị rollback do lỗi trước đó, nhưng script vẫn cố commit

**Giải pháp**: 
- Script đã được cập nhật để kiểm tra transaction state trước khi commit
- Nếu gặp lỗi này, chạy `Fix_Existing_Data.sql` để fix data, sau đó chạy lại migration
- Hoặc rollback thủ công và fix data trước:
```sql
IF @@TRANCOUNT > 0
    ROLLBACK TRANSACTION;
```

### Lỗi: Foreign key constraint fails
**Nguyên nhân**: ApplicationUser table chưa có dữ liệu hoặc Id không tồn tại

**Giải pháp**: Set CreatedBy/ModifiedBy = NULL cho các record cũ, hoặc tạo default user:
```sql
-- Set NULL for non-existent user IDs
UPDATE BusinessPartner
SET CreatedBy = NULL
WHERE CreatedBy IS NOT NULL 
  AND NOT EXISTS (SELECT 1 FROM ApplicationUser WHERE Id = BusinessPartner.CreatedBy);
```

## Support

Nếu gặp vấn đề, kiểm tra:
1. SQL Server version (yêu cầu SQL Server 2016+)
2. Database permissions (cần ALTER TABLE, CREATE INDEX)
3. Transaction log space
4. Error messages trong SQL Server logs

## Version History

- **v1.0** (2025): Initial migration script với tất cả các cải tiến đề xuất

