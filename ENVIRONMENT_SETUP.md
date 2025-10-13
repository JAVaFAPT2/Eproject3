# 🔧 Environment Setup Guide

## Overview

Your Vehicle Showroom Management project now has proper environment configuration files set up for both local development and production deployment.

## 📁 Files Created

### Environment Templates
- **`env.template`** - Template for local development
- **`env.production.template`** - Template for production deployment

### Environment Files
- **`.env`** - Your local development environment (created from template)
- **`.env.production`** - Your production environment (created from template)

### Setup Scripts
- **`setup-env.bat`** - Windows batch script to set up environment files
- **`setup-env.sh`** - Linux/macOS shell script to set up environment files

### Protection
- **`.gitignore`** - Updated to protect your actual `.env` files from being committed to git

## 🚀 Quick Start

### 1. Environment Files are Ready
The setup script has already created your `.env` files from the templates. You can now:

```bash
# Edit your local development environment
notepad .env

# Edit your production environment
notepad .env.production
```

### 2. Update Environment Variables

#### For Local Development (.env)
```bash
# Database - Use your existing MongoDB connection
ConnectionStrings__MongoDB=mongodb+srv://carmanagement:car123@carmanagement.trs6nqj.mongodb.net/VehicleShowroomDB

# JWT - Use your existing key or generate a new secure one
Jwt__Key=eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiYWRtaW4iOnRydWUsImlhdCI6MTUxNjIzOTAyMn0.KMUFsIDTnFmyG3nMiGM6H9FNFUROf3wh7SmqJp-QV30

# Email - Update with your credentials
EmailSettings__SmtpUsername=your-email@gmail.com
EmailSettings__SmtpPassword=your-app-password

# Cloudinary - Use your existing credentials
CloudinarySettings__CloudName=ddygsw2xd
CloudinarySettings__ApiKey=676435736761836
CloudinarySettings__ApiSecret=4jIdCQ58yq8vKgSggBD3UVRGkJI
```

#### For Production (.env.production)
```bash
# Database - Use MongoDB Atlas connection string
ConnectionStrings__MongoDB=mongodb+srv://username:password@cluster.mongodb.net/VehicleShowroomDB

# JWT - Generate a secure random key for production
Jwt__Key=your-super-secure-production-jwt-key-32-characters

# Email - Use production email credentials
EmailSettings__SmtpUsername=your-production-email@gmail.com
EmailSettings__SmtpPassword=your-production-app-password

# Cloudinary - Use production credentials
CloudinarySettings__CloudName=your-production-cloud-name
CloudinarySettings__ApiKey=your-production-api-key
CloudinarySettings__ApiSecret=your-production-api-secret
```

## 🔐 Security Best Practices

### ✅ DO:
- Use different JWT keys for development and production
- Use strong, unique passwords
- Use environment variables for all secrets
- Keep `.env` files out of version control
- Use app passwords for Gmail (not your regular password)

### ❌ DON'T:
- Commit `.env` files to git
- Use the same credentials for development and production
- Use weak passwords or keys
- Hardcode secrets in your application code

## 🚀 Using Environment Variables

### Local Development
Your application will automatically load the `.env` file when running locally.

```bash
# Run your .NET application
dotnet run --project VehicleShowroomManagement/src/WebAPI

# Or use Docker
docker build -t vehicleshowroom .
docker run -p 5000:10000 --env-file .env vehicleshowroom
```

### Production Deployment (Render.com)
When deploying to Render.com, you'll add these environment variables in the Render dashboard:

1. Go to your Render service dashboard
2. Click "Environment" tab
3. Add each variable from your `.env.production` file
4. Click "Save Changes" to redeploy

## 🔄 Regenerating Environment Files

If you need to regenerate the environment files:

### Windows
```cmd
.\setup-env.bat
```

### Linux/macOS
```bash
./setup-env.sh
```

This will:
- Backup existing `.env` files
- Create new files from templates
- Update `.gitignore` if needed

## 📝 Environment Variable Reference

### Application Configuration
- `ASPNETCORE_ENVIRONMENT` - Environment (Development/Production)
- `ASPNETCORE_URLS` - URLs to bind to (localhost:5000 or +:10000)

### Database
- `ConnectionStrings__MongoDB` - MongoDB connection string

### Authentication
- `Jwt__Key` - JWT signing key (32+ characters)
- `Jwt__Issuer` - JWT issuer
- `Jwt__Audience` - JWT audience
- `Jwt__ExpireHours` - Token expiration time

### Email
- `EmailSettings__SmtpHost` - SMTP server host
- `EmailSettings__SmtpPort` - SMTP port
- `EmailSettings__SmtpUsername` - Email username
- `EmailSettings__SmtpPassword` - Email password/app password
- `EmailSettings__EnableSsl` - Enable SSL
- `EmailSettings__FromEmail` - From email address
- `EmailSettings__FromName` - From name

### Cloudinary (Image Storage)
- `CloudinarySettings__CloudName` - Cloudinary cloud name
- `CloudinarySettings__ApiKey` - Cloudinary API key
- `CloudinarySettings__ApiSecret` - Cloudinary API secret

### CORS
- `Cors__Origins` - Allowed origins (semicolon-separated)

### Logging
- `Logging__LogLevel__Default` - Default log level
- `Logging__LogLevel__Microsoft.AspNetCore` - ASP.NET log level

## 🐛 Troubleshooting

### Environment Variables Not Loading
- Check if `.env` file exists in the project root
- Verify the file format (no spaces around `=`)
- Ensure no quotes around values (unless needed)

### Build Errors
- Check if all required environment variables are set
- Verify connection strings are correct
- Ensure JWT key is long enough (32+ characters)

### Database Connection Issues
- Verify MongoDB connection string format
- Check if database allows connections from your IP
- Ensure database user has correct permissions

## 📞 Support

If you encounter issues:
1. Check the application logs
2. Verify environment variables are set correctly
3. Test database connectivity
4. Review the deployment documentation

## 🔗 Related Files

- `DEPLOYMENT.md` - Complete deployment guide
- `render-setup.md` - Quick Render.com setup
- `render.yaml` - Render.com configuration
- `Dockerfile` - Docker configuration

---

**Your environment is now properly configured for both development and production! 🎉**
