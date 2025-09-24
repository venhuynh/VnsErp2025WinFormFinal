# DAL Layer - Cấu Trúc Thư Mục Đề Xuất

## 1. Tổng Quan

**Tên Layer:** Data Access Layer (DAL)  
**Mục đích:** Truy cập và thao tác với cơ sở dữ liệu MS SQL Server  
**Loại Project:** Class Library (.dll)  
**Target Framework:** .NET Framework 4.8  
**ORM Framework:** LINQ to SQL (Drag & Drop Approach)  
**Database:** Microsoft SQL Server  

### Đặc điểm LINQ to SQL Implementation:
- **Drag & Drop Design:** Kéo thả bảng từ Server Explorer vào file `.dbml`
- **Auto-generated Entities:** Tự động tạo entity classes từ database schema
- **Direct CRUD Operations:** Thao tác CRUD trực tiếp trên các entity được generate
- **Simple & Fast:** Phù hợp cho dự án nội bộ, không cần phức tạp
- **Visual Designer:** Sử dụng LINQ to SQL Designer trong Visual Studio  

## 2. Cấu Trúc Thư Mục Đề Xuất

```
Dal/
├── DataContext/                  # LINQ to SQL Data Context (Auto-generated)
│   ├── VnsErp2025.dbml          # LINQ to SQL Designer file (Drag & Drop)
│   ├── VnsErp2025.dbml.layout   # Layout file (Auto-generated)
│   ├── VnsErp2025.designer.cs   # Auto-generated designer code + Entities
│   └── VnsErpDataContext.cs     # Custom DataContext wrapper (Optional)
├── DataAccess/                   # CRUD Operations cho từng Entity
│   ├── NhanVienDataAccess.cs    # CRUD operations cho NhanVien
│   ├── KhachHangDataAccess.cs   # CRUD operations cho KhachHang
│   ├── SanPhamDataAccess.cs     # CRUD operations cho SanPham
│   ├── DonHangDataAccess.cs     # CRUD operations cho DonHang
│   ├── KhoDataAccess.cs         # CRUD operations cho Kho entities
│   ├── TaiChinhDataAccess.cs    # CRUD operations cho TaiChinh entities
│   └── HeThongDataAccess.cs     # CRUD operations cho HeThong entities
├── BaseDataAccess/              # Base Classes cho Data Access
│   ├── BaseDataAccess.cs        # Base class cho tất cả DataAccess classes
│   ├── IDataAccess.cs           # Interface cho DataAccess
│   └── DataAccessHelper.cs      # Helper methods chung
├── Connection/                   # Database Connection Management
│   ├── ConnectionManager.cs     # Quản lý kết nối
│   ├── ConnectionStringHelper.cs # Helper cho connection string
│   ├── DatabaseConfig.cs        # Cấu hình database
│   └── IConnectionManager.cs    # Interface connection manager
├── Extensions/                   # Entity Extensions (Optional)
│   ├── NhanVienExtensions.cs    # Extension methods cho NhanVien
│   ├── KhachHangExtensions.cs   # Extension methods cho KhachHang
│   ├── SanPhamExtensions.cs     # Extension methods cho SanPham
│   └── CommonExtensions.cs      # Common extension methods
├── LinqExtensions/              # LINQ Extensions
│   ├── QueryExtensions.cs       # Extension methods cho queries
│   ├── LinqExtensions.cs        # Custom LINQ extensions
│   ├── ExpressionExtensions.cs  # Expression tree extensions
│   └── DataContextExtensions.cs # DataContext extensions
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
**Mục đích:** LINQ to SQL Data Context (Auto-generated từ Visual Studio Designer)

**Files:**
- `VnsErp2025.dbml`: LINQ to SQL Designer file (Drag & Drop từ Server Explorer)
- `VnsErp2025.designer.cs`: Auto-generated code từ designer (KHÔNG EDIT)
- `VnsErpDataContext.cs`: Custom wrapper cho DataContext (Optional)

**Cách sử dụng:**
1. Mở Server Explorer trong Visual Studio
2. Kéo thả các bảng từ database vào file `.dbml`
3. Visual Studio tự động generate entity classes
4. Sử dụng DataContext để thao tác với entities

**Chức năng:**
- Auto-generated entity classes từ database schema
- Automatic mapping giữa database tables và C# classes
- Built-in CRUD operations
- Change tracking và concurrency control

### 3.2 Entities (Trong VnsErp2025.designer.cs)
**Mục đích:** Auto-generated Entity Classes từ LINQ to SQL Designer

**Vị trí:** Tất cả entities được generate trong file `VnsErp2025.designer.cs`

**Cách tạo:**
- Kéo thả bảng từ Server Explorer vào file `.dbml`
- Visual Studio tự động generate entity class trong `.designer.cs`
- Không cần viết code, chỉ cần drag & drop

**Quy ước:**
- Entity classes tự động map với database tables
- Property names giữ nguyên như database columns
- Relationships tự động được tạo
- KHÔNG EDIT file `.designer.cs` (sẽ bị mất khi regenerate)

**Ví dụ từ file hiện tại:**
```csharp
// Auto-generated trong VnsErp2025.designer.cs
[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ApplicationUser")]
public partial class ApplicationUser : INotifyPropertyChanging, INotifyPropertyChanged
{
    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
    public System.Guid Id { get; set; }
    
    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
    public string UserName { get; set; }
    
    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_HassPassword", DbType="NVarChar(500) NOT NULL", CanBeNull=false)]
    public string HassPassword { get; set; }
    
    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Active", DbType="Bit NOT NULL")]
    public bool Active { get; set; }
}
```

### 3.2 DataAccess/
**Mục đích:** CRUD Operations cho từng Entity (Thay thế Repository Pattern)

**Files:**
- `NhanVienDataAccess.cs`: CRUD operations cho NhanVien entity
- `KhachHangDataAccess.cs`: CRUD operations cho KhachHang entity
- `SanPhamDataAccess.cs`: CRUD operations cho SanPham entity
- ... và các entity khác

**Chức năng:**
- Direct CRUD operations trên auto-generated entities
- Simple và straightforward approach
- Không cần phức tạp Repository pattern
- Sử dụng LINQ to SQL DataContext trực tiếp

**Ví dụ:**
```csharp
public class NhanVienDataAccess
{
    private VnsErp2025DataContext _context;
    
    public List<NhanVien> LayTatCa()
    {
        return _context.NhanViens.ToList();
    }
    
    public NhanVien LayTheoId(int id)
    {
        return _context.NhanViens.FirstOrDefault(n => n.Id == id);
    }
    
    public void Them(NhanVien nhanVien)
    {
        _context.NhanViens.InsertOnSubmit(nhanVien);
        _context.SubmitChanges();
    }
}
```

### 3.3 Connection/
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


### 3.4 Extensions/
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

### 3.5 Helpers/
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

### 3.6 Cache/
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

### 3.7 Logging/
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

## 4. LINQ to SQL Implementation (Drag & Drop Approach)

### 4.1 Cách Tạo Entities
1. **Mở Server Explorer** trong Visual Studio
2. **Kết nối database** (nếu chưa có)
3. **Mở file VnsErp2025.dbml** trong Designer
4. **Kéo thả bảng** từ Server Explorer vào Designer
5. **Visual Studio tự động generate** entity classes

### 4.2 Auto-generated DataContext
```csharp
// File: VnsErp2025.designer.cs (KHÔNG EDIT)
public partial class VnsErp2025DataContext : System.Data.Linq.DataContext
{
    public System.Data.Linq.Table<NhanVien> NhanViens
    {
        get { return this.GetTable<NhanVien>(); }
    }
    
    public System.Data.Linq.Table<KhachHang> KhachHangs
    {
        get { return this.GetTable<KhachHang>(); }
    }
    
    // ... other tables
}
```

### 4.3 Auto-generated Entity Classes
```csharp
// File: VnsErp2025.designer.cs (KHÔNG EDIT)
// Ví dụ từ ApplicationUser entity hiện tại:
[global::System.Data.Linq.Mapping.TableAttribute(Name="dbo.ApplicationUser")]
public partial class ApplicationUser : INotifyPropertyChanging, INotifyPropertyChanged
{
    private System.Guid _Id;
    private string _UserName;
    private string _HassPassword;
    private bool _Active;
    
    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_Id", DbType="UniqueIdentifier NOT NULL", IsPrimaryKey=true)]
    public System.Guid Id
    {
        get { return this._Id; }
        set { /* auto-generated with change tracking */ }
    }
    
    [global::System.Data.Linq.Mapping.ColumnAttribute(Storage="_UserName", DbType="NVarChar(50) NOT NULL", CanBeNull=false)]
    public string UserName
    {
        get { return this._UserName; }
        set { /* auto-generated with change tracking */ }
    }
    
    // ... other properties
}
```

### 4.4 DataAccess Pattern (Thay thế Repository)
```csharp
public class ApplicationUserDataAccess
{
    private VnsErp2025DataContext _context;
    
    public ApplicationUserDataAccess()
    {
        _context = new VnsErp2025DataContext();
    }
    
    // CRUD Operations
    public List<ApplicationUser> LayTatCa()
    {
        return _context.ApplicationUsers.ToList();
    }
    
    public ApplicationUser LayTheoId(Guid id)
    {
        return _context.ApplicationUsers.FirstOrDefault(u => u.Id == id);
    }
    
    public ApplicationUser LayTheoUserName(string userName)
    {
        return _context.ApplicationUsers.FirstOrDefault(u => u.UserName == userName);
    }
    
    public void Them(ApplicationUser user)
    {
        _context.ApplicationUsers.InsertOnSubmit(user);
        _context.SubmitChanges();
    }
    
    public void CapNhat(ApplicationUser user)
    {
        // LINQ to SQL tự động track changes
        _context.SubmitChanges();
    }
    
    public void Xoa(Guid id)
    {
        var user = LayTheoId(id);
        if (user != null)
        {
            _context.ApplicationUsers.DeleteOnSubmit(user);
            _context.SubmitChanges();
        }
    }
    
    public void Dispose()
    {
        _context?.Dispose();
    }
}
```

## 5. Quy Ước Naming

### 5.1 File Naming
- **Entity Classes:** Auto-generated từ .dbml (KHÔNG ĐỔI TÊN)
- **DataAccess Classes:** `{TenEntity}DataAccess.cs`
- **Extension Classes:** `{TenEntity}Extensions.cs`
- **Helper Classes:** `{TenHelper}Helper.cs`

### 5.2 Class Naming
- **Entity Classes:** Tự động từ database table names
- **DataAccess Classes:** `NhanVienDataAccess`, `KhachHangDataAccess`
- **Extension Classes:** `NhanVienExtensions`, `KhachHangExtensions`

### 5.3 Method Naming
- **CRUD Operations:** `LayTatCa`, `LayTheoId`, `Them`, `CapNhat`, `Xoa`
- **Query Operations:** `TimKiem`, `LocTheo`, `SapXepTheo`
- **Custom Operations:** `TinhToan`, `XuLy`, `KiemTra`

### 5.4 Database Naming
- **Table Names:** snake_case tiếng Việt không dấu
- **Column Names:** snake_case tiếng Việt không dấu
- **Database Schema:** Simple và straightforward

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

## 8. Workflow Phát Triển DAL

### 8.1 Quy Trình Tạo Entity
1. **Thiết kế Database Schema** trước
2. **Mở Visual Studio Server Explorer**
3. **Kết nối database** (nếu chưa có)
4. **Mở file VnsErp2025.dbml** trong Designer
5. **Kéo thả bảng** từ Server Explorer vào Designer
6. **Kiểm tra mapping** trong Designer
7. **Build solution** để generate code
8. **KHÔNG EDIT** các file .designer.cs

### 8.2 Quy Trình Tạo DataAccess
1. **Tạo class DataAccess** cho entity mới
2. **Inherit từ BaseDataAccess** (nếu có)
3. **Implement CRUD methods** cơ bản
4. **Thêm business logic** nếu cần
5. **Test CRUD operations**

### 8.3 Quy Trình Cập Nhật Entity
1. **Thay đổi database schema** trước
2. **Refresh trong Server Explorer**
3. **Update trong .dbml Designer**
4. **Build solution** để regenerate
5. **Update DataAccess class** nếu cần
6. **Test lại functionality**

## 9. Security Considerations

### 9.1 SQL Injection Prevention
- LINQ to SQL tự động parameterize queries
- Sử dụng LINQ thay vì raw SQL
- Input validation ở BLL layer
- User permission management

### 9.2 Data Access Security
- Connection string encryption
- Role-based access control
- Audit logging
- Data encryption for sensitive fields

## 10. Best Practices

### 10.1 Entity Management
- **KHÔNG EDIT** auto-generated entity classes
- Sử dụng **partial classes** nếu cần extend
- **Backup .dbml file** trước khi thay đổi
- **Test thoroughly** sau khi regenerate

### 10.2 DataAccess Pattern
- **Simple CRUD** operations
- **Business logic** ở BLL layer
- **Error handling** comprehensive
- **Transaction management** khi cần

### 10.3 Performance
- **Lazy loading** cho relationships
- **CompiledQuery** cho queries thường xuyên
- **Connection pooling** được enable
- **Proper disposal** của DataContext

---

**Ngày tạo:** 15/01/2025  
**Phiên bản:** 2.0  
**Người tạo:** Project Manager  
**Trạng thái:** Cập nhật cho Drag & Drop Approach
