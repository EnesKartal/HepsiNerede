@echo off

echo "Restoring dependencies..."
dotnet restore

if %ERRORLEVEL% neq 0 (
    echo "Error restoring dependencies. Exiting."
    PAUSE
    exit /b 1
)

echo "Running tests..."
dotnet test .\tests\HepsiNerede.Tests\HepsiNerede.Tests.csproj --logger "console;verbosity=detailed"

PAUSE
