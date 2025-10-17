using MediatR;
using VehicleShowroomManagement.Application.Common.Interfaces;
using VehicleShowroomManagement.Domain.Entities;

namespace VehicleShowroomManagement.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler(IRepository<Order> orderRepository)
        : IRequestHandler<UpdateOrderStatusCommand, Unit>
    {
        public async Task<Unit> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken);
            if (order == null) return Unit.Value;
            order.UpdateStatus(request.Status);
            await orderRepository.UpdateAsync(order, cancellationToken);
            return Unit.Value;
        }
    }
}


