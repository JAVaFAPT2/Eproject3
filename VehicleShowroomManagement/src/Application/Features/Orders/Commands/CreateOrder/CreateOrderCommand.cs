using MediatR;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(
        string CustomerId,
        string ModelNumber,
        decimal SalePrice) : IRequest<string>;
}

