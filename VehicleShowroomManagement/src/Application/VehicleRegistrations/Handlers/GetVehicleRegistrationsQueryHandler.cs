using MediatR;
using VehicleShowroomManagement.Application.VehicleRegistrations.DTOs;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Handlers
{
    /// <summary>
    /// Handler for getting vehicle registrations
    /// </summary>
    public class GetVehicleRegistrationsQueryHandler : IRequestHandler<GetVehicleRegistrationsQuery, IEnumerable<VehicleRegistrationDto>>
    {
        private readonly IRepository<VehicleRegistration> _vehicleRegistrationRepository;

        public GetVehicleRegistrationsQueryHandler(IRepository<VehicleRegistration> vehicleRegistrationRepository)
        {
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
        }

        public async Task<IEnumerable<VehicleRegistrationDto>> Handle(GetVehicleRegistrationsQuery request, CancellationToken cancellationToken)
        {
            var vehicleRegistrations = await _vehicleRegistrationRepository.GetAllAsync();

            // Apply filters
            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                vehicleRegistrations = vehicleRegistrations.Where(vr => 
                    vr.RegistrationNumber.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    vr.VIN.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    vr.OwnerName.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    vr.RegistrationState.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                    vr.RegistrationCity.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase));
            }

            if (!string.IsNullOrEmpty(request.Status))
            {
                vehicleRegistrations = vehicleRegistrations.Where(vr => vr.Status == request.Status);
            }

            if (!string.IsNullOrEmpty(request.RegistrationState))
            {
                vehicleRegistrations = vehicleRegistrations.Where(vr => vr.RegistrationState == request.RegistrationState);
            }

            if (!string.IsNullOrEmpty(request.VehicleType))
            {
                vehicleRegistrations = vehicleRegistrations.Where(vr => vr.VehicleType == request.VehicleType);
            }

            if (!string.IsNullOrEmpty(request.FuelType))
            {
                vehicleRegistrations = vehicleRegistrations.Where(vr => vr.FuelType == request.FuelType);
            }

            if (request.FromDate.HasValue)
            {
                vehicleRegistrations = vehicleRegistrations.Where(vr => vr.RegistrationDate >= request.FromDate.Value);
            }

            if (request.ToDate.HasValue)
            {
                vehicleRegistrations = vehicleRegistrations.Where(vr => vr.RegistrationDate <= request.ToDate.Value);
            }

            // Apply pagination
            vehicleRegistrations = vehicleRegistrations
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize);

            return vehicleRegistrations.Select(MapToDto);
        }

        private static VehicleRegistrationDto MapToDto(VehicleRegistration vehicleRegistration)
        {
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
