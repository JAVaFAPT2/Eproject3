using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Auth.Commands.Register;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class RegisterCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly RegisterCommandHandler _handler;

        public RegisterCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _mockPasswordService = new Mock<IPasswordService>();

            _handler = new RegisterCommandHandler(
                _mockUserRepository.Object,
                _mockRoleRepository.Object,
                _mockPasswordService.Object);
        }

        [Fact]
        public async Task Handle_WithValidData_CreatesUserAndReturnsId()
        {
            // Arrange
            var customerRole = new Role("Customer");
            var roles = new List<Role> { customerRole };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(roles);
            _mockPasswordService.Setup(s => s.HashPassword("password123"))
                              .Returns("hashedpassword");
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync("new-user-id");

            var command = new RegisterCommand("newuser", "password123", "newuser@example.com");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("new-user-id");
            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => 
                u.Username == "newuser" &&
                u.Email == "newuser@example.com" &&
                u.PasswordHash == "hashedpassword" &&
                u.RoleId == customerRole.Id
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithExistingUsername_ThrowsException()
        {
            // Arrange
            var existingUser = new User("existinguser", "hash", "Existing User", "existing@example.com", "role1");
            var existingUsers = new List<User> { existingUser };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(existingUsers);

            var command = new RegisterCommand("existinguser", "password123", "newuser@example.com");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithExistingEmail_ThrowsException()
        {
            // Arrange
            var existingUser = new User("newuser", "hash", "Existing User", "existing@example.com", "role1");
            var existingUsers = new List<User> { existingUser };

            _mockUserRepository.SetupSequence(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>()) // Username check passes
                              .ReturnsAsync(existingUsers); // Email check fails

            var command = new RegisterCommand("newuser", "password123", "existing@example.com");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenCustomerRoleNotFound_CreatesRoleAndUser()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Role>());
            _mockRoleRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Role>());
            _mockRoleRepository.Setup(r => r.AddAsync(It.IsAny<Role>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync("new-role-id");
            _mockPasswordService.Setup(s => s.HashPassword("password123"))
                              .Returns("hashedpassword");
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync("new-user-id");

            var command = new RegisterCommand("newuser", "password123", "newuser@example.com");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("new-user-id");
            _mockRoleRepository.Verify(r => r.AddAsync(It.Is<Role>(r => r.Name == "Customer"), It.IsAny<CancellationToken>()), Times.Once);
            _mockUserRepository.Verify(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithInvalidEmail_ThrowsException()
        {
            // Arrange
            var command = new RegisterCommand("newuser", "password123", "invalid-email");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithWeakPassword_ThrowsException()
        {
            // Arrange
            var command = new RegisterCommand("newuser", "123", "newuser@example.com"); // Too short

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyUsername_ThrowsException()
        {
            // Arrange
            var command = new RegisterCommand("", "password123", "newuser@example.com");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var command = new RegisterCommand("newuser", "password123", "newuser@example.com");

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithCaseInsensitiveRoleLookup_CreatesUser()
        {
            // Arrange
            var customerRole = new Role("customer"); // lowercase
            var roles = new List<Role> { customerRole };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<Role>());
            _mockRoleRepository.Setup(r => r.GetAllAsync(It.IsAny<CancellationToken>()))
                              .ReturnsAsync(roles);
            _mockPasswordService.Setup(s => s.HashPassword("password123"))
                              .Returns("hashedpassword");
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync("new-user-id");

            var command = new RegisterCommand("newuser", "password123", "newuser@example.com");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("new-user-id");
            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => u.RoleId == customerRole.Id), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}