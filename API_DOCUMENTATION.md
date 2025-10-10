# 🚀 **Vehicle Showroom Management System - Complete API Reference**

> **Last Updated**: After Complete Refactoring  
> **Architecture**: Clean Architecture + DDD + CQRS  
> **Backend**: .NET 8 Web API + MongoDB  
> **Authentication**: JWT Bearer Tokens

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
**User Login - Returns JWT Token**

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
  "refreshToken": "base64-encoded-refresh-token",
  "tokenExpiresAt": "2024-01-02T00:00:00Z",
  "refreshTokenExpiresAt": "2024-01-08T00:00:00Z",
  "userId": "507f1f77bcf86cd799439011",
  "role": "Admin",
  "message": "Login successful"
}
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
**Refresh JWT Token**

```bash
# Request
POST /api/auth/refresh-token
Content-Type: application/json

{
  "refreshToken": "base64-encoded-refresh-token"
}

# Response (200 OK)
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "new-base64-encoded-refresh-token",
  "tokenExpiresAt": "2024-01-02T00:00:00Z",
  "refreshTokenExpiresAt": "2024-01-08T00:00:00Z",
  "userId": "507f1f77bcf86cd799439011",
  "role": "Admin",
  "message": "Token refreshed successfully"
}
```

### **5. POST /api/auth/revoke-token**
**Revoke Refresh Token (Logout)**
*Requires Authentication*

```bash
# Request
POST /api/auth/revoke-token
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "refreshToken": "base64-encoded-refresh-token"
}

# Response (200 OK)
{
  "message": "Token revoked successfully"
}
```

---

## 👤 **Profile Management APIs** (`/api/profile`)

*All endpoints require authentication*

### **6. GET /api/profile**
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

### **7. PUT /api/profile**
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

### **8. POST /api/profile/change-password**
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

### **9. GET /api/users**
**Get All Users**

```bash
# Request
GET /api/users?searchTerm=john&roleId=507f1f77bcf86cd799439012&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "users": [
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
  ],
  "totalCount": 1,
  "pageNumber": 1,
  "pageSize": 10,
  "totalPages": 1
}
```

### **10. GET /api/users/{id}**
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

### **11. POST /api/users**
**Create New User**

```bash
# Request
POST /api/users
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "username": "jane_smith",
  "password": "SecurePass123!",
  "name": "Jane Smith",
  "email": "jane@showroom.com",
  "phone": "+1234567890",
  "address": "456 Oak Ave",
  "roleId": "507f1f77bcf86cd799439012",
  "hireDate": "2024-01-15T00:00:00Z"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439013",
  "message": "User created successfully"
}
```

### **12. PUT /api/users/{id}**
**Update User**

```bash
# Request
PUT /api/users/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "name": "John Doe Updated",
  "email": "john.updated@showroom.com",
  "phone": "+1987654321",
  "address": "789 New St",
  "roleId": "507f1f77bcf86cd799439012"
}

# Response (200 OK)
{
  "message": "User updated successfully"
}
```

### **13. DELETE /api/users/{id}**
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

### **14. POST /api/vehicle-models**
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
  "price": 28000.00
}

# Response (200 OK)
{
  "modelNumber": "CAMRY2024",
  "message": "Vehicle model created successfully"
}
```

### **15. GET /api/vehicle-models**
**Get All Vehicle Models**

```bash
# Request
GET /api/vehicle-models?pageNumber=1&pageSize=10&brand=Toyota
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "models": [
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

### **18. GET /api/vehicles**
**Get All Vehicles with Pagination**

```bash
# Request
GET /api/vehicles?pageNumber=1&pageSize=10&status=Available&brand=Toyota
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "vehicles": [
    {
      "id": "507f1f77bcf86cd799439011",
      "vehicleId": "VEH-2024-001",
      "modelNumber": "CAMRY2024",
      "modelName": "Toyota Camry 2024",
      "brand": "Toyota",
      "status": "Available",
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

### **19. GET /api/vehicles/search**
**Search Vehicles** *(Advanced filtering)*

```bash
# Request
GET /api/vehicles/search?searchTerm=camry&status=Available&brand=Toyota&minPrice=20000&maxPrice=30000&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "vehicles": [
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
  "status": "Sold"
}

# Response (200 OK)
{
  "message": "Vehicle status updated successfully"
}

# Possible status values:
# - Available (1)
# - Reserved (2)
# - Sold (3)
# - InService (4)
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

### **26. POST /api/purchase-orders/{id}/complete**
**Complete Purchase Order** *(Auto-creates vehicles)*

```bash
# Request
POST /api/purchase-orders/507f1f77bcf86cd799439020/complete
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Purchase order completed successfully",
  "vehiclesCreated": 5,
  "vehicleIds": [
    "507f1f77bcf86cd799439030",
    "507f1f77bcf86cd799439031",
    "507f1f77bcf86cd799439032",
    "507f1f77bcf86cd799439033",
    "507f1f77bcf86cd799439034"
  ]
}

# Note: Completing a PO automatically creates Vehicle entities
# based on the purchase order lines (quantity × model)
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
  "vehicleId": "507f1f77bcf86cd799439030"
}

# Response (200 OK)
{
  "message": "Vehicle assigned successfully"
}

# Status changes: Waiting → Reserved
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

# Status changes: Reserved → Confirmed
```

### **30. POST /api/orders/{id}/complete**
**Complete Order** *(Dealer/Admin only)*

```bash
# Request
POST /api/orders/507f1f77bcf86cd799439050/complete
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Order completed successfully"
}

# Status changes: Confirmed → Completed
# Vehicle status also updated to Sold
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

### **32. POST /api/service-orders/{id}/complete**
**Complete Service Order**

```bash
# Request
POST /api/service-orders/507f1f77bcf86cd799439060/complete
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Service order completed successfully"
}

# Status changes: Scheduled → Completed
```

---

## 💰 **Billing Document APIs** (`/api/billing-documents`)

*Invoice and billing management*  
*Requires Dealer/Admin role*

### **33. POST /api/billing-documents**
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

## 📄 **Document Output APIs** (`/api/document-outputs`)

*Generate PDF/Excel documents*  
*Requires Dealer/Admin role*

### **34. POST /api/document-outputs/generate**
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

### **35. GET /api/dashboard/revenue**
**Get Revenue Analytics**

```bash
# Request
GET /api/dashboard/revenue
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "totalRevenue": 1250000.00,
  "monthlyRevenue": 125000.00,
  "yearlyRevenue": 1500000.00,
  "revenueGrowth": 15.5,
  "topSellingModels": [
    {
      "modelNumber": "CAMRY2024",
      "name": "Toyota Camry 2024",
      "brand": "Toyota",
      "totalSold": 25,
      "revenue": 700000.00
    }
  ]
}
```

### **36. GET /api/dashboard/customer**
**Get Customer Analytics**

```bash
# Request
GET /api/dashboard/customer
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "totalCustomers": 150,
  "newCustomersThisMonth": 12,
  "customerRetentionRate": 85.5,
  "averageOrderValue": 28500.00,
  "customerGrowth": [
    {
      "month": "2024-01",
      "count": 45
    }
  ]
}
```

### **37. GET /api/dashboard/top-vehicles**
**Get Top Selling Vehicles**

```bash
# Request
GET /api/dashboard/top-vehicles
Authorization: Bearer <jwt-token>

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
  "name": "string (required)",
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
  "price": "decimal (required)"
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
  "status": "Available | Reserved | Sold | InService",
  "purchasePrice": "decimal (required)",
  "photos": ["string (urls)"],
  "receiptDate": "DateTime (optional)",
  "createdAt": "DateTime",
  "updatedAt": "DateTime"
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
  "status": "Waiting | Reserved | Confirmed | Completed | Cancelled",
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
  "status": "Scheduled | Completed | Cancelled"
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
✅ **JWT Authentication**: Secure token-based authentication  
✅ **Role-Based Authorization**: Fine-grained access control  
✅ **MongoDB Integration**: NoSQL database with custom primary keys  
✅ **Document Generation**: PDF/Excel generation with iText7  
✅ **Business Workflow**: Complete vehicle showroom operations  

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

### **2. Create First Vehicle**
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

### **3. Process Customer Order**
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
