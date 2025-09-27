using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.BillingDocuments.Commands.CreateBillingDocument
{
    public class CreateBillingDocumentCommandHandler : IRequestHandler<CreateBillingDocumentCommand, string>
    {
        private readonly IBillingDocumentRepository _billingDocumentRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBillingDocumentCommandHandler(
            IBillingDocumentRepository billingDocumentRepository,
            ISalesOrderRepository salesOrderRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _billingDocumentRepository = billingDocumentRepository;
            _salesOrderRepository = salesOrderRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateBillingDocumentCommand request, CancellationToken cancellationToken)
        {
            // Validate sales order exists
            var salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            if (salesOrder == null)
                throw new ArgumentException("Sales order not found", nameof(request.SalesOrderId));

            // Validate employee exists
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
                throw new ArgumentException("Employee not found", nameof(request.EmployeeId));

            // Create billing document
            var billingDocument = new BillingDocument(
                request.BillNumber,
                request.SalesOrderId,
                request.EmployeeId,
                request.BillDate,
                request.Amount
            );

            // Add domain events
            billingDocument.AddDomainEvent(new BillingDocumentCreatedEvent(billingDocument.Id, billingDocument.BillNumber));

            // Save to repository
            await _billingDocumentRepository.AddAsync(billingDocument);
            await _unitOfWork.SaveChangesAsync();

            return billingDocument.Id;
        }
    }
}