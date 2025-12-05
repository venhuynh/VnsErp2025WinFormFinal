# Script to convert file-scoped namespaces to traditional namespace blocks

$dalPath = "C:\Users\Admin\source\Workspaces\2025\VNS_ERP_2025_FINAL\VnsErp2025\Dal"
$count = 0

# Find all .cs files with file-scoped namespaces
Get-ChildItem -Path $dalPath -Filter "*.cs" -Recurse | ForEach-Object {
    $filePath = $_.FullName
    $content = Get-Content $filePath -Raw
    
    # Check if file has file-scoped namespace (pattern: namespace X;)
    if ($content -match '(?m)^namespace\s+[\w\.]+;') {
        Write-Host "Processing: $($_.Name)"
        
        # Replace namespace declaration
        $content = $content -replace '(?m)^(namespace\s+([\w\.]+));', 'namespace $2
{'
        
        # Replace closing of file with namespace closing brace
        # Find the last } and add another } before it (if it's not already there)
        $lines = $content -split "`n"
        $lastNonEmptyIndex = -1
        
        # Find last non-empty line
        for ($i = $lines.Count - 1; $i -ge 0; $i--) {
            if ($lines[$i].Trim() -ne '') {
                $lastNonEmptyIndex = $i
                break
            }
        }
        
        if ($lastNonEmptyIndex -ge 0 -and $lines[$lastNonEmptyIndex].Trim() -eq '}') {
            # Add closing brace for namespace
            $lines[$lastNonEmptyIndex] = '}' + "`n}"
        }
        
        $content = $lines -join "`n"
        
        # Write back to file
        Set-Content $filePath -Value $content -Encoding UTF8
        $count++
    }
}

Write-Host "Converted $count files"
