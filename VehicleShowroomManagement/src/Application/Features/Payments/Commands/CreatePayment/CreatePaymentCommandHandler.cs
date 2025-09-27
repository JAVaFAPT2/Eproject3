using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.Payments.Commands.CreatePayment
{
    public class CreatePaymentCommandHandler : IRequestHandler<CreatePaymentCommand, string>
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePaymentCommandHandler(
            IPaymentRepository paymentRepository,
            IInvoiceRepository invoiceRepository,
            ISalesOrderRepository salesOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _paymentRepository = paymentRepository;
            _invoiceRepository = invoiceRepository;
            _salesOrderRepository = salesOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreatePaymentCommand request, CancellationToken cancellationToken)
        {
            // Validate invoice exists
            var invoice = await _invoiceRepository.GetByIdAsync(request.InvoiceId);
            if (invoice == null)
                throw new ArgumentException("Invoice not found", nameof(request.InvoiceId));

            // Validate sales order exists
            var salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            if (salesOrder == null)
                throw new ArgumentException("Sales order not found", nameof(request.SalesOrderId));

            // Create payment
            var payment = new Payment(
                request.InvoiceId,
                request.SalesOrderId,
                request.Amount,
                request.PaymentMethod,
                request.PaymentDate,
                request.ReferenceNumber
            );

            // Add domain events
            payment.AddDomainEvent(new PaymentCreatedEvent(payment.Id, payment.InvoiceId));

            // Save to repository
            await _paymentRepository.AddAsync(payment);
            await _unitOfWork.SaveChangesAsync();

            return payment.Id;
        }
    }
}