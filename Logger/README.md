# Hệ thống Logging - VNTA.NET 2025

## 📋 Tổng quan

Hệ thống logging được thiết kế để cung cấp khả năng ghi log linh hoạt và mạnh mẽ cho ứng dụng VNTA.NET 2025, hỗ trợ cả ghi log ra file và hiển thị trên console với cấu hình XML linh hoạt.

## 🏗️ Kiến trúc

```
Common/Logger/
├── Interfaces/           # Các interface chính
├── Implementations/      # Các implementation
├── Configuration/        # Cấu hình và enums
├── Models/              # Data models
├── Extensions/          # Extension methods
└── Examples/            # Ví dụ sử dụng
```

## 🚀 Tính năng chính

### ✅ **File Logging**
- Ghi log theo ngày: `VNTA-NET-2025_2025-01-15.log`
- Log rotation tự động (khi file > 10MB)
- Giữ lại tối đa 30 file log
- Thread-safe writing
- Format: `[2025-01-15 14:30:25.123] [INFO] [DAL] [T:1] Message`

### ✅ **Console Logging**
- Hiển thị real-time trên console
- Color coding theo log level
- Có thể bật/tắt theo cấu hình
- Format tùy chỉnh

### ✅ **Log Levels**
- `Trace` - Chi tiết nhất (debugging)
- `Debug` - Thông tin debug
- `Info` - Thông tin chung
- `Warning` - Cảnh báo
- `Error` - Lỗi
- `Fatal` - Lỗi nghiêm trọng

### ✅ **Categories**
- `UI` - User Interface
- `BLL` - Business Logic Layer
- `DAL` - Data Access Layer
- `Security` - Bảo mật
- `Configuration` - Cấu hình
- `Database` - Database operations
- `Authentication` - Xác thực
- `Audit` - Audit trail

## ⚙️ Cấu hình

### App.config
```xml
<appSettings>
  <!-- Logging Configuration -->
  <add key="Logging.MinimumLevel" value="Info" />
  <add key="Logging.EnableConsole" value="true" />
  <add key="Logging.EnableFile" value="true" />
  <add key="Logging.LogDirectory" value="Logs" />
  <add key="Logging.LogFilePattern" value="VNTA-QuangVienPrinting_{date}.log" />
  <add key="Logging.MaxFileSizeMB" value="10" />
  <add key="Logging.MaxFiles" value="30" />
  <add key="Logging.ShowTimestampOnConsole" value="true" />
  <add key="Logging.ShowCategoryOnConsole" value="true" />
</appSettings>
```

## 📖 Cách sử dụng

### 1. **Basic Usage**
```csharp
// Tạo logger với category mặc định
var logger = LoggerFactory.CreateLogger();

logger.Info("Hệ thống khởi động");
logger.Warning("Cảnh báo: Cấu hình chưa được thiết lập");
logger.Error("Lỗi kết nối database");
```

### 2. **Category-specific Logging**
```csharp
// Tạo logger cho từng layer
var uiLogger = LoggerFactory.CreateLogger(LogCategory.UI);
var bllLogger = LoggerFactory.CreateLogger(LogCategory.BLL);
var dalLogger = LoggerFactory.CreateLogger(LogCategory.DAL);

uiLogger.Info("Form được khởi tạo thành công");
bllLogger.Info("Business logic được thực thi");
dalLogger.Info("Kết nối database thành công");
```

### 3. **Exception Logging**
```csharp
var logger = LoggerFactory.CreateLogger(LogCategory.DAL);

try
{
    // Database operation
}
catch (Exception ex)
{
    logger.Error("Lỗi khi thực hiện truy vấn database", ex);
}
```

### 4. **Performance Logging**
```csharp
var logger = LoggerFactory.CreateLogger(LogCategory.DAL);

// Log performance với Action
logger.LogPerformance("Load user data", () =>
{
    // Load data logic
});

// Log performance với Func<T>
var result = logger.LogPerformance("Calculate total", () =>
{
    return CalculateTotal();
});
```

### 5. **Security & Audit Logging**
```csharp
var securityLogger = LoggerFactory.CreateLogger(LogCategory.Security);
var auditLogger = LoggerFactory.CreateLogger(LogCategory.Audit);

// Security events
securityLogger.LogSecurityEvent("Login Attempt", "User: admin, IP: 192.168.1.100");

// Audit trail
auditLogger.LogAudit("CREATE", "User", "123", "admin");
```

## 🔧 Extension Methods

### Performance Logging
```csharp
logger.LogPerformance("Operation name", () => { /* work */ });
logger.LogPerformance("Operation name", () => { return result; });
```

### Method Entry/Exit
```csharp
logger.LogMethodEntry("MethodName", param1, param2);
logger.LogMethodExit("MethodName", returnValue);
```

### Database Operations
```csharp
logger.LogDatabaseOperation("SELECT", "SELECT * FROM Users");
```

### Security Events
```csharp
logger.LogSecurityEvent("Login Attempt", "User: admin");
```

### Audit Trail
```csharp
logger.LogAudit("CREATE", "User", "123", "admin");
```

### Configuration Changes
```csharp
logger.LogConfigChange("Database.ConnectionString", "old", "new");
```

### Structured Logging
```csharp
var data = new { Id = 123, Name = "John" };
logger.LogStructured(LogLevel.Info, "User created", data);
```

## 🎨 Console Output

Console logging hỗ trợ color coding:
- **Trace**: Gray
- **Debug**: Cyan
- **Info**: White
- **Warning**: Yellow
- **Error**: Red
- **Fatal**: Magenta

## 📁 File Output

File logging format:
```
[2025-01-15 14:30:25.123] [INFO] [DAL] [T:1] Kết nối database thành công
[2025-01-15 14:30:25.124] [ERROR] [DAL] [T:1] Lỗi kết nối database
Exception: System.InvalidOperationException: Connection timeout
   at VNTA_QuangVienPrinting.DAL.DatabaseHelper.Connect()
```

## 🔄 Log Rotation

- File log được rotate khi đạt kích thước tối đa (mặc định 10MB)
- Tên file rotated: `VNTA-QuangVienPrinting_2025-01-15_14-30-25.log`
- Giữ lại tối đa 30 file log cũ
- Tự động dọn dẹp file cũ

## 🛡️ Thread Safety

- Tất cả logging operations đều thread-safe
- Sử dụng lock objects để đảm bảo thread safety
- Không block UI thread

## 🚀 Performance

- Logging không ảnh hưởng đến performance của ứng dụng
- Sử dụng async operations khi có thể
- Buffering để tối ưu I/O operations

## 🔧 Customization

### Custom Configuration
```csharp
var config = new LogConfiguration
{
    MinimumLevel = LogLevel.Debug,
    EnableConsole = true,
    EnableFile = false,
    ShowTimestampOnConsole = true,
    ShowCategoryOnConsole = false
};

var logger = LoggerFactory.CreateLogger(config, LogCategory.System);
```

### Custom Targets
```csharp
// Tạo custom target
var customTarget = new MyCustomLogTarget();
var compositeTarget = new CompositeLogTarget();
compositeTarget.AddTarget(customTarget);

var logger = new Logger(compositeTarget, config, LogCategory.System);
```

## 📝 Best Practices

1. **Sử dụng category phù hợp** cho từng layer
2. **Log level phù hợp** - không log quá nhiều hoặc quá ít
3. **Exception logging** - luôn log exception với context
4. **Performance logging** - sử dụng cho các operations quan trọng
5. **Security logging** - log tất cả security events
6. **Audit logging** - log tất cả thay đổi dữ liệu quan trọng

## 🐛 Troubleshooting

### Log không hiển thị
- Kiểm tra `Logging.MinimumLevel` trong App.config
- Kiểm tra `Logging.EnableConsole` và `Logging.EnableFile`
- Kiểm tra quyền ghi file trong thư mục Logs

### File log không được tạo
- Kiểm tra quyền ghi thư mục
- Kiểm tra `Logging.LogDirectory` path
- Kiểm tra `Logging.EnableFile` setting

### Console không hiển thị
- Kiểm tra `Logging.EnableConsole` setting
- Đảm bảo ứng dụng có console window
- Kiểm tra console output redirection

## 📚 Examples

Xem file `Examples/LoggingExamples.cs` để có ví dụ chi tiết về cách sử dụng tất cả tính năng của hệ thống logging.
