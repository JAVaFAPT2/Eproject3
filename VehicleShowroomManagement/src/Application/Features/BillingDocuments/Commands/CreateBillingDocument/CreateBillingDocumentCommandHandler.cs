namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument
{
    public class CreateBillingDocumentCommandHandler(
        IRepository<BillingDocument> billingDocumentRepository,
        IRepository<Order> orderRepository) : IRequestHandler<CreateBillingDocumentCommand, string>
    {
        public async Task<string> Handle(CreateBillingDocumentCommand request, CancellationToken cancellationToken)
        {
            // Validate input parameters first
            if (string.IsNullOrWhiteSpace(request.OrderId))
                throw new ArgumentException("Order ID cannot be null or empty", nameof(request.OrderId));
            
            if (string.IsNullOrWhiteSpace(request.CreatedBy))
                throw new ArgumentException("Created by cannot be null or empty", nameof(request.CreatedBy));
            
            if (request.Amount < 0)
                throw new ArgumentException("Amount cannot be negative", nameof(request.Amount));
            
            if (request.AppointmentDate.HasValue && request.AppointmentDate.Value < DateTime.Now)
                throw new ArgumentException("Appointment date cannot be in the past", nameof(request.AppointmentDate));

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

