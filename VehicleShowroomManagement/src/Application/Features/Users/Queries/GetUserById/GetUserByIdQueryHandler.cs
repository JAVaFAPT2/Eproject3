namespace VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById
{
    /// <summary>
    /// Handler for getting user by ID
    /// </summary>
    public class GetUserByIdQueryHandler(IRepository<User> userRepository, IRepository<Role> roleRepository) : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

            if (user is not { DeletedAt: null })
                return null;

            // Get role name
            var role = await roleRepository.GetByIdAsync(user.RoleId, cancellationToken);

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
