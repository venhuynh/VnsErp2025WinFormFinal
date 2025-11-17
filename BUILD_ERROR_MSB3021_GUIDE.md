# Giải pháp cho lỗi MSB3021: Unable to copy file "obj\Debug\VnsErp2025.exe" to "bin\Debug\VnsErp2025.exe". Access denied

## Nguyên nhân gây lỗi:
1. **Process VnsErp2025.exe vẫn đang chạy** - Windows khóa file khi process đang hoạt động
2. **Visual Studio Debugger vẫn giữ handle** - Debugger chưa release file handle 
3. **Antivirus scanning** - Phần mềm diệt virus đang quét file
4. **Windows Indexing Service** - Service đánh index đang truy cập file
5. **File permissions** - Quyền ghi file bị hạn chế

## Giải pháp tức thì:

### 1. Sử dụng script tự động (Khuyên dùng):
```powershell
# Chạy script có sẵn
.\KillVnsErpProcess.ps1
```

### 2. Dọn dẹp thủ công:
```powershell
# Kill processes
Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue | Stop-Process -Force

# Clean build folders
Remove-Item "VnsErp2025\bin\Debug\*" -Force -Recurse -ErrorAction SilentlyContinue
Remove-Item "VnsErp2025\obj\Debug\*" -Force -Recurse -ErrorAction SilentlyContinue
```

### 3. Build clean:
```bash
# Trong Visual Studio
Build -> Clean Solution
Build -> Rebuild Solution
```

## Giải pháp vĩnh viễn:

### 1. Pre-build Event (Tự động kill process):
Thêm vào VnsErp2025.csproj:
```xml
<PropertyGroup Condition=" '$(Configuration)|$(Platform)' == 'Debug|AnyCPU' ">
  ...
  <PreBuildEvent>powershell.exe -ExecutionPolicy Bypass -File "$(ProjectDir)..\KillVnsErpProcess.ps1"</PreBuildEvent>
</PropertyGroup>
```

### 2. Cải thiện KillVnsErpProcess.ps1:
```powershell
# Enhanced version with better error handling
$ErrorActionPreference = "SilentlyContinue"
$processes = Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue
if ($processes) {
    $processes | Stop-Process -Force
    Start-Sleep -Seconds 1
}

# Clear file attributes
$files = @(
    "VnsErp2025\bin\Debug\VnsErp2025.exe",
    "VnsErp2025\obj\Debug\VnsErp2025.exe"
)
foreach ($file in $files) {
    if (Test-Path $file) {
        Set-ItemProperty -Path $file -Name IsReadOnly -Value $false
    }
}
```

### 3. MSBuild configuration:
Thêm vào Directory.Build.props:
```xml
<Project>
  <PropertyGroup>
    <GenerateAssemblyInfo>false</GenerateAssemblyInfo>
    <UseCommonOutputDirectory>true</UseCommonOutputDirectory>
  </PropertyGroup>
</Project>
```

## Troubleshooting bổ sung:

### Nếu vẫn lỗi:
1. **Restart Visual Studio** với quyền Administrator
2. **Disable Antivirus** tạm thời cho thư mục project
3. **Check Windows Defender** exclusions
4. **Restart Windows** nếu cần thiết

### Monitor file handles:
```powershell
# Sử dụng Process Explorer hoặc Handle.exe từ Sysinternals
handle.exe VnsErp2025.exe
```

### Build configuration:
- Sử dụng **Any CPU** thay vì x86/x64 specific
- Enable **"Enable native code debugging"** trong project properties
- Disable **"Enable Visual Studio hosting process"**

## Notes:
- Script KillVnsErpProcess.ps1 đã được tối ưu cho project này
- Lỗi này rất phổ biến trong .NET development
- Pre-build event là giải pháp tốt nhất cho long-term