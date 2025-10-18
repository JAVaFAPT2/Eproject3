using Xunit;
using FluentAssertions;
using VehicleShowroomManagement.Infrastructure.Services;

namespace VehicleShowroomManagement.Tests.Domain
{
    public class PasswordServiceTests
    {
        private readonly PasswordService _passwordService;

        public PasswordServiceTests()
        {
            _passwordService = new PasswordService();
        }

        [Fact]
        public void HashPassword_WithValidPassword_ReturnsHashedPassword()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var hashedPassword = _passwordService.HashPassword(password);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(password);
            hashedPassword.Length.Should().BeGreaterThan(50); // BCrypt hashes are typically 60 characters
        }

        [Fact]
        public void HashPassword_WithSamePassword_ReturnsDifferentHashes()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var hash1 = _passwordService.HashPassword(password);
            var hash2 = _passwordService.HashPassword(password);

            // Assert
            hash1.Should().NotBe(hash2); // BCrypt generates different salts
        }

        [Fact]
        public void VerifyPassword_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashedPassword = _passwordService.HashPassword(password);

            // Act
            var result = _passwordService.VerifyPassword(password, hashedPassword);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_WithIncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var password = "TestPassword123!";
            var wrongPassword = "WrongPassword123!";
            var hashedPassword = _passwordService.HashPassword(password);

            // Act
            var result = _passwordService.VerifyPassword(wrongPassword, hashedPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithEmptyPassword_ReturnsFalse()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashedPassword = _passwordService.HashPassword(password);

            // Act
            var result = _passwordService.VerifyPassword(string.Empty, hashedPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithNullPassword_ReturnsFalse()
        {
            // Arrange
            var password = "TestPassword123!";
            var hashedPassword = _passwordService.HashPassword(password);

            // Act
            var result = _passwordService.VerifyPassword(null!, hashedPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithEmptyHash_ReturnsFalse()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var result = _passwordService.VerifyPassword(password, string.Empty);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void VerifyPassword_WithNullHash_ReturnsFalse()
        {
            // Arrange
            var password = "TestPassword123!";

            // Act
            var result = _passwordService.VerifyPassword(password, null!);

            // Assert
            result.Should().BeFalse();
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData("123")]
        [InlineData("password")]
        [InlineData("PASSWORD")]
        [InlineData("Password")]
        public void HashPassword_WithWeakPasswords_StillHashes(string weakPassword)
        {
            // Act
            var hashedPassword = _passwordService.HashPassword(weakPassword);

            // Assert
            hashedPassword.Should().NotBeNullOrEmpty();
            hashedPassword.Should().NotBe(weakPassword);
        }
    }
}
