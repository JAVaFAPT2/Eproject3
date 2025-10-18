using Xunit;
using FluentAssertions;
using Moq;
using VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Tests.Application.Commands
{
    public class CreateUserCommandHandlerTests
    {
        private readonly Mock<IRepository<User>> _mockUserRepository;
        private readonly Mock<IRepository<Role>> _mockRoleRepository;
        private readonly Mock<IPasswordService> _mockPasswordService;
        private readonly CreateUserCommandHandler _handler;

        public CreateUserCommandHandlerTests()
        {
            _mockUserRepository = new Mock<IRepository<User>>();
            _mockRoleRepository = new Mock<IRepository<Role>>();
            _mockPasswordService = new Mock<IPasswordService>();

            _handler = new CreateUserCommandHandler(
                _mockUserRepository.Object,
                _mockRoleRepository.Object,
                _mockPasswordService.Object);
        }

        [Fact]
        public async Task Handle_WithValidDataAndRoleId_CreatesUserAndReturnsId()
        {
            // Arrange
            var role = new Role("Admin");
            var roles = new List<Role> { role };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockRoleRepository.Setup(r => r.GetByIdAsync("role1", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(role);
            _mockPasswordService.Setup(s => s.HashPassword("password123"))
                              .Returns("hashedpassword");
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync("new-user-id");

            var command = new CreateUserCommand("newuser", "newuser@example.com", "password123", "New User", "role1", null, null, DateTime.Now);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("new-user-id");
            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => 
                u.Username == "newuser" &&
                u.Email == "newuser@example.com" &&
                u.Name == "New User" &&
                u.PasswordHash == "hashedpassword" &&
                u.RoleId == "role1" &&
                u.HireDate == command.HireDate
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidDataAndHireDate_CreatesEmployeeUser()
        {
            // Arrange
            var employeeRole = new Role("Employee");
            var roles = new List<Role> { employeeRole };

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockRoleRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<Role, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(roles);
            _mockPasswordService.Setup(s => s.HashPassword("password123"))
                              .Returns("hashedpassword");
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync("new-user-id");

            var command = new CreateUserCommand("newuser", "newuser@example.com", "password123", "New User", null, null, null, DateTime.Now);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("new-user-id");
            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => 
                u.RoleId == employeeRole.Id &&
                u.HireDate == command.HireDate
            ), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_WithValidDataWithoutHireDate_CreatesCustomerUser()
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

            var command = new CreateUserCommand("newuser", "newuser@example.com", "password123", "New User");

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("new-user-id");
            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => 
                u.RoleId == customerRole.Id &&
                u.HireDate == null
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

            var command = new CreateUserCommand("existinguser", "newuser@example.com", "password123", "New User");

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

            var command = new CreateUserCommand("newuser", "existing@example.com", "password123", "New User");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithInvalidRoleId_ThrowsException()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockRoleRepository.Setup(r => r.GetByIdAsync("invalid-role", It.IsAny<CancellationToken>()))
                              .ReturnsAsync((Role?)null);

            var command = new CreateUserCommand("newuser", "newuser@example.com", "password123", "New User", "invalid-role");

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithInvalidEmail_ThrowsException()
        {
            // Arrange
            var command = new CreateUserCommand("newuser", "invalid-email", "password123", "New User");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithWeakPassword_ThrowsException()
        {
            // Arrange
            var command = new CreateUserCommand("newuser", "newuser@example.com", "123", "New User"); // Too short

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyUsername_ThrowsException()
        {
            // Arrange
            var command = new CreateUserCommand("", "newuser@example.com", "password123", "New User");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithEmptyName_ThrowsException()
        {
            // Arrange
            var command = new CreateUserCommand("newuser", "newuser@example.com", "password123", "");

            // Act & Assert
            await Assert.ThrowsAsync<ArgumentException>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WhenRepositoryThrows_PropagatesException()
        {
            // Arrange
            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ThrowsAsync(new Exception("Database error"));

            var command = new CreateUserCommand("newuser", "newuser@example.com", "password123", "New User");

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(command, CancellationToken.None));
        }

        [Fact]
        public async Task Handle_WithBothRoleIdAndHireDate_UsesRoleId()
        {
            // Arrange
            var adminRole = new Role("Admin");
            var employeeRole = new Role("Employee");

            _mockUserRepository.Setup(r => r.FindAsync(It.IsAny<System.Linq.Expressions.Expression<Func<User, bool>>>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync(new List<User>());
            _mockRoleRepository.Setup(r => r.GetByIdAsync("admin-role", It.IsAny<CancellationToken>()))
                              .ReturnsAsync(adminRole);
            _mockPasswordService.Setup(s => s.HashPassword("password123"))
                              .Returns("hashedpassword");
            _mockUserRepository.Setup(r => r.AddAsync(It.IsAny<User>(), It.IsAny<CancellationToken>()))
                              .ReturnsAsync("new-user-id");

            var command = new CreateUserCommand("newuser", "newuser@example.com", "password123", "New User", "admin-role", null, null, DateTime.Now);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.Should().Be("new-user-id");
            _mockUserRepository.Verify(r => r.AddAsync(It.Is<User>(u => u.RoleId == "admin-role"), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}