using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;

namespace VehicleShowroomManagement.Application.Features.VehicleRegistrations.Queries.GetVehicleRegistrationById
{
    public class GetVehicleRegistrationByIdQueryHandler : IRequestHandler<GetVehicleRegistrationByIdQuery, VehicleRegistrationDto>
    {
        private readonly IVehicleRegistrationRepository _vehicleRegistrationRepository;

        public GetVehicleRegistrationByIdQueryHandler(IVehicleRegistrationRepository vehicleRegistrationRepository)
        {
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
        }

        public async Task<VehicleRegistrationDto> Handle(GetVehicleRegistrationByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicleRegistration = await _vehicleRegistrationRepository.GetByIdAsync(request.VehicleRegistrationId);

            if (vehicleRegistration == null)
                throw new KeyNotFoundException($"Vehicle registration with ID {request.VehicleRegistrationId} not found");

            return VehicleRegistrationDto.FromEntity(vehicleRegistration);
        }
    }
}