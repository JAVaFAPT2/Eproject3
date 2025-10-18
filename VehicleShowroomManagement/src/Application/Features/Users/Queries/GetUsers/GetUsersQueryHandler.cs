using VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById;
namespace VehicleShowroomManagement.Application.Features.Users.Queries.GetUsers
{
    public class GetUsersQueryHandler(
        IRepository<User> userRepository,
        IRepository<Role> roleRepository) : IRequestHandler<GetUsersQuery, GetUsersResult>
    {
        public async Task<GetUsersResult> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            IEnumerable<User> users;

            if (!string.IsNullOrWhiteSpace(request.RoleName))
            {
                var roles = await roleRepository.FindAsync(r => r.Name == request.RoleName, cancellationToken);
                var role = roles.FirstOrDefault();
                if (role == null)
                {
                    return new GetUsersResult { Items = new List<UserDto>(), TotalCount = 0, PageNumber = request.PageNumber, PageSize = request.PageSize, TotalPages = 0 };
                }
                users = await userRepository.FindAsync(u => u.RoleId == role.Id && u.DeletedAt == null, cancellationToken);
            }
            else
            {
                users = await userRepository.FindAsync(u => u.DeletedAt == null, cancellationToken);
            }

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim().ToLowerInvariant();
                users = users.Where(u =>
                    (!string.IsNullOrWhiteSpace(u.Phone) && u.Phone.ToLowerInvariant().Contains(term)) ||
                    (!string.IsNullOrWhiteSpace(u.Email) && u.Email.ToLowerInvariant().Contains(term)));
            }

            var usersList = users.ToList();
            var total = usersList.Count;
            var totalPages = (int)Math.Ceiling(total / (double)request.PageSize);
            var page = usersList
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            var result = new GetUsersResult
            {
                Items = [.. page.Select(user => new UserDto
                {
                    Id = user.Id,
                    Username = user.Username,
                    Name = user.Name,
                    Email = user.Email,
                    Phone = user.Phone,
                    Address = user.Address,
                    RoleId = user.RoleId,
                    Role = string.Empty,
                    Status = user.Status,
                    HireDate = user.HireDate,
                    IsActive = user.IsActive,
                    CreatedAt = user.CreatedAt,
                    UpdatedAt = user.UpdatedAt
                })],
                TotalCount = total,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = totalPages
            };

            return result;
        }
    }
}


