# Vehicle Showroom Management System - MongoDB Version

## Overview

This is a comprehensive Vehicle Showroom Management System built using **Domain-Driven Design (DDD)**, **Clean Architecture**, and **CQRS** patterns with **MongoDB** as the database backend.

## Architecture

### Core Patterns
- **Domain-Driven Design (DDD)**: Focus on the core business domain
- **Clean Architecture**: Separation of concerns with layered architecture
- **CQRS**: Command Query Responsibility Segregation
- **Repository Pattern**: Data access abstraction
- **Unit of Work**: Transaction management

### Technology Stack
- **Backend**: ASP.NET Core 8.0 Web API
- **Database**: MongoDB with MongoDB Driver
- **Authentication**: JWT Bearer tokens
- **Authorization**: Role-based access control
- **Documentation**: Swagger/OpenAPI
- **Communication**: RESTful APIs with JSON

## Project Structure

```
VehicleShowroomManagement/
├── src/
│   ├── Domain/                    # Core business logic and entities
│   │   ├── Entities/             # Domain entities
│   │   ├── Interfaces/           # Domain interfaces
│   │   ├── Events/               # Domain events
│   │   ├── Services/             # Domain services
│   │   └── ValueObjects/         # Immutable value objects
│   ├── Application/              # Application layer with CQRS
│   │   ├── Commands/             # Write operations
│   │   ├── Queries/              # Read operations
│   │   ├── Handlers/             # Command/Query handlers
│   │   └── DTOs/                 # Data transfer objects
│   ├── Infrastructure/           # Infrastructure layer
│   │   ├── Persistence/          # MongoDB context and configuration
│   │   ├── Repositories/         # Repository implementations
│   │   └── Interfaces/           # Infrastructure interfaces
│   └── WebAPI/                   # API layer
│       ├── Controllers/          # API controllers
│       ├── Extensions/           # Service configuration
│       └── appsettings.json      # Configuration
└── tests/                        # Unit and integration tests
```

## MongoDB Document Structure

### Key Design Decisions

1. **Document-Oriented**: Each entity is stored as a MongoDB document
2. **Embedded Documents**: Related data is embedded for better read performance
3. **References**: Some relationships use ObjectId references for flexibility
4. **Indexing**: Strategic indexes for optimal query performance
5. **Soft Delete**: Documents are marked as deleted rather than removed

### Sample Document Structure

#### User Document
```json
{
  "_id": "ObjectId",
  "username": "johndoe",
  "email": "john@example.com",
  "passwordHash": "hashed_password",
  "firstName": "John",
  "lastName": "Doe",
  "roleId": "role_object_id",
  "role": {
    "roleId": "role_object_id",
    "roleName": "Dealer",
    "description": "Vehicle dealer role"
  },
  "isActive": true,
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false,
  "userRoleIds": ["role1", "role2"],
  "salesOrderIds": ["order1", "order2"]
}
```

#### Vehicle Document
```json
{
  "_id": "ObjectId",
  "vin": "1HGCM82633A123456",
  "modelId": "model_object_id",
  "color": "Blue",
  "year": 2024,
  "price": 35000.00,
  "mileage": 15000,
  "status": "Available",
  "model": {
    "modelId": "model_object_id",
    "modelName": "Civic",
    "brandId": "brand_object_id",
    "brand": {
      "brandId": "brand_object_id",
      "brandName": "Honda",
      "country": "Japan"
    },
    "engineType": "1.5L Turbo",
    "transmission": "CVT",
    "fuelType": "Gasoline"
  },
  "images": [
    {
      "imageId": "image_object_id",
      "imageUrl": "/images/vehicle1.jpg",
      "imageType": "Exterior",
      "fileName": "vehicle1.jpg"
    }
  ],
  "createdAt": "2024-01-01T00:00:00Z",
  "updatedAt": "2024-01-01T00:00:00Z",
  "isDeleted": false
}
```

## Features

### Role-Based Access Control
- **HR Role**: Employee management, user account creation
- **Dealer Role**: Sales management, inventory control
- **Admin Role**: Full system access

### Core Modules
- User management with authentication
- Vehicle inventory management
- Customer relationship management
- Sales order processing
- Purchase order management
- Service order coordination
- Invoice and payment processing
- Reporting and analytics

### MongoDB-Specific Features
- Flexible schema design
- Horizontal scaling capability
- Rich document queries
- Built-in indexing
- ACID transactions support

## Setup Instructions

### Prerequisites
- .NET 8.0 SDK
- MongoDB 5.0+ (local installation or cloud instance)
- IDE (Visual Studio 2022 or VS Code)

### MongoDB Setup
1. Install MongoDB locally or use MongoDB Atlas
2. Create a database named `VehicleShowroomDB`
3. Note the connection string for configuration

### Configuration
1. Update `appsettings.json`:
```json
{
  "MongoDB": {
    "ConnectionString": "mongodb://localhost:27017",
    "DatabaseName": "VehicleShowroomDB"
  },
  "Jwt": {
    "Key": "YourSuperSecretKeyHereThatIsAtLeast32CharactersLong",
    "Issuer": "VehicleShowroomAPI",
    "Audience": "VehicleShowroomClient",
    "ExpireHours": 24
  }
}
```

### Running the Application
1. Navigate to the WebAPI project directory
2. Run the following commands:
```bash
dotnet restore
dotnet build
dotnet run
```

3. The API will be available at `https://localhost:5001` or `http://localhost:5000`
4. Swagger documentation will be available at `https://localhost:5001/swagger`

### Initial Data Seeding
The application automatically seeds initial data including:
- Default roles (HR, Dealer, Admin)
- Default admin user (username: `admin`, password: `Admin123!`)

## API Endpoints

### Authentication
- `POST /api/auth/login` - User authentication
- `POST /api/auth/refresh` - Token refresh

### Users
- `GET /api/users` - Get all users (HR/Admin only)
- `GET /api/users/{id}` - Get user by ID
- `POST /api/users` - Create new user (HR/Admin only)
- `PUT /api/users/{id}` - Update user
- `DELETE /api/users/{id}` - Delete user (Admin only)

### Vehicles
- `GET /api/vehicles` - Get all vehicles with filtering
- `GET /api/vehicles/{id}` - Get vehicle by ID
- `POST /api/vehicles` - Create new vehicle
- `PUT /api/vehicles/{id}` - Update vehicle
- `DELETE /api/vehicles/{id}` - Delete vehicle

### Sales Orders
- `GET /api/salesorders` - Get all sales orders
- `GET /api/salesorders/{id}` - Get sales order by ID
- `POST /api/salesorders` - Create new sales order
- `PUT /api/salesorders/{id}` - Update sales order

### Reports
- `GET /api/reports/stock` - Stock availability report
- `GET /api/reports/sales` - Sales performance report
- `GET /api/reports/customers` - Customer analytics

## Development Guidelines

### Code Organization
- **Domain Layer**: Pure business logic, no external dependencies
- **Application Layer**: Use cases and orchestration
- **Infrastructure Layer**: External concerns (database, file system)
- **API Layer**: HTTP concerns and DTOs

### Adding New Features
1. Define entities in the Domain layer
2. Create commands/queries in Application layer
3. Implement handlers for business logic
4. Add repository methods if needed
5. Create API controllers for HTTP endpoints

### Database Operations
- Use repository pattern for data access
- Implement soft delete for data integrity
- Use transactions for multi-document operations
- Leverage MongoDB indexes for performance

## Testing

### Unit Tests
```bash
cd tests
dotnet test
```

### Integration Tests
- Database integration tests
- API endpoint tests
- Authentication/authorization tests

## Deployment

### Production Considerations
- Use MongoDB Atlas for cloud deployment
- Configure proper authentication secrets
- Set up logging and monitoring
- Implement rate limiting
- Configure SSL/TLS certificates

### Docker Support
Create Dockerfile for containerized deployment:
```dockerfile
FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /app
COPY . .
RUN dotnet publish -c Release -o out
WORKDIR /app/out
ENTRYPOINT ["dotnet", "VehicleShowroomManagement.WebAPI.dll"]
```

## Contributing

1. Follow the established architecture patterns
2. Write clean, maintainable code
3. Add appropriate unit tests
4. Update documentation as needed
5. Use meaningful commit messages

## License

This project is licensed under the MIT License.

## Support

For support and questions:
- Create an issue in the repository
- Check the documentation
- Review the API documentation at `/swagger`