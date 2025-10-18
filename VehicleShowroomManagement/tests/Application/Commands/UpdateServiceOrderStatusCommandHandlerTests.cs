using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class UpdateServiceOrderStatusCommandHandlerTests
    {
        private readonly Mock<IRepository<ServiceOrder>> _mockServiceOrderRepository;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<UpdateServiceOrderStatusCommandHandler>> _mockLogger;
        private readonly UpdateServiceOrderStatusCommandHandler _handler;

        public UpdateServiceOrderStatusCommandHandlerTests()
        {
            _mockServiceOrderRepository = new Mock<IRepository<ServiceOrder>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<UpdateServiceOrderStatusCommandHandler>>();

            _handler = new UpdateServiceOrderStatusCommandHandler(
                _mockServiceOrderRepository.Object,
                _mockUnitOfWork.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithValidCommand_UpdatesStatusAndSaves()
        {
            // Arrange
            var serviceOrderId = "service-order-1";
            var newStatus = ServiceOrderStatus.Completed;
            var existingServiceOrder = new ServiceOrder
            {
                Id = serviceOrderId,
                Status = ServiceOrderStatus.InProgress,
                Cost = 500.00m,
                Description = "Oil Change"
            };

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(existingServiceOrder);

            _mockServiceOrderRepository.Setup(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                                     .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, newStatus);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingServiceOrder.Status.Should().Be(newStatus);
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(existingServiceOrder, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
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
            var existingServiceOrder = new ServiceOrder
            {
                Id = serviceOrderId,
                Status = currentStatus,
                Cost = 500.00m,
                Description = "Oil Change"
            };

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(existingServiceOrder);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, currentStatus);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);

            // Verify warning log
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Warning,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Service order already has the requested status")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidStatusTransition_ThrowsException()
        {
            // Arrange
            var serviceOrderId = "service-order-1";
            var currentStatus = ServiceOrderStatus.Completed;
            var invalidNewStatus = ServiceOrderStatus.Pending; // Can't go back to pending from completed
            var existingServiceOrder = new ServiceOrder
            {
                Id = serviceOrderId,
                Status = currentStatus,
                Cost = 500.00m,
                Description = "Oil Change"
            };

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(existingServiceOrder);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, invalidNewStatus);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));
        }

        [Theory]
        [InlineData(ServiceOrderStatus.Pending, ServiceOrderStatus.InProgress)]
        [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.Completed)]
        [InlineData(ServiceOrderStatus.InProgress, ServiceOrderStatus.Cancelled)]
        public async Task Handle_WithValidStatusTransitions_UpdatesSuccessfully(ServiceOrderStatus currentStatus, ServiceOrderStatus newStatus)
        {
            // Arrange
            var serviceOrderId = "service-order-1";
            var existingServiceOrder = new ServiceOrder
            {
                Id = serviceOrderId,
                Status = currentStatus,
                Cost = 500.00m,
                Description = "Oil Change"
            };

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(serviceOrderId, It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(existingServiceOrder);

            _mockServiceOrderRepository.Setup(r => r.UpdateAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                                     .Returns(Task.CompletedTask);

            _mockUnitOfWork.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                          .Returns(Task.CompletedTask);

            var command = new UpdateServiceOrderStatusCommand(serviceOrderId, newStatus);

            // Act
            await _handler.Handle(command, CancellationToken.None);

            // Assert
            existingServiceOrder.Status.Should().Be(newStatus);
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(existingServiceOrder, It.IsAny<CancellationToken>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
