using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// Handler for getting user by ID
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<Role> _roleRepository;

        public GetUserByIdQueryHandler(IRepository<User> userRepository, IRepository<Role> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId);

            if (user == null || user.DeletedAt != null)
                return null;

            // Get role name
            var role = await _roleRepository.GetByIdAsync(user.RoleId);

            return new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name,
                Phone = user.Phone,
                Address = user.Address,
                RoleId = user.RoleId,
                Role = role?.Name ?? "Unknown",
                Status = user.Status,
                HireDate = user.HireDate,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
