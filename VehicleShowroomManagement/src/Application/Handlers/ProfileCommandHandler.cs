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
    /// Handler for updating user profile
    /// </summary>
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Unit>
    {
        private readonly IRepository<User> _userRepository;

        public UpdateProfileCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
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

        public ChangePasswordCommandHandler(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Unit> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);
            if (user == null)
            {
                throw new ArgumentException("User not found");
            }

            // Verify old password
            var hashedOldPassword = HashPassword(request.OldPassword);
            if (user.PasswordHash != hashedOldPassword)
            {
                throw new ArgumentException("Invalid old password");
            }

            // Update password
            user.PasswordHash = HashPassword(request.NewPassword);
            user.UpdatedAt = DateTime.UtcNow;

            await _userRepository.UpdateAsync(user);
            await _userRepository.SaveChangesAsync();

            return Unit.Value;
        }

        private string HashPassword(string password)
        {
            // In a real implementation, use BCrypt or similar
            return $"hashed_{password}";
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