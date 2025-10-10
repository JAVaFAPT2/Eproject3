# Quick Start Guide - Refactored Vehicle Showroom Management

## ✅ Build Status: SUCCESS

The refactoring is complete and the application builds successfully!

## Prerequisites
- .NET 8.0 SDK
- MongoDB (local or MongoDB Atlas)
- Node.js LTS (for frontend)

## 1. Backend Setup

### Start the API
```bash
cd VehicleShowroomManagement/src/WebAPI
dotnet restore
dotnet build
dotnet run
```

**API will be available at:**
- HTTP: http://localhost:8090
- HTTPS: https://localhost:8091
- Swagger: http://localhost:8090/swagger

### First Run
On first run, the system will automatically:
1. Create MongoDB collections with indexes
2. Seed 4 default roles: Admin, HR, Dealer, Customer
3. Create admin user account

## 2. Frontend Setup

### Configure API URL
Create file: `VehicleShowroom/.env`
```
REACT_APP_API_URL=http://localhost:8090/api/
```

### Start the Frontend
```bash
cd VehicleShowroom
npm install
npm start
```

Frontend will open at: http://localhost:3000

## 3. Default Login

**Username**: `admin`  
**Password**: `Admin123!`

## 4. Test the API

### Login Test
```bash
curl -X POST http://localhost:8090/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}'
```

**Response** (copy the token):
```json
{
  "userId": "...",
  "roleName": "Admin",
  "token": "eyJhbGci...",
  "accessToken": "eyJhbGci...",
  "refreshToken": "...",
  "expiresAt": "2024-...",
  "user": { ... }
}
```

### Create Vehicle Model
```bash
curl -X POST http://localhost:8090/api/vehiclemodels \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  -d '{
    "modelNumber": "CIVIC2024",
    "name": "Honda Civic 2024",
    "brand": "Honda",
    "price": 28000.00
  }'
```

### Get Vehicle Models
```bash
curl -X GET "http://localhost:8090/api/vehiclemodels?pageNumber=1&pageSize=10" \
  -H "Authorization: Bearer YOUR_TOKEN_HERE"
```

## 5. Workflow Example

### Complete Purchase to Sale Workflow

```bash
# 1. Login and get token
TOKEN=$(curl -s -X POST http://localhost:8090/api/auth/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"Admin123!"}' \
  | jq -r '.token')

# 2. Create Vehicle Model
curl -X POST http://localhost:8090/api/vehiclemodels \
  -H "Content-Type: application/json" \
  -H "Authorization: Bearer $TOKEN" \
  -d '{
    "modelNumber": "ACCORD2024",
    "name": "Honda Accord 2024",
    "brand": "Honda",
    "price": 32000.00
  }'

# 3. TODO: Create Purchase Order and Lines (endpoints need to be implemented)
# 4. TODO: Complete Purchase Order to create vehicles
# 5. Search for available vehicles
# 6. Create order
# 7. Confirm and complete order
```

## 6. API Endpoints Available

### Authentication
- `POST /api/auth/login` - Login with username/email
- `POST /api/auth/forgot-password` - Request password reset
- `POST /api/auth/reset-password` - Reset password
- `POST /api/auth/refresh-token` - Refresh access token (placeholder)
- `POST /api/auth/revoke-token` - Revoke token

### Users
- `POST /api/users` - Create user (HR/Admin)
- `GET /api/users/{id}` - Get user by ID
- `PUT /api/users/{id}/profile` - Update profile

### Profile
- `GET /api/profile` - Get current user profile
- `PUT /api/profile` - Update current user profile
- `POST /api/profile/change-password` - Change password

### Vehicle Models
- `POST /api/vehiclemodels` - Create model (Dealer/Admin)
- `GET /api/vehiclemodels` - Get all models (paginated)

### Vehicles
- `POST /api/vehicles` - Create vehicle
- `GET /api/vehicles/{id}` - Get vehicle
- `GET /api/vehicles` - Get all vehicles
- `GET /api/vehicles/search` - Search vehicles
- `PUT /api/vehicles/{id}` - Update vehicle
- `PUT /api/vehicles/{id}/status` - Update status
- `DELETE /api/vehicles/{id}` - Delete vehicle
- `POST /api/vehicles/bulk-delete` - Bulk delete

### Orders
- `POST /api/orders` - Create order (Dealer/Admin)
- `POST /api/orders/{id}/assign-vehicle` - Assign vehicle (Dealer/Admin)
- `POST /api/orders/{id}/confirm` - Confirm order (Customer/Dealer/Admin)
- `POST /api/orders/{id}/complete` - Complete order (Dealer/Admin)

### Purchase Orders
- `POST /api/purchaseorders/{id}/complete` - Complete PO (creates vehicles)

### Reports
- `GET /api/reports/stock` - Stock report
- `GET /api/reports/sales` - Sales report
- `GET /api/reports/customers` - Customer analytics

## 7. MongoDB Collections

The system creates these collections:
- `ROLE` - User roles
- `USER` - All users (employees, dealers, customers)
- `VEHICLE_MODEL` - Vehicle models
- `VEHICLE` - Physical vehicles
- `VEHICLE_PHOTO` - Vehicle photos
- `VEHICLE_SPEC` - Vehicle specifications
- `PURCHASE_ORDER` - Purchase orders
- `PURCHASE_ORDER_LINE` - PO line items
- `ORDER` - Customer orders
- `SERVICE_ORDER` - Service orders
- `BILLING_DOCUMENT` - Billing/invoices
- `DOCUMENT_OUTPUT` - Generated documents

## 8. Troubleshooting

### Build Errors
If you encounter build errors:
```bash
cd VehicleShowroomManagement
dotnet clean
dotnet restore
dotnet build
```

### MongoDB Connection
Ensure MongoDB is running:
- Local: `mongodb://localhost:27017`
- Or update connection string in `appsettings.json`

### Frontend Can't Connect
1. Ensure backend is running on port 8090
2. Check `.env` file exists with correct API URL
3. Check browser console for CORS errors

## 9. Development Tips

### View Logs
Backend logs show:
- MongoDB collection initialization
- Index creation
- Seeding results
- API requests

### Test via Swagger
Open http://localhost:8090/swagger
1. Click "Authorize" button
2. Enter: `Bearer YOUR_TOKEN`
3. Test all endpoints interactively

### MongoDB Data
View your data:
```bash
mongosh
use VehicleShowroomDB
db.USER.find()
db.VEHICLE_MODEL.find()
db.ORDER.find()
```

## 10. Security Reminders

⚠️ **Before Production**:
1. Move secrets from `appsettings.json` to user-secrets
2. Rotate JWT key
3. Rotate MongoDB credentials
4. Update CORS policy (currently allows all origins)
5. Implement proper refresh token storage
6. Add rate limiting
7. Enable HTTPS only
8. Implement audit logging

## Support

For issues or questions:
- Check `REFACTORING_CHANGES.md` for detailed changes
- Check `IMPLEMENTATION_STATUS_FINAL.md` for what's implemented
- Review Swagger documentation at `/swagger`

---

**Status**: ✅ Ready for development and testing!

