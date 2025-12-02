# Hướng dẫn Thiết lập Icon cho VNS ERP 2025

## Tổng quan

Icon "VNS Logo - Website.ico" đã được thiết lập làm icon mặc định cho toàn bộ ứng dụng. Icon này sẽ được hiển thị trên:
- Taskbar khi ứng dụng chạy
- Title bar của tất cả các form
- File explorer (khi xem file .exe)
- Shortcut trên desktop/start menu

## Cấu hình hiện tại

### 1. Application Icon (File .exe)
- **File**: `VnsErp2025/VNS Logo - Website.ico`
- **Cấu hình**: Đã được set trong `VnsErp2025.csproj`:
  ```xml
  <PropertyGroup>
    <ApplicationIcon>VNS Logo - Website.ico</ApplicationIcon>
  </PropertyGroup>
  ```
- **Kết quả**: Icon này sẽ được embed vào file .exe khi build

### 2. Form Icons (Runtime)
- **Helper Class**: `Common.Utils.ApplicationIconHelper`
- **Tự động áp dụng**: Tất cả form được tạo qua `ApplicationSystemUtils.ShowOrActivateForm<T>()` sẽ tự động có icon
- **FormMain**: Icon được thiết lập trong `SetupFormProperties()`

## Cách hoạt động

### ApplicationIconHelper
Helper class này tự động load icon theo thứ tự ưu tiên:

1. **Từ Properties.Resources** (nếu đã thêm vào Resources.resx)
   - Tên: `AppIcon` hoặc `VNS_Logo_Website`
   
2. **Từ file system** (cùng thư mục với .exe)
   - Tên file: `VNS Logo - Website.ico`
   - Đường dẫn: `[ExeDirectory]\VNS Logo - Website.ico`

3. **Từ file system** (cùng thư mục với assembly)
   - Tìm trong nhiều đường dẫn có thể

### Tự động áp dụng cho form mới
Khi tạo form mới qua `ApplicationSystemUtils.ShowOrActivateForm<T>()`:
```csharp
T frm = new T();
ApplicationIconHelper.SetFormIcon(frm); // Tự động set icon
frm.Show();
```

## Thêm Icon vào Resources (Tùy chọn)

Nếu muốn embed icon vào assembly (không cần file .ico riêng):

1. Mở `VnsErp2025/Properties/Resources.resx` trong Visual Studio
2. Click "Add Resource" → "Add Existing File"
3. Chọn file `VNS Logo - Website.ico`
4. Đặt tên là `AppIcon` hoặc `VNS_Logo_Website`
5. Save và rebuild project

Sau đó icon sẽ được load từ Resources thay vì file system.

## Kiểm tra Icon

### Kiểm tra Application Icon
1. Build project (Release mode)
2. Xem file `.exe` trong `bin\Release\`
3. Icon sẽ hiển thị trong File Explorer

### Kiểm tra Form Icons
1. Chạy ứng dụng
2. Mở bất kỳ form nào
3. Kiểm tra icon trên title bar và taskbar

## Troubleshooting

### Icon không hiển thị trên form
- Kiểm tra file `VNS Logo - Website.ico` có trong output directory không
- Kiểm tra `ApplicationIconHelper.SetFormIcon()` có được gọi không
- Xem Debug output để biết lỗi load icon

### Icon không hiển thị trên .exe
- Đảm bảo `<ApplicationIcon>` trong `.csproj` trỏ đúng file
- Rebuild project (Clean + Build)
- Kiểm tra file .ico có hợp lệ không

### Icon bị mờ hoặc không đẹp
- Icon .ico nên có nhiều kích thước (16x16, 32x32, 48x48, 256x256)
- Sử dụng tool như IcoFX hoặc GIMP để tạo icon đa kích thước

## Cập nhật Icon

Nếu muốn thay đổi icon:

1. Thay thế file `VNS Logo - Website.ico` bằng icon mới
2. Đảm bảo tên file giống nhau (hoặc cập nhật trong code)
3. Rebuild project
4. Icon sẽ tự động được áp dụng cho tất cả form

## Lưu ý

- Icon file sẽ được copy vào output directory khi build
- Icon được cache trong `ApplicationIconHelper` để tăng performance
- Nếu icon không tìm thấy, form sẽ không có icon (không gây lỗi)

