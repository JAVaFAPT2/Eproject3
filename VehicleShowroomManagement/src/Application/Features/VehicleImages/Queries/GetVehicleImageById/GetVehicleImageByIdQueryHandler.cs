using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.VehicleImages.Queries.GetVehicleImageById
{
    public class GetVehicleImageByIdQueryHandler : IRequestHandler<GetVehicleImageByIdQuery, VehicleImageDto>
    {
        private readonly IVehicleImageRepository _vehicleImageRepository;

        public GetVehicleImageByIdQueryHandler(IVehicleImageRepository vehicleImageRepository)
        {
            _vehicleImageRepository = vehicleImageRepository;
        }

        public async Task<VehicleImageDto> Handle(GetVehicleImageByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicleImage = await _vehicleImageRepository.GetByIdAsync(request.VehicleImageId);

            if (vehicleImage == null)
                throw new KeyNotFoundException($"Vehicle image with ID {request.VehicleImageId} not found");

            return VehicleImageDto.FromEntity(vehicleImage);
        }
    }
}