using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class CreateBillingDocumentCommandHandlerTests
    {
        private readonly Mock<IRepository<BillingDocument>> _mockBillingDocumentRepository;
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly CreateBillingDocumentCommandHandler _handler;

        public CreateBillingDocumentCommandHandlerTests()
        {
            _mockBillingDocumentRepository = new Mock<IRepository<BillingDocument>>();
            _mockOrderRepository = new Mock<IRepository<Order>>();

            _handler = new CreateBillingDocumentCommandHandler(
                _mockBillingDocumentRepository.Object,
                _mockOrderRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidData_CreatesBillingDocumentAndReturnsId()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", 25000m, DateTime.Now.AddDays(7));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => 
                bd.OrderId == "order1" &&
                bd.CreatedBy == "user1" &&
                bd.Amount == 25000m &&
                bd.Status == BillingStatus.Unpaid &&
                bd.AppointmentDate == command.AppointmentDate
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNonExistentOrder_ThrowsException()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetByIdAsync("nonexistent", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order?)null);

            var command = new CreateBillingDocumentCommand("nonexistent", "user1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithNegativeAmount_ThrowsException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            
            var command = new CreateBillingDocumentCommand("order1", "user1", -1000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithZeroAmount_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);
            
            var command = new CreateBillingDocumentCommand("order1", "user1", 0m);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithEmptyCreatedBy_ThrowsException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            
            var command = new CreateBillingDocumentCommand("order1", "", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyOrderId_ThrowsException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            
            var command = new CreateBillingDocumentCommand("", "user1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithPastAppointmentDate_ThrowsException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            
            var command = new CreateBillingDocumentCommand("order1", "user1", 25000m, DateTime.Now.AddDays(-1)); // Past date

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithValidAmountRange_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", 100000m); // Large amount

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => bd.Amount == 100000m), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNullAppointmentDate_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", 25000m, null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => bd.AppointmentDate == null), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var command = new CreateBillingDocumentCommand("order1", "user1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithMultipleOrders_CreatesBillingDocumentsSuccessfully()
        {
            // Arrange
            var order1 = new Order("customer1", "model1", 25000m);
            var order2 = new Order("customer2", "model2", 30000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order1);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order2", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order2);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var commands = new[]
            {
                new CreateBillingDocumentCommand("order1", "user1", 25000m),
                new CreateBillingDocumentCommand("order2", "user2", 30000m)
            };

            foreach (var cmd in commands)
            {
                // Act
                var result = await _handler.Handle(cmd, CancellationToken.None);

                // Assert
                result.Should().NotBeNullOrEmpty();
            }

            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_WithFutureAppointmentDate_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var futureDate = DateTime.Now.AddDays(30);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", 25000m, futureDate);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => bd.AppointmentDate == futureDate), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithVeryLargeAmount_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 1000000m);
            var largeAmount = 1000000m;

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", largeAmount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => bd.Amount == largeAmount), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithDecimalPrecision_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000.99m);
            var preciseAmount = 25000.99m;

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", preciseAmount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => bd.Amount == preciseAmount), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithWhitespaceCreatedBy_ThrowsException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            
            var command = new CreateBillingDocumentCommand("order1", "   ", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithNullCreatedBy_ThrowsException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            
            var command = new CreateBillingDocumentCommand("order1", null!, 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithWhitespaceOrderId_ThrowsException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("   ", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            
            var command = new CreateBillingDocumentCommand("   ", "user1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithNullOrderId_ThrowsException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(null!, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            
            var command = new CreateBillingDocumentCommand(null!, "user1", 25000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithCancellationToken_CallsRepositoryWithToken()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var cancellationToken = new CancellationToken();

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", cancellationToken))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), cancellationToken))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", 25000m);

            // Act
            var result = await _handler.Handle(command, cancellationToken);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockOrderRepository.Verify(r => r.GetByIdAsync("order1", cancellationToken), Times.Once);
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.IsAny<BillingDocument>(), cancellationToken), Times.Once);
        }

        [Fact]
        public async Task Handle_WithConcurrentRequests_CreatesMultipleBillingDocuments()
        {
            // Arrange
            var order1 = new Order("customer1", "model1", 25000m);
            var order2 = new Order("customer2", "model2", 30000m);

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order1);
            _mockOrderRepository.Setup(r => r.GetByIdAsync("order2", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order2);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command1 = new CreateBillingDocumentCommand("order1", "user1", 25000m);
            var command2 = new CreateBillingDocumentCommand("order2", "user2", 30000m);

            // Act
            var tasks = new[]
            {
                _handler.Handle(command1, CancellationToken.None),
                _handler.Handle(command2, CancellationToken.None)
            };
            var results = await Task.WhenAll(tasks);

            // Assert
            results.Should().HaveCount(2);
            results.Should().AllSatisfy(r => r.Should().NotBeNullOrEmpty());
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_WithSpecialCharactersInCreatedBy_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var specialCreatedBy = "user@domain.com";

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", specialCreatedBy, 25000m);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => bd.CreatedBy == specialCreatedBy), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithLongOrderId_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var longOrderId = "order-" + new string('x', 100);

            _mockOrderRepository.Setup(r => r.GetByIdAsync(longOrderId, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand(longOrderId, "user1", 25000m);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => bd.OrderId == longOrderId), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithMinimalValidAmount_CreatesBillingDocumentSuccessfully()
        {
            // Arrange
            var order = new Order("customer1", "model1", 0.01m);
            var minimalAmount = 0.01m;

            _mockOrderRepository.Setup(r => r.GetByIdAsync("order1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockBillingDocumentRepository.Setup(r => r.AddAsync(It.IsAny<BillingDocument>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((BillingDocument billingDocument, CancellationToken ct) => billingDocument);

            var command = new CreateBillingDocumentCommand("order1", "user1", minimalAmount);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockBillingDocumentRepository.Verify(r => r.AddAsync(It.Is<BillingDocument>(bd => bd.Amount == minimalAmount), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}