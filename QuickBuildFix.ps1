# Quick Build Fix - Chạy script này khi gặp lỗi build MSB3021
# Usage: .\QuickBuildFix.ps1

Write-Host "🚀 VnsErp2025 Quick Build Fix" -ForegroundColor Cyan
Write-Host "================================" -ForegroundColor Cyan

# Step 1: Kill processes
Write-Host "1. Killing VnsErp2025 processes..." -ForegroundColor Yellow
.\KillVnsErpProcess.ps1

# Step 2: Clean build folders
Write-Host "2. Cleaning build folders..." -ForegroundColor Yellow
$foldersToClean = @(
    "VnsErp2025\bin\Debug",
    "VnsErp2025\obj\Debug",
    "Inventory\bin\Debug", 
    "Inventory\obj\Debug",
    "DTO\bin\Debug",
    "DTO\obj\Debug",
    "MasterData\bin\Debug",
    "MasterData\obj\Debug",
    "Bll\bin\Debug",
    "Bll\obj\Debug",
    "Dal\bin\Debug",
    "Dal\obj\Debug",
    "Authentication\bin\Debug",
    "Authentication\obj\Debug"
)

foreach ($folder in $foldersToClean) {
    if (Test-Path $folder) {
        Write-Host "   Cleaning: $folder" -ForegroundColor Gray
        Remove-Item "$folder\*" -Force -Recurse -ErrorAction SilentlyContinue
    }
}

# Step 3: Reset file permissions
Write-Host "3. Resetting file permissions..." -ForegroundColor Yellow
$projectFolders = @(
    "VnsErp2025",
    "Inventory", 
    "DTO",
    "MasterData",
    "Bll",
    "Dal",
    "Authentication"
)

foreach ($folder in $projectFolders) {
    if (Test-Path $folder) {
        # Reset any readonly flags
        Get-ChildItem $folder -Recurse -File | ForEach-Object {
            if ($_.IsReadOnly) {
                $_.IsReadOnly = $false
            }
        }
    }
}

Write-Host "4. Build fix completed!" -ForegroundColor Green
Write-Host "💡 Now try building again in Visual Studio" -ForegroundColor Cyan
Write-Host "   Build -> Clean Solution -> Rebuild Solution" -ForegroundColor Cyan

# Optional: Auto-build
$response = Read-Host "🤔 Do you want to auto-build now? (y/n)"
if ($response -eq "y" -or $response -eq "Y") {
    Write-Host "🔨 Starting build..." -ForegroundColor Yellow
    dotnet build VnsErp2025.sln --configuration Debug --verbosity minimal
}

Write-Host "✅ Script completed!" -ForegroundColor Green