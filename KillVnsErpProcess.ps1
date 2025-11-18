# Enhanced script to kill VnsErp2025.exe process and unlock files
# Improved version with better error handling and file management

$ErrorActionPreference = "SilentlyContinue"

Write-Host "🔄 Cleaning VnsErp2025 processes and files..." -ForegroundColor Yellow

# Kill all VnsErp2025 processes
$processes = Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue
if ($processes) {
    Write-Host "🛑 Stopping VnsErp2025 processes..." -ForegroundColor Red
    $processes | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Seconds 2
    
    # Verify processes are really gone
    $remainingProcesses = Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue
    if ($remainingProcesses) {
        Write-Host "⚠️ Some processes still running, force killing..." -ForegroundColor Yellow
        $remainingProcesses | Stop-Process -Force -ErrorAction SilentlyContinue
        Start-Sleep -Seconds 1
    }
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

# Function to unlock file with multiple attempts
function Unlock-File {
    param([string]$FilePath)
    
    if (-not (Test-Path $FilePath)) { return }
    
    Write-Host "🔓 Unlocking file: $FilePath" -ForegroundColor Green
    
    try {
        # Remove readonly attribute
        $item = Get-Item $FilePath -Force
        $item.IsReadOnly = $false
        
        # Try to delete and recreate if it's an exe file that's locked
        if ($FilePath -like "*.exe" -or $FilePath -like "*.pdb") {
            $tempName = "$FilePath.bak"
            Move-Item $FilePath $tempName -Force -ErrorAction SilentlyContinue
            Remove-Item $tempName -Force -ErrorAction SilentlyContinue
        }
    }
    catch {
        Write-Host "⚠️ Could not fully unlock: $FilePath" -ForegroundColor Yellow
    }
}

# Clear readonly attributes and unlock exe files
$exeFiles = @(
    "VnsErp2025\bin\Debug\VnsErp2025.exe",
    "VnsErp2025\obj\Debug\VnsErp2025.exe",
    "VnsErp2025\bin\Debug\VnsErp2025.pdb",
    "VnsErp2025\obj\Debug\VnsErp2025.pdb",
    "VnsErp2025\bin\Debug\VnsErp2025.exe.config",
    "VnsErp2025\obj\Debug\VnsErp2025.exe.config"
)

foreach ($file in $exeFiles) {
    Unlock-File $file
}

# Optional: Clean temp files that might cause issues
$tempFiles = @(
    "VnsErp2025\obj\Debug\*.cache",
    "VnsErp2025\bin\Debug\*.vshost.*",
    "VnsErp2025\obj\Debug\*.tmp"
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

