# Vehicle Showroom Management - Render.com Deployment Guide

This guide explains how to deploy your Vehicle Showroom Management application to the internet using Render.com with automatic CI/CD from GitHub.

## Prerequisites

1. **Render.com Account**: Sign up at [render.com](https://render.com)
2. **GitHub Repository**: Your code hosted on GitHub
3. **MongoDB Atlas Account**: For database hosting (free tier available)
4. **Cloudinary Account**: For image storage (free tier available)

## Architecture Overview

```
GitHub Push → Render.com → Automatic Build → Deploy → Internet
```

## Why Render.com?

✅ **Free Tier**: 750 hours/month (usually enough for small projects)  
✅ **Easy Setup**: Just connect GitHub repo  
✅ **Automatic Deployments**: Push to main = auto deploy  
✅ **Built-in Database**: PostgreSQL included  
✅ **Custom Domains**: Professional URLs  
✅ **SSL Certificates**: Automatic HTTPS  
✅ **No Credit Card Required**: Truly free to start

## Setup Steps

### 1. Render.com Account Setup

#### Create Account
1. Go to [render.com](https://render.com)
2. Sign up with your GitHub account
3. Connect your GitHub repository
4. No credit card required for free tier

### 2. MongoDB Atlas Setup (Free Database)

#### Create MongoDB Atlas Account
1. Go to [mongodb.com/atlas](https://mongodb.com/atlas)
2. Sign up for free
3. Create a new cluster (free tier: M0)
4. Create a database user
5. Get your connection string

#### Connection String Format
```
mongodb+srv://username:password@cluster.mongodb.net/database?retryWrites=true&w=majority
```

### 3. Cloudinary Setup (Free Image Storage)

#### Create Cloudinary Account
1. Go to [cloudinary.com](https://cloudinary.com)
2. Sign up for free
3. Get your cloud name, API key, and API secret from dashboard

### 4. Deploy to Render.com

#### Method 1: Using render.yaml (Recommended)
1. Push your code to GitHub (the `render.yaml` file is already configured)
2. Go to [render.com](https://render.com)
3. Click "New +" → "Blueprint"
4. Connect your GitHub repository
5. Render will automatically detect the `render.yaml` file
6. Click "Apply" to deploy

#### Method 2: Manual Setup
1. Go to [render.com](https://render.com)
2. Click "New +" → "Web Service"
3. Connect your GitHub repository
4. Configure the service:
   - **Name**: `vehicleshowroom-api`
   - **Environment**: `Docker`
   - **Dockerfile Path**: `./Dockerfile`
   - **Plan**: `Free`

### 5. Environment Variables

In your Render dashboard, add these environment variables:

#### Database
- `ConnectionStrings__MongoDB`: Your MongoDB Atlas connection string

#### JWT Configuration
- `Jwt__Key`: Generate a secure random string (32+ characters)
- `Jwt__Issuer`: `VehicleShowroomAPI`
- `Jwt__Audience`: `VehicleShowroomClient`

#### Email Settings (Optional)
- `EmailSettings__SmtpHost`: `smtp.gmail.com`
- `EmailSettings__SmtpPort`: `587`
- `EmailSettings__SmtpUsername`: Your Gmail address
- `EmailSettings__SmtpPassword`: Your Gmail app password

#### Cloudinary Settings
- `CloudinarySettings__CloudName`: Your Cloudinary cloud name
- `CloudinarySettings__ApiKey`: Your Cloudinary API key
- `CloudinarySettings__ApiSecret`: Your Cloudinary API secret

### 6. Deploy

#### Automatic Deployment
1. Push your code to the `main` branch
2. Render will automatically:
   - Build your Docker container
   - Deploy the application
   - Provide a public URL
   - Set up SSL certificate

#### Manual Deploy
1. Go to your Render dashboard
2. Click "Manual Deploy" → "Deploy latest commit"

## Configuration Files

### render.yaml
- Defines your service configuration
- Sets up environment variables
- Configures the database connection
- Specifies Docker build settings

### Dockerfile
- Multi-stage build for optimized image size
- Exposes port 10000 (Render's default)
- Production-ready configuration
- No unnecessary dependencies

## Monitoring and Maintenance

### Check Application Status
1. Go to your Render dashboard
2. Click on your service
3. Check the "Metrics" tab for performance data
4. View logs in the "Logs" tab

### Update Application
1. Push changes to your GitHub repository
2. Render automatically detects changes
3. Builds and deploys the new version
4. No manual intervention required

### Environment Variables Management
1. Go to your Render dashboard
2. Click on your service
3. Go to "Environment" tab
4. Add/update environment variables
5. Click "Save Changes" to redeploy

## Troubleshooting

### Common Issues

#### 1. Build Failed
- Check Dockerfile syntax
- Verify all dependencies are included
- Check Render build logs for specific errors

#### 2. Application Not Starting
- Verify environment variables are set correctly
- Check application logs in Render dashboard
- Ensure port 10000 is exposed in Dockerfile

#### 3. Database Connection Issues
- Verify MongoDB Atlas connection string
- Check if IP whitelist allows Render's IPs
- Ensure database user has correct permissions

#### 4. SSL Certificate Issues
- Render provides automatic SSL
- Check if custom domain is properly configured
- Verify DNS settings for custom domains

### Logs and Debugging

#### Render Dashboard
1. Go to your service in Render dashboard
2. Click "Logs" tab
3. View real-time logs
4. Filter by log level if needed

#### Local Testing
```bash
# Test Docker build locally
docker build -t vehicleshowroom-test .

# Run container locally
docker run -p 5000:10000 --env-file .env vehicleshowroom-test
```

## Security Considerations

1. **Environment Variables**: Never commit secrets to git
2. **MongoDB Atlas**: Use IP whitelisting and strong passwords
3. **JWT Keys**: Generate secure, random keys
4. **HTTPS**: Render provides automatic SSL certificates
5. **Database**: Use MongoDB Atlas security features

## Cost Estimation

- **Render.com**: Free tier (750 hours/month)
- **MongoDB Atlas**: Free tier (M0 cluster)
- **Cloudinary**: Free tier (25 GB storage, 25 GB bandwidth)
- **Custom Domain**: $10-15/year (optional)
- **Total**: $0-15/year (mostly free!)

## Free Tier Limits

### Render.com Free Tier
- **750 hours/month** (usually enough for small projects)
- **512 MB RAM**
- **0.1 CPU**
- **Sleeps after 15 minutes** of inactivity

### MongoDB Atlas Free Tier
- **512 MB storage**
- **Shared RAM**
- **No backup retention**

### Cloudinary Free Tier
- **25 GB storage**
- **25 GB bandwidth**
- **25,000 transformations**

## Next Steps

1. Set up custom domain (optional)
2. Configure monitoring and alerts
3. Set up staging environment
4. Implement health checks
5. Add performance monitoring

## Quick Start Checklist

- [ ] Create Render.com account
- [ ] Connect GitHub repository
- [ ] Set up MongoDB Atlas
- [ ] Configure Cloudinary
- [ ] Add environment variables
- [ ] Deploy application
- [ ] Test API endpoints
- [ ] Set up custom domain (optional)

## Support

For issues:
1. Check Render dashboard logs
2. Review MongoDB Atlas logs
3. Test API endpoints manually
4. Check environment variables
5. Contact Render support if needed

## Additional Resources

- [Render.com Documentation](https://render.com/docs)
- [MongoDB Atlas Documentation](https://docs.atlas.mongodb.com/)
- [Cloudinary Documentation](https://cloudinary.com/documentation)
- [Docker Documentation](https://docs.docker.com/)
- [.NET Core Documentation](https://docs.microsoft.com/en-us/aspnet/core/)
