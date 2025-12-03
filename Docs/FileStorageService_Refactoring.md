# Refactoring: Mở rộng ImageStorageService thành FileStorageService

## 📋 Tổng Quan

Đã refactor `IImageStorageService` để hỗ trợ lưu trữ nhiều loại file khác nhau (không chỉ hình ảnh), đồng thời giữ backward compatibility với code hiện tại.

## 🎯 Mục Tiêu

- Mở rộng khả năng lưu trữ từ chỉ hình ảnh sang nhiều loại file (PDF, DOCX, XLSX, ZIP, etc.)
- Giữ backward compatibility với `IImageStorageService` hiện tại
- Hỗ trợ lưu trữ chứng từ và tài liệu trên NAS
- Tái sử dụng infrastructure hiện có (NAS, Local storage)

## 🔄 Thay Đổi

### 1. Interface Mới

#### `IFileStorageService` (Mới)
- Interface tổng quát cho mọi loại file
- Method: `SaveFileAsync`, `GetFileAsync`, `DeleteFileAsync`, `FileExistsAsync`, `VerifyFileAsync`
- Hỗ trợ `FileCategory` để phân loại file

#### `IImageStorageService` (Cập nhật)
- Kế thừa từ `IFileStorageService`
- Giữ nguyên các method cũ: `SaveImageAsync`, `GetImageAsync`, `DeleteImageAsync`, etc.
- Backward compatible với code hiện tại

### 2. Enum Mới

#### `FileCategory` (Mới)
```csharp
public enum FileCategory
{
    Product,
    ProductVariant,
    StockInOut,
    Company,
    Avatar,
    Temp,
    StockInOutDocument,      // Mới
    BusinessPartnerDocument,  // Mới
    Document,                 // Mới
    Report                    // Mới
}
```

### 3. Class Mới

#### `FileStorageResult` (Mới)
- Tương tự `ImageStorageResult` nhưng có thêm:
  - `MimeType` - MIME type của file
  - `FileExtension` - Phần mở rộng file

### 4. Implementation Updates

#### `NASImageStorageService`
- Implement cả `IImageStorageService` và `IFileStorageService`
- Thêm method `SaveFileAsync` với `FileCategory`
- Thêm method `ValidateFile` (mở rộng từ `ValidateImage`) để hỗ trợ nhiều loại file
- Thêm method `GetMimeType` để xác định MIME type
- Overload `GenerateRelativePath` cho cả `ImageCategory` và `FileCategory`

#### `LocalImageStorageService`
- Tương tự `NASImageStorageService`
- Implement cả `IImageStorageService` và `IFileStorageService`

## 📁 Cấu Trúc Thư Mục Trên NAS

### Documents (Mới)
```
\\NAS_SERVER\ERP_Images\
├── Documents\
│   ├── StockInOut\
│   │   └── {Year}\{Month}\{FileName}
│   ├── BusinessPartner\
│   │   └── {BusinessPartnerId}\{Year}\{Month}\{FileName}
│   ├── General\
│   │   └── {Year}\{Month}\{FileName}
│   └── Reports\
│       └── {Year}\{Month}\{FileName}
├── Products\... (giữ nguyên)
├── StockInOut\... (giữ nguyên cho images)
└── ...
```

## 🔧 Cách Sử Dụng

### Lưu File (Tổng quát)
```csharp
var fileStorage = ImageStorageFactory.CreateFromConfig(_logger) as IFileStorageService;

var result = await fileStorage.SaveFileAsync(
    fileData: fileBytes,
    fileName: "invoice.pdf",
    category: FileCategory.StockInOutDocument,
    entityId: stockInOutMasterId,
    generateThumbnail: false
);
```

### Lưu Image (Backward Compatible)
```csharp
var imageStorage = ImageStorageFactory.CreateFromConfig(_logger);

var result = await imageStorage.SaveImageAsync(
    imageData: imageBytes,
    fileName: "photo.jpg",
    category: ImageCategory.StockInOut,
    entityId: stockInOutMasterId,
    generateThumbnail: true
);
```

### Lưu Document (Qua StockInOutDocumentBll)
```csharp
var documentBll = new StockInOutDocumentBll();

var document = await documentBll.SaveDocumentFromFileAsync(
    stockInOutMasterId: stockInOutMasterId,
    businessPartnerId: null,
    documentFilePath: @"C:\Temp\invoice.pdf",
    documentType: (int)DocumentTypeEnum.Invoice,
    documentCategory: (int)DocumentCategoryEnum.Financial,
    documentNumber: "INV-2025-001",
    documentDate: DateTime.Now
);
```

## ✅ Backward Compatibility

- Tất cả code hiện tại sử dụng `IImageStorageService` vẫn hoạt động bình thường
- `SaveImageAsync`, `GetImageAsync`, `DeleteImageAsync` vẫn hoạt động như cũ
- Không cần thay đổi code hiện tại

## 📝 Files Đã Tạo/Cập Nhật

### Mới
- `IFileStorageService.cs` - Interface tổng quát cho file storage
- `FileCategory.cs` - Enum phân loại file
- `FileStorageResult.cs` - Kết quả lưu trữ file

### Cập nhật
- `IImageStorageService.cs` - Kế thừa từ `IFileStorageService`
- `NASImageStorageService.cs` - Implement `IFileStorageService`
- `LocalImageStorageService.cs` - Implement `IFileStorageService`

## 🎉 Lợi Ích

1. **Linh hoạt**: Hỗ trợ nhiều loại file, không chỉ hình ảnh
2. **Tái sử dụng**: Sử dụng chung infrastructure (NAS, Local storage)
3. **Backward Compatible**: Không phá vỡ code hiện tại
4. **Mở rộng dễ dàng**: Có thể thêm loại file mới vào `FileCategory`
5. **Type Safety**: Sử dụng enum và interface rõ ràng

## 🔮 Tương Lai

- Có thể tách `IFileStorageService` thành service riêng nếu cần
- Có thể thêm các loại storage khác (Cloud, S3, etc.)
- Có thể thêm compression, encryption cho documents

