using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VehicleShowroomManagement.Application.Common.Exceptions;
using VehicleShowroomManagement.Infrastructure.Services;
using VehicleShowroomManagement.Application.Common.Configuration;
using Polly;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace VehicleShowroomManagement.Tests.Services
{
    public class CloudinaryServiceTests
    {
        private readonly Mock<IOptions<CloudinarySettings>> _mockOptions;
        private readonly Mock<ILogger<CloudinaryService>> _mockLogger;
        private readonly Mock<Cloudinary> _mockCloudinary;
        private readonly Mock<AsyncPolicy> _mockPolicy;
        private readonly CloudinaryService _service;

        public CloudinaryServiceTests()
        {
            _mockOptions = new Mock<IOptions<CloudinarySettings>>();
            _mockLogger = new Mock<ILogger<CloudinaryService>>();
            _mockCloudinary = new Mock<Cloudinary>(Mock.Of<Account>());
            _mockPolicy = new Mock<AsyncPolicy>();

            _mockOptions.Setup(x => x.Value).Returns(new CloudinarySettings
            {
                CloudName = "test-cloud",
                ApiKey = "test-key",
                ApiSecret = "test-secret"
            });

            _service = new CloudinaryService(_mockOptions.Object, _mockLogger.Object, _mockPolicy.Object);
        }

        [Fact]
        public async Task UploadImageAsync_WithValidFile_ReturnsUploadResult()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1024);
            mockFile.Setup(f => f.FileName).Returns("test.jpg");
            mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

            var expectedResult = new ImageUploadResult
            {
                PublicId = "test-public-id",
                SecureUrl = new Uri("https://test.com/image.jpg"),
                Url = new Uri("http://test.com/image.jpg"),
                Width = 800,
                Height = 600,
                Format = "jpg",
                Bytes = 1024
            };

            _mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task<ImageUploadResult>>>()))
                      .ReturnsAsync(expectedResult);

            // Act
            var result = await _service.UploadImageAsync(mockFile.Object, "test-folder");

            // Assert
            result.Should().NotBeNull();
            result.PublicId.Should().Be("test-public-id");
            result.SecureUrl.Should().Be("https://test.com/image.jpg");
            result.Width.Should().Be(800);
            result.Height.Should().Be(600);
        }

        [Fact]
        public async Task UploadImageAsync_WithEmptyFile_ThrowsCloudinaryException()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(0);

            // Act & Assert
            await Assert.ThrowsAsync<CloudinaryException>(() => 
                _service.UploadImageAsync(mockFile.Object, "test-folder"));
        }

        [Fact]
        public async Task DeleteImageAsync_WithValidPublicId_ReturnsTrue()
        {
            // Arrange
            var publicId = "test-public-id";
            var mockDeletionResult = new DeletionResult { Result = "ok" };

            _mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task<bool>>>()))
                      .ReturnsAsync(true);

            // Act
            var result = await _service.DeleteImageAsync(publicId);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void GetOptimizedImageUrl_WithValidParameters_ReturnsUrl()
        {
            // Arrange
            var publicId = "test-public-id";
            var width = 300;
            var height = 200;

            // Act
            var result = _service.GetOptimizedImageUrl(publicId, width, height);

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().Contain(publicId);
        }

        [Fact]
        public async Task UploadImageAsync_WhenPolicyThrows_ThrowsCloudinaryException()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            mockFile.Setup(f => f.Length).Returns(1024);
            mockFile.Setup(f => f.FileName).Returns("test.jpg");
            mockFile.Setup(f => f.OpenReadStream()).Returns(new MemoryStream());

            _mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task<ImageUploadResult>>>()))
                      .ThrowsAsync(new Exception("Network error"));

            // Act & Assert
            await Assert.ThrowsAsync<CloudinaryException>(() => 
                _service.UploadImageAsync(mockFile.Object, "test-folder"));
        }
    }
}
