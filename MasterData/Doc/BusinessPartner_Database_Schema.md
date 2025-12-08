# Cấu Trúc Các Bảng BusinessPartner

Tài liệu này mô tả cấu trúc các bảng liên quan đến BusinessPartner sau khi được cải tiến và migration.

**Ngày cập nhật:** 2025  
**Phiên bản:** 2.0

---

## Mục Lục

1. [BusinessPartner](#1-businesspartner)
2. [BusinessPartnerCategory](#2-businesspartnercategory)
3. [BusinessPartnerSite](#3-businesspartnersite)
4. [BusinessPartnerContact](#4-businesspartnercontact)
5. [BusinessPartner_BusinessPartnerCategory](#5-businesspartner_businesspartnercategory)
6. [Ràng Buộc và Indexes](#6-ràng-buộc-và-indexes)
7. [Quan Hệ Giữa Các Bảng](#7-quan-hệ-giữa-các-bảng)

---

## 1. BusinessPartner

Bảng chính lưu trữ thông tin đối tác kinh doanh.

### 1.1. Các Trường Dữ Liệu

| Tên Trường | Kiểu Dữ Liệu | Nullable | Mô Tả |
|------------|--------------|----------|-------|
| `Id` | `UniqueIdentifier` | NOT NULL | Khóa chính (Primary Key) |
| `PartnerCode` | `NVarChar(50)` | NOT NULL | Mã đối tác (duy nhất) |
| `PartnerName` | `NVarChar(255)` | NOT NULL | Tên đối tác |
| `PartnerType` | `Int` | NOT NULL | Loại đối tác (1: Khách hàng, 2: Nhà cung cấp, ...) |
| `TaxCode` | `NVarChar(50)` | NULL | Mã số thuế |
| `Phone` | `NVarChar(50)` | NULL | Số điện thoại |
| `Email` | `NVarChar(100)` | NULL | Email (có validation format) |
| `Website` | `NVarChar(100)` | NULL | Website |
| `Address` | `NVarChar(255)` | NULL | Địa chỉ |
| `City` | `NVarChar(100)` | NULL | Thành phố |
| `Country` | `NVarChar(100)` | NULL | Quốc gia |
| `IsActive` | `Bit` | NOT NULL | Trạng thái hoạt động (mặc định: 1) |
| `Logo` | `VarBinary(MAX)` | NULL | Dữ liệu binary của logo |
| `LogoFileName` | `NVarChar(255)` | NULL | Tên file logo |
| `LogoRelativePath` | `NVarChar(500)` | NULL | Đường dẫn tương đối của logo |
| `LogoFullPath` | `NVarChar(1000)` | NULL | Đường dẫn đầy đủ của logo |
| `LogoStorageType` | `NVarChar(20)` | NULL | Loại storage (Database, FileSystem, NAS, etc.) |
| `LogoFileSize` | `BigInt` | NULL | Kích thước file logo (bytes) |
| `LogoChecksum` | `NVarChar(64)` | NULL | Checksum để verify integrity |
| `CreatedDate` | `DateTime` | NOT NULL | Ngày tạo |
| `UpdatedDate` | `DateTime` | NULL | Ngày cập nhật |
| `CreatedBy` | `UniqueIdentifier` | NULL | ID người tạo (FK → ApplicationUser.Id) |
| `ModifiedBy` | `UniqueIdentifier` | NULL | ID người sửa (FK → ApplicationUser.Id) |
| `DeletedBy` | `UniqueIdentifier` | NULL | ID người xóa (FK → ApplicationUser.Id) |
| `IsDeleted` | `Bit` | NOT NULL | Đánh dấu xóa mềm (mặc định: 0) |
| `DeletedDate` | `DateTime` | NULL | Ngày xóa |
| `Logo` | `VarBinary(MAX)` | NULL | Dữ liệu binary của logo |
| `LogoFileName` | `NVarChar(255)` | NULL | Tên file logo |
| `LogoRelativePath` | `NVarChar(500)` | NULL | Đường dẫn tương đối của logo |
| `LogoFullPath` | `NVarChar(1000)` | NULL | Đường dẫn đầy đủ của logo |
| `LogoStorageType` | `NVarChar(20)` | NULL | Loại storage (Database, FileSystem, NAS, etc.) |
| `LogoFileSize` | `BigInt` | NULL | Kích thước file logo (bytes) |
| `LogoChecksum` | `NVarChar(64)` | NULL | Checksum để verify integrity |

**Lưu ý:** Các cột sau đã bị xóa khỏi bảng (tách ra bảng riêng):
- `ContactPerson`, `ContactPosition` - Sử dụng bảng `BusinessPartnerContact`
- `BankAccount`, `BankName`, `CreditLimit`, `PaymentTerm` - Sẽ tách ra bảng riêng

### 1.2. Ràng Buộc

- **Primary Key:** `Id`
- **Unique Index:** `IX_BusinessPartner_PartnerCode` trên `PartnerCode`
- **Check Constraint:** 
  - `CK_BusinessPartner_EmailFormat`: Email phải có format hợp lệ (LIKE '%@%.%') hoặc NULL
- **Foreign Keys:**
  - `FK_BusinessPartner_CreatedBy` → `ApplicationUser(Id)`
  - `FK_BusinessPartner_ModifiedBy` → `ApplicationUser(Id)`
  - `FK_BusinessPartner_DeletedBy` → `ApplicationUser(Id)`

### 1.3. Indexes

- `IX_BusinessPartner_PartnerCode` (UNIQUE) trên `PartnerCode`
- `IX_BusinessPartner_PartnerName` trên `PartnerName`
- `IX_BusinessPartner_IsActive_IsDeleted` trên `IsActive`, `IsDeleted`
- `IX_BusinessPartner_CreatedDate` trên `CreatedDate`
- `IX_BusinessPartner_LogoFileName` (FILTERED) trên `LogoFileName` (WHERE LogoFileName IS NOT NULL)
- `IX_BusinessPartner_LogoStorageType` (FILTERED) trên `LogoStorageType` (WHERE LogoStorageType IS NOT NULL)

### 1.4. Các cột đã bị xóa (tách ra bảng riêng)

Các cột sau đã được xóa khỏi bảng `BusinessPartner` và sẽ được tách ra làm bảng riêng:
- `ContactPerson` - Người liên hệ (sử dụng bảng `BusinessPartnerContact`)
- `ContactPosition` - Chức vụ người liên hệ (sử dụng bảng `BusinessPartnerContact`)
- `BankAccount` - Số tài khoản ngân hàng (sẽ tách ra bảng riêng)
- `BankName` - Tên ngân hàng (sẽ tách ra bảng riêng)
- `CreditLimit` - Hạn mức tín dụng (sẽ tách ra bảng riêng)
- `PaymentTerm` - Điều khoản thanh toán (sẽ tách ra bảng riêng)

---

## 2. BusinessPartnerCategory

Bảng lưu trữ danh mục phân loại đối tác.

### 2.1. Các Trường Dữ Liệu

| Tên Trường | Kiểu Dữ Liệu | Nullable | Mô Tả |
|------------|--------------|----------|-------|
| `Id` | `UniqueIdentifier` | NOT NULL | Khóa chính (Primary Key) |
| `CategoryName` | `NVarChar(100)` | NOT NULL | Tên danh mục |
| `Description` | `NVarChar(255)` | NULL | Mô tả |
| `ParentId` | `UniqueIdentifier` | NULL | ID danh mục cha (FK → BusinessPartnerCategory.Id) |
| `CategoryCode` | `NVarChar(50)` | NULL | Mã danh mục |
| `IsActive` | `Bit` | NOT NULL | Trạng thái hoạt động (mặc định: 1) |
| `SortOrder` | `Int` | NULL | Thứ tự sắp xếp |
| `CreatedDate` | `DateTime` | NOT NULL | Ngày tạo |
| `CreatedBy` | `UniqueIdentifier` | NULL | ID người tạo (FK → ApplicationUser.Id) |
| `ModifiedDate` | `DateTime` | NULL | Ngày cập nhật |
| `ModifiedBy` | `UniqueIdentifier` | NULL | ID người sửa (FK → ApplicationUser.Id) |

### 2.2. Ràng Buộc

- **Primary Key:** `Id`
- **Foreign Keys:**
  - `FK_BusinessPartnerCategory_ParentId` → `BusinessPartnerCategory(Id)` (Self-referencing)
  - `FK_BusinessPartnerCategory_CreatedBy` → `ApplicationUser(Id)`
  - `FK_BusinessPartnerCategory_ModifiedBy` → `ApplicationUser(Id)`

### 2.3. Indexes

- `IX_BusinessPartnerCategory_CategoryName` trên `CategoryName`
- `IX_BusinessPartnerCategory_ParentId` trên `ParentId`
- `IX_BusinessPartnerCategory_IsActive` trên `IsActive`

---

## 3. BusinessPartnerSite

Bảng lưu trữ thông tin các địa điểm/chi nhánh của đối tác.

### 3.1. Các Trường Dữ Liệu

| Tên Trường | Kiểu Dữ Liệu | Nullable | Mô Tả |
|------------|--------------|----------|-------|
| `Id` | `UniqueIdentifier` | NOT NULL | Khóa chính (Primary Key) |
| `PartnerId` | `UniqueIdentifier` | NOT NULL | ID đối tác (FK → BusinessPartner.Id) |
| `SiteCode` | `NVarChar(50)` | NOT NULL | Mã địa điểm (duy nhất) |
| `SiteName` | `NVarChar(255)` | NOT NULL | Tên địa điểm |
| `Address` | `NVarChar(255)` | NULL | Địa chỉ |
| `City` | `NVarChar(100)` | NULL | Thành phố |
| `Province` | `NVarChar(100)` | NULL | Tỉnh/Thành phố |
| `Country` | `NVarChar(100)` | NULL | Quốc gia |
| `ContactPerson` | `NVarChar(100)` | NULL | Người liên hệ |
| `Phone` | `NVarChar(50)` | NULL | Số điện thoại |
| `Email` | `NVarChar(100)` | NULL | Email (có validation format) |
| `IsDefault` | `Bit` | NULL | Đánh dấu địa điểm mặc định (1 đối tác chỉ có 1 địa điểm mặc định) |
| `IsActive` | `Bit` | NOT NULL | Trạng thái hoạt động (mặc định: 1) |
| `CreatedDate` | `DateTime` | NOT NULL | Ngày tạo |
| `UpdatedDate` | `DateTime` | NULL | Ngày cập nhật |
| `PostalCode` | `NVarChar(20)` | NULL | Mã bưu điện |
| `District` | `NVarChar(100)` | NULL | Quận/Huyện |
| `Latitude` | `Decimal(10,8)` | NULL | Vĩ độ (GPS) |
| `Longitude` | `Decimal(11,8)` | NULL | Kinh độ (GPS) |
| `SiteType` | `Int` | NULL | Loại địa điểm (1: Trụ sở chính, 2: Chi nhánh, 3: Kho hàng, ...) |
| `Notes` | `NVarChar(1000)` | NULL | Ghi chú |

### 3.2. Ràng Buộc

- **Primary Key:** `Id`
- **Unique Index:** `IX_BusinessPartnerSite_SiteCode` trên `SiteCode`
- **Unique Index:** `IX_BusinessPartnerSite_OneDefaultPerPartner` trên `PartnerId`, `IsDefault` (WHERE IsDefault = 1)
- **Check Constraint:** 
  - `CK_BusinessPartnerSite_EmailFormat`: Email phải có format hợp lệ (LIKE '%@%.%') hoặc NULL
- **Foreign Keys:**
  - `FK_BusinessPartnerSite_PartnerId` → `BusinessPartner(Id)` (CASCADE DELETE)

### 3.3. Indexes

- `IX_BusinessPartnerSite_SiteCode` (UNIQUE) trên `SiteCode`
- `IX_BusinessPartnerSite_PartnerId` trên `PartnerId`
- `IX_BusinessPartnerSite_OneDefaultPerPartner` (UNIQUE, Filtered) trên `PartnerId`, `IsDefault` (WHERE IsDefault = 1)
- `IX_BusinessPartnerSite_IsActive` trên `IsActive`

---

## 4. BusinessPartnerContact

Bảng lưu trữ thông tin liên hệ của đối tác tại từng địa điểm.

### 4.1. Các Trường Dữ Liệu

| Tên Trường | Kiểu Dữ Liệu | Nullable | Mô Tả |
|------------|--------------|----------|-------|
| `Id` | `UniqueIdentifier` | NOT NULL | Khóa chính (Primary Key) |
| `SiteId` | `UniqueIdentifier` | NOT NULL | ID địa điểm (FK → BusinessPartnerSite.Id) |
| `FullName` | `NVarChar(100)` | NOT NULL | Họ và tên |
| `Position` | `NVarChar(100)` | NULL | Chức vụ |
| `Phone` | `NVarChar(50)` | NULL | Số điện thoại |
| `Email` | `NVarChar(100)` | NULL | Email (có validation format) |
| `IsPrimary` | `Bit` | NULL | Đánh dấu liên hệ chính (1 địa điểm chỉ có 1 liên hệ chính) |
| `Avatar` | `VarBinary(MAX)` | NULL | Ảnh đại diện (binary) |
| `IsActive` | `Bit` | NOT NULL | Trạng thái hoạt động (mặc định: 1) |
| `Mobile` | `NVarChar(50)` | NULL | Số điện thoại di động |
| `Fax` | `NVarChar(50)` | NULL | Số fax |
| `Department` | `NVarChar(100)` | NULL | Phòng ban |
| `BirthDate` | `DateTime` | NULL | Ngày sinh |
| `Gender` | `NVarChar(10)` | NULL | Giới tính |
| `LinkedIn` | `NVarChar(200)` | NULL | LinkedIn profile |
| `Skype` | `NVarChar(100)` | NULL | Skype ID |
| `WeChat` | `NVarChar(100)` | NULL | WeChat ID |
| `Notes` | `NVarChar(1000)` | NULL | Ghi chú |
| `AvatarPath` | `NVarChar(500)` | NULL | Đường dẫn file ảnh đại diện |
| `CreatedDate` | `DateTime` | NOT NULL | Ngày tạo |
| `ModifiedDate` | `DateTime` | NULL | Ngày cập nhật |

### 4.2. Ràng Buộc

- **Primary Key:** `Id`
- **Unique Index:** `IX_BusinessPartnerContact_OnePrimaryPerSite` trên `SiteId`, `IsPrimary` (WHERE IsPrimary = 1)
- **Check Constraint:** 
  - `CK_BusinessPartnerContact_EmailFormat`: Email phải có format hợp lệ (LIKE '%@%.%') hoặc NULL
- **Foreign Keys:**
  - `FK_BusinessPartnerContact_SiteId` → `BusinessPartnerSite(Id)` (CASCADE DELETE)

### 4.3. Indexes

- `IX_BusinessPartnerContact_SiteId` trên `SiteId`
- `IX_BusinessPartnerContact_OnePrimaryPerSite` (UNIQUE, Filtered) trên `SiteId`, `IsPrimary` (WHERE IsPrimary = 1)
- `IX_BusinessPartnerContact_Email` trên `Email` (WHERE Email IS NOT NULL)
- `IX_BusinessPartnerContact_Phone` trên `Phone` (WHERE Phone IS NOT NULL)
- `IX_BusinessPartnerContact_IsActive` trên `IsActive`

---

## 5. BusinessPartner_BusinessPartnerCategory

Bảng liên kết nhiều-nhiều giữa BusinessPartner và BusinessPartnerCategory (Junction Table).

### 5.1. Các Trường Dữ Liệu

| Tên Trường | Kiểu Dữ Liệu | Nullable | Mô Tả |
|------------|--------------|----------|-------|
| `PartnerId` | `UniqueIdentifier` | NOT NULL | ID đối tác (FK → BusinessPartner.Id, Primary Key) |
| `CategoryId` | `UniqueIdentifier` | NOT NULL | ID danh mục (FK → BusinessPartnerCategory.Id, Primary Key) |
| `CreatedDate` | `DateTime` | NOT NULL | Ngày tạo |
| `CreatedBy` | `UniqueIdentifier` | NULL | ID người tạo (FK → ApplicationUser.Id) |

### 5.2. Ràng Buộc

- **Primary Key:** Composite (`PartnerId`, `CategoryId`)
- **Foreign Keys:**
  - `FK_BusinessPartner_BusinessPartnerCategory_PartnerId` → `BusinessPartner(Id)` (CASCADE DELETE)
  - `FK_BusinessPartner_BusinessPartnerCategory_CategoryId` → `BusinessPartnerCategory(Id)` (CASCADE DELETE)
  - `FK_BusinessPartner_BusinessPartnerCategory_CreatedBy` → `ApplicationUser(Id)`

### 5.3. Indexes

- `IX_BusinessPartner_BusinessPartnerCategory_PartnerId` trên `PartnerId`
- `IX_BusinessPartner_BusinessPartnerCategory_CategoryId` trên `CategoryId`

---

## 6. Ràng Buộc và Indexes

### 6.1. Check Constraints

#### BusinessPartner
- `CK_BusinessPartner_EmailFormat`: Email phải có format hợp lệ (`Email LIKE '%@%.%' OR Email IS NULL`)

#### BusinessPartnerSite
- `CK_BusinessPartnerSite_EmailFormat`: Email phải có format hợp lệ (`Email LIKE '%@%.%' OR Email IS NULL`)

#### BusinessPartnerContact
- `CK_BusinessPartnerContact_EmailFormat`: Email phải có format hợp lệ (`Email LIKE '%@%.%' OR Email IS NULL`)

### 6.2. Unique Indexes (Filtered)

#### BusinessPartnerSite
- `IX_BusinessPartnerSite_OneDefaultPerPartner`: Đảm bảo mỗi đối tác chỉ có 1 địa điểm mặc định
  - Columns: `PartnerId`, `IsDefault`
  - Filter: `WHERE IsDefault = 1`

#### BusinessPartnerContact
- `IX_BusinessPartnerContact_OnePrimaryPerSite`: Đảm bảo mỗi địa điểm chỉ có 1 liên hệ chính
  - Columns: `SiteId`, `IsPrimary`
  - Filter: `WHERE IsPrimary = 1`

### 6.3. Performance Indexes

Các indexes được tạo để tối ưu hiệu suất truy vấn:

- **BusinessPartner:**
  - `IX_BusinessPartner_PartnerCode` (UNIQUE)
  - `IX_BusinessPartner_PartnerName`
  - `IX_BusinessPartner_IsActive_IsDeleted`
  - `IX_BusinessPartner_CreatedDate`

- **BusinessPartnerSite:**
  - `IX_BusinessPartnerSite_SiteCode` (UNIQUE)
  - `IX_BusinessPartnerSite_PartnerId`
  - `IX_BusinessPartnerSite_IsActive`

- **BusinessPartnerContact:**
  - `IX_BusinessPartnerContact_SiteId`
  - `IX_BusinessPartnerContact_Email` (Filtered: WHERE Email IS NOT NULL)
  - `IX_BusinessPartnerContact_Phone` (Filtered: WHERE Phone IS NOT NULL)
  - `IX_BusinessPartnerContact_IsActive`

---

## 7. Quan Hệ Giữa Các Bảng

### 7.1. Sơ Đồ Quan Hệ

```
BusinessPartner (1) ──< (N) BusinessPartnerSite
BusinessPartnerSite (1) ──< (N) BusinessPartnerContact
BusinessPartner (N) ──< (N) BusinessPartnerCategory [qua BusinessPartner_BusinessPartnerCategory]
BusinessPartnerCategory (1) ──< (N) BusinessPartnerCategory [Self-referencing: ParentId]
ApplicationUser (1) ──< (N) BusinessPartner [CreatedBy, ModifiedBy, DeletedBy]
ApplicationUser (1) ──< (N) BusinessPartnerCategory [CreatedBy, ModifiedBy]
ApplicationUser (1) ──< (N) BusinessPartner_BusinessPartnerCategory [CreatedBy]
```

### 7.2. Các Quan Hệ Chi Tiết

#### BusinessPartner → BusinessPartnerSite
- **Quan hệ:** 1-N (Một đối tác có nhiều địa điểm)
- **Foreign Key:** `BusinessPartnerSite.PartnerId` → `BusinessPartner.Id`
- **Cascade Delete:** Có (khi xóa đối tác, tự động xóa các địa điểm)

#### BusinessPartnerSite → BusinessPartnerContact
- **Quan hệ:** 1-N (Một địa điểm có nhiều liên hệ)
- **Foreign Key:** `BusinessPartnerContact.SiteId` → `BusinessPartnerSite.Id`
- **Cascade Delete:** Có (khi xóa địa điểm, tự động xóa các liên hệ)

#### BusinessPartner ↔ BusinessPartnerCategory
- **Quan hệ:** N-N (Nhiều đối tác thuộc nhiều danh mục)
- **Junction Table:** `BusinessPartner_BusinessPartnerCategory`
- **Foreign Keys:**
  - `BusinessPartner_BusinessPartnerCategory.PartnerId` → `BusinessPartner.Id` (CASCADE DELETE)
  - `BusinessPartner_BusinessPartnerCategory.CategoryId` → `BusinessPartnerCategory.Id` (CASCADE DELETE)

#### BusinessPartnerCategory (Self-referencing)
- **Quan hệ:** 1-N (Một danh mục có nhiều danh mục con)
- **Foreign Key:** `BusinessPartnerCategory.ParentId` → `BusinessPartnerCategory.Id`

#### ApplicationUser → BusinessPartner
- **Quan hệ:** 1-N (Một user tạo/sửa/xóa nhiều đối tác)
- **Foreign Keys:**
  - `BusinessPartner.CreatedBy` → `ApplicationUser.Id`
  - `BusinessPartner.ModifiedBy` → `ApplicationUser.Id`
  - `BusinessPartner.DeletedBy` → `ApplicationUser.Id`

---

## 8. Các Cải Tiến Đã Thực Hiện

### 8.1. Audit Fields
- Thêm các trường audit (`CreatedBy`, `ModifiedBy`, `DeletedBy`, `CreatedDate`, `ModifiedDate`, `DeletedDate`)
- Hỗ trợ soft delete với `IsDeleted` và `DeletedDate`

### 8.2. Data Validation
- Thêm check constraints cho email format
- Xóa các cột liên hệ, ngân hàng, thanh toán (ContactPerson, ContactPosition, BankAccount, BankName, CreditLimit, PaymentTerm)

### 8.3. Data Integrity
- Unique filtered indexes đảm bảo mỗi đối tác chỉ có 1 địa điểm mặc định
- Unique filtered indexes đảm bảo mỗi địa điểm chỉ có 1 liên hệ chính

### 8.4. Performance Optimization
- Thêm các indexes để tối ưu truy vấn theo PartnerCode, PartnerName, Email, Phone
- Filtered indexes cho các trường có thể NULL

### 8.5. Extended Fields
- **BusinessPartnerSite:** Thêm PostalCode, District, Latitude, Longitude, SiteType, Notes
- **BusinessPartnerContact:** Thêm Mobile, Fax, Department, BirthDate, Gender, LinkedIn, Skype, WeChat, Notes, AvatarPath
- **BusinessPartnerCategory:** Thêm CategoryCode, IsActive, SortOrder

---

## 9. Lưu Ý Khi Sử Dụng

### 9.1. Soft Delete
- Tất cả queries nên filter `IsDeleted = 0` để loại trừ các bản ghi đã bị xóa
- Repository layer đã tự động filter các bản ghi đã bị xóa

### 9.2. Email Validation
- Email phải có format hợp lệ (chứa '@' và '.') hoặc NULL
- Check constraint sẽ tự động validate khi insert/update

### 9.3. Default Site và Primary Contact
- Mỗi đối tác chỉ nên có 1 địa điểm mặc định (`IsDefault = 1`)
- Mỗi địa điểm chỉ nên có 1 liên hệ chính (`IsPrimary = 1`)
- Unique filtered indexes sẽ đảm bảo ràng buộc này

### 9.4. Các cột đã bị xóa (tách ra bảng riêng)

Các cột sau đã được xóa khỏi bảng `BusinessPartner`:
- `ContactPerson`, `ContactPosition` - Sử dụng bảng `BusinessPartnerContact` thay thế
- `BankAccount`, `BankName`, `CreditLimit`, `PaymentTerm` - Sẽ tách ra bảng riêng

Xem migration script: `Migration_BusinessPartner_RemoveContactBankFields.sql`

---

## 10. Migration Scripts

Các migration scripts được lưu trong:
- `Database/Migrations/Migration_BusinessPartner_Improvements.sql` - Script migration chính (thêm audit fields, constraints, indexes)
- `Database/Migrations/Migration_BusinessPartner_AddLogo.sql` - Script thêm các cột Logo cho BusinessPartner
- `Database/Migrations/Migration_BusinessPartner_RemoveContactBankFields.sql` - Script xóa các cột liên hệ, ngân hàng, thanh toán
- `Database/Migrations/Fix_Existing_Data.sql` - Script cleanup dữ liệu trước khi migration
- `Database/Migrations/Verify_Migration_Results.sql` - Script kiểm tra kết quả migration

### 10.1. Logo Columns Migration

Migration script `Migration_BusinessPartner_AddLogo.sql` thêm các cột để lưu trữ logo của BusinessPartner:
- Tham khảo: Bảng `Company` có cấu trúc tương tự
- Tất cả các cột đều NULL, không ảnh hưởng đến dữ liệu hiện có
- Xem chi tiết trong `README_BusinessPartner_Logo_Migration.md`

### 10.2. Remove Contact/Bank/Payment Fields Migration

Migration script `Migration_BusinessPartner_RemoveContactBankFields.sql` xóa các cột:
- `ContactPerson`, `ContactPosition` - Sử dụng bảng `BusinessPartnerContact` thay thế
- `BankAccount`, `BankName`, `CreditLimit`, `PaymentTerm` - Sẽ tách ra bảng riêng

⚠️ **CẢNH BÁO**: Script này sẽ XÓA VĨNH VIỄN dữ liệu. Bắt buộc phải backup trước khi chạy!
- Xem chi tiết trong `README_BusinessPartner_RemoveContactBankFields.md`

---

**Tài liệu này được tạo tự động từ database schema và migration scripts.**

