using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Queries.GetVehicles
{
    /// <summary>
    /// Handler for get vehicles query - simplified implementation
    /// </summary>
    public class GetVehiclesQueryHandler : IRequestHandler<GetVehiclesQuery, GetVehiclesResult>
    {
        public async Task<GetVehiclesResult> Handle(GetVehiclesQuery request, CancellationToken cancellationToken)
        {
            // Simplified implementation - return empty result for now
            await Task.CompletedTask;
            return new GetVehiclesResult
            {
                Vehicles = new List<VehicleDto>(),
                TotalCount = 0,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = 0
            };
        }
    }
}