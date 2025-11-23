# Hướng Dẫn Cấu Hình Image Storage (NAS)

## 📋 Tổng Quan

File `App.config` đã được cập nhật với các settings cho Image Storage Service. Bạn cần cập nhật các giá trị sau theo cấu hình NAS thực tế của mình.

## ⚙️ Các Settings Cần Cấu Hình

### 1. Storage Type

```xml
<add key="ImageStorage.StorageType" value="NAS" />
```

**Giá trị:**
- `NAS` - Sử dụng NAS Synology (khuyến nghị)
- `Local` - Sử dụng local file system (fallback)

### 2. NAS Server Configuration

```xml
<!-- Địa chỉ IP hoặc hostname của NAS -->
<add key="ImageStorage.NAS.ServerName" value="\\192.168.1.100" />

<!-- Tên share folder trên NAS -->
<add key="ImageStorage.NAS.ShareName" value="ERP_Images" />

<!-- Đường dẫn đầy đủ (sẽ tự động build nếu không set) -->
<add key="ImageStorage.NAS.BasePath" value="\\192.168.1.100\ERP_Images" />
```

**Cách xác định:**
1. Mở File Explorer trên Windows
2. Map network drive đến NAS share
3. Copy UNC path (ví dụ: `\\192.168.1.100\ERP_Images`)

### 3. NAS Authentication

```xml
<!-- Username để kết nối NAS -->
<add key="ImageStorage.NAS.Username" value="erp_user" />

<!-- Password (nên được encrypt trong production) -->
<add key="ImageStorage.NAS.Password" value="your_password_here" />
```

**Lưu ý:**
- Tạo user riêng cho ứng dụng trên NAS (không dùng admin)
- Chỉ cấp quyền Read/Write cho share folder `ERP_Images`
- Trong production, nên encrypt password

### 4. NAS Protocol

```xml
<add key="ImageStorage.NAS.Protocol" value="SMB" />
```

**Giá trị:**
- `SMB` - SMB/CIFS (khuyến nghị cho Windows)
- `NFS` - Network File System (Linux/Unix)
- `FTP` - FTP protocol

### 5. Network Settings

```xml
<!-- Timeout kết nối (seconds) -->
<add key="ImageStorage.NAS.ConnectionTimeout" value="30" />

<!-- Số lần retry khi kết nối thất bại -->
<add key="ImageStorage.NAS.RetryAttempts" value="3" />
```

## 📁 Path Configuration

Các đường dẫn này là relative paths trong NAS share:

```xml
<add key="ImageStorage.Path.Products" value="Products" />
<add key="ImageStorage.Path.StockInOut" value="StockInOut" />
<add key="ImageStorage.Path.Company" value="Company" />
<add key="ImageStorage.Path.Avatars" value="Avatars" />
<add key="ImageStorage.Path.Temp" value="Temp" />
```

**Cấu trúc thư mục sẽ được tạo tự động:**
```
\\NAS_SERVER\ERP_Images\
├── Products\          (từ ImageStorage.Path.Products)
├── StockInOut\        (từ ImageStorage.Path.StockInOut)
├── Company\           (từ ImageStorage.Path.Company)
├── Avatars\           (từ ImageStorage.Path.Avatars)
└── Temp\              (từ ImageStorage.Path.Temp)
```

## 🖼️ Thumbnail Configuration

```xml
<!-- Bật/tắt tạo thumbnail tự động -->
<add key="ImageStorage.Thumbnail.Enable" value="true" />

<!-- Kích thước thumbnail (pixels) -->
<add key="ImageStorage.Thumbnail.Width" value="200" />
<add key="ImageStorage.Thumbnail.Height" value="200" />

<!-- Chất lượng thumbnail (1-100) -->
<add key="ImageStorage.Thumbnail.Quality" value="80" />
```

## 🔧 Image Processing

```xml
<!-- Bật/tắt nén hình ảnh -->
<add key="ImageStorage.Compression.Enable" value="true" />

<!-- Chất lượng nén (1-100, cao hơn = file lớn hơn nhưng chất lượng tốt hơn) -->
<add key="ImageStorage.Compression.Quality" value="80" />

<!-- Kích thước file tối đa (bytes) -->
<add key="ImageStorage.MaxFileSize" value="10485760" />
<!-- 10MB = 10485760 bytes -->
<!-- 5MB = 5242880 bytes -->
<!-- 20MB = 20971520 bytes -->

<!-- Các extension được phép -->
<add key="ImageStorage.AllowedExtensions" value="jpg,jpeg,png,gif,bmp,webp" />
```

## 🔒 File Management

```xml
<!-- Bật/tắt verify file integrity (checksum) -->
<add key="ImageStorage.Verification.Enable" value="true" />

<!-- Khoảng thời gian verify file (hours) -->
<add key="ImageStorage.Verification.IntervalHours" value="24" />

<!-- Bật/tắt auto cleanup orphaned files -->
<add key="ImageStorage.Cleanup.Enable" value="true" />

<!-- Số ngày giữ lại orphaned files trước khi xóa -->
<add key="ImageStorage.Cleanup.OrphanedFileRetentionDays" value="30" />
```

## ⚡ Performance Settings

```xml
<!-- Bật/tắt cache -->
<add key="ImageStorage.Cache.Enable" value="true" />

<!-- Kích thước cache (MB) -->
<add key="ImageStorage.Cache.SizeMB" value="500" />

<!-- Bật/tắt async processing -->
<add key="ImageStorage.Async.Enable" value="true" />

<!-- Số lượng operations đồng thời tối đa -->
<add key="ImageStorage.Async.MaxConcurrent" value="10" />
```

## 🔐 Security Best Practices

### 1. Password Encryption

Trong production, nên encrypt password:

```xml
<!-- Option 1: Sử dụng encrypted value -->
<add key="ImageStorage.NAS.Password" value="[Encrypted:Base64EncodedPassword]" />

<!-- Option 2: Store trong secure config file -->
<!-- Option 3: Use Windows Credential Manager -->
```

### 2. NAS User Permissions

Trên NAS Synology:
- Tạo user `erp_user` riêng
- Chỉ cấp quyền Read/Write cho share `ERP_Images`
- Không cấp quyền admin
- Enable quota nếu cần

### 3. Network Security

- Sử dụng IP tĩnh cho NAS
- Cấu hình firewall trên NAS
- Chỉ cho phép kết nối từ application server
- Sử dụng VPN nếu cần remote access

## ✅ Checklist Cấu Hình

### Trên NAS Synology

- [ ] NAS đã được cài đặt và cấu hình
- [ ] Đã tạo share folder `ERP_Images`
- [ ] Đã tạo user `erp_user` với quyền Read/Write
- [ ] Đã cấu hình RAID (RAID 5 hoặc RAID 6)
- [ ] Đã enable snapshot (7 days retention)
- [ ] Đã test kết nối từ application server
- [ ] NAS có IP tĩnh

### Trên Application Server

- [ ] Đã cập nhật `App.config` với NAS settings
- [ ] Đã test kết nối đến NAS share
- [ ] Đã verify có quyền Read/Write
- [ ] Đã test tạo file trên NAS
- [ ] Đã test đọc file từ NAS

### Trong Code

- [ ] Đã import `Bll.Common.ImageStorage`
- [ ] Đã tạo `IImageStorageService` instance
- [ ] Đã test save image
- [ ] Đã test get image
- [ ] Đã test delete image

## 🧪 Test Configuration

### Test 1: Connection Test

```csharp
var config = ImageStorageConfiguration.LoadFromConfig();
var logger = LoggerFactory.CreateLogger();
var storageService = ImageStorageFactory.Create(config, logger);

// Test connection
bool exists = await storageService.ImageExistsAsync("test.txt");
```

### Test 2: Save Test

```csharp
// Create test image
byte[] testImage = CreateTestImage();

// Save to NAS
var result = await storageService.SaveImageAsync(
    testImage,
    "test_image.jpg",
    ImageCategory.Temp
);

if (result.Success)
{
    Console.WriteLine($"Saved to: {result.FullPath}");
}
```

### Test 3: Read Test

```csharp
// Read from NAS
byte[] imageData = await storageService.GetImageAsync(result.RelativePath);
Console.WriteLine($"Read {imageData.Length} bytes");
```

## 🐛 Troubleshooting

### Lỗi: "Không có quyền truy cập NAS"

**Nguyên nhân:**
- Username/password sai
- User không có quyền trên share folder
- NAS firewall block connection

**Giải pháp:**
1. Verify username/password trên NAS
2. Check user permissions trên share folder
3. Check NAS firewall settings
4. Test kết nối từ File Explorer trước

### Lỗi: "Network path not found"

**Nguyên nhân:**
- NAS server name/IP sai
- NAS không accessible từ application server
- Network issue

**Giải pháp:**
1. Ping NAS IP từ application server
2. Test map network drive từ File Explorer
3. Verify NAS server name format: `\\192.168.1.100` hoặc `\\NAS_HOSTNAME`

### Lỗi: "Connection timeout"

**Nguyên nhân:**
- NAS quá tải
- Network latency cao
- NAS firewall block

**Giải pháp:**
1. Tăng `ConnectionTimeout` value
2. Check NAS performance
3. Verify network connection

## 📝 Notes

- Tất cả các settings có default values, có thể bỏ qua nếu không cần customize
- Thay đổi settings cần restart application để có hiệu lực
- Backup App.config trước khi thay đổi
- Test configuration trong môi trường dev trước khi deploy production

## 🔗 Related Documents

- [Image Storage Refactoring Architecture](../Docs/ImageStorageRefactoringArchitecture.md)
- [Image Storage Service README](../Bll/Common/ImageStorage/README.md)

