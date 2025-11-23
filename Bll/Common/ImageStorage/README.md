# Image Storage Service Layer

## Tổng Quan

Service layer này cung cấp abstraction cho việc lưu trữ hình ảnh/file, hỗ trợ nhiều loại storage:
- **NAS Storage** (Synology) - Khuyến nghị cho production
- **Local File System** - Fallback option
- **Cloud Storage** - Future implementation (Azure Blob, AWS S3)

## Cấu Trúc

```
Bll/Common/ImageStorage/
├── ImageCategory.cs                    # Enum định nghĩa danh mục hình ảnh
├── ImageStorageResult.cs               # Class kết quả lưu trữ
├── ImageStorageConfiguration.cs        # Configuration class
├── IImageStorageService.cs             # Interface chính
├── NASImageStorageService.cs           # Implementation cho NAS
├── LocalImageStorageService.cs         # Implementation cho Local
├── ImageStorageFactory.cs              # Factory pattern
└── README.md                           # Tài liệu này
```

## Cách Sử Dụng

### 1. Basic Usage

```csharp
using Bll.Common.ImageStorage;
using Logger.Interfaces;
using Logger.Configuration;

// Tạo service từ config
var logger = LoggerFactory.CreateLogger();
var storageService = ImageStorageFactory.CreateFromConfig(logger);

// Hoặc tạo với custom config
var config = ImageStorageConfiguration.LoadFromConfig();
config.StorageType = "NAS";
config.NASBasePath = "\\192.168.1.100\\ERP_Images";
var storageService = ImageStorageFactory.Create(config, logger);
```

### 2. Save Image

```csharp
// Đọc file ảnh
byte[] imageData = File.ReadAllBytes("path/to/image.jpg");

// Generate unique file name
string fileName = $"Product_{productId}_{Guid.NewGuid():N}_{DateTime.Now:yyyyMMddHHmmss}.jpg";

// Save to storage
var result = await storageService.SaveImageAsync(
    imageData: imageData,
    fileName: fileName,
    category: ImageCategory.Product,
    entityId: productId,
    generateThumbnail: true
);

if (result.Success)
{
    // Lưu metadata vào database
    productImage.RelativePath = result.RelativePath;
    productImage.FullPath = result.FullPath;
    productImage.FileName = result.FileName;
    productImage.FileSize = result.FileSize;
    productImage.Checksum = result.Checksum;
    productImage.ThumbnailRelativePath = result.ThumbnailRelativePath;
}
```

### 3. Get Image

```csharp
// Lấy ảnh từ storage
byte[] imageData = await storageService.GetImageAsync(relativePath);

// Lấy thumbnail
byte[] thumbnailData = await storageService.GetThumbnailAsync(relativePath);
```

### 4. Delete Image

```csharp
bool deleted = await storageService.DeleteImageAsync(relativePath);
```

### 5. Verify Image

```csharp
// Verify file integrity
bool isValid = await storageService.VerifyImageAsync(relativePath, checksum);
```

## Configuration

### App.config

```xml
<appSettings>
    <!-- Storage Type: NAS, Local -->
    <add key="ImageStorage.StorageType" value="NAS" />
    
    <!-- NAS Configuration -->
    <add key="ImageStorage.NAS.ServerName" value="\\192.168.1.100" />
    <add key="ImageStorage.NAS.ShareName" value="ERP_Images" />
    <add key="ImageStorage.NAS.BasePath" value="\\192.168.1.100\ERP_Images" />
    <add key="ImageStorage.NAS.Username" value="erp_user" />
    <add key="ImageStorage.NAS.Password" value="[Encrypted]" />
    <add key="ImageStorage.NAS.Protocol" value="SMB" />
    
    <!-- Path Configuration -->
    <add key="ImageStorage.Path.Products" value="Products" />
    <add key="ImageStorage.Path.StockInOut" value="StockInOut" />
    <add key="ImageStorage.Path.Company" value="Company" />
    <add key="ImageStorage.Path.Avatars" value="Avatars" />
    
    <!-- Thumbnail Configuration -->
    <add key="ImageStorage.Thumbnail.Enable" value="true" />
    <add key="ImageStorage.Thumbnail.Width" value="200" />
    <add key="ImageStorage.Thumbnail.Height" value="200" />
    
    <!-- File Management -->
    <add key="ImageStorage.MaxFileSize" value="10485760" /> <!-- 10MB -->
    <add key="ImageStorage.AllowedExtensions" value="jpg,jpeg,png,gif,bmp,webp" />
</appSettings>
```

## Image Categories

- **Product**: Hình ảnh sản phẩm
- **ProductVariant**: Hình ảnh biến thể sản phẩm
- **StockInOut**: Hình ảnh phiếu nhập/xuất kho
- **Company**: Logo công ty
- **Avatar**: Avatar người dùng/đối tác
- **Temp**: Thư mục tạm

## Directory Structure

Khi lưu file, service sẽ tự động tạo cấu trúc thư mục:

```
\\NAS_SERVER\ERP_Images\
├── Products\
│   ├── {ProductId}\
│   │   ├── {Year}\
│   │   │   ├── {Month}\
│   │   │   │   ├── {FileName}.jpg
│   │   │   │   └── {FileName}_thumb.jpg
├── StockInOut\
│   ├── {Year}\
│   │   ├── {Month}\
│   │   │   └── {FileName}.jpg
├── Company\
│   └── {CompanyId}_logo.jpg
└── Avatars\
    └── {UserId}_avatar.jpg
```

## Error Handling

Service sẽ trả về `ImageStorageResult` với:
- `Success`: true/false
- `ErrorMessage`: Thông báo lỗi nếu có
- `Exception`: Exception object (cho logging)

Luôn kiểm tra `result.Success` trước khi sử dụng các properties khác.

## Best Practices

1. **File Naming**: Sử dụng format unique để tránh conflict
   ```
   {EntityType}_{EntityId}_{ImageId}_{Timestamp}.{Extension}
   ```

2. **Error Handling**: Luôn wrap trong try-catch và log errors

3. **Checksum**: Luôn lưu checksum để verify integrity sau này

4. **Thumbnail**: Generate thumbnail cho images lớn để tăng performance

5. **Validation**: Validate file size và extension trước khi save

## Migration Notes

Khi migrate từ database storage sang NAS:
1. Giữ cả `ImageData` và `RelativePath` trong database trong thời gian transition
2. Sử dụng feature flag để switch giữa old/new implementation
3. Verify tất cả files đã migrate trước khi remove `ImageData` column

## Future Enhancements

- [ ] Cloud Storage support (Azure Blob, AWS S3)
- [ ] Image optimization pipeline
- [ ] CDN integration
- [ ] Watermarking support
- [ ] Image versioning

