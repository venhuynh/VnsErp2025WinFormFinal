@echo off
echo ========================================
echo Phase 1 Features Test - VnsErp2025 DAL
echo ========================================
echo.

echo Building DAL project...
msbuild Dal\Dal.csproj /p:Configuration=Debug /verbosity:minimal

if %ERRORLEVEL% NEQ 0 (
    echo ❌ Build failed!
    pause
    exit /b 1
)

echo ✅ Build successful!
echo.

echo Running Phase 1 Tests...
echo.

cd Dal
dotnet run --project Dal.csproj --configuration Debug

echo.
echo Test completed!
pause
