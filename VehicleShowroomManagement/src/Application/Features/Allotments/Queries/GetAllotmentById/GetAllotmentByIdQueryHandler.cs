using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.Allotments.Queries.GetAllotmentById
{
    public class GetAllotmentByIdQueryHandler : IRequestHandler<GetAllotmentByIdQuery, AllotmentDto>
    {
        private readonly IAllotmentRepository _allotmentRepository;

        public GetAllotmentByIdQueryHandler(IAllotmentRepository allotmentRepository)
        {
            _allotmentRepository = allotmentRepository;
        }

        public async Task<AllotmentDto> Handle(GetAllotmentByIdQuery request, CancellationToken cancellationToken)
        {
            var allotment = await _allotmentRepository.GetByIdAsync(request.AllotmentId);

            if (allotment == null)
                throw new KeyNotFoundException($"Allotment with ID {request.AllotmentId} not found");

            return AllotmentDto.FromEntity(allotment);
        }
    }
}