# Backend-Frontend Connection for Render Deployment

## Overview
This guide explains how to connect the .NET backend API with the React frontend for deployment on Render.

## Architecture
```
┌─────────────────┐    HTTPS     ┌─────────────────┐
│   React Frontend │ ──────────► │  .NET Backend   │
│  (Port 10000)    │              │  (Port 10000)   │
└─────────────────┘              └─────────────────┘
         │                                │
         │                                │
         ▼                                ▼
┌─────────────────┐              ┌─────────────────┐
│   Render Web     │              │   MongoDB       │
│   Service        │              │   Database      │
└─────────────────┘              └─────────────────┘
```

## Configuration Changes Made

### 1. Backend Configuration

#### CORS Settings Updated
- **Production**: `https://vehicleshowroom-web.onrender.com`
- **Docker**: `*` (wildcard for flexibility)

#### Environment Variables
```bash
# Database
ConnectionStrings__MongoDB=<render-db-connection-string>

# JWT
Jwt__Key=<generated-secret-key>
Jwt__Issuer=VehicleShowroomAPI
Jwt__Audience=VehicleShowroomClient

# CORS
Cors__Origins=*
```

### 2. Frontend Configuration

#### API URLs Updated
- **ApiClient.js**: `https://vehicleshowroom-api.onrender.com/api`
- **ApiUrl.js**: `https://vehicleshowroom-api.onrender.com/api`

#### Environment Variables
```bash
REACT_APP_API_URL=https://vehicleshowroom-api.onrender.com/api
REACT_APP_BACKEND_URL=https://vehicleshowroom-api.onrender.com
REACT_APP_NAME=Vehicle Showroom Management
REACT_APP_VERSION=2.0.0
REACT_APP_ENVIRONMENT=production
```

### 3. Render Configuration

#### Service Connection
The `render.yaml` automatically connects services:
```yaml
envVars:
  - key: REACT_APP_API_URL
    fromService:
      type: web
      name: vehicleshowroom-api
      property: host
      format: https://${host}/api
```

## Deployment Steps

### 1. Backend Deployment
1. **Database Setup**:
   - Create MongoDB database on Render
   - Note the connection string

2. **Environment Variables**:
   ```bash
   ASPNETCORE_ENVIRONMENT=Production
   ConnectionStrings__MongoDB=<your-mongodb-connection-string>
   Jwt__Key=<generate-32-char-secret>
   Jwt__Issuer=VehicleShowroomAPI
   Jwt__Audience=VehicleShowroomClient
   ```

3. **Deploy Backend**:
   - Push to GitHub
   - Connect to Render
   - Use `render.yaml` configuration

### 2. Frontend Deployment
1. **Environment Variables** (Auto-configured):
   - `REACT_APP_API_URL` - Auto-set from backend service
   - `REACT_APP_BACKEND_URL` - Auto-set from backend service

2. **Deploy Frontend**:
   - Push to GitHub
   - Connect to Render
   - Use `render.yaml` configuration

## API Endpoints

### Authentication
- `POST /api/auth/login` - User login
- `POST /api/auth/register` - User registration
- `POST /api/auth/refresh-token` - Token refresh

### Vehicle Management
- `GET /api/VehicleModels` - Get vehicle models
- `GET /api/Vehicles` - Get vehicles
- `POST /api/Orders` - Create order

### Dashboard
- `GET /api/dashboard/overview` - Dashboard overview
- `GET /api/dashboard/revenue` - Revenue data

## Testing Connection

### 1. Health Check
```bash
curl https://vehicleshowroom-api.onrender.com/health
```

### 2. API Test
```bash
curl https://vehicleshowroom-api.onrender.com/api/VehicleModels
```

### 3. Frontend Test
- Visit: `https://vehicleshowroom-web.onrender.com`
- Check browser console for API calls
- Verify authentication flow

## Troubleshooting

### Common Issues

1. **CORS Errors**:
   - Check CORS configuration in backend
   - Verify frontend URL in CORS origins

2. **API Connection Failed**:
   - Verify `REACT_APP_API_URL` environment variable
   - Check backend service is running
   - Test API endpoints directly

3. **Authentication Issues**:
   - Verify JWT configuration
   - Check token expiration settings
   - Ensure proper token handling in frontend

### Debug Steps

1. **Check Backend Logs**:
   ```bash
   # In Render dashboard
   View Logs → Backend Service
   ```

2. **Check Frontend Logs**:
   ```bash
   # In Render dashboard
   View Logs → Frontend Service
   ```

3. **Test API Directly**:
   ```bash
   curl -X GET https://vehicleshowroom-api.onrender.com/api/VehicleModels \
        -H "Content-Type: application/json"
   ```

## Security Considerations

1. **JWT Secret**: Use strong, randomly generated keys
2. **CORS**: Restrict to specific domains in production
3. **HTTPS**: All communication over HTTPS
4. **Environment Variables**: Never commit secrets to code

## Performance Optimizations

1. **Caching**: Implement Redis for session storage
2. **CDN**: Use Cloudinary for image delivery
3. **Database**: Optimize MongoDB queries
4. **Frontend**: Implement lazy loading and code splitting

## Monitoring

1. **Health Checks**: `/health` endpoint
2. **Logging**: Structured logging with Serilog
3. **Metrics**: Application performance monitoring
4. **Alerts**: Set up error notifications

## Next Steps

1. **Deploy Backend**: Follow backend deployment steps
2. **Deploy Frontend**: Follow frontend deployment steps
3. **Test Connection**: Verify API communication
4. **Monitor**: Set up monitoring and alerts
5. **Optimize**: Implement performance improvements
