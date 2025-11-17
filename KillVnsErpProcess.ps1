# Enhanced script to kill VnsErp2025.exe process and unlock files
# Improved version with better error handling and file management

$ErrorActionPreference = "SilentlyContinue"

Write-Host "🔄 Cleaning VnsErp2025 processes and files..." -ForegroundColor Yellow

# Kill all VnsErp2025 processes
$processes = Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue
if ($processes) {
    Write-Host "🛑 Stopping VnsErp2025 processes..." -ForegroundColor Red
    $processes | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 1
}

# Kill any hung Visual Studio processes related to VnsErp
$vsProcesses = Get-Process | Where-Object { 
    $_.ProcessName -like "*devenv*" -and 
    $_.MainWindowTitle -like "*VnsErp*" 
} -ErrorAction SilentlyContinue

if ($vsProcesses) {
    Write-Host "🛑 Found hung Visual Studio processes related to VnsErp..." -ForegroundColor Red
    # Note: Don't auto-kill VS, just report
    Write-Host "   Consider restarting Visual Studio if build continues to fail" -ForegroundColor Yellow
}

# Clear readonly attributes and unlock exe files
$exeFiles = @(
    "VnsErp2025\bin\Debug\VnsErp2025.exe",
    "VnsErp2025\obj\Debug\VnsErp2025.exe",
    "VnsErp2025\bin\Debug\VnsErp2025.pdb",
    "VnsErp2025\obj\Debug\VnsErp2025.pdb"
)

foreach ($file in $exeFiles) {
    if (Test-Path $file) {
        Write-Host "🔓 Unlocking file: $file" -ForegroundColor Green
        $item = Get-Item $file -Force -ErrorAction SilentlyContinue
        if ($item) {
            $item.IsReadOnly = $false
            # Try to take ownership if needed
            try {
                $item.SetAccessControl((Get-Acl $item.FullName))
            } catch {
                # Ignore ownership errors
            }
        }
    }
}

# Optional: Clean temp files that might cause issues
$tempFiles = @(
    "VnsErp2025\obj\Debug\*.cache",
    "VnsErp2025\bin\Debug\*.vshost.*"
)

foreach ($pattern in $tempFiles) {
    $files = Get-ChildItem $pattern -ErrorAction SilentlyContinue
    if ($files) {
        Write-Host "🗑️ Cleaning temp files: $pattern" -ForegroundColor Cyan
        $files | Remove-Item -Force -ErrorAction SilentlyContinue
    }
}

Write-Host "✅ Process cleanup completed!" -ForegroundColor Green
Start-Sleep -Milliseconds 500
exit 0

