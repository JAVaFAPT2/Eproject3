using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.WaitingLists.Queries.GetWaitingListById
{
    public class GetWaitingListByIdQueryHandler : IRequestHandler<GetWaitingListByIdQuery, WaitingListDto>
    {
        private readonly IWaitingListRepository _waitingListRepository;

        public GetWaitingListByIdQueryHandler(IWaitingListRepository waitingListRepository)
        {
            _waitingListRepository = waitingListRepository;
        }

        public async Task<WaitingListDto> Handle(GetWaitingListByIdQuery request, CancellationToken cancellationToken)
        {
            var waitingList = await _waitingListRepository.GetByIdAsync(request.WaitingListId);

            if (waitingList == null)
                throw new KeyNotFoundException($"Waiting list entry with ID {request.WaitingListId} not found");

            return WaitingListDto.FromEntity(waitingList);
        }
    }
}