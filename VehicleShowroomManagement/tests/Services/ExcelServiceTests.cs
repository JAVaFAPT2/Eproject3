using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using VehicleShowroomManagement.Application.Common.Exceptions;
using VehicleShowroomManagement.Infrastructure.Services;

namespace VehicleShowroomManagement.Tests.Services
{
    public class ExcelServiceTests
    {
        private readonly Mock<ILogger<ExcelService>> _mockLogger;
        private readonly ExcelService _service;

        public ExcelServiceTests()
        {
            _mockLogger = new Mock<ILogger<ExcelService>>();
            _service = new ExcelService(_mockLogger.Object);
        }

        [Fact]
        public async Task GenerateExcelAsync_WithValidData_ReturnsByteArray()
        {
            // Arrange
            var testData = new List<TestModel>
            {
                new TestModel { Id = 1, Name = "Test 1", Value = 100.50m },
                new TestModel { Id = 2, Name = "Test 2", Value = 200.75m }
            };
            var worksheetName = "Test Sheet";
            var fileName = "test.xlsx";

            // Act
            var result = await _service.GenerateExcelAsync(testData, worksheetName, fileName);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GenerateExcelAsync_WithEmptyData_ReturnsByteArray()
        {
            // Arrange
            var testData = new List<TestModel>();
            var worksheetName = "Empty Sheet";
            var fileName = "empty.xlsx";

            // Act
            var result = await _service.GenerateExcelAsync(testData, worksheetName, fileName);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GenerateExcelWithMultipleSheetsAsync_WithValidData_ReturnsByteArray()
        {
            // Arrange
            var worksheets = new Dictionary<string, object>
            {
                ["Sheet1"] = new List<TestModel>
                {
                    new TestModel { Id = 1, Name = "Test 1", Value = 100.50m }
                },
                ["Sheet2"] = new List<TestModel>
                {
                    new TestModel { Id = 2, Name = "Test 2", Value = 200.75m }
                }
            };
            var fileName = "multi-sheet.xlsx";

            // Act
            var result = await _service.GenerateExcelWithMultipleSheetsAsync(worksheets, fileName);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Length.Should().BeGreaterThan(0);
        }

        [Fact]
        public async Task GenerateExcelWithMultipleSheetsAsync_WithNonEnumerableData_ReturnsByteArray()
        {
            // Arrange
            var worksheets = new Dictionary<string, object>
            {
                ["Sheet1"] = "Single value",
                ["Sheet2"] = 12345
            };
            var fileName = "non-enumerable.xlsx";

            // Act
            var result = await _service.GenerateExcelWithMultipleSheetsAsync(worksheets, fileName);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
        }

        [Fact]
        public async Task GenerateExcelAsync_WithNullData_ThrowsExcelGenerationException()
        {
            // Arrange
            List<TestModel>? testData = null;
            var worksheetName = "Test Sheet";
            var fileName = "test.xlsx";

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => 
                _service.GenerateExcelAsync(testData!, worksheetName, fileName));
        }

        public class TestModel
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public decimal Value { get; set; }
        }
    }
}
