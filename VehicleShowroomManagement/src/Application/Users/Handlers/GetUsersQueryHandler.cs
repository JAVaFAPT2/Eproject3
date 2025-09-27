using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using VehicleShowroomManagement.Application.Common.DTOs;
using VehicleShowroomManagement.Application.Users.Queries;

namespace VehicleShowroomManagement.Application.Users.Handlers
{
    /// <summary>
    /// Handler for retrieving employees with filtering and pagination
    /// </summary>
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, IEnumerable<EmployeeDto>>
    {
        private readonly IUserQueryService _userQueryService;

        public GetUsersQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public async Task<IEnumerable<EmployeeDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
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
    /// Handler for getting a single employee by ID
    /// </summary>
    public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, EmployeeDto?>
    {
        private readonly IUserQueryService _userQueryService;

        public GetUserByIdQueryHandler(IUserQueryService userQueryService)
        {
            _userQueryService = userQueryService;
        }

        public async Task<EmployeeDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userQueryService.GetUserByIdAsync(request.UserId);
        }
    }
}
