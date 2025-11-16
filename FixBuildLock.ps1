# Script to fix MSB3021 build error - File access denied
# This script kills any running VnsErp2025.exe processes and cleans build output

Write-Host "Checking for running VnsErp2025.exe processes..." -ForegroundColor Yellow

$processes = Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue

if ($processes) {
    Write-Host "Found $($processes.Count) running instance(s) of VnsErp2025.exe" -ForegroundColor Red
    $processes | ForEach-Object {
        Write-Host "  - PID: $($_.Id), Started: $($_.StartTime)" -ForegroundColor Cyan
    }
    
    $response = Read-Host "Do you want to kill these processes? (Y/N)"
    if ($response -eq 'Y' -or $response -eq 'y') {
        $processes | Stop-Process -Force
        Write-Host "Processes terminated." -ForegroundColor Green
        Start-Sleep -Seconds 2
    } else {
        Write-Host "Processes not terminated. Please close them manually and try again." -ForegroundColor Yellow
        exit
    }
} else {
    Write-Host "No running instances found." -ForegroundColor Green
}

Write-Host "`nCleaning build output directories..." -ForegroundColor Yellow

$paths = @(
    "VnsErp2025\bin\Debug",
    "VnsErp2025\obj\Debug"
)

foreach ($path in $paths) {
    if (Test-Path $path) {
        try {
            Remove-Item -Path $path -Recurse -Force -ErrorAction Stop
            Write-Host "  Cleaned: $path" -ForegroundColor Green
        } catch {
            Write-Host "  Failed to clean: $path - $($_.Exception.Message)" -ForegroundColor Red
        }
    } else {
        Write-Host "  Path does not exist: $path" -ForegroundColor Gray
    }
}

Write-Host "`nDone! You can now rebuild the project in Visual Studio." -ForegroundColor Green

