# Hướng dẫn sử dụng Hệ thống Quản lý Quyền (Permission Management System)

## Tổng quan

Hệ thống quản lý quyền đã được tích hợp vào VNS ERP 2025, cho phép kiểm soát quyền truy cập chi tiết cho từng entity trong hệ thống.

## Cấu trúc

### 1. Database Layer
- **5 bảng**: `Role`, `Permission`, `RolePermission`, `UserRole`, `UserPermission`
- **View**: `vw_UserPermissions` - Lấy tất cả quyền của user
- **Stored Procedure**: `sp_CheckUserPermission` - Kiểm tra quyền nhanh

### 2. Data Access Layer (DAL)
- **Interface**: `IPermissionRepository`
- **Implementation**: `PermissionRepository`

### 3. Business Logic Layer (BLL)
- **PermissionBll**: Class chính để quản lý quyền
- **PermissionHelper**: Helper class static để kiểm tra quyền dễ dàng

## Cách sử dụng

### 1. Kiểm tra quyền cơ bản

```csharp
using Bll.Common;

// Sử dụng PermissionBll
var permissionBll = new PermissionBll();
bool canRead = permissionBll.HasPermission(userId, "ProductService", "Read");
bool canCreate = permissionBll.CanCreate(userId, "ProductService");
bool canUpdate = permissionBll.CanUpdate(userId, "ProductService");
bool canDelete = permissionBll.CanDelete(userId, "ProductService");
```

### 2. Sử dụng PermissionHelper (Khuyến nghị)

```csharp
using Bll.Common;

// Kiểm tra quyền cơ bản
bool canRead = PermissionHelper.CanRead(userId, "ProductService");
bool canCreate = PermissionHelper.CanCreate(userId, "ProductService");
bool canUpdate = PermissionHelper.CanUpdate(userId, "ProductService");
bool canDelete = PermissionHelper.CanDelete(userId, "ProductService");
bool canApprove = PermissionHelper.CanApprove(userId, "StockInOutMaster");

// Sử dụng helper cho entity cụ thể
bool canReadProduct = PermissionHelper.ProductService.CanRead(userId);
bool canCreateProduct = PermissionHelper.ProductService.CanCreate(userId);
bool canApproveStock = PermissionHelper.StockInOutMaster.CanApprove(userId);
```

### 3. Kiểm tra Role

```csharp
// Kiểm tra user có role không
bool isAdmin = PermissionHelper.IsAdministrator(userId);
bool isManager = PermissionHelper.IsManager(userId);
bool hasRole = PermissionHelper.UserHasRole(userId, "Manager");
```

### 4. Lấy danh sách quyền

```csharp
// Lấy tất cả quyền của user
var permissions = PermissionHelper.GetUserPermissions(userId);

// Lấy quyền theo entity
var productPermissions = PermissionHelper.GetUserPermissionsByEntity(userId, "ProductService");

// Lấy danh sách actions mà user có quyền
var actions = PermissionHelper.GetUserActionsForEntity(userId, "ProductService");
// Kết quả: ["Read", "Create", "Update"]
```

### 5. Kiểm tra nhiều quyền cùng lúc

```csharp
// Kiểm tra user có tất cả quyền không
var requiredPermissions = new List<(string EntityName, string Action)>
{
    ("ProductService", "Read"),
    ("ProductService", "Create"),
    ("BusinessPartner", "Read")
};
bool hasAll = PermissionHelper.HasAllPermissions(userId, requiredPermissions);

// Kiểm tra user có ít nhất một quyền
bool hasAny = PermissionHelper.HasAnyPermission(userId, requiredPermissions);
```

## Tích hợp vào BLL Classes

### Ví dụ: ProductServiceBll

```csharp
public class ProductServiceBll
{
    private readonly PermissionBll _permissionBll;
    
    public ProductServiceBll()
    {
        _permissionBll = new PermissionBll();
    }
    
    public ProductService Create(ProductService product, Guid currentUserId)
    {
        // Kiểm tra quyền
        if (!_permissionBll.CanCreate(currentUserId, "ProductService"))
        {
            throw new UnauthorizedAccessException("Bạn không có quyền tạo sản phẩm");
        }
        
        // Logic tạo sản phẩm
        return _repository.Create(product);
    }
    
    public List<ProductService> GetAll(Guid currentUserId)
    {
        // Kiểm tra quyền
        if (!_permissionBll.CanRead(currentUserId, "ProductService"))
        {
            throw new UnauthorizedAccessException("Bạn không có quyền xem sản phẩm");
        }
        
        return _repository.GetAll();
    }
}
```

## Tích hợp vào UI (WinForms)

### Ví dụ: FrmProductServiceList

```csharp
public partial class FrmProductServiceList : Form
{
    private readonly PermissionBll _permissionBll;
    private Guid _currentUserId;
    
    public FrmProductServiceList(Guid currentUserId)
    {
        InitializeComponent();
        _permissionBll = new PermissionBll();
        _currentUserId = currentUserId;
        LoadPermissions();
    }
    
    private void LoadPermissions()
    {
        // Ẩn/hiện button dựa trên quyền
        btnAdd.Enabled = PermissionHelper.CanCreate(_currentUserId, "ProductService");
        btnEdit.Enabled = PermissionHelper.CanUpdate(_currentUserId, "ProductService");
        btnDelete.Enabled = PermissionHelper.CanDelete(_currentUserId, "ProductService");
        btnExport.Enabled = PermissionHelper.CanExport(_currentUserId, "ProductService");
        
        // Hoặc sử dụng helper cho entity cụ thể
        btnAdd.Enabled = PermissionHelper.ProductService.CanCreate(_currentUserId);
    }
    
    private void btnAdd_Click(object sender, EventArgs e)
    {
        // Kiểm tra quyền trước khi thực hiện
        if (!PermissionHelper.CanCreate(_currentUserId, "ProductService"))
        {
            MessageBox.Show("Bạn không có quyền tạo sản phẩm", "Thông báo", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        
        // Mở form tạo mới
        var frm = new FrmProductServiceDetail();
        frm.ShowDialog();
    }
}
```

## Quản lý Role và Permission

### Gán role cho user

```csharp
var permissionBll = new PermissionBll();

// Gán role Manager cho user
var managerRole = permissionBll.GetRoleByName("Manager");
permissionBll.AssignRoleToUser(userId, managerRole.Id, currentAdminId);
```

### Gán permission cho role

```csharp
// Lấy permission
var permission = permissionBll.GetPermission("ProductService", "Create");

// Gán cho role
permissionBll.AssignPermissionToRole(roleId, permission.Id, isGranted: true);
```

### Gán permission trực tiếp cho user (Override)

```csharp
// Gán quyền trực tiếp cho user (override quyền từ role)
var permission = permissionBll.GetPermission("ProductService", "Delete");
permissionBll.AssignPermissionToUser(userId, permission.Id, isGranted: true, createdBy: adminId);
```

## Các Entity đã được cấu hình

Hệ thống đã tự động tạo permissions cho **30 entities**:

### Version & User Management
- AllowedMacAddress
- ApplicationUser
- VnsErpApplicationVersion

### Master Data - Company
- Company
- CompanyBranch
- Department
- Employee
- Position

### Master Data - Business Partner
- BusinessPartner
- BusinessPartnerCategory
- BusinessPartner_BusinessPartnerCategory
- BusinessPartnerContact
- BusinessPartnerSite

### Master Data - Product/Service
- ProductService
- ProductServiceCategory
- ProductVariant
- ProductImage
- Attribute
- AttributeValue
- VariantAttribute
- UnitOfMeasure

### Inventory Management
- Asset
- Device
- DeviceHistory
- DeviceTransfer
- InventoryBalance
- StockInOutMaster
- StockInOutDetail
- StockInOutDocument
- StockInOutImage
- Warranty

## Các Actions mặc định

Mỗi entity có **4 actions** mặc định:
- **Read**: Xem/Đọc dữ liệu
- **Create**: Tạo mới
- **Update**: Cập nhật
- **Delete**: Xóa

Có thể thêm các actions khác:
- **Approve**: Phê duyệt (cho StockInOutMaster, DeviceTransfer)
- **Export**: Xuất dữ liệu
- **Import**: Nhập dữ liệu
- **Print**: In ấn

## Roles mặc định

1. **Administrator**: Toàn quyền (tất cả permissions)
2. **Manager**: Quyền quản lý cao
3. **User**: Quyền cơ bản
4. **Viewer**: Chỉ đọc (Read only)

## Lưu ý quan trọng

1. **Deny by Default**: Nếu không có quyền rõ ràng, hệ thống sẽ từ chối truy cập
2. **UserPermission Override**: Quyền từ `UserPermission` sẽ override quyền từ `Role`
3. **System Role**: Không thể xóa các role có `IsSystemRole = true`
4. **Performance**: Sử dụng View `vw_UserPermissions` và Stored Procedure `sp_CheckUserPermission` để tối ưu performance

## TODO: Cần implement

1. **GetCurrentUserId()**: Cần implement method `GetCurrentUserId()` trong `PermissionBll` để lấy user ID từ session/context
2. **Caching**: Có thể thêm caching cho permissions để tăng performance
3. **Audit Log**: Ghi log các lần kiểm tra quyền và truy cập bị từ chối

## Ví dụ hoàn chỉnh

Xem file: `Dal/Doc/EntityPermissionManagement_Proposal.md` và `Dal/Doc/EntityPermissionMatrix.md` để biết thêm chi tiết.

---

**Tác giả:** AI Assistant  
**Ngày tạo:** 2025-01-27  
**Phiên bản:** 1.0
