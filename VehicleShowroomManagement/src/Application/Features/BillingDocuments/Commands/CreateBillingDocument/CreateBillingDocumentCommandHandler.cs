using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument
{
    public class CreateBillingDocumentCommandHandler : IRequestHandler<CreateBillingDocumentCommand, string>
    {
        private readonly IRepository<BillingDocument> _billingDocumentRepository;
        private readonly IRepository<Order> _orderRepository;

        public CreateBillingDocumentCommandHandler(
            IRepository<BillingDocument> billingDocumentRepository,
            IRepository<Order> orderRepository)
        {
            _billingDocumentRepository = billingDocumentRepository;
            _orderRepository = orderRepository;
        }

        public async Task<string> Handle(CreateBillingDocumentCommand request, CancellationToken cancellationToken)
        {
            // Verify order exists
            var order = await _orderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            var billingDocument = new BillingDocument(
                request.OrderId,
                request.CreatedBy,
                request.Amount,
                request.AppointmentDate);

            await _billingDocumentRepository.AddAsync(billingDocument);

            return billingDocument.Id;
        }
    }
}

