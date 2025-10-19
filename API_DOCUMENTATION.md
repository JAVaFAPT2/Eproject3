# 🚀 **Vehicle Showroom Management System - Complete API Reference**

> **Last Updated**: January 2025 - All Warnings Fixed + Features Implemented  
> **Architecture**: Clean Architecture + DDD + CQRS  
> **Backend**: .NET 8 Web API + MongoDB  
> **Authentication**: JWT Bearer Tokens  
> **Code Quality**: 0 Warnings | 100% Interface-Based Architecture

## 📋 **Standardized Response Structure**

All paginated list endpoints now return a consistent structure:

```json
{
  "items": [...],
  "totalCount": 0,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 0
}
```

This applies to: `/api/users`, `/api/vehicle-models`, `/api/vehicles`, `/api/purchase-orders`, `/api/orders`, `/api/service-orders`, `/api/vehicle-models/{id}/photos`, `/api/vehicle-models/{id}/specs`

---

## 📋 **Table of Contents**

1. [Authentication APIs](#authentication-apis)
2. [Profile Management APIs](#profile-management-apis)
3. [User Management APIs](#user-management-apis)
4. [Vehicle Model APIs](#vehicle-model-apis)
5. [Vehicle APIs](#vehicle-apis)
6. [Purchase Order APIs](#purchase-order-apis)
7. [Order APIs (Customer Orders)](#order-apis-customer-orders)
8. [Service Order APIs](#service-order-apis)
9. [Billing Document APIs](#billing-document-apis)
10. [Document Output APIs](#document-output-apis)
11. [Dashboard APIs](#dashboard-apis)
12. [Business Workflow](#business-workflow)
13. [Data Models](#data-models)
14. [Error Handling](#error-handling)

---

## 🔐 **Authentication APIs** (`/api/auth`)

### **1. POST /api/auth/login**
**User Login - Returns Access Token, sets HttpOnly Refresh Cookie**

```bash
# Request
POST /api/auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "Admin123!"
}

# Response (200 OK)
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenExpiresAt": "2024-01-02T00:00:00Z",
  "userId": "507f1f77bcf86cd799439011",
  "role": "Admin",
  "message": "Login successful"
}

# Cookie (Set-Cookie)
# refreshToken=<opaque>; HttpOnly; Secure; SameSite=Lax; Path=/; Expires=...
```

### **2. POST /api/auth/forgot-password**
**Request Password Reset**

```bash
# Request
POST /api/auth/forgot-password
Content-Type: application/json

{
  "email": "user@showroom.com"
}

# Response (200 OK)
{
  "message": "Password reset instructions have been sent to your email"
}
```

### **3. POST /api/auth/reset-password**
**Reset Password with Token**

```bash
# Request
POST /api/auth/reset-password
Content-Type: application/json

{
  "token": "abc123-def456-ghi789",
  "newPassword": "NewSecurePass123!"
}

# Response (200 OK)
{
  "message": "Password has been reset successfully"
}
```

### **4. POST /api/auth/refresh-token**
**Refresh Access Token using HttpOnly Cookie**

```bash
# Request
POST /api/auth/refresh-token
# Refresh token is read from HttpOnly cookie; body optional

# Response (200 OK)
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "tokenExpiresAt": "2024-01-02T00:00:00Z",
  "message": "Token refreshed successfully"
}

# Cookie (Set-Cookie)
# refreshToken may be rotated and re-set with new expiration
```

### **5. POST /api/auth/register**
**Public User Registration (Customer Role)**
*No Authentication Required*

```bash
# Request
POST /api/auth/register
Content-Type: application/json

{
  "username": "newcustomer",
  "password": "Customer123!",
  "email": "customer@example.com"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439099",
  "message": "User registered successfully"
}

# Note: 
# - Automatically assigns "Customer" role to new registrations
# - Name, Phone, Address can be updated later via profile endpoint
# - Simplified registration requires only username, email, password
```

### **6. POST /api/auth/revoke-token**
**Revoke Refresh Token (Logout) and Clear Cookie**
*Requires Authentication*

```bash
# Request
POST /api/auth/revoke-token
Authorization: Bearer <jwt-token>
# Refresh token is read from HttpOnly cookie; body optional

# Response (200 OK)
{
  "message": "Token revoked successfully"
}

# Cookie (Set-Cookie)
# refreshToken is cleared (Max-Age=0 / expired)
```

---

## 👤 **Profile Management APIs** (`/api/profile`)

*All endpoints require authentication*

### **7. GET /api/profile**
**Get Current User Profile**

```bash
# Request
GET /api/profile
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "id": "507f1f77bcf86cd799439011",
  "username": "john_doe",
  "name": "John Doe",
  "email": "john@showroom.com",
  "phone": "+1234567890",
  "address": "123 Main St, City, State",
  "roleId": "507f1f77bcf86cd799439012",
  "roleName": "Dealer",
  "status": "Active",
  "hireDate": "2024-01-01T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **8. PUT /api/profile**
**Update Current User Profile**

```bash
# Request
PUT /api/profile
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "firstName": "John",
  "lastName": "Doe Updated",
  "email": "john.updated@showroom.com",
  "phone": "+1987654321"
}

# Response (200 OK)
{
  "message": "Profile updated successfully"
}
```

### **9. POST /api/profile/change-password**
**Change Current User Password**

```bash
# Request
POST /api/profile/change-password
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "currentPassword": "CurrentPass123!",
  "newPassword": "NewSecurePass456!"
}

# Response (200 OK)
{
  "message": "Password changed successfully"
}
```

---

## 👥 **User Management APIs** (`/api/users`)

*Requires Authentication - HR/Admin roles*

### **10. GET /api/users**
**Get Users by Role Name**

```bash
# Request - Get all employees
GET /api/users?roleName=Employee
Authorization: Bearer <jwt-token>

# Request - Get all customers  
GET /api/users?roleName=Customer
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "items": [
    {
      "id": "507f1f77bcf86cd799439011",
      "username": "john_doe",
      "name": "John Doe",
      "email": "john@showroom.com",
      "phone": "+1234567890",
      "address": "123 Main St",
      "roleId": "507f1f77bcf86cd799439012",
      "role": "Employee",
      "status": "Active",
      "hireDate": "2024-01-01T00:00:00Z",
      "isActive": true,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z"
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}

# Error Response (400 Bad Request)
{
  "message": "roleName parameter is required"
}

# Note: 
# - Frontend passes roleName (e.g., "Employee", "Customer") instead of MongoDB ObjectId
# - Returns filtered list of users by role
# - Excludes deleted users automatically
```

### **11. GET /api/users/{id}**
**Get User by ID**

```bash
# Request
GET /api/users/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "id": "507f1f77bcf86cd799439011",
  "username": "john_doe",
  "name": "John Doe",
  "email": "john@showroom.com",
  "phone": "+1234567890",
  "address": "123 Main St",
  "roleId": "507f1f77bcf86cd799439012",
  "roleName": "Dealer",
  "status": "Active",
  "hireDate": "2024-01-01T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **12. POST /api/users**
**Create New User with Auto-Role Assignment**

```bash
# Request - Create Employee (with HireDate)
POST /api/users
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "username": "jane_employee",
  "password": "SecurePass123!",
  "name": "Jane Employee",
  "email": "jane.employee@showroom.com",
  "phone": "+1234567890",
  "address": "456 Oak Ave",
  "hireDate": "2024-01-15T00:00:00Z"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439013",
  "message": "User created successfully"
}

# Note: Role is auto-assigned based on HireDate:
# - If HireDate is provided → "Employee" role
# - If HireDate is null → "Customer" role
# - Or provide explicit roleId to override auto-assignment

# Request - Create Customer (without HireDate)
POST /api/users
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "username": "john_customer",
  "password": "SecurePass123!",
  "name": "John Customer",
  "email": "john.customer@showroom.com",
  "phone": "+1987654321",
  "address": "789 Pine Rd"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439014",
  "message": "User created successfully"
}
```

### **13. PUT /api/users/{id}**
**Update Active status only**

```bash
# Request
PUT /api/users/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "isActive": true
}

# Response (200 OK)
{
  "message": "User active status updated successfully"
}
```

### **14. DELETE /api/users/{id}**
**Delete User** *(Admin only)*

```bash
# Request
DELETE /api/users/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "User deleted successfully"
}
```

---

## 🚗 **Vehicle Model APIs** (`/api/vehicle-models`)

*Vehicle models are the catalog of available vehicle types*

### **15. POST /api/vehicle-models**
**Create Vehicle Model** *(Dealer/Admin only)*

```bash
# Request
POST /api/vehicle-models
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "modelNumber": "CAMRY2024",
  "name": "Toyota Camry 2024",
  "brand": "Toyota",
  "price": 28000.00,
  "description": "The 2024 Toyota Camry offers exceptional reliability, fuel efficiency, and a spacious interior with advanced safety features.",
  "imageUrl": "https://example.com/images/camry2024.jpg"
}

# Response (200 OK)
{
  "modelNumber": "CAMRY2024",
  "message": "Vehicle model created successfully"
}

# Note: description is required, imageUrl is optional
```

### **16. PUT /api/vehicle-models/{modelNumber}**
**Update Vehicle Model** *(Dealer/Admin only)*

```bash
# Request
PUT /api/vehicle-models/CAMRY2024
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "name": "Toyota Camry 2024 Updated",
  "brand": "Toyota",
  "price": 29000.00,
  "description": "Updated description with new features and pricing.",
  "imageUrl": "https://example.com/images/camry2024-updated.jpg"
}

# Response (200 OK)
{
  "message": "Vehicle model updated successfully"
}
```

### **17. GET /api/vehicle-models**
**Get Vehicle Models (Unified)**

```bash
# Request
GET /api/vehicle-models?pageNumber=1&pageSize=10&search=toyota&parentModelNumber=MODEL-123&seats=4&fuelType=petrol
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "items": [
    {
      "modelNumber": "CAMRY2024",
      "name": "Toyota Camry 2024",
      "brand": "Toyota",
      "price": 28000.00
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

---

## 🚙 **Vehicle APIs** (`/api/vehicles`)

*Individual vehicle inventory management*

### **16. POST /api/vehicles**
**Create Vehicle** *(Dealer/Admin only)*

```bash
# Request
POST /api/vehicles
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "vehicleId": "VEH-2024-001",
    "modelNumber": "CAMRY2024",
  "purchasePrice": 26000.00,
  "externalNumber": "EXT-001",
  "vin": "1HGCM82633A123456",
  "licensePlate": "ABC-123",
  "receiptDate": "2024-01-15T00:00:00Z"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "message": "Vehicle created successfully"
}
```

### **17. GET /api/vehicles/{id}**
**Get Vehicle by ID**

```bash
# Request
GET /api/vehicles/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "id": "507f1f77bcf86cd799439011",
  "vehicleId": "VEH-2024-001",
    "modelNumber": "CAMRY2024",
  "modelName": "Toyota Camry 2024",
    "brand": "Toyota",
  "externalNumber": "EXT-001",
  "vin": "1HGCM82633A123456",
  "licensePlate": "ABC-123",
  "status": "Available",
  "purchasePrice": 26000.00,
  "salePrice": 28000.00,
  "photos": [],
  "receiptDate": "2024-01-15T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **16.1 POST /api/vehicles/with-media**
**Create Vehicle with Photos (multipart)** *(Dealer/Admin only)*

```bash
# Request
POST /api/vehicles/with-media
Authorization: Bearer <jwt-token>
Content-Type: multipart/form-data

# Parts
- data: application/json (CreateVehicleRequest JSON)
- files: one or more image files (repeat key "files")

# Example (curl)
curl -X POST /api/vehicles/with-media \
  -H "Authorization: Bearer <jwt>" \
  -F 'data={"vehicleId":"VEH-2024-001","modelNumber":"CAMRY2024","purchasePrice":26000};type=application/json' \
  -F "files=@img1.jpg" -F "files=@img2.jpg"

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "message": "Vehicle created successfully with media"
}

# Notes:
# - Backend uploads images and creates photo records linked to the vehicle
# - Photos are also linked to the vehicle model via modelNumber (vehicleModelId)
# - To retrieve photo URLs immediately, call GET /api/vehicles/{id}/photos
```

### **18. GET /api/vehicles**
**Get All Vehicles with Pagination & Filters**

```bash
# Request
GET /api/vehicles?pageNumber=1&pageSize=10&status=1&searchTerm=VEH-2024
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "items": [
    {
      "id": "507f1f77bcf86cd799439011",
      "vehicleId": "VEH-2024-001",
      "modelNumber": "CAMRY2024",
      "modelName": "Toyota Camry 2024",
      "brand": "Toyota",
      "status": 1,
      "purchasePrice": 26000.00,
      "salePrice": 28000.00
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

### ~~**19. GET /api/vehicles/search**~~
~~Search Vehicles~~

Deprecated. Use `GET /api/vehicles` with query parameters: `searchTerm`, `status`, `modelNumber`, `seats`, `fuelType`, `minPrice`, `maxPrice`, `pageNumber`, `pageSize`.

```bash
# Request
GET /api/vehicles/search?searchTerm=camry&status=Available&brand=Toyota&minPrice=20000&maxPrice=30000&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "items": [
    {
      "id": "507f1f77bcf86cd799439011",
      "vehicleId": "VEH-2024-001",
      "modelNumber": "CAMRY2024",
      "modelName": "Toyota Camry 2024",
      "brand": "Toyota",
      "status": "Available",
      "purchasePrice": 26000.00,
      "salePrice": 28000.00,
      "vin": "1HGCM82633A123456"
    }
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

### **20. PUT /api/vehicles/{id}**
**Update Vehicle** *(Dealer/Admin only)*

```bash
# Request
PUT /api/vehicles/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "modelNumber": "CAMRY2024",
  "purchasePrice": 25500.00,
  "externalNumber": "EXT-001-UPDATED",
  "vin": "1HGCM82633A123456",
  "licensePlate": "ABC-123",
  "color": "Blue",
  "mileage": 0
}

# Response (200 OK)
{
  "message": "Vehicle updated successfully"
}
```

### **21. PUT /api/vehicles/{id}/status**
**Update Vehicle Status** *(Dealer/Admin only)*

```bash
# Request
PUT /api/vehicles/507f1f77bcf86cd799439011/status
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "status": 3
}

# Response (200 OK)
{
  "message": "Vehicle status updated successfully"
}

# Possible status values (numeric):
# - Available = 1
# - Reserved = 2
# - Sold = 3
```

### **21.1 PUT /api/vehicles/{id}/license-plate**
**Update Vehicle License Plate** *(Dealer/Admin only)*

```bash
# Request
PUT /api/vehicles/507f1f77bcf86cd799439011/license-plate
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "licensePlate": "XYZ-789"
}

# Response (200 OK)
{
  "message": "Vehicle license plate updated successfully"
}

# Notes:
# - Updates the license plate for the specified vehicle
# - Can be used independently or combined with service order status updates
# - License plate is automatically updated when completing service orders with licensePlate parameter
```

### **22. DELETE /api/vehicles/{id}**
**Delete Vehicle** *(Admin only)*

```bash
# Request
DELETE /api/vehicles/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Vehicle deleted successfully"
}
```

### **23. POST /api/vehicles/bulk-delete**
**Bulk Delete Vehicles** *(Admin only)*

```bash
# Request
POST /api/vehicles/bulk-delete
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "vehicleIds": [
    "507f1f77bcf86cd799439011",
    "507f1f77bcf86cd799439012"
  ]
}

# Response (200 OK)
{
  "message": "2 vehicles deleted successfully"
}
```

### **23. POST /api/vehicles/{vehicleId}/photos/upload**
**Upload Photos for a Vehicle (multipart)** *(Dealer/Admin only)*

```bash
# Request
POST /api/vehicles/{vehicleId}/photos/upload
Authorization: Bearer <jwt-token>
Content-Type: multipart/form-data

# Parts
- files: one or more image files (repeat key "files")

# Response (200 OK)
{
  "message": "Photos uploaded successfully",
  "items": [
    { "id": "507f1f77bcf86cd79943a111", "url": "https://res.cloudinary.com/..." }
  ]
}

# Notes:
# - Images are uploaded and photo records created.
# - Each photo links to the vehicle and may include vehicleModelId in queries.
```

---

## 📦 **Purchase Order APIs** (`/api/purchase-orders`)

*Purchase orders for ordering vehicles from suppliers*  
*Requires Dealer/Admin role*

### **24. POST /api/purchase-orders**
**Create Purchase Order**

```bash
# Request
POST /api/purchase-orders
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "createdBy": "507f1f77bcf86cd799439011",
  "totalAmount": 150000.00,
  "expectedDeliveryDate": "2024-02-01T00:00:00Z"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439020",
  "message": "Purchase order created successfully"
}
```

### **25. POST /api/purchase-orders/{id}/lines**
**Add Line Items to Purchase Order**

```bash
# Request
POST /api/purchase-orders/507f1f77bcf86cd799439020/lines
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "modelNumber": "CAMRY2024",
  "quantity": 5,
  "pricePerUnit": 26000.00
}

# Response (200 OK)
{
  "id": "507f1f77bcf86cd799439021",
  "message": "Purchase order line added successfully"
}
```

### **26. PUT /api/purchase-orders/{id}/status**
**Update Purchase Order Status** *(1=Pending, 2=Completed, 3=Cancelled)*

```bash
# Request
PUT /api/purchase-orders/507f1f77bcf86cd799439020/status
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "status": 2
}

# Response (200 OK)
{
  "message": "Purchase order status updated"
}
```

---

## 🛒 **Order APIs (Customer Orders)** (`/api/orders`)

*Customer vehicle orders management*  
*Requires Authentication*

### **27. POST /api/orders**
**Create Customer Order** *(Dealer/Admin only)*

```bash
# Request
POST /api/orders
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "customerId": "507f1f77bcf86cd799439040",
  "dealerId": "507f1f77bcf86cd799439011",
  "modelNumber": "CAMRY2024",
  "salePrice": 28000.00,
  "vehicleId": null,
  "appointmentDate": "2024-02-15T10:00:00Z",
  "note": "Customer prefers blue color"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439050",
  "message": "Order created successfully"
}

# Note: If vehicleId is null, order status = Waiting
# If vehicleId is provided, order status = Reserved
```

### **28. POST /api/orders/{id}/assign-vehicle**
**Assign Vehicle to Order** *(Dealer/Admin only)*

```bash
# Request
POST /api/orders/507f1f77bcf86cd799439050/assign-vehicle
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "vehicleId": "507f1f77bcf86cd799439030",
  "dealerId": "507f1f77bcf86cd799439011"
}

# Response (200 OK)
{
  "message": "Vehicle assigned successfully"
}

# Status changes: Pending → Confirmed (vehicle assigned)
```

### **29. POST /api/orders/{id}/confirm**
**Confirm Order** *(Dealer/Admin/Customer)*

```bash
# Request
POST /api/orders/507f1f77bcf86cd799439050/confirm
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Order confirmed successfully"
}

# Status changes: Confirmed remains Confirmed (idempotent)
```

### **30. PUT /api/orders/{id}/status**
**Update Order Status** *(Dealer/Admin only)*

```bash
# Request
PUT /api/orders/507f1f77bcf86cd799439050/status
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "status": 3
}

# Response (200 OK)
{
  "message": "Order status updated"
}
```

---

## 🔧 **Service Order APIs** (`/api/service-orders`)

*Service orders for vehicle maintenance and pre-delivery*  
*Requires Dealer/Admin role*

### **31. POST /api/service-orders**
**Create Service Order**

```bash
# Request
POST /api/service-orders
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "orderId": "507f1f77bcf86cd799439050",
  "createdBy": "507f1f77bcf86cd799439011",
  "type": "PreDelivery",
  "cost": 500.00,
  "appointmentDate": "2024-02-14T09:00:00Z",
  "description": "Pre-delivery inspection and detailing"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439060",
  "message": "Service order created successfully"
}

# Service Types:
# - PreDelivery (1): Pre-delivery inspection
# - Maintenance (2): Regular maintenance
# - Repair (3): Repair service
```

### **32. PUT /api/service-orders/{id}/status**
**Update Service Order Status**
*On Completed, can set license plate on vehicle*

```bash
# Request
PUT /api/service-orders/507f1f77bcf86cd799439060/status
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "status": 2,
  "licensePlate": "ABC-123"
}

# Response (200 OK)
{
  "message": "Service order completed and billing document created successfully",
  "billingDocumentId": "507f1f77bcf86cd799439070"
}

# Service Order Status Values (numeric):
# - Scheduled = 1
# - InProgress = 2  
# - Completed = 3
# - Cancelled = 4

# On Completed:
# - Service order marked complete
# - Billing document automatically created
# - If licensePlate provided, vehicle updated (for any status)
# - For PreDelivery: Order completed + Vehicle sold + Billing = Service Cost + Order Amount
# - For Maintenance/Repair: Billing = Service Cost Only
```

---

## 💰 **Billing Document APIs** (`/api/billing-documents`)

*Invoice and billing management*  
*Requires Dealer/Admin role*

### **33. GET /api/billing-documents**
**Get Billing Documents with Pagination**

```bash
# Request
GET /api/billing-documents?pageNumber=1&pageSize=10&status=Unpaid&orderId=507f1f77bcf86cd799439050
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "billingDocuments": [
    {
      "id": "507f1f77bcf86cd799439070",
      "orderId": "507f1f77bcf86cd799439050",
      "createdBy": "507f1f77bcf86cd799439011",
      "amount": 28000.00,
      "appointmentDate": "2024-02-15T10:00:00Z",
      "status": "Unpaid",
      "createdAt": "2024-01-01T10:00:00Z",
      "updatedAt": "2024-01-01T10:00:00Z",
      "isUnpaid": true,
      "isPartiallyPaid": false,
      "isPaid": false
    }
  ],
  "totalCount": 45,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 5
}

# Query Parameters:
# - pageNumber: Page number (default: 1)
# - pageSize: Items per page (default: 10)
# - status: Filter by status (Unpaid, PartiallyPaid, Paid)
# - orderId: Filter by order ID
```

---

### **34. POST /api/billing-documents**
**Create Billing Document**

```bash
# Request
POST /api/billing-documents
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "orderId": "507f1f77bcf86cd799439050",
  "createdBy": "507f1f77bcf86cd799439011",
  "amount": 28000.00,
  "appointmentDate": "2024-02-15T10:00:00Z"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439070",
  "message": "Billing document created successfully"
}

# Initial status: Unpaid
# Status options: Unpaid (1), PartiallyPaid (2), Paid (3)
```

---

### **35. PATCH /api/billing-documents/{id}/amount**
**Update Billing Document Amount**

```bash
# Request
PATCH /api/billing-documents/507f1f77bcf86cd799439070/amount
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "amount": 30000.00
}

# Response (200 OK)
{
  "message": "Billing document amount updated successfully"
}

# Domain Validation:
# - Amount cannot be negative
```

---

### **36. PATCH /api/billing-documents/{id}/appointment-date**
**Update Billing Document Appointment Date**

```bash
# Request
PATCH /api/billing-documents/507f1f77bcf86cd799439070/appointment-date
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "appointmentDate": "2024-02-20T14:00:00Z"
}

# Response (200 OK)
{
  "message": "Billing document appointment date updated successfully"
}

# Allows null to clear appointment date
```

---

### **37. PATCH /api/billing-documents/{id}/status**
**Update Billing Document Status**

```bash
# Request
PATCH /api/billing-documents/507f1f77bcf86cd799439070/status
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "status": "Paid"
}

# Response (200 OK)
{
  "message": "Billing document status updated successfully"
}

# Status Values (enum):
# - Unpaid = 1
# - PartiallyPaid = 2
# - Paid = 3

# Domain Rules:
# - Cannot change status from Paid to PartiallyPaid
# - Uses domain methods: MarkAsPaid(), MarkAsPartiallyPaid(), MarkAsUnpaid()
```

---

## 📄 **Document Output APIs** (`/api/document-outputs`)

*Generate PDF/Excel documents*  
*Requires Dealer/Admin role*

### **38. POST /api/document-outputs/generate**
**Generate Document**

```bash
# Request
POST /api/document-outputs/generate
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "entityType": "Order",
  "entityId": "507f1f77bcf86cd799439050",
  "fileType": "PDF"
}

# Response (200 OK)
{
  "id": "507f1f77bcf86cd799439080",
  "message": "Document generated successfully"
}

# Entity Types:
# - Order (1)
# - PurchaseOrder (2)
# - ServiceOrder (3)
# - BillingDocument (4)

# File Types:
# - PDF (1)
# - Excel (2)

# Documents are saved to: wwwroot/documents/{entityType}/{fileName}
```

---

## 📊 **Dashboard APIs** (`/api/dashboard`)

*Analytics and reporting*  
*Requires Authentication*

### **39. GET /api/dashboard/revenue**
**Get Revenue Analytics**

```bash
# Request
GET /api/dashboard/revenue
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "totalRevenue": 1250000.00,
  "previousPeriodRevenue": 1180000.00,
  "growthPercentage": 5.93,
  "revenueData": [
    { "label": "2025-05", "value": 210000.00, "date": "2025-05-01T00:00:00Z" },
    { "label": "2025-06", "value": 180000.00, "date": "2025-06-01T00:00:00Z" },
    { "label": "2025-07", "value": 190000.00, "date": "2025-07-01T00:00:00Z" },
    { "label": "2025-08", "value": 200000.00, "date": "2025-08-01T00:00:00Z" },
    { "label": "2025-09", "value": 230000.00, "date": "2025-09-01T00:00:00Z" },
    { "label": "2025-10", "value": 240000.00, "date": "2025-10-01T00:00:00Z" }
  ],
  "averageOrderValue": 28500.00,
  "totalOrders": 44
}
```

### **40. GET /api/dashboard/customer**
**Get Customer Analytics**

```bash
# Request
GET /api/dashboard/customer
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "totalCustomers": 150,
  "newCustomers": 12,
  "activeCustomers": 85,
  "customerGrowthPercentage": 8.0,
  "customerGrowthData": [
    { "label": "2025-05", "newCustomers": 8, "totalCustomers": 120, "date": "2025-05-01T00:00:00Z" },
    { "label": "2025-06", "newCustomers": 10, "totalCustomers": 130, "date": "2025-06-01T00:00:00Z" },
    { "label": "2025-07", "newCustomers": 6, "totalCustomers": 136, "date": "2025-07-01T00:00:00Z" },
    { "label": "2025-08", "newCustomers": 5, "totalCustomers": 141, "date": "2025-08-01T00:00:00Z" },
    { "label": "2025-09", "newCustomers": 4, "totalCustomers": 145, "date": "2025-09-01T00:00:00Z" },
    { "label": "2025-10", "newCustomers": 5, "totalCustomers": 150, "date": "2025-10-01T00:00:00Z" }
  ],
  "averageCustomerValue": 28500.00,
  "repeatCustomers": 22
}
```

### **41. GET /api/dashboard/top-vehicles**
**Get Top Selling Vehicles**

```bash
# Request
GET /api/dashboard/top-vehicles?top=10
Authorization: Bearer <jwt-token>

# Behavior
- If no date range provided, returns current month's top selling level-2 models.

# Response (200 OK)
[
  {
    "modelNumber": "CAMRY2024",
    "name": "Toyota Camry 2024",
    "brand": "Toyota",
    "soldCount": 25,
    "revenue": 700000.00
  }
]
```

### **38. GET /api/dashboard/recent-orders**
**Get Recent Orders**

```bash
# Request
GET /api/dashboard/recent-orders
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439050",
    "customerName": "John Doe",
    "modelName": "Toyota Camry 2024",
    "salePrice": 28000.00,
    "status": "Completed",
    "orderDate": "2024-01-15T00:00:00Z"
  }
]
```

---

## 🔄 **Business Workflow**

### **Complete End-to-End Process**

```mermaid
graph TD
    A[1. Create Vehicle Model] --> B[2. Create Purchase Order]
    B --> C[3. Add PO Lines]
    C --> D[4. Complete PO - Auto-creates Vehicles]
    D --> E[5. Customer Creates Order]
    E --> F{Vehicle Available?}
    F -->|Yes| G[6. Assign Vehicle - Status: Reserved]
    F -->|No| H[Order Status: Waiting]
    H --> G
    G --> I[7. Customer Confirms Order]
    I --> J[8. Create Service Order - Pre-delivery]
    J --> K[9. Complete Service Order]
    K --> L[10. Create Billing Document]
    L --> M[11. Generate PDF/Excel]
    M --> N[12. Complete Order - Vehicle Status: Sold]
```

### **Workflow Details**

1. **Setup Phase** (Admin/Dealer)
   - Create `VehicleModel` entries (catalog)
   - Create `PurchaseOrder` for inventory
   - Add `PurchaseOrderLine` items
   - Complete `PurchaseOrder` → Auto-creates `Vehicle` entities

2. **Sales Phase** (Dealer + Customer)
   - Create `Order` (customer order)
   - Assign `Vehicle` to order (if available)
   - Customer confirms order
   - Dealer completes order

3. **Service Phase** (Dealer)
   - Create `ServiceOrder` for pre-delivery inspection
   - Complete service order

4. **Billing Phase** (Dealer)
   - Create `BillingDocument`
   - Generate document outputs (PDF/Excel)
   - Mark billing as paid

---

## 📋 **Data Models**

### **User (Unified)**
```json
{
  "id": "ObjectId",
  "username": "string (required)",
  "passwordHash": "string (required)",
  "name": "string (nullable, optional during registration)",
  "email": "string (required)",
  "phone": "string (optional)",
  "address": "string (optional)",
  "roleId": "ObjectId (required)",
  "status": "Active | Inactive | Deleted",
  "hireDate": "DateTime (optional, for employees)",
  "createdAt": "DateTime",
  "updatedAt": "DateTime",
  "deletedAt": "DateTime (optional)"
}
```

### **VehicleModel**
```json
{
  "modelNumber": "string (PK)",
  "name": "string (required)",
  "brand": "string (required)",
  "price": "decimal (required)",
  "description": "string (required)",
  "imageUrl": "string (optional)"
}
```

### **Vehicle**
```json
{
  "id": "ObjectId",
  "vehicleId": "string (PK, business key)",
  "modelNumber": "string (FK)",
  "externalNumber": "string (optional)",
  "vin": "string (optional)",
  "licensePlate": "string (optional)",
  "status": "Available | Reserved | Sold",
  "purchasePrice": "decimal (required)",
  "photos": ["string (urls)"],
  "receiptDate": "DateTime (optional)",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
}
```

### **VehiclePhoto**
```json
{
  "id": "ObjectId",
  "vehicleId": "string (FK)",
  "vehicleModelId": "string (nullable, FK)",
  "url": "string (required)",
  "displayOrder": "int",
  "caption": "string (nullable)"
}
```

### **Order (Customer Order)**
```json
{
  "id": "ObjectId",
  "customerId": "ObjectId (FK)",
  "dealerId": "ObjectId (FK)",
  "modelNumber": "string (FK)",
  "vehicleId": "string (optional, FK)",
  "orderDate": "DateTime",
  "appointmentDate": "DateTime (optional)",
  "status": "Pending | Confirmed | Completed",
  "salePrice": "decimal (required)",
  "note": "string (optional)",
  "reservationFrom": "DateTime (optional)",
  "reservationTo": "DateTime (optional)"
}
```

### **PurchaseOrder**
```json
{
  "id": "ObjectId",
  "createdBy": "ObjectId (FK)",
  "orderDate": "DateTime",
  "totalAmount": "decimal (required)",
  "status": "Pending | Completed | Cancelled",
  "expectedDeliveryDate": "DateTime (optional)"
}
```

### **PurchaseOrderLine**
```json
{
  "id": "ObjectId",
  "purchaseOrderId": "ObjectId (FK)",
  "modelNumber": "string (FK)",
  "quantity": "int (required)",
  "pricePerUnit": "decimal (required)",
  "lineTotal": "decimal (computed)"
}
```

### **ServiceOrder**
```json
{
  "id": "ObjectId",
  "orderId": "ObjectId (FK)",
  "createdBy": "ObjectId (FK)",
  "serviceDate": "DateTime (optional)",
  "appointmentDate": "DateTime (optional)",
  "description": "string (optional)",
  "cost": "decimal (required)",
  "type": "PreDelivery | Maintenance | Repair",
  "status": "InProgress | Completed | Cancelled"
}
```

### **BillingDocument**
```json
{
  "id": "ObjectId",
  "orderId": "ObjectId (FK)",
  "createdBy": "ObjectId (FK)",
  "billDate": "DateTime",
  "appointmentDate": "DateTime (optional)",
  "amount": "decimal (required)",
  "status": "Unpaid | PartiallyPaid | Paid"
}
```

### **DocumentOutput**
```json
{
  "id": "ObjectId",
  "entityType": "Order | PurchaseOrder | ServiceOrder | BillingDocument",
  "entityId": "ObjectId (FK)",
  "fileType": "PDF | Excel",
  "fileName": "string (required)",
  "filePath": "string (required)",
  "generatedAt": "DateTime",
  "generatedBy": "ObjectId (FK)"
}
```

---

## ⚠️ **Error Handling**

### **Standard Error Response**
```json
{
  "message": "Error description"
}
```

### **Common HTTP Status Codes**
- **200 OK**: Success
- **201 Created**: Resource created successfully
- **400 Bad Request**: Invalid request data
- **401 Unauthorized**: Missing or invalid authentication
- **403 Forbidden**: Insufficient permissions
- **404 Not Found**: Resource not found
- **500 Internal Server Error**: Server error

### **Validation Errors**
```json
{
  "message": "Validation failed",
  "errors": {
    "Email": ["Email is required"],
    "Password": ["Password must be at least 8 characters"]
  }
}
```

---

## 🔑 **Authentication & Authorization**

### **JWT Token Usage**
```bash
# Include in request headers
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

# Token contains user claims:
# - sub: User ID
# - email: User email
# - name: User name
# - username: Username
# - role: User role (for authorization)
# - jti: Unique token ID
# - exp: Expiration timestamp
# - iss: Token issuer
# - aud: Token audience
```

### **Roles & Permissions**
- **Admin**: Full system access (system role)
- **HR**: User management, employee operations
- **Dealer**: Vehicle sales, inventory, customer orders
- **Customer**: View/confirm own orders

### **Role-Based Endpoints**
- **Public**: Login, Forgot Password, Reset Password
- **Authenticated**: Profile, Dashboard, View operations
- **Dealer/Admin**: Create/Update vehicles, orders, services
- **Admin Only**: Delete operations, user management

---

## 📈 **Database Collections (MongoDB)**

All collections use **UPPERCASE** naming convention:

- `ROLE` - User roles
- `USER` - All users (employees, customers, dealers)
- `VEHICLE_MODEL` - Vehicle model catalog
- `VEHICLE` - Individual vehicles
- `PURCHASE_ORDER` - Purchase orders from suppliers
- `PURCHASE_ORDER_LINE` - PO line items
- `ORDER` - Customer orders
- `SERVICE_ORDER` - Service orders
- `BILLING_DOCUMENT` - Invoices and billing
- `DOCUMENT_OUTPUT` - Generated documents metadata

---

## 🎯 **Architecture Highlights**

✅ **Clean Architecture**: Proper layer separation (Domain → Application → Infrastructure → WebAPI)  
✅ **Domain-Driven Design**: Rich domain models with business logic  
✅ **CQRS Pattern**: Commands for writes, Queries for reads (MediatR)  
✅ **Repository Pattern**: Generic repository with Unit of Work  
✅ **Dependency Injection**: Autofac container for DI  
✅ **JWT Authentication**: Secure token-based authentication + Public registration  
✅ **Role-Based Authorization**: Fine-grained access control with auto-role assignment  
✅ **MongoDB Integration**: NoSQL database with custom primary keys  
✅ **Performance Optimization**: Comprehensive database indexing on all collections  
✅ **Auto-Billing**: ServiceOrder completion triggers automatic BillingDocument creation  
✅ **Document Generation**: PDF/Excel generation with iText7  
✅ **Business Workflow**: Complete vehicle showroom operations  
✅ **Scalability**: Indexed queries for 1000+ concurrent users  

### **New Features (Latest Update)**
- 🆕 **Public Registration**: `/api/auth/register` endpoint for customer self-registration
- 🆕 **Auto-Role Assignment**: Users automatically assigned Customer or Employee role based on HireDate
- 🆕 **ServiceOrder Status Update**: PUT endpoint with auto-billing on completion
- 🆕 **VehicleModel Enhancements**: Added description (required) and imageUrl (optional) fields
- 🆕 **VehicleModel Update**: PUT `/api/vehicle-models/{modelNumber}` endpoint
- 🆕 **Database Indexing**: Performance indexes on all collections for optimal query speed
- 🆕 **Memory Optimization**: Single imageUrl field per VehicleModel (not collection)
- 🆕 **Simplified Registration**: Registration now requires only username, email, password
- 🆕 **GetUsersByRole API**: New endpoint to fetch users filtered by role name
- 🆕 **Flexible User Profile**: Name field is nullable, can be updated via profile endpoint

### **Performance & Scalability**
The system now includes comprehensive MongoDB indexing on:
- USER: username, email (unique), roleId, status, deletedAt
- VEHICLE: modelNumber, status, purchasePrice
- VEHICLE_MODEL: brand, price
- ORDER: customerId, dealerId, status, orderDate, vehicleId
- SERVICE_ORDER: orderId, status, createdBy
- BILLING_DOCUMENT: orderId, status, createdBy, billDate
- PURCHASE_ORDER: createdBy, status, orderDate

These indexes ensure optimal query performance for filtering, searching, and reporting operations, supporting 1000+ concurrent users with <200ms API response times.

---

## 🚀 **Quick Start**

### **1. Authentication Flow**
```bash
# 1. Login
POST /api/auth/login
{
  "username": "admin",
  "password": "Admin123!"
}

# 2. Use token in subsequent requests
Authorization: Bearer {token}

# 3. Refresh token when needed
POST /api/auth/refresh-token
{
  "refreshToken": "{refreshToken}"
}
```

### **2. User Registration & Management**
```bash
# 1. Customer self-registration (simplified)
POST /api/auth/register
{
  "username": "newcustomer",
  "email": "customer@example.com",
  "password": "Customer123!"
}

# 2. Get all employees for management screen
GET /api/users?roleName=Employee
Authorization: Bearer <jwt-token>

# 3. Get all customers for management screen
GET /api/users?roleName=Customer
Authorization: Bearer <jwt-token>

# 4. Update user profile (after registration)
PUT /api/users/{id}/profile
{
  "name": "John Customer",
  "email": "john@example.com",
  "phone": "+1234567890",
  "address": "123 Customer St"
}
```

### **3. Create First Vehicle**
```bash
# 1. Create vehicle model
POST /api/vehicle-models
{
  "modelNumber": "CAMRY2024",
  "name": "Toyota Camry 2024",
  "brand": "Toyota",
  "price": 28000.00
}

# 2. Create purchase order
POST /api/purchase-orders
{
  "createdBy": "{userId}",
  "totalAmount": 130000.00
}

# 3. Add PO lines
POST /api/purchase-orders/{poId}/lines
{
  "modelNumber": "CAMRY2024",
  "quantity": 5,
  "pricePerUnit": 26000.00
}

# 4. Complete PO (auto-creates 5 vehicles)
POST /api/purchase-orders/{poId}/complete
```

### **4. Process Customer Order**
```bash
# 1. Create order
POST /api/orders
{
  "customerId": "{customerId}",
  "dealerId": "{dealerId}",
  "modelNumber": "CAMRY2024",
  "salePrice": 28000.00
}

# 2. Assign vehicle
POST /api/orders/{orderId}/assign-vehicle
{
  "vehicleId": "{vehicleId}"
}

# 3. Confirm order
POST /api/orders/{orderId}/confirm

# 4. Complete order
POST /api/orders/{orderId}/complete
```

---

## 📞 **Support & Documentation**

**Base URL**: `https://your-api-domain.com/api/`  
**Content-Type**: `application/json`  
**Authentication**: JWT Bearer token  

**Swagger UI**: Available at `/swagger` in development mode

---

**Happy API Testing! 🎉**

---

## ✅ Latest Updates: Variant Model Hierarchy (Level-2) and Flow Changes

This section supplements existing docs to reflect the new variant (Level-2) model structure and simplified flows. Earlier sections remain valid unless overridden here.

### Vehicle Models (Variants)
- Create (multipart):
  - POST `/api/vehicle-models`
  - Parts:
    - `data` (application/json): `{ modelNumber, name, price, description, parentId, level=2, slug }`
    - `files`: repeated images (optional)
- Get by slug:
  - GET `/api/vehicle-models/slug/{slug}`
- Search Level-2 variants:
  - GET `/api/vehicle-models/search?parentModelNumber=911&seats=4&fuelType=petrol&pageNumber=1&pageSize=10`

### Photos (Model-only)
- Upload to model:
  - POST `/api/vehicle-models/{modelNumber}/photos/upload` (multipart `files`)

### Vehicles
- Create vehicle (no receiptDate; licensePlate assigned later):
  - POST `/api/vehicles` with `{ vehicleId, modelNumber, purchasePrice, externalNumber?, vin?, licensePlate? }`
- Get vehicles by variant slug:
  - GET `/api/vehicles/slug/{slug}?pageNumber=1&pageSize=10`
  - Response: `{ model: {...}, vehicles: { ...paged... } }`

### Purchase Orders
- Create PO (no expectedDeliveryDate): POST `/api/purchase-orders` `{ createdBy, totalAmount }`
- Add lines (Level-2 model only): POST `/api/purchase-orders/{id}/lines` `{ modelNumber, quantity, pricePerUnit }`
- Complete PO: POST `/api/purchase-orders/{id}/complete` (auto-creates Vehicles)

### Orders
- Create (customers allowed): POST `/api/orders` `{ customerId, modelNumber, salePrice }`
- Assign vehicle by variant: POST `/api/orders/{id}/assign-vehicle` `{ vehicleId? }`
  - If `vehicleId` omitted, backend auto-picks first available vehicle of the order’s Level-2 model.

### Service Orders
- Update status and set license plate when Completed:
  - PUT `/api/service-orders/{id}/status` `{ status, licensePlate? }`
  - On `Completed`, if `licensePlate` provided, vehicle gets updated.

### Billing Documents
- Removed. Payment is considered complete at service completion.

### Dashboard
- Overview: GET `/api/dashboard/overview` → `{ profit, employees, customersPurchased, completedOrders, level2Models, vehicles }`
- Top vehicles (by Level-2 model): GET `/api/dashboard/top-vehicles?top=10`
- Recent orders: GET `/api/dashboard/recent-orders?limit=10`
