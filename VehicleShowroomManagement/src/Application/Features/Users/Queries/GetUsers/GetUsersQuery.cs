using VehicleShowroomManagement.Application.Features.Users.Queries.GetUserById;

namespace VehicleShowroomManagement.Application.Features.Users.Queries.GetUsers
{
    public record GetUsersQuery(string? RoleName, string? SearchTerm, int PageNumber = 1, int PageSize = 10)
        : IRequest<GetUsersResult>;

    public class GetUsersResult
    {
        public List<UserDto> Items { get; set; } = new();
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
    }
}


