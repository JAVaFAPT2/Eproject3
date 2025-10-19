# Render Deployment Checklist

## ✅ Pre-Deployment Configuration

### Backend (.NET API)
- [x] **CORS Configuration**: Updated for Render domains
- [x] **Environment Variables**: Configured for production
- [x] **Database Connection**: MongoDB connection string ready
- [x] **JWT Settings**: Secret key and issuer configured
- [x] **Docker Configuration**: Optimized for Render deployment
- [x] **Health Endpoint**: `/health` endpoint available

### Frontend (React)
- [x] **API URLs**: Updated to use Render backend URL
- [x] **Environment Variables**: Configured for production
- [x] **Build Optimization**: CRACO configuration added
- [x] **Performance**: LCP optimizations implemented
- [x] **Service Worker**: Caching strategy implemented

### Render Configuration
- [x] **render.yaml**: Service connection configured
- [x] **Environment Variables**: Auto-mapped between services
- [x] **Database**: MongoDB database configured
- [x] **CORS**: Cross-origin requests enabled

## 🚀 Deployment Steps

### 1. Database Setup
```bash
# Create MongoDB database on Render
# Note the connection string for backend configuration
```

### 2. Backend Deployment
```bash
# 1. Push code to GitHub
git add .
git commit -m "Configure backend for Render deployment"
git push origin main

# 2. Connect to Render
# - Go to Render Dashboard
# - Create New Web Service
# - Connect GitHub repository
# - Use render.yaml configuration
```

### 3. Frontend Deployment
```bash
# 1. Push code to GitHub
git add .
git commit -m "Configure frontend for Render deployment"
git push origin main

# 2. Connect to Render
# - Create New Web Service
# - Connect GitHub repository
# - Use render.yaml configuration
```

## 🔧 Environment Variables

### Backend Environment Variables
```bash
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__MongoDB=<render-mongodb-connection-string>
Jwt__Key=<generate-32-character-secret>
Jwt__Issuer=VehicleShowroomAPI
Jwt__Audience=VehicleShowroomClient
EmailSettings__SmtpHost=smtp.gmail.com
EmailSettings__SmtpPort=587
EmailSettings__SmtpUsername=<your-email>
EmailSettings__SmtpPassword=<your-app-password>
CloudinarySettings__CloudName=<your-cloud-name>
CloudinarySettings__ApiKey=<your-api-key>
CloudinarySettings__ApiSecret=<your-api-secret>
```

### Frontend Environment Variables (Auto-configured)
```bash
REACT_APP_API_URL=https://vehicleshowroom-api.onrender.com/api
REACT_APP_BACKEND_URL=https://vehicleshowroom-api.onrender.com
REACT_APP_NAME=Vehicle Showroom Management
REACT_APP_VERSION=2.0.0
REACT_APP_ENVIRONMENT=production
```

## 🧪 Testing

### 1. Health Check
```bash
curl https://vehicleshowroom-api.onrender.com/health
# Expected: {"status": "healthy"}
```

### 2. API Test
```bash
curl https://vehicleshowroom-api.onrender.com/api/VehicleModels
# Expected: JSON response with vehicle models
```

### 3. Frontend Test
- Visit: `https://vehicleshowroom-web.onrender.com`
- Check browser console for API calls
- Test authentication flow
- Verify data loading

## 🔍 Troubleshooting

### Common Issues

1. **CORS Errors**
   - Check CORS configuration in backend
   - Verify frontend URL in CORS origins
   - Ensure HTTPS is used

2. **API Connection Failed**
   - Verify `REACT_APP_API_URL` environment variable
   - Check backend service is running
   - Test API endpoints directly

3. **Authentication Issues**
   - Verify JWT configuration
   - Check token expiration settings
   - Ensure proper token handling

### Debug Commands
```bash
# Check backend logs
# In Render dashboard → View Logs

# Test API directly
curl -X GET https://vehicleshowroom-api.onrender.com/api/VehicleModels \
     -H "Content-Type: application/json"

# Check frontend build
npm run build
```

## 📊 Monitoring

### Health Checks
- Backend: `https://vehicleshowroom-api.onrender.com/health`
- Frontend: Check browser console for errors

### Performance Monitoring
- Use browser DevTools → Lighthouse
- Monitor Core Web Vitals
- Check API response times

## 🔒 Security Checklist

- [ ] **JWT Secret**: Strong, randomly generated key
- [ ] **CORS**: Restricted to specific domains
- [ ] **HTTPS**: All communication over HTTPS
- [ ] **Environment Variables**: No secrets in code
- [ ] **Database**: Secure connection string
- [ ] **Email**: App-specific password for SMTP

## 📈 Performance Optimization

- [x] **Code Splitting**: Lazy loading implemented
- [x] **Bundle Optimization**: Webpack splitting configured
- [x] **Caching**: Service worker implemented
- [x] **Image Optimization**: Lazy loading and proper formats
- [x] **API Optimization**: Deferred non-critical calls

## 🎯 Success Criteria

- [ ] Backend API responds to health check
- [ ] Frontend loads without errors
- [ ] API calls work from frontend
- [ ] Authentication flow functions
- [ ] Data loads correctly
- [ ] Performance metrics are good (LCP < 2.5s)

## 📞 Support

If issues persist:
1. Check Render service logs
2. Verify environment variables
3. Test API endpoints directly
4. Check browser console for errors
5. Review CORS configuration
