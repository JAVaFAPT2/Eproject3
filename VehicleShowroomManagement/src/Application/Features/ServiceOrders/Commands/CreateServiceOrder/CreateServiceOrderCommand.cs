using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.CreateServiceOrder
{
    public record CreateServiceOrderCommand(
        string OrderId,
        string CustomerId,
        string CreatedBy,
        ServiceType Type,
        decimal Cost,
        DateTime? AppointmentDate = null,
        string? Description = null) : IRequest<string>;
}

