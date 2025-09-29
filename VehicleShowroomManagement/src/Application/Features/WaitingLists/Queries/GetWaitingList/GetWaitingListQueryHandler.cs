using VehicleShowroomManagement.Application.Common.DTOs;

namespace VehicleShowroomManagement.Application.Features.WaitingLists.Queries.GetWaitingList
{
    /// <summary>
    /// Handler for getting waiting list entries
    /// </summary>
    public class GetWaitingListQueryHandler(
        IRepository<WaitingList> waitingListRepository,
        IRepository<Customer> customerRepository) : IRequestHandler<GetWaitingListQuery, GetWaitingListResult>
    {
        public async Task<GetWaitingListResult> Handle(GetWaitingListQuery request, CancellationToken cancellationToken)
        {
            // Get all waiting list entries (simplified for now)
            var allEntries = await waitingListRepository.GetAllAsync(cancellationToken);
            var filteredEntries = allEntries.AsEnumerable().Where(w => !w.IsDeleted);

            // Apply filters
            if (!string.IsNullOrEmpty(request.ModelNumber))
                filteredEntries = filteredEntries.Where(w => w.ModelNumber.Contains(request.ModelNumber));

            if (!string.IsNullOrEmpty(request.CustomerId))
                filteredEntries = filteredEntries.Where(w => w.CustomerId == request.CustomerId);

            if (!string.IsNullOrEmpty(request.Status))
                filteredEntries = filteredEntries.Where(w => w.Status == request.Status);

            // Apply pagination
            var waitingLists = filteredEntries as WaitingList[] ?? filteredEntries.ToArray();
            var totalCount = waitingLists.Length;
            var pagedEntries = waitingLists
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Map to DTOs with customer information
            var waitingListDtos = new List<WaitingListDto>();

            foreach (var entry in pagedEntries)
            {
                var customer = await customerRepository.GetByIdAsync(entry.CustomerId, cancellationToken);
                waitingListDtos.Add(new WaitingListDto
                {
                    Id = entry.Id,
                    WaitId = entry.WaitId,
                    CustomerId = entry.CustomerId,
                    CustomerName = customer?.FullName ?? "Unknown",
                    ModelNumber = entry.ModelNumber,
                    RequestDate = entry.RequestDate,
                    Status = entry.Status,
                    CreatedAt = entry.CreatedAt,
                    UpdatedAt = entry.UpdatedAt
                });
            }

            return new GetWaitingListResult
            {
                WaitingListEntries = waitingListDtos,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
            };
        }
    }
}
