using MediatR;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetTopVehicles
{
    /// <summary>
    /// Handler for get top vehicles query - simplified implementation
    /// </summary>
    public class GetTopVehiclesQueryHandler : IRequestHandler<GetTopVehiclesQuery, List<TopVehicleDto>>
    {
        public async Task<List<TopVehicleDto>> Handle(GetTopVehiclesQuery request, CancellationToken cancellationToken)
        {
            // Simplified implementation - return sample data
            await Task.CompletedTask;
            return new List<TopVehicleDto>();
        }
    }
}