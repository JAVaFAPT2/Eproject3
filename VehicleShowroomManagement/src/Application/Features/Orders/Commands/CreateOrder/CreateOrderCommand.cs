using MediatR;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.CreateOrder
{
    public record CreateOrderCommand(
        string CustomerId,
        string DealerId,
        string ModelNumber,
        decimal SalePrice,
        string? VehicleId = null,
        DateTime? AppointmentDate = null,
        string? Note = null) : IRequest<string>;
}

