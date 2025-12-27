# VNS ERP 2025 - Tổng Quan Hệ Thống

**Phiên bản:** 1.0  
**Ngày cập nhật:** 27/01/2025  
**Trạng thái:** Đang phát triển

---

## 1. Giới Thiệu

**VNS ERP 2025** là hệ thống quản lý doanh nghiệp (Enterprise Resource Planning) được phát triển để quản lý toàn diện các hoạt động kinh doanh của doanh nghiệp. Hệ thống được xây dựng trên nền tảng .NET Framework với kiến trúc 3 lớp (3-Layer Architecture), sử dụng công nghệ Windows Forms và DevExpress Controls.

### 1.1 Mục Đích
- Quản lý dữ liệu master (khách hàng, nhà cung cấp, sản phẩm, nhân viên)
- Quản lý kho và tồn kho
- Quản lý nhập/xuất kho
- Quản lý đơn hàng và giao dịch
- Quản lý tài chính
- Quản lý người dùng và phân quyền
- Báo cáo và thống kê

### 1.2 Đối Tượng Sử Dụng
- **Quản trị viên hệ thống:** Quản lý người dùng, phân quyền, cấu hình hệ thống
- **Quản lý:** Xem báo cáo, quản lý dữ liệu master, phê duyệt giao dịch
- **Nhân viên:** Nhập liệu, xử lý đơn hàng, quản lý kho
- **Kế toán:** Quản lý tài chính, báo cáo tài chính

---

## 2. Kiến Trúc Hệ Thống

### 2.1 Mô Hình 3 Lớp (3-Layer Architecture)

```
┌─────────────────────────────────────┐
│         GUI Layer                   │
│    (Presentation Layer)             │
│         VnsErp2025                 │
│    Windows Forms + DevExpress       │
└─────────────────┬───────────────────┘
                  │
┌─────────────────┴───────────────────┐
│         BLL Layer                    │
│    (Business Logic Layer)            │
│              Bll                     │
│    Business Objects + Services        │
└─────────────────┬───────────────────┘
                  │
┌─────────────────┴───────────────────┐
│         DAL Layer                    │
│    (Data Access Layer)               │
│              Dal                     │
│    LINQ to SQL + Data Access         │
└─────────────────┬───────────────────┘
                  │
┌─────────────────┴───────────────────┐
│      Database Layer                 │
│    Microsoft SQL Server             │
└─────────────────────────────────────┘
```

### 2.2 Mô Tả Các Lớp

#### **GUI Layer (VnsErp2025)**
- **Mục đích:** Giao diện người dùng, xử lý tương tác với người dùng
- **Công nghệ:** Windows Forms với DevExpress Controls v25.1
- **Chức năng:**
  - Hiển thị dữ liệu cho người dùng
  - Thu thập input từ người dùng
  - Xử lý các sự kiện UI
  - Validation dữ liệu ở tầng presentation
- **Framework:** .NET Framework 4.8

#### **BLL Layer (Bll)**
- **Mục đích:** Xử lý logic nghiệp vụ, quy tắc kinh doanh
- **Chức năng:**
  - Xử lý logic nghiệp vụ phức tạp
  - Validation dữ liệu nghiệp vụ
  - Quản lý workflow và business rules
  - Chuyển đổi dữ liệu giữa DAL và GUI
- **Loại project:** Class Library (.dll)

#### **DAL Layer (Dal)**
- **Mục đích:** Truy cập và thao tác với cơ sở dữ liệu
- **Công nghệ:** LINQ to SQL (Drag & Drop Approach)
- **Chức năng:**
  - Kết nối cơ sở dữ liệu
  - Thực hiện các thao tác CRUD (Create, Read, Update, Delete)
  - Quản lý connection string
  - Xử lý stored procedures và SQL queries
- **Loại project:** Class Library (.dll)

---

## 3. Các Module Chính

### 3.1 Authentication (Xác Thực)
- **Mục đích:** Quản lý đăng nhập, xác thực người dùng
- **Chức năng:**
  - Đăng nhập/đăng xuất
  - Quản lý session
  - Cấu hình kết nối database
  - Quản lý MAC address được phép

### 3.2 VersionAndUserManagement (Quản Lý Phiên Bản & Người Dùng)
- **Mục đích:** Quản lý phiên bản ứng dụng, người dùng, vai trò và quyền
- **Các module con:**
  - **ApplicationVersion:** Quản lý phiên bản ứng dụng
  - **UserManagement:** Quản lý người dùng
  - **RoleManagement:** Quản lý vai trò
  - **PermissionManagement:** Quản lý quyền truy cập

### 3.3 MasterData (Dữ Liệu Master)
- **Mục đích:** Quản lý các dữ liệu cơ bản của hệ thống
- **Các module con:**
  - **Company:** Quản lý công ty, chi nhánh, phòng ban
  - **BusinessPartner:** Quản lý đối tác kinh doanh (khách hàng, nhà cung cấp)
  - **ProductService:** Quản lý sản phẩm/dịch vụ, danh mục, biến thể
  - **Customer:** Quản lý khách hàng

### 3.4 Inventory (Quản Lý Kho)
- **Mục đích:** Quản lý kho, nhập/xuất kho, tồn kho
- **Các module con:**
  - **StockIn:** Quản lý nhập kho
    - Nhập kho đối bán
    - Nhập kho theo PO nhà cung cấp
    - Nhập kho theo PO khách hàng
  - **StockOut:** Quản lý xuất kho
    - Xuất kho thương mại
    - Xuất bảo hành
    - Xuất lắp ráp
    - Xuất lưu chuyển kho
    - Xuất nội bộ
    - Xuất cho thuê mượn
  - **Management:** Quản lý tồn kho, tài sản
  - **Query:** Tra cứu lịch sử, kiểm tra bảo hành

### 3.5 AssemblyManufacturing (Lắp Ráp & Sản Xuất)
- **Mục đích:** Quản lý quy trình lắp ráp và sản xuất
- **Chức năng:**
  - Quản lý quy trình lắp ráp
  - Quản lý tháo rời
  - Quản lý sản xuất

---

## 4. Công Nghệ Sử Dụng

### 4.1 Framework và Runtime
- **.NET Framework:** 4.8
- **Platform Target:** Any CPU
- **Language:** C#
- **IDE:** Visual Studio 2022 Enterprise

### 4.2 UI Framework
- **DevExpress:** Version 25.1
  - DevExpress.XtraEditors.v25.1
  - DevExpress.Data.v25.1
  - DevExpress.Utils.v25.1
  - DevExpress.BonusSkins.v25.1
- **Windows Forms:** .NET Framework built-in

### 4.3 Cơ Sở Dữ Liệu
- **Database Engine:** Microsoft SQL Server
- **ORM:** LINQ to SQL (Drag & Drop Approach)
- **Connection:** ADO.NET với connection pooling

### 4.4 Development Tools
- **Version Control:** Azure DevOps TFS / Git
- **Code Quality:** ReSharper
- **Documentation:** Markdown files

---

## 5. Cấu Trúc Solution

### 5.1 Danh Sách Projects

| Tên Project | Loại | Mô Tả |
|-------------|------|-------|
| `VnsErp2025` | Windows Application | GUI Layer - Ứng dụng Windows Forms chính |
| `Bll` | Class Library | Business Logic Layer - Xử lý logic nghiệp vụ |
| `Dal` | Class Library | Data Access Layer - Truy cập cơ sở dữ liệu |
| `Authentication` | Class Library | Module xác thực |
| `MasterData` | Class Library | Module dữ liệu master |
| `Inventory` | Class Library | Module quản lý kho |
| `VersionAndUserManagement` | Class Library | Module quản lý phiên bản và người dùng |
| `Common` | Class Library | Các class dùng chung |
| `DTO` | Class Library | Data Transfer Objects |
| `Logger` | Class Library | Module logging |

### 5.2 Dependencies

```
VnsErp2025 (GUI)
    ├── Bll (Business Logic)
    │   └── Dal (Data Access)
    ├── Authentication
    ├── MasterData
    ├── Inventory
    ├── VersionAndUserManagement
    ├── Common
    ├── DTO
    └── DevExpress Components
        ├── DevExpress.XtraEditors.v25.1
        ├── DevExpress.Data.v25.1
        ├── DevExpress.Utils.v25.1
        └── DevExpress.BonusSkins.v25.1
```

---

## 6. Quy Ước Coding

### 6.1 Naming Convention
- **Class Names:** PascalCase (tiếng Việt không dấu)
  - Ví dụ: `NhanVienBO`, `KhachHangService`
- **Method Names:** PascalCase
  - Ví dụ: `LayTatCa`, `Them`, `CapNhat`, `Xoa`
- **Property Names:** PascalCase (tiếng Việt không dấu)
  - Ví dụ: `TenNhanVien`, `SoDienThoai`
- **Variable Names:** camelCase
  - Ví dụ: `nhanVien`, `danhSachKhachHang`
- **Database Columns:** snake_case (tiếng Việt không dấu)
  - Ví dụ: `ten_nhan_vien`, `so_dien_thoai`

### 6.2 Code Organization
- **Regions:** Sử dụng regions bằng tiếng Việt không dấu
  - `#region thuocTinhDonGian`
  - `#region thuocTinhQuanHe`
  - `#region danhSachLienKet`
  - `#region phuongThuc`
  - `#region enum`
- **Comments:** Tiếng Việt để dễ quản lý
- **XML Documentation:** Cho tất cả public methods và properties

---

## 7. Tính Năng Chính

### 7.1 Quản Lý Dữ Liệu Master
- Quản lý công ty, chi nhánh, phòng ban
- Quản lý đối tác kinh doanh (khách hàng, nhà cung cấp)
- Quản lý sản phẩm/dịch vụ, danh mục, biến thể
- Quản lý nhân viên

### 7.2 Quản Lý Kho
- Nhập kho (đối bán, theo PO, v.v.)
- Xuất kho (thương mại, bảo hành, lắp ráp, v.v.)
- Quản lý tồn kho
- Lưu chuyển kho
- Kiểm kê kho

### 7.3 Quản Lý Đơn Hàng
- Tạo và quản lý đơn hàng
- Xử lý đơn hàng
- Theo dõi trạng thái đơn hàng

### 7.4 Quản Lý Tài Chính
- Quản lý phiếu thu
- Quản lý phiếu chi
- Báo cáo tài chính

### 7.5 Quản Lý Người Dùng & Phân Quyền
- Quản lý người dùng
- Quản lý vai trò (Role)
- Quản lý quyền (Permission)
- Gán quyền cho người dùng và vai trò

### 7.6 Báo Cáo & Thống Kê
- Báo cáo tồn kho
- Báo cáo nhập/xuất kho
- Báo cáo tài chính
- Báo cáo doanh thu

---

## 8. Bảo Mật

### 8.1 Xác Thực
- Đăng nhập với username/password
- Quản lý session
- Kiểm tra MAC address (tùy chọn)

### 8.2 Phân Quyền
- Role-based access control (RBAC)
- Permission-based access control
- Override permission cho từng user
- Kiểm tra quyền ở mọi tầng (GUI, BLL, DAL)

### 8.3 Bảo Mật Dữ Liệu
- Mã hóa password
- Mã hóa connection string
- SQL injection prevention
- Input validation

---

## 9. Hiệu Năng

### 9.1 Tối Ưu Database
- Connection pooling
- Indexed queries
- Query caching
- Stored procedures cho các query phức tạp

### 9.2 Tối Ưu Application
- Lazy loading
- Pagination
- Background processing
- Caching layer

---

## 10. Deployment

### 10.1 Yêu Cầu Hệ Thống
- **OS:** Windows 10/11 hoặc Windows Server 2016+
- **.NET Framework:** 4.8
- **SQL Server:** SQL Server 2016+ hoặc SQL Server Express
- **RAM:** Tối thiểu 4GB (khuyến nghị 8GB+)
- **Disk:** Tối thiểu 10GB dung lượng trống

### 10.2 Cài Đặt
1. Cài đặt .NET Framework 4.8
2. Cài đặt SQL Server
3. Tạo database và chạy migration scripts
4. Cấu hình connection string
5. Cài đặt ứng dụng
6. Cấu hình DevExpress license (nếu cần)

---

## 11. Tài Liệu Liên Quan

- [Kiến Trúc Hệ Thống](./System_Architecture.md)
- [Tài Liệu Module](./Modules/)
- [Tài Liệu Database](./Database/)
- [Hướng Dẫn Phát Triển](./Development_Guide.md)
- [Hướng Dẫn Sử Dụng](./User_Guide.md)

---

## 12. Lịch Sử Phiên Bản

### Version 1.0 (27/01/2025)
- Tài liệu tổng quan hệ thống
- Kiến trúc 3 lớp
- Các module cơ bản
- Quản lý người dùng và phân quyền

---

**Người tạo:** Development Team  
**Ngày tạo:** 27/01/2025  
**Trạng thái:** Đang phát triển








