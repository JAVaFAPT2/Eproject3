using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder
{
    public class CreateServiceOrderCommandHandler : IRequestHandler<CreateServiceOrderCommand, string>
    {
        private readonly IServiceOrderRepository _serviceOrderRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateServiceOrderCommandHandler(
            IServiceOrderRepository serviceOrderRepository,
            ISalesOrderRepository salesOrderRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _serviceOrderRepository = serviceOrderRepository;
            _salesOrderRepository = salesOrderRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateServiceOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate sales order exists
            var salesOrder = await _salesOrderRepository.GetByIdAsync(request.SalesOrderId);
            if (salesOrder == null)
                throw new ArgumentException("Sales order not found", nameof(request.SalesOrderId));

            // Validate employee exists
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
                throw new ArgumentException("Employee not found", nameof(request.EmployeeId));

            // Create service order
            var serviceOrder = new ServiceOrder(
                request.ServiceOrderNumber,
                request.SalesOrderId,
                request.EmployeeId,
                request.ServiceDate,
                request.Description,
                request.Cost,
                request.Notes
            );

            // Add domain events
            serviceOrder.AddDomainEvent(new ServiceOrderCreatedEvent(serviceOrder.Id, serviceOrder.ServiceOrderNumber));

            // Save to repository
            await _serviceOrderRepository.AddAsync(serviceOrder);
            await _unitOfWork.SaveChangesAsync();

            return serviceOrder.Id;
        }
    }
}