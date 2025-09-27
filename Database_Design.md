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

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│PurchaseOrders   │    │  GoodsReceipts  │    │VehicleRegistrations│
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ PurchaseOrderId(PK)│ │ GoodsReceiptId(PK)│ │ VehicleRegId(PK)│
│ OrderNumber     │    │ ReceiptNumber   │    │ RegistrationNumber│
│ OrderDate       │    │ PurchaseOrderId(FK)│ │ VehicleId (FK)  │
│ SupplierId (FK) │    │ ReceiptDate     │    │ VIN             │
│ ModelNumber     │    │ VehicleId (FK)  │    │ CustomerId (FK) │
│ Name            │    │ VIN             │    │ RegistrationDate│
│ Brand           │    │ ManufacturerVIN │    │ ExpiryDate      │
│ Price           │    │ Status          │    │ IssuingAuthority│
│ Quantity        │    │ Notes           │    │ Status          │
│ TotalAmount     │    │ CreatedAt       │    │ OwnerName       │
│ Status          │    │ UpdatedAt       │    │ OwnerAddress    │
│ Notes           │    │ IsDeleted       │    │ CreatedAt       │
│ CreatedAt       │    │ DeletedAt       │    │ UpdatedAt       │
│ UpdatedAt       │    └─────────────────┘    │ IsDeleted       │
│ IsDeleted       │                           │ DeletedAt       │
│ DeletedAt       │                           └─────────────────┘
└─────────────────┘

┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Allotments    │    │  WaitingLists   │    │   ModelInfo     │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ AllotmentId (PK)│    │ WaitingListId(PK)│   │ ModelId (PK)    │
│ AllotmentNumber │    │ RequestNumber   │    │ ModelName       │
│ VehicleId (FK)  │    │ CustomerId (FK) │    │ ModelNumber     │
│ CustomerId (FK) │    │ RequestedVehicleModelId│ BrandId (FK)   │
│ AllotmentDate   │    │ RequestedVehicleBrandId│ EngineType     │
│ ExpiryDate      │    │ PreferredColor  │    │ Transmission    │
│ Status          │    │ MinPrice        │    │ FuelType        │
│ AllotmentType   │    │ MaxPrice        │    │ SeatingCapacity │
│ ReservationAmount│   │ RequestDate     │    │ CreatedAt       │
│ Notes           │    │ Priority        │    │ UpdatedAt       │
│ CreatedAt       │    │ Status          │    │ IsDeleted       │
│ UpdatedAt       │    │ Notes           │    │ DeletedAt       │
│ IsDeleted       │    │ CreatedAt       │    └─────────────────┘
│ DeletedAt       │    │ UpdatedAt       │
└─────────────────┘    │ IsDeleted       │
                      │ DeletedAt       │
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
  "publicId": "vehicle-showroom/abc123",
  "originalFileName": "honda-civic-1.jpg",
  "contentType": "image/jpeg",
  "isPrimary": true,
  "uploadedAt": "2024-01-01T00:00:00Z",
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false
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
  "isDeleted": false
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
  "isDeleted": false
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
  "isDeleted": false
}
```

#### Allotments Collection
```json
{
  "_id": "ObjectId",
  "allotmentNumber": "ALL-20240101-001",
  "vehicleId": "ObjectId",
  "customerId": "ObjectId",
  "allotmentDate": "2024-01-10T00:00:00Z",
  "expiryDate": "2024-01-25T00:00:00Z",
  "status": "Active",
  "allotmentType": "Reservation",
  "reservationAmount": 5000.00,
  "notes": "Customer requested specific color",
  "salesPersonId": "ObjectId",
  "createdAt": "2024-01-10T00:00:00Z",
  "updatedAt": "2024-01-10T00:00:00Z",
  "isDeleted": false
}
```

#### WaitingLists Collection
```json
{
  "_id": "ObjectId",
  "requestNumber": "WL-20240101-001",
  "customerId": "ObjectId",
  "requestedVehicleModelId": "ObjectId",
  "requestedVehicleBrandId": "ObjectId",
  "preferredColor": "Blue",
  "minPrice": 20000.00,
  "maxPrice": 30000.00,
  "requestDate": "2024-01-05T00:00:00Z",
  "priority": "High",
  "status": "Waiting",
  "notes": "Customer prefers automatic transmission",
  "createdAt": "2024-01-05T00:00:00Z",
  "updatedAt": "2024-01-05T00:00:00Z",
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

// PurchaseOrders collection indexes
db.purchaseOrders.createIndex({ "orderNumber": 1 }, { unique: true });
db.purchaseOrders.createIndex({ "supplierId": 1 });
db.purchaseOrders.createIndex({ "status": 1 });
db.purchaseOrders.createIndex({ "orderDate": 1 });
db.purchaseOrders.createIndex({ "isDeleted": 1 });

// GoodsReceipts collection indexes
db.goodsReceipts.createIndex({ "receiptNumber": 1 }, { unique: true });
db.goodsReceipts.createIndex({ "purchaseOrderId": 1 });
db.goodsReceipts.createIndex({ "vehicleId": 1 });
db.goodsReceipts.createIndex({ "vin": 1 }, { unique: true });
db.goodsReceipts.createIndex({ "status": 1 });
db.goodsReceipts.createIndex({ "isDeleted": 1 });

// VehicleRegistrations collection indexes
db.vehicleRegistrations.createIndex({ "registrationNumber": 1 }, { unique: true });
db.vehicleRegistrations.createIndex({ "vehicleId": 1 });
db.vehicleRegistrations.createIndex({ "vin": 1 });
db.vehicleRegistrations.createIndex({ "customerId": 1 });
db.vehicleRegistrations.createIndex({ "status": 1 });
db.vehicleRegistrations.createIndex({ "expiryDate": 1 });
db.vehicleRegistrations.createIndex({ "isDeleted": 1 });

// Allotments collection indexes
db.allotments.createIndex({ "allotmentNumber": 1 }, { unique: true });
db.allotments.createIndex({ "vehicleId": 1 });
db.allotments.createIndex({ "customerId": 1 });
db.allotments.createIndex({ "status": 1 });
db.allotments.createIndex({ "allotmentDate": 1 });
db.allotments.createIndex({ "expiryDate": 1 });
db.allotments.createIndex({ "isDeleted": 1 });

// WaitingLists collection indexes
db.waitingLists.createIndex({ "requestNumber": 1 }, { unique: true });
db.waitingLists.createIndex({ "customerId": 1 });
db.waitingLists.createIndex({ "requestedVehicleModelId": 1 });
db.waitingLists.createIndex({ "requestedVehicleBrandId": 1 });
db.waitingLists.createIndex({ "status": 1 });
db.waitingLists.createIndex({ "priority": 1 });
db.waitingLists.createIndex({ "requestDate": 1 });
db.waitingLists.createIndex({ "isDeleted": 1 });

// VehicleImages collection indexes
db.vehicleImages.createIndex({ "vehicleId": 1 });
db.vehicleImages.createIndex({ "imageType": 1 });
db.vehicleImages.createIndex({ "isPrimary": 1 });
db.vehicleImages.createIndex({ "isDeleted": 1 });
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

## 6. Third Normal Form (3NF) Optimization in MongoDB

### 6.1 MongoDB and Normalization Principles

While MongoDB is a document database and traditional relational normalization rules (1NF, 2NF, 3NF) don't apply directly, we can still optimize our document design to achieve similar benefits:

#### **3NF Principles Applied to MongoDB:**

1. **Eliminate Transitive Dependencies**: Ensure no non-key attributes depend on other non-key attributes
2. **Minimize Data Redundancy**: Avoid duplicating data across documents
3. **Optimize Document Relationships**: Use proper referencing vs. embedding strategies
4. **Ensure Functional Dependencies**: Maintain data integrity through document structure

### 6.2 Current Database Design Analysis

#### **✅ Well-Normalized Collections:**

**Users Collection (3NF Compliant):**
```json
{
  "_id": "ObjectId",
  "username": "johndoe",
  "email": "john@example.com",
  "roleId": "ObjectId",  // References Role collection
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z"
}
```

**Roles Collection (3NF Compliant):**
```json
{
  "_id": "ObjectId",
  "roleName": "Dealer",
  "description": "Vehicle dealer role",
  "permissions": ["READ_VEHICLES", "CREATE_ORDERS"]
}
```

#### **⚠️ Areas for 3NF Optimization:**

**Vehicles Collection (Needs Optimization):**
```json
// Current Design - Embedded Model/Brand (Potential Redundancy)
{
  "_id": "ObjectId",
  "vin": "1HGCM82633A123456",
  "model": {
    "modelId": "ObjectId",
    "modelName": "Civic",
    "brand": {
      "brandId": "ObjectId",
      "brandName": "Honda",
      "country": "Japan"
    },
    "engineType": "1.5L Turbo"
  },
  "price": 35000
}

// Optimized Design - Referenced Model (3NF Compliant)
{
  "_id": "ObjectId",
  "vin": "1HGCM82633A123456",
  "modelId": "ObjectId",  // References Models collection
  "price": 35000,
  "customizations": ["Sunroof", "Navigation"]
}
```

### 6.3 3NF Optimization Recommendations

#### **1. Separate Reference Collections**

**Models Collection (New - 3NF Compliant):**
```json
{
  "_id": "ObjectId",
  "modelName": "Civic",
  "brandId": "ObjectId",  // References Brands collection
  "engineType": "1.5L Turbo",
  "transmission": "CVT",
  "fuelType": "Gasoline",
  "seatingCapacity": 5,
  "specifications": {
    "horsepower": 158,
    "torque": "138 lb-ft",
    "mpg": { "city": 31, "highway": 40 }
  }
}
```

**Brands Collection (New - 3NF Compliant):**
```json
{
  "_id": "ObjectId",
  "brandName": "Honda",
  "country": "Japan",
  "headquarters": "Tokyo, Japan",
  "foundedYear": 1948,
  "website": "https://www.honda.com"
}
```

#### **2. Customer Address Normalization**

**Current Customer (Embedded Address):**
```json
{
  "_id": "ObjectId",
  "name": "John Doe",
  "email": "john@example.com",
  "address": {
    "street": "123 Main St",
    "city": "Anytown",
    "state": "CA",
    "zipCode": "12345"
  }
}
```

**Optimized Customer (Referenced Address):**
```json
{
  "_id": "ObjectId",
  "name": "John Doe",
  "email": "john@example.com",
  "primaryAddressId": "ObjectId"  // References Addresses collection
}
```

**Addresses Collection (New):**
```json
{
  "_id": "ObjectId",
  "street": "123 Main St",
  "city": "Anytown",
  "state": "CA",
  "zipCode": "12345",
  "type": "Primary"
}
```

### 6.4 Benefits of 3NF Optimization

#### **✅ Reduced Data Redundancy**
- **Before**: Brand information repeated in every vehicle document
- **After**: Brand information stored once, referenced by many vehicles
- **Space Savings**: ~60% reduction in storage for frequently repeated data

#### **✅ Improved Data Integrity**
- **Consistency**: Updates to brand information apply to all vehicles
- **Validation**: Centralized business rules and constraints
- **Referential Integrity**: Clear relationships between documents

#### **✅ Enhanced Query Performance**
- **Targeted Queries**: Query specific collections instead of large documents
- **Better Indexing**: More focused indexes on smaller collections
- **Optimized Aggregations**: Cleaner aggregation pipelines

#### **✅ Simplified Maintenance**
- **Updates**: Single source of truth for reference data
- **Schema Evolution**: Easier to modify reference collections
- **Data Migration**: Cleaner migration paths

### 6.5 Implementation Strategy

#### **Phase 1: Reference Data Separation**
1. Extract Brands → Separate `brands` collection
2. Extract Models → Separate `models` collection
3. Extract Addresses → Separate `addresses` collection
4. Update existing documents to use references

#### **Phase 2: Specification Normalization**
1. Extract Vehicle Specifications → Separate `specifications` collection
2. Update Vehicle documents to reference specifications
3. Migrate existing specification data

#### **Phase 3: Query Optimization**
1. Update application queries to use `$lookup` for referenced data
2. Implement proper indexing on reference fields
3. Optimize aggregation pipelines

#### **Phase 4: Performance Monitoring**
1. Monitor query performance after normalization
2. Adjust indexes based on actual usage patterns
3. Optimize document embedding where appropriate

## 7. MongoDB Security Considerations

### 7.1 Document-Level Security
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
db.createCollection("suppliers");
db.createCollection("purchaseOrders");
db.createCollection("goodsReceipts");
db.createCollection("vehicleRegistrations");
db.createCollection("allotments");
db.createCollection("waitingLists");
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