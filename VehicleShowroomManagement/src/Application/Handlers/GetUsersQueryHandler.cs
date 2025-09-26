using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.DTOs;
using VehicleShowroomManagement.Application.Queries;

namespace VehicleShowroomManagement.Application.Handlers
{
    /// <summary>
    /// Handler for retrieving users with filtering and pagination
    /// </summary>
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
    {
        private readonly IUserQueryService _userQueryService;

        public GetUsersQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            return await _userQueryService.GetUsersAsync(
                request.SearchTerm,
                request.RoleId,
                request.IsActive,
                request.PageNumber,
                request.PageSize);
        }
    }

    /// <summary>
    /// Handler for getting a single user by ID
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
    {
        private readonly IUserQueryService _userQueryService;

        public GetUserByIdQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userQueryService.GetUserByIdAsync(request.UserId);
        }
    }
}