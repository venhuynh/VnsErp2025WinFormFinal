# VNS ERP 2025 - Tài Liệu Các Module

**Phiên bản:** 1.0  
**Ngày cập nhật:** 27/01/2025  
**Trạng thái:** Đang phát triển

---

## 1. Tổng Quan

Tài liệu này mô tả chi tiết các module chính trong hệ thống VNS ERP 2025, bao gồm chức năng, cấu trúc, và các tài liệu liên quan.

---

## 2. Authentication (Xác Thực)

### 2.1 Mục Đích
Module xác thực quản lý việc đăng nhập, xác thực người dùng và quản lý session.

### 2.2 Chức Năng Chính
- ✅ Đăng nhập/đăng xuất
- ✅ Quản lý session
- ✅ Cấu hình kết nối database
- ✅ Quản lý MAC address được phép

### 2.3 Cấu Trúc

```
Authentication/
├── Form/
│   ├── FrmLogin.cs                    # Form đăng nhập
│   ├── FrmDatabaseConfig.cs           # Form cấu hình database
│   └── FrmNASConfig.cs                # Form cấu hình NAS
└── Bll/
    └── Authentication/                 # Business logic
```

### 2.4 Tài Liệu
- **[FrmLogin - User Guide](../Authentication/Form/FrmLogin_User_Guide.md)**
- **[FrmLogin - Developer Guide](../Authentication/Form/FrmLogin_Developer_Guide.md)**
- **[FrmDatabaseConfig - User Guide](../Authentication/Form/FrmDatabaseConfig_User_Guide.md)**
- **[FrmDatabaseConfig - Developer Guide](../Authentication/Form/FrmDatabaseConfig_Developer_Guide.md)**

---

## 3. VersionAndUserManagement (Quản Lý Phiên Bản & Người Dùng)

### 3.1 Mục Đích
Module quản lý phiên bản ứng dụng, người dùng, vai trò và quyền truy cập.

### 3.2 Các Module Con

#### 3.2.1 ApplicationVersion
- **Mục đích:** Quản lý phiên bản ứng dụng
- **Chức năng:**
  - Lưu trữ thông tin phiên bản
  - Kiểm tra phiên bản mới
  - Quản lý cập nhật

#### 3.2.2 UserManagement
- **Mục đích:** Quản lý người dùng hệ thống
- **Chức năng:**
  - Thêm/sửa/xóa người dùng
  - Quản lý thông tin người dùng
  - Kích hoạt/vô hiệu hóa người dùng

**Forms:**
- `FrmApplicationUserDto` - Danh sách người dùng
- `FrmApplicationUserDtoAddEdit` - Thêm/sửa người dùng

#### 3.2.3 RoleManagement
- **Mục đích:** Quản lý vai trò (Role) trong hệ thống
- **Chức năng:**
  - Thêm/sửa/xóa vai trò
  - Quản lý thông tin vai trò
  - Phân loại vai trò hệ thống

**Forms:**
- `FrmRoleManagement` - Quản lý vai trò

**DTOs:**
- `RoleDto` - DTO cho vai trò

#### 3.2.4 PermissionManagement
- **Mục đích:** Quản lý quyền truy cập (Permission) trong hệ thống
- **Chức năng:**
  - Quản lý quyền theo Entity và Action
  - Gán quyền cho Role
  - Gán quyền trực tiếp cho User (Override)
  - Xem tổng hợp quyền của User

**Forms (Đề xuất):**
- `FrmRoleManagement` - Quản lý Role
- `FrmPermissionManagement` - Quản lý Permission
- `FrmAssignRoleToUser` - Gán Role cho User
- `FrmAssignPermissionToRole` - Gán Permission cho Role
- `FrmAssignPermissionToUser` - Gán Permission trực tiếp cho User
- `FrmUserPermissionSummary` - Xem tổng hợp quyền của User

**Tài Liệu:**
- **[UI Design Proposal](../VersionAndUserManagement/PermissionManagement/UI_Design_Proposal.md)**
- **[UI Implementation Guide](../VersionAndUserManagement/PermissionManagement/UI_Implementation_Guide.md)**
- **[Entity Permission Matrix](../Dal/Doc/EntityPermissionMatrix.md)**
- **[Entity Permission Management Proposal](../Dal/Doc/EntityPermissionManagement_Proposal.md)**

### 3.3 Cấu Trúc

```
VersionAndUserManagement/
├── ApplicationVersion/
├── UserManagement/
│   ├── FrmApplicationUserDto.cs
│   └── FrmApplicationUserDtoAddEdit.cs
├── RoleManagement/
│   └── FrmRoleManagement.cs
├── PermissionManagement/
│   └── [Documentation files]
└── AllowedMacAddress/
```

### 3.4 DTOs

```
DTO/VersionAndUserManagementDto/
├── ApplicationUserDto.cs
├── ApplicationVersionDto.cs
├── RoleDto.cs
├── PermissionDto.cs
├── RolePermissionDto.cs
├── UserPermissionDto.cs
├── UserRoleDto.cs
└── UserPermissionSummaryDto.cs
```

---

## 4. MasterData (Dữ Liệu Master)

### 4.1 Mục Đích
Module quản lý các dữ liệu cơ bản (master data) của hệ thống.

### 4.2 Các Module Con

#### 4.2.1 Company (Công Ty)
- **Mục đích:** Quản lý công ty, chi nhánh, phòng ban
- **Chức năng:**
  - Quản lý thông tin công ty
  - Quản lý chi nhánh
  - Quản lý phòng ban (cây phòng ban)

**Forms:**
- `FrmCompany` - Quản lý công ty
- `FrmCompanyBranch` - Quản lý chi nhánh
- `FrmCompanyBranchDetail` - Chi tiết chi nhánh
- `FrmDepartmentDetail` - Chi tiết phòng ban

**User Controls:**
- `UcCompany` - User control công ty
- `UcCompanyBranch` - User control chi nhánh

**Tài Liệu:**
- **[FrmCompany - User Guide](../MasterData/Company/FrmCompany_User_Guide.md)**
- **[FrmCompany - Developer Guide](../MasterData/Company/FrmCompany_Developer_Guide.md)**
- **[FrmCompanyBranch - User Guide](../MasterData/Company/FrmCompanyBranch_User_Guide.md)**
- **[FrmCompanyBranch - Developer Guide](../MasterData/Company/FrmCompanyBranch_Developer_Guide.md)**
- **[DepartmentTreeList Implementation](../MasterData/Company/DepartmentTreeList_Implementation.md)**

#### 4.2.2 BusinessPartner (Đối Tác Kinh Doanh)
- **Mục đích:** Quản lý đối tác kinh doanh (khách hàng, nhà cung cấp)
- **Chức năng:**
  - Quản lý thông tin đối tác
  - Quản lý danh mục đối tác
  - Quản lý liên hệ

**Forms:**
- `FrmBusinessPartnerCategory` - Danh mục đối tác

**Tài Liệu:**
- **[BusinessPartner Database Schema](../MasterData/Doc/BusinessPartner_Database_Schema.md)**
- **[FrmBusinessPartnerCategory - User Guide](../MasterData/Company/FrmBusinessPartnerCategory_User_Guide.md)**

#### 4.2.3 ProductService (Sản Phẩm/Dịch Vụ)
- **Mục đích:** Quản lý sản phẩm, dịch vụ, danh mục, biến thể
- **Chức năng:**
  - Quản lý danh mục sản phẩm/dịch vụ
  - Quản lý sản phẩm/dịch vụ
  - Quản lý biến thể sản phẩm
  - Quản lý thuộc tính
  - Quản lý hình ảnh sản phẩm

**Forms:**
- `FrmProductServiceCategory` - Danh mục sản phẩm/dịch vụ
- `FrmProductServiceList` - Danh sách sản phẩm/dịch vụ
- `FrmProductServiceDetail` - Chi tiết sản phẩm/dịch vụ
- `FrmProductVariant` - Biến thể sản phẩm
- `FrmProductVariantDetail` - Chi tiết biến thể
- `FrmAttribute` - Thuộc tính
- `FrmProductImage` - Hình ảnh sản phẩm

**Tài Liệu:**
- **[ProductService Implementation Guide](../MasterData/ProductService/IMPLEMENTATION_GUIDE.md)**

#### 4.2.4 Customer (Khách Hàng)
- **Mục đích:** Quản lý khách hàng
- **Chức năng:**
  - Quản lý thông tin khách hàng
  - Quản lý lịch sử giao dịch

### 4.3 Cấu Trúc

```
MasterData/
├── Company/
│   ├── FrmCompany.cs
│   ├── FrmCompanyBranch.cs
│   └── ...
├── BusinessPartner/
├── ProductService/
│   ├── FrmProductServiceList.cs
│   ├── FrmProductServiceDetail.cs
│   └── ...
└── Customer/
```

---

## 5. Inventory (Quản Lý Kho)

### 5.1 Mục Đích
Module quản lý kho, nhập/xuất kho, tồn kho.

### 5.2 Các Module Con

#### 5.2.1 StockIn (Nhập Kho)
- **Mục đích:** Quản lý các loại nhập kho
- **Các loại nhập kho:**
  - Nhập kho đối bán
  - Nhập kho theo PO nhà cung cấp
  - Nhập kho theo PO khách hàng

**Tài Liệu:**
- **[Nhập Hàng Thương Mại - Tổng Quan](../Inventory/Doc/NhapHang/NhapHangThuongMai_00_TongQuan.md)**
- **[Nhập Kho Đối Bán](../Inventory/Doc/NhapHang/NhapHangThuongMai_01_NhapKhoDoiBan.md)**
- **[Nhập Kho Theo PO Nhà Cung Cấp](../Inventory/Doc/NhapHang/NhapHangThuongMai_02_NhapKhoTheoPONCC.md)**
- **[Nhập Kho Theo PO Khách Hàng](../Inventory/Doc/NhapHang/NhapHangThuongMai_03_NhapKhoTheoPOCustomer.md)**
- **[In Phiếu Nhập Kho](../Inventory/StockIn/InPhieu/README_IN_PHIEU_NHAP_KHO.md)**

#### 5.2.2 StockOut (Xuất Kho)
- **Mục đích:** Quản lý các loại xuất kho
- **Các loại xuất kho:**
  - Xuất kho thương mại
  - Xuất bảo hành
  - Xuất lắp ráp
  - Xuất lưu chuyển kho
  - Xuất nội bộ
  - Xuất cho thuê mượn

**Forms:**
- `FrmXuatKhoThuongMai` - Xuất kho thương mại
- `FrmXuatBaoHanh` - Xuất bảo hành
- `FrmXuatLapRap` - Xuất lắp ráp
- `FrmXuatLuuChuyenKho` - Xuất lưu chuyển kho
- `FrmXuatNoiBo` - Xuất nội bộ
- `FrmXuatChoThueMuon` - Xuất cho thuê mượn

#### 5.2.3 Management (Quản Lý)
- **Mục đích:** Quản lý tồn kho, tài sản
- **Forms:**
  - `FrmInventoryBalanceDto` - Tồn kho
  - `FrmAssetDtoManagement` - Quản lý tài sản

#### 5.2.4 Query (Tra Cứu)
- **Mục đích:** Tra cứu lịch sử, kiểm tra bảo hành
- **Forms:**
  - `FrmStockInOutMasterHistory` - Lịch sử phiếu nhập/xuất
  - `FrmStockInOutProductHistory` - Lịch sử sản phẩm
  - `FrmWarrantyCheck` - Kiểm tra bảo hành
  - `FrmStockInOutDocumentDtoLookup` - Tra cứu phiếu
  - `FrmStockInOutImageLookup` - Tra cứu hình ảnh

**Tài Liệu:**
- **[Stock In History Query](../DTO/Inventory/StockIn/README_STOCK_IN_HISTORY_QUERY.md)**

### 5.3 Tài Liệu Database
- **[Database Schema - StockInOutDocument](../Inventory/Doc/DatabaseSchema_StockInOutDocument_Proposal.md)**
- **[Database Schema - InventoryBalance](../Docs/DatabaseSchema_InventoryBalance_Proposal.md)**

### 5.4 Cấu Trúc

```
Inventory/
├── StockIn/
│   ├── InPhieu/
│   └── ...
├── StockOut/
│   ├── XuatHangThuongMai/
│   ├── XuatBaoHanh/
│   ├── XuatLapRap/
│   ├── XuatLuuChuyenKho/
│   ├── XuatNoiBo/
│   └── XuatChoThueMuon/
├── Management/
│   ├── FrmInventoryBalanceDto.cs
│   └── FrmAssetDtoManagement.cs
└── Query/
    ├── FrmStockInOutMasterHistory.cs
    └── ...
```

---

## 6. AssemblyManufacturing (Lắp Ráp & Sản Xuất)

### 6.1 Mục Đích
Module quản lý quy trình lắp ráp và sản xuất.

### 6.2 Chức Năng
- Quản lý quy trình lắp ráp
- Quản lý tháo rời
- Quản lý sản xuất

### 6.3 Tài Liệu
- **[Assembly Manufacturing Plan](../Docs/AssemblyManufacturing_Plan.md)**
- **[Assembly Manufacturing UI Workflow](../Docs/AssemblyManufacturing_UI_Workflow.md)**
- **[Assembly Manufacturing Business Logic](../Docs/AssemblyManufacturing_BusinessLogic.md)**
- **[Assembly Manufacturing Disassembly Logic](../Docs/AssemblyManufacturing_Disassembly_Logic.md)**
- **[Assembly Manufacturing Implementation Steps](../Docs/AssemblyManufacturing_Implementation_Steps.md)**

---

## 7. Common (Dùng Chung)

### 7.1 Mục Đích
Các class và component dùng chung cho toàn hệ thống.

### 7.2 Các Component
- Validation system
- Image storage
- Image service
- Logger
- Utilities

### 7.3 Tài Liệu
- **[Validation System](../Common/Validation/README.md)**
- **[Image Storage](../Bll/Common/ImageStorage/README.md)**
- **[Image Service](../Bll/Common/ImageService/README.md)**
- **[Logger](../Logger/README.md)**

---

## 8. DTO (Data Transfer Objects)

### 8.1 Mục Đích
Các DTO để truyền dữ liệu giữa các layer.

### 8.2 Cấu Trúc

```
DTO/
├── Inventory/
│   └── [Inventory DTOs]
├── MasterData/
│   └── [MasterData DTOs]
└── VersionAndUserManagementDto/
    ├── ApplicationUserDto.cs
    ├── RoleDto.cs
    ├── PermissionDto.cs
    └── ...
```

---

## 9. Database

### 9.1 Các Schema Chính
- ApplicationUser, Role, Permission
- Company, CompanyBranch, Department
- BusinessPartner, BusinessPartnerContact
- ProductService, ProductServiceCategory, ProductVariant
- StockInOutMaster, StockInOutDetail
- InventoryBalance

### 9.2 Tài Liệu
Xem **[Database Documentation](./Database_Documentation.md)**

---

## 10. Tổng Kết

### 10.1 Module Hoàn Thành
- ✅ Authentication
- ✅ VersionAndUserManagement (cơ bản)
- ✅ MasterData (một phần)
- ✅ Inventory (một phần)

### 10.2 Module Đang Phát Triển
- 🔄 PermissionManagement (UI design)
- 🔄 AssemblyManufacturing
- 🔄 MasterData (mở rộng)

### 10.3 Module Dự Kiến
- 📋 Financial Management
- 📋 Reporting System
- 📋 Integration Module

---

**Người tạo:** Development Team  
**Ngày tạo:** 27/01/2025  
**Trạng thái:** Đang phát triển
