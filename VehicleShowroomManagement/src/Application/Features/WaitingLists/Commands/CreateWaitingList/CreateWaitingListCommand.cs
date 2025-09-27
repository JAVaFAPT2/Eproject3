using MediatR;

namespace VehicleShowroomManagement.Application.Features.WaitingLists.Commands.CreateWaitingList
{
    public class CreateWaitingListCommand : IRequest<string>
    {
        public string WaitId { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;
        public string ModelNumber { get; set; } = string.Empty;
        public DateTime RequestDate { get; set; }
        public string Status { get; set; } = "Waiting";
    }
}