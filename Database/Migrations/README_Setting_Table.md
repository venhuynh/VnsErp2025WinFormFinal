# Bảng Setting - Cấu hình hệ thống

## Tổng quan

Bảng `Setting` được thiết kế để lưu trữ tất cả các cấu hình hệ thống, bao gồm:
- Cấu hình NAS Storage
- Cấu hình Image Storage
- Các cấu hình hệ thống khác

## Cấu trúc bảng

### Các cột chính

| Cột | Kiểu dữ liệu | Mô tả |
|-----|--------------|-------|
| `SettingId` | INT IDENTITY | Primary key, tự động tăng |
| `Category` | NVARCHAR(100) | Phân loại setting (NAS, ImageStorage, System, etc.) |
| `SettingKey` | NVARCHAR(200) | Key của setting (ví dụ: NAS.ServerName, ImageStorage.Thumbnail.Width) |
| `SettingValue` | NVARCHAR(MAX) | Giá trị của setting (có thể là string, JSON, hoặc serialized object) |
| `ValueType` | NVARCHAR(50) | Loại dữ liệu: String, Int, Bool, DateTime, Decimal, JSON |
| `Description` | NVARCHAR(500) | Mô tả về setting này |
| `IsEncrypted` | BIT | Đánh dấu giá trị có được mã hóa không (cho password, etc.) |
| `IsSystem` | BIT | Đánh dấu là system setting (không cho user sửa) |
| `IsActive` | BIT | Có đang sử dụng không |
| `GroupName` | NVARCHAR(100) | Nhóm settings (NAS, Thumbnail, Compression, etc.) |
| `DisplayOrder` | INT | Thứ tự hiển thị trong UI |
| `CreatedDate` | DATETIME | Ngày tạo |
| `CreatedBy` | NVARCHAR(100) | Người tạo |
| `UpdatedDate` | DATETIME | Ngày cập nhật |
| `UpdatedBy` | NVARCHAR(100) | Người cập nhật |

### Constraints

- **Primary Key**: `SettingId`
- **Unique Constraint**: `Category` + `SettingKey` (không được trùng lặp)

### Indexes

- `IX_Setting_Category`: Index trên `Category` để tìm kiếm nhanh theo category
- `IX_Setting_Category_Group`: Index trên `Category` và `GroupName` để nhóm settings
- `IX_Setting_IsActive`: Index trên `IsActive` để lọc settings đang hoạt động

## Cấu trúc dữ liệu mẫu

### Category: NAS

Các settings liên quan đến cấu hình NAS Storage:

| SettingKey | ValueType | Mô tả | GroupName |
|------------|-----------|-------|-----------|
| `StorageType` | String | Loại storage: NAS, Local, Cloud | General |
| `ServerName` | String | Tên server NAS (ví dụ: \\192.168.1.100) | Connection |
| `ShareName` | String | Tên share folder trên NAS | Connection |
| `BasePath` | String | Đường dẫn đầy đủ đến NAS share | Connection |
| `Username` | String | Username để kết nối NAS | Connection |
| `Password` | String | Password để kết nối NAS (được mã hóa) | Connection |
| `Protocol` | String | Protocol: SMB, NFS, FTP | Connection |
| `ConnectionTimeout` | Int | Connection timeout (seconds) | Connection |
| `RetryAttempts` | Int | Số lần retry khi kết nối thất bại | Connection |

### Category: ImageStorage

Các settings liên quan đến cấu hình Image Storage:

#### Group: Paths
- `Path.Products`: Đường dẫn cho hình ảnh sản phẩm
- `Path.StockInOut`: Đường dẫn cho hình ảnh phiếu nhập/xuất
- `Path.Company`: Đường dẫn cho logo công ty
- `Path.Avatars`: Đường dẫn cho avatar
- `Path.Temp`: Đường dẫn cho file tạm

#### Group: Thumbnail
- `Thumbnail.Enable`: Bật/tắt tạo thumbnail tự động (Bool)
- `Thumbnail.Width`: Chiều rộng thumbnail (pixels) (Int)
- `Thumbnail.Height`: Chiều cao thumbnail (pixels) (Int)
- `Thumbnail.Quality`: Chất lượng thumbnail (1-100) (Int)

#### Group: Processing
- `Compression.Enable`: Bật/tắt nén hình ảnh (Bool)
- `Compression.Quality`: Chất lượng nén (1-100) (Int)
- `MaxFileSize`: Kích thước file tối đa (bytes) (Int)
- `AllowedExtensions`: Các extension được phép (String, phân cách bằng dấu phẩy)

#### Group: Management
- `Verification.Enable`: Bật/tắt verify file integrity (Bool)
- `Verification.IntervalHours`: Khoảng thời gian verify file (hours) (Int)
- `Cleanup.Enable`: Bật/tắt auto cleanup orphaned files (Bool)
- `Cleanup.OrphanedFileRetentionDays`: Số ngày giữ lại orphaned files (Int)

#### Group: Performance
- `Cache.Enable`: Bật/tắt cache (Bool)
- `Cache.SizeMB`: Kích thước cache (MB) (Int)
- `Async.Enable`: Bật/tắt async processing (Bool)
- `Async.MaxConcurrent`: Số lượng operations đồng thời tối đa (Int)

## Cách sử dụng

### 1. Đọc setting

```sql
-- Đọc một setting cụ thể
SELECT SettingValue 
FROM [dbo].[Setting]
WHERE Category = 'NAS' AND SettingKey = 'ServerName'

-- Đọc tất cả settings của một category
SELECT SettingKey, SettingValue, ValueType, Description
FROM [dbo].[Setting]
WHERE Category = 'NAS' AND IsActive = 1
ORDER BY DisplayOrder

-- Đọc settings theo group
SELECT SettingKey, SettingValue, ValueType
FROM [dbo].[Setting]
WHERE Category = 'ImageStorage' 
  AND GroupName = 'Thumbnail'
  AND IsActive = 1
ORDER BY DisplayOrder
```

### 2. Cập nhật setting

```sql
-- Cập nhật một setting
UPDATE [dbo].[Setting]
SET SettingValue = '\\192.168.1.200',
    UpdatedDate = GETDATE(),
    UpdatedBy = 'admin'
WHERE Category = 'NAS' AND SettingKey = 'ServerName'

-- Cập nhật password (cần mã hóa trước)
UPDATE [dbo].[Setting]
SET SettingValue = 'encrypted_password_here',
    UpdatedDate = GETDATE(),
    UpdatedBy = 'admin'
WHERE Category = 'NAS' AND SettingKey = 'Password'
```

### 3. Thêm setting mới

```sql
INSERT INTO [dbo].[Setting] 
    (Category, SettingKey, SettingValue, ValueType, Description, GroupName, DisplayOrder, IsSystem)
VALUES 
    ('NAS', 'NewSetting', 'value', 'String', 'Mô tả', 'General', 100, 0)
```

## Lưu ý bảo mật

1. **Mã hóa password**: Tất cả các setting có `IsEncrypted = 1` (như Password) phải được mã hóa trước khi lưu vào database.

2. **System Settings**: Các setting có `IsSystem = 1` không nên cho phép user thông thường chỉnh sửa.

3. **Validation**: Nên validate giá trị theo `ValueType` trước khi lưu:
   - `Int`: Phải là số nguyên
   - `Bool`: Phải là 'true' hoặc 'false'
   - `DateTime`: Phải là định dạng datetime hợp lệ
   - `Decimal`: Phải là số thập phân

## Migration từ App.config/XML

Khi migrate từ App.config hoặc XML file sang bảng Setting:

1. Đọc tất cả settings từ App.config/XML
2. Insert vào bảng Setting với Category và SettingKey tương ứng
3. Đảm bảo password được mã hóa và đánh dấu `IsEncrypted = 1`
4. Cập nhật code để đọc từ database thay vì App.config/XML

## Ví dụ sử dụng trong code

```csharp
// Đọc setting từ database
var serverName = SettingRepository.GetValue("NAS", "ServerName", "");

// Đọc setting với type conversion
var timeout = int.Parse(SettingRepository.GetValue("NAS", "ConnectionTimeout", "30"));

// Đọc và giải mã password
var encryptedPassword = SettingRepository.GetValue("NAS", "Password", "");
var password = VntaCrypto.Decrypt(encryptedPassword);

// Cập nhật setting
SettingRepository.SetValue("NAS", "ServerName", "\\192.168.1.200", "admin");
```

## Mở rộng

Bảng Setting có thể được mở rộng để lưu các cấu hình khác:
- System settings (timeout, retry, etc.)
- Email configuration
- SMS configuration
- Report settings
- Integration settings (API keys, endpoints, etc.)
