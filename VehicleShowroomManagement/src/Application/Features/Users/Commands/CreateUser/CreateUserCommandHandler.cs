using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Features.Users.Commands.CreateUser
{
    /// <summary>
    /// Handler for creating a new user (unified schema)
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordService _passwordService;

        public CreateUserCommandHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordService = passwordService;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Check if username already exists
            var existingUsers = await _userRepository.FindAsync(u => u.Username == request.Username && u.DeletedAt == null);
            if (existingUsers.Any())
            {
                throw new InvalidOperationException("Username already exists");
            }

            // Check if email already exists
            existingUsers = await _userRepository.FindAsync(u => u.Email == request.Email && u.DeletedAt == null);
            if (existingUsers.Any())
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Verify role exists
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
            if (role == null)
            {
                throw new InvalidOperationException("Role not found");
            }

            // Hash password
            var passwordHash = _passwordService.HashPassword(request.Password);

            // Create user
            var user = new User(
                request.Username,
                passwordHash,
                request.Name,
                request.Email,
                request.RoleId,
                request.Phone,
                request.Address,
                request.HireDate);

            // Add to repository
            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}
