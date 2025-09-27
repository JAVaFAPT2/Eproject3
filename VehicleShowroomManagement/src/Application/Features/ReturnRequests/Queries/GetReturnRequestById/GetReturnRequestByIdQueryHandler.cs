using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.ReturnRequests.Queries.GetReturnRequestById
{
    public class GetReturnRequestByIdQueryHandler : IRequestHandler<GetReturnRequestByIdQuery, ReturnRequestDto>
    {
        private readonly IReturnRequestRepository _returnRequestRepository;

        public GetReturnRequestByIdQueryHandler(IReturnRequestRepository returnRequestRepository)
        {
            _returnRequestRepository = returnRequestRepository;
        }

        public async Task<ReturnRequestDto> Handle(GetReturnRequestByIdQuery request, CancellationToken cancellationToken)
        {
            var returnRequest = await _returnRequestRepository.GetByIdAsync(request.ReturnRequestId);

            if (returnRequest == null)
                throw new KeyNotFoundException($"Return request with ID {request.ReturnRequestId} not found");

            return ReturnRequestDto.FromEntity(returnRequest);
        }
    }
}