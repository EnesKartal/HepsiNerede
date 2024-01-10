@echo off

echo "Restoring dependencies..."
dotnet restore

echo "Launching the application..."
start chrome http://localhost:5041
dotnet run --project ./src/HepsiNerede.WebApp/HepsiNerede.WebApp.csproj
