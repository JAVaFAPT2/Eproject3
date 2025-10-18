using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetOverview;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Queries
{
    public class GetOverviewQueryHandlerTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<ServiceOrder>> _mockServiceOrderRepository;
        private readonly Mock<IRepository<PurchaseOrder>> _mockPurchaseOrderRepository;
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<VehicleModel>> _mockModelRepository;
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly Mock<ILogger<GetOverviewQueryHandler>> _mockLogger;
        private readonly GetOverviewQueryHandler _handler;

        public GetOverviewQueryHandlerTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockServiceOrderRepository = new Mock<IRepository<ServiceOrder>>();
            _mockPurchaseOrderRepository = new Mock<IRepository<PurchaseOrder>>();
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockModelRepository = new Mock<IRepository<VehicleModel>>();
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
            _mockLogger = new Mock<ILogger<GetOverviewQueryHandler>>();

            _handler = new GetOverviewQueryHandler(
                _mockOrderRepository.Object,
                _mockServiceOrderRepository.Object,
                _mockPurchaseOrderRepository.Object,
                _mockUserRepository.Object,
                _mockModelRepository.Object,
                _mockVehicleRepository.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsCorrectOverview()
        {
            // Arrange
            var completedOrders = new List<Order>
            {
                new Order { Id = "1", Status = OrderStatus.Completed, SalePrice = 25000, CustomerId = "customer1" },
                new Order { Id = "2", Status = OrderStatus.Completed, SalePrice = 30000, CustomerId = "customer2" },
                new Order { Id = "3", Status = OrderStatus.Pending, SalePrice = 20000, CustomerId = "customer3" }
            };

            var serviceOrders = new List<ServiceOrder>
            {
                new ServiceOrder { Id = "1", Status = ServiceOrderStatus.Completed, Cost = 500 },
                new ServiceOrder { Id = "2", Status = ServiceOrderStatus.Completed, Cost = 750 },
                new ServiceOrder { Id = "3", Status = ServiceOrderStatus.Pending, Cost = 300 }
            };

            var purchaseOrders = new List<PurchaseOrder>
            {
                new PurchaseOrder { Id = "1", TotalAmount = 20000 },
                new PurchaseOrder { Id = "2", TotalAmount = 15000 }
            };

            var employees = new List<User>
            {
                new User { Id = "1", HireDate = DateTime.Now.AddDays(-30) },
                new User { Id = "2", HireDate = DateTime.Now.AddDays(-60) }
            };

            var level2Models = new List<VehicleModel>
            {
                new VehicleModel { Id = "1", Level = 2 },
                new VehicleModel { Id = "2", Level = 2 },
                new VehicleModel { Id = "3", Level = 1 }
            };

            var vehicles = new List<Vehicle>
            {
                new Vehicle { Id = "1" },
                new Vehicle { Id = "2" },
                new Vehicle { Id = "3" }
            };

            _mockOrderRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(completedOrders.Where(o => o.Status == OrderStatus.Completed));

            _mockServiceOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(serviceOrders);

            _mockPurchaseOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(purchaseOrders);

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(employees);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(level2Models.Where(m => m.Level == 2));

            _mockVehicleRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(vehicles);

            var query = new GetOverviewQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CompletedOrders.Should().Be(2);
            result.Employees.Should().Be(2);
            result.CustomersPurchased.Should().Be(2);
            result.Level2Models.Should().Be(2);
            result.Vehicles.Should().Be(3);
            
            // Profit = (25000 + 30000 + 500 + 750) - (20000 + 15000) = 56250 - 35000 = 21250
            result.Profit.Should().Be(21250);
        }

        [Fact]
        public async Task Handle_WithEmptyData_ReturnsZeroValues()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Order>());

            _mockServiceOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                                     .ReturnsAsync(new List<ServiceOrder>());

            _mockPurchaseOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                                      .ReturnsAsync(new List<PurchaseOrder>());

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                             .ReturnsAsync(new List<User>());

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                               .ReturnsAsync(new List<VehicleModel>());

            _mockVehicleRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                                 .ReturnsAsync(new List<Vehicle>());

            var query = new GetOverviewQuery();

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CompletedOrders.Should().Be(0);
            result.Employees.Should().Be(0);
            result.CustomersPurchased.Should().Be(0);
            result.Level2Models.Should().Be(0);
            result.Vehicles.Should().Be(0);
            result.Profit.Should().Be(0);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_LogsErrorAndRethrows()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Order, bool>>>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var query = new GetOverviewQuery();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));

            // Verify logging
            _mockLogger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error calculating dashboard overview")),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
                Times.Once);
        }
    }
}
