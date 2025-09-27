using MediatR;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Queries.GetOrders
{
    /// <summary>
    /// Handler for get orders query - simplified implementation
    /// </summary>
    public class GetOrdersQueryHandler : IRequestHandler<GetOrdersQuery, GetOrdersResult>
    {
        public async Task<GetOrdersResult> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            // Simplified implementation - return empty result for now
            await Task.CompletedTask;
            return new GetOrdersResult
            {
                Orders = new List<OrderSummaryDto>(),
                TotalCount = 0,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = 0
            };
        }
    }
}