# 🔧 Environment Configuration Guide

## Overview

Your Vehicle Showroom Management project has been updated to use `.env` files for configuration. This provides better security and easier deployment management.

## 🚀 What's Been Updated

### 1. **Program.cs** - Added .env file loading
- Added `DotNetEnv` package reference
- Added automatic `.env` file loading at startup
- Environment variables are now loaded before configuration

### 2. **appsettings.json** - Removed hardcoded secrets
- All sensitive values removed
- Environment variables will override these empty values
- Keeps structure for fallback values

### 3. **Package References** - Added DotNetEnv
- Added `DotNetEnv` package to load `.env` files
- Version 3.1.1 for .NET 8 compatibility

## 🔄 How It Works

### Configuration Priority (Highest to Lowest):
1. **Environment Variables** (from `.env` file)
2. **appsettings.{Environment}.json**
3. **appsettings.json**
4. **Command line arguments**

### Environment Variable Format:
```bash
# .NET Configuration uses double underscores for nested properties
ConnectionStrings__MongoDB=mongodb://...
Jwt__Key=your-jwt-key
EmailSettings__SmtpHost=smtp.gmail.com
CloudinarySettings__CloudName=your-cloud-name
```

## 🧪 Testing Your Configuration

### Windows (PowerShell):
```powershell
# Test environment loading
.\test-env.ps1

# Run your application
dotnet run --project VehicleShowroomManagement/src/WebAPI
```

### Linux/macOS (Bash):
```bash
# Make script executable
chmod +x test-env.sh

# Test environment loading
./test-env.sh

# Run your application
dotnet run --project VehicleShowroomManagement/src/WebAPI
```

## 📁 Environment Files

### Local Development (.env):
```bash
# Application Configuration
ASPNETCORE_ENVIRONMENT=Development
ASPNETCORE_URLS=http://localhost:5000

# Database Configuration
ConnectionStrings__MongoDB=mongodb+srv://carmanagement:car123@carmanagement.trs6nqj.mongodb.net/VehicleShowroomDB

# JWT Configuration
Jwt__Key=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30
Jwt__Issuer=VehicleShowroomAPI
Jwt__Audience=VehicleShowroomClient
# Public Access and API Behavior

- Anonymous (guest) access is enabled for GET endpoints on VehicleModels, VehiclePhotos, and VehicleSpecs.
- VehicleModels: single consolidated GET supports `search` (nullable) for both listing and searching.
- Vehicles: main GET supports list + search via optional filters.
- Model creation is JSON-only; image uploads are separate endpoints.


# Email Settings
EmailSettings__SmtpHost=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SmtpUsername=trendify.store.vn@gmail.com
EmailSettings__SmtpPassword=mhgvffaippfpcwjt

# Cloudinary Settings
CloudinarySettings__CloudName=ddygsw2xd
CloudinarySettings__ApiKey=676435736761836
CloudinarySettings__ApiSecret=4jIdCQ58yq8vKgSggBD3UVRGkJI
```

### Production (.env.production):
```bash
# Application Configuration
ASPNETCORE_ENVIRONMENT=Production
ASPNETCORE_URLS=http://+:10000

# Database Configuration (Update with your production values)
ConnectionStrings__MongoDB=mongodb+srv://username:password@cluster.mongodb.net/VehicleShowroomDB

# JWT Configuration (Generate new secure key for production)
Jwt__Key=your-super-secure-production-jwt-key-32-characters
Jwt__Issuer=VehicleShowroomAPI
Jwt__Audience=VehicleShowroomClient

# Email Settings (Update with your production values)
EmailSettings__SmtpHost=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SmtpUsername=your-production-email@gmail.com
EmailSettings__SmtpPassword=your-production-app-password

# Cloudinary Settings (Update with your production values)
CloudinarySettings__CloudName=your-production-cloud-name
CloudinarySettings__ApiKey=your-production-api-key
CloudinarySettings__ApiSecret=your-production-api-secret
```

## 🔐 Security Benefits

### ✅ What's Improved:
- **No hardcoded secrets** in source code
- **Environment-specific** configurations
- **Git-safe** - `.env` files are ignored
- **Deployment-friendly** - Easy to configure for different environments
- **Secret rotation** - Easy to update without code changes

### ✅ What's Protected:
- Database connection strings
- JWT signing keys
- Email credentials
- Cloudinary API keys
- All other sensitive configuration

## 🚀 Deployment Integration

### Local Development:
1. Ensure `.env` file exists in project root
2. Run `dotnet run --project VehicleShowroomManagement/src/WebAPI`
3. Application automatically loads `.env` variables

### Render.com Deployment:
1. Add environment variables in Render dashboard
2. Use the same variable names from `.env.production`
3. Render will override any configuration values

### Docker Deployment:
```dockerfile
# Copy .env file (if needed for local Docker testing)
COPY .env .env

# Environment variables will be loaded automatically
CMD ["dotnet", "VehicleShowroomManagement.WebAPI.dll"]
```

## 🐛 Troubleshooting

### Environment Variables Not Loading:
1. Check if `.env` file exists in project root
2. Verify file format (no spaces around `=`)
3. Check for typos in variable names
4. Ensure no quotes around values (unless needed)

### Application Won't Start:
1. Verify all required environment variables are set
2. Check database connection string format
3. Ensure JWT key is long enough (32+ characters)
4. Check email service configuration

### Configuration Not Working:
1. Check configuration priority order
2. Verify environment variable naming (double underscores)
3. Test with `test-env.ps1` or `test-env.sh`
4. Check application logs for configuration errors

## 📋 Environment Variable Reference

| Variable | Description | Example |
|----------|-------------|---------|
| `ConnectionStrings__MongoDB` | MongoDB connection string | `mongodb+srv://user:pass@cluster.mongodb.net/db` |
| `Jwt__Key` | JWT signing key (32+ chars) | `your-super-secure-jwt-key-here` |
| `Jwt__Issuer` | JWT issuer | `VehicleShowroomAPI` |
| `Jwt__Audience` | JWT audience | `VehicleShowroomClient` |
| `EmailSettings__SmtpHost` | SMTP server host | `smtp.gmail.com` |
| `EmailSettings__SmtpPort` | SMTP port | `587` |
| `EmailSettings__SmtpUsername` | Email username | `your-email@gmail.com` |
| `EmailSettings__SmtpPassword` | Email password/app password | `your-app-password` |
| `CloudinarySettings__CloudName` | Cloudinary cloud name | `your-cloud-name` |
| `CloudinarySettings__ApiKey` | Cloudinary API key | `your-api-key` |
| `CloudinarySettings__ApiSecret` | Cloudinary API secret | `your-api-secret` |
| `Cors__Origins` | Allowed CORS origins | `http://localhost:3000;https://yourdomain.com` |

## 🔄 Migration from Hardcoded Values

### Before (appsettings.json):
```json
{
  "ConnectionStrings": {
    "MongoDB": "mongodb+srv://carmanagement:car123@..."
  },
  "Jwt": {
    "Key": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
  }
}
```

### After (.env file):
```bash
ConnectionStrings__MongoDB=mongodb+srv://carmanagement:car123@...
Jwt__Key=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🎉 Benefits Summary

- ✅ **Secure** - No secrets in source code
- ✅ **Flexible** - Easy environment switching
- ✅ **Deployable** - Works with any deployment platform
- ✅ **Maintainable** - Easy to update configurations
- ✅ **Professional** - Industry standard practice
- ✅ **Git-safe** - Never commit sensitive data

Your application is now properly configured for secure, professional deployment! 🚀
