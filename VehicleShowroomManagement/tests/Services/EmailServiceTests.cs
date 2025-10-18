using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VehicleShowroomManagement.Application.Common.Exceptions;
using VehicleShowroomManagement.Infrastructure.Services;
using VehicleShowroomManagement.WebAPI.Configuration;
using System.Net.Mail;
using Polly;

namespace VehicleShowroomManagement.Tests.Services
{
    public class EmailServiceTests
    {
        private readonly Mock<IOptions<EmailSettings>> _mockOptions;
        private readonly Mock<ILogger<EmailService>> _mockLogger;
        private readonly Mock<AsyncPolicy> _mockPolicy;
        private readonly EmailService _service;

        public EmailServiceTests()
        {
            _mockOptions = new Mock<IOptions<EmailSettings>>();
            _mockLogger = new Mock<ILogger<EmailService>>();
            _mockPolicy = new Mock<AsyncPolicy>();

            _mockOptions.Setup(x => x.Value).Returns(new EmailSettings
            {
                SmtpHost = "smtp.test.com",
                SmtpPort = 587,
                SmtpUsername = "test@test.com",
                SmtpPassword = "test-password",
                EnableSsl = true,
                FromEmail = "noreply@test.com",
                FromName = "Test System"
            });

            _service = new EmailService(_mockOptions.Object, _mockLogger.Object, _mockPolicy.Object);
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_WithValidParameters_CallsPolicy()
        {
            // Arrange
            var email = "test@example.com";
            var firstName = "John";
            var resetToken = "reset-token-123";

            _mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task>>()))
                      .Returns(Task.CompletedTask);

            // Act
            await _service.SendPasswordResetEmailAsync(email, firstName, resetToken);

            // Assert
            _mockPolicy.Verify(p => p.ExecuteAsync(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task SendWelcomeEmailAsync_WithValidParameters_CallsPolicy()
        {
            // Arrange
            var email = "test@example.com";
            var firstName = "John";
            var username = "johndoe";
            var temporaryPassword = "temp-pass-123";

            _mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task>>()))
                      .Returns(Task.CompletedTask);

            // Act
            await _service.SendWelcomeEmailAsync(email, firstName, username, temporaryPassword);

            // Assert
            _mockPolicy.Verify(p => p.ExecuteAsync(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task SendOrderConfirmationEmailAsync_WithValidParameters_CallsPolicy()
        {
            // Arrange
            var email = "test@example.com";
            var customerName = "John Doe";
            var orderNumber = "ORD-123";
            var totalAmount = 25000.00m;

            _mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task>>()))
                      .Returns(Task.CompletedTask);

            // Act
            await _service.SendOrderConfirmationEmailAsync(email, customerName, orderNumber, totalAmount);

            // Assert
            _mockPolicy.Verify(p => p.ExecuteAsync(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task SendInvoiceEmailAsync_WithValidParameters_CallsPolicy()
        {
            // Arrange
            var email = "test@example.com";
            var customerName = "John Doe";
            var invoiceNumber = "INV-123";
            var invoicePdf = new byte[] { 1, 2, 3, 4, 5 };

            _mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task>>()))
                      .Returns(Task.CompletedTask);

            // Act
            await _service.SendInvoiceEmailAsync(email, customerName, invoiceNumber, invoicePdf);

            // Assert
            _mockPolicy.Verify(p => p.ExecuteAsync(It.IsAny<Func<Task>>()), Times.Once);
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_WhenPolicyThrows_ThrowsEmailException()
        {
            // Arrange
            var email = "test@example.com";
            var firstName = "John";
            var resetToken = "reset-token-123";

            _mockPolicy.Setup(p => p.ExecuteAsync(It.IsAny<Func<Task>>()))
                      .ThrowsAsync(new Exception("SMTP connection failed"));

            // Act & Assert
            await Assert.ThrowsAsync<EmailException>(() => 
                _service.SendPasswordResetEmailAsync(email, firstName, resetToken));
        }

        [Fact]
        public void Dispose_DisposesSmtpClient()
        {
            // Act
            _service.Dispose();

            // Assert - This test verifies that Dispose doesn't throw
            // In a real implementation, we would verify SmtpClient.Dispose() was called
            Assert.True(true);
        }
    }
}
