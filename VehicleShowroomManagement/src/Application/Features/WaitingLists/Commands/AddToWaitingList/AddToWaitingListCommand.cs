
namespace VehicleShowroomManagement.Application.Features.WaitingLists.Commands.AddToWaitingList
{
    /// <summary>
    /// Command to add a customer to waiting list for a specific vehicle model
    /// </summary>
    public record AddToWaitingListCommand(
        string CustomerId,
        string ModelNumber,
        DateTime? PreferredDate = null,
        string? Notes = null)
        : IRequest<string>;
}
