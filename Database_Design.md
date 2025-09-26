# Vehicle Showroom Management System - Database Design

## 1. Database Overview

### 1.1 Database Engine
- **Database**: SQL Server 2022
- **Design Pattern**: Code-First with Entity Framework Core
- **Approach**: Domain-Driven Design with normalized schema

### 1.2 Design Principles
- Normalization up to 3rd Normal Form (3NF)
- Primary keys and foreign key constraints
- Indexes for optimal query performance
- Audit fields for data tracking

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

## 3. Database Schema

### 3.1 Tables and Columns

#### Users Table
```sql
CREATE TABLE Users (
    UserId int IDENTITY(1,1) PRIMARY KEY,
    Username nvarchar(50) NOT NULL UNIQUE,
    Email nvarchar(100) NOT NULL UNIQUE,
    PasswordHash nvarchar(255) NOT NULL,
    FirstName nvarchar(50) NOT NULL,
    LastName nvarchar(50) NOT NULL,
    RoleId int NOT NULL,
    IsActive bit NOT NULL DEFAULT 1,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId)
);
```

#### Roles Table
```sql
CREATE TABLE Roles (
    RoleId int IDENTITY(1,1) PRIMARY KEY,
    RoleName nvarchar(50) NOT NULL UNIQUE,
    Description nvarchar(255),
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
);
```

#### UserRoles Table (Many-to-Many between Users and Roles)
```sql
CREATE TABLE UserRoles (
    UserId int NOT NULL,
    RoleId int NOT NULL,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    PRIMARY KEY (UserId, RoleId),
    FOREIGN KEY (UserId) REFERENCES Users(UserId) ON DELETE CASCADE,
    FOREIGN KEY (RoleId) REFERENCES Roles(RoleId) ON DELETE CASCADE
);
```

#### Brands Table
```sql
CREATE TABLE Brands (
    BrandId int IDENTITY(1,1) PRIMARY KEY,
    BrandName nvarchar(100) NOT NULL UNIQUE,
    Country nvarchar(50),
    LogoUrl nvarchar(255),
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
);
```

#### Models Table
```sql
CREATE TABLE Models (
    ModelId int IDENTITY(1,1) PRIMARY KEY,
    ModelName nvarchar(100) NOT NULL,
    BrandId int NOT NULL,
    EngineType nvarchar(50),
    Transmission nvarchar(50),
    FuelType nvarchar(30),
    SeatingCapacity int,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (BrandId) REFERENCES Brands(BrandId),
    UNIQUE (ModelName, BrandId)
);
```

#### Vehicles Table
```sql
CREATE TABLE Vehicles (
    VehicleId int IDENTITY(1,1) PRIMARY KEY,
    VIN nvarchar(17) NOT NULL UNIQUE,
    ModelId int NOT NULL,
    Color nvarchar(50) NOT NULL,
    Year int NOT NULL,
    Price decimal(12,2) NOT NULL,
    Mileage int DEFAULT 0,
    Status nvarchar(20) NOT NULL DEFAULT 'Available',
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (ModelId) REFERENCES Models(ModelId)
);
```

#### Customers Table
```sql
CREATE TABLE Customers (
    CustomerId int IDENTITY(1,1) PRIMARY KEY,
    FirstName nvarchar(50) NOT NULL,
    LastName nvarchar(50) NOT NULL,
    Email nvarchar(100) NOT NULL UNIQUE,
    Phone nvarchar(20),
    Address nvarchar(255),
    City nvarchar(50),
    State nvarchar(50),
    ZipCode nvarchar(10),
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
);
```

#### Suppliers Table
```sql
CREATE TABLE Suppliers (
    SupplierId int IDENTITY(1,1) PRIMARY KEY,
    CompanyName nvarchar(100) NOT NULL,
    ContactPerson nvarchar(100),
    Email nvarchar(100),
    Phone nvarchar(20),
    Address nvarchar(255),
    City nvarchar(50),
    State nvarchar(50),
    ZipCode nvarchar(10),
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE()
);
```

#### PurchaseOrders Table
```sql
CREATE TABLE PurchaseOrders (
    PurchaseId int IDENTITY(1,1) PRIMARY KEY,
    SupplierId int NOT NULL,
    Status nvarchar(20) NOT NULL DEFAULT 'Pending',
    OrderDate datetime2 NOT NULL DEFAULT GETUTCDATE(),
    DeliveryDate datetime2,
    TotalAmount decimal(12,2) NOT NULL,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (SupplierId) REFERENCES Suppliers(SupplierId)
);
```

#### PurchaseOrderItems Table
```sql
CREATE TABLE PurchaseOrderItems (
    PurchaseOrderItemId int IDENTITY(1,1) PRIMARY KEY,
    PurchaseId int NOT NULL,
    VehicleId int NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    UnitPrice decimal(12,2) NOT NULL,
    TotalAmount decimal(12,2) NOT NULL,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (PurchaseId) REFERENCES PurchaseOrders(PurchaseId) ON DELETE CASCADE,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId)
);
```

#### SalesOrders Table
```sql
CREATE TABLE SalesOrders (
    SalesOrderId int IDENTITY(1,1) PRIMARY KEY,
    CustomerId int NOT NULL,
    SalesPersonId int NOT NULL,
    Status nvarchar(20) NOT NULL DEFAULT 'Draft',
    OrderDate datetime2 NOT NULL DEFAULT GETUTCDATE(),
    TotalAmount decimal(12,2) NOT NULL,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId),
    FOREIGN KEY (SalesPersonId) REFERENCES Users(UserId)
);
```

#### SalesOrderItems Table
```sql
CREATE TABLE SalesOrderItems (
    SalesOrderItemId int IDENTITY(1,1) PRIMARY KEY,
    SalesOrderId int NOT NULL,
    VehicleId int NOT NULL,
    Quantity int NOT NULL DEFAULT 1,
    UnitPrice decimal(12,2) NOT NULL,
    Discount decimal(12,2) DEFAULT 0,
    LineTotal decimal(12,2) NOT NULL,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (SalesOrderId) REFERENCES SalesOrders(SalesOrderId) ON DELETE CASCADE,
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId)
);
```

#### Invoices Table
```sql
CREATE TABLE Invoices (
    InvoiceId int IDENTITY(1,1) PRIMARY KEY,
    SalesOrderId int NOT NULL,
    InvoiceNumber nvarchar(50) NOT NULL UNIQUE,
    InvoiceDate datetime2 NOT NULL DEFAULT GETUTCDATE(),
    Subtotal decimal(12,2) NOT NULL,
    TaxAmount decimal(12,2) NOT NULL,
    TotalAmount decimal(12,2) NOT NULL,
    Status nvarchar(20) NOT NULL DEFAULT 'Unpaid',
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (SalesOrderId) REFERENCES SalesOrders(SalesOrderId)
);
```

#### Payments Table
```sql
CREATE TABLE Payments (
    PaymentId int IDENTITY(1,1) PRIMARY KEY,
    InvoiceId int NOT NULL,
    Amount decimal(12,2) NOT NULL,
    PaymentMethod nvarchar(50) NOT NULL,
    PaymentDate datetime2 NOT NULL DEFAULT GETUTCDATE(),
    ReferenceNumber nvarchar(100),
    Status nvarchar(20) NOT NULL DEFAULT 'Completed',
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (InvoiceId) REFERENCES Invoices(InvoiceId)
);
```

#### ServiceOrders Table
```sql
CREATE TABLE ServiceOrders (
    ServiceOrderId int IDENTITY(1,1) PRIMARY KEY,
    VehicleId int NOT NULL,
    CustomerId int NOT NULL,
    ServiceDate datetime2 NOT NULL,
    Status nvarchar(20) NOT NULL DEFAULT 'Scheduled',
    TotalCost decimal(12,2) DEFAULT 0,
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId),
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);
```

#### ServiceOrderItems Table
```sql
CREATE TABLE ServiceOrderItems (
    ServiceOrderItemId int IDENTITY(1,1) PRIMARY KEY,
    ServiceOrderId int NOT NULL,
    ServiceType nvarchar(100) NOT NULL,
    Description nvarchar(255),
    Cost decimal(12,2) NOT NULL,
    Status nvarchar(20) NOT NULL DEFAULT 'Pending',
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (ServiceOrderId) REFERENCES ServiceOrders(ServiceOrderId) ON DELETE CASCADE
);
```

#### VehicleImages Table
```sql
CREATE TABLE VehicleImages (
    ImageId int IDENTITY(1,1) PRIMARY KEY,
    VehicleId int NOT NULL,
    ImageUrl nvarchar(255) NOT NULL,
    ImageType nvarchar(20) NOT NULL,
    FileName nvarchar(100) NOT NULL,
    FileSize int NOT NULL,
    UploadedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    CreatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    UpdatedAt datetime2 NOT NULL DEFAULT GETUTCDATE(),
    FOREIGN KEY (VehicleId) REFERENCES Vehicles(VehicleId) ON DELETE CASCADE
);
```

## 4. Indexes and Performance Optimization

### 4.1 Clustered Indexes
- Primary keys on all tables (automatically created)

### 4.2 Non-Clustered Indexes
```sql
-- Users table
CREATE INDEX IX_Users_Email ON Users(Email);
CREATE INDEX IX_Users_Username ON Users(Username);
CREATE INDEX IX_Users_RoleId ON Users(RoleId);

-- Vehicles table
CREATE INDEX IX_Vehicles_Status ON Vehicles(Status);
CREATE INDEX IX_Vehicles_ModelId ON Vehicles(ModelId);
CREATE INDEX IX_Vehicles_VIN ON Vehicles(VIN);

-- Customers table
CREATE INDEX IX_Customers_Email ON Customers(Email);
CREATE INDEX IX_Customers_Phone ON Customers(Phone);

-- SalesOrders table
CREATE INDEX IX_SalesOrders_CustomerId ON SalesOrders(CustomerId);
CREATE INDEX IX_SalesOrders_Status ON SalesOrders(Status);
CREATE INDEX IX_SalesOrders_OrderDate ON SalesOrders(OrderDate);

-- PurchaseOrders table
CREATE INDEX IX_PurchaseOrders_SupplierId ON PurchaseOrders(SupplierId);
CREATE INDEX IX_PurchaseOrders_Status ON PurchaseOrders(Status);
CREATE INDEX IX_PurchaseOrders_OrderDate ON PurchaseOrders(OrderDate);
```

## 5. Domain-Driven Design Mapping

### 5.1 Aggregates
- **Vehicle Aggregate**: Vehicle, VehicleImages
- **Sales Aggregate**: SalesOrder, SalesOrderItems, Invoice, Payments
- **Purchase Aggregate**: PurchaseOrder, PurchaseOrderItems
- **Service Aggregate**: ServiceOrder, ServiceOrderItems
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

## 6. Security Considerations

### 6.1 Row-Level Security
```sql
-- Users can only see their own data unless they have admin role
CREATE FUNCTION dbo.fn_UserAccess(@UserId int)
RETURNS TABLE
AS
RETURN
(
    SELECT 1 as Access
    WHERE EXISTS (
        SELECT 1 FROM Users u
        INNER JOIN UserRoles ur ON u.UserId = ur.UserId
        INNER JOIN Roles r ON ur.RoleId = r.RoleId
        WHERE u.UserId = @UserId AND r.RoleName IN ('Admin', 'Manager')
    )
);
```

### 6.2 Audit Triggers
```sql
CREATE TRIGGER tr_Users_Audit ON Users
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    INSERT INTO AuditLog (TableName, Operation, UserId, OldValues, NewValues, Timestamp)
    SELECT
        'Users',
        CASE WHEN EXISTS(SELECT * FROM inserted) AND EXISTS(SELECT * FROM deleted) THEN 'UPDATE'
             WHEN EXISTS(SELECT * FROM inserted) THEN 'INSERT'
             ELSE 'DELETE' END,
        SYSTEM_USER,
        (SELECT * FROM deleted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        (SELECT * FROM inserted FOR JSON PATH, WITHOUT_ARRAY_WRAPPER),
        GETUTCDATE()
    FROM inserted FULL OUTER JOIN deleted ON 1=0;
END;
```

## 7. Migration Strategy

### 7.1 Entity Framework Core Migrations
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### 7.2 Seed Data
- Default roles (HR, Dealer, Customer)
- Default admin user
- Sample brands and models
- System configuration

---

**Document Version**: 1.0
**Last Updated**: $(date)
**Prepared by**: Development Team