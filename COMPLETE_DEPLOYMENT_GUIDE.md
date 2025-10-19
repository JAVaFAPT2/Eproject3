# 🚀 Complete Render Deployment Guide - Frontend + Backend

## Overview
This guide will deploy both your **Backend API** and **Frontend Web App** to Render using the same environment configuration approach.

## ✅ What's Already Set Up

### Backend (Already Working)
- ✅ Dockerfile configured for .NET 8
- ✅ Environment variables setup
- ✅ MongoDB connection ready
- ✅ JWT authentication configured

### Frontend (Now Configured)
- ✅ Environment variables template created
- ✅ Dockerfile for React app with nginx
- ✅ nginx configuration for React Router
- ✅ API client already uses environment variables

---

## 🚀 Deployment Steps

### Step 1: Deploy Using Blueprint (Recommended - 5 minutes)

1. **Go to Render.com**
   - Visit [render.com](https://render.com)
   - Click "New +" → "Blueprint"

2. **Connect Repository**
   - Connect your GitHub repository
   - Render will detect `render.yaml` automatically

3. **Deploy**
   - Click "Apply"
   - Render will create both services automatically:
     - `vehicleshowroom-api` (Backend)
     - `vehicleshowroom-web` (Frontend)

### Step 2: Configure Environment Variables (2 minutes)

#### Backend Environment Variables
In Render dashboard → `vehicleshowroom-api` → Environment:

```bash
# Required
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__MongoDB=mongodb+srv://username:password@cluster.mongodb.net/VehicleShowroomDB
Jwt__Key=your-super-secret-jwt-key-that-is-at-least-32-characters-long
Jwt__Issuer=VehicleShowroomAPI
Jwt__Audience=VehicleShowroomClient

# Optional (for full functionality)
EmailSettings__SmtpHost=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SmtpUsername=your-email@gmail.com
EmailSettings__SmtpPassword=your-app-password
EmailSettings__FromEmail=noreply@vehicleshowroom.com
EmailSettings__FromName=Vehicle Showroom Management

CloudinarySettings__CloudName=your-cloud-name
CloudinarySettings__ApiKey=your-api-key
CloudinarySettings__ApiSecret=your-api-secret
```

#### Frontend Environment Variables
In Render dashboard → `vehicleshowroom-web` → Environment:

```bash
# These are automatically set by render.yaml, but you can override:
REACT_APP_API_URL=https://vehicleshowroom-api.onrender.com/api
REACT_APP_NAME=Vehicle Showroom Management
REACT_APP_VERSION=2.0.0
REACT_APP_ENVIRONMENT=production

# Optional - Firebase (if using Google Auth)
REACT_APP_FIREBASE_API_KEY=your-firebase-api-key
REACT_APP_FIREBASE_AUTH_DOMAIN=your-project.firebaseapp.com
REACT_APP_FIREBASE_PROJECT_ID=your-project-id
REACT_APP_FIREBASE_STORAGE_BUCKET=your-project.appspot.com
REACT_APP_FIREBASE_MESSAGING_SENDER_ID=your-sender-id
REACT_APP_FIREBASE_APP_ID=your-app-id
REACT_APP_FIREBASE_MEASUREMENT_ID=your-measurement-id
```

---

## 🎉 Expected Results

### URLs After Deployment
- **Frontend**: `https://vehicleshowroom-web.onrender.com`
- **Backend API**: `https://vehicleshowroom-api.onrender.com`
- **API Documentation**: `https://vehicleshowroom-api.onrender.com/swagger`

### Features Available
- ✅ Full React frontend with Chakra UI
- ✅ Complete REST API with Swagger docs
- ✅ JWT authentication
- ✅ MongoDB database
- ✅ Image upload (if Cloudinary configured)
- ✅ Email notifications (if SMTP configured)
- ✅ Google Auth (if Firebase configured)

---

## 🔧 Local Development Setup

### Backend
```bash
cd VehicleShowroomManagement
# Your existing setup already works
```

### Frontend
```bash
cd VehicleShowroom

# Copy environment template
cp env.template .env.local

# Edit .env.local with your local API URL
REACT_APP_API_URL=http://localhost:10000/api

# Install and run
npm install
npm start
```

---

## 📊 Free Tier Limits

- **Render**: 750 hours/month per service (sleeps after 15min inactivity)
- **MongoDB**: 512MB storage
- **Cloudinary**: 25GB storage + bandwidth
- **Firebase**: Generous free tier

---

## 🚨 Troubleshooting

### Build Fails
- Check Dockerfile syntax
- Verify all files are committed to GitHub
- Check Render build logs

### Frontend Can't Connect to API
- Verify `REACT_APP_API_URL` is correct
- Check CORS settings in backend
- Ensure backend is running

### Database Issues
- Whitelist all IPs in MongoDB Atlas (0.0.0.0/0)
- Verify database user permissions
- Check connection string format

---

## ⏱️ Total Deployment Time: ~7 minutes
- Blueprint deployment: 5 minutes
- Environment variables: 2 minutes

**Cost: $0/month** (Free tier)

---

## 🎯 Next Steps After Deployment

1. **Test both applications**
2. **Set up custom domains** (optional)
3. **Configure monitoring** (optional)
4. **Set up staging environment** (optional)

Your complete Vehicle Showroom Management system will be live and accessible worldwide! 🌍
