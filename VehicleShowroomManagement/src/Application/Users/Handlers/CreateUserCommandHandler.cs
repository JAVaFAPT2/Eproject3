using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Users.Commands;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Users.Handlers
{
    /// <summary>
    /// Handler for creating a new user
    /// </summary>
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, string>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;
        private readonly IUserDomainService _userDomainService;

        public CreateUserCommandHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository,
            IUserDomainService userDomainService)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _userDomainService = userDomainService;
        }

        public async Task<string> Handle(CreateUserCommand request, CancellationToken cancellationToken)
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

            // Use domain service to create user
            var user = await _userDomainService.CreateUserAsync(
                request.Username,
                request.Email,
                request.Password,
                request.FirstName,
                request.LastName,
                request.RoleId.ToString());

            await _userRepository.AddAsync(user);
            await _userRepository.SaveChangesAsync();

            return user.Id;
        }

    }
}
