# Quick script to kill VnsErp2025.exe process (for Pre-build event)
# This script runs silently and automatically kills the process

$ErrorActionPreference = "SilentlyContinue"
$processes = Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue
if ($processes) {
    $processes | Stop-Process -Force -ErrorAction SilentlyContinue
    Start-Sleep -Milliseconds 300
}

# Try to unlock exe files
$exeFiles = @(
    "$(ProjectDir)bin\Debug\VnsErp2025.exe",
    "$(ProjectDir)obj\Debug\VnsErp2025.exe"
)

foreach ($file in $exeFiles) {
    if (Test-Path $file) {
        $item = Get-Item $file -Force -ErrorAction SilentlyContinue
        if ($item) {
            $item.IsReadOnly = $false
        }
    }
}

exit 0

