using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;
using MediatR;

namespace VehicleShowroomManagement.Tests.Integration
{
    public class ComplexWorkflowIntegrationTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<ServiceOrder>> _mockServiceOrderRepository;
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly Mock<IRepository<VehicleModel>> _mockModelRepository;
        private readonly Mock<IRepository<BillingDocument>> _mockBillingDocumentRepository;
        private readonly Mock<IMediator> _mockMediator;
        private readonly CreateOrderCommandHandler _createOrderHandler;
        private readonly CreateServiceOrderCommandHandler _createServiceOrderHandler;
        private readonly UpdateServiceOrderStatusCommandHandler _updateServiceOrderHandler;
        private readonly CreateVehicleCommandHandler _createVehicleHandler;

        public ComplexWorkflowIntegrationTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockServiceOrderRepository = new Mock<IRepository<ServiceOrder>>();
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
            _mockModelRepository = new Mock<IRepository<VehicleModel>>();
            _mockBillingDocumentRepository = new Mock<IRepository<BillingDocument>>();
            _mockMediator = new Mock<IMediator>();

            _createOrderHandler = new CreateOrderCommandHandler(
                _mockOrderRepository.Object,
                _mockModelRepository.Object);

            _createServiceOrderHandler = new CreateServiceOrderCommandHandler(
                _mockServiceOrderRepository.Object,
                _mockOrderRepository.Object);

            _updateServiceOrderHandler = new UpdateServiceOrderStatusCommandHandler(
                _mockServiceOrderRepository.Object,
                _mockOrderRepository.Object,
                _mockVehicleRepository.Object,
                _mockMediator.Object);

            _createVehicleHandler = new CreateVehicleCommandHandler(
                _mockVehicleRepository.Object);
        }

        [Fact]
        public async Task CompleteVehicleSaleWorkflow_CreatesOrderVehicleServiceOrderAndBillingDocument()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Assign vehicle to make it confirmed
            var vehicle = new Vehicle("vehicle1", "model1", 20000m);
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.PreDelivery, 300m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);
            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockVehicleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Vehicle> { vehicle });
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Vehicle
            var createVehicleCommand = new CreateVehicleCommand("vehicle1", "model1", 20000m, "VIN123456789", "EXT001");
            var vehicleId = await _createVehicleHandler.Handle(createVehicleCommand, CancellationToken.None);

            // Act - Step 3: Create PreDelivery Service Order
            var createServiceOrderCommand = new CreateServiceOrderCommand(orderId, "customer1", "user1", ServiceType.PreDelivery, 300m, DateTime.Now.AddDays(1), "Pre-delivery inspection");
            var serviceOrderId = await _createServiceOrderHandler.Handle(createServiceOrderCommand, CancellationToken.None);

            // Act - Step 4: Complete Service Order
            var updateStatusCommand = new UpdateServiceOrderStatusCommand(serviceOrderId, ServiceOrderStatus.Completed);
            var result = await _updateServiceOrderHandler.Handle(updateStatusCommand, CancellationToken.None);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            vehicleId.Should().NotBeNullOrEmpty();
            serviceOrderId.Should().NotBeNullOrEmpty();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.BillingDocumentId.Should().Be("billing-doc-id");

            // Verify all repositories were called
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockVehicleRepository.Verify(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockOrderRepository.Verify(r => r.UpdateAsync(It.Is<Order>(o => o.Status == OrderStatus.Completed), It.IsAny<CancellationToken>()), Times.Once);
            _mockVehicleRepository.Verify(r => r.UpdateAsync(It.Is<Vehicle>(v => v.Status == VehicleStatus.Sold), It.IsAny<CancellationToken>()), Times.Once);
            _mockMediator.Verify(m => m.Send(It.Is<CreateBillingDocumentCommand>(cmd => cmd.Amount == 25300m), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task MultipleServiceOrdersWorkflow_CreatesMultipleBillingDocuments()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1");

            var maintenanceService = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);
            var repairService = new ServiceOrder("order1", "customer1", "user1", ServiceType.Repair, 750m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);
            _mockServiceOrderRepository.SetupSequence(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(maintenanceService)
                              .ReturnsAsync(repairService);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Maintenance Service Order
            var createMaintenanceCommand = new CreateServiceOrderCommand(orderId, "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance");
            var maintenanceId = await _createServiceOrderHandler.Handle(createMaintenanceCommand, CancellationToken.None);

            // Act - Step 3: Create Repair Service Order
            var createRepairCommand = new CreateServiceOrderCommand(orderId, "customer1", "user1", ServiceType.Repair, 750m, DateTime.Now.AddDays(3), "Engine repair");
            var repairId = await _createServiceOrderHandler.Handle(createRepairCommand, CancellationToken.None);

            // Act - Step 4: Complete Maintenance Service Order
            var updateMaintenanceCommand = new UpdateServiceOrderStatusCommand(maintenanceId, ServiceOrderStatus.Completed);
            var maintenanceResult = await _updateServiceOrderHandler.Handle(updateMaintenanceCommand, CancellationToken.None);

            // Act - Step 5: Complete Repair Service Order
            var updateRepairCommand = new UpdateServiceOrderStatusCommand(repairId, ServiceOrderStatus.Completed);
            var repairResult = await _updateServiceOrderHandler.Handle(updateRepairCommand, CancellationToken.None);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            maintenanceId.Should().NotBeNullOrEmpty();
            repairId.Should().NotBeNullOrEmpty();
            maintenanceResult.Success.Should().BeTrue();
            repairResult.Success.Should().BeTrue();

            // Verify two billing documents were created
            _mockMediator.Verify(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task ServiceOrderCancellationWorkflow_DoesNotCreateBillingDocument()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);
            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Service Order
            var createServiceOrderCommand = new CreateServiceOrderCommand(orderId, "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance");
            var serviceOrderId = await _createServiceOrderHandler.Handle(createServiceOrderCommand, CancellationToken.None);

            // Act - Step 3: Cancel Service Order
            var updateStatusCommand = new UpdateServiceOrderStatusCommand(serviceOrderId, ServiceOrderStatus.Cancelled);
            var result = await _updateServiceOrderHandler.Handle(updateStatusCommand, CancellationToken.None);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            serviceOrderId.Should().NotBeNullOrEmpty();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order cancelled");
            result.BillingDocumentId.Should().BeNull();

            // Verify billing document was NOT created
            _mockMediator.Verify(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task ConcurrentOrderCreationWorkflow_CreatesMultipleOrdersSuccessfully()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            // Act - Create multiple orders concurrently
            var tasks = new[]
            {
                _createOrderHandler.Handle(new CreateOrderCommand("customer1", "model1", 25000m), CancellationToken.None),
                _createOrderHandler.Handle(new CreateOrderCommand("customer2", "model1", 30000m), CancellationToken.None),
                _createOrderHandler.Handle(new CreateOrderCommand("customer3", "model1", 20000m), CancellationToken.None)
            };
            var results = await Task.WhenAll(tasks);

            // Assert
            results.Should().HaveCount(3);
            results.Should().AllSatisfy(r => r.Should().NotBeNullOrEmpty());
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task OrderWithMultipleVehiclesWorkflow_CreatesOrderAndMultipleVehicles()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Multiple Vehicles
            var vehicleTasks = new[]
            {
                _createVehicleHandler.Handle(new CreateVehicleCommand("vehicle1", "model1", 20000m, "VIN123456789", "EXT001"), CancellationToken.None),
                _createVehicleHandler.Handle(new CreateVehicleCommand("vehicle2", "model1", 21000m, "VIN987654321", "EXT002"), CancellationToken.None),
                _createVehicleHandler.Handle(new CreateVehicleCommand("vehicle3", "model1", 19000m, "VIN456789123", "EXT003"), CancellationToken.None)
            };
            var vehicleIds = await Task.WhenAll(vehicleTasks);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            vehicleIds.Should().HaveCount(3);
            vehicleIds.Should().AllSatisfy(id => id.Should().NotBeNullOrEmpty());
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockVehicleRepository.Verify(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task ServiceOrderWithDifferentTypesWorkflow_CreatesCorrectServiceOrders()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Different Types of Service Orders
            var serviceOrderTasks = new[]
            {
                _createServiceOrderHandler.Handle(new CreateServiceOrderCommand(orderId, "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance"), CancellationToken.None),
                _createServiceOrderHandler.Handle(new CreateServiceOrderCommand(orderId, "customer1", "user1", ServiceType.Repair, 750m, DateTime.Now.AddDays(3), "Engine repair"), CancellationToken.None),
                _createServiceOrderHandler.Handle(new CreateServiceOrderCommand(orderId, "customer1", "user1", ServiceType.PreDelivery, 300m, DateTime.Now.AddDays(1), "Pre-delivery inspection"), CancellationToken.None)
            };
            var serviceOrderIds = await Task.WhenAll(serviceOrderTasks);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            serviceOrderIds.Should().HaveCount(3);
            serviceOrderIds.Should().AllSatisfy(id => id.Should().NotBeNullOrEmpty());
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task ErrorHandlingWorkflow_HandlesRepositoryErrorsGracefully()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database connection failed"));

            // Act & Assert
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            await Assert.ThrowsAsync<Exception>(() => _createOrderHandler.Handle(createOrderCommand, CancellationToken.None));
        }

        [Fact]
        public async Task CancellationTokenPropagationWorkflow_PropagatesCancellationCorrectly()
        {
            // Arrange
            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), cancellationToken))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), cancellationToken))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            // Act
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, cancellationToken);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), cancellationToken), Times.Once);
        }
    }
}
