namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument
{
    public class CreateBillingDocumentCommandHandler(
        IRepository<BillingDocument> billingDocumentRepository,
        IRepository<Order> orderRepository) : IRequestHandler<CreateBillingDocumentCommand, string>
    {
        public async Task<string> Handle(CreateBillingDocumentCommand request, CancellationToken cancellationToken)
        {
            // Verify order exists
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null)
            {
                throw new InvalidOperationException("Order not found");
            }

            var billingDocument = new BillingDocument(
                request.OrderId,
                request.CreatedBy,
                request.Amount,
                request.AppointmentDate);

            await billingDocumentRepository.AddAsync(billingDocument, cancellationToken);

            return billingDocument.Id;
        }
    }
}

