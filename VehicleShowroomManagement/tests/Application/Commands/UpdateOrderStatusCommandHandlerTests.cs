using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Orders.Commands.UpdateOrderStatus;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class UpdateOrderStatusCommandHandlerTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly UpdateOrderStatusCommandHandler _handler;

        public UpdateOrderStatusCommandHandlerTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();

            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            _handler = new UpdateOrderStatusCommandHandler(
                _mockOrderRepository.Object,
                _mockVehicleRepository.Object,
                _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_CancelOrder_ClearsVehicleAndSetsVehicleAvailable()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1");
            var vehicle = new Vehicle("vehicle1", "model1", 20000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);
            _mockVehicleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(new List<Vehicle> { vehicle });

            var command = new UpdateOrderStatusCommand("order1", OrderStatus.Cancelled);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockVehicleRepository.Verify(r => r.UpdateAsync(It.Is<Vehicle>(v => v.Status == VehicleStatus.Available), It.IsAny<CancellationToken>()), Times.Once);
            _mockOrderRepository.Verify(r => r.UpdateAsync(It.Is<Order>(o => o.Status == OrderStatus.Cancelled), It.IsAny<CancellationToken>()), Times.AtLeastOnce);
            order.VehicleId.Should().BeNull();
            _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenVehicleUpdateFails_RollsBackTransaction()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1");
            var vehicle = new Vehicle("vehicle1", "model1", 20000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                                .ReturnsAsync(order);
            _mockVehicleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>()))
                                  .ReturnsAsync(new List<Vehicle> { vehicle });
            _mockVehicleRepository.Setup(r => r.UpdateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                                  .ThrowsAsync(new Exception("Update vehicle failed"));

            var command = new UpdateOrderStatusCommand("order1", OrderStatus.Cancelled);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
            _mockUnitOfWork.Verify(u => u.RollbackTransactionAsync(It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CommitTransactionAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}


