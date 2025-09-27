using MediatR;
using VehicleShowroomManagement.Application.VehicleRegistrations.Commands;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Handlers
{
    /// <summary>
    /// Handler for updating vehicle registrations
    /// </summary>
    public class UpdateVehicleRegistrationCommandHandler : IRequestHandler<UpdateVehicleRegistrationCommand, bool>
    {
        private readonly IRepository<VehicleRegistration> _vehicleRegistrationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateVehicleRegistrationCommandHandler(
            IRepository<VehicleRegistration> vehicleRegistrationRepository,
            IUnitOfWork unitOfWork)
        {
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(UpdateVehicleRegistrationCommand request, CancellationToken cancellationToken)
        {
            var vehicleRegistration = await _vehicleRegistrationRepository.GetByIdAsync(request.Id);
            if (vehicleRegistration == null)
                return false;

            vehicleRegistration.RegistrationNumber = request.RegistrationNumber;
            vehicleRegistration.ExpiryDate = request.ExpiryDate;
            vehicleRegistration.RegistrationAuthority = request.RegistrationAuthority;
            vehicleRegistration.RegistrationState = request.RegistrationState;
            vehicleRegistration.RegistrationCity = request.RegistrationCity;
            vehicleRegistration.OwnerName = request.OwnerName;
            vehicleRegistration.OwnerAddress = request.OwnerAddress;
            vehicleRegistration.OwnerPhone = request.OwnerPhone;
            vehicleRegistration.OwnerEmail = request.OwnerEmail;
            vehicleRegistration.InsuranceNumber = request.InsuranceNumber;
            vehicleRegistration.InsuranceExpiry = request.InsuranceExpiry;
            vehicleRegistration.PollutionCertificateNumber = request.PollutionCertificateNumber;
            vehicleRegistration.PollutionCertificateExpiry = request.PollutionCertificateExpiry;
            vehicleRegistration.FitnessCertificateNumber = request.FitnessCertificateNumber;
            vehicleRegistration.FitnessCertificateExpiry = request.FitnessCertificateExpiry;
            vehicleRegistration.Notes = request.Notes;

            await _vehicleRegistrationRepository.UpdateAsync(vehicleRegistration);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
