# Đề xuất UI cho Hệ thống Quản lý Quyền (Permission Management System)

## Tổng quan

Tài liệu này đề xuất các form UI cho hệ thống quản lý quyền truy cập, bao gồm:
1. Quản lý Role
2. Quản lý Permission
3. Gán Role cho User
4. Gán Permission cho Role
5. Gán Permission trực tiếp cho User (Override)
6. Xem tổng hợp quyền của User

---

## 1. Form Quản lý Role (FrmRoleManagement.cs)

### 1.1. Mục đích
- Hiển thị danh sách tất cả Roles
- Thêm, sửa, xóa Role
- Xem chi tiết Role (số lượng users, permissions)

### 1.2. Layout

```
┌─────────────────────────────────────────────────────────────┐
│  QUẢN LÝ VAI TRÒ (ROLE MANAGEMENT)                          │
├─────────────────────────────────────────────────────────────┤
│  [Toolbar]                                                  │
│  [Thêm mới] [Sửa] [Xóa] [Làm mới] [Xuất Excel] [Đóng]     │
├─────────────────────────────────────────────────────────────┤
│  [GridControl - Danh sách Roles]                           │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Name | Description | IsSystemRole | IsActive | ...   │ │
│  ├───────────────────────────────────────────────────────┤ │
│  │ Administrator | Quản trị viên | ✓ | ✓ | ...         │ │
│  │ Manager | Quản lý | ✗ | ✓ | ...                      │ │
│  │ User | Người dùng | ✗ | ✓ | ...                     │ │
│  │ Viewer | Người xem | ✗ | ✓ | ...                     │ │
│  └───────────────────────────────────────────────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  [TabControl]                                               │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 1: Thông tin Role                                │ │
│  │ - Tên vai trò: [TextBox]                             │ │
│  │ - Mô tả: [MemoEdit]                                  │ │
│  │ - Là vai trò hệ thống: [CheckEdit] (readonly)        │ │
│  │ - Đang hoạt động: [CheckEdit]                        │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 2: Danh sách Users có Role này                   │ │
│  │ [GridControl - UserRoleDto]                          │ │
│  │ UserName | AssignedDate | AssignedBy | IsActive      │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 3: Danh sách Permissions của Role                │ │
│  │ [GridControl - RolePermissionDto]                    │ │
│  │ EntityName | Action | IsGranted | CreatedDate        │ │
│  │ [Nút: Thêm Permission] [Nút: Xóa Permission]        │ │
│  └───────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

### 1.3. Chức năng chính
- **Thêm mới Role**: Mở form `FrmRoleAddEdit`
- **Sửa Role**: Mở form `FrmRoleAddEdit` với mode Edit
- **Xóa Role**: Soft delete (chỉ xóa được nếu không phải System Role)
- **Làm mới**: Reload danh sách từ database
- **Xuất Excel**: Export danh sách Role ra Excel
- **Xem chi tiết**: Click vào row để xem thông tin chi tiết ở TabControl

### 1.4. Validation
- Tên Role không được trống
- Tên Role không được trùng
- Không thể xóa System Role
- Không thể đổi tên System Role

---

## 2. Form Thêm/Sửa Role (FrmRoleAddEdit.cs)

### 2.1. Mục đích
- Form popup để thêm mới hoặc sửa Role

### 2.2. Layout

```
┌─────────────────────────────────────────────┐
│  THÊM MỚI / SỬA VAI TRÒ                     │
├─────────────────────────────────────────────┤
│  Tên vai trò: [TextBox] *                    │
│  ┌───────────────────────────────────────┐  │
│  │                                       │  │
│  └───────────────────────────────────────┘  │
│                                              │
│  Mô tả: [MemoEdit]                          │
│  ┌───────────────────────────────────────┐  │
│  │                                       │  │
│  │                                       │  │
│  │                                       │  │
│  └───────────────────────────────────────┘  │
│                                              │
│  ☑ Đang hoạt động                           │
│                                              │
│  [Lưu] [Hủy]                                │
└─────────────────────────────────────────────┘
```

### 2.3. Chức năng
- Validate input
- Save/Cancel
- Hiển thị thông báo lỗi nếu có

---

## 3. Form Quản lý Permission (FrmPermissionManagement.cs)

### 3.1. Mục đích
- Hiển thị danh sách tất cả Permissions
- Lọc theo Entity
- Xem chi tiết Permission

### 3.2. Layout

```
┌─────────────────────────────────────────────────────────────┐
│  QUẢN LÝ QUYỀN TRUY CẬP (PERMISSION MANAGEMENT)             │
├─────────────────────────────────────────────────────────────┤
│  [Toolbar]                                                  │
│  [Làm mới] [Xuất Excel] [Đóng]                             │
├─────────────────────────────────────────────────────────────┤
│  [Filter Panel]                                             │
│  Entity: [ComboBox - All Entities] [Lọc]                   │
│  Action: [ComboBox - All Actions]                          │
├─────────────────────────────────────────────────────────────┤
│  [GridControl - Danh sách Permissions]                      │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ EntityName | Action | Description | IsActive | ...   │ │
│  ├───────────────────────────────────────────────────────┤ │
│  │ ProductService | Read | Quyền Read... | ✓ | ...     │ │
│  │ ProductService | Create | Quyền Create... | ✓ | ... │ │
│  │ BusinessPartner | Read | Quyền Read... | ✓ | ...    │ │
│  └───────────────────────────────────────────────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  [TabControl]                                               │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 1: Thông tin Permission                           │ │
│  │ - Entity Name: [TextBox] (readonly)                  │ │
│  │ - Action: [TextBox] (readonly)                       │ │
│  │ - Description: [MemoEdit]                            │ │
│  │ - Đang hoạt động: [CheckEdit]                        │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 2: Danh sách Roles có Permission này             │ │
│  │ [GridControl - RolePermissionDto]                    │ │
│  │ RoleName | IsGranted | CreatedDate                   │ │
│  └───────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

### 3.3. Chức năng
- Lọc theo Entity và Action
- Xem chi tiết Permission
- Xem các Roles có Permission này

---

## 4. Form Gán Role cho User (FrmAssignRoleToUser.cs)

### 4.1. Mục đích
- Gán hoặc gỡ Role cho User
- Xem danh sách Users và Roles của họ

### 4.2. Layout

```
┌─────────────────────────────────────────────────────────────┐
│  GÁN VAI TRÒ CHO NGƯỜI DÙNG                                 │
├─────────────────────────────────────────────────────────────┤
│  [Filter Panel]                                             │
│  Tìm kiếm User: [TextBox] [Tìm]                            │
│  Lọc theo Role: [ComboBox]                                 │
├─────────────────────────────────────────────────────────────┤
│  [SplitContainer - Horizontal]                              │
│  ┌──────────────────────────┬──────────────────────────┐ │
│  │  DANH SÁCH USERS         │  ROLES CỦA USER          │ │
│  ├──────────────────────────┼──────────────────────────┤ │
│  │ [GridControl]            │ [GridControl]             │ │
│  │ UserName | Active | ... │ RoleName | IsActive | ... │ │
│  │                          │                           │ │
│  │                          │ [Nút: Gỡ Role]            │ │
│  │                          │                           │ │
│  │                          │ [ComboBox: Chọn Role]     │ │
│  │                          │ [Nút: Gán Role]           │ │
│  └──────────────────────────┴──────────────────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  [Lưu] [Hủy]                                               │
└─────────────────────────────────────────────────────────────┘
```

### 4.3. Chức năng
- Chọn User từ danh sách bên trái
- Xem Roles của User bên phải
- Gán Role mới cho User
- Gỡ Role khỏi User
- Lưu thay đổi

---

## 5. Form Gán Permission cho Role (FrmAssignPermissionToRole.cs)

### 5.1. Mục đích
- Gán hoặc gỡ Permissions cho Role
- Xem danh sách Permissions của Role

### 5.2. Layout

```
┌─────────────────────────────────────────────────────────────┐
│  GÁN QUYỀN CHO VAI TRÒ                                      │
├─────────────────────────────────────────────────────────────┤
│  Chọn Role: [ComboBox - All Roles] [Chọn]                  │
├─────────────────────────────────────────────────────────────┤
│  [SplitContainer - Horizontal]                              │
│  ┌──────────────────────────┬──────────────────────────┐ │
│  │  TẤT CẢ PERMISSIONS     │  PERMISSIONS CỦA ROLE    │ │
│  ├──────────────────────────┼──────────────────────────┤ │
│  │ [Filter]                 │ [GridControl]             │ │
│  │ Entity: [ComboBox]       │ Entity | Action | ...     │ │
│  │ Action: [ComboBox]       │                           │ │
│  │                          │ [Nút: Gỡ Permission]      │ │
│  │ [TreeList/GridControl]  │                           │ │
│  │ ☑ ProductService        │                           │ │
│  │   ☑ Read                 │                           │ │
│  │   ☑ Create               │                           │ │
│  │   ☑ Update               │                           │ │
│  │   ☑ Delete               │                           │ │
│  │ ☑ BusinessPartner        │                           │ │
│  │   ☑ Read                 │                           │ │
│  │   ☑ Create               │                           │ │
│  │                          │                           │ │
│  │ [Nút: Gán đã chọn]       │                           │ │
│  └──────────────────────────┴──────────────────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  [Lưu] [Hủy]                                               │
└─────────────────────────────────────────────────────────────┘
```

### 5.3. Chức năng
- Chọn Role từ ComboBox
- Hiển thị tất cả Permissions dạng TreeList (group by Entity)
- Check/Uncheck Permissions để gán
- Xem Permissions đã gán cho Role
- Gỡ Permission khỏi Role
- Lưu thay đổi

---

## 6. Form Gán Permission trực tiếp cho User (FrmAssignPermissionToUser.cs)

### 6.1. Mục đích
- Gán Permission trực tiếp cho User (Override quyền từ Role)
- Xem Permissions của User (từ Role + Override)

### 6.2. Layout

```
┌─────────────────────────────────────────────────────────────┐
│  GÁN QUYỀN TRỰC TIẾP CHO NGƯỜI DÙNG (OVERRIDE)              │
├─────────────────────────────────────────────────────────────┤
│  Chọn User: [ComboBox - All Users] [Chọn]                  │
├─────────────────────────────────────────────────────────────┤
│  [TabControl]                                               │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 1: Permissions từ Role                            │ │
│  │ [GridControl - ReadOnly]                              │ │
│  │ Entity | Action | RoleName | Source                   │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 2: Permissions trực tiếp (Override)              │ │
│  │ [GridControl]                                         │ │
│  │ Entity | Action | IsGranted | IsOverride | ...       │ │
│  │ [Nút: Thêm Permission] [Nút: Xóa Permission]         │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 3: Tất cả Permissions (Tổng hợp)                 │ │
│  │ [GridControl - ReadOnly]                              │ │
│  │ Entity | Action | Source (Role/User) | IsGranted     │ │
│  └───────────────────────────────────────────────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  [Lưu] [Hủy]                                               │
└─────────────────────────────────────────────────────────────┘
```

### 6.3. Chức năng
- Chọn User từ ComboBox
- Xem Permissions từ Role (readonly)
- Thêm/Xóa Permissions trực tiếp (Override)
- Xem tổng hợp tất cả Permissions

---

## 7. Form Xem Tổng hợp Quyền của User (FrmUserPermissionSummary.cs)

### 7.1. Mục đích
- Xem tổng hợp đầy đủ quyền của User
- Hiển thị Roles, Permissions từ Role, Permissions trực tiếp

### 7.2. Layout

```
┌─────────────────────────────────────────────────────────────┐
│  TỔNG HỢP QUYỀN CỦA NGƯỜI DÙNG                              │
├─────────────────────────────────────────────────────────────┤
│  Chọn User: [ComboBox - All Users] [Xem]                   │
├─────────────────────────────────────────────────────────────┤
│  [Info Panel]                                               │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tên người dùng: [Label]                               │ │
│  │ Số lượng Roles: [Label]                                │ │
│  │ Số lượng Permissions: [Label]                          │ │
│  │ Số lượng Override: [Label]                             │ │
│  └───────────────────────────────────────────────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  [TabControl]                                               │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 1: Danh sách Roles                                │ │
│  │ [GridControl - RoleDto]                               │ │
│  │ Name | Description | IsSystemRole | IsActive | ...   │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 2: Permissions từ Role                           │ │
│  │ [GridControl - PermissionDto]                        │ │
│  │ EntityName | Action | RoleName | Description | ...   │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 3: Permissions trực tiếp (Override)               │ │
│  │ [GridControl - PermissionDto]                        │ │
│  │ EntityName | Action | IsGranted | CreatedDate | ...  │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 4: Tất cả Permissions (Tổng hợp)                  │ │
│  │ [Filter: Entity] [Filter: Action]                    │ │
│  │ [GridControl - PermissionDto]                        │ │
│  │ EntityName | Action | Source | IsGranted | ...       │ │
│  │ [Nút: Xuất Excel]                                    │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 5: Kiểm tra Quyền                                 │ │
│  │ Entity: [ComboBox]                                    │ │
│  │ Action: [ComboBox]                                    │ │
│  │ [Nút: Kiểm tra]                                       │ │
│  │ Kết quả: [Label - Có quyền / Không có quyền]         │ │
│  └───────────────────────────────────────────────────────┘ │
├─────────────────────────────────────────────────────────────┤
│  [Đóng] [In báo cáo]                                       │
└─────────────────────────────────────────────────────────────┘
```

### 7.3. Chức năng
- Chọn User và xem tổng hợp quyền
- Xem Roles của User
- Xem Permissions từ Role
- Xem Permissions trực tiếp (Override)
- Xem tất cả Permissions (tổng hợp)
- Kiểm tra quyền cụ thể
- Xuất Excel báo cáo

---

## 8. Form Quản lý Quyền nhanh (FrmQuickPermissionManagement.cs)

### 8.1. Mục đích
- Form tổng hợp để quản lý quyền nhanh
- Gán Role cho User, Gán Permission cho Role trong một form

### 8.2. Layout

```
┌─────────────────────────────────────────────────────────────┐
│  QUẢN LÝ QUYỀN NHANH                                        │
├─────────────────────────────────────────────────────────────┤
│  [TabControl]                                               │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 1: Gán Role cho User                               │ │
│  │ (Tương tự FrmAssignRoleToUser)                        │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 2: Gán Permission cho Role                         │ │
│  │ (Tương tự FrmAssignPermissionToRole)                  │ │
│  └───────────────────────────────────────────────────────┘ │
│  ┌───────────────────────────────────────────────────────┐ │
│  │ Tab 3: Gán Permission trực tiếp cho User              │ │
│  │ (Tương tự FrmAssignPermissionToUser)                  │ │
│  └───────────────────────────────────────────────────────┘ │
└─────────────────────────────────────────────────────────────┘
```

---

## 9. Menu Integration

### 9.1. Thêm vào Menu chính

```
Hệ thống
├── Quản lý người dùng
│   ├── Danh sách người dùng
│   ├── Gán vai trò cho người dùng          [NEW]
│   └── Xem quyền của người dùng            [NEW]
├── Quản lý vai trò                         [NEW]
│   ├── Danh sách vai trò
│   └── Gán quyền cho vai trò
├── Quản lý quyền                            [NEW]
│   ├── Danh sách quyền
│   └── Quản lý quyền nhanh
└── ...
```

---

## 10. Components sử dụng (DevExpress)

### 10.1. GridControl
- Hiển thị danh sách Roles, Permissions, Users
- Hỗ trợ sorting, filtering, grouping
- Export Excel

### 10.2. TreeList
- Hiển thị Permissions theo cấu trúc cây (Entity > Action)
- Check/Uncheck để gán quyền

### 10.3. ComboBox
- Chọn User, Role, Entity, Action
- Auto-complete

### 10.4. TabControl
- Tổ chức thông tin theo tabs

### 10.5. SplitContainer
- Chia màn hình thành 2 phần (Users | Roles)

### 10.6. MemoEdit
- Nhập mô tả dài

### 10.7. CheckEdit
- Checkbox cho các trường boolean

---

## 11. Validation Rules

### 11.1. Role
- Tên Role: Required, MaxLength(100), Unique
- Không thể xóa System Role
- Không thể đổi tên System Role

### 11.2. Permission
- EntityName: Required, MaxLength(100)
- Action: Required, MaxLength(50)
- Unique(EntityName, Action)

### 11.3. UserRole
- UserId: Required
- RoleId: Required
- Unique(UserId, RoleId)

### 11.4. RolePermission
- RoleId: Required
- PermissionId: Required
- Unique(RoleId, PermissionId)

### 11.5. UserPermission
- UserId: Required
- PermissionId: Required
- Unique(UserId, PermissionId)

---

## 12. Security trong UI

### 12.1. Kiểm tra quyền trước khi hiển thị
```csharp
// Ẩn/hiện button dựa trên quyền
btnAdd.Enabled = PermissionHelper.CanCreate(currentUserId, "Role");
btnEdit.Enabled = PermissionHelper.CanUpdate(currentUserId, "Role");
btnDelete.Enabled = PermissionHelper.CanDelete(currentUserId, "Role");
```

### 12.2. Kiểm tra quyền trước khi thực hiện action
```csharp
private void btnAdd_Click(object sender, EventArgs e)
{
    if (!PermissionHelper.CanCreate(currentUserId, "Role"))
    {
        MessageBox.Show("Bạn không có quyền tạo vai trò", "Thông báo");
        return;
    }
    // ...
}
```

---

## 13. File Structure

```
VersionAndUserManagement/
├── PermissionManagement/
│   ├── FrmRoleManagement.cs
│   ├── FrmRoleManagement.Designer.cs
│   ├── FrmRoleManagement.resx
│   ├── FrmRoleAddEdit.cs
│   ├── FrmRoleAddEdit.Designer.cs
│   ├── FrmRoleAddEdit.resx
│   ├── FrmPermissionManagement.cs
│   ├── FrmPermissionManagement.Designer.cs
│   ├── FrmPermissionManagement.resx
│   ├── FrmAssignRoleToUser.cs
│   ├── FrmAssignRoleToUser.Designer.cs
│   ├── FrmAssignRoleToUser.resx
│   ├── FrmAssignPermissionToRole.cs
│   ├── FrmAssignPermissionToRole.Designer.cs
│   ├── FrmAssignPermissionToRole.resx
│   ├── FrmAssignPermissionToUser.cs
│   ├── FrmAssignPermissionToUser.Designer.cs
│   ├── FrmAssignPermissionToUser.resx
│   ├── FrmUserPermissionSummary.cs
│   ├── FrmUserPermissionSummary.Designer.cs
│   ├── FrmUserPermissionSummary.resx
│   └── FrmQuickPermissionManagement.cs
│   ├── FrmQuickPermissionManagement.Designer.cs
│   └── FrmQuickPermissionManagement.resx
```

---

## 14. Implementation Priority

### Phase 1: Core Forms (Ưu tiên cao)
1. ✅ FrmRoleManagement - Quản lý Role
2. ✅ FrmRoleAddEdit - Thêm/Sửa Role
3. ✅ FrmAssignRoleToUser - Gán Role cho User

### Phase 2: Permission Management (Ưu tiên trung bình)
4. ✅ FrmPermissionManagement - Quản lý Permission
5. ✅ FrmAssignPermissionToRole - Gán Permission cho Role

### Phase 3: Advanced Features (Ưu tiên thấp)
6. ✅ FrmAssignPermissionToUser - Gán Permission trực tiếp
7. ✅ FrmUserPermissionSummary - Xem tổng hợp quyền
8. ✅ FrmQuickPermissionManagement - Quản lý nhanh

---

## 15. Best Practices

1. **Consistent UI**: Sử dụng cùng pattern với các form hiện có
2. **Validation**: Validate input ở cả client và server
3. **Error Handling**: Hiển thị thông báo lỗi rõ ràng
4. **Loading State**: Hiển thị loading khi đang xử lý
5. **Confirmation**: Xác nhận trước khi xóa
6. **Refresh**: Tự động refresh sau khi thêm/sửa/xóa
7. **Search/Filter**: Hỗ trợ tìm kiếm và lọc dữ liệu
8. **Export**: Cho phép xuất Excel
9. **Permission Check**: Kiểm tra quyền ở mọi nơi cần thiết
10. **User Experience**: UI thân thiện, dễ sử dụng

---

**Tác giả:** AI Assistant  
**Ngày tạo:** 2025-01-27  
**Phiên bản:** 1.0
