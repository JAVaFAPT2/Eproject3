using Xunit;
using FluentAssertions;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Tests.Domain
{
    public class OrderEntityTests
    {
        [Fact]
        public void Constructor_WithValidParameters_CreatesOrderWithCorrectProperties()
        {
            // Arrange
            var customerId = "customer1";
            var modelNumber = "model1";
            var salePrice = 25000m;
            var dealerId = "dealer1";

            // Act
            var order = new Order(customerId, modelNumber, salePrice, dealerId);

            // Assert
            order.CustomerId.Should().Be(customerId);
            order.ModelNumber.Should().Be(modelNumber);
            order.SalePrice.Should().Be(salePrice);
            order.DealerId.Should().Be(dealerId);
            order.Status.Should().Be(OrderStatus.Pending);
            order.VehicleId.Should().BeNull();
            order.AppointmentDate.Should().BeNull();
            order.Note.Should().BeNull();
            order.ReservationFrom.Should().BeNull();
            order.ReservationTo.Should().BeNull();
            order.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Constructor_WithNullCustomerId_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Order(null!, "model1", 25000m));
        }

        [Fact]
        public void Constructor_WithEmptyCustomerId_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Order("", "model1", 25000m));
        }

        [Fact]
        public void Constructor_WithWhitespaceCustomerId_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Order("   ", "model1", 25000m));
        }

        [Fact]
        public void Constructor_WithNullModelNumber_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Order("customer1", null!, 25000m));
        }

        [Fact]
        public void Constructor_WithEmptyModelNumber_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Order("customer1", "", 25000m));
        }

        [Fact]
        public void Constructor_WithNegativeSalePrice_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => new Order("customer1", "model1", -1000m));
        }

        [Fact]
        public void Constructor_WithZeroSalePrice_CreatesOrderSuccessfully()
        {
            // Act
            var order = new Order("customer1", "model1", 0m);

            // Assert
            order.SalePrice.Should().Be(0m);
        }

        [Fact]
        public void AssignVehicle_WithValidVehicleId_UpdatesOrderCorrectly()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var vehicleId = "vehicle1";

            // Act
            order.AssignVehicle(vehicleId);

            // Assert
            order.VehicleId.Should().Be(vehicleId);
            order.Status.Should().Be(OrderStatus.Confirmed);
            order.ReservationFrom.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void AssignVehicle_WithNullVehicleId_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.AssignVehicle(null!));
        }

        [Fact]
        public void AssignVehicle_WithEmptyVehicleId_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.AssignVehicle(""));
        }

        [Fact]
        public void AssignVehicle_WithWhitespaceVehicleId_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.AssignVehicle("   "));
        }

        [Fact]
        public void AssignVehicle_WithNonPendingStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Changes status to Confirmed

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.AssignVehicle("vehicle2"));
        }

        [Fact]
        public void Confirm_WithConfirmedStatus_UpdatesStatusCorrectly()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Status becomes Confirmed

            // Act
            order.Confirm();

            // Assert
            order.Status.Should().Be(OrderStatus.Confirmed);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Confirm_WithPendingStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.Confirm());
        }

        [Fact]
        public void Confirm_WithoutVehicleId_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            // Manually set status to Confirmed without assigning vehicle
            var statusProperty = typeof(Order).GetProperty("Status");
            statusProperty?.SetValue(order, OrderStatus.Confirmed);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.Confirm());
        }

        [Fact]
        public void Complete_WithConfirmedStatus_UpdatesStatusCorrectly()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Status becomes Confirmed

            // Act
            order.Complete();

            // Assert
            order.Status.Should().Be(OrderStatus.Completed);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Complete_WithPendingStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.Complete());
        }

        [Fact]
        public void Complete_WithCompletedStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1");
            order.Complete(); // Status becomes Completed

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.Complete());
        }

        [Fact]
        public void Cancel_WithPendingStatus_UpdatesStatusCorrectly()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act
            order.Cancel();

            // Assert
            order.Status.Should().Be(OrderStatus.Cancelled);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Cancel_WithConfirmedStatus_UpdatesStatusCorrectly()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1");

            // Act
            order.Cancel();

            // Assert
            order.Status.Should().Be(OrderStatus.Cancelled);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void Cancel_WithCompletedStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1");
            order.Complete();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.Cancel());
        }

        [Fact]
        public void Cancel_WithAlreadyCancelledStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.Cancel();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.Cancel());
        }

        [Fact]
        public void SetDealer_WithValidDealerId_UpdatesDealerId()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var dealerId = "dealer2";

            // Act
            order.SetDealer(dealerId);

            // Assert
            order.DealerId.Should().Be(dealerId);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void SetDealer_WithNullDealerId_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.SetDealer(null!));
        }

        [Fact]
        public void SetDealer_WithEmptyDealerId_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.SetDealer(""));
        }

        [Fact]
        public void SetDealer_WithWhitespaceDealerId_ThrowsArgumentException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => order.SetDealer("   "));
        }

        [Fact]
        public void UpdateNote_WithValidNote_UpdatesNote()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var note = "Special requirements";

            // Act
            order.UpdateNote(note);

            // Assert
            order.Note.Should().Be(note);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UpdateNote_WithNullNote_UpdatesNoteToNull()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.UpdateNote("Some note");

            // Act
            order.UpdateNote(null);

            // Assert
            order.Note.Should().BeNull();
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UpdateNote_WithEmptyNote_UpdatesNoteToEmpty()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act
            order.UpdateNote("");

            // Assert
            order.Note.Should().Be("");
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Theory]
        [InlineData(OrderStatus.Pending)]
        public void UpdateStatus_WithValidStatus_UpdatesStatusCorrectly(OrderStatus status)
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act
            order.UpdateStatus(status);

            // Assert
            order.Status.Should().Be(status);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UpdateStatus_WithConfirmedStatus_RequiresReservedOrder()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // This makes it reserved/confirmed

            // Act
            order.UpdateStatus(OrderStatus.Confirmed);

            // Assert
            order.Status.Should().Be(OrderStatus.Confirmed);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UpdateStatus_WithCompletedStatus_RequiresConfirmedOrder()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            order.AssignVehicle("vehicle1"); // Make it confirmed

            // Act
            order.UpdateStatus(OrderStatus.Completed);

            // Assert
            order.Status.Should().Be(OrderStatus.Completed);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UpdateStatus_WithCancelledStatus_UpdatesStatusCorrectly()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act
            order.UpdateStatus(OrderStatus.Cancelled);

            // Assert
            order.Status.Should().Be(OrderStatus.Cancelled);
            order.UpdatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
        }

        [Fact]
        public void UpdateStatus_WithInvalidStatus_ThrowsInvalidOperationException()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);
            var invalidStatus = (OrderStatus)999; // Invalid enum value

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.UpdateStatus(invalidStatus));
        }

        [Fact]
        public void Order_WithSpecialCharactersInModelNumber_CreatesOrderSuccessfully()
        {
            // Arrange
            var modelNumber = "Model-2024@Special#Version";

            // Act
            var order = new Order("customer1", modelNumber, 25000m);

            // Assert
            order.ModelNumber.Should().Be(modelNumber);
        }

        [Fact]
        public void Order_WithVeryLongModelNumber_CreatesOrderSuccessfully()
        {
            // Arrange
            var longModelNumber = "Model-" + new string('X', 1000);

            // Act
            var order = new Order("customer1", longModelNumber, 25000m);

            // Assert
            order.ModelNumber.Should().Be(longModelNumber);
        }

        [Fact]
        public void Order_WithDecimalPrecisionSalePrice_CreatesOrderSuccessfully()
        {
            // Arrange
            var precisePrice = 25000.999m;

            // Act
            var order = new Order("customer1", "model1", precisePrice);

            // Assert
            order.SalePrice.Should().Be(precisePrice);
        }

        [Fact]
        public void Order_WithVeryLargeSalePrice_CreatesOrderSuccessfully()
        {
            // Arrange
            var largePrice = decimal.MaxValue;

            // Act
            var order = new Order("customer1", "model1", largePrice);

            // Assert
            order.SalePrice.Should().Be(largePrice);
        }

        [Fact]
        public void Order_Workflow_CompleteOrderLifecycle()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert - Complete workflow
            order.Status.Should().Be(OrderStatus.Pending);

            order.AssignVehicle("vehicle1");
            order.Status.Should().Be(OrderStatus.Confirmed);
            order.VehicleId.Should().Be("vehicle1");

            order.Complete();
            order.Status.Should().Be(OrderStatus.Completed);
        }

        [Fact]
        public void Order_Workflow_CancelOrderLifecycle()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert - Cancel workflow
            order.Status.Should().Be(OrderStatus.Pending);

            order.Cancel();
            order.Status.Should().Be(OrderStatus.Cancelled);
        }

        [Fact]
        public void Order_Workflow_AssignVehicleThenCancel()
        {
            // Arrange
            var order = new Order("customer1", "model1", 25000m);

            // Act & Assert - Assign then cancel
            order.AssignVehicle("vehicle1");
            order.Status.Should().Be(OrderStatus.Confirmed);

            order.Cancel();
            order.Status.Should().Be(OrderStatus.Cancelled);
        }
    }
}
