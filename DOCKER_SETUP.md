# Vehicle Showroom Management API

## 🚀 Quick Start

### Using Docker Compose (Recommended)
```bash
# Build and run the application
docker-compose up --build

# Run in background
docker-compose up -d --build

# View logs
docker-compose logs -f

# Stop the application
docker-compose down
```

### Using Docker directly
```bash
# Build the image
docker build -t vehicleshowroom-api .

# Run the container
docker run -p 10000:10000 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e DOTNET_RUNNING_IN_CONTAINER=true \
  -v $(pwd)/data/keys:/app/keys \
  -v $(pwd)/logs:/app/logs \
  vehicleshowroom-api
```

## 🔧 Configuration

### Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Set to `Production` for production deployment
- `DOTNET_RUNNING_IN_CONTAINER`: Set to `true` when running in Docker
- `PORT`: Port number (default: 10000)

### Data Protection Keys
The application automatically creates and persists data protection keys in `/app/keys` directory when running in Docker. This ensures:
- ✅ Keys persist across container restarts
- ✅ No warnings about ephemeral key storage
- ✅ Secure key management

### HTTPS Configuration
- **Development**: HTTPS redirection is enabled
- **Production/Docker**: HTTPS redirection is disabled (HTTP only)
- **Health Check**: Available at `http://localhost:10000/health`

## 📊 Monitoring

### Health Check Endpoint
```bash
curl http://localhost:10000/health
```

### API Documentation
- **Swagger UI**: `http://localhost:10000/swagger`
- **OpenAPI Spec**: `http://localhost:10000/swagger/v1/swagger.json`

## 🐛 Troubleshooting

### Common Issues Fixed
1. **Data Protection Keys Warning**: ✅ Fixed with persistent key storage
2. **HTTPS Redirect Warning**: ✅ Fixed with conditional HTTPS redirection
3. **Port Configuration**: ✅ Properly configured for Docker

### Logs
Application logs are written to:
- Console output
- `/app/logs/vehicleshowroom-*.txt` files

### Debugging
```bash
# View container logs
docker logs <container-id>

# Access container shell
docker exec -it <container-id> /bin/bash

# Check health status
curl http://localhost:10000/health
```

## 🔒 Security Notes

- Data protection keys are stored in `/app/keys` directory
- Keys are automatically created with 90-day lifetime
- Application name is set to "VehicleShowroomManagement"
- CORS is configured for frontend integration
