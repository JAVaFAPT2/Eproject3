using MediatR;

namespace VehicleShowroomManagement.Application.Features.Dashboard.Queries.GetRecentOrders
{
    /// <summary>
    /// Handler for get recent orders query - simplified implementation
    /// </summary>
    public class GetRecentOrdersQueryHandler : IRequestHandler<GetRecentOrdersQuery, List<RecentOrderDto>>
    {
        public async Task<List<RecentOrderDto>> Handle(GetRecentOrdersQuery request, CancellationToken cancellationToken)
        {
            // Simplified implementation - return sample data
            await Task.CompletedTask;
            return new List<RecentOrderDto>();
        }
    }
}