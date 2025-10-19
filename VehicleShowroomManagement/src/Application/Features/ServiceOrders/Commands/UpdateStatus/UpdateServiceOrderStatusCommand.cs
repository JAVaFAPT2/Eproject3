using MediatR;
using VehicleShowroomManagement.Application.Common.Models;
using VehicleShowroomManagement.Domain.Enums;

namespace VehicleShowroomManagement.Application.Features.ServiceOrders.Commands.UpdateStatus
{
    /// <summary>
    /// Command to update service order status
    /// </summary>
    public record UpdateServiceOrderStatusCommand(
        string ServiceOrderId,
        ServiceOrderStatus Status)
        : IRequest<UpdateServiceOrderStatusResult>;
}

