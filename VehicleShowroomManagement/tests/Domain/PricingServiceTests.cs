using Xunit;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Tests.Domain
{
    public class PricingServiceTests
    {
        private readonly PricingService _pricingService;

        public PricingServiceTests()
        {
            _pricingService = new PricingService();
        }

        [Theory]
        [InlineData(10000, 0.1, 1000)]
        [InlineData(25000, 0.15, 3750)]
        [InlineData(50000, 0.2, 10000)]
        [InlineData(0, 0.1, 0)]
        public void CalculateProfit_WithValidInputs_ReturnsCorrectProfit(decimal cost, decimal margin, decimal expectedProfit)
        {
            // Act
            var result = _pricingService.CalculateProfit(cost, margin);

            // Assert
            result.Should().Be(expectedProfit);
        }

        [Theory]
        [InlineData(10000, 0.1, 11000)]
        [InlineData(25000, 0.15, 28750)]
        [InlineData(50000, 0.2, 60000)]
        [InlineData(0, 0.1, 0)]
        public void CalculateSellingPrice_WithValidInputs_ReturnsCorrectPrice(decimal cost, decimal margin, decimal expectedPrice)
        {
            // Act
            var result = _pricingService.CalculateSellingPrice(cost, margin);

            // Assert
            result.Should().Be(expectedPrice);
        }

        [Theory]
        [InlineData(10000, 11000, 0.1)]
        [InlineData(25000, 28750, 0.15)]
        [InlineData(50000, 60000, 0.2)]
        public void CalculateMargin_WithValidInputs_ReturnsCorrectMargin(decimal cost, decimal sellingPrice, decimal expectedMargin)
        {
            // Act
            var result = _pricingService.CalculateMargin(cost, sellingPrice);

            // Assert
            result.Should().BeApproximately(expectedMargin, 0.001m);
        }

        [Fact]
        public void CalculateProfit_WithNegativeCost_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _pricingService.CalculateProfit(-1000, 0.1));
        }

        [Fact]
        public void CalculateProfit_WithNegativeMargin_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _pricingService.CalculateProfit(1000, -0.1));
        }

        [Fact]
        public void CalculateSellingPrice_WithNegativeCost_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _pricingService.CalculateSellingPrice(-1000, 0.1));
        }

        [Fact]
        public void CalculateMargin_WithNegativeCost_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _pricingService.CalculateMargin(-1000, 1100));
        }

        [Fact]
        public void CalculateMargin_WithNegativeSellingPrice_ThrowsArgumentException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() => _pricingService.CalculateMargin(1000, -1100));
        }
    }
}
