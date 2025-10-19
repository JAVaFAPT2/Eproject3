using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Common.Models;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;
using MediatR;

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
        public async Task Handle_WithValidStatusUpdate_UpdatesServiceOrderStatus()
        {
            // Arrange
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);
            var order = new Order("customer1", "model1", 25000m);

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.InProgress);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order status updated successfully");
            result.BillingDocumentId.Should().BeNull();
            
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(It.Is<ServiceOrder>(so => so.Status == ServiceOrderStatus.InProgress), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithCompletedMaintenanceService_CreatesBillingDocument()
        {
            // Arrange
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);
            var order = new Order("customer1", "model1", 25000m);

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order completed and billing document created");
            result.BillingDocumentId.Should().Be("billing-doc-id");
            
            _mockMediator.Verify(m => m.Send(It.Is<CreateBillingDocumentCommand>(cmd => 
                cmd.OrderId == "order1" &&
                cmd.CreatedBy == "user1" &&
                cmd.Amount == 500m
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithCompletedPreDeliveryService_CompletesOrderAndCreatesBillingDocument()
        {
            // Arrange
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.PreDelivery, 300m);
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Assign vehicle to make it confirmed
            var vehicle = new Vehicle("vehicle1", "model1", 20000m);

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockVehicleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Vehicle> { vehicle });
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order completed, order completed, vehicle marked as sold, and billing document created");
            result.BillingDocumentId.Should().Be("billing-doc-id");
            
            _mockOrderRepository.Verify(r => r.UpdateAsync(It.Is<Order>(o => o.Status == OrderStatus.Completed), It.IsAny<CancellationToken>()), Times.Once);
            _mockVehicleRepository.Verify(r => r.UpdateAsync(It.Is<Vehicle>(v => v.Status == VehicleStatus.Sold), It.IsAny<CancellationToken>()), Times.Once);
            _mockMediator.Verify(m => m.Send(It.Is<CreateBillingDocumentCommand>(cmd => 
                cmd.OrderId == "order1" &&
                cmd.CreatedBy == "user1" &&
                cmd.Amount == 25300m // 300 + 25000
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithCancelledService_UpdatesStatusOnly()
        {
            // Arrange
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Cancelled);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order cancelled");
            result.BillingDocumentId.Should().BeNull();
            
            _mockServiceOrderRepository.Verify(r => r.UpdateAsync(It.Is<ServiceOrder>(so => so.Status == ServiceOrderStatus.Cancelled), It.IsAny<CancellationToken>()), Times.Once);
            _mockMediator.Verify(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_WithLicensePlate_UpdatesVehicleLicensePlate()
        {
            // Arrange
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Maintenance, 500m);
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Use domain method to assign vehicle
            var vehicle = new Vehicle("vehicle1", "model1", 20000m);

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockVehicleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Vehicle, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Vehicle> { vehicle });

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.InProgress, "ABC-123");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            
            _mockVehicleRepository.Verify(r => r.UpdateAsync(It.Is<Vehicle>(v => v.LicensePlate == "ABC-123"), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentServiceOrder_ThrowsException()
        {
            // Arrange
            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("nonexistent", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder?)null);

            var command = new UpdateServiceOrderStatusCommand("nonexistent", ServiceOrderStatus.InProgress);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithNonExistentOrderForPreDelivery_ThrowsException()
        {
            // Arrange
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.PreDelivery, 300m);

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order?)null);

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyServiceOrderId_ThrowsException()
        {
            // Arrange
            var command = new UpdateServiceOrderStatusCommand("", ServiceOrderStatus.InProgress);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.InProgress);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithCompletedRepairService_CreatesBillingDocument()
        {
            // Arrange
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.Repair, 750m);
            var order = new Order("customer1", "model1", 25000m);

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order completed and billing document created");
            result.BillingDocumentId.Should().Be("billing-doc-id");
            
            _mockMediator.Verify(m => m.Send(It.Is<CreateBillingDocumentCommand>(cmd => 
                cmd.Amount == 750m
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithPreDeliveryServiceWithoutVehicle_CompletesOrderOnly()
        {
            // Arrange
            var serviceOrder = new ServiceOrder("order1", "customer1", "user1", ServiceType.PreDelivery, 300m);
            var order = new Order("customer1", "model1", 25000m); // No VehicleId
            order.AssignVehicle("vehicle1"); // Assign vehicle to make it confirmed

            _mockServiceOrderRepository.Setup(r => r.GetByIdAsync("service1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(serviceOrder);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockMediator.Setup(m => m.Send(It.IsAny<CreateBillingDocumentCommand>(), It.IsAny<CancellationToken>()))
                        .ReturnsAsync("billing-doc-id");

            var command = new UpdateServiceOrderStatusCommand("service1", ServiceOrderStatus.Completed);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Success.Should().BeTrue();
            result.Message.Should().Be("Service order completed, order completed, vehicle marked as sold, and billing document created");
            result.BillingDocumentId.Should().Be("billing-doc-id");
            
            _mockOrderRepository.Verify(r => r.UpdateAsync(It.Is<Order>(o => o.Status == OrderStatus.Completed), It.IsAny<CancellationToken>()), Times.Once);
            _mockVehicleRepository.Verify(r => r.UpdateAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}