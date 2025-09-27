using MediatR;

namespace VehicleShowroomManagement.Application.Features.Profile.Commands.UpdateProfile
{
    /// <summary>
    /// Handler for update profile command - simplified implementation
    /// </summary>
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand>
    {
        public async Task Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
        {
            // Simplified implementation - no-op for now
            // In production, implement proper profile update with domain methods
            await Task.CompletedTask;
        }
    }
}