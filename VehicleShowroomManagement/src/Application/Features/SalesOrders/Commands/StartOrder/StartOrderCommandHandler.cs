using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Commands.StartOrder
{
    /// <summary>
    /// Handler for starting an order - only updates status
    /// </summary>
    public class StartOrderCommandHandler : IRequestHandler<StartOrderCommand, string>
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Vehicle> _vehicleRepository;
        private readonly IRepository<SalesOrder> _salesOrderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StartOrderCommandHandler(
            IRepository<Customer> customerRepository,
            IRepository<Vehicle> vehicleRepository,
            IRepository<SalesOrder> salesOrderRepository,
            IUnitOfWork unitOfWork)
        {
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _salesOrderRepository = salesOrderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(StartOrderCommand request, CancellationToken cancellationToken)
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
                throw new InvalidOperationException($"Vehicle {vehicle.VehicleId} is not available");

            // Reserve the vehicle
            vehicle.Reserve();
            await _vehicleRepository.UpdateAsync(vehicle, cancellationToken);

            // Generate order number
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8]}".ToUpper();

            // Create minimal sales order (only with status)
            var salesOrder = new SalesOrder(
                orderNumber,
                request.CustomerId,
                request.VehicleId,
                "SYSTEM", // Default salesperson, will be updated later
                0, // Will be updated later
                PaymentMethod.Cash); // Default, will be updated later

            // Set initial status
            salesOrder.UpdateStatus(request.InitialStatus);

            var result = await _salesOrderRepository.AddAsync(salesOrder, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Id;
        }
    }
}
