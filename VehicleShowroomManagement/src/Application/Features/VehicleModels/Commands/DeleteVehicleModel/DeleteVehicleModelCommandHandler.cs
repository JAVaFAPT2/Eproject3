using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.VehicleModels.Commands.DeleteVehicleModel
{
    /// <summary>
    /// Handler to soft delete a vehicle model
    /// </summary>
    public class DeleteVehicleModelCommandHandler(IRepository<VehicleModel> modelRepository)
        : IRequestHandler<DeleteVehicleModelCommand, Unit>
    {
        public async Task<Unit> Handle(DeleteVehicleModelCommand request, CancellationToken cancellationToken)
        {
            var model = await modelRepository.GetByIdAsync(request.ModelNumber, cancellationToken);
            if (model == null)
            {
                return Unit.Value; // idempotent
            }

            model.MarkDeleted();
            await modelRepository.UpdateAsync(model, cancellationToken);
            return Unit.Value;
        }
    }
}


