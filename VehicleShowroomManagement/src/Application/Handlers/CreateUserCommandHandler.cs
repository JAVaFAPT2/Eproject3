using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for creating a new user
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
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

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate that role exists
            var role = await _roleRepository.GetByIdAsync(request.RoleId.ToString());
            if (role == null)
            {
                throw new ArgumentException($"Role with ID {request.RoleId} does not exist");
            }

            // Check if username already exists
            var existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Username == request.Username);
            if (existingUser != null)
            {
                throw new ArgumentException($"Username '{request.Username}' already exists");
            }

            // Check if email already exists
            existingUser = await _userRepository.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (existingUser != null)
            {
                throw new ArgumentException($"Email '{request.Email}' already exists");
            }

            // Hash the password using BCrypt
            var passwordHash = _passwordService.HashPassword(request.Password);

            // Create the user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = request.RoleId.ToString(),
                IsActive = true
            };

            // Save the user
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Return the DTO
            return new UserDto
            {
                UserId = int.Parse(user.Id),
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = int.Parse(user.RoleId),
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

    }
}