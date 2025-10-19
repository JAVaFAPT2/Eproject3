using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Common.Models;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;
using MediatR;

namespace VehicleShowroomManagement.Tests.Integration
{
    public class ServiceOrderWorkflowIntegrationTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<ServiceOrder>> _mockServiceOrderRepository;
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly Mock<IRepository<VehicleModel>> _mockModelRepository;
        private readonly Mock<IMediator> _mockMediator;
        private readonly CreateOrderCommandHandler _createOrderHandler;
        private readonly CreateServiceOrderCommandHandler _createServiceOrderHandler;
        private readonly UpdateServiceOrderStatusCommandHandler _updateServiceOrderHandler;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;

        public ServiceOrderWorkflowIntegrationTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockServiceOrderRepository = new Mock<IRepository<ServiceOrder>>();
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
            _mockModelRepository = new Mock<IRepository<VehicleModel>>();
            _mockMediator = new Mock<IMediator>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

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
                _mockMediator.Object,
                _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task CompleteWorkflow_CreateOrderCreateServiceOrderCompleteServiceOrder_CreatesBillingDocument()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);
            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Service Order
            var createServiceOrderCommand = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance");
            var serviceOrderId = await _createServiceOrderHandler.Handle(createServiceOrderCommand, CancellationToken.None);

            // Act - Step 3: Complete Service Order
            var updateStatusCommand = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);
            var result = await _updateServiceOrderHandler.Handle(updateStatusCommand, CancellationToken.None);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            serviceOrderId.Should().NotBeNullOrEmpty();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order completed and billing document created");
            result.BillingDocumentId.Should().Be("billing-doc-id");

            // Verify all repositories were called
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockMediator.Verify(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CompletePreDeliveryWorkflow_CreatesOrderServiceOrderCompletesOrderAndVehicle()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Use domain method to assign vehicle
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.PreDelivery, 300m);
            var vehicle = new Vehicle("vehicle1", "model1", 20000m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);
            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockVehicleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Vehicle> { vehicle });
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create PreDelivery Service Order
            var createServiceOrderCommand = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.PreDelivery, 300m, DateTime.Now.AddDays(1), "Pre-delivery inspection");
            var serviceOrderId = await _createServiceOrderHandler.Handle(createServiceOrderCommand, CancellationToken.None);

            // Act - Step 3: Complete PreDelivery Service Order
            var updateStatusCommand = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);
            var result = await _updateServiceOrderHandler.Handle(updateStatusCommand, CancellationToken.None);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            serviceOrderId.Should().NotBeNullOrEmpty();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order completed, order completed, vehicle marked as sold, and billing document created");
            result.BillingDocumentId.Should().Be("billing-doc-id");

            // Verify all repositories were called
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockOrderRepository.Verify(r => r.UpdateAsync(It.Is<Order>(o => o.Status == OrderStatus.Completed), It.IsAny<CancellationToken>()), Times.Once);
            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Once);
            _mockVehicleRepository.Verify(r => r.UpdateAsync(It.Is<Vehicle>(v => v.Status == VehicleStatus.Sold), It.IsAny<CancellationToken>()), Times.Once);
            _mockMediator.Verify(m => m.Send(It.Is<CreateBillingDocumentCommand>(cmd => cmd.Amount == 25300m), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task CancelServiceOrderWorkflow_DoesNotCreateBillingDocument()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);
            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Service Order
            var createServiceOrderCommand = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance");
            var serviceOrderId = await _createServiceOrderHandler.Handle(createServiceOrderCommand, CancellationToken.None);

            // Act - Step 3: Cancel Service Order
            var updateStatusCommand = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Cancelled);
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
        public async Task MultipleServiceOrdersWorkflow_CreatesMultipleBillingDocuments()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);
            var maintenanceService = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);
            var repairService = new ServiceOrder("order1", "customer1", "user1", ServiceType.Repair, 750m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);
            _mockServiceOrderRepository.SetupSequence(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(maintenanceService)
                              .ReturnsAsync(repairService);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Maintenance Service Order
            var createMaintenanceCommand = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance");
            var maintenanceId = await _createServiceOrderHandler.Handle(createMaintenanceCommand, CancellationToken.None);

            // Act - Step 3: Create Repair Service Order
            var createRepairCommand = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Repair, 750m, DateTime.Now.AddDays(3), "Engine repair");
            var repairId = await _createServiceOrderHandler.Handle(createRepairCommand, CancellationToken.None);

            // Act - Step 4: Complete Maintenance Service Order
            var updateMaintenanceCommand = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);
            var maintenanceResult = await _updateServiceOrderHandler.Handle(updateMaintenanceCommand, CancellationToken.None);

            // Act - Step 5: Complete Repair Service Order
            var updateRepairCommand = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);
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
        public async Task WorkflowWithLicensePlate_UpdatesVehicleLicensePlate()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Use domain method to assign vehicle
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);
            var vehicle = new Vehicle("vehicle1", "model1", 20000m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);
            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockVehicleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Vehicle> { vehicle });
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            // Act - Step 1: Create Order
            var createOrderCommand = new CreateOrderCommand("customer1", "model1", 25000m);
            var orderId = await _createOrderHandler.Handle(createOrderCommand, CancellationToken.None);

            // Act - Step 2: Create Service Order
            var createServiceOrderCommand = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance");
            var serviceOrderId = await _createServiceOrderHandler.Handle(createServiceOrderCommand, CancellationToken.None);

            // Act - Step 3: Complete Service Order with License Plate
            var updateStatusCommand = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);
            var result = await _updateServiceOrderHandler.Handle(updateStatusCommand, CancellationToken.None);

            // Assert
            orderId.Should().NotBeNullOrEmpty();
            serviceOrderId.Should().NotBeNullOrEmpty();
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();

            // License plate is updated via separate endpoint, not via status update
            _mockVehicleRepository.Verify(r => r.UpdateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}