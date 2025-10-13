using VehicleShowroomManagement.Application.Common.Models;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus
{
    /// <summary>
    /// Handler for updating service order status
    /// Auto-creates BillingDocument when status changes to Completed
    /// </summary>
    public class UpdateServiceOrderStatusCommandHandler(
        IRepository<ServiceOrder> serviceOrderRepository,
        IRepository<Order> orderRepository,
        IRepository<Vehicle> vehicleRepository) : IRequestHandler<UpdateServiceOrderStatusCommand, UpdateServiceOrderStatusResult>
    {

        public async Task<UpdateServiceOrderStatusResult> Handle(UpdateServiceOrderStatusCommand request, CancellationToken cancellationToken)
        {
            // Fetch service order
            var serviceOrder = await serviceOrderRepository.GetByIdAsync(request.ServiceOrderId, cancellationToken) ?? throw new ArgumentException("Service order not found");

            // Update status using domain method
            serviceOrder.UpdateStatus(request.Status);
            await serviceOrderRepository.UpdateAsync(serviceOrder, cancellationToken);

            var result = new UpdateServiceOrderStatusResult
            {
                Success = true,
                Message = "Service order status updated successfully"
            };

            // If status is Completed, set vehicle license plate
            if (request.Status == ServiceOrderStatus.Completed)
            {
                var order = await orderRepository.GetByIdAsync(serviceOrder.OrderId, cancellationToken) ?? throw new ArgumentException("Related order not found");
                if (string.IsNullOrEmpty(order.VehicleId)) throw new InvalidOperationException("Order has no assigned vehicle");
                var vehicle = await vehicleRepository.GetByIdAsync(order.VehicleId, cancellationToken) ?? throw new ArgumentException("Vehicle not found");
                if (!string.IsNullOrWhiteSpace(request.LicensePlate))
                {
                    vehicle.SetLicensePlate(request.LicensePlate);
                    await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
                }
                result.Message = "Service order completed and vehicle license plate set";
            }

            return result;
        }
    }
}

