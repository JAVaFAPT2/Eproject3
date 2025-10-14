using MediatR;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateServiceOrderStatus
{
    public record UpdateServiceOrderStatusCommand(
        string ServiceOrderId,
        ServiceOrderStatus Status,
        string? LicensePlate) : IRequest;
}


