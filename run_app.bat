@echo off

echo "Restoring dependencies..."
dotnet restore

if %ERRORLEVEL% neq 0 (
    echo "Error restoring dependencies. Exiting."
    PAUSE
    exit /b 1
)

echo "Launching the application..."
start chrome http://localhost:5041
dotnet run --project ./src/HepsiNerede.WebApp/HepsiNerede.WebApp.csproj

PAUSE
