# VNS ERP 2025 - Tài Liệu Database

**Phiên bản:** 1.0  
**Ngày cập nhật:** 27/01/2025  
**Trạng thái:** Đang phát triển

---

## 1. Tổng Quan

Tài liệu này mô tả cấu trúc database, các bảng chính, relationships, và các migration scripts của hệ thống VNS ERP 2025.

### 1.1 Thông Tin Database
- **Database Engine:** Microsoft SQL Server
- **ORM:** LINQ to SQL (Drag & Drop Approach)
- **Connection:** ADO.NET với connection pooling
- **Naming Convention:** snake_case (tiếng Việt không dấu)

---

## 2. Cấu Trúc Database

### 2.1 Schema Chính

```
VnsErp2025 Database
├── VersionAndUserManagement Schema
│   ├── ApplicationUser
│   ├── ApplicationVersion
│   ├── Role
│   ├── Permission
│   ├── RolePermission
│   ├── UserRole
│   ├── UserPermission
│   └── AllowedMacAddress
├── MasterData Schema
│   ├── Company
│   ├── CompanyBranch
│   ├── Department
│   ├── BusinessPartner
│   ├── BusinessPartnerContact
│   ├── BusinessPartnerCategory
│   ├── ProductService
│   ├── ProductServiceCategory
│   ├── ProductVariant
│   └── ProductImage
├── Inventory Schema
│   ├── StockInOutMaster
│   ├── StockInOutDetail
│   ├── InventoryBalance
│   └── Asset
└── System Schema
    ├── Log
    └── Configuration
```

---

## 3. VersionAndUserManagement Schema

### 3.1 ApplicationUser
**Mục đích:** Lưu trữ thông tin người dùng hệ thống

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `UserName` (NVarChar(50))
- `HassPassword` (NVarChar(500)) - Password đã hash
- `Active` (Bit)
- `CreatedDate` (DateTime)
- `LastLoginDate` (DateTime)

### 3.2 Role
**Mục đích:** Lưu trữ thông tin vai trò

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `Name` (NVarChar(100))
- `Description` (NVarChar(500))
- `IsSystemRole` (Bit)
- `IsActive` (Bit)
- `CreatedDate` (DateTime)

**Tài liệu liên quan:**
- **[RoleDto](../DTO/VersionAndUserManagementDto/RoleDto.cs)**

### 3.3 Permission
**Mục đích:** Lưu trữ thông tin quyền truy cập

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `EntityName` (NVarChar(100)) - Tên entity (ví dụ: ProductService, BusinessPartner)
- `Action` (NVarChar(50)) - Hành động (Read, Create, Update, Delete)
- `Description` (NVarChar(500))
- `IsActive` (Bit)
- `CreatedDate` (DateTime)

**Tài liệu liên quan:**
- **[Entity Permission Matrix](../Dal/Doc/EntityPermissionMatrix.md)**
- **[Entity Permission Management Proposal](../Dal/Doc/EntityPermissionManagement_Proposal.md)**

### 3.4 RolePermission
**Mục đích:** Liên kết Role và Permission (Many-to-Many)

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `RoleId` (UniqueIdentifier, FK → Role)
- `PermissionId` (UniqueIdentifier, FK → Permission)
- `IsGranted` (Bit)
- `CreatedDate` (DateTime)
- `CreatedBy` (UniqueIdentifier, FK → ApplicationUser)

### 3.5 UserRole
**Mục đích:** Liên kết User và Role (Many-to-Many)

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `UserId` (UniqueIdentifier, FK → ApplicationUser)
- `RoleId` (UniqueIdentifier, FK → Role)
- `IsActive` (Bit)
- `AssignedDate` (DateTime)
- `AssignedBy` (UniqueIdentifier, FK → ApplicationUser)

### 3.6 UserPermission
**Mục đích:** Quyền trực tiếp của User (Override quyền từ Role)

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `UserId` (UniqueIdentifier, FK → ApplicationUser)
- `PermissionId` (UniqueIdentifier, FK → Permission)
- `IsGranted` (Bit)
- `IsOverride` (Bit) - Đánh dấu là override
- `CreatedDate` (DateTime)
- `CreatedBy` (UniqueIdentifier, FK → ApplicationUser)

### 3.7 ApplicationVersion
**Mục đích:** Lưu trữ thông tin phiên bản ứng dụng

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `Version` (NVarChar(50))
- `ReleaseDate` (DateTime)
- `Description` (NVarChar(1000))
- `IsActive` (Bit)

### 3.8 AllowedMacAddress
**Mục đích:** Lưu trữ danh sách MAC address được phép truy cập

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `MacAddress` (NVarChar(50))
- `Description` (NVarChar(500))
- `IsActive` (Bit)
- `CreatedDate` (DateTime)

---

## 4. MasterData Schema

### 4.1 Company
**Mục đích:** Lưu trữ thông tin công ty

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `TenCongTy` (NVarChar(200))
- `MaCongTy` (NVarChar(50))
- `DiaChi` (NVarChar(500))
- `SoDienThoai` (NVarChar(50))
- `Email` (NVarChar(100))
- `IsActive` (Bit)

**Tài liệu liên quan:**
- **[Company Entities Summary](../Dal/Doc/CompanyEntities_Summary.md)**

### 4.2 CompanyBranch
**Mục đích:** Lưu trữ thông tin chi nhánh

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `CompanyId` (UniqueIdentifier, FK → Company)
- `TenChiNhanh` (NVarChar(200))
- `MaChiNhanh` (NVarChar(50))
- `DiaChi` (NVarChar(500))
- `IsActive` (Bit)

### 4.3 Department
**Mục đích:** Lưu trữ thông tin phòng ban (cây phòng ban)

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `CompanyId` (UniqueIdentifier, FK → Company)
- `ParentId` (UniqueIdentifier, FK → Department, nullable) - Phòng ban cha
- `TenPhongBan` (NVarChar(200))
- `MaPhongBan` (NVarChar(50))
- `IsActive` (Bit)

### 4.4 BusinessPartner
**Mục đích:** Lưu trữ thông tin đối tác kinh doanh (khách hàng, nhà cung cấp)

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `TenDoiTac` (NVarChar(200))
- `MaDoiTac` (NVarChar(50))
- `LoaiDoiTac` (NVarChar(50)) - KhachHang, NhaCungCap
- `DiaChi` (NVarChar(500))
- `SoDienThoai` (NVarChar(50))
- `Email` (NVarChar(100))
- `IsActive` (Bit)

**Tài liệu liên quan:**
- **[BusinessPartner Database Schema](../MasterData/Doc/BusinessPartner_Database_Schema.md)**

### 4.5 BusinessPartnerContact
**Mục đích:** Lưu trữ thông tin liên hệ của đối tác

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `BusinessPartnerId` (UniqueIdentifier, FK → BusinessPartner)
- `TenLienHe` (NVarChar(200))
- `ChucVu` (NVarChar(100))
- `SoDienThoai` (NVarChar(50))
- `Email` (NVarChar(100))
- `Avatar` (VarBinary(MAX)) - Hình đại diện
- `IsActive` (Bit)

**Tài liệu liên quan:**
- **[BusinessPartner Contact Avatar Migration](../Database/Migrations/README_BusinessPartnerContact_Avatar_Migration.md)**

### 4.6 ProductService
**Mục đích:** Lưu trữ thông tin sản phẩm/dịch vụ

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `CategoryId` (UniqueIdentifier, FK → ProductServiceCategory)
- `TenSanPham` (NVarChar(200))
- `MaSanPham` (NVarChar(50))
- `MoTa` (NVarChar(1000))
- `GiaBan` (Decimal)
- `DonViTinh` (NVarChar(50))
- `IsActive` (Bit)

### 4.7 ProductServiceCategory
**Mục đích:** Lưu trữ danh mục sản phẩm/dịch vụ (cây danh mục)

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `ParentId` (UniqueIdentifier, FK → ProductServiceCategory, nullable)
- `TenDanhMuc` (NVarChar(200))
- `MaDanhMuc` (NVarChar(50))
- `IsActive` (Bit)

### 4.8 ProductVariant
**Mục đích:** Lưu trữ biến thể sản phẩm

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `ProductServiceId` (UniqueIdentifier, FK → ProductService)
- `VariantFullName` (NVarChar(500)) - Tên đầy đủ biến thể
- `Attributes` (NVarChar(1000)) - JSON string chứa thuộc tính
- `IsActive` (Bit)

### 4.9 ProductImage
**Mục đích:** Lưu trữ hình ảnh sản phẩm

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `ProductServiceId` (UniqueIdentifier, FK → ProductService)
- `ImageData` (VarBinary(MAX)) - Dữ liệu hình ảnh
- `ThumbnailData` (VarBinary(MAX)) - Dữ liệu thumbnail
- `FileName` (NVarChar(200))
- `FileSize` (BigInt)
- `IsPrimary` (Bit) - Hình ảnh chính
- `DisplayOrder` (Int) - Thứ tự hiển thị

**Tài liệu liên quan:**
- **[ProductImage Refactor](../Database/Migrations/README_ProductImage_Refactor.md)**

---

## 5. Inventory Schema

### 5.1 StockInOutMaster
**Mục đích:** Lưu trữ thông tin phiếu nhập/xuất kho

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `SoPhieu` (NVarChar(50)) - Số phiếu
- `LoaiPhieu` (NVarChar(50)) - NhapKho, XuatKho
- `NgayPhieu` (DateTime)
- `CompanyId` (UniqueIdentifier, FK → Company)
- `CompanyBranchId` (UniqueIdentifier, FK → CompanyBranch)
- `PartnerId` (UniqueIdentifier, FK → BusinessPartner, nullable)
- `PartnerSiteId` (UniqueIdentifier, nullable) - Địa điểm đối tác
- `TongTien` (Decimal)
- `TrangThai` (NVarChar(50))
- `GhiChu` (NVarChar(1000))
- `CreatedBy` (UniqueIdentifier, FK → ApplicationUser)
- `CreatedDate` (DateTime)

**Tài liệu liên quan:**
- **[Database Schema - StockInOutDocument](../Inventory/Doc/DatabaseSchema_StockInOutDocument_Proposal.md)**

### 5.2 StockInOutDetail
**Mục đích:** Lưu trữ chi tiết phiếu nhập/xuất kho

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `MasterId` (UniqueIdentifier, FK → StockInOutMaster)
- `ProductServiceId` (UniqueIdentifier, FK → ProductService)
- `ProductVariantId` (UniqueIdentifier, FK → ProductVariant, nullable)
- `SoLuong` (Decimal)
- `DonGia` (Decimal)
- `ThanhTien` (Decimal)
- `GhiChu` (NVarChar(500))

### 5.3 InventoryBalance
**Mục đích:** Lưu trữ tồn kho hiện tại

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `CompanyId` (UniqueIdentifier, FK → Company)
- `CompanyBranchId` (UniqueIdentifier, FK → CompanyBranch)
- `ProductServiceId` (UniqueIdentifier, FK → ProductService)
- `ProductVariantId` (UniqueIdentifier, FK → ProductVariant, nullable)
- `SoLuongTon` (Decimal) - Số lượng tồn
- `DonGiaTrungBinh` (Decimal) - Đơn giá trung bình
- `LastUpdatedDate` (DateTime)

**Tài liệu liên quan:**
- **[Database Schema - InventoryBalance](../Docs/DatabaseSchema_InventoryBalance_Proposal.md)**

### 5.4 Asset
**Mục đích:** Lưu trữ thông tin tài sản

**Các cột chính:**
- `Id` (UniqueIdentifier, PK)
- `TenTaiSan` (NVarChar(200))
- `MaTaiSan` (NVarChar(50))
- `LoaiTaiSan` (NVarChar(50))
- `GiaTri` (Decimal)
- `CompanyId` (UniqueIdentifier, FK → Company)
- `CompanyBranchId` (UniqueIdentifier, FK → CompanyBranch)
- `IsActive` (Bit)

---

## 6. Migration Scripts

### 6.1 Permission Management
- **[Create Permission Management Tables](../Database/Migrations/Create_PermissionManagement_Tables.sql)**

### 6.2 Application Version
- **[Rename ApplicationVersion Table](../Database/Migrations/Rename_ApplicationVersion_To_VnsErpApplicationVersion.sql)**

### 6.3 BusinessPartner
- **[BusinessPartner Migration](../Database/Migrations/README_BusinessPartner_Migration.md)**
- **[BusinessPartner Logo Migration](../Database/Migrations/README_BusinessPartner_Logo_Migration.md)**
- **[BusinessPartner Logo Thumbnail Migration](../Database/Migrations/README_BusinessPartner_LogoThumbnail_Migration.md)**
- **[BusinessPartner Remove Contact Bank Fields](../Database/Migrations/README_BusinessPartner_RemoveContactBankFields.md)**

### 6.4 ProductService
- **[ProductImage Refactor](../Database/Migrations/README_ProductImage_Refactor.md)**

### 6.5 StockInOut
- **[Alter StockInOutMaster PartnerSiteId AllowNull](../Alter_StockInOutMaster_PartnerSiteId_AllowNull.sql)**

---

## 7. Seed Data

### 7.1 Company
- **[Company Seed Data](../Dal/DataContext/SeedData/MasterData/Company/README_SeedData.md)**

### 7.2 Customer
- **[Customer Seed Data](../Dal/DataContext/SeedData/MasterData/Customer/README_SeedData.md)**

---

## 8. Indexes & Performance

### 8.1 Indexes Chính
- Primary keys trên tất cả các bảng
- Foreign key indexes
- Indexes trên các cột thường xuyên query:
  - `UserName` trong `ApplicationUser`
  - `SoPhieu` trong `StockInOutMaster`
  - `MaSanPham` trong `ProductService`
  - `CompanyId`, `CompanyBranchId` trong các bảng liên quan

### 8.2 Optimization
- **[Optimize Image Database Indexes](../Scripts/OptimizeImageDatabaseIndexes.sql)**

---

## 9. Relationships

### 9.1 VersionAndUserManagement
```
ApplicationUser
    ├── UserRole (Many-to-Many với Role)
    └── UserPermission (Many-to-Many với Permission)

Role
    ├── UserRole (Many-to-Many với ApplicationUser)
    └── RolePermission (Many-to-Many với Permission)

Permission
    ├── RolePermission (Many-to-Many với Role)
    └── UserPermission (Many-to-Many với ApplicationUser)
```

### 9.2 MasterData
```
Company
    ├── CompanyBranch (One-to-Many)
    └── Department (One-to-Many, self-referencing)

BusinessPartner
    └── BusinessPartnerContact (One-to-Many)

ProductServiceCategory (Self-referencing tree)
    └── ProductService (One-to-Many)
        ├── ProductVariant (One-to-Many)
        └── ProductImage (One-to-Many)
```

### 9.3 Inventory
```
StockInOutMaster
    ├── StockInOutDetail (One-to-Many)
    ├── Company (Many-to-One)
    ├── CompanyBranch (Many-to-One)
    └── BusinessPartner (Many-to-One, nullable)

InventoryBalance
    ├── Company (Many-to-One)
    ├── CompanyBranch (Many-to-One)
    ├── ProductService (Many-to-One)
    └── ProductVariant (Many-to-One, nullable)
```

---

## 10. Constraints & Rules

### 10.1 Unique Constraints
- `UserName` trong `ApplicationUser` phải unique
- `Name` trong `Role` phải unique
- `(EntityName, Action)` trong `Permission` phải unique
- `SoPhieu` trong `StockInOutMaster` phải unique
- `MaSanPham` trong `ProductService` phải unique

### 10.2 Check Constraints
- `SoLuong` >= 0 trong `StockInOutDetail` và `InventoryBalance`
- `GiaBan`, `DonGia` >= 0

### 10.3 Foreign Key Constraints
- Tất cả foreign keys có ON DELETE CASCADE hoặc ON DELETE SET NULL tùy theo business logic

---

## 11. Tài Liệu Liên Quan

- **[Cấu Trúc DAL Layer](../Dal/Doc/DalFolderStructure.md)**
- **[DataContext Documentation](../Dal/DataContext/VnsErp2025.md)**
- **[Entity Permission Matrix](../Dal/Doc/EntityPermissionMatrix.md)**

---

**Người tạo:** Development Team  
**Ngày tạo:** 27/01/2025  
**Trạng thái:** Đang phát triển
