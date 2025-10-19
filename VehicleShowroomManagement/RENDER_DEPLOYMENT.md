# Render.com Deployment Guide

## Quick Setup for Render.com

### 1. Set Required Environment Variables

In your Render.com dashboard, go to your service and add these **REQUIRED** environment variables:

```
MONGODB_CONNECTION_STRING=mongodb+srv://username:password@cluster.mongodb.net/VehicleShowroomDB
JWT_KEY=your-super-secret-jwt-key-that-is-at-least-32-characters-long-for-security
JWT_ISSUER=VehicleShowroomManagement
JWT_AUDIENCE=VehicleShowroomManagement-Users
```

### 2. Optional Environment Variables (for full functionality)

```
EMAIL_SMTP_HOST=smtp.gmail.com
EMAIL_SMTP_PORT=587
EMAIL_SMTP_USERNAME=your-email@gmail.com
EMAIL_SMTP_PASSWORD=your-app-password
EMAIL_FROM_EMAIL=noreply@vehicleshowroom.com
EMAIL_FROM_NAME=Vehicle Showroom Management

CLOUDINARY_CLOUD_NAME=your-cloud-name
CLOUDINARY_API_KEY=your-api-key
CLOUDINARY_API_SECRET=your-api-secret
```

### 3. MongoDB Atlas Setup

1. Go to [MongoDB Atlas](https://www.mongodb.com/atlas)
2. Create a free cluster
3. Create a database user
4. Get your connection string
5. Set `MONGODB_CONNECTION_STRING` in Render.com

### 4. Generate JWT Key

Generate a secure JWT key (32+ characters):
```bash
# Option 1: Use online generator
# Go to: https://generate-secret.vercel.app/32

# Option 2: Use PowerShell
[System.Web.Security.Membership]::GeneratePassword(32, 0)
```

### 5. Deploy

Once you've set the required environment variables, your application should deploy successfully!

## What's Fixed

- ✅ Configuration validation now only fails on critical settings (MongoDB, JWT)
- ✅ Email and Cloudinary settings are optional (warnings only)
- ✅ Better error messages and deployment guidance
- ✅ Environment variable mapping improved

## Troubleshooting

If you still get errors:
1. Check that all 4 required environment variables are set
2. Verify MongoDB connection string is valid
3. Ensure JWT key is at least 32 characters
4. Check Render.com logs for specific error messages
