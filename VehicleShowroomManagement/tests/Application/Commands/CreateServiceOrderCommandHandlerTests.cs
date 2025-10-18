using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class CreateServiceOrderCommandHandlerTests
    {
        private readonly Mock<IRepository<ServiceOrder>> _mockServiceOrderRepository;
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly CreateServiceOrderCommandHandler _handler;

        public CreateServiceOrderCommandHandlerTests()
        {
            _mockServiceOrderRepository = new Mock<IRepository<ServiceOrder>>();
            _mockOrderRepository = new Mock<IRepository<Order>>();

            _handler = new CreateServiceOrderCommandHandler(
                _mockServiceOrderRepository.Object,
                _mockOrderRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidMaintenanceOrder_CreatesServiceOrderAndReturnsId()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.Is<ServiceOrder>(so => 
                so.OrderId == "order1" &&
                so.CustomerId == "customer1" &&
                so.CreatedBy == "user1" &&
                so.Type == ServiceType.Maintenance &&
                so.Cost == 500m &&
                so.Description == "Regular maintenance" &&
                so.Status == ServiceOrderStatus.Scheduled
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidRepairOrder_CreatesServiceOrderAndReturnsId()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Repair, 750m, DateTime.Now.AddDays(3), "Engine repair");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.Is<ServiceOrder>(so => 
                so.Type == ServiceType.Repair &&
                so.Cost == 750m &&
                so.Description == "Engine repair"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidPreDeliveryOrder_CreatesServiceOrderAndReturnsId()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.PreDelivery, 300m, DateTime.Now.AddDays(1), "Pre-delivery inspection");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.Is<ServiceOrder>(so => 
                so.Type == ServiceType.PreDelivery &&
                so.Cost == 300m &&
                so.Description == "Pre-delivery inspection"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentOrder_ThrowsException()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetByIdAsync("nonexistent", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order?)null);

            var command = new CreateServiceOrderCommand("nonexistent", "customer1", "user1", ServiceType.Maintenance, 500m);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithNegativeCost_ThrowsException()
        {
            // Arrange
            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, -100m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithZeroCost_CreatesServiceOrderSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 0m);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task Handle_WithEmptyCustomerId_ThrowsException()
        {
            // Arrange
            var command = new CreateServiceOrderCommand("order1", "", "user1", ServiceType.Maintenance, 500m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyCreatedBy_ThrowsException()
        {
            // Arrange
            var command = new CreateServiceOrderCommand("order1", "customer1", "", ServiceType.Maintenance, 500m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyOrderId_ThrowsException()
        {
            // Arrange
            var command = new CreateServiceOrderCommand("", "customer1", "user1", ServiceType.Maintenance, 500m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithPastAppointmentDate_ThrowsException()
        {
            // Arrange
            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(-1), "Test"); // Past date

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithLongDescription_CreatesServiceOrderSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var longDescription = new string('A', 1000); // Very long description

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m, null, longDescription);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.Is<ServiceOrder>(so => so.Description == longDescription), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithAllServiceTypes_CreatesCorrectServiceOrders()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            var serviceTypes = new[] { ServiceType.Maintenance, ServiceType.Repair, ServiceType.PreDelivery };

            foreach (var serviceType in serviceTypes)
            {
                var command = new CreateServiceOrderCommand("order1", "customer1", "user1", serviceType, 500m, null, $"{serviceType} service");

                // Act
                var result = await _handler.Handle(command, CancellationToken.None);

                // Assert
                result.Should().NotBeNullOrEmpty();
            }

            _mockServiceOrderRepository.Verify(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }
    }
}