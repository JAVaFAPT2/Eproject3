# Test Environment Variables Loading
Write-Host "Testing Environment Variables Loading" -ForegroundColor Green
Write-Host "=========================================" -ForegroundColor Green

# Check if .env file exists
if (Test-Path ".env") {
    Write-Host ".env file found" -ForegroundColor Green
} else {
    Write-Host ".env file not found. Run setup-env.bat first!" -ForegroundColor Red
    exit 1
}

# Load environment variables
Get-Content ".env" | ForEach-Object {
    if ($_ -match '^([^#][^=]+)=(.*)$') {
        $name = $matches[1].Trim()
        $value = $matches[2].Trim()
        
        # Set environment variable
        [Environment]::SetEnvironmentVariable($name, $value, "Process")
        
        # Display (mask sensitive values)
        if ($name -match "(Password|Key|Secret|Token)") {
            Write-Host "$name = [MASKED]" -ForegroundColor Yellow
        } else {
            Write-Host "$name = $value" -ForegroundColor Cyan
        }
    }
}

Write-Host ""
Write-Host "Environment variables loaded successfully!" -ForegroundColor Green
Write-Host ""
Write-Host "Next steps:" -ForegroundColor Yellow
Write-Host "1. Run your .NET application: dotnet run --project VehicleShowroomManagement/src/WebAPI" -ForegroundColor White
Write-Host "2. Check if the application starts without errors" -ForegroundColor White
Write-Host "3. Test API endpoints" -ForegroundColor White
Write-Host ""