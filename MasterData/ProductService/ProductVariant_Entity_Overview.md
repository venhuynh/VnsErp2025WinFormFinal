# 📦 ProductVariant Entity - Tổng Quan

## 🎯 **Mục Đích**

**ProductVariant** là entity đại diện cho các biến thể của sản phẩm/dịch vụ trong hệ thống ERP. Mỗi sản phẩm có thể có nhiều biến thể với các thuộc tính khác nhau như kích thước, màu sắc, chất liệu, giá cả, v.v.

---

## 🏗️ **Cấu Trúc Entity**

### **📋 Thông Tin Cơ Bản**
```csharp
[Table(Name="dbo.ProductVariant")]
public partial class ProductVariant : INotifyPropertyChanging, INotifyPropertyChanged
```

### **🔑 Primary Key**
- **Id**: `System.Guid` - Unique identifier cho biến thể

### **🔗 Foreign Keys**
- **ProductId**: `System.Guid` - Liên kết với ProductService (sản phẩm gốc)
- **UnitId**: `System.Guid` - Liên kết với UnitOfMeasure (đơn vị tính)

### **📊 Thuộc Tính Chính**
- **VariantCode**: `string` - Mã biến thể (tối đa 50 ký tự)
- **PurchasePrice**: `decimal?` - Giá mua vào
- **SalePrice**: `decimal?` - Giá bán ra
- **IsActive**: `bool` - Trạng thái hoạt động
- **ThumbnailImage**: `Binary` - Ảnh thumbnail của biến thể

---

## 🔗 **Mối Quan Hệ (Relationships)**

### **1. ProductService (Many-to-One)**
```csharp
[Association(Name="ProductService_ProductVariant", Storage="_ProductService", 
             ThisKey="ProductId", OtherKey="Id", IsForeignKey=true)]
public ProductService ProductService
```
- **Mô tả**: Mỗi biến thể thuộc về một sản phẩm/dịch vụ
- **Navigation**: `ProductVariant.ProductService`

### **2. UnitOfMeasure (Many-to-One)**
```csharp
[Association(Name="UnitOfMeasure_ProductVariant", Storage="_UnitOfMeasure", 
             ThisKey="UnitId", OtherKey="Id", IsForeignKey=true)]
public UnitOfMeasure UnitOfMeasure
```
- **Mô tả**: Mỗi biến thể có một đơn vị tính
- **Navigation**: `ProductVariant.UnitOfMeasure`

### **3. VariantAttributes (One-to-Many)**
```csharp
[Association(Name="ProductVariant_VariantAttribute", Storage="_VariantAttributes", 
             ThisKey="Id", OtherKey="VariantId")]
public EntitySet<VariantAttribute> VariantAttributes
```
- **Mô tả**: Mỗi biến thể có nhiều thuộc tính (màu sắc, kích thước, v.v.)
- **Navigation**: `ProductVariant.VariantAttributes`

### **4. ProductImages (One-to-Many)**
```csharp
[Association(Name="ProductVariant_ProductImage", Storage="_ProductImages", 
             ThisKey="Id", OtherKey="VariantId")]
public EntitySet<ProductImage> ProductImages
```
- **Mô tả**: Mỗi biến thể có thể có nhiều hình ảnh
- **Navigation**: `ProductVariant.ProductImages`

---

## 📊 **Database Schema**

### **Table: dbo.ProductVariant**
```sql
CREATE TABLE [dbo].[ProductVariant] (
    [Id]            UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
    [ProductId]     UNIQUEIDENTIFIER NOT NULL,
    [VariantCode]   NVARCHAR(50) NOT NULL,
    [UnitId]        UNIQUEIDENTIFIER NOT NULL,
    [PurchasePrice] DECIMAL(18,2) NULL,
    [SalePrice]     DECIMAL(18,2) NULL,
    [IsActive]      BIT NOT NULL,
    [ThumbnailImage] VARBINARY(MAX) NULL
);
```

### **Foreign Key Constraints**
```sql
-- Liên kết với ProductService
ALTER TABLE [dbo].[ProductVariant] 
ADD CONSTRAINT [FK_ProductVariant_ProductService] 
FOREIGN KEY ([ProductId]) REFERENCES [dbo].[ProductService]([Id]);

-- Liên kết với UnitOfMeasure
ALTER TABLE [dbo].[ProductVariant] 
ADD CONSTRAINT [FK_ProductVariant_UnitOfMeasure] 
FOREIGN KEY ([UnitId]) REFERENCES [dbo].[UnitOfMeasure]([Id]);
```

---

## 🎨 **Use Cases Thực Tế**

### **1. Sản Phẩm Quần Áo**
- **Sản phẩm gốc**: "Áo thun nam"
- **Biến thể**:
  - Size S, M, L, XL (UnitOfMeasure)
  - Màu đỏ, xanh, trắng (VariantAttributes)
  - Giá khác nhau theo size

### **2. Sản Phẩm Điện Tử**
- **Sản phẩm gốc**: "iPhone 15"
- **Biến thể**:
  - 128GB, 256GB, 512GB (VariantAttributes)
  - Màu đen, trắng, xanh (VariantAttributes)
  - Giá khác nhau theo dung lượng

### **3. Dịch Vụ**
- **Dịch vụ gốc**: "Dịch vụ vận chuyển"
- **Biến thể**:
  - Giao hàng nhanh, tiêu chuẩn (VariantAttributes)
  - Đơn vị: chuyến, kg, km (UnitOfMeasure)
  - Giá khác nhau theo loại dịch vụ

---

## 🔧 **Business Rules**

### **1. Validation Rules**
- **VariantCode**: Bắt buộc, không trùng lặp trong cùng sản phẩm
- **ProductId**: Bắt buộc, phải tồn tại trong ProductService
- **UnitId**: Bắt buộc, phải tồn tại trong UnitOfMeasure
- **PurchasePrice**: >= 0 nếu có giá trị
- **SalePrice**: >= 0 nếu có giá trị

### **2. Business Logic**
- Mỗi sản phẩm phải có ít nhất 1 biến thể
- Biến thể không thể xóa nếu đã có đơn hàng
- Giá bán thường >= giá mua
- ThumbnailImage được tự động tạo từ ProductImages

### **3. Data Integrity**
- Cascade delete: Xóa ProductService → Xóa tất cả ProductVariant
- Restrict delete: Xóa UnitOfMeasure → Kiểm tra ProductVariant đang sử dụng
- Unique constraint: (ProductId, VariantCode)

---

## 📈 **Performance Considerations**

### **1. Indexing Strategy**
```sql
-- Index cho tìm kiếm theo sản phẩm
CREATE INDEX IX_ProductVariant_ProductId ON [dbo].[ProductVariant] ([ProductId]);

-- Index cho tìm kiếm theo mã biến thể
CREATE INDEX IX_ProductVariant_VariantCode ON [dbo].[ProductVariant] ([VariantCode]);

-- Index cho tìm kiếm theo đơn vị
CREATE INDEX IX_ProductVariant_UnitId ON [dbo].[ProductVariant] ([UnitId]);

-- Index cho tìm kiếm theo trạng thái
CREATE INDEX IX_ProductVariant_IsActive ON [dbo].[ProductVariant] ([IsActive]);
```

### **2. Lazy Loading**
- **VariantAttributes**: Load khi cần thiết
- **ProductImages**: Load khi cần thiết
- **ThumbnailImage**: Load khi hiển thị

### **3. Caching Strategy**
- Cache danh sách biến thể theo ProductId
- Cache giá cả cho báo cáo
- Cache thumbnail images

---

## 🔄 **CRUD Operations**

### **1. Create (Thêm mới)**
```csharp
public async Task<ProductVariant> CreateVariantAsync(ProductVariant variant)
{
    // Validate business rules
    // Generate VariantCode if empty
    // Set default values
    // Save to database
}
```

### **2. Read (Đọc dữ liệu)**
```csharp
public async Task<ProductVariant> GetVariantByIdAsync(Guid id)
public async Task<List<ProductVariant>> GetVariantsByProductIdAsync(Guid productId)
public async Task<List<ProductVariant>> GetActiveVariantsAsync()
```

### **3. Update (Cập nhật)**
```csharp
public async Task<ProductVariant> UpdateVariantAsync(ProductVariant variant)
{
    // Validate business rules
    // Update prices if changed
    // Update thumbnail if changed
    // Save to database
}
```

### **4. Delete (Xóa)**
```csharp
public async Task<bool> DeleteVariantAsync(Guid id)
{
    // Check if variant is used in orders
    // Soft delete or hard delete
    // Update related data
}
```

---

## 🎯 **Integration Points**

### **1. ProductService Integration**
- Hiển thị danh sách biến thể trong form sản phẩm
- Tự động tạo biến thể mặc định khi tạo sản phẩm
- Cập nhật thông tin sản phẩm khi thay đổi biến thể

### **2. Order Management Integration**
- Chọn biến thể khi tạo đơn hàng
- Tính giá theo biến thể
- Kiểm tra tồn kho theo biến thể

### **3. Inventory Management Integration**
- Quản lý tồn kho theo biến thể
- Báo cáo tồn kho chi tiết
- Cảnh báo hết hàng theo biến thể

### **4. Pricing Management Integration**
- Quản lý giá theo biến thể
- Áp dụng chiết khấu theo biến thể
- Báo cáo lợi nhuận theo biến thể

---

## 🚀 **Future Enhancements**

### **1. Advanced Features**
- **Bulk Operations**: Tạo nhiều biến thể cùng lúc
- **Import/Export**: Import biến thể từ Excel
- **Price History**: Lịch sử thay đổi giá
- **Variant Templates**: Template cho biến thể

### **2. Analytics & Reporting**
- **Sales Analysis**: Phân tích bán hàng theo biến thể
- **Profit Analysis**: Phân tích lợi nhuận theo biến thể
- **Inventory Turnover**: Tỷ lệ quay vòng tồn kho
- **Popular Variants**: Biến thể bán chạy nhất

### **3. Integration Enhancements**
- **E-commerce Integration**: Đồng bộ với website
- **Mobile App Integration**: API cho mobile app
- **Third-party Integration**: Tích hợp với hệ thống khác

---

## 📚 **Related Documentation**

- [ProductService Entity Overview](./ProductService_Entity_Overview.md)
- [UnitOfMeasure Entity Overview](./UnitOfMeasure_Entity_Overview.md)
- [VariantAttribute Entity Overview](./VariantAttribute_Entity_Overview.md)
- [ProductImage Entity Overview](./ProductImage_Entity_Overview.md)
- [Database Schema Documentation](../Database_Schema.md)

---

**🎉 ProductVariant Entity đã sẵn sàng để triển khai!**

*Entity này là nền tảng quan trọng cho việc quản lý các biến thể sản phẩm trong hệ thống ERP, cung cấp tính linh hoạt cao trong việc quản lý sản phẩm phức tạp.*
