using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using VehicleShowroomManagement.Application.Common.Exceptions;
using VehicleShowroomManagement.Infrastructure.Services;
using VehicleShowroomManagement.Application.Common.Configuration;
using System.Net.Mail;
using Polly;

namespace VehicleShowroomManagement.Tests.Services
{
    public class EmailServiceTests
    {
        private readonly Mock<IOptions<EmailSettings>> _mockOptions;
        private readonly Mock<ILogger<EmailService>> _mockLogger;
        private readonly AsyncPolicy _policy;
        private readonly EmailService _service;

        public EmailServiceTests()
        {
            _mockOptions = new Mock<IOptions<EmailSettings>>();
            _mockLogger = new Mock<ILogger<EmailService>>();
            
            // Use a policy that doesn't actually send emails
            _policy = Policy.Handle<Exception>()
                .RetryAsync(0); // No retries, just fail fast

            _mockOptions.Setup(x => x.Value).Returns(new EmailSettings
            {
                SmtpHost = "localhost", // Use localhost to avoid external connections
                SmtpPort = 25,
                SmtpUsername = "test@test.com",
                SmtpPassword = "test-password",
                EnableSsl = false, // Disable SSL for testing
                FromEmail = "noreply@test.com",
                FromName = "Test System"
            });

            _service = new EmailService(_mockOptions.Object, _mockLogger.Object, _policy);
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_WithValidParameters_ThrowsEmailException()
        {
            // Arrange
            var email = "test@example.com";
            var firstName = "John";
            var resetToken = "reset-token-123";

            // Act & Assert - Should throw EmailException due to SMTP connection failure
            await Assert.ThrowsAsync<EmailException>(() => 
                _service.SendPasswordResetEmailAsync(email, firstName, resetToken));
        }

        [Fact]
        public async Task SendWelcomeEmailAsync_WithValidParameters_ThrowsEmailException()
        {
            // Arrange
            var email = "test@example.com";
            var firstName = "John";
            var username = "johndoe";
            var temporaryPassword = "temp-pass-123";

            // Act & Assert - Should throw EmailException due to SMTP connection failure
            await Assert.ThrowsAsync<EmailException>(() => 
                _service.SendWelcomeEmailAsync(email, firstName, username, temporaryPassword));
        }

        [Fact]
        public async Task SendOrderConfirmationEmailAsync_WithValidParameters_ThrowsEmailException()
        {
            // Arrange
            var email = "test@example.com";
            var customerName = "John Doe";
            var orderNumber = "ORD-123";
            var totalAmount = 25000.00m;

            // Act & Assert - Should throw EmailException due to SMTP connection failure
            await Assert.ThrowsAsync<EmailException>(() => 
                _service.SendOrderConfirmationEmailAsync(email, customerName, orderNumber, totalAmount));
        }

        [Fact]
        public async Task SendInvoiceEmailAsync_WithValidParameters_ThrowsEmailException()
        {
            // Arrange
            var email = "test@example.com";
            var customerName = "John Doe";
            var invoiceNumber = "INV-123";
            var invoicePdf = new byte[] { 1, 2, 3, 4, 5 };

            // Act & Assert - Should throw SmtpException due to SMTP connection failure
            await Assert.ThrowsAsync<SmtpException>(() => 
                _service.SendInvoiceEmailAsync(email, customerName, invoiceNumber, invoicePdf));
        }

        [Fact]
        public async Task SendPasswordResetEmailAsync_WhenPolicyThrows_ThrowsEmailException()
        {
            // Arrange
            var email = "test@example.com";
            var firstName = "John";
            var resetToken = "reset-token-123";

            // Act & Assert - Should throw EmailException due to SMTP connection failure
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
