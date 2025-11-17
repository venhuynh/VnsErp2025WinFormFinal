# CleanBuildFix.ps1 - Script to fix "Access is denied" build errors
# Run this script when encountering build errors due to locked files

param(
    [switch]$Force = $false
)

$ErrorActionPreference = "SilentlyContinue"

Write-Host "Starting comprehensive build cleanup..." -ForegroundColor Cyan

# 1. Kill all VnsErp2025 processes
Write-Host "1. Terminating VnsErp2025 processes..." -ForegroundColor Yellow
$processes = Get-Process -Name "*VnsErp*" -ErrorAction SilentlyContinue
if ($processes) {
    $processes | Stop-Process -Force -ErrorAction SilentlyContinue
    Write-Host "   Killed $($processes.Count) processes" -ForegroundColor Green
    Start-Sleep -Seconds 2
}

# 2. Kill MSBuild and related processes if Force is specified
if ($Force) {
    Write-Host "2. Force killing MSBuild and VS processes..." -ForegroundColor Yellow
    $buildProcesses = Get-Process -Name "MSBuild", "devenv", "VBCSCompiler" -ErrorAction SilentlyContinue
    if ($buildProcesses) {
        $buildProcesses | Stop-Process -Force -ErrorAction SilentlyContinue
        Write-Host "   Killed build-related processes" -ForegroundColor Green
        Start-Sleep -Seconds 2
    }
}

# 3. Function to force delete locked files
function Force-DeleteFile {
    param([string]$Path)
    
    if (-not (Test-Path $Path)) { return $true }
    
    try {
        Remove-Item $Path -Force -ErrorAction Stop
        return $true
    }
    catch {
        try {
            $tempPath = "$Path.delete_me"
            Move-Item $Path $tempPath -Force
            Remove-Item $tempPath -Force
            return $true
        }
        catch {
            Write-Host "   Could not delete: $Path" -ForegroundColor Red
            return $false
        }
    }
}

# 4. Clean output directories
Write-Host "3. Cleaning bin and obj directories..." -ForegroundColor Yellow

$cleanDirs = @(
    "VnsErp2025\bin\Debug",
    "VnsErp2025\obj\Debug",
    "Inventory\bin\Debug", 
    "Inventory\obj\Debug",
    "Bll\bin\Debug",
    "Bll\obj\Debug",
    "Dal\bin\Debug",
    "Dal\obj\Debug",
    "DTO\bin\Debug",
    "DTO\obj\Debug",
    "Authentication\bin\Debug",
    "Authentication\obj\Debug",
    "MasterData\bin\Debug",
    "MasterData\obj\Debug"
)

foreach ($dir in $cleanDirs) {
    if (Test-Path $dir) {
        Write-Host "   Cleaning: $dir" -ForegroundColor Cyan
        
        # Delete specific problematic files first
        $problemFiles = Get-ChildItem "$dir\*.exe", "$dir\*.pdb", "$dir\*.dll" -ErrorAction SilentlyContinue
        foreach ($file in $problemFiles) {
            Force-DeleteFile $file.FullName | Out-Null
        }
        
        # Clean the entire directory
        Remove-Item "$dir\*" -Recurse -Force -ErrorAction SilentlyContinue
    }
}

Write-Host "Build cleanup completed!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Cyan
Write-Host "   1. Close and restart Visual Studio" -ForegroundColor White
Write-Host "   2. Try building again" -ForegroundColor White
Write-Host "   3. If still fails, run this script with -Force parameter" -ForegroundColor White
Write-Host ""

exit 0