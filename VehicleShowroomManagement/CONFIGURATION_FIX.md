# Vehicle Showroom Management - Configuration Setup

## Issues Resolved

### 1. JWT Configuration Missing
**Problem**: The production configuration was missing JWT settings, causing malformed token errors.

**Solution**: 
- Added JWT configuration sections to `appsettings.Production.json` and `appsettings.Docker.json`
- Improved JWT validation with better error handling and logging
- Added configuration validation warnings

### 2. Lucky Penny Software License Error
**Problem**: The application was using a commercial version of MediatR that required a license.

**Solution**:
- Replaced `MediatR.Extensions.Autofac.DependencyInjection` with standard MediatR
- Updated `ApplicationModule.cs` to use standard MediatR registration
- Removed the commercial package dependency

## Environment Configuration

### Required Environment Variables

Create a `.env` file in the project root with the following variables:

```bash
# MongoDB Connection
MONGODB_CONNECTION_STRING=mongodb://localhost:27017/VehicleShowroomManagement

# JWT Configuration (REQUIRED)
JWT_KEY=your-super-secret-jwt-key-that-is-at-least-32-characters-long-for-security
JWT_ISSUER=VehicleShowroomManagement
JWT_AUDIENCE=VehicleShowroomManagement-Users
JWT_EXPIRE_HOURS=24

# Email Settings
EMAIL_SMTP_HOST=smtp.gmail.com
EMAIL_SMTP_PORT=587
EMAIL_SMTP_USERNAME=your-email@gmail.com
EMAIL_SMTP_PASSWORD=your-app-password
EMAIL_FROM_EMAIL=noreply@vehicleshowroom.com
EMAIL_FROM_NAME=Vehicle Showroom Management

# Cloudinary Settings (for image uploads)
CLOUDINARY_CLOUD_NAME=your-cloud-name
CLOUDINARY_API_KEY=your-api-key
CLOUDINARY_API_SECRET=your-api-secret

# Application Settings
ASPNETCORE_ENVIRONMENT=Development
DOTNET_RUNNING_IN_CONTAINER=false
PORT=10000
```

### Production Deployment

For production deployment (e.g., Render.com), set these environment variables:

1. **MongoDB**: Set `MONGODB_CONNECTION_STRING` to your MongoDB Atlas connection string
2. **JWT**: Generate a secure 32+ character key for `JWT_KEY`
3. **Email**: Configure your SMTP settings
4. **Cloudinary**: Set up your image hosting service

## Key Changes Made

1. **Configuration Files Updated**:
   - `appsettings.Production.json` - Added missing JWT, Email, and Cloudinary sections
   - `appsettings.Docker.json` - Added missing configuration sections

2. **MediatR Configuration**:
   - Replaced commercial MediatR with standard version
   - Updated `ApplicationModule.cs` with proper registration
   - Removed `MediatR.Extensions.Autofac.DependencyInjection` package

3. **JWT Validation Improvements**:
   - Added configuration validation warnings
   - Improved error handling with event handlers
   - Added clock skew tolerance
   - Better logging for debugging

4. **Environment Template**:
   - Created `env.template` file with all required variables
   - Added comments and examples for each setting

## Next Steps

1. **Set Environment Variables**: Copy `env.template` to `.env` and fill in your actual values
2. **Generate JWT Key**: Create a secure 32+ character key for JWT authentication
3. **Configure MongoDB**: Set up your MongoDB connection string
4. **Test Authentication**: Verify JWT token generation and validation works correctly

## Troubleshooting

### JWT Token Errors
- Ensure JWT_KEY is at least 32 characters long
- Verify JWT_ISSUER and JWT_AUDIENCE are set
- Check that tokens are properly formatted (header.payload.signature)

### MediatR Errors
- The commercial license error should be resolved
- All MediatR handlers should work with the standard package

### Configuration Validation
- The application will log warnings for missing configuration
- Check logs for specific configuration issues
