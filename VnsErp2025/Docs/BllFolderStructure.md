# BLL Layer - Cấu Trúc Thư Mục Đề Xuất

## 1. Tổng Quan

**Tên Layer:** Business Logic Layer (BLL)  
**Mục đích:** Xử lý logic nghiệp vụ, quy tắc kinh doanh của hệ thống ERP  
**Loại Project:** Class Library (.dll)  
**Target Framework:** .NET Framework 4.8  

## 2. Cấu Trúc Thư Mục Đề Xuất

```
Bll/
├── BusinessObjects/              # Các đối tượng nghiệp vụ
│   ├── NhanVien/                # Module nhân viên
│   │   ├── NhanVienBO.cs        # Business Object chính
│   │   ├── NhanVienValidator.cs # Validation logic
│   │   └── DataDictionary.md    # Tài liệu đối tượng
│   ├── KhachHang/               # Module khách hàng
│   │   ├── KhachHangBO.cs
│   │   ├── KhachHangValidator.cs
│   │   └── DataDictionary.md
│   ├── SanPham/                 # Module sản phẩm
│   │   ├── SanPhamBO.cs
│   │   ├── SanPhamValidator.cs
│   │   └── DataDictionary.md
│   ├── DonHang/                 # Module đơn hàng
│   │   ├── DonHangBO.cs
│   │   ├── DonHangValidator.cs
│   │   ├── ChiTietDonHangBO.cs
│   │   └── DataDictionary.md
│   ├── Kho/                     # Module quản lý kho
│   │   ├── PhieuNhapKhoBO.cs
│   │   ├── PhieuXuatKhoBO.cs
│   │   ├── TonKhoBO.cs
│   │   └── DataDictionary.md
│   ├── TaiChinh/                # Module tài chính
│   │   ├── PhieuThuBO.cs
│   │   ├── PhieuChiBO.cs
│   │   ├── BaoCaoTaiChinhBO.cs
│   │   └── DataDictionary.md
│   └── HeThong/                 # Module hệ thống
│       ├── NguoiDungBO.cs
│       ├── PhanQuyenBO.cs
│       ├── LogHeThongBO.cs
│       └── DataDictionary.md
├── Services/                     # Các service nghiệp vụ
│   ├── INhanVienService.cs      # Interface cho service
│   ├── NhanVienService.cs       # Implementation
│   ├── IKhachHangService.cs
│   ├── KhachHangService.cs
│   ├── ISanPhamService.cs
│   ├── SanPhamService.cs
│   ├── IDonHangService.cs
│   ├── DonHangService.cs
│   ├── IKhoService.cs
│   ├── KhoService.cs
│   ├── ITaiChinhService.cs
│   ├── TaiChinhService.cs
│   └── IHeThongService.cs
│       └── HeThongService.cs
├── Validators/                   # Các validator chung
│   ├── BaseValidator.cs          # Validator cơ sở
│   ├── EmailValidator.cs         # Validation email
│   ├── PhoneValidator.cs         # Validation số điện thoại
│   ├── DateValidator.cs          # Validation ngày tháng
│   └── NumberValidator.cs        # Validation số
├── Helpers/                      # Các helper class
│   ├── StringHelper.cs           # Xử lý chuỗi
│   ├── DateHelper.cs             # Xử lý ngày tháng
│   ├── NumberHelper.cs           # Xử lý số
│   ├── CryptoHelper.cs           # Mã hóa/giải mã
│   └── ConfigHelper.cs           # Đọc cấu hình
├── Constants/                    # Các hằng số
│   ├── BusinessConstants.cs      # Hằng số nghiệp vụ
│   ├── ErrorMessages.cs          # Thông báo lỗi
│   ├── SuccessMessages.cs        # Thông báo thành công
│   └── SystemConstants.cs        # Hằng số hệ thống
├── Enums/                        # Các enum
│   ├── TrangThaiEnum.cs          # Trạng thái chung
│   ├── LoaiNhanVienEnum.cs       # Loại nhân viên
│   ├── LoaiKhachHangEnum.cs      # Loại khách hàng
│   ├── TrangThaiDonHangEnum.cs   # Trạng thái đơn hàng
│   └── LoaiPhieuKhoEnum.cs       # Loại phiếu kho
├── DTOs/                         # Data Transfer Objects
│   ├── NhanVienDTO.cs            # DTO cho nhân viên
│   ├── KhachHangDTO.cs           # DTO cho khách hàng
│   ├── SanPhamDTO.cs             # DTO cho sản phẩm
│   ├── DonHangDTO.cs             # DTO cho đơn hàng
│   └── BaoCaoDTO.cs              # DTO cho báo cáo
├── Interfaces/                   # Các interface
│   ├── IBusinessService.cs       # Interface service cơ sở
│   ├── IValidator.cs             # Interface validator
│   ├── ILogger.cs                # Interface logging
│   └── IConfiguration.cs         # Interface cấu hình
├── Exceptions/                   # Custom exceptions
│   ├── BusinessException.cs      # Exception nghiệp vụ
│   ├── ValidationException.cs    # Exception validation
│   ├── DataAccessException.cs    # Exception truy cập dữ liệu
│   └── SystemException.cs        # Exception hệ thống
├── Extensions/                   # Extension methods
│   ├── StringExtensions.cs       # Extension cho string
│   ├── DateTimeExtensions.cs     # Extension cho DateTime
│   ├── ListExtensions.cs         # Extension cho List
│   └── ObjectExtensions.cs       # Extension cho Object
├── Utilities/                    # Các utility class
│   ├── BusinessRulesEngine.cs    # Engine xử lý business rules
│   ├── WorkflowEngine.cs         # Engine xử lý workflow
│   ├── CalculationEngine.cs      # Engine tính toán
│   └── NotificationEngine.cs     # Engine thông báo
└── Properties/                   # Properties của project
    └── AssemblyInfo.cs
```

## 3. Mô Tả Chi Tiết Các Thư Mục

### 3.1 BusinessObjects/
**Mục đích:** Chứa các Business Object (BO) đại diện cho các thực thể nghiệp vụ

**Cấu trúc mỗi module:**
- `{TenModule}BO.cs`: Class chính của Business Object
- `{TenModule}Validator.cs`: Logic validation riêng cho module
- `DataDictionary.md`: Tài liệu mô tả cấu trúc dữ liệu

**Quy ước:**
- Tên class sử dụng tiếng Việt không dấu CamelCase
- Mỗi BO có region riêng: `thuocTinhDonGian`, `thuocTinhQuanHe`, `danhSachLienKet`, `phuongThuc`, `enum`

### 3.2 Services/
**Mục đích:** Chứa các service xử lý logic nghiệp vụ phức tạp

**Cấu trúc:**
- `I{ServiceName}Service.cs`: Interface định nghĩa contract
- `{ServiceName}Service.cs`: Implementation của service

**Chức năng:**
- Xử lý workflow nghiệp vụ
- Gọi DAL layer
- Validation phức tạp
- Business rules

### 3.3 Validators/
**Mục đích:** Các validator có thể tái sử dụng

**Loại validator:**
- Validation cơ bản (email, phone, date, number)
- Validation nghiệp vụ phức tạp
- Cross-field validation

### 3.4 Helpers/
**Mục đích:** Các utility class hỗ trợ xử lý dữ liệu

**Chức năng:**
- Format dữ liệu
- Chuyển đổi kiểu dữ liệu
- Mã hóa/giải mã
- Đọc cấu hình

### 3.5 Constants/
**Mục đích:** Định nghĩa các hằng số sử dụng trong toàn bộ hệ thống

**Phân loại:**
- Business constants: Các hằng số nghiệp vụ
- Error messages: Thông báo lỗi
- Success messages: Thông báo thành công
- System constants: Hằng số hệ thống

### 3.6 Enums/
**Mục đích:** Định nghĩa các enum cho các giá trị cố định

**Quy ước:**
- Tên enum kết thúc bằng "Enum"
- Sử dụng tiếng Việt không dấu
- Có documentation cho mỗi giá trị

### 3.7 DTOs/
**Mục đích:** Data Transfer Objects để truyền dữ liệu giữa các layer

**Đặc điểm:**
- Chỉ chứa properties, không có logic
- Serializable
- Có thể có nested DTOs

### 3.8 Interfaces/
**Mục đích:** Định nghĩa contracts cho các component

**Loại interface:**
- Service interfaces
- Validator interfaces
- Infrastructure interfaces

### 3.9 Exceptions/
**Mục đích:** Custom exceptions cho xử lý lỗi

**Phân loại:**
- Business exceptions: Lỗi logic nghiệp vụ
- Validation exceptions: Lỗi validation
- Data access exceptions: Lỗi truy cập dữ liệu
- System exceptions: Lỗi hệ thống

### 3.10 Extensions/
**Mục đích:** Extension methods để mở rộng functionality

**Loại extension:**
- String extensions
- DateTime extensions
- Collection extensions
- Object extensions

### 3.11 Utilities/
**Mục đích:** Các engine xử lý logic phức tạp

**Loại engine:**
- Business rules engine
- Workflow engine
- Calculation engine
- Notification engine

## 4. Quy Ước Naming

### 4.1 File Naming
- **Business Objects:** `{TenEntity}BO.cs`
- **Services:** `{TenEntity}Service.cs`, `I{TenEntity}Service.cs`
- **Validators:** `{TenEntity}Validator.cs`
- **DTOs:** `{TenEntity}DTO.cs`
- **Enums:** `{TenEnum}Enum.cs`
- **Helpers:** `{TenHelper}Helper.cs`
- **Exceptions:** `{TenException}Exception.cs`

### 4.2 Class Naming
- Sử dụng tiếng Việt không dấu CamelCase
- BO classes: `NhanVienBO`, `KhachHangBO`
- Service classes: `NhanVienService`, `KhachHangService`
- Validator classes: `NhanVienValidator`, `KhachHangValidator`

### 4.3 Method Naming
- **CRUD Operations:** `Tao`, `Lay`, `CapNhat`, `Xoa`
- **Business Operations:** `TinhToan`, `XacThuc`, `XuLy`
- **Validation:** `KiemTra`, `Validate`

## 5. Dependencies

### 5.1 Internal Dependencies
```
Bll
├── References Dal (Data Access Layer)
├── Uses System.Data
├── Uses System.Configuration
└── Uses System.ComponentModel.DataAnnotations
```

### 5.2 External Dependencies
- **.NET Framework 4.8** base libraries
- **System.ComponentModel.DataAnnotations** (validation)
- **System.Configuration** (app config)
- **System.Data** (data types)

## 6. Code Standards

### 6.1 Regions
Mỗi class sử dụng regions bằng tiếng Việt không dấu:
```csharp
#region thuocTinhDonGian
#region thuocTinhQuanHe  
#region danhSachLienKet
#region phuongThuc
#region enum
```

### 6.2 Comments
- XML documentation cho public methods
- Comments bằng tiếng Việt cho logic phức tạp
- TODO comments cho các tính năng chưa implement

### 6.3 Error Handling
- Sử dụng custom exceptions
- Logging đầy đủ
- User-friendly error messages

---

**Ngày tạo:** $(Get-Date -Format "dd/MM/yyyy")  
**Phiên bản:** 1.0  
**Người tạo:** Project Manager  
**Trạng thái:** Đề xuất
