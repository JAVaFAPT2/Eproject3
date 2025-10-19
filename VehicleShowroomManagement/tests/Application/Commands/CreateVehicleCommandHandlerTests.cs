using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Vehicles.Commands.CreateVehicle;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class CreateVehicleCommandHandlerTests
    {
        private readonly Mock<IRepository<Vehicle>> _mockVehicleRepository;
        private readonly CreateVehicleCommandHandler _handler;

        public CreateVehicleCommandHandlerTests()
        {
            _mockVehicleRepository = new Mock<IRepository<Vehicle>>();

            _handler = new CreateVehicleCommandHandler(_mockVehicleRepository.Object);
        }

        [Fact]
        public async Task Handle_WithValidData_CreatesVehicleAndReturnsId()
        {
            // Arrange
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, "EXT001", "VIN123456789");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockVehicleRepository.Verify(r => r.AddAsync(It.Is<Vehicle>(v => 
                v.VehicleId == "vehicle1" &&
                v.ModelNumber == "model1" &&
                v.PurchasePrice == 20000m &&
                v.ExternalNumber == "EXT001" &&
                v.Vin == "VIN123456789" &&
                v.Status == VehicleStatus.Available
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithMinimalData_CreatesVehicleAndReturnsId()
        {
            // Arrange
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, "EXT-001", "VIN-123456789");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockVehicleRepository.Verify(r => r.AddAsync(It.Is<Vehicle>(v => 
                v.VehicleId == "vehicle1" &&
                v.ModelNumber == "model1" &&
                v.PurchasePrice == 20000m &&
                v.ExternalNumber == "EXT-001" &&
                v.Vin == "VIN-123456789"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithNegativePurchasePrice_ThrowsException()
        {
            // Arrange
            var command = new CreateVehicleCommand("vehicle1", "model1", -1000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithZeroPurchasePrice_ThrowsException()
        {
            // Arrange
            var command = new CreateVehicleCommand("vehicle1", "model1", 0m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyVehicleId_ThrowsException()
        {
            // Arrange
            var command = new CreateVehicleCommand("", "model1", 20000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyModelNumber_ThrowsException()
        {
            // Arrange
            var command = new CreateVehicleCommand("vehicle1", "", 20000m);

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithValidPriceRange_CreatesVehicleSuccessfully()
        {
            // Arrange
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var command = new CreateVehicleCommand("vehicle1", "model1", 100000m, "EXT001", "VIN123456789"); // Large amount

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockVehicleRepository.Verify(r => r.AddAsync(It.Is<Vehicle>(v => v.PurchasePrice == 100000m), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithLongVin_CreatesVehicleSuccessfully()
        {
            // Arrange
            var longVin = new string('A', 50); // Very long VIN

            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, "EXT-001", longVin);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockVehicleRepository.Verify(r => r.AddAsync(It.Is<Vehicle>(v => v.Vin == longVin), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithLongExternalNumber_CreatesVehicleSuccessfully()
        {
            // Arrange
            var longExternalNumber = new string('B', 100); // Very long external number

            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, longExternalNumber, "VIN-123456789");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockVehicleRepository.Verify(r => r.AddAsync(It.Is<Vehicle>(v => v.ExternalNumber == longExternalNumber), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var command = new CreateVehicleCommand("vehicle1", "model1", 20000m, "EXT-001", "VIN-123456789");

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithMultipleVehicles_CreatesVehiclesSuccessfully()
        {
            // Arrange
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var commands = new[]
            {
                new CreateVehicleCommand("vehicle1", "model1", 20000m, "EXT001", "VIN001"),
                new CreateVehicleCommand("vehicle2", "model2", 25000m, "EXT002", "VIN002"),
                new CreateVehicleCommand("vehicle3", "model3", 30000m, "EXT003", "VIN003")
            };

            foreach (var cmd in commands)
            {
                // Act
                var result = await _handler.Handle(cmd, CancellationToken.None);

                // Assert
                result.Should().NotBeNullOrEmpty();
            }

            _mockVehicleRepository.Verify(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()), Times.Exactly(3));
        }

        [Fact]
        public async Task Handle_WithSpecialCharactersInIds_CreatesVehicleSuccessfully()
        {
            // Arrange
            _mockVehicleRepository.Setup(r => r.AddAsync(It.IsAny<Vehicle>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Vehicle vehicle, CancellationToken ct) => vehicle);

            var command = new CreateVehicleCommand("vehicle-001", "model-2024", 20000m, "EXT-001", "VIN-123456789");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNullOrEmpty();
            _mockVehicleRepository.Verify(r => r.AddAsync(It.Is<Vehicle>(v => 
                v.VehicleId == "vehicle-001" &&
                v.ModelNumber == "model-2024" &&
                v.ExternalNumber == "EXT-001" &&
                v.Vin == "VIN-123456789"
            ), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}