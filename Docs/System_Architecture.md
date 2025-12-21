# VNS ERP 2025 - Kiến Trúc Hệ Thống

**Phiên bản:** 1.0  
**Ngày cập nhật:** 27/01/2025  
**Trạng thái:** Đang phát triển

---

## 1. Tổng Quan Kiến Trúc

VNS ERP 2025 được xây dựng theo mô hình **3-Layer Architecture** (Kiến trúc 3 lớp), đảm bảo tính tách biệt rõ ràng giữa các tầng và dễ dàng bảo trì, mở rộng.

### 1.1 Nguyên Tắc Thiết Kế
- **Separation of Concerns:** Mỗi layer có trách nhiệm riêng biệt
- **Dependency Inversion:** Layer trên phụ thuộc vào interface của layer dưới
- **Single Responsibility:** Mỗi class/module chỉ có một trách nhiệm
- **DRY (Don't Repeat Yourself):** Tái sử dụng code tối đa

---

## 2. Kiến Trúc 3 Lớp

### 2.1 Sơ Đồ Tổng Quan

```
┌─────────────────────────────────────────────────────────────┐
│                    GUI Layer (Presentation)                  │
│                         VnsErp2025                           │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Forms      │  │   Controls   │  │   Events     │      │
│  │  (Windows    │  │ (DevExpress) │  │  Handling    │      │
│  │   Forms)     │  │              │  │              │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└────────────────────────────┬─────────────────────────────────┘
                             │
                             │ Calls
                             ▼
┌─────────────────────────────────────────────────────────────┐
│                 BLL Layer (Business Logic)                   │
│                         Bll                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Business   │  │   Services   │  │  Validators  │      │
│  │   Objects    │  │              │  │              │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Helpers    │  │   Utilities  │  │  Exceptions  │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└────────────────────────────┬─────────────────────────────────┘
                             │
                             │ Calls
                             ▼
┌─────────────────────────────────────────────────────────────┐
│              DAL Layer (Data Access)                         │
│                         Dal                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Data       │  │   Connection │  │   Helpers    │      │
│  │   Access     │  │   Manager    │  │              │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   LINQ to    │  │   Exceptions  │  │   Cache      │      │
│  │   SQL        │  │               │  │              │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└────────────────────────────┬─────────────────────────────────┘
                             │
                             │ SQL Queries
                             ▼
┌─────────────────────────────────────────────────────────────┐
│              Database Layer (SQL Server)                     │
│                  Microsoft SQL Server                       │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐      │
│  │   Tables     │  │   Views      │  │  Stored      │      │
│  │              │  │              │  │  Procedures │      │
│  └──────────────┘  └──────────────┘  └──────────────┘      │
└─────────────────────────────────────────────────────────────┘
```

---

## 3. GUI Layer (Presentation Layer)

### 3.1 Mục Đích
- Giao diện người dùng
- Xử lý tương tác với người dùng
- Hiển thị dữ liệu
- Thu thập input từ người dùng

### 3.2 Công Nghệ
- **Framework:** Windows Forms (.NET Framework 4.8)
- **UI Components:** DevExpress v25.1
- **Language:** C#

### 3.3 Cấu Trúc

```
VnsErp2025/
├── Forms/                    # Các form chính
│   ├── FrmMain.cs           # Form chính
│   ├── FrmLogin.cs          # Form đăng nhập
│   └── ...
├── UserControls/            # User controls
│   ├── UcCompany.cs
│   └── ...
├── Resources/               # Resources (images, icons)
└── Properties/             # Project properties
```

### 3.4 Trách Nhiệm
- ✅ Hiển thị dữ liệu cho người dùng
- ✅ Thu thập input từ người dùng
- ✅ Xử lý các sự kiện UI (click, change, v.v.)
- ✅ Validation dữ liệu ở tầng presentation (format, required fields)
- ✅ Gọi BLL layer để xử lý business logic
- ❌ KHÔNG chứa business logic
- ❌ KHÔNG truy cập database trực tiếp

### 3.5 Data Flow

```
User Input → Form → BLL Service → DAL DataAccess → Database
                ↓
         Display Result
```

---

## 4. BLL Layer (Business Logic Layer)

### 4.1 Mục Đích
- Xử lý logic nghiệp vụ
- Validation dữ liệu nghiệp vụ
- Quản lý workflow và business rules
- Chuyển đổi dữ liệu giữa DAL và GUI

### 4.2 Cấu Trúc

```
Bll/
├── BusinessObjects/         # Business Objects
│   ├── NhanVien/
│   ├── KhachHang/
│   └── ...
├── Services/                # Business Services
│   ├── INhanVienService.cs
│   ├── NhanVienService.cs
│   └── ...
├── Validators/              # Validators
│   ├── BaseValidator.cs
│   └── ...
├── Helpers/                  # Helper classes
├── Constants/               # Constants
├── Enums/                   # Enums
├── DTOs/                    # Data Transfer Objects
└── Exceptions/              # Custom exceptions
```

### 4.3 Trách Nhiệm
- ✅ Xử lý logic nghiệp vụ phức tạp
- ✅ Validation dữ liệu nghiệp vụ (business rules)
- ✅ Quản lý workflow và business rules
- ✅ Chuyển đổi dữ liệu giữa DAL entities và DTOs
- ✅ Gọi DAL layer để truy cập dữ liệu
- ❌ KHÔNG truy cập database trực tiếp
- ❌ KHÔNG chứa UI logic

### 4.4 Business Objects Pattern

```csharp
public class NhanVienBO
{
    #region thuocTinhDonGian
    public int Id { get; set; }
    public string TenNhanVien { get; set; }
    #endregion
    
    #region phuongThuc
    public bool KiemTraHopLe()
    {
        // Business validation logic
    }
    #endregion
}
```

### 4.5 Services Pattern

```csharp
public interface INhanVienService
{
    List<NhanVienDTO> LayTatCa();
    NhanVienDTO LayTheoId(int id);
    void Them(NhanVienDTO nhanVien);
    void CapNhat(NhanVienDTO nhanVien);
    void Xoa(int id);
}

public class NhanVienService : INhanVienService
{
    private NhanVienDataAccess _dataAccess;
    
    public List<NhanVienDTO> LayTatCa()
    {
        var entities = _dataAccess.LayTatCa();
        return entities.Select(e => ConvertToDTO(e)).ToList();
    }
    
    // ... other methods
}
```

---

## 5. DAL Layer (Data Access Layer)

### 5.1 Mục Đích
- Truy cập và thao tác với cơ sở dữ liệu
- Quản lý kết nối database
- Thực hiện các thao tác CRUD

### 5.2 Công Nghệ
- **ORM:** LINQ to SQL (Drag & Drop Approach)
- **Database:** Microsoft SQL Server
- **Connection:** ADO.NET với connection pooling

### 5.3 Cấu Trúc

```
Dal/
├── DataContext/             # LINQ to SQL Data Context
│   ├── VnsErp2025.dbml      # Designer file
│   └── VnsErp2025.designer.cs # Auto-generated
├── DataAccess/               # Data Access classes
│   ├── NhanVienDataAccess.cs
│   └── ...
├── Connection/               # Connection management
│   ├── ConnectionManager.cs
│   └── ...
├── Helpers/                  # Helper classes
├── Exceptions/               # Data access exceptions
└── Cache/                    # Caching layer
```

### 5.4 Trách Nhiệm
- ✅ Kết nối cơ sở dữ liệu
- ✅ Thực hiện các thao tác CRUD
- ✅ Quản lý connection string và connection pooling
- ✅ Xử lý stored procedures và SQL queries
- ✅ Mapping giữa database tables và entities
- ❌ KHÔNG chứa business logic
- ❌ KHÔNG chứa UI logic

### 5.5 LINQ to SQL Pattern

```csharp
public class NhanVienDataAccess
{
    private VnsErp2025DataContext _context;
    
    public NhanVienDataAccess()
    {
        _context = new VnsErp2025DataContext();
    }
    
    public List<NhanVien> LayTatCa()
    {
        return _context.NhanViens.ToList();
    }
    
    public void Them(NhanVien nhanVien)
    {
        _context.NhanViens.InsertOnSubmit(nhanVien);
        _context.SubmitChanges();
    }
    
    // ... other CRUD methods
}
```

---

## 6. Data Flow

### 6.1 Luồng Dữ Liệu Từ GUI Đến Database

```
1. User nhập liệu trên Form
   ↓
2. Form validate input (format, required)
   ↓
3. Form gọi BLL Service
   ↓
4. BLL Service validate business rules
   ↓
5. BLL Service gọi DAL DataAccess
   ↓
6. DAL DataAccess thực hiện SQL query
   ↓
7. Database trả về kết quả
   ↓
8. DAL DataAccess trả về entities
   ↓
9. BLL Service convert entities → DTOs
   ↓
10. Form hiển thị kết quả cho user
```

### 6.2 Ví Dụ Cụ Thể: Thêm Nhân Viên

```csharp
// GUI Layer
private void btnSave_Click(object sender, EventArgs e)
{
    var dto = new NhanVienDTO
    {
        TenNhanVien = txtTen.Text,
        SoDienThoai = txtPhone.Text
    };
    
    // Validate format
    if (string.IsNullOrEmpty(dto.TenNhanVien))
    {
        MessageBox.Show("Tên nhân viên không được trống");
        return;
    }
    
    // Call BLL
    var service = new NhanVienService();
    service.Them(dto);
}

// BLL Layer
public void Them(NhanVienDTO dto)
{
    // Business validation
    if (KiemTraTrungTen(dto.TenNhanVien))
    {
        throw new BusinessException("Tên nhân viên đã tồn tại");
    }
    
    // Convert DTO to Entity
    var entity = ConvertToEntity(dto);
    
    // Call DAL
    var dataAccess = new NhanVienDataAccess();
    dataAccess.Them(entity);
}

// DAL Layer
public void Them(NhanVien entity)
{
    _context.NhanViens.InsertOnSubmit(entity);
    _context.SubmitChanges();
}
```

---

## 7. Design Patterns Sử Dụng

### 7.1 Repository Pattern (Simplified)
- **DAL Layer:** DataAccess classes đóng vai trò repository
- **Mục đích:** Tách biệt data access logic

### 7.2 Service Pattern
- **BLL Layer:** Service classes xử lý business logic
- **Mục đích:** Tách biệt business logic

### 7.3 DTO Pattern
- **Data Transfer Objects:** Chuyển dữ liệu giữa các layer
- **Mục đích:** Giảm coupling giữa các layer

### 7.4 Singleton Pattern
- **ConnectionManager:** Quản lý connection duy nhất
- **DatabaseConfig:** Cấu hình database duy nhất

### 7.5 Factory Pattern
- **Connection Creation:** Tạo connection từ factory
- **Entity Creation:** Tạo entities từ factory (nếu cần)

---

## 8. Exception Handling

### 8.1 Exception Hierarchy

```
Exception
├── DataAccessException (DAL)
│   ├── ConnectionException
│   ├── QueryException
│   └── TransactionException
├── BusinessException (BLL)
│   ├── ValidationException
│   └── BusinessRuleException
└── SystemException (Common)
```

### 8.2 Exception Flow

```
Database Error
    ↓
DAL Layer: Catch → Wrap in DataAccessException
    ↓
BLL Layer: Catch → Wrap in BusinessException (if needed)
    ↓
GUI Layer: Catch → Display user-friendly message
```

---

## 9. Security Architecture

### 9.1 Authentication Flow

```
User Login
    ↓
Authentication Module
    ↓
Validate Credentials
    ↓
Create Session
    ↓
Store User Info
```

### 9.2 Authorization Flow

```
User Action
    ↓
Check Permission (GUI Layer)
    ↓
Check Permission (BLL Layer)
    ↓
Check Permission (DAL Layer - if needed)
    ↓
Execute Action
```

### 9.3 Permission System

- **Role-based:** User có Roles
- **Permission-based:** Role có Permissions
- **Override:** User có thể có Permissions trực tiếp (override Role)

---

## 10. Performance Optimization

### 10.1 Database Level
- Connection pooling
- Indexed queries
- Query caching
- Stored procedures

### 10.2 Application Level
- Lazy loading
- Pagination
- Background processing
- Caching layer

### 10.3 UI Level
- Async operations
- Progress indicators
- Virtual scrolling (DevExpress GridControl)

---

## 11. Testing Strategy

### 11.1 Unit Testing
- **BLL Layer:** Test business logic
- **DAL Layer:** Test data access (with mock database)

### 11.2 Integration Testing
- Test integration giữa các layer
- Test database operations

### 11.3 UI Testing
- Test form functionality
- Test user workflows

---

## 12. Deployment Architecture

### 12.1 Single-Tier Deployment
```
[Client Machine]
    ├── VnsErp2025.exe (GUI)
    ├── Bll.dll
    ├── Dal.dll
    └── [Other DLLs]
            ↓
    [Network]
            ↓
[Database Server]
    └── SQL Server
```

### 12.2 Configuration
- Connection string trong App.config
- Environment-specific settings
- License configuration (DevExpress)

---

## 13. Tài Liệu Liên Quan

- [Tổng Quan Hệ Thống](./System_Overview.md)
- [Cấu Trúc DAL Layer](../Dal/Doc/DalFolderStructure.md)
- [Cấu Trúc BLL Layer](../Bll/Doc/BllFolderStructure.md)
- [Connection Management](../Dal/Connection/README.md)

---

**Người tạo:** Development Team  
**Ngày tạo:** 27/01/2025  
**Trạng thái:** Đang phát triển






