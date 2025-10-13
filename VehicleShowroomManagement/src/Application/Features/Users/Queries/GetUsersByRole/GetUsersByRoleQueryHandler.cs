using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Users.Queries.GetUsersByRole
{
    /// <summary>
    /// Handler for getting users by role name
    /// </summary>
    public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, List<UserDto>>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public GetUsersByRoleQueryHandler(
            IRepository<User> userRepository,
            IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<List<UserDto>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
        {
            // Find role by name
            var roles = await _roleRepository.FindAsync(r => r.Name == request.RoleName);
            var role = roles.FirstOrDefault();

            if (role == null)
            {
                throw new InvalidOperationException($"Role '{request.RoleName}' not found");
            }

            // Get users with this role (excluding deleted users)
            var users = await _userRepository.FindAsync(u => u.RoleId == role.Id && u.DeletedAt == null);

            // Map to DTOs
            return users.Select(user => new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                RoleId = user.RoleId,
                Role = request.RoleName, // Use the role name from the query
                Status = user.Status,
                HireDate = user.HireDate,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            }).ToList();
        }
    }
}
