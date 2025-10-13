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
        IRepository<BillingDocument> billingDocumentRepository) : IRequestHandler<UpdateServiceOrderStatusCommand, UpdateServiceOrderStatusResult>
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

            // If status is Completed, auto-create BillingDocument
            if (request.Status != ServiceOrderStatus.Completed) return result;
            // Fetch related order to get sale price
            var order = await orderRepository.GetByIdAsync(serviceOrder.OrderId, cancellationToken) ?? throw new ArgumentException("Related order not found");

            // Create billing document with order's sale price
            var billingDocument = new BillingDocument(
                orderId: serviceOrder.OrderId,
                createdBy: serviceOrder.CreatedBy,
                amount: order.SalePrice,
                appointmentDate: order.AppointmentDate);

            await billingDocumentRepository.AddAsync(billingDocument, cancellationToken);

            result.BillingDocumentId = billingDocument.Id;
            result.Message = "Service order completed and billing document created successfully";

            return result;
        }
    }
}

