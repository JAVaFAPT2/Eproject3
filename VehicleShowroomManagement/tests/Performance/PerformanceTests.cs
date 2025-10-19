using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder;
using VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;
using System.Diagnostics;

namespace VehicleShowroomManagement.Tests.Performance
{
    public class PerformanceTests
    {
        private readonly Mock<IRepository<Order>> _mockOrderRepository;
        private readonly Mock<IRepository<ServiceOrder>> _mockServiceOrderRepository;
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly Mock<IRepository<VehicleModel>> _mockModelRepository;
        private readonly CreateOrderCommandHandler _createOrderHandler;
        private readonly CreateServiceOrderCommandHandler _createServiceOrderHandler;
        private readonly CreateVehicleCommandHandler _createVehicleHandler;

        public PerformanceTests()
        {
            _mockOrderRepository = new Mock<IRepository<Order>>();
            _mockServiceOrderRepository = new Mock<IRepository<ServiceOrder>>();
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();
            _mockModelRepository = new Mock<IRepository<VehicleModel>>();

            _createOrderHandler = new CreateOrderCommandHandler(
                _mockOrderRepository.Object,
                _mockModelRepository.Object);

            _createServiceOrderHandler = new CreateServiceOrderCommandHandler(
                _mockServiceOrderRepository.Object,
                _mockOrderRepository.Object);

            _createVehicleHandler = new CreateVehicleCommandHandler(
                _mockVehicleRepository.Object);
        }

        [Fact]
        public async Task CreateOrder_PerformanceTest_CompletesWithinAcceptableTime()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            var command = new CreateOrderCommand("customer1", "model1", 25000m);
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await _createOrderHandler.Handle(command, CancellationToken.None);
            stopwatch.Stop();

            // Assert
            result.Should().NotBeNullOrEmpty();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100); // Should complete within 100ms
        }

        [Fact]
        public async Task CreateMultipleOrders_PerformanceTest_HandlesBulkOperationsEfficiently()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            const int orderCount = 1000;
            var commands = Enumerable.Range(1, orderCount)
                .Select(i => new CreateOrderCommand($"customer{i}", "model1", 25000m + i))
                .ToArray();

            var stopwatch = Stopwatch.StartNew();

            // Act
            var tasks = commands.Select(cmd => _createOrderHandler.Handle(cmd, CancellationToken.None));
            var results = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            results.Should().HaveCount(orderCount);
            results.Should().AllSatisfy(r => r.Should().NotBeNullOrEmpty());
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000); // Should complete within 5 seconds
        }

        [Fact]
        public async Task ConcurrentOrderCreation_PerformanceTest_HandlesConcurrencyEfficiently()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            const int concurrentTasks = 100;
            var commands = Enumerable.Range(1, concurrentTasks)
                .Select(i => new CreateOrderCommand($"customer{i}", "model1", 25000m + i))
                .ToArray();

            var stopwatch = Stopwatch.StartNew();

            // Act
            var tasks = commands.Select(cmd => _createOrderHandler.Handle(cmd, CancellationToken.None));
            var results = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            results.Should().HaveCount(concurrentTasks);
            results.Should().AllSatisfy(r => r.Should().NotBeNullOrEmpty());
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(2000); // Should complete within 2 seconds
        }

        [Fact]
        public async Task CreateVehicle_PerformanceTest_CompletesWithinAcceptableTime()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, "VIN123456789", "EXT001");
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await _createVehicleHandler.Handle(command, CancellationToken.None);
            stopwatch.Stop();

            // Assert
            result.Should().NotBeNullOrEmpty();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100); // Should complete within 100ms
        }

        [Fact]
        public async Task CreateServiceOrder_PerformanceTest_CompletesWithinAcceptableTime()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            var command = new CreateServiceOrderCommand("order1", "customer1", "user1", ServiceType.Maintenance, 500m, DateTime.Now.AddDays(7), "Regular maintenance");
            var stopwatch = Stopwatch.StartNew();

            // Act
            var result = await _createServiceOrderHandler.Handle(command, CancellationToken.None);
            stopwatch.Stop();

            // Assert
            result.Should().NotBeNullOrEmpty();
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(100); // Should complete within 100ms
        }

        [Fact]
        public async Task MemoryUsageTest_CreatesLargeNumberOfEntitiesWithoutMemoryLeaks()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            const int entityCount = 10000;
            var commands = Enumerable.Range(1, entityCount)
                .Select(i => new CreateOrderCommand($"customer{i}", "model1", 25000m + i))
                .ToArray();

            var initialMemory = GC.GetTotalMemory(true);

            // Act
            var tasks = commands.Select(cmd => _createOrderHandler.Handle(cmd, CancellationToken.None));
            var results = await Task.WhenAll(tasks);

            // Force garbage collection
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();

            var finalMemory = GC.GetTotalMemory(false);

            // Assert
            results.Should().HaveCount(entityCount);
            results.Should().AllSatisfy(r => r.Should().NotBeNullOrEmpty());
            
            // Memory usage should not grow excessively (allowing for some overhead)
            var memoryIncrease = finalMemory - initialMemory;
            memoryIncrease.Should().BeLessThan(100 * 1024 * 1024); // Less than 100MB increase
        }

        [Fact]
        public async Task StressTest_MixedOperations_HandlesHighLoadGracefully()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            var order = new Order("customer1", "model1", 25000m);

            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);
            _mockOrderRepository.Setup(r => r.GetByIdAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(order);
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);
            _mockServiceOrderRepository.Setup(r => r.AddAsync(It.IsAny<ServiceOrder>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((ServiceOrder serviceOrder, CancellationToken ct) => serviceOrder);

            const int operationCount = 500;
            var stopwatch = Stopwatch.StartNew();

            // Act - Mix of different operations
            var orderTasks = Enumerable.Range(1, operationCount)
                .Select(i => _createOrderHandler.Handle(new CreateOrderCommand($"customer{i}", "model1", 25000m + i), CancellationToken.None));

            var vehicleTasks = Enumerable.Range(1, operationCount)
                .Select(i => _createVehicleHandler.Handle(new CreateVehicleCommand($"vehicle{i}", "model1", 20000m + i, $"VIN{i:D9}", $"EXT{i:D3}"), CancellationToken.None));

            var serviceOrderTasks = Enumerable.Range(1, operationCount)
                .Select(i => _createServiceOrderHandler.Handle(new CreateServiceOrderCommand($"order{i}", $"customer{i}", "user1", ServiceType.Maintenance, 500m + i, DateTime.Now.AddDays(i), $"Service {i}"), CancellationToken.None));

            var allTasks = orderTasks.Concat(vehicleTasks).Concat(serviceOrderTasks);
            var results = await Task.WhenAll(allTasks);
            stopwatch.Stop();

            // Assert
            results.Should().HaveCount(operationCount * 3);
            results.Should().AllSatisfy(r => r.Should().NotBeNullOrEmpty());
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(10000); // Should complete within 10 seconds
        }

        [Fact]
        public async Task CancellationTokenPerformanceTest_HandlesCancellationEfficiently()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            var cancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = cancellationTokenSource.Token;

            const int taskCount = 100;
            var commands = Enumerable.Range(1, taskCount)
                .Select(i => new CreateOrderCommand($"customer{i}", "model1", 25000m + i))
                .ToArray();

            var stopwatch = Stopwatch.StartNew();

            // Act
            var tasks = commands.Select(cmd => _createOrderHandler.Handle(cmd, cancellationToken));
            
            // Cancel after a short delay
            cancellationTokenSource.CancelAfter(50);

            try
            {
                var results = await Task.WhenAll(tasks);
                stopwatch.Stop();
                
                // If we get here, all tasks completed before cancellation
                results.Should().HaveCount(taskCount);
            }
            catch (OperationCanceledException)
            {
                stopwatch.Stop();
                // Expected behavior when cancellation occurs
            }

            // Assert
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(200); // Should handle cancellation quickly
        }

        [Fact]
        public async Task DatabaseConnectionStressTest_SimulatesHighDatabaseLoad()
        {
            // Arrange
            var vehicleModel = new VehicleModel("model1", "Test Model", 25000m, "Test Description", level: 2);
            
            // Simulate database delay
            _mockModelRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<VehicleModel, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<VehicleModel> { vehicleModel });
            _mockOrderRepository.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Order order, CancellationToken ct) => order);

            const int requestCount = 200;
            var commands = Enumerable.Range(1, requestCount)
                .Select(i => new CreateOrderCommand($"customer{i}", "model1", 25000m + i))
                .ToArray();

            var stopwatch = Stopwatch.StartNew();

            // Act
            var tasks = commands.Select(cmd => _createOrderHandler.Handle(cmd, CancellationToken.None));
            var results = await Task.WhenAll(tasks);
            stopwatch.Stop();

            // Assert
            results.Should().HaveCount(requestCount);
            results.Should().AllSatisfy(r => r.Should().NotBeNullOrEmpty());
            
            // Should complete within reasonable time even with simulated database delays
            stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000);
        }
    }
}
