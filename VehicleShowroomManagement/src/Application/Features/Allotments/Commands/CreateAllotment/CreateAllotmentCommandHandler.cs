using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.Allotments.Commands.CreateAllotment
{
    public class CreateAllotmentCommandHandler : IRequestHandler<CreateAllotmentCommand, string>
    {
        private readonly IAllotmentRepository _allotmentRepository;
        private readonly IVehicleRepository _vehicleRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateAllotmentCommandHandler(
            IAllotmentRepository allotmentRepository,
            IVehicleRepository vehicleRepository,
            ICustomerRepository customerRepository,
            IEmployeeRepository employeeRepository,
            IUnitOfWork unitOfWork)
        {
            _allotmentRepository = allotmentRepository;
            _vehicleRepository = vehicleRepository;
            _customerRepository = customerRepository;
            _employeeRepository = employeeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateAllotmentCommand request, CancellationToken cancellationToken)
        {
            // Validate vehicle exists
            var vehicle = await _vehicleRepository.GetByIdAsync(request.VehicleId);
            if (vehicle == null)
                throw new ArgumentException("Vehicle not found", nameof(request.VehicleId));

            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new ArgumentException("Customer not found", nameof(request.CustomerId));

            // Validate employee exists
            var employee = await _employeeRepository.GetByIdAsync(request.EmployeeId);
            if (employee == null)
                throw new ArgumentException("Employee not found", nameof(request.EmployeeId));

            // Check if vehicle is already allotted
            var existingAllotment = await _allotmentRepository.GetByVehicleAndCustomerAsync(request.VehicleId, request.CustomerId);
            if (existingAllotment != null)
                throw new ArgumentException("Vehicle is already allotted to this customer", nameof(request.VehicleId));

            // Create allotment
            var allotment = new Allotment(
                request.AllotmentId,
                request.VehicleId,
                request.CustomerId,
                request.EmployeeId,
                request.AllotmentDate
            );

            // Add domain events
            allotment.AddDomainEvent(new AllotmentCreatedEvent(allotment.Id, allotment.AllotmentId));

            // Save to repository
            await _allotmentRepository.AddAsync(allotment);
            await _unitOfWork.SaveChangesAsync();

            return allotment.Id;
        }
    }
}