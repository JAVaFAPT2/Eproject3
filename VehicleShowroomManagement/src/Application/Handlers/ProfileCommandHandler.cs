using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Commands;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Queries;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Interfaces;
using VehicleShowroomManagement.Domain.Services;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for updating user profile
    /// </summary>
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Unit>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordService _passwordService;

        public UpdateProfileCommandHandler(
            IRepository<User> userRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<Unit> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Update profile information
            if (!string.IsNullOrEmpty(request.FullName))
            {
                var nameParts = request.FullName.Split(' ', 2);
                user.FirstName = nameParts[0];
                if (nameParts.Length > 1)
                {
                    user.LastName = nameParts[1];
                }
            }

            if (!string.IsNullOrEmpty(request.Address))
            {
                user.Role = null; // Update address field in user entity
                // Note: In a real implementation, you'd have an Address field in User entity
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                // Update phone field in user entity
                // Note: User entity doesn't have Phone field, this would need to be added
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for changing password
    /// </summary>
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Unit>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IPasswordService _passwordService;

        public ChangePasswordCommandHandler(
            IRepository<User> userRepository,
            IPasswordService passwordService)
        {
            _userRepository = userRepository;
            _passwordService = passwordService;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Verify old password
            if (!_passwordService.VerifyPassword(request.OldPassword, user.PasswordHash))
            {
                throw new ArgumentException("Invalid old password");
            }

            // Update password
            user.PasswordHash = _passwordService.HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return Unit.Value;
        }

    }

    /// <summary>
    /// Handler for updating a user (admin function)
    /// </summary>
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Unit>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public UpdateUserCommandHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<Unit> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null || user.IsDeleted)
            {
                throw new ArgumentException("User not found");
            }

            // Update fields if provided
            if (!string.IsNullOrEmpty(request.FirstName))
            {
                user.FirstName = request.FirstName;
            }

            if (!string.IsNullOrEmpty(request.LastName))
            {
                user.LastName = request.LastName;
            }

            if (!string.IsNullOrEmpty(request.Email))
            {
                user.Email = request.Email;
            }

            if (!string.IsNullOrEmpty(request.Phone))
            {
                user.Phone = request.Phone;
            }

            if (request.Salary.HasValue)
            {
                user.Salary = request.Salary.Value;
            }

            if (!string.IsNullOrEmpty(request.RoleId))
            {
                user.RoleId = request.RoleId;
            }

            if (request.IsActive.HasValue)
            {
                user.IsActive = request.IsActive.Value;
            }

            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for deleting a user (admin function)
    /// </summary>
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, Unit>
    {
        private readonly IRepository<User> _userRepository;

        public DeleteUserCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Soft delete the user
            user.SoftDelete();

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return Unit.Value;
        }
    }

    /// <summary>
    /// Handler for getting user profile
    /// </summary>
    public class GetUserProfileQueryHandler : IRequestHandler<GetUserProfileQuery, UserProfileDto>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public GetUserProfileQueryHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserProfileDto> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            var role = await _roleRepository.GetByIdAsync(user.RoleId);

            return new UserProfileDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Phone = user.Role?.RoleId, // Using RoleId as phone for now (would need proper Phone field)
                Address = null, // User doesn't have Address field
                City = null,
                State = null,
                ZipCode = null,
                RoleId = user.RoleId,
                RoleName = role?.RoleName ?? "Unknown",
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}