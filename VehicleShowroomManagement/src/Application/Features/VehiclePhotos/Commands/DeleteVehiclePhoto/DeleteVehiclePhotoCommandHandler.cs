using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehiclePhotos.Commands.DeleteVehiclePhoto
{
    /// <summary>
    /// Handler for deleting a vehicle photo
    /// </summary>
    public class DeleteVehiclePhotoCommandHandler : IRequestHandler<DeleteVehiclePhotoCommand>
    {
        private readonly IRepository<VehiclePhoto> _photoRepository;

        public DeleteVehiclePhotoCommandHandler(IRepository<VehiclePhoto> photoRepository)
        {
            _photoRepository = photoRepository;
        }

        public async Task Handle(DeleteVehiclePhotoCommand request, CancellationToken cancellationToken)
        {
            var photo = await _photoRepository.GetByIdAsync(request.PhotoId);
            if (photo == null)
            {
                throw new KeyNotFoundException($"Photo with ID {request.PhotoId} not found");
            }

            await _photoRepository.DeleteAsync(request.PhotoId);
        }
    }
}

