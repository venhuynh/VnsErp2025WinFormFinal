# VNS ERP 2025 - Chỉ Mục Tài Liệu

**Phiên bản:** 1.0  
**Ngày cập nhật:** 27/01/2025  
**Trạng thái:** Đang phát triển

---

## 📚 Tài Liệu Tổng Quan

### 1. Tài Liệu Hệ Thống
- **[README](./README.md)** - Trang chủ tài liệu, hướng dẫn bắt đầu
- **[Tổng Quan Hệ Thống](./System_Overview.md)** - Giới thiệu tổng quan về hệ thống VNS ERP 2025
- **[Kiến Trúc Hệ Thống](./System_Architecture.md)** - Chi tiết về kiến trúc 3 lớp và design patterns
- **[Tài Liệu Các Module](./Modules_Documentation.md)** - Mô tả chi tiết các module chính
- **[Tài Liệu Database](./Database_Documentation.md)** - Schema, migrations, và cấu trúc database
- **[Tiến Độ Dự Án](../VnsErp2025/Docs/ProjectProgress_20250115.md)** - Báo cáo tiến độ phát triển dự án
- **[Mô Tả Solution](../VnsErp2025/Docs/SolutionDescription.md)** - Mô tả solution và cấu trúc projects

---

## 🏗️ Tài Liệu Kiến Trúc & Cấu Trúc

### 2.1 DAL Layer (Data Access Layer)
- **[Cấu Trúc DAL Layer](../Dal/Doc/DalFolderStructure.md)** - Cấu trúc thư mục và tổ chức code DAL
- **[DataContext Documentation](../Dal/DataContext/VnsErp2025.md)** - Tài liệu về LINQ to SQL DataContext
- **[Connection Management](../Dal/Connection/README.md)** - Quản lý kết nối database

### 2.2 BLL Layer (Business Logic Layer)
- **[Cấu Trúc BLL Layer](../Bll/Doc/BllFolderStructure.md)** - Cấu trúc thư mục và tổ chức code BLL
- **[Permission System](../Bll/Common/PermissionSystem_README.md)** - Hệ thống phân quyền

### 2.3 Common & Utilities
- **[Validation System](../Common/Validation/README.md)** - Hệ thống validation
- **[Image Storage](../Bll/Common/ImageStorage/README.md)** - Quản lý lưu trữ hình ảnh
- **[Image Service](../Bll/Common/ImageService/README.md)** - Service xử lý hình ảnh
- **[Logger](../Logger/README.md)** - Hệ thống logging

---

## 📦 Tài Liệu Module

### 3.1 Authentication (Xác Thực)
- **[FrmLogin - User Guide](../Authentication/Form/FrmLogin_User_Guide.md)** - Hướng dẫn sử dụng form đăng nhập
- **[FrmLogin - Developer Guide](../Authentication/Form/FrmLogin_Developer_Guide.md)** - Hướng dẫn phát triển form đăng nhập
- **[FrmDatabaseConfig - User Guide](../Authentication/Form/FrmDatabaseConfig_User_Guide.md)** - Hướng dẫn cấu hình database
- **[FrmDatabaseConfig - Developer Guide](../Authentication/Form/FrmDatabaseConfig_Developer_Guide.md)** - Hướng dẫn phát triển form cấu hình

### 3.2 VersionAndUserManagement (Quản Lý Phiên Bản & Người Dùng)

#### ApplicationVersion
- Tài liệu quản lý phiên bản ứng dụng

#### UserManagement
- Tài liệu quản lý người dùng

#### RoleManagement
- **[FrmRoleManagement](../VersionAndUserManagement/RoleManagement/)** - Form quản lý vai trò

#### PermissionManagement
- **[UI Design Proposal](../VersionAndUserManagement/PermissionManagement/UI_Design_Proposal.md)** - Đề xuất thiết kế UI cho hệ thống quản lý quyền
- **[UI Implementation Guide](../VersionAndUserManagement/PermissionManagement/UI_Implementation_Guide.md)** - Hướng dẫn triển khai UI
- **[Entity Permission Matrix](../Dal/Doc/EntityPermissionMatrix.md)** - Ma trận quyền theo entity
- **[Entity Permission Management Proposal](../Dal/Doc/EntityPermissionManagement_Proposal.md)** - Đề xuất quản lý quyền theo entity

### 3.3 MasterData (Dữ Liệu Master)

#### Company
- **[FrmCompany - User Guide](../MasterData/Company/FrmCompany_User_Guide.md)** - Hướng dẫn sử dụng quản lý công ty
- **[FrmCompany - Developer Guide](../MasterData/Company/FrmCompany_Developer_Guide.md)** - Hướng dẫn phát triển
- **[FrmCompanyBranch - User Guide](../MasterData/Company/FrmCompanyBranch_User_Guide.md)** - Hướng dẫn quản lý chi nhánh
- **[FrmCompanyBranch - Developer Guide](../MasterData/Company/FrmCompanyBranch_Developer_Guide.md)** - Hướng dẫn phát triển
- **[FrmDepartmentDetail - User Guide](../MasterData/Company/FrmDepartmentDetail_User_Guide.md)** - Hướng dẫn quản lý phòng ban
- **[FrmDepartmentDetail - Developer Guide](../MasterData/Company/FrmDepartmentDetail_Developer_Guide.md)** - Hướng dẫn phát triển
- **[DepartmentTreeList Implementation](../MasterData/Company/DepartmentTreeList_Implementation.md)** - Triển khai TreeList phòng ban
- **[UcCompany - User Guide](../../UcCompany_User_Guide.md)** - User control công ty
- **[UcCompany - Developer Guide](../../UcCompany_Developer_Guide.md)** - Hướng dẫn phát triển
- **[UcCompanyBranch - User Guide](../../UcCompanyBranch_User_Guide.md)** - User control chi nhánh
- **[UcCompanyBranch - Developer Guide](../../UcCompanyBranch_Developer_Guide.md)** - Hướng dẫn phát triển

#### BusinessPartner
- **[BusinessPartner Database Schema](../MasterData/Doc/BusinessPartner_Database_Schema.md)** - Schema database đối tác kinh doanh
- **[FrmBusinessPartnerCategory - User Guide](../MasterData/Company/FrmBusinessPartnerCategory_User_Guide.md)** - Hướng dẫn danh mục đối tác
- **[FrmBusinessPartnerCategory - Developer Guide](../MasterData/Company/FrmBusinessPartnerCategory_Developer_Guide.md)** - Hướng dẫn phát triển

#### ProductService
- **[ProductService Implementation Guide](../MasterData/ProductService/IMPLEMENTATION_GUIDE.md)** - Hướng dẫn triển khai sản phẩm/dịch vụ

### 3.4 Inventory (Quản Lý Kho)

#### StockIn (Nhập Kho)
- **[Nhập Hàng Thương Mại - Tổng Quan](../Inventory/Doc/NhapHang/NhapHangThuongMai_00_TongQuan.md)** - Tổng quan nhập hàng thương mại
- **[Nhập Kho Đối Bán](../Inventory/Doc/NhapHang/NhapHangThuongMai_01_NhapKhoDoiBan.md)** - Quy trình nhập kho đối bán
- **[Nhập Kho Theo PO Nhà Cung Cấp](../Inventory/Doc/NhapHang/NhapHangThuongMai_02_NhapKhoTheoPONCC.md)** - Quy trình nhập kho theo PO NCC
- **[Nhập Kho Theo PO Khách Hàng](../Inventory/Doc/NhapHang/NhapHangThuongMai_03_NhapKhoTheoPOCustomer.md)** - Quy trình nhập kho theo PO KH
- **[In Phiếu Nhập Kho](../Inventory/StockIn/InPhieu/README_IN_PHIEU_NHAP_KHO.md)** - Hướng dẫn in phiếu nhập kho

#### StockOut (Xuất Kho)
- Tài liệu các loại xuất kho

#### Management & Query
- **[Stock In History Query](../DTO/Inventory/StockIn/README_STOCK_IN_HISTORY_QUERY.md)** - Query lịch sử nhập kho
- **[Database Schema - StockInOutDocument](../Inventory/Doc/DatabaseSchema_StockInOutDocument_Proposal.md)** - Schema phiếu nhập/xuất kho
- **[Database Schema - InventoryBalance](../Docs/DatabaseSchema_InventoryBalance_Proposal.md)** - Schema tồn kho

### 3.5 AssemblyManufacturing (Lắp Ráp & Sản Xuất)
- **[Assembly Manufacturing Plan](../Docs/AssemblyManufacturing_Plan.md)** - Kế hoạch phát triển
- **[Assembly Manufacturing UI Workflow](../Docs/AssemblyManufacturing_UI_Workflow.md)** - Quy trình UI
- **[Assembly Manufacturing Business Logic](../Docs/AssemblyManufacturing_BusinessLogic.md)** - Logic nghiệp vụ
- **[Assembly Manufacturing Disassembly Logic](../Docs/AssemblyManufacturing_Disassembly_Logic.md)** - Logic tháo rời
- **[Assembly Manufacturing Implementation Steps](../Docs/AssemblyManufacturing_Implementation_Steps.md)** - Các bước triển khai

---

## 🗄️ Tài Liệu Database

### 4.1 Database Schema
- **[BusinessPartner Schema](../MasterData/Doc/BusinessPartner_Database_Schema.md)** - Schema đối tác kinh doanh
- **[StockInOutDocument Schema](../Inventory/Doc/DatabaseSchema_StockInOutDocument_Proposal.md)** - Schema phiếu nhập/xuất kho
- **[InventoryBalance Schema](../Docs/DatabaseSchema_InventoryBalance_Proposal.md)** - Schema tồn kho
- **[Asset Schema](../VnsErp2025/Docs/DatabaseSchema_Asset_Proposal.md)** - Schema tài sản

### 4.2 Database Migrations
- **[ProductImage Refactor](../Database/Migrations/README_ProductImage_Refactor.md)** - Refactor hình ảnh sản phẩm
- **[BusinessPartner Contact Avatar Migration](../Database/Migrations/README_BusinessPartnerContact_Avatar_Migration.md)** - Migration avatar liên hệ
- **[BusinessPartner Remove Contact Bank Fields](../Database/Migrations/README_BusinessPartner_RemoveContactBankFields.md)** - Xóa trường ngân hàng
- **[BusinessPartner Migration](../Database/Migrations/README_BusinessPartner_Migration.md)** - Migration đối tác
- **[BusinessPartner Logo Migration](../Database/Migrations/README_BusinessPartner_Logo_Migration.md)** - Migration logo
- **[BusinessPartner Logo Thumbnail Migration](../Database/Migrations/README_BusinessPartner_LogoThumbnail_Migration.md)** - Migration logo thumbnail

### 4.3 Seed Data
- **[Company Seed Data](../Dal/DataContext/SeedData/MasterData/Company/README_SeedData.md)** - Dữ liệu mẫu công ty
- **[Customer Seed Data](../Dal/DataContext/SeedData/MasterData/Customer/README_SeedData.md)** - Dữ liệu mẫu khách hàng

---

## 🔧 Tài Liệu Kỹ Thuật

### 5.1 Configuration & Setup
- **[Image Storage Configuration](../Docs/ImageStorageConfigurationGuide.md)** - Cấu hình lưu trữ hình ảnh
- **[Icon Setup Guide](../Docs/IconSetupGuide.md)** - Hướng dẫn thiết lập icon
- **[Version Management Strategy](../Docs/VersionManagementStrategy.md)** - Chiến lược quản lý phiên bản

### 5.2 Refactoring & Architecture
- **[Image Storage Refactoring Architecture](../Docs/ImageStorageRefactoringArchitecture.md)** - Kiến trúc refactor lưu trữ hình ảnh
- **[File Storage Service Refactoring](../Docs/FileStorageService_Refactoring.md)** - Refactor service lưu trữ file

### 5.3 Build & Deployment
- **[Build Error Fix](../BUILD_ERROR_FIX.md)** - Sửa lỗi build
- **[Build Access Denied Fix](../BUILD_ACCESS_DENIED_FIX.md)** - Sửa lỗi quyền truy cập build
- **[Build Error MSB3021 Guide](../BUILD_ERROR_MSB3021_GUIDE.md)** - Hướng dẫn lỗi MSB3021

---

## 📖 Hướng Dẫn Sử Dụng

### 6.1 User Guides (Hướng Dẫn Người Dùng)
- Xem các tài liệu User Guide trong từng module ở trên

### 6.2 Developer Guides (Hướng Dẫn Phát Triển)
- Xem các tài liệu Developer Guide trong từng module ở trên

---

## 🔍 Tìm Kiếm Tài Liệu

### Theo Module
- **Authentication:** Xem mục 3.1
- **VersionAndUserManagement:** Xem mục 3.2
- **MasterData:** Xem mục 3.3
- **Inventory:** Xem mục 3.4
- **AssemblyManufacturing:** Xem mục 3.5

### Theo Loại Tài Liệu
- **Tổng quan:** Mục 1
- **Kiến trúc:** Mục 2
- **Module:** Mục 3
- **Database:** Mục 4
- **Kỹ thuật:** Mục 5
- **Hướng dẫn:** Mục 6

---

## 📝 Ghi Chú

- Tài liệu được cập nhật thường xuyên theo tiến độ phát triển
- Các tài liệu có thể chưa hoàn chỉnh, sẽ được bổ sung dần
- Nếu có thắc mắc, vui lòng liên hệ Development Team

---

## 🔄 Cập Nhật

**Lần cập nhật gần nhất:** 27/01/2025  
**Phiên bản:** 1.0  
**Người cập nhật:** Development Team

---

**Người tạo:** Development Team  
**Ngày tạo:** 27/01/2025  
**Trạng thái:** Đang phát triển








