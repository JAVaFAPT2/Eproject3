using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.Profile.Queries.GetProfile
{
    /// <summary>
    /// Handler for get profile query
    /// </summary>
    public class GetProfileQueryHandler(IRepository<User> userRepository, IRepository<Role> roleRepository) : IRequestHandler<GetProfileQuery, UserProfileDto?>
    {
        public async Task<UserProfileDto?> Handle(GetProfileQuery request, CancellationToken cancellationToken)
        {
            var users = await userRepository.FindAsync(u => u.Id == request.UserId && u.DeletedAt == null, cancellationToken);
            var user = users.FirstOrDefault();
            if (user == null)
            {
                return null;
            }

            var role = await roleRepository.GetByIdAsync(user.RoleId, cancellationToken);

            return new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Name = user.Name ?? string.Empty,
                Phone = user.Phone,
                Address = user.Address,
                Role = role?.Name ?? "Unknown",
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };
        }
    }
}
