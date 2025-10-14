#!/bin/bash

# Test Environment Variables Loading
echo "🔧 Testing Environment Variables Loading"
echo "========================================="

# Check if .env file exists
if [ -f ".env" ]; then
    echo "✅ .env file found"
else
    echo "❌ .env file not found. Run setup-env.sh first!"
    exit 1
fi

# Load and display environment variables
echo ""
echo "Loading environment variables:"
echo "-----------------------------"

while IFS= read -r line; do
    # Skip comments and empty lines
    if [[ $line =~ ^[[:space:]]*# ]] || [[ -z $line ]]; then
        continue
    fi
    
    # Parse key=value pairs
    if [[ $line =~ ^([^=]+)=(.*)$ ]]; then
        key="${BASH_REMATCH[1]// /}"
        value="${BASH_REMATCH[2]}"
        
        # Set environment variable
        export "$key=$value"
        
        # Display (mask sensitive values)
        if [[ $key =~ (Password|Key|Secret|Token) ]]; then
            echo "✅ $key = [MASKED]"
        else
            echo "✅ $key = $value"
        fi
    fi
done < .env

echo ""
echo "🎉 Environment variables loaded successfully!"
echo ""
echo "Next steps:"
echo "1. Run your .NET application: dotnet run --project VehicleShowroomManagement/src/WebAPI"
echo "2. Check if the application starts without errors"
echo "3. Test API endpoints"
echo ""
