using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.Appointments.Queries.GetAppointments
{
    public record GetAppointmentsQuery(
        int PageNumber,
        int PageSize,
        AppointmentStatus? Status,
        string? DealerId,
        string? CustomerId,
        DateTime? FromDate,
        DateTime? ToDate,
        AppointmentType? Type) : IRequest<object>;
}