@echo off
echo Checking for existing Charity_BE processes on port 5210...

REM Kill any existing Charity_BE processes
for /f "tokens=5" %%a in ('netstat -ano ^| findstr :5210') do (
    taskkill /PID %%a /F >nul 2>&1
    echo Killed process %%a using port 5210
)

echo Starting Charity_BE application...
dotnet run --project Charity_BE

pause