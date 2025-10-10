using MediatR;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.ConfirmOrder
{
    public record ConfirmOrderCommand(string OrderId) : IRequest<bool>;
}

