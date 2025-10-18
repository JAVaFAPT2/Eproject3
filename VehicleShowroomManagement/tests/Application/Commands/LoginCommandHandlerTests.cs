using Xunit;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Features.Auth.Commands.Login;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class LoginCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly Mock<IConfiguration> _mockConfiguration;
        private readonly LoginCommandHandler _handler;

        public LoginCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _mockPasswordService = new Mock<IPasswordService>();
            _mockConfiguration = new Mock<IConfiguration>();

            _handler = new LoginCommandHandler(
                _mockUserRepository.Object,
                _mockRoleRepository.Object,
                _mockPasswordService.Object,
                _mockConfiguration.Object);
        }

        [Fact]
        public async Task Handle_WithValidUsernameAndPassword_ReturnsLoginResult()
        {
            // Arrange
            var user = new User("testuser", "hashedpassword", "Test User", "test@example.com", "role1");
            var role = new Role("Admin");
            
            var users = new List<User> { user };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockRoleRepository.Setup(r => r.GetByIdAsync(user.RoleId, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(role);
            _mockPasswordService.Setup(s => s.VerifyPassword("password123", "hashedpassword"))
                              .Returns(true);

            // Mock JWT configuration
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("test-secret-key-that-is-long-enough-for-jwt");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("test-issuer");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("test-audience");
            _mockConfiguration.Setup(c => c["Jwt:ExpireHours"]).Returns("24");

            var command = new LoginCommand("testuser", "password123");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Token.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.UserId.Should().Be(user.Id);
            result.User.Username.Should().Be(user.Username);
            result.User.Email.Should().Be(user.Email);
            result.RoleName.Should().Be(role.Name);
        }

        [Fact]
        public async Task Handle_WithValidEmailAndPassword_ReturnsLoginResult()
        {
            // Arrange
            var user = new User("testuser", "hashedpassword", "Test User", "test@example.com", "role1");
            var role = new Role("Admin");
            
            var users = new List<User> { user };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockRoleRepository.Setup(r => r.GetByIdAsync(user.RoleId, It.IsAny<CancellationToken>()))
                              .ReturnsAsync(role);
            _mockPasswordService.Setup(s => s.VerifyPassword("password123", "hashedpassword"))
                              .Returns(true);

            // Mock JWT configuration
            _mockConfiguration.Setup(c => c["Jwt:Key"]).Returns("test-secret-key-that-is-long-enough-for-jwt");
            _mockConfiguration.Setup(c => c["Jwt:Issuer"]).Returns("test-issuer");
            _mockConfiguration.Setup(c => c["Jwt:Audience"]).Returns("test-audience");
            _mockConfiguration.Setup(c => c["Jwt:ExpireHours"]).Returns("24");

            var command = new LoginCommand("test@example.com", "password123");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result!.Token.Should().NotBeNullOrEmpty();
            result.RefreshToken.Should().NotBeNullOrEmpty();
            result.UserId.Should().Be(user.Id);
            result.User.Username.Should().Be(user.Username);
            result.User.Email.Should().Be(user.Email);
            result.RoleName.Should().Be(role.Name);
        }

        [Fact]
        public async Task Handle_WithInvalidUsername_ReturnsNull()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());

            var command = new LoginCommand("nonexistent", "password123");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WithInvalidPassword_ReturnsNull()
        {
            // Arrange
            var user = new User("testuser", "hashedpassword", "Test User", "test@example.com", "role1");
            var users = new List<User> { user };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockPasswordService.Setup(s => s.VerifyPassword("wrongpassword", "hashedpassword"))
                              .Returns(false);

            var command = new LoginCommand("testuser", "wrongpassword");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WithDeletedUser_ReturnsNull()
        {
            // Arrange
            var user = new User("testuser", "hashedpassword", "Test User", "test@example.com", "role1");
            user.SoftDelete(); // Mark as deleted
            var users = new List<User> { user };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>()); // Deleted users should not be returned
            _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);

            var command = new LoginCommand("testuser", "password123");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WhenRoleNotFound_ThrowsException()
        {
            // Arrange
            var user = new User("testuser", "hashedpassword", "Test User", "test@example.com", "nonexistent-role");
            var users = new List<User> { user };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockPasswordService.Setup(s => s.VerifyPassword("password123", "hashedpassword"))
                              .Returns(true);
            _mockRoleRepository.Setup(r => r.GetByIdAsync("nonexistent-role", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Role?)null);

            var command = new LoginCommand("testuser", "password123");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var command = new LoginCommand("testuser", "password123");

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyUsername_ReturnsNull()
        {
            // Arrange
            var command = new LoginCommand("", "password123");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task Handle_WithEmptyPassword_ReturnsNull()
        {
            // Arrange
            var user = new User("testuser", "hashedpassword", "Test User", "test@example.com", "role1");
            var users = new List<User> { user };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);
            _mockUserRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(users);

            var command = new LoginCommand("testuser", "");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().BeNull();
        }
    }
}
