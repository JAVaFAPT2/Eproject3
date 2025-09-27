using MediatR;
using VehicleShowroomManagement.Application.VehicleRegistrations.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Handlers
{
    /// <summary>
    /// Handler for creating vehicle registrations
    /// </summary>
    public class CreateVehicleRegistrationCommandHandler : IRequestHandler<CreateVehicleRegistrationCommand, string>
    {
        private readonly IRepository<VehicleRegistration> _vehicleRegistrationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateVehicleRegistrationCommandHandler(
            IRepository<VehicleRegistration> vehicleRegistrationRepository,
            IUnitOfWork unitOfWork)
        {
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> Handle(CreateVehicleRegistrationCommand request, CancellationToken cancellationToken)
        {
            var vehicleRegistration = new VehicleRegistration
            {
                VehicleId = request.VehicleId,
                VIN = request.VIN,
                RegistrationNumber = request.RegistrationNumber,
                RegistrationDate = request.RegistrationDate,
                ExpiryDate = request.ExpiryDate,
                RegistrationAuthority = request.RegistrationAuthority,
                RegistrationState = request.RegistrationState,
                RegistrationCity = request.RegistrationCity,
                OwnerName = request.OwnerName,
                OwnerAddress = request.OwnerAddress,
                OwnerPhone = request.OwnerPhone,
                OwnerEmail = request.OwnerEmail,
                VehicleType = request.VehicleType,
                FuelType = request.FuelType,
                EngineNumber = request.EngineNumber,
                ChassisNumber = request.ChassisNumber,
                ManufacturingYear = request.ManufacturingYear,
                ManufacturingMonth = request.ManufacturingMonth,
                SeatingCapacity = request.SeatingCapacity,
                GrossWeight = request.GrossWeight,
                UnladenWeight = request.UnladenWeight,
                InsuranceNumber = request.InsuranceNumber,
                InsuranceExpiry = request.InsuranceExpiry,
                PollutionCertificateNumber = request.PollutionCertificateNumber,
                PollutionCertificateExpiry = request.PollutionCertificateExpiry,
                FitnessCertificateNumber = request.FitnessCertificateNumber,
                FitnessCertificateExpiry = request.FitnessCertificateExpiry,
                Notes = request.Notes,
                CreatedBy = "System", // TODO: Get from current user context
                Status = "Active"
            };

            await _vehicleRegistrationRepository.AddAsync(vehicleRegistration);
            await _unitOfWork.SaveChangesAsync();

            return vehicleRegistration.Id;
        }
    }
}
