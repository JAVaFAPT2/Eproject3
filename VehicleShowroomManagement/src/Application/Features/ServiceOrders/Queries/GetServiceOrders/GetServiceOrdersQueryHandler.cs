namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Queries.GetServiceOrders
{
    /// <summary>
    /// Handler for getting service orders with pagination
    /// </summary>
    public class GetServiceOrdersQueryHandler(IRepository<ServiceOrder> serviceOrderRepository) : IRequestHandler<GetServiceOrdersQuery, ServiceOrdersResponse>
    {
        public async Task<ServiceOrdersResponse> Handle(GetServiceOrdersQuery request, CancellationToken cancellationToken)
        {
            var queryable = serviceOrderRepository.AsQueryable();

            // Apply filters
            if (!string.IsNullOrEmpty(request.Status))
            {
                if (Enum.TryParse<ServiceOrderStatus>(request.Status, true, out var statusEnum))
                {
                    queryable = queryable.Where(so => so.Status == statusEnum);
                }
            }

            if (!string.IsNullOrEmpty(request.OrderId))
            {
                queryable = queryable.Where(so => so.OrderId == request.OrderId);
            }

            if (!string.IsNullOrEmpty(request.CustomerId))
            {
                queryable = queryable.Where(so => so.CustomerId == request.CustomerId);
            }

            // Get total count
            var totalCount = await serviceOrderRepository.CountAsync(queryable, cancellationToken);

            // Apply pagination
            var skip = (request.PageNumber - 1) * request.PageSize;
            var serviceOrders = await serviceOrderRepository.GetPagedAsync(
                queryable,
                skip,
                request.PageSize,
                cancellationToken);

            // Map to DTOs
            var serviceOrderDtos = serviceOrders.Select(so => new ServiceOrderDto
            {
                Id = so.Id,
                OrderId = so.OrderId,
                CustomerId = so.CustomerId,
                CreatedBy = so.CreatedBy,
                ServiceDate = so.ServiceDate,
                AppointmentDate = so.AppointmentDate,
                Description = so.Description,
                Cost = so.Cost,
                Type = so.Type.ToString(),
                Status = so.Status.ToString()
            }).ToList();

            return new ServiceOrdersResponse
            {
                Items = serviceOrderDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)totalCount / request.PageSize)
            };
        }
    }
}
