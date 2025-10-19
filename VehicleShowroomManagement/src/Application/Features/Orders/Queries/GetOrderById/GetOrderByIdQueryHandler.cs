namespace VehicleShowroomManagement.Application.Features.Orders.Queries.GetOrderById
{
    /// <summary>
    /// Handler for getting a single order by ID
    /// </summary>
    public class GetOrderByIdQueryHandler(IRepository<Order> orderRepository) : IRequestHandler<GetOrderByIdQuery, OrderDetailDto?>
    {
        public async Task<OrderDetailDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            
            if (order == null)
                return null;

            return new OrderDetailDto
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                DealerId = order.DealerId ?? string.Empty,
                ModelNumber = order.ModelNumber,
                SalePrice = order.SalePrice,
                VehicleId = order.VehicleId,
                AppointmentDate = order.AppointmentDate,
                Note = order.Note,
                Status = order.Status.ToString(),
                OrderDate = order.OrderDate,
                ReservationFrom = order.ReservationFrom,
                ReservationTo = order.ReservationTo,
                CreatedAt = order.CreatedAt,
                UpdatedAt = order.UpdatedAt
            };
        }
    }
}
