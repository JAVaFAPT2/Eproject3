# Vehicle Showroom Management System - Database Design

## 1. Database Overview

### 1.1 Database Engine
- **Database**: MongoDB 5.0+
- **Design Pattern**: Document-Oriented with MongoDB Driver
- **Approach**: Domain-Driven Design with embedded documents

### 1.2 Design Principles
- Document-oriented data modeling (No inheritance/sub-classing)
- Composition over inheritance following DDD principles
- Embedded documents for related data
- ObjectId primary keys
- MongoDB indexes for optimal query performance
- Audit fields for data tracking
- Flexible schema design
- Each entity is a separate aggregate root

## 2. Entity Relationship Diagram (ERD)

```
    EMPLOYEE --o{ SALES_ORDER : handles
    SUPPLIER --o{ PURCHASE_ORDER : supplies
    PURCHASE_ORDER --o{ PURCHASE_ORDER_LINE : has
    PURCHASE_ORDER --o{ GOODS_RECEIPT : received_via
    GOODS_RECEIPT --o{ VEHICLE : creates
    VEHICLE_MODEL --o{ VEHICLE : instance_of
    CUSTOMER --o{ SALES_ORDER : places
    CUSTOMER --o{ WAITING_LIST : on
    SALES_ORDER --|| VEHICLE : assigns
    SALES_ORDER --|| SERVICE_ORDER : requires
    SALES_ORDER --|| BILLING_DOCUMENT : generates
    VEHICLE --o{ ALLOTMENT : assigned_via
    CUSTOMER --o{ ALLOTMENT : assigned_to
    WAITING_LIST --o{ VEHICLE_MODEL : requests
    VEHICLE --o{ VEHICLE_REGISTRATION : has
    VEHICLE --o{ VEHICLE_IMAGE : has
    SALES_ORDER --o{ INVOICE : generates
    INVOICE --o{ PAYMENT : has
    SALES_ORDER --o{ RETURN_REQUEST : may_have

    EMPLOYEE {
        string EmployeeID PK
        string Name
        string Role "Dealer, HR"
        string Position
        date HireDate
        string Status
    }

    VEHICLE_MODEL {
        string ModelNumber PK
        string Name
        string Brand
        decimal BasePrice
    }

    VEHICLE {
        string VehicleID PK
        string ModelNumber FK
        string ExternalNumber
        json RegistrationData
        string Status
        decimal PurchasePrice
        array Photos
        date ReceiptDate
    }

    SUPPLIER {
        string SupplierID PK
        string Name
        string ContactInfo
    }

    PURCHASE_ORDER {
        string POID PK
        string SupplierID FK
        string EmployeeID FK "Dealer only"
        date OrderDate
        decimal TotalAmount
        string Status
    }

    PURCHASE_ORDER_LINE {
        string POLineID PK
        string POID FK
        string ModelNumber FK
        int Quantity
        decimal PricePerUnit
    }

    GOODS_RECEIPT {
        string ReceiptID PK
        string POID FK
        string EmployeeID FK "Dealer only"
        date ReceiptDate
        array ReceivedVehicles
    }

    CUSTOMER {
        string CustomerID PK
        string Name
        string Address
        string Phone
        string Email
    }

    SALES_ORDER {
        string SalesOrderID PK
        string CustomerID FK
        string VehicleID FK
        string EmployeeID FK "Dealer only"
        date OrderDate
        decimal SalePrice
        string Status
        json DataSheetOutput
        json ConfirmationOutput
    }

    SERVICE_ORDER {
        string ServiceOrderID PK
        string SalesOrderID FK
        string EmployeeID FK "Dealer only"
        date ServiceDate
        string Description
        decimal Cost
    }

    BILLING_DOCUMENT {
        string BillID PK
        string SalesOrderID FK
        string EmployeeID FK "Dealer only"
        date BillDate
        decimal Amount
        json Output
    }

    WAITING_LIST {
        string WaitID PK
        string CustomerID FK
        string ModelNumber FK
        date RequestDate
        string Status
    }

    ALLOTMENT {
        string AllotmentID PK
        string VehicleID FK
        string CustomerID FK
        string EmployeeID FK "Dealer only"
        date AllotmentDate
    }
```

## 3. Document Structure

### 3.1 MongoDB Collections and Documents

#### Employee Collection
```json
{
  "_id": "ObjectId",
  "employeeId": "EMP001",
  "name": "John Doe",
  "role": "Dealer",
  "position": "Senior Sales Representative",
  "hireDate": "2023-01-15T00:00:00Z",
  "status": "Active",
  "createdAt": "2023-01-15T00:00:00Z",
  "updatedAt": "2023-01-15T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### Roles Collection
```json
{
  "_id": "ObjectId",
  "roleName": "HR",
  "description": "Human Resources - manages employees and user accounts",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### UserRoles Collection (Many-to-Many between Users and Roles)
```json
{
  "_id": "ObjectId",
  "userId": "ObjectId",
  "roleId": "ObjectId",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z"
}
```

#### Brands Collection
```json
{
  "_id": "ObjectId",
  "brandName": "Honda",
  "country": "Japan",
  "logoUrl": "/images/brands/honda.jpg",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### VehicleModel Collection
```json
{
  "_id": "ObjectId",
  "modelNumber": "CIVIC-2024-001",
  "name": "Civic",
  "brand": "Honda",
  "basePrice": 25000.00,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### Vehicle Collection
```json
{
  "_id": "ObjectId",
  "vehicleId": "VEH001",
  "modelNumber": "CIVIC-2024-001",
  "externalNumber": "EXT-001",
  "registrationData": {
  "vin": "1HGCM82633A123456",
    "licensePlate": "ABC-123",
    "registrationDate": "2024-01-15T00:00:00Z",
    "expiryDate": "2025-01-15T00:00:00Z"
  },
  "status": "Available",
  "purchasePrice": 30000.00,
  "photos": [
    "/images/vehicles/vehicle1.jpg",
    "/images/vehicles/vehicle2.jpg"
  ],
  "receiptDate": "2024-01-15T00:00:00Z",
  "createdAt": "2024-01-15T00:00:00Z",
  "updatedAt": "2024-01-15T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### Customer Collection
```json
{
  "_id": "ObjectId",
  "customerId": "CUST001",
  "name": "John Doe",
  "address": "123 Main St, New York, NY 10001",
  "phone": "+1234567890",
  "email": "john@example.com",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### PurchaseOrders Collection
```json
{
  "_id": "ObjectId",
  "orderNumber": "PO-20240101-001",
  "orderDate": "2024-01-01T00:00:00Z",
  "supplierId": "ObjectId",
  "modelNumber": "CAMRY2024",
  "name": "Toyota Camry 2024",
  "brand": "Toyota",
  "price": 25000.00,
  "quantity": 5,
  "totalAmount": 125000.00,
  "status": "Pending",
  "notes": "Urgent delivery required",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### Suppliers Collection
```json
{
  "_id": "ObjectId",
  "companyName": "Acme Motors",
  "contactPerson": "Jane Doe",
  "email": "jane@acmemotors.com",
  "phone": "+1234567890",
  "address": "123 Supplier St",
  "city": "Detroit",
  "state": "MI",
  "zipCode": "48201",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### PurchaseOrderLine Collection
```json
{
  "_id": "ObjectId",
  "poLineId": "POL001",
  "poId": "PO001",
  "modelNumber": "CIVIC-2024-001",
  "quantity": 5,
  "pricePerUnit": 25000.00,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### SalesOrder Collection
```json
{
  "_id": "ObjectId",
  "salesOrderId": "SO001",
  "customerId": "CUST001",
  "vehicleId": "VEH001",
  "employeeId": "EMP001",
  "orderDate": "2024-01-20T00:00:00Z",
  "salePrice": 35000.00,
  "status": "Confirmed",
  "dataSheetOutput": {
    "format": "PDF",
    "generatedAt": "2024-01-20T00:00:00Z",
    "url": "/documents/sales-order-datasheet-so001.pdf"
  },
  "confirmationOutput": {
    "format": "PDF",
    "generatedAt": "2024-01-20T00:00:00Z",
    "url": "/documents/sales-order-confirmation-so001.pdf"
  },
  "createdAt": "2024-01-20T00:00:00Z",
  "updatedAt": "2024-01-20T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### SalesOrderItems Collection
```json
{
  "_id": "ObjectId",
  "salesOrderId": "ObjectId",
  "vehicleId": "ObjectId",
  "quantity": 1,
  "unitPrice": 35000.00,
  "discount": 0.00,
  "lineTotal": 35000.00,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### Invoices Collection
```json
{
  "_id": "ObjectId",
  "salesOrderId": "ObjectId",
  "invoiceNumber": "INV-2024-001",
  "invoiceDate": "2024-01-01T00:00:00Z",
  "subtotal": 35000.00,
  "taxAmount": 2975.00,
  "totalAmount": 37975.00,
  "status": "Paid",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### Payments Collection
```json
{
  "_id": "ObjectId",
  "invoiceId": "ObjectId",
  "amount": 37975.00,
  "paymentMethod": "CreditCard",
  "paymentDate": "2024-01-01T00:00:00Z",
  "referenceNumber": "CC-123456789",
  "status": "Completed",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### ServiceOrder Collection
```json
{
  "_id": "ObjectId",
  "serviceOrderId": "SERV001",
  "salesOrderId": "SO001",
  "employeeId": "EMP001",
  "serviceDate": "2024-01-25T10:00:00Z",
  "description": "Pre-delivery inspection and vehicle preparation",
  "cost": 250.00,
  "createdAt": "2024-01-25T00:00:00Z",
  "updatedAt": "2024-01-25T10:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### BillingDocument Collection
```json
{
  "_id": "ObjectId",
  "billId": "BILL001",
  "salesOrderId": "SO001",
  "employeeId": "EMP001",
  "billDate": "2024-01-25T00:00:00Z",
  "amount": 35250.00,
  "output": {
    "format": "PDF",
    "generatedAt": "2024-01-25T00:00:00Z",
    "url": "/documents/bill-bill001.pdf"
  },
  "createdAt": "2024-01-25T00:00:00Z",
  "updatedAt": "2024-01-25T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### ServiceOrderItems Collection
```json
{
  "_id": "ObjectId",
  "serviceOrderId": "ObjectId",
  "serviceType": "Pre-Delivery Inspection",
  "description": "Full vehicle inspection before delivery",
  "cost": 150.00,
  "status": "Completed",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-15T10:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### VehicleImages Collection
```json
{
  "_id": "ObjectId",
  "vehicleId": "ObjectId",
  "imageUrl": "/images/vehicles/honda-civic-1.jpg",
  "imageType": "Exterior",
  "fileName": "honda-civic-1.jpg",
  "fileSize": 2048576,
  "publicId": "vehicle-showroom/abc123",
  "originalFileName": "honda-civic-1.jpg",
  "contentType": "image/jpeg",
  "isPrimary": true,
  "uploadedAt": "2024-01-01T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### RefreshTokens Collection
```json
{
  "_id": "ObjectId",
  "token": "base64-encoded-refresh-token",
  "userId": "ObjectId",
  "expiresAt": "2024-01-08T00:00:00Z",
  "isRevoked": false,
  "revokedAt": null,
  "revokedByIp": null,
  "replacedByToken": null,
  "reasonRevoked": null,
  "createdByIp": "192.168.1.1",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### GoodsReceipts Collection
```json
{
  "_id": "ObjectId",
  "receiptNumber": "GR-20240101-001",
  "purchaseOrderId": "ObjectId",
  "receiptDate": "2024-01-15T00:00:00Z",
  "vehicleId": "ObjectId",
  "vin": "1HGCM82633A123456",
  "manufacturerVIN": "MFG-123456789",
  "status": "Accepted",
  "notes": "Vehicle in excellent condition",
  "createdAt": "2024-01-15T00:00:00Z",
  "updatedAt": "2024-01-15T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### VehicleRegistrations Collection
```json
{
  "_id": "ObjectId",
  "registrationNumber": "ABC-123456",
  "vehicleId": "ObjectId",
  "vin": "1HGCM82633A123456",
  "customerId": "ObjectId",
  "registrationDate": "2024-01-20T00:00:00Z",
  "expiryDate": "2025-01-20T00:00:00Z",
  "issuingAuthority": "DMV",
  "status": "Active",
  "ownerName": "John Doe",
  "ownerAddress": "123 Main St, City, State 12345",
  "createdAt": "2024-01-20T00:00:00Z",
  "updatedAt": "2024-01-20T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### Allotment Collection
```json
{
  "_id": "ObjectId",
  "allotmentId": "ALL001",
  "vehicleId": "VEH001",
  "customerId": "CUST001",
  "employeeId": "EMP001",
  "allotmentDate": "2024-01-10T00:00:00Z",
  "createdAt": "2024-01-10T00:00:00Z",
  "updatedAt": "2024-01-10T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### WaitingList Collection
```json
{
  "_id": "ObjectId",
  "waitId": "WAIT001",
  "customerId": "CUST001",
  "modelNumber": "CIVIC-2024-001",
  "requestDate": "2024-01-05T00:00:00Z",
  "status": "Waiting",
  "createdAt": "2024-01-05T00:00:00Z",
  "updatedAt": "2024-01-05T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

#### ReturnRequests Collection
```json
{
  "_id": "ObjectId",
  "salesOrderId": "ObjectId",
  "vehicleId": "ObjectId",
  "reason": "Defective product",
  "status": "Pending",
  "requestDate": "2024-01-25T00:00:00Z",
  "processedDate": null,
  "createdAt": "2024-01-25T00:00:00Z",
  "updatedAt": "2024-01-25T00:00:00Z",
  "isDeleted": false,
  "deletedAt": null
}
```

## 4. MongoDB Indexes and Performance Optimization

### 4.1 Automatic Indexes
- Primary keys (_id) are automatically indexed in MongoDB
- ObjectId fields are automatically indexed for efficient queries

### 4.2 Strategic Indexes for Performance
```javascript
// Employee collection indexes
db.employees.createIndex({ "employeeId": 1 }, { unique: true });
db.employees.createIndex({ "role": 1 });
db.employees.createIndex({ "status": 1 });
db.employees.createIndex({ "isDeleted": 1 });

// VehicleModel collection indexes
db.vehicleModels.createIndex({ "modelNumber": 1 }, { unique: true });
db.vehicleModels.createIndex({ "name": 1 });
db.vehicleModels.createIndex({ "brand": 1 });
db.vehicleModels.createIndex({ "isDeleted": 1 });

// Vehicle collection indexes
db.vehicles.createIndex({ "vehicleId": 1 }, { unique: true });
db.vehicles.createIndex({ "modelNumber": 1 });
db.vehicles.createIndex({ "status": 1 });
db.vehicles.createIndex({ "isDeleted": 1 });

// Customer collection indexes
db.customers.createIndex({ "customerId": 1 }, { unique: true });
db.customers.createIndex({ "email": 1 }, { unique: true });
db.customers.createIndex({ "phone": 1 });
db.customers.createIndex({ "isDeleted": 1 });

// Supplier collection indexes
db.suppliers.createIndex({ "supplierId": 1 }, { unique: true });
db.suppliers.createIndex({ "name": 1 });
db.suppliers.createIndex({ "isDeleted": 1 });

// PurchaseOrder collection indexes
db.purchaseOrders.createIndex({ "poId": 1 }, { unique: true });
db.purchaseOrders.createIndex({ "supplierId": 1 });
db.purchaseOrders.createIndex({ "employeeId": 1 });
db.purchaseOrders.createIndex({ "orderDate": 1 });
db.purchaseOrders.createIndex({ "status": 1 });
db.purchaseOrders.createIndex({ "isDeleted": 1 });

// PurchaseOrderLine collection indexes
db.purchaseOrderLines.createIndex({ "poLineId": 1 }, { unique: true });
db.purchaseOrderLines.createIndex({ "poId": 1 });
db.purchaseOrderLines.createIndex({ "modelNumber": 1 });
db.purchaseOrderLines.createIndex({ "isDeleted": 1 });

// GoodsReceipt collection indexes
db.goodsReceipts.createIndex({ "receiptId": 1 }, { unique: true });
db.goodsReceipts.createIndex({ "poId": 1 });
db.goodsReceipts.createIndex({ "employeeId": 1 });
db.goodsReceipts.createIndex({ "receiptDate": 1 });
db.goodsReceipts.createIndex({ "isDeleted": 1 });

// SalesOrder collection indexes
db.salesOrders.createIndex({ "salesOrderId": 1 }, { unique: true });
db.salesOrders.createIndex({ "customerId": 1 });
db.salesOrders.createIndex({ "vehicleId": 1 });
db.salesOrders.createIndex({ "employeeId": 1 });
db.salesOrders.createIndex({ "orderDate": 1 });
db.salesOrders.createIndex({ "status": 1 });
db.salesOrders.createIndex({ "isDeleted": 1 });

// ServiceOrder collection indexes
db.serviceOrders.createIndex({ "serviceOrderId": 1 }, { unique: true });
db.serviceOrders.createIndex({ "salesOrderId": 1 });
db.serviceOrders.createIndex({ "employeeId": 1 });
db.serviceOrders.createIndex({ "serviceDate": 1 });
db.serviceOrders.createIndex({ "isDeleted": 1 });

// BillingDocument collection indexes
db.billingDocuments.createIndex({ "billId": 1 }, { unique: true });
db.billingDocuments.createIndex({ "salesOrderId": 1 });
db.billingDocuments.createIndex({ "employeeId": 1 });
db.billingDocuments.createIndex({ "billDate": 1 });
db.billingDocuments.createIndex({ "isDeleted": 1 });

// WaitingList collection indexes
db.waitingLists.createIndex({ "waitId": 1 }, { unique: true });
db.waitingLists.createIndex({ "customerId": 1 });
db.waitingLists.createIndex({ "modelNumber": 1 });
db.waitingLists.createIndex({ "requestDate": 1 });
db.waitingLists.createIndex({ "status": 1 });
db.waitingLists.createIndex({ "isDeleted": 1 });

// Allotment collection indexes
db.allotments.createIndex({ "allotmentId": 1 }, { unique: true });
db.allotments.createIndex({ "vehicleId": 1 });
db.allotments.createIndex({ "customerId": 1 });
db.allotments.createIndex({ "employeeId": 1 });
db.allotments.createIndex({ "allotmentDate": 1 });
db.allotments.createIndex({ "isDeleted": 1 });

// Roles collection indexes (keeping for backward compatibility)
db.roles.createIndex({ "roleName": 1 }, { unique: true });
db.roles.createIndex({ "isDeleted": 1 });

// RefreshTokens collection indexes
db.refreshTokens.createIndex({ "token": 1 }, { unique: true });
db.refreshTokens.createIndex({ "userId": 1 });
db.refreshTokens.createIndex({ "expiresAt": 1 });
db.refreshTokens.createIndex({ "isRevoked": 1 });
db.refreshTokens.createIndex({ "isDeleted": 1 });
```

## 5. Domain-Driven Design Mapping

### 5.1 Aggregates
- **Vehicle Aggregate**: Vehicle, VehicleModel
- **Sales Aggregate**: SalesOrder, ServiceOrder, BillingDocument
- **Purchase Aggregate**: PurchaseOrder, PurchaseOrderLine, GoodsReceipt
- **Customer Aggregate**: Customer, WaitingList, Allotment
- **Employee Aggregate**: Employee

### 5.2 Value Objects
- **Address**: Street, City, State, ZipCode (embedded in Customer)
- **Money**: Amount, Currency (decimal fields)
- **ContactInfo**: Email, Phone (embedded in entities)
- **RoleInfo**: RoleId, RoleName, Description (embedded in User)
- **ModelInfo**: Model details (embedded in Vehicle)
- **BrandInfo**: Brand details (embedded in Model)

### 5.3 Domain Events
- VehiclePurchased
- VehicleSold
- VehicleReserved
- PaymentReceived
- ServiceCompleted
- VehicleRegistered
- VehicleAllotted

## 6. MongoDB Security Considerations

### 6.1 Document-Level Security
MongoDB implements security at the document level through:
- **Role-Based Access Control**: Users have roles that define permissions
- **Field-Level Security**: Sensitive fields can be hidden based on user roles
- **Document Filtering**: Queries automatically filter based on user permissions

```javascript
// Example: Users can only access their own documents unless they have admin role
db.users.find({
  $or: [
    { "userId": currentUserId },
    { "role.roleName": { $in: ["Admin", "Manager"] } }
  ]
});
```

### 6.2 Audit Logging
MongoDB provides built-in audit logging capabilities:
- **Document Changes**: All create, update, delete operations are logged
- **User Actions**: Track who performed what operations
- **Field-Level Changes**: Log changes to specific fields
- **Timestamp Tracking**: Automatic audit trail with timestamps

```javascript
// Example audit log entry
{
  "_id": "ObjectId",
  "collectionName": "users",
  "operation": "UPDATE",
  "userId": "ObjectId",
  "documentId": "ObjectId",
  "oldValues": {
    "firstName": "John",
    "lastName": "Doe"
  },
  "newValues": {
    "firstName": "Jane",
    "lastName": "Smith"
  },
  "timestamp": "2024-01-01T10:30:00Z"
}
```

## 7. MongoDB Setup and Initialization

### 7.1 MongoDB Collection Initialization
```javascript
// Create collections with indexes
db.createCollection("employees");
db.createCollection("roles");
db.createCollection("vehicleModels");
db.createCollection("vehicles");
db.createCollection("customers");
db.createCollection("suppliers");
db.createCollection("purchaseOrders");
db.createCollection("purchaseOrderLines");
db.createCollection("goodsReceipts");
db.createCollection("salesOrders");
db.createCollection("serviceOrders");
db.createCollection("billingDocuments");
db.createCollection("waitingLists");
db.createCollection("allotments");
db.createCollection("refreshTokens");

// Create indexes (as defined above)
```

### 7.2 Seed Data
- Default roles (HR, Dealer, Admin)
- Default admin user (username: admin, password: Admin123!)
- Sample brands and models
- System configuration

### 7.3 MongoDB Connection Setup
```csharp
// In Program.cs
services.AddSingleton<IMongoClient>(sp =>
{
    var configuration = sp.GetRequiredService<IConfiguration>();
    var connectionString = configuration.GetConnectionString("MongoDB");
    return new MongoClient(connectionString);
});
```

---

**Document Version**: 2.0
**Last Updated**: 2024-01-15
**Prepared by**: Development Team