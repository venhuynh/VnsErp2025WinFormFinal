# VNS ERP 2025 - Tài Liệu Các Entity Classes (LINQ to SQL)

**Ngày cập nhật:** 05/01/2025  
**Phiên bản:** 2.0  
**Nguồn:** Auto-generated từ LINQ to SQL Designer  

---

## 📋 **Tổng Quan**

Các entity classes này được tự động tạo bởi LINQ to SQL Designer từ database schema. Tất cả các classes đều implement `INotifyPropertyChanging` và `INotifyPropertyChanged` để hỗ trợ data binding và change tracking.

**Lưu ý quan trọng:** 
- ⚠️ **KHÔNG EDIT** file `VnsErp2025.designer.cs` - sẽ bị mất khi regenerate
- 🔄 Để thay đổi schema, sử dụng LINQ to SQL Designer trong Visual Studio
- 📁 File này nằm trong `Dal/DataContext/VnsErp2025.designer.cs`

---

## 🏗️ **DataContext**

### **VnsErp2025DataContext**
**Namespace:** `Dal.DataContext`  
**Kế thừa:** `System.Data.Linq.DataContext`

**Chức năng:**
- Quản lý kết nối database
- Cung cấp các Table properties để truy cập entities
- Hỗ trợ CRUD operations thông qua LINQ

**Connection String:** `VnsErp2025FinalConnectionString2`

**Tables Properties (41 tables):**
- `AllowedMacAddresses` → Table<AllowedMacAddress>
- `Warranties` → Table<Warranty>
- `ApplicationUsers` → Table<ApplicationUser>
- `Assets` → Table<Asset>
- `Attributes` → Table<Attribute>
- `AttributeValues` → Table<AttributeValue>
- `BusinessPartners` → Table<BusinessPartner>
- `BusinessPartner_BusinessPartnerCategories` → Table<BusinessPartner_BusinessPartnerCategory>
- `BusinessPartnerCategories` → Table<BusinessPartnerCategory>
- `BusinessPartnerContacts` → Table<BusinessPartnerContact>
- `BusinessPartnerSites` → Table<BusinessPartnerSite>
- `Companies` → Table<Company>
- `CompanyBranches` → Table<CompanyBranch>
- `Departments` → Table<Department>
- `Devices` → Table<Device>
- `DeviceHistories` → Table<DeviceHistory>
- `DeviceImages` → Table<DeviceImage>
- `DeviceTransactionHistories` → Table<DeviceTransactionHistory>
- `DeviceTransfers` → Table<DeviceTransfer>
- `Employees` → Table<Employee>
- `InventoryBalances` → Table<InventoryBalance>
- `Permissions` → Table<Permission>
- `Positions` → Table<Position>
- `ProductImages` → Table<ProductImage>
- `ProductServices` → Table<ProductService>
- `ProductServiceCategories` → Table<ProductServiceCategory>
- `ProductVariants` → Table<ProductVariant>
- `ProductVariantIdentifiers` → Table<ProductVariantIdentifier> ⭐ **MỚI**
- `ProductVariantIdentifierHistories` → Table<ProductVariantIdentifierHistory> ⭐ **MỚI**
- `Roles` → Table<Role>
- `RolePermissions` → Table<RolePermission>
- `Settings` → Table<Setting>
- `StockInOutDetails` → Table<StockInOutDetail>
- `StockInOutDocuments` → Table<StockInOutDocument>
- `StockInOutImages` → Table<StockInOutImage>
- `StockInOutMasters` → Table<StockInOutMaster>
- `UnitOfMeasures` → Table<UnitOfMeasure>
- `UserPermissions` → Table<UserPermission>
- `UserRoles` → Table<UserRole>
- `VariantAttributes` → Table<VariantAttribute>
- `VnsErpApplicationVersions` → Table<VnsErpApplicationVersion>

---

## 📦 **Danh Sách Entity Classes (41 entities)**

1. **AllowedMacAddress** - Quản lý địa chỉ MAC được phép
2. **Warranty** - Quản lý bảo hành
3. **ApplicationUser** - Người dùng ứng dụng
4. **Asset** - Tài sản
5. **Attribute** - Thuộc tính
6. **AttributeValue** - Giá trị thuộc tính
7. **BusinessPartner** - Đối tác kinh doanh
8. **BusinessPartner_BusinessPartnerCategory** - Junction table (nhiều-nhiều)
9. **BusinessPartnerCategory** - Danh mục đối tác
10. **BusinessPartnerContact** - Liên hệ đối tác
11. **BusinessPartnerSite** - Địa điểm đối tác
12. **Company** - Công ty
13. **CompanyBranch** - Chi nhánh công ty
14. **Department** - Phòng ban
15. **Device** - Thiết bị
16. **DeviceHistory** - Lịch sử thiết bị
17. **DeviceImage** - Hình ảnh thiết bị
18. **DeviceTransactionHistory** - Lịch sử giao dịch thiết bị
19. **DeviceTransfer** - Chuyển giao thiết bị
20. **Employee** - Nhân viên
21. **InventoryBalance** - Tồn kho
22. **Permission** - Quyền
23. **Position** - Chức vụ
24. **ProductImage** - Hình ảnh sản phẩm
25. **ProductService** - Sản phẩm/Dịch vụ
26. **ProductServiceCategory** - Danh mục sản phẩm/dịch vụ
27. **ProductVariant** - Biến thể sản phẩm
28. **ProductVariantIdentifier** ⭐ - Định danh biến thể sản phẩm (MỚI)
29. **ProductVariantIdentifierHistory** ⭐ - Lịch sử định danh (MỚI)
30. **Role** - Vai trò
31. **RolePermission** - Quyền của vai trò
32. **Setting** - Cài đặt
33. **StockInOutDetail** - Chi tiết nhập/xuất kho
34. **StockInOutDocument** - Tài liệu nhập/xuất kho
35. **StockInOutImage** - Hình ảnh nhập/xuất kho
36. **StockInOutMaster** - Phiếu nhập/xuất kho
37. **UnitOfMeasure** - Đơn vị tính
38. **UserPermission** - Quyền người dùng
39. **UserRole** - Vai trò người dùng
40. **VariantAttribute** - Thuộc tính biến thể
41. **VnsErpApplicationVersion** - Phiên bản ứng dụng

---

## 🆕 **ProductVariantIdentifier Entity** ⭐

**Namespace:** `Dal.DataContext`  
**Table:** `dbo.ProductVariantIdentifier`

### **Mô tả**
Bảng quản lý các loại định danh cho ProductVariant. Mỗi loại định danh được khai báo thành cột riêng. Tương tự như bảng Device nhưng quản lý rộng hơn, không chỉ giới hạn cho thiết bị.

### **Properties**

#### **Định danh cơ bản:**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `Id` | `System.Guid` | `Id` | Primary Key, NOT NULL | Unique identifier |
| `ProductVariantId` | `System.Guid` | `ProductVariantId` | NOT NULL, Foreign Key | ID biến thể sản phẩm |
| `SerialNumber` | `string` | `SerialNumber` | NULL, NVarChar(100) | Số serial |
| `Barcode` | `string` | `Barcode` | NULL, NVarChar(255) | Mã vạch |
| `QRCode` | `string` | `QRCode` | NULL, NVarChar(500) | Mã QR |
| `SKU` | `string` | `SKU` | NULL, NVarChar(100) | Stock Keeping Unit |
| `RFID` | `string` | `RFID` | NULL, NVarChar(100) | Radio Frequency Identification |
| `MACAddress` | `string` | `MACAddress` | NULL, NVarChar(50) | Media Access Control Address |
| `IMEI` | `string` | `IMEI` | NULL, NVarChar(50) | International Mobile Equipment Identity |
| `AssetTag` | `string` | `AssetTag` | NULL, NVarChar(50) | Mã tài sản nội bộ |
| `LicenseKey` | `string` | `LicenseKey` | NULL, NVarChar(255) | Khóa bản quyền |
| `UPC` | `string` | `UPC` | NULL, NVarChar(50) | Universal Product Code |
| `EAN` | `string` | `EAN` | NULL, NVarChar(50) | European Article Number |
| `ISBN` | `string` | `ISBN` | NULL, NVarChar(50) | International Standard Book Number |
| `OtherIdentifier` | `string` | `OtherIdentifier` | NULL, NVarChar(255) | Loại định danh khác |

#### **Quản lý hình ảnh QR code:**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `QRCodeImagePath` | `string` | `QRCodeImagePath` | NULL, NVarChar(500) | Đường dẫn tương đối hình ảnh QR code |
| `QRCodeImageFullPath` | `string` | `QRCodeImageFullPath` | NULL, NVarChar(1000) | Đường dẫn đầy đủ hình ảnh QR code |
| `QRCodeImageFileName` | `string` | `QRCodeImageFileName` | NULL, NVarChar(255) | Tên file hình ảnh QR code |
| `QRCodeImageStorageType` | `string` | `QRCodeImageStorageType` | NULL, NVarChar(20), DEFAULT('NAS') | Loại lưu trữ: NAS, Local, Cloud |
| `QRCodeImageLocked` | `bool` | `QRCodeImageLocked` | NOT NULL, Bit, DEFAULT(0) | Khóa hình ảnh (không cho chỉnh sửa/xóa) |
| `QRCodeImageLockedDate` | `System.DateTime?` | `QRCodeImageLockedDate` | NULL, DateTime | Ngày khóa hình ảnh |
| `QRCodeImageLockedBy` | `System.Guid?` | `QRCodeImageLockedBy` | NULL, UniqueIdentifier | Người khóa hình ảnh |

#### **Tình trạng hàng hóa/sản phẩm:**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `Status` | `int` | `Status` | NOT NULL, Int, DEFAULT(0) | Tình trạng: 0=Tại kho VNS, 1=Đã xuất cho KH, 2=Đang lắp đặt tại site KH, 3=Đang gửi Bảo hành NCC, 4=Đã hư hỏng (Tại kho VNS), 5=Đã thanh lý |
| `StatusDate` | `System.DateTime?` | `StatusDate` | NULL, DateTime | Ngày thay đổi trạng thái |
| `StatusChangedBy` | `System.Guid?` | `StatusChangedBy` | NULL, UniqueIdentifier | Người thay đổi trạng thái |
| `StatusNotes` | `string` | `StatusNotes` | NULL, NVarChar(1000) | Ghi chú về trạng thái |

#### **Thông tin khác:**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `IsActive` | `bool` | `IsActive` | NOT NULL, Bit, DEFAULT(1) | Còn sử dụng không |
| `SourceType` | `int?` | `SourceType` | NULL, Int | Nguồn: 0=Manual, 1=Import, 2=AutoGenerate, 3=Scanner, 4=Other |
| `SourceReference` | `string` | `SourceReference` | NULL, NVarChar(255) | Tham chiếu nguồn |
| `ValidFrom` | `System.DateTime?` | `ValidFrom` | NULL, DateTime | Ngày bắt đầu có hiệu lực |
| `ValidTo` | `System.DateTime?` | `ValidTo` | NULL, DateTime | Ngày hết hiệu lực |
| `Notes` | `string` | `Notes` | NULL, NVarChar(1000) | Ghi chú bổ sung |
| `CreatedDate` | `System.DateTime` | `CreatedDate` | NOT NULL, DateTime, DEFAULT(GETDATE()) | Ngày tạo |
| `UpdatedDate` | `System.DateTime?` | `UpdatedDate` | NULL, DateTime | Ngày cập nhật |
| `CreatedBy` | `System.Guid?` | `CreatedBy` | NULL, UniqueIdentifier | Người tạo |
| `UpdatedBy` | `System.Guid?` | `UpdatedBy` | NULL, UniqueIdentifier | Người cập nhật |

### **Navigation Properties**
- `ProductVariant` → `ProductVariant` (nhiều-1)
- `ProductVariantIdentifierHistories` → `EntitySet<ProductVariantIdentifierHistory>` (1-nhiều)

### **Relationships**
- **Nhiều-1** với `ProductVariant` (ProductVariantId → Id)
- **1-nhiều** với `ProductVariantIdentifierHistory` (Id → ProductVariantIdentifierId)
- **CASCADE DELETE** khi ProductVariant bị xóa

---

## 📜 **ProductVariantIdentifierHistory Entity** ⭐

**Namespace:** `Dal.DataContext`  
**Table:** `dbo.ProductVariantIdentifierHistory`

### **Mô tả**
Bảng lưu trữ lịch sử thay đổi của các định danh ProductVariant.

### **Properties**

| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `Id` | `System.Guid` | `Id` | Primary Key, NOT NULL | Unique identifier |
| `ProductVariantIdentifierId` | `System.Guid` | `ProductVariantIdentifierId` | NOT NULL, Foreign Key | ID định danh được thay đổi |
| `ProductVariantId` | `System.Guid` | `ProductVariantId` | NOT NULL, Foreign Key | ID ProductVariant (để query nhanh) |
| `ChangeType` | `int` | `ChangeType` | NOT NULL, Int | Loại thay đổi: 0=Created, 1=Updated, 2=Activated, 3=Deactivated, 4=Deleted |
| `ChangeDate` | `System.DateTime` | `ChangeDate` | NOT NULL, DateTime | Ngày thay đổi |
| `ChangedBy` | `System.Guid?` | `ChangedBy` | NULL, UniqueIdentifier | Người thay đổi |
| `OldValue` | `string` | `OldValue` | NULL, NVarChar(500) | Giá trị cũ |
| `NewValue` | `string` | `NewValue` | NULL, NVarChar(500) | Giá trị mới |
| `FieldName` | `string` | `FieldName` | NULL, NVarChar(100) | Tên trường thay đổi |
| `Description` | `string` | `Description` | NULL, NVarChar(1000) | Mô tả thay đổi |
| `Notes` | `string` | `Notes` | NULL, NVarChar(1000) | Ghi chú bổ sung |

### **Navigation Properties**
- `ProductVariant` → `ProductVariant` (nhiều-1)
- `ProductVariantIdentifier` → `ProductVariantIdentifier` (nhiều-1)

### **Relationships**
- **Nhiều-1** với `ProductVariant` (ProductVariantId → Id)
- **Nhiều-1** với `ProductVariantIdentifier` (ProductVariantIdentifierId → Id)
- **CASCADE DELETE** khi ProductVariantIdentifier bị xóa

---

## 👤 **ApplicationUser Entity**

**Namespace:** `Dal.DataContext`  
**Table:** `dbo.ApplicationUser`

### **Properties**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `Id` | `System.Guid` | `Id` | Primary Key, NOT NULL | Unique identifier |
| `UserName` | `string` | `UserName` | NOT NULL, NVarChar(50) | Tên đăng nhập |
| `HashPassword` | `string` | `HashPassword` | NOT NULL, NVarChar(500) | Mật khẩu đã hash |
| `Active` | `bool` | `Active` | NOT NULL, Bit | Trạng thái hoạt động |

### **Events**
- `PropertyChanging` - Khi property sắp thay đổi
- `PropertyChanged` - Khi property đã thay đổi

---

## 🤝 **BusinessPartner Entity**

**Namespace:** `Dal.DataContext`  
**Table:** `dbo.BusinessPartner`

### **Properties**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `Id` | `System.Guid` | `Id` | Primary Key, NOT NULL | Unique identifier |
| `PartnerCode` | `string` | `PartnerCode` | NOT NULL, NVarChar(50) | Mã đối tác (unique) |
| `PartnerName` | `string` | `PartnerName` | NOT NULL, NVarChar(255) | Tên đối tác |
| `PartnerType` | `int` | `PartnerType` | NOT NULL, Int | Loại đối tác (1=Customer, 2=Vendor, 3=Both) |
| `TaxCode` | `string` | `TaxCode` | NULL, NVarChar(50) | Mã số thuế |
| `Phone` | `string` | `Phone` | NULL, NVarChar(50) | Số điện thoại |
| `Email` | `string` | `Email` | NULL, NVarChar(100) | Email |
| `Website` | `string` | `Website` | NULL, NVarChar(100) | Website |
| `Address` | `string` | `Address` | NULL, NVarChar(255) | Địa chỉ |
| `City` | `string` | `City` | NULL, NVarChar(100) | Thành phố |
| `Country` | `string` | `Country` | NULL, NVarChar(100) | Quốc gia |
| `ContactPerson` | `string` | `ContactPerson` | NULL, NVarChar(100) | Người liên hệ |
| `ContactPosition` | `string` | `ContactPosition` | NULL, NVarChar(100) | Chức vụ người liên hệ |
| `BankAccount` | `string` | `BankAccount` | NULL, NVarChar(50) | Số tài khoản ngân hàng |
| `BankName` | `string` | `BankName` | NULL, NVarChar(100) | Tên ngân hàng |
| `CreditLimit` | `decimal?` | `CreditLimit` | NULL, Decimal(18,2) | Hạn mức tín dụng |
| `PaymentTerm` | `string` | `PaymentTerm` | NULL, NVarChar(50) | Điều khoản thanh toán |
| `IsActive` | `bool` | `IsActive` | NOT NULL, Bit | Trạng thái hoạt động |
| `CreatedDate` | `System.DateTime` | `CreatedDate` | NOT NULL, DateTime | Ngày tạo |
| `UpdatedDate` | `System.DateTime?` | `UpdatedDate` | NULL, DateTime | Ngày cập nhật |

### **Navigation Properties**
- `BusinessPartnerContacts` → `EntitySet<BusinessPartnerContact>` (1-nhiều)
- `BusinessPartner_BusinessPartnerCategories` → `EntitySet<BusinessPartner_BusinessPartnerCategory>` (nhiều-nhiều)

### **Relationships**
- **1-nhiều** với `BusinessPartnerContact` (PartnerId → Id)
- **Nhiều-nhiều** với `BusinessPartnerCategory` thông qua `BusinessPartner_BusinessPartnerCategory`

---

## 📞 **BusinessPartnerContact Entity**

**Namespace:** `Dal.DataContext`  
**Table:** `dbo.BusinessPartnerContact`

### **Properties**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `Id` | `System.Guid` | `Id` | Primary Key, NOT NULL | Unique identifier |
| `PartnerId` | `System.Guid` | `PartnerId` | NOT NULL, Foreign Key | ID của BusinessPartner |
| `FullName` | `string` | `FullName` | NOT NULL, NVarChar(100) | Họ tên đầy đủ |
| `Position` | `string` | `Position` | NULL, NVarChar(100) | Chức vụ |
| `Phone` | `string` | `Phone` | NULL, NVarChar(50) | Số điện thoại |
| `Email` | `string` | `Email` | NULL, NVarChar(100) | Email |
| `IsPrimary` | `bool` | `IsPrimary` | NOT NULL, Bit | Liên hệ chính |

### **Navigation Properties**
- `BusinessPartner` → `BusinessPartner` (nhiều-1)

### **Relationships**
- **Nhiều-1** với `BusinessPartner` (PartnerId → Id)
- **CASCADE DELETE** khi BusinessPartner bị xóa

---

## 🏷️ **BusinessPartnerCategory Entity**

**Namespace:** `Dal.DataContext`  
**Table:** `dbo.BusinessPartnerCategory`

### **Properties**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `Id` | `System.Guid` | `Id` | Primary Key, NOT NULL | Unique identifier |
| `CategoryName` | `string` | `CategoryName` | NOT NULL, NVarChar(100) | Tên danh mục |
| `Description` | `string` | `Description` | NULL, NVarChar(255) | Mô tả |

### **Navigation Properties**
- `BusinessPartner_BusinessPartnerCategories` → `EntitySet<BusinessPartner_BusinessPartnerCategory>` (1-nhiều)

### **Relationships**
- **Nhiều-nhiều** với `BusinessPartner` thông qua `BusinessPartner_BusinessPartnerCategory`

---

## 🔗 **BusinessPartner_BusinessPartnerCategory Entity (Junction Table)**

**Namespace:** `Dal.DataContext`  
**Table:** `dbo.BusinessPartner_BusinessPartnerCategory`

### **Properties**
| Property | Type | Database Column | Constraints | Mô tả |
|----------|------|-----------------|-------------|-------|
| `PartnerId` | `System.Guid` | `PartnerId` | Primary Key, NOT NULL, Foreign Key | ID của BusinessPartner |
| `CategoryId` | `System.Guid` | `CategoryId` | Primary Key, NOT NULL, Foreign Key | ID của BusinessPartnerCategory |

### **Navigation Properties**
- `BusinessPartner` → `BusinessPartner` (nhiều-1)
- `BusinessPartnerCategory` → `BusinessPartnerCategory` (nhiều-1)

### **Relationships**
- **Composite Primary Key** (PartnerId, CategoryId)
- **CASCADE DELETE** khi BusinessPartner hoặc BusinessPartnerCategory bị xóa

---

## 🔄 **Change Tracking & Events**

Tất cả entities đều implement:

### **INotifyPropertyChanging**
- Event: `PropertyChanging`
- Được trigger trước khi property thay đổi
- Sử dụng cho validation và business logic

### **INotifyPropertyChanged**
- Event: `PropertyChanged`
- Được trigger sau khi property đã thay đổi
- Sử dụng cho data binding và UI updates

### **Partial Methods**
Mỗi entity có các partial methods để customization:
- `OnCreated()` - Khi entity được tạo
- `OnValidate(ChangeAction action)` - Validation logic
- `OnLoaded()` - Khi entity được load từ database
- `On[Property]Changing/Changed()` - Khi property thay đổi

---

## 🚀 **Cách Sử Dụng**

### **1. Tạo DataContext**
```csharp
using (var context = new VnsErp2025DataContext())
{
    // Thao tác với entities
}
```

### **2. CRUD Operations**
```csharp
// Create
var partner = new BusinessPartner
{
    PartnerCode = "CUST001",
    PartnerName = "Công ty ABC",
    PartnerType = 1, // Customer
    IsActive = true,
    CreatedDate = DateTime.Now
};
context.BusinessPartners.InsertOnSubmit(partner);
context.SubmitChanges();

// Read
var partners = context.BusinessPartners.Where(p => p.IsActive).ToList();

// Update
partner.PartnerName = "Công ty ABC Ltd";
context.SubmitChanges();

// Delete
context.BusinessPartners.DeleteOnSubmit(partner);
context.SubmitChanges();
```

### **3. Navigation Properties**
```csharp
var partner = context.BusinessPartners.FirstOrDefault(p => p.PartnerCode == "CUST001");
var contacts = partner.BusinessPartnerContacts.ToList();
var categories = partner.BusinessPartner_BusinessPartnerCategories
    .Select(bpc => bpc.BusinessPartnerCategory).ToList();
```

### **4. Sử dụng ProductVariantIdentifier (MỚI)**
```csharp
// Tạo định danh mới
var identifier = new ProductVariantIdentifier
{
    Id = Guid.NewGuid(),
    ProductVariantId = productVariantId,
    SerialNumber = "SN123456789",
    Barcode = "1234567890123",
    QRCode = "QR123456",
    Status = 0, // Tại kho VNS
    IsActive = true,
    CreatedDate = DateTime.Now
};
context.ProductVariantIdentifiers.InsertOnSubmit(identifier);
context.SubmitChanges();

// Tìm theo SerialNumber
var found = context.ProductVariantIdentifiers
    .FirstOrDefault(p => p.SerialNumber == "SN123456789");

// Cập nhật tình trạng
identifier.Status = 1; // Đã xuất cho KH
identifier.StatusDate = DateTime.Now;
identifier.StatusChangedBy = currentUserId;
context.SubmitChanges();
```

---

## ⚠️ **Lưu Ý Quan Trọng**

1. **Không Edit File Designer:** File `VnsErp2025.designer.cs` được auto-generated, không nên edit trực tiếp
2. **Schema Changes:** Để thay đổi schema, sử dụng LINQ to SQL Designer
3. **Performance:** Sử dụng `DataLoadOptions` để control eager loading
4. **Transactions:** Sử dụng `TransactionScope` cho complex operations
5. **Connection Management:** DataContext tự động quản lý connection lifecycle
6. **ProductVariantIdentifier:** Mỗi ProductVariant chỉ nên có một bản ghi ProductVariantIdentifier với nhiều loại định danh khác nhau

---

## 📚 **Tài Liệu Tham Khảo**

- [LINQ to SQL Documentation](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/)
- [Entity Framework vs LINQ to SQL](https://docs.microsoft.com/en-us/ef/efcore-and-ef6/features)
- [Change Tracking in LINQ to SQL](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/change-tracking)

---

## 📝 **Changelog**

### Version 2.0 (05/01/2025)
- ✅ Thêm ProductVariantIdentifier entity
- ✅ Thêm ProductVariantIdentifierHistory entity
- ✅ Cập nhật danh sách đầy đủ 41 entities
- ✅ Cập nhật danh sách Tables Properties

### Version 1.0 (25/09/2025)
- ✅ Tạo tài liệu ban đầu
- ✅ Mô tả các entity cơ bản
