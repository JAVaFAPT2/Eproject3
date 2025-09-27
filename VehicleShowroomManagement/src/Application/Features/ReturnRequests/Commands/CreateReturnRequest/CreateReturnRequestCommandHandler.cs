using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.ReturnRequests.Commands.CreateReturnRequest
{
    public class CreateReturnRequestCommandHandler : IRequestHandler<CreateReturnRequestCommand, string>
    {
        private readonly IReturnRequestRepository _returnRequestRepository;
        private readonly ISalesOrderRepository _salesOrderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReturnRequestCommandHandler(
            IReturnRequestRepository returnRequestRepository,
            ISalesOrderRepository salesOrderRepository,
            ICustomerRepository customerRepository,
            IVehicleRepository vehicleRepository,
            IUnitOfWork unitOfWork)
        {
            _returnRequestRepository = returnRequestRepository;
            _salesOrderRepository = salesOrderRepository;
            _customerRepository = customerRepository;
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateReturnRequestCommand request, CancellationToken cancellationToken)
        {
            // Validate order exists
            var order = await _salesOrderRepository.GetByIdAsync(request.OrderId);
            if (order == null)
                throw new ArgumentException("Sales order not found", nameof(request.OrderId));

            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new ArgumentException("Customer not found", nameof(request.CustomerId));

            // Validate vehicle exists
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
                throw new ArgumentException("Vehicle not found", nameof(request.VehicleId));

            // Create return request
            var returnRequest = new ReturnRequest(
                request.OrderId,
                request.CustomerId,
                request.VehicleId,
                request.Reason,
                request.Description,
                request.RefundAmount
            );

            // Add domain events
            returnRequest.AddDomainEvent(new ReturnRequestCreatedEvent(returnRequest.Id, returnRequest.OrderId));

            // Save to repository
            await _returnRequestRepository.AddAsync(returnRequest);
            await _unitOfWork.SaveChangesAsync();

            return returnRequest.Id;
        }
    }
}