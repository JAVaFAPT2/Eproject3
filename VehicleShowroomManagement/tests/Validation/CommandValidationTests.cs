using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle;
using VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Tests.Validation
{
    public class CommandValidationTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<ServiceOrder>> _mockServiceOrderRepository;
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly Mock<IRepository<VehicleModel>> _mockModelRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly Mock<IRepository<BillingDocument>> _mockBillingDocumentRepository;
        private readonly Mock<IPasswordService> _mockPasswordService;

        private readonly CreateOrderCommandHandler _createOrderHandler;
        private readonly CreateServiceOrderCommandHandler _createServiceOrderHandler;
        private readonly CreateVehicleCommandHandler _createVehicleHandler;
        private readonly CreateUserCommandHandler _createUserHandler;
        private readonly CreateBillingDocumentCommandHandler _createBillingDocumentHandler;

        public CommandValidationTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockServiceOrderRepository = new Mock<IRepository<ServiceOrder>>();
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
            _mockModelRepository = new Mock<IRepository<VehicleModel>>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _mockBillingDocumentRepository = new Mock<IRepository<BillingDocument>>();
            _mockPasswordService = new Mock<IPasswordService>();

            _createOrderHandler = new CreateOrderCommandHandler(
                _mockOrderRepository.Object,
                _mockModelRepository.Object);

            _createServiceOrderHandler = new CreateServiceOrderCommandHandler(
                _mockServiceOrderRepository.Object,
                _mockOrderRepository.Object);

            _createVehicleHandler = new CreateVehicleCommandHandler(
                _mockVehicleRepository.Object);

            _createUserHandler = new CreateUserCommandHandler(
                _mockUserRepository.Object,
                _mockRoleRepository.Object,
                _mockPasswordService.Object);

            _createBillingDocumentHandler = new CreateBillingDocumentCommandHandler(
                _mockBillingDocumentRepository.Object,
                _mockOrderRepository.Object);
        }

        #region CreateOrderCommand Validation Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateOrder_WithInvalidCustomerId_ThrowsArgumentException(string customerId)
        {
            // Arrange
            var command = new CreateOrderCommand(customerId, "model1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createOrderHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateOrder_WithInvalidModelNumber_ThrowsArgumentException(string modelNumber)
        {
            // Arrange
            var command = new CreateOrderCommand("customer1", modelNumber, 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createOrderHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        [InlineData(-0.01)]
        public async Task CreateOrder_WithNegativeSalePrice_ThrowsArgumentException(decimal salePrice)
        {
            // Arrange
            var command = new CreateOrderCommand("customer1", "model1", salePrice);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createOrderHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        [InlineData(25000)]
        [InlineData(1000000)]
        public async Task CreateOrder_WithValidSalePrice_CreatesOrderSuccessfully(decimal salePrice)
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", salePrice, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            var command = new CreateOrderCommand("customer1", "model1", salePrice);

            // Act
            var result = await _createOrderHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region CreateVehicleCommand Validation Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateVehicle_WithInvalidVehicleId_ThrowsArgumentException(string vehicleId)
        {
            // Arrange
            var command = new CreateVehicleCommand(vehicleId, "model1", 20000m, "VIN123456789", "EXT001");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createVehicleHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateVehicle_WithInvalidModelNumber_ThrowsArgumentException(string modelNumber)
        {
            // Arrange
            var command = new CreateVehicleCommand("vehicle1", modelNumber, 20000m, "VIN123456789", "EXT001");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createVehicleHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        [InlineData(-0.01)]
        public async Task CreateVehicle_WithNegativePurchasePrice_ThrowsArgumentException(decimal purchasePrice)
        {
            // Arrange
            var command = new CreateVehicleCommand("vehicle1", "model1", purchasePrice, "VIN123456789", "EXT001");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createVehicleHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateVehicle_WithInvalidVin_ThrowsArgumentException(string vin)
        {
            // Arrange
            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, vin, "EXT001");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createVehicleHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateVehicle_WithInvalidExternalNumber_ThrowsArgumentException(string externalNumber)
        {
            // Arrange
            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, "VIN123456789", externalNumber);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createVehicleHandler.Handle(command, CancellationToken.None));
        }

        #endregion

        #region CreateServiceOrderCommand Validation Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateServiceOrder_WithInvalidOrderId_ThrowsArgumentException(string orderId)
        {
            // Arrange
            var command = new CreateServiceOrderCommand(orderId, "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Service");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createServiceOrderHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateServiceOrder_WithInvalidCustomerId_ThrowsArgumentException(string customerId)
        {
            // Arrange
            var command = new CreateServiceOrderCommand("order1", customerId, "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Service");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createServiceOrderHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateServiceOrder_WithInvalidCreatedBy_ThrowsArgumentException(string createdBy)
        {
            // Arrange
            var command = new CreateServiceOrderCommand("order1", "customer1", createdBy, ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Service");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createServiceOrderHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        [InlineData(-0.01)]
        public async Task CreateServiceOrder_WithNegativeCost_ThrowsArgumentException(decimal cost)
        {
            // Arrange
            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, cost, DateTime.Now.AddDays(7), "Service");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createServiceOrderHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        [InlineData(500)]
        [InlineData(10000)]
        public async Task CreateServiceOrder_WithValidCost_CreatesServiceOrderSuccessfully(decimal cost)
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, cost, DateTime.Now.AddDays(7), "Service");

            // Act
            var result = await _createServiceOrderHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region CreateUserCommand Validation Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateUser_WithInvalidUsername_ThrowsArgumentException(string username)
        {
            // Arrange
            var customerRole = new Role("Customer");
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Role> { customerRole });

            var command = new CreateUserCommand(username, "user@example.com", "password123", "User Name");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createUserHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateUser_WithInvalidEmail_ThrowsArgumentException(string email)
        {
            // Arrange
            var customerRole = new Role("Customer");
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Role> { customerRole });

            var command = new CreateUserCommand("username", email, "password123", "User Name");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createUserHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateUser_WithInvalidPassword_ThrowsArgumentException(string password)
        {
            // Arrange
            var customerRole = new Role("Customer");
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Role> { customerRole });

            var command = new CreateUserCommand("username", "user@example.com", password, "User Name");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createUserHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateUser_WithInvalidName_ThrowsArgumentException(string name)
        {
            // Arrange
            var customerRole = new Role("Customer");
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Role> { customerRole });

            var command = new CreateUserCommand("username", "user@example.com", "password123", name);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createUserHandler.Handle(command, CancellationToken.None));
        }

        #endregion

        #region CreateBillingDocumentCommand Validation Tests

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateBillingDocument_WithInvalidOrderId_ThrowsArgumentException(string orderId)
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);

            var command = new CreateBillingDocumentCommand(orderId, "user1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createBillingDocumentHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task CreateBillingDocument_WithInvalidCreatedBy_ThrowsArgumentException(string createdBy)
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);

            var command = new CreateBillingDocumentCommand("order1", createdBy, 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createBillingDocumentHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-1000)]
        [InlineData(-0.01)]
        public async Task CreateBillingDocument_WithNegativeAmount_ThrowsArgumentException(decimal amount)
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);

            var command = new CreateBillingDocumentCommand("order1", "user1", amount);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createBillingDocumentHandler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(0.01)]
        [InlineData(25000)]
        [InlineData(1000000)]
        public async Task CreateBillingDocument_WithValidAmount_CreatesBillingDocumentSuccessfully(decimal amount)
        {
            // Arrange
            var order = new Order("customer1", "model1", amount);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", amount);

            // Act
            var result = await _createBillingDocumentHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Edge Case Validation Tests

        [Fact]
        public async Task CreateOrder_WithVeryLongCustomerId_CreatesOrderSuccessfully()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var longCustomerId = "customer-" + new string('x', 1000);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            var command = new CreateOrderCommand(longCustomerId, "model1", 25000m);

            // Act
            var result = await _createOrderHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateVehicle_WithSpecialCharactersInVin_CreatesVehicleSuccessfully()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var specialVin = "VIN-123@456#789$Special";

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, specialVin, "EXT001");

            // Act
            var result = await _createVehicleHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateUser_WithSpecialCharactersInEmail_CreatesUserSuccessfully()
        {
            // Arrange
            var customerRole = new Role("Customer");
            var specialEmail = "user+test@domain-name.co.uk";

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Role> { customerRole });
            _mockPasswordService.Setup(s => s.HashPassword(It.IsAny<string>()))
                              .Returns("hashedpassword");
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((User user, CancellationToken ct) => user);

            var command = new CreateUserCommand("username", specialEmail, "password123", "User Name");

            // Act
            var result = await _createUserHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateBillingDocument_WithFutureAppointmentDate_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var futureDate = DateTime.Now.AddDays(30);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", 25000m, futureDate);

            // Act
            var result = await _createBillingDocumentHandler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task CreateBillingDocument_WithPastAppointmentDate_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var pastDate = DateTime.Now.AddDays(-1);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);

            var command = new CreateBillingDocumentCommand("order1", "user1", 25000m, pastDate);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _createBillingDocumentHandler.Handle(command, CancellationToken.None));
        }

        #endregion
    }
}
