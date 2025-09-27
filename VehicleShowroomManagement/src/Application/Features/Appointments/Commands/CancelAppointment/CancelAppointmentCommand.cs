using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Commands.CancelAppointment
{
    public record CancelAppointmentCommand(string Id, string? Reason) : IRequest;
}