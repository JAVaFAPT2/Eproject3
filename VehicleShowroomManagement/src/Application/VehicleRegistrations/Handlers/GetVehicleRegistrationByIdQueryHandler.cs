using MediatR;
using VehicleShowroomManagement.Application.VehicleRegistrations.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Handlers
{
    /// <summary>
    /// Handler for getting vehicle registration by ID
    /// </summary>
    public class GetVehicleRegistrationByIdQueryHandler : IRequestHandler<GetVehicleRegistrationByIdQuery, VehicleRegistrationDto?>
    {
        private readonly IRepository<VehicleRegistration> _vehicleRegistrationRepository;

        public GetVehicleRegistrationByIdQueryHandler(IRepository<VehicleRegistration> vehicleRegistrationRepository)
        {
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
        }

        public async Task<VehicleRegistrationDto?> Handle(GetVehicleRegistrationByIdQuery request, CancellationToken cancellationToken)
        {
            var vehicleRegistration = await _vehicleRegistrationRepository.GetByIdAsync(request.Id);
            if (vehicleRegistration == null)
                return null;

            return new VehicleRegistrationDto
            {
                Id = vehicleRegistration.Id,
                VehicleId = vehicleRegistration.VehicleId,
                VIN = vehicleRegistration.VIN,
                RegistrationNumber = vehicleRegistration.RegistrationNumber,
                RegistrationDate = vehicleRegistration.RegistrationDate,
                ExpiryDate = vehicleRegistration.ExpiryDate,
                RegistrationAuthority = vehicleRegistration.RegistrationAuthority,
                RegistrationState = vehicleRegistration.RegistrationState,
                RegistrationCity = vehicleRegistration.RegistrationCity,
                OwnerName = vehicleRegistration.OwnerName,
                OwnerAddress = vehicleRegistration.OwnerAddress,
                OwnerPhone = vehicleRegistration.OwnerPhone,
                OwnerEmail = vehicleRegistration.OwnerEmail,
                VehicleType = vehicleRegistration.VehicleType,
                FuelType = vehicleRegistration.FuelType,
                EngineNumber = vehicleRegistration.EngineNumber,
                ChassisNumber = vehicleRegistration.ChassisNumber,
                ManufacturingYear = vehicleRegistration.ManufacturingYear,
                ManufacturingMonth = vehicleRegistration.ManufacturingMonth,
                SeatingCapacity = vehicleRegistration.SeatingCapacity,
                GrossWeight = vehicleRegistration.GrossWeight,
                UnladenWeight = vehicleRegistration.UnladenWeight,
                Status = vehicleRegistration.Status,
                IsTransferable = vehicleRegistration.IsTransferable,
                PreviousOwner = vehicleRegistration.PreviousOwner,
                TransferDate = vehicleRegistration.TransferDate,
                InsuranceNumber = vehicleRegistration.InsuranceNumber,
                InsuranceExpiry = vehicleRegistration.InsuranceExpiry,
                PollutionCertificateNumber = vehicleRegistration.PollutionCertificateNumber,
                PollutionCertificateExpiry = vehicleRegistration.PollutionCertificateExpiry,
                FitnessCertificateNumber = vehicleRegistration.FitnessCertificateNumber,
                FitnessCertificateExpiry = vehicleRegistration.FitnessCertificateExpiry,
                Notes = vehicleRegistration.Notes,
                CreatedBy = vehicleRegistration.CreatedBy,
                CreatedAt = vehicleRegistration.CreatedAt,
                UpdatedAt = vehicleRegistration.UpdatedAt
            };
        }
    }
}
