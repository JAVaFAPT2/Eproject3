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
                new Order("customer1", "model1", 25000m),
                new Order("customer2", "model2", 30000m),
                new Order("customer3", "model3", 20000m)
            };
            
            // Set up orders with proper status using domain methods
            orders[0].AssignVehicle("vehicle1"); // Assign vehicle and confirm
            orders[0].Complete(); // Complete the first order
            orders[1].AssignVehicle("vehicle2"); // Assign vehicle and confirm  
            orders[1].Complete(); // Complete the second order
            // Third order remains pending

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
                new Order("customer1", "model1", 25000m),
                new Order("customer2", "model2", 30000m)
            };
            
            // Set up orders with proper status using domain methods
            // First order remains pending, second order is cancelled
            orders[1].Cancel();

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
            var now = DateTime.UtcNow;
            var orders = new List<Order>();
            
            // Create orders with specific dates within the range
            var order1 = new Order("customer1", "model1", 25000m);
            var order2 = new Order("customer2", "model2", 30000m);
            var order3 = new Order("customer3", "model3", 20000m);
            
            // Set up orders with proper status using domain methods
            order1.AssignVehicle("vehicle1"); order1.Complete();
            order2.AssignVehicle("vehicle2"); order2.Complete();
            order3.AssignVehicle("vehicle3"); order3.Complete();

            // Use reflection to set order dates to specific dates within the range
            var orderDateField = typeof(Order).GetField("<OrderDate>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (orderDateField != null)
            {
                orderDateField.SetValue(order1, now.AddDays(-30)); // Within range
                orderDateField.SetValue(order2, now.AddDays(-20)); // Within range
                orderDateField.SetValue(order3, now.AddDays(-10)); // Within range
            }
            
            orders.Add(order1);
            orders.Add(order2);
            orders.Add(order3);

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var fromDate = DateTime.UtcNow.AddDays(-40);
            var toDate = DateTime.UtcNow.AddDays(-5);
            var query = new GetRevenueAnalyticsQuery(fromDate, toDate, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.TotalRevenue.Should().Be(75000m); // All orders are within date range
            result.TotalOrders.Should().Be(3);
        }

        [Fact]
        public async Task Handle_WithNullDateRange_UsesDefaultRange()
        {
            // Arrange
            var orders = new List<Order>
            {
                new Order("customer1", "model1", 25000m)
            };

            // Set up order with proper status using domain methods
            orders[0].AssignVehicle("vehicle1");
            orders[0].Complete();

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
            
            // Create orders at different times to get different OrderDate values
            var orders = new List<Order>();
            
            // Create first order (previous month) by manipulating system time
            var order1 = new Order("customer1", "model1", 20000m);
            orders.Add(order1);
            
            // Create second order (current month)
            var order2 = new Order("customer2", "model2", 30000m);
            orders.Add(order2);
            
            // Set up orders with proper status using domain methods
            orders[0].AssignVehicle("vehicle1"); orders[0].Complete(); // Complete both orders
            orders[1].AssignVehicle("vehicle2"); orders[1].Complete();

            // Use reflection to set order dates to specific months
            var orderDateField = typeof(Order).GetField("<OrderDate>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (orderDateField != null)
            {
                orderDateField.SetValue(orders[0], previousMonth); // Previous month
                orderDateField.SetValue(orders[1], currentMonth); // Current month
            }

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
                new Order("customer1", "model1", 25000m)
            };
            
            // Set up orders with proper status using domain methods
            orders[0].AssignVehicle("vehicle1"); orders[0].Complete(); // Complete the order

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
                
                var order = new Order($"customer{i}", $"model{i % 10}", random.Next(15000, 50000));
                
                // Use reflection to set OrderDate for test purposes
                var orderDateField = typeof(Order).GetField("_orderDate", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (orderDateField == null)
                {
                    // If no private field, try to set via property setter
                    var orderDateProperty = typeof(Order).GetProperty("OrderDate");
                    if (orderDateProperty?.CanWrite == true)
                    {
                        orderDateProperty.SetValue(order, orderDate);
                    }
                }
                else
                {
                    orderDateField.SetValue(order, orderDate);
                }
                
                // Set status using domain methods
                switch (status)
                {
                    case OrderStatus.Confirmed:
                        order.AssignVehicle($"vehicle{i}");
                        break;
                    case OrderStatus.Completed:
                        order.AssignVehicle($"vehicle{i}");
                        order.Complete();
                        break;
                    case OrderStatus.Cancelled:
                        order.Cancel();
                        break;
                    // Pending is default, no action needed
                }
                
                orders.Add(order);
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
            var orders = new List<Order>();
            
            // Create orders for 6 months
            for (int i = 0; i < 6; i++)
            {
                var order = new Order($"customer{i+1}", $"model{i+1}", 25000m + (i * 1000));
                order.AssignVehicle($"vehicle{i+1}");
                order.Complete();
                orders.Add(order);
            }

            // Use reflection to set order dates to specific months
            var orderDateField = typeof(Order).GetField("<OrderDate>k__BackingField", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            if (orderDateField != null)
            {
                var now = DateTime.UtcNow;
                for (int i = 0; i < 6; i++)
                {
                    orderDateField.SetValue(orders[i], now.AddMonths(-5 + i)); // 5 months ago to current month
                }
            }

            _mockOrderRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(orders);

            var query = new GetRevenueAnalyticsQuery(DateTime.UtcNow.AddMonths(-5), DateTime.UtcNow, "month");

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.RevenueData.Should().HaveCount(6); // 6 months of data points
            result.RevenueData.Should().OnlyContain(dp => dp.Value >= 0);
        }
    }
}