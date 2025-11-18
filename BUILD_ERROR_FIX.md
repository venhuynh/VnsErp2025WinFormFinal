# Giải quyết lỗi MSB3021 - File Access Denied

## Vấn đề

Lỗi `MSB3021: Unable to copy file "obj\Debug\VnsErp2025.exe" to "bin\Debug\VnsErp2025.exe". Access to the path is denied.` thường xảy ra khi:

1. **Ứng dụng đang chạy**: File `.exe` đang được sử dụng bởi process `VnsErp2025.exe`
2. **Antivirus đang quét**: Antivirus đang lock file
3. **Visual Studio lock**: Visual Studio hoặc process khác đang giữ file
4. **File read-only**: File có thuộc tính read-only

## Giải pháp đã triển khai

### 1. Pre-build Event (Tự động)

Đã thêm **Pre-build event** vào `VnsErp2025.csproj` để tự động:
- Kill process `VnsErp2025.exe` trước khi build
- Unlock file `.exe` trong `bin\Debug` và `obj\Debug`
- Chạy tự động mỗi khi build, không cần can thiệp thủ công

**Lợi ích**: 
- ✅ Tự động xử lý, không cần nhớ chạy script
- ✅ Không làm gián đoạn workflow
- ✅ Hoạt động với mọi build (Debug/Release)

### 2. Script thủ công (Khi cần)

Nếu Pre-build event không hoạt động, có thể chạy script thủ công:

#### Option 1: Script tự động (không cần confirm)
```powershell
.\FixBuildLock.ps1 -Auto
```

#### Option 2: Script có confirm
```powershell
.\FixBuildLock.ps1
```

#### Option 3: Script silent (cho CI/CD)
```powershell
.\FixBuildLock.ps1 -Auto -Silent
```

## Cách sử dụng

### Trong Visual Studio

1. **Build bình thường**: Pre-build event sẽ tự động chạy
2. **Nếu vẫn lỗi**: 
   - Đóng tất cả instance của ứng dụng
   - Chạy `.\FixBuildLock.ps1 -Auto` từ PowerShell
   - Rebuild project

### Trong Command Line

```bash
# Build với MSBuild
msbuild VnsErp2025.sln /t:Rebuild
```

Pre-build event sẽ tự động chạy.

## Troubleshooting

### Nếu Pre-build event không hoạt động

1. **Kiểm tra Execution Policy**:
   ```powershell
   Get-ExecutionPolicy
   ```
   Nếu là `Restricted`, chạy:
   ```powershell
   Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
   ```

2. **Kiểm tra Visual Studio Build Output**:
   - Xem Output window trong Visual Studio
   - Tìm dòng "BeforeBuild" để xác nhận event đã chạy

3. **Chạy script thủ công**:
   ```powershell
   .\FixBuildLock.ps1 -Auto
   ```

### Nếu vẫn còn lỗi

1. **Đóng Visual Studio hoàn toàn**
2. **Kill tất cả process liên quan**:
   ```powershell
   Get-Process | Where-Object {$_.Path -like "*VnsErp2025*"} | Stop-Process -Force
   ```
3. **Xóa thư mục bin và obj**:
   ```powershell
   Remove-Item -Recurse -Force VnsErp2025\bin, VnsErp2025\obj
   ```
4. **Mở lại Visual Studio và Rebuild**

## Lưu ý

- Pre-build event chỉ chạy khi **build project VnsErp2025**
- Nếu build từ Solution, event vẫn chạy cho project này
- Script sử dụng `ContinueOnError="true"` nên không làm gián đoạn build nếu có lỗi nhỏ
- Thời gian chờ 300ms sau khi kill process để đảm bảo file được unlock

## Files liên quan

- `VnsErp2025/VnsErp2025.csproj` - Pre-build event được định nghĩa ở đây
- `FixBuildLock.ps1` - Script thủ công để fix lỗi
- `KillVnsErpProcess.ps1` - Script đơn giản để kill process (backup)

