using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Features.Auth.Commands.Register
{
    /// <summary>
    /// Handler for public user registration
    /// </summary>
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IPasswordService _passwordService;

        public RegisterCommandHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _passwordService = passwordService;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
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

            // Find Customer role by name
            var customerRoles = await _roleRepository.FindAsync(r => r.Name == "Customer");
            var customerRole = customerRoles.FirstOrDefault();
            
            if (customerRole == null)
            {
                throw new InvalidOperationException("Customer role not found in the system");
            }

            // Hash password
            var passwordHash = _passwordService.HashPassword(request.Password);

            // Create user with Customer role
            var user = new User(
                request.Username,
                passwordHash,
                request.Name,
                request.Email,
                customerRole.Id,
                request.Phone,
                request.Address,
                hireDate: null);

            // Add to repository
            await _userRepository.AddAsync(user);

            return user.Id;
        }
    }
}

