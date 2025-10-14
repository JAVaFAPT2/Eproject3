# 🚀 Quick Render.com Setup Guide

## Step-by-Step Deployment

### 1. Create Accounts (5 minutes)

#### Render.com
1. Go to [render.com](https://render.com)
2. Click "Get Started for Free"
3. Sign up with your GitHub account
4. Verify your email

#### MongoDB Atlas
1. Go to [mongodb.com/atlas](https://mongodb.com/atlas)
2. Click "Try Free"
3. Create account and cluster
4. Create database user
5. Get connection string

#### Cloudinary
1. Go to [cloudinary.com](https://cloudinary.com)
2. Click "Sign Up For Free"
3. Get your cloud credentials

### 2. Deploy to Render (2 minutes)

#### Option A: Blueprint (Easiest)
1. Go to [render.com](https://render.com)
2. Click "New +" → "Blueprint"
3. Connect your GitHub repository
4. Render detects `render.yaml` automatically
5. Click "Apply"

#### Option B: Manual
1. Go to [render.com](https://render.com)
2. Click "New +" → "Web Service"
3. Connect GitHub repository
4. Configure:
   - **Name**: `vehicleshowroom-api`
   - **Environment**: `Docker`
   - **Dockerfile Path**: `./Dockerfile`
   - **Plan**: `Free`

### 3. Add Environment Variables

In Render dashboard → Your Service → Environment:

```
ConnectionStrings__MongoDB=mongodb+srv://username:password@cluster.mongodb.net/database
Jwt__Key=your-super-secret-jwt-key-32-characters-min
Jwt__Issuer=VehicleShowroomAPI
Jwt__Audience=VehicleShowroomClient
CloudinarySettings__CloudName=your-cloud-name
CloudinarySettings__ApiKey=your-api-key
CloudinarySettings__ApiSecret=your-api-secret
```

### 4. Deploy

1. Click "Save Changes"
2. Render builds and deploys automatically
3. Get your public URL: `https://vehicleshowroom-api.onrender.com`

### 5. Test Your API

```bash
# Test health endpoint
curl https://your-app-url.onrender.com/swagger

# Test API
curl https://your-app-url.onrender.com/api/vehicles
```

## 🎉 You're Live!

Your Vehicle Showroom Management API is now deployed and accessible worldwide!

## 📊 Free Tier Limits

- **Render**: 750 hours/month (sleeps after 15min inactivity)
- **MongoDB**: 512MB storage
- **Cloudinary**: 25GB storage + bandwidth

## 🔧 Troubleshooting

### Build Fails
- Check Dockerfile syntax
- Verify all files are committed to GitHub
- Check Render build logs

### App Won't Start
- Verify environment variables
- Check MongoDB connection string
- Review application logs

### Database Issues
- Whitelist all IPs in MongoDB Atlas (0.0.0.0/0)
- Verify database user permissions
- Check connection string format

## 📞 Support

- **Render**: [render.com/support](https://render.com/support)
- **MongoDB**: [docs.atlas.mongodb.com](https://docs.atlas.mongodb.com/)
- **Cloudinary**: [support.cloudinary.com](https://support.cloudinary.com/)

## 🚀 Next Steps

1. Set up custom domain
2. Configure monitoring
3. Set up staging environment
4. Add health checks
5. Implement CI/CD

---

**Total setup time: ~10 minutes**  
**Cost: $0/month**  
**Global availability: ✅**
