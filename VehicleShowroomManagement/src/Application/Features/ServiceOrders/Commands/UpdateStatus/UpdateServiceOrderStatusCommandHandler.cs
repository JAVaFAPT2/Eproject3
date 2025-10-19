using VehicleShowroomManagement.Application.Common.Models;
using VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus
{
    /// <summary>
    /// Handler for updating service order status
    /// Business Logic:
    /// - ServiceOrder Completed + PreDelivery type -> Order Completed + Vehicle Sold + Billing Document (Service Cost + Order Amount)
    /// - ServiceOrder Completed + Maintenance/Repair type -> Billing Document (Service Cost Only)
    /// - Other ServiceOrder types -> No impact on Order/Vehicle status
    /// </summary>
    public class UpdateServiceOrderStatusCommandHandler(
        IRepository<ServiceOrder> serviceOrderRepository,
        IRepository<Order> orderRepository,
        IRepository<Vehicle> vehicleRepository,
        IMediator mediator) : IRequestHandler<UpdateServiceOrderStatusCommand, UpdateServiceOrderStatusResult>
    {

        public async Task<UpdateServiceOrderStatusResult> Handle(UpdateServiceOrderStatusCommand request, CancellationToken cancellationToken)
        {
            // Fetch service order
            var serviceOrder = await serviceOrderRepository.GetByIdAsync(request.ServiceOrderId, cancellationToken) ?? throw new ArgumentException("Service order not found");

            // Update status using domain method
            serviceOrder.UpdateStatus(request.Status);
            await serviceOrderRepository.UpdateAsync(serviceOrder, cancellationToken);

            // Update license plate if provided (for any status)
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


                // Create billing document: Service Cost + Order Amount (PreDelivery)
                var totalAmount = serviceOrder.Cost + order.SalePrice;
                var createBillingCommand = new CreateBillingDocumentCommand(
                    serviceOrder.OrderId,
                    serviceOrder.CreatedBy,
                    totalAmount,
                    serviceOrder.AppointmentDate);

                var billingDocumentId = await mediator.Send(createBillingCommand, cancellationToken);

                result.Message = "Service order completed, order completed, vehicle marked as sold, and billing document created";
                result.BillingDocumentId = billingDocumentId;
            }
            else if (request.Status == ServiceOrderStatus.Completed)
            {
                // For Maintenance/Repair services, create billing document
                var createBillingCommand = new CreateBillingDocumentCommand(
                    serviceOrder.OrderId,
                    serviceOrder.CreatedBy,
                    serviceOrder.Cost,
                    serviceOrder.AppointmentDate);

                var billingDocumentId = await mediator.Send(createBillingCommand, cancellationToken);

                result.Message = "Service order completed and billing document created";
                result.BillingDocumentId = billingDocumentId;
            }

            return result;
        }
    }
}

