# 🚀 **COMPLETE API REFERENCE - Vehicle Showroom Management System**

## 🔐 **Authentication APIs** (`/api/auth`)

### **1. POST /api/auth/login**
**User Login (Returns JWT Token)**
*Returns minimal user info for security. Use `/api/users/profile` for detailed user information.*
```bash
# Request
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@showroom.com",
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
  "message": "Password reset token sent to your email"
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
  "message": "Password reset successfully"
}
```

---

## 👥 **User Management APIs** (`/api/users`) - *Requires Authentication*

### **4. GET /api/users**
**Get All Users (HR/Admin only)**
```bash
# Request with query parameters
GET /api/users?searchTerm=john&roleId=2&isActive=true&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "userId": 1,
    "username": "john_doe",
    "email": "john@showroom.com",
    "firstName": "John",
    "lastName": "Doe",
    "roleId": 2,
    "roleName": "Dealer",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

### **5. GET /api/users/{id}**
**Get User by ID (HR/Admin only)**
```bash
# Request
GET /api/users/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "userId": 1,
  "username": "john_doe",
  "email": "john@showroom.com",
  "firstName": "John",
  "lastName": "Doe",
  "roleId": 2,
  "roleName": "Dealer",
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **6. POST /api/users**
**Create New User (HR/Admin only)**
```bash
# Request
POST /api/users
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "username": "new_user",
  "email": "newuser@showroom.com",
  "password": "SecurePass123!",
  "firstName": "New",
  "lastName": "User",
  "roleId": 3
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "message": "User created successfully"
}
```

### **7. PUT /api/users/{id}**
**Update User (HR/Admin only)**
```bash
# Request
PUT /api/users/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "firstName": "Updated",
  "lastName": "Name",
  "email": "updated@showroom.com",
  "roleId": 2,
  "isActive": true
}

# Response (200 OK)
{
  "message": "User updated successfully"
}
```

### **8. DELETE /api/users/{id}**
**Delete User (Admin only)**
```bash
# Request
DELETE /api/users/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "User deleted successfully"
}
```

### **9. GET /api/users/profile**
**Get Current User Profile**
```bash
# Request
GET /api/users/profile
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "userId": "507f1f77bcf86cd799439011",
  "username": "john_doe",
  "email": "john@showroom.com",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "roleId": "507f1f77bcf86cd799439012",
  "roleName": "Dealer",
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

---

## 🚗 **Vehicle Management APIs** (`/api/vehicles`) - *Requires Authentication*

### **10. GET /api/vehicles**
**Get All Vehicles**
```bash
# Request with query parameters
GET /api/vehicles?searchTerm=toyota&status=AVAILABLE&brand=Toyota&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "vin": "1HGCM82633A123456",
    "modelNumber": "CAMRY2024",
    "name": "Toyota Camry 2024",
    "brand": "Toyota",
    "price": 25000.00,
    "status": "AVAILABLE",
    "year": 2024,
    "color": "White",
    "mileage": 0,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

### **11. GET /api/vehicles/{id}**
**Get Vehicle by ID**
```bash
# Request
GET /api/vehicles/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "id": "507f1f77bcf86cd799439011",
  "vin": "1HGCM82633A123456",
  "modelNumber": "CAMRY2024",
  "name": "Toyota Camry 2024",
  "brand": "Toyota",
  "price": 25000.00,
  "status": "AVAILABLE",
  "year": 2024,
  "color": "White",
  "mileage": 0,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **12. POST /api/vehicles**
**Create New Vehicle (Dealer/Admin only)**
```bash
# Request
POST /api/vehicles
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "modelNumber": "ACCORD2024",
  "name": "Honda Accord 2024",
  "brand": "Honda",
  "price": 28000.00,
  "status": "AVAILABLE",
  "registrationNumber": "ABC-123",
  "registrationDate": "2024-01-15T00:00:00Z",
  "externalId": "EXT-001"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "vin": "1HGCM82633A123456",
  "modelNumber": "ACCORD2024",
  "name": "Honda Accord 2024",
  "brand": "Honda",
  "price": 28000.00,
  "status": "AVAILABLE",
  "year": 2024,
  "color": "Default",
  "mileage": 0,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **13. PUT /api/vehicles/{id}**
**Update Vehicle (Dealer/Admin only)**
```bash
# Request
PUT /api/vehicles/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "price": 27000.00,
  "status": "SOLD"
}

# Response (200 OK)
{
  "message": "Vehicle updated successfully"
}
```

### **14. POST /api/vehicles/{id}**
**Delete Vehicle (Dealer/Admin only)**
```bash
# Request
POST /api/vehicles/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Vehicle deleted successfully"
}
```

### **15. POST /api/vehicles**
**Delete Multiple Vehicles (Dealer/Admin only)**
```bash
# Request
POST /api/vehicles
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "ids": [
    "507f1f77bcf86cd799439011",
    "507f1f77bcf86cd799439012"
  ]
}

# Response (200 OK)
{
  "message": "Vehicles deleted successfully"
}
```

---

## 📦 **Order Management APIs** (`/api/orders`) - *Requires Authentication*

### **16. GET /api/orders**
**Get All Orders**
```bash
# Request
GET /api/orders?pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "orderNumber": "ORD-20240101-ABC1",
    "customerId": "507f1f77bcf86cd799439012",
    "customer": {
      "id": "507f1f77bcf86cd799439012",
      "firstName": "John",
      "lastName": "Doe",
      "email": "john@customer.com",
      "phone": "+1234567890"
    },
    "vehicleId": "507f1f77bcf86cd799439013",
    "vehicle": {
      "vehicleId": "507f1f77bcf86cd799439013",
      "vin": "1HGCM82633A123456",
      "modelNumber": "CAMRY2024",
      "name": "Toyota Camry 2024",
      "brand": "Toyota",
      "price": 25000.00
    },
    "salesPersonId": "507f1f77bcf86cd799439014",
    "status": "COMPLETED",
    "totalAmount": 25000.00,
    "paymentMethod": "CASH",
    "orderDate": "2024-01-01T00:00:00Z",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

### **17. GET /api/orders/{id}**
**Get Order by ID**
```bash
# Request
GET /api/orders/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "id": "507f1f77bcf86cd799439011",
  "orderNumber": "ORD-20240101-ABC1",
  "customerId": "507f1f77bcf86cd799439012",
  "customer": {
    "id": "507f1f77bcf86cd799439012",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@customer.com",
    "phone": "+1234567890"
  },
  "vehicleId": "507f1f77bcf86cd799439013",
  "vehicle": {
    "vehicleId": "507f1f77bcf86cd799439013",
    "vin": "1HGCM82633A123456",
    "modelNumber": "CAMRY2024",
    "name": "Toyota Camry 2024",
    "brand": "Toyota",
    "price": 25000.00
  },
  "salesPersonId": "507f1f77bcf86cd799439014",
  "status": "COMPLETED",
  "totalAmount": 25000.00,
  "paymentMethod": "CASH",
  "orderDate": "2024-01-01T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **18. POST /api/orders**
**Create New Order (Dealer/Admin only)**
```bash
# Request
POST /api/orders
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "customer": {
    "name": "John Doe",
    "email": "john@customer.com",
    "phone": "+1234567890",
    "address": "123 Main St, City, State"
  },
  "vehicleId": "507f1f77bcf86cd799439013",
  "totalAmount": 25000.00,
  "paymentMethod": "CASH"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "orderNumber": "ORD-20240101-ABC1",
  "customerId": "507f1f77bcf86cd799439012",
  "customer": {
    "id": "507f1f77bcf86cd799439012",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@customer.com",
    "phone": "+1234567890"
  },
  "vehicleId": "507f1f77bcf86cd799439013",
  "vehicle": {
    "vehicleId": "507f1f77bcf86cd799439013",
    "vin": "1HGCM82633A123456",
    "modelNumber": "CAMRY2024",
    "name": "Toyota Camry 2024",
    "brand": "Toyota",
    "price": 25000.00
  },
  "salesPersonId": "507f1f77bcf86cd799439014",
  "status": "CONFIRMED",
  "totalAmount": 25000.00,
  "paymentMethod": "CASH",
  "orderDate": "2024-01-01T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **19. POST /api/orders/{id}/print**
**Print Order (Dealer/Admin only)**
```bash
# Request
POST /api/orders/507f1f77bcf86cd799439011/print
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Order printed successfully",
  "printData": {
    "orderNumber": "ORD-20240101-ABC1",
    "customer": {
      "id": "507f1f77bcf86cd799439012",
      "firstName": "John",
      "lastName": "Doe",
      "email": "john@customer.com",
      "phone": "+1234567890"
    },
    "vehicle": {
      "name": "Toyota Camry 2024",
      "brand": "Toyota",
      "price": 25000.00
    },
    "salesPerson": {
      "fullName": "Jane Smith",
      "email": "jane@showroom.com"
    },
    "totalAmount": 25000.00,
    "paymentMethod": "CASH",
    "orderDate": "2024-01-01T00:00:00Z",
    "items": [
      {
        "vehicleId": "507f1f77bcf86cd799439013",
        "quantity": 1,
        "unitPrice": 25000.00,
        "lineTotal": 25000.00
      }
    ],
    "companyInfo": "Vehicle Showroom Management System"
  }
}
```

---

## 👤 **Profile Management APIs** (`/api/profile`) - *Requires Authentication*

### **20. GET /api/profile**
**Get Current User Profile**
```bash
# Request
GET /api/profile
Authorization: Bearer <jwt-token>

# Response (200 OK) - Same as GET /api/users/profile
{
  "userId": "507f1f77bcf86cd799439011",
  "username": "john_doe",
  "email": "john@showroom.com",
  "firstName": "John",
  "lastName": "Doe",
  "phone": "+1234567890",
  "roleId": "507f1f77bcf86cd799439012",
  "roleName": "Dealer",
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **21. PUT /api/profile**
**Update Current User Profile**
```bash
# Request
PUT /api/profile
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "firstName": "Updated",
  "lastName": "Name",
  "phone": "+1987654321"
}

# Response (200 OK)
{
  "message": "Profile updated successfully"
}
```

### **22. POST /api/profile/change-password**
**Change Current User Password**
```bash
# Request
POST /api/profile/change-password
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "oldPassword": "CurrentPass123!",
  "newPassword": "NewSecurePass456!"
}

# Response (200 OK)
{
  "message": "Password changed successfully"
}
```

---

## 👥 **Employee Management APIs** (`/api/employees`) - *Requires Authentication*

### **23. GET /api/employees**
**Get All Employees**
```bash
# Request
GET /api/employees?searchTerm=jane&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "username": "jane_smith",
    "email": "jane@showroom.com",
    "firstName": "Jane",
    "lastName": "Smith",
    "phone": "+1234567890",
    "salary": 50000.00,
    "roleId": "507f1f77bcf86cd799439012",
    "roleName": "Sales",
    "isActive": true,
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z",
    "totalSales": 15,
    "totalRevenue": 375000.00
  }
]
```

---

## 👥 **Customer Management APIs** (`/api/customers`) - *Requires Authentication*

### **24. GET /api/customers**
**Get All Customers**
```bash
# Request
GET /api/customers?pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "firstName": "John",
    "lastName": "Doe",
    "email": "john@customer.com",
    "phone": "+1234567890",
    "address": "123 Main St",
    "city": "Springfield",
    "state": "IL",
    "zipCode": "62701",
    "cccd": "123456789012",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z",
    "orders": []
  }
]
```

### **25. GET /api/customers/{customerId}/orders**
**Get Customer Orders**
```bash
# Request
GET /api/customers/507f1f77bcf86cd799439011/orders
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439012",
    "orderNumber": "ORD-20240101-ABC1",
    "customerId": "507f1f77bcf86cd799439011",
    "vehicleId": "507f1f77bcf86cd799439013",
    "status": "COMPLETED",
    "totalAmount": 25000.00,
    "orderDate": "2024-01-01T00:00:00Z",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

---

## 📦 **Purchase Order Management APIs** (`/api/purchase-orders`) - *Requires Authentication*

### **26. GET /api/purchase-orders**
**Get All Purchase Orders**
```bash
# Request
GET /api/purchase-orders?searchTerm=toyota&status=Pending&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "orderNumber": "PO-20240101-001",
    "orderDate": "2024-01-01T00:00:00Z",
    "supplierId": "507f1f77bcf86cd799439012",
    "modelNumber": "CAMRY2024",
    "name": "Toyota Camry 2024",
    "brand": "Toyota",
    "price": 25000.00,
    "quantity": 5,
    "totalAmount": 125000.00,
    "status": "Pending",
    "notes": "Urgent delivery required",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-01T00:00:00Z"
  }
]
```

### **27. GET /api/purchase-orders/{id}**
**Get Purchase Order by ID**
```bash
# Request
GET /api/purchase-orders/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "id": "507f1f77bcf86cd799439011",
  "orderNumber": "PO-20240101-001",
  "orderDate": "2024-01-01T00:00:00Z",
  "supplierId": "507f1f77bcf86cd799439012",
  "modelNumber": "CAMRY2024",
  "name": "Toyota Camry 2024",
  "brand": "Toyota",
  "price": 25000.00,
  "quantity": 5,
  "totalAmount": 125000.00,
  "status": "Pending",
  "notes": "Urgent delivery required",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **28. POST /api/purchase-orders**
**Create New Purchase Order (Dealer/Admin only)**
```bash
# Request
POST /api/purchase-orders
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "supplierId": "507f1f77bcf86cd799439012",
  "modelNumber": "ACCORD2024",
  "name": "Honda Accord 2024",
  "brand": "Honda",
  "price": 28000.00,
  "quantity": 3,
  "notes": "Standard delivery"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "orderNumber": "PO-20240101-002",
  "orderDate": "2024-01-01T00:00:00Z",
  "supplierId": "507f1f77bcf86cd799439012",
  "modelNumber": "ACCORD2024",
  "name": "Honda Accord 2024",
  "brand": "Honda",
  "price": 28000.00,
  "quantity": 3,
  "totalAmount": 84000.00,
  "status": "Pending",
  "notes": "Standard delivery",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **29. PUT /api/purchase-orders/{id}/approve**
**Approve Purchase Order (Admin only)**
```bash
# Request
PUT /api/purchase-orders/507f1f77bcf86cd799439011/approve
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Purchase order approved successfully"
}
```

---

## 📥 **Goods Receipt Management APIs** (`/api/goods-receipts`) - *Requires Authentication*

### **30. GET /api/goods-receipts**
**Get All Goods Receipts**
```bash
# Request
GET /api/goods-receipts?status=Accepted&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "receiptNumber": "GR-20240101-001",
    "purchaseOrderId": "507f1f77bcf86cd799439012",
    "receiptDate": "2024-01-15T00:00:00Z",
    "vehicleId": "507f1f77bcf86cd799439013",
    "vin": "1HGCM82633A123456",
    "manufacturerVIN": "MFG-123456789",
    "status": "Accepted",
    "notes": "Vehicle in excellent condition",
    "createdAt": "2024-01-15T00:00:00Z",
    "updatedAt": "2024-01-15T00:00:00Z"
  }
]
```

### **31. POST /api/goods-receipts**
**Create New Goods Receipt (Dealer/Admin only)**
```bash
# Request
POST /api/goods-receipts
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "purchaseOrderId": "507f1f77bcf86cd799439012",
  "vehicleId": "507f1f77bcf86cd799439013",
  "vin": "1HGCM82633A123456",
  "manufacturerVIN": "MFG-123456789",
  "notes": "Vehicle in excellent condition"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "receiptNumber": "GR-20240101-001",
  "purchaseOrderId": "507f1f77bcf86cd799439012",
  "receiptDate": "2024-01-15T00:00:00Z",
  "vehicleId": "507f1f77bcf86cd799439013",
  "vin": "1HGCM82633A123456",
  "manufacturerVIN": "MFG-123456789",
  "status": "Pending",
  "notes": "Vehicle in excellent condition",
  "createdAt": "2024-01-15T00:00:00Z",
  "updatedAt": "2024-01-15T00:00:00Z"
}
```

### **32. PUT /api/goods-receipts/{id}/accept**
**Accept Goods Receipt (Dealer/Admin only)**
```bash
# Request
PUT /api/goods-receipts/507f1f77bcf86cd799439011/accept
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Goods receipt accepted successfully"
}
```

---

## 🚗 **Vehicle Registration APIs** (`/api/vehicle-registrations`) - *Requires Authentication*

### **33. GET /api/vehicle-registrations**
**Get All Vehicle Registrations**
```bash
# Request
GET /api/vehicle-registrations?status=Active&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "registrationNumber": "ABC-123456",
    "vehicleId": "507f1f77bcf86cd799439012",
    "vin": "1HGCM82633A123456",
    "customerId": "507f1f77bcf86cd799439013",
    "registrationDate": "2024-01-20T00:00:00Z",
    "expiryDate": "2025-01-20T00:00:00Z",
    "issuingAuthority": "DMV",
    "status": "Active",
    "ownerName": "John Doe",
    "ownerAddress": "123 Main St, City, State 12345",
    "createdAt": "2024-01-20T00:00:00Z",
    "updatedAt": "2024-01-20T00:00:00Z"
  }
]
```

### **34. POST /api/vehicle-registrations**
**Create New Vehicle Registration (Dealer/Admin only)**
```bash
# Request
POST /api/vehicle-registrations
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "vehicleId": "507f1f77bcf86cd799439012",
  "customerId": "507f1f77bcf86cd799439013",
  "registrationDate": "2024-01-20T00:00:00Z",
  "expiryDate": "2025-01-20T00:00:00Z",
  "issuingAuthority": "DMV",
  "ownerName": "John Doe",
  "ownerAddress": "123 Main St, City, State 12345"
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "registrationNumber": "ABC-123456",
  "vehicleId": "507f1f77bcf86cd799439012",
  "vin": "1HGCM82633A123456",
  "customerId": "507f1f77bcf86cd799439013",
  "registrationDate": "2024-01-20T00:00:00Z",
  "expiryDate": "2025-01-20T00:00:00Z",
  "issuingAuthority": "DMV",
  "status": "Active",
  "ownerName": "John Doe",
  "ownerAddress": "123 Main St, City, State 12345",
  "createdAt": "2024-01-20T00:00:00Z",
  "updatedAt": "2024-01-20T00:00:00Z"
}
```

---

## 🔧 **Service Order Management APIs** (`/api/service-orders`) - *Requires Authentication*

### **35. GET /api/service-orders**
**Get All Service Orders**
```bash
# Request
GET /api/service-orders?status=Completed&pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "vehicleId": "507f1f77bcf86cd799439012",
    "customerId": "507f1f77bcf86cd799439013",
    "serviceDate": "2024-01-15T10:00:00Z",
    "status": "Completed",
    "totalCost": 250.00,
    "description": "Pre-delivery inspection and detailing",
    "serviceType": "PreDelivery",
    "createdAt": "2024-01-01T00:00:00Z",
    "updatedAt": "2024-01-15T10:00:00Z"
  }
]
```

### **36. POST /api/service-orders**
**Create New Service Order (Dealer/Admin only)**
```bash
# Request
POST /api/service-orders
Authorization: Bearer <jwt-token>
Content-Type: application/json

{
  "vehicleId": "507f1f77bcf86cd799439012",
  "customerId": "507f1f77bcf86cd799439013",
  "serviceDate": "2024-01-15T10:00:00Z",
  "description": "Pre-delivery inspection and detailing",
  "serviceType": "PreDelivery",
  "totalCost": 250.00
}

# Response (201 Created)
{
  "id": "507f1f77bcf86cd799439011",
  "vehicleId": "507f1f77bcf86cd799439012",
  "customerId": "507f1f77bcf86cd799439013",
  "serviceDate": "2024-01-15T10:00:00Z",
  "status": "Pending",
  "totalCost": 250.00,
  "description": "Pre-delivery inspection and detailing",
  "serviceType": "PreDelivery",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

### **37. PUT /api/service-orders/{id}/start**
**Start Service Order (Dealer/Admin only)**
```bash
# Request
PUT /api/service-orders/507f1f77bcf86cd799439011/start
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "message": "Service order started successfully"
}
```

---

## 🖼️ **Image Upload APIs** (`/api/images`) - *Requires Authentication*

### **38. POST /api/images/upload/vehicle/{vehicleId}**
**Upload Vehicle Image (Dealer/Admin only)**
```bash
# Request
POST /api/images/upload/vehicle/507f1f77bcf86cd799439011
Authorization: Bearer <jwt-token>
Content-Type: multipart/form-data

# Form Data
file: [image file]

# Response (200 OK)
{
  "imageUrl": "https://res.cloudinary.com/your-cloud/image/upload/v1234567890/vehicle-showroom/abc123.jpg"
}
```

---

## 📊 **Reports APIs** (`/api/reports`) - *Requires Authentication*

### **39. GET /api/reports/stock-availability**
**Get Stock Availability Report**
```bash
# Request
GET /api/reports/stock-availability?brand=Toyota&status=Available
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "reportData": [
    {
      "brand": "Toyota",
      "model": "Camry",
      "vin": "1HGCM82633A123456",
      "color": "Blue",
      "year": 2024,
      "price": 25000.00,
      "status": "Available",
      "lastUpdated": "2024-01-01T00:00:00Z"
    }
  ],
  "summary": {
    "totalVehicles": 150,
    "availableVehicles": 120,
    "soldVehicles": 25,
    "reservedVehicles": 5
  },
  "generatedAt": "2024-01-01T00:00:00Z"
}
```

### **40. GET /api/reports/customer-info**
**Get Customer Information Report**
```bash
# Request
GET /api/reports/customer-info?city=Springfield&includeOrderHistory=true
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "reportData": [
    {
      "customerName": "John Doe",
      "email": "john@customer.com",
      "phone": "+1234567890",
      "address": "123 Main St",
      "city": "Springfield",
      "state": "IL",
      "totalOrders": 2,
      "totalSpent": 50000.00,
      "lastOrderDate": "2024-01-01T00:00:00Z"
    }
  ],
  "summary": {
    "totalCustomers": 150,
    "newCustomersThisMonth": 12,
    "averageOrderValue": 28500.00
  },
  "generatedAt": "2024-01-01T00:00:00Z"
}
```

### **41. GET /api/reports/vehicle-master**
**Get Vehicle Master Report**
```bash
# Request
GET /api/reports/vehicle-master?brand=Toyota&includeRegistrationInfo=true
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "reportData": [
    {
      "vin": "1HGCM82633A123456",
      "brand": "Toyota",
      "model": "Camry",
      "year": 2024,
      "color": "Blue",
      "price": 25000.00,
      "mileage": 0,
      "status": "Available",
      "registrationNumber": "ABC-123456",
      "serviceCount": 1,
      "lastServiceDate": "2024-01-15T00:00:00Z"
    }
  ],
  "summary": {
    "totalVehicles": 150,
    "statusSummary": [
      {
        "status": "Available",
        "count": 120
      }
    ]
  },
  "generatedAt": "2024-01-01T00:00:00Z"
}
```

### **42. GET /api/reports/allotment-details**
**Get Allotment Details Report**
```bash
# Request
GET /api/reports/allotment-details?status=Active&includeExpired=false
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "reportData": [
    {
      "allotmentNumber": "ALL-20240101-001",
      "vehicleVin": "1HGCM82633A123456",
      "customerName": "John Doe",
      "allotmentDate": "2024-01-10T00:00:00Z",
      "expiryDate": "2024-01-25T00:00:00Z",
      "status": "Active",
      "allotmentType": "Reservation",
      "reservationAmount": 5000.00
    }
  ],
  "summary": {
    "totalAllotments": 25,
    "activeAllotments": 20,
    "expiredAllotments": 5,
    "totalReservationAmount": 125000.00
  },
  "generatedAt": "2024-01-01T00:00:00Z"
}
```

### **43. GET /api/reports/waiting-list**
**Get Waiting List Report**
```bash
# Request
GET /api/reports/waiting-list?priority=High&status=Waiting
Authorization: Bearer <jwt-token>

# Response (200 OK)
{
  "reportData": [
    {
      "requestNumber": "WL-20240101-001",
      "customerName": "John Doe",
      "requestedModel": "Camry",
      "requestedBrand": "Toyota",
      "preferredColor": "Blue",
      "minPrice": 20000.00,
      "maxPrice": 30000.00,
      "requestDate": "2024-01-05T00:00:00Z",
      "priority": "High",
      "status": "Waiting"
    }
  ],
  "summary": {
    "totalRequests": 15,
    "averageWaitingDays": 7.5,
    "priorityDistribution": [
      {
        "priority": "High",
        "count": 5
      }
    ]
  },
  "generatedAt": "2024-01-01T00:00:00Z"
}
```

---

## 📈 **Excel Export APIs** (`/api/reports/export`) - *Requires Authentication*

### **44. GET /api/reports/export/stock-availability**
**Export Stock Availability Report to Excel**
```bash
# Request
GET /api/reports/export/stock-availability?brand=Toyota&status=Available
Authorization: Bearer <jwt-token>

# Response (200 OK) - Excel File Download
Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
Content-Disposition: attachment; filename="StockAvailabilityReport_20240101.xlsx"
```

### **45. GET /api/reports/export/customer-info**
**Export Customer Information Report to Excel**
```bash
# Request
GET /api/reports/export/customer-info?city=Springfield&includeOrderHistory=true
Authorization: Bearer <jwt-token>

# Response (200 OK) - Excel File Download
Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
Content-Disposition: attachment; filename="CustomerInfoReport_20240101.xlsx"
```

### **46. GET /api/reports/export/vehicle-master**
**Export Vehicle Master Report to Excel**
```bash
# Request
GET /api/reports/export/vehicle-master?brand=Toyota&includeRegistrationInfo=true
Authorization: Bearer <jwt-token>

# Response (200 OK) - Excel File Download
Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
Content-Disposition: attachment; filename="VehicleMasterReport_20240101.xlsx"
```

### **47. GET /api/reports/export/allotment-details**
**Export Allotment Details Report to Excel**
```bash
# Request
GET /api/reports/export/allotment-details?status=Active&includeExpired=false
Authorization: Bearer <jwt-token>

# Response (200 OK) - Excel File Download
Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
Content-Disposition: attachment; filename="AllotmentDetailsReport_20240101.xlsx"
```

### **48. GET /api/reports/export/waiting-list**
**Export Waiting List Report to Excel**
```bash
# Request
GET /api/reports/export/waiting-list?priority=High&status=Waiting
Authorization: Bearer <jwt-token>

# Response (200 OK) - Excel File Download
Content-Type: application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
Content-Disposition: attachment; filename="WaitingListReport_20240101.xlsx"
```

---

## 🔄 **Returns Management APIs** (`/api/returns`) - *Requires Authentication*

### **49. GET /api/returns**
**Get All Returns**
```bash
# Request
GET /api/returns?pageNumber=1&pageSize=10
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "orderId": "507f1f77bcf86cd799439012",
    "customerId": "507f1f77bcf86cd799439013",
    "customer": {
      "id": "507f1f77bcf86cd799439013",
      "firstName": "John",
      "lastName": "Doe",
      "email": "john@customer.com",
      "phone": "+1234567890"
    },
    "vehicleId": "507f1f77bcf86cd799439014",
    "vehicle": {
      "vehicleId": "507f1f77bcf86cd799439014",
      "vin": "1HGCM82633A123456",
      "modelNumber": "CAMRY2024",
      "name": "Toyota Camry 2024",
      "brand": "Toyota",
      "price": 25000.00
    },
    "reason": "DEFECTIVE_ENGINE",
    "status": "APPROVED",
    "description": "Engine making strange noises",
    "refundAmount": 25000.00,
    "requestedAt": "2024-01-15T00:00:00Z",
    "processedAt": "2024-01-16T00:00:00Z",
    "processedBy": "507f1f77bcf86cd799439015",
    "notes": "Approved for full refund"
  }
]
```

---

## 📊 **Dashboard Analytics APIs** (`/api/dashboard`) - *Requires Authentication*

### **27. GET /api/dashboard/revenue**
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
  "topSellingVehicles": [
    {
      "vehicleId": "507f1f77bcf86cd799439011",
      "name": "Toyota Camry 2024",
      "brand": "Toyota",
      "totalSold": 25,
      "revenue": 625000.00
    }
  ]
}
```

### **28. GET /api/dashboard/customer**
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

### **29. GET /api/dashboard/top-vehicles**
**Get Top Selling Vehicles**
```bash
# Request
GET /api/dashboard/top-vehicles
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "vehicleId": "507f1f77bcf86cd799439011",
    "name": "Toyota Camry 2024",
    "brand": "Toyota",
    "soldCount": 25,
    "revenue": 625000.00
  }
]
```

### **30. GET /api/dashboard/recent-orders**
**Get Recent Orders**
```bash
# Request
GET /api/dashboard/recent-orders
Authorization: Bearer <jwt-token>

# Response (200 OK)
[
  {
    "id": "507f1f77bcf86cd799439011",
    "orderNumber": "ORD-20240101-ABC1",
    "customerName": "John Doe",
    "vehicleName": "Toyota Camry 2024",
    "totalAmount": 25000.00,
    "status": "COMPLETED",
    "orderDate": "2024-01-01T00:00:00Z"
  }
]
```

---

## 🔑 **Authentication & Authorization Notes**

### **🔐 Login Security**
- **Minimal Response**: Login returns only essential info (token, userId, role)
- **Token Claims**: All user info embedded in JWT for performance
- **Profile Endpoint**: Use `/api/users/profile` for detailed user information
- **Token Expiry**: 24 hours, automatic logout on expiry
- **User Roles**: 2 main roles (HR, Dealer) + 1 system role (Admin)

### **Roles & Permissions**
- **HR**: Human Resources - manages employees and user accounts
- **Dealer**: Vehicle Dealer - manages sales, inventory, and customer relations
- **Admin**: System Administrator - full system access (system role, not user-assignable)

### **JWT Token Usage**
```bash
# Include in request headers (token from login response)
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...

# Token contains user claims for authorization:
# - sub: User ID
# - email: User email
# - name: User full name
# - username: Username
# - role: User role (used for authorization)
# - jti: Unique token ID
# - exp: Expiration timestamp (24 hours)
# - iss: Token issuer
# - aud: Token audience

# Note: User details are embedded in token for performance
# No additional API calls needed for basic user info
```

### **Error Responses**
```json
{
  "message": "Error description"
}
```

### **Pagination**
Most list endpoints support:
- `pageNumber`: Page number (default: 1)
- `pageSize`: Items per page (default: 10)

---

## 🚀 **Ready to Test!**

All **49 API endpoints** are now documented with examples. The system follows **Clean Architecture + CQRS** principles with **feature-based organization** for maximum developer productivity! 🎯

**Base URL**: `https://your-api-domain.com/api/`
**Content-Type**: `application/json` for POST/PUT requests
**Authentication**: JWT Bearer token required for protected endpoints
**User Roles**: HR (Human Resources), Dealer (Vehicle Sales), Admin (System Administration)

### **New Features Added:**
- ✅ **Purchase Order Management** - Complete workflow for ordering vehicles
- ✅ **Goods Receipt Management** - Vehicle receiving and inspection system
- ✅ **Vehicle Registration** - Registration and ownership tracking
- ✅ **Service Order Management** - Pre-delivery and maintenance services
- ✅ **Image Upload System** - Cloudinary integration for vehicle photos
- ✅ **Comprehensive Reporting** - 5 detailed business reports
- ✅ **Excel Export** - All reports exportable to Excel format
- ✅ **Complete Business Workflow** - End-to-end vehicle showroom management

---

## 📋 **API Summary Table**

| Category | Endpoint | Method | Description | Auth Required |
|----------|----------|--------|-------------|---------------|
| **Authentication** | `/api/auth/login` | POST | User login | No |
| **Authentication** | `/api/auth/forgot-password` | POST | Request password reset | No |
| **Authentication** | `/api/auth/reset-password` | POST | Reset password with token | No |
| **Users** | `/api/users` | GET | Get all users | Yes (HR/Admin) |
| **Users** | `/api/users/{id}` | GET | Get user by ID | Yes (HR/Admin) |
| **Users** | `/api/users` | POST | Create new user | Yes (HR/Admin) |
| **Users** | `/api/users/{id}` | PUT | Update user | Yes (HR/Admin) |
| **Users** | `/api/users/{id}` | DELETE | Delete user | Yes (Admin) |
| **Users** | `/api/users/profile` | GET | Get current profile | Yes |
| **Vehicles** | `/api/vehicles` | GET | Get all vehicles | Yes |
| **Vehicles** | `/api/vehicles/{id}` | GET | Get vehicle by ID | Yes |
| **Vehicles** | `/api/vehicles` | POST | Create vehicle | Yes (Dealer/Admin) |
| **Vehicles** | `/api/vehicles/{id}` | PUT | Update vehicle | Yes (Dealer/Admin) |
| **Vehicles** | `/api/vehicles/{id}` | POST | Delete vehicle | Yes (Dealer/Admin) |
| **Vehicles** | `/api/vehicles` | POST | Delete multiple vehicles | Yes (Dealer/Admin) |
| **Orders** | `/api/orders` | GET | Get all orders | Yes |
| **Orders** | `/api/orders/{id}` | GET | Get order by ID | Yes |
| **Orders** | `/api/orders` | POST | Create order | Yes (Dealer/Admin) |
| **Orders** | `/api/orders/{id}/print` | POST | Print order | Yes (Dealer/Admin) |
| **Profile** | `/api/profile` | GET | Get current profile | Yes |
| **Profile** | `/api/profile` | PUT | Update profile | Yes |
| **Profile** | `/api/profile/change-password` | POST | Change password | Yes |
| **Employees** | `/api/employees` | GET | Get all employees | Yes |
| **Customers** | `/api/customers` | GET | Get all customers | Yes |
| **Customers** | `/api/customers/{id}/orders` | GET | Get customer orders | Yes |
| **Purchase Orders** | `/api/purchase-orders` | GET | Get all purchase orders | Yes |
| **Purchase Orders** | `/api/purchase-orders/{id}` | GET | Get purchase order by ID | Yes |
| **Purchase Orders** | `/api/purchase-orders` | POST | Create purchase order | Yes (Dealer/Admin) |
| **Purchase Orders** | `/api/purchase-orders/{id}/approve` | PUT | Approve purchase order | Yes (Admin) |
| **Goods Receipts** | `/api/goods-receipts` | GET | Get all goods receipts | Yes |
| **Goods Receipts** | `/api/goods-receipts` | POST | Create goods receipt | Yes (Dealer/Admin) |
| **Goods Receipts** | `/api/goods-receipts/{id}/accept` | PUT | Accept goods receipt | Yes (Dealer/Admin) |
| **Vehicle Registrations** | `/api/vehicle-registrations` | GET | Get all vehicle registrations | Yes |
| **Vehicle Registrations** | `/api/vehicle-registrations` | POST | Create vehicle registration | Yes (Dealer/Admin) |
| **Service Orders** | `/api/service-orders` | GET | Get all service orders | Yes |
| **Service Orders** | `/api/service-orders` | POST | Create service order | Yes (Dealer/Admin) |
| **Service Orders** | `/api/service-orders/{id}/start` | PUT | Start service order | Yes (Dealer/Admin) |
| **Images** | `/api/images/upload/vehicle/{id}` | POST | Upload vehicle image | Yes (Dealer/Admin) |
| **Reports** | `/api/reports/stock-availability` | GET | Stock availability report | Yes |
| **Reports** | `/api/reports/customer-info` | GET | Customer information report | Yes |
| **Reports** | `/api/reports/vehicle-master` | GET | Vehicle master report | Yes |
| **Reports** | `/api/reports/allotment-details` | GET | Allotment details report | Yes |
| **Reports** | `/api/reports/waiting-list` | GET | Waiting list report | Yes |
| **Excel Export** | `/api/reports/export/stock-availability` | GET | Export stock report to Excel | Yes |
| **Excel Export** | `/api/reports/export/customer-info` | GET | Export customer report to Excel | Yes |
| **Excel Export** | `/api/reports/export/vehicle-master` | GET | Export vehicle report to Excel | Yes |
| **Excel Export** | `/api/reports/export/allotment-details` | GET | Export allotment report to Excel | Yes |
| **Excel Export** | `/api/reports/export/waiting-list` | GET | Export waiting list report to Excel | Yes |
| **Returns** | `/api/returns` | GET | Get all returns | Yes |
| **Dashboard** | `/api/dashboard/revenue` | GET | Revenue analytics | Yes |
| **Dashboard** | `/api/dashboard/customer` | GET | Customer analytics | Yes |
| **Dashboard** | `/api/dashboard/top-vehicles` | GET | Top selling vehicles | Yes |
| **Dashboard** | `/api/dashboard/recent-orders` | GET | Recent orders | Yes |

---

## 🎯 **Architecture Benefits**

✅ **Clean Architecture**: Proper layer separation (Domain → Application → Infrastructure)  
✅ **CQRS Pattern**: Commands for writes, Queries for reads  
✅ **Feature Organization**: APIs grouped by business domain  
✅ **Role-Based Security**: Proper authorization throughout  
✅ **RESTful Design**: Standard HTTP methods and status codes  
✅ **Comprehensive Documentation**: Examples for all endpoints  
✅ **Production Ready**: Error handling, pagination, validation included

**Happy API Testing! 🚀**
