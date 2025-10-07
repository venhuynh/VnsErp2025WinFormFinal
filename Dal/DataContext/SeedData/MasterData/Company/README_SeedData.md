# Company Seed Data Documentation

## Tổng quan
Tài liệu này mô tả seed data cho các Company entities trong hệ thống VnsErp2025.

## Cấu trúc thư mục
```
Dal/DataContext/SeedData/MasterData/Company/
├── SeedData_Master_Company.cs          # Class chính quản lý seed data
├── 00.Seed_Company_DeleteData.sql      # Xóa tất cả dữ liệu
├── 01.Seed_Company_TestData.sql        # Tạo dữ liệu Companies
├── 02.Seed_CompanyBranch_TestData.sql  # Tạo dữ liệu CompanyBranches
├── 03.Seed_Position_TestData.sql       # Tạo dữ liệu Positions
├── 04.Seed_Department_TestData.sql     # Tạo dữ liệu Departments
├── 05.Seed_Employee_TestData.sql       # Tạo dữ liệu Employees
├── 99.Check_Company_Data.sql           # Kiểm tra dữ liệu
└── README_SeedData.md                  # Tài liệu này
```

## Entities được quản lý

### 1. Company (Công ty)
- **Mục đích**: Quản lý thông tin công ty chính
- **Số lượng**: 2 công ty mẫu
- **Dữ liệu mẫu**:
  - VNS Technology (VNS001)
  - VNS Solutions (VNS002)

### 2. CompanyBranch (Chi nhánh)
- **Mục đích**: Quản lý các chi nhánh thuộc công ty
- **Số lượng**: 3 chi nhánh mẫu
- **Dữ liệu mẫu**:
  - VNS Technology - Hà Nội
  - VNS Technology - Đà Nẵng
  - VNS Solutions - Hà Nội

### 3. Position (Chức vụ)
- **Mục đích**: Quản lý các chức vụ trong công ty
- **Số lượng**: 6 chức vụ mẫu
- **Dữ liệu mẫu**:
  - CEO, CTO, DEV, HR (VNS Technology)
  - CEO, SALES (VNS Solutions)

### 4. Department (Phòng ban)
- **Mục đích**: Quản lý cấu trúc phòng ban với hierarchy
- **Số lượng**: 8 phòng ban mẫu
- **Cấu trúc phân cấp**:
  ```
  VNS Technology:
  ├── Ban Giám Đốc (CEO)
  ├── Phòng Công Nghệ Thông Tin (IT)
  │   ├── Bộ phận Phát Triển (IT-DEV)
  │   └── Bộ phận Hỗ Trợ (IT-SUPPORT)
  └── Phòng Nhân Sự (HR)
      └── Bộ phận Tuyển Dụng (HR-REC)
  
  VNS Solutions:
  ├── Ban Giám Đốc (CEO)
  └── Phòng Kinh Doanh (SALES)
  ```

### 5. Employee (Nhân viên)
- **Mục đích**: Quản lý thông tin nhân viên
- **Số lượng**: 6 nhân viên mẫu
- **Dữ liệu mẫu**:
  - Nguyễn Văn Giám Đốc (CEO)
  - Trần Thị Công Nghệ (CTO)
  - Lê Văn Lập Trình (DEV)
  - Phạm Thị Nhân Sự (HR)
  - Hoàng Văn Giám Đốc (CEO - VNS Solutions)
  - Võ Thị Kinh Doanh (SALES)

## Cách sử dụng

### 1. Từ C# Code
```csharp
// Tạo tất cả Company seed data
SeedData_Master_Company.CreateAllCompanyData();

// Xóa tất cả Company seed data
SeedData_Master_Company.DeleteAllCompanyData();

// Tạo tất cả seed data (bao gồm Company)
SeedData_All.CreateAllData();

// Xóa tất cả seed data (bao gồm Company)
SeedData_All.DeleteAllData();
```

### 2. Từ SQL Scripts
```sql
-- Chạy theo thứ tự:
-- 1. Xóa dữ liệu cũ
EXEC sp_executesql N'-- Nội dung file 00.Seed_Company_DeleteData.sql'

-- 2. Tạo dữ liệu mới
EXEC sp_executesql N'-- Nội dung file 01.Seed_Company_TestData.sql'
EXEC sp_executesql N'-- Nội dung file 02.Seed_CompanyBranch_TestData.sql'
EXEC sp_executesql N'-- Nội dung file 03.Seed_Position_TestData.sql'
EXEC sp_executesql N'-- Nội dung file 04.Seed_Department_TestData.sql'
EXEC sp_executesql N'-- Nội dung file 05.Seed_Employee_TestData.sql'

-- 3. Kiểm tra dữ liệu
EXEC sp_executesql N'-- Nội dung file 99.Check_Company_Data.sql'
```

## Thứ tự tạo dữ liệu

### Tạo dữ liệu (Create)
1. **Companies** (root entity)
2. **CompanyBranches** (cần CompanyId)
3. **Positions** (cần CompanyId)
4. **Departments** (cần CompanyId, có thể có ParentId)
5. **Employees** (cần CompanyId, DepartmentId, PositionId)

### Xóa dữ liệu (Delete)
1. **Employees** (có foreign key đến các bảng khác)
2. **Departments** (có foreign key đến Company và self-reference)
3. **Positions** (có foreign key đến Company)
4. **CompanyBranches** (có foreign key đến Company)
5. **Companies** (root entity)

## Ràng buộc dữ liệu

### Foreign Key Constraints
- `CompanyBranches.CompanyId` → `Companies.Id`
- `Positions.CompanyId` → `Companies.Id`
- `Departments.CompanyId` → `Companies.Id`
- `Departments.ParentId` → `Departments.Id` (self-reference)
- `Employees.CompanyId` → `Companies.Id`
- `Employees.DepartmentId` → `Departments.Id`
- `Employees.PositionId` → `Positions.Id`

### Unique Constraints
- `Companies.CompanyCode` (unique)
- `CompanyBranches.(CompanyId, BranchCode)` (unique)
- `Positions.(CompanyId, PositionCode)` (unique)
- `Departments.(CompanyId, DepartmentCode)` (unique)
- `Employees.(CompanyId, EmployeeCode)` (unique)

## Dữ liệu mẫu chi tiết

### Companies
| CompanyCode | CompanyName | TaxCode | Phone | Email |
|-------------|-------------|---------|-------|-------|
| VNS001 | Công ty TNHH VNS Technology | 0123456789 | 028-1234-5678 | info@vnstech.com |
| VNS002 | Công ty TNHH VNS Solutions | 0987654321 | 028-8765-4321 | contact@vnssolutions.com |

### CompanyBranches
| BranchCode | BranchName | Company | ManagerName |
|------------|------------|---------|--------------|
| VNS001-HN | Chi nhánh Hà Nội | VNS Technology | Nguyễn Văn A |
| VNS001-DN | Chi nhánh Đà Nẵng | VNS Technology | Trần Thị B |
| VNS002-HN | Chi nhánh Hà Nội | VNS Solutions | Lê Văn C |

### Positions
| PositionCode | PositionName | Company | IsManagerLevel |
|--------------|--------------|---------|----------------|
| CEO | Tổng Giám Đốc | VNS Technology | Yes |
| CTO | Giám Đốc Công Nghệ | VNS Technology | Yes |
| DEV | Lập Trình Viên | VNS Technology | No |
| HR | Chuyên Viên Nhân Sự | VNS Technology | No |
| CEO | Tổng Giám Đốc | VNS Solutions | Yes |
| SALES | Nhân Viên Kinh Doanh | VNS Solutions | No |

### Departments (Hierarchy)
| DepartmentCode | DepartmentName | Company | ParentDepartment | Level |
|----------------|----------------|---------|------------------|-------|
| CEO | Ban Giám Đốc | VNS Technology | - | 1 |
| IT | Phòng Công Nghệ Thông Tin | VNS Technology | - | 1 |
| IT-DEV | Bộ phận Phát Triển | VNS Technology | IT | 2 |
| IT-SUPPORT | Bộ phận Hỗ Trợ | VNS Technology | IT | 2 |
| HR | Phòng Nhân Sự | VNS Technology | - | 1 |
| HR-REC | Bộ phận Tuyển Dụng | VNS Technology | HR | 2 |
| CEO | Ban Giám Đốc | VNS Solutions | - | 1 |
| SALES | Phòng Kinh Doanh | VNS Solutions | - | 1 |

### Employees
| EmployeeCode | FullName | Company | Department | Position |
|--------------|----------|---------|------------|----------|
| VNS001-001 | Nguyễn Văn Giám Đốc | VNS Technology | Ban Giám Đốc | Tổng Giám Đốc |
| VNS001-002 | Trần Thị Công Nghệ | VNS Technology | Phòng Công Nghệ Thông Tin | Giám Đốc Công Nghệ |
| VNS001-003 | Lê Văn Lập Trình | VNS Technology | Bộ phận Phát Triển | Lập Trình Viên |
| VNS001-004 | Phạm Thị Nhân Sự | VNS Technology | Bộ phận Tuyển Dụng | Chuyên Viên Nhân Sự |
| VNS002-001 | Hoàng Văn Giám Đốc | VNS Solutions | Ban Giám Đốc | Tổng Giám Đốc |
| VNS002-002 | Võ Thị Kinh Doanh | VNS Solutions | Phòng Kinh Doanh | Nhân Viên Kinh Doanh |

## Lưu ý quan trọng

1. **Thứ tự thực hiện**: Phải tạo dữ liệu theo đúng thứ tự để tránh foreign key constraint
2. **Xóa dữ liệu**: Phải xóa theo thứ tự ngược lại
3. **GUID cố định**: Sử dụng GUID cố định để dễ dàng reference giữa các bảng
4. **Dữ liệu thực tế**: Có thể thay đổi dữ liệu mẫu theo nhu cầu thực tế
5. **Backup**: Nên backup database trước khi chạy seed data

## Troubleshooting

### Lỗi thường gặp
1. **Foreign key constraint**: Kiểm tra thứ tự tạo dữ liệu
2. **Duplicate key**: Kiểm tra unique constraints
3. **Invalid GUID**: Kiểm tra format GUID
4. **Date format**: Kiểm tra format ngày tháng

### Giải pháp
1. Chạy script xóa dữ liệu trước khi tạo mới
2. Kiểm tra log console để xem lỗi chi tiết
3. Sử dụng SQL script để debug từng bước
4. Kiểm tra dữ liệu với script 99.Check_Company_Data.sql
