using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Domain.Events;

namespace VehicleShowroomManagement.Application.Features.WaitingLists.Commands.CreateWaitingList
{
    public class CreateWaitingListCommandHandler : IRequestHandler<CreateWaitingListCommand, string>
    {
        private readonly IWaitingListRepository _waitingListRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateWaitingListCommandHandler(
            IWaitingListRepository waitingListRepository,
            ICustomerRepository customerRepository,
            IUnitOfWork unitOfWork)
        {
            _waitingListRepository = waitingListRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateWaitingListCommand request, CancellationToken cancellationToken)
        {
            // Validate customer exists
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId);
            if (customer == null)
                throw new ArgumentException("Customer not found", nameof(request.CustomerId));

            // Check if customer is already on waiting list for this model
            var existingWaitingList = await _waitingListRepository.GetByCustomerAndModelAsync(request.CustomerId, request.ModelNumber);
            if (existingWaitingList != null)
                throw new ArgumentException("Customer is already on waiting list for this model", nameof(request.ModelNumber));

            // Create waiting list entry
            var waitingList = new WaitingList(
                request.WaitId,
                request.CustomerId,
                request.ModelNumber,
                request.RequestDate,
                request.Status
            );

            // Add domain events
            waitingList.AddDomainEvent(new WaitingListCreatedEvent(waitingList.Id, waitingList.WaitId));

            // Save to repository
            await _waitingListRepository.AddAsync(waitingList);
            await _unitOfWork.SaveChangesAsync();

            return waitingList.Id;
        }
    }
}