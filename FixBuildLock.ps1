# Script to fix MSB3021 build error - File access denied
# This script kills any running VnsErp2025.exe processes and cleans build output
# Usage: .\FixBuildLock.ps1 [-Auto] [-Silent]

param(
    [switch]$Auto,      # Auto-kill without confirmation
    [switch]$Silent     # Silent mode (minimal output)
)

$ErrorActionPreference = "SilentlyContinue"

if (-not $Silent) {
    Write-Host "Checking for running VnsErp2025.exe processes..." -ForegroundColor Yellow
}

$processes = Get-Process -Name "VnsErp2025" -ErrorAction SilentlyContinue

if ($processes) {
    if (-not $Silent) {
        Write-Host "Found $($processes.Count) running instance(s) of VnsErp2025.exe" -ForegroundColor Red
        $processes | ForEach-Object {
            Write-Host "  - PID: $($_.Id), Started: $($_.StartTime)" -ForegroundColor Cyan
        }
    }
    
    if ($Auto) {
        $processes | Stop-Process -Force -ErrorAction SilentlyContinue
        if (-not $Silent) {
            Write-Host "Processes terminated automatically." -ForegroundColor Green
        }
        Start-Sleep -Milliseconds 500
    } else {
        $response = Read-Host "Do you want to kill these processes? (Y/N)"
        if ($response -eq 'Y' -or $response -eq 'y') {
            $processes | Stop-Process -Force -ErrorAction SilentlyContinue
            if (-not $Silent) {
                Write-Host "Processes terminated." -ForegroundColor Green
            }
            Start-Sleep -Seconds 2
        } else {
            if (-not $Silent) {
                Write-Host "Processes not terminated. Please close them manually and try again." -ForegroundColor Yellow
            }
            exit 1
        }
    }
} else {
    if (-not $Silent) {
        Write-Host "No running instances found." -ForegroundColor Green
    }
}

# Try to unlock files in obj and bin directories
$paths = @(
    "$PSScriptRoot\VnsErp2025\bin\Debug\VnsErp2025.exe",
    "$PSScriptRoot\VnsErp2025\obj\Debug\VnsErp2025.exe"
)

foreach ($filePath in $paths) {
    if (Test-Path $filePath) {
        try {
            # Try to remove read-only attribute
            $file = Get-Item $filePath -Force -ErrorAction SilentlyContinue
            if ($file) {
                $file.IsReadOnly = $false
            }
            # Try to delete the file
            Remove-Item -Path $filePath -Force -ErrorAction SilentlyContinue
            if (-not $Silent) {
                Write-Host "  Unlocked: $filePath" -ForegroundColor Green
            }
        } catch {
            if (-not $Silent) {
                Write-Host "  Could not unlock: $filePath" -ForegroundColor Yellow
            }
        }
    }
}

if (-not $Silent) {
    Write-Host "`nDone! You can now rebuild the project in Visual Studio." -ForegroundColor Green
}

exit 0
