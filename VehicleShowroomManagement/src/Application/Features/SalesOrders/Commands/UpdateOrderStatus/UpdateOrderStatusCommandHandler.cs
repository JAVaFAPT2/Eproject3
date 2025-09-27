using MediatR;

namespace VehicleShowroomManagement.Application.Features.SalesOrders.Commands.UpdateOrderStatus
{
    /// <summary>
    /// Handler for update order status command - simplified implementation
    /// </summary>
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand>
    {
        public async Task Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            // Simplified implementation - no-op for now
            // In production, implement with proper domain methods
            await Task.CompletedTask;
        }
    }
}