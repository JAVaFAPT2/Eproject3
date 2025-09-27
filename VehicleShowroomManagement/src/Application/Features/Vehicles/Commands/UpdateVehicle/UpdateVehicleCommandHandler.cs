using MediatR;

namespace VehicleShowroomManagement.Application.Features.Vehicles.Commands.UpdateVehicle
{
    /// <summary>
    /// Handler for update vehicle command - simplified implementation
    /// </summary>
    public class UpdateVehicleCommandHandler : IRequestHandler<UpdateVehicleCommand>
    {
        public async Task Handle(UpdateVehicleCommand request, CancellationToken cancellationToken)
        {
            // Simplified implementation - no-op for now
            // In production, implement with proper domain methods
            await Task.CompletedTask;
        }
    }
}