using MediatR;
using VehicleShowroomManagement.Application.VehicleRegistrations.Commands;
using VehicleShowroomManagement.Domain.Entities;
using VehicleShowroomManagement.Infrastructure.Interfaces;

namespace VehicleShowroomManagement.Application.VehicleRegistrations.Handlers
{
    /// <summary>
    /// Handler for transferring vehicle ownership
    /// </summary>
    public class TransferOwnershipCommandHandler : IRequestHandler<TransferOwnershipCommand, bool>
    {
        private readonly IRepository<VehicleRegistration> _vehicleRegistrationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public TransferOwnershipCommandHandler(
            IRepository<VehicleRegistration> vehicleRegistrationRepository,
            IUnitOfWork unitOfWork)
        {
            _vehicleRegistrationRepository = vehicleRegistrationRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(TransferOwnershipCommand request, CancellationToken cancellationToken)
        {
            var vehicleRegistration = await _vehicleRegistrationRepository.GetByIdAsync(request.Id);
            if (vehicleRegistration == null)
                return false;

            if (!vehicleRegistration.IsTransferable)
                return false;

            vehicleRegistration.TransferOwnership(request.NewOwnerName, request.NewOwnerAddress);
            vehicleRegistration.OwnerPhone = request.NewOwnerPhone;
            vehicleRegistration.OwnerEmail = request.NewOwnerEmail;

            await _vehicleRegistrationRepository.UpdateAsync(vehicleRegistration);
            _unitOfWork.SaveChangesAsync();

            return true;
        }
    }
}
