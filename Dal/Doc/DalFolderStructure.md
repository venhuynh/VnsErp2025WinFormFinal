# DAL Layer - Cấu Trúc Thư Mục Đề Xuất

## 1. Tổng Quan

**Tên Layer:** Data Access Layer (DAL)  
**Mục đích:** Truy cập và thao tác với cơ sở dữ liệu MS SQL Server  
**Loại Project:** Class Library (.dll)  
**Target Framework:** .NET Framework 4.8  
**ORM Framework:** LINQ to SQL  
**Database:** Microsoft SQL Server  

## 2. Cấu Trúc Thư Mục Đề Xuất

```
Dal/
├── DataContext/                  # LINQ to SQL Data Context
│   ├── VnsErp2025.dbml          # LINQ to SQL Designer file
│   ├── VnsErp2025.dbml.layout   # Layout file
│   ├── VnsErp2025.designer.cs   # Auto-generated designer code
│   └── VnsErpDataContext.cs     # Custom DataContext wrapper
├── Entities/                     # LINQ to SQL Entity Classes
│   ├── NhanVien/                # Module nhân viên
│   │   ├── NhanVienEntity.cs    # Entity class
│   │   ├── NhanVienRepository.cs # Repository pattern
│   │   └── NhanVienMapping.cs   # Custom mapping
│   ├── KhachHang/               # Module khách hàng
│   │   ├── KhachHangEntity.cs
│   │   ├── KhachHangRepository.cs
│   │   └── KhachHangMapping.cs
│   ├── SanPham/                 # Module sản phẩm
│   │   ├── SanPhamEntity.cs
│   │   ├── SanPhamRepository.cs
│   │   └── SanPhamMapping.cs
│   ├── DonHang/                 # Module đơn hàng
│   │   ├── DonHangEntity.cs
│   │   ├── ChiTietDonHangEntity.cs
│   │   ├── DonHangRepository.cs
│   │   └── DonHangMapping.cs
│   ├── Kho/                     # Module quản lý kho
│   │   ├── PhieuNhapKhoEntity.cs
│   │   ├── PhieuXuatKhoEntity.cs
│   │   ├── TonKhoEntity.cs
│   │   ├── KhoRepository.cs
│   │   └── KhoMapping.cs
│   ├── TaiChinh/                # Module tài chính
│   │   ├── PhieuThuEntity.cs
│   │   ├── PhieuChiEntity.cs
│   │   ├── BaoCaoTaiChinhEntity.cs
│   │   ├── TaiChinhRepository.cs
│   │   └── TaiChinhMapping.cs
│   └── HeThong/                 # Module hệ thống
│       ├── NguoiDungEntity.cs
│       ├── PhanQuyenEntity.cs
│       ├── LogHeThongEntity.cs
│       ├── HeThongRepository.cs
│       └── HeThongMapping.cs
├── Repositories/                 # Repository Pattern Implementation
│   ├── BaseRepository.cs        # Base repository class
│   ├── IRepository.cs           # Repository interface
│   ├── IUnitOfWork.cs           # Unit of Work interface
│   ├── UnitOfWork.cs            # Unit of Work implementation
│   └── RepositoryFactory.cs     # Factory for creating repositories
├── Connection/                   # Database Connection Management
│   ├── ConnectionManager.cs     # Quản lý kết nối
│   ├── ConnectionStringHelper.cs # Helper cho connection string
│   ├── DatabaseConfig.cs        # Cấu hình database
│   └── IConnectionManager.cs    # Interface connection manager
├── StoredProcedures/             # Stored Procedures Support
│   ├── SP_NhanVien.cs           # Stored procedures cho nhân viên
│   ├── SP_KhachHang.cs          # Stored procedures cho khách hàng
│   ├── SP_SanPham.cs            # Stored procedures cho sản phẩm
│   ├── SP_DonHang.cs            # Stored procedures cho đơn hàng
│   ├── SP_Kho.cs                # Stored procedures cho kho
│   ├── SP_TaiChinh.cs           # Stored procedures cho tài chính
│   └── SP_HeThong.cs            # Stored procedures cho hệ thống
├── Mappings/                     # Entity Mappings
│   ├── EntityMapping.cs         # Base mapping class
│   ├── NhanVienMapping.cs       # Mapping cho nhân viên
│   ├── KhachHangMapping.cs      # Mapping cho khách hàng
│   ├── SanPhamMapping.cs        # Mapping cho sản phẩm
│   ├── DonHangMapping.cs        # Mapping cho đơn hàng
│   ├── KhoMapping.cs            # Mapping cho kho
│   ├── TaiChinhMapping.cs       # Mapping cho tài chính
│   └── HeThongMapping.cs        # Mapping cho hệ thống
├── Extensions/                   # LINQ Extensions
│   ├── QueryExtensions.cs       # Extension methods cho queries
│   ├── LinqExtensions.cs        # Custom LINQ extensions
│   ├── ExpressionExtensions.cs  # Expression tree extensions
│   └── DbContextExtensions.cs   # DataContext extensions
├── Helpers/                      # Helper Classes
│   ├── SqlHelper.cs             # SQL helper utilities
│   ├── DataHelper.cs            # Data manipulation helpers
│   ├── LinqHelper.cs            # LINQ query helpers
│   ├── TransactionHelper.cs     # Transaction management
│   └── PerformanceHelper.cs     # Performance monitoring
├── Interfaces/                   # Data Access Interfaces
│   ├── IRepository.cs           # Generic repository interface
│   ├── IUnitOfWork.cs           # Unit of Work interface
│   ├── IConnectionManager.cs    # Connection manager interface
│   ├── IDataContext.cs          # Data context interface
│   └── ICacheManager.cs         # Cache manager interface
├── Cache/                        # Caching Layer
│   ├── CacheManager.cs          # Cache management
│   ├── MemoryCache.cs           # Memory cache implementation
│   ├── ICacheManager.cs         # Cache manager interface
│   └── CachePolicy.cs           # Cache policies
├── Logging/                      # Data Access Logging
│   ├── DataLogger.cs            # Data access logger
│   ├── SqlLogger.cs             # SQL query logger
│   ├── PerformanceLogger.cs     # Performance logger
│   └── IDataLogger.cs           # Logger interface
├── Exceptions/                   # Data Access Exceptions
│   ├── DataAccessException.cs   # Base data access exception
│   ├── ConnectionException.cs   # Connection related exceptions
│   ├── QueryException.cs        # Query execution exceptions
│   ├── TransactionException.cs  # Transaction related exceptions
│   └── ValidationException.cs   # Data validation exceptions
├── Utilities/                    # Utility Classes
│   ├── DatabaseUtility.cs       # Database utilities
│   ├── SqlUtility.cs            # SQL utilities
│   ├── DataTypeUtility.cs       # Data type utilities
│   ├── SchemaUtility.cs         # Database schema utilities
│   └── BackupUtility.cs         # Database backup utilities
├── Configuration/                # Configuration Classes
│   ├── DatabaseSettings.cs      # Database settings
│   ├── ConnectionSettings.cs    # Connection settings
│   ├── PerformanceSettings.cs   # Performance settings
│   └── CacheSettings.cs         # Cache settings
├── Scripts/                      # Database Scripts
│   ├── CreateTables.sql         # Script tạo bảng
│   ├── CreateIndexes.sql        # Script tạo index
│   ├── CreateStoredProcedures.sql # Script tạo stored procedures
│   ├── CreateFunctions.sql      # Script tạo functions
│   ├── SampleData.sql           # Dữ liệu mẫu
│   └── Migrations/              # Migration scripts
│       ├── 001_InitialSchema.sql
│       ├── 002_AddIndexes.sql
│       └── 003_AddStoredProcedures.sql
├── Tests/                        # Unit Tests cho DAL
│   ├── RepositoryTests/         # Tests cho repositories
│   ├── EntityTests/             # Tests cho entities
│   ├── DataContextTests/        # Tests cho data context
│   └── IntegrationTests/        # Integration tests
└── Properties/                   # Project Properties
    └── AssemblyInfo.cs
```

## 3. Mô Tả Chi Tiết Các Thư Mục

### 3.1 DataContext/
**Mục đích:** Quản lý LINQ to SQL Data Context

**Files:**
- `VnsErp2025.dbml`: LINQ to SQL Designer file (đã có)
- `VnsErp2025.designer.cs`: Auto-generated code từ designer
- `VnsErpDataContext.cs`: Custom wrapper cho DataContext

**Chức năng:**
- Kết nối database
- Quản lý entity mappings
- Transaction management
- Connection pooling

### 3.2 Entities/
**Mục đích:** LINQ to SQL Entity Classes và Repository Pattern

**Cấu trúc mỗi module:**
- `{EntityName}Entity.cs`: Entity class từ LINQ to SQL
- `{EntityName}Repository.cs`: Repository implementation
- `{EntityName}Mapping.cs`: Custom mapping logic

**Quy ước:**
- Entity classes sử dụng tiếng Việt không dấu CamelCase
- Repository pattern cho encapsulation
- Custom mapping cho business logic

### 3.3 Repositories/
**Mục đích:** Repository Pattern Implementation

**Files:**
- `BaseRepository.cs`: Base repository với common operations
- `IRepository.cs`: Generic repository interface
- `IUnitOfWork.cs`: Unit of Work pattern
- `UnitOfWork.cs`: Implementation của Unit of Work
- `RepositoryFactory.cs`: Factory pattern cho repositories

**Chức năng:**
- CRUD operations
- Query optimization
- Transaction management
- Caching support

### 3.4 Connection/
**Mục đích:** Quản lý kết nối database

**Files:**
- `ConnectionManager.cs`: Quản lý connection pooling
- `ConnectionStringHelper.cs`: Helper cho connection string
- `DatabaseConfig.cs`: Cấu hình database settings
- `IConnectionManager.cs`: Interface cho connection manager

**Chức năng:**
- Connection pooling
- Connection string management
- Database configuration
- Connection monitoring

### 3.5 StoredProcedures/
**Mục đích:** Hỗ trợ Stored Procedures

**Files:**
- `SP_{Module}.cs`: Stored procedure wrappers cho từng module

**Chức năng:**
- Execute stored procedures
- Parameter mapping
- Result set handling
- Performance optimization

### 3.6 Mappings/
**Mục đích:** Entity Mappings và Data Transformation

**Files:**
- `EntityMapping.cs`: Base mapping class
- `{Module}Mapping.cs`: Mapping cho từng module

**Chức năng:**
- Entity to DTO mapping
- DTO to Entity mapping
- Data transformation
- Validation mapping

### 3.7 Extensions/
**Mục đích:** LINQ Extensions và Custom Query Methods

**Files:**
- `QueryExtensions.cs`: Extension methods cho LINQ queries
- `LinqExtensions.cs`: Custom LINQ extensions
- `ExpressionExtensions.cs`: Expression tree extensions
- `DbContextExtensions.cs`: DataContext extensions

**Chức năng:**
- Custom query methods
- Performance optimizations
- Reusable query patterns
- Expression tree manipulation

### 3.8 Helpers/
**Mục đích:** Helper Classes cho Data Access

**Files:**
- `SqlHelper.cs`: SQL query helpers
- `DataHelper.cs`: Data manipulation helpers
- `LinqHelper.cs`: LINQ query helpers
- `TransactionHelper.cs`: Transaction management
- `PerformanceHelper.cs`: Performance monitoring

**Chức năng:**
- SQL query building
- Data type conversions
- Performance monitoring
- Transaction management

### 3.9 Cache/
**Mục đích:** Caching Layer cho Performance

**Files:**
- `CacheManager.cs`: Cache management
- `MemoryCache.cs`: Memory cache implementation
- `ICacheManager.cs`: Cache manager interface
- `CachePolicy.cs`: Cache policies

**Chức năng:**
- Query result caching
- Entity caching
- Cache invalidation
- Performance optimization

### 3.10 Logging/
**Mục đích:** Data Access Logging

**Files:**
- `DataLogger.cs`: Data access logger
- `SqlLogger.cs`: SQL query logger
- `PerformanceLogger.cs`: Performance logger
- `IDataLogger.cs`: Logger interface

**Chức năng:**
- SQL query logging
- Performance monitoring
- Error logging
- Audit trail

## 4. LINQ to SQL Implementation

### 4.1 DataContext Configuration
```csharp
public partial class VnsErpDataContext : DataContext
{
    public VnsErpDataContext() : base(GetConnectionString()) { }
    
    public Table<NhanVien> NhanViens => GetTable<NhanVien>();
    public Table<KhachHang> KhachHangs => GetTable<KhachHang>();
    public Table<SanPham> SanPhams => GetTable<SanPham>();
    // ... other tables
}
```

### 4.2 Entity Classes
```csharp
[Table(Name = "nhan_vien")]
public partial class NhanVien
{
    [Column(IsPrimaryKey = true, IsDbGenerated = true)]
    public int Id { get; set; }
    
    [Column(Name = "ho_ten")]
    public string HoTen { get; set; }
    
    [Column(Name = "email")]
    public string Email { get; set; }
    
    // ... other properties
}
```

### 4.3 Repository Pattern
```csharp
public interface IRepository<T> where T : class
{
    IEnumerable<T> GetAll();
    T GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    void SaveChanges();
}
```

## 5. Quy Ước Naming

### 5.1 File Naming
- **Entity Classes:** `{TenEntity}Entity.cs`
- **Repository Classes:** `{TenEntity}Repository.cs`
- **Mapping Classes:** `{TenEntity}Mapping.cs`
- **Stored Procedures:** `SP_{TenModule}.cs`

### 5.2 Class Naming
- Sử dụng tiếng Việt không dấu CamelCase
- Entity classes: `NhanVienEntity`, `KhachHangEntity`
- Repository classes: `NhanVienRepository`, `KhachHangRepository`

### 5.3 Method Naming
- **CRUD Operations:** `LayTatCa`, `LayTheoId`, `Them`, `CapNhat`, `Xoa`
- **Query Operations:** `TimKiem`, `LocTheo`, `SapXepTheo`
- **Custom Operations:** `ThucThiStoredProcedure`, `TinhToan`

### 5.4 Database Naming
- **Table Names:** snake_case tiếng Việt không dấu
- **Column Names:** snake_case tiếng Việt không dấu
- **Stored Procedures:** `sp_{ten_module}_{hanh_dong}`

## 6. Dependencies

### 6.1 Required References
```xml
<Reference Include="System.Data.Linq" />
<Reference Include="System.Data" />
<Reference Include="System.Configuration" />
<Reference Include="System.Transactions" />
```

### 6.2 Connection String Configuration
```xml
<connectionStrings>
  <add name="VnsErp2025ConnectionString" 
       connectionString="Data Source=localhost;Initial Catalog=VnsErp2025;Integrated Security=True" 
       providerName="System.Data.SqlClient" />
</connectionStrings>
```

## 7. Performance Considerations

### 7.1 LINQ to SQL Optimization
- Sử dụng CompiledQuery cho queries thường xuyên
- Implement proper indexing strategy
- Use DataLoadOptions để control eager loading
- Implement query caching

### 7.2 Connection Management
- Connection pooling
- Proper connection disposal
- Transaction scope management
- Deadlock prevention

### 7.3 Caching Strategy
- Entity-level caching
- Query result caching
- Cache invalidation policies
- Memory management

## 8. Security Considerations

### 8.1 SQL Injection Prevention
- Parameterized queries
- Stored procedure usage
- Input validation
- User permission management

### 8.2 Data Access Security
- Connection string encryption
- Role-based access control
- Audit logging
- Data encryption for sensitive fields

---

**Ngày tạo:** $(Get-Date -Format "dd/MM/yyyy")  
**Phiên bản:** 1.0  
**Người tạo:** Project Manager  
**Trạng thái:** Đề xuất
