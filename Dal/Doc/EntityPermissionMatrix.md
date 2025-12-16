# Ma trận Quyền Truy cập cho Entity

## Tổng quan

Tài liệu này liệt kê tất cả 30 entities trong hệ thống VNS ERP 2025 và đề xuất cấu trúc quyền truy cập cho từng entity.

---

## Danh sách Entity và Quyền đề xuất

### 1. Version & User Management

| Entity | Read | Create | Update | Delete | Ghi chú |
|--------|------|--------|--------|--------|---------|
| **AllowedMacAddress** | ✅ | ✅ | ✅ | ✅ | Quản lý MAC address được phép |
| **ApplicationUser** | ✅ | ✅ | ✅ | ✅ | Quản lý người dùng (cần quyền cao) |
| **VnsErpApplicationVersion** | ✅ | ✅ | ✅ | ✅ | Quản lý phiên bản ứng dụng |

**Đề xuất Role:**
- **Administrator**: Full Access
- **Manager**: Read, Create, Update (không xóa)
- **User**: Read only
- **Viewer**: Read only

---

### 2. Master Data - Company

| Entity | Read | Create | Update | Delete | Ghi chú |
|--------|------|--------|--------|--------|---------|
| **Company** | ✅ | ✅ | ✅ | ⚠️ | Công ty (xóa cần quyền cao) |
| **CompanyBranch** | ✅ | ✅ | ✅ | ⚠️ | Chi nhánh (xóa cần quyền cao) |
| **Department** | ✅ | ✅ | ✅ | ⚠️ | Phòng ban (xóa cần quyền cao) |
| **Employee** | ✅ | ✅ | ✅ | ⚠️ | Nhân viên (xóa cần quyền cao) |
| **Position** | ✅ | ✅ | ✅ | ⚠️ | Chức vụ (xóa cần quyền cao) |

**Đề xuất Role:**
- **Administrator**: Full Access
- **Manager**: Read, Create, Update (Delete cần approval)
- **User**: Read, Create
- **Viewer**: Read only

---

### 3. Master Data - Business Partner

| Entity | Read | Create | Update | Delete | Ghi chú |
|--------|------|--------|--------|--------|---------|
| **BusinessPartner** | ✅ | ✅ | ✅ | ⚠️ | Đối tác kinh doanh |
| **BusinessPartnerCategory** | ✅ | ✅ | ✅ | ⚠️ | Danh mục đối tác |
| **BusinessPartner_BusinessPartnerCategory** | ✅ | ✅ | ✅ | ✅ | Liên kết đối tác - danh mục |
| **BusinessPartnerContact** | ✅ | ✅ | ✅ | ✅ | Liên hệ đối tác |
| **BusinessPartnerSite** | ✅ | ✅ | ✅ | ✅ | Địa điểm đối tác |

**Đề xuất Role:**
- **Administrator**: Full Access
- **Manager**: Read, Create, Update (Delete cần approval)
- **User**: Read, Create, Update
- **Viewer**: Read only

---

### 4. Master Data - Product/Service

| Entity | Read | Create | Update | Delete | Ghi chú |
|--------|------|--------|--------|--------|---------|
| **ProductService** | ✅ | ✅ | ✅ | ⚠️ | Sản phẩm/Dịch vụ (xóa cần quyền cao) |
| **ProductServiceCategory** | ✅ | ✅ | ✅ | ⚠️ | Danh mục sản phẩm/dịch vụ |
| **ProductVariant** | ✅ | ✅ | ✅ | ✅ | Biến thể sản phẩm |
| **ProductImage** | ✅ | ✅ | ✅ | ✅ | Hình ảnh sản phẩm |
| **Attribute** | ✅ | ✅ | ✅ | ⚠️ | Thuộc tính (xóa cần quyền cao) |
| **AttributeValue** | ✅ | ✅ | ✅ | ✅ | Giá trị thuộc tính |
| **VariantAttribute** | ✅ | ✅ | ✅ | ✅ | Thuộc tính biến thể |
| **UnitOfMeasure** | ✅ | ✅ | ✅ | ⚠️ | Đơn vị tính (xóa cần quyền cao) |

**Đề xuất Role:**
- **Administrator**: Full Access
- **Manager**: Read, Create, Update (Delete cho ProductService, ProductServiceCategory, Attribute, UnitOfMeasure cần approval)
- **User**: Read, Create, Update (cho ProductVariant, ProductImage, AttributeValue, VariantAttribute)
- **Viewer**: Read only

---

### 5. Inventory Management

| Entity | Read | Create | Update | Delete | Approve | Ghi chú |
|--------|------|--------|--------|--------|---------|---------|
| **Asset** | ✅ | ✅ | ✅ | ⚠️ | - | Tài sản |
| **Device** | ✅ | ✅ | ✅ | ⚠️ | - | Thiết bị |
| **DeviceHistory** | ✅ | ✅ | - | - | - | Lịch sử thiết bị (chỉ đọc và tạo) |
| **DeviceTransfer** | ✅ | ✅ | ✅ | ⚠️ | ✅ | Chuyển giao thiết bị (cần approve) |
| **InventoryBalance** | ✅ | ✅ | ✅ | - | - | Tồn kho (không xóa trực tiếp) |
| **StockInOutMaster** | ✅ | ✅ | ✅ | ⚠️ | ✅ | Phiếu nhập/xuất (cần approve) |
| **StockInOutDetail** | ✅ | ✅ | ✅ | ✅ | - | Chi tiết nhập/xuất |
| **StockInOutDocument** | ✅ | ✅ | ✅ | ✅ | - | Tài liệu nhập/xuất |
| **StockInOutImage** | ✅ | ✅ | ✅ | ✅ | - | Hình ảnh nhập/xuất |
| **Warranty** | ✅ | ✅ | ✅ | ⚠️ | - | Bảo hành |

**Đề xuất Role:**
- **Administrator**: Full Access + Approve
- **Manager**: Read, Create, Update, Approve (Delete cần approval)
- **User**: Read, Create, Update (không Approve, không Delete)
- **Viewer**: Read only

**Ghi chú đặc biệt:**
- **DeviceHistory**: Chỉ có thể tạo mới, không thể sửa/xóa (lịch sử không thể thay đổi)
- **InventoryBalance**: Không thể xóa trực tiếp, chỉ cập nhật qua StockInOut
- **StockInOutMaster**: Cần quyền Approve để phê duyệt phiếu
- **DeviceTransfer**: Cần quyền Approve để phê duyệt chuyển giao

---

## Ma trận Quyền theo Role

### Administrator
- **Tất cả entities**: Full Access (Read, Create, Update, Delete, Approve nếu có)

### Manager
- **Master Data**: Read, Create, Update (Delete cần approval cho các entity quan trọng)
- **Inventory**: Read, Create, Update, Approve (Delete cần approval)
- **System Entities**: Read, Create, Update (không Delete)

### User
- **Master Data**: Read, Create, Update (không Delete)
- **Inventory**: Read, Create, Update (không Approve, không Delete)
- **System Entities**: Read only

### Viewer
- **Tất cả entities**: Read only

---

## Quyền đặc biệt (Custom Permissions)

### 1. Export/Import
- **Export**: Xuất dữ liệu ra Excel/PDF
  - Áp dụng cho: ProductService, BusinessPartner, Employee, StockInOutMaster
- **Import**: Nhập dữ liệu từ Excel
  - Áp dụng cho: ProductService, BusinessPartner, Employee

### 2. Print
- **Print**: In ấn tài liệu
  - Áp dụng cho: StockInOutMaster, StockInOutDocument, Warranty

### 3. Approve
- **Approve**: Phê duyệt
  - Áp dụng cho: StockInOutMaster, DeviceTransfer

### 4. View Reports
- **ViewReports**: Xem báo cáo
  - Áp dụng cho: InventoryBalance, StockInOutMaster (báo cáo tồn kho, nhập xuất)

---

## Quy tắc Quyền (Permission Rules)

### 1. Cascade Permissions
- Nếu có quyền **Update** trên Master entity, tự động có quyền **Update** trên Detail entities
  - Ví dụ: Update StockInOutMaster → Update StockInOutDetail

### 2. Dependent Permissions
- Quyền **Delete** trên Master entity yêu cầu quyền **Delete** trên tất cả Detail entities
  - Ví dụ: Delete StockInOutMaster → Phải có quyền Delete StockInOutDetail

### 3. Read-Only Entities
- Một số entity chỉ cho phép Read:
  - **DeviceHistory**: Chỉ Read và Create (không Update/Delete)
  - **InventoryBalance**: Chỉ Read và Update (không Create/Delete trực tiếp)

### 4. Approval Workflow
- Các entity cần Approval:
  - **StockInOutMaster**: Cần Approve trước khi hoàn tất
  - **DeviceTransfer**: Cần Approve trước khi chuyển giao

---

## Implementation Notes

### 1. Permission Checking Order
1. Kiểm tra **UserPermission** trước (Override)
2. Nếu không có, kiểm tra **RolePermission**
3. Nếu không có, **Deny by Default**

### 2. Performance Optimization
- Cache quyền của user trong session
- Sử dụng View `vw_UserPermissions` để query nhanh
- Index trên các cột thường query

### 3. Audit Trail
- Ghi log tất cả các thay đổi quyền
- Ghi log các lần truy cập bị từ chối (Denied Access)

---

## Ví dụ Phân quyền

### Scenario 1: User thường
- **ProductService**: Read, Create, Update
- **BusinessPartner**: Read, Create, Update
- **StockInOutMaster**: Read, Create, Update (không Approve)
- **Employee**: Read only

### Scenario 2: Manager kho
- **ProductService**: Read, Create, Update
- **InventoryBalance**: Read, Update
- **StockInOutMaster**: Read, Create, Update, Approve
- **StockInOutDetail**: Read, Create, Update, Delete

### Scenario 3: Kế toán
- **BusinessPartner**: Read, Create, Update
- **StockInOutMaster**: Read, Create, Update, Approve
- **ProductService**: Read only
- **Employee**: Read only

---

## Migration Path

1. **Phase 1**: Tạo database schema (Role, Permission, RolePermission, UserRole, UserPermission)
2. **Phase 2**: Insert dữ liệu mặc định (4 roles, 120+ permissions)
3. **Phase 3**: Gán quyền mặc định cho các role
4. **Phase 4**: Tích hợp vào code (Repository, BLL)
5. **Phase 5**: Cập nhật UI để hiển thị/ẩn chức năng theo quyền

---

**Tác giả:** AI Assistant  
**Ngày tạo:** 2025-01-27  
**Phiên bản:** 1.0
