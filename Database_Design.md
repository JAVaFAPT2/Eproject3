# Vehicle Showroom Management System - Database Design

## 1. Database Overview

### 1.1 Database Engine
- **Database**: MongoDB 5.0+
- **Design Pattern**: Document-Oriented with MongoDB Driver
- **Approach**: Domain-Driven Design with embedded documents

### 1.2 Design Principles
- Document-oriented data modeling
- Embedded documents for related data
- ObjectId primary keys
- MongoDB indexes for optimal query performance
- Audit fields for data tracking
- Flexible schema design

## 2. Entity Relationship Diagram (ERD)

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│     Users       │    │     Roles       │    │   UserRoles     │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ UserId (PK)     │◄──►│ RoleId (PK)     │    │ UserId (PK,FK)  │
│ Username        │    │ RoleName        │    │ RoleId (PK,FK)  │
│ Email           │    │ Description     │    │ CreatedAt       │
│ PasswordHash    │    │ CreatedAt       │    │ UpdatedAt       │
│ FirstName       │    │ UpdatedAt       │    └─────────────────┘
│ LastName        │    └─────────────────┘
│ Role (FK)       │
│ IsActive        │
│ CreatedAt       │
│ UpdatedAt       │
└─────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Vehicles      │    │    Brands       │    │    Models       │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ VehicleId (PK)  │    │ BrandId (PK)    │    │ ModelId (PK)    │
│ VIN             │    │ BrandName       │    │ ModelName       │
│ ModelId (FK)    │    │ Country         │    │ BrandId (FK)    │
│ Color           │    │ LogoUrl         │    │ EngineType      │
│ Year            │    │ CreatedAt       │    │ Transmission    │
│ Price           │    │ UpdatedAt       │    │ FuelType        │
│ Mileage         │    └─────────────────┘    │ SeatingCapacity │
│ Status          │                          │ CreatedAt       │
│ CreatedAt       │                          │ UpdatedAt       │
│ UpdatedAt       │                          └─────────────────┘
└─────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Customers     │    │ PurchaseOrders  │    │    Suppliers    │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ CustomerId (PK) │    │ PurchaseId (PK) │    │ SupplierId (PK) │
│ FirstName       │    │ SupplierId (FK) │    │ CompanyName     │
│ LastName        │    │ VehicleId (FK)  │    │ ContactPerson   │
│ Email           │    │ Quantity        │    │ Email           │
│ Phone           │    │ UnitPrice       │    │ Phone           │
│ Address         │    │ TotalAmount     │    │ Address         │
│ City            │    │ Status          │    │ City            │
│ State           │    │ OrderDate       │    │ State           │
│ ZipCode         │    │ DeliveryDate    │    │ ZipCode         │
│ CreatedAt       │    │ CreatedAt       │    │ CreatedAt       │
│ UpdatedAt       │    │ UpdatedAt       │    │ UpdatedAt       │
└─────────────────┘    └─────────────────┘    └─────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│  SalesOrders    │    │ SalesOrderItems │    │     Invoices    │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ SalesOrderId(PK)│    │ SalesOrderItemId│    │ InvoiceId (PK)  │
│ CustomerId (FK) │    │ SalesOrderId(FK)│    │ SalesOrderId(FK)│
│ SalesPersonId(FK)│   │ VehicleId (FK)  │    │ InvoiceNumber   │
│ OrderDate       │    │ Quantity        │    │ InvoiceDate     │
│ Status          │    │ UnitPrice       │    │ Subtotal        │
│ TotalAmount     │    │ Discount        │    │ TaxAmount       │
│ CreatedAt       │    │ LineTotal       │    │ TotalAmount     │
│ UpdatedAt       │    │ CreatedAt       │    │ Status          │
└─────────────────┘    │ UpdatedAt       │    │ CreatedAt       │
                      └─────────────────┘    │ UpdatedAt       │
                                             └─────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│ ServiceOrders   │    │ServiceOrderItems│    │   VehicleImages │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ ServiceOrderId(PK)│ │ServiceOrderItemId│ │ ImageId (PK)    │
│ VehicleId (FK)  │    │ ServiceOrderId(FK)│ │ VehicleId (FK)  │
│ CustomerId (FK) │    │ ServiceType     │ │ ImageUrl        │
│ ServiceDate     │    │ Description     │ │ ImageType       │
│ Status          │    │ Cost            │ │ FileName        │
│ TotalCost       │    │ Status          │ │ FileSize        │
│ CreatedAt       │    │ CreatedAt       │ │ UploadedAt      │
│ UpdatedAt       │    │ UpdatedAt       │ │ CreatedAt       │
└─────────────────┘    └─────────────────┘    │ UpdatedAt       │
                                             └─────────────────┘
```

## 3. Document Structure

### 3.1 MongoDB Collections and Documents

#### Users Collection
```json
{
  "_id": "ObjectId",
  "username": "johndoe",
  "email": "john@example.com",
  "passwordHash": "hashed_password",
  "firstName": "John",
  "lastName": "Doe",
  "roleId": "ObjectId",
  "role": {
    "roleId": "ObjectId",
    "roleName": "Dealer",
    "description": "Vehicle dealer role"
  },
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "userRoleIds": ["ObjectId1", "ObjectId2"],
  "salesOrderIds": ["ObjectId1", "ObjectId2"]
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
  "userIds": ["ObjectId1", "ObjectId2"]
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
  "modelIds": ["ObjectId1", "ObjectId2"]
}
```

#### Models Collection
```json
{
  "_id": "ObjectId",
  "modelName": "Civic",
  "brandId": "ObjectId",
  "brand": {
    "brandId": "ObjectId",
    "brandName": "Honda",
    "country": "Japan"
  },
  "engineType": "1.5L Turbo",
  "transmission": "CVT",
  "fuelType": "Gasoline",
  "seatingCapacity": 5,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "vehicleIds": ["ObjectId1", "ObjectId2"]
}
```

#### Vehicles Collection
```json
{
  "_id": "ObjectId",
  "vin": "1HGCM82633A123456",
  "modelId": "ObjectId",
  "model": {
    "modelId": "ObjectId",
    "modelName": "Civic",
    "brandId": "ObjectId",
    "brand": {
      "brandId": "ObjectId",
      "brandName": "Honda",
      "country": "Japan"
    },
    "engineType": "1.5L Turbo",
    "transmission": "CVT",
    "fuelType": "Gasoline",
    "seatingCapacity": 5
  },
  "color": "Blue",
  "year": 2024,
  "price": 35000.00,
  "mileage": 15000,
  "status": "Available",
  "images": [
    {
      "imageId": "ObjectId",
      "imageUrl": "/images/vehicles/vehicle1.jpg",
      "imageType": "Exterior",
      "fileName": "vehicle1.jpg"
    }
  ],
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "purchaseOrderItemIds": ["ObjectId1"],
  "salesOrderItemIds": ["ObjectId1"],
  "serviceOrderIds": ["ObjectId1"]
}
```

#### Customers Collection
```json
{
  "_id": "ObjectId",
  "firstName": "John",
  "lastName": "Doe",
  "email": "john@example.com",
  "phone": "+1234567890",
  "address": "123 Main St",
  "city": "New York",
  "state": "NY",
  "zipCode": "10001",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "salesOrderIds": ["ObjectId1", "ObjectId2"],
  "serviceOrderIds": ["ObjectId1"]
}
```

#### Suppliers Collection
```json
{
  "_id": "ObjectId",
  "companyName": "Honda Motor Co.",
  "contactPerson": "John Smith",
  "email": "john.smith@honda.com",
  "phone": "+1234567890",
  "address": "123 Corporate Blvd",
  "city": "Tokyo",
  "state": "Tokyo",
  "zipCode": "100-0011",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false
}
```

#### PurchaseOrders Collection
```json
{
  "_id": "ObjectId",
  "supplierId": "ObjectId",
  "status": "Confirmed",
  "orderDate": "2024-01-01T00:00:00Z",
  "deliveryDate": "2024-02-01T00:00:00Z",
  "totalAmount": 35000.00,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "items": [
    {
      "purchaseOrderItemId": "ObjectId",
      "vehicleId": "ObjectId",
      "quantity": 1,
      "unitPrice": 35000.00,
      "totalAmount": 35000.00,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z",
      "isDeleted": false
    }
  ]
}
```

#### PurchaseOrderItems Collection
```json
{
  "_id": "ObjectId",
  "purchaseId": "ObjectId",
  "vehicleId": "ObjectId",
  "quantity": 1,
  "unitPrice": 35000.00,
  "totalAmount": 35000.00,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false
}
```

#### SalesOrders Collection
```json
{
  "_id": "ObjectId",
  "customerId": "ObjectId",
  "salesPersonId": "ObjectId",
  "status": "Confirmed",
  "orderDate": "2024-01-01T00:00:00Z",
  "totalAmount": 35000.00,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "items": [
    {
      "salesOrderItemId": "ObjectId",
      "vehicleId": "ObjectId",
      "quantity": 1,
      "unitPrice": 35000.00,
      "discount": 0.00,
      "lineTotal": 35000.00,
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-01T00:00:00Z",
      "isDeleted": false
    }
  ]
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
  "isDeleted": false
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
  "isDeleted": false
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
  "isDeleted": false
}
```

#### ServiceOrders Collection
```json
{
  "_id": "ObjectId",
  "vehicleId": "ObjectId",
  "customerId": "ObjectId",
  "serviceDate": "2024-01-15T10:00:00Z",
  "status": "Completed",
  "totalCost": 250.00,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-15T10:00:00Z",
  "isDeleted": false,
  "items": [
    {
      "serviceOrderItemId": "ObjectId",
      "serviceType": "Pre-Delivery Inspection",
      "description": "Full vehicle inspection before delivery",
      "cost": 150.00,
      "status": "Completed",
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-15T10:00:00Z",
      "isDeleted": false
    },
    {
      "serviceOrderItemId": "ObjectId",
      "serviceType": "Detailing",
      "description": "Interior and exterior detailing",
      "cost": 100.00,
      "status": "Completed",
      "createdAt": "2024-01-01T00:00:00Z",
      "updatedAt": "2024-01-15T10:00:00Z",
      "isDeleted": false
    }
  ]
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
  "isDeleted": false
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
  "uploadedAt": "2024-01-01T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false
}
```

## 4. MongoDB Indexes and Performance Optimization

### 4.1 Automatic Indexes
- Primary keys (_id) are automatically indexed in MongoDB
- ObjectId fields are automatically indexed for efficient queries

### 4.2 Strategic Indexes for Performance
```javascript
// Users collection indexes
db.users.createIndex({ "email": 1 }, { unique: true });
db.users.createIndex({ "username": 1 }, { unique: true });
db.users.createIndex({ "roleId": 1 });
db.users.createIndex({ "isDeleted": 1 });

// Vehicles collection indexes
db.vehicles.createIndex({ "vin": 1 }, { unique: true });
db.vehicles.createIndex({ "status": 1 });
db.vehicles.createIndex({ "modelId": 1 });
db.vehicles.createIndex({ "isDeleted": 1 });

// Customers collection indexes
db.customers.createIndex({ "email": 1 }, { unique: true });
db.customers.createIndex({ "phone": 1 });
db.customers.createIndex({ "isDeleted": 1 });

// SalesOrders collection indexes
db.salesOrders.createIndex({ "customerId": 1 });
db.salesOrders.createIndex({ "status": 1 });
db.salesOrders.createIndex({ "orderDate": 1 });
db.salesOrders.createIndex({ "isDeleted": 1 });

// SalesOrderItems collection indexes
db.salesOrderItems.createIndex({ "salesOrderId": 1 });
db.salesOrderItems.createIndex({ "vehicleId": 1 });
db.salesOrderItems.createIndex({ "isDeleted": 1 });

// Roles collection indexes
db.roles.createIndex({ "roleName": 1 }, { unique: true });
db.roles.createIndex({ "isDeleted": 1 });

// Brands collection indexes
db.brands.createIndex({ "brandName": 1 }, { unique: true });
db.brands.createIndex({ "isDeleted": 1 });

// Models collection indexes
db.models.createIndex({ "modelName": 1, "brandId": 1 }, { unique: true });
db.models.createIndex({ "isDeleted": 1 });
```

## 5. Domain-Driven Design Mapping

### 5.1 Aggregates
- **Vehicle Aggregate**: Vehicle, VehicleImages
- **Sales Aggregate**: SalesOrder, SalesOrderItems, Invoice, Payments
- **Service Aggregate**: ServiceOrder
- **User Aggregate**: User, UserRoles

### 5.2 Value Objects
- **Address**: Street, City, State, ZipCode
- **Money**: Amount, Currency
- **ContactInfo**: Email, Phone

### 5.3 Domain Events
- VehiclePurchased
- VehicleSold
- VehicleReserved
- PaymentReceived
- ServiceCompleted

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
db.createCollection("users");
db.createCollection("roles");
db.createCollection("userRoles");
db.createCollection("brands");
db.createCollection("models");
db.createCollection("vehicles");
db.createCollection("vehicleImages");
db.createCollection("customers");
db.createCollection("salesOrders");
db.createCollection("salesOrderItems");
db.createCollection("invoices");
db.createCollection("payments");
db.createCollection("serviceOrders");

// Create indexes (already defined in VehicleShowroomDbContext)
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

**Document Version**: 1.0
**Last Updated**: $(date)
**Prepared by**: Development Team