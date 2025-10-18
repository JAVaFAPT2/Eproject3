namespace VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrders
{
    /// <summary>
    /// Handler for getting orders with pagination
    /// </summary>
    public class GetOrdersQueryHandler(IRepository<Order> orderRepository) : IRequestHandler<GetOrdersQuery, OrdersResponse>
    {

        public async Task<OrdersResponse> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
        {
            var queryable = orderRepository.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Status))
            {
                if (Enum.TryParse<OrderStatus>(request.Status, true, out var statusEnum))
                {
                    queryable = queryable.Where(o => o.Status == statusEnum);
                }
            }

            if (!string.IsNullOrEmpty(request.CustomerId))
            {
                queryable = queryable.Where(o => o.CustomerId == request.CustomerId);
            }

            // Get total count
            var totalCount = await orderRepository.CountAsync(queryable, cancellationToken);

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var orders = await orderRepository.GetPagedAsync(
                queryable,
                skip,
                request.PageSize,
                cancellationToken);

            // Map to DTOs
            var orderDtos = orders.Select(order => new OrderDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                DealerId = order.DealerId ?? string.Empty,
                ModelNumber = order.ModelNumber,
                SalePrice = order.SalePrice,
                VehicleId = order.VehicleId,
                AppointmentDate = order.AppointmentDate,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            }).ToList();

            return new OrdersResponse
            {
                Items = orderDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }
    }
}
