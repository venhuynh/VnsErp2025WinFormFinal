## Tổng quan

Tài liệu này tổng hợp các entity LINQ to SQL vừa được sinh ra từ cơ sở dữ liệu `VnsErp2025Final` cho module Master Data – Product/Service. Nội dung bao gồm:
- Mappings giữa entity và bảng SQL Server
- Danh sách property (tên, kiểu .NET, column mapping, ràng buộc, mô tả)
- Navigation property (EntitySet, EntityRef)
- Quan hệ 1-nhiều, nhiều-1, nhiều-nhiều
- Sơ đồ ERD dạng text
- Ví dụ LINQ CRUD cơ bản cho từng entity

DataContext tương ứng: `Dal.DataContext.VnsErp2025DataContext` (mapping `Name="VnsErp2025Final"`).

Lưu ý: Các mô tả cột được diễn giải theo tên trường và attribute trong file designer.

---

## ProductServiceCategory → dbo.ProductServiceCategory

### Properties

| Tên | Kiểu .NET | Column mapping (DbType) | Ràng buộc | Mô tả |
|---|---|---|---|---|
| Id | Guid | UniqueIdentifier NOT NULL | Primary Key | Khóa chính danh mục |
| CategoryName | string | NVarChar(200) NOT NULL | Required | Tên danh mục |
| Description | string | NVarChar(255) | Nullable | Mô tả |
| ParentId | Guid? | UniqueIdentifier | Nullable, FK tự tham chiếu | Danh mục cha |

### Navigation
- EntitySet<ProductService> `ProductServices` (1-nhiều)
- EntitySet<ProductServiceCategory> `ProductServiceCategories` (1-nhiều, self children)
- EntityRef<ProductServiceCategory> `ProductServiceCategory1` (nhiều-1, self parent)

### Partial methods / Events
- OnLoaded, OnValidate, OnCreated
- OnIdChanging/Changed, OnCategoryNameChanging/Changed, OnDescriptionChanging/Changed, OnParentIdChanging/Changed

---

## ProductService → dbo.ProductService

### Properties

| Tên | Kiểu .NET | Column mapping (DbType) | Ràng buộc | Mô tả |
|---|---|---|---|---|
| Id | Guid | UniqueIdentifier NOT NULL | Primary Key | Khóa chính |
| Code | string | NVarChar(50) NOT NULL | Required | Mã hàng hóa/dịch vụ |
| Name | string | NVarChar(255) NOT NULL | Required | Tên hàng hóa/dịch vụ |
| CategoryId | Guid? | UniqueIdentifier | Nullable, FK → ProductServiceCategory(Id) | Danh mục |
| IsService | bool | Bit NOT NULL | Required | Có phải dịch vụ |
| Description | string | NVarChar(500) | Nullable | Mô tả |
| IsActive | bool | Bit NOT NULL | Required | Trạng thái hoạt động |
| ThumbnailPath | string | NVarChar(500) | Nullable | Ảnh đại diện |

### Navigation
- EntitySet<ProductImage> `ProductImages` (1-nhiều)
- EntitySet<ProductVariant> `ProductVariants` (1-nhiều)
- EntityRef<ProductServiceCategory> `ProductServiceCategory` (nhiều-1)

### Partial methods / Events
- OnLoaded, OnValidate, OnCreated
- OnId/Code/Name/CategoryId/IsService/Description/IsActive/ThumbnailPath Changing/Changed

---

## UnitOfMeasure → dbo.UnitOfMeasure

### Properties

| Tên | Kiểu .NET | Column mapping (DbType) | Ràng buộc | Mô tả |
|---|---|---|---|---|
| Id | Guid | UniqueIdentifier NOT NULL | Primary Key | Khóa chính |
| Code | string | NVarChar(20) NOT NULL | Required | Mã đơn vị |
| Name | string | NVarChar(100) NOT NULL | Required | Tên đơn vị |
| Description | string | NVarChar(255) | Nullable | Mô tả |
| IsActive | bool | Bit NOT NULL | Required | Trạng thái |

### Navigation
- EntitySet<ProductVariant> `ProductVariants` (1-nhiều)

### Partial methods / Events
- OnLoaded, OnValidate, OnCreated
- OnId/Code/Name/Description/IsActive Changing/Changed

---

## ProductVariant → dbo.ProductVariant

### Properties

| Tên | Kiểu .NET | Column mapping (DbType) | Ràng buộc | Mô tả |
|---|---|---|---|---|
| Id | Guid | UniqueIdentifier NOT NULL | Primary Key | Khóa chính biến thể |
| ProductId | Guid | UniqueIdentifier NOT NULL | Required, FK → ProductService(Id) | Sản phẩm cha |
| VariantCode | string | NVarChar(50) NOT NULL | Required | Mã biến thể |
| UnitId | Guid | UniqueIdentifier NOT NULL | Required, FK → UnitOfMeasure(Id) | Đơn vị tính |
| PurchasePrice | decimal? | Decimal(18,2) | Nullable | Giá mua |
| SalePrice | decimal? | Decimal(18,2) | Nullable | Giá bán |
| IsActive | bool | Bit NOT NULL | Required | Trạng thái |
| ThumbnailPath | string | NVarChar(500) | Nullable | Ảnh đại diện |

### Navigation
- EntitySet<VariantAttribute> `VariantAttributes` (1-nhiều)
- EntitySet<ProductImage> `ProductImages` (1-nhiều)
- EntityRef<ProductService> `ProductService` (nhiều-1)
- EntityRef<UnitOfMeasure> `UnitOfMeasure` (nhiều-1)

### Partial methods / Events
- OnLoaded, OnValidate, OnCreated
- OnId/ProductId/VariantCode/UnitId/PurchasePrice/SalePrice/IsActive/ThumbnailPath Changing/Changed

---

## Attribute → dbo.Attribute

### Properties

| Tên | Kiểu .NET | Column mapping (DbType) | Ràng buộc | Mô tả |
|---|---|---|---|---|
| Id | Guid | UniqueIdentifier NOT NULL | Primary Key | Khóa chính thuộc tính |
| Name | string | NVarChar(100) NOT NULL | Required | Tên thuộc tính (vd: Màu, Size) |
| DataType | string | NVarChar(50) NOT NULL | Required | Kiểu dữ liệu (text, number...) |
| Description | string | NVarChar(255) | Nullable | Mô tả |

### Navigation
- EntitySet<VariantAttribute> `VariantAttributes` (1-nhiều)
- EntitySet<AttributeValue> `AttributeValues` (1-nhiều)

### Partial methods / Events
- OnLoaded, OnValidate, OnCreated
- OnId/Name/DataType/Description Changing/Changed

---

## AttributeValue → dbo.AttributeValue

### Properties

| Tên | Kiểu .NET | Column mapping (DbType) | Ràng buộc | Mô tả |
|---|---|---|---|---|
| Id | Guid | UniqueIdentifier NOT NULL | Primary Key | Khóa chính giá trị thuộc tính |
| AttributeId | Guid | UniqueIdentifier NOT NULL | Required, FK → Attribute(Id) | Thuộc tính cha |
| Value | string | NVarChar(255) NOT NULL | Required | Giá trị (vd: Đỏ, XL) |

### Navigation
- EntitySet<VariantAttribute> `VariantAttributes` (1-nhiều)
- EntityRef<Attribute> `Attribute` (nhiều-1)

### Partial methods / Events
- OnLoaded, OnValidate, OnCreated
- OnId/AttributeId/Value Changing/Changed

---

## VariantAttribute → dbo.VariantAttribute

Đại diện mối liên hệ giữa `ProductVariant` và cặp `Attribute`/`AttributeValue`.

### Properties (Composite Key)

| Tên | Kiểu .NET | Column mapping (DbType) | Ràng buộc | Mô tả |
|---|---|---|---|---|
| VariantId | Guid | UniqueIdentifier NOT NULL | Primary Key (part), FK → ProductVariant(Id) | Biến thể |
| AttributeId | Guid | UniqueIdentifier NOT NULL | Primary Key (part), FK → Attribute(Id) | Thuộc tính |
| AttributeValueId | Guid | UniqueIdentifier NOT NULL | Primary Key (part), FK → AttributeValue(Id) | Giá trị thuộc tính |

### Navigation
- EntityRef<Attribute> `Attribute` (nhiều-1)
- EntityRef<AttributeValue> `AttributeValue` (nhiều-1)
- EntityRef<ProductVariant> `ProductVariant` (nhiều-1)

### Partial methods / Events
- OnLoaded, OnValidate, OnCreated
- OnVariantId/AttributeId/AttributeValueId Changing/Changed

---

## ProductImage → dbo.ProductImage

### Properties

| Tên | Kiểu .NET | Column mapping (DbType) | Ràng buộc | Mô tả |
|---|---|---|---|---|
| Id | Guid | UniqueIdentifier NOT NULL | Primary Key | Khóa chính |
| ProductId | Guid? | UniqueIdentifier | Nullable, FK → ProductService(Id) | Ảnh gắn với sản phẩm |
| VariantId | Guid? | UniqueIdentifier | Nullable, FK → ProductVariant(Id) | Ảnh gắn với biến thể |
| ImagePath | string | NVarChar(500) NOT NULL | Required | Đường dẫn ảnh |
| SortOrder | int? | Int | Nullable | Thứ tự |
| IsPrimary | bool? | Bit | Nullable | Ảnh chính |

### Navigation
- EntityRef<ProductService> `ProductService` (nhiều-1)
- EntityRef<ProductVariant> `ProductVariant` (nhiều-1)

### Partial methods / Events
- OnLoaded, OnValidate, OnCreated
- OnId/ProductId/VariantId/ImagePath/SortOrder/IsPrimary Changing/Changed

---

## Navigation & Relationships

- ProductServiceCategory (1) — (n) ProductService qua `CategoryId`
- ProductServiceCategory self (1) — (n) ProductServiceCategory qua `ParentId`
- ProductService (1) — (n) ProductVariant qua `ProductId`
- UnitOfMeasure (1) — (n) ProductVariant qua `UnitId`
- ProductVariant (1) — (n) VariantAttribute
- Attribute (1) — (n) AttributeValue
- AttributeValue (1) — (n) VariantAttribute
- Attribute (1) — (n) VariantAttribute
- ProductService (1) — (n) ProductImage qua `ProductId` (nullable)
- ProductVariant (1) — (n) ProductImage qua `VariantId` (nullable)

Quan hệ nhiều-nhiều được mô hình hóa:
- ProductVariant — VariantAttribute — Attribute/AttributeValue

### ERD (text)

```
ProductServiceCategory (Id PK) ──┐
  ├─< ProductService (Id PK, CategoryId FK)
  └─< ProductServiceCategory (Id PK, ParentId FK self)

ProductService (Id PK)
  ├─< ProductVariant (Id PK, ProductId FK)
  │     ├─< VariantAttribute (VariantId+AttributeId+AttributeValueId PK)
  │     │     ├─> Attribute (Id PK)
  │     │     └─> AttributeValue (Id PK, AttributeId FK)
  │
  │     └─< ProductImage (Id PK, VariantId FK nullable)
  │
  └─< ProductImage (Id PK, ProductId FK nullable)

UnitOfMeasure (Id PK)
  └─< ProductVariant (UnitId FK)
```

---

## Ví dụ LINQ CRUD

Các ví dụ dưới đây sử dụng `VnsErp2025DataContext`. Nhớ gọi `SubmitChanges()` để lưu thay đổi.

> Namespace: `Dal.DataContext`

### ProductServiceCategory

```csharp
using (var db = new VnsErp2025DataContext())
{
    // CREATE
    var cat = new ProductServiceCategory
    {
        Id = Guid.NewGuid(),
        CategoryName = "Thiết bị",
        Description = "Danh mục thiết bị"
    };
    db.ProductServiceCategories.InsertOnSubmit(cat);
    db.SubmitChanges();

    // READ
    var list = db.ProductServiceCategories
        .Where(x => x.CategoryName.Contains("thiết"))
        .ToList();

    // UPDATE
    cat.Description = "Danh mục thiết bị (cập nhật)";
    db.SubmitChanges();

    // DELETE
    db.ProductServiceCategories.DeleteOnSubmit(cat);
    db.SubmitChanges();
}
```

### ProductService

```csharp
using (var db = new VnsErp2025DataContext())
{
    // CREATE
    var service = new ProductService
    {
        Id = Guid.NewGuid(),
        Code = "SP001",
        Name = "Máy in",
        IsService = false,
        IsActive = true
    };
    db.ProductServices.InsertOnSubmit(service);
    db.SubmitChanges();

    // READ
    var activeProducts = db.ProductServices.Where(p => p.IsActive).ToList();

    // UPDATE
    service.Description = "Máy in laser";
    db.SubmitChanges();

    // DELETE
    db.ProductServices.DeleteOnSubmit(service);
    db.SubmitChanges();
}
```

### UnitOfMeasure

```csharp
using (var db = new VnsErp2025DataContext())
{
    var unit = new UnitOfMeasure
    {
        Id = Guid.NewGuid(),
        Code = "PCS",
        Name = "Cái",
        IsActive = true
    };
    db.UnitOfMeasures.InsertOnSubmit(unit);
    db.SubmitChanges();

    var pcs = db.UnitOfMeasures.Single(x => x.Code == "PCS");
    pcs.Description = "Đơn vị cái";
    db.SubmitChanges();

    db.UnitOfMeasures.DeleteOnSubmit(pcs);
    db.SubmitChanges();
}
```

### ProductVariant

```csharp
using (var db = new VnsErp2025DataContext())
{
    var product = db.ProductServices.First();
    var unit = db.UnitOfMeasures.First();

    var variant = new ProductVariant
    {
        Id = Guid.NewGuid(),
        ProductId = product.Id,
        VariantCode = "SP001-BLACK",
        UnitId = unit.Id,
        IsActive = true
    };
    db.ProductVariants.InsertOnSubmit(variant);
    db.SubmitChanges();

    var variants = db.ProductVariants.Where(v => v.ProductId == product.Id).ToList();

    variant.SalePrice = 1000000m;
    db.SubmitChanges();

    db.ProductVariants.DeleteOnSubmit(variant);
    db.SubmitChanges();
}
```

### Attribute và AttributeValue

```csharp
using (var db = new VnsErp2025DataContext())
{
    var attr = new Attribute
    {
        Id = Guid.NewGuid(),
        Name = "Màu sắc",
        DataType = "text"
    };
    db.Attributes.InsertOnSubmit(attr);
    db.SubmitChanges();

    var red = new AttributeValue
    {
        Id = Guid.NewGuid(),
        AttributeId = attr.Id,
        Value = "Đỏ"
    };
    db.AttributeValues.InsertOnSubmit(red);
    db.SubmitChanges();

    // Đổi mô tả thuộc tính
    attr.Description = "Thuộc tính màu sắc";
    db.SubmitChanges();

    // Xóa
    db.AttributeValues.DeleteOnSubmit(red);
    db.Attributes.DeleteOnSubmit(attr);
    db.SubmitChanges();
}
```

### VariantAttribute (gắn thuộc tính cho biến thể)

```csharp
using (var db = new VnsErp2025DataContext())
{
    var variant = db.ProductVariants.First();
    var attr = db.Attributes.First();
    var val = db.AttributeValues.First(x => x.AttributeId == attr.Id);

    var va = new VariantAttribute
    {
        VariantId = variant.Id,
        AttributeId = attr.Id,
        AttributeValueId = val.Id
    };
    db.VariantAttributes.InsertOnSubmit(va);
    db.SubmitChanges();

    // Truy vấn thuộc tính của 1 biến thể
    var attrs = db.VariantAttributes
                 .Where(x => x.VariantId == variant.Id)
                 .Select(x => new { x.Attribute.Name, x.AttributeValue.Value })
                 .ToList();

    // Xóa liên kết
    db.VariantAttributes.DeleteOnSubmit(va);
    db.SubmitChanges();
}
```

### ProductImage

```csharp
using (var db = new VnsErp2025DataContext())
{
    var product = db.ProductServices.First();
    var image = new ProductImage
    {
        Id = Guid.NewGuid(),
        ProductId = product.Id,
        ImagePath = @"images/sp001-1.jpg",
        SortOrder = 1,
        IsPrimary = true
    };
    db.ProductImages.InsertOnSubmit(image);
    db.SubmitChanges();

    var images = db.ProductImages.Where(i => i.ProductId == product.Id).ToList();
    image.SortOrder = 2;
    db.SubmitChanges();

    db.ProductImages.DeleteOnSubmit(image);
    db.SubmitChanges();
}
```

---

Tài liệu này nhằm phục vụ mục đích nội bộ, làm chuẩn tham chiếu nhanh cho đội phát triển khi làm việc với module Master Data – Product/Service.


