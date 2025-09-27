using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Appointments.Queries.GetCustomerAppointments
{
    public record GetCustomerAppointmentsQuery(
        string CustomerId, 
        AppointmentStatus? Status, 
        DateTime? FromDate, 
        DateTime? ToDate) : IRequest<object>;
}