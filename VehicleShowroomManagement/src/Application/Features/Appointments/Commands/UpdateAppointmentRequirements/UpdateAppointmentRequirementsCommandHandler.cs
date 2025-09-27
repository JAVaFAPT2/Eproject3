using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.UpdateAppointmentRequirements
{
    public class UpdateAppointmentRequirementsCommandHandler : IRequestHandler<UpdateAppointmentRequirementsCommand>
    {
        public async Task Handle(UpdateAppointmentRequirementsCommand request, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }
    }
}