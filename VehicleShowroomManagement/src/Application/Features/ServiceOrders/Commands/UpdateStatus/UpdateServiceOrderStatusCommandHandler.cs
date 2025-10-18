using VehicleShowroomManagement.Application.Common.Models;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus
{
    /// <summary>
    /// Handler for updating service order status
    /// Business Logic:
    /// - ServiceOrder Completed + PreDelivery type -> Order Completed + Vehicle Sold
    /// - Other ServiceOrder types -> No impact on Order/Vehicle status
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

            // Business Logic: Handle different status updates
            if (request.Status == ServiceOrderStatus.Cancelled)
            {
                // For cancelled service orders, no impact on Order/Vehicle status
                result.Message = "Service order cancelled";
            }
            else if (request.Status == ServiceOrderStatus.Completed && serviceOrder.Type == ServiceType.PreDelivery)
            {
                // Get related order
                var order = await orderRepository.GetByIdAsync(serviceOrder.OrderId, cancellationToken) ?? throw new ArgumentException("Related order not found");
                
                // Complete the order
                order.Complete();
                await orderRepository.UpdateAsync(order, cancellationToken);

                // Mark vehicle as sold if assigned
                if (!string.IsNullOrEmpty(order.VehicleId))
                {
                    var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == order.VehicleId, cancellationToken);
                    var vehicle = vehicles.FirstOrDefault();
                    if (vehicle != null)
                    {
                        vehicle.Sell();
                        await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
                    }
                }

                // Set license plate if provided
                if (!string.IsNullOrWhiteSpace(request.LicensePlate) && !string.IsNullOrEmpty(order.VehicleId))
                {
                    var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == order.VehicleId, cancellationToken);
                    var vehicle = vehicles.FirstOrDefault();
                    if (vehicle != null)
                    {
                        vehicle.SetLicensePlate(request.LicensePlate);
                        await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
                    }
                }

                result.Message = "Service order completed, order completed, and vehicle marked as sold";
            }
            else if (request.Status == ServiceOrderStatus.Completed)
            {
                // For non-PreDelivery services, only set license plate if provided
                if (!string.IsNullOrWhiteSpace(request.LicensePlate))
                {
                    var order = await orderRepository.GetByIdAsync(serviceOrder.OrderId, cancellationToken);
                    if (order != null && !string.IsNullOrEmpty(order.VehicleId))
                    {
                        var vehicles = await vehicleRepository.FindAsync(v => v.VehicleId == order.VehicleId, cancellationToken);
                        var vehicle = vehicles.FirstOrDefault();
                        if (vehicle != null)
                        {
                            vehicle.SetLicensePlate(request.LicensePlate);
                            await vehicleRepository.UpdateAsync(vehicle, cancellationToken);
                        }
                    }
                }
                result.Message = "Service order completed (no impact on order/vehicle status)";
            }

            return result;
        }
    }
}

