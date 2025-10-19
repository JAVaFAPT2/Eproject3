using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.UpdateOrderStatus
{
    /// <summary>
    /// Handler for updating order status
    /// Business Logic:
    /// - Order Confirmed -> Vehicle Reserved
    /// - Order Completed -> Vehicle Sold
    /// </summary>
    public class UpdateOrderStatusCommandHandler(
        IRepository<Order> orderRepository,
        IRepository<Vehicle> vehicleRepository) : IRequestHandler<UpdateOrderStatusCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null) return Unit.Value;
            
            order.UpdateStatus(request.Status);
            await orderRepository.UpdateAsync(order, cancellationToken);

            // Business Logic: Update vehicle status based on order status
            if (!string.IsNullOrEmpty(order.VehicleId))
            {
                var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == order.VehicleId, cancellationToken);
                var vehicle = vehicles.FirstOrDefault();
                
                if (vehicle != null)
                {
                    if (request.Status == OrderStatus.Confirmed)
                    {
                        vehicle.Reserve();
                        await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
                    }
                    else if (request.Status == OrderStatus.Completed)
                    {
                        vehicle.Sell();
                        await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
                    }
                    else if (request.Status == OrderStatus.Cancelled)
                    {
                        // When order is cancelled, make vehicle available again
                        vehicle.UpdateStatus(VehicleStatus.Available);
                        await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
                    }
                }
            }

            return Unit.Value;
        }
    }
}


