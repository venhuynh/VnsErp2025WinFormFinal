# VNS ERP 2025 - Tài Liệu Các Entity Classes (LINQ to SQL)

**Ngày cập nhật:** 25/09/2025  
**Phiên bản:** 1.0  
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

**Connection String:** `VnsErp2025FinalConnectionString`

**Tables Properties:**
- `ApplicationUsers` → Table<ApplicationUser>
- `BusinessPartners` → Table<BusinessPartner>
- `BusinessPartnerContacts` → Table<BusinessPartnerContact>
- `BusinessPartner_BusinessPartnerCategories` → Table<BusinessPartner_BusinessPartnerCategory>
- `BusinessPartnerCategories` → Table<BusinessPartnerCategory>

---

## 👤 **1. ApplicationUser Entity**

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

## 🤝 **2. BusinessPartner Entity**

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

## 📞 **3. BusinessPartnerContact Entity**

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

## 🏷️ **4. BusinessPartnerCategory Entity**

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

## 🔗 **5. BusinessPartner_BusinessPartnerCategory Entity (Junction Table)**

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

---

## ⚠️ **Lưu Ý Quan Trọng**

1. **Không Edit File Designer:** File `VnsErp2025.designer.cs` được auto-generated, không nên edit trực tiếp
2. **Schema Changes:** Để thay đổi schema, sử dụng LINQ to SQL Designer
3. **Performance:** Sử dụng `DataLoadOptions` để control eager loading
4. **Transactions:** Sử dụng `TransactionScope` cho complex operations
5. **Connection Management:** DataContext tự động quản lý connection lifecycle

---

## 📚 **Tài Liệu Tham Khảo**

- [LINQ to SQL Documentation](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/)
- [Entity Framework vs LINQ to SQL](https://docs.microsoft.com/en-us/ef/efcore-and-ef6/features)
- [Change Tracking in LINQ to SQL](https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql/linq/change-tracking)
