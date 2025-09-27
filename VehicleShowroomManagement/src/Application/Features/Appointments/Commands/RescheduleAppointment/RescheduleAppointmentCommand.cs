using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.RescheduleAppointment
{
    public record RescheduleAppointmentCommand(string Id, DateTime NewAppointmentDate) : IRequest;
}