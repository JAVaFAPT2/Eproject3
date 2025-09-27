using MediatR;

namespace VehicleShowroomManagement.Application.Features.Appointments.Queries.GetAppointmentById
{
    public record GetAppointmentByIdQuery(string Id) : IRequest<object?>;
}