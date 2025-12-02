# Chiến lược Quản lý Phiên bản cho VNS ERP 2025

## 1. Tổng quan

Ứng dụng VNS ERP 2025 là ứng dụng nội bộ WinForms (.NET Framework 4.8) cần được cập nhật thường xuyên. Tài liệu này đề xuất giải pháp quản lý phiên bản và tự động cập nhật.

## 2. Yêu cầu

- ✅ Cập nhật tự động hoặc bán tự động
- ✅ Kiểm tra phiên bản khi khởi động
- ✅ Tải và cài đặt bản cập nhật
- ✅ Rollback khi cập nhật lỗi
- ✅ Thông báo cho người dùng về bản cập nhật
- ✅ Lịch sử phiên bản
- ✅ Hỗ trợ cập nhật delta (chỉ tải phần thay đổi)

## 3. Giải pháp đề xuất

### 3.1. Kiến trúc tổng thể

```
┌─────────────────┐
│  VNS ERP 2025   │
│   (Client)      │
└────────┬────────┘
         │
         │ 1. Check Version
         ▼
┌─────────────────┐
│  Update Server  │
│  (HTTP/HTTPS)   │
└────────┬────────┘
         │
         │ 2. Version Info
         │ 3. Download Update
         ▼
┌─────────────────┐
│  Update Files   │
│  (Shared Folder)│
└─────────────────┘
```

### 3.2. Các phương án triển khai

#### **Phương án 1: ClickOnce Deployment (Khuyến nghị cho môi trường nội bộ)**

**Ưu điểm:**
- ✅ Tích hợp sẵn với Visual Studio
- ✅ Tự động kiểm tra và cập nhật
- ✅ Rollback tự động
- ✅ Dễ triển khai
- ✅ Hỗ trợ delta updates

**Nhược điểm:**
- ❌ Yêu cầu IIS hoặc file share
- ❌ Cần cấu hình certificate cho HTTPS
- ❌ Giới hạn một số tùy chỉnh

**Triển khai:**
1. Cấu hình ClickOnce trong Visual Studio
2. Publish lên file share hoặc IIS
3. Người dùng cài đặt một lần từ URL
4. Ứng dụng tự động kiểm tra và cập nhật khi khởi động

#### **Phương án 2: Custom Auto-Updater (Linh hoạt nhất)**

**Ưu điểm:**
- ✅ Kiểm soát hoàn toàn quy trình
- ✅ Tùy chỉnh UI/UX
- ✅ Hỗ trợ nhiều nguồn cập nhật
- ✅ Có thể tích hợp với hệ thống hiện có

**Nhược điểm:**
- ❌ Cần phát triển và bảo trì
- ❌ Phức tạp hơn ClickOnce

**Triển khai:**
- Sử dụng thư viện như Squirrel hoặc tự phát triển
- Tạo Update Service để quản lý phiên bản
- Tạo Update Client để kiểm tra và tải cập nhật

#### **Phương án 3: Hybrid (ClickOnce + Custom Check)**

**Ưu điểm:**
- ✅ Kết hợp ưu điểm của cả hai
- ✅ Kiểm soát tốt hơn ClickOnce thuần
- ✅ Dễ triển khai hơn Custom hoàn toàn

**Nhược điểm:**
- ❌ Cần bảo trì cả hai phần

## 4. Giải pháp được đề xuất: Custom Auto-Updater

### 4.1. Lý do chọn

- Ứng dụng nội bộ cần kiểm soát tốt quy trình cập nhật
- Có thể tích hợp với hệ thống quản lý hiện có
- Linh hoạt trong việc thông báo và xử lý lỗi
- Có thể mở rộng trong tương lai

### 4.2. Kiến trúc chi tiết

```
┌─────────────────────────────────────┐
│         VNS ERP 2025 Client         │
│                                     │
│  ┌───────────────────────────────┐ │
│  │   ApplicationVersionService   │ │
│  │   - CheckVersion()            │ │
│  │   - DownloadUpdate()          │ │
│  │   - InstallUpdate()           │ │
│  └───────────────────────────────┘ │
│                                     │
│  ┌───────────────────────────────┐ │
│  │      UpdateChecker             │ │
│  │   - Check on startup           │ │
│  │   - Background check            │ │
│  └───────────────────────────────┘ │
└─────────────────────────────────────┘
              │
              │ HTTP/HTTPS
              ▼
┌─────────────────────────────────────┐
│         Update Server                │
│                                     │
│  ┌───────────────────────────────┐ │
│  │   Version API                  │ │
│  │   GET /api/version/current     │ │
│  │   GET /api/version/{version}   │ │
│  └───────────────────────────────┘ │
│                                     │
│  ┌───────────────────────────────┐ │
│  │   Update Files Storage         │ │
│  │   - Installer files            │ │
│  │   - Delta patches              │ │
│  │   - Release notes               │ │
│  └───────────────────────────────┘ │
└─────────────────────────────────────┘
```

### 4.3. Quy trình cập nhật

```
1. Application Startup
   │
   ├─► Check Version (Background)
   │   │
   │   ├─► Current Version: 1.0.0
   │   └─► Server Version: 1.0.1
   │
   ├─► Show Update Notification
   │   │
   │   ├─► User chooses: Update Now
   │   │   │
   │   │   ├─► Download Update Package
   │   │   ├─► Verify Package Integrity
   │   │   ├─► Backup Current Version
   │   │   ├─► Install Update
   │   │   └─► Restart Application
   │   │
   │   └─► User chooses: Update Later
   │       │
   │       └─► Continue with current version
   │
   └─► Application Running
```

## 5. Cấu trúc dữ liệu

### 5.1. Version Information (JSON)

```json
{
  "version": "1.0.1",
  "releaseDate": "2025-01-20T10:00:00Z",
  "isMandatory": false,
  "minSupportedVersion": "1.0.0",
  "downloadUrl": "https://updates.vns.local/v1.0.1/VnsErp2025_1.0.1.exe",
  "fileSize": 52428800,
  "checksum": "SHA256:abc123...",
  "releaseNotes": {
    "en": "Bug fixes and performance improvements",
    "vi": "Sửa lỗi và cải thiện hiệu suất"
  },
  "changes": [
    "Fixed issue with inventory management",
    "Improved database connection handling",
    "Added new features to stock in/out"
  ]
}
```

### 5.2. Update Configuration

```xml
<appSettings>
  <add key="UpdateServerUrl" value="https://updates.vns.local/api" />
  <add key="UpdateCheckInterval" value="24" /> <!-- hours -->
  <add key="AutoUpdateEnabled" value="true" />
  <add key="UpdateDownloadPath" value="%TEMP%\VnsErp2025\Updates" />
  <add key="BackupPath" value="%APPDATA%\VnsErp2025\Backups" />
</appSettings>
```

## 6. Triển khai

### 6.1. Các thành phần cần phát triển

1. **ApplicationVersionService**
   - Kiểm tra phiên bản từ server
   - Tải bản cập nhật
   - Cài đặt bản cập nhật
   - Quản lý backup và rollback

2. **UpdateChecker**
   - Kiểm tra định kỳ
   - Kiểm tra khi khởi động
   - Hiển thị thông báo cập nhật

3. **UpdateServer API** (Optional - có thể dùng file share)
   - Endpoint trả về thông tin phiên bản
   - Endpoint tải file cập nhật
   - Quản lý lịch sử phiên bản

4. **Update UI**
   - Form thông báo cập nhật
   - Progress bar khi tải
   - Release notes viewer

### 6.2. Cấu hình Version trong Assembly

```csharp
// AssemblyInfo.cs
[assembly: AssemblyVersion("1.0.1.0")]
[assembly: AssemblyFileVersion("1.0.1.0")]
[assembly: AssemblyInformationalVersion("1.0.1-beta")]
```

### 6.3. Versioning Strategy

**Format:** `Major.Minor.Build.Revision`

- **Major**: Thay đổi lớn, breaking changes
- **Minor**: Tính năng mới, backward compatible
- **Build**: Bug fixes, improvements
- **Revision**: Hotfixes, patches

**Ví dụ:**
- `1.0.0.0` - Release đầu tiên
- `1.0.1.0` - Bug fixes
- `1.1.0.0` - Tính năng mới
- `2.0.0.0` - Major update

## 7. Bảo mật

- ✅ Verify checksum của file cập nhật
- ✅ Sử dụng HTTPS cho download
- ✅ Code signing cho installer
- ✅ Kiểm tra digital signature
- ✅ Validate version information từ server

## 8. Rollback Strategy

1. **Backup trước khi cập nhật**
   - Copy toàn bộ thư mục ứng dụng
   - Lưu registry settings
   - Lưu user data

2. **Rollback tự động**
   - Nếu cập nhật thất bại
   - Nếu ứng dụng không khởi động được
   - Nếu có lỗi nghiêm trọng

3. **Rollback thủ công**
   - Menu option để rollback
   - Chọn phiên bản cần rollback

## 9. Testing Strategy

- ✅ Test trên môi trường dev trước
- ✅ Test với nhiều phiên bản khác nhau
- ✅ Test rollback
- ✅ Test với network issues
- ✅ Test với insufficient permissions

## 10. Deployment Process

1. **Build và Package**
   ```
   - Build Release version
   - Update AssemblyInfo version
   - Create installer
   - Generate checksum
   ```

2. **Upload to Server**
   ```
   - Upload installer to update server
   - Update version information API
   - Test download link
   ```

3. **Notify Users**
   ```
   - Users will be notified on next startup
   - Or send email notification
   ```

## 11. Monitoring và Logging

- Log tất cả các thao tác cập nhật
- Track số lượng user đã cập nhật
- Monitor lỗi cập nhật
- Thống kê phiên bản đang sử dụng

## 12. Tài liệu tham khảo

- [ClickOnce Deployment](https://docs.microsoft.com/en-us/visualstudio/deployment/clickonce-security-and-deployment)
- [Squirrel.Windows](https://github.com/Squirrel/Squirrel.Windows)
- [AutoUpdater.NET](https://github.com/ravibpatel/AutoUpdater.NET)

