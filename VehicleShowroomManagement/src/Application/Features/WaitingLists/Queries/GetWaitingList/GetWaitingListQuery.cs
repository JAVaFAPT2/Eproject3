
namespace VehicleShowroomManagement.Application.Features.WaitingLists.Queries.GetWaitingList
{
    /// <summary>
    /// Query to get waiting list entries with pagination and filters
    /// </summary>
    public record GetWaitingListQuery(
        int PageNumber,
        int PageSize,
        string? ModelNumber = null,
        string? CustomerId = null,
        string? Status = null) : IRequest<GetWaitingListResult>;
}
