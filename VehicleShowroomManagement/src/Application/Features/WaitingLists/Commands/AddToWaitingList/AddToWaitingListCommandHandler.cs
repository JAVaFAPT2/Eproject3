
namespace VehicleShowroomManagement.Application.Features.WaitingLists.Commands.AddToWaitingList
{
    /// <summary>
    /// Handler for adding customers to waiting list
    /// </summary>
    public class AddToWaitingListCommandHandler(
        IRepository<WaitingList> waitingListRepository,
        IRepository<Customer> customerRepository,
        IUnitOfWork unitOfWork)
        : IRequestHandler<AddToWaitingListCommand, string>
    {
        public async Task<string> Handle(AddToWaitingListCommand request, CancellationToken cancellationToken)
        {
            // Validate customer exists
            var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken);
            if (customer == null || customer.IsDeleted)
                throw new ArgumentException($"Customer with ID {request.CustomerId} not found");

            // Check if customer is already on waiting list for this model
            var existingWaitingList = await waitingListRepository
                .FirstOrDefaultAsync(w => w.CustomerId == request.CustomerId &&
                                        w.ModelNumber == request.ModelNumber &&
                                        w.Status == "Waiting" &&
                                        !w.IsDeleted, cancellationToken);

            if (existingWaitingList != null)
                throw new InvalidOperationException($"Customer is already on the waiting list for model {request.ModelNumber}");

            // Generate unique wait ID
            var waitId = $"WAIT-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8]}".ToUpper();

            // Create waiting list entry
            var waitingList = new WaitingList
            {
                WaitId = waitId,
                CustomerId = request.CustomerId,
                ModelNumber = request.ModelNumber,
                RequestDate = DateTime.UtcNow,
                Status = "Waiting"
            };

            var result = await waitingListRepository.AddAsync(waitingList, cancellationToken);
            await unitOfWork.SaveChangesAsync(cancellationToken);

            return result.Id;
        }
    }
}
