# Tóm tắt các Entity Company trong VnsErp2025

## Tổng quan
Tài liệu này mô tả các entity liên quan đến quản lý công ty, chi nhánh, phòng ban, chức vụ và nhân viên trong hệ thống VnsErp2025.

## 1. Company (Công ty chính)

### Mô tả
Entity chính quản lý thông tin công ty, là root entity cho toàn bộ hệ thống quản lý nhân sự.

### Thuộc tính chính
- **Id**: UniqueIdentifier (Primary Key)
- **CompanyCode**: NVARCHAR(50) - Mã công ty (Unique)
- **CompanyName**: NVARCHAR(255) - Tên công ty
- **TaxCode**: NVARCHAR(50) - Mã số thuế
- **Phone**: NVARCHAR(50) - Số điện thoại
- **Email**: NVARCHAR(100) - Email liên hệ
- **Website**: NVARCHAR(100) - Website công ty
- **Address**: NVARCHAR(255) - Địa chỉ
- **Country**: NVARCHAR(100) - Quốc gia
- **LogoPath**: NVARCHAR(500) - Đường dẫn logo
- **IsActive**: BIT - Trạng thái hoạt động
- **CreatedDate**: DATETIME - Ngày tạo
- **UpdatedDate**: DATETIME - Ngày cập nhật

### Quan hệ
- **1:N** với CompanyBranch
- **1:N** với Department
- **1:N** với Employee
- **1:N** với Position

---

## 2. CompanyBranch (Chi nhánh)

### Mô tả
Quản lý thông tin các chi nhánh thuộc công ty.

### Thuộc tính chính
- **Id**: UniqueIdentifier (Primary Key)
- **CompanyId**: UniqueIdentifier (Foreign Key → Company.Id)
- **BranchCode**: NVARCHAR(50) - Mã chi nhánh
- **BranchName**: NVARCHAR(255) - Tên chi nhánh
- **Address**: NVARCHAR(255) - Địa chỉ chi nhánh
- **Phone**: NVARCHAR(50) - Số điện thoại
- **Email**: NVARCHAR(100) - Email chi nhánh
- **ManagerName**: NVARCHAR(100) - Tên quản lý
- **IsActive**: BIT - Trạng thái hoạt động

### Quan hệ
- **N:1** với Company
- **1:N** với Department
- **1:N** với Employee

### Ràng buộc
- Unique constraint: (CompanyId, BranchCode)

---

## 3. Department (Phòng ban)

### Mô tả
Quản lý cấu trúc phòng ban trong công ty, hỗ trợ cấu trúc phân cấp.

### Thuộc tính chính
- **Id**: UniqueIdentifier (Primary Key)
- **CompanyId**: UniqueIdentifier (Foreign Key → Company.Id)
- **BranchId**: UniqueIdentifier (Foreign Key → CompanyBranch.Id, nullable)
- **DepartmentCode**: NVARCHAR(50) - Mã phòng ban
- **DepartmentName**: NVARCHAR(255) - Tên phòng ban
- **ParentId**: UniqueIdentifier (Foreign Key → Department.Id, nullable) - Phòng ban cha
- **Description**: NVARCHAR(255) - Mô tả
- **IsActive**: BIT - Trạng thái hoạt động

### Quan hệ
- **N:1** với Company
- **N:1** với CompanyBranch (optional)
- **1:N** với Department (self-reference - cấu trúc phân cấp)
- **1:N** với Employee

### Ràng buộc
- Unique constraint: (CompanyId, DepartmentCode)

---

## 4. Position (Chức vụ)

### Mô tả
Danh mục các chức vụ trong công ty.

### Thuộc tính chính
- **Id**: UniqueIdentifier (Primary Key)
- **CompanyId**: UniqueIdentifier (Foreign Key → Company.Id)
- **PositionCode**: NVARCHAR(50) - Mã chức vụ
- **PositionName**: NVARCHAR(255) - Tên chức vụ
- **Description**: NVARCHAR(255) - Mô tả chức vụ
- **IsManagerLevel**: BIT (nullable) - Cấp quản lý
- **IsActive**: BIT - Trạng thái hoạt động

### Quan hệ
- **N:1** với Company
- **1:N** với Employee

### Ràng buộc
- Unique constraint: (CompanyId, PositionCode)

---

## 5. Employee (Nhân viên)

### Mô tả
Quản lý thông tin nhân viên trong công ty.

### Thuộc tính chính
- **Id**: UniqueIdentifier (Primary Key)
- **CompanyId**: UniqueIdentifier (Foreign Key → Company.Id)
- **BranchId**: UniqueIdentifier (Foreign Key → CompanyBranch.Id, nullable)
- **DepartmentId**: UniqueIdentifier (Foreign Key → Department.Id, nullable)
- **PositionId**: UniqueIdentifier (Foreign Key → Position.Id, nullable)
- **EmployeeCode**: NVARCHAR(50) - Mã nhân viên
- **FullName**: NVARCHAR(100) - Họ tên đầy đủ
- **Gender**: NVARCHAR(10) - Giới tính
- **BirthDate**: DATE - Ngày sinh
- **Phone**: NVARCHAR(50) - Số điện thoại
- **Email**: NVARCHAR(100) - Email
- **HireDate**: DATE - Ngày vào làm
- **ResignDate**: DATE - Ngày nghỉ việc
- **AvatarPath**: NVARCHAR(500) - Đường dẫn ảnh đại diện
- **IsActive**: BIT - Trạng thái hoạt động

### Quan hệ
- **N:1** với Company
- **N:1** với CompanyBranch (optional)
- **N:1** với Department (optional)
- **N:1** với Position (optional)

### Ràng buộc
- Unique constraint: (CompanyId, EmployeeCode)

---

## Sơ đồ quan hệ

```
Company (1) ──→ (N) CompanyBranch
  │
  ├── (N) Department ──→ (1) Department (self-reference)
  │
  ├── (N) Position
  │
  └── (N) Employee ──→ (1) CompanyBranch
                      └── (1) Department
                      └── (1) Position
```

## Đặc điểm thiết kế

### 1. Cấu trúc phân cấp
- **Company** là root entity
- **Department** hỗ trợ cấu trúc phân cấp thông qua ParentId
- **Employee** có thể thuộc về Company, Branch, Department và Position

### 2. Ràng buộc dữ liệu
- Tất cả các entity đều có IsActive để quản lý trạng thái
- Unique constraints đảm bảo tính duy nhất của mã trong phạm vi công ty
- Foreign key constraints đảm bảo tính toàn vẹn dữ liệu

### 3. Quan hệ linh hoạt
- Employee có thể không thuộc Branch/Department/Position (nullable)
- Department có thể không thuộc Branch (nullable)
- Hỗ trợ cấu trúc tổ chức phức tạp

### 4. Audit trail
- Company có CreatedDate và UpdatedDate
- Các entity khác có thể mở rộng thêm audit fields nếu cần

## Sử dụng trong ứng dụng

### 1. Quản lý tổ chức
- Thiết lập cấu trúc công ty, chi nhánh, phòng ban
- Quản lý chức vụ và vị trí công việc

### 2. Quản lý nhân sự
- Thông tin nhân viên đầy đủ
- Liên kết với cấu trúc tổ chức
- Theo dõi lịch sử làm việc

### 3. Báo cáo và phân tích
- Báo cáo theo cấu trúc tổ chức
- Thống kê nhân sự theo phòng ban, chức vụ
- Phân tích cơ cấu tổ chức
