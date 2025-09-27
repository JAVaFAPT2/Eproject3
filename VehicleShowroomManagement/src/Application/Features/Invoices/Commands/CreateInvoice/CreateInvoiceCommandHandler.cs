using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.Invoices.Commands.CreateInvoice
{
    public class CreateInvoiceCommandHandler : IRequestHandler<CreateInvoiceCommand, string>
    {
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateInvoiceCommandHandler(
            IInvoiceRepository invoiceRepository,
            ISalesOrderRepository salesOrderRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _salesOrderRepository = salesOrderRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
        {
            // Validate sales order exists
            var salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            if (salesOrder == null)
                throw new ArgumentException("Sales order not found", nameof(request.SalesOrderId));

            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new ArgumentException("Customer not found", nameof(request.CustomerId));

            // Create invoice
            var invoice = new Invoice(
                request.SalesOrderId,
                request.InvoiceNumber,
                request.CustomerId,
                request.InvoiceDate,
                request.DueDate,
                request.Subtotal,
                request.TaxAmount,
                request.TotalAmount
            );

            // Add domain events
            invoice.AddDomainEvent(new InvoiceCreatedEvent(invoice.Id, invoice.InvoiceNumber));

            // Save to repository
            await _invoiceRepository.AddAsync(invoice);
            await _unitOfWork.SaveChangesAsync();

            return invoice.Id;
        }
    }
}