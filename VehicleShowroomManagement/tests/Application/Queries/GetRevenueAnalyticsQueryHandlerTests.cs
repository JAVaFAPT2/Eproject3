using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRevenueAnalytics;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Queries
{
    public class GetRevenueAnalyticsQueryHandlerTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly GetRevenueAnalyticsQueryHandler _handler;

        public GetRevenueAnalyticsQueryHandlerTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();

            _handler = new GetRevenueAnalyticsQueryHandler(_mockOrderRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidData_ReturnsRevenueAnalytics()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("customer1", "model1", 25000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-30) },
                new Order("customer2", "model2", 30000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-15) },
                new Order("customer3", "model3", 20000m) { Status = OrderStatus.Pending, OrderDate = DateTime.UtcNow.AddDays(-10) }
            };

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var query = new GetRevenueAnalyticsQuery(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalRevenue.Should().Be(55000m); // Only completed orders
            result.TotalOrders.Should().Be(2); // Only completed orders
            result.RevenueData.Should().NotBeEmpty();
            result.AverageOrderValue.Should().Be(27500m); // 55000 / 2
        }

        [Fact]
        public async Task Handle_WithNoCompletedOrders_ReturnsZeroRevenue()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("customer1", "model1", 25000m) { Status = OrderStatus.Pending, OrderDate = DateTime.UtcNow.AddDays(-30) },
                new Order("customer2", "model2", 30000m) { Status = OrderStatus.Cancelled, OrderDate = DateTime.UtcNow.AddDays(-15) }
            };

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var query = new GetRevenueAnalyticsQuery(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalRevenue.Should().Be(0m);
            result.TotalOrders.Should().Be(0);
            result.AverageOrderValue.Should().Be(0m);
            result.GrowthPercentage.Should().Be(0m);
        }

        [Fact]
        public async Task Handle_WithEmptyOrderList_ReturnsZeroRevenue()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Order>());

            var query = new GetRevenueAnalyticsQuery(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalRevenue.Should().Be(0m);
            result.TotalOrders.Should().Be(0);
            result.AverageOrderValue.Should().Be(0m);
            result.GrowthPercentage.Should().Be(0m);
        }

        [Fact]
        public async Task Handle_WithCustomDateRange_ReturnsFilteredRevenue()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("customer1", "model1", 25000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-60) },
                new Order("customer2", "model2", 30000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-30) },
                new Order("customer3", "model3", 20000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-10) }
            };

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var fromDate = DateTime.UtcNow.AddDays(-40);
            var toDate = DateTime.UtcNow.AddDays(-5);
            var query = new GetRevenueAnalyticsQuery(fromDate, toDate, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalRevenue.Should().Be(50000m); // Only orders within date range
            result.TotalOrders.Should().Be(2);
        }

        [Fact]
        public async Task Handle_WithNullDateRange_UsesDefaultRange()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("customer1", "model1", 25000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-30) }
            };

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var query = new GetRevenueAnalyticsQuery(null, null, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalRevenue.Should().Be(25000m);
            result.RevenueData.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Handle_WithGrowthCalculation_ReturnsCorrectGrowthPercentage()
        {
            // Arrange
            var currentMonth = DateTime.UtcNow;
            var previousMonth = currentMonth.AddMonths(-1);
            
            var orders = new List<Order>
            {
                // Previous month orders
                new Order("customer1", "model1", 20000m) { Status = OrderStatus.Completed, OrderDate = previousMonth.AddDays(15) },
                // Current month orders
                new Order("customer2", "model2", 30000m) { Status = OrderStatus.Completed, OrderDate = currentMonth.AddDays(15) }
            };

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var query = new GetRevenueAnalyticsQuery(null, null, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.PreviousPeriodRevenue.Should().Be(20000m);
            result.GrowthPercentage.Should().Be(50m); // (30000 - 20000) / 20000 * 100
        }

        [Fact]
        public async Task Handle_WithZeroPreviousRevenue_ReturnsZeroGrowth()
        {
            // Arrange
            var currentMonth = DateTime.UtcNow;
            
            var orders = new List<Order>
            {
                // Only current month orders
                new Order("customer1", "model1", 25000m) { Status = OrderStatus.Completed, OrderDate = currentMonth.AddDays(15) }
            };

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var query = new GetRevenueAnalyticsQuery(null, null, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.PreviousPeriodRevenue.Should().Be(0m);
            result.GrowthPercentage.Should().Be(0m);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var query = new GetRevenueAnalyticsQuery(DateTime.UtcNow.AddDays(-30), DateTime.UtcNow, "month");

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(query, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithLargeDataset_ReturnsCorrectAnalytics()
        {
            // Arrange
            var orders = new List<Order>();
            var random = new Random(42); // Fixed seed for consistent results
            
            for (int i = 0; i < 1000; i++)
            {
                var orderDate = DateTime.UtcNow.AddDays(-random.Next(0, 180));
                var status = random.Next(0, 4) switch
                {
                    0 => OrderStatus.Pending,
                    1 => OrderStatus.Confirmed,
                    2 => OrderStatus.Completed,
                    _ => OrderStatus.Cancelled
                };
                
                orders.Add(new Order($"customer{i}", $"model{i % 10}", random.Next(15000, 50000)) 
                { 
                    Status = status, 
                    OrderDate = orderDate 
                });
            }

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var query = new GetRevenueAnalyticsQuery(DateTime.UtcNow.AddDays(-180), DateTime.UtcNow, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalOrders.Should().BeGreaterThan(0);
            result.RevenueData.Should().NotBeEmpty();
            result.RevenueByCategory.Should().NotBeNull();
        }

        [Fact]
        public async Task Handle_WithDifferentPeriods_ReturnsCorrectDataPoints()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("customer1", "model1", 25000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-90) },
                new Order("customer2", "model2", 30000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-60) },
                new Order("customer3", "model3", 20000m) { Status = OrderStatus.Completed, OrderDate = DateTime.UtcNow.AddDays(-30) }
            };

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var query = new GetRevenueAnalyticsQuery(DateTime.UtcNow.AddDays(-90), DateTime.UtcNow, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.RevenueData.Should().HaveCount(6); // 6 months of data points
            result.RevenueData.Should().OnlyContain(dp => dp.Value >= 0);
        }
    }
}