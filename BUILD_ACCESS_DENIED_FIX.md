# Hướng dẫn khắc phục lỗi "Access to the path is denied" khi build VnsErp2025

## Nguyên nhân của lỗi

Lỗi **"Unable to copy file 'obj\Debug\VnsErp2025.exe' to 'bin\Debug\VnsErp2025.exe'. Access to the path is denied"** xảy ra khi:

1. **File VnsErp2025.exe đang được chạy** - Process vẫn đang active
2. **Visual Studio debugger đang giữ file handle** 
3. **Windows Explorer đang preview file exe**
4. **Antivirus đang scan file**
5. **File bị lock bởi system process khác**

## Giải pháp tự động

### 1. Script tự động (Khuyến nghị)

Chạy script `CleanBuildFix.ps1` khi gặp lỗi:

```powershell
# Cleanup thông thường
.\CleanBuildFix.ps1

# Cleanup mạnh mẽ (force kill VS processes)
.\CleanBuildFix.ps1 -Force
```

### 2. Script có sẵn trong Pre-build Event

Project đã được cấu hình để tự động chạy `KillVnsErpProcess.ps1` trước mỗi lần build.

## Giải pháp thủ công

### Nhanh chóng:
```powershell
# Kill process VnsErp2025 nếu đang chạy
Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue | Stop-Process -Force

# Xóa output directories
Remove-Item "VnsErp2025\bin\Debug" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item "VnsErp2025\obj\Debug" -Recurse -Force -ErrorAction SilentlyContinue
```

### Chi tiết hơn:

1. **Đóng tất cả instance của VnsErp2025**
   - Kiểm tra Task Manager
   - Kill process qua PowerShell

2. **Restart Visual Studio** 
   - Close VS completely
   - Kill devenv.exe process nếu cần
   - Restart VS

3. **Clean Solution**
   - Build → Clean Solution
   - Hoặc delete manually bin/obj folders

4. **Check file permissions**
   - Đảm bảo không có readonly files
   - Check antivirus exclusions

## Ngăn ngừa vấn đề

### 1. Thiết lập Visual Studio
- **Tools → Options → Debugging → General**
  - ✅ Enable "Suppress JIT optimization"
  - ✅ Uncheck "Enable Diagnostic Tools while debugging"

### 2. Thiết lập Project
- Đã thêm `UseVSHostingProcess = false` vào project
- Pre-build event sẽ tự động cleanup

### 3. Antivirus exclusions
Thêm exclusions cho:
- `C:\Users\Admin\source\Workspaces\2025\VNS_ERP_2025_FINAL\VnsErp2025\`
- `%TEMP%\VBCSCompiler\`
- Visual Studio temp folders

### 4. Best practices khi development
- **Stop debugging trước khi build lại**
- **Không chạy app từ Windows Explorer khi đang develop**
- **Close app hoàn toàn trước khi build**
- **Sử dụng Clean Solution thường xuyên**

## Troubleshooting Commands

```powershell
# Kiểm tra processes đang chạy
Get-Process -Name "*VnsErp*", "*devenv*", "*MSBuild*"

# Kiểm tra file locks
handle.exe VnsErp2025.exe  # Cần SysInternals Handle tool

# Force unlock files
Remove-Item "VnsErp2025\bin\Debug\VnsErp2025.exe" -Force
Remove-Item "VnsErp2025\obj\Debug\VnsErp2025.exe" -Force

# Reset file attributes 
Get-ChildItem -Recurse | ForEach-Object { $_.Attributes = $_.Attributes -band (-bnot "ReadOnly") }
```

## Scripts có sẵn trong project

1. **`KillVnsErpProcess.ps1`** - Chạy tự động trước mỗi build
2. **`CleanBuildFix.ps1`** - Chạy thủ công khi gặp vấn đề
3. **`QuickBuildFix.ps1`** - Quick fix cho build errors

Chạy script phù hợp tùy theo mức độ nghiêm trọng của vấn đề.