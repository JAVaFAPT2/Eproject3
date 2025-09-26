using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for creating a new user
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public CreateUserCommandHandler(IRepository<User> userRepository, IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            // Validate that role exists
            var role = await _roleRepository.GetByIdAsync(request.RoleId);
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

            // Hash the password (in a real implementation, use proper password hashing)
            var passwordHash = HashPassword(request.Password);

            // Create the user
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                PasswordHash = passwordHash,
                FirstName = request.FirstName,
                LastName = request.LastName,
                RoleId = request.RoleId,
                IsActive = true
            };

            // Save the user
            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            // Return the DTO
            return new UserDto
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                RoleId = user.RoleId,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }

        private string HashPassword(string password)
        {
            // In a real implementation, use BCrypt or similar
            return $"hashed_{password}";
        }
    }
}