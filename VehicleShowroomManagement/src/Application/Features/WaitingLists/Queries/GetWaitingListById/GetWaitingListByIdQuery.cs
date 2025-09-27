using MediatR;

namespace VehicleShowroomManagement.Application.Features.WaitingLists.Queries.GetWaitingListById
{
    public class GetWaitingListByIdQuery : IRequest<WaitingListDto>
    {
        public string WaitingListId { get; set; } = string.Empty;
    }
}