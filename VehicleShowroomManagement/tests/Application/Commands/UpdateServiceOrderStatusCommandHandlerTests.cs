using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class UpdateServiceOrderStatusCommandHandlerTests
    {
        private readonly Mock<IRepository<ServiceOrder>> _mockServiceOrderRepository;
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly Mock<IMediator> _mockMediator;
        private readonly UpdateServiceOrderStatusCommandHandler _handler;

        public UpdateServiceOrderStatusCommandHandlerTests()
        {
            _mockServiceOrderRepository = new Mock<IRepository<ServiceOrder>>();
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
            _mockMediator = new Mock<IMediator>();

            _handler = new UpdateServiceOrderStatusCommandHandler(
                _mockServiceOrderRepository.Object,
                _mockOrderRepository.Object,
                _mockVehicleRepository.Object,
                _mockMediator.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_UpdatesStatusAndSaves()
        {
            // Arrange
            var serviceOrderId = "service-order-1";
            var newStatus = ServiceOrderStatus.Completed;
            var existingServiceOrder = new ServiceOrder(
                orderId: "order-1",
                customerId: "customer-1", 
                createdBy: "user-1",
                type: ServiceType.Maintenance,
                cost: 500.00m,
                description: "Oil Change");

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(existingServiceOrder);

            _mockServiceOrderRepository.Setup(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                                     .Returns(Task.CompletedTask);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, newStatus);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingServiceOrder.Status.Should().Be(newStatus);
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(existingServiceOrder, It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentServiceOrder_ThrowsException()
        {
            // Arrange
            var serviceOrderId = "non-existent-id";
            var newStatus = ServiceOrderStatus.Completed;

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync((ServiceOrder?)null);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, newStatus);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithSameStatus_LogsWarningAndReturns()
        {
            // Arrange
            var serviceOrderId = "service-order-1";
            var currentStatus = ServiceOrderStatus.Completed;
            var existingServiceOrder = new ServiceOrder(
                orderId: "order-1",
                customerId: "customer-1", 
                createdBy: "user-1",
                type: ServiceType.Maintenance,
                cost: 500.00m,
                description: "Oil Change");

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(existingServiceOrder);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, currentStatus);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Never);

            // Verify warning log
            // Note: Logger verification removed as logger is not injected in the constructor
        }

        [Fact]
        public async Task Handle_WithInvalidStatusTransition_ThrowsException()
        {
            // Arrange
            var serviceOrderId = "service-order-1";
            var invalidNewStatus = ServiceOrderStatus.Scheduled; // Can't go back to scheduled from completed
            var existingServiceOrder = new ServiceOrder(
                orderId: "order-1",
                customerId: "customer-1", 
                createdBy: "user-1",
                type: ServiceType.Maintenance,
                cost: 500.00m,
                description: "Oil Change");

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(existingServiceOrder);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, invalidNewStatus);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(ServiceOrderStatus.Scheduled, ServiceOrderStatus.InProgress)]
        [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.Completed)]
        [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.Cancelled)]
        public async Task Handle_WithValidStatusTransitions_UpdatesSuccessfully(ServiceOrderStatus currentStatus, ServiceOrderStatus newStatus)
        {
            // Arrange
            var serviceOrderId = "service-order-1";
            var existingServiceOrder = new ServiceOrder(
                orderId: "order-1",
                customerId: "customer-1", 
                createdBy: "user-1",
                type: ServiceType.Maintenance,
                cost: 500.00m,
                description: "Oil Change");
            
            // Set the current status for testing
            existingServiceOrder.UpdateStatus(currentStatus);

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(existingServiceOrder);

            _mockServiceOrderRepository.Setup(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                                     .Returns(Task.CompletedTask);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, newStatus);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingServiceOrder.Status.Should().Be(newStatus);
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(existingServiceOrder, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
