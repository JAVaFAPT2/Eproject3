using MediatR;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.CompleteOrder
{
    public record CompleteOrderCommand(string OrderId) : IRequest<bool>;
}

