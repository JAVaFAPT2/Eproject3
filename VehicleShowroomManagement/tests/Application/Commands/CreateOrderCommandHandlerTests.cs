using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class CreateOrderCommandHandlerTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<VehicleModel>> _mockModelRepository;
        private readonly CreateOrderCommandHandler _handler;

        public CreateOrderCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockModelRepository = new Mock<IRepository<VehicleModel>>();

            _handler = new CreateOrderCommandHandler(
                _mockOrderRepository.Object,
                _mockModelRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidData_CreatesOrderAndReturnsId()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            var command = new CreateOrderCommand("customer1", "model1", 25000m);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockOrderRepository.Verify(r => r.AddAsync(It.Is<Order>(o => 
                o.CustomerId == "customer1" &&
                o.ModelNumber == "model1" &&
                o.SalePrice == 25000m &&
                o.Status == OrderStatus.Pending
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentModel_ThrowsException()
        {
            // Arrange
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel>());

            var command = new CreateOrderCommand("customer1", "nonexistent", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithNegativeSalePrice_ThrowsException()
        {
            // Arrange
            var command = new CreateOrderCommand("customer1", "model1", -1000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithZeroSalePrice_CreatesOrderSuccessfully()
        {
            // Arrange
            var model = new VehicleModel("model1", "Brand1", 20000m, "Model description");
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new[] { model });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            
            var command = new CreateOrderCommand("customer1", "model1", 0m);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithEmptyCustomerId_ThrowsException()
        {
            // Arrange
            var command = new CreateOrderCommand("", "model1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyModelNumber_ThrowsException()
        {
            // Arrange
            var command = new CreateOrderCommand("customer1", "", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var command = new CreateOrderCommand("customer1", "model1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithDifferentModels_CreatesOrdersSuccessfully()
        {
            // Arrange
            var model1 = new VehicleModel("model1", "Model 1", 25000m, "Description 1", level: 1);
            var model2 = new VehicleModel("model2", "Model 2", 30000m, "Description 2", level: 2);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((System.Linq.Expressions.Expression<Func<VehicleModel, bool>> predicate, CancellationToken ct) => 
                              {
                                  var models = new List<VehicleModel> { model1, model2 };
                                  return models.Where(predicate.Compile()).ToList();
                              });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            var commands = new[]
            {
                new CreateOrderCommand("customer1", "model1", 20000m),
                new CreateOrderCommand("customer2", "model2", 25000m)
            };

            foreach (var cmd in commands)
            {
                // Act
                var result = await _handler.Handle(cmd, CancellationToken.None);

                // Assert
                result.Should().NotBeNullOrEmpty();
            }

            _mockOrderRepository.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_WithValidPriceRange_CreatesOrderSuccessfully()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            var command = new CreateOrderCommand("customer1", "model1", 50000m);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockOrderRepository.Verify(r => r.AddAsync(It.Is<Order>(o => o.SalePrice == 50000m), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}