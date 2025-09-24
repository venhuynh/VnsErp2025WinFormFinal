@echo off
echo ========================================
echo Phase 1 Features Test - VnsErp2025 DAL
echo ========================================
echo.

echo Building DAL project...
dotnet build Dal\Dal.csproj --configuration Debug --verbosity minimal

if %ERRORLEVEL% NEQ 0 (
    echo ❌ Build failed!
    pause
    exit /b 1
)

echo ✅ Build successful!
echo.

echo Running Phase 1 Tests...
echo.

cd Dal\bin\Debug
Dal.exe

echo.
echo Test completed!
cd ..\..\..
pause
