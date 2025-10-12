using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Application.Common.Models;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus
{
    /// <summary>
    /// Handler for updating service order status
    /// Auto-creates BillingDocument when status changes to Completed
    /// </summary>
    public class UpdateServiceOrderStatusCommandHandler : IRequestHandler<UpdateServiceOrderStatusCommand, UpdateServiceOrderStatusResult>
    {
        private readonly IRepository<ServiceOrder> _serviceOrderRepository;
        private readonly IRepository<Order> _orderRepository;
        private readonly IRepository<BillingDocument> _billingDocumentRepository;

        public UpdateServiceOrderStatusCommandHandler(
            IRepository<ServiceOrder> serviceOrderRepository,
            IRepository<Order> orderRepository,
            IRepository<BillingDocument> billingDocumentRepository)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _orderRepository = orderRepository;
            _billingDocumentRepository = billingDocumentRepository;
        }

        public async Task<UpdateServiceOrderStatusResult> Handle(UpdateServiceOrderStatusCommand request, CancellationToken cancellationToken)
        {
            // Fetch service order
            var serviceOrder = await _serviceOrderRepository.GetByIdAsync(request.ServiceOrderId);
            if (serviceOrder == null)
            {
                throw new InvalidOperationException("Service order not found");
            }

            // Update status using domain method
            serviceOrder.UpdateStatus(request.Status);
            await _serviceOrderRepository.UpdateAsync(serviceOrder);

            var result = new UpdateServiceOrderStatusResult
            {
                Success = true,
                Message = "Service order status updated successfully"
            };

            // If status is Completed, auto-create BillingDocument
            if (request.Status == ServiceOrderStatus.Completed)
            {
                // Fetch related order to get sale price
                var order = await _orderRepository.GetByIdAsync(serviceOrder.OrderId);
                if (order == null)
                {
                    throw new InvalidOperationException("Related order not found");
                }

                // Create billing document with order's sale price
                var billingDocument = new BillingDocument(
                    orderId: serviceOrder.OrderId,
                    createdBy: serviceOrder.CreatedBy,
                    amount: order.SalePrice,
                    appointmentDate: order.AppointmentDate);

                await _billingDocumentRepository.AddAsync(billingDocument);

                result.BillingDocumentId = billingDocument.Id;
                result.Message = "Service order completed and billing document created successfully";
            }

            return result;
        }
    }
}

