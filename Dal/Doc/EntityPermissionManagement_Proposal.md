# Đề xuất Hệ thống Quản lý Quyền Truy cập cho Entity

## 1. Tổng quan

Tài liệu này đề xuất một hệ thống quản lý quyền truy cập (Permission Management System) cho tất cả các entity trong hệ thống VNS ERP 2025.

### 1.1. Danh sách Entity (30 entities)

Dựa trên phân tích file `VnsErp2025.designer.cs`, hệ thống có **30 entities** sau:

#### Version & User Management
1. **AllowedMacAddress** - Quản lý địa chỉ MAC được phép
2. **ApplicationUser** - Người dùng ứng dụng
3. **VnsErpApplicationVersion** - Phiên bản ứng dụng

#### Master Data - Company
4. **Company** - Công ty
5. **CompanyBranch** - Chi nhánh công ty
6. **Department** - Phòng ban
7. **Employee** - Nhân viên
8. **Position** - Chức vụ

#### Master Data - Business Partner
9. **BusinessPartner** - Đối tác kinh doanh
10. **BusinessPartnerCategory** - Danh mục đối tác
11. **BusinessPartner_BusinessPartnerCategory** - Liên kết đối tác - danh mục
12. **BusinessPartnerContact** - Liên hệ đối tác
13. **BusinessPartnerSite** - Địa điểm đối tác

#### Master Data - Product/Service
14. **ProductService** - Sản phẩm/Dịch vụ
15. **ProductServiceCategory** - Danh mục sản phẩm/dịch vụ
16. **ProductVariant** - Biến thể sản phẩm
17. **ProductImage** - Hình ảnh sản phẩm
18. **Attribute** - Thuộc tính
19. **AttributeValue** - Giá trị thuộc tính
20. **VariantAttribute** - Thuộc tính biến thể
21. **UnitOfMeasure** - Đơn vị tính

#### Inventory Management
22. **Asset** - Tài sản
23. **Device** - Thiết bị
24. **DeviceHistory** - Lịch sử thiết bị
25. **DeviceTransfer** - Chuyển giao thiết bị
26. **InventoryBalance** - Tồn kho
27. **StockInOutMaster** - Phiếu nhập/xuất (Master)
28. **StockInOutDetail** - Chi tiết nhập/xuất
29. **StockInOutDocument** - Tài liệu nhập/xuất
30. **StockInOutImage** - Hình ảnh nhập/xuất
31. **Warranty** - Bảo hành

---

## 2. Kiến trúc Hệ thống Quyền

### 2.1. Mô hình Quyền (Permission Model)

Hệ thống đề xuất sử dụng **Role-Based Access Control (RBAC)** kết hợp với **Entity-Based Permissions**:

```
ApplicationUser (1) ──→ (N) UserRole ──→ (1) Role ──→ (N) RolePermission ──→ (1) Permission
                                                                                    │
                                                                                    └──→ EntityName + Action
```

### 2.2. Các loại Quyền (Permission Types)

Mỗi entity sẽ có 4 quyền cơ bản (CRUD):
- **Read** (R) - Xem/Đọc dữ liệu
- **Create** (C) - Tạo mới
- **Update** (U) - Cập nhật
- **Delete** (D) - Xóa

Ngoài ra, có thể mở rộng thêm:
- **Approve** - Phê duyệt (cho các entity như StockInOutMaster)
- **Export** - Xuất dữ liệu
- **Import** - Nhập dữ liệu
- **Print** - In ấn

### 2.3. Cấp độ Quyền (Permission Levels)

1. **Full Access** - Toàn quyền (Read, Create, Update, Delete)
2. **Read Only** - Chỉ đọc
3. **Read + Create** - Đọc và tạo mới
4. **Read + Update** - Đọc và cập nhật
5. **Custom** - Tùy chỉnh theo từng action

---

## 3. Database Schema

### 3.1. Bảng Role

```sql
CREATE TABLE [dbo].[Role] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [Name] NVARCHAR(100) NOT NULL UNIQUE,
    [Description] NVARCHAR(500) NULL,
    [IsSystemRole] BIT NOT NULL DEFAULT 0, -- Role hệ thống không thể xóa
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    [ModifiedDate] DATETIME NULL,
    [ModifiedBy] UNIQUEIDENTIFIER NULL
);
```

**Các Role mặc định:**
- `Administrator` - Quản trị viên (toàn quyền)
- `Manager` - Quản lý (quyền cao)
- `User` - Người dùng (quyền cơ bản)
- `Viewer` - Người xem (chỉ đọc)

### 3.2. Bảng Permission

```sql
CREATE TABLE [dbo].[Permission] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [EntityName] NVARCHAR(100) NOT NULL, -- Tên entity: "ProductService", "BusinessPartner", etc.
    [Action] NVARCHAR(50) NOT NULL, -- "Read", "Create", "Update", "Delete", "Approve", etc.
    [Description] NVARCHAR(500) NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [UQ_Permission_EntityName_Action] UNIQUE ([EntityName], [Action])
);
```

**Ví dụ Permissions:**
- `ProductService.Read`
- `ProductService.Create`
- `ProductService.Update`
- `ProductService.Delete`
- `StockInOutMaster.Approve`
- `BusinessPartner.Export`

### 3.3. Bảng RolePermission (Many-to-Many)

```sql
CREATE TABLE [dbo].[RolePermission] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [RoleId] UNIQUEIDENTIFIER NOT NULL,
    [PermissionId] UNIQUEIDENTIFIER NOT NULL,
    [IsGranted] BIT NOT NULL DEFAULT 1, -- true = cho phép, false = từ chối
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT [FK_RolePermission_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_RolePermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_RolePermission] UNIQUE ([RoleId], [PermissionId])
);
```

### 3.4. Bảng UserRole (Many-to-Many)

```sql
CREATE TABLE [dbo].[UserRole] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [RoleId] UNIQUEIDENTIFIER NOT NULL,
    [IsActive] BIT NOT NULL DEFAULT 1,
    [AssignedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [AssignedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_UserRole_ApplicationUser] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [Role]([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_UserRole] UNIQUE ([UserId], [RoleId])
);
```

### 3.5. Bảng UserPermission (Override - Tùy chọn)

Cho phép gán quyền trực tiếp cho user (override role):

```sql
CREATE TABLE [dbo].[UserPermission] (
    [Id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    [UserId] UNIQUEIDENTIFIER NOT NULL,
    [PermissionId] UNIQUEIDENTIFIER NOT NULL,
    [IsGranted] BIT NOT NULL DEFAULT 1,
    [IsOverride] BIT NOT NULL DEFAULT 1, -- Override quyền từ role
    [CreatedDate] DATETIME NOT NULL DEFAULT GETDATE(),
    [CreatedBy] UNIQUEIDENTIFIER NULL,
    CONSTRAINT [FK_UserPermission_ApplicationUser] FOREIGN KEY ([UserId]) REFERENCES [ApplicationUser]([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_UserPermission_Permission] FOREIGN KEY ([PermissionId]) REFERENCES [Permission]([Id]) ON DELETE CASCADE,
    CONSTRAINT [UQ_UserPermission] UNIQUE ([UserId], [PermissionId])
);
```

---

## 4. Cấu trúc Code

### 4.1. Entity Classes (LINQ to SQL)

Các entity sẽ được tạo tự động bởi LINQ to SQL Designer khi thêm các bảng trên vào database.

### 4.2. Repository Layer

#### 4.2.1. IPermissionRepository.cs

```csharp
namespace Dal.DataAccess.Interfaces
{
    public interface IPermissionRepository
    {
        // Permission CRUD
        Permission Create(Permission permission);
        Permission GetById(Guid id);
        List<Permission> GetAll();
        List<Permission> GetByEntityName(string entityName);
        Permission Update(Permission permission);
        void Delete(Guid id);
        
        // Check permissions
        bool HasPermission(Guid userId, string entityName, string action);
        List<Permission> GetUserPermissions(Guid userId);
        List<Permission> GetRolePermissions(Guid roleId);
        
        // Role management
        Role CreateRole(Role role);
        Role GetRoleById(Guid id);
        List<Role> GetAllRoles();
        void AssignRoleToUser(Guid userId, Guid roleId);
        void RemoveRoleFromUser(Guid userId, Guid roleId);
        List<Role> GetUserRoles(Guid userId);
    }
}
```

#### 4.2.2. PermissionRepository.cs

Implement interface với các method kiểm tra quyền hiệu quả.

### 4.3. Business Logic Layer

#### 4.3.1. PermissionBll.cs

```csharp
namespace Bll.Common
{
    public class PermissionBll
    {
        private readonly IPermissionRepository _permissionRepository;
        private readonly ILogger _logger;
        
        public PermissionBll()
        {
            _permissionRepository = new PermissionRepository(ConnectionManager.GetConnectionString());
            _logger = LoggerFactory.CreateLogger(LogCategory.BLL);
        }
        
        /// <summary>
        /// Kiểm tra user có quyền thực hiện action trên entity không
        /// </summary>
        public bool HasPermission(Guid userId, string entityName, string action)
        {
            try
            {
                return _permissionRepository.HasPermission(userId, entityName, action);
            }
            catch (Exception ex)
            {
                _logger?.Error($"Lỗi khi kiểm tra quyền: {ex.Message}", ex);
                return false; // Deny by default
            }
        }
        
        /// <summary>
        /// Kiểm tra quyền cho user hiện tại (từ session/context)
        /// </summary>
        public bool HasPermission(string entityName, string action)
        {
            var currentUserId = GetCurrentUserId(); // Lấy từ session
            return HasPermission(currentUserId, entityName, action);
        }
        
        /// <summary>
        /// Lấy tất cả quyền của user
        /// </summary>
        public List<Permission> GetUserPermissions(Guid userId)
        {
            return _permissionRepository.GetUserPermissions(userId);
        }
        
        /// <summary>
        /// Gán role cho user
        /// </summary>
        public void AssignRoleToUser(Guid userId, Guid roleId)
        {
            _permissionRepository.AssignRoleToUser(userId, roleId);
        }
    }
}
```

### 4.4. Permission Attributes (Decorator Pattern)

Tạo attribute để đánh dấu các method cần kiểm tra quyền:

```csharp
namespace Bll.Common.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class RequirePermissionAttribute : Attribute
    {
        public string EntityName { get; }
        public string Action { get; }
        
        public RequirePermissionAttribute(string entityName, string action)
        {
            EntityName = entityName;
            Action = action;
        }
    }
}
```

**Sử dụng:**
```csharp
[RequirePermission("ProductService", "Create")]
public ProductService CreateProductService(ProductService product)
{
    // Implementation
}
```

### 4.5. Permission Service (Middleware/Interceptor)

Tạo service để tự động kiểm tra quyền trước khi thực thi method:

```csharp
namespace Bll.Common.Services
{
    public class PermissionInterceptor
    {
        private readonly PermissionBll _permissionBll;
        
        public PermissionInterceptor()
        {
            _permissionBll = new PermissionBll();
        }
        
        /// <summary>
        /// Kiểm tra quyền trước khi thực thi method
        /// </summary>
        public void CheckPermission(MethodInfo method, Guid userId)
        {
            var permissionAttr = method.GetCustomAttribute<RequirePermissionAttribute>();
            if (permissionAttr != null)
            {
                if (!_permissionBll.HasPermission(userId, permissionAttr.EntityName, permissionAttr.Action))
                {
                    throw new UnauthorizedAccessException(
                        $"User không có quyền {permissionAttr.Action} trên {permissionAttr.EntityName}"
                    );
                }
            }
        }
    }
}
```

---

## 5. Implementation Strategy

### 5.1. Phase 1: Database Setup

1. Tạo các bảng: `Role`, `Permission`, `RolePermission`, `UserRole`, `UserPermission`
2. Tạo script migration SQL
3. Insert dữ liệu mặc định:
   - 4 roles cơ bản
   - Permissions cho 30 entities (4 actions mỗi entity = 120 permissions)
   - Gán quyền mặc định cho Administrator role

### 5.2. Phase 2: Repository Layer

1. Tạo `IPermissionRepository` interface
2. Implement `PermissionRepository`
3. Tạo các method kiểm tra quyền hiệu quả (có cache nếu cần)

### 5.3. Phase 3: Business Logic Layer

1. Tạo `PermissionBll`
2. Tạo `RequirePermissionAttribute`
3. Tạo `PermissionInterceptor` (nếu cần)

### 5.4. Phase 4: Integration

1. Cập nhật các BLL class hiện có để thêm kiểm tra quyền
2. Cập nhật UI layer để hiển thị/ẩn các button dựa trên quyền
3. Thêm form quản lý Role và Permission

### 5.5. Phase 5: Testing & Documentation

1. Unit test cho PermissionBll
2. Integration test
3. Tài liệu hướng dẫn sử dụng

---

## 6. Ví dụ Sử dụng

### 6.1. Kiểm tra quyền trong BLL

```csharp
public class ProductServiceBll
{
    private readonly PermissionBll _permissionBll;
    
    public ProductServiceBll()
    {
        _permissionBll = new PermissionBll();
    }
    
    public ProductService Create(ProductService product)
    {
        // Kiểm tra quyền
        if (!_permissionBll.HasPermission("ProductService", "Create"))
        {
            throw new UnauthorizedAccessException("Bạn không có quyền tạo sản phẩm");
        }
        
        // Logic tạo sản phẩm
        return _repository.Create(product);
    }
    
    public List<ProductService> GetAll()
    {
        // Kiểm tra quyền
        if (!_permissionBll.HasPermission("ProductService", "Read"))
        {
            throw new UnauthorizedAccessException("Bạn không có quyền xem sản phẩm");
        }
        
        return _repository.GetAll();
    }
}
```

### 6.2. Kiểm tra quyền trong UI

```csharp
public partial class FrmProductServiceList : Form
{
    private readonly PermissionBll _permissionBll;
    
    private void LoadPermissions()
    {
        // Ẩn/hiện button dựa trên quyền
        btnAdd.Enabled = _permissionBll.HasPermission("ProductService", "Create");
        btnEdit.Enabled = _permissionBll.HasPermission("ProductService", "Update");
        btnDelete.Enabled = _permissionBll.HasPermission("ProductService", "Delete");
    }
}
```

---

## 7. Matrix Quyền đề xuất

### 7.1. Role: Administrator
- **Tất cả entities**: Full Access (Read, Create, Update, Delete)

### 7.2. Role: Manager
- **Master Data**: Read, Create, Update
- **Inventory**: Read, Create, Update, Approve
- **System**: Read only

### 7.3. Role: User
- **Master Data**: Read, Create
- **Inventory**: Read only
- **System**: No access

### 7.4. Role: Viewer
- **Tất cả entities**: Read only

---

## 8. Performance Considerations

### 8.1. Caching

- Cache danh sách quyền của user trong session
- Cache role-permission mapping
- Invalidate cache khi có thay đổi quyền

### 8.2. Database Indexing

```sql
CREATE INDEX IX_UserRole_UserId ON UserRole(UserId);
CREATE INDEX IX_UserRole_RoleId ON UserRole(RoleId);
CREATE INDEX IX_RolePermission_RoleId ON RolePermission(RoleId);
CREATE INDEX IX_RolePermission_PermissionId ON RolePermission(PermissionId);
CREATE INDEX IX_Permission_EntityName_Action ON Permission(EntityName, Action);
```

### 8.3. Query Optimization

Sử dụng stored procedure hoặc view để lấy quyền của user một lần:

```sql
CREATE VIEW vw_UserPermissions AS
SELECT DISTINCT
    ur.UserId,
    p.EntityName,
    p.Action,
    rp.IsGranted
FROM UserRole ur
INNER JOIN RolePermission rp ON ur.RoleId = rp.RoleId
INNER JOIN Permission p ON rp.PermissionId = p.Id
WHERE ur.IsActive = 1 AND rp.IsGranted = 1
UNION
SELECT 
    up.UserId,
    p.EntityName,
    p.Action,
    up.IsGranted
FROM UserPermission up
INNER JOIN Permission p ON up.PermissionId = p.Id
WHERE up.IsGranted = 1;
```

---

## 9. Security Best Practices

1. **Deny by Default**: Nếu không có quyền rõ ràng, từ chối truy cập
2. **Principle of Least Privilege**: Gán quyền tối thiểu cần thiết
3. **Audit Log**: Ghi log tất cả các thay đổi quyền
4. **Regular Review**: Định kỳ review và revoke quyền không cần thiết
5. **Encryption**: Mã hóa thông tin nhạy cảm trong database

---

## 10. Migration Script Template

Xem file: `Database/Migrations/Create_PermissionManagement_Tables.sql`

---

## 11. Kết luận

Hệ thống quản lý quyền này cung cấp:
- ✅ Kiểm soát quyền truy cập chi tiết cho từng entity
- ✅ Hỗ trợ Role-Based Access Control
- ✅ Dễ dàng mở rộng và bảo trì
- ✅ Performance tốt với caching
- ✅ Tích hợp dễ dàng với code hiện có

**Ưu tiên triển khai:**
1. Database schema (Phase 1)
2. Repository layer (Phase 2)
3. Business logic layer (Phase 3)
4. Integration với các BLL hiện có (Phase 4)

---

**Tác giả:** AI Assistant  
**Ngày tạo:** 2025-01-27  
**Phiên bản:** 1.0
