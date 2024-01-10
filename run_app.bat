@echo off

echo "Restoring dependencies..."
dotnet restore

if %ERRORLEVEL% neq 0 (
    echo "Error restoring dependencies. Exiting."
    PAUSE
    exit /b 1
)

echo "Building and launching the application..."
dotnet build
if %ERRORLEVEL% neq 0 (
    echo "Error building the application. Exiting."
    PAUSE
    exit /b 1
)

start chrome http://localhost:5041
dotnet run --project ./src/HepsiNerede.WebApp/HepsiNerede.WebApp.csproj

PAUSE
