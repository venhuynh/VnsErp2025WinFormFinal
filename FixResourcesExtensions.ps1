# Script để fix lỗi System.Resources.Extensions cho tất cả projects

$projects = @(
    "Authentication\Authentication.csproj",
    "MasterData\MasterData.csproj",
    "Inventory\Inventory.csproj"
)

foreach ($proj in $projects) {
    Write-Host "Processing $proj..." -ForegroundColor Cyan
    
    $content = Get-Content $proj -Raw
    
    # Thêm GenerateResourceUsePreserializedResources vào PropertyGroup đầu tiên
    if ($content -notmatch "GenerateResourceUsePreserializedResources") {
        $content = $content -replace "(<Deterministic>true</Deterministic>)", "`$1`n    <GenerateResourceUsePreserializedResources>true</GenerateResourceUsePreserializedResources>"
    }
    
    # Thêm System.Resources.Extensions reference nếu chưa có
    if ($content -notmatch "System.Resources.Extensions") {
        # Tìm vị trí phù hợp để thêm reference (sau System.Windows.Forms nếu có)
        if ($content -match "<Reference Include=`"System.Windows.Forms`" />") {
            $content = $content -replace "(<Reference Include=`"System.Windows.Forms`" />)", "`$1`n    <Reference Include=`"System.Resources.Extensions, Version=4.0.0.0, Culture=neutral, PublicKeyToken=cc7b13ffcd2ddd51, processorArchitecture=MSIL`">`n      <HintPath>..\packages\System.Resources.Extensions.4.6.0\lib\netstandard2.0\System.Resources.Extensions.dll</HintPath>`n    </Reference>"
        }
    }
    
    $content | Set-Content $proj -NoNewline
    Write-Host "Done $proj" -ForegroundColor Green
}

Write-Host "`nAll projects updated!" -ForegroundColor Yellow
