using MediatR;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.AssignVehicle
{
    public record AssignVehicleCommand(string OrderId, string VehicleId, string DealerId) : IRequest<bool>;
}

