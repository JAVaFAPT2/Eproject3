using MediatR;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Commands.CreateSalesOrder
{
    /// <summary>
    /// Handler for creating sales orders - implements core business process
    /// </summary>
    public class CreateSalesOrderCommandHandler : IRequestHandler<CreateSalesOrderCommand, string>
    {
        private readonly IRepository<SalesOrder> _salesOrderRepository;
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<Employee> _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMediator _mediator;

        public CreateSalesOrderCommandHandler(
            IRepository<SalesOrder> salesOrderRepository,
            IRepository<Customer> customerRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<Employee> employeeRepository,
            IUnitOfWork unitOfWork,
            IMediator mediator)
        {
            _salesOrderRepository = salesOrderRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
            _mediator = mediator;
        }

        public async Task<string> Handle(CreateSalesOrderCommand request, CancellationToken cancellationToken)
        {
            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer == null || customer.IsDeleted)
                throw new ArgumentException($"Customer with ID {request.CustomerId} not found");

            // Validate vehicle exists and is available
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId, cancellationToken);
            if (vehicle == null || vehicle.IsDeleted)
                throw new ArgumentException($"Vehicle with ID {request.VehicleId} not found");

            if (!vehicle.IsAvailable)
                throw new InvalidOperationException($"Vehicle {vehicle.VehicleId} is not available for sale");

            // Validate sales person exists
            var salesPerson = await _employeeRepository.GetByIdAsync(request.SalesPersonId, cancellationToken);
            if (salesPerson == null || salesPerson.IsDeleted)
                throw new ArgumentException($"Sales person with ID {request.SalesPersonId} not found");

            if (!salesPerson.IsDealer && salesPerson.Role != Domain.Enums.UserRole.Admin)
                throw new UnauthorizedAccessException("Only dealers can create sales orders");

            // Reserve the vehicle
            vehicle.Reserve();
            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

            // Create sales order
            var salesOrder = new SalesOrder(
                request.OrderNumber,
                request.CustomerId,
                request.VehicleId,
                request.SalesPersonId,
                request.TotalAmount,
                request.PaymentMethod);

            var result = await _salesOrderRepository.AddAsync(salesOrder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Id;
        }
    }
}